<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CASPApplicationForm.ascx.cs" Inherits="CBP_EMS_SP.CASPApplicationForm.CASPApplicationForm.CASPApplicationForm" %>


<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />
<%--remove while checkin start--%>
<%--<script
    src="https://code.jquery.com/jquery-2.2.4.min.js"
    integrity="sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44="
    crossorigin="anonymous"></script>
<link href="/_layouts/15/Styles/CBP_Styles/customLogin.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/userstyles.css" rel="stylesheet" />
<script src="/_layouts/15/Styles/CBP_Styles/userscripts.js" type="text/javascript"></script>

<script src="/_layouts/15/Styles/CBP_Styles/bootstrap.min.js" type="text/javascript"></script>

<link href="/_layouts/15/Styles/CBP_Styles/bootstrap.min.css" rel="stylesheet" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<script type="text/javascript" src="/_layouts/15/Styles/CBP_Styles/cookies.js"></script>
<style>
    .nav-list {
        /*top:34px;*/
        top: 24px;
    }

        .nav-list .active {
            background-color: #80C343 !important;
        }

    .lang-nav {
        width: 180px;
        display: block;
        background-color: white;
        left: 0 !important;
        height: auto;
    }

        .lang-nav li {
            display: block;
            color: white;
            text-align: center !important;
        }

            .lang-nav li:hover a {
                color: white !important;
            }

    #lang-bar {
        display: none;
    }

    .lang_change:hover #lang-bar {
        display: block !important;
    }

    .modal {
        position: fixed;
        top: 0;
        left: 0;
        background: rgba(0,0,0,0.4);
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }

    .loading {
        /*font-family: Arial;
        font-size: 10pt;*/
        /*border: 5px solid #67CFF5;*/
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        /*background-color: White;*/
        z-index: 999;
        top: 35%;
        left: 50%;
    }

    .line-height {
        line-height: 1.33;
    }

    .imglogo-wrpr {
        padding: 11px 11px;
    }

    .hr {
        border: 5px solid #145DAA;
    }

    *::-ms-backdrop, .line-height {
        line-height: 1.3;
    }
</style>--%>

<%--remove while checkin start--%>
<asp:HiddenField ID="hdn_ProgramID" runat="server" />
<asp:HiddenField ID="hdn_ApplicationID" runat="server" />


