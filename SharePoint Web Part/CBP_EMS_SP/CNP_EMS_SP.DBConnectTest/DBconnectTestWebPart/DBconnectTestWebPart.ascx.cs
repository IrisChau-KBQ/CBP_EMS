using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data.Models;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.DatabaseProvider;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.Office.Server.ActivityFeed;
using Microsoft.SharePoint.Client;

namespace CNP_EMS_SP.DBConnectTest.DBconnectTestWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class DBconnectTestWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DBconnectTestWebPart()
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

        protected void btnDBconnectTest_Click(object sender, EventArgs e)
        {
           var TemplateName = "forgotpassword";
           //string strEmailContent = CBPEmail.GetEmailTemplate("forgotpassword");

           CyberportEMS_EDM db = new CyberportEMS_EDM();
           var EmailContent = db.TB_EMAIL_TEMPLATE.FirstOrDefault(x => x.Email_Template.ToLower() == TemplateName.ToLower()).Email_Template_Content;
           
           var a = db.TB_CCMF_APPLICATION.ToList();
            foreach(var b in db.TB_CCMF_APPLICATION){

            }
           lbltxt.Text = EmailContent;

           SPSite mySite = SPControl.GetContextSite(Context);
           SPWeb myWeb = SPControl.GetContextWeb(Context);
           SPSite site = SPContext.Current.Site;
           SPWeb web = site.OpenWeb();

            

           
            
        }
    }
}
