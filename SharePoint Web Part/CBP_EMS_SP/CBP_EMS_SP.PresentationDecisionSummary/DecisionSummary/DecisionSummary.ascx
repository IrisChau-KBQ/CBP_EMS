<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DecisionSummary.ascx.cs" Inherits="CBP_EMS_SP.PresentationDecisionSummary.DecisionSummary.DecisionSummary" %>

<style>
    .input-width {
        width: 135px !important;
    }

    .summaryform th, .summaryform td {
        font-size: 14px;
        padding: 3px !important;
    }

    .btnrefresh {
        margin-top: -55px !important;
        float: right;
    }
</style>


<div class="card-theme page_main_block summaryform" style="width: 100%">
    <h1 class="pagetitle">Vetting Meeting Summary</h1>

    <asp:Button runat="server" OnClick="btnRefresh_Click" Text="Refresh" CssClass="apply-btn skytheme btnrefresh" ID="btnrefresh" Visible="false" />


    <div style="border: 1px solid #c4c4c4; border-left: 0px; border-right: 0px; padding: 10px 0px; margin-bottom: 15px;">


        <table>
            <tr>
                <td style="color: #0072C6">Programme Name</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblprogramme" runat="server" /></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Go</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label ID="lblTotalGo" Text="0" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="color: #0072C6">Intake No.</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblintake" runat="server" /></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Not Go</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label ID="lblTotalNotGo" Text="0" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="color: #0072C6">Meeting Date</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblmeetingdate" runat="server" />&nbsp;(Vetting Meeting)</td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6"></td>
                <td style="width: 15px;"></td>
                <td></td>
            </tr>
            <tr>
                <td style="color: #0072C6">Venue</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblvenue" runat="server" /></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6"></td>
                <td style="width: 15px;"></td>
                <td>
                    <%--                    <asp:Label Text="" ID="lblnotrecommended" runat="server" />--%>
                </td>
            </tr>

        </table>
    </div>
    <div style="border: 1px solid #c4c4c4; padding: 10px;">
        <asp:Repeater runat="server" ID="rptrprogrammesummary" OnItemDataBound="rptrprogrammesummary_ItemDataBound">
            <HeaderTemplate>

                <table class="datatable fullwidth">
                    <thead>
                        <tr>
                            <th valign="top" align="center">Sequence</th>
                            <th valign="top" align="center">Application No.</th>
                            <th valign="top" align="left" style="text-align: left">Company/Project Name</th>

                            <th valign="top" style="width: 130px;" align="center">Go/Not Go choice of each vetting member
                             <asp:Repeater ID="Rptr_HeaderGo" runat="server">
                                 <HeaderTemplate>
                                     <table style="width: 100%">
                                         <tr>
                                 </HeaderTemplate>
                                 <ItemTemplate>
                                     <th valign="top" align="center" style="width: 20px">0<%#Container.ItemIndex+1 %>
                                     </th>
                                 </ItemTemplate>
                                 <FooterTemplate>
                                     </tr>
                                    </table>

                                 </FooterTemplate>
                             </asp:Repeater>
                            </th>
                            <th valign="top" align="center">Go</th>
                            <th valign="top" align="center">Not Go</th>
                            <th valign="top" align="center">No. of Votes</th>
                            <th valign="top" align="center">Remarks</th>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>

                <tbody>
                    <tr>
                        <td align="center" valign="top" style="color: #6D6E71">
                            <%#(Container.ItemIndex+1) %></td>
                        <td align="center" valign="top" style="color: #80C343">
                            <asp:Label ID="lblapplicationnumber" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Application_Number")) %>' /></td>
                        <td align="center" valign="top" style="text-align: left">
                            <asp:Label ID="lblcompanyname" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("company_name")) %>' /></td>
                        <td align="center" valign="top">
                            <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# (( CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary)Container.DataItem).Score_of_vettingmember %>'>
                                <HeaderTemplate>
                                    <table style="width: 100%">

                                        <tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td align="center" valign="top" style="color: #6D6E71; font-weight: 100; width: 20px">
                                        <%#(Eval("Go")==null)?"NA":(Eval("Go").ToString().ToLower()=="true")?"Go":"Not Go" %>
                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                        <td align="center" valign="top">
                            <asp:Label ID="lblRecommended" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("isRecommended")) %>' /></td>
                        <td align="center" valign="top">
                            <asp:Label ID="lblNotRecommended" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("isNotRecommended")) %>' /></td>
                        <td align="center" valign="top">
                            <asp:Label ID="lblTotalvotes" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("totalvotes")) %>' /></td>
                        <td align="center" valign="top"><%#(Convert.ToInt32(Eval("isRecommended"))> Convert.ToInt32(Eval("isNotRecommended"))? "GO":Convert.ToInt32(Eval("isRecommended"))< Convert.ToInt32(Eval("isNotRecommended"))?"NO":Convert.ToInt32(Eval("isRecommended"))== Convert.ToInt32(Eval("isNotRecommended"))?"TBC":"") %></td>
                    </tr>

                </tbody>
            </ItemTemplate>

            <FooterTemplate>
                </table>
         
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <asp:HiddenField ID="hdn_ApplicationGo" Value="0" runat="server" />
    <asp:HiddenField ID="hdn_isTbc" Value="0" runat="server" />
    <asp:HiddenField ID="hdn_isConfirm" Value="0" runat="server" />

    <asp:Repeater ID="rptrUserList" runat="server" OnItemDataBound="rptrUserList_ItemDataBound">
        <HeaderTemplate>
            <table>
                <tr>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HiddenField ID="isconfirmed" runat="server" Value='<%#Eval("Isconfirmed") %>' />
            <asp:HiddenField ID="hdndeclaration" runat="server" Value='<%#Eval("IsDeclared") %>' />
            <td>
                <span style="color: #0072C6">0<%#Container.ItemIndex+1 %>
                    &nbsp;<%# ((CBP_EMS_SP.Data.Models.TB_VETTING_MEMBER_INFO)Eval("UserData")).Full_Name %>
                </span> &nbsp;<asp:Label runat="server" ID="lbldeclaration" Visible="false"> <a   href='/SitePages/declarationofinterest.aspx?VMID=<%#((CBP_EMS_SP.Data.Models.TB_VETTING_MEMBER)Eval("VettingMember")).Vetting_Meeting_ID %>&VTemail=<%#((CBP_EMS_SP.Data.Models.TB_VETTING_MEMBER_INFO)Eval("UserData")).Email %>&Backurl=<%#HttpContext.Current.Request.Url.AbsolutePath %>' style="color: #80C343; font-size: 11px;">Declaration</a></asp:Label>
                <asp:Label runat="server" ID="lbldeclarationlabel" Visible="false">Declaration</asp:Label>
                <asp:Image ImageUrl='<%#Eval("Isconfirmed") %>' runat="server" ID="right" />
        </ItemTemplate>
        <FooterTemplate>
            </tr>
            </table>

        </FooterTemplate>
    </asp:Repeater>
    <div style="margin-top: 50px;" class="btn-box">
        <asp:Button OnClick="btn_Confirm_Click" ID="btn_Confirm" Text="Confirm" CssClass="apply-btn skytheme" runat="server" Enabled="false" />
        <%--  <asp:Button ID="btn_CancelIntake" Text="Cancel" CssClass="apply-btn greentheme" runat="server" Visible="false" />--%>
        <asp:Button Text="Export as Word" ID="btnexport" runat="server" CssClass="apply-btn skytheme" OnClick="btnexport_Click" Visible="false" />
        <%-- <button id="btn_Cancel" class="apply-btn greentheme" onclick="btn_Back_Click" value="" >Cancel</button>--%>
        <asp:Button Text="Cancel" runat="server" class="apply-btn greentheme" ID="btn_Cancel" />
        <%--   <button id="btn_Back" class="apply-btn greentheme" onclick="btn_Back_Click" value="">Back</button>--%>

        <div style="padding: 12px 0;">
            <p class="text-danger" id="errormsg" runat="server"></p>
        </div>

    </div>



