<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncubationFormInstructions.ascx.cs" Inherits="CBP_EMS_SP.IncubationFormsWebPart.IncubationFormInstructions.IncubationFormInstructions" %>
<asp:HiddenField ID="hdn_ProgramID" runat="server" />
<asp:HiddenField ID="hdn_ApplicationID" runat="server" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />

<style>
    .noviewnote {
        display: none;
    }

    .emscontent .main {
        margin-top: 50px;
    }

    .emscontent .aspNetDisabled {
        color: #cfcfcf;
    }

    .word-wrap {
        overflow: hidden;
        word-break: break-all;
    }

    .label-text {
        color: black;
    }


    .dis-label-text {
        font-size: 12px;
        font-family: Arial;
        margin-bottom: 12px;
        color: #6D6E71;
        font-weight: 500;
    }
    /*#ems-userleft .table-row {
        display: none;
    }
        Resources:Step_6_cyberport
    #contentBox {
        margin: 0 auto;
        margin-top: 70px;
    }

    .emscontent .custom-form-wd-img .head {
        top: -5% !important;
    }

    .emscontent .main .form {
        padding-bottom: 0;
    }

    .login-btn {
        width: 400px;
    }

    #ems-userleft {
        position: relative;
        top: 6px;
    }

    .table-eligibility td {
        vertical-align: top;
    }*/
</style>
<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->

        <div class="custom-form-wd-img border-gray boxcenter width-80 pagewhiteblock">

            <div class="head">
                <img src="/_layouts/15/Images/CBP_Images/Programme.png" alt="head-logo" class="head-logo" />
            </div>

            <div class="form __upr">

                <h1 class="form__h1">
                    <%=SPFunctions.LocalizeUI("Incubation", "CyberportEMS_Incubation") %>
                    <%--  <img src="" class="" />--%>
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/question-mark.png" runat="server" CssClass="question-mark" OnClick="SetPanel1_Click" />

                </h1>

            </div>


            <asp:Panel ID="pnl_InstructionForm" runat="server">

                <h2 class="subheading" style="text-align: left !important; color: #80C343;"><%=SPFunctions.LocalizeUI("intruction_you_are", "CyberportEMS_Incubation") %></h2>

                <div>

                    <ol>
                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_1", "CyberportEMS_Incubation") %>
                            <ol>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_1", "CyberportEMS_Incubation") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_2", "CyberportEMS_Incubation") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_3", "CyberportEMS_Incubation") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_4", "CyberportEMS_Incubation") %>
                                </li>
                            </ol>
                        </li>

                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_2", "CyberportEMS_Incubation") %>
                        </li>

                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_3", "CyberportEMS_Incubation") %>
                        </li>

                        <li class="eligibility__list"><%=SPFunctions.LocalizeUI("Instruction_4", "CyberportEMS_Incubation") %>
                        </li>
                        <li class="eligibility__list" style="padding-top: 10px;">
                            <%=SPFunctions.LocalizeUI("Instruction_4_1", "CyberportEMS_Incubation") %>
                        </li>

