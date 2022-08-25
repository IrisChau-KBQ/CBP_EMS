<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FinalVettingResult.ascx.cs" Inherits="FinalVettingResult.FinalVettingResult.FinalVettingResult" %>
<style>
    .input-width {
        width: 135px !important;
    }
</style>
<div class="card-theme page_main_block summaryform" style="width: 100%">
    <h1 class="pagetitle">Final Vetting Result</h1>
    <div style="border:1px solid #c4c4c4; border-left:0px; border-right:0px; padding: 10px 0px; margin-bottom:15px;">
     
    
    <table>
        <tr>
            <td style="color: #0072C6">Total No. of Applications</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label Text="text" ID="lbltotalprojects" runat="server"  /></td>
        </tr>
        <tr>
            <td style="color: #0072C6">Short Listed For Presentation</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label ID="lblShort_Listed" runat="server" /></td>
        </tr>
        <tr>
            <td style="color: #0072C6">Recommended</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label Text="text" ID="lblrecommended" runat="server" /></td>
        </tr>
        <tr>
            <td style="color: #0072C6">Not Recommended</td>
            <td style="width: 15px;"></td>
            <td>
                <asp:Label Text="text" ID="lblnotrecommended" runat="server" />
            </td>
        </tr>
    </table>
        </div>
    <p style="color:#383838"> List of Applications</p>
     <div style="border:1px solid #c4c4c4; padding:10px;">
    <asp:Repeater runat="server" ID="rptrprogrammesummary">
        <HeaderTemplate>

            <table class="datatable fullwidth">
                <thead>
                    <tr>

                        <th>Application No.</th>
                        <th>Company/Project Name</th>



                        <th>Recommended</th>
                        <th>Not Recommended</th>
                        <th>No. of Votes</th>
                        <th>Remarks</th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>

            <tbody>
                <tr>

                    <td style="color: #80C343">
                        <asp:Label ID="lblapplicationnumber" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Application_Number")) %>' /></td>
                    <td>
                        <asp:Label ID="lblcompanyname" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("company_name")) %>' /></td>



                    <td>
                        <asp:Label ID="lblRecommended" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Recommendedcount")) %>' /></td>
                    <td>
                        <asp:Label ID="lblNotRecommended" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("NotRecommendedcount")) %>' /></td>
                    <td>
                        <asp:Label ID="lblTotalvotes" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("totalvotes")) %>' /></td>
                    <td align="center">
                            <asp:Repeater ID="rptremarks" runat="server" DataSource='<%# (( CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary)Container.DataItem).Score_of_vettingmember %>'>
                                <HeaderTemplate>
                                    <table style="width: 100%">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style='<%# (!string.IsNullOrEmpty(Convert.ToString(Eval("Remarks")))? "": "display:none;")%>'>
                                        <td align="center" style="color: #6D6E71; font-weight: 100">0<%#Container.ItemIndex+1 %> :
                                    <%# System.Web.HttpUtility.HtmlEncode(Eval("Remarks")) %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>

                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    <%--  <td><asp:Repeater ID="rptremarks" runat="server" DataSource='<%# (( CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary)Container.DataItem).Score_of_vettingmember %>'>
                        <HeaderTemplate>
                            <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>--%>
                    <%-- <td style="color:#6D6E71;font-weight:100">0<%#Container.ItemIndex+1 %> :--%>
                  <%--  <td>
                        <asp:Label ID="Label1" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Remarks")) %>' /></td>--%>

                </tr>




            </tbody>
        </ItemTemplate>

        <FooterTemplate>
            </table>
         
        </FooterTemplate>
    </asp:Repeater>
      
         </div>
    <div style="margin-top: 50px;" class="btn-box">
        <asp:Button  ID="btn_Confirm" Text="Confirm" CssClass="apply-btn skytheme" runat="server" OnClick="btn_Confirm_Click"  />
        <asp:Button  ID="btn_export" Text="Export as Word" CssClass="apply-btn skytheme" runat="server" OnClick="btnexport_Click" Visible="false" />
     <asp:Button  Text="Cancel" runat="server" class="apply-btn greentheme" ID="btn_Cancel"/>
        <%-- <button ID="btn_Back"   class="apply-btn greentheme"  OnClick="btn_Back_Click" value="" >Cancel</button>--%>

    </div>
</div>
   <asp:Panel ID="pnlsubmissionpopup" runat="server" Visible="false">
        
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                   <%=SPFunctions.LocalizeUI("Final_Vetting_Confirmation", "CyberportEMS_Common") %> 
                </p>
                 
               
                             
            </div>
        </div>
    </asp:Panel>
<asp:Panel ID="PopupBConfirm" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">

            <p class="popup--para">
                Do you want to save?
            </p>
            <div class="table full-width">
                <div class="form-group">
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="No" runat="server" CssClass="bluetheme" />
                    <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Yes" runat="server" CssClass="greentheme" />
                    <%--  <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton2" OnClick="ImageButton1_Click" />--%>
                </div>
            </div>
            <%--<p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <%=SPFunctions.LocalizeUI("Final_Vetting_Confirmation", "CyberportEMS_Common") %>
            </p>--%>

        </div>
    </div>
</asp:Panel>
<script>
   
    //$("#btn_Back").click(function () {
       
    //    window.history.back();
    //})
</script>

