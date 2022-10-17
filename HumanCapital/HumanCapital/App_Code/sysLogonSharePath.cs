using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for sysLogonSharePath
/// </summary>
public class sysLogonSharePath
{
    public sysLogonSharePath()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool IsLogonSharePaht()
    {
        bool cCheck = false;
        string sIsLogon = WebConfigurationManager.AppSettings["IsLogonSharePath"] + "";
        if (sIsLogon == "Y")
            cCheck = true;
        else
            cCheck = false;

        return cCheck;
    }

    public static bool Logon()
    {
        if (IsLogonSharePaht())
        {
            if (impersonateValidUser(ConfigurationSettings.AppSettings["SharePathUser"] + "", ConfigurationSettings.AppSettings["SharePathDomain"] + "", ConfigurationSettings.AppSettings["SharePathPassword"] + ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
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