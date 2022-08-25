<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VMEditWP.ascx.cs" Inherits="CBP_EMS_SP.VMEditWP.VMEditWP.VMEditWP" %>




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
    /*max-width: 150px;*/
    min-width: 80px;
    /*width: 80px !important;*/
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
.VTMBox{
   display: -webkit-inline-box;
}
.VTMlstLeft,.VTMlstRight{
   width:40%;
}
.VTMbtn{
    width: 20%;
    display: -webkit-inline-box;
    margin-left: 20px;
    padding-top: 14px;
}
.VTMbtn .btnmove{
    min-width: 2em;
    margin-bottom: 6px;
}
.griidview th, .griidview td {
    padding: 10px;
    text-align: left;
}
.blue{
    background-color:#58a1e4 !important;
}
.SelectedCount{
    margin-right: 25px;
}
.margin1{
    margin-bottom:15px;
}
.margin2{
    margin-top:50px;
}
</style>
    
<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<div class="outsideboard card-theme page_main_block" style="width: 100%">
     <div>    
     <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
     
     <div class="divother" ><h2 class="titleFont">Vetting Meeting Arrangement</h2></div>
   
    <div class="rTable">
        <div class="rTableRow">
            <div class="rTableCell">
                <div class="margin1 textBlackFont">Programme Name</div>
                <div class="margin1">
                    <asp:Label ID="lblCyberportProgramme" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>
                </div>
            </div>
       
            <div class="rTableCell">
                <div class="margin1 textBlackFont">Intake Number</div>
                <div class="margin1">
                    <asp:Label ID="lblIntakeNumber" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>
                </div>
            </div>
       
        </div>
    </div>
    <div class="rTable">
        <div class="rTableRow">
            <div class="rTableCell"> 
                <div class="margin1 textBlackFont">Date</div>
                <div class="margin1">
                    <asp:Label ID="lblDate" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>
                </div>
            </div>            
            <div class="rTableCell ">                           
                <div class="margin1 textBlackFont">Venue</div>
                <div class="margin1">
                    <asp:Label ID="lblVenue" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>
                </div>
            </div> 
            <div class="rTableCell">                           
                <div class="margin1 textBlackFont">Time Interval</div>
                <div class="margin1">
                    <asp:Label ID="lblTimeInterval" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>
                </div>
            </div> 
        </div>
      </div>

    <div class="rTable">
        <div class="rTableRow">
            <div class="rTableCell">
                <div class="margin1 textBlackFont">Vetting Meeting From</div>
                <div class="margin1">
                    <asp:Label ID="lblVMFrom" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>               
                </div>
            </div> 

            <div class="rTableCell">
                <div class="margin1 textBlackFont">Vetting Meeting To</div>
                <div class="margin1">
                   <asp:Label ID="lblVMTo" runat="server" Text=""  CssClass="ddplist fontbold" ></asp:Label>               
                </div>
            
            </div>
        </div>   

        <div class="rTableRow">
            <div class="rTableCell">
                <div class="margin1 textBlackFont">Presentation From</div>
                <div class="margin1">
                    <asp:Label ID="lblPresentFm" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>  
                </div>
            </div> 

            <div class="rTableCell">
                <div class="margin1 textBlackFont">Presentation To</div>
                <div class="margin1">
                   <asp:Label ID="lblPresentTo" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>  
                </div>
            
            </div>
        </div>

        <div class="rTableRow">
            <div class="rTableCell">
                <div class="margin1 textBlackFont">Vetting Team Leader</div>
                <div class="margin1">
                    <asp:Label ID="lblVettingTeamLeader" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>  
                </div>
            
            </div>
        </div>
    </div>
        <%--<div class="rTableRow">
            <div class="rTableCell"> </div>
            <div class="rTableCell">
                <SharePoint:PeopleEditor ID="spVTLeader" runat="server" Width="350" MultiSelect="false" SelectionSet="User" AcceptAnyEmailAddresses="true" />
            </div>
            
        </div>--%>
         <div class="rTable">
            <div class="rTableRow">
                <div class="rTableCell">
                    <div class="margin1 textBlackFont">Vetting Team Member</div>
                    <div class="margin1">
                        <asp:Label ID="lblVettingTeamMember" runat="server" Text=""  CssClass="ddplist fontbold"></asp:Label>  
                    </div>
            
                </div>
            </div>

            
        </div>
        

    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="rTable">
                <div class="rTableRow"></div> 
                <div class="rTableRow">
                    <div class="rTableCell">List of Applications</div>
                    <asp:Label ID="lbl_validate_SelectCnt" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>                 
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <asp:DropDownList ID="lstCluster" runat="server" CssClass="ddplist" DataTextField="ClusterText" DataValueField="ClusterValue" AutoPostBack="true" OnSelectedIndexChanged="lstCluster_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </div>
                </div> 
                <div class="rTableRow">
                    <div class="rTableCell">Selected Count</div>                
                </div>
                <div class="rTableRow">
                    <div class="rTableCell">
                        <asp:Label ID="lblSelectedCount" runat="server"></asp:Label>
                    </div>                
                </div>
                <div class="rTableRow">
                    <asp:DropDownList ID="ddlSelectedList" runat="server" Visible="false" ></asp:DropDownList>
                    <asp:GridView ID="gvAppl" runat="server" GridLines="None" AutoGenerateSelectButton="False" EditRowStyle-HorizontalAlign="Left" HorizontalAlign="Left" PagerStyle-HorizontalAlign="Left" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CssClass="griidview" OnRowDataBound="gvAppl_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>                            
                                    <asp:CheckBox ID="CheckBoxselection" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "selection") %>' ItemStyle-Width="3%" AutoPostBack="True" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ApplicationNo" HeaderText="Application No."  HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20%"/>
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="ProgrammeType" HeaderText="Programme Type" />
                            <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" />
                            <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:TemplateField HeaderText="Shortlisted">
                                    <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxShortlisted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Shortlisted") %>'  Enabled="false"/>
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PresentationDetails" HeaderText="Presentation Details" />
                        </Columns>

                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>--%>

    <div style="text-align: left; margin-top: 50px;" class="btnBox">
        <%--<asp:Button ID="btnsubmit" runat="server" Text="Edit" CssClass="apply-btn blue button_width160" OnClick="btnsubmit_Click" ValidationGroup="submitVM"/>
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="apply-btn blue button_width160" OnClick="btnDelete_Click"  OnClientClick="return confirm('Are you sure to delete?');"/>--%>
        <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="apply-btn greentheme" OnClick="btnCancel_Click" ValidationGroup="1"/>
    </div> 
    </asp:Panel>       
        <asp:Label ID="lbltest" runat="server" Text="" />
    </div>
</div>