using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Hosting;
using System.IO;
using ClosedXML.Excel;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Net;

public partial class task_djsi : System.Web.UI.Page
{
    public static int nSec = 5;
    public static string sID = DateTime.Now.ToString("ddMMyyyyHHmmss");
    public static string SharePathUpFile = Human_Function.SharePathUpFile();
    public static string SharePathDomain = Human_Function.SharePathDomain();
    public static bool IsLogonSharePath = Human_Function.IsLogonSharePath();
    public static string SharePathUser = ConfigurationSettings.AppSettings["SharePathUser"].ToString();
    public static string SharePathPassword = ConfigurationSettings.AppSettings["SharePathPassword"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetLog_SyncDJSI(new StringBuilder().Append("Start Run Task DJSI " + sID));
            //if (impersonateValidUser(SharePathUser, SharePathDomain, SharePathPassword))
            //{
            #region Copy File and Read File
            Get_File();
            #endregion
            //}
            //else
            //{
            //    SetLog_SyncDJSI(new StringBuilder().Append("Login Failed " + sID));
            //}
            SetLog_SyncDJSI(new StringBuilder().Append("End Run Task DJSI " + sID));
        }
        catch (Exception ex)
        {
            SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + sID + " Error : " + ex.Message));
        }
        finally
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "window.opener = 'Self';window.open('','_parent',''); window.close();", true);
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

    public void Get_File_V2()
    {
        var sPathSave = "UploadFiles/DJSI/";
        string sPathTo = HttpContext.Current.Server.MapPath("~/" + sPathSave);
        if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/") + sPathSave.Replace("/", "\\")))
        {
            System.IO.Directory.CreateDirectory(sPathTo);
        }

        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qFileName = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 44);
        string sFilename = qFileName != null ? qFileName.sDescription : "";

        if (Directory.Exists(HttpContext.Current.Server.MapPath("./ReadFileHR/")))
        {
            SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + sID + " Has path ReadFileHR"));
            DirectoryInfo d = new DirectoryInfo(Server.MapPath("./ReadFileHR/"));

            var qFile = d.GetFiles(sFilename).FirstOrDefault();
            if (qFile != null)
            {
                sPathTo = sPathTo + "/" + sID + (!string.IsNullOrEmpty(sFilename) ? sFilename.Split('.')[0] : "");
                qFile.CopyTo(sPathTo);

                SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + sID + " Path to : " + sPathTo));

                System.Threading.Thread.Sleep(nSec * 1000);
                Sync_Data(sPathTo);
            }
            else
            {
                SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + sID + " Don't have File"));
            }
        }
        else
        {
            SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + sID + " Don't have Path File ReadFileHR"));
        }
    }

    public static void Get_File()
    {
        var sPathSave = "UploadFiles/DJSI/";
        string sPathTo = HttpContext.Current.Server.MapPath("~/" + sPathSave);
        if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/") + sPathSave.Replace("/", "\\")))
        {
            System.IO.Directory.CreateDirectory(sPathTo);
        }

        PTTGC_HumanEntities db = new PTTGC_HumanEntities();
        var qFileName = db.TM_MasterData_Sub.FirstOrDefault(w => w.nSubID == 44);
        string sFilename = qFileName != null ? qFileName.sDescription : "";

        var sUrl = HttpContext.Current.Request.Url;
        var sPathFrom = sUrl.AbsoluteUri.Replace(sUrl.PathAndQuery, "/") + (sUrl.Host != "localhost" ? sUrl.Segments[1] : "") + "ReadFileHR/" + sFilename;
        //var sPathFrom = sUrl.AbsoluteUri.Replace(sUrl.PathAndQuery, "/") + (sUrl.Host != "localhost" ? sUrl.Segments[1] : "") + "UploadFiles/" + sFilename;
        sPathTo = sPathTo + sID + "." + (!string.IsNullOrEmpty(sFilename) ? sFilename.Split('.')[1] : "xlxs");

        SetLog_SyncDJSI(new StringBuilder().Append("Task DJSI " + sID + " Path from : " + sPathFrom + ", Path to : " + sPathTo));

        WebClient wb = new WebClient();
        wb.DownloadFile(sPathFrom, sPathTo);

        System.Threading.Thread.Sleep(nSec * 1000);
        Sync_Data(sPathTo);
    }

    public static void Sync_Data(string sPathFile)
    {
        if (File.Exists(sPathFile))
        {
            PTTGC_HumanEntities db = new PTTGC_HumanEntities();

            #region Declare List
            var lstCom = db.TB_Company.Where(w => w.IsActive && !w.IsDel && (w.IsSync ?? false)).ToList();
            var lstComID = lstCom.Select(s => s.nCompanyID).ToList();
            var lstComCode = lstCom.Select(s => s.sCompanyCode).ToList();
            var lstDJSI_Old = db.TB_Sync_Item.ToList();
            #endregion

            #region Define Variable
            int nRow = 1;
            var lstDJSI = new List<c_djsi>();
            var lstError = new List<string>();

            string sCol1 = "nitem";
            string sCol2 = "Company";
            string sCol3 = "Month";
            string sCol4 = "Year";
            string sCol5 = "Male";
            string sCol6 = "Female";
            #endregion

            #region Get Data from Excel
            var workbook = new XLWorkbook(sPathFile);
            foreach (var SheetItem in workbook.Worksheets)
            {
                foreach (IXLRow row in SheetItem.Rows())
                {
                    #region Declare Value
                    var sVal1 = (row.Cell(1).Value + "").Trim().Replace(" ", "");
                    if (sVal1 == "") { break; }
                    var sVal2 = (row.Cell(2).Value + "").Trim().Replace(" ", "");
                    var sVal3 = (row.Cell(3).Value + "").Trim().Replace(" ", "");
                    var sVal4 = (row.Cell(4).Value + "").Trim().Replace(" ", "");
                    var sVal5 = (row.Cell(5).Value + "").Trim().Replace(" ", "").Replace(",", "");
                    var sVal6 = (row.Cell(6).Value + "").Trim().Replace(" ", "").Replace(",", "");

                    var lstColumnError = new List<string>();
                    #endregion

                    if (nRow == 1)
                    {
                        #region Head
                        if (sVal1 != sCol1 || sVal2 != sCol2 || sVal3 != sCol3 || sVal4 != sCol4 || sVal5 != sCol5 || sVal6 != sCol6)
                        {
                            if (sVal1 != sCol1) lstColumnError.Add(sCol1 + " : " + sVal1);
                            if (sVal2 != sCol2) lstColumnError.Add(sCol2 + " : " + sVal2);
                            if (sVal3 != sCol3) lstColumnError.Add(sCol3 + " : " + sVal3);
                            if (sVal4 != sCol4) lstColumnError.Add(sCol4 + " : " + sVal4);
                            if (sVal5 != sCol5) lstColumnError.Add(sCol5 + " : " + sVal5);
                            if (sVal6 != sCol6) lstColumnError.Add(sCol6 + " : " + sVal6);
                            lstError.Add("Header incorrect : " + (string.Join(", ", lstColumnError)));
                        }
                        #endregion
                    }
                    else
                    {
                        #region Data                  
                        var cDJSI = new c_djsi();

                        var nItem = CommonFunction.ParseIntNull(sVal1);
                        var sComCode = lstComCode.Contains(sVal2) ? sVal2 : "";
                        var nComID = !string.IsNullOrEmpty(sComCode) ? lstCom.First(w => w.sCompanyCode == sComCode).nCompanyID : 0;
                        var nMonth = CommonFunction.ParseIntNull(sVal3);
                        var nYear = CommonFunction.ParseIntNull(sVal4);
                        var dDate = nMonth.HasValue && nYear.HasValue ? new DateTime(nYear.Value, nMonth.Value, 1) : (DateTime?)null;
                        var nMale = CommonFunction.ParseDecimalNull(sVal5);
                        var nFemale = CommonFunction.ParseDecimalNull(sVal6);

                        if (nItem.HasValue) { cDJSI.nItem = nItem.Value; } else { lstColumnError.Add(sCol1 + " : " + sVal1); }
                        if (nComID > 0) { cDJSI.nComID = nComID; } else { lstColumnError.Add(sCol2 + " : " + sVal2); }
                        if (!nMonth.HasValue) { lstColumnError.Add(sCol3 + " : " + sVal3); }
                        if (!nYear.HasValue) { lstColumnError.Add(sCol4 + " : " + sVal4); }
                        if (dDate.HasValue) { cDJSI.dDate = dDate.Value; }
                        if (nMale.HasValue) { cDJSI.nMale = nMale.Value; } else if (!nFemale.HasValue) { lstColumnError.Add(sCol5 + " : " + sVal5); }
                        if (nFemale.HasValue) { cDJSI.nFemale = nFemale.Value; } else if (!nMale.HasValue) { lstColumnError.Add(sCol6 + " : " + sVal6); }

                        if (lstDJSI_Old.Any(w => w.nCompanyID == nComID && w.dDate == dDate && w.nItem == nItem))
                        {
                            lstError.Add("Data duplication : nCompanyID = " + nComID + ", dDate = " + dDate.Value.ToString("dd/MM/yyyy") + ", nItem = " + nItem);
                        }
                        else if (lstColumnError.Any()) { lstError.Add("Data incorrect : " + (string.Join(", ", lstColumnError))); }
                        else { lstDJSI.Add(cDJSI); }
                        #endregion
                    }
                    nRow++;
                }
            }
            #endregion

            #region Save            
            if (lstDJSI.Any())
            {
                #region Save TB_Sync
                var nUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.nUserID : 0;
                var qTB_Sync = new TB_Sync();
                qTB_Sync.sSysFileName = sPathFile.Split('\\')[sPathFile.Split('\\').Length - 1];
                qTB_Sync.sPath = "UploadFiles/DJSI/";
                qTB_Sync.nCreateBy = nUserID;
                qTB_Sync.dCreate = DateTime.Now;
                db.TB_Sync.Add(qTB_Sync);
                db.SaveChanges();

                var nID = qTB_Sync.nID;
                #endregion

                #region Save TB_Sync_Item

                #region Script Insert
                string sInsert = @"INSERT INTO TB_Sync_Item
                               ([nID]
                               ,[nCompanyID]
                               ,[dDate]
                               ,[nItem]
                               ,[nMale]
                               ,[nFemale])
                                VALUES
                               (" + nID + @"--<nID, int,>
                               ,{0}--<nCompanyID, int,>
                               ,'{1}'--<dDate, date,>
                               ,{2}--<nItem, int,>
                               ,{3}--<nMale, decimal(18,2),>
                               ,{4})--<nFemale, decimal(18,2),>)" + Environment.NewLine;
                #endregion

                StringBuilder sb = new StringBuilder();
                foreach (var item in lstDJSI)
                {
                    sb.Append(string.Format(sInsert, item.nComID, item.dDate.ToString("yyyy-MM-dd"), item.nItem, item.nMale, item.nFemale));
                }

                CommonFunction.ExecuteNonQuery(sb + "");
                #endregion
            }
            #endregion

            #region Save Log Error
            if (lstError.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Task DJSI " + sID + " Error");
                foreach (var item in lstError)
                {
                    sb.Append(Environment.NewLine + item);
                }
                SetLog_SyncDJSI(sb);
            }
            #endregion
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

    #region Class
    public class c_djsi
    {
        public int nComID { get; set; }
        public DateTime dDate { get; set; }
        public int nItem { get; set; }
        public decimal nMale { get; set; }
        public decimal nFemale { get; set; }
    }

    public class c_error
    {
        public int nItem { get; set; }
        public List<string> lstError { get; set; }
    }
    #endregion
}