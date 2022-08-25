using CBP_EMS_SP.Common;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.ApplicationServer.Caching;

namespace CBP_EMS_SP.PublicUserControls.HeaderWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class HeaderWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        public HeaderWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            bool IsOuterPage = Context.Request.Url.ToString().ToLower().Contains("authenticationpage");
            if (!string.IsNullOrEmpty(new SPFunctions().GetCurrentUser()) && !IsOuterPage)
            {
                pnlLoggedIn.Visible = true;
                lblUserName.Text = new SPFunctions().GetCurrentUser();
                pnlNotLoggedIn.Visible = false;
                if (new SPFunctions().CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                {
                    lang_change.Visible = true;
                }
                else
                {
                    lang_change.Visible = false;
                }
            }
            else
            {
                pnlNotLoggedIn.Visible = true;
                pnlLoggedIn.Visible = false;
            }
            //if (Context.Request.Cookies["emsuserlanguage"] != null)
            //{
            //    string CultureName = Convert.ToString(Context.Request.Cookies["emsuserlanguage"].Value);
            //    if (CultureName == "en-US")
            //    {
            //        LinkButton1.CssClass = "active";
            //        LanguageEng.CssClass = "active";
            //    }
            //}
        }

        protected void LanguageEng_Click(object sender, EventArgs e)
        {

            //string macaddress = SPFunctions.GetMacAddress();
            //CacheManager objCommonFunctions = new CacheManager();
            //if (!string.IsNullOrEmpty(new SPFunctions().GetCurrentUser()))
            //{
            //    macaddress = new SPFunctions().GetCurrentUser();
            //}
            //if (objCommonFunctions.DefaultCache.GetCacheItem(macaddress) != null)
            //{

            //    objCommonFunctions.DefaultCache.Remove(macaddress);
            //}
            //DataCache emsuserlanguage = objCommonFunctions.DefaultCache;
            //emsuserlanguage.Add(macaddress, "en-US");
            //var CultureName = emsuserlanguage[macaddress];
            //CultureInfo culture = new CultureInfo(CultureName.ToString());
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;
            //Activelanguage(LinkButton1, sender);
            //Activelanguage(LanguageEng, sender);
            //Activelanguage(LinkButton2, sender);
            //Activelanguage(LanguageHK, sender);
            //Activelanguage(LinkButton3, sender);
            //Activelanguage(LanguageCH, sender);

            ////Context.Response.Redirect(Context.Request.Url.OriginalString);
            ////Context.Server.Transfer(Context.Request.Url.OriginalString);
        }

        protected void LanguageHK_Click(object sender, EventArgs e)
        {
            //string macaddress = SPFunctions.GetMacAddress();
            //CacheManager objCommonFunctions = new CacheManager();
            //if (!string.IsNullOrEmpty(new SPFunctions().GetCurrentUser()))
            //{
            //    macaddress = new SPFunctions().GetCurrentUser();
            //}
            //if (objCommonFunctions.DefaultCache.GetCacheItem(macaddress) != null)
            //{

            //    objCommonFunctions.DefaultCache.Remove(macaddress);
            //}
            //DataCache emsuserlanguage = objCommonFunctions.DefaultCache;
            //emsuserlanguage.Add(macaddress, "zh-HK");

            //var CultureName = emsuserlanguage[macaddress];
            //CultureInfo culture = new CultureInfo(CultureName.ToString());
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            //Activelanguage(LinkButton2, sender);
            //Activelanguage(LanguageHK, sender);
            //Activelanguage(LinkButton1, sender);
            //Activelanguage(LanguageEng, sender);
            //Activelanguage(LinkButton3, sender);
            //Activelanguage(LanguageCH, sender);

            ////Context.Response.Redirect(Context.Request.Url.OriginalString);
            ////Context.Server.Transfer(Context.Request.Url.OriginalString);
        }

        protected void LanguageCH_Click(object sender, EventArgs e)
        {
            ////Context.Response.Cookies.Add(new System.Web.HttpCookie("emsuserlanguage", "zh-CN") { Expires = DateTime.Now.AddDays(15) });
            ////Context.Request.Cookies.Add(new System.Web.HttpCookie("emsuserlanguage", "zh-CN") { Expires = DateTime.Now.AddDays(15) });
            ////            Thread.CurrentThread.CurrentCulture =
            ////CultureInfo.CreateSpecificCulture("zh-CN");
            ////            Thread.CurrentThread.CurrentUICulture = new
            ////                CultureInfo("zh-CN");
            //string macaddress = SPFunctions.GetMacAddress();
            //CacheManager objCommonFunctions = new CacheManager();
            //if (!string.IsNullOrEmpty(new SPFunctions().GetCurrentUser()))
            //{
            //    macaddress = new SPFunctions().GetCurrentUser();
            //}
            //if (objCommonFunctions.DefaultCache.GetCacheItem(macaddress) != null)
            //{

            //    objCommonFunctions.DefaultCache.Remove(macaddress);
            //}
            //DataCache emsuserlanguage = objCommonFunctions.DefaultCache;
            //emsuserlanguage.Add(macaddress, "zh-CN");
            //var CultureName = emsuserlanguage[macaddress];
            //CultureInfo culture = new CultureInfo(CultureName.ToString());
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            //Activelanguage(LinkButton3, sender);
            //Activelanguage(LanguageCH, sender);
            //Activelanguage(LinkButton1, sender);
            //Activelanguage(LanguageEng, sender);
            //Activelanguage(LinkButton2, sender);
            //Activelanguage(LanguageHK, sender);

            ////Context.Response.Redirect(Context.Request.Url.OriginalString);
            ////Context.Server.Transfer(Context.Request.Url.OriginalString);
        }
        public void Activelanguage(LinkButton Id, object sender)
        {

            LinkButton btn = (LinkButton)sender;
            if (btn == Id)
            {
                Id.CssClass = "active";
            }
            else
            {
                Id.CssClass = "";
            }

        }

    }
}
