<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InternalUserApplications.ascx.cs" Inherits="CBP_EMS_SP.PublicUserControls.InternalUserApplications.InternalUserApplications" %>
<style>
    .dropdownwidth
    {
       height:40px;
    }
</style>
<div class="card-theme page_main_block" style="width: 100%">
    <h1 class="pagetitle">Application</h1>

    <asp:Panel ID="pnlSearch" runat="server">
        <asp:DropDownList ID="ddlProgramName" runat="server" CssClass="dropdownwidth">
           
           
        </asp:DropDownList>
         <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
        <asp:DropDownList ID="ddlIntakeNumber" runat="server" CssClass="dropdownwidth">
             <asp:ListItem Text="Intake 20" Value="0"></asp:ListItem>
        </asp:DropDownList>
         <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
        <asp:DropDownList ID="ddlallCluster" runat="server" CssClass="dropdownwidth">
            <asp:ListItem Text="All Cluster" Value="0"></asp:ListItem>
        </asp:DropDownList>
         <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdownwidth">
            <asp:ListItem Text="All Status" Value="0"></asp:ListItem>
        </asp:DropDownList>
         <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
           <asp:Button ID="btn_showselectedlist" OnClick="btn_showselectedlist_Click" Text="Show" runat="server" CssClass="bluetheme" />
        <asp:Label ID="lbl_ProgramDeadLine" runat="server"></asp:Label>
         <div class="form-group">
                <p class="skylbl">Deadline: 1 Dec 2016 5.00pm (GMT +8)</p>
                <p class="skylbl">201 Applicants</p>
            </div>
            
            <div class="form-group">
                <h3 class="greenlbl">Sorted By</h3>
                
                <div class="form-group">
                
                <div class="row">   
                 <div class="col-md-2" style="line-height: 35px;">
                    <asp:DropDownList runat="server" ID="ddlcluster" CssClass="dropdownwidth">
                        <asp:ListItem Text="Cluster" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-10">
                    <asp:RadioButtonList ID="rbtnorder1" runat="server" RepeatDirection="Horizontal" CssClass="box-inline listcss">
                        <asp:ListItem Text=" Ascending"></asp:ListItem>
                        <asp:ListItem Text="Descending"></asp:ListItem>
                    </asp:RadioButtonList>                    
                </div>    
                </div>
                
                <div class="form-group">
                    <div class="col-md-2" style="line-height: 35px;">
                        <asp:DropDownList ID="ddlApplication" runat="server" CssClass="dropdownwidth">
                            <asp:ListItem Text="Application No." Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>  
                    
                    <div class="col-md-10">
                        <asp:RadioButtonList ID="rdobtnorder2" runat="server" RepeatDirection="Horizontal" CssClass="box-inline listcss">
                            <asp:ListItem Text="Ascending"></asp:ListItem>
                            <asp:ListItem Text="Descending"></asp:ListItem>
                        </asp:RadioButtonList>                    
                    </div>    
                    
                </div>
            </div>
                </div>
             <asp:Repeater runat="server" ID="rptrshowlist">
        <HeaderTemplate>
            <table class="datatable">
              
                <thead>
                    <tr>
                        <th>Application No.</th>
                        <th>Project Desc.</th>
                        <th>Company Name</th>
                        <th>Cluster</th>
                        <th>Status</th>
                        <th>Score(BDM)</th>
                        <th>Avg. Score</th>
                        <th>Remarks</th>
                        <th>Shortlisted</th>
                    </tr>
                </thead>
                </HeaderTemplate>
                  <ItemTemplate>
                <tbody>
                    <tr>
                        <td>
                         <asp:Label  runat="server" Id="lblappno" Text='<%# Eval("Application_Number") %>'/></td>
                        <td>
                        <asp:ImageButton ImageUrl="../_layouts/15/images/CBP_Images/.png" runat="server" ID="projdesc" /></td>
                        <td>
                         <asp:Label runat="server" ID="lblcompanyname" Text='<%# Eval("ComapnyName") %>' /> </td>
                        
                        <td>
                             <asp:Label Text="" runat="server" ID="lblcluster"/></td>
                        <td>
                        <asp:Label Text='<%# Eval("Status") %>' runat="server"  ID="lblstatus"/></td>
                        <td>
                         <asp:Label Text="" runat="server" ID="lblscore"/></td>
                        <td>
                           <asp:Label Text="" runat="server" ID="lblavgscore" /></td>
                        <td>
                         <asp:Label Text="" runat="server" ID="lblremarks"/></td>
                        <td>
                        <asp:CheckBox Text="" runat="server" ID="chkshortlisted" CssClass="box-inline listcss"/>
                            
                        </td>
                    </tr>
                  
                </tbody>
                      </ItemTemplate>

        <FooterTemplate>
            </table>
         
        </FooterTemplate>
    </asp:Repeater>
         
        
    </asp:Panel>
</div>
