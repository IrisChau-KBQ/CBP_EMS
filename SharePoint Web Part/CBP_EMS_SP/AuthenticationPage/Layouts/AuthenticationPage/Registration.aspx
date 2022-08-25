<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="CBP_EMS_SP.Page.Registrations" MasterPageFile="../CBP_EMS_SP_UserPages/EMS-Public.master" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
      <style>
        
        .emscontent .login-form .input{
  margin-top:0;
}
    </style>
    <asp:Panel ID="pnlRegistration" runat="server">
        <div class="main">
            <div class="login-form border-gray registrationfrm">
                <div class="head">
                    <img src="/_layouts/15/Images/CBP_Images/sign up icon.png" class="head-logo" alt="head-logo" />
                </div>

                <div class="logo-wrapper">
                  <h1 class="form-heading" runat="server" id="txtsignup"></h1>
                    <p class="form-tagline" runat="server" id="txtsignupdetails"> </p>
                </div>

                <div class="form">
                    <asp:TextBox ID="txtRegEmail" autocomplete="off" runat="server" class="input" placeholder="Email" />
                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtemailid" ControlToValidate="txtRegEmail" runat="server" />
                    <asp:TextBox ID="txtRegPassword" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="Password" />
                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtpass" ControlToValidate="txtRegPassword" runat="server"  />
                    <asp:RegularExpressionValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtpassvalid" ControlToValidate="txtRegPassword" runat="server" ValidationExpression="(?=^.{8,20}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-@#$%^&amp;*()_+!}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$"/>
                    <asp:TextBox ID="txtRegPasswordConfirm" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="Retype Password" />
                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtpassconfirm" ControlToValidate="txtRegPasswordConfirm" runat="server" />
                    <asp:CompareValidator ErrorMessage="" ID="txtvalidmsg" ControlToValidate="txtRegPasswordConfirm" Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ControlToCompare="txtRegPassword" runat="server" />
                    <div>
                        <label class="css-label" style="color:#595A5C;" id="txtpassmsg" runat="server">
                           
                        </label>
                    </div>
                    <div class="submit">
                        <asp:LinkButton ID="lnkRegistration" ValidationGroup="signupVGroup" OnClick="lnkRegistration_Click" Text="" runat="server" CssClass="submit-btn btn-green" />
                    </div>
                </div>
                <div style="padding: 12px 0;">
                    <p class="text-danger" id="UserCustomerrorReg" runat="server"></p>
                </div>
            </div>
        </div>
    </asp:Panel>



</asp:Content>
