<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisqAppVisualWebPart.ascx.cs" Inherits="CBP_EMS_SP.DisqAppWebPart.DisqAppVisualWebPart.DisqAppVisualWebPart" %>

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
<asp:Panel ID="MainPanel" runat="server" Visible="true">
        <div class="outsideboard">
            <div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
            <div class="divother" ><h2 class="center">Disqualify Application</h2></div><br />
            <div class="divother">
            <h4>Comment</h4>
            <asp:TextBox ID="txtcomment" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="text-danger" Display="Static" ErrorMessage="Comment cannot be empty." ControlToValidate="txtcomment" runat="server" validationgroup="DisqAppVisualWebPartGroup"/>
            </div> 
            <div style="text-align: right">
                <asp:Label ID="lblmessage" runat="server" Text="" CssClass="messagered" /><asp:Button ID="BtnSubmit" runat="server" Text="Disqualify" OnClick="btnsubmit_Click"  OnClientClick="return confirm('Disqualify Application?')" validationgroup="DisqAppVisualWebPartGroup"/>
            </div>
            </asp:Panel>
            </div>
        </div>
</asp:Panel>