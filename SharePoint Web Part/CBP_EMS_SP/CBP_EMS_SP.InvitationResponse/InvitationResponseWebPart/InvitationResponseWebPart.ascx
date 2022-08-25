<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvitationResponseWebPart.ascx.cs" Inherits="CBP_EMS_SP.InvitationResponse.InvitationResponseWebPart.InvitationResponseWebPart" %>




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

    .blue {
        background-color: #58a1e4 !important;
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

    .msg {
        padding-left: 9px;
        padding-top: 6px;
    }

    .emscontent input.apply-btn.btnvideo {
        font-size: 10px;
        padding: 9px 0px;
        margin-bottom: 5px;
        border: none;
    }

    .removePadding{
        padding: 0px !important;
    }
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<script>
    $(document).ready(function () {
        <% if (m_PopUpStatus)
           { %>
        $("#PopUpIR").modal("show");
        <% } %>

        <% if (m_SubmitValidation)
           { %>
        $("#PopUpIRConfirm").modal("show");
        <% } %>
        <% if (m_SubmitPresention)
           { %>
        $("#PopUpUloadConfirm").modal("show");
        <% } %>
        <% if (m_SubmitVideo)
           { %>
        $("#PopUpVideoConfirm").modal("show");
        <% } %>

    });

</script>
<asp:Panel ID="PanelIR" runat="server">
    <div class="outsideboard page_main_block" style="width: 100%">
        <div class="divother">
            <h2 class="titleFont">Invitation Response</h2>
        </div>
        <br />
        <!-- <asp:Label ID="lbltest" runat="server" Text="" Visible="true"></asp:Label> -->
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
                    Venue
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblVenue" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
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
            </div>

            <div class="rTableRow">
                <div class="rTableCell textBlueFont">
                    Application No.
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblApplicationNo" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                </div>
                <div class="rTableCell textBlueFont">
                    <asp:Label ID="lblCompanyNameLabel" runat="server" Text="Company Name"></asp:Label>
                    <asp:Label ID="lblProjectNameLabel" runat="server" Text="Project Name" Visible="false"></asp:Label>
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblCompanyName" runat="server" Text="" CssClass="textBlackFont"></asp:Label>
                    <asp:Label ID="lblProjectName" runat="server" Text="" Visible="false" CssClass="textBlackFont"></asp:Label>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlueFont">
                    <asp:Label ID="lblProgrammeTypeLabel" runat="server" Text="Programme Type" Visible="false"></asp:Label>
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblProgrammeType" runat="server" Text="" Visible="false" CssClass="textBlackFont"></asp:Label>
                </div>
                <div class="rTableCell textBlueFont">
                    <asp:Label ID="lblApplicationTypeLabel" runat="server" Text="Application Type" Visible="false"></asp:Label>
                </div>
                <div class="rTableCell">
                    <asp:Label ID="lblApplicationType" runat="server" Text="" Visible="false" CssClass="textBlackFont"></asp:Label>
                </div>
            </div>

        </div>
        <hr />

        <div class="rTable table2">
            <div class="divother">
                <div class="spmDeadline">
                    <asp:Label ID="lblConfirmationDeadline" runat="server" Text="Confirmation Deadline: " CssClass="textBlueFont"></asp:Label>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell removePadding">

                    <div class="rTable">
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div class="margindiv textBlackFont"><font color='red'>*</font>Name of Principal Applicant</div>
                                <asp:TextBox ID="txtNameofPrincipalApplicant" runat="server" CssClass="inputStyle"></asp:TextBox>
                                <div>
                                    <asp:Label ID="lblerrorNameOfApplicant" runat="server" Text="" ForeColor="red"></asp:Label>
                                </div>
                            </div>

                            <div class="rTableCell">
                                <div class="margindiv textBlackFont">Email</div>
                                <div>
                                    <asp:Label ID="txtEmail" runat="server" Text=""></asp:Label>
                                    <div>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="The email cannot empty" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" ValidationGroup ="FileUpload"> </asp:RequiredFieldValidator>--%>
                                        <%--<asp:CustomValidator ID="CustomValidatorEmail" runat="server" ErrorMessage="The email format is Incorrect" ValidationGroup ="FileUpload" Display="Dynamic" ControlToValidate="HiddenFieldtxtEmail" ForeColor="Red" OnServerValidate="CustomValidatorEmail_ServerValidate" ></asp:CustomValidator>--%>
                                    </div>
                                </div>
                            </div>

                            <div class="rTableCell">
                                <div class="margindiv textBlackFont"><font color='red'>*</font>Mobile Number</div>
                                <div>
                                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="inputStyle"></asp:TextBox>
                                    <div>
                                        <asp:CustomValidator ID="CustomValidatorMobileNumber" runat="server" ErrorMessage="the mobile number can only contain numbers  and - ( ) + " ValidationGroup="group1" Display="Dynamic" ControlToValidate="txtMobileNumber" ForeColor="Red" OnServerValidate="CustomValidatorMobileNumber_ServerValidate"></asp:CustomValidator>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblerrorMoble" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="rTableCell textBlackFont">
                                <div class="Attenddiv ">Attend</div>
                                <div>
                                    <asp:RadioButtonList ID="radioAttend" runat="server" RepeatDirection="Horizontal" CssClass="listcss">
                                        <asp:ListItem Text="Yes" Value="1" Selected></asp:ListItem>
                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="divother">
                <div class="margindiv textBlackFont">
                    <font color='red'>*</font>Name of Attendees (First Name, Last Name, Title, Email, Mobile Number)
            <asp:Label ID="lblerrorAttendee" runat="server" Text="" ForeColor="red"></asp:Label>

                </div>
                <div>
                    <asp:TextBox ID="txtNameofAttendees" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="lblErrorNameofAttendees" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="divother">
                <div class="margindiv textBlackFont">
                    <font color='red'>*</font>Type of Presentation Tools            
            <asp:Label ID="lblerrorTool" runat="server" Text="" ForeColor="red"></asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="txtTypeofPresentationTools" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="lblErrorTypeofPresentationTools" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="divother">
                <div class="margindiv textBlackFont">Special Request</div>
                <div>
                    <asp:TextBox ID="txtSpecialRequest" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea" placeholder="Please provide Skype ID if you are using Skype for presentation"></asp:TextBox>
                    <span class="textBlackFont"><font color='red'>*</font>Required inputs</span>
                </div>
            </div>
            <div class="rTable">
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margindiv">
                            <div>
                                <span class="textBlackFont">i have read the Personal Information Collection Statement of Hong Kong Cyberport Management Company Limited (“HKCMCL”) 香港數碼港管理有限公司, including the information about the use of my personal data in direct marketing, and I understand its contents. By ticking the box below, I signify my consent for HKCMCL to use my personal data (primarily my name and contact details) in direct marketing services, products, facilities, activities and other subjects to me (primarily services, products, facilities, activities, events and subjects offered in relation to HKCMCL or Cyberport or partners of Cyberport or shops or merchants in Cyberport or offered or hosted at Cyberport) as more particularly set out in the Personal Information Collection Statement. For more on privacy policy, please refer to our website at</span>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.cyberport.hk/en/privacy_policy" Target="_blank">http://www.cyberport.hk/en/privacy_policy</asp:HyperLink>
                                <span class="textBlackFont">.</span>
                            </div>
                            <div style="margin-top: 20px">
                                <asp:CheckBox ID="CheckBoxCF" runat="server" AutoPostBack="false" CssClass="listcss" Text="I agree to receive direct marketing information." />
                                <%--<asp:Label ID="Label1" runat="server" Text="I agree to receive direct marketing information." CssClass="textBlackFont"></asp:Label>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="text-align: left; margin-top: 50px;" class="btnBox">
                <asp:HiddenField ID="HiddenFieldApplicationID" runat="server" />
                <asp:HiddenField ID="HiddenFieldProgrammeID" runat="server" />
                <%--<asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn skytheme"  data-toggle="modal" data-target="#PopUpIRConfirm" />--%>
                <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn skytheme" OnClick="btnsubmit_Click1" ValidationGroup="group1" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="apply-btn greentheme" ValidationGroup="1" OnClick="btnCancel_Click" />
                <div>
                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" CssClass="msg"></asp:Label>
                </div>
            </div>
            <div>
                <asp:Label ID="lblerror" runat="server" Text="" ForeColor="red"></asp:Label>
            </div>

            <hr>
            <div class="divother">
                <h2 class="titleFont">Submit Presentation Material</h2>
                <div class="spmDeadline">
                    <asp:Label ID="lbldeadline" runat="server" Text="Presentation Material Submission Deadline: " CssClass="textBlueFont"></asp:Label>
                    <asp:HiddenField ID="HiddenFieldisOverDeadline" runat="server" />
                </div>
            </div>
            <div class="rTable">
                <div class="rTableRow">
                    <div class="rTableCell" style="width: 340px;">
                        <div class="margindiv textBlackFont">Video Clip</div>
                        <div>
                            <asp:TextBox ID="txtVideoClip" runat="server" placeholder="Please input hyperlink of the vido" Width="216px" CssClass="inputStyle"></asp:TextBox>
                            <asp:Button ID="btnVideo" runat="server" Text="Submit" CssClass="apply-btn skytheme btnvideo" ValidationGroup="videoclip" OnClick="btnVideo_Click" />
                        </div>
                        <div>
                            <asp:Label ID="lblMvideo" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div><%--<asp:RegularExpressionValidator CssClass="text-danger" ErrorMessage='Video Clip format is incorrect' ValidationGroup="FileUpload" ValidationExpression="^(https?)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$" ControlToValidate="txtVideoClip" runat="server" />--%></div>
                    </div>

                    <div class="rTableCell" style="width: 409px;">
                        <div class="margindiv textBlackFont">Upload Presentation Slide</div>
                        <div>
                            <asp:FileUpload ID="FileUploadPresentation" runat="server" CssClass="FileUploadinputStyle" />
                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="btnImageUpload" OnClick="btnImageUpload_Click" name="btnImageUpload" ValidationGroup="FileUpload" />
                            <asp:CustomValidator ID="CustomValidatorFileUpload" runat="server" ErrorMessage="" ValidationGroup="FileUpload" Display="Dynamic" ControlToValidate="FileUploadPresentation" ForeColor="Red" OnServerValidate="CustomValidatorFileUpload_ServerValidate"></asp:CustomValidator>

                            <div>
                                <asp:Repeater ID="RepeaterLinks" runat="server" OnItemCommand="RepeaterLinks_ItemCommand">
                                    <ItemTemplate>
                                        <div style="display: inline-block;">
                                            <asp:HyperLink ID="HyperLinkFileName" runat="server" Target="_blank" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "NavigateUrl") %>' Text='<%# DataBinder.Eval(Container.DataItem, "Text") %>'></asp:HyperLink>
                                            <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandArgument="<%# Container.ItemIndex %>" ForeColor="Red" runat="server" />
                                            <asp:HiddenField ID="HiddenFieldAttachmentId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AttachmentId") %>' />
                                            <asp:HiddenField ID="HiddenFieldAttachmentPath" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AttachmentPath") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>


                            </div>
                        </div>
                        <div>
                            <asp:Label ID="lblmsgUpload" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <div class="rTableCell FileUploadInfo">
                        <div>
                            <asp:Label ID="lblFileUploadInfo" runat="server" Text="(File Type is limited to PPT, PPTX, PDF, PNG, JPG, GIF, tiff, HTML, odt, PAGES, wmv, avi, MP4, m4a, mov. File size is limited to 5MB)" CssClass="textBlackFont"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

        </div>
