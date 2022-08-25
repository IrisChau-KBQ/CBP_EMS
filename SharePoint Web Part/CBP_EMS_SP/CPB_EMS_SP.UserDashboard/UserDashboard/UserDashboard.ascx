<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.ascx.cs" Inherits="CPB_EMS_SP.UserDashboard.UserDashboard.UserDashboard" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>

<div class="page dashboard-box">
    <%--<header class="table theme-blue full-width">
        <div class="table-row">
            <p class="table-col main-heading">Entrepreneurship Management System</p>
            <nav class="table-col">
                <ul class="navigation txt-rt">
                    <li class="navigation-el">
                        <img src="images/_layouts/15/Images/CBP_Images/Globe.png" class="" alt="logo" height="24"></li>
                    <li class="navigation-el">Manson Chong</li>
                    <li class="navigation-el">
                        <img src="images/_layouts/15/Images/CBP_Images/downword menu.png" class="" alt="logo" height="16"></li>
                </ul>
            </nav>
        </div>
    </header>--%>

    <%--    <div class="float-lt" style="padding: 22px 30px;">
        <img src="images/_layouts/15/Images/CBP_Images/logo 1.png" alt="logo" class="logo">
    </div>--%>

    <div class="section">
        <div class="table full-width">
            <div class="table-row">
                <%--<div class="table-col" style="vertical-align: top; width: 20%;">
                    <ul class="left-navigation">
                        <li class="left-nav-el"><a class="active" href="#homesection">Home</a></li>
                        <li class="left-nav-el"><a href="#applicationsection">My Application</a></li>
                        <li class="left-nav-el"><a href="#tempsection">Temp</a></li>
                    </ul>
                </div>--%>

                <div class="table-col tabs-wrpr">
                    <asp:Repeater runat="server" ID="Rptr_IntakeProgram" OnItemDataBound="Rptr_IntakeProgram_ItemDataBound">
                        <HeaderTemplate>
                            <div id="homesection" class="tab-content">
                                <div class="table width94p">
                                    <div class="table-row">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="home-intake-list third-width">

                                <div class="apply-card">
                                    <div class="apply-head">
                                        <asp:Image ID="img_Program" runat="server" Height="100" />
                                    </div>

                                    <div class="apply-wpr">
                                        <h1 class="apply-card-heading">
                                            <asp:Label Text="text" runat="server" ID="lblprogramname" />
                                        </h1>
                                    </div>
                                     <div class="apply-wpr">
                                            <p class="apply-card--intake"><%= SPFunctions.LocalizeUI("Application_Summary_Lbl_Intake", "CyberportEMS_Common")%> <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number")) %></p>
                                        </div>
                                    <asp:Panel ID="pnl_CASPInfo" runat="server">
                                        <div style="height: 68px"></div>
                                          <div style="height: 100px" class="apply-wpr">
                                            <p class="second"><%= System.Web.HttpUtility.HtmlEncode(SPFunctions.LocalizeUI("Dashboard_CASPAppInfo", "CyberportEMS_Common"))%></p>
                                        </div>
                                        <div style="height: 65px"></div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnl_IntakeInfo" runat="server">

                                       

                                        <div class="apply-wpr">

                                            <p class="first"><%= System.Web.HttpUtility.HtmlEncode(SPFunctions.LocalizeUI("Submission_Deadline", "CyberportEMS_Common"))%></p>
                                            <p class="second"><%# System.Web.HttpUtility.HtmlEncode(SPFunctions.dbTextbyLanguage(Eval("Application_Deadline_Eng").ToString(),Eval("Application_Deadline_SimpChin").ToString(),Eval("Application_Deadline_TradChin").ToString())) %></p>
                                        </div>

                                        <div class="apply-wpr">
                                            <p class="first"><%= System.Web.HttpUtility.HtmlEncode(SPFunctions.LocalizeUI("Vetting_and_Presentation", "CyberportEMS_Common"))%></p>
                                            <p class="second"><%# System.Web.HttpUtility.HtmlEncode(SPFunctions.dbTextbyLanguage(Eval("Vetting_Session_Eng").ToString(),Eval("Vetting_Session_SimpChin").ToString(),Eval("Vetting_Session_TradChin").ToString())) %></p>
                                        </div>

                                        <div class="apply-wpr">

                                            <p class="first"><%= System.Web.HttpUtility.HtmlEncode(SPFunctions.LocalizeUI("Result_Announcement", "CyberportEMS_Common"))%></p>
                                            <p class="second"><%# System.Web.HttpUtility.HtmlEncode(SPFunctions.dbTextbyLanguage(Eval("Result_Announce_Eng").ToString(),Eval("Result_Announce_Simp_Chin").ToString(),Eval("Result_Announce_TradChin").ToString())) %></p>
                                        </div>
                                    </asp:Panel>

                                    <div class="row txt-center">
                                        <asp:Button Text="" ID="btn_ProgramRedirection" runat="server" CssClass="apply-btn primary-theme theme-blue" />
                                    </div>
                                </div>

                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </div>
                    </div>
                        </FooterTemplate>
                    </asp:Repeater>



                </div>
            </div>
        </div>
    </div>

</div>
<%--<div class="cpy-footer">
    <span>&copy; 2016 Hong Kong Cyberport Management Company Limited.</span>
</div>--%>

<!-- popup -->

<%--<div class="popup-overlay"></div>
<div class="popup">
    <div class="pos-relative card-theme full-width">
        <div class="pop-close">
            <img src="images/_layouts/15/Images/CBP_Images/close.png" height="20" alt="closebtn">
        </div>

        <h1 class="card-heading">Application Collaboration</h1>

        <div class="table full-width">
            <div class="table-row">
                <div class="table-col half-width padding-no" style="vertical-align: top">
                    <div class="row-btm">
                        <input type="text" class="card-input" value="account@outlook.com" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'account@outlook.com';}">
                        <img src="images/_layouts/15/Images/CBP_Images/delete.png" alt="deletebtn">
                    </div>

                    <div class="row-btm">
                        <input type="text" class="card-input" value="account@outlook.com" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'account@outlook.com';}">
                        <img src="images/_layouts/15/Images/CBP_Images/delete.png" alt="deletebtn">
                    </div>

                    <div class="row-btm">
                        <input type="text" class="card-input" value="account@outlook.com" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'account@outlook.com';}">
                        <img src="images/_layouts/15/Images/CBP_Images/delete.png" alt="deletebtn">
                    </div>

                    <div>
                        <img src="images/_layouts/15/Images/CBP_Images/more.png" alt="deletebtn" id="morebtn">
                    </div>
                </div>
                <div class="table-col half-width padding-no">
                    <div class="member-card">
                        <h1 class="member-card-heading">Invite Member</h1>
                        <div class="row">
                            <input type="text" class="card-input" value="Email" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'Email';}">
                            <img src="images/_layouts/15/Images/CBP_Images/invite.png">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>--%>
