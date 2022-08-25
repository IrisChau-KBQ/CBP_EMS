<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ECWebPart.ascx.cs" Inherits="CBP_EMS_SP.ECWebPart.ECWebPart.ECWebPart" %>

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

<asp:Panel ID="MainPanel" runat="server" Visible="true">
   <%-- <ContentTemplate>--%>

        <div class="card-theme page_main_block" style="width: 100%">
            <div class="outsideboard  greenheaderborder" style="width: 100%">
                <div>
                    <asp:Panel ID="panel2" runat="server" CssClass="aspPanel">
                        <div class="divother">
                            <h2 class="titleFont textGreenFont">Eligibility Checking</h2>
                        </div>
                        <br />
                        <div class="divMargin">

                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZipECWP" Display="Dynamic"></asp:CustomValidator>
                            <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
                        </div>
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
                        <hr />
                        <div class="divother textBlueFont">Result</div>
                        <div>
                            <asp:RadioButtonList ID="rbtnresult" runat="server" RepeatColumns="1" RepeatDirection="Vertical" OnSelectedIndexChanged="rbtnresult_SelectedIndexChanged" CssClass="listcss" AutoPostBack="true">
                                <asp:ListItem Value="Eligibility checked" Text="Confirm" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Waiting for response from applicant" Text="Require additional information"></asp:ListItem>
                                <asp:ListItem Value="To be disqualified" Text="To be disqualified"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Button ID="btnDownload" runat="server" Text="DownLoad Attachment" OnClick="btnDownload_Click" CssClass="bluetheme" ValidationGroup="DownloadZipECWP" />
                        </div>
                        <div class="divother">
                            <div class="textBlueFont">Comment for applicants</div>
                            <asp:TextBox ID="txtcommentforapplicants" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="RequiredFieldValidator" ID="RequiredFieldValidator" runat="server" ErrorMessage="Comment for applicants cannot be empty." ControlToValidate="txtcommentforapplicants" Enabled="False" ValidationGroup="ECWebPartGroup"></asp:RequiredFieldValidator>
                        </div>
                        <div class="divother">
                            <div class="textBlueFont">Comment for internal use</div>
                            <asp:TextBox ID="txtcommentforinternaluse" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
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
                            <asp:Button ID="BtnSubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click" CssClass="apply-btn skytheme" ValidationGroup="ECWebPartGroup" OnClientClick="StartLoading()" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    <%--</ContentTemplate>--%>
</asp:Panel>
