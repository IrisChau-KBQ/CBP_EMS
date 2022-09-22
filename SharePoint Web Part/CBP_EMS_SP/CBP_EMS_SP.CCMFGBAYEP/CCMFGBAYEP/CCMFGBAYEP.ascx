<%--<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>--%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Import Namespace="CBP_EMS_SP.Common" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CCMFGBAYEP.ascx.cs" Inherits="CBP_EMS_SP.CCMFGBAYEP.CCMFGBAYEP.CCMFGBAYEP" %>
<asp:HiddenField ID="hdn_ProgramID" runat="server" />
<asp:HiddenField ID="hdn_ApplicationID" runat="server" />
<link href="/_layouts/15/Styles/CBP_Styles/Muserstyle.css" rel="stylesheet" />
<style>
    .emscontent .main {
        margin-top: 50px;
    }

    .emscontent .tr_width .col-md-1 {
        width: 12px;
    }

    .emscontent .tr_width {
        padding-left: 40px;
    }

    .word-wrap {
        overflow: hidden;
        word-break: break-all;
    }
    .label-text {
        color: black;
        word-wrap: normal;
    }
    #pnl_IncubationStep6 .form-group {
        color: gray;
    }
</style>

<div class="page">
    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->
        <div class="custom-form-wd-img border-gray boxcenter width-80 pagewhiteblock">
             <div class="head">
                <asp:Image ImageUrl="" runat="server" ID="mainpagelogo" />
            </div>
            <div class="form __upr">
                <h1 class="form__h1">
                    <asp:Label Text="text" runat="server" ID="mainpageheading" />
                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/question-mark.png" runat="server" CssClass="question-mark" OnClick="SetPanel1_Click" />
                </h1>
            </div>
            <asp:Panel ID="pnl_InstructionForm" runat="server">
                <h2 class="subheading" style="text-align: left !important; color: #80C343;"><%=SPFunctions.LocalizeUI("Instruction_0", "CyberportEMS_CCMFGBAYEP") %></h2>
                <div>
                    <ol>
                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_1", "CyberportEMS_CCMFGBAYEP") %>
                            <ol>
                                <li>
                                    <%=SPFunctions.LocalizeUI("Instruction_1_1", "CyberportEMS_CCMFGBAYEP") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_2", "CyberportEMS_CCMFGBAYEP") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_3", "CyberportEMS_CCMFGBAYEP") %>
                                </li>
                                <li><%=SPFunctions.LocalizeUI("Instruction_1_4", "CyberportEMS_CCMFGBAYEP") %>
                                </li>
                            </ol>
                        </li>
                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_2", "CyberportEMS_CCMFGBAYEP") %>
                        </li>
                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_3", "CyberportEMS_CCMFGBAYEP") %>
                        </li>

                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_4", "CyberportEMS_CCMFGBAYEP") %>

                           
                        </li>
                        <li class="eligibility__list" style="padding-top: 10px;">
                            <%=SPFunctions.LocalizeUI("Instruction_4_1", "CyberportEMS_CCMFGBAYEP") %>
                        </li>
                        <li class="eligibility__list">
                            <%=SPFunctions.LocalizeUI("Instruction_5", "CyberportEMS_CCMFGBAYEP") %>
                        </li>
                    </ol>
                    <div style="padding-top: 10px;">
                        <p class="form-group txt-gray"><%=SPFunctions.LocalizeUI("Instruction_6", "CyberportEMS_CCMFGBAYEP") %></p>
                        <p class="from-group bold txt-gray"><%=SPFunctions.LocalizeUI("Instruction_7", "CyberportEMS_CCMFGBAYEP") %></p>
                        <p class="from-group bold txt-gray"><%=SPFunctions.LocalizeUI("Instruction_8", "CyberportEMS_CCMFGBAYEP") %></p>
                    </div>

                </div>


                <div style="margin-top: 50px; text-align: center;">
                    <asp:Button runat="server" ID="btnIncubationForm" CssClass="apply-btn skytheme" Text="Continue" OnClick="btnIncubationForm_Click" Style="width: 400px" />
                </div>

            </asp:Panel>

            <asp:Panel ID="pnl_programDetail" Visible="false" runat="server">
                <div class="form__upr">
                    <div class="row">
                        <div class="col-md-2 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Intake", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblIntake" runat="server" Text=""></asp:Label>

                        </div>
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Deadline", "CyberportEMS_Common") %></div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDeadline" runat="server" Text=""></asp:Label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_Applicant", "CyberportEMS_Common") %></div>
                        <div class="col-md-3 word-wrap">
                            <asp:Label ID="lblApplicant" runat="server" Text="" CssClass="word-wrap"></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_AppNum", "CyberportEMS_Common") %></div>
                        <div class="col-md-4">
                            <asp:Label ID="lblApplicationNo" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 boldgraylbl"><%=SPFunctions.LocalizeUI("Application_Summary_Lbl_LastSubmit", "CyberportEMS_Common") %></div>
                        <div class="col-md-3">
                            <asp:Label ID="lblLastSubmitted" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl"></div>
                        <div class="col-md-4 boldgraylbl"></div>
                    </div>
                </div>
            </asp:Panel>
            <div style="position: relative; width: 100%">
                <div class="full-line"></div>
                <asp:Panel ID="progressList" Visible="false" runat="server" CssClass="progressbar progressbar_width" Style="margin: 50px 0; overflow: hidden;">
                    <asp:LinkButton ID="quicklnk_1" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="1" />
                    <asp:LinkButton ID="quicklnk_2" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="2" />
                    <asp:LinkButton ID="quicklnk_3" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="3" />
                    <asp:LinkButton ID="quicklnk_4" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="4" />
                    <asp:LinkButton ID="quicklnk_5" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="5" />
                    <asp:LinkButton ID="quicklnk_6" Text="" runat="server" OnClick="quicklnk_1_Click" CommandArgument="6" />

                </asp:Panel>
            </div>

            <div class="form">
                <asp:Panel ID="pnl_IncubationStep1" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">

                            <h2 class="subheading">
                                <%=SPFunctions.LocalizeUI("Step1_Types_Of_CCMF", "CyberportEMS_CCMFGBAYEP") %></h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>1.1</span>
                                    <%=SPFunctions.LocalizeUI("Step1_CCMF_Selection", "CyberportEMS_CCMFGBAYEP") %></h2>
                                <div class="sidemargin">
                                    <div class="form-group" style="margin-bottom: 10px;">
                                        <asp:RadioButton ID="rdo_HK" runat="server" CssClass="width90 radiocss" GroupName="rdoformopt" Value="HongKong_Programme" />

                                    </div>
                                    <div class="sidemargin">
                                        <asp:RadioButtonList ID="rdo_HK_Option" runat="server" RepeatDirection="Vertical" CssClass="width90 listcss">
                                            <asp:ListItem Value="Professional" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="Young Entrepreneur" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                    <div class="form-group" style="margin-bottom: 10px;">
                                        <asp:RadioButton ID="rdo_Crossborder" runat="server" CssClass="width90 radiocss" GroupName="rdoformopt" Value="HongKong_Programme" />

                                    </div>
                                    <div class="sidemargin">
                                        <asp:RadioButtonList ID="rdo_CrossborderOptions" runat="server" RepeatDirection="Vertical" CssClass="width90 listcss">
                                            <asp:ListItem Value="CrossBorder-Shenzhen" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="CrossBorder-Guangdong" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                    <%-- <div class="form-group" style="margin-top: 10px;">
                                        <asp:RadioButton ID="rdo_Crossborder" runat="server" CssClass="width90 radiocss" Text="" GroupName="rdoformopt" Value="Cross_Border" />
                                    </div>--%>
                                    <div class="form-group" style="margin-top: 10px;">
                                        <asp:RadioButton ID="rdo_CUPP" runat="server" CssClass="width90 radiocss" Text="" GroupName="rdoformopt" Value="CUPP" OnCheckedChanged="rdo_CUPP_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <%-- <asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Vertical" CssClass="width90">
                                        <asp:ListItem Value="True" Text="">Cross-Border Programme(s) Supported by CCMF</asp:ListItem>
                                        <asp:ListItem Value="False" Text="">Cyberport University Partnership Programme Supported by CCMF</asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                    <div class="form-group" style="margin-top: 10px;">
                                        <asp:RadioButton ID="rdo_CCMFGBAYEP" runat="server" CssClass="width90 radiocss" Text="" GroupName="rdoformopt" Value="CCMFGBAYEP" OnCheckedChanged="rdo_CCMFGBAYEP_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>1.2</span>

                                    <%=SPFunctions.LocalizeUI("Step_1_CCMF_app", "CyberportEMS_CCMFGBAYEP") %></h2>
                                <div class="sidemargin">
                                    <asp:RadioButtonList ID="rdo_CCMFApplication" runat="server" RepeatDirection="Vertical" CssClass="width90 listcss">
                                        <asp:ListItem Value="Individual" Text="&nbsp;"></asp:ListItem>
                                        <asp:ListItem Value="Company" Text="&nbsp;"></asp:ListItem>
                                    </asp:RadioButtonList>

                                </div>
                            </div>
                            <div class="sidemargin">
                                <span><%=SPFunctions.LocalizeUI("Step1_Note", "CyberportEMS_CCMFGBAYEP") %></span>

                            </div>
                            <div class="form-group" style="margin-top:10px;">
                                <h2 class="subheading text-left" style="line-height:35px;">
                                    <span>1.3</span>

                                    <%=SPFunctions.LocalizeUI("Step1_Question1_3", "CyberportEMS_CCMFGBAYEP") %></h2>
                                <div class="sidemargin">
                                    <asp:RadioButtonList ID="rdo1_3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdo1_3_SelectedIndexChanged" RepeatDirection="Vertical" CssClass="width90 listcss">
                                        <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                    </asp:RadioButtonList>

                                </div>
                            </div>

                            <div class="form-group" runat="server" id="div1_3Remark" visible="false">
                                <h2 class="subheading text-left"><%=SPFunctions.LocalizeUI("Remarks", "CyberportEMS_CCMFGBAYEP") %></h2>
                                <div class="sidemargin">
                                    <%=SPFunctions.LocalizeUI("Step_1_Remark", "CyberportEMS_CCMFGBAYEP") %>
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep2" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step2_Heading", "CyberportEMS_CCMFGBAYEP") %></h2>
                        <div class="form-group">
                             <h2 class="subheading text-left">
                                <span>2.1</span>
                                <asp:Label ID="hkorcrossborder" runat="server" class="bluelbl" />
                            </h2>
                            <p class="subheading2">
                                <asp:Label ID="heading1" runat="server" class="bluelbl" />
                            </p>
                            <table>
                                <tr>
                                    <td></td>
                                    <td style="width: 90%">
                                        <span><%=SPFunctions.LocalizeUI("lbl_yes", "CyberportEMS_Common") %></span>
                                        <span>&emsp;</span>
                                        <span><%=SPFunctions.LocalizeUI("lbl_no", "CyberportEMS_Common") %></span>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="color-green">2.1.1
                                        <asp:Label ID="Applicant" runat="server" class="color-gray" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr id="div211a" runat="server">
                                    <td class="tr_width">
                                        <span class="col-md-1">a)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl211a" runat="server" class="bluelbl" />
                                        </span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo211a" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss" OnSelectedIndexChanged="rdo211a_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div211b" runat="server">
                                    <td class="tr_width">

                                        <span class="col-md-1">
                                            <asp:Label Text="b)" runat="server" ID="lbl211btdi" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl211b" runat="server" class="bluelbl" />
                                        </span>
                                    </td>
                                    <td id="lbl211btd">
                                        <asp:RadioButtonList ID="rdo211b" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="Tr11c" runat="server" visible="false">

                                    <td class="tr_width">
                                        <span class="col-md-1">
                                            <asp:Label Text="c)" runat="server" ID="lbl211ctdi" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl211c" runat="server" class="bluelbl" />
                                        </span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo211c" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdo211c_SelectedIndexChanged" AutoPostBack="true" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="tr_width">
                                        <asp:Label ID="lbl211cYepInd" runat="server" Visible="false" class="color-gray" />
                                        <asp:Label ID="lbl211cYepComp" runat="server" Visible="false" class="color-gray" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="color-green">2.1.2 
                                        <asp:Label ID="BothApplicants" runat="server" class="color-gray" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr id="div212a" runat="server" visible="false">

                                    <td class="tr_width">
                                        <span runat="server" id="spn212a" class="col-md-1">a)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212a" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212a" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;" />
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212b" runat="server" visible="false">

                                    <td class="tr_width">
                                        <span runat="server" id="spn212b" class="col-md-1">b)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212b" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212b" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;" />
                                            <asp:ListItem Value="False" Text="&nbsp;" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212c" runat="server">

                                    <td class="tr_width">
                                        <span runat="server" id="spn212c" class="col-md-1">c)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212c" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212c" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212d" runat="server">

                                    <td class="tr_width">
                                        <span class="col-md-1">
                                            <asp:Label Text="d)" runat="server" ID="lbl212dtd" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212d" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212d" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212e" runat="server">

                                    <td class="tr_width">
                                        <span class="col-md-1">
                                            <asp:Label Text="e)" runat="server" ID="lbl212etdd" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212e" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212e" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212f" runat="server">

                                    <td class="tr_width">
                                        <span class="col-md-1">
                                            <asp:Label Text="f)" runat="server" ID="lbl212ftd" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212f" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212f" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212g" runat="server">

                                    <td class="tr_width">
                                        <span class="col-md-1">
                                            <asp:Label Text="g)" runat="server" ID="lbl212gtd" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212g" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212g" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212h" runat="server">

                                    <td class="tr_width">
                                        <span runat="server" id="spn212h" class="col-md-1">h)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212h" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212h" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212i" runat="server">

                                    <td class="tr_width">
                                        <span class="col-md-1">
                                            <asp:Label Text="i)" runat="server" ID="lbl212itd" /></span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212i" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212i" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212j" runat="server">

                                    <td class="tr_width">
                                        <span runat="server" id="spn212j" class="col-md-1">j)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212j" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212j" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <%--Extended Question for CCMF 12 dec 2019--%>
                                <tr id="div212f_1" visible="false" runat="server">

                                    <td class="tr_width">
                                        <span runat="server" id="spn212f_1" class="col-md-1">j)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212f_1" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212f_1" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212k" runat="server">
                                    <td class="tr_width">
                                        <span runat="server" id="spn212k" class="col-md-1">k)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212k" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212k" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212l" runat="server">
                                    <td class="tr_width">
                                        <span runat="server" id="spn212l" class="col-md-1">l)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212l" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212l" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212m" runat="server">
                                    <td class="tr_width">
                                        <span runat="server" id="spn212m" class="col-md-1">m)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212m" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212m" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="div212n" runat="server">
                                    <td class="tr_width">
                                        <span runat="server" id="spn212n" class="col-md-1">n)</span>
                                        <span class="col-md-11">
                                            <asp:Label ID="lbl212n" runat="server" class="bluelbl" /></span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdo212n" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss">
                                            <asp:ListItem Value="True" Text="&nbsp;"></asp:ListItem>
                                            <asp:ListItem Value="False" Text="&nbsp;"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="form-group">
                            <div class="sidemargin">
                                <span><%=SPFunctions.LocalizeUI("Step2_Note", "CyberportEMS_CCMFGBAYEP") %></span>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnl_IncubationStep3" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step3_PROJECT_INFORMATION", "CyberportEMS_CCMFGBAYEP") %></h2>


                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.1</span>
                                <%=SPFunctions.LocalizeUI("Step3_Project_Name", "CyberportEMS_CCMFGBAYEP") %></h2>
                            <div class="row sidemargin">
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" CssClass="form-control input-sm-nobackground" ID="English" placeholder="English"></asp:TextBox>
                                    <asp:Label runat="server" CssClass="label-text" ID="lblEnglish" Visible="false" />
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" CssClass="form-control input-sm-nobackground" ID="Chinese" placeholder="Chinese"></asp:TextBox>
                                    <asp:Label runat="server" CssClass="label-text" ID="lblChinese" Visible="false" />
                                </div>
                            </div>

                        </div>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.2</span> <%=SPFunctions.LocalizeUI("Step3_1b_CompanyName", "CyberportEMS_CCMFGBAYEP") %>
                                <small><%=SPFunctions.LocalizeUI("Step3_1b_CompanyNameDetail", "CyberportEMS_CCMFGBAYEP") %></small>
                            </h2>
                            <div class="row sidemargin">
                                <asp:TextBox runat="server" ID="txtCompanyName" CssClass="input-sm"></asp:TextBox>
                                <asp:Label runat="server" ID="lblCompanyName" CssClass="label-text" Visible="false"></asp:Label>
                            </div>

                            <div class="row sidemargin">
                                <div class="col-md-6">
                                    <h4 class="subheading2">
                                        <span>3.2 a</span> <%=SPFunctions.LocalizeUI("Step3_1c_Establishment", "CyberportEMS_CCMFGBAYEP") %>
                                    </h4>
                                    <asp:TextBox runat="server" ID="txtestablishmentyear" CssClass="input-sm hidedates datepickerMonthY"></asp:TextBox>
                                    <asp:Image ID="imgEstablishmentyear" ImageUrl="/_layouts/15/images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />

                                    <asp:Label runat="server" ID="lblestablishmentyear" CssClass="label-text" Visible="false"></asp:Label>

                                </div>
                                <div class="col-md-6">
                                    <h4 class="subheading2">
                                        <span>3.2 b</span> <%=SPFunctions.LocalizeUI("Step3_1d_CountryOFOrigin", "CyberportEMS_CCMFGBAYEP") %>
                                    </h4>

                                    <%--   <asp:TextBox runat="server" ID="txtCountryOrigin" CssClass="input-sm"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblCountryOrigin" CssClass="label-text" Visible="false"></asp:Label>--%>
                                    <div class="selectboxheight">
                                        <asp:DropDownList ID="ddlCountryOrigin" runat="server"></asp:DropDownList>
                                    </div>

                                </div>
                            </div>

                            <div class="form-group sidemargin">

                                <h4 class="subheading2">
                                    <span>3.2 c</span> <%=SPFunctions.LocalizeUI("Step3_1d_NewHK", "CyberportEMS_CCMFGBAYEP") %>
                                </h4>


                                <asp:RadioButtonList ID="rdbNEWHK" runat="server" RepeatDirection="Vertical" CssClass="rdoBusiness_Area radiocss">
                                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <h2 class="subheading text-left">
                                <span>3.3</span>
                                <%=SPFunctions.LocalizeUI("Step3_Abstract", "CyberportEMS_CCMFGBAYEP") %>

                                <small><%=SPFunctions.LocalizeUI("Step3_Abstract_summary", "CyberportEMS_CCMFGBAYEP") %></small></h2>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.1</span>
                                    <%=SPFunctions.LocalizeUI("Step3_English", "CyberportEMS_CCMFGBAYEP") %>
                                </h4>
                                <asp:TextBox runat="server" ID="txtProjectInfoAbsEng" TextMode="MultiLine" CssClass="form-control" />
                                <asp:Label runat="server" ID="lblProjectInfoAbsEng" CssClass="label-text" Visible="false" />

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.2</span>
                                    <%=SPFunctions.LocalizeUI("Step3_Chinese", "CyberportEMS_CCMFGBAYEP") %>
                                </h4>
                                <asp:TextBox runat="server" ID="txtProjectInfoAbschi" TextMode="MultiLine" CssClass="form-control" />
                                <asp:Label runat="server" ID="lblProjectInfoAbschi" CssClass="label-text" Visible="false"></asp:Label>
                            </div>
                        </div>


                        <div class="form-group">
                            <h2 class="subheading text-left" style="margin-bottom: 8px;">
                                <span>3.4</span>
                                <%=SPFunctions.LocalizeUI("Step3_BusinessArea", "CyberportEMS_CCMFGBAYEP") %></h2>
                            <p class="subheading2 sidemargin"><%=SPFunctions.LocalizeUI("Ste3_BusinessAreasub", "CyberportEMS_CCMFGBAYEP") %></p>

                            <div class="form-group sidemargin">
                                <div class="form-group">
                                    <asp:RadioButtonList ID="RadioButtonList1" CssClass="rdoBusiness_Area radiocss" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                                    </asp:RadioButtonList>


                                </div>

                                <div class="form-group">

                                    <asp:TextBox runat="server" CssClass="input-sm input-half" ID="txtOther_Bussiness_Area" placeholder="Please specify" Style="display: none" />
                                    <asp:Label runat="server" CssClass="label-text" ID="lblOther_Bussiness_Area" Visible="false" />

                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.5</span>
                                <%=SPFunctions.LocalizeUI("Step3_AnticipatedDate", "CyberportEMS_CCMFGBAYEP") %></h2>

                            <div class="row">
                                <div class="col-md-6">
                                    <p class="form-group"><%=SPFunctions.LocalizeUI("step3_Commencement", "CyberportEMS_CCMFGBAYEP") %><small class="italic"><%=SPFunctions.LocalizeUI("Step3_day_month_year", "CyberportEMS_CCMFGBAYEP") %></small></p>
                                    <asp:TextBox runat="server" CssClass="input-sm datepickerDMonthY" ID="txtantisdate"></asp:TextBox>
                                    <asp:Label runat="server" CssClass="label-text" ID="lblantisdate" Visible="false" />
                                </div>
                                <div class="col-md-6">
                                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step3_Complete_Date", "CyberportEMS_CCMFGBAYEP") %><small class="italic"><%=SPFunctions.LocalizeUI("Step3_day_month_year", "CyberportEMS_CCMFGBAYEP") %></small></p>
                                    <asp:TextBox runat="server" CssClass="input-sm datepickerDMonthY" ID="txtanticdate"></asp:TextBox>
                                    <asp:Label runat="server" CssClass="label-text" ID="lblanticdate" Visible="false" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.6</span>
                                <%=SPFunctions.LocalizeUI("Step_3_Smart_Space", "CyberportEMS_CCMFGBAYEP") %></h2>
                            <div class="col-md-12">

                                <asp:RadioButtonList runat="server" ID="ddlsmartspace" RepeatDirection="Vertical" CssClass="radiocss"></asp:RadioButtonList>

                                <asp:Label runat="server" CssClass="label-text" ID="lblsmartspace" Visible="false" />
                            </div>

                        </div>
                        <br />
                        <br />
                        <br />
                        <br />
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnl_IncubationStep4" Visible="false" runat="server">
                    <div class="form-group">
                        <div class="box-wrpr">
                            <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step_4_Applicatin_Information", "CyberportEMS_CCMFGBAYEP") %></h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.1(a)</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_project_management", "CyberportEMS_CCMFGBAYEP") %></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2"><%=SPFunctions.LocalizeUI("Step_4_project_managementsub", "CyberportEMS_CCMFGBAYEP") %>
										</h4>
                                    <asp:GridView ID="grvCoreMember" runat="server"
                                        ShowFooter="True" AutoGenerateColumns="False"
                                        ShowHeader="false"
                                        GridLines="None" Width="100%" OnRowCommand="grvCoreMember_RowCommand" OnRowDataBound="grvCoreMember_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="Core_Member_ID" runat="server" Value='<%#Eval("Core_Member_ID") %>' />

                                                    <div class="form-box form-group" data-id='<%# Container.DataItemIndex+1 %>'>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_Name", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>

                                                                <asp:TextBox CssClass="input-sm" Text='<%#Eval("Name") %>' ID="Name" runat="server" />
                                                                <asp:Label CssClass="label-text" Text='<%#Eval("Name") %>' ID="lblName" runat="server" Visible="false" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage="Name Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="Name" runat="server" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_Position", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:TextBox CssClass="input-sm" ID="Title" Text='<%#Eval("Position") %>' runat="server" />
                                                                <asp:Label CssClass="label-text" Text='<%#Eval("Position") %>' ID="lblTitle" runat="server" Visible="false" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage="Positions / Title Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="Title" runat="server" />
                                                                </p>
                                                            </div>
                                                        </div>


                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Email", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>

                                                                <asp:TextBox CssClass="input-sm" Text='<%#Eval("Email") %>' ID="txtEmail" runat="server" />
                                                                <asp:Label CssClass="label-text" Text='<%#Eval("Email") %>' ID="lblEmail" runat="server" Visible="false" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage="Email Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="txtEmail" runat="server" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Nationality", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:HiddenField ID="hdnNationality" Value='<%# Bind("Nationality") %>' runat="server" />

                                                                <div class="selectboxheight">
                                                                    <asp:DropDownList ID="ddlContactNationality" runat="server" Width="70%"></asp:DropDownList>
                                                                </div>
                                                                <asp:Label CssClass="label-text" Text='<%#Eval("Nationality") %>' ID="lblnationality" runat="server" Visible="false" />

                                                                <%--                                                                <asp:TextBox CssClass="input-sm" ID="txtnationality" Text='<%#Eval("Nationality") %>' runat="server" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage="Positions Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="txtnationality" runat="server" />
                                                                </p>--%>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_3_Hkid", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:TextBox CssClass="input-sm" ID="HKID" Text='<%#(MD5Encryption.DecryptData(Convert.ToString(Eval("HKID")))) %>' runat="server" />
                                                                <asp:Label CssClass="label-text" Text='<%#(MD5Encryption.DecryptData(Convert.ToString(Eval("HKID")))) %>' ID="lblHKID" runat="server" Visible="false" />
                                                                <br />
                                                                <asp:Label Text="" ID="Label9" runat="server" CssClass="text-gray-client"><small class="italic"><%=SPFunctions.LocalizeUI("ID_Passport_Format", "CyberportEMS_CCMFGBAYEP") %></small></asp:Label>
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_hkid_required", "CyberportEMS_CCMFGBAYEP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newCoreMemberValidation" ControlToValidate="HKID" runat="server" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6" style="display: none">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_project_management", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:TextBox CssClass="input-sm" ID="Bootcampno" Text='<%#Eval("Bootcamp_Eligible_Number") %>' runat="server" />

                                                            </div>

                                                        </div>

                                                        <div class="form-group">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_3_Background", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox CssClass="input-sm" ID="Backgroundinfo" Text='<%#Eval("Background_Information") %>' runat="server" TextMode="MultiLine" Style="width: 81%" />
                                                            <asp:Label CssClass="label-text" ID="lblBackgroundinfo" Text='<%# ProcessMyDataItem(Eval("Background_Information")) %>' runat="server" Visible="false" />


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
                                            <small><%=SPFunctions.LocalizeUI("Step_4_project_managementadd", "CyberportEMS_CCMFGBAYEP") %></small>
                                        </h2>

                                        <asp:Label ID="lblCorememberError" CssClass="text-danger" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.1(b)</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_Advisor", "CyberportEMS_CCMFGBAYEP") %>
                                </h2>

                                <div class="form-group sidemargin">
                                    <asp:TextBox runat="server" ID="txtAdvisor" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblAdvisor" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.2</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_Business_model", "CyberportEMS_CCMFGBAYEP") %>
                                </h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <%=SPFunctions.LocalizeUI("Step_4_Business_modelsub", "CyberportEMS_CCMFGBAYEP") %>
										</h4>
                                    <asp:TextBox runat="server" ID="txtbusinessmodelteam" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblbusinessmodelteam" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.3</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_creativity", "CyberportEMS_CCMFGBAYEP") %>
                                </h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <%=SPFunctions.LocalizeUI("Step_4_creativitysub", "CyberportEMS_CCMFGBAYEP") %>
										</h4>
                                    <asp:TextBox runat="server" ID="txtcreativity" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblcreativity" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.4</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_socialresponsibility", "CyberportEMS_CCMFGBAYEP") %>
                                </h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <%=SPFunctions.LocalizeUI("Step_4_socialresponsibilitysub", "CyberportEMS_CCMFGBAYEP") %>
										</h4>
                                    <asp:TextBox runat="server" ID="txtsocialrespon" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblsocialrespon" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.5</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_competition", "CyberportEMS_CCMFGBAYEP") %> </h2>

                                <div class="form-group sidemargin">

                                    <asp:TextBox runat="server" ID="txtcompanalysis" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblcompanalysis" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.6</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_Projectmilestone", "CyberportEMS_CCMFGBAYEP") %>

                                    <p class="subheading2"><small><%=SPFunctions.LocalizeUI("Step_4_Projectmilestonesub", "CyberportEMS_CCMFGBAYEP") %> </small></p>
                                </h2>



                                <div class="row">
                                    <div class="col-md-3 greenlbl"><%=SPFunctions.LocalizeUI("Step_4_Milestone", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9 greenlbl"><%=SPFunctions.LocalizeUI("Step_4_Details", "CyberportEMS_CCMFGBAYEP") %> </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_4_Month_1", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth1" TextMode="MultiLine" CssClass="form-control" />
                                        <asp:Label runat="server" ID="lblmonth1" CssClass="label-text" Visible="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_4_Month_2", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth2" TextMode="MultiLine" CssClass="form-control" />
                                        <asp:Label runat="server" ID="lblmonth2" CssClass="label-text" Visible="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_4_Month_3", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth3" TextMode="MultiLine" CssClass="form-control" />
                                        <asp:Label runat="server" ID="lblmonth3" CssClass="label-text" Visible="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_4_Month_4", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth4" TextMode="MultiLine" CssClass="form-control" />
                                        <asp:Label runat="server" ID="lblmonth4" CssClass="label-text" Visible="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_4_Month_5", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth5" TextMode="MultiLine" CssClass="form-control" />
                                        <asp:Label runat="server" ID="lblmonth5" CssClass="label-text" Visible="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl"><%=SPFunctions.LocalizeUI("Step_4_Month_6", "CyberportEMS_CCMFGBAYEP") %> </div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth6" TextMode="MultiLine" CssClass="form-control" />
                                        <asp:Label runat="server" ID="lblmonth6" CssClass="label-text" Visible="false" />
                                    </div>
                                </div>

                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left" style="margin-bottom: 8px;">
                                    <span>4.7</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_Cost_Projections", "CyberportEMS_CCMFGBAYEP") %> 
                                </h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2"><small class="italic"><%=SPFunctions.LocalizeUI("Step_4_Cost_Projectionssub", "CyberportEMS_CCMFGBAYEP") %> </small>
                                    </h4>
                                    <asp:TextBox runat="server" ID="txtcostprojection" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblcostprojection" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.8</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_Funding_Status", "CyberportEMS_CCMFGBAYEP") %> 
                                </h2>
                                <p class="sidemargin subheading2"><small class="italic"><%=SPFunctions.LocalizeUI("Step_4_Funding_Statussub", "CyberportEMS_CCMFGBAYEP") %> </small></p>
                                <div class="form-group sidemargin">
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
                                                                    <label><%=SPFunctions.LocalizeUI("Step3_Date", "CyberportEMS_CCMFGBAYEP") %> </label>
                                                                </p>

                                                                <asp:TextBox ID="txtApplicationDate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>' runat="server" CssClass="input-sm hidedates datepickerMonthY" />
                                                                <asp:Label ID="lblApplicationDate" Text='<%# Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy")!="Jan-0001" ? Convert.ToDateTime( Eval("Date")).ToString("MMM-yyyy") : "" %>' runat="server" CssClass="label-text" Visible="false" />
                                                                <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Calender.png" runat="server" Style="vertical-align: top" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_date_required", "CyberportEMS_CCMFGBAYEP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationDate" runat="server" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Name_of_Programme", "CyberportEMS_CCMFGBAYEP") %> </label>
                                                                </p>
                                                                <asp:TextBox ID="txtNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="input-sm" />
                                                                <asp:Label ID="lblNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="label-text" Visible="false" />

                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_name_required", "CyberportEMS_CCMFGBAYEP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtNameofProgram" runat="server" />
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
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Application_Status", "CyberportEMS_CCMFGBAYEP") %> </label>
                                                                </p>
                                                                <asp:TextBox ID="txtApplicationStatus" Text='<%#Eval("Application_Status") %>' runat="server" CssClass="input-sm" />
                                                                <asp:Label ID="lblApplicationStatus" Text='<%#Eval("Application_Status") %>' runat="server" CssClass="label-text" Visible="false" />
                                                                <p>
                                                                    <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_appstatus_required", "CyberportEMS_CCMFGBAYEP") %>' CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationStatus" runat="server" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Funding_Status", "CyberportEMS_CCMFGBAYEP") %> </label>
                                                                </p>
                                                                <asp:TextBox ID="txtFundingStatus" Text='<%#Eval("Funding_Status") %>' runat="server" CssClass="input-sm" />
                                                                <asp:Label ID="lblFundingStatus" Text='<%#Eval("Funding_Status") %>' runat="server" CssClass="label-text" Visible="false" />
                                                                <%--<asp:RequiredFieldValidator ErrorMessage="Funding Status Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtFundingStatus" runat="server" />--%>
                                                            </div>

                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_expenditure", "CyberportEMS_CCMFGBAYEP") %> </label>
                                                                </p>
                                                                <asp:TextBox ID="txtNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' CssClass="input-sm" />
                                                                <asp:Label ID="lblNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' CssClass="label-text" Visible="false" />
                                                                <%-- <asp:RequiredFieldValidator ErrorMessage="Nature of expenditure covered Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtNature" runat="server" />--%>
                                                            </div>
                                                            <div class="col-md-6">

                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Currency", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:HiddenField ID="hdnCurrency" Value='<%# Eval("Currency") %>' runat="server" />
                                                                <asp:DropDownList ID="Currency" runat="server" Style="height: 40px">
                                                                    <asp:ListItem Text="HKD" Value="HKD"></asp:ListItem>
                                                                    <asp:ListItem Text="USD" Value="USD"></asp:ListItem>
                                                                    <asp:ListItem Text="RMB" Value="RMB"></asp:ListItem>
                                                                    <asp:ListItem Text="EUR" Value="EUR"></asp:ListItem>
                                                                    <asp:ListItem Text="GBP" Value="GBP"></asp:ListItem>

                                                                </asp:DropDownList>
                                                                <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
                                                            </div>

                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Amount_received", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' CssClass="input-sm" />
                                                                <asp:Label ID="lblAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' CssClass="label-text" Visible="false" />

                                                                <%--  <asp:RequiredFieldValidator ErrorMessage="Amount received Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtAmountReceived" runat="server" />--%>

                                                                <br />
                                                                <asp:Label Text="" ID="lblfileformat" runat="server" CssClass="text-gray-client"><small class="italic"><%=SPFunctions.LocalizeUI("Amount_Format", "CyberportEMS_CCMFGBAYEP") %></small></asp:Label>

                                                                <p>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_funding_amount1", "CyberportEMS_CCMFGBAYEP") %>' ControlToValidate="txtAmountReceived" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
                                                                </p>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label><%=SPFunctions.LocalizeUI("Step_4_Maximum_amount", "CyberportEMS_CCMFGBAYEP") %></label>
                                                                </p>
                                                                <asp:TextBox ID="txtApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' CssClass="input-sm" />
                                                                <asp:Label ID="lblApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' CssClass="label-text" Visible="false" />
                                                                <br />
                                                                <asp:Label Text="" ID="Label9" runat="server" CssClass="text-gray-client"><small class="italic"><%=SPFunctions.LocalizeUI("Amount_Format", "CyberportEMS_CCMFGBAYEP") %></small></asp:Label>
                                                                <%-- <asp:RequiredFieldValidator ErrorMessage="Maximum amount Required" CssClass="text-danger" Display="Dynamic" ValidationGroup="newfundingValidation" ControlToValidate="txtApplicationMaximumAmount" runat="server" />--%>
                                                                <p>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_max_amt_number", "CyberportEMS_CCMFGBAYEP") %>' ControlToValidate="txtApplicationMaximumAmount" ValidationGroup="newfundingValidation" CssClass="text-danger" runat="server" ValidationExpression="^(?=.*\d)\d*(?:\.\d\d)?$" />
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
                                            <small><%=SPFunctions.LocalizeUI("Step_4_fundingadd", "CyberportEMS_CCMFGBAYEP") %></small>
                                        </h2>
                                        <asp:Label CssClass="text-danger" ID="lbl_fundingError" runat="server" />
                                    </div>


                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.9</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_Exit_strategy", "CyberportEMS_CCMFGBAYEP") %>
                                </h2>

                                <div class="form-group sidemargin">
                                    <asp:TextBox runat="server" ID="txtExitStrategy" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lblExitStrategy" CssClass="label-text" Visible="false" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.10</span>
                                    <%=SPFunctions.LocalizeUI("Step_4_additional_information", "CyberportEMS_CCMFGBAYEP") %>
                                </h2>

                                <div class="form-group sidemargin">
                                    <asp:TextBox runat="server" ID="txtaddinformation" TextMode="MultiLine" CssClass="form-control" />
                                    <asp:Label runat="server" ID="lbladdinformation" CssClass="label-text" Visible="false" />
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnl_IncubationStep5" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading">
                            <span>5.</span>
                            <%=SPFunctions.LocalizeUI("Step_5_CONTACT_DETAILS", "CyberportEMS_CCMFGBAYEP") %></h2>

                        <div class="form-group">
                            <asp:GridView ID="gv_CONTACT_DETAIL" runat="server" OnRowDataBound="gv_CONTACT_DETAIL_RowDataBound"
                                ShowFooter="True" AutoGenerateColumns="False"
                                ShowHeader="false"
                                GridLines="None" Width="100%" OnRowCommand="gv_CONTACT_DETAIL_RowCommand">


                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="CONTACT_DETAILS_ID" runat="server" Value='<%#Eval("CONTACT_DETAILS_ID") %>' />
                                            <%--<h2 class="subheading text-left"><%=SPFunctions.LocalizeUI("Step_5_Contact_Person", "CyberportEMS_CCMFGBAYEP") %> <%# Container.DataItemIndex+1 %> <%=SPFunctions.LocalizeUI("Step_5_Principal_Applicant", "CyberportEMS_CCMFGBAYEP") %></h2> --%>
                                            <h2 class="subheading text-left">
                                                <span>5.<%# Container.DataItemIndex+1 %></span>
                                                <%=SPFunctions.LocalizeUI("Step_5_Contact_Person", "CyberportEMS_CCMFGBAYEP") %> <%# Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblContactSubTitle" runat="server" Visible="true"><%#Container.DataItemIndex == 0 ? SPFunctions.LocalizeUI("Step_5_Principal_Applicant", "CyberportEMS_CCMFGBAYEP") : "" %></asp:Label>
                                            </h2>
                                            <div class="form-group sidemargin">
                                                <div class="form-box form-group" data-id="<%# Container.DataItemIndex+1 %>">

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <%--<p>
                                                                <asp:Label Text="text" runat="server" Id="lblarea"></asp:label>
                                                            </p>--%>
                                                            <asp:HiddenField ID="hdn_Area" Value='<%# Bind("Area") %>' runat="server" />
                                                            <asp:RadioButtonList ID="rdo_Area" runat="server" RepeatDirection="Horizontal" CssClass="width90 listcss" Visible="false">
                                                                <asp:ListItem Value="HongKong" Text="&nbsp;">Hongkong</asp:ListItem>
                                                                <asp:ListItem Value="China" Text="&nbsp;">China</asp:ListItem>
                                                            </asp:RadioButtonList>

                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Salution", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:HiddenField ID="hdnSalutation" Value='<%# Bind("Salutation") %>' runat="server" />
                                                            <asp:DropDownList ID="Salutation" runat="server" Style="height: 40px">
                                                                <asp:ListItem Text="Dr" Value="Dr"></asp:ListItem>
                                                                <asp:ListItem Text="Mr" Value="Mr"></asp:ListItem>
                                                                <asp:ListItem Text="Ms" Value="Ms"></asp:ListItem>
                                                                <asp:ListItem Text="Miss" Value="Miss"></asp:ListItem>

                                                            </asp:DropDownList>
                                                            <asp:Image ImageUrl="/_layouts/15/images/CBP_Images/Intrnal%20Use-Drop%20Down.png" runat="server" Style="vertical-align: top" />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Last_Name", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactLast_name" runat="server" Text='<%#Eval("Last_Name_Eng") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblContactLast_name" runat="server" Text='<%#Eval("Last_Name_Eng") %>' CssClass="label-text width90per" Visible="false" />

                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_contact_lastname_req", "CyberportEMS_CCMFGBAYEP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactLast_name" runat="server" />
                                                            </p>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_First_Name", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactFirst_name" runat="server" Text='<%#Eval("First_Name_Eng") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblContactFirst_name" runat="server" Text='<%#Eval("First_Name_Eng") %>' CssClass="label-text width90per" Visible="false" />
                                                            <p>
                                                                <asp:RequiredFieldValidator ErrorMessage='<%#SPFunctions.LocalizeUI("Error_contact_firstname_req", "CyberportEMS_CCMFGBAYEP") %>' Display="Dynamic" ValidationGroup="newCOntactDetailValidation" CssClass="text-danger" ControlToValidate="txtContactFirst_name" runat="server" />
                                                            </p>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            &nbsp;
											
                                                        </div>
                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Last_Name_Chi", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtlast_chiname" runat="server" Text='<%#Eval("Last_Name_Chi") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lbllast_chiname" runat="server" Text='<%#Eval("Last_Name_Chi") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>

                                                        <div class="col-md-4">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_First_Name_Chi", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtFisrt_Chiname" runat="server" Text='<%#Eval("First_Name_Chi") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblFisrt_Chiname" runat="server" Text='<%#Eval("First_Name_Chi") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Contact_No", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactNo" runat="server" Text='<%#Eval("Contact_No") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("Contact_No") %>' CssClass="label-text width90per" Visible="false" />
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_Contact_no", "CyberportEMS_CCMFGBAYEP") %>' ControlToValidate="txtContactNo" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Fax", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactFax" runat="server" Text='<%#Eval("Fax") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblContactFax" runat="server" Text='<%#Eval("Fax") %>' CssClass="label-text width90per" Visible="false" />
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_fax", "CyberportEMS_CCMFGBAYEP") %>' ControlToValidate="txtContactFax" CssClass="text-danger" runat="server" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$" />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Email", "CyberportEMS_CCMFGBAYEP") %></label>
                                                            </p>
                                                            <asp:TextBox ID="txtContactEmail" runat="server" Text='<%#Eval("Email") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblContactEmail" runat="server" Text='<%#Eval("Email") %>' CssClass="label-text width90per" Visible="false" />
                                                            <br />
                                                            <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage='<%#SPFunctions.LocalizeUI("Error_email", "CyberportEMS_CCMFGBAYEP") %>' CssClass="text-danger" ControlToValidate="txtContactEmail" runat="server" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$" />
                                                        </div>

                                                    </div>

                                                    <div class="form-group">
                                                        <p>
                                                            <label><%=SPFunctions.LocalizeUI("Step_5_Mailing_Address", "CyberportEMS_CCMFGBAYEP") %></label>
                                                        </p>
                                                        <asp:TextBox ID="txtContactAddress" TextMode="MultiLine" Text='<%#Eval("Mailing_Address") %>' runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:Label ID="lblContactAddress" runat="server" Text='<%# ProcessMyDataItem(Eval("Mailing_Address")) %>' CssClass="label-text" Visible="false" />
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label>
                                                                    <%=SPFunctions.LocalizeUI("Step_5_Institutiuon", "CyberportEMS_CCMFGBAYEP") %>
                                                                    <br />
                                                                    <small><%=SPFunctions.LocalizeUI("Step_5_applicable", "CyberportEMS_CCMFGBAYEP") %></small></label>
                                                            </p>
                                                            <asp:TextBox ID="Nameofinsititute" runat="server" Text='<%#Eval("Education_Institution_Eng") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblNameofinsititute" runat="server" Text='<%#Eval("Education_Institution_Eng") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label>
                                                                    <%=SPFunctions.LocalizeUI("Step_5_Student_idcard", "CyberportEMS_CCMFGBAYEP") %>

                                                                    <br />
                                                                    <small><%=SPFunctions.LocalizeUI("Step_5_applicable", "CyberportEMS_CCMFGBAYEP") %></small>

                                                                </label>
                                                            </p>
                                                            <asp:TextBox ID="Studentidcard" runat="server" Text='<%#Eval("Student_ID_Number") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblStudentidcard" runat="server" Text='<%#Eval("Student_ID_Number") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label>
                                                                    <%=SPFunctions.LocalizeUI("Step_5_Programme_Enrolled_Eng", "CyberportEMS_CCMFGBAYEP") %><br />
                                                                    <small><%=SPFunctions.LocalizeUI("Step_5_applicable", "CyberportEMS_CCMFGBAYEP") %></small></label>
                                                            </p>
                                                            <asp:TextBox ID="Programmerolled" runat="server" Text='<%#Eval("Programme_Enrolled_Eng") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblProgrammerolled" runat="server" Text='<%#Eval("Programme_Enrolled_Eng") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label>
                                                                    <%=SPFunctions.LocalizeUI("Step_5_dateofgrad", "CyberportEMS_CCMFGBAYEP") %>
                                                                    <br />
                                                                    <small><%=SPFunctions.LocalizeUI("Step_5_applicable", "CyberportEMS_CCMFGBAYEP") %></small></label>
                                                            </p>
                                                            <asp:HiddenField ID="hdn_year" Value='<%#Eval("Graduation_Year") %>' runat="server" />
                                                            <asp:HiddenField ID="hdn_month" Value='<%# Eval("Graduation_Month") %>' runat="server" />
                                                            <asp:TextBox ID="Dateofgraduation" runat="server" CssClass="input-sm width90per hidedates datepickerMonthY"></asp:TextBox>
                                                            <asp:Label ID="lblDateofgraduation" runat="server" CssClass="label-text" Visible="false" />
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Org_name", "CyberportEMS_CCMFGBAYEP") %><small><%=SPFunctions.LocalizeUI("Step_5_applicable", "CyberportEMS_CCMFGBAYEP") %></small></label>
                                                            </p>
                                                            <asp:TextBox ID="OrganisationName" runat="server" Text='<%#Eval("Organisation_Name") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblOrganisationName" runat="server" Text='<%#Eval("Organisation_Name") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <p>
                                                                <label><%=SPFunctions.LocalizeUI("Step_5_Position", "CyberportEMS_CCMFGBAYEP") %><small><%=SPFunctions.LocalizeUI("Step_5_applicable", "CyberportEMS_CCMFGBAYEP") %></small></label>
                                                            </p>

                                                            <asp:TextBox ID="Position" runat="server" Text='<%#Eval("Position") %>' CssClass="input-sm width90per"></asp:TextBox>
                                                            <asp:Label ID="lblPosition" runat="server" Text='<%#Eval("Position") %>' CssClass="label-text width90per" Visible="false" />
                                                        </div>
                                                    </div>
                                                    <asp:ImageButton ID="btn_ContactRemoveNew" ImageUrl="/_layouts/15/Images/CBP_Images/delete.png" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%#Container.DataItemIndex %>' />
                                                </div>

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

                            <p class="form-group"><%=SPFunctions.LocalizeUI("Step_5_Notice", "CyberportEMS_CCMFGBAYEP") %> </p>

                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnl_IncubationStep6" Visible="false" runat="server">
                    <div class="box-wrpr" style="width: 700px; margin: 0 auto;">
                        <h2 class="subheading"><%=SPFunctions.LocalizeUI("Step6_Attachment_Header", "CyberportEMS_CCMFGBAYEP") %></h2>


                        <div class="row" runat="server" id="attachbrcopy" visible="false">
                            <label class="col-md-6 lbl" runat="server" id="br_copy">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("BRCOPY", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
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

                                    <asp:Label Text="" ID="lblfileformat" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="lblfilesize" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="BRCOPY" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="attachstudentid" visible="false">
                            <label class="col-md-6 lbl" runat="server" id="studnt_id">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fu_StudentID" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="2" ID="btnstudentid" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrStudentID" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <a>
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></a>
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
                                    <asp:Label Text="" ID="Label1" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label2" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="lblStudentID" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="attachhkid" visible="false">
                            <label class="col-md-6 lbl" runat="server" id="hk_id">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("HKID", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fu_HKID" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="5" ID="btnHKID" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrHKID" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrOtherAttachement_ItemDataBound">
                                        <HeaderTemplate>
                                            <table style="width: 100%;" cellpadding="1">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <a>
                                                        <%#((Convert.ToString(Eval("Attachment_Path")) != "") ? Convert.ToString(Eval("Attachment_Path")).Remove(0,  Convert.ToString(Eval("Attachment_Path")).LastIndexOf("/") + 1) : "") %></a>
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
                                    <asp:Label Text="" ID="Label3" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label4" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="lblHKID" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <%--<div class="row">
                            <label class="col-md-6 lbl" runat="server" id="Company_Document">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("CompanyDocument", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuCompanyDocument" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="6" ID="btnCompanyDocument" ToolTip="Click to Upload" />
                                    <br />
                                    <asp:Repeater runat="server" ID="rptrCompanyDocument" OnItemCommand="Attachments_ItemCommand" OnItemDataBound="rptrCompanyDocument_ItemDataBound">
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
                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="Label11" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label12" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="CompanyDocument" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>--%>

                        <div class="row">
                            <label class="col-md-6 lbl" runat="server" id="video_clip">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:TextBox CssClass="input-sm input-half1 input-trs" Style="width: 63%;" ID="txtVideoClip" runat="server" placeholder="Please input hyperlink of the video" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" runat="server" id="presentation_slide">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuPresentationSlide" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="3" ID="btnPresentation" ToolTip="Click to Upload" />
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
                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="Label5" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type_presntation", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label6" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="PresentationSlide" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" runat="server" id="other">
                                <asp:PlaceHolder runat="server"><%=SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMFGBAYEP") %></asp:PlaceHolder>
                            </label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuOtherAttachement" runat="server" />
                                    <asp:ImageButton ImageUrl="/_layouts/15/Images/CBP_Images/dir.png" runat="server" OnClick="SaveAttachment_Click" CommandName="4" ID="btnOtherAttachmnets" ToolTip="Click to Upload" />
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
                                                    <asp:LinkButton Text="X" ID="lnkAttachmentDelete" CommandName="RemoveAttachment" CommandArgument='<%#Eval("Attachment_ID") %>' ForeColor="Red" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <asp:Label Text="" ID="Label7" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_type", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text=" " ID="Label8" runat="server" CssClass="text-gray-client"><%=SPFunctions.LocalizeUI("File_size", "CyberportEMS_CCMFGBAYEP") %></asp:Label>
                                    <br />
                                    <asp:Label Text="" ID="OtherAttachement" runat="server" CssClass="text-danger" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <h2 class="subheading text-center" style="margin: 20px 0;"><%=SPFunctions.LocalizeUI("Step_6_DECLARATION", "CyberportEMS_CCMFGBAYEP") %></h2>

                        <div class="form-group green-clr-theme">
                            <asp:CheckBox ID="chkDeclaration" runat="server" CssClass="listcss" Text="&nbsp;" />
                            <%=SPFunctions.LocalizeUI("Step_6_Consideration", "CyberportEMS_CCMFGBAYEP") %>
                        </div>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_1", "CyberportEMS_CCMFGBAYEP") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_2", "CyberportEMS_CCMFGBAYEP") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_3", "CyberportEMS_CCMFGBAYEP") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_4", "CyberportEMS_CCMFGBAYEP") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_5", "CyberportEMS_CCMFGBAYEP") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_6", "CyberportEMS_CCMFGBAYEP") %></p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_7", "CyberportEMS_CCMFGBAYEP") %> </p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_8", "CyberportEMS_CCMFGBAYEP") %> </p>

                        <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Consideration_9", "CyberportEMS_CCMFGBAYEP") %> </p>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <p class="form-group lbl"><%=SPFunctions.LocalizeUI("Step_6_Full_Name", "CyberportEMS_CCMFGBAYEP") %></p>
                            <asp:TextBox CssClass="input-sm" ID="txtName_PrincipalApplicant" runat="server" />
                            <asp:Label CssClass="label-text" ID="lblName_PrincipalApplicant" runat="server" Visible="false" />
                        </div>
                        <div class="col-md-6">
                            <p class="form-group lbl"><%=SPFunctions.LocalizeUI("Step_6_Title_Principal_Applicant", "CyberportEMS_CCMFGBAYEP") %></p>
                            <asp:TextBox CssClass="input-sm" ID="txtPosition_PrincipalApplicant" runat="server" ReadOnly="true"/>
                            <asp:Label CssClass="label-text" ID="lblPosition_PrincipalApplicant" runat="server" Visible="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <p class="form-group lbl"><%=SPFunctions.LocalizeUI("Step_6_Full_Name_2", "CyberportEMS_CCMFGBAYEP") %></p>
                            <asp:TextBox CssClass="input-sm" ID="txtName_2ndApplicant" runat="server" ReadOnly="true"/>
                            <asp:Label CssClass="label-text" ID="lblName_2ndApplicant" runat="server" Visible="false" />
                        </div>
                        <div class="col-md-6">
                            <p class="form-group lbl"><%=SPFunctions.LocalizeUI("Step_6_Title_2nd_Applicant", "CyberportEMS_CCMFGBAYEP") %></p>
                            <asp:TextBox CssClass="input-sm" ID="txtPosition_2ndApplicant" runat="server" Enable ="false"/>
                            <asp:Label CssClass="label-text" ID="lblPosition_2ndApplicant" runat="server" Visible ="false" />
                        </div>
                    </div>
                    <div class="row">
                        <asp:Button ID="btn_gotoInsert2ndSign" Visible="true" OnClick="btn_gotoInsert2ndSign_Click" runat="server" Text="Insert GuangDong or Macau Leader Full name and Position Title" CssClass="btnSubmitIncubation apply-btn skytheme" />
                    </div>

                    <h2 class="subheading text-center" style="margin: 20px 0;"><%=SPFunctions.LocalizeUI("Step_6_PERSONAL_INFORMATION", "CyberportEMS_CCMFGBAYEP") %> 
                    </h2>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_PERSONAL_INFORMATION_1", "CyberportEMS_CCMFGBAYEP") %> </p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Purpose", "CyberportEMS_CCMFGBAYEP") %> </p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Purpose_1", "CyberportEMS_CCMFGBAYEP") %> </p>

                    <ul class="form-group bult">
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_2", "CyberportEMS_CCMFGBAYEP") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_3", "CyberportEMS_CCMFGBAYEP") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_4", "CyberportEMS_CCMFGBAYEP") %>  </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_5", "CyberportEMS_CCMFGBAYEP") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_6", "CyberportEMS_CCMFGBAYEP") %> </li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Purpose_7", "CyberportEMS_CCMFGBAYEP") %> </li>
                    </ul>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Purpose_8", "CyberportEMS_CCMFGBAYEP") %></p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Transfer_data", "CyberportEMS_CCMFGBAYEP") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_1", "CyberportEMS_CCMFGBAYEP") %> </p>

                    <ul class="form-group  bult">
                        <li><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_2", "CyberportEMS_CCMFGBAYEP") %></li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_3", "CyberportEMS_CCMFGBAYEP") %></li>
                        <li><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_4", "CyberportEMS_CCMFGBAYEP") %> </li>
                    </ul>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Transfer_data_note", "CyberportEMS_CCMFGBAYEP") %></p>


                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Direct_marketing", "CyberportEMS_CCMFGBAYEP") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Direct_marketing_1", "CyberportEMS_CCMFGBAYEP") %></p>

                    <p class="form-group"><span class="bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_cyberport", "CyberportEMS_CCMFGBAYEP") %></span> <%=SPFunctions.LocalizeUI("Step_6_cyberport_1", "CyberportEMS_CCMFGBAYEP") %> </p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_cyberport_2", "CyberportEMS_CCMFGBAYEP") %> </p>

                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Privacy", "CyberportEMS_CCMFGBAYEP") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Privacy_2", "CyberportEMS_CCMFGBAYEP") %> </p>
                    <p class="form-group bold graylbl"><%=SPFunctions.LocalizeUI("Step_6_Access", "CyberportEMS_CCMFGBAYEP") %></p>
                    <p class="form-group"><%=SPFunctions.LocalizeUI("Step_6_Access_1", "CyberportEMS_CCMFGBAYEP") %></p>

                    <p class="form-group"><%=SPFunctions.LocalizeUI("STEP_6_STATEMENT", "CyberportEMS_CCMFGBAYEP") %> </p>


                    <div class="green-clr-theme">
                        <asp:CheckBox runat="server" ID="Personal_Information" CssClass="listcss" Text="&nbsp;" />
                        <%=SPFunctions.LocalizeUI("step_6_personal_information_collection", "CyberportEMS_CCMFGBAYEP") %>
                        <br />
                        <asp:CheckBox runat="server" ID="Marketing_Information" CssClass="listcss" Text="&nbsp;" />
                        <%=SPFunctions.LocalizeUI("step_6_marketing", "CyberportEMS_CCMFGBAYEP") %>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnl_Buttons" Visible="false" runat="server">
                    <div style="margin-top: 70px; margin-left: 0" class="btn-box sidemargin">

                        <asp:Button ID="btn_StepPrevious" runat="server" Text="Previous" ValidationGroup="anything" CssClass="apply-btn bluetheme" OnClick="btn_StepPrevious_Click" Style="margin-left: 0" />

                        <asp:Button ID="btn_StepSave" runat="server" Text="Save" CssClass="apply-btn greentheme" OnClick="btn_StepSave_Click" />

                        <asp:Button ID="btn_StepNext" runat="server" Text="Next" ValidationGroup="anything" CssClass="apply-btn skytheme" OnClick="btn_StepNext_Click" />
                        <asp:Button ID="btn_Submit" Visible="false" OnClick="btn_Submit_Click1" runat="server" Text="Submit" CssClass="btnSubmitIncubation apply-btn skytheme" />
                        <asp:Button ID="btn_Back" Visible="false" class="apply-btn btn_historyBack greentheme" Text="Back" runat="server" />
                        <asp:HiddenField ID="hdn_ActiveStep" runat="server" Value="0" />
                    </div>
                </asp:Panel>
                <div id="lbl_success" style="color: green" runat="server"></div>
                <div id="Div1" class="text-danger" runat="server"></div>
                <div id="lbl_Exception" class="text-danger" runat="server"></div>
                <asp:BulletedList ID="lblgrouperror" runat="server" CssClass="text-danger" Style="margin-top: 12px; margin-left: 44px"></asp:BulletedList>
                <label class="" style="margin-top: 3%; right: 12%; position: absolute; font-size: x-small; color: #999999;">Doc Ref:  ENC.SF.041</label>

            </div>

        </div>
    </div>

    <!---728x90--->
