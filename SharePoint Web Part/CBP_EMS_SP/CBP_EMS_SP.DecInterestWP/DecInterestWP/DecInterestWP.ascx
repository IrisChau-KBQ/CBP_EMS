<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DecInterestWP.ascx.cs" Inherits="CBP_EMS_SP.DecInterestWP.DecInterestWP.DecInterestWP" %>

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

        .resetLineHeight{
            line-height: 0px !important;
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

.maxWidthCompany td{
    max-width: 400px;
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

    .messagered {
        color:red;
    }   
    .margindiv{
        margin-bottom:10px;
    }

</style>
<script src="/_layouts/15/Styles/CBP_Styles/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<asp:Panel ID="mainPanel" runat="server" CssClass="aspPanel" >
<div class="outsideboard  page_main_block" style="width: 100%">


    <div>
        <div>
            <div class="divother" ><h2 class="titleFont">Note of Declaration of Interests</h2></div><br />
        </div>
        <hr />
        
        <div class="rTable">
        <div class="rTableRow">
            <div class="rTableCell lbl width20per textBlueFont">Programme Name</div>
            <div class="rTableCell width30per textBlackFont"><asp:Label ID="lblProName" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl width20per textBlueFont">Intake No.</div>
            <div class="rTableCell width30per textBlackFont"><asp:Label ID="lblIntakeNo" runat="server" Text="N/A" /></div>
        </div>
        <div class="rTableRow">
            <div class="rTableCell lbl textBlueFont">Date</div>
            <div class="rTableCell textBlackFont"><asp:Label ID="lblDate" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl textBlueFont">Time</div>
            <div class="rTableCell textBlackFont"><asp:Label ID="lblTime" runat="server" Text="N/A" /></div>
        </div>
        <div class="rTableRow">
            <div class="rTableCell lbl textBlueFont">Venue</div>
            <div class="rTableCell textBlackFont"><asp:Label ID="lblvenus" runat="server" Text="N/A" /></div>
            <div class="rTableCell lbl textBlueFont">Name</div>
            <div class="rTableCell textBlackFont"><asp:Label ID="lblname" runat="server" Text="N/A" /></div>
        </div>
        </div>

        <hr />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

        <div class="rTable">
        <div class="rTableRow">
            <div class="rTableCell witdth20px ">
                
                <asp:RadioButton id="rbtDec1" GroupName="RegularMenu" Text="&nbsp;" runat="server" Checked="true" CssClass="listcss" OnCheckedChanged="rbtDec1_CheckedChanged" AutoPostBack="true"/>
            </div>
            <div class="rTableCell textGreenFont">Declare that there was no conflict of interest in the list of applications.<asp:Label ID="lblmessageerror1" runat="server" Text="" CssClass="messagered" /></div>
        </div>        
        <div class="rTableRow">
            <div class="rTableCell witdth20px ">
                
                <asp:RadioButton id="rbtDec2" GroupName="RegularMenu" Text="&nbsp;" runat="server"  CssClass="listcss" OnCheckedChanged="rbtDec2_CheckedChanged" AutoPostBack="true"/>
            </div>
            <div class="rTableCell textGreenFont">Declare my interest and will abstain from voting for the application(s) as indicated with a ‘tick’ in the list of applications. I declare that there was no conflict of interest for the rest of applications of the list. <font color='red'>If you have Conflict of Interest of the below applications, please choose this option.</font>
                <asp:Label ID="lblmessageerror2" runat="server" Text="" CssClass="messagered" />
            </div>
        </div>
        </div>
            </ContentTemplate>
            </asp:UpdatePanel>

        <br />
        <div class="textBlackFont margindiv">List of Application</div>
        <div class="Gridbox" >
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
            
        <asp:GridView ID="GridView_App"  runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false" CssClass="GridViewstyle" ShowHeader="true" GridLines="None" ShowHeaderWhenEmpty="false"
            OnRowCommand="GridView_App_RowCommand" OnRowDataBound="GridView_App_RowDataBound" OnRowEditing="GridView_App_RowEditing"
            width="100%" BorderStyle="None" HeaderStyle-CssClass="textBlueFont"  RowStyle-CssClass="textBlackFont">
            <Columns>
                <%--<asp:BoundField DataField="Application_Number" HeaderText="Application_Number" HtmlEncode="False" ItemStyle-CssClass="textGreenFont" ItemStyle-Wrap="true" ReadOnly="True"/>--%>
                <asp:HyperLinkField DataNavigateUrlFormatString="{0}" DataNavigateUrlFields="APPNoURL" DataTextField="Application_Number" HeaderText="Application Number" ItemStyle-CssClass="textGreenFont" ItemStyle-Width="200"  />
                <asp:BoundField DataField="Company" HeaderText="Company" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True" ItemStyle-Width="250px"  />
                <asp:BoundField DataField="Project_Name_Eng" HeaderText="Project Name" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True" ItemStyle-Width="250px"/>
                <asp:BoundField DataField="Programme_Type" HeaderText="Programme Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True" ItemStyle-Width="50px"/>
                <asp:BoundField DataField="CCMF_Application_Type" HeaderText="Application Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True" ItemStyle-Width="50px"/>
                <asp:BoundField DataField="CoreMember" HeaderText="Core Member" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True" ItemStyle-Width="120px"/>
                <asp:TemplateField HeaderText="Conflict of Interest" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:HiddenField ID="HiddenApplication_Number" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>' />
                        <asp:CheckBox ID="chkcoi" runat="server" CssClass="chk_center listcss" Text="&nbsp;"   />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reason" ItemStyle-CssClass="resetLineHeight" >
                    <ItemTemplate>
                        <asp:Label ID="lblReson" runat="server" Text="" />
                        <asp:TextBox ID="TxtReson" runat="server" Text="" Width="100%"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

                    

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbtDec1" />
                    <asp:PostBackTrigger ControlID="rbtDec2" />
                </Triggers>

            </asp:UpdatePanel>
        </div>        
        <div style="text-align: left; margin-top: 50px;" class="btnBox">
            <div class="rTableCell"><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="apply-btn skytheme button_width160"/></div>
            <div class="rTableCell"><asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" CssClass="apply-btn greentheme button_width160"/></div>            
            
        </div>
        <div><label class="" style="margin-top: -1%; right: 3%; position: absolute; font-size: x-small; color: #999999;">Doc Ref: ENC.SF.050</label></div>
    </div>

    <asp:Panel ID="pnlWarning" runat="server" Visible="false">
        <div class="popup-overlay"></div>
        <div class="popup IncubationSubmitPopup" style="width: 35%!important">
            <div class="pos-relative card-theme full-width">
                <div class="pop-close">
                    <asp:ImageButton PostBackUrl="~/SitePages/Application%20List%20for%20Vetting%20Team.aspx" ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="imbClose" OnClick="imbClose_Click" />
                </div>
                <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color:red">
                    <asp:Label Text="You are not able to submit after the vetting team leader has confirmed the result." runat="server" ID="lblMessage" />
                </p>
            </div>
        </div>
    </asp:Panel>
    
</div>
<asp:Label ID="lbltest" runat="server" Text="" />
</asp:Panel>