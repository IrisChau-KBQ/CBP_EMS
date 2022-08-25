<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ECWebPartCASPFA.ascx.cs" Inherits="CBP_EMS_SP.ECWebPartCASPFA.ECWebPartCASPFA.ECWebPartCASPFA" %>
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
        max-width: 150px;
        min-width: 90px;
        width: 90px !important;
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
        min-width: 100%;
        width: 200px !important;
        max-height: 250px;
        min-height: 150px;
        height: 150px !important;
    }

    .RequiredFieldValidator {
        color: red;
    }

    .modal2 {
        position: fixed;
        top: 0;
        left: 0;
        background: rgba(0,0,0,0.4);
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }

    .loading2 {
        /*font-family: Arial;
        font-size: 10pt;*/
        /*border: 5px solid #67CFF5;*/
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        /*background-color: White;*/
        z-index: 999;
        top: 35%;
        left: 50%;
    }

    .emscontent .input-sm {
        padding: 2px 12px !important;
    }
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<script>

    function StartLoading() {
        $(".ecProgModel").show();

        HideLoader();
    }
    function HideLoader() {
        setTimeout(function () {
            $(".modal2").hide();
        }, 3000);
    }
    $(document).ready(function () {
        //$(".SharePointTime select").after('<img src="/_layouts/15/Images/CBP_Images/Intrnal%20Use-Drop%20Down.png" style="vertical-align:top; margin-left:5px;">');
        setTimeout(function () {
            $(".modal2").hide();
        }, 300);

        //setTimeout(function () {
        //    $("#ems-customheader .loading img").hide();
        //}, 300);
    });


</script>
<div class="modal2 ecProgModel">
    <div class="loading2" style="align-content: center">
        <img src="/_layouts/15/Images/CBP_Images/ajax-loader.gif" alt="" />
    </div>
</div>
<asp:HiddenField ID="hdnApplicationStatus" runat="server" />
<asp:HiddenField ID="hdnCurrentRole" runat="server" />
<asp:Panel ID="MainPanel" runat="server" Visible="true">
    <%-- <ContentTemplate>--%>

    <div class="card-theme page_main_block" style="width: 100%">
        <div class="outsideboard  greenheaderborder" style="width: 100%">
            <div>
                <asp:Panel ID="panel2" runat="server" CssClass="aspPanel">
                    <div class="divother">
                        <h2 class="titleFont textGreenFont" id="panelHeading" runat="server">Eligibility Checking</h2>
                    </div>
                    <br />
                   
                    <div class="rTableRow">
                        <div class="rTableCell textBlackFont">Role</div>
                        <div class="rTableCell">
                            <asp:Label ID="lblrole" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                        </div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlackFont">Status</div>
                        <div class="rTableCell">
                            <asp:Label ID="lblApplicationStatus" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                        </div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlackFont">Claimed Amount</div>
                        <div class="rTableCell">
                            <asp:Label ID="lblClaimedAmount" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <div class="divother textBlueFont">Result</div>
                    <div>
                        <asp:RadioButtonList ID="rbtnresult" DataTextField="Text" DataValueField="Value" runat="server" RepeatColumns="1" RepeatDirection="Vertical" CssClass="listcss rdoResultList">
                        </asp:RadioButtonList>


                    </div>

                    <div class="divother">
                        <asp:Button ID="btnDownload" runat="server" Text="DownLoad Attachment" OnClick="btnDownload_Click" CssClass="bluetheme" ValidationGroup="DownloadZipECWP" />
                             <p>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZipECWP" Display="Dynamic"></asp:CustomValidator>
                        <asp:Label ID="lbldownloadmessage" runat="server" style="color:#075CA9"></asp:Label>
                            </p>
                    </div>
                    <asp:Panel ID="pnlFinanceAccountClaimInfo" Visible="false" runat="server">

                        <table style="width: 100%">


                            <tr>
                                <td class="textBlackFont" colspan="2">Confirm Claim Amount</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtActualClaimAmount" Visible="false" runat="server" Text="" CssClass="input-sm"></asp:TextBox>
                                    <asp:Label ID="lblActualClaimAmount" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
<%--                                    <asp:RequiredFieldValidator ID="reqClaimAmount" ErrorMessage="Confirm Claim Amount Required" ControlToValidate="txtActualClaimAmount"
                                        Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server" />
                                    <asp:RegularExpressionValidator ID="regExpClaimAmount" ErrorMessage="Invalid Actual Claim Amount " ControlToValidate="txtActualClaimAmount"
                                        Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server"
                                        ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="textBlackFont" colspan="2">Deliver Cheque</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rdoDeliverChequeBy" Enabled="false" runat="server" CssClass="listcss rdoResultList">
                                        <asp:ListItem Text="By Coordinator" Value="By Coordinator" />
                                        <asp:ListItem Text="By Post" Value="By Post" />
                                    </asp:RadioButtonList>
                                   <asp:RequiredFieldValidator ID="reqrdoDeliverChequeBy" ErrorMessage="Deliver Cheque Required" ControlToValidate="rdoDeliverChequeBy"
                                       InitialValue="" Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="textBlackFont" colspan="2">Deliver Cheque Date</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDeliver_Cheque_DateFinance"  runat="server" Visible="false" Text="" ></asp:Label>
                                    <asp:TextBox ID="txtDeliver_Cheque_DateFinance" Width="76%" runat="server" Visible="false" Text="" CssClass="input-sm datepickerDMonthY_App"></asp:TextBox>
                                </td>
                                <%--                                <td >
                                    <asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" ID="imgCalender" Visible="false" runat="server" Style="vertical-align: top" />
                                </td>--%>
                            </tr>
                            <tr>
                                <td colspan="2">
