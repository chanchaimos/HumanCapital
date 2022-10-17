using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class course_edit : System.Web.UI.Page
{
    private static int nMenuID = 3;

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

                SetControl();

                string sPageType = "Add";
                string sPer = hddPermission.Value = SystemFunction.GetPMS(nMenuID);

                if (!string.IsNullOrEmpty(sID))
                {
                    SetData(sID);
                    sPageType = sPer == "A" ? "Edit" : "Detail";
                }
                lblHeader.Text = SystemFunction.GetMenuName(nMenuID, true, sPageType);
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

        var lstCategory = Human_Function.Get_ddl_Category();
        ddlCourseCat.DataSource = lstCategory;
        ddlCourseCat.DataValueField = "Value";
        ddlCourseCat.DataTextField = "Text";
        ddlCourseCat.DataBind();
        ddlCourseCat.Items.Insert(0, new ListItem("- Category -", ""));
    }

    public void SetData(string sCourseID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qCom = db.T_Course.FirstOrDefault(w => w.nCourseID + "" == sCourseID && !w.IsDel);
        if (qCom != null)
        {
            ddlCompany.SelectedValue = qCom.nCompanyID + "";
            txtCoursename.Text = qCom.sName;
            ddlCourseCat.SelectedValue = qCom.nCategoryID + "";
            txtDescription.Text = qCom.sDescription;
            rdlActive.SelectedValue = qCom.IsActive ? "1" : "0";
        }
        else
        {
            Response.Redirect("course.aspx");
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(int nCourseID, string sCompany, string sCoursename, string sCourseCat, string sDes, bool IsActive)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            var IsNew = nCourseID == 0;
            var qDup = db.T_Course.FirstOrDefault(w => (!IsNew ? w.nCourseID != nCourseID : true) && w.sName == sCoursename && !w.IsDel && (!string.IsNullOrEmpty(sCompany) ? w.nCompanyID + "" == sCompany : true));
            if (qDup == null)
            {
                #region Add/Update T_Course
                var qCourse = db.T_Course.FirstOrDefault(w => w.nCourseID == nCourseID);
                if (!IsNew && qCourse.IsDel)
                {
                    result.Msg = SystemFunction.sMsgSaveInNotStep;
                    result.Status = SystemFunction.process_SaveFail;
                    return result;
                }

                var nUserThis = UserAccount.SessionInfo.nUserID;

                if (IsNew)
                {
                    qCourse = new T_Course();
                    qCourse.nCreateBy = nUserThis;
                    qCourse.dCreate = DateTime.Now;
                    db.T_Course.Add(qCourse);
                }
                qCourse.nCompanyID = CommonFunction.GetIntNullToZero(sCompany);
                qCourse.sName = sCoursename;
                qCourse.nCategoryID = CommonFunction.ParseIntNull(sCourseCat);
                qCourse.sDescription = sDes;
                qCourse.IsActive = IsActive;
                qCourse.IsDel = false;
                qCourse.nUpdateBy = nUserThis;
                qCourse.dUpdate = DateTime.Now;
                db.SaveChanges();

                Human_Function.UpdateLog(3, "", (IsNew ? "INSERT " : "UPDATE ") + " nCourseID =  " + qCourse.nCourseID);
                result.Status = SystemFunction.process_Success;
                #endregion           
            }
            else
            {
                result.Status = SystemFunction.process_Duplicate;
            }

        }
        return result;
    }
}