</asp:Panel>
<!-- Modal -->
<div class="modal fade" id="PopUpIR" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Invitation Response</h4>
            </div>
            <div class="modal-body">
                <div>
                    <asp:Label ID="lblPopUptext" runat="server" Text=""></asp:Label>
                </div>

            </div>
            <div class="modal-footer">
                <asp:Button ID="btnPopUpIR" runat="server" Text="OK" CssClass="btn btn-default" OnClick="btnPopUpIR_Click" />
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="PopUpIRConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Invitation Response Confirm</h4>
            </div>
            <div class="modal-body">
                <div>
                    <asp:Label ID="Label1" runat="server" Text="Confirm submit ?"></asp:Label>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="apply-btn greentheme" data-dismiss="modal">Close</button>
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="apply-btn bluetheme" OnClick="btnsubmit_Click" ValidationGroup="group1" />
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="PopUpUloadConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Invitation Response Confirm</h4>
            </div>
            <div class="modal-body">
                <div>
                    <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em" />
                    <asp:Label ID="Label2" runat="server" Text="Presentation slide successfully uploaded."></asp:Label>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="apply-btn greentheme" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="PopUpVideoConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Invitation Response Confirm</h4>
            </div>
            <div class="modal-body">
                <div>
                    <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em" />
                    <asp:Label ID="Label3" runat="server" Text="Video clip successfully uploaded."></asp:Label>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="apply-btn greentheme" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>
