<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RejectBDMWP.ascx.cs" Inherits="CBP_EMS_SP.RejectBDMWP.RejectBDMWP.RejectBDMWP" %>

<link href="/_layouts/15/Images/CBP_EMS_SP.ApplicationListWP/bootstrap.min.css" rel="stylesheet" />
<script src="/_layouts/15/Images/CBP_EMS_SP.ApplicationListWP/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Images/CBP_EMS_SP.ApplicationListWP/bootstrap.min.js" type="text/javascript"></script>

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

<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<asp:Panel ID="MainPanel" runat="server" Visible="true">
    <div class="card-theme page_main_block" style="width: 100%">
        <div class="outsideboard greenheaderborder">
            <div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
            <div class="divother" ><h2 class="titleFont textGreenFont">Reject / Disqualify Application</h2></div><br />
            <div class="rTableRow">
                <div class="rTableCell textBlackFont">Role </div>
                <div class="rTableCell"><asp:Label ID="lblrole" runat="server" Text="" CssClass="textBlackFont"></asp:Label></div>
            </div>
                <div class="rTableRow">
                <div class="rTableCell textBlackFont">Status </div>
                <div class="rTableCell"><asp:Label ID="lblApplicationStatus" runat="server" Text="" CssClass="textBlackFont"></asp:Label></div>
            </div>
            <hr />
            <div class="hide">
                <div class="textBlueFont">Comments from Coordinator</div>
                <div>
                    <asp:Label id="lblCommentsCoordinator" runat="server" Text="" CssClass="textBlackFont"/>
                </div>
            </div>
            <div>
                <asp:RadioButtonList ID="rbt_Reject_Disqualify" runat="server" CssClass="listcss" >
                    <asp:ListItem Text="Reject Application to Coordinator" Value="BDM Rejected" Selected="True" />
                    <asp:ListItem Text="Disqualify Application" Value="Disqualified" />
                </asp:RadioButtonList>
            </div>
            <br />
            <div class="divother">
            <div class="textBlueFont">Comment for internal use</div>
            <asp:TextBox ID="txtcommentforinternaluse" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="whitebackground"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="text-danger" Display="Static" ErrorMessage="Comment for internal use cannot be empty." ControlToValidate="txtcommentforinternaluse" runat="server" validationgroup="RejectBDMWPGroup"/>
            </div> 

            <div class="divother">
             <div class="textBlueFont">History</div>
            </div>
            <div class="Historyscroll">
            <div class="rTable">
                <div class="rTableRow">
                <asp:Repeater ID="RepeaterHistory" runat="server" OnItemDataBound="RepeaterHistory_ItemDataBound">
                    <ItemTemplate>
                        <div class="rTableRow">
                            <div class="rTableCell">
                                <asp:Label ID="lbluser" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"User") %>' CssClass="textGreenFont fontbold"></asp:Label>
                                <asp:Label ID="LabelDatetime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"datetime") %>'  CssClass="textBlackFont"></asp:Label>
                            </div>
                        </div>

                        <div class="rTableRow">
                            <div class="rTableCell">
                                <asp:Label ID="lblResultLabel" runat="server" Text="Status :" CssClass="textBlackFont fontbold"></asp:Label>
                                <asp:Label ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Result") %>'  CssClass="textBlackFont"></asp:Label>
                            </div>
                        </div>

                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div><asp:Label ID="lblCommentforApplicationtsLabel" runat="server" Text="Comment for applicants" CssClass="textBlackFont fontbold"></asp:Label></div>
                                <div><asp:Label ID="lblCommentforApplicationts" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CommentForApplicants") %>'  CssClass="textBlackFont"></asp:Label></div>
                            </div>
                        </div>

                        <div class="rTableRow">
                            <div class="rTableCell">
                                <div><asp:Label ID="lblCommentforInternualUseLabel" runat="server" Text="Comment for internal use" CssClass="textBlackFont fontbold"></asp:Label></div>
                                <div><asp:Label ID="lblCommentforInternualUse" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CommentForInternualUse") %>'  CssClass="textBlackFont"></asp:Label></div>
                            </div>
                        </div>
                        <hr />
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            </div>
            </div>

            <div style="text-align: left;margin-top: 50px;" class="btnBox">
                <asp:Label ID="lblmessage" runat="server" Text="" CssClass="messagered" />
                <asp:Button ID="BtnSubmit" runat="server" Text="Submit" data-toggle="modal" data-target="#SaveMessagebox" CssClass="apply-btn skytheme"/>
            </div>
            </asp:Panel>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Label id="lbltest" runat="server" Text="" />

<!-- Modal -->
<div class="modal fade" id="SaveMessagebox" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Reject / Disqualify Application</h4>
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