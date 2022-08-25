<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresentResultSummaryWebPart.ascx.cs" Inherits="CBP_EMS_SP.PresentResultSummary.PresentResultSummaryWebPart.PresentResultSummaryWebPart" %>





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
    min-width: 279px;
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
    min-width:80%;
    max-height:250px;
    min-height:150px;
    height:150px !important;
}
.griidview th, .griidview td {
    padding: 10px;
    text-align: left;
    
}
.griidview{
    width:100%;
    border: 1px solid #e0dede;
}
.temptext {
    color:white;
}

.btntext {
    color:black;
}
.divMargin {
    margin-top: 24px;
    margin-bottom: 22px;
}
.radio{
    position: relative;
    top: 10px;
}
.radio td{
     padding-right: 25px;
}
.radio td input{
    margin-top: 10px;
}

.blue{
    background-color:#58a1e4 !important;
    cursor:pointer;
}
.table1 .rTableCell{
    padding: 6px 95px 6px 10px;
}
.table2 .rTableCell{
    padding-right: 151px;
}
.blue1{
    color:#0072C6;
}
.margindiv{
    margin-bottom:5px;
}
.Attenddiv{
     margin-bottom: -2px;
     padding-left: 7px;
}
.FileUploadInfo{
    padding-right: 209px;
    position: relative;
    top: 21px;
}
.left{
    text-align:left;
}
.textColor{
	color:#145DAA;
    font-weight: 600;
}
.textColorBlack{
	color: #66666b;
    font-weight: 500;
}
.textGreenColor{
	color:#80C343;
    font-weight: 600;
}
input.btnDeepBlue,input.btnDeepBlue:hover{
    margin-left: 0px;
    margin-right: 15px;
    background-color: #0072c6;
    color: white;
    padding: 8px 15px;
    cursor: pointer;
}
input.greentheme{
    cursor:pointer;
}
.hmember{
    position: relative;
    top: 25px;
}
.hmemberGoNotGo{
    position: relative;
    top: 20px;
}
</style>
<div class="outsideboard card-theme page_main_block" style="width: 100%">
    <div class="divother" ><h2 class="left textColor">Presentation Result Summary</h2></div><br />

    <div class="divother">
        <asp:GridView ID="GridViewPresentation" runat="server" GridLines="None"  HeaderStyle-CssClass="textColor" 
            RowStyle-CssClass="textColorBlack"   AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
            CssClass="griidview" 
            >
        
            <Columns>
                <asp:BoundField  DataField="Sequence" HeaderText="Sequence" />
                <asp:BoundField  DataField="ApplicationNo" HeaderText="Application No." ItemStyle-CssClass="textGreenColor"/>
                <asp:BoundField DataField="CompanyNameProjectName" HeaderText="CompanyName / Project Name" />
                <asp:BoundField DataField="Scoreofeachvettingmember" HeaderText="<div>Score of each<br>vetting member</div><div class='hmember'>01 02 03 04</div>" HtmlEncode="false" HeaderStyle-Wrap="false"/>
                <asp:BoundField DataField="TotalScore" HeaderText="Total Score" />
                <asp:BoundField DataField="AverageScore" HeaderText="Average Score" />
                <asp:BoundField DataField="GoNotGo" HeaderText="<div>Go / Not Go<br>choice of each<br>vetting member</div><div class='hmemberGoNotGo'>01 02 03 04</div>" HtmlEncode="false" HeaderStyle-Wrap="false"/>
                <asp:BoundField DataField="Recommend" HeaderText="Recommend" />
                <asp:BoundField DataField="NotRecommend" HeaderText="Not Recommend" />
                <asp:BoundField DataField="NoofVote" HeaderText="No. of Vote" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
            </Columns>

        </asp:GridView>
    </div>
    
    <div style="text-align: left;margin-top: 50px;">
        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="apply-btn blue button_width160"  OnClick="btnConfirm_Click" ValidationGroup ="PLOA" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="apply-btn greentheme button_width160"  ValidationGroup="1" OnClick="btnCancel_Click"/>
    </div> 
</div>