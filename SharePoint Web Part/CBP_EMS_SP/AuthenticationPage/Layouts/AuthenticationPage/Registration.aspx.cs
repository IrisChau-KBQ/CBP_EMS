using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using CBP_EMS_SP.Common;
using System.Web.Security;
using System.Web.Configuration;
using Microsoft.SharePoint.Administration.Claims;
using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.Utilities;
using System.Linq;
namespace CBP_EMS_SP.Page
{
    public partial class Registrations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            txtsignup.InnerText = Localize("Registration_signup");
            txtsignupdetails.InnerText = Localize("Registration_signuodetails");
            txtemailid.Text = Localize("Error_Registration_email");
            txtpass.Text = Localize("Error_Registration_Password");
            txtpassconfirm.Text = Localize("Error_Registration_Passwordconfirmation");
            txtpassvalid.Text = Localize("Error_Registration_Pass_validation");
            lnkRegistration.Text = Localize("Registration_newbtn");
            txtpassmsg.InnerText = Localize("Registration_Passwordmessage");
            txtvalidmsg.Text = Localize(" Error_Registration_Passwordconfirmation");




            if (!IsPostBack)
            {

            }
        }

        protected void lnkRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtRegEmail.Text) && !string.IsNullOrEmpty(txtRegPassword.Text))
                {
                    if (txtRegEmail.Text.ToLower().Trim().EndsWith("@cyberport.hk"))
                    {
                        UserCustomerrorReg.InnerText = Localize("Registration_Email_restriction");
                    }
                    else
                    {
                        CreateUser(txtRegEmail.Text.Trim(), txtRegPassword.Text.Trim());
                    }

                }
                else
                {
                    UserCustomerrorReg.InnerText = Localize("Registration_Email_Format");
                }
            }
            catch (Exception ex)
            {
                UserCustomerrorReg.InnerText = ex.Message;
            }
        }

        private void CreateUser(string UserEmail, string Password)
        {
            bool IsValid = false;
            SPWeb site = null;
            SPGroup oGroup = null;
            try
            {
                MembershipCreateStatus UserState;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string spUserRoleGroup = (String)WebConfigurationManager.AppSettings["SPExternalUserGroupName"];
                    //spUserRoleGroup = "Vetting Team";
                    SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    site = siteCollection.OpenWeb();
                    site.AllowUnsafeUpdates = true;
                    oGroup = site.SiteGroups[spUserRoleGroup];

                    // spUserRoleGroup = "CBP EMS DEV Vetting Team";

                    // Attempt to create the user
                    Membership.CreateUser(txtRegEmail.Text, txtRegPassword.Text, txtRegEmail.Text
                                            , "SecurityQuestion"
                        , "SecurityAnswer"
                        , true
                        , out UserState);

                    // Validate user creation
                    string Message = "";
                    #region User account creation status handling
                    switch (UserState)
                    {
                        case MembershipCreateStatus.Success:
                            IsValid = true;
                            break;

                        case MembershipCreateStatus.DuplicateEmail:
                            Message = Localize("Registration_email_dublication");
                            break;

                        case MembershipCreateStatus.DuplicateUserName:
                            Message = Localize("Registration_email_dublication");
                            break;

                        case MembershipCreateStatus.InvalidAnswer:
                            Message = Localize("Registration_invalid_security_ans");
                            break;

                        case MembershipCreateStatus.InvalidEmail:
                            Message = Localize("Registration_Invalid_email");
                            break;

                        case MembershipCreateStatus.InvalidPassword:
                            Message = Localize("Registration_Pass_notsecure");
                            break;

                        case MembershipCreateStatus.InvalidQuestion:
                            Message = Localize("Registration_invalid_question");
                            break;

                        default:
                            Message = Localize("Registration_internal_error");
                            break;
                    }
                    #endregion

                    if (!IsValid)
                    {
                        UserCustomerrorReg.InnerText = Message;
                    }

                    if (IsValid)
                    {
                        Roles.AddUserToRole(UserEmail, spUserRoleGroup);

                        var userinfo = SPUtility.ResolvePrincipal(site, UserEmail, SPPrincipalType.User, SPPrincipalSource.All, null, true);

                        SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                        if (mgr != null)
                        {

                            SPUser oUser = site.EnsureUser(userinfo.LoginName);
                            oUser.Email = UserEmail;
                            oUser.Name = UserEmail;
                            oGroup.AddUser(oUser);
                        }
                        string strEmailContent = CBPEmail.GetEmailTemplate("registration");
                        int IsEmailed = CBPEmail.SendMail(UserEmail, Localize("Mail_Regestration"), strEmailContent);

                    }
                });
            }
            catch (Exception ex)
            {
                IsValid = false;
                Membership.DeleteUser(UserEmail);
                IsValid = false;
                Membership.DeleteUser(UserEmail);
                SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                if (mgr != null)
                {
                    SPUser oUser = site.EnsureUser(UserEmail);
                    oGroup.RemoveUser(oUser);
                }
                UserCustomerrorReg.InnerText = ex.Message;
            }
            if (IsValid)
            {
                bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, UserEmail, Password);
                if (!status)
                {
                    UserCustomerrorReg.InnerText = Localize("Error_Registration_invalid_emailpass");
                }
                else
                {
                    Response.Redirect("~/SitePages/Home.aspx");
                }
            }
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
    }
}
