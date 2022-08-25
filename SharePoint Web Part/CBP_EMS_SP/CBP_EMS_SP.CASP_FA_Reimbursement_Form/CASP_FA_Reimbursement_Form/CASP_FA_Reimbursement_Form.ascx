<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CASP_FA_Reimbursement_Form.ascx.cs" Inherits="CBP_EMS_SP.CASP_FA_Reimbursement_Form.VisualWebPart1.VisualWebPart1" %>
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />
<script
    src="https://code.jquery.com/jquery-2.2.4.min.js"
    integrity="sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44="
    crossorigin="anonymous"></script>
<style>
    .marginR3 {
        margin-right: 3px !important;
    }

</style>

<asp:HiddenField ID="hdn_ApplicationID" runat="server" />
<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->

        <div class="custom-form-wd-img border-gray boxcenter width-80 pagewhiteblock">
            <asp:Panel ID="pnl_programDetail" runat="server">

                <h1 class="form__h1"><%=SPFunctions.LocalizeUI("Header", "CyberportEMS_CASP_Reimbursement") %>
                </h1>

                <div class="form__upr" style="margin-bottom: 20px">
                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Applicant", "CyberportEMS_Common") %></div>
                        <div class="col-md-3 word-wrap">
                            <asp:Label ID="lblApplicant" runat="server" Font-Bold="true" CssClass="word-wrap"></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblApplicationNo" Font-Bold="true" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_LastSubmit", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblLastSubmitted" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"></div>
                        <div class="col-md-3 boldgraylbl"></div>
                    </div>
                </div>
            </asp:Panel>


            <div class="">
                <asp:Panel runat="server" ID="pnl_FormOutside">
                    <div class="form-group margin-bottom10 border-bottom" style="padding-bottom: 20px;">
                        <div><%=SPFunctions.LocalizeUI("Rdo_Category", "CyberportEMS_CASP_Reimbursement") %></div>

                        <asp:RadioButtonList runat="server" CssClass="radiocss" ID="rdo_Categories" OnSelectedIndexChanged="rdo_Categories_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3">
                        </asp:RadioButtonList>

                    </div>
                    <div class="row">
                        <p><%=SPFunctions.LocalizeUI("lbl_Header", "CyberportEMS_CASP_Reimbursement") %> </p>

                        <p><%=SPFunctions.LocalizeUI("lbl_undersignDeclaration", "CyberportEMS_CASP_Reimbursement") %> </p>
                    </div>
                    <div class="row">
                        <div class="col-md-4 selectboxheight">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_CompanyName", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:DropDownList runat="server" ID="ddlcompanyname" Style="height: 40px"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_ChequePaybleTo", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:Label ID="lblcheque" CssClass="label-text" Font-Bold="true" Visible="false" runat="server"></asp:Label>
                            <asp:TextBox runat="server" ID="txtcheque" CssClass="input-sm"></asp:TextBox>
                        </div>
                        <div class="col-md-4 selectboxheight">
                            <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_ApplicationProgrammeAttended", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:DropDownList runat="server" ID="ddlProgrammeattended" Style="height: 40px; width: 100%;"></asp:DropDownList>
                        </div>

                    </div>

                    <asp:Panel ID="pnlPreApproveSR" Visible="false" runat="server" CssClass="selectboxheight">
                        <p><%=SPFunctions.LocalizeUI("lbl_PreApproved", "CyberportEMS_CASP_Reimbursement") %></p>

                        <asp:DropDownList runat="server" ID="ddl_preapproved"></asp:DropDownList>

                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnl_Section2">
                        <div class="row">
                            <p>
                                <asp:CheckBox runat="server" ID="chkbx_prepaidservice" CssClass="listcss prepaidService" Text="Items are prepaid service and service is not completed" />
                            </p>
                            <div class="prepaidServiceItems">
                                <div class="col-md-4">
                                    <p>
                                        <label><%=SPFunctions.LocalizeUI("lbl_EstimatedServiceFrom", "CyberportEMS_CASP_Reimbursement") %></label>
                                    </p>
                                    <asp:TextBox Width="40%" runat="server" ID="txtServiceperiodfrom" autocomplete="off" Enabled="false" CssClass="input-sm datepickerDMonthY"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblServiceperiodfrom" Font-Bold="true" CssClass="label-text" Visible="false"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <p><%=SPFunctions.LocalizeUI("lbl_EstimatedServiceTo", "CyberportEMS_CASP_Reimbursement") %></p>
                                    <asp:TextBox Width="40%" runat="server" ID="txtServiceperiodTo" autocomplete="off" Enabled="false" CssClass="input-sm datepickerDMonthY"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblServiceperiodTo" Font-Bold="true" CssClass="label-text" Visible="false"></asp:Label>
                                </div>
