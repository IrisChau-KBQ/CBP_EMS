<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Registration.ascx.cs" Inherits="CBP_EMS_SP.RegistrationWebPart.Registration.Registration" %>
<link href="/_layouts/15/Styles/CBP_Styles/customLogin.css" rel="stylesheet" />

<asp:Panel ID="pnlRegistration" runat="server">
   
    <div class="main">
        <div class="login-form border-gray registrationfrm">
            <div class="head">
                <img src="/_layouts/15/Images/CBP_Images/sign up icon.png" class="head-logo" alt="head-logo" />
            </div>

            <div class="logo-wrapper">
                <h1 class="form-heading">Sign Up</h1>
                <p class="form-tagline">Sign up Entrepreneurship Management System to apply Cyberport Incubation Programme, CCMF or CASP! </p>
            </div>

            <div class="form">
                <asp:TextBox ID="txtRegEmail" autocomplete="off" runat="server" class="input" placeholder="Email" />
                <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="Email can not be empty." ControlToValidate="txtRegEmail" runat="server" />
                <asp:TextBox ID="txtRegPassword" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="Password" /><br />
                <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="Password can not be empty." ControlToValidate="txtRegPassword" runat="server" />
                <asp:TextBox ID="txtRegPasswordConfirm" TextMode="Password" autocomplete="o ff" runat="server" class="input" placeholder="Retype Password" /><br />
                <asp:CompareValidator ErrorMessage="Password Mismatch" ControlToValidate="txtRegPasswordConfirm" Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ControlToCompare="txtRegPassword" runat="server" />
                <div class="submit">
                    <asp:LinkButton ID="lnkRegistration" ValidationGroup="signupVGroup" OnClick="lnkRegistration_Click" Text="Create New Account" runat="server" CssClass="submit-btn btn-green" />
                </div>
            </div>
            <div style="padding: 12px 0;">
                <p class="text-danger" id="UserCustomerrorReg" runat="server"></p>
            </div>
        </div>
    </div>


</asp:Panel>
