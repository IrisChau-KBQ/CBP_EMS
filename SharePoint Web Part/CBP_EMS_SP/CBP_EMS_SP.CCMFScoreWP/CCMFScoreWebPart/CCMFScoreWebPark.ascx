<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CCMFScoreWebPark.ascx.cs" Inherits="CBP_EMS_SP.CCMFScoreWP.CCMFScoreWebPark.CCMFScoreWebPark" %>



<script type="text/javascript">
     function SelectedChangeMT_CCMFScore(ddl) {


         var lstMT = document.getElementById('<%=lstMT.ClientID%>').value;
        var lstBMTM = document.getElementById('<%=lstBMTM.ClientID%>').value;
        var lstCIPP = document.getElementById('<%=lstCIPP.ClientID%>').value;
        var lstSR = document.getElementById('<%=lstSR.ClientID%>').value;

        var TotalScore = (lstMT * 0.3) + (lstBMTM * 0.3) + (lstCIPP * 0.3) + (lstSR * 0.1);
        document.getElementById('<%=lblTotalScore.ClientID%>').innerHTML = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(3);
    }

</script>

<style>
    .outsideboard {
            min-width:150px;
            max-width:100%;
            width:auto;
            padding:15px 30px 15px 15px;            
            vertical-align: middle;
        }
        .insideboard {            
            width:300px;
            border:1px solid black;
        }

.center {
    text-align: center;    }

.rTable {
  	display: table;
  	width: 100%;
}
.rTableRow {
  	display: table-row;
}

.rTableCell{
  	display: table-cell;
    padding: 5px 10px;
}

.ddplist {
    max-width: 150px;
    min-width: 90px;
    width: 90px !important;
}

.divother{  	    
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
    max-width:100%;
    min-width:100%;
    width: 200px !important;
    max-height:250px;
    min-height:150px;
    height:150px !important;
}

    .messagered {
        color:red;
    }

.divMargin {
    margin-bottom:6px;
}

</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<asp:Panel ID="MainPanel" runat="server" Visible="true">
<div class="card-theme page_main_block" style="width: 100%">
    <div class="outsideboard greenheaderborder">
        <div>
            <asp:Panel ID="panel1" runat="server" CssClass="aspPanel">
                <div class="divother">
                    <h2 class="titleFont textGreenFont">CCMF Score</h2>
                </div>
                <br />
                <div class="rTableRow">
                    <div class="rTableCell">Role </div>
                    <div class="rTableCell">
                        <asp:Label ID="lblrole" runat="server" Text=""></asp:Label></div>
                </div>
                <hr />
                <div class="divMargin">
                    <asp:Button ID="btnDownload" runat="server" Text="DownLoad" OnClick="btnDownload_Click" ValidationGroup="DownloadZip" CssClass="apply-btn bluetheme"  />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZip" Display="Dynamic"></asp:CustomValidator>
                    <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlueFont textFont300">Management Team (30%)</div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
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
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlueFont textFont300">Business Model & Time to Market (30%)</div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
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
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlueFont textFont300">Creativty and Innovation of the Proposed Project, Product and Service (30%)</div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                        <asp:DropDownList ID="lstCIPP" runat="server" onchange="SelectedChangeMT_CCMFScore(this)" CssClass="ddplist">
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
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlueFont textFont300">Social Responsibility (10%)</div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
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
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlueFont textFont300">Total Score</div>
                    </div>
                    <div class="rTableCell">
                        <asp:Label ID="lblTotalScore" runat="server" Text="000"></asp:Label></div>
                </div>

                <div class="rTableRow">
                    <div class="rTableCell">
                        <div class="margin1 textBlueFont textFont300">Comments</div>
                    </div>
                    <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                        <asp:DropDownList ID="lstcomment" runat="server">
                            <asp:ListItem Value="Business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                            <asp:ListItem Value="Creativity" Text="Creativity"></asp:ListItem>
                            <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                            <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="divother">
                    <div class="textBlueFont">Remarks</div>
                    <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
                    <div class="textBlueFont">Score criteria: 0 - not available, 1 - fair, 2 - good, 3 - very good</div>
                </div>
                <br /><br />
                <div style="text-align: left">
                    <asp:Label ID="lblmessage" runat="server" Text="" CssClass="messagered" />
                    <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn skytheme button_width160" OnClick="btnsubmit_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
</asp:Panel>
