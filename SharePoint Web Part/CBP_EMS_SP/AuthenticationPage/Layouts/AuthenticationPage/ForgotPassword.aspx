<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CBP_EMS_SP.Page.ForgotPassword" MasterPageFile="../CBP_EMS_SP_UserPages/EMS-Public.master" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <style>
        
        .emscontent .login-form .input{
  margin-top:0;
}
    </style>
    <!-----start-main---->
    <div class="main">
        <!---728x90--->
        <div class="login-form border-gray forgotpasswordfrm">

            <div class="head" style="top: -19%;">
                <img src="/_layouts/15/Images/CBP_Images/Forget Password.png" alt="head-logo" class="head-logo" />
            </div>

            <div class="logo-wrapper">
                <h1 class="form-heading" id="txtheading" runat="server"></h1>
                <p class="form-tagline" id="txtsubheading" runat="server" style="letter-spacing:2px"></p>
            </div>

            <div class="form" style="padding-top:0px!important">
                <asp:TextBox ID="txtRegEmail" autocomplete="off" runat="server" class="input"  />
                <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtemailid" ControlToValidate="txtRegEmail" runat="server" />
               <asp:RegularExpressionValidator  Display="Dynamic"  ErrorMessage="" ID="txtemailformat" CssClass="text-danger"  ControlToValidate="txtRegEmail" runat="server" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"/>
                 <div class="submit">
                    <asp:LinkButton ID="lnkRegistration" ValidationGroup="signupVGroup" OnClick="lnkRegistration_Click" Text="" runat="server" CssClass="submit-btn btn-green" />
                </div>
            </div>
            <div style="padding: 12px 0;">
                <p id="UserCustomerrorReg" runat="server" class="text-danger"></p>
            </div>
        </div>
    </div>

</asp:Content>
