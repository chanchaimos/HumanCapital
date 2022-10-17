using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_masterdata : System.Web.UI.Page
{
    private static int nMenuID = 14;

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

        var lstType = db.TM_MasterData.Where(w => w.IsManage).ToList();
        ddlType.DataSource = lstType;
        ddlType.DataValueField = "nMainID";
        ddlType.DataTextField = "sName";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("- Type  -", ""));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sType, string sActive)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            sName = sName.ToLower().Replace(" ", "");

            var lstMain = db.TM_MasterData.Where(w => w.IsManage).ToList();
            var lstMainID = lstMain.Where(w => w.IsManage).Select(s => s.nMainID).ToList();
            var lstSub = db.TM_MasterData_Sub.Where(w => !w.IsDel && lstMainID.Contains(w.nMainID) &&
                             (!string.IsNullOrEmpty(sName) ? w.sName.ToLower().Replace(" ", "").Contains(sName) : true) &&
                             (!string.IsNullOrEmpty(sType) ? (w.nMainID + "" == sType) : true) &&
                             (!string.IsNullOrEmpty(sActive) ? (w.IsActive == (sActive == "1")) : true)
                             ).OrderByDescending(o => o.dUpdate).ToList();

            var lstCategoryID = db.T_Course.Where(w => !w.IsDel).Select(s => s.nCategoryID.Value).Distinct().ToList();
            var lstSC_ID = db.T_Course_Sub.Where(w => !w.IsDel).Select(s => s.nCourseSubID).ToList();
            var lstTargetGroupID = db.T_Course_Sub_TG.Where(w => lstSC_ID.Contains(w.nCourseSubID)).Select(s => s.nTargetGroup).Distinct().ToList();
            var lstCantDel = lstCategoryID.Concat(lstTargetGroupID).ToList();

            var lstData = (from a in lstSub
                           from b in lstMain.Where(w => w.nMainID == a.nMainID)
                           select new c_Master
                           {
                               nSubID = a.nSubID,
                               sName = a.sName,
                               sType = b != null ? b.sName : "",
                               sActive = a.IsActive ? "Active" : "Inactive",
                               sUpdateDate = a.dUpdate.HasValue ? a.dUpdate.Value.ToString("dd/MM/yyyy") : "-",
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nSubID + "")),
                               IsCanDel = !lstCantDel.Any(aa => aa == a.nSubID)
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
                var lstData = db.TM_MasterData_Sub.Where(w => lstID.Contains(w.nSubID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                Human_Function.UpdateLog(nMenuID, "", "Delete Master Data = " + string.Join(", ", lstID));
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
        public IEnumerable<c_Master> lstData { get; set; }
    }

    [Serializable]
    public class c_Master
    {
        public int nSubID { get; set; }
        public string sName { get; set; }
        public string sType { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public bool IsCanDel { get; set; }
    }
    #endregion
}