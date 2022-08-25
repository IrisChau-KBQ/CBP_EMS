<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemarkVWP.ascx.cs" Inherits="CBP_EMS_SP.RemarkVWP.RemarkVWP.RemarkVWP" %>


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

    .Gridwidth {

    }
.commentWidth{
    width:90px;
}
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

  <script type="text/javascript">
    function SelectedChangeMT(ddl) {

        var lstqcmt = document.getElementById('<%=lstqcmt.ClientID%>').value;
        var lstcipp = document.getElementById('<%=lstcipp.ClientID%>').value;
        var lstmbv = document.getElementById('<%=lstmbv.ClientID%>').value;
        var lstbhkdti = document.getElementById('<%=lstbhkdti.ClientID%>').value;
        var lstpsmpb = document.getElementById('<%=lstpsmpb.ClientID%>').value;
        
        var TotalScore = (lstqcmt * 0.2) + (lstcipp * 0.2) + (lstmbv * 0.3) + (lstbhkdti * 0.1) + (lstpsmpb * 0.2);
        document.getElementById('<%=lblBDM_TotalScore_CPIP.ClientID%>').innerHTML = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(2);

        var lblBDM_TotalScore_CPIP = parseFloat(document.getElementById('<%=lblBDM_TotalScore_CPIP.ClientID%>').innerHTML);
        var lblSrMagr_TotalScore_CPIP = parseFloat(document.getElementById('<%=lblSrMagr_TotalScore_CPIP.ClientID%>').innerHTML);
        var lblCPMO_TotalScore_CPIP = parseFloat(document.getElementById('<%=lblCPMO_TotalScore_CPIP.ClientID%>').innerHTML);

        var count = (lblBDM_TotalScore_CPIP == 0 ? 0 : 1) + (lblSrMagr_TotalScore_CPIP == 0 ? 0 : 1) + (lblCPMO_TotalScore_CPIP == 0 ? 0 : 1);
        var totalAverage = (lblBDM_TotalScore_CPIP + lblSrMagr_TotalScore_CPIP + lblCPMO_TotalScore_CPIP) / count;
        document.getElementById('<%=lblAverage_TotalScore_CPIP.ClientID%>').innerHTML = (Math.round(totalAverage * 100) / 100).toFixed(2);

        var id = $(ddl).attr("ID");
        if (id == '<%=lstqcmt.ClientID%>') {
            AverageCalculation('<%=lstqcmt.ClientID%>', '<%=lblSrMagr_lstqcmt.ClientID%>', '<%=lblCPMO_lstqcmt.ClientID%>', '<%=lblAverage_lstqcmt.ClientID%>');
        }

        if (id == '<%=lstcipp.ClientID%>') {
            AverageCalculation('<%=lstcipp.ClientID%>', '<%=lblSrMagr_lstcipp.ClientID%>', '<%=lblCPMP_lstcipp.ClientID%>', '<%=lblAverage_lstcipp.ClientID%>');
        }

        if (id == '<%=lstmbv.ClientID%>') {
            AverageCalculation('<%=lstmbv.ClientID%>', '<%=lblSrMagr_lstmbv.ClientID%>', '<%=lblCPMP_lstmbv.ClientID%>', '<%=lblAverage_lstmbv.ClientID%>');
        }

        if (id == '<%=lstbhkdti.ClientID%>') {
            AverageCalculation('<%=lstbhkdti.ClientID%>', '<%=lblSrMagr_lstbhkdti.ClientID%>', '<%=lblCPMP_lstbhkdti.ClientID%>', '<%=lblAverage_lstbhkdti.ClientID%>');
        }

        if (id == '<%=lstpsmpb.ClientID%>') {
            AverageCalculation('<%=lstpsmpb.ClientID%>', '<%=lblSrMagr_lstpsmpb.ClientID%>', '<%=lblCPMP_lstpsmpb.ClientID%>', '<%=lblAverage_lstpsmpb.ClientID%>');
        }
    }

    function SelectedChangeMT_CCMF(ddl) {


        var lstMT = document.getElementById('<%=lstMT.ClientID%>').value;
        var lstBMTM = document.getElementById('<%=lstBMTM.ClientID%>').value;
        var lstCIPP_CCMF = document.getElementById('<%=lstCIPP_CCMF.ClientID%>').value;
        var lstSR = document.getElementById('<%=lstSR.ClientID%>').value;

        var TotalScore = (lstMT * 0.3) + (lstBMTM * 0.3) + (lstCIPP_CCMF * 0.3) + (lstSR * 0.1);
        document.getElementById('<%=lblBDM_TotalScore_CCMF.ClientID%>').innerHTML = parseFloat(Math.round(TotalScore * 100) / 100).toFixed(3);


        var lblBDM_TotalScore_CCMF = parseFloat(document.getElementById('<%=lblBDM_TotalScore_CCMF.ClientID%>').innerHTML);
        var lblSrMagr_TotalScore_CCMF = parseFloat(document.getElementById('<%=lblSrMagr_TotalScore_CCMF.ClientID%>').innerHTML);
        var lblCPMO_TotalScore_CCMF = parseFloat(document.getElementById('<%=lblCPMO_TotalScore_CCMF.ClientID%>').innerHTML);
        
        var count = (lblBDM_TotalScore_CCMF == 0 ? 0 : 1) + (lblSrMagr_TotalScore_CCMF == 0 ? 0 : 1) + (lblCPMO_TotalScore_CCMF == 0 ? 0 : 1);
        var totalAverage = (lblBDM_TotalScore_CCMF + lblSrMagr_TotalScore_CCMF + lblCPMO_TotalScore_CCMF) / count;
        document.getElementById('<%=lblAverage_TotalScore_CCMF.ClientID%>').innerHTML = (Math.round(totalAverage * 100) / 100).toFixed(3);

        var id = $(ddl).attr("ID");
        if (id == '<%=lstMT.ClientID%>') {
            AverageCalculation('<%=lstMT.ClientID%>', '<%=lblSrMagr_lstMT.ClientID%>', '<%=lblCPMO_lstMT.ClientID%>', '<%=lblAverage_lstMT.ClientID%>');
        }

        if (id == '<%=lstBMTM.ClientID%>') {
            AverageCalculation('<%=lstBMTM.ClientID%>', '<%=lblSrMagr_lstBMTM.ClientID%>', '<%=lblCPMO_lstBMTM.ClientID%>', '<%=lblAverage_lstBMTM.ClientID%>');
        }

        if (id == '<%=lstCIPP_CCMF.ClientID%>') {
            AverageCalculation('<%=lstCIPP_CCMF.ClientID%>', '<%=lblSrMagr_lstCIPP_CCMF.ClientID%>', '<%=lblCPMO_lstCIPP_CCMF.ClientID%>', '<%=lblAverage_lstCIPP_CCMF.ClientID%>');
        }

        if (id == '<%=lstSR.ClientID%>') {
            AverageCalculation('<%=lstSR.ClientID%>', '<%=lblSrMagr_lstSR.ClientID%>', '<%=lblCPMO_lstSR.ClientID%>', '<%=lblAverage_lstSR.ClientID%>');
        }

    }

      function AverageCalculation(BDMID, SrMgrID, CPMOID, AverageID) {
          var BDM = parseFloat(document.getElementById(BDMID).value);
          var SrMgr = parseFloat(document.getElementById(SrMgrID).innerHTML);
          var CPMO = parseFloat(document.getElementById(CPMOID).innerHTML);

          count = (BDM == 0 ? 0 : 1) + (SrMgr == 0 ? 0 : 1) + (CPMO == 0 ? 0 : 1);
          totalAverage = (BDM + SrMgr + CPMO) / count;;
          document.getElementById(AverageID).innerHTML = (Math.round(totalAverage * 100) / 100).toFixed(3);
      }
    

