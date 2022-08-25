<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VMDecisionWebPart.ascx.cs" Inherits="CBP_EMS_SP.VMDecisionWebPart.VMDecisionWebPart.VMDecisionWebPart" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>

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

.RequiredFieldValidator {
    color:red;
}

</style>
    
    <div class="outsideboard">
        <div>
        <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
    <div class="divother" ><h2 class="center">Vetting Meeting Decision Form</h2></div><br />
    <div class="rTableRow">
        <div class="rTableCell">Date </div>
        <div class="rTableCell"><asp:Label ID="lblDate" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Time </div>
        <div class="rTableCell"><asp:Label ID="lblTime" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Application No. </div>
        <div class="rTableCell"><asp:Label ID="lblApplicationNo" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="divother">
        <h4>Name </h4>
        <asp:TextBox ID="txtboxName" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox> 
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="RequiredFieldValidator" runat="server" ErrorMessage="Name cannot be empty." ControlToValidate="txtboxName" validationgroup="VMDecisionWebPartGroup"></asp:RequiredFieldValidator>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Go/ Not Go</div>
        <div class="rTableCell">
            <asp:RadioButtonList ID="radioGoNotGo" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Go" Value="1" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Not Go" Value="0"></asp:ListItem>
            </asp:RadioButtonList>   
        </div>
    </div>
    <div style="text-align: right">
        <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn greentheme button_width160" OnClick="btnsubmit_Click" validationgroup="VMDecisionWebPartGroup"/>
    </div> 
    </asp:Panel>       
        </div>
    </div>
<asp:Panel ID="PopupAlreadyconfirmed" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
             <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <%=SPFunctions.LocalizeUI("TeamLeader_already_Confirmed", "CyberportEMS_Common") %>
            </p>

        </div>
    </div>
</asp:Panel>