using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data.Models;
using Microsoft.SharePoint.IdentityModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.PublicUserControls.ChangePassword
{
    [ToolboxItemAttribute(false)]
    public partial class ChangePassword : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ChangePassword()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            //txtsignup.InnerText = Localize("Registration_signup");
            //txtsignupdetails.InnerText = Localize("Registration_signuodetails");
            txtoldpassreg.Text = Localize("error_Changepass_oldpass");
            txtpass.Text = Localize("Error_Registration_Password");
            //txtpassvalid.Text = Localize("Error_Registration_Passwordconfirmation");
            txtoldpassreg.Text = Localize("Error_Registration_Password");
            txtpassconfirm.Text = Localize("Error_Registration_Pass_validation");
            txtpassreg1.Text = Localize("Error_Registration_Pass_validation");
            lnkChangePssword.Text = Localize("btn_ChangePassword");
            txtpassmsg.InnerText = Localize("Registration_Passwordmessage");
            txtvalidmsg.Text = Localize(" Error_Registration_Passwordconfirmation");
            if (!Page.IsPostBack) { }
        }

        protected void lnkChangePssword_Click(object sender, EventArgs e)
        {
           
            SPFunctions objSPFunctions = new SPFunctions();
            
  String SqlMembershipProvider = string.Empty;
            using (var db = new CBP_EMS_SP.Data.Models.CyberportEMS_EDM())
            {

                SqlMembershipProvider = db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;
            }
            MembershipUser Username = Membership.Providers[SqlMembershipProvider].GetUser(objSPFunctions.GetCurrentUser(), false);
            bool IsLocked = false;
            if (Username != null)
            {
                IsLocked = Username.IsLockedOut;
                if (!IsLocked)
                {
                    //Validating user with password
                    IsLocked = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, objSPFunctions.GetCurrentUser(), txtoldpass.Text);
                    if (!IsLocked) // User not valid
                    {
                        UserCustomerrorReg.InnerText = Localize("Error_login_invalid_pass");
                    }
                    else
                    {

                        Username.ChangePassword(txtoldpass.Text.Trim(), txtRegPassword.Text.Trim());
                        bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, Username.Email, txtRegPassword.Text);
                        if (status)
                        {
                            UserCustomerrorReg.InnerText = Localize("ChangePassword");
                            UserCustomerrorReg.Style.Add("color", "green");
                           // Response.Redirect("~/SitePages/Home.aspx");
                        }


                    }
                }
                else
                    UserCustomerrorReg.InnerText = Localize("Error_login_acc_lock");

            }

        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
    }
}
