using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _test : System.Web.UI.Page
{
    public static string SharePathUpFile = Human_Function.SharePathUpFile();
    public static string SharePathDomain = Human_Function.SharePathDomain();
    public static bool IsLogonSharePath = Human_Function.IsLogonSharePath();
    public static string SharePathUser = ConfigurationSettings.AppSettings["SharePathUser"].ToString();
    public static string SharePathPassword = ConfigurationSettings.AppSettings["SharePathPassword"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();


        //File.Copy(@"\\pttgc.corp\sgcfs\_Central\BI Project\Data Management\Sustain\HR", @"D:\xxx\test.txt", true);

        //ImpersonateUser("pttgc.corp", @"pttgc\z0004472", "Akeji13!");


        //sysLogonSharePath.Logon();

        //txtResult.Text = "Status Login :" + ((impersonateValidUser(SharePathUser, SharePathDomain, SharePathPassword)) ? "Y" : "N") + Environment.NewLine +
        //                "Domain : " + SharePathDomain + Environment.NewLine +
        //                "SharePathUser : " + SharePathUser + Environment.NewLine +
        //                "SharePathPassword : " + SharePathPassword;

        var x = DateTime.Now.ToString("ddMMyyyyHHmmss");
        try
        {
            var sPathSave = "/UploadFiles/DJSI/";
            string sPathTo = HttpContext.Current.Server.MapPath(sPathSave);
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/") + sPathSave.Replace("/", "\\")))
            {
                System.IO.Directory.CreateDirectory(sPathTo);
            }

            var qFileName = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 44);
            string sFilename = qFileName != null ? qFileName.sDescription : "";

            var sPathShare = "/ReadFileHR/";
            string sPathFrom = HttpContext.Current.Server.MapPath(sPathShare) + sFilename;
            
            sPathTo = sPathTo + x + ".xlsx";

            System.IO.File.Copy(sPathFrom, sPathTo, true);
        }
        catch (Exception ex)
        {
            SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + x + " Error : " + ex.Message));
        }
    }

    public static void SetLog_SyncDJSI(StringBuilder sEvent)
    {
        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        db.TM_Log_SyncDJSI.Add(new TM_Log_SyncDJSI()
        {
            dLog = DateTime.Now,
            sEvent = sEvent + ""
        });
        db.SaveChanges();
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static string Login(string Domain, string User, string PW)
    {
        return "Status Login :" + ((impersonateValidUser(User, Domain, PW)) ? "Y" : "N") + Environment.NewLine +
                        "Domain : " + Domain + Environment.NewLine +
                        "SharePathUser : " + User + Environment.NewLine +
                        "SharePathPassword : " + PW;
    }

    #region Logon To Share Path
    public const int LOGON32_LOGON_INTERACTIVE = 2;
    public const int LOGON32_PROVIDER_DEFAULT = 0;
    private static WindowsImpersonationContext impersonationContext;

    [DllImport("advapi32.dll")]
    public static extern int LogonUserA(String lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int DuplicateToken(IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool RevertToSelf();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);

    private static bool impersonateValidUser(String userName, String domain, String password)
    {
        WindowsIdentity tempWindowsIdentity;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;

        if (RevertToSelf())
        {
            if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                    {
                        CloseHandle(token);
                        CloseHandle(tokenDuplicate);
                        return true;
                    }
                }
            }
        }
        if (token != IntPtr.Zero)
            CloseHandle(token);
        if (tokenDuplicate != IntPtr.Zero)
            CloseHandle(tokenDuplicate);
        return false;
    }

    /// <summary>
    /// Undoes the impersonation.
    /// </summary>
    public void undoImpersonation()
    {
        impersonationContext.Undo();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            //Dispose of managed resources.
            if (impersonationContext != null)
            {
                this.undoImpersonation();
                impersonationContext.Dispose();
                impersonationContext = null;
            }
        }
    }
    #endregion

}