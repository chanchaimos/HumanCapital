using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project_edit : System.Web.UI.Page
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

                string str = Request.QueryString["str"];
                string sID = hddID.Value = !string.IsNullOrEmpty(str) ? STCrypt.Decrypt(str) : "";

                //SetControl();

                string sPageType = "Add";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (!string.IsNullOrEmpty(sID))
                {
                    SetData(sID);
                    sPageType = sPer == "A" ? "Edit" : "Detail";
                }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
                SetControl();
            }
        }
    }

    private void SetData(string sID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var q = db.T_Project.FirstOrDefault(w => w.nProjectID + "" == sID);
        if (q != null)
        {
            ddlCompany.SelectedValue = q.nCompanyID + "";
            txtProjectname.Text = q.sProjectName;
            txtOrganization.Text = q.sOrganization;
            txtObjective.Text = q.sObjective;
            rdlProductivity.SelectedValue = q.nProductivity == 1 ? "1" : "0";
            rdlActive.SelectedValue = q.IsActive ? "1" : "0";
            hddIsPageload.Value = "1";

            txtEconomic.Text = q.nReturn_Economy + "";
            txtSocial.Text = q.nReturn_Social + "";
            txtEnvironment.Text = q.nReturn_Environment + "";
            txtOther.Text = q.nReturn_Other + "";
            txtOpex.Text = q.nPrice_Opex + "";
            txtCapex.Text = q.nPrice_Capex + "";
            txtDes.Text = q.sDescription;
            txtStartdate.Text = q.dStart.HasValue ? q.dStart.Value.ToString("dd/MM/yyyy") : "";
            txtEnddate.Text = q.dEnd.HasValue ? q.dEnd.Value.ToString("dd/MM/yyyy") : "";
        }
    }

    private void SetControl()
    {
        var lstCompany = Human_Function.Get_ddl_CompanyByPermission();
        ddlCompany.DataSource = lstCompany;
        ddlCompany.DataValueField = "Value";
        ddlCompany.DataTextField = "Text";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("- Company -", ""));
    }

    #region WebMedthod 
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(CData_Project item)
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
            Func<int, int, string, string, bool> CheckDataDup = (id, nCompany, sProjectname, sOrganization) =>
             {
                 bool IsDup = false;
                 sProjectname = sProjectname.Replace(" ", "").ToLower();
                 sOrganization = sOrganization.Replace(" ", "").ToLower();
                 var lst = db.T_Project.Where(w => !w.IsDel
                 && (id != 0 ? w.nProjectID != id : true)
                 //&& (nCompany != 0 ? w.nCompanyID != nCompany : true)
                 && w.sProjectName.Replace(" ", "").ToLower() == sProjectname
                 && w.sOrganization.Replace(" ", "").ToLower() == sOrganization
                 ).ToList();
                 IsDup = lst.Any();
                 return IsDup;
             };
            #endregion
            if (CheckDataDup(item.nProjectID, item.nCompany, item.sProjectname, item.sOrganization))
            {
                result.Status = SystemFunction.process_Duplicate;
            }
            else
            {
                bool Isnew = false;
                try
                {
                    var q = db.T_Project.FirstOrDefault(w => w.nProjectID == item.nProjectID);
                    if (q == null)
                    {
                        q = new T_Project();
                        //q.nProjectID = db.T_Project.Any() ? db.T_Project.Max(m => m.nProjectID) + 1 : 1;
                        q.nCreateBy = UserAccount.SessionInfo.nUserID;
                        q.dCreate = DateTime.Now;
                        db.T_Project.Add(q);
                        Isnew = true;
                    }
                    q.nCompanyID = item.nCompany;
                    q.sProjectName = item.sProjectname;
                    q.sOrganization = item.sOrganization;
                    q.sObjective = item.sObjective;

                    //nProductivity 1=Return,0=Non-Monetary
                    q.nProductivity = item.nProductivity;
                    q.nReturn_Economy = (q.nProductivity == 1 ? CommonFunction.ParseDecimalNull(item.sEconomic) : null);
                    q.nReturn_Environment = (q.nProductivity == 1 ? CommonFunction.ParseDecimalNull(item.sEnvironment) : null);
                    q.nReturn_Social = (q.nProductivity == 1 ? CommonFunction.ParseDecimalNull(item.sSocial) : null);
                    q.nReturn_Other = (q.nProductivity == 1 ? CommonFunction.ParseDecimalNull(item.sOther) : null);
                    q.sDescription = item.sDes;//(item.nProductivity == 0 ? item.sDes : null);

                    q.nPrice_Opex = CommonFunction.ParseDecimalNull(item.sOpex);
                    q.nPrice_Capex = CommonFunction.ParseDecimalNull(item.sCapex);
                    q.dStart = CommonFunction.ConvertStringToDateTime(item.sStartdate);
                    q.dEnd = CommonFunction.ConvertStringToDateTime(item.sEnddate);
                    q.nCourse = item.lstCourseSub.Count();
                    q.IsActive = item.IsActive;
                    q.IsDel = false;
                    q.dUpdate = DateTime.Now;
                    q.nUpdateBy = UserAccount.SessionInfo.nUserID;
                    db.SaveChanges();

                    #region CourseSub
                    db.T_Project_Course.Where(a => a.nProjectID == item.nProjectID).ToList().ForEach(p => db.T_Project_Course.Remove(p));
                    if (item.lstCourseSub.Any())
                    {
                        item.lstCourseSub.ForEach(f =>
                        {
                            db.T_Project_Course.Add(new T_Project_Course
                            {
                                nProjectID = q.nProjectID,
                                nCourseSubID = CommonFunction.GetIntNullToZero(f.nCourseSubID + ""),
                                nBenefit = f.nBenefit
                            });
                        });
                    }
                    #endregion

                    db.SaveChanges();

                    Human_Function.UpdateLog(5, "", (Isnew ? "INSERT " : "UPDATE ") + " ProjectID =  " + q.nProjectID);
                    result.Status = SystemFunction.process_Success;
                }
                catch (Exception e)
                {
                    result.Msg = e + "";
                    result.Status = SystemFunction.process_SaveFail;
                }
            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnCourseSub GetCourse(string sSearch, string sCompany)
    {
        c_ReturnCourseSub TReturn = new c_ReturnCourseSub();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";

            TReturn.lstData = new List<T_Course_Sub>();
            TReturn.lstData = db.T_Course_Sub.Where(w => w.IsActive && !w.IsDel && w.sName.ToLower().Replace(" ", "").Contains(sSearch) && w.nCompanyID + "" == sCompany).Take(20).ToList();
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_ReturnCourseSub GetData(int nProjectID)
    {

        c_ReturnCourseSub result = new c_ReturnCourseSub();
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
                                           nBenefit = b.nBenefit
                                       }).ToList();
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod CheckDate(string sDatestr, string sDateEnd)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            if (!string.IsNullOrEmpty(sDatestr) && !string.IsNullOrEmpty(sDateEnd))
            {
                string[] arrStr = sDatestr.Split('/');
                string[] arrEnd = sDateEnd.Split('/');

                if (arrStr[2] == arrEnd[2]) result.Status = SystemFunction.process_Success;
                else result.Status = SystemFunction.process_Failed; result.Msg = "Please select a date range within the same year.";
            }
        }
        return result;
    }
    #endregion

    #region Class

    [Serializable]
    public class c_ReturnCourseSub : sysGlobalClass.CResutlWebMethod
    {
        public List<T_Course_Sub> lstData { get; set; }
        public List<CData_CourseSub> lstDataCourseSub { get; set; }
    }

    [Serializable]
    public class CData_Project
    {
        public int nProjectID { get; set; }
        public int nCompany { get; set; }
        public string sProjectname { get; set; }
        public string sOrganization { get; set; }
        public string sObjective { get; set; }
        public int nProductivity { get; set; }
        public string sEconomic { get; set; }
        public string sSocial { get; set; }
        public string sEnvironment { get; set; }
        public string sOther { get; set; }
        public string sOpex { get; set; }
        public string sCapex { get; set; }
        public string sStartdate { get; set; }
        public string sEnddate { get; set; }
        public string sDes { get; set; }
        public List<CData_CourseSub> lstCourseSub { get; set; }
        public bool IsActive { get; set; }
    }

    [Serializable]
    public class CData_CourseSub
    {
        public int nCourseSubID { get; set; }
        public int nCompanyID { get; set; }
        public string sName { get; set; }
        public int nBenefit { get; set; }
    }
    #endregion
}