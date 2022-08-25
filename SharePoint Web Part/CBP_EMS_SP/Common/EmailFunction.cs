using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using CBP_EMS_SP.Data.Models;
using Microsoft.SharePoint;

namespace CBP_EMS_SP.Common
{
    public class CBPEmail
    {
        public static string GetEmailTemplate(string TemplateName)
        {
            string EmailContent = "";
            try
            {
                using (var db = new CyberportEMS_EDM())
                {
                    EmailContent = db.TB_EMAIL_TEMPLATE.FirstOrDefault(x => x.Email_Template.ToLower() == TemplateName.ToLower()).Email_Template_Content;
                }
                //string TemplateFolder = SPUtility.GetVersionedGenericSetupPath(@"TEMPLATE\Styles\ApplicationAttachement\", 15);
                //string TemplateFileName = "";
                //switch (TemplateName.ToLower())
                //{
                //    case "registration":
                //        TemplateFileName = "Tpl_Registration.html";
                //        break;
                //    case "forgotpassword":
                //        TemplateFileName = "Tbl_ForgotPassword.html";
                //        break;
                //    default:
                //        break;
                //}
                //if (TemplateFileName.Length > 0)
                //{
                //    EmailContent = System.IO.File.ReadAllText(TemplateFolder + TemplateFileName);
                //}
            }
            catch (Exception ex)
            {
                EmailContent = "";
            }
            return EmailContent;
        }
        public static int SendMail(string toAddress, string subject, string body)
        {
            int isEmailSend = 0;
            IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();

            try
            {

                using (var db = new CyberportEMS_EDM())
                {
                    objTbParams = db.TB_SYSTEM_PARAMETER;

                    string UserSharepointEmailSend = objTbParams.FirstOrDefault(x => x.Config_Code == "UserSharepointEmailSend").Value; //= WebConfigurationManager.AppSettings["IsInDebug"];
                    string IsInDebug = objTbParams.FirstOrDefault(x => x.Config_Code == "ApplicationIsInDebug").Value; //= WebConfigurationManager.AppSettings["IsInDebug"];
                    string DebugEmail = objTbParams.FirstOrDefault(x => x.Config_Code == "ApplicationDebugEmailSentTo").Value; //= WebConfigurationManager.AppSettings["DebugEmailTo"];
                    string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                    WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;
                    body = body.Replace("@@WebsiteUrl", WebsiteUrl);
                    if (IsInDebug == "1")
                    {
                        body = "(" + toAddress + ")<br/>" + body;
                        toAddress = DebugEmail;
                        subject = "Debug Mode :" + subject;

                    }
                    if (!string.IsNullOrEmpty(UserSharepointEmailSend) && UserSharepointEmailSend == "0")
                    {
                        string senderID = objTbParams.FirstOrDefault(x => x.Config_Code == "EmailSettingFromAddress").Value; //= WebConfigurationManager.AppSettings["EmailFrom"];
                        string senderPassword = objTbParams.FirstOrDefault(x => x.Config_Code == "EmailSettingEmailPassword").Value; // WebConfigurationManager.AppSettings["EmailPassword"]; // sender password here…
                        string EmailHostAdder = objTbParams.FirstOrDefault(x => x.Config_Code == "EmailSettingHostAddress").Value;//WebConfigurationManager.AppSettings["EmailHostAddress"]
                        string EmailHostPort = objTbParams.FirstOrDefault(x => x.Config_Code == "EmailSettingHostPort").Value;//WebConfigurationManager.AppSettings["EmailHostPort"]
                        string EnableSsl = objTbParams.FirstOrDefault(x => x.Config_Code == "EmailSettingEnableSsl").Value;//WebConfigurationManager.AppSettings["EnableSsl"]


                        SmtpClient smtp = new SmtpClient
                        {
                            Host = EmailHostAdder, // smtp server address here…
                            Port = Convert.ToInt32(EmailHostPort),
                            EnableSsl = Convert.ToBoolean(EnableSsl),

                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                            Timeout = 30000,
                        };
                        MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                    }
                    else
                    {
                        SPSecurity.RunWithElevatedPrivileges(
                         delegate ()
                         {
                             using (SPSite site = new SPSite(
                               SPContext.Current.Site.ID,
                               SPContext.Current.Site.Zone))
                             {
                                 using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                                 {
                                     SPUtility.SendEmail(web, false, false,
                                                               toAddress,
                                                               subject,
                                                               body);
                                 }
                             }
                         });
                    }

                    isEmailSend = 1;
                }
            }
            catch (SmtpException ex)
            {
                isEmailSend = 0;
            }
            catch (Exception e) {
                isEmailSend = 0;
            }
            return isEmailSend;
        }

    }
}