<%--                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_5", "CyberportEMS_Incubation") %>
                        </li>--%>
                    </ol>
                    <div style="padding-top: 10px;">
                        <p class="bold"><%=SPFunctions.LocalizeUI("Instruction_6", "CyberportEMS_Incubation") %></p>
                        <p><%=SPFunctions.LocalizeUI("Instruction_7", "CyberportEMS_Incubation") %>.</p>
                    </div>

                </div>


                <div style="margin-top: 50px; text-align: center;">
                    <asp:Button runat="server" ID="btnIncubationForm" CssClass="btn-green login-btn" Text="Continue" OnClick="btnIncubationForm_Click" />
                </div>

            </asp:Panel>

            <asp:Panel ID="pnl_programDetail" Visible="false" runat="server">
                <div class="form__upr">
                    <div class="row">
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Intake", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblIntake" runat="server" Text=""></asp:Label>

                        </div>
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Deadline", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblDeadline" runat="server" Text=""></asp:Label>
                        </div>
                    </div>

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
                        <div class="col-md-3 boldgraylbl"></div>
                        <div class="col-md-3 boldgraylbl"></div>
                    </div>
                </div>
            </asp:Panel>


            <div style="position: relative; width: 100%">
                <div class="full-line"></div>
                <%-- <asp:BulletedList ID="progressList" Visible="false" runat="server" class="progressbar" Style="margin: 50px 0; overflow: hidden;">
                    <asp:ListItem Value="1" Text="" />
                    <asp:ListItem Value="2" Text="" />
                    <asp:ListItem Value="3" Text="" />
                    <asp:ListItem Value="4" Text="" />
                    <asp:ListItem Value="5" Text="" />
                </asp:BulletedList>--%>
                <asp:Panel ID="progressList" Visible="false" runat="server" CssClass="progressbar progressbar_width" Style="margin: 50px 0; overflow: hidden; width: 122%!important">
                    <asp:LinkButton ID="quicklnk_1" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="1" />
                    <asp:LinkButton ID="quicklnk_2" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="2" />
                    <asp:LinkButton ID="quicklnk_3" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="3" />
                    <asp:LinkButton ID="quicklnk_4" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="4" />
                    <asp:LinkButton ID="quicklnk_5" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="5" />


                </asp:Panel>
            </div>
            <div class="form">
                <asp:Panel ID="pnl_IncubationStep1" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">

                            <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step_1_Profile", "CyberportEMS_Incubation") %></h2>

                            <div>

                                <table class="table-eligibility" cellpadding="10">
                                    <tr>
                                        <th style="width: 50px"></th>
                                        <th></th>
                                        <th style="width: 50px"><%=SPFunctions.LocalizeUI("lbl_yes", "CyberportEMS_Common") %></th>
                                        <th style="width: 50px"><%=SPFunctions.LocalizeUI("lbl_no", "CyberportEMS_Common") %></th>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.1</td>
                                        <td><%=SPFunctions.LocalizeUI("Step_1_Q1", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q1" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss rdoQuestionNote" data-qid="q1note">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>

                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>

                                            </asp:RadioButtonList>

                                            <%--<input type="radio" name="radiog_dark" id="radio5" class="css-checkbox" checked="checked">--%>
                                        </td>

                                    </tr>
                                    <tr id="q1note" class="noviewnote ">
                                        <td></td>
                                        <td>
                                            <p><%=SPFunctions.LocalizeUI("Question_1_1_Note", "CyberportEMS_Incubation") %></p>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="bluelbl" valign="top">1.2</td>
                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q2", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q2" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss rdoQuestionNote" data-qid="q2note">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                     <tr id="q2note" class="noviewnote ">
                                        <td></td>
                                        <td>
                                            <p><%=SPFunctions.LocalizeUI("Question_1_2_Note", "CyberportEMS_Incubation") %></p>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="bluelbl" valign="top">1.3</td>
                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q3", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q3" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss rdoQuestionNote" data-qid="q3note">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                     <tr id="q3note" class="noviewnote ">
                                        <td></td>
                                        <td>
                                            <p><%=SPFunctions.LocalizeUI("Question_1_3_Note", "CyberportEMS_Incubation") %></p>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="bluelbl">1.4</td>

                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q4", "CyberportEMS_Incubation") %>
                                           
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q4" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss rdoQuestionNote" data-qid="q4note">
                                                <%--OnSelectedIndexChanged="rbtnPanel1Q4_SelectedIndexChanged" AutoPostBack="true"--%>
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                     <tr id="q4note" class="noviewnote ">
                                        <td></td>
                                        <td>
                                            <p><%=SPFunctions.LocalizeUI("Question_1_4_Note", "CyberportEMS_Incubation") %></p>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="bluelbl" valign="top">
                                            <asp:Label ID="lblQ15td" Text="1.5" runat="server"></asp:Label></td>

                                        <td>
                                            <asp:Label ID="lblQ15" Text="" runat="server">
                                                 <%=SPFunctions.LocalizeUI("Step_1_Q5", "CyberportEMS_Incubation") %>
                                            </asp:Label>
                                           

                                        </td>
                                        <td colspan="2" valign="top">
                                            <asp:RadioButtonList ID="rbtnPanel1Q5" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss rdoQuestionNote" data-qid="q5note">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                     <tr id="q5note" class="noviewnote ">
                                        <td></td>
                                        <td>
                                            <p><%=SPFunctions.LocalizeUI("Question_1_5_Note", "CyberportEMS_Incubation") %></p>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="bluelbl" valign="top">1.6</td>

                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q6", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q6" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl" valign="top">1.7</td>

                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q7", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q7" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl" valign="top">1.8</td>

                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q8", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q8" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
<%--                                    <tr>
                                        <td class="bluelbl" valign="top">1.9</td>

                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q8_1", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q8_1" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>--%>

                                     <tr>
                                        <td class="bluelbl"  valign="top">1.9</td>

<%--                                       <tr>
                                        <td class="bluelbl">
                                            <asp:Label ID="lblQ19td" Enabled="false" Text="1.9" runat="server"></asp:Label></td>
--%> 
                                        <td>
<%--                                            <asp:Label ID="lblQ19" Enabled="false" runat="server">--%>
                                               
                                         <%=SPFunctions.LocalizeUI("Step_1_Q9", "CyberportEMS_Incubation") %>
                                            <%--</asp:Label>--%>


                                     
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q9" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

<%--                                    <tr>
                                        <td class="bluelbl" valign="top">1.10</td>

                                        <td>
                                            <%=SPFunctions.LocalizeUI("Step_1_Q10", "CyberportEMS_Incubation") %>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q10" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>--%>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </div>


                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep2" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">
                            <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step2_COMPANY_INFORMATION", "CyberportEMS_Incubation") %></h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.1</span>
                                    <%=SPFunctions.LocalizeUI("Step2_Company_Name", "CyberportEMS_Incubation") %>
                                    <small><%=SPFunctions.LocalizeUI("Step2_Company_NameSub", "CyberportEMS_Incubation") %></small></h2>


                                <div class="row sidemargin">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCompanyNameEnglish" runat="server" CssClass="input w90" placeholder="English"></asp:TextBox>
                                        <asp:Label ID="lblCompanyNameEnglish" runat="server" CssClass="label-text w90" Visible="false"></asp:Label>

                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCompanyNameChinese" runat="server" CssClass="input w90" placeholder="Chinese"></asp:TextBox>
                                        <asp:Label ID="lblCompanyNameChinese" runat="server" CssClass="label-text w90" Visible="false"></asp:Label>

                                    </div>
                                </div>
                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.1 a</span> <%=SPFunctions.LocalizeUI("Step2_Website", "CyberportEMS_Incubation") %>
                                    </h4>
                                    <asp:TextBox runat="server" ID="txtWebsiteName" CssClass="input-sm"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblWebsiteName" CssClass="label-text" Visible="false"></asp:Label>
                                </div>
<%--                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.1 b</span> <%=SPFunctions.LocalizeUI("Step2_2a_ProjectName", "CyberportEMS_Incubation") %>
                                    </h4>
                                    <asp:TextBox runat="server" ID="txtProjectName" CssClass="input-sm"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblProjectName" CssClass="label-text" Visible="false"></asp:Label>
                                </div>--%>

                                <div class="row sidemargin">
                                    <div class="col-md-6">
                                        <h4 class="subheading2">
                                            <span>2.1 b</span> <%=SPFunctions.LocalizeUI("Step2_2b_Establishment", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox runat="server" ID="txtestablishmentyear" CssClass="input-sm hidedates datepickerYM"></asp:TextBox>
                                        <asp:Image ID="imgEstablishmentyear" ImageUrl="/_layouts/15/images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />
                                        <asp:Label runat="server" ID="lblestablishmentyear" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>
                                    <div class="col-md-6">
                                        <h4 class="subheading2">
                                            <span>2.1 c</span> <%=SPFunctions.LocalizeUI("Step2_2c_CountryOFOrigin", "CyberportEMS_Incubation") %>
                                        </h4>

                                        <%-- <asp:TextBox runat="server" ID="txtCountryOrigin" CssClass="input-sm"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblCountryOrigin" CssClass="label-text" Visible="false"></asp:Label>--%>
                                        <div class="selectboxheight">
                                            <asp:DropDownList ID="ddlCountryOrigin" runat="server"></asp:DropDownList>
                                        </div>

                                    </div>
                                </div>

<%--                                <div class="form-group sidemargin">

                                    <h4 class="subheading2">
                                        <span>2.1 e</span> <%=SPFunctions.LocalizeUI("Step2_2d_NewHK", "CyberportEMS_Incubation") %>
                                    </h4>

                                    <div class="form-group" style="margin-bottom: 12px;">

                                        <asp:RadioButtonList ID="rdbNEWHK" runat="server" RepeatDirection="Vertical" CssClass="rdoBusiness_Area radiocss">
                                            <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>

                                </div>--%>

                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>2.2</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Abstract", "CyberportEMS_Incubation") %>
                                        <small><%=SPFunctions.LocalizeUI("Step_2_Abstractsub", "CyberportEMS_Incubation") %></small></h2>


                                    <%--                                       <div class="form-group sidemargin">
                                            <h4 class="subheading2">
                                                <span>2.2 a</span> <%=SPFunctions.LocalizeUI("Step2_2a_ProjectName", "CyberportEMS_Incubation") %>
                                            </h4>
                                            <asp:TextBox runat="server" ID="txtProjectName" CssClass="input-sm"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblProjectName" CssClass="label-text"  Visible="false"></asp:Label>
                                        </div>

                                        <div class="row sidemargin">
                                            <div class="col-md-6">
                                                <h4 class="subheading2">
                                                    <span>2.2 b</span> <%=SPFunctions.LocalizeUI("Step2_2b_Establishment", "CyberportEMS_Incubation") %>
                                                </h4>
                                                <asp:TextBox runat="server" ID="txtestablishmentyear" CssClass="input-sm hidedates datepickerMonthY"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblestablishmentyear" CssClass="label-text"  Visible="false"></asp:Label>
                              
                                            </div>
                                            <div class="col-md-6">
                                                <h4 class="subheading2">
                                                    <span>2.2 c</span> <%=SPFunctions.LocalizeUI("Step2_2c_CountryOFOrigin", "CyberportEMS_Incubation") %>
                                                </h4>
                                                         
                                                <asp:TextBox runat="server" ID="txtCountryOrigin" CssClass="input-sm"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblCountryOrigin" CssClass="label-text"  Visible="false" ></asp:Label>
                                            </div>                
                                        </div>

                                        <div class="form-group sidemargin">
                                                                
                                            <h4 class="subheading2">
                                                <span>2.2 d</span> <%=SPFunctions.LocalizeUI("Step2_2d_NewHK", "CyberportEMS_Incubation") %>
                                            </h4>
                                                      
                                                        
                                            <asp:RadioButtonList ID="rdbNEWHK" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                                <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>--%>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.2.1</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_English", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="txtAbstractEnglish" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        <asp:Label ID="lblAbstractEnglish" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                    </div>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.2.2</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Chinese", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="TxtAbstractChinese" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                        <asp:Label ID="lblAbstractChinese" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>2.3</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Company_Details", "CyberportEMS_Incubation") %>
                                    </h2>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.3.1</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Objectives", "CyberportEMS_Incubation") %> 
                                        </h4>
                                        <asp:TextBox ID="txtObjective" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblObjective" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>

                                    <div class="form-group sidemargin" >
                                        <h4 class="subheading2">
                                            <span>2.3.2</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Background", "CyberportEMS_Incubation") %> 
                                        </h4>

                                        <div class="form-group sidemargin">
                                            <h4 class="subheading2">
                                                <p>2.3.2.1</p>
                                                <%=SPFunctions.LocalizeUI("Step_2_Backgroundsub", "CyberportEMS_Incubation") %>
                                                <h4></h4>
                                                <asp:TextBox ID="txtbackground" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                <asp:Label ID="lblbackground" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                <h4></h4>
                                                <h4></h4>
                                                <h4></h4>
                                            </h4>

                                        </div>

                                        <div class="form-group sidemargin">
                                            <h4 class="subheading2">
                                                <p>2.3.2.2</p>
                                                <%=SPFunctions.LocalizeUI("Step_2_pilot", "CyberportEMS_Incubation") %>
                                                <h4></h4>
                                                <asp:TextBox ID="txtPilot_Work_Done" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                <asp:Label ID="lblPilot_Work_Done" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                <h4></h4>
                                                <h4></h4>
                                                <h4></h4>
                                            </h4>

                                        </div>

                                        <div class="form-group sidemargin">
                                            <h4 class="subheading2">
                                                <p>2.3.2.3</p>
                                                <p><%=SPFunctions.LocalizeUI("Step_4_Funding_Status", "CyberportEMS_Incubation") %> </p>
                                                <%=SPFunctions.LocalizeUI("Step_4_Funding_Statussub", "CyberportEMS_Incubation") %> 
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

                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step3_Date", "CyberportEMS_Incubation") %> </label>
                                                                    </p>

                                                                    <asp:TextBox ID="txtApplicationDate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>' runat="server" CssClass="input-sm hidedates datepickerYM" />
                                                                    <asp:Label ID="lblApplicationDate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>' runat="server" CssClass="label-text" Visible="false" />
                                                                    <asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />
                                                                    <p>
                                                                        <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Date_Required", "CyberportEMS_Incubation") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationDate" runat="server" />
                                                                    </p>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_4_Name_of_Programme", "CyberportEMS_Incubation") %> </label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="input-sm" />
                                                                    <asp:Label ID="lblNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="label-text" Visible="false" />
                                                                    <p>
                                                                        <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_programename_required", "CyberportEMS_Incubation") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtNameofProgram" runat="server" />
                                                                    </p>
                                                                </div>
                                                            </div>

                                                            <%--<div class="form-group">
                                                            <p>
                                                                <label>Name of Programme</label>
                                                            </p>
                                                            <asp:TextBox ID="txtNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="input-sm" />
                                                            <asp:RequiredFieldValidator ErrorMessage="Name of Programme Required" CssClass="text-danger" Display="None" ValidationGroup="newfundingValidation" ControlToValidate="txtNameofProgram" runat="server" />
                                                        </div>--%>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_2_Application_status", "CyberportEMS_Incubation") %></label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtApplicationStatus" Text='<%#Eval("Application_Status") %>' runat="server" CssClass="input-sm" />
                                                                    <asp:Label ID="lblApplicationStatus" Text='<%#Eval("Application_Status") %>' runat="server" CssClass="label-text" Visible="false" />
                                                                    <p>
                                                                        <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Applicationstatus_required", "CyberportEMS_Incubation") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationStatus" runat="server" />
                                                                    </p>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_4_Funding_Status", "CyberportEMS_Incubation") %></label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtFundingStatus" Text='<%#Eval("Funding_Status") %>' runat="server" CssClass="input-sm" />
                                                                    <asp:Label ID="lblFundingStatus" Text='<%#Eval("Funding_Status") %>' runat="server" CssClass="label-text" Visible="false" />
                                                                    <%-- <asp:RequiredFieldValidator ErrorMessage="Funding Status Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtFundingStatus" runat="server" />--%>
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_2_expendiuture_covered", "CyberportEMS_Incubation") %></label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' CssClass="input-sm" />
                                                                    <asp:Label ID="lblNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' CssClass="label-text" Visible="false" />
                                                                    <%-- <asp:RequiredFieldValidator ErrorMessage="Nature of expenditure covered Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtNature" runat="server" />--%>
                                                                </div>
                                                                <div class="col-md-6">

                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_2_Currency", "CyberportEMS_Incubation") %></label>
                                                                    </p>
                                                                    <asp:HiddenField ID="hdnCurrency" Value='<%# Bind("Currency") %>' runat="server" />
                                                                    <asp:DropDownList ID="Currency" runat="server" Style="height: 40px">
                                                                        <asp:ListItem Text="HKD" Value="HKD"></asp:ListItem>
                                                                        <asp:ListItem Text="USD" Value="USD"></asp:ListItem>
                                                                        <asp:ListItem Text="RMB" Value="RMB"></asp:ListItem>
                                                                        <asp:ListItem Text="EUR" Value="EUR"></asp:ListItem>
                                                                        <asp:ListItem Text="GBP" Value="GBP"></asp:ListItem>

                                                                    </asp:DropDownList>
                                                                    <asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_2_amount_received", "CyberportEMS_Incubation") %></label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' CssClass="input-sm" />
                                                                    <asp:Label ID="lblAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' CssClass="label-text" Visible="false" />
                                                                    <%--    <asp:RequiredFieldValidator ErrorMessage="Amount received Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtAmountReceived" runat="server" />--%>
                                                                    <p>
                                                                        <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_funding_amount", "CyberportEMS_Incubation") %>' ControlToValidate="txtAmountReceived" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                                                    </p>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <p>
                                                                        <label><%=SPFunctions.LocalizeUI("Step_2_Maximum_amount", "CyberportEMS_Incubation") %></label>
                                                                    </p>
                                                                    <asp:TextBox ID="txtApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' CssClass="input-sm" />
                                                                    <asp:Label ID="lblApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' CssClass="label-text" Visible="false" />
                                                                    <%--                                                                <asp:RequiredFieldValidator ErrorMessage="Maximum amount Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationMaximumAmount" runat="server" />--%>
                                                                    <p>
                                                                        <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_funding_maxamount", "CyberportEMS_Incubation") %>' ControlToValidate="txtApplicationMaximumAmount" ValidationGroup="newfundingValidation" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                                                    </p>
                                                                </div>
                                                            </div>
                                                            <asp:ImageButton ID="btn_FundingRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                        <div class="form-group sidemargin">
                                            <asp:ImageButton ID="btn_FundingAddNew" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" OnClick="btn_FundingAddNew_Click" ValidationGroup="newfundingValidation" />
                                            <h2 class="subheading text-left" style="display: inline; margin: 0;">
                                                <small><%=SPFunctions.LocalizeUI("Step_2_fundingadd", "CyberportEMS_CCMF") %></small>
                                            </h2>
                                            <asp:Label CssClass="text-danger" ID="lbl_fundingError" runat="server" />
                                        </div>

                                        <div class="form-group sidemargin">
                                            <h4 class="subheading2">
                                                <p>2.3.2.4</p>

                                                <%=SPFunctions.LocalizeUI("Step_2_additionalinfo", "CyberportEMS_Incubation") %>
                                                <h4></h4>
                                                <asp:TextBox ID="txtAdditionalInformation" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                <asp:Label ID="lblAdditionalInformation" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                <h4></h4>
                                                <h4></h4>
                                                <h4></h4>
                                            </h4>

                                        </div>

                                    </div>
                                </div>


                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>2.4</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Implementation", "CyberportEMS_Incubation") %>
                                    </h2>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.1</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_proposed_products", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="txtProposedProducts" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblProposedProducts" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.2</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_target_market", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="txtTargetMarket" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblTargetMarket" runat="server" CssClass="label-text" Visible="false"></asp:Label>


                                    </div>


                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.3</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_competition_analysis", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="txtCompetitionAnalysis" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblCompetitionAnalysis" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>


                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.4</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Revenue_model", "CyberportEMS_Incubation") %>
                                       
                                        </h4>
                                        <asp:TextBox ID="txtRevenueModel" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblRevenueModel" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>


                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.5</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Exit_strategy", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="txtExitStrategy" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblExitStrategy" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.6</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Business_model", "CyberportEMS_Incubation") %>
                                        </h4>

                                        <div class="row">
                                            <div class="col-md-3 bluelbl"><%=SPFunctions.LocalizeUI("Step_2_Milestone", "CyberportEMS_Incubation") %></div>
                                            <div class="col-md-9 bluelbl"><%=SPFunctions.LocalizeUI("Step_2_Details", "CyberportEMS_Incubation") %></div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_2_Month_1", "CyberportEMS_Incubation") %></div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtFirst6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                <asp:Label ID="lblFirst6Months" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_2_Month_2", "CyberportEMS_Incubation") %></div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtSecond6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                <asp:Label ID="lblSecond6Months" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_2_Month_3", "CyberportEMS_Incubation") %></div>
                                            <div class="col-md-9">

                                                <asp:TextBox ID="txtThird6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                <asp:Label ID="lblThird6Months" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_2_Month_4", "CyberportEMS_Incubation") %></div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtForth6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                <asp:Label ID="lblForth6Months" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                            </div>
                                        </div>

                                    </div>


                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.7</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Resubmission", "CyberportEMS_Incubation") %>
                                        </h4>

                                        <div class="form-group" style="margin-bottom: 12px;">
                                            <asp:RadioButtonList ID="rbtnResubmission" runat="server" RepeatDirection="Horizontal" CssClass="radiocss">
                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>

                                        <asp:TextBox ID="txtResubmission_Project_Reference" Visible="false" runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblResubmission_Project_Reference" runat="server" CssClass="label-text" Visible="false"></asp:Label>


                                    </div>


                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.4.8</span>
                                            <%=SPFunctions.LocalizeUI("Step_2_Main_Difference", "CyberportEMS_Incubation") %>
                                        </h4>
                                        <asp:TextBox ID="txtResubmission_Main_Differences" Enabled="false" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblResubmission_Main_Differences" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                    </div>
                                </div>


                                <div class="form-group">
                                    <h2 class="subheading text-left">
                                        <span>2.5</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Classification_of_Company", "CyberportEMS_Incubation") %></h2>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <span>2.5.1</span>
                                            <%=SPFunctions.LocalizeUI("step_2_Company_Type", "CyberportEMS_Incubation") %>
                                        </h4>

                                        <div class="form-group">
                                            <asp:RadioButtonList ID="rbtnCompany_Type" CssClass="rboCompany_Type radiocss" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="txtOther_Company_Type" Style="display: none" runat="server" CssClass="input-sm input-half" placeholder="Please specify" MaxLength="255"></asp:TextBox>
                                            <asp:Label ID="lblOther_Company_Type" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.5.2</span>
                                        <%=SPFunctions.LocalizeUI("Step2_BusinessArea", "CyberportEMS_Incubation") %>
                                   
                                    </h4>
                                    <div class="form-group">
                                        <asp:RadioButtonList ID="rbtnList_Business_Area" CssClass="rdoBusiness_Area radiocss" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                                        </asp:RadioButtonList>


                                        <div class="form-group">
                                            <asp:TextBox ID="txtOther_Bussiness_Area" Style="display: none" runat="server" CssClass="input-sm input-half" placeholder="Please specify" MaxLength="255"></asp:TextBox>
                                            <asp:Label ID="lblOther_Bussiness_Area" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                        </div>
                                    </div>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.5.3</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Position", "CyberportEMS_Incubation") %>
                                    </h4>
                                    <div class="form-group">
                                        <asp:CheckBoxList ID="chkPositioning" runat="server" CssClass="chkPositioning listcss" RepeatDirection="Horizontal" RepeatColumns="3">
                                            <asp:ListItem Value="Content creation" Text=""></asp:ListItem>
                                            <asp:ListItem Value="Production / post-production" Text=""></asp:ListItem>
                                            <asp:ListItem Value="Publishing / distribution / delivery" Text=""></asp:ListItem>
                                            <asp:ListItem Value="Platform / device development" Text=""></asp:ListItem>
                                            <asp:ListItem Value="Management / trading / service" Text=""></asp:ListItem>
                                            <asp:ListItem Value="Others" Text="Others"></asp:ListItem>
                                        </asp:CheckBoxList>

                                    </div>
                                    <div class="form-group">

                                        <asp:TextBox ID="txtManagementOther" Style="display: none; width: 60%!important" runat="server" CssClass="input-sm input-half" placeholder="Please specify your Management / Trading / Service Position" MaxLength="255"></asp:TextBox>
                                        <asp:Label ID="lblDisManagementOther" runat="server" CssClass="dis-label-text" Visible="false" />
                                        <asp:Label ID="lblManagementOther" runat="server" CssClass="label-text" Visible="false" />
                                    </div>
                                    <div class="form-group">

                                        <asp:TextBox ID="txtPositioningOther" Style="display: none; width: 60%!important" runat="server" CssClass="input-sm input-half" placeholder="Please specify your Positioning(Others)" MaxLength="255"></asp:TextBox>
                                        <asp:Label ID="lblDisPositioningOther" runat="server" CssClass="dis-label-text" Visible="false" />
                                        <asp:Label ID="lblPositioningOther" runat="server" CssClass="label-text" Visible="false" />
                                    </div>

                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.5.4</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Other_Attributes", "CyberportEMS_Incubation") %>
                                    </h4>
                                    <asp:TextBox ID="txtOtherAttributes" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    <asp:Label ID="lblOtherAttributes" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.5.5</span>
                                        <%=SPFunctions.LocalizeUI("Step_2_Preferred_Track", "CyberportEMS_Incubation") %>
                                    </h4>

                                    <div class="form-group">
                                        <asp:RadioButtonList ID="rbtnPreferred_Track" runat="server" RepeatDirection="Horizontal" CssClass="radiocss">
                                            <asp:ListItem Text="On-site incubation"></asp:ListItem>
                                            <asp:ListItem Text="Off-site incubation"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <%=SPFunctions.LocalizeUI("Step_2_5_5_Note", "CyberportEMS_Incubation") %>
                                </div>
                            </div>
                        </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep3" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step_3_Company_ownership", "CyberportEMS_Incubation") %></h2>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.1</span>
                                <%=SPFunctions.LocalizeUI("Step_3_Comapny_ownership_sub1", "CyberportEMS_Incubation") %>
                            </h2>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.1.1</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Comapny_ownership_sub2", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtCompany_Ownership_1" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label runat="server" CssClass="label-text" ID="lblCompany_Ownership_1" Visible="false"></asp:Label>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.1.2</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Comapny_ownership_sub2", "CyberportEMS_Incubation") %>
                                </h4>
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" ID="fu_Company_Ownership_2" runat="server" />
                                    <%--<img src="/_layouts/15/Images/CBP_Images/dir.png" alt="" />--%>
                                    <%-- <asp:RegularExpressionValidator ID="rexpfu_Company_Ownership_2" runat="server" ControlToValidate="fu_Company_Ownership_2"
                                        ErrorMessage="Only .gif, .jpg, .png, .tiff and .jpeg"
                                        ValidationExpression="(.*\.([Pp][Dd][Ff])|.*\.([Dd][Oo][Cc][Xx])|.*\.([Dd][Oo][Cc])|.*\.([Gg][Ii][Ff])|.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Xx][Ll][Ss]|.*\.([Xx][Ll][Ss][Xx]|.*\.([Pp][Pp][Tt]|.*\.([Pp][Pp][Tt][Xx])$)"></asp:RegularExpressionValidator>
                                    --%>
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="5" ID="btncompanyownership" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrcompanyownership" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                                </td>
                                                <td>
                                                    <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

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
                                <span>3.2</span>
                                <%=SPFunctions.LocalizeUI("Step_3_Company_Core_Members ", "CyberportEMS_Incubation") %></h2>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.2.1</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Profiles ", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:GridView ID="grvCoreMember" runat="server"
                                    ShowFooter="True" AutoGenerateColumns="False"
                                    ShowHeader="false"
                                    GridLines="None" Width="100%" OnRowCommand="grvCoreMember_RowCommand">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="Core_Member_ID" runat="server" Value='<%#Eval("Core_Member_ID") %>' />

                                                <div class="form-box form-group" data-id='<%# Container.DataItemIndex+1 %>'>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_3_Name ", "CyberportEMS_Incubation") %></label>
                                                            </p>

                                                            <asp:TextBox CssClass="input-sm" Text='<%#Eval("Name") %>' ID="Name" runat="server" />
                                                            <asp:Label CssClass="label-text" Text='<%#Eval("Name") %>' ID="lblName" runat="server" Visible="false" />
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Core_name_required", "CyberportEMS_Incubation") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="Name" runat="server" />
                                                            </p>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_3_Postitions ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox CssClass="input-sm" ID="Title" Text='<%#Eval("Position") %>' runat="server" />
                                                            <asp:Label CssClass="label-text" Text='<%#Eval("Position") %>' ID="lblTitle" runat="server" Visible="false" />
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Core_positions_required", "CyberportEMS_Incubation") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="Title" runat="server" />
                                                            </p>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_3_HKID ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox CssClass="input-sm" ID="HKID" Text='<%#MD5Encryption.DecryptData(Convert.ToString(Eval("HKID"))) %>' runat="server" />
                                                            <asp:Label CssClass="label-text" ID="lblHKID" Text='<%#MD5Encryption.DecryptData(Convert.ToString(Eval("HKID"))) %>' runat="server" Visible="false" />
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Core_hkid_required", "CyberportEMS_Incubation") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="HKID" runat="server" />
                                                            </p>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <p>
                                                            <label><%=SPFunctions.LocalizeUI("Step_3_Academic_and_Professionals ", "CyberportEMS_Incubation") %></label>
                                                        </p>
                                                        <asp:TextBox CssClass="input-sm" ID="txtCoreMembersProfile" TextMode="MultiLine" Text='<%#Eval("CoreMember_Profile") %>' runat="server" />
                                                        <asp:Label CssClass="label-text" ID="lblCoreMembersProfile" Text='<%# ProcessMyDataItem(Eval("CoreMember_Profile")) %>' runat="server" Visible="false" />
                                                    </div>
                                                    <asp:ImageButton ID="btn_CoreRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                </div>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>


                                <div class="form-group">
                                    <asp:ImageButton ID="ButtonAddCoreMembers" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" ValidationGroup="newCoreMemberValidation" OnClick="ButtonAddCoreMembers_Click" />
                                    <h2 class="subheading text-left" style="display: inline; margin: 0;">
                                        <small><%=SPFunctions.LocalizeUI("Step_3_memberadd", "CyberportEMS_CCMF") %></small>
                                    </h2>
                                    <asp:Label ID="lblCorememberError" CssClass="text-danger" runat="server" />
                                </div>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.2.2</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Major_Partner_Profiles ", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtPartner_Profiles" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lblPartner_Profiles" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>3.3</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Expenditure ", "CyberportEMS_Incubation") %></h2>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.1</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Manpower_Distribution ", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Manpower_Distribution" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lblManpower_Distribution" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.2</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Equipment ", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Equipment_Distribution" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lblEquipment_Distribution" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.3</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Other_cost ", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Other_Costs" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lblOther_Costs" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.4</span>
                                    <%=SPFunctions.LocalizeUI("Step_3_Forest_Income ", "CyberportEMS_Incubation") %>
                                </h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Forecast_Income" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lblForecast_Income" runat="server" CssClass="label-text" Visible="false"></asp:Label>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep4" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading"><span>4.</span>  <%=SPFunctions.LocalizeUI("Step_4_CONTACT_DETAILS ", "CyberportEMS_Incubation") %></h2>

                        <div class="form-group">

                            <asp:GridView ID="gv_CONTACT_DETAIL" runat="server"
                                ShowFooter="True" AutoGenerateColumns="False"
                                ShowHeader="false"
                                GridLines="None" Width="100%" OnRowDataBound="gv_CONTACT_DETAIL_RowDataBound" OnRowCommand="gv_CONTACT_DETAIL_RowCommand">


                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="CONTACT_DETAILS_ID" runat="server" Value='<%#Eval("CONTACT_DETAILS_ID") %>' />
                                            <h2 class="subheading text-left">
                                                <span>4.<%#Container.DataItemIndex+1 %></span>
                                                <%=SPFunctions.LocalizeUI("Step_4_Contact_Person ", "CyberportEMS_Incubation") %>
                                                <label data-id='<%# Container.DataItemIndex+1 %>'>
                                                    <%#Container.DataItemIndex == 0 ? SPFunctions.LocalizeUI("Step_4_Principal_Applicant ", "CyberportEMS_Incubation") : "" %></label></h2>
                                            <div class="form-group sidemargin">
                                                <div class="form-box form-group" data-id='<%# Container.DataItemIndex+1 %>'>

                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Salution ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:HiddenField ID="hdnSalutation" Value='<%# Bind("Salutation") %>' runat="server" />
                                                            <asp:DropDownList ID="Salutation" runat="server" Style="height: 40px">
                                                                <asp:ListItem Text="Dr" Value="Dr"></asp:ListItem>
                                                                <asp:ListItem Text="Mr" Value="Mr"></asp:ListItem>
                                                                <asp:ListItem Text="Ms" Value="Ms"></asp:ListItem>
                                                                <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>

                                                            </asp:DropDownList>
                                                            <asp:Image ImageUrl="/_layouts/15/Images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Last_Name ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactLast_name" Text='<%#Eval("Last_Name_Eng") %>' runat="server" CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactLast_name" Text='<%#Eval("Last_Name_Eng") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Lastname_required", "CyberportEMS_Incubation") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactLast_name" runat="server" />
                                                            </p>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_First_Name ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactFirst_name" Text='<%#Eval("First_Name_Eng") %>' runat="server" CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactFirst_name" Text='<%#Eval("First_Name_Eng") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Contact_Firstname_required", "CyberportEMS_Incubation") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactFirst_name" runat="server" />
                                                            </p>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <p>
                                                            <label><%=SPFunctions.LocalizeUI("Step_4_Position ", "CyberportEMS_Incubation") %></label>
                                                        </p>
                                                        <asp:TextBox ID="txtContactPostition" runat="server" Text='<%#Eval("Position") %>' CssClass="input-sm"></asp:TextBox>
                                                        <asp:Label ID="lblContactPostition" runat="server" Text='<%#Eval("Position") %>' CssClass="label-text" Visible="false"></asp:Label>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Contact_Home ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactNoHome" runat="server" Text='<%#Eval("Contact_No_Home") %>' CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactNoHome" Text='<%#Eval("Contact_No_Home") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_contact_validation", "CyberportEMS_Incubation") %>' ControlToValidate="txtContactNoHome" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Contact_Office ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactNoOffice" runat="server" Text='<%#Eval("Contact_No_Office") %>' CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactNoOffice" Text='<%#Eval("Contact_No_Office") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_contact_validation", "CyberportEMS_Incubation") %>' ControlToValidate="txtContactNoOffice" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Contact_No_Mobile ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactNoMobile" runat="server" Text='<%#Eval("Contact_No_Mobile") %>' CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactNoMobile" Text='<%#Eval("Contact_No_Mobile") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_contact_validation", "CyberportEMS_Incubation") %>' ControlToValidate="txtContactNoMobile" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Fax ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactFax" runat="server" Text='<%#Eval("Fax") %>' CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactFax" Text='<%#Eval("Fax") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_fax_validation", "CyberportEMS_Incubation") %>' ControlToValidate="txtContactFax" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Email ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactEmail" runat="server" Text='<%#Eval("Email") %>' CssClass="input-sm"></asp:TextBox>
                                                            <asp:Label ID="lblContactEmail" Text='<%#Eval("Email") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_email_validation", "CyberportEMS_Incubation") %>' CssClass="text-danger" ControlToValidate="txtContactEmail" runat="server" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$" />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_4_Nationality ", "CyberportEMS_Incubation") %></label>
                                                            </p>
                                                            <asp:HiddenField ID="hdnNationality" Value='<%# Bind("Nationality") %>' runat="server" />

                                                            <div class="selectboxheight">
                                                                <asp:DropDownList ID="ddlContactNationality" Width="70%" runat="server"></asp:DropDownList>
                                                            </div>
                                                            <asp:Label ID="lblContactNationality" Text='<%# Eval("Nationality") %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>

                                                            <%--                                                            <asp:TextBox ID="txtContactNationality" runat="server" Text='<%#Eval("Nationality") %>' CssClass="input-sm"></asp:TextBox>--%>
                                                            <br />

                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <p>
                                                            <label><%=SPFunctions.LocalizeUI("Step_4_Mailing_Address ", "CyberportEMS_Incubation") %></label>
                                                        </p>
                                                        <asp:TextBox ID="txtContactAddress" TextMode="MultiLine" runat="server" Text='<%#Eval("Mailing_Address") %>' CssClass="form-control"></asp:TextBox>
                                                        <asp:Label ID="lblContactAddress" Text='<%# ProcessMyDataItem(Eval("Mailing_Address")) %>' runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                                    </div>
                                                    <asp:ImageButton ID="btn_ContactRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                </div>

                                            </div>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="form-group sidemargin">
                                <asp:ImageButton ID="btn_ContactsAddNew" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" OnClick="btn_ContactsAddNew_Click" ValidationGroup="newCOntactDetailValidation" />
                                <asp:Label CssClass="text-danger" ID="lblcontactdetails" runat="server" />
                            </div>



                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep5" Visible="false" runat="server">
                    <div class="box-wrpr" style="width: 700px; margin: 0 auto;">
                        <h2 class="subheading"><%=SPFunctions.LocalizeUI("ATTACHMENT_Title", "CyberportEMS_Incubation") %></h2>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.1"><%=SPFunctions.LocalizeUI("BRCOPY ", "CyberportEMS_Incubation") %></label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fu_BrCopy" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="1" ID="btnbr" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrbrcopy" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/" + Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0, Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                                </td>
                                                <td>
                                                    <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="lbCIReminder" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("CI_Reminder ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="lblfileformat" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="lblfilesize" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="BRCOPY" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.2"><%=SPFunctions.LocalizeUI("CompanyAnnualReturn ", "CyberportEMS_Incubation") %></label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fu_AnnualReturn" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="2" ID="btnannual" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrannual" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                                </td>
                                                <td>
                                                    <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="Label1" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label2" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="AnnualReturn" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.3"><%=SPFunctions.LocalizeUI("VideoClip ", "CyberportEMS_Incubation") %></label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:TextBox CssClass="input-sm input-half1 input-trs" Style="width: 63%;" ID="txtVideoClip" runat="server" placeholder="Please input hyperlink of the video" />
                                    <asp:RegularExpressionValidator CssClass="text-danger" ErrorMessage='<%# SPFunctions.LocalizeUI("Error_videourl_validation ", "CyberportEMS_Incubation") %>' ValidationGroup="vdourl" ValidationExpression="^(http://)?(https://)?(www\.)?[A-Za-z0-9]+\.[a-z]{2,3}" ControlToValidate="txtVideoClip" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.4"><%=SPFunctions.LocalizeUI("PresentationSlide ", "CyberportEMS_Incubation") %></label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuPresentationSlide" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="3" ID="btnpresenattion" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrpresentation" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                                </td>
                                                <td>
                                                    <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="Label3" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type_presntation", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label4" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="PresentationSlide" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.5"><%=SPFunctions.LocalizeUI("OtherAttachment ", "CyberportEMS_Incubation") %></label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuOtherAttachement" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="4" ID="btnotherattach" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrOtherAttachement" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="hypNavigation" runat="server" NavigateUrl='<%#SPContext.GetContext(System.Web.HttpContext.Current).Site.Url +"/"+Eval("Attachment_Path") %>' Target="_blank">
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></asp:HyperLink>
                                                </td>
                                                <td>
                                                    <%--                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ImageUrl="/_layouts/15/images/CBP_Images/remove_red.5-Delete.png" runat="server" />--%>

                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="Label5" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label6" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size ", "CyberportEMS_Incubation") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="OtherAttachement" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
