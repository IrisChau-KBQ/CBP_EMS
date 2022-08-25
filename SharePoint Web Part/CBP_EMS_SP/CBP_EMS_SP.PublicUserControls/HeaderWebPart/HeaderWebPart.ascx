<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderWebPart.ascx.cs" Inherits="CBP_EMS_SP.PublicUserControls.HeaderWebPart.HeaderWebPart" %>
<script type="text/javascript" src="/_layouts/15/Styles/CBP_Styles/cookies.js"></script>
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
    .line-height{
            line-height: 1.33;
    }
    .imglogo-wrpr
    {
        padding:11px 11px;
    }
    .hr {
    border: 5px solid #145DAA;
}
    *::-ms-backdrop, .line-height{
            line-height: 1.3;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        $('#<%= LanguageEng.ClientID %>,#<%= LinkButton1.ClientID %>').click(function () {
          
            $.cookie("CBP_User_Language", "en-US", { expires: 7, path: '/' });
            return true;
        });

        $('#<%= LanguageHK.ClientID %>,#<%= LinkButton2.ClientID %>').click(function () {
           
            $.cookie("CBP_User_Language", "zh-HK",{ expires: 7, path: '/' });
            return true;
        });

        $('#<%= LanguageCH.ClientID %>,#<%= LinkButton3.ClientID %>').click(function () {
          
            $.cookie("CBP_User_Language", "zh-CN", { expires: 7, path: '/' });
            return true;
        });

    });
</script>

<%--<script type="text/javascript">
    function OnSelectionChange(value) {
        alert(value);
        var today = new Date();
        var oneYear = new Date(today.getTime() + 365 * 24 * 60 * 60 * 1000);
        var url = window.location.href;
        document.cookie = "lcid=" + value + ";path=/;expires=" + oneYear.toGMTString();

        window.location.href = url;
    }
</script>--%>
<asp:Panel ID="pnlLoggedIn" runat="server">
    <header class="row theme-blue full-width loggedinheader">

        <p class="col-md-8 main-heading"><a href="/SitePages/Home.aspx" style="color: white;">Entrepreneurship Management System</a> </p>
        <nav class="col-md-4">
            <ul class="navigation txt-rt">
                <li class="navigation-el lang_change" style="width: 15%; position: relative;" id="lang_change" runat="server">
                    <img src="/_layouts/15/Images/CBP_Images/Globe.png" class="" alt="logo" height="24" id="img-globe">
                    <ul class="lang-nav" id="lang-bar">
                        <li style="display: block !important">
                            <asp:LinkButton Text="Eng" ID="LanguageEng" OnClick="LanguageEng_Click" runat="server" Style="width: 60px; text-align: center;" class="active" />
                        </li>
                        <li style="display: block !important">
                            <asp:LinkButton Text="繁" ID="LanguageHK" OnClick="LanguageHK_Click" runat="server" Style="width: 60px; text-align: center;" class="active" />
                        </li>
                        <li style="display: block !important">
                            <asp:LinkButton Text="简" ID="LanguageCH" OnClick="LanguageCH_Click" runat="server" Style="width: 60px; text-align: center;" class="active" />
                        </li>
                    </ul>

                </li>
                <li class="navigation-el" style="text-align: center; margin-left: 2%; margin-right: 2%">
                    <asp:Label ID="lblUserName" runat="server" /></li>
                <li class="navigation-el logoutarrow" style="width: 10%; text-align: left;">
                    <img src="/_layouts/15/Images/CBP_Images/downword menu.png" style="padding-top: 5px;" alt="logo" height="16">
                    <ul style="margin-top: 11px!important">
                        <li style="display: block !important">
                            <a href="/_layouts/closeConnection.aspx?loginasanotheruser=true">Sign Out</a></li>
                    </ul>
                </li>
            </ul>
        </nav>

    </header>
</asp:Panel>
<asp:Panel ID="pnlNotLoggedIn" runat="server">
    <%--    <script> var IsFullMask = 0;</script>--%>
    <div class="hr"></div>
    <header class="header-section txt-rt">

        <div class="imglogo-wrpr">
            <img src="/_layouts/15/Images/CBP_Images/Globe.png" alt="logo-icon" class="logo-icon" height="22">
        </div>
        <nav class="nav-list" style="display: block!important">
            <ul class="">
                <li>
                    <asp:LinkButton Text="Eng" ID="LinkButton1" OnClick="LanguageEng_Click" CssClass="line-height" runat="server"  />
                </li>
                <li>
                    <asp:LinkButton Text="繁" ID="LinkButton2" OnClick="LanguageHK_Click" runat="server" />
                </li>
                <li>
                    <asp:LinkButton Text="简" ID="LinkButton3" OnClick="LanguageCH_Click" runat="server" />
                </li>
            </ul>
        </nav>
    </header>
    <div class="float-lt outer_logo">
        <a href="/SitePages/Home.aspx" style="color: white;">
            <asp:Image CssClass="logo" runat="server" ImageUrl="/_layouts/15/Images/CBP_Images/logo 1.png" Style="width: 55%" /></a>
        <%--<img src="/_layouts/15/Images/CBP_Images/logo 1.png" alt="logo" class="logo" style="width:55%">--%>
    </div>

    <%-- <header class="table theme-blue full-width">
        <div class="table-row">
            <nav class="table-col" style="padding:6px;">
            </nav>
        </div>
    </header>--%>
</asp:Panel>
<div class="loading" style="align-content: center">
    <img src="/_layouts/15/Images/CBP_Images/ajax-loader.gif" alt="" />
</div>
<%--<script type="text/javascript"  src="../_layouts/15/STYLES/CBP_Styles/jquery.js"></script>--%>


<script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal progressive");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            HideProgress();
        }, 100);
    }
    function HideProgress() {
        setTimeout(function () {
            $('.progressive').remove();
            var loading = $(".loading");
            loading.hide();

        }, 600);
    }
    $('form').submit(function () {
        ShowProgress();

    });
</script>
