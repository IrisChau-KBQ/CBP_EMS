<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VMCreateWP.ascx.cs" Inherits="CBP_EMS_SP.VMCreateWP.VMCreateWP.VMCreateWP" %>
<asp:HiddenField ID="hdn_VMID" runat="server" />
<asp:HiddenField ID="hdn_Old_Leader" runat="server" />

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
        /*max-width: 150px;*/
        min-width: 80px;
        /*width: 80px !important;*/
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

    .VTMBox {
        /*display: -webkit-inline-box;*/
    }

    .VTMlstLeft, .VTMlstRight {
        display: inline;
        float: left;
    }

    .griidview th, .griidview td {
        padding: 10px;
        text-align: left;
    }

    .blue {
        background-color: #58a1e4 !important;
    }

    .SelectedCount {
        margin-right: 25px;
    }

    .margin1 {
        margin-bottom: 15px;
    }

    .margin2 {
        margin-top: 50px;
    }
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<script>
    function modalDisable() {
        setTimeout(function () {
            $(".modal").hide();
        }, 300);

    }

    $(document).ready(function () {
        //$(".SharePointTime select").after('<img src="/_layouts/15/Images/CBP_Images/Intrnal%20Use-Drop%20Down.png" style="vertical-align:top; margin-left:5px;">');
        setTimeout(function () {
            $(".modal").hide();
        }, 300);

        setTimeout(function () {
            $("#ems-customheader .loading img").hide();
        }, 300);

        $(".DeadlinePicker .ms-dtinput a").click(function () {
            setTimeout(function () {
                location.hash = "Deadline";
                var path = $(location).attr('href');
                path = path.replace("#Deadline", "");
                history.replaceState("", "", path);
            }, 300);
        });

        <% if(m_VMStatus == "1"){%>
        $(".DeadlinePicker .ms-dtinput a").attr("en")
        <% } %>

        
    });

