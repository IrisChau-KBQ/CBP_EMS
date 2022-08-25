<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="CBP_EMS_SP.LoginWebPart.Login.Login" %>
<link href="/_layouts/15/Styles/CBP_Styles/customLogin.css" rel="stylesheet" />

<asp:Panel ID="pnlLogin" runat="server">
    <div id="customLoginMain" class="loginbox">
        <div class="login-form border-no">
            <div class="logo-wrapper">
                <img src="/_layouts/15/Images/CBP_Images/logo 1.png" alt="" />
                <p class="tagline">Entrepreneurship Management System</p>
            </div>
            <div class="form">
                <%--<asp:TextBox runat="server" ID="txtLoginUserName" CssClass="input" placeholder="<%$Resources:CPB_EMS_Localization, p%>" />--%>
                <asp:TextBox runat="server" ID="txtLoginUserName" CssClass="input" placeholder="Email" />
                <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" ValidationGroup="LoginVGroup" ErrorMessage="Email can not be empty." ControlToValidate="txtLoginUserName" runat="server" />
                <asp:TextBox TextMode="Password" runat="server" CssClass="input" ID="txtLoginPassword" placeholder="Password" />
                <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" ValidationGroup="LoginVGroup" ErrorMessage="Password can not be empty." ControlToValidate="txtLoginPassword" runat="server" />
                <label>
                    <span> Password should contain small and capital letter, numbers, symbol with length of 8-20 characters</span>
                </label>
                <%--<a href="~/SitePages/ForgotPassword.aspx" class="copy-right">Forgot your password?</a>--%>
                <asp:LinkButton Text="Forgot your password???" PostBackUrl="~/SitePages/ForgotPassword.aspx" CssClass="copy-right" runat="server" />
                <div class="login-btn-wrp">
                    <label for="rememberme" class="rememberme">   
                        <input type="checkbox" id="txtLoginRememberMe" />Remember Me</label>
                    <asp:LinkButton ID="lnk_LoginUser" OnClick="lnk_LoginUser_Click" Text="Login" runat="server" ValidationGroup="LoginVGroup" CssClass="login-btn btn-green" />
                </div>
                <div class="submit">
                    <asp:LinkButton ID="lnkRegistrationNav" PostBackUrl="~/SitePages/Registration.aspx" CssClass="registration-btn btn-blue" Text="Registration" runat="server" />
                </div>
            </div>
            <div style="padding: 12px 0;">
                <p class="text-danger" id="UserCustomerrorLogin" runat="server"></p>
            </div>
        </div>
    </div>
</asp:Panel>