<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->

        <div class="custom-form-wd-img border-gray boxcenter width-80 pagewhiteblock">

            <div class="head">
                <img src="/_layouts/15/Images/CBP_Images/Cross Border.png" alt="head-logo" class="head-logo" />
            </div>

            <div class="form __upr">
                <h1 class="form__h1">
                    <%=SPFunctions.LocalizeUI("CASP_ApplicationHeader", "CyberportEMS_CASP") %>
                    <%--  <img src="" class="" />--%>
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/question-mark.png" runat="server" CssClass="question-mark" OnClick="SetPanel1_Click" />
                </h1>
            </div>
            <asp:Panel ID="pnl_InstructionForm" runat="server">

                <h2 class="subheading" style="text-align: left !important; color: #80C343;"><%=SPFunctions.LocalizeUI("intruction_you_are", "CyberportEMS_CASP") %></h2>

                <div>

                    <ol>
                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_1", "CyberportEMS_CASP") %>
                            <ol>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_1", "CyberportEMS_CASP") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_2", "CyberportEMS_CASP") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_3", "CyberportEMS_CASP") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_4", "CyberportEMS_CASP") %>
                                </li>
                            </ol>
                        </li>

                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_2", "CyberportEMS_CASP") %>
                        </li>

                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_3", "CyberportEMS_CASP") %>
                        </li>

                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_4", "CyberportEMS_CASP") %>
                        </li>
                        <%--<li class="eligibility__list" style="padding-top: 10px;">
                            <%=SPFunctions.LocalizeUI("Instruction_4_1", "CyberportEMS_CASP") %>
                        </li>--%>

                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_5", "CyberportEMS_CASP") %>
                        </li>
                    </ol>
                    <%--     <div style="padding-top: 10px;">
                        <p class="bold"><%=SPFunctions.LocalizeUI("Instruction_6", "CyberportEMS_CASP") %></p>
                        <p><%=SPFunctions.LocalizeUI("Instruction_7", "CyberportEMS_CASP") %>.</p>
                    </div>--%>
                </div>


                <div style="margin-top: 50px; text-align: center;">
                    <asp:Button runat="server" ID="btnCASPForm" CssClass="btn-green login-btn" Text="Continue" OnClick="btnCASPForm_Click" />
                </div>

            </asp:Panel>

            <asp:Panel ID="pnl_programDetail" Visible="false" runat="server">
                <div class="form__upr">


                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Applicant", "CyberportEMS_Common") %></div>
                        <div class="col-md-3 word-wrap">
                            <asp:Label ID="lblApplicant" runat="server" Text="" CssClass="word-wrap"></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblApplicationNo" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_LastSubmit", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblLastSubmitted" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-3  boldgraylbl"></div>
                        <div class="col-md-3 boldgraylbl"></div>
                    </div>
                </div>
            </asp:Panel>


            <div style="position: relative; width: 100%">
                <div class="full-line"></div>

                <asp:Panel ID="progressList" Visible="false" runat="server" CssClass="progressbar progressbar_width" Style="margin: 50px 0; overflow: hidden; width: 156%!important">
                    <asp:LinkButton ID="quicklnk_1" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="1" />
                    <asp:LinkButton ID="quicklnk_2" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="2" />
                    <asp:LinkButton ID="quicklnk_3" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="3" />
                    <asp:LinkButton ID="quicklnk_4" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="4" />
                </asp:Panel>
            </div>
            <div class="form">
                <asp:Panel ID="pnl_CASPStep1" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">

                            <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step_1_Profile", "CyberportEMS_CASP") %></h2>

                            <div>


                                <div class="form-group">

                                    <h2 class="subheading text-left">
                                        <span>1.1</span>
                                        <%=SPFunctions.LocalizeUI("Step_1_1", "CyberportEMS_CASP") %></h2>
                                    <div class="row">
                                        <div class="col-md-3 margin1  selectboxheight">
                                            <asp:DropDownList runat="server" ID="ddl_companyProjects" CssClass="ddplist" AutoPostBack="true" OnSelectedIndexChanged="ddl_companyProjects_SelectedIndexChanged" Style="height: 40px;">
                                            </asp:DropDownList>
                                            <%--<asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />--%>
                                        </div>

                                        <div class="col-md-3">
                                            <br />
                                            <label>
                                                <strong><%=SPFunctions.LocalizeUI("Step_1_1_Category","CyberportEMS_CASP") %></strong>
                                                <asp:Label runat="server" Font-Bold="true" ID="lblCategory"></asp:Label>
                                            </label>

                                        </div>


                                        <div class="col-md-4">
                                            <br />
                                            <label>
                                                <strong><%=SPFunctions.LocalizeUI("Step_1_1_Period","CyberportEMS_CASP") %></strong>
                                                <asp:Label runat="server" Font-Bold="true" ID="lblPeriod"></asp:Label>
                                            </label>


                                        </div>


                                    </div>
                                </div>

                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>1.2</span>
                                        <%=SPFunctions.LocalizeUI("Step_1_2", "CyberportEMS_CASP") %>
                                    </h2>

                                    <asp:TextBox ID="txtCompanyRegAdd" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    <asp:Label ID="lblCompanyRegAdd" runat="server" Font-Bold="true" Visible="false" CssClass="form-control"></asp:Label>

                                </div>

                                <div class="form-group">
                                    <h2 class="subheading text-left"><span>1.3</span> <%=SPFunctions.LocalizeUI("Step_1_3", "CyberportEMS_CASP") %></h2>
                                    <small><%=SPFunctions.LocalizeUI("Step_1_3_Abstractsub","CyberportEMS_CASP") %></small>
                                    <asp:TextBox runat="server" ID="txtabstract" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblabstract" Font-Bold="true" Visible="false" CssClass="form-control"></asp:Label>
                                </div>

                                <div class="form-group">
                                    <h2 class="subheading text-left"><span>1.4</span> <%=SPFunctions.LocalizeUI("Step_1_4", "CyberportEMS_CASP") %></h2>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>1.4.1</span>
                                            <%=SPFunctions.LocalizeUI("Step_1_4_1", "CyberportEMS_CASP") %>
                                        </h4>
                                        <asp:TextBox ID="txtownership" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        <asp:Label ID="lblownership" runat="server" Font-Bold="true" Visible="false" CssClass="form-control"></asp:Label>

                                    </div>
                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>1.4.2</span>
                                            <%=SPFunctions.LocalizeUI("Step_1_4_2", "CyberportEMS_CASP") %>
                                        </h4>
                                        <div class="dirbox">
                                            <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" ID="fu_Company_Ownership_2" runat="server" />
                                            <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="1" ID="ImageButton1" ToolTip="Click to Upload" />
                                            <br />
                                            <asp:Repeater runat="server" ID="rptrcompanyownership" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
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
                                            <asp:Label Text=" " ID="lblfilesize_1" runat="server" CssClass="text-gray-client" />
                                            <br />
                                            <asp:Label Text="" ID="lblcompanyownership" runat="server" CssClass="text-danger" />

                                        </div>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>1.5</span>
                                        <%=SPFunctions.LocalizeUI("Step_1_5", "CyberportEMS_CASP")%>
                                    </h2>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>1.5.1</span>
                                            <%=SPFunctions.LocalizeUI("Step_1_5_1", "CyberportEMS_CASP") %> 
                                                                            
                                        </h4>
                                        <asp:GridView ID="grvCoreMember" runat="server"
                                            ShowFooter="True" AutoGenerateColumns="False"
                                            ShowHeader="false"
                                            GridLines="None" Width="100%" OnRowCommand="grvCoreMember_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="Core_Member_ID" runat="server" Value='<%#Eval("Core_Member_ID") %>' />
                                                        <div class="form-box form-group sidemargin" data-id='<%# Container.DataItemIndex+1 %>'>
                                                            <div class="row">

                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_1_5_1_Name", "CyberportEMS_CASP") %> </label>
                                                                    </p>

                                                                    <asp:TextBox runat="server" ID="txtcorename" CssClass="input-sm" Text='<%#Eval("Name") %>'></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblcorename" Visible="false" Font-Bold="true" Text='<%#Eval("Name") %>'></asp:Label>

                                                                    <p>
                                                                        <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Name_Required", "CyberportEMS_CASP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="txtcorename" runat="server" />
                                                                    </p>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_1_5_1_Titles", "CyberportEMS_CASP") %> </label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtcorePosition" Text='<%#Eval("Position") %>' runat="server" CssClass="input-sm" />
                                                                    <asp:Label ID="lblcorePosition" Visible="false" Font-Bold="true" Text='<%#Eval("Position") %>' runat="server" />
                                                                    <p>
                                                                        <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Title_required", "CyberportEMS_CASP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="txtcorePosition" runat="server" />
                                                                    </p>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_5_1_CoreMember", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox CssClass="input-sm" ID="txtCoreMemberProfile" Text='<%#Eval("CoreMember_Profile") %>' runat="server" TextMode="MultiLine" Style="width: 81%" />
                                                                <asp:Label CssClass="label-text" ID="lblCoreMemberProfile" Font-Bold="true" Text='<%# Eval("CoreMember_Profile") %>' runat="server" Visible="false" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Applicationstatus_required", "CyberportEMS_CASP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="txtCoreMemberProfile" runat="server" />
                                                                </p>
                                                            </div>
                                                            <asp:ImageButton ID="btn_CoreRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Panel ID="pnl_CoreMemberAddNew" runat="server" class="form-group">
                                            <asp:ImageButton ID="ButtonAddCoreMembers" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" ValidationGroup="newCoreMemberValidation" OnClick="ButtonAddCoreMembers_Click" />
                                            <h2 class="subheading text-left" style="display: inline; margin: 0;">
                                                <small><%=SPFunctions.LocalizeUI("Step_1_memberadd", "CyberportEMS_CASP") %></small>
                                            </h2>
                                            <asp:Label ID="lblCorememberError" CssClass="text-danger" runat="server" />
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>1.6</span>
                                        <%=SPFunctions.LocalizeUI("Step_1_6", "CyberportEMS_CASP") %>
                                    </h2>

                                    <div class="form-group sidemargin">
                                        <h4><%=SPFunctions.LocalizeUI("Step_1_6_Background", "CyberportEMS_CASP") %> 
                                        </h4>
                                    </div>
                                    <asp:GridView ID="Grd_FundingStatus" runat="server" ShowFooter="false" AutoGenerateColumns="False"
                                        ShowHeader="false"
                                        GridLines="None" Width="100%" OnRowDataBound="Grd_FundingStatus_RowDataBound" OnRowCommand="Grd_FundingStatus_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="FundingID" runat="server" Value='<%#Eval("Funding_ID") %>' />
                                                    <div class="form-box form-group sidemargin" data-id='<%# Container.DataItemIndex+1 %>'>
                                                        <div class="row">

                                                            <div class="col-md-6  selectboxheight">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_Date", "CyberportEMS_CASP") %> </label>
                                                                </p>

                                                                <asp:TextBox ID="txtApplicationDate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>' runat="server" CssClass="input-sm hidedates datepickerYM" />
                                                                <asp:Label ID="lblApplicationDate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>' runat="server" Font-Bold="true" CssClass="label-text" Visible="false" />
                                                                <asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" ID="imgCalender" runat="server" Style="vertical-align: top" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Date_Required", "CyberportEMS_CASP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationDate" runat="server" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6  ">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_Programme", "CyberportEMS_CASP") %> </label>
                                                                </p>


                                                                <asp:TextBox ID="txtNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="input-sm" />
                                                                <asp:Label ID="lblNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" Font-Bold="true" CssClass="label-text" Visible="false" />

                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_programename_required", "CyberportEMS_CASP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtNameofProgram" runat="server" />
                                                                </p>
                                                            </div>

                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6 margin1 selectboxheight">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_ApplicationStatus", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:HiddenField runat="server" ID="hdnApplicationStatus" Value='<%# Bind("Application_Status") %>' />
                                                                <asp:DropDownList runat="server" ID="ddlApplication" Style="height: 40px"></asp:DropDownList>
                                                                <asp:Label ID="lblApplicationStatue" Text='<%# Eval("Application_Status") %>' Font-Bold="true" CssClass="label-text" Visible="false" runat="server" />

                                                            </div>
                                                            <div class="col-md-6 margin1 selectboxheight">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_FundingStatus", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:HiddenField runat="server" ID="hdnFunding" Value='<%# Bind("Funding_Status")%>' />
                                                                <asp:DropDownList runat="server" ID="ddlFunding" Style="height: 40px"></asp:DropDownList>
                                                                <asp:Label ID="lblFundingStatus" Text='<%# Eval("Funding_Status") %>' Font-Bold="true" CssClass="label-text" Visible="false" runat="server" />

                                                            </div>


                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_Expenditure", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' CssClass="input-sm" />
                                                                <asp:Label ID="lblNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' Font-Bold="true" CssClass="label-text" Visible="false" />
                                                            </div>
                                                            <div class="col-md-6 margin1  selectboxheight">

                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_Currency", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:HiddenField ID="hdnCurrency" Value='<%# Bind("Currency") %>' runat="server" />
                                                                <asp:DropDownList ID="Currency" runat="server" Style="height: 40px">
                                                                    <asp:ListItem Text="HKD" Value="HKD"></asp:ListItem>
                                                                    <asp:ListItem Text="USD" Value="USD"></asp:ListItem>
                                                                    <asp:ListItem Text="RMB" Value="RMB"></asp:ListItem>
                                                                    <asp:ListItem Text="EUR" Value="EUR"></asp:ListItem>
                                                                    <asp:ListItem Text="GBP" Value="GBP"></asp:ListItem>

                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblCurrency" Text='<%# Eval("Currency") %>' Font-Bold="true" CssClass="label-text" Visible="false" runat="server" />

                                                            </div>


                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_amount", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' CssClass="input-sm" />
                                                                <asp:Label ID="lblAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' Font-Bold="true" CssClass="label-text" Visible="false" />

                                                                <p>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_funding_amount", "CyberportEMS_CASP") %>' ControlToValidate="txtAmountReceived" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_1_6_maximumAmount", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' CssClass="input-sm" />
                                                                <asp:Label ID="lblApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' Font-Bold="true" CssClass="label-text" Visible="false" />

                                                                <p>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_funding_maxamount", "CyberportEMS_CASP") %>' ControlToValidate="txtApplicationMaximumAmount" ValidationGroup="newfundingValidation" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <asp:ImageButton ID="btn_FundingRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <asp:Panel ID="pnl_FundingAddNew" runat="server" class="form-group sidemargin">
                                        <asp:ImageButton ID="ImageButton2" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" OnClick="btn_FundingAddNew_Click" ValidationGroup="newfundingValidation" />
                                        <h2 class="subheading text-left" style="display: inline; margin: 0;">
                                            <small><%=SPFunctions.LocalizeUI("Step_1_fundingadd", "CyberportEMS_CASP") %></small>
                                        </h2>
                                        <asp:Label CssClass="text-danger" ID="lbl_fundingError" runat="server" />
                                    </asp:Panel>


                                </div>

                                <div class="form-group ">
                                    <h2 class="subheading text-left">
                                        <span>1.7</span>
                                        <%=SPFunctions.LocalizeUI("Step_1_7", "CyberportEMS_CASP")%> 
                                    </h2>
                                    <div class="form-group sidemargin">
                                        <asp:TextBox ID="txtAdditionalInformation" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>

                                        <asp:Label ID="lblAdditionalInformation" Font-Bold="true" runat="server" Visible="false" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnl_CASPStep2" runat="server" Visible="false">
                    <div class="form-group">

                        <div class="box-wrpr">
                            <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step_2", "CyberportEMS_CASP") %></h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.1</span>
                                    <%=SPFunctions.LocalizeUI("Step_2_1", "CyberportEMS_CASP") %>
                                </h2>

                                <asp:TextBox runat="server" ID="txtprogramme" CssClass="input-sm"></asp:TextBox>
                                <asp:Label runat="server" ID="lblprogramme" Font-Bold="true" Visible="false"></asp:Label>

                                <div class="form-group sidemargin">
                                    <div class="form-group">
                                        <asp:RadioButtonList ID="rbtnProgrammeEndorsed" CssClass="rboEndorsed radiocss" runat="server">
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group subheading2">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <p class="form-group"><%=SPFunctions.LocalizeUI("Step_2_CommencementDate", "CyberportEMS_CASP") %></p>
                                            <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lblcommencementdate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtcommencementdate" CssClass="input-sm datepickerDMonthY"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <p class="form-group"><%=SPFunctions.LocalizeUI("Step_2_ProgrammeDuration", "CyberportEMS_CASP") %></p>
                                            <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lblprogrammeduration"> </asp:Label>
                                            <asp:TextBox runat="server" ID="txtprogrammeduration" TextMode="Number" CssClass="input-sm"></asp:TextBox>
                                             <%=SPFunctions.LocalizeUI("Step_2_ProgrammeDurationUnit", "CyberportEMS_CASP") %>
                                       </div>

                                    </div>

                                </div>
                            </div>
                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.2</span>
                                    <%=SPFunctions.LocalizeUI("Step_2_2", "CyberportEMS_CASP") %></h2>

                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" ID="FuAdmissionRecord" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="2" ID="btnAdmissionAttachement" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptradmission" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
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
                                    <asp:Label Text="" ID="lblfiletype2" runat="server" CssClass="text-gray-client" />
                                    <br />
                                    <asp:Label Text=" " ID="lblfilesize_2" runat="server" CssClass="text-gray-client" />
                                    <br />
                                    <asp:Label Text="" ID="lbladmissionrecord" runat="server" CssClass="text-danger" />

                                </div>

                            </div>


                            <div class="form-group progFactsheet">
                                <h2 class="subheading text-left"><span>2.3</span> <%=SPFunctions.LocalizeUI("Step_2_3", "CyberportEMS_CASP") %></h2>



                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.1</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_3_1", "CyberportEMS_CASP") %>

                                    </h4>
                                    <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtbackground"></asp:TextBox>
                                    <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lblbackground"></asp:Label>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.2</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_3_2", "CyberportEMS_CASP") %>
                                    </h4>
                                    <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtoffering"></asp:TextBox>
                                    <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lbloffering"></asp:Label>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.3</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_3_3", "CyberportEMS_CASP") %>
                                    </h4>
                                    <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtfundraising"></asp:TextBox>
                                    <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lblfundraising"></asp:Label>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.4</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_3_4", "CyberportEMS_CASP") %>
                                    </h4>
                                    <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtalumnisize"></asp:TextBox>
                                    <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lblalumnisize"></asp:Label>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.5</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_3_5", "CyberportEMS_CASP") %>
                                    </h4>
                                    <asp:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" ID="txtreputation"></asp:TextBox>

                                    <asp:Label runat="server" Visible="false" Font-Bold="true" ID="lblreputation"></asp:Label>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.6</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_3_6", "CyberportEMS_CASP") %>
                                    </h4>
                                    <asp:TextBox runat="server" CssClass="input-sm" ID="txtwebsite"></asp:TextBox>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblwebsite"></asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_CASPStep3" runat="server" Visible="false">
                    <div class="form-group">

                        <div class="box-wrpr">
                            <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step_3", "CyberportEMS_CASP") %></h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>3.1</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_1", "CyberportEMS_CASP") %></h2>

                                <asp:GridView ID="gv_CONTACT_DETAIL" runat="server"
                                    ShowFooter="True" AutoGenerateColumns="False"
                                    ShowHeader="false"
                                    GridLines="None" Width="100%" OnRowDataBound="gv_CONTACT_DETAIL_RowDataBound" OnRowCommand="gv_CONTACT_DETAIL_RowCommand">


                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="CONTACT_DETAILS_ID" runat="server" Value='<%#Eval("CONTACT_DETAILS_ID") %>' />

                                                <div class="form-group sidemargin">
                                                    <div class="form-box form-group" data-id='<%# Container.DataItemIndex+1 %>'>

                                                        <h2 class="subheading text-left">

                                                            <label data-id='<%# Container.DataItemIndex+1 %>'>
                                                                <%#Container.DataItemIndex == 0 ? SPFunctions.LocalizeUI("Step_3_1_Principal_Applicant", "CyberportEMS_CASP") : "" %></label></h2>
                                                        <div class="row">
                                                            <div class="col-md-3 margin1  selectboxheight">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_1_Salutation", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:HiddenField ID="hdnSalutation" Value='<%# Bind("Salutation") %>' runat="server" />
                                                                <asp:DropDownList ID="Salutation" runat="server" Style="height: 40px">
                                                                    <asp:ListItem Text="Dr" Value="Dr"></asp:ListItem>
                                                                    <asp:ListItem Text="Mr" Value="Mr"></asp:ListItem>
                                                                    <asp:ListItem Text="Ms" Value="Ms"></asp:ListItem>
                                                                    <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>

                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblSalutation" Visible="false" Text='<%# Eval("Salutation") %>' runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_1_lastname", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtContactLast_name" Text='<%#Eval("Last_Name_Eng") %>' runat="server" CssClass="input-sm"></asp:TextBox>
                                                                <asp:Label ID="lblContactLast_name" Text='<%#Eval("Last_Name_Eng") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Lastname_required", "CyberportEMS_CASP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactLast_name" runat="server" />
                                                                </p>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_1_Firstname", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtContactFirst_name" Text='<%#Eval("First_Name_Eng") %>' runat="server" CssClass="input-sm"></asp:TextBox>
                                                                <asp:Label ID="lblContactFirst_name" Text='<%#Eval("First_Name_Eng") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Firstname_required", "CyberportEMS_CASP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactFirst_name" runat="server" />
                                                                </p>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_3_1_Position", "CyberportEMS_CASP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactPostition" runat="server" Text='<%#Eval("Position") %>' CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactPostition" runat="server" Text='<%#Eval("Position") %>' CssClass="label-text" Visible="false"></asp:Label>
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Position_Required", "CyberportEMS_CASP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactPostition" runat="server" />
                                                            </p>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Contact_Home", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtContactNoHome" runat="server" Text='<%#Eval("Contact_No") %>' CssClass="input-sm"></asp:TextBox>
                                                                <asp:Label ID="lblContactNoHome" Text='<%#Eval("Contact_No") %>' runat="server" Visible="false"></asp:Label>
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_ContactNo_Required", "CyberportEMS_CASP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactNoHome" runat="server" />
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_contact_validation", "CyberportEMS_CASP") %>' ControlToValidate="txtContactNoHome" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_1_Email", "CyberportEMS_CASP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtContactEmail" runat="server" Text='<%#Eval("Email") %>' CssClass="input-sm"></asp:TextBox>
                                                                <asp:Label ID="lblContactEmail" Text='<%#Eval("Email") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Email_Required", "CyberportEMS_CASP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactEmail" runat="server" />
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_email_validation", "CyberportEMS_CASP") %>' CssClass="text-danger" ControlToValidate="txtContactEmail" runat="server" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$" />
                                                                </p>
                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_3_1_mailingAdd", "CyberportEMS_CASP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactAddress" TextMode="MultiLine" runat="server" Text='<%#Eval("Mailing_Address") %>' CssClass="form-control"></asp:TextBox>
                                                            <asp:Label ID="lblContactAddress" Text='<%#ProcessMyDataItem(Eval("Mailing_Address")) %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Mailing_Address_Required", "CyberportEMS_CASP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactAddress" runat="server" />
                                                            </p>
                                                        </div>
                                                        <asp:ImageButton ID="btn_ContactRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                    </div>

                                                </div>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                               <asp:Panel ID="pnl_ContactAddNew" runat="server" class="form-group">
                                    <asp:ImageButton ID="btn_ContactsAddNew" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" OnClick="btn_ContactsAddNew_Click" ValidationGroup="newCOntactDetailValidation" />
                                    <h2 class="subheading text-left" style="display: inline; margin: 0;">
                                                <small><%=SPFunctions.LocalizeUI("Step_3_contactadd", "CyberportEMS_CASP") %></small>
                                            </h2>
                                    <asp:Label CssClass="text-danger" ID="lblcontactdetails" runat="server" />
                                </asp:Panel>
                            </div>

                        </div>

                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_CASPStep4" runat="server" Visible="false">




                    <div class="form-group">

                        <div class="box-wrpr">
                            <div class="form-group">
                                <h2 class="subheading text-center"><%=SPFunctions.LocalizeUI("Step_4 ", "CyberportEMS_CASP") %></h2>
                                <div class="form-group green-clr-theme">
                                    <asp:CheckBox ID="chkDeclaration" runat="server" CssClass="listcss" />

                                </div>


                                <ol>
                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration1 ", "CyberportEMS_CASP") %></li>

                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration2 ", "CyberportEMS_CASP") %></li>

                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration3 ", "CyberportEMS_CASP") %></li>

                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration4 ", "CyberportEMS_CASP") %></li>

                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration5 ", "CyberportEMS_CASP") %></li>

                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration6 ", "CyberportEMS_CASP") %></li>

                                    <li class="form-group"><%=SPFunctions.LocalizeUI("Step_4_Declaration7 ", "CyberportEMS_CASP") %></li>
                                </ol>


                            </div>


                            <div class="row">
                                <div class="col-md-6">
                                    <p>
                                        <label><%=SPFunctions.LocalizeUI("Step_4_1", "CyberportEMS_CASP") %></label>
                                    </p>
                                    <asp:TextBox ID="txtPrinciple_Full_Name" runat="server" CssClass="input-sm" />
                                    <asp:Label ID="lblPrinciple_Full_Name" Text='' runat="server" Font-Bold="true" CssClass="label-text" Visible="false" />
                                </div>
                                <div class="col-md-6">
                                    <p>
                                        <label><%=SPFunctions.LocalizeUI("Step_4_2", "CyberportEMS_CASP") %></label>
                                    </p>
                                    <asp:TextBox ID="txtPrinciple_Title" runat="server" CssClass="input-sm" />
                                    <asp:Label ID="lblPrinciple_Title" Text='' runat="server" Font-Bold="true" CssClass="label-text" Visible="false" />
                                </div>

                            </div>


                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnl_Buttons" Visible="false" runat="server">
                    <div style="margin-top: 50px; margin-left: 0" class="btn-box sidemargin">

                        <asp:Button ID="btn_StepPrevious" runat="server" Text="Previous" ValidationGroup="anything" CssClass="apply-btn bluetheme" OnClick="btn_StepPrevious_Click" Style="margin-left: 0" />

                        <asp:Button ID="btn_StepSave" runat="server" Text="Save" CssClass="apply-btn greentheme" OnClick="btn_StepSave_Click" />

                        <asp:Button ID="btn_StepNext" runat="server" Text="Next" ValidationGroup="anything" CssClass="apply-btn skytheme" OnClick="btn_StepNext_Click" />
                        <asp:Button ID="btn_Submit" Visible="false" OnClick="btn_Submit_Click1" runat="server" Text="Submit" CssClass="btnSubmitIncubation apply-btn skytheme" />

                        <asp:Button ID="btn_Back" Visible="false" class="apply-btn btn_historyBack greentheme" Text="Back" runat="server" />

                        <asp:HiddenField ID="hdn_ActiveStep" runat="server" Value="0" />
                    </div>
                </asp:Panel>
                <div id="lbl_success" style="color: green" runat="server"></div>
                <div id="lbl_Exception" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                <label class="" style="margin-top: 3%; right: 12%; position: absolute; font-size: x-small; color: #999999;">Doc Ref:  ENC.SF.040</label>
            </div>

        </div>


    </div>
