<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInvitationWebPart.ascx.cs" Inherits="CBP_EMS_SP.MyInvitationWP.MyInvitationWebPart.MyInvitationWebPart" %>

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
  	/*width: 100%;*/
}
.rTableRow {
  	display: table-row;
}

.rTableCell{
  	display: table-cell;
    padding: 5px 10px;
}

.ddplist {
    /*min-width: 279px;*/
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
.griidview th, .griidview td {
    padding: 10px;
    text-align: left;
}

.temptext {
    color:white;
}

.btntext {
    color:black;
}
.divMargin {
    margin-bottom:6px;
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
.savebtn{
    text-align:right;
    padding-right: 14px;
}

.tableMargin{
    margin-bottom:21px;
}
.sortitem{
    margin-top:-29px;
}
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />


<div class="outsideboard  page_main_block" style="width: 100%">
    <div class="divother">
        <h2 class="titleFont">
            <asp:Label ID="lblTitle" runat="server" Text="My Invitation"></asp:Label>

        </h2>
    </div>

    <div class="griidview">
        <asp:GridView ID="GridViewMyInvitation" CssClass="griidviewTable" runat="server"  CellSpacing="-1" GridLines="None" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" 
                HeaderStyle-CssClass="textBlueFont"  RowStyle-CssClass="textBlackFont"
                OnRowCommand="GridViewMyInvitation_RowCommand"  > 
            <Columns>
                <asp:BoundField DataField="ItemText" HeaderText="" />
                <asp:TemplateField >
                    <ItemTemplate>
                        <asp:HiddenField ID="HiddenFieldVMID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VMID") %>' />
                        <asp:HiddenField ID="HiddenFieldVAID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "VAID") %>' />
                        <asp:ImageButton ID="ImageButtonEdit" runat="server" ImageUrl="/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Edit%20Button.png" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ToolTip="Respone to Invitation" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

        <asp:Label ID="lbltest" runat="server" Text="" />
    </div>
 
</div>