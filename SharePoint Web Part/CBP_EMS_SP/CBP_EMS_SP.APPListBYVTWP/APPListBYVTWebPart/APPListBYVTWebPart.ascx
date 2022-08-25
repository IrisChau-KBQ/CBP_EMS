<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APPListBYVTWebPart.ascx.cs" Inherits="CBP_EMS_SP.APPListBYVTWP.APPListBYVTWebPart.APPListBYVTWebPart" %>





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
</style>

<div class="outsideboard">
    <div class="divother" ><h2 class="center">Application List</h2></div><br />
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
                <asp:Label ID="lbldatetime" runat="server" Text="Deadline: "></asp:Label>
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
    <div class="griidview">
        <asp:GridView ID="GridViewApplicationVT" runat="server"  CellSpacing="-1" GridLines="None" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
          <Columns>
                <asp:HyperLinkField DataNavigateUrlFormatString="{0}" DataNavigateUrlFields="APPNoURL" DataTextField="ApplicationNo" HeaderText="Application No." />
                <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                <asp:BoundField DataField="ProgrammeType" HeaderText="Programme Type" />
                <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" />
                <%--<asp:BoundField DataField="ProjectDescription" HeaderText="Project Description" />--%>
               <asp:TemplateField HeaderText="ProjectDescription">
                    <ItemTemplate>
                        <asp:Image  runat="server" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.APPListBYVTWP/Information.png" Title='<%# DataBinder.Eval(Container.DataItem, "ProjectDescription") %>' width="15" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="BDMScore" HeaderText="BDM Score" />
                <asp:BoundField DataField="SrManagerScore" HeaderText="Sr. Manager Score" />
                <asp:BoundField DataField="CPMOScore" HeaderText="CPMO Score" />
                <asp:BoundField DataField="AverageScore" HeaderText="Averager Score" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks"  HtmlEncode="false"/>
                <asp:BoundField DataField="RemarksForVetting" HeaderText="Remarks for Vetting" />
                <asp:TemplateField HeaderText="Shortlisted">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxShortlisted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Shortlisted") %>'  Enabled="false"/>
                    </ItemTemplate>
                </asp:TemplateField>

              <asp:BoundField DataField="PresentationFrom" HeaderText="Presentation From" />
              <asp:BoundField DataField="PresentationTo" HeaderText="Presentation To" />
              <asp:BoundField DataField="Email" HeaderText="Email" />
              <asp:BoundField DataField="MobileNumber" HeaderText="MobileNumber" />
              <asp:TemplateField HeaderText="Attend">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxShortlisted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Attend") %>'  Enabled="false"/>
                    </ItemTemplate>
                </asp:TemplateField>
              <asp:BoundField DataField="NameOfAttendees" HeaderText="Name Of Attendees" />
              <asp:BoundField DataField="PresentationTools" HeaderText="Presentation Tools" />
              <asp:BoundField DataField="SpecialRequest" HeaderText="Special Request" />
              <asp:BoundField DataField="GoNotGo" HeaderText="Go / Not Go" />
            </Columns>

        </asp:GridView>
    </div>
 
</div>