</div>

<!---728x90--->
<!-----//end-main---->
<asp:Panel ID="Incubation2ndSignGroup" runat="server" DefaultButton="btn_submitFinal">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">                	 
                <%--<%=SPFunctions.LocalizeUI("Submission_popup_password_GDorMacau", "CyberportEMS_Common") %>--%>
                Please input GuangDong or Macau Leader login email and password for confirmation.
            </p>

            <div class="table full-width">
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Email" ID="txtLogin2ndSignEmail" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Password" TextMode="Password" ID="txtLogin2ndSignPassword" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Full Name of Principal Applicant (GuandDong or Macau Leader)" ID="txtInsert2ndFullName" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Position Title of Principal Applicant (GuandDong or Macau Leader)" ID="txtInsert2ndPosition" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Button ID="Button1" OnClick="btn_HideSubmit2ndSignPopup_Click" Text="Cancel" runat="server" CssClass="apply-btn graytheme" />
                    <asp:Button ID="Button2" OnClick="btn_submit2ndSign_Click" Text="Submit" runat="server" CssClass="apply-btn skytheme" />
                </div>
                <div style="padding: 12px 0;">
                    <p class="text-danger" id="UserCustomerrorLogin2" runat="server"></p>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>



<asp:Panel ID="IncubationSubmitPopup" runat="server" DefaultButton="btn_submitFinal">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                <%=SPFunctions.LocalizeUI("Submission_popup", "CyberportEMS_Common") %>
                <asp:Label Text="" runat="server" ID="lblintakeno" />
                <%=SPFunctions.LocalizeUI("Submission_popup_deadline", "CyberportEMS_Common") %>
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
                    <asp:Button ID="btn_HideSubmitPopup" OnClick="btn_HideSubmitPopup_Click" Text="Cancel" runat="server" CssClass="apply-btn graytheme" />
                    <asp:Button ID="btn_submitFinal" OnClick="btn_submitFinal_Click" Text="Submit" runat="server" CssClass="apply-btn skytheme" />
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

<script src="/_layouts/15/Styles/CBP_Styles/jquery-ui.js" type="text/javascript"></script>
<style id='hideMonth'></style>

<script>
    $(".datepickerMonthY").datepicker({

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
    $('.datepickerMonthY').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'MM yy'
    }).focus(function () {
        var thisCalendar = $(this);
        $('.ui-datepicker-calendar').detach();
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
            $('#hideMonth').html('.ui-datepicker-calendar{display:inline-table;}');
            $('.ui-datepicker-calendar').show();

        }

    });
</script>
