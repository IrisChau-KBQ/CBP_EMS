<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadPageWebPart.ascx.cs" Inherits="CBP_EMS_SP.DownloadPageWP.DownloadPageWebPart.DownloadPageWebPart" %>

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
</style>
<script>
    function StartDownload() {
        setTimeout(
            function ()
            {
                _spFormOnSubmitCalled = false;
                _spSuppressFormOnSubmitWrapper = true;
                //location.reload();
            }, 3000);
    }
</script>
<div class="outsideboard">
    <div class="divother" ><h2 class="center">Download List</h2></div>
    <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="griidview">
        <asp:GridView ID="GridViewDownload" runat="server"  CellSpacing="-1" GridLines="None" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
            <Columns>
                <%--<asp:HyperLinkField DataNavigateUrlFormatString="httP://{0}" DataNavigateUrlFields="Path" DataTextField="FileName" HeaderText="File Name" />--%>
                <asp:BoundField DataField="FileName" HeaderText="File Name" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Zip Date" />

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="LinkButtonDownload" runat="server" CommandName='<%# DataBinder.Eval(Container.DataItem, "Path") %>' OnCommand="LinkButtonDownload_Command">Download</asp:LinkButton>--%>
                        <asp:Button ID="BtnDownloadFile" runat="server" Text="Download" CommandName='<%# DataBinder.Eval(Container.DataItem, "Path") %>' OnCommand="LinkButtonDownload_Command" OnClientClick="StartDownload();" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            
        </asp:GridView>

        <asp:UpdatePanel runat="server" ID="up1">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
