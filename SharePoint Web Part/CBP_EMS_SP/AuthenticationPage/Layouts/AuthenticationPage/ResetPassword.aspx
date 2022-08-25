<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="CBP_EMS_SP.Page.ResetPassword" MasterPageFile="../CBP_EMS_SP_UserPages/EMS-Public.master" %>

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
        <div class="login-form border-gray registrationfrm">

            <div class="head" style="top: -16%;">
                <img src="/_layouts/15/Images/CBP_Images/Reset Password.png" alt="head-logo" class="head-logo" />
            </div>

            <div class="logo-wrapper">
                <h1 class="form-heading" runat="server" id="lblheading"></h1>
                <p class="form-tagline" runat="server" id="txtsubheading">  </p>
            </div>

            <div class="form">
                <asp:TextBox ID="txt_UserEmail" ReadOnly="true" CssClass="input" runat="server" />
                <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtpasserror" ControlToValidate="txtRegPassword" runat="server" />
                <asp:TextBox ID="txtRegPassword" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="Password" />
                <asp:RegularExpressionValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ID="txtpasswordvalidation" ErrorMessage="" ControlToValidate="txtRegPassword" runat="server" ValidationExpression="(?=^.{8,20}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-@#$%^&amp;*()_+!}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$"/>                                
                <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtpassconfirm" ControlToValidate="txtRegPasswordConfirm" runat="server" />
                <asp:TextBox ID="txtRegPasswordConfirm" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="Retype Password" />
                <asp:CompareValidator ErrorMessage="Password Mismatch" ID="errormismatch" ControlToValidate="txtRegPasswordConfirm" Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ControlToCompare="txtRegPassword" runat="server" />
                <div>
                        <label class="css-label" style="color:#595A5C;" id="passvalidation" runat="server">
                      
                        </label>
                    </div>
                <div class="submit">
                    <asp:LinkButton ID="lnkReset" ValidationGroup="signupVGroup" OnClick="lnkReset_Click" Text="Reset" runat="server" CssClass="submit-btn btn-green" />
                </div>

                <asp:Label runat="server" ID="lbErrorMsg" Visible="false" ForeColor="red"></asp:Label>
            </div>
            <div style="padding: 12px 0;">
                <p class="text-danger" id="UserCustomerrorReg" runat="server"></p>
            </div>
        </div>
        <!--//End-login-form-->
        <!---728x90--->
    </div>
  <asp:Panel ID="userPopup" Visible="false" runat="server">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width:50%">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para" style="font-size:17px; color:red;" runat="server" id="txtpassexpired"></p>

            <div class="table full-width">
                <div class="form-group" style="text-align:center;">
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="" runat="server" CssClass="bluetheme"  style="border:none; padding:10px;"/>
                </div>
                        </div>
        </div>
    </div>
</asp:Panel>
</asp:Content>