<%--                        <h2 class="subheading text-center">DECLARATION</h2>--%>
                        <h2 class="subheading text-center"><%=SPFunctions.LocalizeUI("Declaration_Title", "CyberportEMS_Incubation") %></h2>
                        <div class="form-group green-clr-theme">
                            <asp:CheckBox ID="chkDeclaration" runat="server" CssClass="listcss" Text="&nbsp;" />
                            <%=SPFunctions.LocalizeUI("Step_5_have_read ", "CyberportEMS_Incubation") %>
                        </div>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration1 ", "CyberportEMS_Incubation") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration2 ", "CyberportEMS_Incubation") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration3 ", "CyberportEMS_Incubation") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration4 ", "CyberportEMS_Incubation") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration5 ", "CyberportEMS_Incubation") %></p>

<%--                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration6 ", "CyberportEMS_Incubation") %></p>--%>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration7 ", "CyberportEMS_Incubation") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration8 ", "CyberportEMS_Incubation") %></p>

<%--                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Declaration9 ", "CyberportEMS_Incubation") %></p>--%>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <p class="form-group lbl"><%=SPFunctions.LocalizeUI("Step_5_Full_Name ", "CyberportEMS_Incubation") %></p>
                            <asp:TextBox CssClass="input-sm" ID="txtName_PrincipalApplicant" runat="server" />
                            <asp:Label CssClass="label-text" ID="lblName_PrincipalApplicant" runat="server" Visible="false" />
                        </div>
                        <div class="col-md-6">
                            <p class="form-group lbl"><%=SPFunctions.LocalizeUI("Step_5_Title_Principal_Applicant ", "CyberportEMS_Incubation") %></p>
                            <asp:TextBox CssClass="input-sm" ID="txtPosition_PrincipalApplicant" runat="server" />
                            <asp:Label CssClass="label-text" ID="lblPosition_PrincipalApplicant" runat="server" Visible="false" />
                        </div>
                    </div>

                    <h2 class="subheading text-center" style="margin: 20px 0;"><%=SPFunctions.LocalizeUI("Step_6_PERSONAL_INFORMATION", "CyberportEMS_Incubation") %> 
                    </h2>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_PERSONAL_INFORMATION_1", "CyberportEMS_Incubation") %> </p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Purpose", "CyberportEMS_Incubation") %> </p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Purpose_1", "CyberportEMS_Incubation") %> </p>

                    <ul class="form-group bult">
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_2", "CyberportEMS_Incubation") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_3", "CyberportEMS_Incubation") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_4", "CyberportEMS_Incubation") %>  </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_5", "CyberportEMS_Incubation") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_6", "CyberportEMS_Incubation") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_7", "CyberportEMS_Incubation") %> </li>
                    </ul>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Purpose_8", "CyberportEMS_Incubation") %></p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Transfer_data", "CyberportEMS_Incubation") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_1", "CyberportEMS_Incubation") %> </p>

                    <ul class="form-group  bult">
                        <li><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_2", "CyberportEMS_Incubation") %></li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_3", "CyberportEMS_Incubation") %></li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_4", "CyberportEMS_Incubation") %> </li>
                    </ul>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_note", "CyberportEMS_Incubation") %> </p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Direct_marketing", "CyberportEMS_Incubation") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Direct_marketing_1", "CyberportEMS_Incubation") %></p>

                    <p class="form-group"><span class="bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_cyberport", "CyberportEMS_Incubation") %></span> <%=SPFunctions.LocalizeUI("Step_6_cyberport_1", "CyberportEMS_Incubation") %> </p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_cyberport_2", "CyberportEMS_Incubation") %> </p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Privacy", "CyberportEMS_Incubation") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Privacy_2", "CyberportEMS_Incubation") %> </p>
                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Access", "CyberportEMS_Incubation") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Access_1", "CyberportEMS_Incubation") %></p>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("STEP_6_STATEMENT", "CyberportEMS_Incubation") %> </p>


                    <div class="green-clr-theme">
                        <asp:CheckBox runat="server" ID="Personal_Information" CssClass="listcss" Text="&nbsp;" />
                        <%=SPFunctions.LocalizeUI("step_6_personal_information_collection", "CyberportEMS_Incubation") %>
                        <br />
                        <asp:CheckBox runat="server" ID="Marketing_Information" CssClass="listcss" Text="&nbsp;" />
                        <%=SPFunctions.LocalizeUI("step_6_marketing", "CyberportEMS_Incubation") %>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_Buttons" Visible="false" runat="server">
                    <div style="margin-top: 50px; margin-left: 0" class="btn-box sidemargin">

                        <asp:Button ID="btn_StepPrevious" runat="server" Text="Previous" ValidationGroup="anything" CssClass="apply-btn bluetheme" OnClick="btn_StepPrevious_Click" Style="margin-left: 0" />

                        <asp:Button ID="btn_StepSave" runat="server" Text="Save" CssClass="apply-btn greentheme" OnClick="btn_StepSave_Click" />

                        <asp:Button ID="btn_StepNext" runat="server" Text="Next" ValidationGroup="anything" CssClass="apply-btn skytheme" OnClick="btn_StepNext_Click" />
                        <asp:Button ID="btn_Submit" Visible="false" OnClick="btn_Submit_Click1" runat="server" Text="Submit" CssClass="btnSubmitIncubation apply-btn skytheme" />

                        <%--  <button id="btn_Back" visible="false" runat="server" class="apply-btn btn_historyBack greentheme" onclick="btn_Back_Click" value="">Back</button>--%>
                        <asp:Button ID="btn_Back" Visible="false" class="apply-btn btn_historyBack greentheme" Text="Back" runat="server" />
                        <%--<script>

                            $(".btn_historyBack").click(function () {

                                window.history.back();
                            })
                        </script>--%>
                        <asp:HiddenField ID="hdn_ActiveStep" runat="server" Value="0" />
                    </div>
                </asp:Panel>
                <div id="lbl_success" style="color: green" runat="server"></div>
                <div id="lbl_Exception" class="text-danger" runat="server" style="margin-top: 12px; margin-left: 44px"></div>
                <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                <label class="" style="margin-top: 3%; right: 12%; position: absolute; font-size: x-small; color: #999999;">Doc Ref:  ENC.SF.040</label>
            </div>
        </div>


        <!---728x90--->
    </div>
    <!---728x90--->
    <!-----//end-main---->