<%--                                 <div class="col-md-4" />--%>
                           </div>                          
                        </div>
                        <div class="row" /> 

                        <div class="row">
                            <div class="col-md-4">
                                <p>
                                    <label><%=SPFunctions.LocalizeUI("lbl_ServiceProviderName", "CyberportEMS_CASP_Reimbursement") %></label>
                                </p>
                                  <asp:TextBox runat="server" ID="txtServiceProviderName" CssClass="input-sm"></asp:TextBox>
                                <asp:Label runat="server" ID="lblServiceProviderName" Font-Bold="true" CssClass="label-text" Visible="false"></asp:Label>

                            </div>

                            <div class="col-md-4">
                                <p>
                                <label><%=SPFunctions.LocalizeUI("lbl_ServiceContract", "CyberportEMS_CASP_Reimbursement") %></label>
                                </p>
                                    <asp:TextBox runat="server" ID="txtServiceContractName" CssClass="input-sm"></asp:TextBox>
                                <asp:Label runat="server" ID="lblServiceContractName" Font-Bold="true" CssClass="label-text" Visible="false"></asp:Label>

                            </div>

                            <div class="col-md-4">
                                <p>
                                    <label><%=SPFunctions.LocalizeUI("lbl_TotalFee", "CyberportEMS_CASP_Reimbursement") %></label>
                                </p>
                                   <asp:TextBox runat="server" ID="txtServiceTotalFee" CssClass="input-sm"></asp:TextBox>
                                <asp:Label runat="server" ID="lblServiceTotalFee" Font-Bold="true" CssClass="label-text" Visible="false"></asp:Label>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <p>
                                    <asp:CheckBox runat="server" ID="chkbx_freelance" CssClass="listcss freelancerService" Text="Service provider is freelancer" />
                                </p>
                            </div>

                            <div class="col-md-4">
                                <p class="freelancerServiceItem">
                                    <%=SPFunctions.LocalizeUI("lbl_FreelancerResume", "CyberportEMS_CASP_Reimbursement") %>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:FileUpload runat="server" ID="Fu_FreelanceResume" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" Enabled="false" runat="server" ID="ImageButton17" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="16" />

                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label Text="" ID="lblResumeError" runat="server" CssClass="text-danger" />
                                            </td>
                                        </tr>
                                    </table>
                                </p>
                             <asp:Repeater runat="server" ID="rptr_Resume" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 40%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            </div>



                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnl_ReimbursementItems" Style="border: 1px solid #999999; padding: 5px; margin: 10px 0px;">

                        <asp:GridView ID="grdReimbursementItems" OnRowDataBound="grdReimbursementItems_RowDataBound" OnRowCommand="grdReimbursementItems_RowCommand" runat="server" AutoGenerateColumns="False"
                            ShowHeader="true"
                            GridLines="None" Width="100%">
                            <Columns>

                                <asp:TemplateField>

                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_Reimbursement_Date", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdn_Item_ID" Value='<%#Eval("Item_ID") %>' runat="server" />
                                        <asp:TextBox ID="txt_Date" autocomplete="off" Width="90px" Text='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Date")))?(Convert.ToDateTime(Eval("Date")).ToString("dd/MMM/yyyy")):string.Empty %>' runat="server" CssClass="input-sm datepickerDMonthY" />
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Date")))?(Convert.ToDateTime(Eval("Date")).ToString("dd/MMM/yyyy")):string.Empty %>' CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage='Date Required' CssClass="text-danger" Display="Dynamic" ValidationGroup="newReimbursementItemsValidation" ControlToValidate="txt_Date" runat="server" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_Reimbursement_ItemDescription", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox Width="80%" ID="txtItemDescription" Text='<%#Eval("Description") %>' runat="server" CssClass="input-sm" />
                                        <asp:Label ID="lblItemDescription" Text='<%#Eval("Description") %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage='Item/Description Required' CssClass="text-danger" Display="Dynamic" ValidationGroup="newReimbursementItemsValidation" ControlToValidate="txtItemDescription" runat="server" />
                                        </p>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_ForAdvertisement", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAdvertisement" Checked='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Advertisement")))?(bool)Eval("Advertisement"):false %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_Amount", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox Width="80%" ID="txtAmount" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Amount")))?Convert.ToDouble(Eval("Amount")).ToString("F2"):"" %>' runat="server" CssClass="input-sm clsamount" />
                                        <asp:Label ID="lblAmount" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Amount")))?Convert.ToDouble(Eval("Amount")).ToString("#,#.00"):"" %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage='Amount Required' CssClass="text-danger" Display="Dynamic" ValidationGroup="newReimbursementItemsValidation" ControlToValidate="txtAmount" runat="server" />
                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="Invalid Amount" ValidationGroup="newReimbursementItemsValidation" ControlToValidate="txtAmount" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_GridRemove" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="form-group">
                            <asp:ImageButton ID="btn_AddReimbursementItems" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" ValidationGroup="newReimbursementItemsValidation" OnClick="btn_AddReimbursementItems_Click" />
                            <h2 class="subheading text-left" style="display: inline; margin: 0;"></h2>
                            <asp:Label ID="lblgriderror" CssClass="text-danger" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnl_ReimbursementSalary" Style="border: 1px solid #999999; padding: 5px; margin: 10px 0px;">

                        <asp:GridView ID="grdReimbursementSalary" runat="server" OnRowDataBound="grdReimbursementSalary_RowDataBound" OnRowCommand="grdReimbursementSalary_RowCommand" AutoGenerateColumns="False"
                            ShowHeader="true"
                            GridLines="None" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_InternsName", "CyberportEMS_CASP_Reimbursement") %></div> 
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdn_Item_ID" Value='<%#Eval("Salary_ID") %>' runat="server" />
                                        <asp:TextBox ID="txtname" Width="200px" Text='<%#Eval("Intern_Name")%>' runat="server" CssClass="input-sm" />
                                        <asp:Label ID="lblname" Text='<%#Eval("Intern_Name")%>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage="Intern's Name Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newReimbursementSalaryValidation"
                                                ControlToValidate="txtname" runat="server" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_MonthlySalary", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox Width="70px" ID="txtmonthlysalary" Text='<%#Eval("Monthly_Salary") %>' runat="server" CssClass="input-sm clssal" />
                                        <asp:Label ID="lblmonthlysalary" Text='<%#Eval("Monthly_Salary") %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage="Monthly Salary Required"  CssClass="text-danger" Display="Dynamic"
                                                ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtmonthlysalary" runat="server" />
                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="Invalid Monthly Salary"
                                                ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtmonthlysalary"   CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                        <asp:RangeValidator ID="validateSalaryRange" Display="Dynamic" CssClass="text-danger" Type="Double" MinimumValue="0" MaximumValue="9000"
                                                 ErrorMessage="Maximum allow salary is 9,000.00" ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtmonthlysalary" runat="server" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_MonthlySalaryFrom", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox Width="90px" ID="txtsalaryfrom" Text='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Period_From")))?(Convert.ToDateTime(Eval("Period_From")).ToString("dd/MMM/yyyy")):string.Empty %>' runat="server" CssClass="input-sm datepickerDMonthY" />
                                        <asp:Label ID="lblsalaryfrom" Text='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Period_From")))?(Convert.ToDateTime(Eval("Period_From")).ToString("dd/MMM/yyyy")):string.Empty %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage="Salary From Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newReimbursementSalaryValidation"
                                                ControlToValidate="txtsalaryfrom" runat="server" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_MonthlySalaryTo", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox Width="90px" ID="txtsalaryto" Text='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Period_To")))?(Convert.ToDateTime(Eval("Period_To")).ToString("dd/MMM/yyyy")):string.Empty %>' runat="server" CssClass="input-sm datepickerDMonthY" />
                                        <asp:Label ID="lblsalaryto" Text='<%# !string.IsNullOrEmpty( Convert.ToString( Eval("Period_To")))?(Convert.ToDateTime(Eval("Period_To")).ToString("dd/MMM/yyyy")):string.Empty %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage="Salary From To" CssClass="text-danger" Display="Dynamic" ValidationGroup="newReimbursementSalaryValidation"
                                                ControlToValidate="txtsalaryto" runat="server" />
                                        </p>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_Tax", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTax" Width="70px" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("MPF")))?Convert.ToDouble(Eval("MPF")).ToString("#,#.00"):"" %>' runat="server" CssClass="input-sm clsmpf" />
                                        <asp:Label ID="lblTax" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("MPF")))?Convert.ToDouble(Eval("MPF")).ToString("#,#.00"):"" %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage="MPF Required" CssClass="text-danger" Display="Dynamic"
                                                ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtTax" runat="server" />
                                            <asp:RangeValidator CssClass="text-danger" ID="validateMPFRange" Display="Dynamic" MinimumValue="0" MaximumValue="450" Type="Double"
                                                 ErrorMessage="Maximum allow MPF is 450.00" ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtTax" runat="server" />
                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="Invalid MPF"
                                                ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtTax" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="text-align:left;"><%=SPFunctions.LocalizeUI("Gv_Total", "CyberportEMS_CASP_Reimbursement") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" Width="100px" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Amount")))?Convert.ToDouble(Eval("Amount")).ToString("F2"):"" %>' runat="server" CssClass="input-sm clsamount" />
                                        <asp:Label ID="lblAmount" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Amount")))?Convert.ToDouble(Eval("Amount")).ToString("#,#.00"):"" %>' runat="server" CssClass="label-text" Font-Bold="true" Visible="false"></asp:Label>
                                        <p>
                                            <asp:RequiredFieldValidator ErrorMessage="Total Required" CssClass="text-danger" Display="Dynamic"
                                                ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtAmount" runat="server" />
                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="Invalid Total"
                                                ValidationGroup="newReimbursementSalaryValidation" ControlToValidate="txtAmount" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                        </p>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_GridCRemove" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="form-group">
                            <asp:ImageButton ID="btn_AddReimbursementSalary" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" ValidationGroup="newReimbursementSalaryValidation" OnClick="btn_AddReimbursementSalary_Click" />
                            <h2 class="subheading text-left" style="display: inline; margin: 0;">
                                <small></small>
                            </h2>
                            <asp:Label CssClass="text-danger" ID="lbl_gridcError" runat="server"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnl_ReimbursementCalcSummary" Style="border: 1px solid #999999; padding: 5px; margin: 10px 0px;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 80%; text-align: right;">
                                    <asp:Label ID="lbl_CalcTotal" runat="server" /></td>
                                <td style="width: 50px; text-align: center">:</td>
                                <td>
                                    <asp:Label ID="lbl_CalcTotalVal" CssClass="clctotal" runat="server" Text="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80%; text-align: right;">
                                    <asp:Label ID="lbl_CalcDeduction" runat="server" />
                                </td>
                                <td style="width: 50px; text-align: center">:</td>
                                <td>
                                    <asp:Label ID="lbl_CalcDeductionVal" CssClass="clcdeduction" runat="server" Text="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80%; text-align: right;">
                                    <asp:Label ID="lbl_CalcTotalReimbursement" runat="server" />

                                </td>
                                <td style="width: 50px; text-align: center">:</td>
                                <td>
                                    <asp:Label ID="lbl_CalcTotalReimbursementVal" CssClass="clcreimbursement" runat="server" Text="0" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="panel_F_Declaration">
                        <div class="row">
                            <asp:RadioButtonList CssClass="radiocss rboConflicts" ID="rdoFDeclarationofConflicts" CellSpacing="5" runat="server">
                        </asp:RadioButtonList>
                            <asp:Label runat="server" ID="lblconflicts" CssClass="label-text" Visible="false"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" CssClass="txtConfli" placeholder="Conflict Detail" TextMode="MultiLine" ID="txtconflicts"></asp:TextBox>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnl_DeclareA" runat="server">
                        <p style="min-height: 167px;">
                            <asp:CheckBox runat="server" ID="chkDeclareA" CssClass="listcss" Text="" />
                        </p>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnl_DeclareB">
                        <p style="min-height: 125px;">
                            <asp:CheckBox runat="server" ID="chkDeclareB" CssClass="listcss" Text="" />
                        </p>
                    </asp:Panel>
                    <p><%=SPFunctions.LocalizeUI("lbl_Remarks", "CyberportEMS_CASP_Reimbursement") %></p>
                    <p><%=SPFunctions.LocalizeUI("lbl_Supporting", "CyberportEMS_CASP_Reimbursement") %></p>

                    <asp:Panel runat="server" ID="pnl_Documents" BorderWidth="1" BorderColor="Black">

                        <asp:Panel ID="pnlOriginalReceipt" CssClass="col-md-4 marginR3" runat="server">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_Document_OriginalReceipt", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_OriginalReceipt" AllowMultiple="false" accept=".pdf,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                               <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton1" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="1" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrOriginalReceipt" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%-- <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_OriginalReceipterr" runat="server" CssClass="text-danger" />
                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnlOriginal_Invoice" CssClass="col-md-4 marginR3">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_Document_OriginalInvoice", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_Original_Invoice" accept=".pdf,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton2" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="2" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrOriginal_Invoice" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_Original_Invoiceerr" runat="server" CssClass="text-danger" />
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlQuotation">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_Quotation", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_Quotation" accept=".pdf,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton3" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="3" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrQuotation" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_Quotationerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlEventPhoto">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_EventPhoto", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_EventPhoto" accept=".pdf,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton4" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="4" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_EventPhoto" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_EventPhotoerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlEventPass">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_EventPass", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_EventPass" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton5" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="5" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_EventPass" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_EventPasserr" runat="server" CssClass="text-danger" />
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlPrintSample">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_PrintSample", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_PrintSample" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton6" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="6" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_PrintSample" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_PrintSampleerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlBoardingPass">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_BoardingPass", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_BoardingPass" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton7" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="7" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_BoardingPass" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_BoardingPasserr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlflight_Inventory">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_FlightInventory", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="fu_flight_Inventory" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton8" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="8" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrfu_flight_Inventory" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="fu_flight_Inventoryerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlBusinessCard">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_BusinesCard", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_BusinessCard" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton9" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="9" />
                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_BusinessCard" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_BusinessCarderr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlInternPayroll">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_internPayroll", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_InternPayroll" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton10" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="10" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_InternPayroll" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_InternPayrollerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlInternCertification">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_InternAcademicCertification", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_InternCertification" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton11" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="11" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_InternCertification" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_InternCertificationerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlPaymentProof">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_MpfPaymentProof", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_PaymentProof" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton12" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="12" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_PaymentProof" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_PaymentProoferr" runat="server" CssClass="text-danger" />

                        </asp:Panel>


                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlEmployementContract">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_EmployementContract", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_EmployementContract" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton13" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="13" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_EmployementContract" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_EmployementContracterr" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlResume">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_ResumeCV", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_Resume" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton14" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="14" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_Resume" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_Resumeerr" runat="server" CssClass="text-danger" />

                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="pnlHKIDCard">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_HKIDCard", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_HKIDCard" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton15" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="15" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_HKIDCard" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="Fu_HKIDCarderr" runat="server" CssClass="text-danger" />

                        </asp:Panel>
                        <div style="clear: both"></div>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnl_DocumentsSubsequent" runat="server" Visible="false" >
                        <p><%=SPFunctions.LocalizeUI("lbl_SubsequentSupportDocument", "CyberportEMS_CASP_Reimbursement") %></p>
                        <asp:Panel runat="Server" CssClass="row" ID="Pnl_subsequentSupportDecument" BorderWidth="1" BorderColor="Black">
                            <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="Panel1">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_PaySlip", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_EmpPayslip" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton18" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="17" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_EmpPayslip" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="lblEmpPayslip" runat="server" CssClass="text-danger" />

                        </asp:Panel>
                              <asp:Panel runat="server" CssClass="col-md-4 marginR3" ID="Panel2">
                            <p>
                                <label style="margin-left:10px"><%=SPFunctions.LocalizeUI("pnl_SaleryAdjustment", "CyberportEMS_CASP_Reimbursement") %></label>
                            </p>
                            <asp:FileUpload runat="server" ID="Fu_SalaryProof" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton19" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="18" />

                            <br />
                            <asp:Repeater runat="server" ID="rptrFu_SalaryProof" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="width: 100%;" cellpadding="1">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                        </td>
                                        <td>
                                         

                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <asp:Label Text="" ID="lblSalaryProofError" runat="server" CssClass="text-danger" />

                        </asp:Panel>

                             <div style="clear: both"></div>

                        </asp:Panel>
                         <div style="clear: both"></div>
                    </asp:Panel>

                    <br />
                    <div style="clear: both"></div>
                </asp:Panel>
                <br />
                <p style="color: red"><%=SPFunctions.LocalizeUI("lbl_warning", "CyberportEMS_CASP_Reimbursement") %></p>
                <br />
                <asp:Button ID="btn_Save" runat="server" Text="Save" CssClass="apply-btn greentheme" OnClick="btn_Save_Click" />

                <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="apply-btn bluetheme" OnClick="btn_Submit_Click1" />
                <br />
                <div id="lbl_success" style="color: green" runat="server"></div>
                <div id="lbl_Exception" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                <label class="" style="margin-top: 3%; right: 12%; position: absolute; font-size: x-small; color: #999999;"><%=SPFunctions.LocalizeUI("lbl_DocRef", "CyberportEMS_CASP_Reimbursement") %></label>

                <br />

            </div>

        </div>
    </div>
