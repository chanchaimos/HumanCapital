using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    private static int nMenuID = 1;

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

                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                if (sPer != "N")
                {
                    lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
                    SetControl();
                }
                else
                {
                    SetBodyEventOnLoad("PopupNoPermission('" + GetPathRedirect() + "');");
                }
            }
        }
    }

    private void SetControl()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        #region Company
        var lstCompany = Human_Function.Get_ddl_CompanyByPermission();

        ddlCompany.DataSource = lstCompany;
        ddlCompany.DataValueField = "Value";
        ddlCompany.DataTextField = "Text";
        ddlCompany.DataBind();
        //ddlCompany.Items.Insert(0, new ListItem("- Company -", ""));
        if (lstCompany.FirstOrDefault() != null)
        {
            ddlCompany.SelectedValue = lstCompany.FirstOrDefault().Value;
        }
        #endregion

        #region Year
        int nYearNow = DateTime.Now.Year;
        var lstYear = Human_Function.Get_ddl_Year();
        ddlYear.DataSource = lstYear;
        ddlYear.DataValueField = "Value";
        ddlYear.DataTextField = "Text";
        ddlYear.DataBind();
        ddlYear.SelectedValue = nYearNow + "";
        //ddlYear.Items.Insert(0, new ListItem("- Year -", ""));
        #endregion
    }

    public static string GetPathRedirect()
    {
        string sRet = "";
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        int nUserID = UserAccount.SessionInfo.nUserID;
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.IsActive && !w.IsDel);
        var lstTM_Menu = db.TM_Menu.Where(w => w.IsActive).OrderBy(o => o.nMenuOrder).ToList();
        var lstMenuID = lstTM_Menu.Select(s => s.nMenuID).ToList();
        var qHasPer = db.TB_User_Permission.FirstOrDefault(w => w.nUserID == nUserID && lstMenuID.Contains(w.nMenuID) && w.nPermission > 0);
        if (qHasPer != null)
        {
            var qMenu = lstTM_Menu.FirstOrDefault(w => w.nMenuID == qHasPer.nMenuID);
            sRet = qMenu != null ? qMenu.sMenuLink : "unauthorize.aspx";
        }
        else
        {
            sRet = "unauthorize.aspx";
        }

        return sRet;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sComID, string sYear, string cType)
    {
        TReturnData result = new TReturnData();
        result.Content = "";
        result.lstData = new List<CData_Quarter>();
        result.lstDash_1 = new List<CData_Graph>();
        result.lstDash_2 = new List<CData_Graph>();
        result.lstDash_3 = new List<CData_Graph>();
        result.lstDash_4 = new List<CData_Graph>();
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            if (!string.IsNullOrEmpty(sComID) && !string.IsNullOrEmpty(sYear))
            {
                var lstDJSIAPP = db.T_ReportDJSI_Approve.ToList();
                var lstReport_DJSI = db.T_ReportDJSI.Where(w => w.nCompanyID + "" == sComID && w.nYear + "" == sYear).ToList();
                var lstReport_DJSI_Item = db.T_ReportDJSI_Item.ToList();
                var lstStatus = db.TM_WFStatus.ToList();
                var lstTM_DJSI = db.TM_DJSI.ToList();

                result.lstData =
                            (from a in lstReport_DJSI
                             from b in lstDJSIAPP.Where(w => w.nReportID == a.nReportID && w.nID == (lstDJSIAPP.Where(f => f.nReportID == a.nReportID).OrderByDescending(o => o.nID).FirstOrDefault().nID)).DefaultIfEmpty()
                             from c in lstStatus.Where(w => b != null ? w.nStatusID == b.nStatusID : true).DefaultIfEmpty()
                             select new CData_Quarter
                             {
                                 nQuarter = a.nQuarter,
                                 sMg = b != null ? b.sComment : "",
                                 sStatus = c != null ? c.sDescription : "",
                             }).ToList();

                #region Variable 
                List<string> lstNameDash_1 = new List<string>() { "Employee", "Contractor", "Other" };
                List<string> lstNameDash_2 = new List<string>() { "Rayong", "Bangkok", "Other" };
                List<string> lstNameDash_3 = new List<string>() { "< 30", "30-50", "> 50" };
                List<string> lstNameDash_4 = new List<string>() { "New Employee", "Turnover" };
                List<string> lstSex = new List<string>() { "Male", "Female" };

                List<int> lstDash_1 = new List<int>() { 5, 7, 9 };
                List<int> lstDash_2 = new List<int>() { 11, 12, 13 };
                List<int> lstDash_3 = new List<int>() { 32, 34, 36 };
                List<int> lstDash_4 = new List<int>() { 50, 69 };
                #endregion

                #region Set Data Graph
                if (cType == "0") // By Quarter
                {
                    var qG = lstReport_DJSI.Where(w => w.nStatusID == 3).OrderByDescending(o => o.nQuarter).FirstOrDefault();
                    if (qG != null)
                    {
                        result.Content = qG.nQuarter + "/" + qG.nYear;
                        var qlstItem = lstReport_DJSI_Item.Where(w => w.nReportID == qG.nReportID).ToList();

                        #region Dash 1
                        lstDash_1.ForEach(f =>
                        {
                            var qItem = qlstItem.FirstOrDefault(w => w.nItem == f);
                            if (qItem != null)
                            {
                                string sName = "";
                                switch (f)
                                {
                                    case 5: sName = lstNameDash_1[0]; break;
                                    case 7: sName = lstNameDash_1[1]; break;
                                    case 9: sName = lstNameDash_1[2]; break;
                                }
                                result.lstDash_1.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = (qItem.nMale_3 ?? 0) + "",//sValue1 = ((decimal)qItem.nMale_1 + qItem.nMale_2 + qItem.nMale_3) + "",
                                    sValue2 = (qItem.nFemale_3 ?? 0) + ""//sValue2 = ((decimal)qItem.nFemale_1 + qItem.nFemale_2 + qItem.nFemale_3) + "",
                                });
                            }
                        });

                        #endregion

                        #region Dash 2
                        lstDash_2.ForEach(f =>
                        {
                            var qItem = qlstItem.FirstOrDefault(w => w.nItem == f);
                            if (qItem != null)
                            {
                                string sName = "";
                                switch (f)
                                {
                                    case 11: sName = lstNameDash_2[0]; break;
                                    case 12: sName = lstNameDash_2[1]; break;
                                    case 13: sName = lstNameDash_2[2]; break;
                                }
                                result.lstDash_2.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = (qItem.nMale_3 ?? 0) + "",//sValue1 = ((decimal)qItem.nMale_1 + qItem.nMale_2 + qItem.nMale_3) + "",
                                    sValue2 = (qItem.nFemale_3 ?? 0) + ""//sValue2 = ((decimal)qItem.nFemale_1 + qItem.nFemale_2 + qItem.nFemale_3) + "",
                                });

                            }
                        });
                        #endregion

                        #region Dash 3
                        lstDash_3.ForEach(f =>
                        {
                            var qItem = qlstItem.FirstOrDefault(w => w.nItem == f);
                            if (qItem != null)
                            {
                                string sName = "";
                                switch (f)
                                {
                                    case 32: sName = lstNameDash_3[0]; break;
                                    case 34: sName = lstNameDash_3[1]; break;
                                    case 36: sName = lstNameDash_3[2]; break;
                                }
                                result.lstDash_3.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = (qItem.nMale_3 ?? 0) + "",//sValue1 = ((decimal)qItem.nMale_1 + qItem.nMale_2 + qItem.nMale_3) + "",
                                    sValue2 = (qItem.nFemale_3 ?? 0) + ""//sValue2 = ((decimal)qItem.nFemale_1 + qItem.nFemale_2 + qItem.nFemale_3) + "",
                                });

                            }
                        });
                        #endregion

                        #region Dash 4

                        lstDash_4.ForEach(f =>
                        {
                            var qItem = qlstItem.FirstOrDefault(w => w.nItem == f);
                            if (qItem != null)
                            {
                                string sName = "";
                                switch (f)
                                {
                                    case 50: sName = lstNameDash_4[0]; break;
                                    case 69: sName = lstNameDash_4[1]; break;
                                }
                                result.lstDash_4.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = (qItem.nMale_3 ?? 0) + "",//sValue1 = ((decimal)qItem.nMale_1 + qItem.nMale_2 + qItem.nMale_3) + "",
                                    sValue2 = (qItem.nFemale_3 ?? 0) + ""//sValue2 = ((decimal)qItem.nFemale_1 + qItem.nFemale_2 + qItem.nFemale_3) + "",
                                });
                            }
                        });


                        #endregion
                    }
                }
                else
                {
                    var lstReDJSI = lstReport_DJSI.Where(w => w.nStatusID == 3).OrderByDescending(o => o.nQuarter).ToList();
                    var lstReDJSI_Q = lstReDJSI.Select(s => s.nQuarter).ToList();
                    if (lstReDJSI.Any())
                    {
                        result.Content = lstReDJSI_Q.FirstOrDefault() + "/" + lstReDJSI.FirstOrDefault().nYear;
                        var qlstItem = lstReport_DJSI_Item.Where(w => lstReDJSI_Q.Contains(w.nReportID)).ToList();

                        #region Dash 1
                        lstDash_1.ForEach(f1 =>
                        {
                            var qItem = qlstItem.Where(w => w.nItem == f1).ToList();
                            if (qItem.Any())
                            {
                                string sName = "";
                                switch (f1)
                                {
                                    case 5: sName = lstNameDash_1[0]; break;
                                    case 7: sName = lstNameDash_1[1]; break;
                                    case 9: sName = lstNameDash_1[2]; break;
                                }
                                result.lstDash_1.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = qItem.Sum(s => (s.nMale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nMale_1) + qItem.Sum(s => s.nMale_2) + qItem.Sum(s => s.nMale_3)) + "",
                                    sValue2 = qItem.Sum(s => (s.nFemale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nFemale_1) + qItem.Sum(s => s.nFemale_2) + qItem.Sum(s => s.nFemale_3)) + "",
                                });
                            }
                        });

                        #endregion

                        #region Dash 2
                        lstDash_2.ForEach(f1 =>
                        {
                            var qItem = qlstItem.Where(w => w.nItem == f1).ToList();
                            if (qItem.Any())
                            {
                                string sName = "";
                                switch (f1)
                                {
                                    case 11: sName = lstNameDash_2[0]; break;
                                    case 12: sName = lstNameDash_2[1]; break;
                                    case 13: sName = lstNameDash_2[2]; break;
                                }
                                result.lstDash_2.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = qItem.Sum(s => (s.nMale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nMale_1) + qItem.Sum(s => s.nMale_2) + qItem.Sum(s => s.nMale_3)) + "",
                                    sValue2 = qItem.Sum(s => (s.nFemale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nFemale_1) + qItem.Sum(s => s.nFemale_2) + qItem.Sum(s => s.nFemale_3)) + "",
                                });

                            }
                        });
                        #endregion

                        #region Dash 3
                        lstDash_3.ForEach(f1 =>
                        {
                            var qItem = qlstItem.Where(w => w.nItem == f1).ToList();
                            if (qItem.Any())
                            {
                                string sName = "";
                                switch (f1)
                                {
                                    case 32: sName = lstNameDash_3[0]; break;
                                    case 34: sName = lstNameDash_3[1]; break;
                                    case 36: sName = lstNameDash_3[2]; break;
                                }
                                result.lstDash_3.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = qItem.Sum(s => (s.nMale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nMale_1) + qItem.Sum(s => s.nMale_2) + qItem.Sum(s => s.nMale_3)) + "",
                                    sValue2 = qItem.Sum(s => (s.nFemale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nFemale_1) + qItem.Sum(s => s.nFemale_2) + qItem.Sum(s => s.nFemale_3)) + "",
                                });

                            }
                        });
                        #endregion

                        #region Dash 4

                        lstDash_4.ForEach(f1 =>
                        {
                            var qItem = qlstItem.Where(w => w.nItem == f1).ToList();
                            if (qItem.Any())
                            {
                                string sName = "";
                                switch (f1)
                                {
                                    case 50: sName = lstNameDash_4[0]; break;
                                    case 69: sName = lstNameDash_4[1]; break;
                                }
                                result.lstDash_4.Add(new CData_Graph()
                                {
                                    sName = sName,
                                    sValue1 = qItem.Sum(s => (s.nMale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nMale_1) + qItem.Sum(s => s.nMale_2) + qItem.Sum(s => s.nMale_3)) + "",
                                    sValue2 = qItem.Sum(s => (s.nFemale_3 ?? 0)) + "",//((decimal)qItem.Sum(s => s.nFemale_1) + qItem.Sum(s => s.nFemale_2) + qItem.Sum(s => s.nFemale_3)) + "",
                                });
                            }
                        });


                        #endregion
                    }
                }
                #endregion
            }
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
        public List<CData_Quarter> lstData { get; set; }
        public List<CData_Graph> lstDash_1 { get; set; }
        public List<CData_Graph> lstDash_2 { get; set; }
        public List<CData_Graph> lstDash_3 { get; set; }
        public List<CData_Graph> lstDash_4 { get; set; }
    }
    [Serializable]
    public class CData_Quarter
    {
        public int nQuarter { get; set; }
        public string sStatus { get; set; }
        public string sMg { get; set; }
    }
    [Serializable]
    public class CData_Graph
    {
        public string sName { get; set; }
        public string sValue1 { get; set; }
        public string sValue2 { get; set; }
    }

    #endregion
}