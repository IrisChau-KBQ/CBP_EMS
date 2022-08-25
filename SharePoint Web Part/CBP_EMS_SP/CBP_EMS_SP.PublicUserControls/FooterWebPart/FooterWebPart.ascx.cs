using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Common;

namespace CBP_EMS_SP.PublicUserControls.FooterWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class FooterWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public FooterWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FooterCopyRight.Text = "© " + (DateTime.Now).Year.ToString() + " " + SPFunctions.LocalizeUI("Footer_Message", "CyberportEMS_Common"); 
        }
    }
}
