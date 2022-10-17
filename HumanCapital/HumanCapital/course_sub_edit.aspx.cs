using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class course_sub_edit : System.Web.UI.Page
{
    private static int nMenuID = 4;

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

    private void SetControl()
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var lstCompany = Human_Function.Get_ddl_CompanyByPermission();
        ddlCompany.DataSource = lstCompany;
        ddlCompany.DataValueField = "Value";
        ddlCompany.DataTextField = "Text";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("- Company -", ""));

        //var lstTargetGroup = db.TM_MasterData_Sub.Where(w => w.IsActive && !w.IsDel && w.nMainID == 7).ToList();
        //int nIndex = 0;
        //lstTargetGroup.ForEach(f =>
        //{
        //    ddlTargetGroup.Items.Insert(nIndex, new ListItem(f.sName, f.nSubID + ""));
        //    ddlTargetGroup.Items[nIndex].Attributes.Add("title", f.sDescription);
        //});
        //ddlTargetGroup.Items.Insert(0, new ListItem("- Target Group -", ""));

        var lstTargetGroup = Human_Function.Get_ddl_TargetGroup();
        ddlTargetGroup.DataSource = lstTargetGroup;
        ddlTargetGroup.DataValueField = "Value";
        ddlTargetGroup.DataTextField = "Text";
        ddlTargetGroup.DataBind();
        //ddlTargetGroup.Items.Insert(0, new ListItem("- Target Group -", ""));

        var lstEmployee_type = db.TM_MasterData_Sub.Where(w => w.nMainID == 6 && w.IsActive && !w.IsDel).ToList();
        rdlTrainingmethod.DataSource = lstEmployee_type;
        rdlTrainingmethod.DataValueField = "nSubID";
        rdlTrainingmethod.DataTextField = "sName";
        rdlTrainingmethod.DataBind();
        rdlTrainingmethod.SelectedValue = "29";
    }

    public void SetData(string sCourseSubID)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var q = db.T_Course_Sub.FirstOrDefault(w => w.nCourseSubID + "" == sCourseSubID && !w.IsDel);
        if (q != null)
        {
            var qCom = db.T_Course.FirstOrDefault(w => w.nCourseID == q.nCourseID && !w.IsDel);
            if (qCom != null)
            {
                txtCoursename.Text = qCom.sName;
                txtCourseID.Text = qCom.nCourseID + "";
            }
            ddlCompany.SelectedValue = q.nCompanyID + "";
            txtSubCoursename.Text = q.sName;
            txtDescription.Text = q.sDescription;
            txtStartDate.Text = (q.dStart.HasValue ? q.dStart.Value.ToString("dd/MM/yyyy") : "");
            txtEndDate.Text = (q.dEnd.HasValue ? q.dEnd.Value.ToString("dd/MM/yyyy") : "");
            txtStartTime.Text = (q.tStart.HasValue ? q.tStart.Value.ToString(@"hh\:mm") : "");
            txtEndTime.Text = (q.tEnd.HasValue ? q.tEnd.Value.ToString(@"hh\:mm") : "");
            txtPrice.Text = (q.nPrice.HasValue ? q.nPrice.Value.ToString("0.##") : "");
            txtAmount.Text = (q.nAmount.HasValue ? q.nAmount.Value.ToString() : "");
            rdlActive.SelectedValue = q.IsActive ? "1" : "0";
            rdlTrainingmethod.SelectedValue = q.nTraining_method.Value + "";

            var lstTG = db.T_Course_Sub_TG.Where(w => w.nCourseSubID + "" == sCourseSubID).Select(s => s.nTargetGroup).ToList();
            hddTGID.Value = string.Join(",", lstTG);
        }
        else
        {
            Response.Redirect("course.aspx");
        }
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static c_Return GetCourse(string sSearch, int nCompanyID)
    {
        c_Return TReturn = new c_Return();

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            sSearch = sSearch.ToLower().Replace(" ", "") + "";
            TReturn.lstData = db.Database.SqlQuery<c_Course>(@"SELECT * FROM T_Course where IsActive=1 and IsDel=0 and sName like '%" + sSearch + "%' and nCompanyID = '" + nCompanyID + @"' ").Select(s => new c_Course
            {
                nCourseID = s.nCourseID,
                sName = s.sName,
            }).ToList();
        }
        else
        {
            TReturn.Status = SystemFunction.process_SessionExpired;
        }

        return TReturn;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(c_SaveData_Subcourse item)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            try
            {
                PTTGC_HumanEntities db = new PTTGC_HumanEntities();
                var IsNew = item.nSubCourseID == 0;
                var qDup = db.T_Course_Sub.FirstOrDefault(w => (!IsNew ? w.nCourseSubID != item.nSubCourseID : true)
                && w.sName == item.sSubCoursename && !w.IsDel
                && (!string.IsNullOrEmpty(item.sCompany) ? w.nCompanyID + "" == item.sCompany : true)
                && (!string.IsNullOrEmpty(item.nCourseID + "") ? w.nCourseID == item.nCourseID : true));
                if (qDup == null)
                {
                    #region Add/Update T_Course_Sub
                    var qSubCourse = db.T_Course_Sub.FirstOrDefault(w => w.nCourseSubID == item.nSubCourseID);
                    if (!IsNew && qSubCourse.IsDel)
                    {
                        result.Msg = SystemFunction.sMsgSaveInNotStep;
                        result.Status = SystemFunction.process_SaveFail;
                        return result;
                    }

                    var nUserThis = UserAccount.SessionInfo.nUserID;
                    if (IsNew)
                    {
                        qSubCourse = new T_Course_Sub();
                        qSubCourse.nCreateBy = nUserThis;
                        qSubCourse.dCreate = DateTime.Now;
                        db.T_Course_Sub.Add(qSubCourse);
                    }
                    qSubCourse.dStart = (!string.IsNullOrEmpty(item.sStartDate) ? CommonFunction.ConvertStringToDateTime(item.sStartDate) : null);
                    qSubCourse.dEnd = (!string.IsNullOrEmpty(item.sEndDate) ? CommonFunction.ConvertStringToDateTime(item.sEndDate) : null);
                    qSubCourse.tStart = (!string.IsNullOrEmpty(item.sStartTime) ? CommonFunction.ConvertStringToTimespan(item.sStartTime) : null);
                    qSubCourse.tEnd = (!string.IsNullOrEmpty(item.sEndTime) ? CommonFunction.ConvertStringToTimespan(item.sEndTime) : null);
                    qSubCourse.nTraining_method = CommonFunction.ParseIntNull(item.sTraining_method);
                    qSubCourse.nPrice = CommonFunction.ParseDecimalNull(item.sPrice);
                    qSubCourse.nAmount = CommonFunction.ParseIntNull(item.sAmount);
                    qSubCourse.nCourseID = item.nCourseID;
                    qSubCourse.nCompanyID = CommonFunction.GetIntNullToZero(item.sCompany);
                    qSubCourse.sName = item.sSubCoursename;
                    qSubCourse.sDescription = item.sDescription;
                    qSubCourse.IsActive = item.IsActive;
                    qSubCourse.IsDel = false;
                    qSubCourse.nUpdateBy = nUserThis;
                    qSubCourse.dUpdate = DateTime.Now;
                    db.SaveChanges();

                    int nCourseSubID = qSubCourse.nCourseSubID;
                    CommonFunction.ExecuteNonQuery("delete T_Course_Sub_TG where nCourseSubID = " + nCourseSubID);
                    if (item.lstTargetGroup.Any())
                    {
                        foreach (var itemTG in item.lstTargetGroup)
                        {
                            db.T_Course_Sub_TG.Add(new T_Course_Sub_TG()
                            {
                                nCourseSubID = nCourseSubID,
                                nTargetGroup = itemTG
                            });
                        }
                        db.SaveChanges();
                    }

                    Human_Function.UpdateLog(4, "", (IsNew ? "INSERT " : "UPDATE ") + " nCourseSubID =  " + qSubCourse.nCourseSubID);
                    result.Status = SystemFunction.process_Success;
                    #endregion
                }
                else
                {
                    result.Status = SystemFunction.process_Duplicate;
                }


            }
            catch (Exception e)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = e + "";

            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod CheckDateTime(string sStartDate, string sEndDate, string sStartTime, string sEndTime)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();

        if (UserAccount.IsExpired)
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            if (!string.IsNullOrEmpty(sStartDate) && !string.IsNullOrEmpty(sEndDate))
            {
                if (!string.IsNullOrEmpty(sStartTime) && !string.IsNullOrEmpty(sEndTime))
                {
                    TimeSpan? dStartTime = CommonFunction.ConvertStringToTimespan(sStartTime);
                    TimeSpan? dEndTime = CommonFunction.ConvertStringToTimespan(sEndTime);
                    if (sStartDate == sEndDate)
                    {
                        if (dEndTime < dStartTime)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "The end time is less than the start time.";
                        }
                    }
                }
            }
        }
        return result;
    }
    #endregion

    #region Class
    [Serializable]
    public class c_Return : sysGlobalClass.CResutlWebMethod
    {
        public List<c_Course> lstData { get; set; }
    }

    [Serializable]
    public class c_Course
    {
        public int nCourseID { get; set; }
        public string sName { get; set; }
    }
    [Serializable]
    public class c_SaveData_Subcourse
    {
        public int nCourseID { get; set; }
        public int nSubCourseID { get; set; }
        public string sCompany { get; set; }
        public string sSubCoursename { get; set; }
        public string sDescription { get; set; }
        public string sStartDate { get; set; }
        public string sStartTime { get; set; }
        public string sEndDate { get; set; }
        public string sEndTime { get; set; }
        public List<int> lstTargetGroup { get; set; }
        public string sTraining_method { get; set; }
        public string sPrice { get; set; }
        public string sAmount { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion
}