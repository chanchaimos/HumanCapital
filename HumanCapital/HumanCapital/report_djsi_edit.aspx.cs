using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassExecute;
using ClosedXML.Excel;
using System.Web.Hosting;
using System.Web.Script.Serialization;

public partial class report_djsi_edit : System.Web.UI.Page
{
    private static int nMenuID = 2;

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

                string str = Request.QueryString["str"];
                string sReportID = hddReportID.Value = (!string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "");

                SetControl();

                string sPageType = "Add";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (sReportID != "")
                {
                    SetData(sReportID);
                    sPageType = sPer == "A" ? "Edit" : "Delete";
                }
                else if (UserAccount.SessionInfo.nRole != 2)//L0
                {
                    hddPermission.Value = "N";
                }
                else if (UserAccount.SessionInfo.nRole == 2)
                {
                    hddIsL0.Value = "1";
                }
                hddUserID.Value = UserAccount.SessionInfo.nUserID + "";
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
                SystemFunction.BindDdlPageSize(ddlPageSize);
                SystemFunction.BindDdlPageSize(ddlPageSize_log);
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
        var lstYear = Human_Function.Get_ddl_Year();
        ddlYear.DataSource = lstYear;
        ddlYear.DataValueField = "Value";
        ddlYear.DataTextField = "Text";
        ddlYear.DataBind();
        ddlYear.Items.Insert(0, new ListItem("- Year -", ""));
        #endregion
    }

    public void SetData(string sReportID)
    {
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            var q = db.T_ReportDJSI.FirstOrDefault(w => w.nReportID + "" == sReportID);
            if (q != null)
            {
                ddlCompany.SelectedValue = q.nCompanyID + "";

                //Set hdd Company To Save
                hddIsCompanyInternal.Value = CompanyChange(q.nCompanyID).Content == "6" ? "0" : "1";
                //End

                ddlYear.SelectedValue = q.nYear + "";
                ddlQuarter.SelectedValue = q.nQuarter + "";
                hddStatusID.Value = q.nStatusID + "";
                ddlCompany.Enabled = ddlYear.Enabled = ddlQuarter.Enabled = false;


                #region Check Prms
                int nUserID = UserAccount.SessionInfo.nUserID;
                int nRole = UserAccount.SessionInfo.nRole;
                if (!UserAccount.IsExpired)
                {
                    var lstUserL0 = Human_Function.GetUserInRole(nRole, q.nCompanyID);
                    switch (nRole)
                    {
                        case 2://L0 Reporter
                            hddIsL0.Value = lstUserL0.Any(a => a == nUserID) ? "1" : "";
                            break;
                        case 3://L1 Manager
                            hddIsL1.Value = nUserID == q.nL1 ? "1" : "";
                            break;
                        case 4://L2 SSHE Corporate
                            hddIsL2.Value = nUserID == q.nL2 ? "1" : "";
                            break;
                    }
                }
                #endregion
                //hddFile.Value = "{sPath: ../UploadFiles/2/Temp/,sSysFileName :Template_2701202011255914.xlsx,sFileName:Template.xlsx}";
                var obj = new sysGlobalClass.FileUploadMaster
                {
                    sPath = q.sPath,
                    sSysFileName = q.sSysFileName,
                    sFileName = q.sFilename,
                };
                var json = new JavaScriptSerializer().Serialize(obj);
                hddFile.Value = json;
            }
        }
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturnLoadData LoadData(int nReportID, int nComID, int nYear, int nQuarter, bool IsAuto, List<c_ReportDJSI_Item> lstDataDJSI)
    {
        TReturnLoadData result = new TReturnLoadData();
        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            #region Func Check Dup
            Func<int, int, int, int, bool> CheckDataDUP = (nID, nCom, nQ, nY) =>
            {
                bool IsDup = false;
                var lst = db.T_ReportDJSI.Where(w => (nID != 0 ? w.nReportID != nID : true) && w.nCompanyID == nCom && w.nQuarter == nQ && w.nYear == nY).ToList();
                IsDup = lst.Any();
                return IsDup;
            };
            #endregion

            if (CheckDataDUP(nReportID, nComID, nQuarter, nYear))
            {
                result.Status = SystemFunction.process_Duplicate;
            }
            else
            {
                var nRole = UserAccount.SessionInfo.nRole;

                var lstDJSI_Master = db.TM_DJSI.ToList();
                var lstUnit = db.TM_MasterData_Sub.Where(w => w.nMainID == 5 && w.IsActive && !w.IsDel).ToList();
                var lstCom_DJSI = db.TB_Company_DJSI.Where(w => w.nCompanyID == nComID).ToList();
                var lstCom_DJSI_nItem = lstCom_DJSI.Select(s => s.nItem).ToList();

                var qReportDJSI = db.T_ReportDJSI.FirstOrDefault(w => w.nCompanyID == nComID && w.nYear == nYear && w.nQuarter == nQuarter);
                var lstReportItem = db.T_ReportDJSI_Item.Where(w => w.nReportID == nReportID).ToList();
                var lstDJSI = new List<c_ReportDJSI_Item>();

                #region lst Data DJSI
                if (IsAuto)
                {
                    if (nReportID != 0)
                    {
                        if (qReportDJSI != null)
                        {
                            lstDJSI = db.T_ReportDJSI_Item.Where(w => w.nReportID == qReportDJSI.nReportID).ToList().Select(s => new c_ReportDJSI_Item
                            {
                                nReportID = s.nReportID,
                                nItem = s.nItem,
                                nMale_1 = s.nMale_1,
                                nMale_2 = s.nMale_2,
                                nMale_3 = s.nMale_3,
                                nFemale_1 = s.nFemale_1,
                                nFemale_2 = s.nFemale_2,
                                nFemale_3 = s.nFemale_3,
                            }).ToList();
                        }
                    }
                    else
                    {
                        //Out to list no Data
                    }
                }
                else
                {
                    bool IsInternal = CompanyChange(nComID).Content != 6 + "";
                    if (IsInternal)
                    {
                        lstDJSI = new List<c_ReportDJSI_Item>();
                        var lstDateInQuarter = GetMonthFromQuarter(nQuarter, nYear);
                        var lst_Month = lstDateInQuarter.Select(s => s.Month).OrderBy(o => o).ToList();
                        var lstReportSync = db.TB_Sync_Item.AsEnumerable().Where(w => w.nCompanyID == nComID && lstCom_DJSI_nItem.Contains(w.nItem) && lstDateInQuarter.Contains(w.dDate.Date)).ToList();
                        var lstReportSyncGroupItem = lstReportSync.GroupBy(o => o.nItem).Select(s => s.Key).ToList();

                        lstReportSyncGroupItem.ForEach(f =>
                        {
                            #region Set Value
                            var qData = lstReportSync.Where(w => w.nItem == f).ToList();
                            decimal? nM_1 = qData.Any(a => a.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[0].ToString())) ? qData.First(w => w.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[0].ToString())).nMale : null;
                            decimal? nM_2 = qData.Any(a => a.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[1].ToString())) ? qData.First(w => w.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[1].ToString())).nMale : null;
                            decimal? nM_3 = qData.Any(a => a.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[2].ToString())) ? qData.First(w => w.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[2].ToString())).nMale : null;
                            decimal? nF_1 = qData.Any(a => a.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[0].ToString())) ? qData.First(w => w.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[0].ToString())).nFemale : null;
                            decimal? nF_2 = qData.Any(a => a.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[1].ToString())) ? qData.First(w => w.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[1].ToString())).nFemale : null;
                            decimal? nF_3 = qData.Any(a => a.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[2].ToString())) ? qData.First(w => w.dDate.Month == CommonFunction.GetIntNullToZero(lst_Month[2].ToString())).nFemale : null;

                            #endregion

                            lstDJSI.Add(new c_ReportDJSI_Item()
                            {
                                nReportID = 0,
                                nItem = f,
                                nMale_1 = nM_1,
                                nMale_2 = nM_2,
                                nMale_3 = nM_3,
                                nFemale_1 = nF_1,
                                nFemale_2 = nF_2,
                                nFemale_3 = nF_3,
                            });
                        });
                    }
                    else lstDJSI = lstDataDJSI;
                }
                #endregion

                var lstData = (from a in lstDJSI_Master
                               from b in lstUnit.Where(w => w.nSubID == a.nUnit).DefaultIfEmpty()
                               from c in lstDJSI.Where(w => w.nItem == a.nItem).DefaultIfEmpty()
                               select new c_djsi
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

                                   nMale_1 = c != null ? c.nMale_1 : null,
                                   nMale_2 = c != null ? c.nMale_2 : null,
                                   nMale_3 = c != null ? c.nMale_3 : null,
                                   nFemale_1 = c != null ? c.nFemale_1 : null,
                                   nFemale_2 = c != null ? c.nFemale_2 : null,
                                   nFemale_3 = c != null ? c.nFemale_3 : null,
                               }).ToList();


                result.lstDJSI = lstData;
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(c_DataSave item)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var IsNew = item.nReportID == 0;
            int? nL1 = null, nL2 = null;
            int nRole = 0, nUserID = UserAccount.SessionInfo.nUserID;

            #region GetNext Step and Check Step Approver
            var qStepApprover = CheckStepApprover(item.nReportID, item.IsL0, item.IsL1, item.IsL2, item.nType, item.nCurrentStatusID);
            item.nStatusID = qStepApprover.nStatusID;
            #endregion

            #region CheckDataDUP
            Func<int, int, int, int, bool> CheckDUP = (nID, nCom, nY, nQ) =>
            {
                bool IsDup = false;
                var lst = db.T_ReportDJSI.Where(w => (nID != 0 ? w.nReportID != nID : true)
                && w.nCompanyID == nCom
                && w.nYear == nY
                && w.nQuarter == nQ).ToList();
                IsDup = lst.Any();
                return IsDup;
            };
            #endregion

            #region Get Approver
            var lstStatusID_App = new List<int>() { 1, 2 };
            bool IsNoHaveApprover = false;
            if (lstStatusID_App.Contains(item.nStatusID))
            {
                nRole = item.nStatusID == 1 ? 3 : 4;
                if (nRole == 3) nL1 = Human_Function.GetApprover(nRole, item.nCompanyID);
                else nL2 = Human_Function.GetApprover(nRole, item.nCompanyID);
                if (!string.IsNullOrEmpty(nL1 + "") || !string.IsNullOrEmpty(nL2 + "")) IsNoHaveApprover = false; else IsNoHaveApprover = true;
            }
            #endregion

            if (CheckDUP(item.nReportID, item.nCompanyID, item.nYear, item.nQuarter))
            {
                result.Status = SystemFunction.process_Duplicate;
            }
            else if (IsNoHaveApprover)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Please contact the system administrator (since there is no approval yet. In this department)";
            }
            else if (!qStepApprover.Issuccess)//(item.nStatusID == 0 ? false : !qStepApprover.Issuccess)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "The data has been recorded.";
            }
            else
            {
                try
                {
                    var q = db.T_ReportDJSI.FirstOrDefault(w => w.nReportID == item.nReportID);
                    if (q == null)
                    {
                        q = new T_ReportDJSI();
                        q.nReportID = db.T_ReportDJSI.Any() ? db.T_ReportDJSI.Max(m => m.nReportID) + 1 : 1;
                        q.nCompanyID = item.nCompanyID;
                        q.nQuarter = item.nQuarter;
                        q.nYear = item.nYear;
                        q.IsDel = false;
                        q.nCreateBy = nUserID;
                        q.dCreate = DateTime.Now;
                        db.T_ReportDJSI.Add(q);
                    }
                    q.nStatusID = item.nStatusID;

                    if (nRole == 3) q.nL1 = nL1;
                    else if (nRole == 4) q.nL2 = nL2;

                    q.nUpdateBy = nUserID;
                    q.dUpdate = DateTime.Now;

                    #region Item DJSI

                    StringBuilder SQL = new StringBuilder();

                    #region SQL
                    string _SQL_INSERT = @" INSERT INTO T_ReportDJSI_Item
                                            (nReportID
                                            ,nItem
                                            ,nMale_1
                                            ,nMale_2
                                            ,nMale_3
                                            ,nFemale_1
                                            ,nFemale_2
                                            ,nFemale_3)
                                       VALUES
                                            ({0}--<nReportID, int,>
                                            ,{1}--<nItem, int,>
                                            ,{2}--<nMale_1, decimal(18,2),>
                                            ,{3}--<nMale_2, decimal(18,2),>
                                            ,{4}--<nMale_3, decimal(18,2),>
                                            ,{5}--<nFemale_1, decimal(18,2),>
                                            ,{6}--<nFemale_2, decimal(18,2),>
                                            ,{7})--<nFemale_3, decimal(18,2),> " + Environment.NewLine;
                    //,{ 8}--<nMale_Total, decimal(18,2),>
                    //,{9})--<nFemale_Total, decimal(18,2),> " 

                    string _SQL_Delete = @" DELETE FROM T_ReportDJSI_Item WHERE nReportID = " + q.nReportID + Environment.NewLine;
                    CommonFunction.ExecuteNonQuery(_SQL_Delete);
                    #endregion
                    if (item.lstDJSI.Any())
                    {
                        item.lstDJSI.ForEach(f =>
                        {
                            SQL.Append(string.Format(_SQL_INSERT, q.nReportID, f.nItem,
                                (f.nMale_1.HasValue ? f.nMale_1 + "" : "NULL"),
                                (f.nMale_2.HasValue ? f.nMale_2 + "" : "NULL"),
                                (f.nMale_3.HasValue ? f.nMale_3 + "" : "NULL"),
                                (f.nFemale_1.HasValue ? f.nFemale_1 + "" : "NULL"),
                                (f.nFemale_2.HasValue ? f.nFemale_2 + "" : "NULL"),
                                (f.nFemale_3.HasValue ? f.nFemale_3 + "" : "NULL")));
                        });
                        CommonFunction.ExecuteNonQuery(SQL.ToString());
                    }
                    #endregion

                    Human_Function.UpdateReport_ApproveLOG(q.nReportID, q.nStatusID, item.sComment);

                    string sUploadPath = "UploadFiles/" + nUserID + "/Report/" + q.nReportID + "/";

                    #region File DJSI
                    if (IsNew && !string.IsNullOrEmpty(item.objFile.sPath))
                    {
                        #region Move File

                        string sMapPath = HttpContext.Current.Server.MapPath("./");

                        q.sFilename = item.objFile.sFileName;
                        q.sSysFileName = item.objFile.sSysFileName;
                        q.sPath = sUploadPath;
                        SystemFunction.CheckPathAndMoveFile(item.objFile.sSysFileName, item.objFile.sFileName, sUploadPath, (item.objFile.sPath).Replace("../", ""));
                        #endregion
                    }
                    #endregion

                    db.SaveChanges();

                    #region Set DJSI IN Year
                    if (q.nStatusID == 3) Human_Function.SetDJSIInYear(q.nCompanyID, q.nYear);
                    #endregion

                    #region File Document
                    sUploadPath = "UploadFiles/" + nUserID + "/Report/" + q.nReportID + "/Document/";
                    string sUploadPath_TEMP = "UploadFiles/" + UserAccount.SessionInfo.nUserID + "/Temp/";
                    SaveFile_DJSI(q.nReportID, sUploadPath, sUploadPath_TEMP, item.lstFile);
                    #endregion

                    #region Send mail

                    #region Variable declaration
                    string _to = "", _from = "", _cc = "", subject = "", message = "", sStatusName = "", sText = "", sFoot = "", BtnUrl = "", sStep = "";
                    var qTo = new TB_User();
                    string Applicationpath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath != "/" ? HttpContext.Current.Request.ApplicationPath + "/" : HttpContext.Current.Request.ApplicationPath);
                    string sURL = Applicationpath + "AD/index.aspx?link=";
                    sURL += CommonFunction.Encrypt_UrlEncrypt("report_djsi_edit.aspx?str=" + CommonFunction.Encrypt_UrlEncrypt(q.nReportID + ""));

                    #region Btn Link
                    if (!string.IsNullOrEmpty(sURL))
                    {
                        BtnUrl = @" <tr>
                                                    <td style='word-break: break-word; font-size: 0px;' align='center'>
                                                        <table role='presentation' cellpadding='0' cellspacing='0' style='border-collapse: separate' align='center' border='0'>
                                                          <tbody>
                                                              <tr>
                                                                  <td style='border: none; border-radius: 3px; color: white; padding: 15px 19px' align='center' valign='middle' bgcolor='#7289DA'><a href='" + sURL + @"' style='text-decoration: none; line-height: 100%; background: #7289da; color: white; font-family: Ubuntu,Helvetica,Arial,sans-serif; font-size: 15px; font-weight: normal; text-transform: none; margin: 0px' target='_blank'>Link</a></td>
                                                               </tr>
                                                           </tbody>
                                                        </table>
                                                    </td>
                                                </tr> ";
                    }
                    #endregion

                    var lstUser = db.TB_User.ToList();
                    var lstTM_WFStatus = db.TM_WFStatus.ToList();
                    sStatusName = lstTM_WFStatus.Any(a => a.nStatusID == q.nStatusID) ? lstTM_WFStatus.FirstOrDefault(a => a.nStatusID == q.nStatusID).sDescription : "";
                    #endregion

                    if (q.nStatusID != 0) // Draft
                    {
                        switch (q.nStatusID)
                        {
                            case 1://Waiting L1 Approve
                                sStep = "submitted";
                                _from = Human_Function.GetUserInfo(nUserID != q.nCreateBy ? nUserID : q.nCreateBy, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nL1 ?? 0, lstUser);
                                _to = qTo.sEmail;
                                break;
                            case 4://L0 Recall from Waiting L1 Approve
                                sStep = "recalled";
                                _from = Human_Function.GetUserInfo(nUserID != q.nCreateBy ? nUserID : q.nCreateBy, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nL1 ?? 0, lstUser);
                                _to = qTo.sEmail;

                                break;
                            case 2://Waiting L2 Approve
                                sStep = "approved";
                                _from = Human_Function.GetUserInfo(q.nL1 ?? 0, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nL2 ?? 0, lstUser);
                                _to = qTo.sEmail;

                                break;
                            case 3://Completed
                                sStep = "approved";
                                _from = Human_Function.GetUserInfo(q.nL2 ?? 0, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nCreateBy, lstUser);
                                _to = qTo.sEmail;

                                break;
                            case 5://L0 Request Edit from Waiting L2 Approve
                                sStep = "requested edit";
                                _from = Human_Function.GetUserInfo(nUserID != q.nCreateBy ? nUserID : q.nCreateBy, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nL2 ?? 0, lstUser);
                                _to = qTo.sEmail;

                                break;
                            case 6://L0 Request Edit from Completed
                                break;
                            case 7://L1 Reject
                                sStep = "rejected";
                                _from = Human_Function.GetUserInfo(q.nL1 ?? 0, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nCreateBy, lstUser);
                                _to = qTo.sEmail;

                                break;
                            case 8://L2 Reject
                                sStep = "rejected";
                                _from = Human_Function.GetUserInfo(q.nL2 ?? 0, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nCreateBy, lstUser);
                                _to = qTo.sEmail;

                                break;

                            default:
                                _from = Human_Function.GetUserInfo(q.nL1 ?? 0, lstUser).sEmail;
                                qTo = Human_Function.GetUserInfo(q.nCreateBy, lstUser);
                                _to = qTo.sEmail;
                                break;
                        }
                        subject = "[PTTGC Human] Update status report DJSI : " + sStatusName;
                        sText += "<p>Report DJSI has been " + sStep + ".</p>";
                        sText += "<p>Company : " + Human_Function.GET_Companyname(q.nCompanyID) + "</p>";
                        sText += "<p>Quarter : " + q.nQuarter + "</p>";
                        sText += "<p>Year : " + q.nYear + "</p>";

                        message = string.Format(Human_Sendmail.GET_TemplateEmail(),
                        "Dear K." + qTo.sFirstname + ' ' + qTo.sLastname,
                        sText,
                        BtnUrl,
                        sFoot,
                        "");
                        Human_Sendmail.SendNetMail(_from, _to, _cc, subject, message, new List<string>());
                    }
                    #endregion

                    result.Status = SystemFunction.process_Success;
                }
                catch (Exception e)
                {
                    result.Status = SystemFunction.process_SaveFail;
                    result.Msg = e + "";
                }
            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod CompanyChange(int nComID)
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

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TReturntemplete CheckFile(string sPath, string sSysFileName, string sFileName)
    {
        TReturntemplete result = new TReturntemplete();
        result.lstData = new List<c_ReportDJSI_Item>();
        result.lstDataError = new List<c_TempleteError>();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            string sMapPath = HttpContext.Current.Server.MapPath(sPath.Replace("../", "") + sSysFileName);
            if (File.Exists(sMapPath))
            {
                var a = CheckTempleteFile(sPath, sSysFileName, sFileName);
                if (a.Issucess)
                {
                    result.lstData = a.lstData;
                }
                else result.Status = SystemFunction.process_Failed; result.Msg = a.Msg; result.lstDataError = a.lstDataError;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "File not found";
            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnGetData GetData(int nReportID)
    {
        c_ReturnGetData TReturn = new c_ReturnGetData();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            var qDJSI = db.T_ReportDJSI.FirstOrDefault(w => w.nReportID == nReportID);
            if (qDJSI != null)
            {
                #region Log
                var lstLogApp = db.T_ReportDJSI_Approve.Where(w => w.nReportID == nReportID).OrderByDescending(o => o.dAction).ToList();
                var lstUserID = lstLogApp.Select(s => s.nActionBy).Distinct().ToList();
                var lstUser = db.TB_User.Where(w => lstUserID.Contains(w.nUserID)).ToList();
                var lstStatus = db.TM_WFStatus.ToList();

                var lstLog = (from a in lstLogApp
                              from b in lstStatus.Where(w => w.nStatusID == a.nStatusID)
                              from c in lstUser.Where(w => w.nUserID == a.nActionBy).DefaultIfEmpty()
                              select new c_Log
                              {
                                  sAction = b.sStatusName,
                                  sActionBy = c != null ? (c.sFirstname + "  " + c.sLastname) : (a.nActionBy == 0 ? "System" : ""),
                                  sActionDate = a.dAction.Value.ToString("dd/MM/yyyy <br> HH:mm"),
                                  sComment = a.sComment
                              }).ToList();

                TReturn.lstLog = lstLog;
                #endregion

                #region File
                List<sysGlobalClass.FileUploadMaster> lstFile = new List<sysGlobalClass.FileUploadMaster>();

                var q = db.T_Report_File.FirstOrDefault(w => w.nReportID == nReportID);
                if (q != null)
                {
                    int nOrder = 1;

                    lstFile = db.T_Report_File.Where(w => w.nReportID == nReportID).ToList().Select(s => new sysGlobalClass.FileUploadMaster { nID = s.nItem, sDescription = s.sDescription, sSysFileName = s.sSysFileName, sPath = s.sPath, sFileName = s.sFilename }).ToList();
                }

                TReturn.lstFile = lstFile;
                #endregion
            }
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }
    #endregion

    #region Check Templete File
    private static TReturntemplete CheckTempleteFile(string sPath, string sSysFileName, string sFileName)
    {
        TReturntemplete result = new TReturntemplete();
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        result.lstData = new List<c_ReportDJSI_Item>();
        result.lstDataError = new List<c_TempleteError>();
        var sPathFile = (HostingEnvironment.MapPath(sPath.Replace("..", "~")) + sSysFileName);

        if (File.Exists(sPathFile))
        {
            #region Msg Error
            string sValueMax100 = "Values greater than 0 and not more than 100";
            string sValueRatio = "Values No more than 1 result";
            string sValueIncorrect_Format = "Can only enter numbers";
            string sSpecify = "Specify.";
            #endregion

            int nRow = 1;
            var workbook = new XLWorkbook(sPathFile);
            var sColorError = XLColor.Yellow;
            IXLWorksheet SheetItem = workbook.Worksheet("Human Capital");

            foreach (IXLRow row in SheetItem.Rows())
            {
                var lstRatio = new List<int?>() { 23 };
                var lstMax100 = db.TM_MasterData_Sub.Where(w => w.nMainID == 5 && w.nOrder == 0 && !lstRatio.Contains(w.nSubID)).ToList().Select(s => s.nSubID).ToList();
                var lstItemToAdd = db.Database.SqlQuery<c_TM_DJSI>(@"SELECT nItem+6 as nRowExcel,* FROM TM_DJSI where IsAutoCal=0 and IsHead =0").ToList();
                var q = lstItemToAdd.FirstOrDefault(w => w.nRowExcel == nRow);
                if (q != null)
                {
                    #region Define Variable
                    int nCol = 3;
                    decimal? nMale_1 = null;
                    decimal? nMale_2 = null;
                    decimal? nMale_3 = null;
                    decimal? nFemale_1 = null;
                    decimal? nFemale_2 = null;
                    decimal? nFemale_3 = null;

                    bool IsRatio = lstRatio.Contains(q.nUnit);
                    bool IsValMax100 = false;//lstMax100.Contains(q.nUnit ?? 0);
                    #endregion

                    #region Check Data Type

                    Func<int, string> GetValue = (nCol_) =>
                    {
                        try
                        {
                            return row.Cell(nCol).Value + "";
                        }
                        catch (Exception ex)
                        {
                            return row.Cell(nCol).ValueCached + "";
                        }
                    };

                    c_TempleteError er = new c_TempleteError();
                    er.nRowExcel = q.nRowExcel;
                    er.lstMgError = new List<string>();

                    if (q.IsTotal == false || q.IsTotal == null)
                    {
                        #region Data 1-3 Month M&F

                        #region Col 3
                        if (!string.IsNullOrEmpty(GetValue(nCol) + ""))
                        {
                            if (IsValMax100)
                            {
                                nMale_1 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nMale_1 == null || nMale_1 > 100 || nMale_1 == 0) { er.lstMgError.Add("Male_Month 1 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nMale_1 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_1 == null || nMale_1 > 1) { er.lstMgError.Add("Male_Month 1 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nMale_1 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_1 == null) { er.lstMgError.Add("Male_Month 1 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Male_Month 1 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++;
                        #endregion

                        #region Col 4
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nFemale_1 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nFemale_1 == null || nFemale_1 > 100 || nFemale_1 == 0) { er.lstMgError.Add("Female_Month 1 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nFemale_1 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nFemale_1 == null || nFemale_1 > 1) { er.lstMgError.Add("Female_Month 1 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nFemale_1 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nFemale_1 == null) { er.lstMgError.Add("Female_Month 1 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Female_Month 1 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++;
                        #endregion

                        #region Col 5
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nMale_2 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nMale_2 == null || nMale_2 > 100 || nMale_2 == 0) { er.lstMgError.Add("Male_Month 2 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nMale_2 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_2 == null || nMale_2 > 1) { er.lstMgError.Add("Male_Month 2 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nMale_2 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_2 == null) { er.lstMgError.Add("Male_Month 2 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Male_Month 2 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++;
                        #endregion

                        #region Col 6
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nFemale_2 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nFemale_2 == null || nFemale_2 > 100 || nFemale_2 == 0) { er.lstMgError.Add("Female_Month 2 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nFemale_2 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nFemale_2 == null || nFemale_2 > 1) { er.lstMgError.Add("Female_Month 2 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nFemale_2 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nFemale_2 == null) { er.lstMgError.Add("Female_Month 2 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Female_Month 2 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++;
                        #endregion

                        #region Col 7
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nMale_3 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nMale_3 == null || nMale_3 > 100 || nMale_3 == 0) { er.lstMgError.Add("Male_Month 3 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nMale_3 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_3 == null || nMale_3 > 1) { er.lstMgError.Add("Male_Month 3 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nMale_3 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_3 == null) { er.lstMgError.Add("Male_Month 3 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Male_Month 3 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++;
                        #endregion

                        #region Col 8
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nFemale_3 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nFemale_3 == null || nFemale_3 > 100 || nFemale_3 == 0) { er.lstMgError.Add("Female_Month 3 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nFemale_3 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nFemale_3 == null || nFemale_3 > 1) { er.lstMgError.Add("Female_Month 3 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nFemale_3 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nFemale_3 == null) { er.lstMgError.Add("Female_Month 3 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Female_Month 3 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region Data Total

                        #region Col 3-4
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nMale_1 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nMale_1 == null || nMale_1 > 100 || nMale_1 == 0) { er.lstMgError.Add("Male_Month 1 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nMale_1 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_1 == null || nMale_1 > 1) { er.lstMgError.Add("Male_Month 1 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nMale_1 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_1 == null) { er.lstMgError.Add("Male_Month 1 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Male_Month 1 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++; nCol++;
                        #endregion

                        #region Col 5-6
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nMale_2 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nMale_2 == null || nMale_2 > 100 || nMale_2 == 0) { er.lstMgError.Add("Male_Month 2 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nMale_2 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_2 == null || nMale_2 > 1) { er.lstMgError.Add("Male_Month 2 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nMale_2 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_2 == null) { er.lstMgError.Add("Male_Month 2 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Male_Month 2 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        nCol++; nCol++;
                        #endregion

                        #region Col 7-8
                        if (!string.IsNullOrEmpty(GetValue(nCol)))
                        {
                            if (IsValMax100)
                            {
                                nMale_3 = CommonFunction.ParseIntNull(GetValue(nCol));
                                if (nMale_3 == null || nMale_3 > 100 || nMale_3 == 0) { er.lstMgError.Add("Male_Month 3 : " + sValueMax100); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else if (IsRatio)
                            {
                                nMale_3 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_3 == null || nMale_3 > 1) { er.lstMgError.Add("Male_Month 3 : " + sValueRatio); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                            else
                            {
                                nMale_3 = CommonFunction.ParseDecimalNull(GetValue(nCol));
                                if (nMale_3 == null) { er.lstMgError.Add("Male_Month 3 : " + sValueIncorrect_Format); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError; }
                            }
                        }
                        else
                        {
                            er.lstMgError.Add("Male_Month 3 : " + sSpecify); row.Cell(nCol).Style.Fill.BackgroundColor = sColorError;
                        }
                        #endregion

                        #endregion
                    }
                    if (er.lstMgError.Any())
                    {
                        result.lstDataError.Add(er);
                    }
                    #endregion

                    c_ReportDJSI_Item a = new c_ReportDJSI_Item();
                    a.nItem = q.nItem;
                    a.nMale_1 = nMale_1;
                    a.nMale_2 = nMale_2;
                    a.nMale_3 = nMale_3;
                    a.nFemale_1 = nFemale_1;
                    a.nFemale_2 = nFemale_2;
                    a.nFemale_3 = nFemale_3;
                    result.lstData.Add(a);
                }
                nRow++;
            }
            result.Issucess = result.lstDataError.Any(a => a.lstMgError.Any()) ? false : true; result.Msg = result.Issucess ? "" : "Please check the file again!";
            if (!result.Issucess)
            {
                workbook.SaveAs(HostingEnvironment.MapPath(sPath.Replace("..", "~")) + "Error_" + sSysFileName);
            }
        }
        else
        {
            result.Issucess = false;
            result.Msg = "File not found";
        }
        return result;
    }
    #endregion

    #region Check PRMS Approver
    private static TReturnStepApprover CheckStepApprover(int nReportID, bool IsL0, bool IsL1, bool IsL2, int nType, int nCurrentStatusID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        TReturnStepApprover r = new TReturnStepApprover();
        r.Issuccess = false;
        r.nStatusID = 0;

        #region TM_WFStatus  
        int nNextStatusID = 0;
        //    0 - Save Draft          
        //    1 - Waiting L1 Approve
        //    2 - Waiting L2 Approve 
        //    3 - Completed   
        //    4 - L0 Recall from Waiting L1 Approve  
        //    5 - L0 Request Edit from Waiting L2 Approve   
        //    6 - L0 Request Edit from Completed    
        //    7 - L1 Reject  
        //    8 - L2 Reject  
        var lstStepNext_L0 = new List<int>() { 0, 1, 4, 5, 6 };
        var lstStepNext_L1 = new List<int>() { 2, 7 };
        var lstStepNext_L2 = new List<int>() { 3, 8 };

        #endregion

        #region GetNext StepApprove
        switch (nType)
        {
            case 1://$btnSubmit
                if (IsL0)
                {
                    nNextStatusID = 1;//Waiting L1 Approve
                }
                else if (IsL1)
                {
                    nNextStatusID = 2;//Waiting L2 Approve
                }
                else if (IsL2)
                {
                    nNextStatusID = 3;//Completed
                }
                break;
            case 2://$btnRecall
                if (IsL0)
                {
                    switch (nCurrentStatusID)
                    {
                        case 1: nNextStatusID = 4; break;//L0 Recall from Waiting L1 Approve
                    }
                }
                break;
            case 3://$btnRequestEdit
                if (IsL0)
                {
                    switch (nCurrentStatusID)
                    {
                        case 2: nNextStatusID = 5; break;//L0 Request Edit from Waiting L2 Approve
                        case 3: nNextStatusID = 6; break;//L0 Request Edit from Completed
                    }
                }
                break;
            case 4://$btnReject
                if (IsL1)
                {
                    nNextStatusID = 7;//L1 Reject
                }
                else if (IsL2)
                {
                    nNextStatusID = 8;//L2 Reject
                }
                break;
            default: nNextStatusID = 0; break; //Save Draft
        }
        #endregion

        #region Check Step 2 User Submit
        if (nReportID != 0)
        {
            var q = db.T_ReportDJSI.FirstOrDefault(w => w.nReportID == nReportID);
            if (q != null)
            {
                int nStatusIDInDB = q.nStatusID;
                if (nNextStatusID == nStatusIDInDB && nNextStatusID != 0 && nStatusIDInDB != 0)
                {
                    r.Issuccess = false;
                }
                else
                {
                    if (IsL0)
                    {
                        if (lstStepNext_L0.Contains(nNextStatusID))
                        {
                            bool IsDraft = nNextStatusID == 0 && nStatusIDInDB == 0;
                            if (IsDraft)
                            {
                                r.Issuccess = true;
                            }
                            else if (nNextStatusID != nStatusIDInDB)
                            {
                                r.Issuccess = true;
                            }
                        }
                    }
                    else if (IsL1)
                    {

                        if (lstStepNext_L1.Contains(nNextStatusID))
                        {
                            lstStepNext_L1 = lstStepNext_L1.Where(w => w != nStatusIDInDB).ToList();
                            if (!lstStepNext_L1.Contains(nNextStatusID) || lstStepNext_L0.Contains(nStatusIDInDB))
                            {
                                r.Issuccess = true;
                            }
                        }
                    }
                    else if (IsL2)
                    {
                        lstStepNext_L2 = lstStepNext_L2.Where(w => w != nStatusIDInDB).ToList();
                        if (!lstStepNext_L2.Contains(nNextStatusID) || lstStepNext_L1.Contains(nStatusIDInDB))
                        {
                            r.Issuccess = true;
                        }
                    }
                }
            }
        }
        else
        {
            r.Issuccess = true;
        }
        #endregion
        r.nStatusID = nNextStatusID;
        return r;
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

    private void CheckPRMS_Step(int nRole, int nCompany)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        int nUserID = UserAccount.SessionInfo.nUserID;
        if (!UserAccount.IsExpired)
        {
            var a = Human_Function.GetApprover(nRole, nCompany);
            switch (nRole)
            {
                case 2://L0 Reporter
                    break;
                case 3://L1 Manager
                    break;
                case 4://L2 SSHE Corporate
                    break;
            }
        }
    }

    public static void SaveFile_DJSI(int nReportID, string sUploadPath, string sUploadPath_TEMP, List<sysGlobalClass.FileUploadMaster> lstFile)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        db.T_Report_File.Where(w => w.nReportID == nReportID).ToList().ForEach(p => db.T_Report_File.Remove(p));
        if (lstFile.Any())
        {
            int i = 1;
            lstFile = lstFile.Where(w => w.isDel == false).ToList();
            lstFile.ForEach(f =>
            {
                T_Report_File a = new T_Report_File();
                a.nReportID = nReportID;
                a.nItem = i;
                a.sFilename = f.sFileName;
                a.sPath = sUploadPath;
                a.sSysFileName = f.sSysFileName;
                a.sDescription = f.sDescription;
                if (f.isNew)
                {
                    a.sPath = sUploadPath;
                }
                else
                {
                    a.sPath = f.sPath;
                }
                db.T_Report_File.Add(a);
                i++;
                #region File
                SystemFunction.CheckPathAndMoveFile(f.sSysFileName, f.sFileName, (f.isNew ? sUploadPath : f.sPath), (f.isNew ? sUploadPath_TEMP : f.sPath));
                #endregion
            });

            db.SaveChanges();
        }
    }

    #region Class
    [Serializable]
    public class TReturnLoadData : sysGlobalClass.CResutlWebMethod
    {
        public List<c_djsi> lstDJSI { get; set; }
    }

    [Serializable]
    public class TReturntemplete : sysGlobalClass.CResutlWebMethod
    {
        public List<c_ReportDJSI_Item> lstData { get; set; }
        public List<c_TempleteError> lstDataError { get; set; }
        public bool Issucess { get; set; }
        public string Msg { get; set; }
    }

    [Serializable]
    public class c_ReportDJSI_Item
    {
        public int nReportID { get; set; }
        public int nItem { get; set; }
        public int? nItemHead { get; set; }
        public decimal? nMale_1 { get; set; }
        public decimal? nMale_2 { get; set; }
        public decimal? nMale_3 { get; set; }
        public decimal? nFemale_1 { get; set; }
        public decimal? nFemale_2 { get; set; }
        public decimal? nFemale_3 { get; set; }
        public decimal? nMale_Total { get; set; }
        public decimal? nFemale_Total { get; set; }
        public bool? IsAutoCal { get; set; }
        public bool? IsTotal { get; set; }
        public string sName { get; set; }
    }
    [Serializable]
    public class c_TM_DJSI
    {
        public int nRowExcel { get; set; }
        public int nItem { get; set; }
        public string sName { get; set; }
        public int? nItemHead { get; set; }
        public int? nSibling { get; set; }
        public bool? IsTotal { get; set; }
        public int? nUnit { get; set; }
        public bool? IsHead { get; set; }
        public bool? IsAutoCal { get; set; }
    }
    [Serializable]
    public class c_TempleteError
    {
        public int nRowExcel { get; set; }
        public List<string> lstMgError { get; set; }
    }
    [Serializable]
    public class c_DataSave
    {
        public int nReportID { get; set; }
        public int nCompanyID { get; set; }
        public int nYear { get; set; }
        public int nQuarter { get; set; }
        public int nStatusID { get; set; }
        public List<c_ReportDJSI_Item> lstDJSI { get; set; }
        public sysGlobalClass.FileUploadMaster objFile { get; set; }

        public bool IsL0 { get; set; }
        public bool IsL1 { get; set; }
        public bool IsL2 { get; set; }
        public int nType { get; set; }
        public int nCurrentStatusID { get; set; }
        public string sComment { get; set; }
        public List<sysGlobalClass.FileUploadMaster> lstFile { get; set; }
    }
    [Serializable]
    public class TReturnStepApprover
    {
        public bool Issuccess { get; set; }
        public int nStatusID { get; set; }
    }
    [Serializable]
    public class c_ReturnGetData : sysGlobalClass.CResutlWebMethod
    {
        public List<c_Log> lstLog { get; set; }
        public List<sysGlobalClass.FileUploadMaster> lstFile { get; set; }
    }
    [Serializable]
    public class c_Log
    {
        public string sAction { get; set; }
        public string sActionBy { get; set; }
        public string sActionDate { get; set; }
        public string sComment { get; set; }
    }
    #endregion
}