<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ECResultWebPart.ascx.cs" Inherits="CBP_EMS_SP.ECResult.ECResultWebPart.ECResultWebPart" %>


<style>
    .input-width {
        width: 135px !important;
    }

    .textmail {
        width: 93% !important;
        /*overflow: hidden!important;*/
        padding: 30px !important;
    }
</style>
<div class="card-theme page_main_block summaryform" style="width: 100%">
    <h1 class="pagetitle">EC Result</h1>
    <table>
        <tr>
            <td style="color: #0072C6">Programme Type</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label Text="text" ID="lblprogramtype" runat="server" /></td>
        </tr>
        <tr>
            <td style="color: #0072C6">Intake Number</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label ID="lblintake" runat="server" /></td>
        </tr>
        <tr>
            <td style="color: #0072C6">Deadline</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label Text="text" ID="lbldeadline" runat="server" /></td>
        </tr>
        <tr>
            <td style="color: #0072C6">Total No. of Applications</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label Text="text" ID="lbltotalapplications" runat="server" />
            </td>
        </tr>
    </table>
    <p style="color: #383838">List of Applications</p>

    <div style="border: 1px solid #c4c4c4; padding: 10px;">
        <asp:Repeater runat="server" ID="rptrprogrammesummary" OnItemDataBound="rptrprogrammesummary_ItemDataBound">
            <HeaderTemplate>

                <table class="datatable fullwidth">
                    <thead>
                        <tr>

                            <th>Application No.</th>
                            <th>Cluster</th>



                            <th>
                                <asp:Label Text="" runat="server" ID="thcompanyname" />
                            </th>
                            <th>
                                <asp:Label Text="Program Type" runat="server" ID="thprogramname" Visible="true" />
                            </th>
                            <th>
                                <asp:Label Text="Application Type" runat="server" ID="thapplicationame" Visible="true" />
                            </th>

                            <th>Status</th>
                            <th>EC Confirmed</th>

                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>

                <tbody>
                    <tr>
                        <asp:HiddenField ID="Hdn_Application_Number" runat="server" Value='<%# System.Web.HttpUtility.HtmlEncode(Eval("Application_Number")) %>' />
                        <asp:HiddenField ID="hdn_ProgramID" runat="server" Value='<%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_ID")) %>' />
                        <td style="color: #80C343">
                            <asp:Label ID="lblapplicationnumber" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Application_Number")) %>' /></td>
                        <td style="color: #80C343">
                            <asp:Label ID="lblCluster" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Cluster")) %>' /></td>
                        <td>
                            <asp:Label ID="lblcompanyname" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Company_Program")) %>' /></td>
                        <td>
                            <asp:Label ID="lblProgramtype" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Programme_Type")) %>' Visible="false" /></td>
                        <td>
                            <asp:Label ID="lblApplicationtype" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Application_Type")) %>' Visible="false" /></td>

                        <td>
                            <asp:Label ID="lblrecommended" runat="server" /></td>

                        <%-- <%#(Eval("Recommended").ToString().ToLower()=="true")?"Recommended":"NotRecommended" %>--%>
                   
                    </td>
                    <td>

                        <asp:CheckBox runat="server" ID="chKECconfirmed" CssClass="listcss" Text="&nbsp;" />

                    </td>
                    </tr>




                </tbody>
            </ItemTemplate>

            <FooterTemplate>
                </table>
         
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <%-- <div style="padding:10px;margin-top:50px" id="mailarea" runat="server" visible="false">
        Sample Email
          <textarea id="textmail" class="textmail" runat="server" style="margin-top:20px" >Hi,
          Please noted that your application for Cyberport Incubation Programme
           Intake  201612 is succeed.Further information will be provided in the coming mail.
             Regards,
             EMS Administrator
         </textarea>    
         </div>--%>
    <div style="margin-top: 50px;" class="btn-box">
        <asp:Button OnClick="btn_Confirm_Click1" ID="btn_Confirm" Text="Confirm" CssClass="apply-btn skytheme" runat="server" Visible="false" />
        <%--    <asp:Button ID="btn_CancelIntake"  Text="Cancel" CssClass="apply-btn greentheme" runat="server" Visible="false"/>--%>
        <%-- <button ID="btn_Back"   class="apply-btn greentheme"  OnClick="btn_Back_Click" value="" >Cancel</button>--%>
        <asp:Button Text="Cancel" runat="server" class="apply-btn greentheme" ID="btn_Cancel" />
        <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>

    </div>
</div>
<asp:Panel ID="SubmitPopup" runat="server" DefaultButton="btn_submitFinal" Visible="false">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                Your submission for intake
                <asp:Label Text="" runat="server" ID="lblintakeno" />, deadline is
                <asp:Label ID="lblDeadlinePopup" runat="server" />.
                	Do you want to confirm EC result?
               
            </p>

            <div class="table full-width">
                <div class="form-group">
                    <asp:Button ID="btn_submitFinal" OnClick="btn_Confirm_Click" Text="Yes" runat="server" CssClass="greentheme" />
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="No" runat="server" CssClass="bluetheme" />
                </div>
                <div style="padding: 12px 0;">
                    <p class="text-danger" id="UserCustomerrorLogin" runat="server"></p>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlsubmissionpopup" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <%=SPFunctions.LocalizeUI("Application_submitted_confirmation", "CyberportEMS_Common") %>
            </p>



        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlCompanyConflicts" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">

            <asp:Repeater ID="rptrConflictCompanies" runat="server">
                <HeaderTemplate>
                    <table class="datatable fullwidth">
                        <tr>
                            <th>Application Number</th>
                            <th>Existing Company</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style='<%# (bool)Eval("HasConflict")==true?"": "display:none" %>'>
                        <td>
                            <asp:HiddenField Value='<%#Eval("ApplicationId") %>' ID="hdnApplicationId" runat="server" />
                            <asp:Label Text='<%#Eval("ApplicationNo") %>' ID="lblAwardingAppNumber" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlConflictCompanies" DataSource='<%# DataBinder.Eval(Container.DataItem,"CompaniesConflicts") %>' DataTextField="Company_ApplicationNo" DataValueField="CompanyProfileId" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <div class="table full-width">
                <div class="form-group">
                    <asp:Button ID="btn_CompanyConflictsConfirm" OnClick="btn_CompanyConflictsConfirm_Click" Text="Confirm" runat="server" CssClass="greentheme" />
                    <asp:Button ID="btn_CompanyConflictsHide" OnClick="btn_CompanyConflictsHide_Click" Text="Cancel" runat="server" CssClass="bluetheme" />
                </div>
                <div style="padding: 12px 0;">
                    <p class="text-danger" id="P1" runat="server"></p>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<script>

    //$("#btn_Back").click(function () {

    //    window.history.back();
    //})
</script>