</div>


<asp:Panel ID="UserSubmitPasswordPopup" runat="server" Visible="false" DefaultButton="btn_submitFinal">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                <%=SPFunctions.LocalizeUI("Submission_popup", "CyberportEMS_CASP") %> (<asp:Label Text="" runat="server" ID="lblSubmissionApplication" />), <%=SPFunctions.LocalizeUI("Submission_popup_password", "CyberportEMS_CASP") %>
            </p>

            <div class="table full-width">
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Email" ID="txtLoginUserName" runat="server" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Password" TextMode="Password" ID="txtLoginPassword" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="bluetheme" />
                    <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Submit" runat="server" CssClass="greentheme" />
                </div>
                <div style="padding: 12px 0;">
                    <p class="text-danger" id="UserCustomerrorLogin" runat="server"></p>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlsubmissionpopup" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton3" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em">
                <asp:Label Text="text" runat="server" ID="lblappsucess" />
                <%-- <%=SPFunctions.LocalizeUI("Application_submitted_confirmation", "CyberportEMS_Common") %>--%>
            </p>



        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlRestricted" runat="server" Visible="false">

    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup" style="width: 35%!important">
        <div class="pos-relative card-theme full-width">
            <div class="pop-close">
                <asp:ImageButton PostBackUrl="~/SitePages/Home.aspx" ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton4" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color: red">
                <%=SPFunctions.LocalizeUI("Application_submitted_Restriction", "CyberportEMS_Common") %>
            </p>



        </div>
    </div>
</asp:Panel>
<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
<style id='hideMonth'></style>

<script>
    //$(".datepickerYM").attr('readonly', 'readonly');

    $(".datepickerYM").datepicker({
        dateFormat: "M-yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,

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




    $(".rboEndorsed input[type='radio']").click(function (e) {
        if ($(this).val() == "false") {

            $(".progFactsheet").show();
        } else
            $(".progFactsheet").hide();
    });

    $(window).load(function () {
        $(".progFactsheet").hide();
        $(".rboEndorsed input[type='radio']").each(function (i, j) {
            if ($(j).prop("checked") == true && $(j).val() == "false") {
                $(".progFactsheet").show();
            }
        })
    })
</script>
