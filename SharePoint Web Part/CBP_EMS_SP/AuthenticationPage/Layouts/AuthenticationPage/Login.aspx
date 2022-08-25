<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Assembly Name="Microsoft.SharePoint.IdentityModel, Version=15.0.0.0, 
Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, 
PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Import Namespace="Microsoft.SharePoint.WebControls" %>

<%@ Register TagPrefix="SharePoint"
    Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI"
    Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CBP_EMS_SP.Page.Login" MasterPageFile="../CBP_EMS_SP_UserPages/EMS-Public.master" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <style>
        .theme-page, .background-theme {
            background-color: #ffffff !important;
        }

        .outer_logo {
            display: none;
        }

        .emscontent .login-form .input {
            margin-top: 0;
        }
    </style>
    <asp:Panel ID="pnlLogin" runat="server">
        <div id="customLoginMain" class="loginbox">
            <div class="login-form border-no">
                <div class="logo-wrapper">
                    <img src="/_layouts/15/Images/CBP_Images/logo 1.png" alt="" />
                    <p class="tagline" id="txtheading" runat="server" style="margin-top: 10px"></p>
                </div>
                <div class="form">
                    <asp:TextBox runat="server" ID="txtLoginUserName" CssClass="input" />
                    <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" ValidationGroup="LoginVGroup" ErrorMessage="" ID="txterrorusename"  ControlToValidate="txtLoginUserName" runat="server" />
                    <asp:TextBox TextMode="Password" runat="server" CssClass="input passwordtext" ID="txtLoginPassword" />
                    <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" ValidationGroup="LoginVGroup" ErrorMessage="" ID="texterrorlogin" ControlToValidate="txtLoginPassword" runat="server" />
                    <div>
                        <label class="css-label" style="font-size:smaller; color: #999999;" id="txtpasswordmsg" runat="server">
                         
                        </label>
                    </div>
                    <%--<asp:LinkButton Text="Forgot your password?" PostBackUrl="~/_layouts/15/AuthenticationPage/ForgotPassword.aspx" CssClass="copy-right" runat="server" /> --%>
                    <div class="login-btn-wrp row">
                        <%--<label for="rememberme" class="rememberme">
                            <input type="checkbox" id="txtLoginRememberMe" />Remember Me</label>--%>
                        <div class="col-md-6">
                            <input type="checkbox" name="checkboxG5" id="txtLoginRememberMe" class="css-checkbox" checked="checked" style="display:none">
                            <label for="txtLoginRememberMe" class="css-label chi rememberme" style="display:none" id="txtrememberme" runat="server"></label>
                        
                        <asp:LinkButton ID="lnk_forgot_password" Text="Forgot your password ?" PostBackUrl="~/_layouts/15/AuthenticationPage/ForgotPassword.aspx" CssClass="copy-right" runat="server" />
                        
                        </div>
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnk_LoginUser" OnClick="lnk_LoginUser_Click" Text="Login" runat="server" ValidationGroup="LoginVGroup" CssClass="login-btn btn-green" />
                        </div>

                    </div>
                    <div class="submit">
                        <asp:LinkButton Text="Registration" id="btnregistration" PostBackUrl="~/_layouts/15/AuthenticationPage/Registration.aspx" CssClass="registration-btn btn-blue" runat="server" />
                    </div>
                </div>
                <div style="padding: 12px 0;">
                    <p class="text-danger" id="UserCustomerrorLogin" runat="server"></p>
                </div>
<a href="https://cpas.icac.hk/EN/Home" target="_blank"><img src="/_layouts/15/Images/CBP_Images/ICAC.png"/></a>
<%--                                 <div id="ICAC">
                    <asp:ImageButton id="btnICAC" runat="server" ImageUrl="/_layouts/15/Images/CBP_Images/ICAC.png" OnClick="btnICAC_Click" />
                </div>--%>
            </div>
        </div>
    </asp:Panel>
    <script>   
    $(window).load(function () {
       
        $(".passwordtext").keyup(function (event) {
            
            if (event.keyCode == 13) {
             
                document.getElementById('<%= lnk_LoginUser.ClientID %>').click();
           
               
        }
    });
    });
</script>
</asp:Content>