</div>
<asp:Panel ID="IncubationSubmitPopup" runat="server" DefaultButton="btn_submitFinal">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                <%=SPFunctions.LocalizeUI("Submission_popup", "CyberportEMS_Common") %>
                <asp:Label Text="" runat="server" ID="lblintakeno" /><%=SPFunctions.LocalizeUI("Submission_popup_deadline", "CyberportEMS_Common") %>
                <asp:Label ID="lblDeadlinePopup" runat="server" />.
                  <%=SPFunctions.LocalizeUI("Submission_popup_password", "CyberportEMS_Common") %>
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
                <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton1" OnClick="ImageButton1_Click" />
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
                <asp:ImageButton PostBackUrl="~/SitePages/Home.aspx" ImageUrl="/_layouts/15/Images/CBP_Images/close.png" runat="server" ID="ImageButton2" OnClick="ImageButton1_Click" />
            </div>
            <p class="popup--para" style="margin-top: 0; color: #075CA9; font-size: 2em; color: red">
                <%=SPFunctions.LocalizeUI("Application_submitted_Restriction", "CyberportEMS_Common") %>
            </p>



        </div>
    </div>
</asp:Panel>
<link href="/_layouts/15/Styles/CBP_Styles/jquery-ui.css" rel="stylesheet" />
<style>
    .ui-datepicker-calendar {
        display: none;
    }