</div>


<asp:Panel ID="UserSubmitPasswordPopup" runat="server" Visible="false" DefaultButton="btn_submitFinal">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                <%=SPFunctions.LocalizeUI("Submission_popup", "CyberportEMS_CASP_Reimbursement") %> (<asp:Label Text="" runat="server" ID="lblSubmissionApplication" />), <%=SPFunctions.LocalizeUI("Submission_popup_password", "CyberportEMS_CASP_Reimbursement") %>
            </p>

            <div class="table full-width">
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Email" ID="txtLoginUserName" runat="server" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Password" TextMode="Password" ID="txtLoginPassword" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="bluetheme" />
                    <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Submit" runat="server" CssClass="greentheme" />
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
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton16" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <asp:Label Text="text" runat="server" ID="lblappsucess" />
                <%-- <%=SPFunctions.LocalizeUI("Application_submitted_confirmation", "CyberportEMS_Common") %>--%>
            </p>



        </div>
    </div>
</asp:Panel>


<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
<style id='hideMonth'></style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
<style>
    .ui-datepicker-trigger {
        vertical-align: top;
    }
</style>
<script>
    $(".datepickerDMonthY").datepicker({

        dateFormat: "dd/M/yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        changeDate: true,
        showOn: 'both',
        constrainInput: false,
        buttonImageOnly: true,
        buttonImage: '/_layouts/15/images/CBP_Images/Calender.png'
   ,
        beforeShow: function (el, dp) {
            $('.ui-datepicker-calendar').show();
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
    $(".datepickerDMonthY").keydown(false);
   
   
        $('.clsamount').change(function (e) {
            var rad = $('input[name=rdo_Categories]:checked').val()

            var sum = 0;
            $("input[class *= 'clsamount']").each(function () {
                sum += +$(this).val();
            });

            if (rad != "c") {
                var calcdeductionval = parseFloat(sum * 25) / 100;
                var calcReimbursementval = parseFloat(sum * 75) / 100;
            }
            else {
                var calcReimbursementval = parseFloat(sum * 50) / 100;

            }
            $('.clctotal').html("$" + sum);
            $('.clcdeduction').html("$" + calcdeductionval);
            $('.clcreimbursement').html("$" + calcReimbursementval);

        }
        );

        //$('.clssal').change(function (e) {
        //    $('.Errsal').remove();
        //        var max = parseFloat('9000');
        //        if ($(this).val() > max)
        //        {
        //            $(this).val() == 0;
        //            $(this).after('<p class="Errsal" style="color:red;">Maximum allow salary is 9,000.00</p>')
        //        }
        //});

        //$('.clsmpf').change(function (e) {
        //    $('.Errmpf').remove();
        //    var max = parseFloat('450');
        //    if ($(this).val() > max)
        //    {
        //        $(this).val() == "";
        //        $(this).after('<p  class="Errmpf"style="color:red;">Maximum allow MPF is 450.00</p>')
        //    }

        //});
    
        $( window ).load(function() {

            $("input[class *= 'clsamount']").each(function () {
                var rad = $('input[name=rdo_Categories]:checked').val()

                var sum = 0;
                $("input[class *= 'clsamount']").each(function () {
                    sum += +$(this).val();
                });

                if (rad != "c") {
                    var calcdeductionval = parseFloat(sum * 25) / 100;
                    var calcReimbursementval = parseFloat(sum * 75) / 100;
                }
                else {
                    var calcReimbursementval = parseFloat(sum * 50) / 100;

                }
                $('.clctotal').html("$" + sum);
                $('.clcdeduction').html("$" + calcdeductionval);
                $('.clcreimbursement').html("$" + calcReimbursementval);

            }
);

        });




    $(".prepaidService input[type='checkbox']").click(function (e) {
        if ($(this).is(':checked'))
            $("#<%=txtServiceperiodfrom.ClientID%>,#<%=txtServiceperiodTo.ClientID%>").datepicker("option", "disabled", false);  // checked
            //$("#<%=txtServiceperiodfrom.ClientID%>,#<%=txtServiceperiodTo.ClientID%>").removeAttr("disabled");  // checked
        else
            $("#<%=txtServiceperiodfrom.ClientID%>,#<%=txtServiceperiodTo.ClientID%>").datepicker("option", "disabled", true);  // checked
        // $("#<%=txtServiceperiodfrom.ClientID%>,#<%=txtServiceperiodTo.ClientID%>").attr("disabled", "disabled");  // checked
    });
    $(".freelancerService input[type='checkbox']").click(function (e) {
        if ($(this).is(':checked'))
            $("#<%=Fu_FreelanceResume.ClientID%>,#<%=ImageButton17.ClientID%>").removeAttr("disabled");  // checked
        else
            $("#<%=Fu_FreelanceResume.ClientID%>,#<%=ImageButton17.ClientID%>").attr("disabled", "disabled");  // checked


    });

    $(".rboConflicts input[type='radio']").click(function (e) {
        if ($(this).val() == "true") {

            $(".txtConfli").show();
        } else
            $(".txtConfli").hide();
    });

    $(window).load(function () {
        $(".txtConfli").hide();
        $(".prepaidService input[type='checkbox']").each(function (i, j) {
            if ($(j).prop("checked") == true) {
                $("#<%=txtServiceperiodfrom.ClientID%>,#<%=txtServiceperiodTo.ClientID%>").datepicker("option", "disabled", false);  // checked
                // $("#<%=txtServiceperiodfrom.ClientID%>,#<%=txtServiceperiodTo.ClientID%>").removeAttr("disabled");
            }
        });
        $(".freelancerService input[type='checkbox']").each(function (i, j) {
            if ($(j).prop("checked") == true) {
                $("#<%=Fu_FreelanceResume.ClientID%>,#<%=ImageButton17.ClientID%>").removeAttr("disabled");  // checked
            }
        })

        $(".rboConflicts input[type='radio']").each(function (i, j) {
            if ($(j).prop("checked") == true && $(j).val() == "true") {
                $(".txtConfli").show();
            }
        })
    })
</script>
