using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Human_Function
/// </summary>
public class Human_Function
{
    public Human_Function()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void UpdateLog(int nMenuID, string sMenuName, string sEvent)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qMenu = db.Database.SqlQuery<TM_Menu>("select top 1 * from TM_Menu where nMenuID = " + nMenuID).FirstOrDefault();

        string sInsert = @"INSERT INTO TM_Log
           ([dLog]
           ,[nUserID]
           ,[nMenuID]
           ,[sMenuName]
           ,[sEvent])
            VALUES
           ('" + DateTime.Now + @"'
           ," + (!UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0) + @"
           ," + nMenuID + @"
           ,'" + (sMenuName != "" ? sMenuName : (qMenu != null ? qMenu.sMenuName + "" : "")) + @"'
           ,'" + sEvent + "')";

        CommonFunction.ExecuteNonQuery(sInsert);
    }
    public static string GetUserID(int? nUserID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        if (!nUserID.HasValue) nUserID = UserAccount.SessionInfo.nUserID;

        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.IsActive && !w.IsDel);
        return qUser != null ? qUser.sUserID : "";
    }
    public static string GetFirstNameNotAbbr(string sFName)
    {
        return sFName.Replace("นาง", "").Replace("นางสาว", "").Replace("น.ส.", "").Replace("นาย", "").Replace(" ", "");
    }
    public static string GetColorHeader_Report()
    {
        return "#F0F0F0";
    }
    public static bool IsAdmin(int? nUserID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        if (!nUserID.HasValue) nUserID = UserAccount.SessionInfo.nUserID;

        return db.TB_User.Any(w => w.nUserID == nUserID && w.nRole == 1 && w.IsActive && !w.IsDel);
    }
    public static List<sysGlobalClass.TBindDropdown> Get_ddl_Company()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TB_Company.Where(w => w.IsActive && !w.IsDel).ToList().Select(s => new sysGlobalClass.TBindDropdown { Text = s.sCompanyName, Value = s.nCompanyID + "" }).ToList();
    }
    public static List<sysGlobalClass.TBindDropdown> Get_ddl_TargetGroup()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.Where(w => w.IsActive && !w.IsDel && w.nMainID == 7).ToList().Select(s => new sysGlobalClass.TBindDropdown { Text = s.sName, Value = s.nSubID + "" }).ToList();
    }
    public static List<sysGlobalClass.TBindDropdown> Get_ddl_Category()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.Where(w => w.IsActive && !w.IsDel && w.nMainID == 8).ToList().Select(s => new sysGlobalClass.TBindDropdown { Text = s.sName, Value = s.nSubID + "" }).ToList();
    }
    public static List<sysGlobalClass.TBindDropdown> Get_ddl_CompanyByPermission()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        var lstRet = new List<sysGlobalClass.TBindDropdown>();
        var nUserID = UserAccount.SessionInfo.nUserID;
        var lstCom = db.TB_Company.Where(w => w.IsActive && !w.IsDel).ToList();
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.IsActive && !w.IsDel);
        if (qUser != null)
        {
            var lstComID_Per = db.TB_User_Company.Where(w => w.nUserID == nUserID).Select(s => s.nCompanyID).ToList();
            lstRet = lstCom.Where(w => lstComID_Per.Contains(w.nCompanyID)).Select(s => new sysGlobalClass.TBindDropdown { Text = s.sCompanyName, Value = s.nCompanyID + "" }).ToList();
        }

        return lstRet;
    }
    public static List<sysGlobalClass.TBindDropdown> Get_ddl_Course()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.T_Course.Where(w => w.IsActive && !w.IsDel).ToList().Select(s => new sysGlobalClass.TBindDropdown { Text = s.sName, Value = s.nCourseID + "" }).ToList();
    }
    public static List<sysGlobalClass.TBindDropdown> Get_ddl_Year()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        List<sysGlobalClass.TBindDropdown> lst = new List<sysGlobalClass.TBindDropdown>();
        int nYearStart = 2019;
        int nYearNow = DateTime.Now.Year;
        for (int nYear = nYearNow; nYear >= nYearStart; nYear--)
        {
            lst.Add(new sysGlobalClass.TBindDropdown { Value = nYear + "", Text = nYear + "" });
        }
        return lst;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sType">1=Course, 2=Project</param> 
    /// <param name="nID"></param>
    /// <returns></returns>
    public static string GetSubCourseNameTO_Report(int nType, int nID, List<T_Course_Sub> lst)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        string result = "";
        switch (nType)
        {
            case 1:
                var lstCourse_Sub = lst.Where(w => w.nCourseID == nID).ToList();
                result = " " + lstCourse_Sub.Count() + " Record(s)";
                lstCourse_Sub.ForEach(f =>
                {
                    result += Environment.NewLine + "   - " + f.sName;
                });
                break;
            case 2:
                var lstPro_Course = db.T_Project_Course.Where(w => w.nProjectID == nID).ToList();
                result = " " + lstPro_Course.Count() + " Record(s)";
                lstPro_Course.ForEach(f =>
                {
                    result += Environment.NewLine + "   - " + (lst.Any(w => w.nCourseSubID == f.nCourseSubID) ? lst.FirstOrDefault(w => w.nCourseSubID == f.nCourseSubID).sName + " (" + f.nBenefit + "%)" : "");
                });
                break;
        }
        return result;
    }
    public static void UpdateReport_ApproveLOG(int nReportID, int nStatusID, string sMg)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        if (!UserAccount.IsExpired)
        {
            db.T_ReportDJSI_Approve.Add(new T_ReportDJSI_Approve { nReportID = nReportID, nStatusID = nStatusID, sComment = sMg, nActionBy = UserAccount.SessionInfo.nUserID, dAction = DateTime.Now });
            db.SaveChanges();
        }
    }
    public static string GET_Companyname(int nComID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TB_Company.Any(w => w.nCompanyID == nComID) ? db.TB_Company.FirstOrDefault(w => w.nCompanyID == nComID).sCompanyName : "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nUser"></param>
    /// <param name="nRole"></param> 3= L1->Manager ,4=L2->SSHE Corporate
    /// <param name="nCompanyID"></param>
    public static int? GetApprover(int nRole, int nCompanyID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var lstUser = db.TB_User.Where(w => w.nRole == nRole && w.IsActive && !w.IsDel).ToList();
        var lstUserID = lstUser.Select(s => s.nUserID).ToList();
        var lstUserInCom = db.TB_User_Company.FirstOrDefault(w => lstUserID.Contains(w.nUserID) && w.nCompanyID == nCompanyID);
        return lstUserInCom == null ? (int?)null : lstUserInCom.nUserID;
    }
    public static List<int> GetUserInRole(int nRole, int nCompanyID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var lstUser = db.TB_User.Where(w => w.nRole == nRole && w.IsActive && !w.IsDel).ToList();
        var lstUserID = lstUser.Select(s => s.nUserID).ToList();
        var lstUserInCom = db.TB_User_Company.Where(w => lstUserID.Contains(w.nUserID) && w.nCompanyID == nCompanyID).ToList();
        return lstUserInCom.Any() ? lstUserInCom.Select(s => s.nUserID).ToList() : new List<int>();//lstUserInCom == null ? (int?)null : lstUserInCom.nUserID; 
    }

    public static TB_User GetUserInfo(int nUserID, List<TB_User> lst)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qUser = lst.FirstOrDefault(a => a.nUserID == nUserID && a.IsActive && !a.IsDel);
        return qUser != null ? qUser : new TB_User();
    }

    #region Get Config Mail    
    public static bool IsUseRealMail()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 25 && w.sDescription == "Y") != null;
    }
    public static string smtpmail()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 26).sDescription;
    }
    public static string SystemMail()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 27).sDescription;
    }
    public static string DemoMail_Reciever()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 28).sDescription;
    }
    #endregion

    #region Get Config SharePath    
    public static string SharePathUpFile()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 41).sDescription;
    }
    public static bool IsLogonSharePath()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 42 && w.sDescription == "Y") != null;
    }
    public static string SharePathDomain()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        return db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 43).sDescription;
    }
    #endregion

    #region Calculate Report DJSI in year
    public static void SetDJSIInYear(int nCompanyID, int nYear)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        var lstReport = db.T_ReportDJSI.Where(w => w.nCompanyID == nCompanyID && w.nYear == nYear && w.nStatusID == 3).OrderByDescending(o => o.nQuarter).ToList();
        if (lstReport.Any())
        {
            #region Define && Declare Value
            List<c_report_year> lstYear = new List<c_report_year>();
            var lstReportID = lstReport.Select(s => s.nReportID).ToList();
            var lstReportItem = db.T_ReportDJSI_Item.Where(w => lstReportID.Contains(w.nReportID)).ToList();

            Func<int, decimal[]> GetValue_Year = (nItem) =>
            {
                var lstItemAll = lstReportItem.Where(w => w.nItem == nItem);
                return new decimal[] { lstItemAll.Sum(s => (s.nMale_1 ?? 0) + (s.nMale_2 ?? 0) + (s.nMale_3 ?? 0)),
                                      lstItemAll.Sum(s => (s.nFemale_1 ?? 0) + (s.nFemale_2 ?? 0) + (s.nFemale_3 ?? 0)) };
            };

            Func<int, decimal[]> GetValue_LastMonth = (nItem) =>
            {
                decimal nMale = 0;
                decimal nFemale = 0;
                bool IsMale = false; bool IsFemale = false;
                foreach (var nReportID_ in lstReportID)
                {
                    var qItem = lstReportItem.FirstOrDefault(w => w.nReportID == nReportID_ && w.nItem == nItem);
                    if (qItem != null)
                    {
                        if (!IsMale)
                        {
                            if ((qItem.nMale_3 ?? 0) > 0) { nMale = qItem.nMale_3.Value; IsMale = true; }
                            else if ((qItem.nMale_2 ?? 0) > 0) { nMale = qItem.nMale_2.Value; IsMale = true; }
                            else if ((qItem.nMale_1 ?? 0) > 0) { nMale = qItem.nMale_1.Value; IsMale = true; }
                        }
                        if (!IsFemale)
                        {
                            if ((qItem.nFemale_3 ?? 0) > 0) { nFemale = qItem.nFemale_3.Value; IsFemale = true; }
                            else if ((qItem.nFemale_2 ?? 0) > 0) { nFemale = qItem.nFemale_2.Value; IsFemale = true; }
                            else if ((qItem.nFemale_1 ?? 0) > 0) { nFemale = qItem.nFemale_1.Value; IsFemale = true; }
                        }
                    }

                    if (IsMale && IsFemale) { break; }
                }

                return new decimal[] { nMale, nFemale };
            };

            Func<decimal, decimal> CalDeimal = (nVal) => { return Math.Round(nVal, 2, MidpointRounding.AwayFromZero); };
            #endregion

            #region Set Data

            #region 3. Total Employee by Employment Contract and by Area

            #region Permanent contract[4]

            #region 3.2 Rayong
            var n16 = GetValue_LastMonth(16);
            decimal nM_16 = n16[0];
            decimal nF_16 = n16[1];
            lstYear.Add(new c_report_year() { nItem = 16, nMale = nM_16, nFemale = nF_16 });
            #endregion

            #region 3.3 Bangkok
            var n17 = GetValue_LastMonth(17);
            decimal nM_17 = n17[0];
            decimal nF_17 = n17[1];
            lstYear.Add(new c_report_year() { nItem = 17, nMale = nM_17, nFemale = nF_17 });
            #endregion

            #region 3.4 Other provinces
            var n18 = GetValue_LastMonth(18);
            decimal nM_18 = n18[0];
            decimal nF_18 = n18[1];
            lstYear.Add(new c_report_year() { nItem = 18, nMale = nM_18, nFemale = nF_18 });
            #endregion

            #region 3.1 Permanent contract[4]
            decimal nM_15 = nM_16 + nM_17 + nM_18;
            decimal nF_15 = nF_16 + nF_17 + nF_18;
            lstYear.Add(new c_report_year() { nItem = 15, nMale = nM_15, nFemale = nF_15 });
            #endregion

            #endregion

            #region On contract (Temporary contract) < 1 ปี[5]

            #region 3.6 Rayong
            var n20 = GetValue_LastMonth(20);
            decimal nM_20 = n20[0];
            decimal nF_20 = n20[1];
            lstYear.Add(new c_report_year() { nItem = 20, nMale = nM_20, nFemale = nF_20 });
            #endregion

            #region 3.7 Bangkok
            var n21 = GetValue_LastMonth(21);
            decimal nM_21 = n21[0];
            decimal nF_21 = n21[1];
            lstYear.Add(new c_report_year() { nItem = 21, nMale = nM_21, nFemale = nF_21 });
            #endregion

            #region 3.8 Other provinces
            var n22 = GetValue_LastMonth(22);
            decimal nM_22 = n22[0];
            decimal nF_22 = n22[1];
            lstYear.Add(new c_report_year() { nItem = 22, nMale = nM_22, nFemale = nF_22 });
            #endregion

            #region 3.5 On contract (Temporary contract) < 1 ปี[5]
            decimal nM_19 = nM_20 + nM_21 + nM_22;
            decimal nF_19 = nF_20 + nF_21 + nF_22;
            lstYear.Add(new c_report_year() { nItem = 19, nMale = nM_19, nFemale = nF_19 });
            #endregion

            #endregion

            #region On contract (Temporary contract) > 1 ปี[5]

            #region 3.10 Rayong
            var n24 = GetValue_LastMonth(24);
            decimal nM_24 = n24[0];
            decimal nF_24 = n24[1];
            lstYear.Add(new c_report_year() { nItem = 24, nMale = nM_24, nFemale = nF_24 });
            #endregion

            #region 3.11 Bangkok
            var n25 = GetValue_LastMonth(25);
            decimal nM_25 = n25[0];
            decimal nF_25 = n25[1];
            lstYear.Add(new c_report_year() { nItem = 25, nMale = nM_25, nFemale = nF_25 });
            #endregion

            #region 3.12 Other provinces
            var n26 = GetValue_LastMonth(26);
            decimal nM_26 = n26[0];
            decimal nF_26 = n26[1];
            lstYear.Add(new c_report_year() { nItem = 26, nMale = nM_26, nFemale = nF_26 });
            #endregion

            #region 3.9 On contract (Temporary contract) < 1 ปี[5]
            decimal nM_23 = nM_24 + nM_25 + nM_26;
            decimal nF_23 = nF_24 + nF_25 + nF_26;
            lstYear.Add(new c_report_year() { nItem = 23, nMale = nM_23, nFemale = nF_23 });
            #endregion

            #endregion

            #endregion

            #region 2. Total employee by Area

            #region 2.1 Rayong
            decimal nM_11 = nM_16 + nM_20 + nM_24;
            decimal nF_11 = nF_16 + nF_20 + nF_24;
            lstYear.Add(new c_report_year() { nItem = 11, nMale = nM_11, nFemale = nF_11 });
            #endregion

            #region 2.2 Bangkok
            decimal nM_12 = nM_17 + nM_21 + nM_25;
            decimal nF_12 = nF_17 + nF_21 + nF_25;
            lstYear.Add(new c_report_year() { nItem = 12, nMale = nM_12, nFemale = nF_12 });
            #endregion

            #region 2.3 Other provinces
            decimal nM_13 = nM_18 + nM_22 + nM_26;
            decimal nF_13 = nF_18 + nF_22 + nF_26;
            lstYear.Add(new c_report_year() { nItem = 13, nMale = nM_13, nFemale = nF_13 });
            #endregion

            #endregion

            #region 1. Worker

            #region 1.2 Total employee[2] => from 2. Total employee by Area
            decimal nM_5 = nM_11 + nM_12 + nM_13;
            decimal nF_5 = nF_11 + nF_12 + nF_13;
            lstYear.Add(new c_report_year() { nItem = 5, nMale = nM_5, nFemale = nF_5 });

            decimal nM_4 = nM_5 + nF_5;
            bool IsM4 = nM_4 > 0;
            lstYear.Add(new c_report_year() { nItem = 4, nMale = nM_4, nFemale = 0 });
            #endregion

            #region 1.3 Contractor[3]
            var n7 = GetValue_LastMonth(7);
            decimal nM_7 = n7[0];
            decimal nF_7 = n7[1];
            lstYear.Add(new c_report_year() { nItem = 7, nMale = nM_7, nFemale = nF_7 });

            decimal nM_6 = nM_7 + nF_7;
            lstYear.Add(new c_report_year() { nItem = 6, nMale = nM_6, nFemale = 0 });
            #endregion            

            #region 1.4 Other (please specific, if any)
            var n9 = GetValue_LastMonth(9);
            decimal nM_9 = n9[0];
            decimal nF_9 = n9[1];
            lstYear.Add(new c_report_year() { nItem = 9, nMale = nM_9, nFemale = nF_9 });

            decimal nM_8 = nM_9 + nF_9;
            lstYear.Add(new c_report_year() { nItem = 8, nMale = nM_8, nFemale = 0 });
            #endregion

            #region 1.1 Total worker[1]
            var nM_3 = nM_5 + nM_7 + nM_9;
            var nF_3 = nF_5 + nF_7 + nF_9;
            lstYear.Add(new c_report_year() { nItem = 3, nMale = nM_3, nFemale = nF_3 });

            decimal nM_2 = nM_3 + nF_3;
            lstYear.Add(new c_report_year() { nItem = 2, nMale = nM_2, nFemale = 0 });
            #endregion

            #endregion            

            #region 4. Total Employee by Employment Type

            #region 4.1 Full-time
            var n28 = GetValue_LastMonth(28);
            decimal nM_28 = n28[0];
            decimal nF_28 = n28[1];
            lstYear.Add(new c_report_year() { nItem = 28, nMale = nM_28, nFemale = nF_28 });
            #endregion

            #region 4.2 Part-time
            decimal nM_29 = nM_5 - nM_28;
            decimal nF_29 = nF_5 - nF_28;
            //var n29 = GetValue_LastMonth(29);
            //decimal nM_29 = n29[0];
            //decimal nF_29 = n29[1];
            lstYear.Add(new c_report_year() { nItem = 29, nMale = nM_29, nFemale = nF_29 });
            #endregion

            #endregion

            #region 5. Total Employee by Age group

            #region 5.1 <30 years
            var n32 = GetValue_LastMonth(32);
            decimal nM_32 = n32[0];
            decimal nF_32 = n32[1];
            lstYear.Add(new c_report_year() { nItem = 32, nMale = nM_32, nFemale = nF_32 });

            decimal nM_31 = IsM4 ? CalDeimal((nM_32 / nM_4) * 100) : 0;
            decimal nF_31 = IsM4 ? CalDeimal((nF_32 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 31, nMale = nM_31, nFemale = nF_31 });
            #endregion

            #region 5.2 30 - 50 years
            var n34 = GetValue_LastMonth(34);
            decimal nM_34 = n34[0];
            decimal nF_34 = n34[1];
            lstYear.Add(new c_report_year() { nItem = 34, nMale = nM_34, nFemale = nF_34 });

            decimal nM_33 = IsM4 ? CalDeimal((nM_34 / nM_4) * 100) : 0;
            decimal nF_33 = IsM4 ? CalDeimal((nF_34 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 33, nMale = nM_33, nFemale = nF_33 });
            #endregion

            #region 5.3 >50 years
            decimal nM_36 = nM_5 - nM_32 - nM_34;
            decimal nF_36 = nF_5 - nF_32 - nF_34;
            //var n36 = GetValue_LastMonth(36);
            //decimal nM_36 = n36[0];
            //decimal nF_36 = n36[1];
            lstYear.Add(new c_report_year() { nItem = 36, nMale = nM_36, nFemale = nF_36 });

            decimal nM_35 = IsM4 ? CalDeimal((nM_36 / nM_4) * 100) : 0;
            decimal nF_35 = IsM4 ? CalDeimal((nF_36 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 35, nMale = nM_35, nFemale = nF_35 });
            #endregion

            #endregion

            #region 6. Total Employee by Employee Category (level)

            #region 6.1 Executive (Level 13-18)
            var n39 = GetValue_LastMonth(39);
            decimal nM_39 = n39[0];
            decimal nF_39 = n39[1];
            lstYear.Add(new c_report_year() { nItem = 39, nMale = nM_39, nFemale = nF_39 });

            decimal nM_38 = IsM4 ? CalDeimal((nM_39 / nM_4) * 100) : 0;
            decimal nF_38 = IsM4 ? CalDeimal((nF_39 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 38, nMale = nM_38, nFemale = nF_38 });
            #endregion

            #region 6.2 Middle management (Level 10-12)
            var n41 = GetValue_LastMonth(41);
            decimal nM_41 = n41[0];
            decimal nF_41 = n41[1];
            lstYear.Add(new c_report_year() { nItem = 41, nMale = nM_41, nFemale = nF_41 });

            decimal nM_40 = IsM4 ? CalDeimal((nM_41 / nM_4) * 100) : 0;
            decimal nF_40 = IsM4 ? CalDeimal((nF_41 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 40, nMale = nM_40, nFemale = nF_40 });
            #endregion

            #region 6.3 Senior (Level 8-9)
            var n43 = GetValue_LastMonth(43);
            decimal nM_43 = n43[0];
            decimal nF_43 = n43[1];
            lstYear.Add(new c_report_year() { nItem = 43, nMale = nM_43, nFemale = nF_43 });

            decimal nM_42 = IsM4 ? CalDeimal((nM_43 / nM_4) * 100) : 0;
            decimal nF_42 = IsM4 ? CalDeimal((nF_43 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 42, nMale = nM_42, nFemale = nF_42 });
            #endregion

            #region 6.4 Employee (Level 7 and Below)
            var n45 = GetValue_LastMonth(45);
            decimal nM_45 = n45[0];
            decimal nF_45 = n45[1];
            lstYear.Add(new c_report_year() { nItem = 45, nMale = nM_45, nFemale = nF_45 });

            decimal nM_44 = IsM4 ? CalDeimal((nM_45 / nM_4) * 100) : 0;
            decimal nF_44 = IsM4 ? CalDeimal((nF_45 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 44, nMale = nM_44, nFemale = nF_44 });
            #endregion

            #region 6.5 Unclassified[6]
            decimal nM_47 = nM_5 - (nM_39 + nM_41 + nM_43 + nM_45);
            decimal nF_47 = nF_5 - (nF_39 + nF_41 + nF_43 + nF_45);
            //var n47 = GetValue_LastMonth(47);
            //decimal nM_47 = n47[0];
            //decimal nF_47 = n47[1];
            lstYear.Add(new c_report_year() { nItem = 47, nMale = nM_47, nFemale = nF_47 });

            decimal nM_46 = IsM4 ? CalDeimal((nM_47 / nM_4) * 100) : 0;
            decimal nF_46 = IsM4 ? CalDeimal((nF_47 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 46, nMale = nM_46, nFemale = nF_46 });
            #endregion

            #endregion

            #region 8. New Employee by Area

            #region 8.1 Rayong
            var n54 = GetValue_Year(54);
            var nM_54 = n54[0];
            var nF_54 = n54[1];
            lstYear.Add(new c_report_year() { nItem = 54, nMale = nM_54, nFemale = nF_54 });

            var nM_55 = IsM4 ? CalDeimal((nM_54 / nM_4) * 100) : 0;
            var nF_55 = IsM4 ? CalDeimal((nF_54 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 55, nMale = nM_55, nFemale = nF_55 });
            #endregion

            #region 8.2 Bangkok
            var n56 = GetValue_Year(56);
            var nM_56 = n56[0];
            var nF_56 = n56[1];
            lstYear.Add(new c_report_year() { nItem = 56, nMale = nM_56, nFemale = nF_56 });

            var nM_57 = IsM4 ? CalDeimal((nM_56 / nM_4) * 100) : 0;
            var nF_57 = IsM4 ? CalDeimal((nF_56 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 57, nMale = nM_57, nFemale = nF_57 });
            #endregion

            #region 8.3 Other provinces
            var n58 = GetValue_Year(58);
            var nM_58 = n58[0];
            var nF_58 = n58[1];
            lstYear.Add(new c_report_year() { nItem = 58, nMale = nM_58, nFemale = nF_58 });

            var nM_59 = IsM4 ? CalDeimal((nM_58 / nM_4) * 100) : 0;
            var nF_59 = IsM4 ? CalDeimal((nF_58 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 59, nMale = nM_59, nFemale = nF_59 });
            #endregion

            #endregion

            #region 7. New Employee

            #region 7.1 New employee
            //var n50 = GetValue_Year(50);
            //decimal nM_50 = n50[0];
            //decimal nF_50 = n50[1];
            decimal nM_50 = nM_54 + nM_56 + nM_58;
            decimal nF_50 = nF_54 + nF_56 + nF_58;
            lstYear.Add(new c_report_year() { nItem = 50, nMale = nM_50, nFemale = nF_50 });

            decimal nM_49 = nM_50 + nF_50;
            lstYear.Add(new c_report_year() { nItem = 49, nMale = nM_49, nFemale = 0 });
            #endregion

            #region 7.2 New hire rate
            decimal nM_51 = IsM4 ? CalDeimal((nM_49 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 51, nMale = nM_51, nFemale = 0 });

            decimal nM_52 = IsM4 ? CalDeimal((nM_50 / nM_4) * 100) : 0;
            decimal nF_52 = IsM4 ? CalDeimal((nF_50 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 52, nMale = nM_52, nFemale = nF_52 });
            #endregion

            #endregion            

            #region 9. New Employee Hire by Age Group

            #region 9.1 <30 years
            var n61 = GetValue_Year(61);
            var nM_61 = n61[0];
            var nF_61 = n61[1];
            lstYear.Add(new c_report_year() { nItem = 61, nMale = nM_61, nFemale = nF_61 });

            var nM_62 = IsM4 ? CalDeimal((nM_61 / nM_4) * 100) : 0;
            var nF_62 = IsM4 ? CalDeimal((nF_61 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 62, nMale = nM_62, nFemale = nF_62 });
            #endregion

            #region 9.2 30 - 50 years
            var n63 = GetValue_Year(63);
            var nM_63 = n63[0];
            var nF_63 = n63[1];
            lstYear.Add(new c_report_year() { nItem = 63, nMale = nM_63, nFemale = nF_63 });

            var nM_64 = IsM4 ? CalDeimal((nM_63 / nM_4) * 100) : 0;
            var nF_64 = IsM4 ? CalDeimal((nF_63 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 64, nMale = nM_64, nFemale = nF_64 });
            #endregion

            #region 9.3 >50 years
            var nM_65 = nM_50 - (nM_61 + nM_63);
            var nF_65 = nF_50 - (nF_61 + nF_63);
            //var n65 = GetValue_LastMonth(65);
            //var nM_65 = n65[0];
            //var nF_65 = n65[1];
            lstYear.Add(new c_report_year() { nItem = 65, nMale = nM_65, nFemale = nF_65 });

            var nM_66 = IsM4 ? CalDeimal((nM_65 / nM_4) * 100) : 0;
            var nF_66 = IsM4 ? CalDeimal((nF_65 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 66, nMale = nM_66, nFemale = nF_66 });
            #endregion

            #endregion

            #region 11. Turnover Rate by Age Group

            #region 11.1 < 30 years
            var n77 = GetValue_Year(77);
            var nM_77 = n77[0];
            var nF_77 = n77[1];
            lstYear.Add(new c_report_year() { nItem = 77, nMale = nM_77, nFemale = nF_77 });

            var nM_78 = IsM4 ? CalDeimal((nM_77 / nM_4) * 100) : 0;
            var nF_78 = IsM4 ? CalDeimal((nF_77 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 78, nMale = nM_78, nFemale = nF_78 });
            #endregion

            #region 11.2 30 - 50 years
            var n79 = GetValue_Year(79);
            var nM_79 = n79[0];
            var nF_79 = n79[1];
            lstYear.Add(new c_report_year() { nItem = 79, nMale = nM_79, nFemale = nF_79 });

            var nM_80 = IsM4 ? CalDeimal((nM_79 / nM_4) * 100) : 0;
            var nF_80 = IsM4 ? CalDeimal((nF_79 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 80, nMale = nM_80, nFemale = nF_80 });
            #endregion

            #region 11.3 > 50 years
            var n81 = GetValue_Year(81);
            var nM_81 = n81[0];
            var nF_81 = n81[1];
            lstYear.Add(new c_report_year() { nItem = 81, nMale = nM_81, nFemale = nF_81 });

            var nM_82 = IsM4 ? CalDeimal((nM_81 / nM_4) * 100) : 0;
            var nF_82 = IsM4 ? CalDeimal((nF_81 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 82, nMale = nM_82, nFemale = nF_82 });
            #endregion

            #endregion

            #region 10. Turnover

            #region 10.1 Total employee turnover rate
            decimal nM_69 = nM_77 + nM_79 + nM_81;
            decimal nF_69 = nF_77 + nF_79 + nF_81;
            //var n69 = GetValue_LastMonth(69);
            //decimal nM_69 = n69[0];
            //decimal nF_69 = n69[1];
            lstYear.Add(new c_report_year() { nItem = 69, nMale = nM_69, nFemale = nF_69 });

            decimal nM_68 = nM_69 + nF_69;
            lstYear.Add(new c_report_year() { nItem = 68, nMale = nM_68, nFemale = 0 });

            var nM_70 = IsM4 ? CalDeimal((nM_68 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 70, nMale = nM_70, nFemale = 0 });

            var nM_71 = IsM4 ? CalDeimal((nM_69 / nM_4) * 100) : 0;
            var nF_71 = IsM4 ? CalDeimal((nF_69 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 71, nMale = nM_71, nFemale = nF_71 });
            #endregion

            #region 10.2 Voluntary employee turnover rate
            var n73 = GetValue_Year(73);
            decimal nM_73 = n73[0];
            decimal nF_73 = n73[1];
            lstYear.Add(new c_report_year() { nItem = 73, nMale = nM_73, nFemale = nF_73 });

            decimal nM_72 = nM_73 + nF_73;
            lstYear.Add(new c_report_year() { nItem = 72, nMale = nM_72, nFemale = 0 });

            var nM_74 = IsM4 ? CalDeimal((nM_72 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 74, nMale = nM_74, nFemale = 0 });

            var nM_75 = IsM4 ? CalDeimal((nM_73 / nM_4) * 100) : 0;
            var nF_75 = IsM4 ? CalDeimal((nF_73 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 75, nMale = nM_75, nFemale = nF_75 });
            #endregion

            #endregion

            #region 12. Turnover by Area

            #region 12.1 Rayong
            var n84 = GetValue_Year(84);
            var nM_84 = n84[0];
            var nF_84 = n84[1];
            lstYear.Add(new c_report_year() { nItem = 84, nMale = nM_84, nFemale = nF_84 });

            var nM_85 = IsM4 ? CalDeimal((nM_84 / nM_4) * 100) : 0;
            var nF_85 = IsM4 ? CalDeimal((nF_84 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 85, nMale = nM_85, nFemale = nF_85 });
            #endregion

            #region 12.2 Bangkok
            var n86 = GetValue_Year(86);
            var nM_86 = n86[0];
            var nF_86 = n86[1];
            lstYear.Add(new c_report_year() { nItem = 86, nMale = nM_86, nFemale = nF_86 });

            var nM_87 = IsM4 ? CalDeimal((nM_86 / nM_4) * 100) : 0;
            var nF_87 = IsM4 ? CalDeimal((nF_86 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 87, nMale = nM_87, nFemale = nF_87 });
            #endregion

            #region 12.3 Other provinces
            var nM_88 = nM_69 - (nM_84 + nM_86);
            var nF_88 = nF_69 - (nF_84 + nF_86);
            //var n88 = GetValue_LastMonth(88);
            //var nM_88 = n88[0];
            //var nF_88 = n88[1];
            lstYear.Add(new c_report_year() { nItem = 88, nMale = nM_88, nFemale = nF_88 });

            var nM_89 = IsM4 ? CalDeimal((nM_88 / nM_4) * 100) : 0;
            var nF_89 = IsM4 ? CalDeimal((nF_88 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 89, nMale = nM_89, nFemale = nF_89 });
            #endregion

            #region 12.4 Average hiring cost / FTE in the last fiscal year
            var n90 = GetValue_LastMonth(90);
            var nM_90 = n90[0];
            lstYear.Add(new c_report_year() { nItem = 90, nMale = nM_90, nFemale = 0 });
            #endregion

            #endregion

            #region 13. Parental Leave

            #region 13.1 Number of employees entitled to parental leave
            var n92 = GetValue_LastMonth(92);
            decimal nM_92 = n92[0];
            decimal nF_92 = n92[1];
            lstYear.Add(new c_report_year() { nItem = 92, nMale = nM_92, nFemale = nF_92 });
            #endregion

            #region 13.2 Number of employees taking parental leave
            var n93 = GetValue_LastMonth(93);
            decimal nM_93 = n93[0];
            decimal nF_93 = n93[1];
            lstYear.Add(new c_report_year() { nItem = 93, nMale = nM_93, nFemale = nF_93 });
            #endregion

            #region 13.3 Number of employees returning to work after parental leave
            var n94 = GetValue_LastMonth(94);
            decimal nM_94 = n94[0];
            decimal nF_94 = n94[1];
            lstYear.Add(new c_report_year() { nItem = 94, nMale = nM_94, nFemale = nF_94 });
            #endregion

            #region 13.4 Number of employees returning to work after parental leave who were still employed for 12 months after returning
            var n95 = GetValue_LastMonth(95);
            decimal nM_95 = n95[0];
            decimal nF_95 = n95[1];
            lstYear.Add(new c_report_year() { nItem = 95, nMale = nM_95, nFemale = nF_95 });
            #endregion

            #region 13.5 Employee returning to work retention rate
            decimal n93_ = nM_93 + nF_93;
            bool Is93 = n93_ > 0;
            decimal nM_96 = Is93 ? CalDeimal((nM_95 / n93_) * 100) : 0;
            decimal nF_96 = Is93 ? CalDeimal((nF_95 / n93_) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 96, nMale = nM_96, nFemale = nF_96 });
            #endregion

            #endregion

            #region 14. Training and Development

            #region 14.1 Average hours per FTE of training and development
            var n99 = GetValue_LastMonth(99);
            var nM_99 = n99[0];
            var nF_99 = n99[1];
            lstYear.Add(new c_report_year() { nItem = 99, nMale = nM_99, nFemale = nF_99 });

            decimal nM_98 = nM_99 + nF_99;
            //var n98 = GetValue_LastMonth(98);
            //decimal nM_98 = n98[0];
            lstYear.Add(new c_report_year() { nItem = 98, nMale = nM_98, nFemale = 0 });
            #endregion

            #region 14.2 Average amount spent per FTE on training and development
            var n100 = GetValue_LastMonth(100);
            decimal nM_100 = n100[0];
            lstYear.Add(new c_report_year() { nItem = 100, nMale = nM_100, nFemale = 0 });
            #endregion

            #region 14.3 Percentage of open positions filled by internal candidates
            var n101 = GetValue_LastMonth(101);
            decimal nM_101 = n101[0];
            lstYear.Add(new c_report_year() { nItem = 101, nMale = nM_101, nFemale = 0 });
            #endregion

            #region 14.4 Total investment on employees training
            var n102 = GetValue_LastMonth(102);
            decimal nM_102 = n102[0];
            decimal nF_102 = n102[1];
            lstYear.Add(new c_report_year() { nItem = 102, nMale = nM_102, nFemale = nF_102 });
            #endregion

            #region 14.6 Executive
            var n104 = GetValue_LastMonth(104);
            decimal nM_104 = n104[0];
            decimal nF_104 = n104[1];
            lstYear.Add(new c_report_year() { nItem = 104, nMale = nM_104, nFemale = nF_104 });
            #endregion

            #region 14.7 Middle management
            var n105 = GetValue_LastMonth(105);
            decimal nM_105 = n105[0];
            decimal nF_105 = n105[1];
            lstYear.Add(new c_report_year() { nItem = 105, nMale = nM_105, nFemale = nF_105 });
            #endregion

            #region 14.8 Senior
            var n106 = GetValue_LastMonth(106);
            decimal nM_106 = n106[0];
            decimal nF_106 = n106[1];
            lstYear.Add(new c_report_year() { nItem = 106, nMale = nM_106, nFemale = nF_106 });
            #endregion

            #region 14.9 Employee
            var n107 = GetValue_LastMonth(107);
            decimal nM_107 = n107[0];
            decimal nF_107 = n107[1];
            lstYear.Add(new c_report_year() { nItem = 107, nMale = nM_107, nFemale = nF_107 });
            #endregion

            #region 14.5 Average hours of training by employee category (level)
            var nM_103 = CalDeimal((nM_104 + nM_105 + nM_106 + nM_107) / 4);
            var nF_103 = CalDeimal((nF_104 + nF_105 + nF_106 + nF_107) / 4);
            lstYear.Add(new c_report_year() { nItem = 103, nMale = nM_103, nFemale = nF_103 });
            #endregion

            #region 14.10 Quantitative financial benefit of human capital investment over time (e.g. EVA/FTEs, HROI)
            var n108 = GetValue_LastMonth(108);
            decimal nM_108 = n108[0];
            lstYear.Add(new c_report_year() { nItem = 108, nMale = nM_108, nFemale = 0 });
            #endregion

            #region 14.11 a) Total Revenue
            var n109 = GetValue_LastMonth(109);
            decimal nM_109 = n109[0];
            lstYear.Add(new c_report_year() { nItem = 109, nMale = nM_109, nFemale = 0 });
            #endregion

            #region 14.12 b) Total Operating expenses
            var n110 = GetValue_LastMonth(110);
            decimal nM_110 = n110[0];
            lstYear.Add(new c_report_year() { nItem = 110, nMale = nM_110, nFemale = 0 });
            #endregion

            #region 14.13 c) Total employee-related expense (Salaries+Benefits)
            var n111 = GetValue_LastMonth(111);
            decimal nM_111 = n111[0];
            lstYear.Add(new c_report_year() { nItem = 111, nMale = nM_111, nFemale = 0 });
            #endregion

            #region 14.14 Resulting HC ROI : (a - (b-c)) / c
            decimal nM_112 = nM_111 > 0 ? CalDeimal((nM_109 - (nM_110 - nM_111)) / nM_111) : 0;
            lstYear.Add(new c_report_year() { nItem = 112, nMale = nM_112, nFemale = 0 });
            #endregion

            #region 14.15 Total FTEs
            decimal nM_113 = nM_28 + nF_28 - nM_98;
            //var n113 = GetValue_LastMonth(113);
            //decimal nM_113 = n113[0];
            lstYear.Add(new c_report_year() { nItem = 113, nMale = nM_113, nFemale = 0 });
            #endregion

            #endregion

            #region 15. Talent Attraction and Retention

            #region 15.1 Management by objectives: systematic use of agreed measurable targets by line superior
            var n115 = GetValue_LastMonth(115);
            decimal nM_115 = n115[0];
            lstYear.Add(new c_report_year() { nItem = 115, nMale = nM_115, nFemale = 0 });
            #endregion

            #region 15.2 Multidimensional performance appraisal
            var n116 = GetValue_LastMonth(116);
            decimal nM_116 = n116[0];
            lstYear.Add(new c_report_year() { nItem = 116, nMale = nM_116, nFemale = 0 });
            #endregion

            #region 15.3 Formal comparative ranking of employees within one employee category
            var n117 = GetValue_LastMonth(117);
            decimal nM_117 = n117[0];
            lstYear.Add(new c_report_year() { nItem = 117, nMale = nM_117, nFemale = 0 });
            #endregion

            #region 15.4 Employee engagement result
            var n118 = GetValue_LastMonth(118);
            decimal nM_118 = n118[0];
            lstYear.Add(new c_report_year() { nItem = 118, nMale = nM_118, nFemale = 0 });

            var n119 = GetValue_LastMonth(119);
            decimal nM_119 = n119[0];
            decimal nF_119 = n119[1];
            lstYear.Add(new c_report_year() { nItem = 119, nMale = nM_119, nFemale = nF_119 });
            #endregion

            #region 15.5 Employee engagement target
            var n120 = GetValue_LastMonth(120);
            decimal nM_120 = n120[0];
            lstYear.Add(new c_report_year() { nItem = 120, nMale = nM_120, nFemale = 0 });

            var n121 = GetValue_LastMonth(121);
            decimal nM_121 = n121[0];
            decimal nF_121 = n121[1];
            lstYear.Add(new c_report_year() { nItem = 121, nMale = nM_121, nFemale = nF_121 });
            #endregion

            #region 15.6 Coverage
            var n122 = GetValue_LastMonth(122);
            decimal nM_122 = n122[0];
            lstYear.Add(new c_report_year() { nItem = 122, nMale = nM_122, nFemale = 0 });

            var n123 = GetValue_LastMonth(123);
            decimal nM_123 = n123[0];
            decimal nF_123 = n123[1];
            lstYear.Add(new c_report_year() { nItem = 123, nMale = nM_123, nFemale = nF_123 });
            #endregion

            #endregion

            #region 16. Employee Receiving Regular Performance and Career Development Reviews (Excl. People in Unclassified Group)

            #region 16.1 Executive
            var n125 = GetValue_LastMonth(125);
            decimal nM_125 = n125[0];
            decimal nF_125 = n125[1];
            lstYear.Add(new c_report_year() { nItem = 125, nMale = nM_125, nFemale = nF_125 });

            decimal nM_126 = nM_39 > 0 ? CalDeimal((nM_125 / nM_39) * 100) : 0;
            decimal nF_126 = nF_39 > 0 ? CalDeimal((nF_125 / nF_39) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 126, nMale = nM_126, nFemale = nF_126 });
            #endregion

            #region 16.2 Middle management
            var n127 = GetValue_LastMonth(127);
            decimal nM_127 = n127[0];
            decimal nF_127 = n127[1];
            lstYear.Add(new c_report_year() { nItem = 127, nMale = nM_127, nFemale = nF_127 });

            decimal nM_128 = nM_41 > 0 ? CalDeimal((nM_127 / nM_41) * 100) : 0;
            decimal nF_128 = nF_41 > 0 ? CalDeimal((nF_127 / nF_41) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 128, nMale = nM_128, nFemale = nF_128 });
            #endregion

            #region 16.3 Senior
            var n129 = GetValue_LastMonth(129);
            decimal nM_129 = n129[0];
            decimal nF_129 = n129[1];
            lstYear.Add(new c_report_year() { nItem = 129, nMale = nM_129, nFemale = nF_129 });

            decimal nM_130 = nM_43 > 0 ? CalDeimal((nM_129 / nM_43) * 100) : 0;
            decimal nF_130 = nF_43 > 0 ? CalDeimal((nF_129 / nF_43) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 130, nMale = nM_130, nFemale = nF_130 });
            #endregion

            #region 16.4 Employee
            var n131 = GetValue_LastMonth(131);
            decimal nM_131 = n131[0];
            decimal nF_131 = n131[1];
            lstYear.Add(new c_report_year() { nItem = 131, nMale = nM_131, nFemale = nF_131 });

            decimal nM_132 = nM_45 > 0 ? CalDeimal((nM_131 / nM_45) * 100) : 0;
            decimal nF_132 = nF_45 > 0 ? CalDeimal((nF_131 / nF_45) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 132, nMale = nM_132, nFemale = nF_132 });
            #endregion

            #endregion

            #region 18. Board structure

            #region 18.2 Number of executive director
            var n149 = GetValue_LastMonth(149);
            decimal nM_149 = n149[0];
            decimal nF_149 = n149[0];
            lstYear.Add(new c_report_year() { nItem = 149, nMale = nM_149, nFemale = nF_149 });
            #endregion

            #region 18.3 Number of non-executive directors (excl. independent directors)
            var n150 = GetValue_LastMonth(150);
            decimal nM_150 = n150[0];
            decimal nF_150 = n150[0];
            lstYear.Add(new c_report_year() { nItem = 150, nMale = nM_150, nFemale = nF_150 });
            #endregion

            #region 18.4 Number of independent directors
            var n151 = GetValue_LastMonth(151);
            decimal nM_151 = n151[0];
            decimal nF_151 = n151[0];
            lstYear.Add(new c_report_year() { nItem = 151, nMale = nM_151, nFemale = nF_151 });
            #endregion

            #region 18.1 Total number of board members
            decimal nM_148 = nM_149 + nM_150 + nM_151;
            decimal nF_148 = nF_149 + nF_150 + nF_151;
            lstYear.Add(new c_report_year() { nItem = 148, nMale = nM_148, nFemale = nF_148 });
            #endregion

            #endregion

            #region 17. Gender Diversity and Equal Remuneration

            #region 17.1 Women in workforce
            decimal nM_134 = nF_5;
            lstYear.Add(new c_report_year() { nItem = 134, nMale = nM_134, nFemale = 0 });

            decimal nM_135 = IsM4 ? CalDeimal((nM_134 / nM_4) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 135, nMale = nM_135, nFemale = 0 });
            #endregion

            #region 17.2 Women in management positions
            decimal nM_136 = nF_39 + nF_41;
            lstYear.Add(new c_report_year() { nItem = 136, nMale = nM_136, nFemale = 0 });

            decimal n3941 = nM_39 + nF_39 + nM_41 + nF_41;
            decimal nM_137 = n3941 > 0 ? CalDeimal((nM_136 / n3941) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 137, nMale = nM_137, nFemale = 0 });
            #endregion

            #region 17.3 Women in top management positions
            decimal nM_138 = nF_39;
            lstYear.Add(new c_report_year() { nItem = 138, nMale = nM_138, nFemale = 0 });

            decimal n39_ = nM_39 + nF_39;
            decimal nM_139 = n39_ > 0 ? CalDeimal((nM_138 / n39_) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 139, nMale = nM_139, nFemale = 0 });
            #endregion

            #region 17.4 Women in junior management positions
            decimal nM_140 = nF_41;
            lstYear.Add(new c_report_year() { nItem = 140, nMale = nM_140, nFemale = 0 });

            decimal n41_ = nM_41 + nF_41;
            decimal nM_141 = n41_ > 0 ? CalDeimal((nM_140 / n41_) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 141, nMale = nM_141, nFemale = 0 });
            #endregion

            #region 17.5 Women in management positions in revenue generating functions e.g. Sales, Marketing, Operation and BD that under BU
            var n142 = GetValue_LastMonth(142);
            decimal nM_142 = n142[0];
            lstYear.Add(new c_report_year() { nItem = 142, nMale = nM_142, nFemale = 0 });

            decimal nM_143 = CalDeimal((nM_142 / 18) * 100);
            lstYear.Add(new c_report_year() { nItem = 143, nMale = nM_143, nFemale = 0 });
            #endregion

            #region 17.6 Number of women on board of directors/supervisory board
            var n144 = GetValue_LastMonth(144);
            decimal nM_144 = n144[0];
            lstYear.Add(new c_report_year() { nItem = 144, nMale = nM_144, nFemale = 0 });

            decimal nM_145 = (nM_148 + nF_148) > 0 ? CalDeimal((nM_144 / (nM_148 + nF_148)) * 100) : 0;
            lstYear.Add(new c_report_year() { nItem = 145, nMale = nM_145, nFemale = 0 });
            #endregion

            #region 17.7 Ratio basic salary women/men
            var n146 = GetValue_LastMonth(146);
            decimal nM_146 = n146[0];
            decimal nF_146 = n146[1];
            lstYear.Add(new c_report_year() { nItem = 146, nMale = nM_146, nFemale = nF_146 });
            #endregion

            #endregion

            #region 19. Labor Practice Indicators

            #region 19.1 Employees represented by an independent trade union
            var n153 = GetValue_LastMonth(153);
            decimal nM_153 = n153[0];
            decimal nF_153 = n153[1];
            lstYear.Add(new c_report_year() { nItem = 153, nMale = nM_153, nFemale = nF_153 });
            #endregion

            #endregion

            #endregion

            #region Save T_Report_Year
            var qT_Report_Year = db.T_Report_Year.FirstOrDefault(w => w.nCompanyID == nCompanyID && w.nYear == nYear);
            if (qT_Report_Year == null)
            {
                qT_Report_Year = new T_Report_Year();
                qT_Report_Year.nCompanyID = nCompanyID;
                qT_Report_Year.nYear = nYear;
                db.T_Report_Year.Add(qT_Report_Year);
            }
            else
            {
                CommonFunction.ExecuteNonQuery("delete T_Report_Year_Item where nReportID = " + qT_Report_Year.nReportID);
            }
            qT_Report_Year.dUpdate = DateTime.Now;
            db.SaveChanges();
            int nReportID = qT_Report_Year.nReportID;
            #endregion

            #region Save T_Report_Year_Item

            #region Script Insert
            string sInsert = @"INSERT INTO T_Report_Year_Item
           ([nReportID]
           ,[nItem]
           ,[nMale]
           ,[nFemale])
            VALUES
           (" + nReportID + @"--<nReportID, int,>
           ,{0}--<nItem, int,>
           ,{1}--<nMale, decimal(18,2),>
           ,{2})--<nFemale, decimal(18,2),>)" + Environment.NewLine;
            #endregion

            StringBuilder sb = new StringBuilder();
            foreach (var item in lstYear)
            {
                sb.Append(string.Format(sInsert, item.nItem, item.nMale, item.nFemale));
            }
            CommonFunction.ExecuteNonQuery(sb + "");
            #endregion
        }
    }

    public class c_report_year
    {
        public int nItem { get; set; }
        public decimal nMale { get; set; }
        public decimal nFemale { get; set; }
    }
    #endregion

}