<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="My_Reimbursement.ascx.cs" Inherits="CBP_EMS_SP.My_Reimbursement.My_Reimbursement.My_Reimbursement" %>

<%@ Import Namespace="CBP_EMS_SP.Common" %>

<style>
    th {
        text-align: left;
    }
</style>

<div class="card-theme page_main_block">
    <h1 class="pagetitle"><%=SPFunctions.LocalizeUI("Menu_User_Reimbursements", "CyberportEMS_Common") %></h1>
    <br />

    <table style="display: none;">
        <tr>
            <td style="width: 60%" class="title2"><%=SPFunctions.LocalizeUI("lbl_CPIP_Reimbursement", "CyberportEMS_Common") %></td>
            <td>
                <asp:Button ID="btn_CPIP_SR_New" PostBackUrl="/SitePages/Special_Request_CPIP.aspx" CssClass="apply-btn bluetheme" runat="server" />
            </td>
            <td>
                <asp:Button ID="btn_CPIP_FA_New" PostBackUrl="/SitePages/Financial_Reimbursements_CPIP.aspx" CssClass="apply-btn bluetheme" runat="server" />
            </td>
        </tr>
    </table>
    <br />

    <table class="custtable" style="display: none;">
        <thead>
            <tr>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("lbl_Company", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">

                    <%=SPFunctions.LocalizeUI("lbl_Category", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("lbl_Submission_Date", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("lbl_Status", "CyberportEMS_Common") %>
                </th>
            </tr>
        </thead>
        <asp:Repeater ID="rptrReimbursementListCPIP" runat="server">
            <HeaderTemplate>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("ApplicationNo") %></td>
                    <td><%# ((CBP_EMS_SP.Data.Models.TB_COMPANY_PROFILE_BASIC)Eval("TB_COMPANY_PROFILE_BASIC")) != null?((CBP_EMS_SP.Data.Models.TB_COMPANY_PROFILE_BASIC)Eval("TB_COMPANY_PROFILE_BASIC")).Company_Name:"" %></td>
                    <td><%# Eval("Category") %></td>
                    <td><%# !string.IsNullOrEmpty( Convert.ToString( Eval("Submitted_Date")))?((DateTime)Eval("Submitted_Date")).ToString("dd-MMM-yyyy"):"" %></td>
                    <td><%# Eval("Status") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
            </FooterTemplate>
        </asp:Repeater>
    </table>

    <table>
        <tr>
            <td style="width: 60%" class="title2"><%=SPFunctions.LocalizeUI("lbl_CASP_Reimbursement", "CyberportEMS_Common") %></td>
            <td>
                <asp:Button ID="btn_CASP_SR_New" Visible="false" CssClass="apply-btn bluetheme" runat="server" />
            </td>
            <td>
                <asp:Button ID="btn_CASP_FA_New" Visible="false" CssClass="apply-btn bluetheme" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <table class="custtable">
        <thead>
            <tr>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("lbl_Company", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">

                    <%=SPFunctions.LocalizeUI("lbl_Category", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("lbl_Submission_Date", "CyberportEMS_Common") %>
                </th>
                <th class="bluecolor">
                    <%=SPFunctions.LocalizeUI("lbl_Status", "CyberportEMS_Common") %>
                </th>
                <th></th>
            </tr>
        </thead>
        <asp:Repeater ID="rptrReimbursementListCASP" OnItemCommand="rptrReimbursementListCASP_ItemCommand" OnItemDataBound="rptrReimbursementListCASP_ItemDataBound" runat="server">
            <HeaderTemplate>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>


                    <td><%# Eval("ApplicationNo") %></td>
                    <td><%# Eval("Company")%></td>
                    <td><%# Eval("Category") %></td>
                    <td><%# !string.IsNullOrEmpty( Convert.ToString( Eval("Submitted")))?((DateTime)Eval("Submitted")).ToString("dd-MMM-yyyy"):"" %></td>
                    <td><%# Eval("Status") %></td>
                    <td>
                        
                        <asp:ImageButton runat="server" ID="ImgReimbursement" PostBackUrl='<%# (Convert.ToString(Eval("CASPType"))=="sr"?"/SitePages/Special_Request_CASP.aspx?resubmit=Y&app="+ Eval("ApplicationID") :"/SitePages/Financial_Reimbursements_CASP.aspx?resubmit=Y&app="+ Eval("ApplicationID") )%>' />
                       
                         <asp:ImageButton runat="server" ID="imgbtnDelete" Visible="false" CommandArgument='<%# Eval("ApplicationID") %>'/>
                        <%-- <a href='<%# (Convert.ToString(Eval("CASPType"))=="sr"?"/SitePages/Special_Request_CASP.aspx?resubmit=Y&app="+ Eval("ApplicationID") :"/SitePages/Financial_Reimbursements_CASP.aspx?resubmit=Y&app="+ Eval("ApplicationID") )%>'  title="Edit Reimbursement" class="apply-btn bluetheme">Edit</a>--%>

                        <%--<a href='<%# Eval(CASPType)) %>'/SitePages/Financial_Reimbursements_CASP.aspx?resubmit=Y&app=<%#Eval("ApplicationID") %>' class="apply-btn bluetheme">Edit</a>--%>

                    </td>

                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
            </FooterTemplate>

        </asp:Repeater>
    </table>
</div>
