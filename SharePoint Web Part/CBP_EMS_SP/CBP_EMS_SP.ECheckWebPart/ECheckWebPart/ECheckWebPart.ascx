<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ECheckWebPart.ascx.cs" Inherits="CBP_EMS_SP.ECheckWebPart.ECheckWebPart.ECheckWebPart" %>


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

.RequiredFieldValidator {
    color:red;
}
</style>
<asp:Panel ID="MainPanel" runat="server" Visible="true">
        <div class="outsideboard">
        <div>
        <asp:Panel ID="panel2" runat="server" CssClass="aspPanel" >
        <div class="divother" ><h2 class="center">Eligibility Checking.</h2></div><br />    
        <div class="divMargin">
            <asp:Button ID="btnDownload" runat="server" Text="DownLoad" OnClick="btnDownload_Click"  ValidationGroup="DownloadZipECWP"/>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red"  OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZipECWP" Display="Dynamic"></asp:CustomValidator>
            <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
        </div>    
        <div class="rTableRow">
        <div class="rTableCell">Coordinator :</div>
        <div class="rTableCell"><asp:Label ID="lblCoordinator" runat="server" Text=""></asp:Label></div>
        </div>
        <div class="divother">Result</div>
        <div>
            <asp:RadioButtonList ID="rbtnresult" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnresult_SelectedIndexChanged">
                <asp:ListItem Value="Coordinator Reviewed" Text="Confirm" Selected="True"></asp:ListItem>
                <asp:ListItem Value="Require additional information" Text="Require additional information"></asp:ListItem>
                <asp:ListItem Value="To be disqualified" Text="To be disqualified"></asp:ListItem>
            </asp:RadioButtonList>    
        </div>
        <div class="divother">
        <h4>Comment for applicants</h4>
        <asp:TextBox ID="txtcommentforapplicants" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="RequiredFieldValidator" ID="RequiredFieldValidator" runat="server" ErrorMessage="Comment for applicants cannot be empty." ControlToValidate="txtcommentforapplicants" Enabled="False" ValidationGroup="ECWebPartGroup" ></asp:RequiredFieldValidator>
        </div>
        <div class="divother">
        <h4>Comment for internal use</h4>
        <asp:TextBox ID="txtcommentforinternaluse" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>
        </div>
        <div style="text-align: right">
            <asp:Label ID="lblmessage" runat="server" Text="" CssClass="RequiredFieldValidator" /><asp:Button ID="BtnSubmit" runat="server" Text="Submit" OnClick="BtnSubmit_Click" ValidationGroup="ECWebPartGroup"/>
        </div>
        </asp:Panel>
        </div>
    </div>
</asp:Panel>