</script>
<asp:Panel ID="MainPanel" runat="server" CssClass="aspPanel" >
    <div class="card-theme page_main_block" style="width: 100%">
        <div class="outsideboard greenheaderborder">
            <div>
                <div class="divother">
                    <h2 class="titleFont textGreenFont">Final BDM remark</h2>
                </div>

                <asp:Panel ID="CCMFScorePanel" runat="server">
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont"><asp:Label ID="Label2" runat="server" Text="" /></div>
                        <div class="rTableCell textBlueFont"><asp:Label ID="Label3" runat="server" Text="BDM" CssClass="textBlueFont" /></div>
                        <div class="rTableCell textBlueFont"><asp:Label ID="Label4" runat="server" Text="Sr. Mgr." CssClass="textBlueFont" /></div>
                        <div class="rTableCell textBlueFont"><asp:Label ID="Label5" runat="server" Text="CPMO" CssClass="textBlueFont" /></div>
                        <div class="rTableCell textBlueFont"><asp:Label ID="Label1" runat="server" Text="Average Score" CssClass="textBlueFont" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell">
                            <div class="margin1 textBlueFont textFont300">Management Team (30%)</div>
                        </div>
                        <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="lstMT" runat="server" onchange="SelectedChangeMT_CCMF(this)" CssClass="ddplist">
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
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstMT" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_lstMT" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstMT" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell">
                            <div class="margin1 textBlueFont textFont300">Business Model & Time to Market (30%)</div>
                        </div>
                        <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="lstBMTM" runat="server" onchange="SelectedChangeMT_CCMF(this)" CssClass="ddplist">
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
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstBMTM" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_lstBMTM" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstBMTM" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell">
                            <div class="margin1 textBlueFont textFont300">Creativty and Innovation of the Proposed Project, Product and Service (30%)</div>
                        </div>
                        <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="lstCIPP_CCMF" runat="server" onchange="SelectedChangeMT_CCMF(this)" CssClass="ddplist">
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
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstCIPP_CCMF" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_lstCIPP_CCMF" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstCIPP_CCMF" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell">
                            <div class="margin1 textBlueFont textFont300">Social Responsibility (10%)</div>
                        </div>
                        <div class="rTableCell">
                        <div class="margin1 selectboxcolor selectboxheight">
                            <asp:DropDownList ID="lstSR" runat="server" onchange="SelectedChangeMT_CCMF(this)" CssClass="ddplist">
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
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstSR" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_lstSR" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstSR" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell">
                            <div class="margin1 textBlueFont textFont300">Total Score</div>
                        </div>
                        <div class="rTableCell"><asp:Label ID="lblBDM_TotalScore_CCMF" runat="server" Text="0"></asp:Label></div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_TotalScore_CCMF" runat="server" Text="0"></asp:Label></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_TotalScore_CCMF" runat="server" Text="0"></asp:Label></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_TotalScore_CCMF" runat="server" Text="0"></asp:Label></div>
                    </div>
                     <div class="rTableRow">
                        <div class="rTableCell">
                            <div class="margin1 textBlueFont textFont300">Comments</div>
                            </div>
                        <div class="rTableCell selectboxheight">
                            <%--<asp:Label ID="lblBDM_Comments_CCMF" runat="server" Text=""></asp:Label>--%>
                            <asp:DropDownList ID="lstBDM_Comments_CCMF" runat="server" CssClass="commentWidth">
                                <asp:ListItem Value="business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell selectboxheight">
                            <asp:Label ID="lblSrMagr_Comments_CCMF" runat="server" Text=""></asp:Label>
                            <%--<asp:DropDownList ID="lstSrMagr_Comments_CCMF" runat="server" CssClass="commentWidth">
                                <asp:ListItem Value="business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </div>
                        <div class="rTableCell selectboxheight">
                            <asp:Label ID="lblCPMO_Comments_CCMF" runat="server" Text=""></asp:Label>
                            <%--<asp:DropDownList ID="lstCPMO_Comments_CCMF" runat="server" CssClass="commentWidth">
                                <asp:ListItem Value="business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:DropDownList>--%>

                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="CPIPScorePanel" runat="server">
                    <div class="rTableRow">
                        <div class="rTableCell"><asp:Label ID="lblTitle" runat="server" Text="" CssClass="textBlueFont" /></div>
                        <div class="rTableCell"><asp:Label ID="lblBDM" runat="server" Text="BDM" CssClass="textBlueFont" /></div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr" runat="server" Text="Sr Magr" CssClass="textBlueFont" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMP" runat="server" Text="CPMO" CssClass="textBlueFont" /></div>
                        <div class="rTableCell"><asp:Label ID="Label6" runat="server" Text="Average Score" CssClass="textBlueFont" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Quality and competence of the management team (20%)</div>
                        <div class="rTableCell selectboxheight">
                            <asp:DropDownList ID="lstqcmt" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
                                <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                                <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                                <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                                <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                                <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                                <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstqcmt" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_lstqcmt" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstqcmt" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Creativity and innovation of the proposed project, product and service (20%)</div>
                        <div class="rTableCell selectboxheight">
                            <asp:DropDownList ID="lstcipp" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
                                <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                                <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                                <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                                <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                                <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                                <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstcipp" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMP_lstcipp" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstcipp" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Market and business viability (30%)</div>
                        <div class="rTableCell selectboxheight">
                            <asp:DropDownList ID="lstmbv" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
                                <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                                <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                                <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                                <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                                <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                                <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstmbv" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMP_lstmbv" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstmbv" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Benefit to Hong Kong’s digital tech industry (10%)</div>
                        <div class="rTableCell selectboxheight">
                            <asp:DropDownList ID="lstbhkdti" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
                <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                                <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                                <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                                <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                                <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                                <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstbhkdti" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMP_lstbhkdti" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstbhkdti" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Proposed six-monthly milestones for the project or business after admission (20%)</div>
                        <div class="rTableCell selectboxheight">
                            <asp:DropDownList ID="lstpsmpb" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
                <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                                <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                                <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                                <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                                <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                                <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_lstpsmpb" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMP_lstpsmpb" runat="server" Text="0" /></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_lstpsmpb" runat="server" Text="0" /></div>
                    </div>
                    <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Total Score</div>
                        <div class="rTableCell"><asp:Label ID="lblBDM_TotalScore_CPIP" runat="server" Text="0"></asp:Label></div>
                        <div class="rTableCell"><asp:Label ID="lblSrMagr_TotalScore_CPIP" runat="server" Text="0"></asp:Label></div>
                        <div class="rTableCell"><asp:Label ID="lblCPMO_TotalScore_CPIP" runat="server" Text="0"></asp:Label></div>
                        <div class="rTableCell"><asp:Label ID="lblAverage_TotalScore_CPIP" runat="server" Text="0"></asp:Label></div>
                    </div>
                     <div class="rTableRow">
                        <div class="rTableCell textBlueFont textFont300">Comments</div>
                        <div class="rTableCell selectboxheight">
                            <%--<asp:Label ID="lblBDM_Comments_CPIP" runat="server" Text=""></asp:Label>--%>
                            <asp:DropDownList ID="lstBDM_Comments_CPIP" runat="server" CssClass="commentWidth">
                                <asp:ListItem Value="business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rTableCell selectboxheight">
                            <asp:Label ID="lblSrMagr_Comments_CPIP" runat="server" Text=""></asp:Label>
                            <%--<asp:DropDownList ID="lstSrMagr_Comments_CPIP" runat="server" CssClass="commentWidth">
                                <asp:ListItem Value="business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:DropDownList>--%>
                        </div>
                        <div class="rTableCell selectboxheight">
                            <asp:Label ID="lblCPMO_Comments_CPIP" runat="server" Text=""></asp:Label>
                            <%--<asp:DropDownList ID="lstCPMO_Comments_CPIP" runat="server" CssClass="commentWidth">
                                <asp:ListItem Value="business viability" Text="Business Viability" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="unique market proposition" Text="Unique Market Proposition"></asp:ListItem>
                                <asp:ListItem Value="creativity" Text="Creativity"></asp:ListItem>
                                <asp:ListItem Value="Management Team Ability" Text="Management Team Ability"></asp:ListItem>
                                <asp:ListItem Value="Technology Capability" Text="Technology Capability"></asp:ListItem>
                            </asp:DropDownList>--%>

                        </div>
                    </div>
               </asp:Panel>

                

            <div class="divother">
            <asp:Panel ID="CCMFPanel" runat="server" Visible="false">
                <asp:GridView ID="CCMFGridViewScore" runat="server" AutoGenerateColumns="false" CssClass="Gridwidth" Width="100%" >
                     <Columns>
                        <asp:BoundField DataField="ScoreTitle" HeaderText="" HtmlEncode="False" ControlStyle-CssClass="cssclass1"/>
                        <asp:BoundField DataField="ScoreBDM" HeaderText="BDM" HtmlEncode="False" ControlStyle-CssClass="cssclass2"  />
                        <asp:BoundField DataField="ScoreSrMagr" HeaderText="Sr. Mgr." HtmlEncode="False" ControlStyle-CssClass="cssclass3" />
                        <asp:BoundField DataField="ScoreCPMO" HeaderText="CPMO" HtmlEncode="False" ControlStyle-CssClass="cssclass3" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel ID="CPIPPanel" runat="server" Visible="false">
                <asp:GridView ID="CPIPGridViewScore" runat="server" AutoGenerateColumns="false" CssClass="Gridwidth" Width="100%" ShowHeader="true" OnRowDataBound="CPIPGridViewScore_RowDataBound" >
                     <Columns>
                        <asp:BoundField DataField="ScoreTitle" HeaderText="Title" HtmlEncode="False" ControlStyle-CssClass="cssclass1"/>
                        <asp:TemplateField HeaderText="BDM"  ItemStyle-Width="65px" ItemStyle-Wrap="true" >
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenddlBDM" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ScoreBDM") %>' />
                                <asp:DropDownList ID="ddlBDM" runat="server" AutoPostBack="true">
                                    <asp:ListItem Value="0.0" Text="0.0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                                    <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                                    <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                                    <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                                    <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                                    <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ScoreSrMagr" HeaderText="Sr Mgr" HtmlEncode="False" ControlStyle-CssClass="cssclass3"  ItemStyle-Width="65px" ItemStyle-Wrap="true"/>
                        <asp:BoundField DataField="ScoreCPMO" HeaderText="CPMO" HtmlEncode="False" ControlStyle-CssClass="cssclass3"  ItemStyle-Width="65px" ItemStyle-Wrap="true"/>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="CPIPGridViewScoreTotal" runat="server" AutoGenerateColumns="false" CssClass="Gridwidth" Width="100%" ShowHeader="false">
                     <Columns>
                        <asp:BoundField DataField="ScoreTitle" HeaderText="Title" HtmlEncode="False" ControlStyle-CssClass="cssclass1"/>
                        <asp:BoundField DataField="ScoreBDM" HeaderText="BDM" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Width="65px" ItemStyle-Wrap="true" />
                        <asp:BoundField DataField="ScoreSrMagr" HeaderText="Sr Mgr" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Width="65px" ItemStyle-Wrap="true"/>
                        <asp:BoundField DataField="ScoreCPMO" HeaderText="CPMO" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Width="65px" ItemStyle-Wrap="true"/>
                    </Columns>
                </asp:GridView>
                <div><asp:Label ID="lblTotal" runat="server" Text="Total Score" /><asp:Label ID="lblBDMTotal" runat="server" Text="90" /><asp:Label ID="lblSrMgrTotal" runat="server" Text="99" /><asp:Label ID="lblCPMOTotal" runat="server" Text="99" /></div>
                <div><asp:Label ID="lblComments" runat="server" Text="Comments" /><asp:Label ID="lblBDMComments" runat="server" Text="Comments" /><asp:Label ID="lblSrMgrComments" runat="server" Text="Comments" /><asp:Label ID="lblCPMOComments" runat="server" Text="Comments" /></div>
            </asp:Panel>

            <div class="textBlueFont">Final BDM remark</div>

            <asp:Label ID="lblremark" runat="server" Text="" />
            <asp:TextBox ID="remark" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="text-danger" Display="Static" ErrorMessage="Remark cannot be empty." ControlToValidate="remark" runat="server" validationgroup="FinalRemarkVWPGroup"/>
 
            <div class="textBlueFont">Score criteria: 0 - not available, 1 - fair, 2 - good, 3 - very good</div>
            </div> 
            <div style="text-align: left;margin-top: 50px;" class="">
                <asp:Label ID="lblmessage" runat="server" Text="" CssClass="RequiredFieldValidator" />
                <asp:Button ID="BtnSubmit" runat="server" Text="Save remark" OnClick="btnsubmit_Click" validationgroup="FinalRemarkVWPGroup"  CssClass="apply-btn skytheme"/>
                <%--<asp:Button ID="btnWithdraw" runat="server" Text="Withdraw" OnClick="btnWithdraw_Click"   CssClass="apply-btn bluetheme" OnClientClick="return confirm('Are you confirm Withdraw?')"/>--%>
                <asp:Button ID="BtnConfirm" runat="server" Text="Withdraw" data-toggle="modal" data-target="#WithdrawMessagebox" CssClass="apply-btn bluetheme"/> 
                <asp:Label ID="lbltest" runat="server" Text="" />                
            </div>
                     
            
            </div>
        </div>
        </div>
</asp:Panel>

<!-- Modal -->
<div class="modal fade" id="WithdrawMessagebox" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Final BDM remark</h4>
      </div>
      <div class="modal-body">
        Are you confirm?
      </div>
      <div class="modal-footer">
        <asp:Button ID="btnSubmitPop" runat="server" Text="Yes" CssClass="apply-btn greentheme button_width160" OnClick="btnWithdraw_Click" />
        <asp:Button ID="btnCancelPop" runat="server" Text="No"  data-dismiss="modal" CssClass="apply-btn graytheme button_width160" />
      </div>
    </div>
  </div>
</div>
