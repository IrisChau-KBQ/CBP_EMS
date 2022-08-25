<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresentationListOfAppWebPart.ascx.cs" Inherits="CBP_EMS_SP.PresentationOfAppWP.PresentationListOfAppWebPart.PresentationListOfAppWebPart" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>


<style>
    .outsideboard {
        min-width: 150px;
        max-width: 100%;
        width: auto;
        padding: 15px 30px 15px 15px;
        vertical-align: middle;
    }

    .insideboard {
        width: 300px;
        border: 1px solid black;
    }

    .center {
        text-align: center;
    }

    .rTable {
        display: table;
        width: 100%;
    }

    .rTableRow {
        display: table-row;
    }

    .rTableCell {
        display: table-cell;
        padding: 5px 10px;
    }

    .ddplist {
        min-width: 279px;
    }

    .divother {
        padding: 5px 10px;
    }

    .divother-rigth {
        padding: 5px 10px;
    }

    .right {
        position: absolute;
        right: 0px;
        width: 300px;
        background-color: #b0e0e6;
    }

    .txtarea {
        max-width: 100%;
        min-width: 80%;
        max-height: 250px;
        min-height: 150px;
        height: 150px !important;
    }

    .griidview th {
        padding: 10px;
        text-align: left;
    }

    .griidview td {
        padding: 10px;
        text-align: left;
        white-space: -moz-pre-wrap !important; /* Mozilla, since 1999 */
        white-space: -webkit-pre-wrap; /*Chrome & Safari */
        white-space: -pre-wrap; /* Opera 4-6 */
        white-space: -o-pre-wrap; /* Opera 7 */
        white-space: pre-wrap; /* css-3 */
        word-wrap: break-word; /* Internet Explorer 5.5+ */
        word-break: break-all;
        white-space: normal;
    }

    .griidview {
        width: 100%;
        border: 1px solid #e0dede;
    }

    .temptext {
        color: white;
    }

    .btntext {
        color: black;
    }

    .divMargin {
        margin-top: 24px;
        margin-bottom: 22px;
    }

    .radio {
        position: relative;
        top: 10px;
    }

        .radio td {
            padding-right: 25px;
        }

            .radio td input {
                margin-top: 10px;
            }

    .blue {
        background-color: #58a1e4 !important;
        cursor: pointer;
    }

    .table1 .rTableCell {
        padding: 6px 95px 6px 10px;
    }

    .table2 .rTableCell {
        padding-right: 151px;
    }

    .blue1 {
        color: #0072C6;
    }

    .margindiv {
        margin-bottom: 5px;
    }

    .Attenddiv {
        margin-bottom: -2px;
        padding-left: 7px;
    }

    .FileUploadInfo {
        padding-right: 209px;
        position: relative;
        top: 21px;
    }

    .left {
        text-align: left;
    }

    input.btnDeepBlue, input.btnDeepBlue:hover {
        margin-left: 0px;
        margin-right: 15px;
        background-color: #0072c6;
        color: white;
        padding: 8px 15px;
        cursor: pointer;
    }

    input.greentheme {
        cursor: pointer;
    }
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<script>
    function PopUpBox(ProDesc) {
        $("#PopUpProjectDescription .modal-body").html(ProDesc);
        $("#PopUpProjectDescription").modal("show");
    }

