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

public partial class course : System.Web.UI.Page
{
    private static int nMenuID = 3;

    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnload(myFunc);
    }
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
        var lstCompany = Human_Function.Get_ddl_CompanyByPermission();
        ddlCompany.DataSource = lstCompany;
        ddlCompany.DataValueField = "Value";
        ddlCompany.DataTextField = "Text";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("- Company -", ""));
    }

    #region  [WebMethod]
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sCompanyID, string sActive)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            sName = sName.ToLower().Replace(" ", "");

            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();

            var lstTB_Course = db.T_Course.Where(w => !w.IsDel && lstCP.Contains(w.nCompanyID + "") &&
                            (!string.IsNullOrEmpty(sName) ? (w.sName.ToLower().Replace(" ", "").Contains(sName)) : true) &&
                            (!string.IsNullOrEmpty(sCompanyID) ? (w.nCompanyID + "" == sCompanyID) : true) &&
                            (!string.IsNullOrEmpty(sActive) ? (w.IsActive == (sActive == "1")) : true)
                            ).OrderByDescending(o => o.dUpdate).ToList();

            var lstTB_Company = db.TB_Company.Where(w => !w.IsDel && w.IsActive).ToList();
            var lstT_Cource_Sub = db.T_Course_Sub.Where(w => !w.IsDel && w.IsActive).ToList();

            var lstData = (from a in lstTB_Course
                           from b in lstTB_Company.Where(w => w.nCompanyID == a.nCompanyID).DefaultIfEmpty()
                           select new c_course
                           {
                               nCourseID = a.nCourseID,
                               sCourseName = a.sName,
                               sCompanyName = b != null ? b.sCompanyName : "",
                               sActive = a.IsActive ? "Active" : "Inactive",
                               sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                               nCountSub = lstT_Cource_Sub.Any(w => w.nCourseID == a.nCourseID && !w.IsDel && (b != null ? b.nCompanyID == a.nCompanyID : false)) ? lstT_Cource_Sub.Where(w => w.nCourseID == a.nCourseID && !w.IsDel && (b != null ? b.nCompanyID == a.nCompanyID : false)).Count() : 0,
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nCourseID + "")),
                               sLink = "course_sub.aspx?str=" + HttpUtility.UrlEncode(STCrypt.Encrypt(a.nCourseID + "")),
                               sDescription = a.sDescription,
                               dUpdate = a.dUpdate,
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
                var lstData = db.T_Course.Where(w => lstID.Contains(w.nCourseID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                Human_Function.UpdateLog(nMenuID, "", "Delete Course = " + string.Join(", ", lstID));
            }
        }
        else
        {
            sRet = SystemFunction.process_SessionExpired;
        }
        return sRet;
    }
    #endregion

    #region Export Excel
    protected void btnExport__Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            hddtxtName.Value = hddtxtName.Value.ToLower().Replace(" ", "");

            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();
            var lstTB_Course = db.T_Course.Where(w => !w.IsDel && lstCP.Contains(w.nCompanyID + "") &&
                            (!string.IsNullOrEmpty(hddtxtName.Value) ? (w.sName.ToLower().Replace(" ", "").Contains(hddtxtName.Value)) : true) &&
                            (!string.IsNullOrEmpty(hddCompanyID.Value) ? (w.nCompanyID + "" == hddCompanyID.Value) : true) &&
                            (!string.IsNullOrEmpty(hddActive.Value) ? (w.IsActive == (hddActive.Value == "1")) : true)
                            ).OrderByDescending(o => o.dUpdate).ToList();

            var lstTB_Company = db.TB_Company.Where(w => !w.IsDel && w.IsActive && lstCP.Contains(w.nCompanyID + "")).ToList();
            var lstT_Cource_Sub = db.T_Course_Sub.Where(w => !w.IsDel && w.IsActive).ToList();

            var lstData = (from a in lstTB_Course
                           from b in lstTB_Company.Where(w => w.nCompanyID == a.nCompanyID).DefaultIfEmpty()
                           select new c_course
                           {
                               nCourseID = a.nCourseID,
                               sCourseName = a.sName,
                               sCompanyName = b != null ? b.sCompanyName : "",
                               sActive = a.IsActive ? "Active" : "Inactive",
                               sUpdateDate = a.dUpdate.ToString("dd/MM/yyyy"),
                               nCountSub = lstT_Cource_Sub.Any(w => w.nCourseID == a.nCourseID && !w.IsDel && (b != null ? b.nCompanyID == a.nCompanyID : false)) ? lstT_Cource_Sub.Where(w => w.nCourseID == a.nCourseID && !w.IsDel && (b != null ? b.nCompanyID == a.nCompanyID : false)).Count() : 0,
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(a.nCourseID + "")),
                               sLink = "course_sub.aspx?str=" + HttpUtility.UrlEncode(STCrypt.Encrypt(a.nCourseID + "")),
                               sDescription = a.sDescription,
                               dUpdate = a.dUpdate,
                           }).OrderByDescending(o => o.dUpdate).ToList();


            string sColorHeadTb = Human_Function.GetColorHeader_Report();

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
                        if (arr[1] != "00")
                        {
                            string sformate = dec == 0 ? "#,#" : "#,#0.0";
                            //string sformate = "0.#";
                            sWorkSheet.Cell(row, col).Style.NumberFormat.Format = sformate;
                        }
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

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 8, "No");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 37, "Course Name");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 33, "Sub Course");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 55, "Company");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 42, "Description");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 16, "Last Update");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "Status");
            nMaxCol = nCol;
            nCol++;

            nRow++;
            #endregion

            #region Add Data Cell
            int nOder = 1;
            //lstData = lstData.Take(1000).ToList();
            foreach (var i in lstData)
            {
                nCol = 1;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, nOder + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sCourseName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, Human_Function.GetSubCourseNameTO_Report(1, i.nCourseID, db.T_Course_Sub.Where(w => w.IsActive && !w.IsDel).ToList()));
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sCompanyName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sDescription);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sUpdateDate);
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

            string sName = "ReportCourse_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

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
        public IEnumerable<c_course> lstData { get; set; }
    }

    [Serializable]
    public class c_course
    {
        public int nCourseID { get; set; }
        public string sCourseName { get; set; }
        public string sCompanyName { get; set; }
        public string sDescription { get; set; }
        public int nCountSub { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public string sLink { get; set; }
        public DateTime dUpdate { get; set; }
    }
    #endregion
}