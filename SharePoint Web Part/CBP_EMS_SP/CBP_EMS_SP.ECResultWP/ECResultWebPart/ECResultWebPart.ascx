<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ECResultWebPart.ascx.cs" Inherits="CBP_EMS_SP.ECResultWP.ECResultWebPart.ECResultWebPart" %>



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
.left{
    text-align:left;
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

.marginBox{
    margin-left:857px;
}
</style>

<div class="outsideboard">
    <div class="divother" ><h2 class="center">EC Result List</h2></div><br />
    <div class="rTable">
         <div class="rTableRow">
        <div class="rTableCell">
            <asp:DropDownList ID="lstCyberportProgramme" runat="server" CssClass="ddplist" DataTextField="ProgrammeName" DataValueField="ProgrammeName" AutoPostBack="true">
               <%--<asp:ListItem Text="CCMF" ></asp:ListItem>--%>
            </asp:DropDownList>
        </div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstIntakeNumber" runat="server" CssClass="ddplist" DataTextField="IntakeNumber" DataValueField="IntakeNumber" AutoPostBack="true">
               <%--<asp:ListItem Text="201612" ></asp:ListItem>--%>
            </asp:DropDownList>
        </div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstCluster" runat="server" CssClass="ddplist" AutoPostBack="true">
               <asp:ListItem Text="All Cluster" Value="%" ></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="rTableCell">
            <asp:DropDownList ID="lstStatus" runat="server" CssClass="ddplist" AutoPostBack="true">
                <asp:ListItem Text="All Status" Value="%" ></asp:ListItem>
                <asp:ListItem Text="Submitted" Value="Submitted" ></asp:ListItem>
                <asp:ListItem Text="Waiting for response from application" Value="Waiting for response from application" ></asp:ListItem>
                <asp:ListItem Text="Eligibility checked" Value="Eligibility checked" ></asp:ListItem>
                <asp:ListItem Text="To be disqualified" Value="To be disqualified" ></asp:ListItem>
                <asp:ListItem Text="Disqualified" Value="Disqualified" ></asp:ListItem>
                <asp:ListItem Text="BDM Reviewed" Value="BDM Reviewed" ></asp:ListItem>
                <asp:ListItem Text="Sr. Mgr. Reviewed" Value="Sr. Mgr. Reviewed" ></asp:ListItem>
                <asp:ListItem Text="CPMO Reviewed" Value="CPMO Reviewed" ></asp:ListItem>
                <asp:ListItem Text="Complete Screening" Value="Complete Screening" ></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    
        <div class="rTableRow">
            <div class="rTableCell">
                <asp:Label ID="lbldatetime" runat="server" Text="Deadline: 1 Dec 2016 5:00pm(GMT +8)"></asp:Label>
            </div>
            <div class="rTableCell"></div>
                <div class="rTableCell"></div>
            <div class="rTableCell">
                <asp:Label ID="lblcount" runat="server" Text="0 applications"></asp:Label>
            </div>
        </div>
    </div>
    <div class="rTable">
        <div class="rTableRow">
            <div class="rTableCell">
                <h3>Sorted by</h3>
            </div>
        </div>
        <div class="rTableRow">
            <div class="rTableCell">
                <asp:DropDownList ID="lstSortCluster" runat="server" CssClass="ddplist" AutoPostBack="true">
                    <asp:ListItem Text="Cluster"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="rTableCell">
                <asp:RadioButtonList ID="radioSortCluster" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                    <asp:ListItem Selected Text="Ascending" Value="asc"></asp:ListItem> 
                    <asp:ListItem  Text="Descending" Value="desc"></asp:ListItem> 
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="rTableRow">
            <div class="rTableCell">
                <asp:DropDownList ID="lstSortApplicationNo" runat="server" CssClass="ddplist" AutoPostBack="true">
                    <asp:ListItem Text="Application No" Value="Application_Number" ></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="rTableCell">
                <asp:RadioButtonList ID="radioSortApplicationNo" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                    <asp:ListItem Selected Text="Ascending" Value="asc"></asp:ListItem> 
                    <asp:ListItem  Text="Descending" Value="desc"></asp:ListItem> 
                </asp:RadioButtonList>
            </div>
        </div>
      </div>
    <div class="left marginBox">
        <div><h3>Comgirmtion Email Sample</h3></div>
        <asp:TextBox ID="txtConfirmtionEmailSample" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea" ></asp:TextBox>    
    </div> 
    <div>
        <%--<asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="apply-btn greentheme button_width160" OnClick="btnConfirm_Click"/>--%>
    </div>
    <div class="griidview">
        <asp:GridView ID="GridViewApplication" runat="server"  CellSpacing="-1" GridLines="None" Visible="true" ShowHeaderWhenEmpty="true"  AutoGenerateColumns="false">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFormatString="#?id={0}" DataNavigateUrlFields="ApplicationNo" DataTextField="ApplicationNo" HeaderText="Application No." />
                <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:TemplateField HeaderText="EC Confirmed">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxECConfirmed" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "ECConfirmed") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>
    </div>
 
</div>