</style>

<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>

<script>
    //$(".datepickerYM").attr('readonly', 'readonly');

    $(".datepickerYM").datepicker({
        dateFormat: "M-yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        beforeShow: function (el, dp) {

            var datestr;

            if ((datestr = $(this).val()).length > 0) {
                var year = datestr.substring(datestr.length - 4, datestr.length);
                var month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
                $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                $(this).datepicker('setDate', new Date(year, month, 1));

                console.log("this" + $(this).val());

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

    $('[id$="rbtnResubmission_0"]').change(function () {
        if ($('[id$="rbtnResubmission_0"]').prop("checked") == true) {
            $('[id$="txtResubmission_Project_Reference"]').removeClass("aspNetDisabled");
            $('[id$="txtResubmission_Main_Differences"]').removeClass("aspNetDisabled");
        } else {
            $('[id$="txtResubmission_Project_Reference"]').addClass("aspNetDisabled");
            $('[id$="txtResubmission_Main_Differences"]').addClass("aspNetDisabled");
        }
    });

    $(".rdoQuestionNote").click(function () {

        var qId = $(this).attr("data-qid");


        var chkValue = $(this).find("input[type=radio]:checked").val();


        if (chkValue == "False") {
            $("#" + qId).removeClass("noviewnote");
        } else {
            $("#" + qId).addClass("noviewnote");

        }
    });

    if ("<%=rbtnPanel1Q1.SelectedValue%>" == "False") {
        $("#q1note").removeClass("noviewnote");
    }
    if ("<%=rbtnPanel1Q2.SelectedValue%>" == "False") {
        $("#q2note").removeClass("noviewnote");
    }
    if ("<%=rbtnPanel1Q3.SelectedValue%>" == "False") {
        $("#q3note").removeClass("noviewnote");
    }
    if ("<%=rbtnPanel1Q4.SelectedValue%>" == "False") {
        $("#q4note").removeClass("noviewnote");
    }

    if ("<%=rbtnPanel1Q5.SelectedValue%>" == "False") {
        $("#q5note").removeClass("noviewnote");
    }

</script>
