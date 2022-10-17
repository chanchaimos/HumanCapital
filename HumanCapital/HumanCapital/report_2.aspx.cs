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

public partial class report_2 : System.Web.UI.Page
{
    private static int nMenuID = 13;

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
                //SetControl();
            }
        }
    }

    #region WebMedthod 
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnCourseSub GetSubCourse(string sSearch)
    {
        c_ReturnCourseSub TReturn = new c_ReturnCourseSub();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            TReturn.lstData = new List<T_Course_Sub>();
            TReturn.lstData = db.T_Course_Sub.Where(w => w.IsActive && !w.IsDel && w.sName.ToLower().Replace(" ", "").Contains(sSearch)).Take(20).ToList();
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnProject GetProject(string sSearch)
    {
        c_ReturnProject TReturn = new c_ReturnProject();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            TReturn.lstData = new List<T_Project>();
            TReturn.lstData = db.T_Project.Where(w => w.IsActive && !w.IsDel && w.sProjectName.ToLower().Replace(" ", "").Contains(sSearch)).Take(20).ToList();
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static C_Return SearchData(string sSubCourseYear, string sProYear, string sSubCourseID, string sProID)
    {
        C_Return TReturn = new C_Return();
        TReturn.lstData = new List<C_DataReport2>();
        if (UserAccount.IsExpired)
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            var lstSub_C = db.T_Course_Sub.AsEnumerable().Where(w =>
            (!string.IsNullOrEmpty(sSubCourseYear) ? (w.dStart.HasValue ? w.dStart.Value.Year + "" == sSubCourseYear : false) : true) &&
            (!string.IsNullOrEmpty(sSubCourseID) ? w.nCourseSubID + "" == sSubCourseID : true)).ToList();

            var lstPro = db.T_Project.Where(w => w.IsActive && !w.IsDel).ToList();
            var lstPro_ID = lstPro.Select(s => s.nProjectID).Distinct().ToList();

            var lstSubPro = db.T_Project_Course.Where(w => lstPro_ID.Contains(w.nProjectID)).ToList();

            bool IsSearchPro = false;
            if (!string.IsNullOrEmpty(sProID) || !string.IsNullOrEmpty(sProYear))
            {
                IsSearchPro = true;
            }

            foreach (var item in lstSub_C)
            {
                C_DataReport2 a = new C_DataReport2();
                a.sSubC_Year = item.dStart.HasValue ? item.dStart.Value.Year + "" : "";
                a.sSubC_Name = item.sName;
                a.sTraining_Cost = item.nPrice.HasValue ? item.nPrice.Value + "" : "";
                a.lstData = new List<C_DataProject>();
                a.lstData = (from sp in lstSubPro.Where(w => w.nCourseSubID == item.nCourseSubID)
                             from pj in lstPro.Where(w => w.nProjectID == sp.nProjectID
                             && (!string.IsNullOrEmpty(sProID) ? w.nProjectID + "" == sProID : true)
                             && (!string.IsNullOrEmpty(sProYear) ? (w.dStart.HasValue ? w.dStart.Value.Year + "" == sProYear : false) : true))
                             select new C_DataProject
                             {
                                 sPro_Year = pj != null ? (pj.dStart.HasValue ? pj.dStart.Value.Year + "" : "") : "",
                                 sPro_Name = pj != null ? pj.sProjectName : "",
                                 sPro_Return = pj != null ? (pj.nProductivity == 1 ? (pj.nReturn_Economy + pj.nReturn_Environment + pj.nReturn_Other + pj.nReturn_Social) + "" : pj.sDescription) : "",
                             }).OrderByDescending(o => o.sPro_Year).ToList();

                if (IsSearchPro)
                {
                    if (a.lstData.Count > 0)
                        TReturn.lstData.Add(a);
                }
                else TReturn.lstData.Add(a);
            }
        }
        return TReturn;
    }
    #endregion

    #region Export Excel
    protected void btnExport__Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            List<C_DataReport2> lstData = new List<C_DataReport2>();
            string sColorHeadTb = Human_Function.GetColorHeader_Report();

            #region Query Data
            var lstSub_C = db.T_Course_Sub.AsEnumerable().Where(w =>
            (!string.IsNullOrEmpty(hddSubYear.Value) ? (w.dStart.HasValue ? w.dStart.Value.Year + "" == hddSubYear.Value : false) : true) &&
            (!string.IsNullOrEmpty(hddSubCourseID.Value) ? w.nCourseSubID + "" == hddSubCourseID.Value : true)).ToList();

            var lstPro = db.T_Project.Where(w => w.IsActive && !w.IsDel).ToList();
            var lstPro_ID = lstPro.Select(s => s.nProjectID).Distinct().ToList();

            var lstSubPro = db.T_Project_Course.Where(w => lstPro_ID.Contains(w.nProjectID)).ToList();

            bool IsSearchPro = false;
            if (!string.IsNullOrEmpty(hddProYear.Value) || !string.IsNullOrEmpty(hddProID.Value))
            {
                IsSearchPro = true;
            }

            foreach (var item in lstSub_C)
            {
                C_DataReport2 a = new C_DataReport2();
                a.sSubC_Year = item.dStart.HasValue ? item.dStart.Value.Year + "" : "";
                a.sSubC_Name = item.sName;
                a.sTraining_Cost = item.nPrice.HasValue ? item.nPrice.Value + "" : "";
                a.lstData = new List<C_DataProject>();
                a.lstData = (from sp in lstSubPro.Where(w => w.nCourseSubID == item.nCourseSubID)
                             from pj in lstPro.Where(w => w.nProjectID == sp.nProjectID
                             && (!string.IsNullOrEmpty(hddProID.Value) ? w.nProjectID + "" == hddProID.Value : true)
                             && (!string.IsNullOrEmpty(hddProYear.Value) ? (w.dStart.HasValue ? w.dStart.Value.Year + "" == hddProYear.Value : false) : true))
                             select new C_DataProject
                             {
                                 sPro_Year = pj != null ? (pj.dStart.HasValue ? pj.dStart.Value.Year + "" : "") : "",
                                 sPro_Name = pj != null ? pj.sProjectName : "",
                                 sPro_Return = pj != null ? (pj.nProductivity == 1 ? (pj.nReturn_Economy + pj.nReturn_Environment + pj.nReturn_Other + pj.nReturn_Social) + "" : pj.sDescription) : "",
                             }).OrderByDescending(o => o.sPro_Year).ToList();

                if (IsSearchPro)
                {
                    if (a.lstData.Count > 0)
                        lstData.Add(a);
                }
                else lstData.Add(a);
            }

            #endregion

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

            #region Create Sheet
            HttpResponse httpResponse = Response;
            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws1 = wb.Worksheets.Add("Sheet1");
            ws1.PageSetup.Margins.Top = 0.2;
            ws1.PageSetup.Margins.Bottom = 0.2;
            ws1.PageSetup.Margins.Left = 0.1;
            ws1.PageSetup.Margins.Right = 0;
            ws1.PageSetup.Margins.Footer = 0;
            ws1.PageSetup.Margins.Header = 0;
            ws1.Style.Font.FontName = "Cordia New";//"Browallia New";
            ws1.Style.Font.FontSize = 12;
            //ws1.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //ws1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws1.Style.Alignment.WrapText = true;

            XLColor colorHead = XLColor.FromHtml(sColorHeadTb);
            #endregion

            #region Create Head
            int nRow = 1, nCol = 1, nColLast = 0, nRowLast = 0;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15, "Sub-Course Year");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 40, "Sub-Course Name");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 22, "Training Cost (THB/Year)");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20, "Project Year");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 45, "Project Name");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 30, "Return/Non-Monetary (THB/Year)");
            nColLast = nCol;

            nCol++;
            nRow++;
            nRowLast++;
            #endregion

            #region Bind Data

            foreach (var item in lstData)
            {
                nCol = 1;
                int nRowMerge = item.lstData.Count;
                int nRowFirstSub = 0;
                bool IsHasSub = nRowMerge > 0;

                if (IsHasSub) ws1.Range(nRow, nCol, (nRow + (nRowMerge - 1)), nCol).Merge();
                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, item.sSubC_Year);
                nCol++;

                if (IsHasSub) ws1.Range(nRow, nCol, (nRow + (nRowMerge - 1)), nCol).Merge();
                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, item.sSubC_Name);
                nCol++;

                if (IsHasSub) ws1.Range(nRow, nCol, (nRow + (nRowMerge - 1)), nCol).Merge();
                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 0, null, item.sTraining_Cost);
                nCol++;
                nRowFirstSub = nRow;
                if (IsHasSub)
                {
                    item.lstData.ForEach(f =>
                    {
                        nRowLast++;
                        nCol = 4;
                        SetTbl(ws1, nRowFirstSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, f.sPro_Year);
                        nCol++;

                        SetTbl(ws1, nRowFirstSub, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, f.sPro_Name);
                        nCol++;

                        SetTbl(ws1, nRowFirstSub, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 0, null, f.sPro_Return);
                        nCol++;
                        nRowFirstSub++;
                    });
                }
                else
                {
                    nRowLast++;
                    SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "");
                    nCol++;

                    SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, "");
                    nCol++;

                    SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null, "");
                    nCol++;
                }

                if (IsHasSub) nRow = (nRow + nRowMerge);
                else nRow++;
            }
            ws1.Range(1, 1, nRowLast, nColLast).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(1, 1, nRowLast, nColLast).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            #endregion

            #region CreateEXCEL
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string sName = "Project_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

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
    public class c_ReturnCourseSub : sysGlobalClass.CResutlWebMethod
    {
        public List<T_Course_Sub> lstData { get; set; }
    }

    [Serializable]
    public class c_ReturnProject : sysGlobalClass.CResutlWebMethod
    {
        public List<T_Project> lstData { get; set; }
    }

    [Serializable]
    public class C_Return : sysGlobalClass.CResutlWebMethod
    {
        public List<C_DataReport2> lstData { get; set; }
    }

    [Serializable]
    public class C_DataReport2
    {
        public string sSubC_Year { get; set; }
        public string sSubC_Name { get; set; }
        public string sTraining_Cost { get; set; }
        public List<C_DataProject> lstData { get; set; }
    }

    [Serializable]
    public class C_DataProject
    {
        public string sPro_Year { get; set; }
        public string sPro_Name { get; set; }
        public string sPro_Return { get; set; }
    }
    #endregion
}