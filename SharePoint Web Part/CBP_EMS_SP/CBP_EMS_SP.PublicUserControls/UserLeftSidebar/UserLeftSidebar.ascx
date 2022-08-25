<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>

<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLeftSidebar.ascx.cs" Inherits="CBP_EMS_SP.PublicUserControls.UserLeftSidebar.UserLeftSidebar" %>
<asp:Panel ID="PnluserLeftBar" runat="server">
                       <style>
                         
                           /*.page_main_block {
                           width:95% !important;
                           margin-top:150px !important;
                           margin-left:45px !important;
                           }*/
                   </style>

    <div class="col-md-3" id="ems-userleft" style="display: none;">
      <a href="/SitePages/Home.aspx" style="color:white;">  <asp:Image  CssClass="logo" runat="server" ImageUrl="/_layouts/15/Images/CBP_Images/logo 1.png" style="width:200px"/></a>
        <div class="table-row">
            <div class="table-col" style="vertical-align: top; width: 20%;">
                <ul class="left-navigation" id="MenuApplicant" runat="server">
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Home.aspx" runat="server"><%=SPFunctions.LocalizeUI("Home", "CyberportEMS_Common") %></asp:HyperLink></li>
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/MyApplications.aspx" runat="server"><%=SPFunctions.LocalizeUI("My_Applications", "CyberportEMS_Common") %></asp:HyperLink></li>
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/MyInvitations.aspx" runat="server"><%=SPFunctions.LocalizeUI("My_Invitations", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/ChangePassword.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Change_Password", "CyberportEMS_Common") %></asp:HyperLink></li>
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/MyReimbursements.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_User_Reimbursements", "CyberportEMS_Common") %></asp:HyperLink></li>
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/my_company_profile.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_User_Company", "CyberportEMS_Common") %></asp:HyperLink></li>
                </ul>
               <ul class="left-navigation" id="MenuAdUsers" runat="server">
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Application%20List.aspx" runat="server" ><%=SPFunctions.LocalizeUI("Menu_Applications", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/ProgrammeManagement.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Programme_Management", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/VettingTeam.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Vetting_Team", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Vetting%20Meeting%20Arrangement.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Vetting_Meetings", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/ECProgramList.aspx" runat="server" ><%=SPFunctions.LocalizeUI("Menu_Programme_Results", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Reimbursements.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Reimbursements_List", "CyberportEMS_Common") %></asp:HyperLink></li>  
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/company_internal.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Company_Profile_List", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/EMS%20Reports.aspx" runat="server">Reports</asp:HyperLink></li>
                </ul>
                <ul class="left-navigation" id="MenuVettingMembers" runat="server">
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Application%20List%20for%20Vetting%20Team.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Vetting_Meeting_List", "CyberportEMS_Common") %></asp:HyperLink></li>
<%--                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Presentation%20Result%20Summary.aspx"  runat="server"><%=SPFunctions.LocalizeUI("Menu_Presentation_Summary_Result", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/ECProgramList.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Programme_List", "CyberportEMS_Common") %></asp:HyperLink></li>
                    <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/Vetting%20Meeting%20Arrangement.aspx" runat="server"><%=SPFunctions.LocalizeUI("Menu_Vetting_Meetings", "CyberportEMS_Common") %></asp:HyperLink></li>
                     <li class="left-nav-el">
                        <asp:HyperLink NavigateUrl="~/SitePages/ECProgramList.aspx" runat="server" ><%=SPFunctions.LocalizeUI("Menu_Programme_Results", "CyberportEMS_Common") %></asp:HyperLink></li>--%>
                </ul>

            </div>
        </div>
    </div>
</asp:Panel>
