<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReimbursementAppList.ascx.cs" Inherits="CBP_EMS_SP.ReimbursementApprovals.ReimbursementAppList.ReimbursementAppList" %>


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
        /*width: 100%;*/
    }

    .rTableRow {
        display: table-row;
    }

    .rTableCell {
        display: table-cell;
        padding: 5px 10px;
    }

    .ddplist {
        /*min-width: 279px;*/
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

    .griidview th, .griidview td {
        padding: 10px;
        text-align: left;
    }

    .temptext {
        color: white;
    }

    .btntext {
        color: black;
    }

    .divMargin {
        margin-bottom: 6px;
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

    .savebtn {
        text-align: right;
        padding-right: 14px;
    }

    .tableMargin {
        margin-bottom: 21px;
    }

    .sortitem {
        margin-top: -29px;
    }

    .float-right {
        margin-right: 85px;
    }

    .heading {
        width: 100%;
        float: left;
    }

    .heading1 {
        width: 50%;
        float: right;
    }
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<style>
    .selectboxheight select {
        width: 250px;
    }
</style>
<asp:Panel ID="PanelApplist" runat="server">
    <div class="outsideboard  page_main_block" style="width: 98%">
        <div class="divother">
            <div class="heading">
                <h2 class="titleFont heading">Financial Assistance Reimbursement</h2>
            </div>
        </div>
        <div style="width: 100%">
        </div>
        <table style="width: 70%" cellpadding="10px">
            <tr>
                <td style="width: 200px;">
                    <label>Application No.</label></td>
                <td>
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="searchddlApplicationNumber" runat="server" CssClass="ddplist">
                        </asp:DropDownList>
                    </div>
                </td>
                <td style="width: 100px;">
                    <label>Status</label></td>
                <td>
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="searchddlApplicationStatus" runat="server" CssClass="ddplist">
                            <asp:ListItem Text="All Status" Value=""></asp:ListItem>
                            <asp:ListItem Text="Submitted" Value="Submitted"></asp:ListItem>
                            <asp:ListItem Text="Waiting for response from applicant" Value="Waiting for response from applicant"></asp:ListItem>
                            <asp:ListItem Text="Resubmitted information" Value="Resubmitted information"></asp:ListItem>
                            <asp:ListItem Text="Eligibility checked" Value="Eligibility checked"></asp:ListItem>
                            <asp:ListItem Text="To be disqualified" Value="To be disqualified"></asp:ListItem>
                            <asp:ListItem Text="BDM Approved" Value="BDM Approved"></asp:ListItem>
                            <asp:ListItem Text="BDM Rejected" Value="BDM Rejected"></asp:ListItem>
                            <asp:ListItem Text="BDM Disqualifed" Value="BDM Disqualifed"></asp:ListItem>
                            <asp:ListItem Text="Senior Manager Approved" Value="Senior Manager Approved"></asp:ListItem>
                            <asp:ListItem Text="Senior Manager Rejected" Value="Senior Manager Rejected"></asp:ListItem>
                            <asp:ListItem Text="CPMO Approved" Value="CPMO Approved"></asp:ListItem>
                            <asp:ListItem Text="CPMO Rejected" Value="CPMO Rejected"></asp:ListItem>
                            <asp:ListItem Text="Finance Assist Accountant Approved" Value="Finance Assist Accountant Approved"></asp:ListItem>
                            <asp:ListItem Text="Finance Assist Accountant Rejected" Value="Finance Assist Accountant Rejected"></asp:ListItem>
                            <asp:ListItem Text="Finance Senior Manager Approved" Value="Finance Senior Manager Approved"></asp:ListItem>
                            <asp:ListItem Text="Finance Senior Manager Rejected" Value="Finance Senior Manager Rejected"></asp:ListItem>
                            <asp:ListItem Text="CFO Approved" Value="CFO Approved"></asp:ListItem>
                            <asp:ListItem Text="CFO Rejected" Value="CFO Rejected"></asp:ListItem>
                            <asp:ListItem Text="CEO Approved" Value="CEO Approved"></asp:ListItem>
                            <asp:ListItem Text="CEO Rejected" Value="CEO Rejected"></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Company Name</label></td>
                <td colspan="3">
                    <asp:TextBox CssClass="form-control" Style="border-radius: 0;" ID="searchTxtCompany" Width="75%" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <label>Programme Type</label></td>
                <td>
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="searchddlProgType" runat="server" CssClass="ddplist">
                            <asp:ListItem Text="All Category" Value=""></asp:ListItem>
                            <%-- <asp:ListItem Text="CPIP" Value="CPIP"></asp:ListItem>--%>
                            <asp:ListItem Text="Financial Assistance" Value="FA"></asp:ListItem>
                            <asp:ListItem Text="Special Request" Value="SR"></asp:ListItem>
                       </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <label>Category</label></td>
                <td>
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="searchddlReimbursementCategory" DataTextField="Value" DataValueField="Key" runat="server" CssClass="ddplist">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Submission Date</label></td>
                <td>
                    <asp:TextBox ID="txtSubmissionFromDate" Width="30%" runat="server" Text="" CssClass="input-sm datepickerDMonthY"></asp:TextBox>
                    <%--<asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" ID="imgCalenderFrom" runat="server" Style="vertical-align: top" />--%>
                </td><td>
                    <label>To</label></td>
                <td>
                    <asp:TextBox ID="txtSubmissionToDate" Width="30%" runat="server" Text="" CssClass="input-sm datepickerDMonthY"></asp:TextBox>
<%--                    <asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" ID="imgCalenderTo" runat="server" Style="vertical-align: top" />--%>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
                <td>
                    <asp:Button ID="btn_Search" OnClick="btn_Search_Click" CssClass="apply-btn greentheme" Text="Search" runat="server" />

                </td>
            </tr>
        </table>

        <div class="griidview">
            <asp:GridView ID="GridViewApplication" OnRowDataBound="GridViewApplication_RowDataBound" CssClass="griidviewTable" runat="server" CellSpacing="-1" GridLines="None" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                HeaderStyle-CssClass="textBlueFont" RowStyle-CssClass="textBlackFont">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Application No.
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkApplicationApproval" Text='<%#Eval("ApplicationNo") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                    <asp:BoundField DataField="Category" HeaderText="Reimbursement Category" />
                    <asp:TemplateField HeaderText="Submission Date">
                        <ItemTemplate>
                            <%# !string.IsNullOrEmpty( Convert.ToString( Eval("SubmissionDate")))?((DateTime)Eval("SubmissionDate")).ToString("dd MMM yyyy"):string.Empty %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>

            </asp:GridView>
        </div>


    </div>
</asp:Panel>
<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
    <script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
    <script>
        $(".datepickerDMonthY").keydown(false);
        $(".datepickerDMonthY").datepicker({

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

                $('.ui-datepicker-calendar').css("display", "block");
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


    </script>
