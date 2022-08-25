<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyManagement.ascx.cs" Inherits="CBP_EMS_SP.CompanyManagement.VisualWebPart1.CompanyManagement" %>

<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<asp:HiddenField runat="server" ID="hdn_CompanyID" />
<style>
    .emscontent .input-sm {
        width: 95%;
    }

    .IncubationSubmitPopup {
        height: 70vh;
        overflow-y: scroll;
    }

    .subhead {
        font-size: 22px;
    }

    .subhead1 {
        font-size: 18px;
    }

    .clearfix {
        clear: both;
        display: block;
    }

    .emscontent table.datatable td, .emscontent table.datatable th {
        height: auto;
        text-align: left;
    }

        .emscontent table.datatable td:first-child {
            color: #6D6E71;
            font-weight: normal;
        }
</style>
<style>
    .ui-datepicker-trigger {
        vertical-align: top;
    }
</style>
<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->

        <div class="custom-form-wd-img border-gray pagewhiteblock">
            <br />

            <div class="form" style="width: 95%">
                <h1 class="form__h1" style="text-align: left">Company Profile Management         
                </h1>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Panel runat="server" ID="pnlQuickLinks" BorderStyle="Solid" BorderWidth="1px" BorderColor="#cccccc">
                            <ul>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_1" OnClick="quicklnk_1_Click" Text="Basic Information" CommandArgument="1"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_2" OnClick="quicklnk_1_Click" Text="Members" CommandArgument="2"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_3" OnClick="quicklnk_1_Click" Text="Programme" CommandArgument="3"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_4" OnClick="quicklnk_1_Click" Text="Reimbursement" CommandArgument="4"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_5" OnClick="quicklnk_1_Click" Text="Fund" CommandArgument="5"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_6" OnClick="quicklnk_1_Click" Text="Merge & Acquisition" CommandArgument="6"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_7" OnClick="quicklnk_1_Click" Text="Awards" CommandArgument="7"></asp:LinkButton>
                            </li>
                                <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_10" OnClick="quicklnk_1_Click" Text="Joined Accelerator" CommandArgument="10"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_8" OnClick="quicklnk_1_Click" Text="IP" CommandArgument="8"></asp:LinkButton>
                            </li>
                            <li style="padding-left:5px">
                                <asp:LinkButton runat="server" ID="quicklnk_9" OnClick="quicklnk_1_Click" Text="Manage Administrator" CommandArgument="9"></asp:LinkButton>
                            </li>
                                </ul>
                            <br />
                            <br />
                           <asp:HiddenField ID="hdn_ActiveStep" runat="server" Value="0" />
                        </asp:Panel>
                    </div>
                    <div class="col-md-9">

                        <asp:Panel runat="server" ID="pnl_BasicProfile" Visible="false">
                            <div>
                                <h1 class="subhead">Basic Profile</h1>
                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>Profile Name (Eng)</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtNameEng" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNameEng" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="Profile Name (Eng) required"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>Profile Name (Chi)</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtNameChi" CssClass="input-sm"></asp:TextBox>
<%--                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNameChi" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="Profile Name (Chi) required"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>Company Name</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtCompanyName" MaxLength="255" CssClass="input-sm"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>Brand Name</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtBrandName" MaxLength="255" CssClass="input-sm"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class=" selectboxheight">
                                            <p>
                                                <label>Cluster - CCMF</label>
                                            </p>
                                            <asp:DropDownList runat="server" ID="ddlClusterCCMF" CssClass="input-sm"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class=" selectboxheight">
                                            <p>
                                                <label>Cluster - CPIP</label>
                                            </p>
                                            <asp:DropDownList runat="server" ID="ddlclusterCPIP" CssClass="input-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4">

                                        <p>Tags</p>
                                        <asp:TextBox runat="server" ID="txtcompanyTag" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>CCMF Abstract (Eng)</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtCCMFAbsEng" TextMode="MultiLine" MaxLength="255" CssClass="form-control"></asp:TextBox>
<%--                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCCMFAbsEng" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="required"></asp:RequiredFieldValidator>--%>

                                    </div>

                                </div>
                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>CCMF Abstract (Chi)</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtCCMFAbsChi" TextMode="MultiLine" MaxLength="255" CssClass="form-control"></asp:TextBox>
<%--                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCCMFAbsChi" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="required"></asp:RequiredFieldValidator>--%>


                                    </div>

                                </div>
                                <div class="row">

                                    <p>
                                        <label>CPIP Abstract (Eng)</label>
                                    </p>
                                    <asp:TextBox runat="server" ID="txtCPIPAbsEng" TextMode="MultiLine" MaxLength="255" CssClass="form-control"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCPIPAbsEng" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="required"></asp:RequiredFieldValidator>--%>



                                </div>

                                <div class="row">

                                    <p>
                                        <label>CPIP Abstract (Chi)</label>
                                    </p>
                                    <asp:TextBox runat="server" ID="txtCPIPAbsChi" TextMode="MultiLine" MaxLength="255" CssClass="form-control"></asp:TextBox>
<%--                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCPIPAbsChi" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="required"></asp:RequiredFieldValidator>--%>
                                </div>

                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>CASP Abstract</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtCASPAbstract" TextMode="MultiLine" MaxLength="255" CssClass="form-control"></asp:TextBox>
                                       
                                    </div>

                                </div>


                                <div class="row">
                                    <div class="">
                                        <p>
                                            <label>Company Ownership Structure</label>
                                        </p>
                                        <asp:TextBox runat="server" ID="txtcompanyOwnership" TextMode="MultiLine" MaxLength="255" CssClass="form-control"></asp:TextBox>
