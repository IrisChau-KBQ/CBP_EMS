<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CCMFPresentScoreWebpart.ascx.cs" Inherits="CBP_EMS_SP.CCMFPresentScoreWP.CCMFPresentScoreWebpart.CCMFPresentScoreWebpart" %>







<script type="text/javascript">
    function SelectedChangeMT(ddl) {


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
    min-width: 80px;
    width: 80px !important;
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
</style>
    
    
    <div class="outsideboard">
        <div>
        <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
    <div class="divother" ><h2 class="center">CCMF Present Score</h2></div><br />
    <div class="rTableRow">
        <div class="rTableCell">Date </div>
        <div class="rTableCell"><asp:Label ID="lblDate" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Time </div>
        <div class="rTableCell"><asp:Label ID="lblTime" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Application No. </div>
        <div class="rTableCell"><asp:Label ID="lblApplicationNo" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Company Name </div>
        <div class="rTableCell"><asp:Label ID="lblCompanyName" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Management Team (30%)</div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstMT" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
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
        <div class="rTableCell">Business Model & Time to Market (30%)</div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstBMTM" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
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
        <div class="rTableCell">Creativty and Innovation of the Proposed Project, Product and Service (30%)</div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstCIPP" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
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
        <div class="rTableCell">Social Responsibility (10%)</div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstSR" runat="server" onchange="SelectedChangeMT(this)" CssClass="ddplist">
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
        <div class="rTableCell">Total Score</div>
        <div class="rTableCell"><asp:Label ID="lblTotalScore" runat="server" Text="000"></asp:Label></div>
    </div>

    <div class="rTableRow">
        <div class="rTableCell">Go/ Not Go</div>
        <div class="rTableCell">
            <asp:RadioButtonList ID="radioGoNotGo" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Go" Value="1" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Not Go" Value="0"></asp:ListItem>
            </asp:RadioButtonList>   
        </div>
    </div>

    <div class="divother">
        <h4>Remarks</h4>
        <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>                
    </div> 
    <div style="text-align: right">
        <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn greentheme button_width160" OnClick="btnsubmit_Click" />
    </div> 
    </asp:Panel>       
        </div>
    </div>