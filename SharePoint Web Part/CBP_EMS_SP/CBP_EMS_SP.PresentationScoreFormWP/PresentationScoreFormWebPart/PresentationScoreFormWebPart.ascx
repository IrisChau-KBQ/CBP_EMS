<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresentationScoreFormWebPart.ascx.cs" Inherits="CBP_EMS_SP.PresentationScoreFormWP.PresentationScoreFormWebPart.PresentationScoreFormWebPart" %>



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
        padding: 12px 10px;
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
        padding: 6px 80px 6px 10px;
    }

    .table2 .rTableCell {
        padding-right: 151px;
    }

    .blue1 {
        color: #0072C6;
    }

    .Hideddplist {
        float: right;
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

    .textColor {
        color: #145DAA;
        font-weight: 600;
    }

    .textColorBlack {
        color: #66666b;
        font-weight: 500;
    }

    .textGreenColor {
        color: #80C343;
        font-weight: 600;
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

<script type="text/javascript">
    function SelectedChangeMT_CPIPScore(ddl) {

        var lstqcmt = document.getElementById('<%=lstqcmt.ClientID%>').value;
        var lstcipp = document.getElementById('<%=lstcipp.ClientID%>').value;
        var lstmbv = document.getElementById('<%=lstmbv.ClientID%>').value;
        var lstbhkdti = document.getElementById('<%=lstbhkdti.ClientID%>').value;
        var lstpsmpb = document.getElementById('<%=lstpsmpb.ClientID%>').value;

        //var TotalScore = (lstqcmt * 0.2) + (lstcipp * 0.2) + (lstmbv * 0.3) + (lstbhkdti * 0.1) + (lstpsmpb * 0.2);
        var TotalScore = (lstqcmt * 0.3) + (lstcipp * 0.2) + (lstmbv * 0.2) + (lstbhkdti * 0.2) + (lstpsmpb * 0.1);
        document.getElementById('<%=lblTotalScore.ClientID%>').innerText = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(2);

        document.getElementById('<%=HiddenFieldTatalScore.ClientID%>').value = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(2);
    }

    function SelectedChangeMT_CCMFScore(ddl) {


        var lstMT = document.getElementById('<%=lstMT.ClientID%>').value;
        var lstBMTM = document.getElementById('<%=lstBMTM.ClientID%>').value;
        var lstCCMFcipp = document.getElementById('<%=lstCCMFcipp.ClientID%>').value;
        var lstSR = document.getElementById('<%=lstSR.ClientID%>').value;

        var TotalScore = (lstMT * 0.3) + (lstBMTM * 0.3) + (lstCCMFcipp * 0.3) + (lstSR * 0.1);
        document.getElementById('<%=LabelCCMFTotal.ClientID%>').innerText = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(2);
        document.getElementById('<%=HiddenFieldTatalScore.ClientID%>').value = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(2);
    }

    function getCheckedRadio(item, divID) {
        var commentDiv = document.getElementById(divID);

        if (item.value == 0) {

            document.getElementById(divID).style.display = "block";

        }
        else {
            document.getElementById(divID).style.display = "none";
            $(".chklistUI input:checkbox").each(function (index, item) {
                $(item).prop('checked', false);
            })
        }
    }
    $(window).load(function () {
        if ('<%=hdnApplicationName.Value%>' == 'ccmf') {
            if ('<%=hdnApplicationGoState.Value%>' == "0") {
                document.getElementById("ccmfComment").style.display = "block";
            }
        } else {

            if ('<%=hdnApplicationGoState.Value%>' == "0") {
                document.getElementById("cpipComment").style.display = "block";
            }
        }
    });
</script>
<style>
    .chklistUI input[type='checkbox'] {
    margin-top:-5px;
    margin-right:5px;
    }
    .chklistUI label {
        font-weight:normal;
    }
</style>
<asp:HiddenField ID="hdnApplicationGoState" Value="1" runat="server" />
<asp:HiddenField ID="hdnApplicationName" runat="server" />
<div class="outsideboard card-theme page_main_block" style="width: 100%">
    <div class="divother">
        <h2 class="titleFont">Presentation Scoring Form</h2>
    </div>
    <br />
    <hr />
    <div class="rTable table1">
        <div class="rTableRow">
            <div class="rTableCell textBlueFont">
                Programme Name
            </div>
            <div class="rTableCell">
                <asp:Label ID="lblProgrammeName" runat="server" Text="" CssClass="textBlackFont">  </asp:Label>
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

    <asp:Panel ID="UpdatePanelCPIP" runat="server">
        <div class="rTable">
            <div class="rTableRow">
<%--                <div class="rTableCell textBlackFont">Quality and competence of the management team (20%)</div>--%>
<%--               <div class="rTableCell textBlackFont">Market viability with milestones (30%)</div>--%>
               <div class="rTableCell textBlackFont">Market viability with milestones and contribution to Cyberport’s strategic clusters (30%)</div>
               <div class="rTableCell  selectboxheight">
                    <asp:DropDownList ID="lstqcmt" runat="server" onchange="SelectedChangeMT_CPIPScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
<%--                <div class="rTableCell textBlackFont">Creativity and innovation of the proposed project, product and service (20%)</div>--%>
                <div class="rTableCell textBlackFont">Quality and competence of the management team (20%)</div>
                <div class="rTableCell  selectboxheight">
                    <asp:DropDownList ID="lstcipp" runat="server" onchange="SelectedChangeMT_CPIPScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
<%--                <div class="rTableCell textBlackFont">Market and business viability (30%)</div>--%>
                <div class="rTableCell textBlackFont">Business scalability (20%)</div>
                <div class="rTableCell  selectboxheight">
                    <asp:DropDownList ID="lstmbv" runat="server" onchange="SelectedChangeMT_CPIPScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow ">
<%--                <div class="rTableCell textBlackFont">Benefit to Hong Kong’s digital tech industry (10%)</div>--%>
 <%--                 <div class="rTableCell textBlackFont">Functional prototype or product to solve a real problem (20%)</div>--%>
                <div class="rTableCell textBlackFont">Functional prototype or product secured by design to solve a real problem (20%)</div>
                <div class="rTableCell selectboxheight">
                    <asp:DropDownList ID="lstbhkdti" runat="server" onchange="SelectedChangeMT_CPIPScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
<%--                <div class="rTableCell textBlackFont">Proposed six-monthly milestones for the project or business after admission (20%)</div>--%>
                <div class="rTableCell textBlackFont">Innovativeness (10%)</div>
                <div class="rTableCell selectboxheight">
                    <asp:DropDownList ID="lstpsmpb" runat="server" onchange="SelectedChangeMT_CPIPScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Total Score</div>
                <div class="rTableCell">
                    <asp:Label ID="lblTotalScore" runat="server" Text="0"></asp:Label>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Recommend / Not Recommend</div>
                <div class="rTableCell">
                    <asp:RadioButtonList ID="RadioButtonListGoNotGo" runat="server" RepeatDirection="Horizontal" CssClass="textBlackFont listcss">
                        <asp:ListItem Text="Yes" Value="1" onclick="getCheckedRadio(this,'cpipComment')"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" onclick="getCheckedRadio(this,'cpipComment')"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="cpipComment" style="display: none; min-height: 225px;">
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="TableCell textBlackFont">Comments<br /> If not recommend, please comment on the area(s) for improvement:<br />(Multiple section allowed and answers have no direct relation with the above)</div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:CheckBoxList ID="chkCPIPComments" runat="server" CssClass="right chklistUI" Style="background: none; width: 300px;" RepeatDirection="Vertical">
                                <asp:ListItem Value="Business Viability" Text="Business Viability"></asp:ListItem>
                                <asp:ListItem Value="Unique Market Proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="Creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="divother">
            <h4 class="textBlackFont">Other Comments / Remarks</h4>
            <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
            <div class="textBlueFont">Remarks:The Vetting Team Member shall assess the applications by giving the score 0 to 3 on each criteria:<br /><br />  0 - not available<br />  1 - fair<br />  2 - good<br />  3 - very good</div>
        </div>
        <div style="text-align: left; margin-top: 50px;" class="btnBox">
            <asp:Button ID="btnCPIPSubmit" runat="server" Text="Submit" CssClass="apply-btn skytheme" OnClick="btnCPIPSubmit_Click" ValidationGroup="PLOA" />
            <asp:Button ID="btnCPIPCancel" runat="server" Text="Cancel" CssClass="apply-btn greentheme" ValidationGroup="1" OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>

    <asp:Panel ID="UpdatePanelCCMF" runat="server" Visible="false">
        <div class="rTable">
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Management Team (30%)</div>
                <div class="rTableCell selectboxheight">
                    <asp:DropDownList ID="lstMT" runat="server" onchange="SelectedChangeMT_CCMFScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Business Model & Time to Market (30%)</div>
                <div class="rTableCell selectboxheight">
                    <asp:DropDownList ID="lstBMTM" runat="server" onchange="SelectedChangeMT_CCMFScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Creativty and Innovation of the Proposed Project, Product and Service (30%)</div>
                <div class="rTableCell selectboxheight">
                    <asp:DropDownList ID="lstCCMFcipp" runat="server" onchange="SelectedChangeMT_CCMFScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Social Responsibility (10%)</div>
                <div class="rTableCell selectboxheight">
                    <asp:DropDownList ID="lstSR" runat="server" onchange="SelectedChangeMT_CCMFScore(this)" CssClass="ddplist">
                        <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                        <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                        <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                        <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                        <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                        <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Total Score</div>
                <div class="rTableCell">
                    <asp:Label ID="LabelCCMFTotal" runat="server" Text="0"></asp:Label>
                </div>
            </div>

            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Recommend / Not Recommend</div>
                <div class="rTableCell">
                    <asp:RadioButtonList ID="RadioButtonListCCMFGoNotGo" runat="server" RepeatDirection="Horizontal" CssClass="textBlackFont listcss">
                        <asp:ListItem Text="Yes" Value="1" onclick="getCheckedRadio(this,'ccmfComment')"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" onclick="getCheckedRadio(this,'ccmfComment')"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="ccmfComment" style="display: none; min-height: 225px;">
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="TableCell textBlackFont">Comments<br /> If not recommend, please comment on the area(s) for improvement:<br />(Multiple section allowed and answers have no direct relation with the above)</div>
                    </div>
                    <div class="rTableCell">
                        <asp:CheckBoxList ID="chkCCMFComments" runat="server" CssClass="right chklistUI" Style="background: none; width: 300px;" RepeatDirection="Vertical">
                            <asp:ListItem Value="Business viability" Text="Business Viability"></asp:ListItem>
                            <asp:ListItem Value="Unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                            <asp:ListItem Value="Creativity" Text="Creativity"></asp:ListItem>
                            <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                            <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                    <div style="clear: both"></div>
                </div>
            </div>
        </div>
        <div class="divother">
            <h4 class="textBlackFont">Other Comments / Remarks</h4>
            <asp:TextBox ID="txtCCMFRemart" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
            <div class="textBlueFont">Remarks:The Vetting Team Member shall assess the applications by giving the score 0 to 3 on each criteria:<br /><br />  0 - not available<br />  1 - fair<br />  2 - good<br />  3 - very good</div>
        </div>
        <div style="text-align: left; margin-top: 50px;" class="btnBox">
            <asp:Button ID="btnCCMFSubmit" runat="server" Text="Submit" CssClass="apply-btn skytheme" OnClick="btnCCMFSubmit_Click" ValidationGroup="PLOA" />
            <asp:Button ID="btnCCMFCancel" runat="server" Text="Cancel" CssClass="apply-btn greentheme" ValidationGroup="1" OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlWarning" runat="server" Visible="false">
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton PostBackUrl=<%# "~/SitePages/Presentation%20List%20of%20Applications.aspx?VMID=" + Eval("m_VMID") %> ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="imbClose" OnClick="imbClose_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color:red">
                    <asp:Label Text="You are not able to submit after the vetting team leader has confirmed the result." runat="server" ID="lblMessage" />
                </p>
            </div>
        </div>
    </asp:Panel>

    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text="" style="margin-left:10px; margin-top:10px;"></asp:Label>
    <asp:HiddenField ID="HiddenFieldProgramID" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenFieldMemberEmail" runat="server" />
    <asp:HiddenField ID="HiddenFieldTatalScore" Value="0" runat="server" />
</div>
