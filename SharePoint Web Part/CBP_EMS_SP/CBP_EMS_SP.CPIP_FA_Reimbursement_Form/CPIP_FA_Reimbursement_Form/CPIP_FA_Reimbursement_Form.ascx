<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CPIP_FA_Reimbursement_Form.ascx.cs" Inherits="CBP_EMS_SP.CPIP_FA_Reimbursement_Form.VisualWebPart1.VisualWebPart1" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>

<asp:HiddenField ID="hdn_FAApplicationID" runat="server" />

<style>
    .emscontent .main {
        margin-top: 50px;
    }

    .emscontent .tr_width .col-md-1 {
        width: 12px;
    }

    .emscontent .tr_width {
        padding-left: 40px;
    }

    .word-wrap {
        overflow: hidden;
        word-break: break-all;
    }

    .label-text {
        color: black;
        word-wrap: normal;
    }
</style>

<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->
        <div class="custom-form-wd-img border-gray boxcenter width-80 pagewhiteblock">
            <div>
                <span class="form__head"><%=SPFunctions.LocalizeUI("lbl_Application_Heading_Reimbursement", "CyberportEMS_CPIP_Reimbursement") %></span>
            </div>
            <div class="form__summary">
                <div class="row" style="padding: 12px 20px">
                    <div class="col-md-2 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Applicant", "CyberportEMS_Common") %></div>
                    <div class="col-md-3 word-wrap">
                        <asp:Label ID="lblApplicant" runat="server" Text="" CssClass="word-wrap"></asp:Label>
                    </div>
                    <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %></div>
                    <div class="col-md-4">
                        <asp:Label ID="lblApplicationNo" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row" style="padding: 12px 20px">
                    <div class="col-md-2 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_LastSubmit", "CyberportEMS_Common") %></div>
                    <div class="col-md-3">
                        <asp:Label ID="lblLastSubmitted" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-3 boldgraylbl"></div>
                    <div class="col-md-4 boldgraylbl"></div>
                </div>
            </div>
            <div class="form-group margin-bottom10 border-bottom">
                <div><%=SPFunctions.LocalizeUI("lblHead_FA_Option", "CyberportEMS_CPIP_Reimbursement") %></div>
                <asp:RadioButtonList OnSelectedIndexChanged="rdo_Categories_SelectedIndexChanged" AutoPostBack="true" CellPadding="5" Width="100%" ID="rdo_Categories" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CssClass="listcss">
                </asp:RadioButtonList>
            </div>
            <div class="row">
                <p>
                    <%=SPFunctions.LocalizeUI("lblHead_FA_Company", "CyberportEMS_CPIP_Reimbursement") %>
                </p>
                <p>
                    <%=SPFunctions.LocalizeUI("lbl_FA_CompanyDescription", "CyberportEMS_CPIP_Reimbursement") %>
                </p>
            </div>
            <div class="row">

                <div class="col-md-4">
                    <p>
                        <label><%=SPFunctions.LocalizeUI("lbl_CompanyName", "CyberportEMS_CPIP_Reimbursement") %></label>
                    </p>
                    <asp:DropDownList ID="ddlCompanyList" runat="server" Style="height: 40px">
                    </asp:DropDownList>
                    <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
                </div>
                <asp:Panel runat="server" ID="pnlBankChequePayble" Visible="false" class="col-md-4">
                    <p>
                        <label><%=SPFunctions.LocalizeUI("lbl_BankChequePayble", "CyberportEMS_CPIP_Reimbursement") %></label>
                    </p>
                    <asp:TextBox ID="txtBankChequePayble" runat="server" CssClass="form-control input-sm-nobackground" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlEventsAttend" Visible="false" class="col-md-4">
                    <p>
                        <label><%=SPFunctions.LocalizeUI("lbl_EventsAttend", "CyberportEMS_CPIP_Reimbursement") %></label>
                    </p>
                    <asp:TextBox ID="txtEventsAttend" runat="server" CssClass="form-control input-sm" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlslct_Location" Visible="false" class="col-md-4">
                    <p>
                        <label><%=SPFunctions.LocalizeUI("lbl_slct_Location", "CyberportEMS_CPIP_Reimbursement") %></label>
                    </p>
                    <asp:DropDownList ID="ddlslct_Location" runat="server" Style="height: 40px">
                    </asp:DropDownList>
                    <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />

                </asp:Panel>
            </div>
            <asp:Panel ID="pnplEventCatBC" runat="server" CssClass="row" Visible="false">
                <div class="col-md-4">
                    <p>
                        <label><%=SPFunctions.LocalizeUI("lbl_EventFrom", "CyberportEMS_CPIP_Reimbursement") %></label>
                    </p>
                    <asp:TextBox ID="txtEventFrom" runat="server" CssClass="input-sm hidedates datepickermmddyyy" />
                    <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />
                </div>
                <div class="col-md-4">
                    <p>
                        <label><%=SPFunctions.LocalizeUI("lbl_EventTo", "CyberportEMS_CPIP_Reimbursement") %></label>
                    </p>
                    <asp:TextBox ID="txtEventTo" runat="server" CssClass="input-sm hidedates datepickermmddyyy" />
                    <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlReimbursementItems" CssClass="row" Visible="false">
                <asp:GridView ID="grdReimbursementItems" runat="server" AutoGenerateColumns="False"
                    ShowHeader="true"
                    GridLines="None" Width="100%">
                    <Columns>

                        <asp:TemplateField ControlStyle-Width="20%">

                            <HeaderTemplate>
                                
                                <%=SPFunctions.LocalizeUI("lbl_Bank_Date", "CyberportEMS_CPIP_Reimbursement") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdn_Item_ID" runat="server" />
                                <asp:TextBox ID="txtBank_Date" Width="90px" value='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Date")))?(Convert.ToDateTime(Eval("Date")).ToString("MM/dd/yyyy")):string.Empty %>' runat="server" CssClass="input-sm hidedates datepickermmddyyy" />
                                <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  ControlStyle-Width="45%">
                            <HeaderTemplate>
                                <%=SPFunctions.LocalizeUI("lbl_Bank_Item", "CyberportEMS_CPIP_Reimbursement") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox Width="85%" ID="txtItemDescription" Text='<%#Eval("Description") %>' runat="server" CssClass="input-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false"  ControlStyle-Width="7%">
                            <HeaderTemplate>
                                <%=SPFunctions.LocalizeUI("lbl_BankAdvertise", "CyberportEMS_CPIP_Reimbursement") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAdvertisement" Checked='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Advertisement")))?(bool)Eval("Advertisement"):false %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <%=SPFunctions.LocalizeUI("lbl_Bank_Amount", "CyberportEMS_CPIP_Reimbursement") %>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtAmount" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Amount")))?Convert.ToDouble(Eval("Amount")).ToString("f2"):"" %>' runat="server" CssClass="input-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel ID="pnlReimbursementItemTotalInitialGrant" runat="server">
                <div class="row border-gray">
                    <table style="width: 100%" cellpadding="10px">
                        <tr>
                            <th style="text-align: right">
                                <%=SPFunctions.LocalizeUI("lbl_BankNetAmountInitial", "CyberportEMS_CPIP_Reimbursement") %>
                            </th>
                            <th style="text-align: left">$ 50,000.00</th>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlReimbursementItemTotal" Visible="false" runat="server">
                <div class="form-group">
                    <asp:ImageButton ID="btnAddReimbursementItem" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" ValidationGroup="valgroupAddReimbursementItem" />
                    <h2 class="subheading text-left" style="display: inline; margin: 0;">
                        <small><%=SPFunctions.LocalizeUI("lbl_BankAddItems", "CyberportEMS_CPIP_Reimbursement") %></small>
                    </h2>
                    <asp:Label ID="btnReimbursementItemError" CssClass="text-danger" runat="server" />
                </div>
                <div class="row border-gray">
                    <table style="width: 100%" cellpadding="10px">
                        <tr>
                            <th style="text-align: right">
                                <%=SPFunctions.LocalizeUI("lbl_BankTotal", "CyberportEMS_CPIP_Reimbursement") %>
                            </th>
                            <th style="text-align: left">$
                                <asp:Label ID="lbl_BankTotal" runat="server"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <th style="text-align: right">
                                <%=SPFunctions.LocalizeUI("lbl_BankDeduct", "CyberportEMS_CPIP_Reimbursement") %>
                            </th>
                            <th style="text-align: left">$
                                <asp:Label ID="lbl_BankDeduct" runat="server"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <th style="text-align: right">
                                <%=SPFunctions.LocalizeUI("lbl_BankNetAmount", "CyberportEMS_CPIP_Reimbursement") %>
                            </th>
                            <th style="text-align: left">$
                                <asp:Label ID="lbl_BankNetAmount" runat="server"></asp:Label>
                            </th>
                        </tr>
                    </table>
                </div>
            </asp:Panel>


            <asp:Panel runat="server" ID="pnlReimbursementSalary" Visible="false" CssClass="row">
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDirectBankTransfer" CssClass="row">
                <p><b><%=SPFunctions.LocalizeUI("lblHead_BankInfo", "CyberportEMS_CPIP_Reimbursement") %></b></p>
                <div class="border-gray " style="padding-left: 10px; padding-right: 10px;">
                    <div class="row" style="padding-bottom: 0px;">
                        <div class="col-md-4">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_BankName", "CyberportEMS_CPIP_Reimbursement") %></label>
                            </p>
                            <asp:TextBox ID="txtTransferBankName" runat="server" CssClass="input-sm" />
                        </div>
                        <div class="col-md-4">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_BankCode", "CyberportEMS_CPIP_Reimbursement") %></label>
                            </p>
                            <asp:TextBox ID="txtTransferBankCode" runat="server" CssClass="input-sm" />
                        </div>
                        <div class="col-md-4">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_BankAccNum", "CyberportEMS_CPIP_Reimbursement") %></label>
                            </p>
                            <asp:TextBox ID="txtTransferBankAccount" runat="server" CssClass="input-sm" />
                        </div>
                    </div>
                    <div class="row" style="padding-bottom: 0px;">

                        <div class="col-md-4">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_BankSwift", "CyberportEMS_CPIP_Reimbursement") %></label>
                            </p>
                            <asp:TextBox ID="txtTransferBankSwift" runat="server" CssClass="input-sm" />
                        </div>
                        <div class="col-md-4">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_BankAccHolder", "CyberportEMS_CPIP_Reimbursement") %></label>
                            </p>
                            <asp:TextBox ID="txtTransferBankAcHolderName" runat="server" CssClass="input-sm" />
                        </div>
                    </div>
                    <div class="row">
                        <p>
                            <label><%=SPFunctions.LocalizeUI("lbl_Remarks", "CyberportEMS_CPIP_Reimbursement") %></label>
                        </p>
                        <ol class="numberlist" style="margin-top: 0px;">
                            <li>1. <%=SPFunctions.LocalizeUI("lbl_Bank_InitGrant_Remarks1", "CyberportEMS_CPIP_Reimbursement") %></li>
                            <li>2. <%=SPFunctions.LocalizeUI("lbl_Bank_InitGrant_Remarks2", "CyberportEMS_CPIP_Reimbursement") %></li>
                            <li>3. <%=SPFunctions.LocalizeUI("lbl_Bank_InitGrant_Remarks3", "CyberportEMS_CPIP_Reimbursement") %></li>
                        </ol>
                        <p>
                            <asp:CheckBox ID="chk_RemarksInitialGrant" runat="server" />
                            <%=SPFunctions.LocalizeUI("lbl_chk_Remarks", "CyberportEMS_CPIP_Reimbursement") %>
                        </p>

                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />

<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
<script type="text/javascript">
    $(".datepickermmddyyy").datepicker({

        dateFormat: "mm/dd/yy",
        // showButtonPanel: true,
        // changeMonth: true,
        // changeYear: true,
        //changeDate: true,

        //beforeShow: function (el, dp) {
        //    $('.ui-datepicker-calendar').show();
        //    //var datestr;
        //    //console.log(datestr);
        //    //console.log($(this).val());
        //    //console.log(($(this).val()).length);
        //    //if ((datestr = $(this).val()).length > 0) {
        //    //    //var date = datestr.substring(0, 2);
        //    //    //console.log(date);
        //    //    var year = datestr.substring(datestr.length - 4, datestr.length);
        //    //    var month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
        //    //    $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
        //    //    $(this).datepicker('setDate', new Date(year, month, 1));
        //    //}
        //}
        //onClose: function (dateText, inst) {

        //    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
        //    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
        //    $(this).datepicker('setDate', new Date(year, month, 1));

        //}
    });
</script>
