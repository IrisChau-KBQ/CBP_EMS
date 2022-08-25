<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreListApp.ascx.cs" Inherits="CBP_EMS_SP.PreListAppWP.PreListApp.PreListApp" %>

<%--<link href="/_layouts/15/Images/CBP_EMS_SP.ApplicationListWP/bootstrap.min.css" rel="stylesheet" />
<script src="/_layouts/15/Images/CBP_EMS_SP.ApplicationListWP/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Images/CBP_EMS_SP.ApplicationListWP/bootstrap.min.js" type="text/javascript"></script>--%>

<script type="text/javascript">
    $(function () {
        var sheight = $(window).height();
        $("#<%=pnlContent.ClientID %>").height((sheight - 400) + "px");
    });
</script>

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


    

    .messagered {
        color:red;
    }

    .gridcount {
        color:red;
        font-weight: bold;
    }

    .width300 {
        width: 300px;
    }
    


</style>
<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<asp:Panel ID="mainPanel" runat="server" CssClass="aspPanel" >
<div class="card-theme page_main_block" style="width: 100%">
<div>
    <div>
        <div class="divother" ><h2 >Presentation List of Application</h2></div><br />
    </div>
    <hr />
    <div class="rTable table1">
        <div class="rTableRow">
            <div class="rTableCell lbl">Date</div>
            <div class="rTableCell"><asp:Label ID="lblDate" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl">Time Interval</div>
            <div class="rTableCell"><asp:Label ID="lblTime_Interval" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl">Venue</div>
            <div class="rTableCell"><asp:Label ID="lblVenue" runat="server" Text="N/A" /></div>
        </div>
        <div class="rTableRow">
            <div class="rTableCell lbl">Programme Name</div>
            <div class="rTableCell"><asp:Label ID="lblProName" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl">Intake No.</div>
            <div class="rTableCell"><asp:Label ID="lblIntakeNo" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl">Meeting Status</div>
            <div class="rTableCell"><asp:Label ID="lblMeetingStatus" runat="server" Text="N/A" /></div>
        </div>
    </div>


    <hr />
    <asp:Panel ID="PresentationPanel" runat="server" Visible="false" >
    <div class="rTableRow">
        <div class="rTableCell">Presentation From</div>
        <div class="rTableCell"><asp:Label ID="lblPresentation_From" runat="server" Text="N/A" /></div>
        <div class="rTableCell">Presentation To</div>
        <div class="rTableCell"><asp:Label ID="lblPresentation_To" runat="server" Text="N/A" /></div>
        <div class="rTableCell">Programme ID</div>
        <div class="rTableCell"><asp:Label ID="lblm_Programme_ID" runat="server" Text="N/A" /></div>
    </div>
    </asp:Panel>
</div>

