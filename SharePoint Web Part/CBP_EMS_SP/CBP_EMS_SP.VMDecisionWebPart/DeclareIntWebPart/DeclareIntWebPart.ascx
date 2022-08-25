<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeclareIntWebPart.ascx.cs" Inherits="CBP_EMS_SP.DeclareIntWebPart.DeclareIntWebPart.DeclareIntWebPart" %>

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
    <div class="divother" ><h2 class="center">Declaration of Interests</h2></div><br />
    <div class="rTableRow">
        <div class="rTableCell">Date </div>
        <div class="rTableCell"><asp:Label ID="lblDate" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Time </div>
        <div class="rTableCell"><asp:Label ID="lblTime" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Venue </div>
        <div class="rTableCell"><asp:Label ID="lblVenue" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Name </div>
        <div class="rTableCell"><asp:Label ID="lblName" runat="server" Text=""></asp:Label></div>
    </div>

    <div class="rTableRow">
        <div class="rTableCell">No conflict of interest</div>
        <div class="rTableCell"><asp:CheckBox ID="ckbNoConflict" runat="server"></asp:CheckBox></div>
    </div>
    
    <div class="rTableRow">
        <div class="rTableCell">Conflict of interest</div>
        <div class="rTableCell"><asp:CheckBox ID="ckbHaveConflict" runat="server"></asp:CheckBox></div>
    </div>
    
    <div class="rTableRow">
        <div class="rTableCell">List of Application</div>                
    </div> 
          
    <div style="text-align: right">
        <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn greentheme button_width160" OnClick="btnsubmit_Click" />
    </div> 
    </asp:Panel>       
        </div>
    </div>