<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationListWebPart.ascx.cs" Inherits="CBP_EMS_SP.ApplicationListWP.ApplicationListWebPart.ApplicationListWebPart" %>


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
        width: 50%;
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




<script>
    function PopUpBox(ProDesc) {
        try {
            $("#PopUpProjectDescription .modal-body").html(ProDesc);
            $("#PopUpProjectDescription").modal("show");
        } catch (e) {
            alert(e.message);
        }
    }

    function StartDownload() {
        setTimeout(
            function () {
                _spFormOnSubmitCalled = false;
                _spSuppressFormOnSubmitWrapper = true;
                //location.reload();
            }, 1500);
    }


    $(document).ready(function () {
        $(".savebtn").attr("Style", "width:" + $(".griidviewTable")[0].clientWidth + "px");
    })
    function setFormSubmitToFalse() {
        setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
        return true;
    }
</script>
<asp:HiddenField ID="btnExportPDFList" runat="server" />
<asp:Panel ID="PanelApplist" runat="server">
    <div class="outsideboard  page_main_block" style="width: 100%">
        <div class="divother">
            <span class="heading">
                <h2 class="titleFont heading">Application List</h2>
            </span>
        </div>
        <div class="rTable">
            <div class="rTableRow">
                <div class="rTableCell">
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="lstCyberportProgramme" runat="server" CssClass="ddplist" DataTextField="ProgrammeName" DataValueField="ProgrammeName" AutoPostBack="true" OnSelectedIndexChanged="lstCyberportProgramme_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="rTableCell">
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="lstIntakeNumber" runat="server" CssClass="ddplist" DataTextField="IntakeNumber" DataValueField="IntakeNumber" AutoPostBack="true" OnSelectedIndexChanged="lstIntakeNumber_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="rTableCell">
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="lstCluster" runat="server" CssClass="ddplist" DataTextField="ClusterText" DataValueField="ClusterValue" AutoPostBack="true" OnSelectedIndexChanged="lstCluster_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="rTableCell">
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="lststream" runat="server" CssClass="ddplist" AutoPostBack="true" OnSelectedIndexChanged="lststream_SelectedIndexChanged" Visible="false">
                            <asp:ListItem Text="All Stream" Value="%"></asp:ListItem>
                            <asp:ListItem Text="PRO" Value="Professional"></asp:ListItem>
                            <asp:ListItem Text="YEP" Value="Young Entrepreneur"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="rTableCell">
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList ID="lstStatus" runat="server" CssClass="ddplist" AutoPostBack="true" OnSelectedIndexChanged="lstStatus_SelectedIndexChanged">
                            <asp:ListItem Text="All Status" Value="%"></asp:ListItem>
                            <asp:ListItem Text="Submitted" Value="Submitted"></asp:ListItem>
                            <asp:ListItem Text="Waiting for response from applicant" Value="Waiting for response from applicant"></asp:ListItem>
                            <asp:ListItem Text="Resubmitted information" Value="Resubmitted information"></asp:ListItem>
                            <asp:ListItem Text="Eligibility checked" Value="Eligibility checked"></asp:ListItem>
                            <asp:ListItem Text="To be disqualified" Value="To be disqualified"></asp:ListItem>
                            <asp:ListItem Text="Disqualified" Value="Disqualified"></asp:ListItem>
                            <asp:ListItem Text="Withdraw" Value="Withdraw"></asp:ListItem>
                            <asp:ListItem Text="BDM Reviewed" Value="BDM Reviewed"></asp:ListItem>
                            <asp:ListItem Text="BDM Rejected" Value="BDM Rejected"></asp:ListItem>
                            <asp:ListItem Text="Sr. Mgr. Reviewed" Value="Sr. Mgr. Reviewed"></asp:ListItem>
                            <asp:ListItem Text="CPMO Reviewed" Value="CPMO Reviewed"></asp:ListItem>
                            <asp:ListItem Text="Complete Screening" Value="Complete Screening"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="rTableCell">
                    <div class="margin1  selectboxheight">
                        <asp:DropDownList runat="server" ID="lstSS8" CssClass="ddplist" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="lstSS8_SelectedIndexChanged">
                            <asp:ListItem Text="Smart Space" Value="%"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            <asp:ListItem Text="SS8" Value="SS8"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    </div>

            </div>

            <div class="rTableRow">
                <div class="rTableCell">
                    <asp:Label ID="lbldatetime" runat="server" Text="Deadline: " CssClass="textLightBlueFont"></asp:Label>
                </div>
            </div>

            <div class="rTableRow">
                <div class="rTableCell">
                    <asp:Label ID="lblcount" runat="server" Text="0 applications" CssClass="textLightBlueFont"></asp:Label>
                </div>
            </div>
            <%-- below 2 column is for debug --%>
            <div class="rTableRow">
                <div class="rTableCell">
                    <asp:Label ID="lbldebug" runat="server" Text="All Cluster" CssClass="textLightBlueFont" Visible="false">   </asp:Label>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell">
                    <asp:Label ID="lbldebug2" runat="server" Text="Where" CssClass="textLightBlueFont" Visible="false"></asp:Label>
                </div>
            </div>
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
                        <asp:DropDownList ID="lstSortCluster" runat="server" CssClass="ddplist" AutoPostBack="true" DataTextField="ColumnText" DataValueField="ColumnValue" OnSelectedIndexChanged="lstSortCluster_SelectedIndexChanged">
                            <%--<asp:ListItem Text="Application No" Value="Application_Number" ></asp:ListItem>--%>
                            <%--<asp:ListItem Text="Cluster" Value="Cluster" ></asp:ListItem>--%>
                            <%--<asp:ListItem Text="Status" Value="Status" ></asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div class="rTableCell selectboxheight selectboxGreenIcon">
                        <asp:DropDownList ID="lstSortApplicationNo" runat="server" CssClass="ddplist" AutoPostBack="true" DataTextField="ColumnText" DataValueField="ColumnValue" OnSelectedIndexChanged="lstSortApplicationNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="rTableCell">
                        <asp:RadioButtonList ID="radioSortCluster" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="radioSortCluster_SelectedIndexChanged" CssClass="radio listcss">
                            <asp:ListItem Selected Text="Ascending" Value="asc"></asp:ListItem>
                            <asp:ListItem Text="Descending" Value="desc"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </div>
        <div class="divMargin">
            <asp:Button ID="btnDownload" runat="server" Text="DownLoad Attachments" OnClick="btnDownload_Click" ValidationGroup="DownloadZip" CssClass="apply-btn bluetheme" />
            <asp:Button ID="btnCSV" runat="server" Text="Application Summary" OnClick="btnCSV_Click" CssClass="apply-btn bluetheme" OnClientClick="StartDownload()" />
            <asp:Button ID="BtnDuplicated_Submission" runat="server" Text="Duplicated Submission" OnClick="BtnDuplicated_Submission_Click" CssClass="apply-btn bluetheme" OnClientClick="StartDownload()" />
            <asp:Button ID="btnExportPDF" runat="server" OnClick="btnExportPDF_Click" Text="Batch Print Applications" Visible="false" CssClass="apply-btn bluetheme float-right" OnClientClick="StartDownload()" />
            <%--OnClientClick="javascript:setFormSubmitToFalse()"--%>
        </div>
        <div class="divMargin">
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZip" Display="Dynamic"></asp:CustomValidator>
            <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
        </div>
        <div class="divMargin">
            <asp:Button ID="btnCoordinatorproceedtoBDM" runat="server" Text="Coordinator Proceed to BDM" OnClick="btnCoordinatorproceedtoBDM_Click" CssClass="apply-btn bluetheme" />
            <asp:Button ID="btnBDMproceedtoSeniorManager" runat="server" Text="Proceed to Senior Manager" OnClick="btnBDMproceedtoSeniorManager_Click" CssClass="apply-btn bluetheme" />
            <asp:Button ID="btnSeniorManagerproceedtoCPMO" runat="server" Text="Senior Manager Proceed to CPMO" OnClick="btnSeniorManagerproceedtoCPMO_Click" CssClass="apply-btn bluetheme" />
            <asp:Button ID="btnCPMOproceedtoFinalBDM" runat="server" Text="CPMO Proceed to Final BDM" OnClick="btnCPMOproceedtoFinalBDM_Click" CssClass="apply-btn bluetheme" />
            <asp:Button ID="btnCompleteScreening" runat="server" Text="Complete Screening" OnClick="btnCompleteScreening_Click" CssClass="apply-btn bluetheme" />
        </div>
        <div class="griidview">
            <asp:GridView ID="GridViewApplication" CssClass="griidviewTable" runat="server" CellSpacing="-1" GridLines="None" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                HeaderStyle-CssClass="textBlueFont" RowStyle-CssClass="textBlackFont">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFormatString="{0}" DataNavigateUrlFields="APPNoURL" DataTextField="ApplicationNo" HeaderText="Application No." ItemStyle-CssClass="textGreenFont" />
                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                    <asp:BoundField DataField="ProgrammeType" HeaderText="Programme Type" />
                    <asp:BoundField DataField="HongKongProgrammeStream" HeaderText="Stream" />
                    <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" />
                    <asp:TemplateField HeaderText="ProjectDescription">
                        <ItemTemplate>
                            <asp:HiddenField ID="HiddenProjectDescription" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ProjectDescription") %>' />
                            <asp:ImageButton runat="server" ImageUrl="~/_layouts/15/Images/CBP_Images/SearchIcon.png" OnClientClick='<%# String.Format("PopUpBox(\"{0}\");return false;", ProcessMyDataItem(DataBinder.Eval(Container.DataItem, "ProjectDescription"))) %>' Width="25" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Submission Date">
                        <ItemTemplate>
                            <%# !string.IsNullOrEmpty( Convert.ToString( Eval("Submitted_Date")))?((DateTime)Eval("Submitted_Date")).ToString("yyyy-MM-dd"):string.Empty %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="BDMScore" HeaderText="BDM Score" />
                    <asp:BoundField DataField="SrManagerScore" HeaderText="Sr. Manager Score" />
                    <asp:BoundField DataField="CPMOScore" HeaderText="CPMO Score" />
                    <asp:BoundField DataField="AverageScore" HeaderText="Average Score" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" HtmlEncode="false" />
                    <asp:BoundField DataField="RemarksForVetting" HeaderText="Remarks for Vetting" HtmlEncode="false" />
                    <asp:TemplateField HeaderText="Shortlisted">
                        <ItemTemplate>
                            <div class="listcss">
                                <asp:CheckBox ID="CheckBoxShortlisted" CssClass="greencheckbox" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Shortlisted") %>' Text="&nbsp;" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="SmartSpace" HeaderText="Smart Space" />

                </Columns>

            </asp:GridView>
            <div style="text-align: left; margin-top: 50px;" class="btnBox">
                <asp:Button ID="btnSaveShortlisted" runat="server" Text="Save Shortlist" OnClick="btnSaveShortlisted_Click" CssClass="apply-btn skytheme" Width="186px" />
            </div>
            <asp:Label ID="lbltest" runat="server" Text="" />
            <asp:Button ID="btn" runat="server" Visible="false" OnClick="btn_Click" />
        </div>

    </div>



    <%--<!-- Button trigger modal -->
<button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal">
  Launch demo modal
</button>--%>

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
</asp:Panel>