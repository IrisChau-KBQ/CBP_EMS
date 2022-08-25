<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanySearch.ascx.cs" Inherits="CBP_EMS_SP.CompanyProfile.CompanySearch.CompanySearch" %>

<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />
<div class="card-theme page_main_block">
    <h1 class="pagetitle">Company Profiles</h1>
    <div class="form">
    <div class="row">
        <div class="col-md-6">
            <div class="col-md-3">
                <label>Profile/Company/Brand Name</label>
            </div>
            <div >
                <asp:TextBox runat="server" ID="txtprofile" CssClass="input-sm"></asp:TextBox>
            </div>
        </div>
         <div class="col-md-6">
            <div class="col-md-3">
                <label>Core Member Name</label>
            </div>
            <div>
                <asp:TextBox runat="server" ID="txtcoremember" CssClass="input-sm"></asp:TextBox>
            </div>
        </div>

    </div>
        <div class="row">
        <div class="col-md-6">
            <div class="col-md-3">
                <label>Program Type</label>
            </div>
            <div class="selectboxheight">
                <asp:DropDownList runat="server" ID="ddlprogramtype" style="height:40px">
                    <asp:ListItem Text="CCMF" Value="CCMF"></asp:ListItem>
                    <asp:ListItem Text="Incubation" Value="CPIP"></asp:ListItem>
                    <asp:ListItem Text="CASP" Value="CASP"></asp:ListItem>
                    <asp:ListItem Text="All" Selected="True" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
         <div class="col-md-6">
            <div class="col-md-3">
                <label>Intake no.</label>
            </div>
            <div>
                <asp:TextBox runat="server" ID="txtintake" CssClass="input-sm"></asp:TextBox>
            </div>
        </div>

    </div>
        <div class="row">
        <div class="col-md-6">
            <div class="col-md-3">
                <label>cluster</label>
            </div>
            <div>
                <asp:TextBox runat="server" ID="txtcluster" CssClass="input-sm"></asp:TextBox>
            </div>
        </div>
              <div class="col-md-6">
            <div class="col-md-3">
                <label>Tags</label>
            </div>
            <div>
                <asp:TextBox runat="server" ID="txttags" CssClass="input-sm"></asp:TextBox>
            </div>
        </div>
    </div>

        <div class="row" style="float: right;">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="apply-btn skytheme" onclick="btnSubmit_Click"/>
        </div>
        </div>
    <div class="clearfix"></div>
    <div>
        <div>
            <p>Total No of Application : <span runat="server" id="totalappNo">0</span></p>
        </div>
          <asp:GridView runat="server" ID="gdv_companyProfileList" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                       
                                          <asp:TemplateField HeaderText="Profile Name (Eng)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <a href="../SitePages/CompanyBasicInfo.aspx?app=<%#Eval("Company_Profile_ID") %>"><%#Eval("Name_Eng") %></a>
                                          
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Profile Name (Chi)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Name_Chi") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Company Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Company_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Brand Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Brand_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="CCMF Cluster" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("CCMF_Custer") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="CPIP Cluster" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("CPIP_Custer") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Prog. Type" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Programme_Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Tags" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Tag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     
                                            </Columns>
              </asp:GridView>

    </div>

</div>