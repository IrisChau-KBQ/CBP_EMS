<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyApplications.ascx.cs" Inherits="CBP_EMS_SP.PublicUserControls.MyApplications.MyApplications" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
    
<div class="card-theme page_main_block">
   
    <h1 class="pagetitle" runat="server" id="lblheading"></h1>
  <%--   <asp:Button Text="Download PDF" OnClick="Incubation_Pdf" runat="server" ID="pdf_incubation" />
    <asp:Button Text="test" runat="server" OnClick="Incubation_Pdf" />--%>
    
    <asp:Repeater ID="rptrMyApplicationsIncubation" runat="server" OnItemCommand="rptrMyApplicationsIncubation_ItemCommand">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 86%; padding: 8px 0;">
                    <a href='<%# SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/IncubationProgram.aspx?prog=" + Eval("Programme_ID") + "&app=" + Eval("Programme_guid") + "&resubmitversion=Y"%> '>
                        <%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Name")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number"))  %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("ApplicationNumber"))+ (!string.IsNullOrEmpty( Convert.ToString( System.Web.HttpUtility.HtmlEncode(Eval("ProjectName"))))?" - "+System.Web.HttpUtility.HtmlEncode(Eval("ProjectName")):"" ) %>
                    </a>

                </td>
                
                <td>
                    <asp:LinkButton ID="lnkCollaboration" CommandName="i" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber")+":"+Eval("Programme_guid")  %>'  runat="server" CssClass="apply-btn theme-green"><%#SPFunctions.LocalizeUI("btn_Collaboration", "CyberportEMS_Common") %></asp:LinkButton>
                </td>
                <td style="width: 6%;text-align: right;">
                    
                     <asp:ImageButton ID="imgBtnDelete" CommandName="Delete" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber")+":"+Eval("Programme_guid")  %>' ImageUrl="/_layouts/15/images/CBP_Images/Internal%20Use-6.5-Delete.png" runat="server" Visible='<%#Convert.ToDateTime(Eval("Application_Deadline")) >= DateTime.Now?true:false %>'/>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>

        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptrMyApplicationsCCMF" runat="server" OnItemCommand="rptrMyApplicationsIncubation_ItemCommand">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 86%; padding: 8px 0;">
                    <a href='<%# SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + Eval("Programme_ID") + "&app=" + Eval("Programme_guid")  + "&resubmitversion=Y"%> '>
                        <%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Name")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number")) %> - <%# Eval("ApplicationNumber") + (!string.IsNullOrEmpty( Convert.ToString( System.Web.HttpUtility.HtmlEncode(Eval("ProjectName"))))?" - "+System.Web.HttpUtility.HtmlEncode(Eval("ProjectName")):"" ) %> 
                    </a>
                </td>
                
                <td>
                    <asp:LinkButton ID="lnkCollaboration" CommandName="c" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber")+":"+Eval("Programme_guid") %>' runat="server" CssClass="apply-btn theme-green" ><%#SPFunctions.LocalizeUI("btn_Collaboration", "CyberportEMS_Common") %></asp:LinkButton>
                </td>
                 <td style="width: 6%;text-align: right;">
                     <asp:ImageButton ID="imgBtnDelete" CommandName="Delete" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber")+":"+Eval("Programme_guid")  %>' ImageUrl="/_layouts/15/images/CBP_Images/Internal%20Use-6.5-Delete.png" runat="server"  Visible='<%#Convert.ToDateTime(Eval("Application_Deadline")) >= DateTime.Now?true:false %>'/>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>

        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptrMyApplicationCASP" runat="server" OnItemCommand="rptrMyApplicationsIncubation_ItemCommand">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 86%; padding: 8px 0;">
                    <a href='<%# SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CASPProgram.aspx?prog=" + Eval("Programme_ID") + "&app=" + Eval("Programme_guid")  + "&resubmitversion=Y"%> '>
                        <%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Name")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number")) %> - <%# Eval("ApplicationNumber") + (!string.IsNullOrEmpty( Convert.ToString( System.Web.HttpUtility.HtmlEncode(Eval("ProjectName"))))?" - "+System.Web.HttpUtility.HtmlEncode(Eval("ProjectName")):"" ) %> - [<%# System.Web.HttpUtility.HtmlEncode(Eval("Status")) %> ]
                    </a>
                </td>
                
                <td>
                    <asp:LinkButton ID="lnkCollaboration" CommandName="c" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber")+":"+Eval("Programme_guid") %>' runat="server" CssClass="apply-btn theme-green" ><%#SPFunctions.LocalizeUI("btn_Collaboration", "CyberportEMS_Common") %></asp:LinkButton>
                </td>
                 <td style="width: 6%;text-align: right;">
                     <asp:ImageButton ID="imgBtnDelete" CommandName="Delete" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber")+":"+Eval("Programme_guid")  %>' ImageUrl="/_layouts/15/images/CBP_Images/Internal%20Use-6.5-Delete.png" runat="server"  Visible='<%#Convert.ToDateTime(Eval("Application_Deadline")) >= DateTime.Now?true:false %>'/>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>

        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptrMyApplicationsIncubationColb" runat="server" OnItemCommand="rptrMyApplicationsIncubation_ItemCommand">
        <HeaderTemplate>
            <table style="width: 100%">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 86%; padding: 8px 0;">
                    <a href='<%# SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/IncubationProgram.aspx?prog=" + Eval("Programme_ID") + "&app=" + Eval("Programme_guid")+"&type=fed46de9f573" %> '>
                        <%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Name")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("ApplicationNumber"))  + (!string.IsNullOrEmpty( Convert.ToString( System.Web.HttpUtility.HtmlEncode(Eval("ProjectName"))))?" - "+System.Web.HttpUtility.HtmlEncode(Eval("ProjectName")):"" ) %>
                    </a>

                </td>
                <td>
                    <%--                    <asp:LinkButton ID="lnkCollaboration" CommandName="i" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber") %>' Text="Collaboration" runat="server" CssClass="apply-btn theme-green" />--%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>

        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptrMyApplicationsCCMFColb" runat="server" OnItemCommand="rptrMyApplicationsIncubation_ItemCommand">
        <HeaderTemplate>
            <table style="width: 100%">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 86%; padding: 8px 0;">
                    <a href='<%# SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + Eval("Programme_ID") + "&app=" + Eval("Programme_guid")+"&type=fed46de9f573" %> '>
                        <%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Name")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("ApplicationNumber"))  + (!string.IsNullOrEmpty( Convert.ToString( System.Web.HttpUtility.HtmlEncode(Eval("ProjectName"))))?" - "+System.Web.HttpUtility.HtmlEncode(Eval("ProjectName")):"" ) %> 
                    </a>
                </td>
                <td>
                    <%--                    <asp:LinkButton ID="lnkCollaboration" CommandName="c" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber") %>' Text="Collaboration" runat="server" CssClass="apply-btn theme-green"/>--%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>

        </FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater ID="rptrMyApplicationsCASPColb" runat="server" OnItemCommand="rptrMyApplicationsIncubation_ItemCommand">
        <HeaderTemplate>
            <table style="width: 100%">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="width: 86%; padding: 8px 0;">
                    <a href='<%# SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CASPProgram.aspx?prog=" + Eval("Programme_ID") + "&app=" + Eval("Programme_guid")+"&type=fed46de9f573" %> '>
                        <%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Name")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("Intake_Number")) %> - <%# System.Web.HttpUtility.HtmlEncode(Eval("ApplicationNumber"))  + (!string.IsNullOrEmpty( Convert.ToString( System.Web.HttpUtility.HtmlEncode(Eval("ProjectName"))))?" - "+System.Web.HttpUtility.HtmlEncode(Eval("ProjectName")):"" ) %> - [<%# System.Web.HttpUtility.HtmlEncode(Eval("Status")) %> ]
                    </a>
                </td>
                <td>
                    <%--                    <asp:LinkButton ID="lnkCollaboration" CommandName="c" CommandArgument='<%#Eval("Programme_ID")+":"+Eval("ApplicationNumber") %>' Text="Collaboration" runat="server" CssClass="apply-btn theme-green"/>--%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>

        </FooterTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlcollaboratorPopup" runat="server" Visible="false">
        <asp:HiddenField ID="hdnProgramID" runat="server" />
        <asp:HiddenField ID="hdnApplicationNumber" runat="server" />
       <asp:HiddenField ID="hdnApplicationId" runat="server" />  
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="imgPopupClose" OnClick="imgPopupClose_Click" />
                </div>
                <p class="popup--para" runat="server" id="lblheadincol" style="margin-top: 0; color: #075CA9; font-size: 2em">

                </p>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Repeater ID="rptrCollaboratorList" runat="server" OnItemCommand="rptrCollaboratorList_ItemCommand">
                            <ItemTemplate>
                                <div class="row-btm">
                                    <span class="card-input"><%#System.Web.HttpUtility.HtmlEncode(Eval("Email")) %></span>
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" CommandName="delete" runat="server" CommandArgument='<%#Eval("Email") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:TextBox CssClass="card-input" placeholder="Email" ID="txtCollaboratorEmail" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Email Required" CssClass="text-danger" Display="Dynamic" ControlToValidate="txtCollaboratorEmail" ValidationGroup="newfundingValidation" runat="server" />
                        <asp:ImageButton ID="btn_InviteCollaborator" OnClick="btn_InviteCollaborator_Click" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" ValidationGroup="newfundingValidation" />
                        <p>
                            <asp:Label ID="lblInvitation" runat="server"></asp:Label></p>
                    </div>

                </div>
            </div>
        </div>
    </asp:Panel>
      <asp:Panel ID="pnldeleteapplication" runat="server" Visible="false">
         <asp:HiddenField ID="Hdnprogid" runat="server" />
        <asp:HiddenField ID="hdnappno" runat="server" />
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
                </div>
                <p class="popup--para" runat="server" id="deletepopup" style="margin-top: 0; color: #075CA9; font-size: 2em">
                  
                </p>
                 
                <div class="row">
                    <div class="col-md-12">
                       
                      <div class="col-md-6">
                           <asp:LinkButton ID="lnkyes" CommandName="yes"  Text="" runat="server" CssClass="apply-btn theme-green" OnClick="lnkyes_Click"/>
                      </div>
                        <div class="col-md-6">
                           <asp:LinkButton ID="lnkno" CommandName="no"  Text="" runat="server" CssClass="apply-btn theme-green" OnClick="lnkno_Click"/>
                      </div>
                    </div>

                </div>
                             
            </div>
        </div>
    </asp:Panel>
</div>
