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

public partial class course_sub : System.Web.UI.Page
{
    private static int nMenuID = 4;

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

                #region Link Course
                string str = Request.QueryString["str"];
                string sID = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";
                if (!string.IsNullOrEmpty(sID))
                {
                    PTTGC_HumanEntities db = new PTTGC_HumanEntities();
                    var q = db.T_Course.FirstOrDefault(w => w.nCourseID + "" == sID);
                    //txtName.Text = (q != null ? q.sName : "");
                    lblHeader.Text = SystemFunction.GetMenuName(nMenuID, false, "") + " <i class='fa fa-chevron-right'></i> " + (q != null ? q.sName : "");
                }
                ddlCourse.Items.FindByValue(sID).Selected = true;
                #endregion
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

        var lstCourse = Human_Function.Get_ddl_Course();
        ddlCourse.DataSource = lstCourse;
        ddlCourse.DataValueField = "Value";
        ddlCourse.DataTextField = "Text";
        ddlCourse.DataBind();
        ddlCourse.Items.Insert(0, new ListItem("- Course -", ""));
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnData Search(string sName, string sCompanyID, string sActive, string sCourseID)
    {
        TReturnData result = new TReturnData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            sName = sName.ToLower().Replace(" ", "");

            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();

            var lstTB_SubCourse = db.T_Course_Sub.Where(w => !w.IsDel && lstCP.Contains(w.nCompanyID + "")
                            && (!string.IsNullOrEmpty(sName) ? (w.sName.ToLower().Replace(" ", "").Contains(sName)) : true)
                            && (!string.IsNullOrEmpty(sCompanyID) ? (w.nCompanyID + "" == sCompanyID) : true)
                            && (!string.IsNullOrEmpty(sCourseID) ? (w.nCourseID + "" == sCourseID) : true)
                            && (!string.IsNullOrEmpty(sActive) ? (w.IsActive == (sActive == "1")) : true)
                            ).OrderByDescending(o => o.dUpdate).ToList();
            var lstSubCourseID = lstTB_SubCourse.Select(s => s.nCourseSubID).ToList();
            var lstMT_TM = db.TM_MasterData_Sub.Where(w => w.nMainID == 6).ToList();
            var lstTB_Company = db.TB_Company.Where(w => !w.IsDel && w.IsActive).ToList();

            //var lstT_Course_Sub_TG = db.T_Course_Sub_TG.Where(w => lstSubCourseID.Contains(w.nCourseSubID)).ToList();
            //var lstMT_TG = db.TM_MasterData_Sub.Where(w => w.nMainID == 7).ToList();
            //Func<int, string> GetTG = (nCourseSubID) =>
            //{
            //    string sRet = "";

            //    var lstC_TG = lstT_Course_Sub_TG.Where(w => w.nCourseSubID == nCourseSubID).Select(s => s.nTargetGroup).ToList();
            //    var lstTG = lstMT_TG.Where(w => lstC_TG.Contains(w.nSubID)).Select(s => s.sName).ToList();

            //    return string.Join("<br>", lstTG);
            //};

            var lstData = (from sc in lstTB_SubCourse
                           from c in db.T_Course.Where(w => w.nCourseID == sc.nCourseID && w.nCompanyID == sc.nCompanyID && !w.IsDel).DefaultIfEmpty()
                           from tc in lstTB_Company.Where(w => w.nCompanyID == sc.nCompanyID).DefaultIfEmpty()
                           from ms2 in lstMT_TM.Where(w => w.nSubID == sc.nTraining_method).DefaultIfEmpty()
                           select new c_Subcourse
                           {
                               sYear = (sc.dStart.HasValue ? sc.dStart.Value.Year + "" : ""),
                               sCompanyName = (tc != null ? tc.sCompanyName : ""),
                               nCourseSubID = sc.nCourseSubID,
                               sSubCourseName = sc.sName,
                               sCourseName = c != null ? c.sName : "",
                               sDateStart = (sc.dStart.HasValue ? sc.dStart.Value.ToString("dd/MM/yyyy") : ""),
                               sDateEnd = (sc.dEnd.HasValue ? sc.dEnd.Value.ToString("dd/MM/yyyy") : ""),
                               sPrice = (sc.nPrice.HasValue ? sc.nPrice.Value.ToString() : ""),
                               sAmount = (sc.nAmount.HasValue ? sc.nAmount.Value.ToString() : ""),
                               //sTargetGroup = GetTG(sc.nCourseSubID),
                               sTraining_method = ms2 != null ? ms2.sName : "",
                               sActive = sc.IsActive ? "Active" : "Inactive",
                               sUpdateDate = sc.dUpdate.ToString("dd/MM/yyyy"),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(sc.nCourseSubID + "")),
                               dUpdate = sc.dUpdate,
                               sDescription = sc.sDescription,
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
                var lstData = db.T_Course_Sub.Where(w => lstID.Contains(w.nCourseSubID)).ToList();
                foreach (var item in lstData)
                {
                    item.IsDel = true;
                    item.dUpdate = DateTime.Now;
                    item.nUpdateBy = nUserID;
                }
                db.SaveChanges();
                Human_Function.UpdateLog(nMenuID, "", "Delete SubCourse = " + string.Join(", ", lstID));
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
            #region Query Data
            hddtxtName.Value = hddtxtName.Value.ToLower().Replace(" ", "");

            var lstCP = Human_Function.Get_ddl_CompanyByPermission().Select(s => s.Value).ToList();
            var lstTB_SubCourse = db.T_Course_Sub.Where(w => !w.IsDel && lstCP.Contains(w.nCompanyID + "")
                            && (!string.IsNullOrEmpty(hddtxtName.Value) ? (w.sName.ToLower().Replace(" ", "").Contains(hddtxtName.Value)) : true)
                            && (!string.IsNullOrEmpty(hddCompanyID.Value) ? (w.nCompanyID + "" == hddCompanyID.Value) : true)
                            && (!string.IsNullOrEmpty(hddCourseID.Value) ? (w.nCourseID + "" == hddCourseID.Value) : true)
                            && (!string.IsNullOrEmpty(hddActive.Value) ? (w.IsActive == (hddActive.Value == "1")) : true)
                            ).OrderByDescending(o => o.dUpdate).ToList();

            var lstTB_Company = db.TB_Company.Where(w => !w.IsDel && w.IsActive).ToList();

            var lstSubCourseID = lstTB_SubCourse.Select(s => s.nCourseSubID).ToList();
            var lstT_Course_Sub_TG = db.T_Course_Sub_TG.Where(w => lstSubCourseID.Contains(w.nCourseSubID)).ToList();
            var lstMT_TG = db.TM_MasterData_Sub.Where(w => w.nMainID == 7).ToList();
            Func<int, string> GetTG = (nCourseSubID) =>
            {
                string sRet = "";

                var lstC_TG = lstT_Course_Sub_TG.Where(w => w.nCourseSubID == nCourseSubID).Select(s => s.nTargetGroup).ToList();
                var lstTG = lstMT_TG.Where(w => lstC_TG.Contains(w.nSubID)).Select(s => s.sName).ToList();

                return string.Join(", ", lstTG);
            };

            var lstData = (from sc in lstTB_SubCourse
                           from c in db.T_Course.Where(w => w.nCourseID == sc.nCourseID && w.nCompanyID == sc.nCompanyID && !w.IsDel).DefaultIfEmpty()
                           from tc in lstTB_Company.Where(w => w.nCompanyID == sc.nCompanyID).DefaultIfEmpty()
                           from ms2 in db.TM_MasterData_Sub.Where(w => w.nSubID == sc.nTraining_method).DefaultIfEmpty()
                           select new c_Subcourse
                           {
                               sYear = (sc.dStart.HasValue ? sc.dStart.Value.Year + "" : ""),
                               sCompanyName = (tc != null ? tc.sCompanyName : ""),
                               nCourseSubID = sc.nCourseSubID,
                               sSubCourseName = sc.sName,
                               sCourseName = c != null ? c.sName : "",
                               sDateStart = (sc.dStart.HasValue ? sc.dStart.Value.ToString("dd/MM/yyyy") : ""),
                               sDateEnd = (sc.dEnd.HasValue ? sc.dEnd.Value.ToString("dd/MM/yyyy") : ""),
                               sPrice = (sc.nPrice.HasValue ? sc.nPrice.Value.ToString() : ""),
                               sAmount = (sc.nAmount.HasValue ? sc.nAmount.Value.ToString() : ""),
                               sTargetGroup = GetTG(sc.nCourseSubID),
                               sTraining_method = ms2 != null ? ms2.sName : "",
                               sActive = sc.IsActive ? "Active" : "Inactive",
                               sUpdateDate = sc.dUpdate.ToString("dd/MM/yyyy"),
                               sIDEncrypt = HttpUtility.UrlEncode(STCrypt.Encrypt(sc.nCourseSubID + "")),
                               dUpdate = sc.dUpdate,
                               sDescription = sc.sDescription,
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

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 8, "No");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 8, "Year");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 55, "Company");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 37, "Course");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 33, "Sub Course");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 55, "Description");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "Start Date");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "End Date");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20, "Training Method");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20, "Target Group");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "Price");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17, "Number of Participants");
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

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sYear + "");
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sCompanyName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sCourseName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sSubCourseName);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sDescription);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sDateStart);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, i.sDateEnd);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sTraining_method);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null, i.sTargetGroup);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 0, null, i.sPrice);
                nCol++;

                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, 0, null, i.sAmount);
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

            string sName = "ReportCourseSub_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

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
        public IEnumerable<c_Subcourse> lstData { get; set; }
    }

    [Serializable]
    public class c_Subcourse
    {
        public int nCourseSubID { get; set; }
        public string sYear { get; set; }
        public string sCompanyName { get; set; }
        public string sCourseName { get; set; }
        public string sSubCourseName { get; set; }
        public string sDescription { get; set; }
        public string sDateStart { get; set; }
        public string sDateEnd { get; set; }
        public string sTimeStart { get; set; }
        public string sTimeEnd { get; set; }
        public string sPrice { get; set; }
        public string sAmount { get; set; }
        public string sActive { get; set; }
        public string sUpdateDate { get; set; }
        public string sIDEncrypt { get; set; }
        public DateTime dUpdate { get; set; }
        public string sTraining_method { get; set; }
        public string sTargetGroup { get; set; }
    }
    #endregion
}