<%--                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtcompanyOwnership" ValidationGroup="valcompanybasic" ForeColor="Red" ErrorMessage="required"></asp:RequiredFieldValidator>--%>

                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="">
                                            <p>
                                                <label>Remarks</label>
                                            </p>
                                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input-sm"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <br />
                                        <br />
                                        <asp:FileUpload runat="server" ID="fu_companyAttachement" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" />
                                        <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" ID="ImageButton14" ToolTip="Click to Upload" OnClick="SaveAttachment_Click" CommandName="1" />
                                        <br />

                                        <br />

                                    </div>


                                </div>
                                <asp:Repeater runat="server" ID="rptrdocuments" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                    <HeaderTemplate>
                                        <table style="width: 100%;" cellpadding="1">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 60%">
                                                <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <br />
                                <asp:Label Text="" ID="lblfiletype" runat="server" CssClass="text-gray-client" />
                                <br />
                                <asp:Label Text=" " ID="lblfilesize" runat="server" CssClass="text-gray-client" />
                                <br />
                                <asp:Label Text="" ID="lbldocuments" runat="server" CssClass="text-danger" />


                                <br />

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button runat="server" ID="btn_Save" ValidationGroup="valcompanybasic" OnClick="btn_Save_Click" CssClass="apply-btn bluetheme" Text="Save" />
                                    </div>
                                </div>

                                <div id="lbl_Exception" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                            </div>
                        </asp:Panel>


                        <asp:Panel runat="server" ID="pnl_CoreMember" Visible="false">

                            <div>

                                <h1 class="subhead">Core Members</h1>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_Add_CoreMember" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" Text="New" CommandArgument="1" OnClick="openPopup" />
                                </div>
                                <div class="clearfix"></div>
                                <asp:GridView runat="server" ID="gdv_CoreMember" OnRowCommand="gdv_CoreMember_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Position" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Position") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CCMF" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Convert.ToBoolean(Eval("CCMF"))?"Yes" : "No" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CPIP" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Convert.ToBoolean(Eval("CPIP"))?"Yes" : "No"  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CASP" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Convert.ToBoolean(Eval("CASP"))?"Yes" : "No"  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Masked HKID" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>

                                                <asp:Label runat="server" Text='<%#Eval("Masked_HKID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Background Information" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Background_Information") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bootcamp Eligible Number" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Bootcamp_Eligible_Number") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Professional Qualification" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Professional_Qualifications") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Working Experience" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Working_Experiences") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Special Achievements" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Special_Achievements") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Core Member Profile" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("CoreMember_Profile") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_Member_Edit" Visible='<%#(Eval("No_Edit")!=null?((bool) Eval("No_Edit")==true?false:true):true) %>' CommandName="EditRow" CommandArgument='<%#Eval("Core_Member_ID") %>' Text="Edit" CssClass="btn brn-sm btn_primary " />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <br />
                                <br />
                                <h1 class="subhead">Contact</h1>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_Add_Contacts" Text="New" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" CommandArgument="2" OnClick="openPopup" />
                                </div>
                                <div class="clearfix"></div>

                                <asp:GridView runat="server" ID="gdv_Contact" OnRowCommand="gdv_Contact_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name(Eng)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtNameeng" Text='<%# Convert.ToString( Eval("Name_Eng")).Replace(":"," ") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Name(Chi)" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtNameChi" Text='<%#Convert.ToString( Eval("Name_Chi")).Replace(":"," ") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Salutation" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsalutation" Text='<%#Eval("Salutation") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Position" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtposition" Text='<%#Eval("Position") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Contact No" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtcontactNo" Text='<%#Eval("Contact_No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Fax" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtfax" Text='<%#Eval("Fax_No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtEmail" Text='<%#Eval("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Mailing Address" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtMailingAddress" Text='<%#Eval("Mailing_Address") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">

                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_Contact_Edit" Visible='<%#(Eval("No_Edit")!=null?((bool) Eval("No_Edit")==true?false:true):true) %>' Text="Edit" CommandName="EditRow" CommandArgument='<%#Eval("Contact_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>


                            </div>

                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnl_Programme" Visible="false">
                            <div class="">
                                <h1 class="subhead">Programme</h1>
                                <div>
                                    <h1 class="subhead1">CCMF</h1>
                                    <div class="griidview">
                                        <asp:GridView runat="server" ID="gdv_ccmf_Programme" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                            ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Intake" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("Intake_Number") %>' ID="lblCCMfIntake"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Application No" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMfApplicationNo" Text='<%#Eval("Application_Number") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Programme Type" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFProgrammeType" Text='<%#Eval("Programme_Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Programme Stream" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFProgrammeStream" Text='<%#Eval("Hong_Kong_Programme_Stream") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Application Type" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFApplicationType" Text='<%#Eval("CCMF_Application_Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Cluster" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFCluster" Text='<%#Eval("Business_Area") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Project Name(Eng)" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFProjectEng" Text='<%#Eval("Project_Name_Eng") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Project Name(Chi)" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMfProjectChi" Text='<%#Eval("Project_Name_Eng") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Full Description(Eng)" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFDescriptionEng" Text='<%#Eval("Abstract_Chi") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Full Description(Chi)" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMfDescriptionChi" Text='<%#Eval("Abstract_Chi") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCCMFStatus" Text='<%#Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="View Application Form" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a target="_blank" href='<%# "/SitePages/CCMF.aspx?prog="+Eval("Programme_ID") +"&app="+ Eval("CCMF_ID") +"&resubmitversion=Y&new=Y"%>'>View</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                                <br />
                                <br />

                                <div class="clearfix"></div>
                                <div>
                                    <h1 class="subhead1">CPIP</h1>

                                    <asp:GridView runat="server" ID="Gdv_Cpip_Programme" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                        ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Intake No" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPIntakeNo" Text='<%#Eval("Intake_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Application No" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPApplicationNo" Text='<%#Eval("Application_Number") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Cluster" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPCluster" Text='<%#Eval("Business_Area") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Company Name(Eng)" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPCompanyNameEng" Text='<%#Eval("Company_Name_Eng") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Company Name(Chi)" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPCompanyNameChi" Text='<%#Eval("Company_Name_Chi") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Full Description(Eng)" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPDescriptionEng" Text='<%#Eval("Abstract") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Full Desctiption(Chi)" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPDescriptionChi" Text='<%#Eval("Abstract_Chi") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCPIPStatus" Text='<%#Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="View Application Form" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>

                                                    <a target="_blank" href='<%# "/SitePages/IncubationProgram.aspx?prog="+ Eval("Programme_ID") +"&app="+ Eval("Incubation_ID") +"&resubmitversion=Y&new=Y"%>'>View</a>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                </div>
                                <br />
                                <br />
                                <div class="clearfix"></div>

                                <div>
                                    <h1 class="subhead1">CASP</h1>
                                    <asp:GridView runat="server" ID="Gdv_CASP_Programme" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                        ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Submission month" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCASPSubmissionmonth" Text='<%#Eval("Submitted_Date") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Application No" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCASPApplicationNo" Text='<%#Eval("Application_No") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Company/Project Name(Eng)" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCASPProjectNameEng" Text='<%#Eval("Company_Project") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%--<asp:TemplateField HeaderText="Company/Project Name (Chi)"  HeaderStyle-HorizontalAlign="Center"> 
                                   <ItemTemplate>
                                       <asp:Label runat="server" ID="lblCASPProjectNameChi" Text='<%#Eval("") %>'></asp:Label>
                                       </ItemTemplate>
                                       </asp:TemplateField>--%>


                                            <asp:TemplateField HeaderText="Accelerator Programme" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCASPAcceleratorProgramm" Text='<%#Eval("Accelerator_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCASPStatus" Text='<%#Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="View Application Form" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <a target="_blank" href='<%# "/SitePages/CASPProgram.aspx?prog="+ Eval("Programme_ID") +"&app="+ Eval("CASP_ID") +"&resubmitversion=Y&new=Y"%>'>View</a>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnl_Reimbursement" Visible="false">
                            <h1 class="subhead">Reimbursement</h1><br />
                            <div class="">
                                <div>
                                    <h1 class="subhead1">CASP Financial Assistance Reimbursment</h1>
                                    <table class="datatable fullwidth" style="width: 100%">
                                        <tr>
                                            <th>Total Amount</th>
                                            <th>Initial Capital</th>
                                            <th>Claimed Amount</th>
                                            <th>In-Progress Amount</th>
                                            <th>Balance</th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblCASPFATotalAmount"></asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" ID="lblInitialCapital"></asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCASPFAClaimedAmount"></asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCASPFAProgressAmount"></asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCASPFABalance"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <div>
                                    <asp:GridView runat="server" ID="gdvFaCASP" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                        ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Reimbursement No" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Application_No") %>' ID="lblApplicationNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Category") %>' ID="Label13"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Submission Date" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Submitted_Date","{0:dd MMM yyyy}") %>' ID="Label14"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Total_Amount","{0:#,##0.00}") %>' ID="Label15"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Status") %>' ID="Label17"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Special Request Form" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnViewSR" Visible='<%# Convert.ToString(Eval("Preapproved_SpecialRequest"))=="" ? false : true %>' PostBackUrl='/SitePages/Special_Request_CASP.aspx?new=y&app=<%#Eval("Preapproved_SpecialRequest")%>'>View</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Reimbursement Form" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnViewReimbursement" PostBackUrl='<%# "/SitePages/Financial_Reimbursements_CASP.aspx?new=Y&app="+Eval("CASP_FA_ID")%>'>View</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                                <br />
                                <br />
                                 <div class="clearfix"></div>
                                        <div>
                                             <%--<h1 class="subhead">Special Request</h1>--%>
                                            <div>
                                                <h1 class="subhead1">CASP Special Request</h1><br />
                                            
                                    <asp:GridView runat="server" ID="gdvSR" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                        ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Application No" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Application_No") %>' ID="lblApplicationNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Service Provider Name" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Service_Provider_Name") %>' ID="lblApplicationNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Estimate Amount" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Estimate_Amount","{0:#,##0.00}") %>' ID="lblApplicationNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Submitted Date" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Submitted_Date", "{0:dd MMM yyyy}") %>' ID="lblApplicationNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Status") %>' ID="lblApplicationNo"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                       <a target="_blank" href='<%# "/SitePages/Special_Request_CASP.aspx?new=y&app="+Eval("CASP_Special_Request_ID")%>'>View</a>
                                                   
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            </Columns>
                                        </asp:GridView>
                                                </div>
                                            </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Pnl_Fund" Visible="false">
                            <div class="">
                                <h1 class="subhead">Fund</h1>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_fund_AddNew" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" CommandArgument="3" OnClick="openPopup" Text="New" />
                                </div>
                                <div class="clearfix"></div>
                                <asp:GridView runat="server" ID="gdv_Fund" OnRowCommand="gdv_Fund_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>

                                                <asp:Label runat="server" ID="lblfundcreatedate" Text='<%#Convert.ToDateTime(Eval("Created_Date")).ToString("dd MMM yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Programme Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundprogname" Text='<%#Eval("Programme_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Programme Type" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundProgtype" Text='<%#Eval("Programme_Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Application No" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundAppNo" Text='<%#Eval("Application_No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Application Status" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundAppstatus" Text='<%#Eval("Application_Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Funding Status" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundstatus" Text='<%#Eval("Funding_Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nature of Expenditure Covered" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundnature" Text='<%#Eval("Expenditure_Nature") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFundCurrency" Text='<%#Eval("Currency") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount Received/ to be Received" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFundReceived" Text='<%#Convert.ToDecimal(Eval("Amount_Received")).ToString("#,##0.00") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Maximum Amount to be Received" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundMaxamount" Text='<%#Convert.ToDecimal(Eval("Maximum_Amount")).ToString("#,##0.00") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Investor Info" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundInfo" Text='<%#Eval("Invertor_Info") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Funding Org" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFundOrigin" Text='<%#Eval("Funding_Origin") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblfundRemark" Text='<%#Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnFundEdit" Visible='<%#((bool) Eval("No_Edit")==true?false:true) %>' CommandArgument='<%#Eval("Funding_ID") %>' CommandName="EditRow" Text="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnl_MergeAcquisition" Visible="false">
                            <div class="">
                                <h1 class="subhead">Merge & Acquisition</h1>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_MergeAcquNew" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" CommandArgument="4" OnClick="openPopup" Text="New" />

                                </div>
                                <div class="clearfix"></div>

                                <asp:GridView runat="server" ID="gdv_MergeAcquition" OnRowCommand="gdv_MergeAcquition_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Company" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmnacompany" Text='<%#Eval("Company_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Merge/Acquisition" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmna" Text='<%#Eval("Merge_Acquistion") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmnacurrency" Text='<%#Eval("Currency") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmnaamount" Text='<%#Eval("Amount","{0:#,##0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Valuation" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmnavaluation" Text='<%#Eval("Valuation","{0:#,##0.00}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmnadate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_Merge_Edit" CommandName="EditRow" Text="Edit" CommandArgument='<%#Eval("Merge_Acquistion_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="Pnl_Awards" Visible="false">
                            <div class="">
                                <h1 class="subhead">Awards</h1>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_Awards_New" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" CommandArgument="5" OnClick="openPopup" Text="New" />

                                </div>
                                <div class="clearfix"></div>

                                <asp:GridView runat="server" ID="Gdv_awards" OnRowCommand="Gdv_awards_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Awarded(Year-Month)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>

                                                <asp:Label runat="server" ID="lblawardyear" Text='<%# Convert.ToDateTime( Eval("Awarded_Year_Month")).ToString("MMM yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Awarded_Year_Month")).ToString("MMM-yyyy") : "" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblawardtype" Text='<%#Eval("Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Award/Recognition" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblawardrecognition" Text='<%#Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Nature of Awardee" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblawardnature" Text='<%#Eval("Nature") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Product(Chi)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblawardproduct" Text='<%#Eval("Product_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Award(Chi)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblawardchi" Text='<%#Eval("Award_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblawardremark" Text='<%#Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_Award_edit" Text="Edit" CommandArgument='<%#Eval("Award_ID") %>' CommandName="EditRow" CssClass="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>

                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnl_JoinedAccelerator" Visible="false">
                            <div class="">
                                <h1 class="subhead">Joined Accelerator</h1>
                               
                                <div style="float: right">
                                     <asp:Button Text="New" ID="btn_Accelerator" runat="server" style="margin-bottom: 15px;" CssClass="apply-btn primary-theme theme-blue" CommandArgument="9" OnClick="openPopup"  />
                                </div>
                                <div class="clearfix"></div>

                                <asp:GridView runat="server" ID="gdv_JoinedAccelerator" OnRowCommand="gdv_JoinedAccelerator_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Year-Month" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbljoinedyear" Text='<%# Convert.ToDateTime( Eval("Participation_Year_Month")).ToString("MMM yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Participation_Year_Month")).ToString("MMM yyyy") : "" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Accelerator Programme" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbljoinedprogramme" Text='<%#Eval("Accelerator_Programme") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbljoinedremarks" Text='<%#Eval("Remarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_Accelerator_Edit" Text="Edit" CommandArgument='<%#Eval("Joined_Accelerator_ID") %>' CommandName="EditRow" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnl_Ip" Visible="false">
                            <div class="">
                                <h1 class="subhead">IP</h1>

                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_IPNew" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" CommandArgument="7" OnClick="openPopup" Text="New" />

                                </div>
                                <div class="clearfix"></div>

                                <asp:GridView runat="server" ID="gdv_IP" OnRowCommand="gdv_IP_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbliptitle" Text='<%#Eval("Title") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblipcategory" Text='<%#Eval("Category") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Registration Date" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblipregdate" Text='<%# Convert.ToDateTime( Eval("Registration_Date")).ToString("MMM yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Registration_Date")).ToString("MMM yyyy") : "" %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Reported Date" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblipreportdate" Text='<%# Convert.ToDateTime( Eval("Reported_Date")).ToString("MMM yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Reported_Date")).ToString("MMM yyyy") : "" %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbliprefrenceno" Text='<%#Eval("Reference_No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_IP_Edit" CommandName="EditRow" CommandArgument='<%#Eval("IP_ID") %>' Text="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>

                                </asp:GridView>
                            </div>
                        </asp:Panel>


                        <asp:Panel runat="server" ID="Pnl_ManageAdministrator" Visible="false">
                            <div class="">
                                <h1 class="subhead">IP
                               Manage Administrator</h1>
                                <div style="float: right">
                                    <asp:Button runat="server" ID="btn_ManageAdmin" CssClass="apply-btn primary-theme theme-blue" Style="margin-bottom: 15px;" CommandArgument="8" OnClick="openPopup" Text="Add Admin" />

                                </div>
                                <div class="clearfix"></div>

                                <asp:GridView runat="server" ID="gdv_ManageAdministrator" OnRowCommand="gdv_ManageAdministrator_RowCommand" CssClass="datatable" ShowHeaderWhenEmpty="True" ShowFooter="false" AutoGenerateColumns="False"
                                    ShowHeader="true" GridLines="Horizontal" AlternatingRowStyle-BackColor="#eeeeee" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Admin Full Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbladminFullName" Text='<%#Eval("Full_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email Address" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbladminEmail" Text='<%#Eval("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btn_AdminEdit" CommandArgument='<%#Eval("Administrator_ID") %>' CommandName="EditRow" Text="Edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>

                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnl_CoreMemberPopup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2coremember"></h2>
                                    <asp:HiddenField runat="server" ID="hdn_MemberId" />
                                    <div class="">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Name</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreName" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Name Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="cormember" ControlToValidate="txtCoreName" runat="server" />
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Position</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCorePosition" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Name Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="cormember" ControlToValidate="txtCorePosition" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>HKID</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreHkid" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="HKID Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="cormember" ControlToValidate="txtCoreHkid" runat="server" />

                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Background</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreBackground" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Bootcamp Eligible Number</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreBootcamp" CssClass="input-sm"></asp:TextBox>
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Professional Qualification</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreQualification" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Working Experiences</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreWorkingExperience" CssClass="input-sm"></asp:TextBox>
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Special Achievements</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreAchievement" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Core Member Profiles</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtCoreMemberProfile" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <asp:Button runat="server" ID="btnCoreCancel" Text="Cancel" OnClick="cancelClosePopup" CssClass="apply-btn greentheme" />                                        
                                        <asp:Button runat="server" ID="btnCoreSave" Text="Save" OnClick="btnCoreSave_Click" ValidationGroup="cormember" CssClass="apply-btn bluetheme" />
                                        <asp:Button runat="server" ID="btn_CoreMember_Delete" Text="Remove"  Visible="false" OnClick="open_Delete_confirmation" CommandArgument="1" CssClass="apply-btn bluetheme "/>
                                    </div>

                                    <div>

                                        <div id="Div2" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                        <asp:BulletedList ID="corememberperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                                    </div>
                                </div>
                            </div>


                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnl_ContactPopup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2contact"></h2>

                                    <asp:HiddenField runat="server" ID="hdn_contactID" />
                                    <div>
                                        <div class="row">

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Last Name(Eng)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactLastNameEng" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContactLastNameEng" ValidationGroup="contactval" ForeColor="Red" ErrorMessage="Last Name (Eng) required"></asp:RequiredFieldValidator>

                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>First Name(Eng)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactFirstNameEng" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContactFirstNameEng" ValidationGroup="contactval" ForeColor="Red" ErrorMessage="First Name (Eng) required"></asp:RequiredFieldValidator>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Last Name(Chi)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactLastNameChi" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContactFirstNameEng" ValidationGroup="contactval" ForeColor="Red" ErrorMessage="Last Name (Chi) required"></asp:RequiredFieldValidator>
                                            </div>


                                            <div class="col-md-5">
                                                <p>
                                                    <label>First Name(Chi)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactFirstNameChi" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContactFirstNameChi" ValidationGroup="contactval" ForeColor="red" ErrorMessage="First Name (Chi) required"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5 selectboxheight">
                                                <p>
                                                    <label>Salutation</label>
                                                </p>
                                                <asp:DropDownList ID="ddlContactSalutation" Width="60%" runat="server" Style="height: 40px">
                                                    <asp:ListItem Text="Dr" Value="Dr"></asp:ListItem>
                                                    <asp:ListItem Text="Mr" Value="Mr"></asp:ListItem>
                                                    <asp:ListItem Text="Ms" Value="Ms"></asp:ListItem>
                                                    <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>


                                            <div class="col-md-5">
                                                <p>
                                                    <label>HKID</label>
                                                </p>
                                                <asp:TextBox runat="server" MaxLength="255" ID="txtContactHKID" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContactHKID" ValidationGroup="contactval" ForeColor="Red" ErrorMessage="HKID required"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Education Institution(Eng)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactInstitution" MaxLength="255" CssClass="input-sm"></asp:TextBox>

                                            </div>


                                            <div class="col-md-5">
                                                <p>
                                                    <label>Student ID Number</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactStudentID" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Programme Enrolled</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactProgrammeEnrolled" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                            </div>


                                            <div class="col-md-5">
                                                <p>
                                                    <label>Gradutation (Year - Month)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactGraduationDate" MaxLength="255" CssClass="hidedates datepickerYM input-sm" Width="60%"></asp:TextBox>

                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Organization Name</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactOrganizationName" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                            </div>


                                            <div class="col-md-5">
                                                <p>
                                                    <label>Position</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactPosition" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Contact No.(Home) </label>
                                                </p>
                                                <asp:TextBox runat="server" ID="TxtContactNoHome" CssClass="input-sm"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" Display="Dynamic" ControlToValidate="TxtContactNoHome" ForeColor="red" ValidationGroup="contactval" ErrorMessage="Contact No. (Home) required"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ForeColor="red" Display="Dynamic" ErrorMessage="Invalid Contact No.(Home)" ControlToValidate="TxtContactNoHome" ValidationGroup="contactval" ValidationExpression="^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{2})[-. )]*(\d{2})[-. ]*(\d{4})(?: *x(\d+))?\s*$"></asp:RegularExpressionValidator>

                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Contact No.(office)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="TxtContactNoOffice" CssClass="input-sm"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ForeColor="red" ErrorMessage="Invalid Contact No.(Office)" ControlToValidate="TxtContactNoOffice" ValidationGroup="contactval" ValidationExpression="^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{2})[-. )]*(\d{2})[-. ]*(\d{4})(?: *x(\d+))?\s*$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Contact No.(Mobile)</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="TxtContactNoMobile" CssClass="input-sm"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ForeColor="red" ErrorMessage="Invalid Contact No.(Mobile)" ControlToValidate="TxtContactNoMobile" ValidationGroup="contactval" ValidationExpression="^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{2})[-. )]*(\d{2})[-. ]*(\d{4})(?: *x(\d+))?\s*$"></asp:RegularExpressionValidator>

                                            </div>


                                            <div class="col-md-5">
                                                <p>
                                                    <label>Fax No</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactFaxNo" CssClass="input-sm"></asp:TextBox>
                                                <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="Invalid Fax" ControlToValidate="txtContactFaxNo" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />

                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <p>
                                                    <label>Email</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactEmail" CssClass="input-sm"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regexEmailValid" Display="Dynamic" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtContactEmail" ErrorMessage="Invalid Email Format" ForeColor="Red"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <p>
                                                    <label>Mailing Address</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactMailingAddress" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <p>
                                                    <label>Area</label>
                                                </p>
                                                <asp:TextBox runat="server" ID="txtContactArea" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <asp:Button runat="server" ID="btn_Contact_Cancel" CssClass="apply-btn greentheme" OnClick="cancelClosePopup" Text="Cancel" />
                                        <asp:Button runat="server" ID="btn_Contact_Save" ValidationGroup="contactval" CssClass="apply-btn bluetheme" OnClick="btn_Contact_Save_Click" Text="Save" />
                                        <asp:Button runat="server" ID="btn_contact_remove" Visible="false"  OnClick="open_Delete_confirmation" CommandArgument="2" Text="Remove" CssClass="apply-btn bluetheme" />
                                        <div id="Div4" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                        <asp:BulletedList ID="contactMemberError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Pnl_Fund_Popup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2fund"></h2>
                                    <asp:HiddenField runat="server" ID="hdn_FundingID" />
                                    <div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Reported Date</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="hidedates datepickerYM input-sm" Width="60%" ID="txtFundReportedDate"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Reported Date Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundReportedDate" runat="server" />
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Received Date</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="hidedates datepickerYM input-sm" Width="60%" ID="txtFundReceivedDate"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Received Date Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundReceivedDate" runat="server" />
                                            </div>

                                        </div>
                                        <div class="row">

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Name of Programme</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtFundProgrammeName"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Programme Name Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundProgrammeName" runat="server" />
                                            </div>

                                            <div class="col-md-5 selectboxheight">
                                                <p>
                                                    <label>Application Status</label>
                                                </p>
                                                <asp:DropDownList runat="server" CssClass="input-sm" ID="ddlFundApplicationStatus">
                                                    <asp:ListItem Value="Submitted" Text="Submitted"></asp:ListItem>
                                                    <asp:ListItem Value="Under negotiation" Text="Under negotiation"></asp:ListItem>
                                                    <asp:ListItem Value="Completed" Text="Completed"></asp:ListItem>
                                                    <asp:ListItem Value="Declined" Text="Declined"></asp:ListItem>
                                                    <asp:ListItem Value="Fail" Text="Fail"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Funding Status</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtfundFundingstatus"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Funding Status Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtfundFundingstatus" runat="server" />
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Nature of expenditure covered</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtFundExpenditureCovered"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Expenditure Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundExpenditureCovered" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                 <div class="selectboxheight">
                                                <p>
                                                    <label>Currency</label>
                                                </p>                                              
                                                  <asp:DropDownList CssClass="input-sm" runat="server" ID="ddlfundCurrency">
                                                        <asp:ListItem Text="USD" Value="USD" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="HKD" Value="HKD"></asp:ListItem>
                                                        <asp:ListItem Text="RMB" Value="RMB"></asp:ListItem>
                                                        <asp:ListItem Text="EUR" Value="EUR"></asp:ListItem>
                                                        <asp:ListItem Text="JPY" Value="JPY"></asp:ListItem>
                                                    </asp:DropDownList>   
                                                     </div>                                         
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Amount Received/ to be received</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtFundAmountReceived"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Amount Received Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundAmountReceived" runat="server" />
                                                <asp:RegularExpressionValidator runat="server" ErrorMessage="Invalid Amount" CssClass="text-danger" Display="Dynamic" ControlToValidate="txtFundAmountReceived" ValidationGroup="fundval" ValidationExpression="^[1-9]\d*(\.\d+)?$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Maximum amount to be received</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtFundMaximumAmountReceived"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Maximum Amount received Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundMaximumAmountReceived" runat="server" />
                                                <asp:RegularExpressionValidator ErrorMessage="Invalid Amount" CssClass="text-danger" Display="Dynamic" runat="server" ControlToValidate="txtFundMaximumAmountReceived" ValidationGroup="fundval" ValidationExpression="^[1-9]\d*(\.\d+)?$"></asp:RegularExpressionValidator>
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Funding Origin</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtFundFundingOrigin"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Funding Origin Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="fundval" ControlToValidate="txtFundFundingOrigin" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Invester information</label>
                                                </p>
                                                <asp:TextBox runat="server" MaxLength="255" CssClass="input-sm" ID="txtFundInvestorInformation"></asp:TextBox>
                                            </div>

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Remarks</label>
                                                </p>
                                                <asp:TextBox runat="server" MaxLength="255" CssClass="input-sm" ID="txtFundRemarks"></asp:TextBox>
                                            </div>

                                        </div>

                                        <div class="">
                                            <asp:Button runat="server" ID="btn_fund_Cancel" CssClass="apply-btn greentheme" OnClick="cancelClosePopup" Text="Cancel" />
                                            <asp:Button runat="server" ID="btn_fund_save" ValidationGroup="fundval" CssClass="apply-btn bluetheme" OnClick="btn_fund_save_Click" Text="Save" />
                                            <asp:Button runat="server" ID="btn_Fund_Remove"  OnClick="open_Delete_confirmation" CommandArgument="3" Visible="false" Text="Remove" CssClass="apply-btn bluetheme" />
                                        </div>
                                        <div id="Div1" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                        <asp:BulletedList ID="FundError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Pnl_MergeAcquisition_PopUp" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2MNA"></h2>
                                    <asp:HiddenField runat="server" ID="hdn_MergeAcquisitionId" />
                                    <div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <p>
                                                    <label>Company</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtmergeCompany"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Company Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="mnaval" ControlToValidate="txtmergeCompany" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5 ">
                                                <div class="selectboxheight">
                                                    <p>
                                                        <label>MNA</label>
                                                    </p>
                                                    <asp:DropDownList CssClass="input-sm" runat="server" ID="ddlMergeMna">
                                                        <asp:ListItem Text="Acquisition" Value="Acquisition" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Merge" Value="Merge"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-5">
                                                <div class="selectboxheight">
                                                    <p>
                                                        <label>Currency</label>
                                                    </p>
                                                    <asp:DropDownList CssClass="input-sm" runat="server" ID="ddlMergeCurrency">
                                                        <asp:ListItem Text="USD" Value="USD" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="HKD" Value="HKD"></asp:ListItem>
                                                        <asp:ListItem Text="RMB" Value="RMB"></asp:ListItem>
                                                        <asp:ListItem Text="EUR" Value="EUR"></asp:ListItem>
                                                        <asp:ListItem Text="JPY" Value="JPY"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Amount</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtmergeAmount"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Amount Required" Display="Dynamic" CssClass="text-danger" ValidationGroup="mnaval" ControlToValidate="txtmergeAmount" runat="server" />
                                                <asp:RegularExpressionValidator Display="Dynamic" runat="server" CssClass="text-danger" ErrorMessage="Invalid Amount" ControlToValidate="txtmergeAmount" ValidationGroup="mnaval" ValidationExpression="^[1-9]\d*(\.\d+)?$"></asp:RegularExpressionValidator>
                                            </div>

                                            <div class="col-md-5">
                                                <p>Valuation</p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtMergeValuation"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Valuation Required" CssClass="text-danger" ValidationGroup="mnaval" ControlToValidate="txtMergeValuation" runat="server" />
                                                <asp:RegularExpressionValidator Display="Dynamic" runat="server" CssClass="text-danger" ErrorMessage="Invalid Valuation" ControlToValidate="txtMergeValuation" ValidationGroup="mnaval" ValidationExpression="^[1-9]\d*(\.\d+)?$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>Date</p>
                                                <asp:TextBox runat="server" CssClass="hidedates datepickerYM input-sm" Width="80%" ID="txtmergeDate"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Date Required" Display="Dynamic" CssClass="text-danger" ValidationGroup="mnaval" ControlToValidate="txtmergeDate" runat="server" />
                                            </div>



                                        </div>
                                        <div class="row">

                                            <asp:Button runat="server" ID="btn_MergeEditCancel" CssClass="apply-btn greentheme" OnClick="cancelClosePopup" Text="Cancel" />
                                            <asp:Button runat="server" ID="btn_Merge_Edit_Save" ValidationGroup="mnaval" CssClass="apply-btn bluetheme" OnClick="btn_Merge_Edit_Save_Click" Text="Save" />
                                            <asp:Button runat="server" ID="btn_MNA_Remove" Visible="false"  OnClick="open_Delete_confirmation" CommandArgument="4" Text="Remove" CssClass="apply-btn bluetheme" />

                                        </div>

                                        <div id="Div3" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                        <asp:BulletedList ID="MnAError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>

                                    </div>
                                </div>
                            </div>

                        </asp:Panel>

                        <asp:Panel runat="server" ID="Pnl_Award_Popup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2award"></h2>
                                    <asp:HiddenField runat="server" ID="hdn_AwardID" />

                                    <div class="row">
                                        <div class="col-md-5">
                                            <p>
                                                <label>Awarded (Year-Month)</label>
                                            </p>
                                            <asp:TextBox runat="server" CssClass="hidedates datepickerYM" ID="txtawardAwarded"></asp:TextBox>
                                        </div>
                                        <div class="col-md-5 ">
                                            <div class="selectboxheight">
                                                <p>
                                                    <label>Type</label>
                                                </p>
                                                <asp:DropDownList CssClass="input-sm" runat="server" ID="ddlAwardType">
                                                    <asp:ListItem Value="Local" Text="Local"></asp:ListItem>
                                                    <asp:ListItem Value="Mainland" Text="Mainland"></asp:ListItem>
                                                    <asp:ListItem Value="Overseas" Text="Overseas"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-5 ">
                                            <div class="selectboxheight">
                                                <p>
                                                    <label>Nature of Awardee</label>
                                                </p>
                                                <asp:DropDownList CssClass="input-sm" runat="server" ID="ddlAwardNatureAwardee">
                                                    <asp:ListItem Value="CCMF" Text="CCMF"></asp:ListItem>
                                                    <asp:ListItem Value="CPIP" Text="CPIP"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-5">

                                            <p>
                                                <label>Award / Recognition</label>
                                            </p>
                                            <asp:TextBox runat="server" CssClass="input-sm" ID="txtAwardRecognition"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <p>
                                                <label>Product (Chi)</label>
                                            </p>
                                            <asp:TextBox runat="server" CssClass="input-sm" ID="txtAwardProductChi"></asp:TextBox>
                                        </div>
                                        <div class="col-md-5">
                                            <p>
                                                <label>Award (Chi)</label>
                                            </p>
                                            <asp:TextBox runat="server" CssClass="input-sm" ID="txtAwardAwardChi"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-5">
                                            <p>
                                                <label>Remarks</label>
                                            </p>
                                            <asp:TextBox runat="server" CssClass="input-sm" ID="txtAwardRemarks"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <asp:Button runat="server" ID="btn_award_cancel" CssClass="apply-btn greentheme" OnClick="cancelClosePopup" Text="Cancel" />
                                        <asp:Button runat="server" ID="btn_Award_Save" CssClass="apply-btn bluetheme" OnClick="btn_Award_Save_Click" Text="Save" />
                                       <asp:Button runat="server"  ID="btn_Award_Remove" OnClick="open_Delete_confirmation" CommandArgument="5"  Visible="false" Text="Remove" CssClass="apply-btn bluetheme" />
                                          </div>
                                    <div id="Div5" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                    <asp:BulletedList ID="AwardError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>

                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnl_IP_Popup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2ip"></h2>
                                    <asp:HiddenField runat="server" ID="hdn_IPID" />
                                    <div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Title</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtIPTitle"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Title Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="IPVal" ControlToValidate="txtIPTitle" runat="server" />
                                            </div>
                                            <div class="col-md-5">
                                                <div class="selectboxheight">
                                                    <p>
                                                        <label>Category</label>
                                                    </p>
                                                    <asp:DropDownList runat="server" ID="ddlIPCategory">
                                                        <asp:ListItem Value="Trademark" Text="Trademark"></asp:ListItem>
                                                        <asp:ListItem Value="Patent" Text="Patent"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Registration Date</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="hidedates datepickerYM input-sm" Width="60%" ID="txtIPRegistrationDate"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Registration Date Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="IPVal" ControlToValidate="txtIPRegistrationDate" runat="server" />
                                            </div>
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Reported Date</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="hidedates  datepickerYM input-sm" Width="60%" ID="txtReportedDate"></asp:TextBox>
                                                <asp:RequiredFieldValidator ErrorMessage="Reported Date Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="IPVal" ControlToValidate="txtReportedDate" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Reference No.</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtIPRefrence"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div>

                                            <asp:Button runat="server" ID="btn_IP_Cancel" CssClass="apply-btn greentheme" OnClick="cancelClosePopup" Text="Cancel" />
                                            <asp:Button runat="server" ID="btn_IP_Save" ValidationGroup="IPVal" CssClass="apply-btn bluetheme" OnClick="btn_IP_Save_Click" Text="Save" />
                                            <asp:Button runat="server" ID="btn_IP_Remove" Visible="false" Text="Remove"  OnClick="open_Delete_confirmation" CommandArgument="6"  CssClass="apply-btn bluetheme" />
                                        </div>
                                        <div id="Div6" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                        <asp:BulletedList ID="IPError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>

                                    </div>
                                </div>
                            </div>

                        </asp:Panel>


                        <asp:Panel runat="server" ID="pnl_JoinedAccelerator_Popup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2joined"></h2>
                                    <asp:HiddenField runat="server" ID="hdnJoinedID" />
                                    <div class="row">
                                        <div>
                                            <p>
                                                <label>Year-Month</label>
                                            </p>
                                            <asp:TextBox runat="server" CssClass="hidedates datepickerYM" ID="txtjoinedYearMonth"></asp:TextBox>
                                            <asp:RequiredFieldValidator ErrorMessage="Accelerator Programme Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="Joinedval" ControlToValidate="txtjoinedYearMonth" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <label>Accelerator Programme</label>
                                            <asp:TextBox runat="server" ID="txtJoinedProgramme" CssClass="input-sm"></asp:TextBox>
                                            <asp:RequiredFieldValidator ErrorMessage="Reported Date Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="Joinedval" ControlToValidate="txtJoinedProgramme" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <label>Remarks</label>
                                            <asp:TextBox runat="server" ID="txtjoinedRemark" MaxLength="255" CssClass="input-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                      <div>
                                <asp:Button runat="server" ID="Button1" CssClass="apply-btn bluetheme" OnClick="cancelClosePopup" Text="Cancel" />
                                <asp:Button runat="server" ID="btn_joinedAccelerator_save" ValidationGroup="IPVal" CssClass="apply-btn greentheme" OnClick="btn_joinedAccelerator_save_Click" Text="Save" />
                                   <asp:Button runat="server" ID="btn_JoinedAccelerator_Remove" Text="Remove" Visible="false"  OnClick="open_Delete_confirmation" CommandArgument="7" CssClass="apply-btn bluetheme" />
                                           </div>
                                <asp:BulletedList ID="joinedError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                                </div>
                              
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="Pnl_CompanyAdministrator_Popup" Visible="false">
                            <div class="popup-overlay"></div>
                            <div class="popup IncubationSubmitPopup">
                                <div class="pos-relative card-theme full-width">
                                    <h2 runat="server" id="h2administrator"></h2>
                                    <asp:HiddenField runat="server" ID="hdn_CompAdminID" />
                                    <div>
                                        <div class="row">
                                            <div class="col-md-5">
                                                <p>
                                                    <label>Administrator Full Name</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtAdminFullName"></asp:TextBox>
                                                <p>
                                                    <asp:RequiredFieldValidator ErrorMessage="Full Name is required" ControlToValidate="txtAdminFullName" ValidationGroup="companyAdminAdd" CssClass="text-danger" Display="Dynamic" runat="server" />
                                                </p>
                                            </div>
                                        </div>

                                        <div class="row">

                                            <div class="col-md-5">
                                                <p>
                                                    <label>Email Address</label>
                                                </p>
                                                <asp:TextBox runat="server" CssClass="input-sm" ID="txtAdminEmail"></asp:TextBox>
                                                <p>
                                                    <asp:RequiredFieldValidator ErrorMessage="Email is required" ControlToValidate="txtAdminEmail" ValidationGroup="companyAdminAdd" CssClass="text-danger" Display="Dynamic" runat="server" />
                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='Invalid Email' CssClass="text-danger" ValidationGroup="companyAdminAdd" ControlToValidate="txtAdminEmail" runat="server" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$" />
                                                </p>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Button runat="server" ID="btn_Admin_Save" ValidationGroup="companyAdminAdd" CssClass="apply-btn bluetheme" OnClick="btn_Admin_Save_Click" Text="Save" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Button runat="server" ID="btn_Admin_cancel" CssClass="apply-btn greentheme" OnClick="cancelClosePopup" Text="Cancel" />
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Button runat="server" ID="btn_Admin_Remove" Visible="false"  OnClick="open_Delete_confirmation" CommandArgument="8"  CssClass="apply-btn bluetheme" Text="Remove"/>
                                            </div>
                                        </div>
                                        <div id="Div7" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                                        <asp:BulletedList ID="adminError" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        
