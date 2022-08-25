<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FinalResultWP.ascx.cs" Inherits="CBP_EMS_SP.FinalResultWP.FinalResultWP.FinalResultWP" %>


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

.gvTable th, .gvTable td {
    padding: 10px;
    text-align: left;
}

.GridView1 th, .GridView1 td {
    padding: 10px;
    text-align: left;
}

</style>

    <div class="outsideboard">
        <div>
            <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
                <div class="divother" ><h2 class="center">Final Vetting Result</h2></div><br />
                     <asp:GridView ID="GridView1" CssClass="gvTable" runat="server" AutoGenerateColumns="false" GridLines="None" AllowSorting="false" ShowHeaderWhenEmpty="true">
                         <Columns>                         
                            <asp:BoundField DataField="TotalNoOfApplications" HeaderText="TotalNoOfApplications" />
                            <asp:BoundField DataField="ShortlistedForPresentation" HeaderText="ShortlistedForPresentation" />
                            <asp:BoundField DataField="TRecommend" HeaderText="Recommend" />
                            <asp:BoundField DataField="TNotRecommend" HeaderText="Not Recommend" />
                        </Columns>
                     </asp:GridView>   

                     <asp:GridView ID="gvTable" CssClass="gvTable" runat="server" AutoGenerateColumns="false" GridLines="None" AllowSorting="false" ShowHeaderWhenEmpty="true">
                        <Columns>                         
                            <asp:BoundField DataField="ApplicationNo" HeaderText="Application No." SortExpression="ApplicationNo"/>
                            <asp:BoundField DataField="CompProjName" HeaderText="Company/Project Name" SortExpression="CompProjName"/>
                            <asp:BoundField DataField="Recommend" HeaderText="Recommend" SortExpression="Recommend"/>
                            <asp:BoundField DataField="NotRecommend" HeaderText="Not Recommend" SortExpression="NotRecommend"/>
                            <asp:BoundField DataField="NoOfVote" HeaderText="No. of Votes" SortExpression="NoOfVote"/>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                        </Columns>
                    </asp:GridView>             
            </asp:Panel>       
        </div>
    </div>