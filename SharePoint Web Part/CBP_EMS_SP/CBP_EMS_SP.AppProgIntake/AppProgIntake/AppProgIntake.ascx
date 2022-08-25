<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppProgIntake.ascx.cs" Inherits="CBP_EMS_SP.AppProgIntake.AppProgIntake.AppProgIntake" %>

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

.witdth20px {
    width:20px;
    }

.width20per {
    width:20%
}

.width30per {
    width:30%
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
}
    .messagered {
        color:red;
    }

    .Gridbox {
        border: 1px solid #BCBEC0;
        background: #FFFFFF;
        width: 100%;
        padding: 20px 22px;
        box-sizing: border-box;
    }

    .blue {
    background-color: #58a1e4 !important;
    cursor: pointer;
    }

    .chk_center {
     text-align: center;
    }

.margindiv{
    margin-bottom:25px;
}









     

        


</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<script>
function PopUpBox(ProDesc) {
        $("#PopUpProjectDescription .modal-body").html(ProDesc);
        $("#PopUpProjectDescription").modal("show");
    }
</script>
<asp:Panel ID="mainPanel" runat="server" CssClass="aspPanel" >
<div class="outsideboard page_main_block" style="width: 100%">
    <div>
        <div>
            <div class="divother" ><h2 class="titleFont">Application for Programme Intake</h2></div><br />
        </div>

        <hr />
        <div class="rTable">
            <div class="rTableRow">
                <div class="rTableCell lbl width20per textBlueFont">Programme Name</div>
                <div class="rTableCell width20per"><asp:Label ID="lblProName" runat="server" Text="N/A"  CssClass="textBlackFont"/></div>
                <div class="rTableCell lbl width18per textBlueFont">Intake No.</div>
                <div class="rTableCell width20per"><asp:Label ID="lblIntakeNo" runat="server" Text="N/A" CssClass="textBlackFont"/></div>
                <div class="rTableCell lbl width10per textBlueFont">Date.</div>
                <div class="rTableCell width20per"><asp:Label ID="lblMeetingDate" runat="server" Text="N/A" CssClass="textBlackFont"/></div>
            </div>
        </div>
        <hr />
        <div class="margindiv">
            <asp:Button ID="btnDownload" runat="server" Text="Download All Attachments" OnClick="btnDownload_Click" CssClass="apply-btn bluetheme button_width260" ValidationGroup="DownloadZip" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red"  OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZip" Display="Dynamic"></asp:CustomValidator>
            <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
        </div>
        
        <div class="rTable">
            <div class="rTableRow textGreenFont">
                <div class="rTableCell textGreenFont">Filter by</div>
            </div>
            <div class="rTableRow ">
                <div class="rTableCell selectboxheight SharePointTime">
                    <asp:DropDownList ID="ddlDateFilter" runat="server" AutoPostBack="true" Height="40px" Width="150px" OnSelectedIndexChanged="ddlDateFilter_SelectedIndexChanged" Visible="false" >
                    <asp:ListItem Value="0.0" Text="0.0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlStream" runat="server" AutoPostBack="true" Height="40px" Width="150px" OnSelectedIndexChanged="ddlDateFilter_SelectedIndexChanged" >
                    <asp:ListItem Value="0.0" Text="0.0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlShortlisted" runat="server" AutoPostBack="true" Height="40px" Width="150px" OnSelectedIndexChanged="ddlDateFilter_SelectedIndexChanged" >
                    <asp:ListItem Value="0.0" Text="0.0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

        </div>

        <div>
        <asp:GridView ID="GridView_App" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false" CssClass="GridViewstyle" ShowHeader="true" GridLines="None" ShowHeaderWhenEmpty="true"
                OnRowCommand="GridView_App_RowCommand" OnRowEditing="GridView_App_RowEditing"
                width="100%" BorderStyle="None" HeaderStyle-CssClass="textBlueFont" 
                RowStyle-CssClass="textBlackFont" >
                <Columns>
                    <asp:BoundField DataField="Vetting_Meeting_Date" HeaderText="Date" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True" />
                    <asp:BoundField DataField="TimeSlot" HeaderText="Time Slot" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Application No.">
                        <ItemTemplate>
                       <asp:Label ID="lblApplication_Number" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>' Visible="false" ></asp:Label>
                       <asp:LinkButton ID="btnApplication_Number" runat="server" CssClass="textGreenFont" Text='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>' CommandName="Application_Number" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Company" HeaderText="Company" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Project_Name_Eng" HeaderText="Project Name" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Programme_Type" HeaderText="Programme Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Hong_Kong_Programme_Stream" HeaderText="Stream" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="CCMF_Application_Type" HeaderText="Application Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:TemplateField HeaderText="Project Description">
                    <ItemTemplate>
                        <asp:ImageButton  runat="server" ImageUrl="~/_layouts/15/Images/CBP_Images/SearchIcon.png" OnClientClick='<%# String.Format("PopUpBox(\"{0}\");return false;",DataBinder.Eval(Container.DataItem, "Projectdescription")) %>' width="25" />
                     </ItemTemplate>
                </asp:TemplateField>
                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>  
                    <asp:TemplateField HeaderText="Score" Visible="false">
                        <ItemTemplate>
                        <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Total_Score") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AverageScore" HeaderText="Average Score" />
                    <asp:BoundField DataField="Remarksforvettingteam" HeaderText="Remarks for Vetting" />
                    <asp:TemplateField HeaderText="Shortlisted">
                        <ItemTemplate>
                            <div class="listcss">
                                <asp:CheckBox ID="CheckBoxShortlisted" CssClass="greencheckbox" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "ShortlistedChecked") %>' Text="&nbsp;" Enabled="false"/>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="HiddenVetting_Application_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Application_ID") %>' />
                            <asp:HiddenField ID="HiddenVetting_Meeting_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Meeting_ID") %>' />
                            <asp:HiddenField ID="HiddenPresentation_From" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Presentation_From") %>' />
                            <asp:HiddenField ID="HiddenPresentation_To" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Presentation_To") %>' />
                            <asp:HiddenField ID="HiddenAttendance" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Attendance") %>' />
                            <asp:HiddenField ID="HiddenApplication_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Application_ID") %>' />
                        </ItemTemplate> 
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="text-align: left; margin-top: 50px;" class="btnBox">
                <asp:Button ID="btnback" runat="server" Text="Back" CssClass="apply-btn greentheme button_width160" OnClick="btnback_Click"/>

            </div>
        
            </div>
    </div>
</div>
    <!-- Modal -->
<div class="modal fade" id="PopUpProjectDescription" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Project Description</h4>
      </div>
      <div class="modal-body">
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

    <asp:Label ID="lbltest" runat="server" Text="" />
</asp:Panel>