<asp:Panel ID="pnlconfirmation" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important;height: 30%!important">
        <div class="">
            <div class="pop-close">
                <asp:ImageButton PostBackUrl="~/SitePages/Home.aspx" ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton4" OnClick="ImageButton1_Click" />
            </div>
            
            <p class="popup--para" style="margin-top:2px; color: #075CA9; font-size: 2em;text-align:center">
              Are You Sure To Delete?  
            </p>
            <div class="row" style="text-align:center">
              
                <asp:Button runat="server" ID="btn_confirmation_No" Text="No"  CommandArgument="9" OnClick="Delete_confirmation" CssClass="apply-btn greentheme"/>
                <asp:Button runat="server" ID="btn_Confirmation_Yes"  Text="Yes" CommandArgument=""  OnClick="Delete_confirmation" CssClass="apply-btn bluetheme"/>
                

            </div>
       

        </div>
    </div>
</asp:Panel>

                    </div>

                </div>

            </div>

        </div>

    </div>
</div>

<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
<style id='hideMonth'></style>
<script>

    $(".datepickerYM").datepicker({
        dateFormat: "M-yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        showOn: 'both',
        constrainInput: false,
        buttonImageOnly: true,
        buttonImage: '/_layouts/15/images/CBP_Images/Calender.png'
   ,
        beforeShow: function (el, dp) {
            $('#hideMonth').html('.ui-datepicker-calendar{display:none;}');
            var datestr;
            $(".ui-datepicker-month").removeAttr('selected');
            $(".ui-datepicker-year").removeAttr('selected');

            if ((datestr = $(this).val()).length > 0) {

                var year = datestr.substring(datestr.length - 4, datestr.length);
                var month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
                $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                $(this).datepicker('setDate', new Date(year, month, 1));

            }

        },


    }).focus(function () {
        var thisCalendar = $(this);

        $('.ui-datepicker-close').click(function () {
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            thisCalendar.datepicker('setDate', new Date(year, month, 1));
        });
    });

    $(".datepickerDMonthY").datepicker({

        dateFormat: "dd-M-yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        changeDate: true,
        showOn: 'both',
        constrainInput: false,
        buttonImageOnly: true,
        buttonImage: '/_layouts/15/images/CBP_Images/Calender.png'
   ,
        beforeShow: function (el, dp) {

            $('.ui-datepicker-calendar').css("display", "block");
            //$('.ui-datepicker-calendar').show();
            //var datestr;
            //console.log(datestr);
            //console.log($(this).val());
            //console.log(($(this).val()).length);
            //if ((datestr = $(this).val()).length > 0) {
            //    //var date = datestr.substring(0, 2);
            //    //console.log(date);
            //    var year = datestr.substring(datestr.length - 4, datestr.length);
            //    var month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
            //    $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
            //    $(this).datepicker('setDate', new Date(year, month, 1));
            //}
        }
        //onClose: function (dateText, inst) {

        //    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
        //    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
        //    $(this).datepicker('setDate', new Date(year, month, 1));

        //}
    });

    $(".datepickerDMonthY,.datepickerYM").keydown(false);


</script>
