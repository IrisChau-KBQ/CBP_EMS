<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvResWP.ascx.cs" Inherits="CBP_EMS_SP.InvResWP.InvResWP.InvResWP" %>


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
</style>
    
    
    <div class="outsideboard">
        <div>
        <asp:Panel ID="panel1" runat="server" CssClass="aspPanel" >
    <div class="divother" ><h2 class="center">Invitation Response by Applicant</h2></div><br />
    <div class="rTableRow">
        <div class="rTableCell">Date </div>
        <div class="rTableCell"><asp:Label ID="lblDate" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Venue </div>
        <div class="rTableCell"><asp:Label ID="lblVenue" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Application No. </div>
        <div class="rTableCell"><asp:Label ID="lblApplicationNo" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Name </div>
        <div class="rTableCell"><asp:Label ID="lblName" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Email </div>
        <div class="rTableCell"><asp:TextBox ID="txtEmail" runat="server" Text=""></asp:TextBox></div>
    </div>
    <div class="rTableRow">
        <div class="rTableCell">Mobile Number </div>
        <div class="rTableCell"><asp:TextBox ID="txtMobile" runat="server" Text=""></asp:TextBox></div>
    </div>

    <div class="rTableRow">
        <div class="rTableCell">Attend </div>
        <div class="rTableCell">
            <asp:RadioButtonList ID="radioAttend" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                <asp:ListItem Text="No" Value="0"></asp:ListItem>
            </asp:RadioButtonList>   
        </div>
    </div>

    <div class="divother">
        <div class="rTableCell">Name of Attendees</div>
        <asp:TextBox ID="txtNameOfAttendees" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>                
    </div>

    <div class="divother">
        <div class="rTableCell">Type of Presentation Tools</div>
        <asp:TextBox ID="txtPresentTools" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>                
    </div>

    <div class="divother">
        <div class="rTableCell">Special Request</div>
        <asp:TextBox ID="txtSpecialRequest" runat="server" TextMode="MultiLine" MaxLength="300" CssClass="txtarea"></asp:TextBox>                
    </div>


    <div class="rTableRow">
        <div class="rTableCell">Video Clip </div>
        <div class="rTableCell"><asp:TextBox ID="txtVideoClip" runat="server" Text="" ></asp:TextBox></div>
    </div>

    <div class="rTableRow">
        <div class="rTableCell">Presentation Slide </div>
        <div class="rTableCell"><asp:FileUpload ID="upPresentSlide" runat="server" /></div>
    </div> 
        
    <div style="text-align: right">
        <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="apply-btn greentheme button_width160" OnClick="btnsubmit_Click" />
    </div> 
    </asp:Panel>       
        </div>
    </div>