</div>
<asp:Panel ID="SubmitPopup" runat="server" DefaultButton="btn_submitFinal" Visible="false">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">

            <div class="pop-close">
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButtonClose" OnClick="ImageButtonClose_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <%=SPFunctions.LocalizeUI("Vetting_Metting_Application_Confirmed", "CyberportEMS_Common") %>
            </p>
            <p>
                <asp:Label ID="lbWarning" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </p>
           <%-- <p class="popup--para" style="visibility: hidden">
                commented in 2018
            </p>

            <div class="table full-width">
               commented in 2018
                <div class="form-group " style="visibility:hidden" >
                    <asp:TextBox CssClass="input" placeholder="Email" ID="txtUserName" runat="server" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Password" TextMode="Password" ID="txtLoginPassword" runat="server"></asp:TextBox>
                </div>



                <div class="form-group" style="visibility:hidden">
                   commented in 2018
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="bluetheme" />
                    <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Submit" runat="server" CssClass="greentheme" />OnClick="btn_submitFinal_Click"

                   <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="No" runat="server" CssClass="bluetheme" />--%>
                    <asp:Button ID="btn_submitFinal" Text="Confirm" runat="server" CssClass="greentheme" Visible="false" />
                </div>
                <%--<div style="padding: 12px 0; visibility:hidden">
                    <p class="text-danger" id="UserCustomerrorLogin" runat="server"></p>
                </div>--%>
            </div>
    <%--    </div>
    </div>--%>
</asp:Panel>
<asp:Panel ID="pnlsubmissionpopup" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                Vetting Meeting Result Confirm successful
            </p>



        </div>
    </div>
</asp:Panel>
<%--<asp:Panel ID="popupformnotsubmitted" runat="server" Visible="false">
        
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton2" OnClick="ImageButton2_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                  Form cannot be submitted
                </p>
                 
               
                             
            </div>
        </div>
    </asp:Panel>--%>
<script>

    $("#btn_Cancel").click(function () {

        window.history.go(-1);
    })
</script>
