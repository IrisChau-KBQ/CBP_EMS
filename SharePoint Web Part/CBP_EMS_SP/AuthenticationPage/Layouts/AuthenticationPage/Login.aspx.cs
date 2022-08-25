using System;

using Microsoft.SharePoint.IdentityModel.Pages;
using Microsoft.SharePoint.IdentityModel;
using CBP_EMS_SP.Common;
using Microsoft.SharePoint;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.Configuration;
using Microsoft.SharePoint.Administration.Claims;
using Microsoft.SharePoint.Utilities;
using System.Net;
using System.Linq;
using System.Diagnostics;
using Microsoft.SharePoint.Mobile.Controls;
using System.Web.UI;

namespace CBP_EMS_SP.Page
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ClientScript.GetPostBackEventReference(this, string.Empty);
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            txtheading.InnerText = Localize("Login_heading");
            txterrorusename.Text = Localize("Error_Registration_email");
            texterrorlogin.Text = Localize("Error_Registration_Password");
            txtpasswordmsg.InnerText = Localize("Registration_Passwordmessage");
            txtrememberme.InnerText = Localize("Login_Remember_me");
            lnk_LoginUser.Text = Localize("Login");
            lnk_forgot_password.Text = Localize("Login_forgot_password");
            btnregistration.Text = Localize("Login_Registration");
            txtLoginUserName.Attributes.Add("placeholder", Localize("Placeholder_Email"));
            txtLoginPassword.Attributes.Add("placeholder", Localize("Placeholder_Password"));
          
            if (!IsPostBack)
            {



            }
        }

       

        protected void lnk_LoginUser_Click(object sender, EventArgs e)
        {
            try
            {

                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtLoginUserName.Text.Trim()) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                {
                    String SqlMembershipProvider = string.Empty;
                    using (var db = new CBP_EMS_SP.Data.Models.CyberportEMS_EDM())
                    {

                        SqlMembershipProvider = db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;
                    }
                    MembershipUser Username = Membership.Providers[SqlMembershipProvider].GetUser(txtLoginUserName.Text.Trim(), false);
                    bool IsLocked = false;
                    if (Username != null)
                    {
                        IsLocked = Username.IsLockedOut;
                        if (!IsLocked)
                        {
                            //Validating user with password
                            IsLocked = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtLoginUserName.Text.Trim(), txtLoginPassword.Text);
                            if (!IsLocked) // User not valid
                            {
                                UserCustomerrorLogin.InnerText = Localize("Error_login_invalid_pass");
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Context.Request.QueryString["ReturnUrl"]))
                                    Response.Redirect(Context.Request.QueryString["ReturnUrl"].ToString(), false);
                                else
                                    Response.Redirect("~/SitePages/Home.aspx");
                            }
                        }
                        else
                            UserCustomerrorLogin.InnerText = Localize("Error_login_acc_lock");

                    }
                    else UserCustomerrorLogin.InnerText = Localize("Error_login_email_notregistered");



                }
                else
                {
                    UserCustomerrorLogin.InnerText = Localize("Error_login_fill_passnemail");
                }
            }
            catch (Exception ex)
            {

                UserCustomerrorLogin.InnerText = ex.Message;
            }
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }

        public void btnICAC_Click(object sender, ImageClickEventArgs e)
        {
            Response.Write("<script>window.open ('https://cpas.icac.hk/EN/Home','_blank');</script>");

        }

    }
}