<%--                                    <asp:RequiredFieldValidator ID="reqCheque_DateFinance" ErrorMessage="Date Required" ControlToValidate="txtDeliver_Cheque_DateFinance"
                                        Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server" />
                                    <asp:RegularExpressionValidator ID="regCheque_DateFinance" ErrorMessage="Invalid Date" ControlToValidate="txtDeliver_Cheque_DateFinance"
                                        Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server"
                                        ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$" />--%>
                                    <%-- ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$" --%>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <%--<asp:Panel ID="pnlCoordinatorClaimInfo" Visible="false" runat="server">

                        <table>
                            <tr>
                                <td class="textBlackFont" colspan="2">Actual Claim Amount</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="textBlackFont" colspan="2">Deliver Cheque Date</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDeliver_Cheque_DateCoordinator" Width="76%" runat="server" Text="" CssClass="input-sm datepickerDMonthY_App"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="reqCheque_DateCoordinator" ErrorMessage="Date Required" ControlToValidate="txtDeliver_Cheque_DateCoordinator"
                                        Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server" />
                                    <asp:RegularExpressionValidator ID="regCheque_DateCoordinator" ErrorMessage="Invalid Date" ControlToValidate="txtDeliver_Cheque_DateCoordinator"
                                        Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server"
                                        ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$" />
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>--%>
                    <asp:Panel ID="pnl_ApplicantComment" runat="server" CssClass="divother">
                        <div class="textBlueFont">Comment for applicants</div>
                        <asp:TextBox ID="txtcommentforapplicants" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
                    </asp:Panel>
                    <div class="divother">
                        <div class="textBlueFont">Comment for internal use</div>
                        <asp:TextBox ID="txtcommentforinternaluse" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqtxtcommentforinternaluse" ErrorMessage="Comment for internal use is required." ControlToValidate="txtcommentforinternaluse"
                            Enabled="false" Display="Dynamic" ForeColor="Red" CssClass="RequiredFieldValidator" runat="server" />

                    </div>

                    <div class="divother">
                        <div class="textBlueFont">History</div>
                    </div>

                    <div class="Historyscroll">
                        <div class="rTable">
                            <div class="rTableRow">
                                <asp:Repeater ID="RepeaterHistory" runat="server" OnItemDataBound="RepeaterHistory_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="rTableRow">
                                            <div class="rTableCell">
                                                <asp:Label ID="lbluser" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"User") %>' CssClass="textGreenFont fontbold"></asp:Label>
                                                <asp:Label ID="LabelDatetime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"datetime") %>' CssClass="textBlackFont"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="rTableRow">
                                            <div class="rTableCell">
                                                <asp:Label ID="lblResultLabel" runat="server" Text="Status :" CssClass="textBlackFont fontbold"></asp:Label>
                                                <asp:Label ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Result") %>' CssClass="textBlackFont"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="rTableRow">
                                            <div class="rTableCell">
                                                <div>
                                                    <asp:Label ID="lblCommentforApplicationtsLabel" runat="server" Text="Comment for applicants" CssClass="textBlackFont fontbold"></asp:Label>
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblCommentforApplicationts" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CommentForApplicants") %>' CssClass="textBlackFont"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="rTableRow">
                                            <div class="rTableCell">
                                                <div>
                                                    <asp:Label ID="lblCommentforInternualUseLabel" runat="server" Text="Comment for internal use" CssClass="textBlackFont fontbold"></asp:Label>
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblCommentforInternualUse" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CommentForInternualUse") %>' CssClass="textBlackFont"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <hr />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="divother hide">
                        <div class="textBlueFont">
                            <asp:Label ID="lblBDMCommentsTitle" runat="server" Text="BDM" class="textBlueFont" />
                        </div>
                        <asp:Label ID="lblBDMComments" runat="server" Text="" />
                    </div>
                    <div style="text-align: left; margin-top: 50px;" class="btnBox">
                        <asp:Label ID="lblmessage" runat="server" Text="" CssClass="RequiredFieldValidator" />
                        <asp:Button ID="BtnSubmit"  runat="server" Text="Submit" OnClick="btnsubmit_Click" CssClass="apply-btn skytheme" OnClientClick="StartLoading()" />
                    </div>

                    <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px;"></asp:BulletedList>

                </asp:Panel>
            </div>
        </div>
    </div>
    <link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
    <script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
    <style>
        .ui-datepicker-trigger {
            vertical-align: top;
        }
    </style>
    <script>
        $.noConflict();
        $(".datepickerDMonthY_App").keydown(false);

        $(".datepickerDMonthY_App").datepicker({

            dateFormat: "dd-M-yy",
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            changeDate: true,
            showOn: 'both',
            constrainInput: false,
            buttonImageOnly: true,
            buttonImage: '/_layouts/15/images/CBP_Images/Calender.png',
            beforeShow: function (el, dp) {

                //$('#hideMonth').html('.ui-datepicker-calendar{display:inline-table;}');
                //$('.ui-datepicker-calendar').show();
                //var datestr;
                //console.log(datestr);
                //console.log($(this).val());
                //console.log(($(this).val()).length);
                //if ((datestr = $(this).val()).length > 0) {
                //    //var date = datestr.substring(0, 2);
                //    //console.log(date);
                //    var year = datestr.substring(datestr.length - 4, datestr.length);
                //    var month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
                //    $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                //    $(this).datepicker('setDate', new Date(year, month, 1));
                //}
            }
            //onClose: function (dateText, inst) {

            //    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            //    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            //    $(this).datepicker('setDate', new Date(year, month, 1));

            //}
        });

           function DisableButton()
            {
                document.getElementById("<%=BtnSubmit.ClientID %>").disabled = true;
            }
            window.onbeforeunload = DisableButton;
    </script>

</asp:Panel>