</script>
<asp:HiddenField ID="hdnVAIDList" runat="server" />
<div class="outsideboard  page_main_block" style="width: 100%">
    <div class="divother">
        <h2 class="titleFont">Presentation List of Applications</h2>
    </div>
    <br />
    <asp:Panel ID="Panelplof" runat="server">
        <hr />
        <div class="rTable table1">
            <div class="rTableRow">
                <div class="rTableCell textBlueFont">
                    Date
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblDate" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                </div>
                <div class="rTableCell textBlueFont">
                    Presentation Session Time
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblTime" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                </div>
                <div class="rTableCell lbl">
                    Venue
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblVenue" runat="server" Text="N/A" />
                </div>
            </div>

            <div class="rTableRow">
                <div class="rTableCell textBlueFont">
                    Programme Name
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblProgrammeName" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                </div>
                <div class="rTableCell textBlueFont">
                    Intake No.
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblIntakeNo" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                </div>
                <div class="rTableCell lbl">
                    Meeting Status
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblMeetingStatus" runat="server" Text="N/A" />
                </div>
            </div>
        </div>
        <hr />
        <div class="divother divMargin">
            <asp:Button ID="btnDecisionSummary" runat="server" Text="Vetting Meeting Summary" CssClass="apply-btn bluetheme" Visible="false" OnClick="btnDecisionSummary_Click" />
            <asp:Button ID="btnPresentationResultSummary" runat="server" Text="Presentation Result Summary" CssClass="apply-btn bluetheme" Visible="false" OnClick="btnPresentationResultSummary_Click" />
            <asp:Button ID="btnDowmloadAllApplications" runat="server" Text="Download Shortlisted Application Files" CssClass="apply-btn bluetheme" OnClick="btnDowmloadAllApplications_Click" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZip" Display="Dynamic"></asp:CustomValidator>
            <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
        </div>

        <div class="rTableRow">
            <asp:GridView ID="GridViewApplicationList" runat="server" GridLines="None" AutoGenerateSelectButton="False" HeaderStyle-CssClass="textBlueFont"
                RowStyle-CssClass="textBlackFont" EditRowStyle-HorizontalAlign="Left" HorizontalAlign="Left" PagerStyle-HorizontalAlign="Left" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                CssClass="griidview" OnRowDataBound="GridViewApplicationList_RowDataBound"
                OnRowCommand="GridViewApplicationList_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TimeSlot" HeaderText="Time Slot" />
                    <%--<asp:BoundField  DataField="ApplicationNo" HeaderText="Application No." ItemStyle-CssClass="textGreenFont"/>--%>
                    <asp:HyperLinkField DataNavigateUrlFormatString="{0}" DataNavigateUrlFields="APPNoURL" DataTextField="ApplicationNo" HeaderText="Application No." ItemStyle-CssClass="textGreenFont" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                    <asp:BoundField DataField="ProgrammeType" HeaderText="Programme Type" />
                    <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" />
                    <asp:TemplateField HeaderText="Project Description">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ImageUrl="~/_layouts/15/Images/CBP_Images/SearchIcon.png" OnClientClick='<%# String.Format("PopUpBox(\"{0}\");return false;",(DataBinder.Eval(Container.DataItem, "ProjectDescription"))) %>' Width="25" ToolTip="View Project Description" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                    <asp:BoundField DataField="AverageScore" HeaderText="Screening Score" />
                    <asp:BoundField DataField="RemarksForVetting" HeaderText="Remarks for Vetting" HtmlEncode="false" />
                    <asp:TemplateField HeaderText="Score">
                        <ItemTemplate>
                            <asp:HiddenField ID="HiddenFieldVAID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VAID") %>' />
                            <asp:HiddenField ID="HiddenFieldApplicationID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ApplicationID") %>' />
                            <asp:LinkButton ID="LinkButtonScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Score") %>' Enabled="False"> </asp:LinkButton>
                            <%--<asp:ImageButton ID="ImageButtonEdit" runat="server" ImageUrl="/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Edit%20Button.png" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CommentsForVetting" HeaderText="Comments" />
                    <asp:BoundField DataField="GoOrNotgo" HeaderText="Recommend / Not Recommend" />
                    <asp:BoundField DataField="Remarks" HeaderText="Scoring Remarks" />
                    <asp:TemplateField HeaderText="Go / Not Go*">
                        <ItemTemplate>
                            <div class="listcss">
                                <asp:CheckBox ID="CheckBoxGonotGo" CssClass="greencheckbox" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "DecisionGoNotGO") %>' Enabled='<%# DataBinder.Eval(Container.DataItem, "EnabledDecisionGoNotGO") %>' Text="&nbsp;" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButtonEdit" runat="server" ImageUrl="/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Edit%20Button.png" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Input/Update Application Score" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </div>
        <div class="rTableCell textBlueFont">
            * Please click the box to confirm the shortlisted applicants to enter the presentation session.
        </div>

        <div style="text-align: left; margin-top: 50px;" class="btnBox">
            <asp:Button ID="btnConfirm" runat="server" Text="Confirm Presentation List" CssClass="apply-btn skytheme" OnClick="btnConfirm_Click" ValidationGroup="PLOA" Width="294px" />
            <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="apply-btn greentheme" ValidationGroup="1" OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>


    <asp:Panel ID="pnlWarning" runat="server" Visible="false">
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton PostBackUrl='<%# "~/SitePages/Presentation%20List%20of%20Applications.aspx?VMID=" + Eval("m_VMID") %>' ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="imbClose" OnClick="imbClose_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color: red">
                    <asp:Label Text="You are not able to submit after the vetting team leader has confirmed the presentation list." runat="server" ID="lblMessage" />
                </p>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="PopupTeamConfirmed" runat="server" Visible="false">
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 65%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton PostBackUrl='<%# "~/SitePages/Presentation%20List%20of%20Applications.aspx?VMID=" + Eval("m_VMID") %>' ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButtonTemClose" OnClick="ImageButtonTemClose_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color: red">
                    <%=SPFunctions.LocalizeUI("TeamLeader_already_Confirmed", "CyberportEMS_Common") %>
                </p>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="SubmitPopup" runat="server" DefaultButton="btn_submitFinal" Visible="false">
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 50%!important">
            <div class="pos-relative card-theme full-width">

                <div class="pop-close">
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButtonClose" OnClick="ImageButtonClose_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                    <%=SPFunctions.LocalizeUI("Vetting_Metting_Application_Confirmed", "CyberportEMS_Common") %>
                </p>

                <%--<p class="popup--para">

                	   Please input your login email and password for confirmation.
            
                </p>

                <div class="table full-width">
                   <%-- <div class="form-group">
                        <asp:TextBox CssClass="input" placeholder="Email" ID="txtUserName" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:TextBox CssClass="input" placeholder="Password" TextMode="Password" ID="txtLoginPassword" runat="server"></asp:TextBox>
                    </div>-

                    <div class="form-group">
                        <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="bluetheme" />
                        <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Submit" runat="server" CssClass="greentheme" />OnClick="btn_submitFinal_Click"

                        <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="bluetheme" />--%>
                <asp:Button ID="btn_submitFinal" Text="Confirm" runat="server" CssClass="greentheme" Visible="false" />
            </div>
            <%--  <div style="padding: 12px 0; visibility:hidden">
                        <p class="text-danger" id="UserCustomerrorLogin" runat="server"></p>
                    </div>--%>
        </div>
        <%--  </div>--%>
        <%--  </div>--%>
    </asp:Panel>

    <!-- Modal -->
    <div class="modal fade" id="PopUpProjectDescription" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Project Description</h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



</div>
