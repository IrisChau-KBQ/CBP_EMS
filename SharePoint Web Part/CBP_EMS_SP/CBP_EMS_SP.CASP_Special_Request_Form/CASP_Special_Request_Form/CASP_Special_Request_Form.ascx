<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CASP_Special_Request_Form.ascx.cs" Inherits="CBP_EMS_SP.CASP_Special_Request_Form.VisualWebPart1.VisualWebPart1" %>
<asp:HiddenField ID="hdn_ProgramID" runat="server" />
<asp:HiddenField ID="hdn_ApplicationID" runat="server" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />
<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->

        <div class="custom-form-wd-img border-gray boxcenter width-80 pagewhiteblock">
            <asp:Panel ID="pnl_programDetail" runat="server">

                <h1 class="form__h1">Special Request Form (CASP)
                </h1>

                <div class="form__upr">


                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Applicant", "CyberportEMS_Common") %></div>
                        <div class="col-md-3 word-wrap">
                            <asp:Label ID="lblApplicant" runat="server" Font-Bold="true" Text="" CssClass="word-wrap"></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblApplicationNo" Font-Bold="true" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_LastSubmit", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblLastSubmitted" runat="server" Font-Bold="true" Text=""></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"></div>
                        <div class="col-md-3 boldgraylbl"></div>
                    </div>
                </div>
            </asp:Panel>


            <div class="form">
                <h2><%=SPFunctions.LocalizeUI("Part_A", "CyberportEMS_CASP_SR") %></h2>
                <div class="row">
                    <div class="row">
                        <div class="col-md-6 selectboxheight">
                            <p><%=SPFunctions.LocalizeUI("Part_A_Company_Name", "CyberportEMS_CASP_SR") %></p>
                            <asp:DropDownList runat="server" ID="ddlcompanyname" Style="height: 40px"></asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <p><%=SPFunctions.LocalizeUI("Part_A_Contact_Name", "CyberportEMS_CASP_SR") %></p>
                            <asp:TextBox ID="txtContactName" runat="server" CssClass="input-sm"></asp:TextBox>
                            <asp:Label runat="server" ID="lblContactName" Text="" Font-Bold="true" Visible="false"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <p><%=SPFunctions.LocalizeUI("Part_A_Phone", "CyberportEMS_CASP_SR") %></p>
                            <asp:TextBox runat="server" ID="txtPhoneNo" CssClass="input-sm"></asp:TextBox>
                            <asp:Label runat="server" ID="lblPhoneNo" Font-Bold="true" Visible="false"></asp:Label>


                        </div>

                        <div class="col-md-6">
                            <p><%=SPFunctions.LocalizeUI("Part_A_Email", "CyberportEMS_CASP_SR") %></p>
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="input-sm"></asp:TextBox>
                            <asp:Label runat="server" ID="lblEmail" Font-Bold="true" Visible="false"></asp:Label>
                      <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid Email." ControlToValidate="txtEmail" ForeColor="Red" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />

                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 selectboxheight">
                            <p><%=SPFunctions.LocalizeUI("Part_A_AcceleratorProgramme", "CyberportEMS_CASP_SR") %></p>
                            <asp:DropDownList runat="server" ID="ddlProgrammeattended" Style="height: 40px"></asp:DropDownList>
                        </div>
                        <div class="col-md-6 ">
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <h2><%=SPFunctions.LocalizeUI("Part_B", "CyberportEMS_CASP_SR") %></h2>
                    <div class="row">
                        <div class="col-md-6">
                            <label><%=SPFunctions.LocalizeUI("Part_B_ServiceProviderName", "CyberportEMS_CASP_SR") %></label>
                            <asp:TextBox runat="server" ID="txtServiceProviderName" CssClass="input-sm"></asp:TextBox>
                            <asp:Label runat="server" ID="lblServiceProviderName" Font-Bold="true" Visible="false"></asp:Label>
                        </div>

                        <div class="col-md-6">
                            <label><%=SPFunctions.LocalizeUI("Part_B_EstimatedAmount", "CyberportEMS_CASP_SR") %></label>
                            <asp:TextBox runat="server" ID="txtEstimatedAmount" CssClass="input-sm">  </asp:TextBox>
                            <asp:Label runat="server" ID="lblEstimatedAmount" Font-Bold="true" Visible="false">  </asp:Label>
                             <asp:RegularExpressionValidator runat="server" ID="rgamount" ControlToValidate="txtEstimatedAmount" ErrorMessage="Invalid Amount" ForeColor="Red" ValidationExpression="((\d+)((\.\d{1,2})?))$" ></asp:RegularExpressionValidator>
                        </div>
                    </div>

                </div>

                <div class="form-group">

                    <label><%=SPFunctions.LocalizeUI("Part_B_Purpose", "CyberportEMS_CASP_SR") %></label>
                    <asp:TextBox runat="server" ID="txtPurpose" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    <asp:Label runat="server" ID="lblPurpose" Font-Bold="true" Visible="false"></asp:Label>
                </div>
                <div class="form-group">

                    <label><%=SPFunctions.LocalizeUI("Part_B_Description", "CyberportEMS_CASP_SR") %></label>
                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    <asp:Label runat="server" ID="lblDescription" Font-Bold="true" Visible="false"></asp:Label>
                </div>

                <div class="form-group">

                    <label><%=SPFunctions.LocalizeUI("Part_B_Justification", "CyberportEMS_CASP_SR") %></label>
                    <asp:TextBox runat="server" ID="txtjustification" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    <asp:Label runat="server" ID="lbljustification" Font-Bold="true" Visible="false"></asp:Label>
                </div>

                <p><strong></strong><%=SPFunctions.LocalizeUI("Remark", "CyberportEMS_CASP_SR") %></p>
                <asp:Panel runat="server" ID="pnlSupportingDocument">
                    <div class="dirbox">
                        <label><%=SPFunctions.LocalizeUI("SupportingDocument", "CyberportEMS_CASP_SR") %></label>
                        <br />
                        <asp:FileUpload runat="server" ID="Fu_SupportingDocuments" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                        <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton14" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="1" />
                        <br />
                        <asp:Repeater runat="server" ID="rptrdocuments" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                            <HeaderTemplate>
                                <table style="width: 100%;" cellpadding="1">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 60%">
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
                        <asp:Label Text="" ID="lblfiletype" runat="server" CssClass="text-gray-client" />
                        <br />
                        <asp:Label Text=" " ID="lblfilesize" runat="server" CssClass="text-gray-client" />
                        <br />
                        <asp:Label Text="" ID="lbldocuments" runat="server" CssClass="text-danger" />

                    </div>
                </asp:Panel>


                <br />
                <br />
                <asp:Button ID="btn_Save" runat="server" Text="Save" CssClass="apply-btn greentheme" OnClick="btn_Save_Click" />

                <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="apply-btn bluetheme" OnClick="btn_Submit_Click" />
                <br />
                <div id="lbl_success" style="color: green" runat="server"></div>
                <div id="lbl_Exception" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>

            </div>
        </div>
    </div>
</div>

<asp:Panel ID="UserSubmitPasswordPopup" runat="server" Visible="false" DefaultButton="btn_submitFinal">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                <%=SPFunctions.LocalizeUI("Submission_popup", "CyberportEMS_CASP_SR") %> (<asp:Label Text="" runat="server" ID="lblSubmissionApplication" />), <%=SPFunctions.LocalizeUI("Submission_popup_password", "CyberportEMS_CASP") %>
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
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton3" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <asp:Label Text="" runat="server" ID="lblappsucess" />
                <%-- <%=SPFunctions.LocalizeUI("Application_submitted_confirmation", "CyberportEMS_Common") %>--%>
            </p>



        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlRestricted" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton PostBackUrl="~/SitePages/Home.aspx" ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton4" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color: red">
                <%=SPFunctions.LocalizeUI("Application_submitted_Restriction", "CyberportEMS_Common") %>
            </p>



        </div>
    </div>
</asp:Panel>
<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
<style id='hideMonth'></style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>



