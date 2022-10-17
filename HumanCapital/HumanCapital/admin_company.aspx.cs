using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_company : System.Web.UI.Page
{
    private static int nMenuID = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAccount.IsExpired)
            {
                ((_MP_Front)Master).MenuID_Selected = nMenuID;

                SetControl();
                SystemFunction.BindDdlPageSize(ddlPageSize);
                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
            }
        }
    }

    public void SetControl()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        var lstComType = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && w.IsActive && !w.IsDel).ToList();
        ddlCompanyType.DataSource = lstComType;
        ddlCompanyType.DataValueField = "nSubID";
        ddlCompanyType.DataTextField = "sName";
        ddlCompanyType.DataBind();
        ddlCompanyType.Items.Insert(0, new ListItem("- Company Type  -", ""));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sCompanyType, string sActive)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            sName = sName.ToLower().Replace(" ", "");

            var lstTB_Company = db.TB_Company.Where(w => !w.IsDel &&
                            (!string.IsNullOrEmpty(sName) ? (w.sCompanyName.ToLower().Replace(" ", "").Contains(sName) || w.sCompanyAbbr.ToLower().Replace(" ", "").Contains(sName)) : true) &&
                            (!string.IsNullOrEmpty(sCompanyType) ? (w.nCompanyType + "" == sCompanyType) : true) &&
                            (!string.IsNullOrEmpty(sActive) ? (w.IsActive == (sActive == "1")) : true)
                            ).OrderByDescending(o => o.dUpdate).ToList();
            var lstComType = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && !w.IsDel && w.IsActive).ToList();

            var lstComID_Report = db.T_ReportDJSI.Where(w => !w.IsDel).Select(s => s.nCompanyID).Distinct().ToList();
            var lstComID_Course = db.T_Course.Where(w => !w.IsDel && w.IsActive).Select(s => s.nCompanyID).Distinct().ToList();
            var lstComID_Pro = db.T_Project.Where(w => !w.IsDel && w.nCompanyID.HasValue).Select(s => s.nCompanyID ?? 0).Distinct().ToList();
            var lstComID = lstComID_Report.Concat(lstComID_Course).Concat(lstComID_Pro).Distinct().ToList();

            var lstData = (from a in lstTB_Company
                           from b in lstComType.Where(w => w.nSubID == a.nCompanyType).DefaultIfEmpty()
                           select new c_company
                           {
                               nCompanyID = a.nCompanyID,
                               sCompanyName = a.sCompanyName,
                               sCompanyAbbr = a.sCompanyAbbr,
                               sCompanyType = b != null ? b.sName : "",
                               sActive = a.IsActive ? "Applicable" : "Non applicable",
                               sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nCompanyID + "")),
                               isCanDel = a.nCompanyType != 5 && !lstComID.Contains(a.nCompanyID)
                           }).ToList();

            result.lstData = lstData;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SyncCompany()
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            var lstCom = HR_WebService.Companay_List();
            if (lstCom.Any())
            {
                var nUserID = UserAccount.SessionInfo.nUserID;
                var dNow = DateTime.Now;

                foreach (var item in lstCom)
                {
                    var qCom = db.TB_Company.FirstOrDefault(w => w.sCompanyCode == item.CompanyCode && w.nCompanyType == 5);
                    var IsNew = qCom == null;
                    if (IsNew)
                    {
                        qCom = new TB_Company();
                        qCom.nCompanyType = 5;
                        qCom.IsActive = true;
                        qCom.nCreateBy = nUserID;
                        qCom.dCreate = dNow;
                        db.TB_Company.Add(qCom);
                    }

                    qCom.sCompanyCode = item.CompanyCode;
                    qCom.sCompanyName = item.CompanyName;
                    qCom.sCompanyAbbr = item.CompanyAbbreviation;
                    qCom.IsDel = false;
                    qCom.nUpdateBy = nUserID;
                    qCom.dUpdate = dNow;
                }
                db.SaveChanges();
            }

            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static string Delete(List<int> lstID)
    {
        string sRet = "";
        if (!UserAccount.IsExpired)
        {
            int nUserID = UserAccount.SessionInfo.nUserID;
            if (lstID.Any())
            {
                PTTGC_HumanEntities db = new PTTGC_HumanEntities();
                var lstData = db.TB_Company.Where(w => lstID.Contains(w.nCompanyID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                Human_Function.UpdateLog(nMenuID, "", "Delete Company = " + string.Join(", ", lstID));
            }
        }
        else
        {
            sRet = SystemFunction.process_SessionExpired;
        }
        return sRet;
    }

    #region Class 
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_company> lstData { get; set; }
    }

    [Serializable]
    public class c_company
    {
        public int nCompanyID { get; set; }
        public string sCompanyName { get; set; }
        public string sCompanyAbbr { get; set; }
        public string sCompanyType { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public bool isCanDel { get; set; }
    }
    #endregion
}