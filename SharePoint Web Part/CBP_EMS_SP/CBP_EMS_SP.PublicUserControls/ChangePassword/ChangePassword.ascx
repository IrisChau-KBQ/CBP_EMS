<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="CBP_EMS_SP.PublicUserControls.ChangePassword.ChangePassword" %>
<asp:Panel ID="PnluserLeftBar" runat="server">
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
               <asp:TextBox ID="txtoldpass" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="OldPassword" />
                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtoldpassreg" ControlToValidate="txtoldpass" runat="server"  />
                    <asp:RegularExpressionValidator Display="Dynamic" CssClass="text-danger" ValidationGroup="signupVGroup" ErrorMessage="" ID="txtpassreg1" ControlToValidate="txtoldpass" runat="server" ValidationExpression="(?=^.{8,20}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-@#$%^&amp;*()_+!}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$"/>
                    <asp:TextBox ID="txtRegPassword" TextMode="Password" autocomplete="off" runat="server" class="input" placeholder="NewPassword" />
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
                    <asp:LinkButton ID="lnkChangePssword" ValidationGroup="signupVGroup" OnClick="lnkChangePssword_Click" Text="" runat="server" CssClass="submit-btn btn-green" />
                </div>
            </div>
            <div style="padding: 12px 0;">
                <p id="UserCustomerrorReg" runat="server" class="text-danger"></p>
            </div>
        </div>
    </div>

</asp:Panel>

