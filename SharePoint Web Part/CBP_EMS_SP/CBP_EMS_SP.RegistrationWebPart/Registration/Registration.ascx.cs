using CBP_EMS_SP.Common;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.Configuration;
using System.Web.Security;
using Microsoft.SharePoint.IdentityModel;

using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Administration.Claims;

namespace CBP_EMS_SP.RegistrationWebPart.Registration
{
    [ToolboxItemAttribute(false)]
    public partial class Registration : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public Registration()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void lnkRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtRegEmail.Text) && !string.IsNullOrEmpty(txtRegPassword.Text))
                {
                    if(txtRegEmail.Text.EndsWith("cyberport.hk"))
                    {
                        UserCustomerrorReg.InnerText = "Cannot end with @cyberport.hk";
                    }
                    else
                    {
                        CreateUser(txtRegEmail.Text.Trim(), txtRegPassword.Text.Trim());
                    }
                   

                }
                else {
                    UserCustomerrorReg.InnerText = "Incorrect Email format";
                }
            }
            catch (Exception ex)
            {

                UserCustomerrorReg.InnerText = ex.Message;
            }
        }


        private void CreateUser(string UserEmail, string Password)
        {
            try
            {
                MembershipCreateStatus UserState;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string spUserRoleGroup = (String)WebConfigurationManager.AppSettings["SPExternalUserGroupName"];
                    SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    SPWeb site = siteCollection.OpenWeb();
                    site.AllowUnsafeUpdates = true;
                    SPGroup oGroup = site.SiteGroups[spUserRoleGroup];


                    // Attempt to create the user
                    Membership.CreateUser(txtRegEmail.Text, txtRegPassword.Text, txtRegEmail.Text
                                            , "SecurityQuestion"
                        , "SecurityAnswer"
                        , true
                        , out UserState);

                    // Validate user creation
                    bool IsValid = false;
                    string Message = "";
                    #region User account creation status handling
                    switch (UserState)
                    {
                        case MembershipCreateStatus.Success:
                            IsValid = true;
                            break;

                        case MembershipCreateStatus.DuplicateEmail:
                            Message = "Email already exists.";
                            break;

                        case MembershipCreateStatus.DuplicateUserName:
                            Message = "Email already exists.";
                            break;

                        case MembershipCreateStatus.InvalidAnswer:
                            Message = "Invalid Security answer.";
                            break;

                        case MembershipCreateStatus.InvalidEmail:
                            Message = "Invalid Email address.";
                            break;

                        case MembershipCreateStatus.InvalidPassword:
                            Message = "Password is not secure.";
                            break;

                        case MembershipCreateStatus.InvalidQuestion:
                            Message = "Invalid security question";
                            break;

                        default:
                            Message = "internal error";
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

                        SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                        if (mgr != null)
                        {
                            SPUser oUser = site.EnsureUser(UserEmail);
                            oGroup.AddUser(oUser);
                        }
                        UserCustomerrorReg.InnerText = "User Created successfully";

                        //SPUtility.SendEmail(SPContext.Current.Web, true, false, UserEmail, "RegistrationSuccess");

                    }
                });

            }
            catch (Exception ex)
            {
                Membership.DeleteUser(UserEmail);
                UserCustomerrorReg.InnerText = ex.Message;
            }
        }

    }
}