</script>
<div class="outsideboard card-theme page_main_block" style="width: 100%">

        <asp:Panel ID="panel1" runat="server" CssClass="aspPanel">
        <div>
            <div class="divother">
                <h2 class="titleFont">Vetting Meeting Arrangement</h2>
            </div>

            <div class="rTable" style="width:604px;">
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Programme Name</div>
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="lstCyberportProgramme" runat="server" CssClass="ddplist" DataTextField="ProgrammeName" DataValueField="ProgrammeName" AutoPostBack="true" OnSelectedIndexChanged="lstCyberportProgramme_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div>
                                <asp:Label ID="lblErrorlstCyberportProgramme" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Intake Number</div>
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="lstIntakeNumber" runat="server" CssClass="ddplist" DataTextField="IntakeNumber" DataValueField="IntakeNumber" AutoPostBack="false" OnSelectedIndexChanged="lstIntakeNumber_SelectedIndexChanged">
                            </asp:DropDownList>
                            <div>
                                <asp:Label ID="lblErrorIntakeNumber" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="rTable" style="width:654px;">
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Date</div>
                        <div class="margin1">
                            <SharePoint:DateTimeControl ID="DatePicker" runat="server" Calendar="Gregorian" DateOnly="true" IsRequiredField="false" ErrorMessage="Vetting Meeting Date is required." OnDateChanged="DatePicker_DateChanged" CalendarImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" CssClassTextBox="inputStyle" />
                            <asp:CompareValidator ID="valDate" runat="server" ControlToValidate="DatePicker$DatePickerDate" Type="Date" Operator="DataTypeCheck" Display="Dynamic" ErrorMessage="Please enter an valid date" Text="Please enter an valid date" ForeColor="Red" ValidationGroup="submitVM"></asp:CompareValidator>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Date must be input" Display="Dynamic" ValidationGroup="submitVM" ForeColor="Red" ControlToValidate="DatePicker$DatePickerDate"></asp:RequiredFieldValidator>
                            <div>
                                <asp:Label ID="lblerrordateinfo" runat="server" Text="" ForeColor="red"></asp:Label>

                            </div>
                        </div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Venue</div>
                        <div class="margin1">
                            <asp:TextBox ID="TxtVenue" runat="server" CssClass="inputStyle"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorTxtVenue" runat="server" ErrorMessage="Venue must be input" Display="Dynamic" ValidationGroup="submitVM" ForeColor="Red" ControlToValidate="TxtVenue"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Time Interval</div>
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="ddlTimeInterval" runat="server">
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="35" Value="35"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </div>
                    </div>
                </div>
            </div>

            <div class="rTable" style="width:594px;">
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Vetting Meeting From</div>
                        <div class="margin1 selectboxcolor selectboxheight SharePointTime">
                            <SharePoint:DateTimeControl ID="VMFrom" runat="server" TimeOnly="true" IsRequiredField="false" OnDateChanged="VMFrom_DateChanged" />
                        </div>
                    </div>

                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Vetting Meeting To</div>
                        <div class="margin1 selectboxcolor selectboxheight SharePointTime">
                            <SharePoint:DateTimeControl ID="VMTo" runat="server" TimeOnly="true" IsRequiredField="false" OnDateChanged="VMTo_DateChanged" />
                            <asp:Label ID="validate_msg1" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                        </div>

                    </div>
                </div>

                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Presentation From</div>
                        <div class="margin1 selectboxcolor selectboxheight SharePointTime">
                            <SharePoint:DateTimeControl ID="PresentFm" runat="server" TimeOnly="true" IsRequiredField="false" OnDateChanged="PresentFm_DateChanged" />
                        </div>
                    </div>

                    <div class="rTableCell">
                        <div class="margin1 textBlackFont">Presentation To</div>
                        <div class="margin1 selectboxcolor selectboxheight SharePointTime">
                            <SharePoint:DateTimeControl ID="PresentTo" runat="server" TimeOnly="true" IsRequiredField="false" OnDateChanged="PresentTo_DateChanged" />
                            <div><asp:Label ID="validate_msg2" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label></div>
                            <div><asp:Label ID="validate_msgMP" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label></div>
                            <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                        </div>

                    </div>
                </div>


            </div>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="rTable DeadlinePicker">
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div class="margin1 textBlackFont">Confirmation Deadline</div>
                                <div class="margin1 selectboxcolor selectboxheight" style="display: flex;">
                                    <a name="Deadline"></a>
                                    <SharePoint:DateTimeControl ID="ConfirmDeadlinePicker" runat="server" Calendar="Gregorian" DateOnly="True" IsRequiredField="false" ErrorMessage="Vetting Meeting Date is required." OnDateChanged="DatePicker_DateChanged" CalendarImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" CssClassTextBox="inputStyle"/>
                                    <SharePoint:DateTimeControl ID="ConfirmDeadlineTimePicker" runat="server" TimeOnly="true" IsRequiredField="false" OnDateChanged="ConfirmDeadlineTimePicker_DateChanged" />                                   
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ConfirmDeadlinePicker$ConfirmDeadlinePickerDate" Type="Date" Operator="DataTypeCheck" Display="Dynamic" ErrorMessage="Please enter an valid date" Text="Please enter an valid date" ForeColor="Red" ValidationGroup="submitVM"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Date must be input" Display="Dynamic" ValidationGroup="submitVM" ForeColor="Red" ControlToValidate="ConfirmDeadlinePicker$ConfirmDeadlinePickerDate"></asp:RequiredFieldValidator>
                                    <div>
                                        <asp:Label ID="lblerrorCondateinfo" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div class="margin1 textBlackFont">Presentation Material Submission Deadline</div>
                                <div class="margin1 selectboxcolor selectboxheight" style="display: flex;">
                                    <a name="Deadline"></a>
                                    <SharePoint:DateTimeControl ID="SubDeadlinePicker" runat="server" Calendar="Gregorian" DateOnly="True" IsRequiredField="false" ErrorMessage="Vetting Meeting Date is required." OnDateChanged="DatePicker_DateChanged" CalendarImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" CssClassTextBox="inputStyle"  />
                                    <SharePoint:DateTimeControl ID="SubDeadlineTimePicker" runat="server" TimeOnly="true" IsRequiredField="false" OnDateChanged="SubDeadlineTimePicker_DateChanged" />                                   

                                    <asp:CompareValidator ID="valSubDeadline" runat="server" ControlToValidate="SubDeadlinePicker$SubDeadlinePickerDate" Type="Date" Operator="DataTypeCheck" Display="Dynamic" ErrorMessage="Please enter an valid date" Text="Please enter an valid date" ForeColor="Red" ValidationGroup="submitVM"></asp:CompareValidator>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Date must be input" Display="Dynamic" ValidationGroup="submitVM" ForeColor="Red" ControlToValidate="SubDeadlinePicker$SubDeadlinePickerDate"></asp:RequiredFieldValidator>
                                    <div>
                                        <asp:Label ID="lblerrorSubdateinfo" runat="server" Text="" ForeColor="red"></asp:Label>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div class="margin1 textBlackFont">Parking Request Deadline</div>
                                <div class="margin1">
                                    <a name="Deadline"></a>
                                    <SharePoint:DateTimeControl ID="ParkingDeadlinePicker" runat="server" Calendar="Gregorian" DateOnly="true" IsRequiredField="false" ErrorMessage="Vetting Meeting Date is required." OnDateChanged="DatePicker_DateChanged" CalendarImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" CssClassTextBox="inputStyle"  />
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ParkingDeadlinePicker$ParkingDeadlinePickerDate" Type="Date" Operator="DataTypeCheck" Display="Dynamic" ErrorMessage="Please enter an valid date" Text="Please enter an valid date" ForeColor="Red" ValidationGroup="submitVM"></asp:CompareValidator>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Date must be input" Display="Dynamic" ValidationGroup="submitVM" ForeColor="Red" ControlToValidate="ParkingDeadlinePicker$ParkingDeadlinePickerDate"></asp:RequiredFieldValidator>
                                    <div>
                                        <asp:Label ID="lblerrorParkingdateinfo" runat="server" Text="" ForeColor="red"></asp:Label>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<div class="rTableRow">
            <div class="rTableCell"> </div>
            <div class="rTableCell">
                <SharePoint:PeopleEditor ID="spVTLeader" runat="server" Width="350" MultiSelect="false" SelectionSet="User" AcceptAnyEmailAddresses="true" />
            </div>
            
        </div>--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="rTable">
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div class="margin1 textBlackFont">Vetting Team Leader</div>
                                <div class="margin1 selectboxcolor selectboxheight">
                                    <asp:DropDownList ID="ddlVettingTeamLeader" runat="server" DataTextField="FieldText" DataValueField="FieldValue" OnSelectedIndexChanged="ddlVettingTeamLeader_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    
                                    <div>
                                        <asp:Label ID="lblerrorVettingTeamLeader" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="rTable">
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div class="margin1 textBlackFont">Vetting Team Member</div>
                                <div class="VTMBox margin1">
                                    <div class="VTMlstLeft selectboxcolor">
                                        <asp:ListBox ID="lstLeft" runat="server" SelectionMode="multiple" Rows="5" Width="270"></asp:ListBox>
                                    </div>
                                    <div class="btnBox VTMbtn" style="float: left;">
                                        <div>
                                            <asp:Button ID="btnLeft" runat="server" Text="<" OnClick="btnLeft_Click" CausesValidation="false" CssClass="btnmove apply-btn bluetheme" />
                                        </div>
                                        <div>
                                            <asp:Button ID="btnRight" runat="server" Text=">" OnClick="btnRight_Click" CausesValidation="false" CssClass="btnmove apply-btn bluetheme" />
                                        </div>
                                    </div>
                                    <div class="VTMlstRight">
                                        <asp:ListBox ID="lstRight" runat="server" SelectionMode="multiple" Rows="5" DataTextField="FieldText" DataValueField="FieldValue" Width="270"></asp:ListBox>
                                        <asp:Panel ID="lstPanel" runat="server" Visible="false">
                                            <asp:ListBox ID="lstoldlist" runat="server" SelectionMode="multiple" Rows="5" Width="270"></asp:ListBox>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div>
                                    <asp:CustomValidator ID="CustomValidatorVettingTeamMember" runat="server" ErrorMessage="Vetting Team Member cannot contain the vetting Team Leader" ValidationGroup="submitVM" ForeColor="Red" OnServerValidate="CustomValidatorVettingTeamMember_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                                    <asp:Label ID="validate_msg3" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                                </div>

                            </div>
                        </div>


                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnLeft" />     
                    <asp:PostBackTrigger ControlID="btnRight" />     
                    <asp:PostBackTrigger ControlID="ddlVettingTeamLeader" />                    
                </Triggers>
            </asp:UpdatePanel>

        </div>

        <div style="text-align: left; margin-top: 20px;" >
            <asp:Button ID="btnPreviewEmail" runat="server" Text="Preview email" CssClass="apply-btn bluetheme" OnClick="btnPreviewtEmail_Click"  OnClientClick="modalDisable()" />
            <asp:Button ID="btnSendInvitation" runat="server" Text="Invite vetting team" CssClass="apply-btn bluetheme" OnClick="btnSendInvitation_Click" OnClientClick="modalDisable()" />
            <asp:Label ID="lblInviteStatus" runat="server" Text="" />

        </div>

        <div style="text-align: left; margin-top: 50px;" class="btnBox">
            <asp:Button ID="btnsubmit" runat="server" Text="Save" CssClass="apply-btn bluetheme" OnClick="btnsubmit_Click" ValidationGroup="submitVM" OnClientClick="modalDisable()" />
            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="apply-btn skytheme" OnClick="btnConfirm_Click" ValidationGroup="submitVM" OnClientClick="modalDisable()" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="apply-btn greentheme" OnClick="btnCancel_Click" CausesValidation="false" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" data-toggle="modal" data-target="#DeleteMessagebox" CssClass="apply-btn bluetheme" />
        </div>
    </asp:Panel>
    <asp:Label ID="lbltest" runat="server" Text=""  />
</div>

<!-- Modal -->
<div class="modal fade" id="DeleteMessagebox" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Vettiing Meeting</h4>
      </div>
      <div class="modal-body">
        Confirm to Delete Veeting Meeting?
      </div>
      <div class="modal-footer">
        <asp:Button ID="btnDeletePop" runat="server" Text="Yes" CssClass="apply-btn greentheme button_width160" OnClick="btnDeletePop_Click" />
        <asp:Button ID="btnCancelPop" runat="server" Text="No"  data-dismiss="modal" CssClass="apply-btn graytheme button_width160" />
      </div>
    </div>
  </div>
</div>

