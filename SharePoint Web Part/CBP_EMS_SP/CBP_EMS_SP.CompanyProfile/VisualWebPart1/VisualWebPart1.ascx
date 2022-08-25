<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualWebPart1.ascx.cs" Inherits="CBP_EMS_SP.CompanyProfile.VisualWebPart1.VisualWebPart1" %>

<style>
    th {
        text-align:left;
    }
</style>

<div class="card-theme page_main_block">
    <h1 class="pagetitle">My Company Profile</h1>

    <div class="form">
        <asp:Label runat="server" ID="lbltest"></asp:Label>

        <div class="row">

            <table class="custtable">
                <thead>
                    <tr>
                        <th class="bluecolor">Sr. No.
                        </th>
                        <th class="bluecolor">Company Name (Eng)
                        </th>
                        <th class="bluecolor">Company Name (Chi)
                        </th>
                        <th class="bluecolor">Programme Type
                        </th>
                        <th class="bluecolor">Action
                        </th>

                    </tr>
                </thead>
                <asp:Repeater ID="rptrReimbursementListCASP" runat="server">
                    <HeaderTemplate>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text='<%# Container.ItemIndex+1%>' ID="lblprofileid"></asp:Label>
                            </td>
                            <td><%# Eval("Name_Eng")%></td>
                            <td><%# Eval("Name_Chi") %></td>
                            <td><%# Eval("Applicaition_Type") %></td>
                            <td>
                                <a href='/SitePages/CompanyBasicInfo.aspx?app=<%# Eval("Company_Profile_ID") %>'><asp:Image ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.VMArrangeWebPart/view.png" runat="server" /></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                    </FooterTemplate>

                </asp:Repeater>
            </table>

            

        </div>

    </div>



</div>
