<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvitationResSum.ascx.cs" Inherits="CBP_EMS_SP.InvitationResSum.InvitationResSum.InvitationResSum" %>

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
    vertical-align: text-top;
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

    .button_width260{
    width:260px;
}
</style>

<script>
    function StartDownload() {
        setTimeout(
            function () {
                _spFormOnSubmitCalled = false;
                _spSuppressFormOnSubmitWrapper = true;
                //location.reload();
            }, 1500);
    }
</script>

<asp:Panel ID="mainPanel" runat="server" CssClass="aspPanel" >
<div class="card-theme page_main_block" style="width: 100%">
    <div>
        <div>
            <div class="divother" ><h2 >Invitation Response Summary</h2></div><br />
        </div>
        <hr />
        <div class="rTable">
            <div class="rTableRow">
                <div class="rTableCell lbl width20per">Date</div>
                <div class="rTableCell width30per"><asp:Label ID="lblDate" runat="server" Text="N/A" /></div>
                <div class="rTableCell lbl width20per">Venue</div>
                <div class="rTableCell width30per"><asp:Label ID="lblVenue" runat="server" Text="N/A" /></div>
            </div>
            <div class="rTableRow">
                <div class="rTableCell lbl width20per">Programme Name</div>
                <div class="rTableCell width30per"><asp:Label ID="lblProName" runat="server" Text="N/A" /></div>
                <div class="rTableCell lbl width20per">Intake No.</div>
                <div class="rTableCell width30per"><asp:Label ID="lblIntakeNo" runat="server" Text="N/A" /></div>
            </div>
        </div>
        <hr />
        <br />
        <div class="rTableCell">
            <asp:Button ID="btnDownload" runat="server" OnClick="btnDownload_Click" Text="Download All Attachments" CssClass="apply-btn bluetheme button_width260 "  ValidationGroup="DownloadZip"/>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Your email is empty and the zipped file cannot be downloaded." ForeColor="Red"  OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="DownloadZip" Display="Dynamic"></asp:CustomValidator>
            <asp:Label ID="lbldownloadmessage" runat="server"></asp:Label>
        </div>

        <div class="rTableCell">
            <asp:Button ID="btnCSV" runat="server" OnClick="btnCSV_Click" Text="Export Summary" CssClass="apply-btn bluetheme button_width260 "  OnClientClick="StartDownload()"/>

        </div>

        <br/>    
        <asp:GridView ID="GridView_App" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false" CssClass="GridViewstyle" ShowHeader="true" GridLines="None" ShowHeaderWhenEmpty="true"
                OnRowCommand="GridView_App_RowCommand" OnRowDataBound="GridView_App_RowDataBound" OnRowEditing="GridView_App_RowEditing"
                width="100%" BorderStyle="None" >
                <Columns>
                    <asp:TemplateField HeaderText="Application No.">
                        <ItemTemplate>
                        <asp:Label ID="lblApplication_Number" runat="server" CssClass="greenlbl" Text='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Company" HeaderText="Company" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Project_Name_Eng" HeaderText="Project Name" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Programme_Type" HeaderText="Programme Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="CCMF_Application_Type" HeaderText="Application Type" HtmlEncode="False" ControlStyle-CssClass="cssclass2" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Name_of_Principal_Applicant" HeaderText="Name of Principal Applicant" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Email" HeaderText="Email" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Mobile_Number" HeaderText="Mobile No." HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Attendance" HeaderText="Attend" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Name_of_Attendees" HeaderText="Name of Attendees" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Presentation_Tools" HeaderText="Type of Presentation Tools" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:BoundField DataField="Special_Request" HeaderText="Special Request" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:TemplateField HeaderText="Video Clip">
                        <ItemTemplate>
                        <asp:Label ID="lblVideoClip" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Upload Presentation Slide">
                        <ItemTemplate>
                        <asp:Label ID="lblUploadPresentationSlide" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblUploadPresentationSlide_full" runat="server" Text="" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Agreement" HeaderText="Marketing Agreement" HtmlEncode="False" ControlStyle-CssClass="cssclass3" ItemStyle-Wrap="true" ReadOnly="True"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="HiddenVetting_Application_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Application_ID") %>' />
                            <asp:HiddenField ID="HiddenVetting_Meeting_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Vetting_Meeting_ID") %>' />
                            <asp:HiddenField ID="HiddenPresentation_From" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Presentation_From") %>' />
                            <asp:HiddenField ID="HiddenPresentation_To" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Presentation_To") %>' />
                            <asp:HiddenField ID="HiddenAttendance" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Attendance") %>' />
                            <asp:HiddenField ID="HiddenApplication_Number" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Application_Number") %>' />
                            
                        </ItemTemplate> 
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        <div class="rTableCell"><asp:Button ID="btnback" runat="server" Text="Back" CssClass="apply-btn greentheme button_width160" OnClick="btnback_Click"/></div>
        

    </div>
</div>


    <asp:Label ID="lbltest" runat="server" Text="" />
</asp:Panel>