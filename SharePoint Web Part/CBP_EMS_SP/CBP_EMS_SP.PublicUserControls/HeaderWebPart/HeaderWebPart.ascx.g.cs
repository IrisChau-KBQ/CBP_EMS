﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBP_EMS_SP.PublicUserControls.HeaderWebPart {
    using System.Web.UI.WebControls.Expressions;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Text;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.SessionState;
    using System.Configuration;
    using Microsoft.SharePoint;
    using System.Web;
    using System.Web.DynamicData;
    using System.Web.Caching;
    using System.Web.Profile;
    using System.ComponentModel.DataAnnotations;
    using System.Web.UI.WebControls;
    using System.Web.Security;
    using System;
    using Microsoft.SharePoint.Utilities;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint.WebControls;
    using System.CodeDom.Compiler;
    
    
    public partial class HeaderWebPart {
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton LanguageEng;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton LanguageHK;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton LanguageCH;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl lang_change;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Label lblUserName;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Panel pnlLoggedIn;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton LinkButton1;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton LinkButton2;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton LinkButton3;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Panel pnlNotLoggedIn;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebPartCodeGenerator", "12.0.0.0")]
        public static implicit operator global::System.Web.UI.TemplateControl(HeaderWebPart target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlLanguageEng() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.LanguageEng = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.Text = "Eng";
            @__ctrl.ID = "LanguageEng";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("Style", "width: 60px; text-align: center;");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "active");
            @__ctrl.Click -= new System.EventHandler(this.LanguageEng_Click);
            @__ctrl.Click += new System.EventHandler(this.LanguageEng_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlLanguageHK() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.LanguageHK = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.Text = "繁";
            @__ctrl.ID = "LanguageHK";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("Style", "width: 60px; text-align: center;");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "active");
            @__ctrl.Click -= new System.EventHandler(this.LanguageHK_Click);
            @__ctrl.Click += new System.EventHandler(this.LanguageHK_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlLanguageCH() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.LanguageCH = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.Text = "简";
            @__ctrl.ID = "LanguageCH";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("Style", "width: 60px; text-align: center;");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "active");
            @__ctrl.Click -= new System.EventHandler(this.LanguageCH_Click);
            @__ctrl.Click += new System.EventHandler(this.LanguageCH_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControllang_change() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("li");
            this.lang_change = @__ctrl;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "navigation-el lang_change");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "width: 15%; position: relative;");
            @__ctrl.ID = "lang_change";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                    <img src=""/_layouts/15/Images/CBP_Images/Globe.png"" class="""" alt=""logo"" height=""24"" id=""img-globe"">
                    <ul class=""lang-nav"" id=""lang-bar"">
                        <li style=""display: block !important"">
                            "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl1;
            @__ctrl1 = this.@__BuildControlLanguageEng();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        </li>\r\n                        <li style=\"display: bloc" +
                        "k !important\">\r\n                            "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl2;
            @__ctrl2 = this.@__BuildControlLanguageHK();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        </li>\r\n                        <li style=\"display: bloc" +
                        "k !important\">\r\n                            "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl3;
            @__ctrl3 = this.@__BuildControlLanguageCH();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        </li>\r\n                    </ul>\r\n\r\n                "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Label @__BuildControllblUserName() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lblUserName = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lblUserName";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Panel @__BuildControlpnlLoggedIn() {
            global::System.Web.UI.WebControls.Panel @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Panel();
            this.pnlLoggedIn = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "pnlLoggedIn";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
    <header class=""row theme-blue full-width loggedinheader"">

        <p class=""col-md-8 main-heading""><a href=""/SitePages/Home.aspx"" style=""color: white;"">Entrepreneurship Management System</a> </p>
        <nav class=""col-md-4"">
            <ul class=""navigation txt-rt"">
                "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControllang_change();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                <li class=\"navigation-el\" style=\"text-align: center; margin-lef" +
                        "t: 2%; margin-right: 2%\">\r\n                    "));
            global::System.Web.UI.WebControls.Label @__ctrl2;
            @__ctrl2 = this.@__BuildControllblUserName();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"</li>
                <li class=""navigation-el logoutarrow"" style=""width: 10%; text-align: left;"">
                    <img src=""/_layouts/15/Images/CBP_Images/downword menu.png"" style=""padding-top: 5px;"" alt=""logo"" height=""16"">
                    <ul style=""margin-top: 11px!important"">
                        <li style=""display: block !important"">
                            <a href=""/_layouts/closeConnection.aspx?loginasanotheruser=true"">Sign Out</a></li>
                    </ul>
                </li>
            </ul>
        </nav>

    </header>
"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlLinkButton1() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.LinkButton1 = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.Text = "Eng";
            @__ctrl.ID = "LinkButton1";
            @__ctrl.Click -= new System.EventHandler(this.LanguageEng_Click);
            @__ctrl.Click += new System.EventHandler(this.LanguageEng_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlLinkButton2() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.LinkButton2 = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.Text = "繁";
            @__ctrl.ID = "LinkButton2";
            @__ctrl.Click -= new System.EventHandler(this.LanguageHK_Click);
            @__ctrl.Click += new System.EventHandler(this.LanguageHK_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlLinkButton3() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.LinkButton3 = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.Text = "简";
            @__ctrl.ID = "LinkButton3";
            @__ctrl.Click -= new System.EventHandler(this.LanguageCH_Click);
            @__ctrl.Click += new System.EventHandler(this.LanguageCH_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Image @__BuildControl__control2() {
            global::System.Web.UI.WebControls.Image @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Image();
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.CssClass = "logo";
            @__ctrl.ImageUrl = "/_layouts/15/Images/CBP_Images/logo 1.png";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("Style", "width: 55%");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.Panel @__BuildControlpnlNotLoggedIn() {
            global::System.Web.UI.WebControls.Panel @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Panel();
            this.pnlNotLoggedIn = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "pnlNotLoggedIn";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
    
    <div class=""hr""></div>
    <header class=""header-section txt-rt"">

        <div class=""imglogo-wrpr"">
            <img src=""/_layouts/15/Images/CBP_Images/Globe.png"" alt=""logo-icon"" class=""logo-icon"" height=""22"">
        </div>
        <nav class=""nav-list"" style=""display: block!important"">
            <ul class="""">
                <li>
                    "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl1;
            @__ctrl1 = this.@__BuildControlLinkButton1();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                </li>\r\n                <li>\r\n                    "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl2;
            @__ctrl2 = this.@__BuildControlLinkButton2();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                </li>\r\n                <li>\r\n                    "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl3;
            @__ctrl3 = this.@__BuildControlLinkButton3();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                </li>\r\n            </ul>\r\n        </nav>\r\n    </header>\r\n    <d" +
                        "iv class=\"float-lt outer_logo\">\r\n        <a href=\"/SitePages/Home.aspx\" style=\"c" +
                        "olor: white;\">\r\n            "));
            global::System.Web.UI.WebControls.Image @__ctrl4;
            @__ctrl4 = this.@__BuildControl__control2();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("</a>\r\n        \r\n    </div>\r\n\r\n    \r\n"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControlTree(global::CBP_EMS_SP.PublicUserControls.HeaderWebPart.HeaderWebPart @__ctrl) {
            global::System.Web.UI.WebControls.Panel @__ctrl1;
            @__ctrl1 = this.@__BuildControlpnlLoggedIn();
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(@__ctrl1);
            global::System.Web.UI.WebControls.Panel @__ctrl2;
            @__ctrl2 = this.@__BuildControlpnlNotLoggedIn();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__ctrl.SetRenderMethodDelegate(new System.Web.UI.RenderMethod(this.@__Render__control1));
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__Render__control1(System.Web.UI.HtmlTextWriter @__w, System.Web.UI.Control parameterContainer) {
            @__w.Write(@"
<script type=""text/javascript"" src=""/_layouts/15/Styles/CBP_Styles/cookies.js""></script>
<style>
    .nav-list {
        /*top:34px;*/
        top: 24px;
    }

        .nav-list .active {
            background-color: #80C343 !important;
        }

    .lang-nav {
        width: 180px;
        display: block;
        background-color: white;
        left: 0 !important;
        height: auto;
    }

        .lang-nav li {
            display: block;
            color: white;
            text-align: center !important;
        }

            .lang-nav li:hover a {
                color: white !important;
            }

    #lang-bar {
        display: none;
    }

    .lang_change:hover #lang-bar {
        display: block !important;
    }

    .modal {
        position: fixed;
        top: 0;
        left: 0;
        background: rgba(0,0,0,0.4);
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }

    .loading {
        /*font-family: Arial;
        font-size: 10pt;*/
        /*border: 5px solid #67CFF5;*/
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        /*background-color: White;*/
        z-index: 999;
        top: 35%;
        left: 50%;
    }
</style>
<script type=""text/javascript"">
    $(document).ready(function () {
        $('#");
    @__w.Write( LanguageEng.ClientID );

            @__w.Write(",#");
                                 @__w.Write( LinkButton1.ClientID );

            @__w.Write("\').click(function () {\r\n          \r\n            $.cookie(\"CBP_User_Language\", \"en" +
                    "-US\", { expires: 7, path: \'/\' });\r\n            return true;\r\n        });\r\n\r\n    " +
                    "    $(\'#");
    @__w.Write( LanguageHK.ClientID );

            @__w.Write(",#");
                                @__w.Write( LinkButton2.ClientID );

            @__w.Write("\').click(function () {\r\n           \r\n            $.cookie(\"CBP_User_Language\", \"z" +
                    "h-HK\",{ expires: 7, path: \'/\' });\r\n            return true;\r\n        });\r\n\r\n    " +
                    "    $(\'#");
    @__w.Write( LanguageCH.ClientID );

            @__w.Write(",#");
                                @__w.Write( LinkButton3.ClientID );

            @__w.Write("\').click(function () {\r\n          \r\n            $.cookie(\"CBP_User_Language\", \"zh" +
                    "-CN\", { expires: 7, path: \'/\' });\r\n            return true;\r\n        });\r\n\r\n    " +
                    "});\r\n</script>\r\n\r\n\r\n");
            parameterContainer.Controls[0].RenderControl(@__w);
            @__w.Write("\r\n");
            parameterContainer.Controls[1].RenderControl(@__w);
            @__w.Write(@"
<div class=""loading"" style=""align-content: center"">
    <img src=""/_layouts/15/Images/CBP_Images/ajax-loader.gif"" alt="""" />
</div>



<script type=""text/javascript"">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass(""modal progressive"");
            $('body').append(modal);
            var loading = $("".loading"");
            loading.show();
            HideProgress();
        }, 100);
    }
    function HideProgress() {
        setTimeout(function () {
            $('.progressive').remove();
            var loading = $("".loading"");
            loading.hide();

        }, 600);
    }
    $('form').submit(function () {
        ShowProgress();

    });
</script>
");
        }
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
