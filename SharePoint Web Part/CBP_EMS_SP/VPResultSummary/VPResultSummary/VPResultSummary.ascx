<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VPResultSummary.ascx.cs" Inherits="VPResultSummary.VisualWebPart1.VPResultSummary" %>
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

    .rTable {
        display: table;
    }

    .rTableRow {
        display: table-row;
    }

    .rTableCell {
        display: table-cell;
        padding: 5px 10px;
    }

    .sortitem {
    }
</style>
<script type="text/javascript">
      function StartDownload() {
        setTimeout(
            function () {
                _spFormOnSubmitCalled = false;
                _spSuppressFormOnSubmitWrapper = true;
                //location.reload();
            }, 1500);
    }
</script>
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />



<div class="card-theme page_main_block summaryform" style="width: 100%">
    <h1 class="pagetitle">Presentation Result Summary</h1>

    <asp:Button runat="server" OnClick="btnRefresh_Click" Text="Refresh" CssClass="apply-btn skytheme btnrefresh" ID="btnrefresh" Visible="false" />


    <div style="border: 1px solid #c4c4c4; border-left: 0px; border-right: 0px; padding: 10px 0px; margin-bottom: 15px;">


        <table>
            <tr>
                <td style="color: #0072C6">Programme Name</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblprogramme" runat="server" /></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Total No. of Applications</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lbltotalprojects" runat="server" /></td>
            </tr>
            <tr>
                <td style="color: #0072C6">Intake No.</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblintake" runat="server" /></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Shortlisted For Presentation</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label ID="lblShort_Listed" runat="server" /></td>
            </tr>
            <tr>
                <td style="color: #0072C6">Meeting Date</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblmeetingdate" runat="server" />&nbsp;(Presentation Session)</td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Recommended</td>
                <td style="width: 15px;"></td>
                <td runat="server" id="tdrecommended">
                    <asp:Label Text="" ID="lblrecommended" runat="server" Visible="false" /></td>
                <td>
                    <asp:Label Text="" ID="lblonsite" runat="server" Visible="false" /></td>
                <td>
                    <asp:Label Text="" ID="lbloffsite" runat="server" Visible="false" /></td>
            </tr>
            <tr>
                <td style="color: #0072C6">Venue</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblvenue" runat="server" /></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Not Recommended</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblnotrecommended" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="color: #0072C6"></td>
                <td style="width: 15px;"></td>
                <td></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">Withdraw</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblWithdrawCount" runat="server" />
                </td>
            </tr>

            <tr>
                <td style="color: #0072C6"></td>
                <td style="width: 15px;"></td>
                <td></td>
                <td style="width: 30px;"></td>
                <td style="color: #0072C6">TBC</td>
                <td style="width: 15px;"></td>
                <td>
                    <asp:Label Text="" ID="lblTBC" runat="server" />
                </td>
            </tr>

            <%-- <tr>
                
            </tr>
            <tr>
               
            </tr>
            <tr>
               
            </tr>
            <tr>
               
            </tr>--%>
        </table>
    </div>

    <div class="rTable tableMargin">
        <div class="rTableRow">
            <div class="rTableCell">
                <h3 class="textGreenFont">Sorted by</h3>
            </div>
        </div>
        <div class="rTableRow">
            <div class="sortitem">

                <div class="rTableCell selectboxheight selectboxGreenIcon">
                    <asp:DropDownList ID="lstSort" runat="server" CssClass="ddplist" AutoPostBack="true" DataTextField="ColumnText" DataValueField="ColumnValue" OnSelectedIndexChanged="lstSort_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="rTableCell">
                    <asp:RadioButtonList ID="radioSort" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="radioSort_SelectedIndexChanged" CssClass="radio listcss">
                        <asp:ListItem Selected="True" Text="Ascending" Value="asc"></asp:ListItem>
                        <asp:ListItem Text="Descending" Value="desc"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
    </div>

    <div style="border: 1px solid #c4c4c4; padding: 10px;">
        <asp:Repeater runat="server" ID="rptrprogrammesummary" OnItemDataBound="rptrprogrammesummary_ItemDataBound" OnItemCommand="rptrprogrammesummary_ItemCommand">
            <HeaderTemplate>

                <table class="datatable fullwidth">
                    <thead>
                        <tr>
                            <th valign="top" align="center">Sequence</th>
                            <th valign="top" align="center">Application No.</th>
                            <th valign="top" align="center" style="text-align: left">Company/Project Name</th>
                            <th valign="top" align="center">Score of each vetting member
                            <asp:Repeater ID="Rptr_Header" runat="server">
                                <HeaderTemplate>
                                    <table style="width: 100%">
                                        <tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <th valign="top" align="center" style="width: 22px">0<%#Container.ItemIndex+1 %>
                                    </th>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tr>
                                    </table>

                                </FooterTemplate>
                            </asp:Repeater>

                            </th>
                            <th valign="top" align="center">Total Score</th>
                            <th valign="top" align="center">Average Score</th>
                            <th valign="top" style="width: 130px;" align="center">Recommended choice of each vetting member
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
                            <th valign="top" align="center">Recommended</th>
                            <th valign="top" align="center">Not Recommended</th>
                            <th valign="top" align="center">No. of Votes</th>
                            <th valign="top" align="center">Result</th>
                            <th valign="top" align="center">Withdraw</th>
                            <th valign="top" align="center">Remarks</th>

                            <th valign="top" align="center">
                                <asp:Label ID="lbl_Edit" Text="Edit" runat="server" Visible="false" /></th>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>

                <tbody>
                    <tr>
                        <td align="center" valign="top" style="color: #6D6E71; text-align: center">
                            <%#(Container.ItemIndex+1) %></td>
                        <td align="center" valign="top" style="color: #80C343">
                            <asp:Label ID="lblapplicationnumber" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Application_Number")) %>' /></td>
                        <td align="center" valign="top" style="text-align: left">
                            <asp:Label ID="lblcompanyname" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("company_name")) %>' /></td>

                        <td align="center" valign="top">
                            <asp:Repeater ID="ChildRepeater" runat="server" DataSource='<%# (( CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary)Container.DataItem).Score_of_vettingmember %>'>
                                <HeaderTemplate>
                                    <table style="width: 100%">

                                        <tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td style="color: #6D6E71; width: 20px; font-weight: 100" valign="top" align="center">

                                        <%-- <%# Convert.ToString(DataBinder.Eval(Container.Parent.Parent, "DataItem.Withdraw"))=="True"?(string.IsNullOrEmpty(Convert.ToString( Eval("Total_Score")))?"NA":System.Web.HttpUtility.HtmlEncode(  Eval("Total_Score"))) :(string.IsNullOrEmpty(Convert.ToString( Eval("Total_Score")))?"--":System.Web.HttpUtility.HtmlEncode(  Eval("Total_Score"))) %>--%>
                                        <%# Convert.ToString(DataBinder.Eval(Container.Parent.Parent, "DataItem.Withdraw"))=="True"?(string.IsNullOrEmpty(Convert.ToString( Eval("Total_Score")))?"NA":"NA") :(string.IsNullOrEmpty(Convert.ToString( Eval("Total_Score")))?"--":System.Web.HttpUtility.HtmlEncode(  Eval("Total_Score"))) %>

                                        <%-- <%# string.IsNullOrEmpty( Convert.ToString( Eval("Total_Score")))?"--":System.Web.HttpUtility.HtmlEncode(Eval("Total_Score")) %>--%>
                                        <%-- <%#DataBinder.Eval(Container.Parent.Parent, "DataItem.Withdraw")%>--%>
                                    &nbsp;
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                        <td align="center" valign="top">
                            <%--   <asp:Label ID="Label12" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Totalscore")) %>' />--%>
                            <asp:Label ID="Label12" runat="server" Text='<%# Convert.ToString( Eval("Withdraw"))=="True"?"NA": System.Web.HttpUtility.HtmlEncode(Eval("Totalscore")) %>' />
                        </td>
                        <td align="center" valign="top">
                            <asp:Label ID="Label13" runat="server" Text='<%# Convert.ToString( Eval("Withdraw"))=="True"?"NA":System.Web.HttpUtility.HtmlEncode(Eval("Averagescore")) %>' /></td>
                        <td align="center" valign="top">
                            <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# (( CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary)Container.DataItem).Score_of_vettingmember %>'>
                                <HeaderTemplate>
                                    <table style="width: 100%">

                                        <tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td align="center" valign="top" style="color: #6D6E71; font-weight: 100; width: 20px">
                                        <%# Convert.ToString(DataBinder.Eval(Container.Parent.Parent, "DataItem.Withdraw"))=="True"?"NA": ((Eval("Go")==null)?"NA":(Eval("Go").ToString().ToLower()=="true")?"Yes":"No") %>

                                        <%-- <%#Eval("Go")%>--%>

                                    </td>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                        <td align="center" valign="top">
                            <asp:Label ID="lblRecommended" runat="server" Text='<%# Convert.ToString( Eval("Withdraw"))=="True"?"NA":System.Web.HttpUtility.HtmlEncode(Eval("isRecommended")) %>' /></td>
                        <td align="center" valign="top">
                            <asp:Label ID="lblNotRecommended" runat="server" Text='<%# Convert.ToString( Eval("Withdraw"))=="True"?"NA":System.Web.HttpUtility.HtmlEncode(Eval("isNotRecommended")) %>' /></td>
                        <td align="center" valign="top">
                            <asp:Label ID="lblTotalvotes" runat="server" Text='<%# Convert.ToString( Eval("Withdraw"))=="True"?"NA":System.Web.HttpUtility.HtmlEncode(Eval("totalvotes")) %>' /></td>
                        <%--Result--%>
                        <td align="center" valign="top"><%# Convert.ToString( Eval("Withdraw"))=="True"?"NA":(Convert.ToInt32(Eval("isRecommended"))> Convert.ToInt32(Eval("isNotRecommended"))? "Recommended":Convert.ToInt32(Eval("isRecommended"))< Convert.ToInt32(Eval("isNotRecommended"))?"Not Recommended": Convert.ToInt32( Eval("totalvotes"))==0?"NA":Convert.ToInt32(Eval("isRecommended"))== Convert.ToInt32(Eval("isNotRecommended"))?"TBC":"") %></td>
                        <%--Withdraw--%>
                        <td align="center" valign="top">

                            <%#(Eval("Withdraw")==null)?"No":(Eval("Withdraw").ToString().ToLower()=="true")?"Yes":"No" %>
                        </td>
                        <%-- Remarks--%>
                        <td align="center" valign="top">
                            <asp:Label ID="lblRemarks" runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Remark")) %>' /></td>
                        <%--Edit--%>
                        <td align="center" valign="top">

                            <asp:ImageButton runat="server" ID="EditButton" CommandName="Edit" Visible="false" CommandArgument='<%#Eval("Vetting_Application_ID") %>' ToolTip="Add result remark" ImageUrl="/_layouts/15/Images/CBP_Images/Internal Use-6.5-Edit Button.png" />
                    </tr>

                </tbody>
            </ItemTemplate>

            <FooterTemplate>
                </table>
         
            </FooterTemplate>
        </asp:Repeater>
    </div>
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
                </span>
                 &nbsp;<asp:Label runat="server" ID="lbldeclaration" Visible="false"> 
                     <a   href='/SitePages/declarationofinterest.aspx?VMID=<%#((CBP_EMS_SP.Data.Models.TB_VETTING_MEMBER)Eval("VettingMember")).Vetting_Meeting_ID %>&VTemail=<%#((CBP_EMS_SP.Data.Models.TB_VETTING_MEMBER_INFO)Eval("UserData")).Email %>&Backurl=<%#HttpContext.Current.Request.Url.AbsolutePath %>' style="color: #80C343; font-size: 11px;">Declaration</a>

                       </asp:Label>
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
        <asp:Button Text="Recommendation letter" ID="btnexport" runat="server" CssClass="apply-btn skytheme" OnClick="btnexport_Click" Visible="false" />
        <%-- <button id="btn_Cancel" class="apply-btn greentheme" onclick="btn_Back_Click" value="" >Cancel</button>--%>
        <asp:Button Text="Cancel" runat="server" class="apply-btn greentheme" ID="btn_Cancel" />
        <asp:Button CssClass="apply-btn skytheme" Text="Export to Excel Sheet" ID="btn_ExportXls" OnClientClick="StartDownload()" OnClick="btn_ExportXls_Click" runat="server" />
        <%--   <button id="btn_Back" class="apply-btn greentheme" onclick="btn_Back_Click" value="">Back</button>--%>

        <div style="padding: 12px 0;">
            <p class="text-danger" id="errormsg" runat="server"></p>
        </div>

    </div>



