<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ECProgramList.ascx.cs" Inherits="ECProgramList.ECProgramList.ECProgramList" %>
<div class="card-theme page_main_block summaryform" style="width: 100%">
    <h1 class="pagetitle">Programme Results</h1>
    <asp:Repeater runat="server" ID="rptrprogrammesummary" >
        <HeaderTemplate>

            <table class="datatable fullwidth">
                <thead>
                    <tr>
                        <th>Program Name</th>
                        <th>Intake No.</th>
                        <th>Report Status</th>
                        
                       
                       
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>

            <tbody>
                <tr>
                
                  
                    <td>
                        <asp:Label ID="lblcompanyname" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Programe_name")) %>' /></td>
                      <td>
                        <asp:Label ID="lblintakenumber" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("intake_number")) %>' /></td>

                    
                    <td>
                        <asp:Label ID="lblstatus" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("status")) %>' /></td>
                    <%--<td>
                        <asp:LinkButton Text="Final Vetting Result" runat="server" OnClick="Final_vetting_result_click" CommandArgument='<%#Eval("Program_id") %>'/></td>--%>
                    
                    <td>
                        <asp:LinkButton ID="lblRecommended"  Text='EC Result' runat="server"  OnClick="ECResult_Click" CommandArgument='<%#Eval("Program_id") %>'/> </td>
                   
            </tbody>
        </ItemTemplate>

        <FooterTemplate>
            </table>
         
        </FooterTemplate>
    </asp:Repeater>


     



</div>
