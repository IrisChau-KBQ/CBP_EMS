<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VMArrangeWebPart.ascx.cs" Inherits="VMArrangeWebPart.VMArrangeWebPart.VMArrangeWebPart" %>

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

.GridViewstyle {
        border:none
    }


    .GridViewstyle th {
        color: #075CA9;
        font-weight: bold;
    }

    .GridViewstyle th, .GridViewstyle td {
    padding: 10px;
    text-align: left;
    vertical-align: text-top;
    }

</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<div class="outsideboard page_main_block" style="width: 100%">
    <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
                <h1 class="titleFont">Vetting Meeting List</h1><br />
                     <asp:GridView ID="gvTable" CssClass="GridViewstyle" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeaderWhenEmpty="true" 
                         OnRowCommand="gvTable_RowCommand" OnDataBound="gvTable_DataBound" OnRowDataBound="gvTable_RowDataBound" Width="100%" HeaderStyle-CssClass="textBlueFont" 
                         RowStyle-CssClass="textBlackFont">
                        <Columns>                         
                            <asp:TemplateField HeaderText="Programme Name">
                            <ItemTemplate>
                                <asp:LinkButton ID=btnview runat=server Text='<%# DataBinder.Eval(Container.DataItem, "Programme_Name") %>' CommandName="Programme Name" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CssClass="textGreenFont" ToolTip="View Invitation Response Summary">
                            </asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Intake_No" HeaderText="Intake No" />
                            <asp:BoundField DataField="Date" HeaderText="Date" />
                            <asp:BoundField DataField="Venue" HeaderText="Venue" />
                            <asp:BoundField DataField="Vetting_Meeting_Time" HeaderText="Vetting Meeting Duration" />
                            <asp:BoundField DataField="Presentation_Time" HeaderText="Presentation Duration" />
                            <asp:BoundField DataField="Time_Interval" HeaderText="Time Interval" />
                            <asp:TemplateField>
                              <ItemTemplate>                      
                                <asp:HiddenField ID="HiddenVetting_Meeting_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Meeting_ID") %>' />
                                <asp:HiddenField ID="HiddenMeeting_status" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Meeting_status") %>' />
                                  <asp:HiddenField ID="HiddenProgramme_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Programme_ID") %>' />
                                <asp:ImageButton ID="ViewButton" runat="server" 
                                  CommandName="View" 
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                  Text="View" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.VMArrangeWebPart/view.png" Width="31px" ToolTip="View Meeting details"/>
                              </ItemTemplate> 
                            </asp:TemplateField>
                            <asp:TemplateField>
                              <ItemTemplate>
                                 <asp:ImageButton ID="AddButton" runat="server" 
                                  CommandName="Add" 
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                  Text="Edit" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.VMArrangeWebPart/addPresentation.png" Width="31px" ToolTip="Update unconfirmated presetation list"/>
                                <asp:ImageButton ID="EditButton" runat="server" 
                                  CommandName="Edit" 
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                  Text="Edit" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.VMArrangeWebPart/viewPresentation.png" Width="31px" ToolTip="View/Update confirmed presentation list"/>
                              </ItemTemplate> 
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>             
                    <hr />
                    <asp:ImageButton ID="ImgbtnAdd" runat="server" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.VMArrangeWebPart/add.png" OnClick="btnAdd_Click" width="31px" ToolTip="Create new meeting"/>
            </asp:Panel>       
    <asp:Label ID="lbltest" runat="server" Text="" />
</div>