</div>
<asp:Panel ID="SubmitPopup" runat="server" DefaultButton="btn_submitFinal" Visible="false">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButtonSubmitClose" OnClick="ImageButtonSubmitClose_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">

                <%=SPFunctions.LocalizeUI("Final_Vetting_Presentation_Confirmation", "CyberportEMS_Common") %>
                <%-- Please input your login email and password for confirmation.--%>
            </p>
            <%--<div class="table full-width">--%>
            <%-- <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Email" ID="txtUserName" runat="server" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Password" TextMode="Password" ID="txtLoginPassword" runat="server"></asp:TextBox>
                </div>--%>

            <%--<div class="form-group">--%>
            <%--  <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="bluetheme" />
                    <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Submit" runat="server" CssClass="greentheme" />--%>

            <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Confirm" runat="server" CssClass="apply-btn greentheme" />
            <%--</div>--%>
            <div style="padding: 12px 0; visibility: hidden">
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

<asp:Panel ID="EditPopupRemarks" runat="server" Visible="false">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <h2>Edit</h2>
            <div class="table full-width">
                <div class="form-group">
                    <asp:HiddenField ID="hdnRemarkid" runat="server" />

                    Withdraw:
                    <asp:CheckBox ID="chkwithdraw" runat="server" />
                </div>
                <div class="form-group">
                    Remarks:
                    <asp:TextBox CssClass="input" TextMode="MultiLine" ID="txtRemarks" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Button ID="btnsave" Text="Save" OnClick="btnsave_Click" runat="server" CssClass="greentheme" />
                    <asp:Button ID="btnClosePopup" Text="Cancel" OnClick="btnClosePopup_Click" runat="server" CssClass="bluetheme" />

                </div>
                <%--  <div style="padding: 12px 0;">
                       <p class="text-danger" id="P1" runat="server"></p>
                </div>--%>
            </div>
        </div>
    </div>
</asp:Panel>









<script>

    $("#btn_Cancel").click(function () {

        window.history.go(-1);
    })
</script>