<div>
    <div class="rTable">
        <div>            
            <div class="rTableCell"><asp:Button ID="btnDecisionSummary" runat="server" Text="Vetting Meeting Summary" CssClass="apply-btn bluetheme width300" OnClick="btnDecisionSummary_Click"/><asp:Button ID="BtnPRS" runat="server" Text="Presentation Result Summary" CssClass="apply-btn bluetheme width300" OnClick="BtnPRS_Click"/><asp:Button ID="btnReset" runat="server" Text="Clear" OnClick="btnReset_Click" CssClass="apply-btn bluetheme button_width160"/></div>
            <div class="rTableCell"><asp:Button ID="btnInsert" runat="server" Text="Insert" OnClick="btnInsert_Click" CssClass="apply-btn bluetheme button_width160"/></div>
            <div class="rTableCell selectboxheight">
            <asp:DropDownList ID="ddlCluster" runat="server" AutoPostBack="false" Height="40px" Width="150px" >
                <asp:ListItem Value="0.0" Text="0.0"></asp:ListItem>
            </asp:DropDownList>
            </div>
            <div class="rTableCell">
                <asp:Button ID="BtnConfirm" runat="server" Text="Confirm" data-toggle="modal" data-target="#SaveMessagebox" CssClass="apply-btn bluetheme button_width160"/>
            </div>
            <div class="rTableCell"><asp:Label ID="lblMessageConfirm" runat="server" Text="" CssClass="messagered" /></div>
            <div class="rTableCell"></div>
        </div>

    
    <div class="rTableRow">
        <div class="rTableCell">
            <asp:GridView ID="GridView_CPIP" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false" CssClass="GridViewstyle" ShowHeader="true" GridLines="None" ShowHeaderWhenEmpty="true"
                OnRowCommand="GridView_CPIP_RowCommand" OnRowDataBound="GridView_CPIP_RowDataBound" OnRowEditing="GridView_CPIP_RowEditing"
                width="100%" BorderStyle="None" >
                <Columns>
                    <asp:BoundField DataField="TimeSlot" HeaderText="Time Slot" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Application No.">
                        <ItemTemplate>
                        <asp:Label ID="lblApplication_Number" runat="server" CssClass="greenlbl" Text='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>'></asp:Label>
                        <asp:DropDownList ID="ddlApplicationNo" runat="server" AutoPostBack="false">
                            <asp:ListItem Value="0.0" Text="0.0"></asp:ListItem>
                            <asp:ListItem Value="0.5" Text="0.5"></asp:ListItem>
                            <asp:ListItem Value="1.0" Text="1.0"></asp:ListItem>
                            <asp:ListItem Value="1.5" Text="1.5"></asp:ListItem>
                            <asp:ListItem Value="2.0" Text="2.0"></asp:ListItem>
                            <asp:ListItem Value="2.5" Text="2.5"></asp:ListItem>
                            <asp:ListItem Value="3.0" Text="3.0"></asp:ListItem>
                        </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Company" HeaderText="Company" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Project_Name_Eng" HeaderText="Project Name" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Programme_Type" HeaderText="Programme Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Hong_Kong_Programme_Stream" HeaderText="Stream" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="CCMF_Application_Type" HeaderText="Application Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="avgscore" HeaderText="Average Score" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Remarks_To_Vetting" HeaderText="Remarks To Vetting" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Attendance" HeaderText="Attendance" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="HiddenVetting_Application_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Application_ID") %>' />
                            <asp:HiddenField ID="HiddenVetting_Meeting_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Meeting_ID") %>' />
                            <asp:HiddenField ID="HiddenPresentation_From" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Presentation_From") %>' />
                            <asp:HiddenField ID="HiddenPresentation_To" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Presentation_To") %>' />
                            <asp:HiddenField ID="HiddenAttendance" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Attendance") %>' />
                            <asp:ImageButton ID="SaveButton" runat="server" 
                            CommandName="SaveStatus" 
                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            Text="Edit" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Save.png" Width="31px" ToolTip="Save input" />
                            <asp:ImageButton ID="CancelButton" runat="server" 
                            CommandName="CancelStatus" 
                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            Text="Edit" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Cancel.png" Width="31px" ToolTip="Cancel and discard input" />
                            <asp:ImageButton ID="EditButton" runat="server" 
                            CommandName="EditStatus" 
                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            Text="Edit" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Edit.png" Width="31px" ToolTip="Change time slot"/>
                        <asp:ImageButton ID="DeleteButton" runat="server" 
                            CommandName="DeleteStatus" 
                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            Text="Edit" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Del.png" Width="31px" ToolTip="Remove Application from time slot" />
                        </ItemTemplate> 
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
         </div>
        <div class="rTableCell">
            <asp:Label ID="lblTitleforGridView" runat="server" Text="Unassigned Application" CssClass="lbl" /><asp:Label ID="lblcount" runat="server" Text="" CssClass="gridcount" />
            <asp:Panel ID="pnlContent" runat="server" ScrollBars="Vertical" Height="300px">
            <asp:GridView ID="GridViewUA" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false" CssClass="GridViewstyle" ShowHeader="false" GridLines="None"
                ShowHeaderWhenEmpty="true" width="100%" BorderStyle="None" OnRowDataBound="GridViewUA_RowDataBound" OnRowCommand="GridViewUA_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                        <div style="text-align: right" >
                        <asp:Image ID="ImgSh" runat="server"  Visible="false" ImageUrl="~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Shortlist.png" Width="21px"/>
                        </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                        <div style="text-align: right" >
                        <asp:Label ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RowNumber") %>'></asp:Label>
                        </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unassigned Application">
                        <ItemTemplate>
                        <asp:HiddenField ID="HiddenApplication_Number" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>' />
                        <asp:HiddenField ID="HiddenApplicationId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ApplicationId") %>' />
                        <asp:HiddenField ID="HiddenProgramId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ProgramId") %>' />
                        <asp:HiddenField ID="HiddenCluster" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Cluster") %>' />
                        <asp:HiddenField ID="HiddenCompany" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Company") %>' />
                        <asp:HiddenField ID="HiddenShortlisted" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Shortlisted") %>' />
                        <div><asp:Label ID="lblApplication_Number" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>'></asp:Label></div>
                        <div><asp:Label ID="lblCustDisplay" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CustDisplay") %>'></asp:Label></div>
                        
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField >
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButtonWithdraw" runat="server" 
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/_layouts/15/Images/CBP_Images/withdraw.png" Width="25px" ToolTip="Withdraw Application"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
        </div>
    </div>
    </div>
</div>
<asp:Label ID="lbltest" runat="server" Text="" />
 <br />
<asp:Label ID="lbltest2" runat="server" Text="" />
</div>
</asp:Panel>

<!-- Modal -->
<div class="modal fade" id="SaveMessagebox" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Presentation List of Application</h4>
      </div>
      <div class="modal-body">
        Are you confirm?
      </div>
      <div class="modal-footer">
        <asp:Button ID="btnSubmitPop" runat="server" Text="Yes" CssClass="apply-btn greentheme button_width160" OnClick="btnSubmitPop_Click" />
        <asp:Button ID="btnCancelPop" runat="server" Text="No"  data-dismiss="modal" CssClass="apply-btn graytheme button_width160" />
      </div>
    </div>
  </div>
</div>

<!-- Modal -->
<div class="modal fade" id="WithdrawMessagebox" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Presentation List of Application</h4>
      </div>
      <div class="modal-body">
        <asp:Label ID="lblWithdrawMessageboxbody" runat="server" Text="Are you confirm?"></asp:Label>
      </div>
      <div class="modal-footer">
        <asp:HiddenField ID="HiddenWithdrawApplication_Number" runat="server"  />
        <asp:HiddenField ID="HiddenWithdrawApplicationId" runat="server"  />
        <asp:HiddenField ID="HiddenWithdrawProgramId" runat="server"  />
        <asp:Button ID="Button1" runat="server" Text="Yes" CssClass="apply-btn greentheme button_width160" OnClick="btnWithdraw_Click" />
        <asp:Button ID="Button2" runat="server" Text="No"  data-dismiss="modal" CssClass="apply-btn graytheme button_width160" />
      </div>
    </div>
  </div>
</div>

<script>
    $(document).ready(function () {
        <% if(isShowWithdrawPopUp){ %>
            $("#WithdrawMessagebox").modal("show");
        <% } else { %>
            $("#WithdrawMessagebox").modal("hide");
        <% }%>
    });
</script>