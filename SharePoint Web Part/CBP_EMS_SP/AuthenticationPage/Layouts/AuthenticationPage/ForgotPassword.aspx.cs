using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using CBP_EMS_SP.Common;
using System.Web.Security;
using System.Linq;
using CBP_EMS_SP.Data.Models;

namespace CBP_EMS_SP.Page
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            txtheading.InnerText = Localize("Forget_password_heading");
            txtsubheading.InnerText = Localize("Forgot_pass_subheading");
            txtemailid.Text = Localize("Error_Registration_email");
            txtemailformat.Text = Localize("Forgot_pass_emailaddress");
            lnkRegistration.Text= Localize("Forgot_pass_btnsubmit");
            txtRegEmail.Attributes.Add("placeholder", Localize("Placeholder_Email"));
            if (!IsPostBack) { }
        }

        protected void lnkRegistration_Click(object sender, EventArgs e)
        {
            try
            {

                UserCustomerrorReg.Style.Add("color", "red");

                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtRegEmail.Text.Trim()))
                {
                    String SqlMembershipProvider = string.Empty;
                    using (var db = new CyberportEMS_EDM())
                    {

                        SqlMembershipProvider = db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;
                    }
                    MembershipUser Username = Membership.Providers[SqlMembershipProvider].GetUser(txtRegEmail.Text.Trim(), false);
                    if (Username == null)
                    { UserCustomerrorReg.InnerText = Localize("Forgot_pass_emaildublication"); }
                    else
                    {
                        using (var db = new CyberportEMS_EDM())
                        {
                            TB_RESET_PASSWORD objRequestPassword = db.TB_RESET_PASSWORD.FirstOrDefault(x => x.Email.ToLower() == txtRegEmail.Text.Trim().ToLower());
                            if (objRequestPassword == null)
                            {
                                objRequestPassword = new TB_RESET_PASSWORD()
                                {
                                    Created_Date = DateTime.Now,
                                    Email = txtRegEmail.Text.Trim(),
                                    Email_Type = "user",
                                };
                                db.TB_RESET_PASSWORD.Add(objRequestPassword);
                            }
                            else
                            {
                                objRequestPassword.Created_Date = DateTime.Now;
                            }
                            db.SaveChanges();
                            objRequestPassword = db.TB_RESET_PASSWORD.FirstOrDefault(x => x.Email.ToLower() == txtRegEmail.Text.Trim().ToLower());
                            string strEmailContent = CBPEmail.GetEmailTemplate("forgotpassword");
                            strEmailContent = strEmailContent.Replace("@@ForgotPasswordLink", objRequestPassword.Reset_Password_ID.ToString());
                            CBPEmail.SendMail(txtRegEmail.Text.Trim(), Localize("Mail_Reset_Password"), strEmailContent);
                            UserCustomerrorReg.InnerText = Localize("Forgot_pass_successfull_msg");
                            UserCustomerrorReg.Style.Add("color", "green");
                        }
                    }

                }
                else
                {
                    UserCustomerrorReg.InnerText = Localize("Registration_Invalid_email");
                }
            }
            catch (Exception ex)
            {
                UserCustomerrorReg.InnerText = ex.Message;
            }

        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
    }
}
