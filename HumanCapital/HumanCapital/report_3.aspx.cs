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

public partial class report_3 : System.Web.UI.Page
{
    private static int nMenuID = 15;

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
        #endregion

        #region Year
        var lstDJSI = Human_Function.Get_ddl_Year();
        ddlYear.DataSource = lstDJSI;
        ddlYear.DataValueField = "Value";
        ddlYear.DataTextField = "Text";
        ddlYear.DataBind();
        #endregion
    }

    public static List<c_djsi_report> GetDataReport(string[] arrComID, int nYear)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        #region Variable Declaration
        var lstDJSI_Master = db.TM_DJSI.ToList();
        var lstUnit = db.TM_MasterData_Sub.Where(w => w.nMainID == 5 && w.IsActive && !w.IsDel).ToList();

        var lstReportYear = db.T_Report_Year.Where(w => arrComID.Contains(w.nCompanyID + "") && w.nYear == nYear).ToList();
        var lstReportID = lstReportYear.Select(s => s.nReportID).ToList();
        var lstReportYearItem = db.T_Report_Year_Item.Where(w => lstReportID.Contains(w.nReportID)).ToList();

        Func<int, bool, List<decimal?>> GetValue_Sum = (nItem, IsSetHead) =>
        {
            var lstRet = new List<decimal?>();
            var lstData_ = lstReportYearItem.Where(w => w.nItem == nItem);
            if (IsSetHead) { lstRet = new List<decimal?>() { lstData_.Sum(s => s.nMale ?? 0), lstData_.Sum(s => s.nFemale ?? 0) }; }
            foreach (var nComID in arrComID)
            {
                var qReport = lstReportYear.FirstOrDefault(w => w.nCompanyID + "" == nComID);
                var nReportID = qReport != null ? qReport.nReportID : 0;
                var qItem = lstReportYearItem.FirstOrDefault(w => w.nItem == nItem && w.nReportID == nReportID);
                var hasItem = qItem != null;
                lstRet.Add(hasItem ? qItem.nMale : null);
                lstRet.Add(hasItem ? qItem.nFemale : null);
            }
            return lstRet;
        };

        Func<decimal, decimal> CalDeimal = (nVal) => { return Math.Round(nVal, 2, MidpointRounding.AwayFromZero); };

        #endregion

        #region Set Data
        var lstDJSI = new List<c_ReportDJSI_Item>();
        var lstFreeText = new List<decimal?>();
        int nComCount = (arrComID.Length + 1) * 2;
        for (int i = 1; i <= nComCount; i++) { lstFreeText.Add(null); }

        #region 3. Total Employee by Employment Contract and by Area

        #region Permanent contract[4]

        #region 3.2 Rayong
        var n16 = GetValue_Sum(16, true);
        decimal nM_16 = n16[0] ?? 0;
        decimal nF_16 = n16[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 16, lstData = n16 });
        #endregion

        #region 3.3 Bangkok
        var n17 = GetValue_Sum(17, true);
        decimal nM_17 = n17[0] ?? 0;
        decimal nF_17 = n17[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 17, lstData = n17 });
        #endregion

        #region 3.4 Other provinces
        var n18 = GetValue_Sum(18, true);
        decimal nM_18 = n18[0] ?? 0;
        decimal nF_18 = n18[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 18, lstData = n18 });
        #endregion

        #region 3.1 Permanent contract[4]
        decimal nM_15 = nM_16 + nM_17 + nM_18;
        decimal nF_15 = nF_16 + nF_17 + nF_18;
        var n15 = GetValue_Sum(15, false);
        n15.Insert(0, nM_15);
        n15.Insert(1, nF_15);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 15, lstData = n15 });
        #endregion

        #endregion

        #region On contract (Temporary contract) < 1 ปี[5]

        #region 3.6 Rayong
        var n20 = GetValue_Sum(20, true);
        decimal nM_20 = n20[0] ?? 0;
        decimal nF_20 = n20[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 20, lstData = n20 });
        #endregion

        #region 3.7 Bangkok
        var n21 = GetValue_Sum(21, true);
        decimal nM_21 = n21[0] ?? 0;
        decimal nF_21 = n21[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 21, lstData = n21 });
        #endregion

        #region 3.8 Other provinces
        var n22 = GetValue_Sum(22, true);
        decimal nM_22 = n22[0] ?? 0;
        decimal nF_22 = n22[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 22, lstData = n22 });
        #endregion

        #region 3.5 On contract (Temporary contract) < 1 ปี[5]
        decimal nM_19 = nM_20 + nM_21 + nM_22;
        decimal nF_19 = nF_20 + nF_21 + nF_22;
        var n19 = GetValue_Sum(19, false);
        n19.Insert(0, nM_19);
        n19.Insert(1, nF_19);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 19, lstData = n19 });
        #endregion

        #endregion

        #region On contract (Temporary contract) > 1 ปี[5]

        #region 3.10 Rayong
        var n24 = GetValue_Sum(24, true);
        decimal nM_24 = n24[0] ?? 0;
        decimal nF_24 = n24[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 24, lstData = n24 });
        #endregion

        #region 3.11 Bangkok
        var n25 = GetValue_Sum(25, true);
        decimal nM_25 = n25[0] ?? 0;
        decimal nF_25 = n25[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 25, lstData = n25 });
        #endregion

        #region 3.12 Other provinces
        var n26 = GetValue_Sum(26, true);
        decimal nM_26 = n26[0] ?? 0;
        decimal nF_26 = n26[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 26, lstData = n26 });
        #endregion

        #region 3.9 On contract (Temporary contract) < 1 ปี[5]
        decimal nM_23 = nM_24 + nM_25 + nM_26;
        decimal nF_23 = nF_24 + nF_25 + nF_26;
        var n23 = GetValue_Sum(23, false);
        n23.Insert(0, nM_23);
        n23.Insert(1, nF_23);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 23, lstData = n23 });
        #endregion

        #endregion

        #endregion

        #region 2. Total employee by Area

        #region 2.1 Rayong
        decimal nM_11 = nM_16 + nM_20 + nM_24;
        decimal nF_11 = nF_16 + nF_20 + nF_24;
        var n11 = GetValue_Sum(11, false);
        n11.Insert(0, nM_11);
        n11.Insert(1, nF_11);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 11, lstData = n11 });
        #endregion

        #region 2.2 Bangkok
        decimal nM_12 = nM_17 + nM_21 + nM_25;
        decimal nF_12 = nF_17 + nF_21 + nF_25;
        var n12 = GetValue_Sum(12, false);
        n12.Insert(0, nM_12);
        n12.Insert(1, nF_12);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 12, lstData = n12 });
        #endregion

        #region 2.3 Other provinces
        decimal nM_13 = nM_18 + nM_22 + nM_26;
        decimal nF_13 = nF_18 + nF_22 + nF_26;
        var n13 = GetValue_Sum(13, false);
        n13.Insert(0, nM_13);
        n13.Insert(1, nF_13);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 13, lstData = n13 });
        #endregion

        #endregion

        #region 1. Worker

        #region 1.2 Total employee[2] => from 2. Total employee by Area
        decimal nM_5 = nM_11 + nM_12 + nM_13;
        decimal nF_5 = nF_11 + nF_12 + nF_13;
        var n5 = GetValue_Sum(5, false);
        n5.Insert(0, nM_5);
        n5.Insert(1, nF_5);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 5, lstData = n5 });

        decimal nM_4 = nM_5 + nF_5;
        var n4 = GetValue_Sum(4, false);
        n4.Insert(0, nM_4);
        n4.Insert(1, null);
        bool IsM4 = nM_4 > 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 4, lstData = n4 });
        #endregion

        #region 1.3 Contractor[3]
        var n7 = GetValue_Sum(7, true);
        decimal nM_7 = n7[0] ?? 0;
        decimal nF_7 = n7[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 7, lstData = n7 });

        decimal nM_6 = nM_7 + nF_7;
        var n6 = GetValue_Sum(6, false);
        n6.Insert(0, nM_6);
        n6.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 6, lstData = n6 });
        #endregion

        #region 1.4 Other (please specific, if any)
        var n9 = GetValue_Sum(9, true);
        decimal nM_9 = n9[0] ?? 0;
        decimal nF_9 = n9[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 9, lstData = n9 }); ;

        decimal nM_8 = nM_9 + nF_9;
        var n8 = GetValue_Sum(8, false);
        n8.Insert(0, nM_8);
        n8.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 8, lstData = n8 });
        #endregion

        #region 1.1 Total worker[1]
        var nM_3 = nM_5 + nM_7 + nM_9;
        var nF_3 = nF_5 + nF_7 + nF_9;
        var n3 = GetValue_Sum(3, false);
        n3.Insert(0, nM_3);
        n3.Insert(1, nF_3); ;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 3, lstData = n3 });

        decimal nM_2 = nM_3 + nF_3;
        var n2 = GetValue_Sum(2, false);
        n2.Insert(0, nM_2);
        n2.Insert(1, null); ;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 2, lstData = n2 });
        #endregion

        #endregion

        #region 4. Total Employee by Employment Type

        #region 4.1 Full-time
        var n28 = GetValue_Sum(28, true);
        decimal nM_28 = n28[0] ?? 0;
        decimal nF_28 = n28[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 28, lstData = n28 });
        #endregion

        #region 4.2 Part-time
        decimal nM_29 = nM_5 - nM_28;
        decimal nF_29 = nF_5 - nF_28;
        var n29 = GetValue_Sum(29, false);
        n29.Insert(0, nM_29);
        n29.Insert(1, nF_29);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 29, lstData = n29 });
        #endregion

        #endregion

        #region 5. Total Employee by Age group

        #region 5.1 <30 years
        var n32 = GetValue_Sum(32, true);
        decimal nM_32 = n32[0] ?? 0;
        decimal nF_32 = n32[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 32, lstData = n32 });

        decimal nM_31 = IsM4 ? CalDeimal((nM_32 / nM_4) * 100) : 0;
        decimal nF_31 = IsM4 ? CalDeimal((nF_32 / nM_4) * 100) : 0;
        var n31 = GetValue_Sum(31, false);
        n31.Insert(0, nM_31);
        n31.Insert(1, nF_31);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 31, lstData = n31 });
        #endregion

        #region 5.2 30 - 50 years
        var n34 = GetValue_Sum(34, true);
        decimal nM_34 = n34[0] ?? 0;
        decimal nF_34 = n34[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 34, lstData = n34 });

        decimal nM_33 = IsM4 ? CalDeimal((nM_34 / nM_4) * 100) : 0;
        decimal nF_33 = IsM4 ? CalDeimal((nF_34 / nM_4) * 100) : 0;
        var n33 = GetValue_Sum(33, false);
        n33.Insert(0, nM_33);
        n33.Insert(1, nF_33);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 33, lstData = n33 });
        #endregion

        #region 5.3 >50 years
        decimal nM_36 = nM_5 - nM_32 - nM_34;
        decimal nF_36 = nF_5 - nF_32 - nF_34;
        var n36 = GetValue_Sum(36, false);
        n36.Insert(0, nM_36);
        n36.Insert(1, nF_36);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 36, lstData = n36 });

        decimal nM_35 = IsM4 ? CalDeimal((nM_36 / nM_4) * 100) : 0;
        decimal nF_35 = IsM4 ? CalDeimal((nF_36 / nM_4) * 100) : 0;
        var n35 = GetValue_Sum(35, false);
        n35.Insert(0, nM_35);
        n35.Insert(1, nF_35);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 35, lstData = n35 });
        #endregion

        #endregion

        #region 6. Total Employee by Employee Category (level)

        #region 6.1 Executive (Level 13-18)
        var n39 = GetValue_Sum(39, true);
        decimal nM_39 = n39[0] ?? 0;
        decimal nF_39 = n39[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 39, lstData = n39 });

        decimal nM_38 = IsM4 ? CalDeimal((nM_39 / nM_4) * 100) : 0;
        decimal nF_38 = IsM4 ? CalDeimal((nF_39 / nM_4) * 100) : 0;
        var n38 = GetValue_Sum(38, false);
        n38.Insert(0, nM_38);
        n38.Insert(1, nF_38);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 38, lstData = n38 });
        #endregion

        #region 6.2 Middle management (Level 10-12)
        var n41 = GetValue_Sum(41, true);
        decimal nM_41 = n41[0] ?? 0;
        decimal nF_41 = n41[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 41, lstData = n41 });

        decimal nM_40 = IsM4 ? CalDeimal((nM_41 / nM_4) * 100) : 0;
        decimal nF_40 = IsM4 ? CalDeimal((nF_41 / nM_4) * 100) : 0;
        var n40 = GetValue_Sum(40, false);
        n40.Insert(0, nM_40);
        n40.Insert(1, nF_40);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 40, lstData = n40 });
        #endregion

        #region 6.3 Senior (Level 8-9)
        var n43 = GetValue_Sum(43, true);
        decimal nM_43 = n43[0] ?? 0;
        decimal nF_43 = n43[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 43, lstData = n43 });

        decimal nM_42 = IsM4 ? CalDeimal((nM_43 / nM_4) * 100) : 0;
        decimal nF_42 = IsM4 ? CalDeimal((nF_43 / nM_4) * 100) : 0;
        var n42 = GetValue_Sum(42, false);
        n42.Insert(0, nM_42);
        n42.Insert(1, nF_42);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 42, lstData = n42 });
        #endregion

        #region 6.4 Employee (Level 7 and Below)
        var n45 = GetValue_Sum(45, true);
        decimal nM_45 = n45[0] ?? 0;
        decimal nF_45 = n45[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 45, lstData = n45 });

        decimal nM_44 = IsM4 ? CalDeimal((nM_45 / nM_4) * 100) : 0;
        decimal nF_44 = IsM4 ? CalDeimal((nF_45 / nM_4) * 100) : 0;
        var n44 = GetValue_Sum(44, false);
        n44.Insert(0, nM_44);
        n44.Insert(1, nF_44);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 44, lstData = n44 });
        #endregion

        #region 6.5 Unclassified[6]
        decimal nM_47 = nM_5 - (nM_39 + nM_41 + nM_43 + nM_45);
        decimal nF_47 = nF_5 - (nF_39 + nF_41 + nF_43 + nF_45);
        var n47 = GetValue_Sum(47, false);
        n47.Insert(0, nM_47);
        n47.Insert(1, nF_47);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 47, lstData = n47 });

        decimal nM_46 = IsM4 ? CalDeimal((nM_47 / nM_4) * 100) : 0;
        decimal nF_46 = IsM4 ? CalDeimal((nF_47 / nM_4) * 100) : 0;
        var n46 = GetValue_Sum(46, false);
        n46.Insert(0, nM_46);
        n46.Insert(1, nF_46);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 46, lstData = n46 });
        #endregion

        #endregion

        #region 8. New Employee by Area

        #region 8.1 Rayong
        var n54 = GetValue_Sum(54, true);
        var nM_54 = n54[0] ?? 0;
        var nF_54 = n54[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 54, lstData = n54 });

        var nM_55 = IsM4 ? CalDeimal((nM_54 / nM_4) * 100) : 0;
        var nF_55 = IsM4 ? CalDeimal((nF_54 / nM_4) * 100) : 0;
        var n55 = GetValue_Sum(55, false);
        n55.Insert(0, nM_55);
        n55.Insert(1, nF_55);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 55, lstData = n55 });
        #endregion

        #region 8.2 Bangkok
        var n56 = GetValue_Sum(56, true);
        var nM_56 = n56[0] ?? 0;
        var nF_56 = n56[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 56, lstData = n56 });

        var nM_57 = IsM4 ? CalDeimal((nM_56 / nM_4) * 100) : 0;
        var nF_57 = IsM4 ? CalDeimal((nF_56 / nM_4) * 100) : 0;
        var n57 = GetValue_Sum(57, false);
        n57.Insert(0, nM_57);
        n57.Insert(1, nF_57);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 57, lstData = n57 });
        #endregion

        #region 8.3 Other provinces
        var n58 = GetValue_Sum(58, true);
        var nM_58 = n58[0] ?? 0;
        var nF_58 = n58[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 58, lstData = n58 });

        var nM_59 = IsM4 ? CalDeimal((nM_58 / nM_4) * 100) : 0;
        var nF_59 = IsM4 ? CalDeimal((nF_58 / nM_4) * 100) : 0;
        var n59 = GetValue_Sum(59, false);
        n59.Insert(0, nM_59);
        n59.Insert(1, nF_59);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 59, lstData = n59 });
        #endregion

        #endregion

        #region 7. New Employee

        #region 7.1 New employee
        var n50 = GetValue_Sum(50, false);
        decimal nM_50 = nM_54 + nM_56 + nM_58;
        decimal nF_50 = nF_54 + nF_56 + nF_58;
        n50.Insert(0, nM_50);
        n50.Insert(1, nF_50);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 50, lstData = n50 });

        decimal nM_49 = nM_50 + nF_50;
        var n49 = GetValue_Sum(49, false);
        n49.Insert(0, nM_49);
        n49.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 49, lstData = n49 });
        #endregion

        #region 7.2 New hire rate
        decimal nM_51 = IsM4 ? CalDeimal((nM_49 / nM_4) * 100) : 0;
        var n51 = GetValue_Sum(51, false);
        n51.Insert(0, nM_51);
        n51.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 51, lstData = n51 });

        decimal nM_52 = IsM4 ? CalDeimal((nM_50 / nM_4) * 100) : 0;
        decimal nF_52 = IsM4 ? CalDeimal((nF_50 / nM_4) * 100) : 0;
        var n52 = GetValue_Sum(52, false);
        n52.Insert(0, nM_52);
        n52.Insert(1, nF_52);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 52, lstData = n52 });
        #endregion

        #endregion

        #region 9. New Employee Hire by Age Group

        #region 9.1 <30 years
        var n61 = GetValue_Sum(61, true);
        var nM_61 = n61[0] ?? 0;
        var nF_61 = n61[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 61, lstData = n61 });

        var nM_62 = IsM4 ? CalDeimal((nM_61 / nM_4) * 100) : 0;
        var nF_62 = IsM4 ? CalDeimal((nF_61 / nM_4) * 100) : 0;
        var n62 = GetValue_Sum(62, false);
        n62.Insert(0, nM_62);
        n62.Insert(1, nF_62);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 62, lstData = n62 });
        #endregion

        #region 9.2 30 - 50 years
        var n63 = GetValue_Sum(63, true);
        var nM_63 = n63[0] ?? 0;
        var nF_63 = n63[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 63, lstData = n63 });

        var nM_64 = IsM4 ? CalDeimal((nM_63 / nM_4) * 100) : 0;
        var nF_64 = IsM4 ? CalDeimal((nF_63 / nM_4) * 100) : 0;
        var n64 = GetValue_Sum(64, false);
        n64.Insert(0, nM_64);
        n64.Insert(1, nF_64);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 64, lstData = n64 });
        #endregion

        #region 9.3 >50 years
        var nM_65 = nM_50 - (nM_61 + nM_63);
        var nF_65 = nF_50 - (nF_61 + nF_63);
        var n65 = GetValue_Sum(65, false);
        n65.Insert(0, nM_65);
        n65.Insert(1, nF_65);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 65, lstData = n65 });

        var nM_66 = IsM4 ? CalDeimal((nM_65 / nM_4) * 100) : 0;
        var nF_66 = IsM4 ? CalDeimal((nF_65 / nM_4) * 100) : 0;
        var n66 = GetValue_Sum(66, false);
        n66.Insert(0, nM_66);
        n66.Insert(1, nF_66);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 66, lstData = n66 });
        #endregion

        #endregion

        #region 11. Turnover Rate by Age Group

        #region 11.1 < 30 years
        var n77 = GetValue_Sum(77, true);
        var nM_77 = n77[0] ?? 0;
        var nF_77 = n77[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 77, lstData = n77 });

        var nM_78 = IsM4 ? CalDeimal((nM_77 / nM_4) * 100) : 0;
        var nF_78 = IsM4 ? CalDeimal((nF_77 / nM_4) * 100) : 0;
        var n78 = GetValue_Sum(78, false);
        n78.Insert(0, nM_78);
        n78.Insert(1, nF_78);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 78, lstData = n78 });
        #endregion

        #region 11.2 30 - 50 years
        var n79 = GetValue_Sum(79, true);
        var nM_79 = n79[0] ?? 0;
        var nF_79 = n79[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 79, lstData = n79 });

        var nM_80 = IsM4 ? CalDeimal((nM_79 / nM_4) * 100) : 0;
        var nF_80 = IsM4 ? CalDeimal((nF_79 / nM_4) * 100) : 0;
        var n80 = GetValue_Sum(80, false);
        n80.Insert(0, nM_80);
        n80.Insert(1, nF_80);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 80, lstData = n80 });
        #endregion

        #region 11.3 > 50 years
        var n81 = GetValue_Sum(81, true);
        var nM_81 = n81[0] ?? 0;
        var nF_81 = n81[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 81, lstData = n81 });

        var nM_82 = IsM4 ? CalDeimal((nM_81 / nM_4) * 100) : 0;
        var nF_82 = IsM4 ? CalDeimal((nF_81 / nM_4) * 100) : 0;
        var n82 = GetValue_Sum(82, false);
        n82.Insert(0, nM_82);
        n82.Insert(1, nF_82);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 82, lstData = n82 });
        #endregion

        #endregion

        #region 10. Turnover

        #region 10.1 Total employee turnover rate
        decimal nM_69 = nM_77 + nM_79 + nM_81;
        decimal nF_69 = nF_77 + nF_79 + nF_81;
        var n69 = GetValue_Sum(69, false);
        n69.Insert(0, nM_69);
        n69.Insert(1, nF_69);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 69, lstData = n69 });

        decimal nM_68 = nM_69 + nF_69;
        var n68 = GetValue_Sum(68, false);
        n68.Insert(0, nM_68);
        n68.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 68, lstData = n68 });

        var nM_70 = IsM4 ? CalDeimal((nM_68 / nM_4) * 100) : 0;
        var n70 = GetValue_Sum(70, false);
        n70.Insert(0, nM_70);
        n70.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 70, lstData = n70 });

        var nM_71 = IsM4 ? CalDeimal((nM_69 / nM_4) * 100) : 0;
        var nF_71 = IsM4 ? CalDeimal((nF_69 / nM_4) * 100) : 0;
        var n71 = GetValue_Sum(71, false);
        n71.Insert(0, nM_71);
        n71.Insert(1, nF_71);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 71, lstData = n71 });
        #endregion

        #region 10.2 Voluntary employee turnover rate
        var n73 = GetValue_Sum(73, true);
        decimal nM_73 = n73[0] ?? 0;
        decimal nF_73 = n73[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 73, lstData = n73 });

        decimal nM_72 = nM_73 + nF_73;
        var n72 = GetValue_Sum(72, false);
        n72.Insert(0, nM_72);
        n72.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 72, lstData = n72 });

        var nM_74 = IsM4 ? CalDeimal((nM_72 / nM_4) * 100) : 0;
        var n74 = GetValue_Sum(74, false);
        n74.Insert(0, nM_74);
        n74.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 74, lstData = n74 });

        var nM_75 = IsM4 ? CalDeimal((nM_73 / nM_4) * 100) : 0;
        var nF_75 = IsM4 ? CalDeimal((nF_73 / nM_4) * 100) : 0;
        var n75 = GetValue_Sum(75, false);
        n75.Insert(0, nM_75);
        n75.Insert(1, nF_75);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 75, lstData = n75 });
        #endregion

        #endregion

        #region 12. Turnover by Area

        #region 12.1 Rayong
        var n84 = GetValue_Sum(84, true);
        var nM_84 = n84[0] ?? 0;
        var nF_84 = n84[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 84, lstData = n84 });

        var nM_85 = IsM4 ? CalDeimal((nM_84 / nM_4) * 100) : 0;
        var nF_85 = IsM4 ? CalDeimal((nF_84 / nM_4) * 100) : 0;
        var n85 = GetValue_Sum(85, false);
        n85.Insert(0, nM_85);
        n85.Insert(1, nF_85);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 85, lstData = n85 });
        #endregion

        #region 12.2 Bangkok
        var n86 = GetValue_Sum(86, true);
        var nM_86 = n86[0] ?? 0;
        var nF_86 = n86[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 86, lstData = n86 });

        var nM_87 = IsM4 ? CalDeimal((nM_86 / nM_4) * 100) : 0;
        var nF_87 = IsM4 ? CalDeimal((nF_86 / nM_4) * 100) : 0;
        var n87 = GetValue_Sum(87, false);
        n87.Insert(0, nM_87);
        n87.Insert(1, nF_87);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 87, lstData = n87 });
        #endregion

        #region 12.3 Other provinces
        var nM_88 = nM_69 - (nM_84 + nM_86);
        var nF_88 = nF_69 - (nF_84 + nF_86);
        var n88 = GetValue_Sum(88, false);
        n88.Insert(0, nM_88);
        n88.Insert(1, nF_88);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 88, lstData = n88 });

        var nM_89 = IsM4 ? CalDeimal((nM_88 / nM_4) * 100) : 0;
        var nF_89 = IsM4 ? CalDeimal((nF_88 / nM_4) * 100) : 0;
        var n89 = GetValue_Sum(89, false);
        n89.Insert(0, nM_89);
        n89.Insert(1, nF_89);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 89, lstData = n89 });
        #endregion

        #region 12.4 Average hiring cost / FTE in the last fiscal year
        var n90 = GetValue_Sum(90, true);
        var nM_90 = n90[0];
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 90, lstData = n90 });
        #endregion

        #endregion

        #region 13. Parental Leave

        #region 13.1 Number of employees entitled to parental leave
        var n92 = GetValue_Sum(92, true);
        decimal nM_92 = n92[0] ?? 0;
        decimal nF_92 = n92[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 92, lstData = n92 });
        #endregion

        #region 13.2 Number of employees taking parental leave
        var n93 = GetValue_Sum(93, true);
        decimal nM_93 = n93[0] ?? 0;
        decimal nF_93 = n93[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 93, lstData = n93 });
        #endregion

        #region 13.3 Number of employees returning to work after parental leave
        var n94 = GetValue_Sum(94, true);
        decimal nM_94 = n94[0] ?? 0;
        decimal nF_94 = n94[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 94, lstData = n94 });
        #endregion

        #region 13.4 Number of employees returning to work after parental leave who were still employed for 12 months after returning
        var n95 = GetValue_Sum(95, true);
        decimal nM_95 = n95[0] ?? 0;
        decimal nF_95 = n95[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 95, lstData = n95 });
        #endregion

        #region 13.5 Employee returning to work retention rate
        //decimal n93_ = nM_93 + nF_93;
        //bool Is93 = n93_ > 0;
        //decimal nM_96 = Is93 ? CalDeimal((nM_95 / n93_) * 100) : 0;
        //decimal nF_96 = Is93 ? CalDeimal((nF_95 / n93_) * 100) : 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 96, lstData = lstFreeText });
        #endregion

        #endregion

        #region 14. Training and Development

        #region 14.1 Average hours per FTE of training and development            
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 99, lstData = lstFreeText });

        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 98, lstData = lstFreeText });
        #endregion

        #region 14.2 Average amount spent per FTE on training and development
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 100, lstData = lstFreeText });
        #endregion

        #region 14.3 Percentage of open positions filled by internal candidates
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 101, lstData = lstFreeText });
        #endregion

        #region 14.4 Total investment on employees training
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 102, lstData = lstFreeText });
        #endregion

        #region 14.6 Executive
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 104, lstData = lstFreeText });
        #endregion

        #region 14.7 Middle management
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 105, lstData = lstFreeText });
        #endregion

        #region 14.8 Senior
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 106, lstData = lstFreeText });
        #endregion

        #region 14.9 Employee
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 107, lstData = lstFreeText });
        #endregion

        #region 14.5 Average hours of training by employee category (level)
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 103, lstData = lstFreeText });
        #endregion

        #region 14.10 Quantitative financial benefit of human capital investment over time (e.g. EVA/FTEs, HROI)
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 108, lstData = lstFreeText });
        #endregion

        #region 14.11 a) Total Revenue
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 109, lstData = lstFreeText });
        #endregion

        #region 14.12 b) Total Operating expenses
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 110, lstData = lstFreeText });
        #endregion

        #region 14.13 c) Total employee-related expense (Salaries+Benefits)
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 111, lstData = lstFreeText });
        #endregion

        #region 14.14 Resulting HC ROI : (a - (b-c)) / c
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 112, lstData = lstFreeText });
        #endregion

        #region 14.15 Total FTEs
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 113, lstData = lstFreeText });
        #endregion

        #endregion

        #region 15. Talent Attraction and Retention

        #region 15.1 Management by objectives: systematic use of agreed measurable targets by line superior
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 115, lstData = lstFreeText });
        #endregion

        #region 15.2 Multidimensional performance appraisal
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 116, lstData = lstFreeText });
        #endregion

        #region 15.3 Formal comparative ranking of employees within one employee category
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 117, lstData = lstFreeText });
        #endregion

        #region 15.4 Employee engagement result
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 118, lstData = lstFreeText });

        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 119, lstData = lstFreeText });
        #endregion

        #region 15.5 Employee engagement target
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 120, lstData = lstFreeText });

        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 121, lstData = lstFreeText });
        #endregion

        #region 15.6 Coverage
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 122, lstData = lstFreeText });

        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 123, lstData = lstFreeText });
        #endregion

        #endregion

        #region 16. Employee Receiving Regular Performance and Career Development Reviews (Excl. People in Unclassified Group)

        #region 16.1 Executive
        var n125 = GetValue_Sum(125, true);
        decimal nM_125 = n125[0] ?? 0;
        decimal nF_125 = n125[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 125, lstData = n125 });

        decimal nM_126 = nM_39 > 0 ? CalDeimal((nM_125 / nM_39) * 100) : 0;
        decimal nF_126 = nF_39 > 0 ? CalDeimal((nF_125 / nF_39) * 100) : 0;
        var n126 = GetValue_Sum(126, false);
        n126.Insert(0, nM_126);
        n126.Insert(1, nF_126);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 126, lstData = n126 });
        #endregion

        #region 16.2 Middle management
        var n127 = GetValue_Sum(127, true);
        decimal nM_127 = n127[0] ?? 0;
        decimal nF_127 = n127[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 127, lstData = n127 });

        decimal nM_128 = nM_41 > 0 ? CalDeimal((nM_127 / nM_41) * 100) : 0;
        decimal nF_128 = nF_41 > 0 ? CalDeimal((nF_127 / nF_41) * 100) : 0;
        var n128 = GetValue_Sum(128, false);
        n128.Insert(0, nM_128);
        n128.Insert(1, nF_128);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 128, lstData = n128 });
        #endregion

        #region 16.3 Senior
        var n129 = GetValue_Sum(129, true);
        decimal nM_129 = n129[0] ?? 0;
        decimal nF_129 = n129[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 129, lstData = n129 });

        decimal nM_130 = nM_43 > 0 ? CalDeimal((nM_129 / nM_43) * 100) : 0;
        decimal nF_130 = nF_43 > 0 ? CalDeimal((nF_129 / nF_43) * 100) : 0;
        var n130 = GetValue_Sum(130, false);
        n130.Insert(0, nM_130);
        n130.Insert(1, nF_130);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 130, lstData = n130 });
        #endregion

        #region 16.4 Employee
        var n131 = GetValue_Sum(131, true);
        decimal nM_131 = n131[0] ?? 0;
        decimal nF_131 = n131[1] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 131, lstData = n131 });

        decimal nM_132 = nM_45 > 0 ? CalDeimal((nM_131 / nM_45) * 100) : 0;
        decimal nF_132 = nF_45 > 0 ? CalDeimal((nF_131 / nF_45) * 100) : 0;
        var n132 = GetValue_Sum(132, false);
        n132.Insert(0, nM_132);
        n132.Insert(1, nF_132);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 132, lstData = n132 });
        #endregion

        #endregion

        #region 18. Board structure

        #region 18.2 Number of executive director
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 149, lstData = lstFreeText });
        #endregion

        #region 18.3 Number of non-executive directors (excl. independent directors)
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 150, lstData = lstFreeText });
        #endregion

        #region 18.4 Number of independent directors
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 151, lstData = lstFreeText });
        #endregion

        #region 18.1 Total number of board members
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 148, lstData = lstFreeText });
        #endregion

        #endregion

        #region 17. Gender Diversity and Equal Remuneration

        #region 17.1 Women in workforce
        decimal nM_134 = nF_5;
        var n134 = GetValue_Sum(134, false);
        n134.Insert(0, nM_134);
        n134.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 134, lstData = n134 });

        decimal nM_135 = IsM4 ? CalDeimal((nM_134 / nM_4) * 100) : 0;
        var n135 = GetValue_Sum(135, false);
        n135.Insert(0, nM_135);
        n135.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 135, lstData = n135 });
        #endregion

        #region 17.2 Women in management positions
        decimal nM_136 = nF_39 + nF_41;
        var n136 = GetValue_Sum(136, false);
        n136.Insert(0, nM_136);
        n136.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 136, lstData = n136 });

        decimal n3941 = nM_39 + nF_39 + nM_41 + nF_41;
        decimal nM_137 = n3941 > 0 ? CalDeimal((nM_136 / n3941) * 100) : 0;
        var n137 = GetValue_Sum(137, false);
        n137.Insert(0, nM_137);
        n137.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 137, lstData = n137 });
        #endregion

        #region 17.3 Women in top management positions
        decimal nM_138 = nF_39;
        var n138 = GetValue_Sum(138, false);
        n138.Insert(0, nM_138);
        n138.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 138, lstData = n138 });

        decimal n39_ = nM_39 + nF_39;
        decimal nM_139 = n39_ > 0 ? CalDeimal((nM_138 / n39_) * 100) : 0;
        var n139 = GetValue_Sum(139, false);
        n139.Insert(0, nM_139);
        n139.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 139, lstData = n139 });
        #endregion

        #region 17.4 Women in junior management positions
        decimal nM_140 = nF_41;
        var n140 = GetValue_Sum(140, false);
        n140.Insert(0, nM_140);
        n140.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 140, lstData = n140 });

        decimal n41_ = nM_41 + nF_41;
        decimal nM_141 = n41_ > 0 ? CalDeimal((nM_140 / n41_) * 100) : 0;
        var n141 = GetValue_Sum(141, false);
        n141.Insert(0, nM_141);
        n141.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 141, lstData = n141 });
        #endregion

        #region 17.5 Women in management positions in revenue generating functions e.g. Sales, Marketing, Operation and BD that under BU
        var n142 = GetValue_Sum(142, true);
        decimal nM_142 = n142[0] ?? 0;
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 142, lstData = n142 });

        decimal nM_143 = CalDeimal((nM_142 / 18) * 100);
        var n143 = GetValue_Sum(142, false);
        n143.Insert(0, nM_143);
        n143.Insert(1, 0);
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 143, lstData = n143 });
        #endregion

        #region 17.6 Number of women on board of directors/supervisory board
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 144, lstData = lstFreeText });

        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 145, lstData = lstFreeText });
        #endregion

        #region 17.7 Ratio basic salary women/men
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 146, lstData = lstFreeText });
        #endregion

        #endregion

        #region 19. Labor Practice Indicators

        #region 19.1 Employees represented by an independent trade union
        lstDJSI.Add(new c_ReportDJSI_Item() { nItem = 153, lstData = lstFreeText });
        #endregion

        #endregion

        #endregion

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
                           lstData = c != null ? c.lstData : new List<decimal?>(),
                       }).ToList();

        return lstData;
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnLoadData LoadData(string[] arrComID, int nYear)
    {
        TReturnLoadData result = new TReturnLoadData();
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            result.lstDJSI = GetDataReport(arrComID, nYear);

            var lstCompanyName = db.TB_Company.Where(w => arrComID.Contains(w.nCompanyID + "")).Select(s => s.sCompanyAbbr).ToList();
            lstCompanyName.Insert(0, "GC Group");
            result.lstCompanyName = lstCompanyName;
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

            #region Get Data
            string[] arrComID = hddCompanyID.Value.Split(',');
            int nYear = CommonFunction.GetIntNullToZero(hddYear.Value);

            var lstDJSI = GetDataReport(arrComID, nYear);

            var lstCompanyName = db.TB_Company.Where(w => arrComID.Contains(w.nCompanyID + "")).Select(s => s.sCompanyAbbr).ToList();
            lstCompanyName.Insert(0, "GC Group");
            #endregion

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

            ws1.Style.Alignment.WrapText = true;

            XLColor colorHead = XLColor.FromHtml(sColorHeadTb);
            #endregion
            #endregion

            #region Set Data
            int nRow = 1, nCol = 1;
            int nColCom = lstCompanyName.Count() * 2;
            int nCol_Last = (nColCom + 2);

            #region Set Header
            ws1.Range(nRow, nCol, (nRow + 2), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 2), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 46, "Required Data");
            nCol++;

            ws1.Range(nRow, nCol, (nRow + 2), nCol).Merge();
            ws1.Range(nRow, nCol, (nRow + 2), nCol).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 18, "Unit");
            nCol++;

            ws1.Range(nRow, nCol, nRow, nCol_Last).Merge();
            ws1.Range(nRow, nCol, nRow, nCol_Last).Style.Fill.BackgroundColor = colorHead;
            SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Data Collection Period");
            nRow++;

            nCol = 3;
            foreach (var sComAbbr in lstCompanyName)
            {
                ws1.Range(nRow, nCol, nRow, (nCol + 1)).Merge();
                ws1.Range(nRow, nCol, nRow, (nCol + 1)).Style.Fill.BackgroundColor = colorHead;
                SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, sComAbbr);
                nCol += 2;
            }
            nRow++;

            nCol = 3;
            for (int i = 0; i < lstCompanyName.Count; i++)
            {
                ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Male");
                nCol++;

                ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                SetTbl(ws1, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null, "Female");
                nCol++;
            }

            nRow++;
            #endregion

            #region Set Border All Table
            ws1.Range(1, 1, 156, nCol_Last).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(1, 1, 156, nCol_Last).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            #endregion

            #region Set Body
            if (lstDJSI.Any())
            {
                var lstHead = lstDJSI.Where(w => w.IsHead == true).ToList();
                foreach (var qHead in lstHead)
                {
                    nCol = 1;

                    #region Head
                    string sHeadname = qHead.sName;
                    ws1.Range(nRow, nCol, nRow, nCol_Last).Merge();
                    ws1.Range(nRow, nCol, nRow, nCol_Last).Style.Fill.BackgroundColor = colorHead;
                    SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, false, null, null, qHead.sName);
                    nCol++; nRow++;
                    #endregion

                    #region Sub
                    var lstSub = lstDJSI.Where(w => w.nItemHead == qHead.nItem && w.nSibling == null).ToList();
                    foreach (var qSub in lstSub)
                    {
                        #region Declare & Define Variable
                        var lstSibling = lstDJSI.Where(w => w.nSibling == qSub.nItem).ToList();
                        var hasSib = lstSibling.Count() > 0;
                        var nSiblingCount = (hasSib ? lstSibling.Count() : 0) + 1;

                        var IsSibling4 = nSiblingCount == 4;
                        var qSibling_2 = lstDJSI.FirstOrDefault(w => w.nSibling == qSub.nItem);
                        var qSibling_3 = IsSibling4 ? lstSibling[1] : null;
                        var qSibling_4 = IsSibling4 ? lstSibling[2] : null;

                        nCol = 1;
                        int nRowInSib = 0;
                        int nRowInSib_2 = 0;
                        int nRowInSib_3 = 0;
                        int nRowInSib_4 = 0;
                        #endregion

                        #region Name Sub
                        string sNameSub = qSub.sName.Replace("\r", "").Replace("\n", "");
                        ws1.Range(nRow, nCol, nRow + (nSiblingCount - 1), nCol).Merge();
                        ws1.Range(nRow, nCol, nRow + (nSiblingCount - 1), nCol).Style.Fill.BackgroundColor = colorHead;
                        SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, false, null, null, sNameSub);
                        nCol++;
                        #endregion

                        #region Set Unit
                        if (!hasSib)
                        {
                            #region Has Sib
                            nRowInSib = nRow;
                            string sUnit = qSub.sUnit.Replace("\r", "").Replace("\n", "");
                            ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                            SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, sUnit);
                            nRow++;
                            #endregion
                        }
                        else
                        {
                            #region Row 1 - 2                         
                            if (qSub.nUnit == qSibling_2.nUnit)
                            {
                                #region Same Unit
                                nRowInSib = nRow;
                                string sUnit = qSub.sUnit.Replace("\r", "").Replace("\n", "");
                                ws1.Range(nRow, nCol, (nRow + 1), nCol).Merge();
                                ws1.Range(nRow, nCol, (nRow + 1), nCol).Style.Fill.BackgroundColor = colorHead;
                                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, sUnit);
                                nRowInSib_2 = nRow + 1;
                                nRow += 2;
                                #endregion
                            }
                            else
                            {
                                #region Each Unit
                                nRowInSib = nRow;
                                string sUnit = qSub.sUnit.Replace("\r", "").Replace("\n", "");
                                ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, sUnit);
                                nRow++;

                                nRowInSib_2 = nRow;
                                string sUnit2 = qSibling_2.sUnit.Replace("\r", "").Replace("\n", "");
                                ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                                SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, sUnit2);
                                nRow++;
                                #endregion
                            }
                            #endregion

                            #region Row 3- 4
                            if (IsSibling4)
                            {
                                if (qSibling_3.nUnit == qSibling_4.nUnit)
                                {
                                    #region Same Unit
                                    nRowInSib_3 = nRow;
                                    nCol = 2;
                                    string sUnit3 = qSibling_3.sUnit.Replace("\r", "").Replace("\n", "");
                                    ws1.Range(nRowInSib_3, nCol, (nRowInSib_3 + 1), nCol).Merge();
                                    ws1.Range(nRowInSib_3, nCol, (nRowInSib_3 + 1), nCol).Style.Fill.BackgroundColor = colorHead;
                                    SetTbl(ws1, nRowInSib_3, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, sUnit3);
                                    nRowInSib_4 = nRow + 1;
                                    nRow += 2;
                                    #endregion
                                }
                                else
                                {
                                    #region Each Unit
                                    nCol = 2;

                                    string sUnit3 = qSibling_3.sUnit.Replace("\r", "").Replace("\n", "");
                                    nRowInSib_3 = nRow;
                                    ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                                    SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSibling_3.sUnit);
                                    nRow++;

                                    string sUnit4 = qSibling_4.sUnit.Replace("\r", "").Replace("\n", "");
                                    nRowInSib_4 = nRow;
                                    ws1.Range(nRow, nCol, nRow, nCol).Style.Fill.BackgroundColor = colorHead;
                                    SetTbl(ws1, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, false, null, null, qSibling_4.sUnit);
                                    nRow++;
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region Set Value
                        nCol = 3;
                        if (!hasSib)
                        {
                            #region No Sibling                            
                            if ((qSub.IsTotal ?? false))
                            {
                                for (var x = 0; x < nColCom; x = x += 2)
                                {
                                    ws1.Range(nRowInSib, nCol, nRowInSib, (nCol + 1)).Merge();
                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lstData[x] + "");
                                    nCol += 2;
                                }
                            }
                            else
                            {
                                foreach (var item in qSub.lstData)
                                {
                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, item + "");
                                    nCol++;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Has Sibling    

                            #region Row 1
                            if ((qSub.IsTotal ?? false))
                            {
                                for (var x = 0; x < nColCom; x = x += 2)
                                {
                                    ws1.Range(nRowInSib, nCol, nRowInSib, (nCol + 1)).Merge();
                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSub.lstData[x] + "");
                                    nCol += 2;
                                }
                            }
                            else
                            {
                                foreach (var item in qSub.lstData)
                                {
                                    SetTbl(ws1, nRowInSib, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, item + "");
                                    nCol++;
                                }
                            }
                            #endregion

                            #region Row 2
                            nCol = 3;
                            if ((qSibling_2.IsTotal ?? false))
                            {
                                for (var x = 0; x < nColCom; x = x += 2)
                                {
                                    ws1.Range(nRowInSib_2, nCol, nRowInSib_2, (nCol + 1)).Merge();
                                    SetTbl(ws1, nRowInSib_2, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_2.lstData[x] + "");
                                    nCol += 2;
                                }
                            }
                            else
                            {
                                foreach (var item in qSibling_2.lstData)
                                {
                                    SetTbl(ws1, nRowInSib_2, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, item + "");
                                    nCol++;
                                }
                            }
                            #endregion

                            #region Row 3-4
                            if (IsSibling4)
                            {
                                #region Row 3
                                nCol = 3;
                                if ((qSibling_3.IsTotal ?? false))
                                {
                                    for (var x = 0; x < nColCom; x = x += 2)
                                    {
                                        ws1.Range(nRowInSib_3, nCol, nRowInSib_3, (nCol + 1)).Merge();
                                        SetTbl(ws1, nRowInSib_3, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_3.lstData[x] + "");
                                        nCol += 2;
                                    }
                                }
                                else
                                {
                                    foreach (var item in qSibling_3.lstData)
                                    {
                                        SetTbl(ws1, nRowInSib_3, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, item + "");
                                        nCol++;
                                    }
                                }
                                #endregion

                                #region Row 4      
                                nCol = 3;
                                if ((qSibling_4.IsTotal ?? false))
                                {
                                    for (var x = 0; x < nColCom; x = x += 2)
                                    {
                                        ws1.Range(nRowInSib_4, nCol, nRowInSib_4, (nCol + 1)).Merge();
                                        SetTbl(ws1, nRowInSib_4, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, qSibling_4.lstData[x] + "");
                                        nCol += 2;
                                    }
                                }
                                else
                                {
                                    foreach (var item in qSibling_4.lstData)
                                    {
                                        SetTbl(ws1, nRowInSib_4, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, false, 0, null, item + "");
                                        nCol++;
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion

            #endregion

            #region CreateEXCEL
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string sName = "GCGroup_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("th-TH"));

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
    public class TReturnLoadData : sysGlobalClass.CResutlWebMethod
    {
        public List<c_djsi_report> lstDJSI { get; set; }
        public List<string> lstCompanyName { get; set; }
    }

    [Serializable]
    public class c_ReportDJSI_Item
    {
        public int nItem { get; set; }
        public List<decimal?> lstData { get; set; }
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
        public List<decimal?> lstData { get; set; }
    }

    #endregion
}