using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class report_djsi : System.Web.UI.Page
{
    private static int nMenuID = 2;

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
                hddRole.Value = UserAccount.SessionInfo.nRole + "";
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
            }
        }
    }

    public void SetControl()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        #region Company
        var lstCompany = Human_Function.Get_ddl_CompanyByPermission();

        ddlCompany.DataSource = lstCompany;
        ddlCompany.DataValueField = "Value";
        ddlCompany.DataTextField = "Text";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("- Company -", ""));
        #endregion

        #region Year
        int nYearNow = DateTime.Now.Year;
        var lstYear = Human_Function.Get_ddl_Year();
        ddlYear.DataSource = lstYear;
        ddlYear.DataValueField = "Value";
        ddlYear.DataTextField = "Text";
        ddlYear.DataBind();
        ddlYear.Items.Insert(0, new ListItem("- Year -", ""));
        ddlYear.SelectedValue = nYearNow + "";

        //int nYearStart = 2019;
        //int nYearNow = DateTime.Now.Year;
        //int nIndex = 0;
        //for (int nYear = nYearStart; nYear <= nYearNow; nYear++)
        //{
        //    ddlYear.Items.Insert(nIndex, new ListItem(nYear + "", nYear + ""));
        //    nIndex++;
        //}
        //ddlYear.Items.Insert(0, new ListItem("- Year -", ""));
        //ddlYear.SelectedValue = nYearNow + "";
        #endregion

        #region Status
        var lstStatus = db.TM_WFStatus.Select(s => new { nStatusID = s.nStatusID, sStatusName = (s.sStatusName + " (L" + s.nLevel + (s.nStatusID == 6 ? " Completed" : "") + ")") }).ToList();
        //var lstStatus = db.TM_WFStatus.Select(s => new { nStatusID = s.nStatusID, sStatusName = (s.sStatusName + " (L" + s.nLevel + ")") }).ToList();
        //var lstStatus = db.TM_WFStatus.Select(s => new { nStatusID = s.nStatusID, sStatusName = s.sDescription }).ToList();
        ddlStatus.DataSource = lstStatus;
        ddlStatus.DataValueField = "nStatusID";
        ddlStatus.DataTextField = "sStatusName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("- Status -", ""));
        #endregion
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sCompanyID, string sYear, string sQuarter, string sStatusID)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            int nUserID = UserAccount.SessionInfo.nUserID;
            int nRole = UserAccount.SessionInfo.nRole;
            string sCon = "";

            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();
            sCon += "and a.nCompanyID in(" + (lstCP.Any() ? string.Join(",", lstCP) : "0") + ")";

            if (!string.IsNullOrEmpty(sCompanyID))
            {
                sCon += "and a.nCompanyID = " + sCompanyID;
            }
            if (!string.IsNullOrEmpty(sYear))
            {
                sCon += "and a.nYear = " + sYear;
            }
            if (!string.IsNullOrEmpty(sQuarter))
            {
                sCon += "and a.nQuarter = " + sQuarter;
            }
            if (!string.IsNullOrEmpty(sStatusID))
            {
                sCon += "and a.nStatusID =" + sStatusID;
            }

            string _SQL = @" DECLARE @nRole int,@nUserID int;
            SET @nUserID = " + nUserID + @";
            SET @nRole = " + nRole + @";
            
            SELECT a.nReportID 'nReportID'
                  ,a.nCompanyID 'nCompanyID'
                  ,a.nQuarter 'nQuarter'
                  ,a.nYear 'nYear'
            	  ,b.sCompanyName 'sCompanyName'
            	  ,b.sCompanyAbbr 'sCompanyAbbr'
            	  ,c.sName 'sCompanyType'
            	  ,d.sDescription 'sStatus'
                  ,a.dUpdate
                  ,a.nStatusID
                  ,a.nL1
                  ,a.nL2
              FROM T_ReportDJSI a 
              LEFT JOIN TB_Company b on a.nCompanyID = b.nCompanyID
              LEFT JOIN TM_MasterData_Sub c on c.nSubID = b.nCompanyType
              LEFT JOIN TM_WFStatus d on d.nStatusID = a.nStatusID
              WHERE a.IsDel =0 
              and (
              	--OR
            	-- 0 L0 Save Draft
            	-- 4 L0 Recall from Waiting L1 Approve
            	-- 5 L0 Request Edit from Waiting L2 Approve
            	-- 6 L0 Request Edit from Completed
            	-- 7 L1 Reject
            	-- 8 L2 Reject
            	(a.nStatusID in (0,1,2,3,4,5,6,7,8) 
            		AND (a.nCreateBy = @nUserID 
            			OR @nUserID in 
            			(SELECT us.nUserID FROM TB_User us 
						LEFT JOIN TB_User_Company usc on us.nUserID = usc.nUserID
					    where us.nRole =2 and us.IsActive =1 and us.IsDel=0 
						and usc.nCompanyID = a.nCompanyID 
						GROUP by us.nUserID)))
            	OR
            	-- Waiting L1 Approve
            	(a.nStatusID =1 and a.nL1 = @nUserID)
            	OR
            	-- Waiting L2 Approve
            	(a.nStatusID =2 and a.nL2 = @nUserID)
            	OR
            	-- Completed
            	(a.nStatusID =3 
            		AND 
            		(a.nCreateBy = @nUserID 
						OR a.nL1 = @nUserID 
						OR a.nL2 = @nUserID 
						OR @nUserID in 
						(SELECT us.nUserID FROM TB_User us 
						LEFT JOIN TB_User_Company usc on us.nUserID = usc.nUserID 
						where us.nRole =2 and us.IsActive =1 and us.IsDel=0 
						and usc.nCompanyID = a.nCompanyID
            	GROUP by us.nUserID)))
				-- Admin
				OR
				(@nRole = 1 
					AND @nUserID in 
					(SELECT us.nUserID FROM TB_User us 
					LEFT JOIN TB_User_Company usc on us.nUserID = usc.nUserID 
					where us.nRole =@nRole and us.IsActive =1 and us.IsDel=0 
					and usc.nCompanyID = a.nCompanyID
            	GROUP by us.nUserID))
              ) " + sCon + Environment.NewLine;
            var lstData = db.Database.SqlQuery<c_ReportDJSI>(_SQL).OrderByDescending(o => o.dUpdate).ToList();
            foreach (var item in lstData)
            {
                item.sUpdateDate = item.dUpdate.HasValue ? item.dUpdate.Value.ToString("dd/MM/yyyy") : "";
                item.sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(item.nReportID + ""));
                item.isCanDel = false;//b.nCompanyType != 5 && !lstComID.Contains(a.nCompanyID)
                item.sPrms = CheckPRMS_Button(nRole, item.nStatusID ?? 0, item.nL1 ?? 0, item.nL2 ?? 0);
            }

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
    #endregion

    private static string CheckPRMS_Button(int nRole, int nStatusID, int nUserIDL1, int nUserIDL2)
    {
        string r = "V";
        if (!UserAccount.IsExpired)
        {
            int nUserID = UserAccount.SessionInfo.nUserID;

            List<int> lstL1 = new List<int>() { 1 };
            List<int> lstL2 = new List<int>() { 2 };

            switch (nRole)
            {
                case 1://Administrator
                    break;
                case 2://L0
                    r = "E";
                    break;
                case 3://L1
                    if (nUserID == nUserIDL1)
                        if (lstL1.Contains(nStatusID)) r = "App";
                    break;
                case 4://L2
                    if (nUserID == nUserIDL2)
                        if (lstL2.Contains(nStatusID)) r = "App";
                    break;
            }
        }

        return r;
    }

    #region Class 
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_ReportDJSI> lstData { get; set; }
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
    [Serializable]
    public class c_ReportDJSI
    {
        public int nReportID { get; set; }
        public int nCompanyID { get; set; }
        public int nYear { get; set; }
        public int nQuarter { get; set; }
        public int? nStatusID { get; set; }
        public int? nL1 { get; set; }
        public int? nL2 { get; set; }
        public string sCompanyName { get; set; }
        public string sCompanyAbbr { get; set; }
        public string sCompanyType { get; set; }
        public string sStatus { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public bool isCanDel { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sPrms { get; set; }
    }
    #endregion
}