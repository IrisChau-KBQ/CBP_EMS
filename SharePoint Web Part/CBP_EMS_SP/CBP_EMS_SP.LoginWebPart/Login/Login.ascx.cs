using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Common;
using Microsoft.SharePoint.IdentityModel;

namespace CBP_EMS_SP.LoginWebPart.Login
{
    [ToolboxItemAttribute(false)]
    public partial class Login : WebPart
    {
        public object Response { get; private set; }

        public Login()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void lnk_LoginUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtLoginUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                {
                    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtLoginUserName.Text, txtLoginPassword.Text);
                    if (!status)
                    {
                        UserCustomerrorLogin.InnerText = "Invalid Email and Password";
                    }
                    else
                    {
                        Context.Response.Redirect("~/SitePages/Home.aspx");
                    }
                }
                else
                {
                    UserCustomerrorLogin.InnerText = "Invalid Email and Password";
                }
            }
            catch (Exception ex)
            {
                UserCustomerrorLogin.InnerText = ex.Message;
            }
        }

    }
}
