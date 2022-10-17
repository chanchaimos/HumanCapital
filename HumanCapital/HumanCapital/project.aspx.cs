using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project : System.Web.UI.Page
{
    private static int nMenuID = 5;

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

                hddPermission.Value = SystemFunction.GetPMS(nMenuID);
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "");
                SystemFunction.BindDdlPageSize(ddlPageSize);
                SetControl();
            }
        }
    }

    public void SetControl()
    {
        var lstCompany = Human_Function.Get_ddl_CompanyByPermission();
        ddlCompany.DataSource = lstCompany;
        ddlCompany.DataValueField = "Value";
        ddlCompany.DataTextField = "Text";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("- Company -", ""));
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sCompanyID, string sActive, string sYear)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            List<c_Project> lstData = new List<c_Project>();
            sName = sName.ToLower().Replace(" ", "");

            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();
            var lstPro = db.T_Project.Where(w => w.IsActive && !w.IsDel && lstCP.Contains(w.nCompanyID + "")).ToList();
            var lstPro_ID = lstPro.Select(s => s.nProjectID).Distinct().ToList();
            var lstSubPro = db.T_Project_Course.Where(w => lstPro_ID.Contains(w.nProjectID)).ToList();

            lstData = (from a in db.T_Project.AsEnumerable().Where(w => !w.IsDel && lstCP.Contains(w.nCompanyID + "")
                       && (!string.IsNullOrEmpty(sName) ? w.sProjectName.ToLower().Replace(" ", "").Contains(sName) : true)
                       && (!string.IsNullOrEmpty(sActive) ? w.IsActive == (sActive == "1" ? true : false) : true)
                       && (!string.IsNullOrEmpty(sYear) ? (w.dStart.HasValue && w.dEnd.HasValue) ? (w.dStart.Value.Year + "" == sYear || w.dEnd.Value.Year + "" == sYear)
                       : w.dStart.HasValue ? w.dStart.Value.Year + "" == sYear : w.dEnd.HasValue ? w.dEnd.Value.Year + "" == sYear : false : true))
                       from b in db.TB_Company.Where(w => !w.IsDel && w.IsActive && w.nCompanyID == a.nCompanyID && (!string.IsNullOrEmpty(sCompanyID) ? w.nCompanyID + "" == sCompanyID : true))
                       select new c_Project
                       {
                           nProjectID = a.nProjectID,
                           sProjectName = a.sProjectName,
                           sCompanyName = b.sCompanyName,
                           nProductivity = a.nProductivity ?? 0,
                           sProductivityName = (a.nProductivity.HasValue ? (a.nProductivity == 1 ? "Return" : "Non-Monetary") : ""),
                           nCourse = lstSubPro.AsEnumerable().Where(w => w.nProjectID == a.nProjectID).Count(),
                           sStartDate = a.dStart.HasValue ? a.dStart.Value.ToString("dd/MM/yyyy") : "",
                           sEndDate = a.dEnd.HasValue ? a.dEnd.Value.ToString("dd/MM/yyyy") : "",
                           sActive = a.IsActive ? "Active" : "Inactive",
                           sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                           sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nProjectID + "")),
                           sLink = "project_edit.aspx?str=" + HttpUtility.UrlEncode(STCrypt.Encrypt(a.nProjectID + "")),
                           dUpdate = a.dUpdate,
                           sObjective = a.sObjective,
                           sOrganization = a.sOrganization,
                           nReturn_Economy = a.nReturn_Economy,
                           nReturn_Environment = a.nReturn_Environment,
                           nReturn_Social = a.nReturn_Social,
                           nReturn_Other = a.nReturn_Other,
                           nPrice_Opex = a.nPrice_Opex,
                           nPrice_Capex = a.nPrice_Capex,
                       }).OrderByDescending(o => o.dUpdate).ToList();

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
                var lstData = db.T_Project.Where(w => lstID.Contains(w.nProjectID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                Human_Function.UpdateLog(nMenuID, "", "Delete Project = " + string.Join(", ", lstID));
            }
        }
        else
        {
            sRet = SystemFunction.process_SessionExpired;
        }
        return sRet;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    #endregion

    public static TReturnData Get_detail(int nProjectID)
    {
        TReturnData result = new TReturnData();
        result.lstDataCourseSub = new List<CData_CourseSub>();
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        if (!UserAccount.IsExpired)
        {
            result.lstDataCourseSub = (from a in db.T_Project.Where(w => w.nProjectID == nProjectID && !w.IsDel)
                                       from b in db.T_Project_Course.Where(w => w.nProjectID == nProjectID)
                                       from c in db.T_Course_Sub.Where(w => w.nCourseSubID == b.nCourseSubID && w.IsActive && !w.IsDel)
                                       select new CData_CourseSub
                                       {
                                           nCourseSubID = b.nCourseSubID,
                                           nCompanyID = a.nCompanyID ?? 0,
                                           sName = c.sName,
                                       }).ToList();
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    #region Export Excel
    protected void btnExport__Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            #region Query Data
            hddtxtName.Value = hddtxtName.Value.ToLower().Replace(" ", "");
            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();
            var lstT_Project_Course = db.T_Project_Course.ToList();

            var lstData = (from a in db.T_Project.AsEnumerable().Where(w => !w.IsDel && lstCP.Contains(w.nCompanyID + "")
                        && (!string.IsNullOrEmpty(hddtxtName.Value) ? w.sProjectName.ToLower().Replace(" ", "").Contains(hddtxtName.Value) : true)
                        && (!string.IsNullOrEmpty(hddActive.Value) ? w.IsActive == (hddActive.Value == "1" ? true : false) : true)
                        && (!string.IsNullOrEmpty(hddYear.Value) ? (w.dStart.HasValue && w.dEnd.HasValue) ? (w.dStart.Value.Year + "" == hddYear.Value || w.dEnd.Value.Year + "" == hddYear.Value)
                        : w.dStart.HasValue ? w.dStart.Value.Year + "" == hddYear.Value : w.dEnd.HasValue ? w.dEnd.Value.Year + "" == hddYear.Value : false : true))
                           from b in db.TB_Company.Where(w => !w.IsDel && w.IsActive && w.nCompanyID == a.nCompanyID && (!string.IsNullOrEmpty(hddCompanyID.Value) ? w.nCompanyID + "" == hddCompanyID.Value : true))
                           select new c_Project
                           {
                               nProjectID = a.nProjectID,
                               sProjectName = a.sProjectName,
                               sCompanyName = b.sCompanyName,
                               nProductivity = a.nProductivity ?? 0,
                               sProductivityName = (a.nProductivity.HasValue ? (a.nProductivity == 1 ? "Return" : "Non-Monetary") : ""),
                               nCourse = lstT_Project_Course.AsEnumerable().Where(w => w.nProjectID == a.nProjectID).Count(),
                               sStartDate = a.dStart.HasValue ? a.dStart.Value.ToString("dd/MM/yyyy") : "",
                               sEndDate = a.dEnd.HasValue ? a.dEnd.Value.ToString("dd/MM/yyyy") : "",
                               sActive = a.IsActive ? "Active" : "Inactive",
                               sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nProjectID + "")),
                               sLink = "project_edit.aspx?str=" + HttpUtility.UrlEncode(STCrypt.Encrypt(a.nProjectID + "")),
                               dUpdate = a.dUpdate,
                               sObjective = a.sObjective,
                               sOrganization = a.sOrganization,
                               nReturn_Economy = a.nReturn_Economy,
                               nReturn_Environment = a.nReturn_Environment,
                               nReturn_Social = a.nReturn_Social,
                               nReturn_Other = a.nReturn_Other,
                               nPrice_Opex = a.nPrice_Opex,
                               nPrice_Capex = a.nPrice_Capex,
                           }).OrderByDescending(o => o.dUpdate).ToList();
            #endregion
            string sColorHeadTb = Human_Function.GetColorHeader_Report();
            var lstT_Cource_Sub = db.T_Course_Sub.Where(w => !w.IsDel && w.IsActive).ToList();

            #region Action
            Action<IXLWorksheet, int, int, int, bool, XLAlignmentHorizontalValues, XLAlignmentVerticalValues, bool, int?, double?, string> SetTbl = (sWorkSheet, row, col, FontSize, Bold, Horizontal, Vertical, wraptext, dec, width, sTxt) =>
            {
                sTxt = sTxt + "";
                sWorkSheet.Cell(row, col).Value = sTxt;
                sWorkSheet.Cell(row, col).Style.Font.Bold = Bold;
                //sWorkSheet.Cell(row, col).Style.Alignment.WrapText = wraptext;
                sWorkSheet.Cell(row, col).Style.Alignment.Horizontal = Horizontal;
                sWorkSheet.Cell(row, col).Style.Alignment.Vertical = Vertical;
                if (width != null)
                    sWorkSheet.Column(col).Width = width.Value;
                if (dec != null || dec == 0)
                {
                    string[] arr = sTxt.Split('.');
                    if (arr.Length > 1)
                    {
                        string sFormat = sTxt == "0.00" ? "@" : (arr[1] == "00" ? "#,#" : "#,#0.00");
                        sWorkSheet.Cell(row, col).Style.NumberFormat.Format = sFormat;
                    }
                }

                var nIndex = sTxt.Split('/').Length;
                if (nIndex == 3)
                {
                    sWorkSheet.Cell(row, col).Style.DateFormat.Format = "dd/MM/yyyy";
                }
            };
            #endregion

            #region variable
            XLColor colorHead = XLColor.FromHtml(sColorHeadTb);

            int nRow = 1;
            int nCol = 1;
            int nMaxCol = 0;
            int nMaxRow = 0;

            #endregion

            HttpResponse httpResponse = Response;
            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws1 = wb.Worksheets.Add("Sheet1");

            #region Header Info
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 8, "No");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 55, "Company");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 37, "Project Name");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 33, "Organization");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 55, "Objective");
            nCol++;

            var lstProductivity = new List<string>() { "Economic", "Social", "Environment", "Other" };
            ws1.Range(nRow, nCol, nRow, nCol + 3).Merge();
            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 80, "Productivity");
            lstProductivity.ForEach(f =>
            {
                ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
                SetTbl(ws1, (nRow + 1), nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20, f);
                nCol++;
            });

            var lstPrice = new List<string>() { "OPEX", "CAPEX" };
            ws1.Range(nRow, nCol, nRow, nCol + 1).Merge();
            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 60, "Price");
            lstPrice.ForEach(f =>
            {
                ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
                SetTbl(ws1, (nRow + 1), nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20, f);
                nCol++;
            });

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "Start Date");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "End Date");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 40, "Sub Course");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "Status");
            nMaxCol = nCol;
            nCol++;

            nRow++;
            #endregion

            #region Add Data Cell
            int nOder = 1;
            nRow = 3;
            lstData = lstData.Take(1000).ToList();
            foreach (var i in lstData)
            {
                nCol = 1;
                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, nOder + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sCompanyName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sProjectName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sOrganization);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sObjective);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 2, null, i.nReturn_Economy + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 2, null, i.nReturn_Social + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 2, null, i.nReturn_Environment + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 2, null, i.nReturn_Social + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 2, null, i.nPrice_Opex + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 2, null, i.nPrice_Capex + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sStartDate);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sEndDate);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, Human_Function.GetSubCourseNameTO_Report(2, i.nProjectID, db.T_Course_Sub.Where(w => w.IsActive && !w.IsDel).ToList()));
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sActive);
                nCol++;

                nMaxRow = nRow;
                nOder++;
                nRow++;
            }
            #endregion

            ws1.PageSetup.Margins.Top = 0.2;
            ws1.PageSetup.Margins.Bottom = 0.2;
            ws1.PageSetup.Margins.Left = 0.1;
            ws1.PageSetup.Margins.Right = 0;
            ws1.PageSetup.Margins.Footer = 0;
            ws1.PageSetup.Margins.Header = 0;
            ws1.Style.Font.FontName = "Browallia New";
            ws1.Style.Font.FontSize = 14;
            ws1.Range(1, 1, nMaxRow, nMaxCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(1, 1, nMaxRow, nMaxCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws1.Style.Alignment.WrapText = true;

            #region CreateEXCEL
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string sName = "ReportProject_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

            httpResponse.AddHeader("content-disposition", "attachment;filename=" + sName + ".xlsx");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }
            httpResponse.End();
            #endregion
        }
    }
    #endregion

    #region Class
    [Serializable]
    public class TReturnData : sysGlobalClass.CResutlWebMethod
    {
        public IEnumerable<c_Project> lstData { get; set; }
        public IEnumerable<CData_CourseSub> lstDataCourseSub { get; set; }
    }

    [Serializable]
    public class c_Project
    {
        public int nProjectID { get; set; }
        public string sProjectName { get; set; }
        public string sCompanyName { get; set; }
        public int nProductivity { get; set; }
        public string sProductivityName { get; set; }
        public int nCourse { get; set; }
        public string sStartDate { get; set; }
        public string sEndDate { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public string sLink { get; set; }
        public string sOrganization { get; set; }
        public string sObjective { get; set; }
        public decimal? nReturn_Economy { get; set; }
        public decimal? nReturn_Environment { get; set; }
        public decimal? nReturn_Social { get; set; }
        public decimal? nReturn_Other { get; set; }
        public decimal? nPrice_Opex { get; set; }
        public decimal? nPrice_Capex { get; set; }
        public DateTime dUpdate { get; set; }
    }
    [Serializable]
    public class CData_CourseSub
    {
        public int nCourseSubID { get; set; }
        public int nCompanyID { get; set; }
        public string sName { get; set; }
    }
    #endregion
}