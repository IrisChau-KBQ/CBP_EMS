using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Common;
using Microsoft.ApplicationServer.Caching;
using System.Globalization;
using System.Threading;

using System.Web.Configuration;
namespace CBP_EMS_SP.PublicUserControls.UserLeftSidebar
{
    [ToolboxItemAttribute(false)]
    public partial class UserLeftSidebar : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public UserLeftSidebar()
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

            MenuApplicant.Visible = false;
            MenuAdUsers.Visible = false;
            MenuVettingMembers.Visible = false;
            ActivateMenu();
            //this.Page.MasterPageFile = "~/MasterPages/seattle.master";
        }

        protected void ActivateMenu()
        {
            Common.SPFunctions spFun = new Common.SPFunctions();

            if (spFun.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup) || spFun.CurrentUserIsInGroup("Collaborator"))
            {
                MenuApplicant.Visible = true;
            }
            else if (spFun.CurrentUserIsInGroup("CPIP Coordinator",true) || spFun.CurrentUserIsInGroup("CPIP BDM", true)
                 || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true)
                  || spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true)
                || spFun.CurrentUserIsInGroup("CASP Coordinator", true) || spFun.CurrentUserIsInGroup("CASP BDM", true)
                || spFun.CurrentUserIsInGroup("CASP Senior Manager", true) 
                || spFun.CurrentUserIsInGroup("Finance Accountant", true)
                || spFun.CurrentUserIsInGroup("Finance Senior Manger", true)
                || spFun.CurrentUserIsInGroup("CFO", true)
                || spFun.CurrentUserIsInGroup("CEO", true)
                )
            {
                MenuAdUsers.Visible = true;
            }
            else if (spFun.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"], true))
            {
                MenuVettingMembers.Visible = true;
            }
        }
    }
}

