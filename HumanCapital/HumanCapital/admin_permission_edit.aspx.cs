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

public partial class admin_permission_edit : System.Web.UI.Page
{
    private static int nMenuID = 10;

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
                string sUserID = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";

                SetControl();

                string sPageType = "Add";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (sUserID != "")
                {
                    SetData(sUserID);
                    sPageType = sPer == "A" ? "Edit" : "Delete";
                }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
            }
        }
    }

    public void SetControl()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        var lstRole = db.TM_MasterData_Sub.Where(w => w.nMainID == 1 && w.IsActive && !w.IsDel).ToList();
        ddlRole.DataSource = lstRole;
        ddlRole.DataValueField = "nSubID";
        ddlRole.DataTextField = "sName";
        ddlRole.DataBind();
        ddlRole.Items.Insert(0, new ListItem("- User group - ", ""));

        //var lstCompany = db.TB_Company.Where(w => w.IsActive && !w.IsDel).OrderBy(o => o.nCompanyType).ThenBy(o => o.sCompanyCode).ToList();
        //cblCompany.DataSource = lstCompany;
        //cblCompany.DataValueField = "nCompanyID";
        //cblCompany.DataTextField = "sCompanyName";
        //cblCompany.DataBind();

        var lstEmployee_type = db.TM_MasterData_Sub.Where(w => w.nMainID == 3 && w.IsActive && !w.IsDel).ToList();
        rdlUserType.DataSource = lstEmployee_type;
        rdlUserType.DataValueField = "nSubID";
        rdlUserType.DataTextField = "sName";
        rdlUserType.DataBind();
        rdlUserType.SelectedValue = "5";
        txtPassword.Attributes.Add("type", "password");
    }

    public void SetData(string sUserID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID + "" == sUserID && !w.IsDel);
        if (qUser != null)
        {
            rdlUserType.SelectedValue = qUser.nCompanyType + "";
            hddnUserID.Value = sUserID;
            bool IsGC = qUser.nCompanyType == 5;
            if (IsGC)//GC
            {
                txtEmpName.Enabled = false;
                txtEmpName.Text = qUser.sUserID + " - " + qUser.sFirstname + "  " + qUser.sLastname;
                txtEmpID.Text = qUser.sUserID;
                txtFName.Text = qUser.sFirstname;
                txtLName.Text = qUser.sLastname;
            }
            else
            {
                txtName.Text = qUser.sFirstname;
                txtSurname.Text = qUser.sLastname;
                txtUsername.Text = qUser.sUserID;
                txtPassword.Text = STCrypt.Decrypt(qUser.sPasswordEncrypt);
            }

            txtEmail.Text = qUser.sEmail;
            ddlRole.SelectedValue = qUser.nRole + "";
            rdlActive.SelectedValue = qUser.IsActive ? "1" : "0";

            //ddlRole.Enabled = false;
            txtEmail.Enabled = !IsGC;

            var lstCompany = db.TB_User_Company.Where(w => w.nUserID + "" == sUserID).Select(s => s.nCompanyID).ToList();
        }
        else
        {
            Response.Redirect("admin_permission.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnEmp GetEmp(string sSearch)
    {
        c_ReturnEmp TReturn = new c_ReturnEmp();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            var lstEmpNotCode = db.Database.SqlQuery<TB_User>("select * from TB_User where IsDel = 0").Select(s => s.sUserID).ToList();
            //var sOrg = IsCSR ? string.Join(",", db.TM_Organization.Select(w => w.sOrgID).ToList()) : "";
            TReturn.lstData = HR_WebService.EmployeeService_Search(sSearch, "", lstEmpNotCode);
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ResetPassword(int nUserID)
    {
        sysGlobalClass.CResutlWebMethod TReturn = new sysGlobalClass.CResutlWebMethod();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && w.nCompanyType != 5 && !w.IsDel);
            if (qUser != null)
            {
                string sPasswordDefault = SystemFunction.sPasswordDefault;
                qUser.sPassword = STCrypt.encryptMD5(sPasswordDefault);
                qUser.sPasswordEncrypt = STCrypt.Encrypt(sPasswordDefault);
                db.SaveChanges();

                string _to = qUser.sEmail;

                string subject = SystemFunction.SystemName + " | Your password has been reset";
                string message = string.Format(Human_Sendmail.GET_TemplateEmail(),
                        "Dear K." + qUser.sFirstname + ' ' + qUser.sLastname,
                        "Your new password is " + sPasswordDefault,
                        "",
                        "",
                        "");

                Human_Sendmail.SendNetMail("", _to, "", subject, message, new List<string>());
            }
            else
            {
                TReturn.Msg = SystemFunction.sMsgSaveInNotStep;
                TReturn.Status = SystemFunction.process_SaveFail;
            }
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(int nUserID, string sUserID, string sPassword, string sFirstname, string sLastname, string sEmail, bool IsGC, int nRole, List<c_Permission> lstPermission, bool IsActive, List<string> lstCompany)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            #region Fuc Check Dup
            Func<int, int, List<string>, string> CheckDupCompany = (id, nRoleID, lst) =>
            {
                string sMg = "";
                bool IsAdmin_or_L0 = (nRoleID == 1 || nRole == 2);
                if (!IsAdmin_or_L0)
                {
                    var lstDataCompany = db.TB_Company.Where(w => !w.IsDel && w.IsActive).ToList();
                    var lstUser = db.TB_User.Where(w => !w.IsDel && w.nRole == nRoleID && (id != 0 ? w.nUserID != id : true)).ToList();

                    if (lstUser.Any())
                    {
                        var lstUserID_InRole = lstUser.Select(s => s.nUserID).ToList();
                        var lstCompanyIsUse = db.TB_User_Company.Where(w => lstUserID_InRole.Contains(w.nUserID)).GroupBy(o => new { o.nCompanyID }).Select(s => s.Key).ToList();
                        if (lstCompanyIsUse.Any())
                        {
                            lst.ForEach(f =>
                            {
                                var q = lstCompanyIsUse.FirstOrDefault(w => w.nCompanyID == CommonFunction.GetIntNullToZero(f));
                                if (q != null)
                                {
                                    sMg += "- " + (lstDataCompany.Any(w => w.nCompanyID == q.nCompanyID) ? lstDataCompany.FirstOrDefault(w => w.nCompanyID == q.nCompanyID).sCompanyName : "") + "<br/>";
                                }
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(sMg))
                    {
                        sMg += "Data cannot be duplicated.";
                    }
                }
                return sMg;
            };

            Func<int, string, bool> CheckDupUsername = (id, sUsername) =>
            {
                bool IsDup = false;
                sUsername = sUsername.Replace(" ", "").ToLower();
                var lst = db.TB_User.Where(w => !w.IsDel && (id != 0 ? w.nUserID != id : true) && w.sUserID.Replace(" ", "").ToLower() == sUsername).ToList();
                IsDup = lst.Any();
                return IsDup;
            };
            #endregion

            if (CheckDupUsername(nUserID, sUserID))
            {
                result.Msg = "Username is duplicated";
                result.Status = SystemFunction.process_Duplicate;
            }
            else
            {
                string sCompanyDup = CheckDupCompany(nUserID, nRole, lstCompany);
                if (!string.IsNullOrEmpty(sCompanyDup))
                {
                    result.Msg = sCompanyDup;
                    result.Status = SystemFunction.process_Duplicate;
                }
                else
                {
                    var IsNew = nUserID == 0;
                    var qDup = db.TB_User.FirstOrDefault(w => (!IsNew ? w.nUserID != nUserID : true) && w.sUserID == sUserID && !w.IsDel);
                    if (qDup == null)
                    {
                        #region Add/Update TB_User
                        var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID);
                        if (!IsNew && qUser.IsDel)
                        {
                            result.Msg = SystemFunction.sMsgSaveInNotStep;
                            result.Status = SystemFunction.process_SaveFail;
                            return result;
                        }

                        var nUserThis = UserAccount.SessionInfo.nUserID;

                        if (IsNew)
                        {
                            qUser = new TB_User();
                            qUser.nCreateBy = nUserThis;
                            qUser.dCreate = DateTime.Now;
                            db.TB_User.Add(qUser);
                        }

                        qUser.sUserID = sUserID;
                        qUser.sPassword = !IsGC ? STCrypt.encryptMD5(sPassword.Trim()) : null;
                        qUser.sPasswordEncrypt = !IsGC ? STCrypt.Encrypt(sPassword) : null;
                        qUser.sFirstname = sFirstname;
                        qUser.sLastname = sLastname;
                        qUser.sEmail = sEmail;
                        qUser.nCompanyType = (IsGC ? 5 : 6);
                        qUser.nRole = nRole;
                        qUser.IsActive = IsActive;
                        qUser.IsDel = false;
                        qUser.nUpdateBy = nUserThis;
                        qUser.dUpdate = DateTime.Now;
                        db.SaveChanges();
                        #endregion

                        #region Add/Company
                        if (lstCompany.Any())
                        {
                            db.TB_User_Company.Where(w => w.nUserID == qUser.nUserID).ToList().ForEach(p => db.TB_User_Company.Remove(p));
                            lstCompany.ForEach(f =>
                            {
                                string[] sID = f.Split('^');
                                TB_User_Company g = new TB_User_Company();
                                g.nUserID = qUser.nUserID;
                                g.nCompanyID = CommonFunction.GetIntNullToZero(f);
                                db.TB_User_Company.Add(g);
                                db.SaveChanges();
                            });
                        }
                        #endregion

                        #region Delete/Add
                        if (!IsNew) CommonFunction.ExecuteNonQuery("delete TB_User_Permission where nUserID = '" + nUserID + "'");

                        foreach (var item in lstPermission)
                        {
                            db.TB_User_Permission.Add(new TB_User_Permission() { nUserID = qUser.nUserID, nMenuID = item.nMenuID, nPermission = item.nPermission });
                        }

                        db.SaveChanges();
                        #endregion
                    }
                }
            }

        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_Return GetData_Default(int? nUserID)
    {
        c_Return result = new c_Return();

        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            #region Bind Data Menu
            var lstPermission = db.TB_User_Permission.Where(w => w.nUserID == nUserID).ToList();

            var lstTM_Menu = db.TM_Menu.Where(w => w.IsActive).OrderBy(o => o.nMenuOrder).ToList();
            var lstMenuLV1 = lstTM_Menu.Where(w => w.nLevel == 1).ToList();

            var lstData = new List<c_Menu>();

            foreach (var item in lstMenuLV1)
            {
                var lstTM_MenuSub = lstTM_Menu.Where(w => w.nMenuHead == item.nMenuID).ToList();
                if (lstTM_MenuSub.Any())
                {
                    lstData.Add(new c_Menu() { nMenuID = item.nMenuID, sMenuName = item.sMenuName, nPermission = 0, nLevel = (item.nLevel ?? 0), IsHead = true });

                    foreach (var item1 in lstTM_MenuSub)
                    {
                        var qPer = lstPermission.FirstOrDefault(w => w.nMenuID == item1.nMenuID);
                        int? nPer = qPer != null ? qPer.nPermission : null;
                        lstData.Add(new c_Menu() { nMenuID = item1.nMenuID, sMenuName = item1.sMenuName, nPermission = nPer, nLevel = (item1.nLevel ?? 0), IsHead = false });
                    }
                }
                else
                {
                    var qPer = lstPermission.FirstOrDefault(w => w.nMenuID == item.nMenuID);
                    int? nPer = qPer != null ? qPer.nPermission : null;
                    lstData.Add(new c_Menu() { nMenuID = item.nMenuID, sMenuName = item.sMenuName, nPermission = nPer, nLevel = (item.nLevel ?? 0), IsHead = false });
                }
            }
            #endregion

            #region Company All
            result.lstCompanay = db.TB_Company.Where(w => w.IsActive && !w.IsDel).OrderBy(o => o.nCompanyType).ThenBy(o => o.sCompanyCode).ToList();
            #endregion

            #region Bind Data Company
            result.lstDataCompany = new List<int>();
            var lstCompany = db.TB_User_Company.Where(w => w.nUserID == nUserID).Select(s => s.nCompanyID).ToList();
            #endregion

            result.lstDataCompany = lstCompany;
            result.lstDataMenu = lstData;
        }

        return result;
    }

    #region Class
    [Serializable]
    public class c_Return : sysGlobalClass.CResutlWebMethod
    {
        public List<c_Menu> lstDataMenu { get; set; }
        public List<int> lstDataCompany { get; set; }
        public List<TB_Company> lstCompanay { get; set; }
    }

    [Serializable]
    public class c_Menu
    {
        public int nMenuID { get; set; }
        public string sMenuName { get; set; }
        public int? nPermission { get; set; }
        public int nLevel { get; set; }
        public bool IsHead { get; set; }
    }

    [Serializable]
    public class c_ReturnEmp : sysGlobalClass.CResutlWebMethod
    {
        public HR_WebService.ObjectData lstData { get; set; }
    }

    [Serializable]
    public class c_User
    {
        public string sUserID { get; set; }
        public string sName { get; set; }
    }

    [Serializable]
    public class c_Permission
    {
        public int nMenuID { get; set; }
        public int? nPermission { get; set; }
    }
    #endregion
}