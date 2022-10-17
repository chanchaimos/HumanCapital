using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ClassExecute;

public partial class admin_permission_edit : System.Web.UI.Page
{
    private static int nMenuID = 9;

    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnload(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                string str = Request.QueryString["str"];
                string sComID = hddComID.Value = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";

                SetControl();

                string sPageType = "Add";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (sComID != "")
                {
                    SetData(sComID);
                    sPageType = sPer == "A" ? "Edit" : "View";
                }
                else { rdlCompanyType.SelectedValue = "6"; }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
            }
        }
    }

    public void SetControl()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        var lstComType = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && w.IsActive && !w.IsDel).ToList();
        rdlCompanyType.DataSource = lstComType;
        rdlCompanyType.DataValueField = "nSubID";
        rdlCompanyType.DataTextField = "sName";
        rdlCompanyType.DataBind();
    }

    public void SetData(string sComID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qCom = db.TB_Company.FirstOrDefault(w => w.nCompanyID + "" == sComID && !w.IsDel);
        if (qCom != null)
        {
            rdlCompanyType.SelectedValue = qCom.nCompanyType + "";
            txtCompanyName.Text = qCom.sCompanyName;
            txtCompanyAbbr.Text = qCom.sCompanyAbbr;
            rdlActive.SelectedValue = qCom.IsActive ? "1" : "0";
            if (qCom.nCompanyType == 5) { txtCompanyName.Enabled = false; txtCompanyAbbr.Enabled = false; }
        }
        else
        {
            Response.Redirect("admin_company.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnLoadData GetData(int nComID)
    {
        TReturnLoadData result = new TReturnLoadData();
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            var lstDJSI_Master = db.TM_DJSI.ToList();
            var lstDJSI = db.TB_Company_DJSI.Where(w => w.nCompanyID == nComID).ToList();
            var lstData = (from a in lstDJSI_Master
                           from b in lstDJSI.Where(w => w.nItem == a.nItem).DefaultIfEmpty()
                           select new c_djsi
                           {
                               nItem = a.nItem,
                               sName = a.sName,
                               nItemHead = a.nItemHead,
                               nSibling = a.nSibling,
                               IsHead = a.IsHead,
                               IsAutoCal = a.IsAutoCal,
                               IsChecked = b != null//Select Item
                           }).ToList();

            result.lstDJSI = lstData;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(int nComID, int nComType, string sCompanyName, string sCompanyAbbr, bool IsActive, List<int> lstDJSI)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var IsNew = nComID == 0;
            var qDup = db.TB_Company.FirstOrDefault(w => (!IsNew ? w.nCompanyID != nComID : true) && w.sCompanyName == sCompanyName && !w.IsDel);
            if (qDup == null)
            {
                #region Add/Update TB_Company
                var qUser = db.TB_Company.FirstOrDefault(w => w.nCompanyID == nComID);
                if (!IsNew && qUser.IsDel)
                {
                    result.Msg = SystemFunction.sMsgSaveInNotStep;
                    result.Status = SystemFunction.process_SaveFail;
                    return result;
                }

                var nUserThis = UserAccount.SessionInfo.nUserID;

                if (IsNew)
                {
                    qUser = new TB_Company();
                    qUser.nCompanyType = nComType;
                    qUser.sCompanyCode = "";
                    qUser.nCreateBy = nUserThis;
                    qUser.dCreate = DateTime.Now;
                    db.TB_Company.Add(qUser);
                }
                qUser.sCompanyName = sCompanyName;
                qUser.sCompanyAbbr = sCompanyAbbr;
                qUser.IsActive = IsActive;
                qUser.IsDel = false;
                qUser.nUpdateBy = nUserThis;
                qUser.dUpdate = DateTime.Now;
                db.SaveChanges();

                #region TB_Company_DJSI
                CommonFunction.ExecuteNonQuery("delete TB_Company_DJSI where nCompanyID = " + nComID);

                StringBuilder sb = new StringBuilder();
                var sInsert = @"INSERT INTO TB_Company_DJSI
               ([nCompanyID]
               ,[nItem])
                VALUES
               ({0}--<nCompanyID, int,>
               ,{1})--<nItem, int,>)";

                if (lstDJSI.Any())
                {
                    foreach (var item in lstDJSI)
                    {
                        sb.Append(string.Format(sInsert, nComID, item) + Environment.NewLine);
                        if (item == 98) { sb.Append(string.Format(sInsert, nComID, 99) + Environment.NewLine); }//14.1
                        if (item == 118) { sb.Append(string.Format(sInsert, nComID, 119) + Environment.NewLine); }//15.4
                        if (item == 120) { sb.Append(string.Format(sInsert, nComID, 121) + Environment.NewLine); }//15.5
                        if (item == 122) { sb.Append(string.Format(sInsert, nComID, 123) + Environment.NewLine); }//15.5
                    }
                    CommonFunction.ExecuteNonQuery(sb + "");
                }
                #endregion

                #endregion
            }
            else
            {
                result.Status = SystemFunction.process_Duplicate;
            }

        }
        return result;
    }

    #region Class
    [Serializable]
    public class TReturnLoadData : sysGlobalClass.CResutlWebMethod
    {
        public List<c_djsi> lstDJSI { get; set; }
    }
    #endregion
}