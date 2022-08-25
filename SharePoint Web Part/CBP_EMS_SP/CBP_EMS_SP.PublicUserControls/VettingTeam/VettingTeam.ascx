<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VettingTeam.ascx.cs" Inherits="CBP_EMS_SP.PublicUserControls.VettingTeam.VettingTeam" %>

  <style>
 .emscontent .card-input-vetting-address {
    width: 80%;
    display: inline-block;
    background: #E9E9EA;
    color: #595A5C;
    font-size: 18px;
    font-weight: 500;
    outline: none;
    border: none;
    border: 1px solid #DED6D6;
    padding: 1%;
    margin-right: 12px;
    -webkit-appearance: none;
}

    </style>
<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
<%--<h1 class="pagetitle">Vetting Team</h1> --%>   

       
<%--  <div class="popup-overlay"></div>--%>
<div class="card-theme page_main_block" style="width: 100%">
    <div>
        <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
            Vetting Team
        </p>
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <div class="col-md-12">
            <table  class="datatable fullwidth">
                <thead>
            <tr>
                <th>Salutation</th>
                 <th>First Name</th>
                 <th>Last Name</th>
                 <th>Email</th>
                 <th>Title</th>
                <th>Address1</th>
                <th>Address2</th>
                <th>Address3</th>
                <th>City</th>
                <th>Country</th>
                
            </tr>
        </thead>
                <tbody>
                <asp:Repeater ID="rptrvettingteam" runat="server"  OnItemCommand="rptrvettingteam_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td><span><%#Eval("Salutation") %></span></td>
                            <td><span><%#Eval("FirstName") %></span></td>
                            <td><span><%#Eval("fullname") %></span></td>
                            <td><span><%#Eval("email") %></span></td>
                            <td><span><%#Eval("Title") %></span></td>
                            <td><span><%#Eval("Address1") %></span></td>
                            <td><span><%#Eval("Address2") %></span></td>
                            <td><span><%#Eval("Address3") %></span></td>
                            <td><span><%#Eval("City") %></span></td>
                            <td><span><%#Eval("Country") %></span></td>
                            <td><asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Edit%20Button.png"  runat="server"  CommandName="edit" CommandArgument='<%#Eval("Vetting_Member_ID") %>' ToolTip="Update vetting member"/></td>
                            <td><asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Reset%20Password.png"  runat="server"  CommandName="reset" CommandArgument='<%#Eval("Vetting_Member_ID") %>' ToolTip="Resend password activation email"/></td>
                            <td><asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Delete.png"  runat="server"  CommandName="delete" CommandArgument='<%#Eval("Vetting_Member_ID") %>' ToolTip="Remove vetting member"/></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            </table>
        </div>
        
    <div style="padding: 12px 0;">
        <asp:ImageButton ImageUrl="../_layouts/15/images/CBP_Images/invite.png" ID="btn_addVettingMember" runat="server" OnClick="AddVettingMember_Click" ToolTip="Add new vetting member" />
    </div>
    </div>

    <asp:HiddenField ID="hdnVettingMemberID" runat="server" Value='' />
    <asp:Panel ID="MemberPanel" runat="server">
        <table style="width:100%;border-collapse:collapse;">
            <tbody>
                <tr>
                    <td>
        <div class="form-group">
            <div class="form-box form-box-edit form-group" data-id="+">
                <div class="row">
                      <div class="row">
                    <asp:DropDownList runat="server" CssClass="card-input-vetting" ID="ddlSalutation">
                        <asp:ListItem Text="Salutation" Value=""></asp:ListItem>
                        <asp:ListItem Text="Dr." Value="Dr."></asp:ListItem>
                         <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>
                         <asp:ListItem Text="Mr." Value="Mr."></asp:ListItem>
                         <asp:ListItem Text="Mrs." Value="Ms."></asp:ListItem>
                         <asp:ListItem Text="Prof." Value="Prof."></asp:ListItem>
                     </asp:DropDownList>

                  
                     <asp:TextBox CssClass="card-input-vetting" placeholder="First Name" ID="txtFirstName" runat="server"></asp:TextBox>
                     
                    <asp:TextBox CssClass="card-input-vetting" placeholder="Last Name" ID="txtvettingteamfullname" runat="server"></asp:TextBox>
                    
                    </div>
                    
                    <div class="row">
                    <asp:TextBox CssClass="card-input-vetting" placeholder="Email" ID="txtvettingteamEmail" runat="server"></asp:TextBox>
                    
                    <asp:TextBox CssClass="card-input-vetting" placeholder="Title" ID="txtTitle" runat="server"></asp:TextBox></div>
                    <div class="row">
                    <asp:TextBox CssClass="card-input-vetting-address" placeholder="Address 1" ID="txtAddress1" runat="server"></asp:TextBox></div>
                    <div class="row">
                    <asp:TextBox CssClass="card-input-vetting-address" placeholder="Address 2" ID="txtAddress2" runat="server"></asp:TextBox></div>
                    <div class="row">
                    <asp:TextBox CssClass="card-input-vetting-address" placeholder="Address 3" ID="txtAddress3" runat="server"></asp:TextBox></div>
                    <div class="row">
                    <asp:TextBox CssClass="card-input-vetting" placeholder="City" ID="txtCity" runat="server"></asp:TextBox>
                    <asp:TextBox CssClass="card-input-vetting" placeholder="Country" ID="txtCountry" runat="server"></asp:TextBox></div>
                     <div style="margin-top: 50px;" class="btn-box">
                        <asp:Button ValidationGroup="vgIntakeProgram" OnClick="btn_Save_Click" ID="btn_Save" Text="Submit" CssClass="apply-btn skytheme" runat="server" ToolTip ="Save input"/>
                        <asp:Button ID="btn_Cancel" OnClick="btn_Cancel_Click" Text="Cancel" CssClass="apply-btn greentheme" runat="server" ToolTip ="Cancel and discard input"/>
                    </div>
                </div>
                <div class="row">
                    <p>
                        <asp:Label ID="lblInvitationvettingteam" runat="server"></asp:Label>
                    </p>
                </div>
            </div>
        </div>
                        </td>
                    </tr>
                </tbody>
            </table>
    </asp:Panel>            
</div>



<asp:Panel ID="panelResetPassword" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
               <asp:Label Text="text" runat="server" ID="lblappsucess" />
            </p>

        </div>
    </div>
</asp:Panel>





