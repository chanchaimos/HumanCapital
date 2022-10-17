﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class T_Course
{
    public int nCourseID { get; set; }
    public int nCompanyID { get; set; }
    public string sName { get; set; }
    public string sDescription { get; set; }
    public Nullable<int> nCategoryID { get; set; }
    public bool IsActive { get; set; }
    public bool IsDel { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
    public int nUpdateBy { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class T_Course_Sub
{
    public int nCourseSubID { get; set; }
    public int nCourseID { get; set; }
    public int nCompanyID { get; set; }
    public string sName { get; set; }
    public string sDescription { get; set; }
    public Nullable<System.DateTime> dStart { get; set; }
    public Nullable<System.DateTime> dEnd { get; set; }
    public Nullable<System.TimeSpan> tStart { get; set; }
    public Nullable<System.TimeSpan> tEnd { get; set; }
    public Nullable<int> nTraining_method { get; set; }
    public Nullable<decimal> nPrice { get; set; }
    public Nullable<int> nAmount { get; set; }
    public bool IsActive { get; set; }
    public bool IsDel { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
    public int nUpdateBy { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class T_Course_Sub_TG
{
    public int nCourseSubID { get; set; }
    public int nTargetGroup { get; set; }
}

public partial class T_Project
{
    public int nProjectID { get; set; }
    public Nullable<int> nCompanyID { get; set; }
    public string sProjectName { get; set; }
    public string sOrganization { get; set; }
    public string sObjective { get; set; }
    public Nullable<int> nProductivity { get; set; }
    public Nullable<decimal> nReturn_Economy { get; set; }
    public Nullable<decimal> nReturn_Environment { get; set; }
    public Nullable<decimal> nReturn_Social { get; set; }
    public Nullable<decimal> nReturn_Other { get; set; }
    public Nullable<decimal> nPrice_Opex { get; set; }
    public Nullable<decimal> nPrice_Capex { get; set; }
    public string sDescription { get; set; }
    public Nullable<System.DateTime> dStart { get; set; }
    public Nullable<System.DateTime> dEnd { get; set; }
    public Nullable<int> nCourse { get; set; }
    public bool IsActive { get; set; }
    public bool IsDel { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
    public int nUpdateBy { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class T_Project_Course
{
    public int nProjectID { get; set; }
    public int nCourseSubID { get; set; }
    public int nBenefit { get; set; }
}

public partial class T_Report_File
{
    public int nReportID { get; set; }
    public int nItem { get; set; }
    public string sFilename { get; set; }
    public string sSysFileName { get; set; }
    public string sPath { get; set; }
    public string sDescription { get; set; }
}

public partial class T_Report_Year
{
    public int nReportID { get; set; }
    public int nCompanyID { get; set; }
    public int nYear { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class T_Report_Year_Item
{
    public int nReportID { get; set; }
    public int nItem { get; set; }
    public Nullable<decimal> nMale { get; set; }
    public Nullable<decimal> nFemale { get; set; }
}

public partial class T_ReportDJSI
{
    public int nReportID { get; set; }
    public int nCompanyID { get; set; }
    public int nQuarter { get; set; }
    public int nYear { get; set; }
    public string sFilename { get; set; }
    public string sSysFileName { get; set; }
    public string sPath { get; set; }
    public int nStatusID { get; set; }
    public Nullable<int> nL1 { get; set; }
    public Nullable<int> nL2 { get; set; }
    public bool IsDel { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
    public int nUpdateBy { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class T_ReportDJSI_Approve
{
    public int nID { get; set; }
    public int nReportID { get; set; }
    public Nullable<int> nStatusID { get; set; }
    public string sComment { get; set; }
    public Nullable<int> nActionBy { get; set; }
    public Nullable<System.DateTime> dAction { get; set; }
}

public partial class T_ReportDJSI_Item
{
    public int nReportID { get; set; }
    public int nItem { get; set; }
    public Nullable<decimal> nMale_1 { get; set; }
    public Nullable<decimal> nMale_2 { get; set; }
    public Nullable<decimal> nMale_3 { get; set; }
    public Nullable<decimal> nFemale_1 { get; set; }
    public Nullable<decimal> nFemale_2 { get; set; }
    public Nullable<decimal> nFemale_3 { get; set; }
    public Nullable<decimal> nMale_Total { get; set; }
    public Nullable<decimal> nFemale_Total { get; set; }
}

public partial class TB_Company
{
    public int nCompanyID { get; set; }
    public int nCompanyType { get; set; }
    public string sCompanyCode { get; set; }
    public string sCompanyName { get; set; }
    public string sCompanyAbbr { get; set; }
    public bool IsActive { get; set; }
    public Nullable<bool> IsSync { get; set; }
    public bool IsDel { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
    public int nUpdateBy { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class TB_Company_DJSI
{
    public int nCompanyID { get; set; }
    public int nItem { get; set; }
}

public partial class TB_Sync
{
    public int nID { get; set; }
    public string sSysFileName { get; set; }
    public string sPath { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
}

public partial class TB_Sync_Item
{
    public int nID { get; set; }
    public int nCompanyID { get; set; }
    public System.DateTime dDate { get; set; }
    public int nItem { get; set; }
    public Nullable<decimal> nMale { get; set; }
    public Nullable<decimal> nFemale { get; set; }
}

public partial class TB_User
{
    public int nUserID { get; set; }
    public string sUserID { get; set; }
    public string sPassword { get; set; }
    public string sPasswordEncrypt { get; set; }
    public string sFirstname { get; set; }
    public string sLastname { get; set; }
    public string sEmail { get; set; }
    public int nCompanyType { get; set; }
    public int nRole { get; set; }
    public bool IsActive { get; set; }
    public bool IsDel { get; set; }
    public int nCreateBy { get; set; }
    public System.DateTime dCreate { get; set; }
    public int nUpdateBy { get; set; }
    public System.DateTime dUpdate { get; set; }
}

public partial class TB_User_Company
{
    public int nUserID { get; set; }
    public int nCompanyID { get; set; }
}

public partial class TB_User_Permission
{
    public int nUserID { get; set; }
    public int nMenuID { get; set; }
    public Nullable<int> nPermission { get; set; }
}

public partial class TM_DJSI
{
    public int nItem { get; set; }
    public string sName { get; set; }
    public Nullable<int> nItemHead { get; set; }
    public Nullable<int> nSibling { get; set; }
    public Nullable<bool> IsTotal { get; set; }
    public Nullable<int> nUnit { get; set; }
    public Nullable<bool> IsHead { get; set; }
    public Nullable<bool> IsAutoCal { get; set; }
}

public partial class TM_Log
{
    public long nLogID { get; set; }
    public Nullable<System.DateTime> dLog { get; set; }
    public Nullable<int> nUserID { get; set; }
    public Nullable<int> nMenuID { get; set; }
    public string sMenuName { get; set; }
    public string sEvent { get; set; }
}

public partial class TM_Log_SyncDJSI
{
    public long nLogID { get; set; }
    public System.DateTime dLog { get; set; }
    public string sEvent { get; set; }
}

public partial class TM_LogMail
{
    public int nID { get; set; }
    public string sTo { get; set; }
    public string sCc { get; set; }
    public string sSubject { get; set; }
    public string sMessage { get; set; }
    public Nullable<bool> IsSend { get; set; }
    public string sMessage_Error { get; set; }
    public Nullable<System.DateTime> dSend { get; set; }
}

public partial class TM_MasterData
{
    public int nMainID { get; set; }
    public string sName { get; set; }
    public string sDescription { get; set; }
    public bool IsManage { get; set; }
}

public partial class TM_MasterData_Sub
{
    public int nSubID { get; set; }
    public int nMainID { get; set; }
    public string sName { get; set; }
    public string sDescription { get; set; }
    public Nullable<decimal> nOrder { get; set; }
    public bool IsActive { get; set; }
    public bool IsDel { get; set; }
    public Nullable<int> nCreateBy { get; set; }
    public Nullable<System.DateTime> dCreate { get; set; }
    public Nullable<int> nUpdateBy { get; set; }
    public Nullable<System.DateTime> dUpdate { get; set; }
}

public partial class TM_Menu
{
    public int nMenuID { get; set; }
    public string sMenuName { get; set; }
    public Nullable<int> nMenuHead { get; set; }
    public Nullable<int> nLevel { get; set; }
    public string sMenuLink { get; set; }
    public string sIcon { get; set; }
    public Nullable<int> nMenuOrder { get; set; }
    public bool IsActive { get; set; }
}

public partial class TM_WFStatus
{
    public int nStatusID { get; set; }
    public string sStatusName { get; set; }
    public string sStatusNameAbbr { get; set; }
    public string sDescription { get; set; }
    public int nLevel { get; set; }
    public string sColor { get; set; }
}
