using ClassExecute;
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

public partial class report_1 : System.Web.UI.Page
{
    private static int nMenuID = 12;

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
                SetControl();
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
        var lstYear = Human_Function.Get_ddl_Year();
        ddlYearStr.DataSource = lstYear;
        ddlYearStr.DataValueField = "Value";
        ddlYearStr.DataTextField = "Text";
        ddlYearStr.DataBind();
        ddlYearStr.Items.Insert(0, new ListItem("- Year -", ""));

        ddlYearEnd.DataSource = lstYear;
        ddlYearEnd.DataValueField = "Value";
        ddlYearEnd.DataTextField = "Text";
        ddlYearEnd.DataBind();
        ddlYearEnd.Items.Insert(0, new ListItem("- Year -", ""));
        #endregion
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnLoadData LoadData(string sComID, int nType, string sYearStr, string sYearEnd, string sQuarterStr, string sQuarterEnd)// int nYear, int nQuarter
    {
        TReturnLoadData result = new TReturnLoadData();
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            result.lstQuarter_Year = new List<int>();

            #region Variable declaration
            var lstQ = new List<int>();
            var lstY = new List<int>();
            int nComID = 0;

            var nRole = UserAccount.SessionInfo.nRole;

            var lstDJSI_Master = db.TM_DJSI.ToList();
            var lstUnit = db.TM_MasterData_Sub.Where(w => w.nMainID == 5 && w.IsActive && !w.IsDel).ToList();

            var lstDJSI = new List<c_ReportDJSI_Item>();
            #endregion

            if (!string.IsNullOrEmpty(sComID))
            {
                nComID = CommonFunction.GetIntNullToZero(sComID);
            }
            switch (nType)
            {
                case 1:
                    #region By Quarter
                    if (!string.IsNullOrEmpty(sYearStr))
                    {
                        lstY.Add(CommonFunction.GetIntNullToZero(sYearStr));
                    }
                    if (!string.IsNullOrEmpty(sQuarterStr) && !string.IsNullOrEmpty(sQuarterEnd))
                    {
                        int nQStr = CommonFunction.GetIntNullToZero(sQuarterStr);
                        int nQEnd = CommonFunction.GetIntNullToZero(sQuarterEnd);
                        for (int i = nQStr; i <= nQEnd; i++)
                        {
                            lstQ.Add(i);
                        }
                    }
                    else if (!string.IsNullOrEmpty(sQuarterStr))
                    {
                        lstQ.Add(CommonFunction.GetIntNullToZero(sQuarterStr));
                    }
                    else if (!string.IsNullOrEmpty(sQuarterEnd))
                    {
                        lstQ.Add(CommonFunction.GetIntNullToZero(sQuarterEnd));
                    }
                    result.lstQuarter_Year = lstQ;

                    #region SET Value
                    var lstReportDJSI = db.T_ReportDJSI.Where(w => w.nCompanyID == nComID && lstY.Contains(w.nYear) && lstQ.Contains(w.nQuarter) && w.nStatusID == 3).OrderBy(o => o.nQuarter).ToList();
                    var lstItem = db.T_ReportDJSI_Item.ToList();
                    foreach (var item in lstDJSI_Master)
                    {
                        c_ReportDJSI_Item Data = new c_ReportDJSI_Item();
                        Data.nItem = item.nItem;
                        Data.lst = new List<decimal?>();

                        #region Quarter Search
                        lstQ.ForEach(f =>
                        {
                            #region Query Report in Quarter

                            #region Have Data Report DJSI
                            var qReport = lstReportDJSI.FirstOrDefault(w => w.nQuarter == f);
                            if (qReport != null)
                            {
                                #region Add Value Item
                                var qItem = lstItem.FirstOrDefault(w => w.nReportID == qReport.nReportID && w.nItem == item.nItem);
                                if (qItem != null)
                                {
                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(qItem.nMale_1);
                                        Data.lst.Add(qItem.nMale_2);
                                        Data.lst.Add(qItem.nMale_3);
                                    }
                                    else
                                    {
                                        Data.lst.Add(qItem.nMale_1); Data.lst.Add(qItem.nFemale_1);
                                        Data.lst.Add(qItem.nMale_2); Data.lst.Add(qItem.nFemale_2);
                                        Data.lst.Add(qItem.nMale_3); Data.lst.Add(qItem.nFemale_3);
                                    }
                                }
                                else
                                {

                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(null);
                                        Data.lst.Add(null);
                                        Data.lst.Add(null);
                                    }
                                    else
                                    {
                                        Data.lst.Add(null); Data.lst.Add(null);
                                        Data.lst.Add(null); Data.lst.Add(null);
                                        Data.lst.Add(null); Data.lst.Add(null);
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region No Data IN TB DJSI Report
                            else
                            {
                                if (item.IsTotal == true)
                                {
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                }
                                else
                                {
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                }
                            }
                            #endregion

                            #endregion
                        });
                        #endregion

                        lstDJSI.Add(Data);
                    }
                    #endregion
                    #endregion
                    break;
                case 2:
                    #region By Year
                    if (!string.IsNullOrEmpty(sYearStr) && !string.IsNullOrEmpty(sYearEnd))
                    {
                        int nYStr = CommonFunction.GetIntNullToZero(sYearStr);
                        int nYEnd = CommonFunction.GetIntNullToZero(sYearEnd);
                        for (int i = nYStr; i <= nYEnd; i++)
                        {
                            lstY.Add(i);
                        }
                    }
                    else if (!string.IsNullOrEmpty(sYearStr))
                    {
                        lstY.Add(CommonFunction.GetIntNullToZero(sYearStr));
                    }
                    else if (!string.IsNullOrEmpty(sYearEnd))
                    {
                        lstY.Add(CommonFunction.GetIntNullToZero(sYearEnd));
                    }
                    result.lstQuarter_Year = lstY;

                    #region SET Value
                    var lstReportDJSI_Year = db.T_Report_Year.Where(w => lstY.Contains(w.nYear) && w.nCompanyID == nComID).OrderBy(o => o.nYear).ToList();
                    var lstReport_Year_ReportID = lstReportDJSI_Year.Select(s => s.nReportID).ToList();
                    var lstItemReport_Year_ = db.T_Report_Year_Item.Where(w => lstReport_Year_ReportID.Contains(w.nReportID)).ToList();

                    foreach (var item in lstDJSI_Master)
                    {
                        c_ReportDJSI_Item Data = new c_ReportDJSI_Item();
                        Data.nItem = item.nItem;
                        Data.lst = new List<decimal?>();

                        lstY.ForEach(f =>
                        {
                            #region Have Data Report DJSI
                            var qReport_Year = lstReportDJSI_Year.FirstOrDefault(w => w.nYear == f);
                            if (qReport_Year != null)
                            {
                                #region Add Value Item
                                var qItem = lstItemReport_Year_.FirstOrDefault(w => w.nReportID == qReport_Year.nReportID && w.nItem == item.nItem);
                                if (qItem != null)
                                {
                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(qItem.nMale);
                                    }
                                    else
                                    {
                                        Data.lst.Add(qItem.nMale); Data.lst.Add(qItem.nFemale);
                                    }
                                }
                                else
                                {
                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(null);
                                    }
                                    else
                                    {
                                        Data.lst.Add(null); Data.lst.Add(null);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                            #region No Data IN TB DJSI Report
                            else
                            {
                                if (item.IsTotal == true)
                                {
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                }
                                else
                                {
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                }
                            }
                            #endregion
                        });
                        lstDJSI.Add(Data);
                    }

                    #endregion

                    #endregion
                    break;
            }

            var lstData = (from a in lstDJSI_Master
                           from b in lstUnit.Where(w => w.nSubID == a.nUnit).DefaultIfEmpty()
                           from c in lstDJSI.Where(w => w.nItem == a.nItem).DefaultIfEmpty()
                           select new c_djsi_report
                           {
                               nItem = a.nItem,
                               sName = a.sName,
                               nItemHead = a.nItemHead,
                               nSibling = a.nSibling,
                               IsTotal = a.IsTotal,
                               nUnit = a.nUnit,
                               sUnit = b != null ? b.sName : "",
                               IsHead = a.IsHead,
                               IsAutoCal = a.IsAutoCal,
                               IsDecimal = b != null ? b.nOrder == 1 : false,
                               lst = c != null ? c.lst : new List<decimal?>(),
                           }).ToList();


            result.lstDJSI = lstData;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    #endregion

    #region Export Excel
    protected void btnExport__Click(object sender, EventArgs e)
    {
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            #region Variable declaration
            List<int> lstQuarter_Year = new List<int>();
            var lstQ = new List<int>();
            var lstY = new List<int>();
            int nComID = 0;

            var nRole = UserAccount.SessionInfo.nRole;

            var lstDJSI_Master = db.TM_DJSI.ToList();
            var lstUnit = db.TM_MasterData_Sub.Where(w => w.nMainID == 5 && w.IsActive && !w.IsDel).ToList();

            var lstDJSI = new List<c_ReportDJSI_Item>();
            #endregion
            if (!string.IsNullOrEmpty(hddCompanyID.Value))
            {
                nComID = CommonFunction.GetIntNullToZero(hddCompanyID.Value);
            }
            switch (CommonFunction.GetIntNullToZero(hddType.Value))
            {
                case 1:
                    #region By Quarter
                    if (!string.IsNullOrEmpty(hddYearStr.Value))
                    {
                        lstY.Add(CommonFunction.GetIntNullToZero(hddYearStr.Value));
                    }
                    if (!string.IsNullOrEmpty(hddQuarterStr.Value) && !string.IsNullOrEmpty(hddQuarterEnd.Value))
                    {
                        int nQStr = CommonFunction.GetIntNullToZero(hddQuarterStr.Value);
                        int nQEnd = CommonFunction.GetIntNullToZero(hddQuarterEnd.Value);
                        for (int i = nQStr; i <= nQEnd; i++)
                        {
                            lstQ.Add(i);
                        }
                    }
                    else if (!string.IsNullOrEmpty(hddQuarterStr.Value))
                    {
                        lstQ.Add(CommonFunction.GetIntNullToZero(hddQuarterStr.Value));
                    }
                    else if (!string.IsNullOrEmpty(hddQuarterEnd.Value))
                    {
                        lstQ.Add(CommonFunction.GetIntNullToZero(hddQuarterEnd.Value));
                    }
                    lstQuarter_Year = lstQ;

                    #region SET Value
                    var lstReportDJSI = db.T_ReportDJSI.Where(w => w.nCompanyID == nComID && lstY.Contains(w.nYear) && lstQ.Contains(w.nQuarter) && w.nStatusID == 3).OrderBy(o => o.nQuarter).ToList();
                    var lstItem = db.T_ReportDJSI_Item.ToList();
                    foreach (var item in lstDJSI_Master)
                    {
                        c_ReportDJSI_Item Data = new c_ReportDJSI_Item();
                        Data.nItem = item.nItem;
                        Data.lst = new List<decimal?>();

                        #region Quarter Search
                        lstQ.ForEach(f =>
                        {
                            #region Query Report in Quarter

                            #region Have Data Report DJSI
                            var qReport = lstReportDJSI.FirstOrDefault(w => w.nQuarter == f);
                            if (qReport != null)
                            {
                                #region Add Value Item
                                var qItem = lstItem.FirstOrDefault(w => w.nReportID == qReport.nReportID && w.nItem == item.nItem);
                                if (qItem != null)
                                {
                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(qItem.nMale_1);
                                        Data.lst.Add(qItem.nMale_2);
                                        Data.lst.Add(qItem.nMale_3);
                                    }
                                    else
                                    {
                                        Data.lst.Add(qItem.nMale_1); Data.lst.Add(qItem.nFemale_1);
                                        Data.lst.Add(qItem.nMale_2); Data.lst.Add(qItem.nFemale_2);
                                        Data.lst.Add(qItem.nMale_3); Data.lst.Add(qItem.nFemale_3);
                                    }
                                }
                                else
                                {

                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(null);
                                        Data.lst.Add(null);
                                        Data.lst.Add(null);
                                    }
                                    else
                                    {
                                        Data.lst.Add(null); Data.lst.Add(null);
                                        Data.lst.Add(null); Data.lst.Add(null);
                                        Data.lst.Add(null); Data.lst.Add(null);
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region No Data IN TB DJSI Report
                            else
                            {
                                if (item.IsTotal == true)
                                {
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                }
                                else
                                {
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                }
                            }
                            #endregion

                            #endregion
                        });
                        #endregion

                        lstDJSI.Add(Data);
                    }
                    #endregion
                    #endregion
                    break;
                case 2:
                    #region By Year
                    if (!string.IsNullOrEmpty(hddYearStr.Value) && !string.IsNullOrEmpty(hddYearEnd.Value))
                    {
                        int nYStr = CommonFunction.GetIntNullToZero(hddYearStr.Value);
                        int nYEnd = CommonFunction.GetIntNullToZero(hddYearEnd.Value);
                        for (int i = nYStr; i <= nYEnd; i++)
                        {
                            lstY.Add(i);
                        }
                    }
                    else if (!string.IsNullOrEmpty(hddYearStr.Value))
                    {
                        lstY.Add(CommonFunction.GetIntNullToZero(hddYearStr.Value));
                    }
                    else if (!string.IsNullOrEmpty(hddYearEnd.Value))
                    {
                        lstY.Add(CommonFunction.GetIntNullToZero(hddYearEnd.Value));
                    }
                    lstQuarter_Year = lstY;

                    #region SET Value
                    var lstReportDJSI_Year = db.T_Report_Year.Where(w => lstY.Contains(w.nYear) && w.nCompanyID == nComID).OrderBy(o => o.nYear).ToList();
                    var lstReport_Year_ReportID = lstReportDJSI_Year.Select(s => s.nReportID).ToList();
                    var lstItemReport_Year_ = db.T_Report_Year_Item.Where(w => lstReport_Year_ReportID.Contains(w.nReportID)).ToList();

                    foreach (var item in lstDJSI_Master)
                    {
                        c_ReportDJSI_Item Data = new c_ReportDJSI_Item();
                        Data.nItem = item.nItem;
                        Data.lst = new List<decimal?>();

                        lstY.ForEach(f =>
                        {
                            #region Have Data Report DJSI
                            var qReport_Year = lstReportDJSI_Year.FirstOrDefault(w => w.nYear == f);
                            if (qReport_Year != null)
                            {
                                #region Add Value Item
                                var qItem = lstItemReport_Year_.FirstOrDefault(w => w.nReportID == qReport_Year.nReportID && w.nItem == item.nItem);
                                if (qItem != null)
                                {
                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(qItem.nMale);
                                    }
                                    else
                                    {
                                        Data.lst.Add(qItem.nMale); Data.lst.Add(qItem.nFemale);
                                    }
                                }
                                else
                                {
                                    if (item.IsTotal == true)
                                    {
                                        Data.lst.Add(null);
                                    }
                                    else
                                    {
                                        Data.lst.Add(null); Data.lst.Add(null);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                            #region No Data IN TB DJSI Report
                            else
                            {
                                if (item.IsTotal == true)
                                {
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                    Data.lst.Add(null);
                                }
                                else
                                {
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                    Data.lst.Add(null); Data.lst.Add(null);
                                }
                            }
                            #endregion
                        });
                        lstDJSI.Add(Data);
                    }

                    #endregion
                    #endregion
                    break;
            }

            var lstData = (from a in lstDJSI_Master
                           from b in lstUnit.Where(w => w.nSubID == a.nUnit).DefaultIfEmpty()
                           from c in lstDJSI.Where(w => w.nItem == a.nItem).DefaultIfEmpty()
                           select new c_djsi_report
                           {
                               nItem = a.nItem,
                               sName = a.sName,
                               nItemHead = a.nItemHead,
                               nSibling = a.nSibling,
                               IsTotal = a.IsTotal,
                               nUnit = a.nUnit,
                               sUnit = b != null ? b.sName : "",
                               IsHead = a.IsHead,
                               IsAutoCal = a.IsAutoCal,
                               IsDecimal = b != null ? b.nOrder == 1 : false,
                               lst = c != null ? c.lst : new List<decimal?>(),
                           }).ToList();

            #region Variable declaration Export
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
            #endregion

            #region Add Data Cell
            int nRow = 1, nCol = 1, nCol_CheckPoint = 0, nCol_Head = 0, nCol_Q = 0, nCol_SubQ = 0, nCol_Sex = 0, nCol_Last = 0;
            int nColSpan = 0, nCount_Col = 0;

            #region Create Head
            lstQuarter_Year.Count();
            ws1.Range(nRow, nCol, (nRow + 3), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 3), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 46, "Required Data");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 3), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 3), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 18, "Unit");
            nCol++;

            nCol_CheckPoint = nCol;

            switch (hddType.Value)
            {
                #region By Quarter
                case "1":
                    nCol_Head = nCol_CheckPoint;
                    nColSpan = lstQuarter_Year.Count * 6;
                    nCount_Col = lstQuarter_Year.Count * 3;

                    ws1.Range(nRow, nCol_Head, nRow, (nCol_Head - 1) + nColSpan).Merge();
                    ws1.Range(nRow, nCol_Head, nRow, (nCol_Head - 1) + nColSpan).Style.Fill.BackgroundColor = colorHead;
                    SetTbl(ws1, nRow, nCol_Head, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Data Collection Period");
                    nCol_Head++;

                    nCol_Q = nCol_CheckPoint;
                    nCol_SubQ = nCol_CheckPoint;
                    nCol_Sex = nCol_CheckPoint;
                    lstQuarter_Year.ForEach(f =>
                    {
                        string sMonth1 = "1", sMonth2 = "2", sMonth3 = "3";
                        switch (f)
                        {
                            case 2: sMonth1 = "4"; sMonth2 = "5"; sMonth3 = "6"; break;
                            case 3: sMonth1 = "7"; sMonth2 = "8"; sMonth3 = "9"; break;
                            case 4: sMonth1 = "10"; sMonth2 = "11"; sMonth3 = "12"; break;
                            default: break;
                        }

                        nRow = 2;
                        ws1.Range(nRow, nCol_Q, nRow, nCol_Q + (6 - 1)).Merge();
                        ws1.Range(nRow, nCol_Q, nRow, nCol_Q + (6 - 1)).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Q, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Q" + f);
                        nCol_Q = nCol_Q++ + 6;

                        nRow = 3;
                        ws1.Range(nRow, nCol_SubQ, nRow, nCol_SubQ + 1).Merge();
                        ws1.Range(nRow, nCol_SubQ, nRow, nCol_SubQ + 1).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_SubQ, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, sMonth1);
                        nCol_SubQ = nCol_SubQ + 2;

                        ws1.Range(nRow, nCol_SubQ, nRow, nCol_SubQ + 1).Merge();
                        ws1.Range(nRow, nCol_SubQ, nRow, nCol_SubQ + 1).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_SubQ, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, sMonth2);
                        nCol_SubQ = nCol_SubQ + 2;

                        ws1.Range(nRow, nCol_SubQ, nRow, nCol_SubQ + 1).Merge();
                        ws1.Range(nRow, nCol_SubQ, nRow, nCol_SubQ + 1).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_SubQ, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, sMonth3);
                        nCol_SubQ = nCol_SubQ + 2;

                        nRow = 4;
                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Male");
                        nCol_Sex++;

                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Female");
                        nCol_Sex++;

                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Male");
                        nCol_Sex++;

                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Female");
                        nCol_Sex++;

                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Male");
                        nCol_Sex++;

                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Female");
                        nCol_Sex++;
                        nCol_Last = nCol_Sex;
                    });
                    break;
                #endregion

                #region By Year
                case "2":
                    nCol_Head = nCol_CheckPoint;
                    nColSpan = lstQuarter_Year.Count * 2;
                    nCount_Col = lstQuarter_Year.Count;

                    ws1.Range(nRow, nCol_Head, (nRow + 1), (nCol_Head - 1) + nColSpan).Merge();
                    ws1.Range(nRow, nCol_Head, (nRow + 1), (nCol_Head - 1) + nColSpan).Style.Fill.BackgroundColor = colorHead;
                    SetTbl(ws1, nRow, nCol_Head, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Data Collection Period");
                    nCol_Head = nCol_CheckPoint;
                    nRow++; nRow++;

                    //ws1.Range(nRow, nCol_Head, nRow, (nCol_Head - 1) + nColSpan).Merge();
                    //ws1.Range(nRow, nCol_Head, nRow, (nCol_Head - 1) + nColSpan).Style.Fill.BackgroundColor = colorHead;
                    //SetTbl(ws1, nRow, nCol_Head, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Year");
                    //nCol_Head = nCol_CheckPoint;


                    nCol_Q = nCol_CheckPoint;
                    nCol_SubQ = nCol_CheckPoint;
                    nCol_Sex = nCol_CheckPoint;
                    lstQuarter_Year.ForEach(f =>
                    {
                        nRow = 3;
                        ws1.Range(nRow, nCol_Q, nRow, nCol_Q + (2 - 1)).Merge();
                        ws1.Range(nRow, nCol_Q, nRow, nCol_Q + (2 - 1)).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Q, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, f + "");
                        nCol_Q = nCol_Q++ + 2;

                        nRow = 4;
                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Male");
                        nCol_Sex++;

                        ws1.Range(nRow, nCol_Sex, nRow, nCol_Sex).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol_Sex, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Female");
                        nCol_Sex++;
                        nCol_Last = nCol_Sex;
                    });
                    break;
                    #endregion
            }

            ws1.Range(1, 1, 157, (nColSpan + 2)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(1, 1, 157, (nColSpan + 2)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            #endregion

            if (lstData.Any())
            {
                nCol = 1;
                nCol_Last = nCol_Last - 2;
                int nRowHead = 5;

                var lstHead = lstData.Where(w => w.IsHead == true).ToList();
                for (var i = 0; i < lstHead.Count(); i++)
                {
                    int nRowSub = 0;
                    var qHead = lstHead[i];
                    string sHeadname = lstHead[i].sName;

                    nCol = 1;
                    ws1.Range(nRowHead, nCol, nRowHead, (nCol + nCol_Last)).Merge();
                    ws1.Range(nRowHead, nCol, nRowHead, (nCol + nCol_Last)).Style.Fill.BackgroundColor = colorHead;
                    SetTbl(ws1, nRowHead, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, false, null, null, qHead.sName);
                    nRowHead++;
                    nRowSub = nRowHead;

                    var lstSub = lstData.Where(w => w.nItemHead == qHead.nItem && w.nSibling == null).ToList();
                    for (var ii = 0; ii < lstSub.Count(); ii++)
                    {
                        #region Declare & Define Variable
                        var qSub = lstSub[ii];
                        var lstSibling = lstData.Where(w => w.nSibling == qSub.nItem).ToList();
                        var hasSib = lstSibling.Count() > 0;
                        var nSiblingCount = (hasSib ? lstSibling.Count() : 0) + 1;

                        var IsSibling4 = nSiblingCount == 4;
                        var qSibling_2 = lstData.FirstOrDefault(w => w.nSibling == qSub.nItem);
                        var qSibling_3 = IsSibling4 ? lstSibling[1] : null;
                        var qSibling_4 = IsSibling4 ? lstSibling[2] : null;
                        #endregion

                        nCol = 1;

                        int nRowInSib = 0;
                        int nRowInSib_2 = 0;
                        int nRowInSib_3 = 0;
                        int nRowInSib_4 = 0;

                        qSub.sName = qSub.sName.Replace("\r", "").Replace("\n", "");

                        ws1.Range(nRowHead, nCol, nRowHead + (nSiblingCount - 1), nCol).Merge();
                        ws1.Range(nRowHead, nCol, nRowHead + (nSiblingCount - 1), nCol).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRowHead, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, false, null, null, qSub.sName);
                        nRowHead = nRowHead++ + (nSiblingCount);
                        nCol++;

                        if (!hasSib)
                        {
                            nRowInSib = nRowSub;
                            ws1.Range(nRowSub, nCol, nRowSub, nCol).Style.Fill.BackgroundColor = colorHead;
                            qSub.sUnit = qSub.sUnit.Replace("\r", "").Replace("\n", "");
                            SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSub.sUnit);
                            nCol++;
                            nRowSub++;
                        }
                        else
                        {
                            if (qSub.nUnit == qSibling_2.nUnit)
                            {
                                nRowInSib = nRowSub;
                                qSub.sUnit = qSub.sUnit.Replace("\r", "").Replace("\n", "");
                                ws1.Range(nRowSub, nCol, (nRowSub + 1), nCol).Merge();
                                ws1.Range(nRowSub, nCol, (nRowSub + 1), nCol).Style.Fill.BackgroundColor = colorHead;
                                SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSub.sUnit);
                                nRowSub++;
                                nRowInSib_2 = nRowSub;
                                nRowSub++;
                            }
                            else
                            {
                                nRowInSib = nRowSub;
                                qSub.sUnit = qSub.sUnit.Replace("\r", "").Replace("\n", "");
                                qSibling_2.sUnit = qSibling_2.sUnit.Replace("\r", "").Replace("\n", "");
                                ws1.Range(nRowSub, nCol, nRowSub, nCol).Style.Fill.BackgroundColor = colorHead;
                                SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSub.sUnit);
                                nRowSub++;

                                nRowInSib_2 = nRowSub;
                                nCol = 2;
                                ws1.Range(nRowSub, nCol, nRowSub, nCol).Style.Fill.BackgroundColor = colorHead;
                                SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSibling_2.sUnit);
                                nRowSub++;
                            }

                            if (IsSibling4)
                            {
                                if (qSibling_3.nUnit == qSibling_4.nUnit)
                                {
                                    nRowInSib_3 = nRowSub;
                                    qSibling_3.sUnit = qSibling_3.sUnit.Replace("\r", "").Replace("\n", "");
                                    nCol = 2;
                                    ws1.Range(nRowSub, nCol, (nRowSub + 1), nCol).Merge();
                                    ws1.Range(nRowSub, nCol, (nRowSub + 1), nCol).Style.Fill.BackgroundColor = colorHead;
                                    SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSibling_3.sUnit);
                                    nRowSub++;
                                    nRowInSib_4 = nRowSub;
                                    nRowSub++;
                                }
                                else
                                {
                                    qSibling_3.sUnit = qSibling_3.sUnit.Replace("\r", "").Replace("\n", "");
                                    qSibling_4.sUnit = qSibling_4.sUnit.Replace("\r", "").Replace("\n", "");
                                    nRowInSib_3 = nRowSub;
                                    ws1.Range(nRowSub, nCol, nRowSub, nCol).Style.Fill.BackgroundColor = colorHead;
                                    SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSibling_3.sUnit);
                                    nRowSub++;

                                    nCol = 2;
                                    nRowInSib_4 = nRowSub;
                                    ws1.Range(nRowSub, nCol, nRowSub, nCol).Style.Fill.BackgroundColor = colorHead;
                                    SetTbl(ws1, nRowSub, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSibling_4.sUnit);
                                    nRowSub++;
                                }
                            }
                        }
                        nCol = 3;
                        int nCol_1 = 3;
                        int nCol_2 = 3;
                        int nCol_3 = 3;
                        int nCol_4 = 3;

                        int Index = 0;
                        int Index_Sibling_2 = 0;
                        int Index_qSibling_3 = 0;
                        int Index_qSibling_4 = 0;

                        for (var iii = 0; iii < nCount_Col; iii++)
                        {
                            if (!hasSib)
                            {
                                #region No Sibling                            
                                if ((qSub.IsTotal == true))
                                {
                                    ws1.Range(nRowInSib, nCol, nRowInSib, (nCol + 1)).Merge();
                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lst[Index] + "");
                                    nCol++; nCol++;
                                }
                                else
                                {
                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lst[Index] + "");
                                    Index++; nCol++;

                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lst[Index] + "");
                                    nCol++;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Has Sibling    

                                #region Row 1
                                if ((qSub.IsTotal == true))
                                {
                                    ws1.Range(nRowInSib, nCol_1, nRowInSib, (nCol_1 + 1)).Merge();
                                    SetTbl(ws1, nRowInSib, nCol_1, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lst[Index] + "");
                                    nCol_1++; nCol_1++;
                                }
                                else
                                {
                                    SetTbl(ws1, nRowInSib, nCol_1, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lst[Index] + "");
                                    Index++; nCol_1++;

                                    SetTbl(ws1, nRowInSib, nCol_1, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lst[Index] + "");
                                    nCol_1++;
                                }
                                #endregion

                                #region Row 2
                                if ((qSibling_2.IsTotal == true))
                                {
                                    ws1.Range(nRowInSib_2, nCol_2, nRowInSib_2, (nCol_2 + 1)).Merge();
                                    SetTbl(ws1, nRowInSib_2, nCol_2, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_2.lst[Index] + "");
                                    nCol_2++; nCol_2++;
                                }
                                else
                                {
                                    SetTbl(ws1, nRowInSib_2, nCol_2, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_2.lst[Index_Sibling_2] + "");
                                    Index_Sibling_2++; nCol_2++;

                                    SetTbl(ws1, nRowInSib_2, nCol_2, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_2.lst[Index_Sibling_2] + "");
                                    nCol_2++;
                                }
                                #endregion

                                #region Row 3-4
                                if (IsSibling4)
                                {

                                    //    // #region Row 3
                                    if ((qSibling_3.IsTotal == true))
                                    {
                                        ws1.Range(nRowInSib_3, nCol_3, nRowInSib_3, (nCol_3 + 1)).Merge();
                                        SetTbl(ws1, nRowInSib_3, nCol_3, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_3.lst[Index_qSibling_3] + "");
                                        nCol_3++; nCol_3++;
                                    }
                                    else
                                    {
                                        SetTbl(ws1, nRowInSib_3, nCol_3, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_3.lst[Index_qSibling_3] + "");
                                        Index_qSibling_3++; nCol_3++;

                                        SetTbl(ws1, nRowInSib_3, nCol_3, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_3.lst[Index_qSibling_3] + "");
                                        nCol_3++;
                                    }
                                    //    // #endregion

                                    // #region Row 4      
                                    if ((qSibling_4.IsTotal == true))
                                    {
                                        ws1.Range(nRowInSib_4, nCol_4, nRowInSib_4, (nCol_4 + 1)).Merge();
                                        SetTbl(ws1, nRowInSib_4, nCol_4, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_4.lst[Index_qSibling_4] + "");
                                        nCol_4++; nCol_4++;
                                    }
                                    else
                                    {
                                        SetTbl(ws1, nRowInSib_4, nCol_4, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_4.lst[Index_qSibling_4] + "");
                                        Index_qSibling_4++; nCol_4++;

                                        SetTbl(ws1, nRowInSib_4, nCol_4, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_4.lst[Index_qSibling_4] + "");
                                        nCol_4++;
                                    }
                                    // #endregion
                                }
                                #endregion

                                #endregion
                            }
                            Index++;
                            Index_Sibling_2++;
                            Index_qSibling_3++;
                            Index_qSibling_4++;
                        }
                    }
                }
            }
            #endregion

            #region CreateEXCEL
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string sName = "LaborData_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

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

    private static List<DateTime> GetMonthFromQuarter(int nQuarter, int nYear)
    {
        List<DateTime> lst = new List<DateTime>();
        switch (nQuarter)
        {
            case 1: lst = new List<DateTime>() { new DateTime(nYear, 1, 1).Date, new DateTime(nYear, 2, 1).Date, new DateTime(nYear, 3, 1).Date }; break;
            case 2: lst = new List<DateTime>() { new DateTime(nYear, 4, 1).Date, new DateTime(nYear, 5, 1).Date, new DateTime(nYear, 6, 1).Date }; break;
            case 3: lst = new List<DateTime>() { new DateTime(nYear, 7, 1).Date, new DateTime(nYear, 8, 1).Date, new DateTime(nYear, 9, 1).Date }; break;
            case 4: lst = new List<DateTime>() { new DateTime(nYear, 10, 1).Date, new DateTime(nYear, 11, 1).Date, new DateTime(nYear, 12, 1).Date }; break;
        }
        return lst;
    }

    private static sysGlobalClass.CResutlWebMethod CompanyChange(int nComID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAccount.IsExpired)
        {
            result.Content = "";
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            var q = db.TB_Company.FirstOrDefault(w => w.nCompanyID == nComID);
            if (q != null) result.Content = q.nCompanyType + "";
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    #region Class
    [Serializable]
    public class TReturnLoadData : sysGlobalClass.CResutlWebMethod
    {
        public List<c_djsi_report> lstDJSI { get; set; }
        public List<int> lstQuarter_Year { get; set; }
    }

    [Serializable]
    public class c_ReportDJSI_Item
    {
        public int nItem { get; set; }
        public List<decimal?> lst { get; set; }
    }
    [Serializable]
    public class c_djsi_report
    {
        public int nItem { get; set; }
        public string sName { get; set; }
        public Nullable<int> nItemHead { get; set; }
        public Nullable<int> nSibling { get; set; }
        public Nullable<bool> IsTotal { get; set; }
        public Nullable<int> nUnit { get; set; }
        public string sUnit { get; set; }
        public Nullable<bool> IsHead { get; set; }
        public Nullable<bool> IsAutoCal { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDecimal { get; set; }
        public List<decimal?> lst { get; set; }
    }

    #endregion
}