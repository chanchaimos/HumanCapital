using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_permission : System.Web.UI.Page
{
    private static int nMenuID = 10;

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

        var lstRole = db.TM_MasterData_Sub.Where(w => w.nMainID == 1 && w.IsActive && !w.IsDel).ToList();
        ddlRole.DataSource = lstRole;
        ddlRole.DataValueField = "nSubID";
        ddlRole.DataTextField = "sName";
        ddlRole.DataBind();
        ddlRole.Items.Insert(0, new ListItem("- User group -", ""));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sRole, string sActive)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            sName = sName.ToLower().Replace(" ", "");

            var lstTB_User = db.TB_User.Where(w => !w.IsDel &&
                            (!string.IsNullOrEmpty(sName) ? (w.sUserID.Contains(sName) || (w.sFirstname + w.sLastname).ToLower().Replace(" ", "").Contains(sName)) : true) &&
                            (!string.IsNullOrEmpty(sRole) ? (w.nRole + "" == sRole) : true) &&
                            (!string.IsNullOrEmpty(sActive) ? (w.IsActive == (sActive == "1")) : true)
                            ).OrderByDescending(o => o.dUpdate).ToList();
            var lstRole = db.TM_MasterData_Sub.Where(w => w.nMainID == 1 && !w.IsDel && w.IsActive).ToList();

            var lstData = (from a in lstTB_User
                           from b in lstRole.Where(w => w.nSubID == a.nRole).DefaultIfEmpty()
                           select new c_User
                           {
                               nUserID = a.nUserID,
                               sUserID = a.sUserID,
                               sName = a.sFirstname + "  " + a.sLastname,
                               sRole = b != null ? b.sName : "",
                               sActive = a.IsActive ? "Active" : "Inactive",
                               sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nUserID + ""))
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
    public static string Delete(List<int> lstID)
    {
        string sRet = "";
        if (!UserAccount.IsExpired)
        {
            int nUserID = UserAccount.SessionInfo.nUserID;
            if (lstID.Any())
            {
                PTTGC_HumanEntities db = new PTTGC_HumanEntities();
                var lstData = db.TB_User.Where(w => lstID.Contains(w.nUserID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                Human_Function.UpdateLog(nMenuID, "", "Delete nUserID = " + string.Join(", ", lstID));
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
        public IEnumerable<c_User> lstData { get; set; }
    }

    [Serializable]
    public class c_User
    {
        public int nUserID { get; set; }
        public string sUserID { get; set; }
        public string sName { get; set; }
        public string sRole { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
    }
    #endregion
}