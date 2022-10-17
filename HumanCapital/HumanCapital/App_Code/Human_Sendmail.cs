using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for Human_Sendmail
/// </summary>
public class Human_Sendmail
{
    public Human_Sendmail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Mail
    public static bool SendNetMail(string sfrom, string sto, string scc, string subject, string message, List<string> lstFile)
    {
        bool IsSend = false;
        string sMessage_Error = "";
        try
        {
            if (!Human_Function.IsUseRealMail())//Demo
            {
                //message += " to : " + sto + " cc : " + scc;
                sfrom = Human_Function.SystemMail(); //ConfigurationSettings.AppSettings["DemoMail_Sender"].ToString();
                sto = Human_Function.DemoMail_Reciever();//ConfigurationSettings.AppSettings["DemoMail_Reciever"].ToString();
                scc = "";
            }

            System.Net.Mail.MailMessage oMsg = new System.Net.Mail.MailMessage();

            // TODO: Replace with sender e-mail address. 
            oMsg.From = new System.Net.Mail.MailAddress(sfrom, "PTTGC Human");

            // TODO: Replace with recipient e-mail address.
            string[] Arrsto = sto.Split(',');
            for (int i = 0; i < Arrsto.Length; i++)
            {
                if (!string.IsNullOrEmpty(Arrsto[i]))
                {
                    oMsg.To.Add(Arrsto[i]);
                }
            }

            // Add CC Mail
            string[] ArrsCc = scc.Split(',');
            for (int i = 0; i < ArrsCc.Length; i++)
            {
                if (!string.IsNullOrEmpty(ArrsCc[i]))
                {
                    oMsg.CC.Add(ArrsCc[i]);
                }
            }

            // TODO: Replace with subject.
            oMsg.Subject = subject;

            // SEND IN HTML FORMAT (comment this line to send plain text).
            oMsg.IsBodyHtml = true;

            oMsg.Body = "<HTML><BODY>" + message + "</BODY></HTML>";

            if (lstFile.Any())
            {
                lstFile.ForEach(f =>
                {
                    if (!string.IsNullOrEmpty(f))
                    {
                        string[] arr = f.Split('/');
                        string filename = arr[arr.Length - 1];
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(f);
                        attachment.Name = filename;  // set name here
                                                     //msg.Attachments.Add(attachment);
                                                     // oMsg.Attachments.Add(new System.Net.Mail.Attachment(f));
                        oMsg.Attachments.Add(attachment);
                    }
                });
            }
            // ADD AN ATTACHMENT.
            // TODO: Replace with path to attachment.
            // String sFile = @"D:\FTP\username\Htdocs\Hello.txt";
            // String sFile = @sfilepath;
            // MailAttachment oAttch = new MailAttachment(sfilepath, MailEncoding.Base64);
            // oMsg.Attachments.Add(oAttch);

            // TODO: Replace with the name of your remote SMTP server.
            /**/

            // TODO: Replace with the name of your remote SMTP server.
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Port = 25;
            smtp.Host = Human_Function.smtpmail();//ConfigurationSettings.AppSettings["smtpmail"];
            smtp.Send(oMsg);
            oMsg = null;
            IsSend = true;//return true;
        }
        catch (Exception e)
        {
            IsSend = false; //return false;
            sMessage_Error = e.Message;
        }

        LogMailSend(sto, scc, subject, message, IsSend, sMessage_Error);

        return IsSend;
    }

    public static void LogMailSend(string sTo, string sCc, string sSubject, string sMessage, bool IsSend, string sMessage_Error)
    {
        // Encode the content for storing in Sql server.
        string htmlEncoded = WebUtility.HtmlEncode(sMessage);

        // Decode the content for showing on Web page.
        //string original = WebUtility.HtmlDecode(htmlEncoded);

        PTTGC_HumanEntities db = new PTTGC_HumanEntities();

        var tb = new TM_LogMail();
        tb.sTo = sTo;
        tb.sCc = sCc;
        tb.sSubject = sSubject;
        tb.sMessage = sMessage;
        tb.IsSend = IsSend;
        tb.sMessage_Error = !string.IsNullOrEmpty(sMessage_Error) ? sMessage_Error : null;
        tb.dSend = DateTime.Now;

        db.TM_LogMail.Add(tb);
        db.SaveChanges();
    }

    public static string GET_TemplateEmail()
    {
        return @"<div id=':km' class='ii gt adP adO'>
                <div id=':l9' class='a3s aXjCH m15f05c377e26ea4b'>
                    <u></u>
                    <div style='background: #f9f9f9'>
                        <div style='background-color: #f9f9f9'>

                            <div style='margin: 0px auto; /* max-width: 630px; */background: transparent;'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent;' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; /* padding: 40px 0px */'>
                                                <div style='font-size: 1px; line-height: 12px'>&nbsp;{4}</div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style='max-width: 640px; margin: 0 auto; border-radius: 4px; overflow: hidden'>
                                <div style='margin: 0px auto; max-width: 640px; background: #ffffff'>
                                    <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: #ffffff' align='center' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 40px 70px'>
                                                    <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                        <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>

                                                                            <h2 style='font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-weight: 500; font-size: 20px; color: #4f545c; letter-spacing: 0.27px'>{0}</h2>
                                                                            {1}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                {2}
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding-top: 15px;'>
                                                                        <p style='font-size: 1px; margin: 0px auto; border-top: 1px solid #dcddde; width: 100%'></p>
                                                                    </td>
                                                                </tr>
                                                                <tr style='display:none;'>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #747f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 13px; line-height: 16px; text-align: left'>
                                                                            <p>
                                                                                {3}
                                                                            </p>
                                                                            <p>
                                                                                Best regards,<br>
                                                                                Technology Management System Team
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div style='margin: 0px auto; max-width: 640px; background: transparent'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 0px'>
                                                <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                    <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                        <tbody>
                                                            <tr>
                                                                <td style='word-break: break-word; font-size: 0px'>
                                                                    <div style='font-size: 1px; line-height: 12px'>&nbsp;</div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>";
    }
    #endregion

}