using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Globalization;
//using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Reflection;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
/// <summary>
/// Summary description for SystemFunction
/// </summary>
public class SystemFunction
{
    static string conn = WebConfigurationManager.ConnectionStrings["PTTGC_Human_ConnectionString"].ConnectionString.ToString();

    public static string sSessionName_UserInfo = "SS_UserInfo";
    public static string process_SessionExpired = "SSEXP";
    public static string sPageRedirectSessionExpired = "Login.aspx";

    public static string process_Success = "Success";
    public static string process_Failed = "Failed";
    public static string process_FileOversize = "OverSize";
    public static string process_FileInvalidType = "InvalidType";
    public static string process_Duplicate = "DUP";
    public static string process_SaveFail = "SaveFail";
    public static string sMsgUsernamePasswordWrong = "Username or password is incorrect.";
    public static string sMsgDontPRMSLogin = "You don't have a permission to login, please contact your administrator";
    public static string sMsgSaveInNotStep = "Cannot action";

    public const string SystemName = "PTTGC HUMAN Capital";

    public static bool IsWebServiceMode = (ConfigurationManager.AppSettings["WebServiceMode"] + "") == "Y";
    public static string sPasswordBypass = ConfigurationManager.AppSettings["PasswordBypass"] + "";
    public static string sPasswordDefault = ConfigurationManager.AppSettings["PasswordDefault"] + "";

    public SystemFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetPasswordBypass()
    {
        return ConfigurationSettings.AppSettings["PasswordBypass"].ToString();
    }

    public string ParseObjectToJson(object ob)
    {
        try
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
            string res = serializer.Serialize(ob);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

            return res;
        }
        catch
        {
            return "";
        }
    }

    public T JsonDeserialize<T>(string jsonString)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }

    public static string GetMenuName(int nMenuID, bool IsEdit, string sPer)
    {
        string sRet = "";
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qMenu = db.TM_Menu.FirstOrDefault(w => w.nMenuID == nMenuID);
        if (qMenu != null)
        {
            string sIconR = " <i class='fa fa-chevron-right'></i> ";

            sRet = "<i class='" + qMenu.sIcon + "'></i> ";
            if (qMenu.nMenuHead.HasValue)
            {
                var qMenuHead = db.TM_Menu.FirstOrDefault(w => w.nMenuID == (qMenu.nMenuHead ?? 0));
                if (qMenuHead != null)
                {
                    sRet += qMenuHead.sMenuName + sIconR;
                }
            }

            string sMenuName = IsEdit ? ("<a href='" + qMenu.sMenuLink + "' class='aMenu'>" + qMenu.sMenuName + "</a>") : qMenu.sMenuName;
            sRet += sMenuName + (sPer != "" ? sIconR + sPer : "");
        }

        return sRet;
    }

    public static void BindDdlPageSize(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 10, 30, 50, 100 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static void BindDdlPageSize20(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 20, 30, 50, 100 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static void BindDdlPageSizeSmall(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 5, 10, 20 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static string GetPMS(int nMenuID)
    {
        string result = "A";

        if (!UserAccount.IsExpired)
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();
            int nUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0;
            var qUser = db.TB_User.FirstOrDefault(w => w.nUserID == nUserID && !w.IsDel && w.IsActive);
            if (qUser != null)
            {
                var IsAdmin = qUser.nRole == 1;
                result = db.TM_Menu.FirstOrDefault(w => w.nMenuID == nMenuID) != null ? "A" : "N";//&& (!IsAdmin ? w.nMenuID != 7 && w.nMenuHead != 7 : true)
            }
        }

        return result;
    }
    public static int GetIntNullToZero(string sVal)
    {
        int nTemp = 0;
        int nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = int.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = int.Parse(sVal);
            }
        }

        return nTemp;
    }
    public static string ConvertExponentialToString(string sVal)
    {
        string sRsult = "";
        try
        {
            decimal nTemp = 0;
            bool check = Decimal.TryParse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float, null, out nTemp);
            if (check)
            {
                decimal d = Decimal.Parse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float);
                sRsult = (d + "").Replace(",", "");
            }
            else
            {
                sRsult = sVal;
            }
        }
        catch
        {
            sRsult = sVal;
        }

        return sRsult != null ? (sRsult.Length < 50 ? sRsult : sRsult.Remove(50)) : ""; //เพื่อไม่ให้ตอน Save Error หากค่าที่เกิดจากผลการคำนวนเกิน Type ใน DB (varchar(50))
    }

    public static string GetSysCon()
    {
        string sCon = WebConfigurationManager.ConnectionStrings["PTTGC_Human_ConnectionString"].ConnectionString.ToString();
        string smtp = WebConfigurationManager.AppSettings["smtpmail"].ToString();
        string PasswordBypass = WebConfigurationManager.AppSettings["PasswordBypass"].ToString();
        string PasswordDefault = WebConfigurationManager.AppSettings["PasswordDefault"].ToString();

        return "Con : " + sCon + Environment.NewLine +
               "SMTP : " + smtp + Environment.NewLine +
               "PasswordBypass : " + PasswordBypass + Environment.NewLine +
               "PasswordDefault : " + PasswordDefault;
    }

    #region File
    public static void CheckPathAndMoveFile(string sysFileName, string FileName, string sUploadPath, string sUploadPath_Temp)
    {
        HttpContext context = HttpContext.Current;
        string sMapPath = context.Server.MapPath("./");
        string sPathSave = sMapPath + sUploadPath;
        if (!Directory.Exists(sPathSave))
        {
            Directory.CreateDirectory(sPathSave);
        }
        if (File.Exists(sMapPath + sUploadPath_Temp + sysFileName))
        {
            string currentPath = context.Server.MapPath("./" + sUploadPath_Temp);
            File.Move(currentPath + "/" + sysFileName, sPathSave + "/" + sysFileName);
        }
    }

    public static void RemoveFile(string sPathAndFileName)
    {
        HttpContext context = HttpContext.Current;
        string sMapPath = context.Server.MapPath("./");
        if (File.Exists(sPathAndFileName))
        {
            File.Delete(sPathAndFileName);
        }
    }

    public static void DeleteAllFile(string _pathFile)
    {
        if (Directory.Exists(_pathFile.Replace("/", "\\")))
        {
            DirectoryInfo di = new DirectoryInfo(_pathFile.Replace("/", "\\"));
            FileInfo[] fi = di.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in fi)
            {
                try
                {
                    f.Delete();
                }
                finally
                {

                }
            }
        }
    }

    public static void RemoveFileAllInFolfer(string sPath)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(sPath);
        if (di.Exists)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
    #endregion
}