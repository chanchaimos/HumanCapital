using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_monitoring : System.Web.UI.Page
{
    private static int nMenuID = 8;

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

        ddlYear.DataSource = Human_Function.Get_ddl_Year();
        ddlYear.DataValueField = "Value";
        ddlYear.DataTextField = "Text";
        ddlYear.DataBind();
        ddlYear.SelectedValue = nYearNow + "";
        #endregion
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sCompanyID, string sYear)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            var nUserID = UserAccount.SessionInfo.nUserID;
            var lstComID_Per = db.TB_User_Company.Where(w => w.nUserID == nUserID).Select(s => s.nCompanyID).ToList();
            var lstTB_Company = db.TB_Company.Where(w => !w.IsDel && w.IsActive && lstComID_Per.Contains(w.nCompanyID) && (!string.IsNullOrEmpty(sCompanyID) ? (w.nCompanyID + "" == sCompanyID) : true)).ToList();

            var lstDJSI = db.T_ReportDJSI.Where(w => lstComID_Per.Contains(w.nCompanyID) && w.nYear + "" == sYear).ToList();
            var lstQuarter = new List<c_quarter>();

            foreach (var item in lstComID_Per)
            {
                var lstDJSI_ = lstDJSI.Where(w => w.nCompanyID == item).ToList();

                Func<int, string> GetStatusByQuater = (nQ) =>
                {
                    string sStatus = "NoAction";
                    int nLevel = 0;
                    var qDJSI = lstDJSI_.FirstOrDefault(w => w.nQuarter == nQ);
                    if (qDJSI != null)
                    {
                        switch (qDJSI.nStatusID)
                        {
                            case 0: sStatus = "SaveDraft"; nLevel = 0; break;//Save Draft L0
                            case 1: sStatus = "NoAction"; nLevel = 1; break;//Waiting for approval L1
                            case 2: sStatus = "NoAction"; nLevel = 2; break;//Waiting for approval L2
                            case 3: sStatus = "Completed"; nLevel = 2; break;//Completed L2
                            case 4: sStatus = "Recall"; nLevel = 0; break;//L0 Recall from Waiting L1 Approve
                            case 5: //L0 Request Edit from Waiting L2 Approve
                            case 6: sStatus = "RequestEdit"; nLevel = 0; break;//L0 Request Edit from Completed
                            case 7: sStatus = "Reject"; nLevel = 1; break;//L1 Reject
                            case 8: sStatus = "Reject"; nLevel = 2; break;//L2 Reject
                            default: break;
                        }
                    }
                    return "<div class='circle ml42 c" + sStatus + "'>L" + nLevel + "</div>";
                };

                lstQuarter.Add(new c_quarter()
                {
                    nCompanyID = item,
                    sQuarter1 = GetStatusByQuater(1),
                    sQuarter2 = GetStatusByQuater(2),
                    sQuarter3 = GetStatusByQuater(3),
                    sQuarter4 = GetStatusByQuater(4)
                });
            }

            var lstData = (from a in lstTB_Company
                           from b in lstQuarter.Where(w => w.nCompanyID == a.nCompanyID)
                           select new c_monitoring
                           {
                               sCompanyName = a.sCompanyName,
                               sQuarter1 = b.sQuarter1,
                               sQuarter2 = b.sQuarter2,
                               sQuarter3 = b.sQuarter3,
                               sQuarter4 = b.sQuarter4,
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

    #region Class 
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_monitoring> lstData { get; set; }
    }

    [Serializable]
    public class c_monitoring
    {
        public string sCompanyName { get; set; }
        public string sQuarter1 { get; set; }
        public string sQuarter2 { get; set; }
        public string sQuarter3 { get; set; }
        public string sQuarter4 { get; set; }
    }

    [Serializable]
    public class c_quarter
    {
        public int nCompanyID { get; set; }
        public string sQuarter1 { get; set; }
        public string sQuarter2 { get; set; }
        public string sQuarter3 { get; set; }
        public string sQuarter4 { get; set; }
    }
    #endregion
}