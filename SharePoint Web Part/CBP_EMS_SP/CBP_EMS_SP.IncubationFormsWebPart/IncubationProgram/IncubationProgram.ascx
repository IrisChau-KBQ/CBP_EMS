<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncubationProgram.ascx.cs" Inherits="CBP_EMS_SP.IncubationFormsWebPart.IncubationProgram.IncubationProgram" %>

<asp:HiddenField ID="hdn_ProgramID" runat="server" />
<asp:HiddenField ID="hdn_ApplicationID" runat="server" />
<div class="page">

    <!-- start-main -->
    <div class="main">
        <!-- -728x90- -->

        <div class="custom-form-wd-img border-gray boxcenter width-80">

            <div class="head">
                <img src="/_layouts/15/Images/CBP_Images/Programme.png" alt="head-logo" class="head-logo" />
            </div>

            <div class="form __upr">

                <h1 class="form__h1">Cyberport Incubation Program Application
                    <img src="/_layouts/15/Images/CBP_Images/question-mark.png" class="question-mark" />
                </h1>

            </div>


            <asp:Panel ID="pnl_InstructionForm" runat="server">

                <h2 class="subheading" style="text-align: left !important; color: #80C343;">You are required:</h2>

                <div>

                    <ol>
                        <li class="eligibility__list">To observe the following requirements when filling in the application:
                            <ol>
                                <li>Use <mark>strong</mark> to complete the application unless otherwise specified;
                                </li>
                                <li>Present all monetary figures in <mark>Hong Kong Dollar</mark>;
                                </li>
                                <li>Use <mark>Traditional Chinese characters</mark> for Chinese information;
                                </li>
                                <li>Put <mark>“NA”</mark> where the information sought is not applicable or not available;
                                </li>
                            </ol>
                        </li>

                        <li class="eligibility__list">To read the Cyberport Incubation Programme (On-site &amp; Off-site Incubatee) Guides and Notes for the Applicants (ENC.RF.010) before filling in this Application Form.
                        </li>

                        <li class="eligibility__list">The form must be signed by the applicant. If the applicant is a company, the form must be signed by the director or owner of the applicant organisation. Otherwise it will not be processed.
                        </li>

                        <li class="eligibility__list">To submit 1 online application first, then send 1 signed hardcopy of Cyberport Incubation Programme application form to the following address within 5 working days after online submission:
                            <p>Cyberport Incubation Programme</p>
                            <p>Hong Kong Cyberport Management Company Limited</p>
                            <p>Entrepreneurship Centre</p>
                            <p>Level 5, Cyberport 3 (Core F), 100 Cyberport Road, Hong Kong</p>
                        </li>

                        <li class="eligibility__list">For enquiries, please call (852) 3166 3900 or email to ecentre@cyberport.hk.
                            <p>You are recommended to prepare a 1-minute video clip/presentation file/slide show to illustrate the idea of your project if applicable. Kindly provide the link in Section 2.3.2.4,if there is any.</p>
                            <p class="bold">Remarks: This applicationform has been translated into Chinese for reference only. In case of discrepancy, the English version shall prevail.</p>
                        </li>
                    </ol>

                </div>


                <div style="margin-top: 50px; text-align: center;">
                    <asp:Button runat="server" ID="btnIncubationForm" CssClass="btn-green login-btn" Text="Next" OnClick="btnIncubationForm_Click" />
                </div>

            </asp:Panel>

            <asp:Panel ID="pnl_programDetail" Visible="false" runat="server">
                <div class="form__upr">
                    <div class="row">
                        <div class="col-md-2 boldgraylbl">Intake</div>
                        <div class="col-md-3">
                            <asp:Label ID="lblIntake" runat="server" Text="201612"></asp:Label>

                        </div>
                        <div class="col-md-3 boldgraylbl">Deadline</div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDeadline" runat="server" Text=" 1 Dec 2016 5:00pm (GTM+8)"></asp:Label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2 boldgraylbl">Applicant</div>
                        <div class="col-md-3">
                            <asp:Label ID="lblApplicant" runat="server" Text="applicant@email.com"></asp:Label>
                        </div>
                        <div class="col-md-3 boldgraylbl">Application No.</div>
                        <div class="col-md-4">
                            <asp:Label ID="lblApplicationNo" runat="server" Text="CPIP201612-0001"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 boldgraylbl">Last Submitted</div>
                        <div class="col-md-3">
                            <asp:Label ID="lblLastSubmitted" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </asp:Panel>


            <div class="form">
                <asp:BulletedList ID="progressList" Visible="false" runat="server" class="progressbar" Style="margin: 50px 0; overflow: hidden;">
                    <asp:ListItem Value="1" Text="" />
                    <asp:ListItem Value="2" Text="" />
                    <asp:ListItem Value="3" Text="" />
                    <asp:ListItem Value="4" Text="" />
                    <asp:ListItem Value="5" Text="" />
                </asp:BulletedList>
                <asp:Panel ID="pnl_IncubationStep1" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">

                            <h2 class="subheading">Your profile and eligibility</h2>

                            <div>

                                <table class="table-eligibility">
                                    <tr>
                                        <th style="width: 50px"></th>
                                        <th></th>
                                        <th style="width: 50px">Yes</th>
                                        <th style="width: 50px">No</th>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.1</td>
                                        <td>Is your idea rooted in digital tech related areas?</td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q1" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.2</td>
                                        <td>Do you have a viable business plan for a product or service to be ready for marketing in 12 to 18 months?
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q2" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.3</td>
                                        <td>Do you have sufficient funds, or plans to raise sufficient funds, for the company to operate for at least one year upon admission to the Cyberport Incubation Programme?
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q3" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.4</td>

                                        <td>Do you presently have a company registered as a legal entity and incorporated in Hong Kong?
                            
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q4" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.5</td>

                                        <td>If not, do you agree to set up a company registered as a legal entity and incorporated in Hong Kong upon admission to the Cyberport Incubation Programme?
                            
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q5" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.6</td>

                                        <td>Have your proposed project or your other similar digital tech project(s) applied for or received any grant or funding from any publicly and / or privately funded organizations/ programmes in the past 18 months? If “Yes”, please list out in details in Section 2.3.2.3 below.
                            
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q6" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.7</td>

                                        <td>Is your proposed project or your other similar digital tech projects applying for, or does your project or your other similar digital tech project(s) anticipates to receiving or to becoming entitled to receive any grant or funding from any publicly and / or privately-funded organizations or programmes in the coming 18 months? If “Yes”, please list out in details in Section 2.3.2.3 below.
                            
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q7" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.8</td>

                                        <td>Have your project or your other similar digital tech project(s) applied for the Cyberport Creative Micro Funds (CCMF)? If “Yes”, please list out in details in Section 2.3.2.3 below.
                            
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q8" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.9</td>

                                        <td>If you have answered “yes” to Question 1.8 above, are you applying for the Cyberport Incubation Programme using the same or similar project under CCMF? If similar or different, please clarify with details in Section 2.3.2.4 below to enable our Vetting Team to distinguish this application from any others.
                            
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q9" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="bluelbl">1.10</td>

                                        <td>Please state whether this application is a re-submission of a previously-submitted application to the programme(s) of Cyberport, and / or other publicly-administered funding schemes. If yes, please set out the project reference of the previous application in Section 2.3.2.4 and highlight the main differences of this application vis-à- vis the previous one.
                                    </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rbtnPanel1Q10" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="True" Text=""></asp:ListItem>
                                                <asp:ListItem Value="False" Text=""></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </div>




                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep2" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">
                            <h2 class="subheading">COMPANY INFORMATION</h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.1</span>
                                    Company Name</h2>

                                <div class="row sidemargin">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCompanyNameEnglish" runat="server" CssClass="input w90" placeholder="English"></asp:TextBox>

                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCompanyNameChinese" runat="server" CssClass="input w90" placeholder="Chinese"></asp:TextBox>

                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.2</span>
                                    Abstract
									<small>(A summary of your company’s vision, mission and positioning.)</small></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.2.1</span>
                                        English <small>(Within 300 words)</small>
                                    </h4>
                                    <asp:TextBox ID="txtAbstractEnglish" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.2.2</span>
                                        Chinese <small>(Within 300 words)</small>
                                    </h4>
                                    <textarea class="form-control"></textarea>
                                </div>
                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.3</span>
                                    Company Details</h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.3.1</span>
                                        Objectives - the “why” of the company
                                    </h4>
                                    <asp:TextBox ID="txtObjective" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>

                                <div class="form-group">
                                    <h4 class="subheading2">
                                        <span>2.3.2</span>
                                        Background
                                    </h4>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <p>2.3.2.1</p>
                                            General background, history and ownership of this company. Please provide supporting documents, such as BR copy, company annual return, and names of the company’s founder(s), owner(s), major shareholder(s) and key member(s).
                                                    <h4></h4>
                                            <asp:TextBox ID="txtbackground" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            <h4></h4>
                                            <h4></h4>
                                            <h4></h4>
                                        </h4>

                                    </div>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <p>2.3.2.2</p>
                                            Has any pilot work been done by your company and / or company partners in preparation for this company? If yes, please describe the work done.
                                                    <h4></h4>
                                            <asp:TextBox ID="txtPilot_Work_Done" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            <h4></h4>
                                            <h4></h4>
                                            <h4></h4>
                                        </h4>

                                    </div>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <p>2.3.2.3</p>
                                            <p>Funding Status</p>
                                            (List out in detail (i) all grants and funding received/to be received from other publicly and / or privately funded organizations/ programmes which the applicant (or companies established by he/she/it) has applied for, will receive or will be entitled to receive in the coming 18 months, or have received in the past 18 months; (ii) the nature of expenditure covered/to be covered by such funding sources; and (iii) the amount and the maximum amount received/to be received under such funding sources )
                                                   
                                        </h4>

                                    </div>

                                    <asp:GridView ID="Grd_FundingStatus" runat="server"
                                        ShowFooter="True" AutoGenerateColumns="False"
                                        ShowHeader="false"
                                        GridLines="None" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="FundingID" runat="server" Value='<%#Eval("Funding_ID") %>' />
                                                    <div class="form-box form-group sidemargin" data-id='<%# Container.DataItemIndex+1 %>'>

                                                        <div class="form-group">
                                                            <p>
                                                                <label>Name of Programme</label>
                                                            </p>
                                                            <asp:TextBox ID="txtNameofProgram" Text='<%#Eval("Programme_Name") %>' runat="server" CssClass="input-sm" />
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label>Date</label>
                                                                </p>

                                                                <asp:TextBox ID="txtApplicationDate" Text='<%#Eval("Date") %>' runat="server" CssClass="input-sm" />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label>Application Status</label>
                                                                </p>
                                                                <asp:TextBox ID="txtApplicationStatus" Text='<%#Eval("Application_Status") %>' runat="server" CssClass="input-sm" />
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label>Funding Status</label>
                                                                </p>
                                                                <asp:TextBox ID="txtFundingStatus" Text='<%#Eval("Funding_Status") %>' runat="server" CssClass="input-sm" />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label>Nature of expenditure covered</label>
                                                                </p>
                                                                <asp:TextBox ID="txtNature" runat="server" Text='<%#Eval("Expenditure_Nature") %>' CssClass="input-sm" />
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label>Amount received/to be received</label>
                                                                </p>
                                                                <asp:TextBox ID="txtAmountReceived" runat="server" Text='<%#Eval("Amount_Received") %>' CssClass="input-sm" />
                                                            </div>
                                                            <div class="col-md-6">
                                                                <p>
                                                                    <label>Maximum amount to be received</label>
                                                                </p>
                                                                <asp:TextBox ID="txtApplicationMaximumAmount" runat="server" Text='<%#Eval("Maximum_Amount") %>' CssClass="input-sm" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <div class="form-group sidemargin">
                                        <asp:ImageButton ID="btn_FundingAddNew" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" OnClick="btn_FundingAddNew_Click" />
                                        <asp:Label CssClass="text-danger" ID="lbl_fundingError" runat="server" />
                                    </div>

                                    <div class="form-group sidemargin">
                                        <h4 class="subheading2">
                                            <p>2.3.2.4</p>

                                            Additional Information <small>(If applicable)</small>
                                            <h4></h4>
                                            <asp:TextBox ID="txtAdditionalInformation" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
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
                                    Implementation
									<small>(A summary of your company’s vision, mission and positioning.)</small></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.1</span>
                                        Proposed Products / Services
                                    </h4>
                                    <asp:TextBox ID="txtProposedProducts" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.2</span>
                                        Target Market / Clients and Market Potential
                                    </h4>
                                    <asp:TextBox ID="txtTargetMarket" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>


                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.3</span>
                                        Competition analysis: Any other similar or competitive products / services in the market? If yes, how do you differentiate your products / services from them and what are your competitive edges?
                                    </h4>
                                    <asp:TextBox ID="txtCompetitionAnalysis" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>


                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.4</span>
                                        Revenue Model / Revenue Plan
                                    </h4>
                                    <asp:TextBox ID="txtRevenueModel" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>


                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.5</span>
                                        Exit Strategy <small>(If applicable)</small>
                                    </h4>
                                    <asp:TextBox ID="txtExitStrategy" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.6</span>
                                        Business Performance Milestones set for every six months during the entire incubation period.
                                    </h4>

                                    <div class="row">
                                        <div class="col-md-3 bluelbl">Milestone</div>
                                        <div class="col-md-9 bluelbl">Details</div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3 graylbl">First 6 Months</div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtFirst6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3 graylbl">Second 6 Months</div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtSecond6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3 graylbl">Third 6 Months</div>
                                        <div class="col-md-9">

                                            <asp:TextBox ID="txtThird6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3 graylbl">Forth 6 Months</div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtForth6Months" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>


                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.7</span>
                                        Please state whether this application is a re-submission of a previously-rejected application to Cyberport, and / or other Government-administered funding schemes. If yes, please set out the project reference of the previous application, in Section 2.4.8
                                    </h4>

                                    <div class="form-group" style="margin-bottom: 12px;">
                                        <asp:RadioButtonList ID="rbtnResubmission" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                    <asp:TextBox ID="txtResubmission_Project_Reference" runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>


                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.4.8</span>
                                        If you have answered “yes” to Question 2.4.7 above, please highlight the main differences of this application vis-à- vis the previous one and explain how the differences have addressed the previously-raised concerns.
                                    </h4>
                                    <asp:TextBox ID="txtResubmission_Main_Differences" Enabled="false" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>2.5</span>
                                    Classification of Company</h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">
                                        <span>2.5.1</span>
                                        Company Type
                                    </h4>

                                    <div class="form-group">
                                        <asp:RadioButtonList ID="rbtnCompany_Type" CssClass="rboCompany_Type" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="private" Text="Unlimited (private)"></asp:ListItem>
                                            <asp:ListItem Value="Limited" Text="Limited"></asp:ListItem>
                                            <asp:ListItem Value="Public" Text="Publicly listed"></asp:ListItem>
                                            <asp:ListItem Value="Others" Text="Others"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtOther_Company_Type" Style="display: none" runat="server" CssClass="input-sm input-half" placeholder="Please specify"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>2.5.2</span>
                                    Business Area <small>(Please choose one closest to your nature)</small>
                                </h4>
                                <div class="form-group">
                                    <asp:RadioButtonList ID="rbtnList_Business_Area" CssClass="rdoBusiness_Area" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                                        <asp:ListItem Text=" App Design/ Web Production"></asp:ListItem>
                                        <asp:ListItem Text="E-commerce"></asp:ListItem>
                                        <asp:ListItem Text="Edutech"></asp:ListItem>
                                        <asp:ListItem Text="Fintech"></asp:ListItem>
                                        <asp:ListItem Text="Gaming"></asp:ListItem>
                                        <asp:ListItem Text="Healthcare"></asp:ListItem>
                                        <asp:ListItem Text="Open Data"></asp:ListItem>
                                        <asp:ListItem Text="Social Media"></asp:ListItem>
                                        <asp:ListItem Text="Wearable"></asp:ListItem>
                                        <asp:ListItem Text="Others"></asp:ListItem>
                                    </asp:RadioButtonList>


                                    <div class="form-group">
                                        <asp:TextBox ID="txtOther_Bussiness_Area" Style="display: none" runat="server" CssClass="input-sm input-half" placeholder="Please specify"></asp:TextBox>

                                    </div>
                                </div>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>2.5.3</span>
                                    Positioning <small>(You may choose more than one)</small>
                                </h4>
                                <div class="form-group">
                                    <asp:CheckBoxList ID="chkPositioning" runat="server" CssClass="chkPositioning" RepeatDirection="Horizontal" RepeatColumns="4">
                                        <asp:ListItem Text="Content creation"></asp:ListItem>
                                        <asp:ListItem Text="Production / post-production"></asp:ListItem>
                                        <asp:ListItem Text="Publishing / distribution / delivery"></asp:ListItem>
                                        <asp:ListItem Text="Platform / device development"></asp:ListItem>
                                        <asp:ListItem Text="Management / trading / service"></asp:ListItem>
                                        <asp:ListItem Text="Others"></asp:ListItem>
                                    </asp:CheckBoxList>

                                </div>
                                <div class="form-group">

                                    <asp:TextBox ID="txtPositioningOther" Style="display: none" runat="server" CssClass="input-sm input-half" placeholder="Please specify"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>2.5.4</span>
                                    Other Attributes, if any
                                </h4>
                                <asp:TextBox ID="txtOtherAttributes" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>2.5.5</span>
                                    Please indicate your preferred track of incubation
											<small>(You may choose ONLY one)</small>
                                </h4>

                                <div class="form-group">
                                    <asp:RadioButtonList ID="rbtnPreferred_Track" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text=" On-site incubation"></asp:ListItem>
                                        <asp:ListItem Text="Off-site incubation"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep3" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading">COMPANY OWNERSHIP, ORGANIZATION CHART, CORE MEMBERS, PARTNERS AND FINANCIAL INFORMATION</h2>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.1</span>
                                Company Ownership and Organization Chart</h2>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.1.1</span>
                                    Company Ownership Structure
										</h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtCompany_Ownership_1" TextMode="MultiLine"></asp:TextBox>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.1.2</span>
                                    Company Ownership Structure
										</h4>
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif" CssClass="input-sm input-half input-trs" ID="fu_Company_Ownership_2" runat="server" />
                                    <img src="/_layouts/15/Images/CBP_Images/dir.png" alt="" />
                                    <%-- <asp:RegularExpressionValidator ID="rexpfu_Company_Ownership_2" runat="server" ControlToValidate="fu_Company_Ownership_2"
                                        ErrorMessage="Only .gif, .jpg, .png, .tiff and .jpeg"
                                        ValidationExpression="(.*\.([Pp][Dd][Ff])|.*\.([Dd][Oo][Cc][Xx])|.*\.([Dd][Oo][Cc])|.*\.([Gg][Ii][Ff])|.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Xx][Ll][Ss]|.*\.([Xx][Ll][Ss][Xx]|.*\.([Pp][Pp][Tt]|.*\.([Pp][Pp][Tt][Xx])$)"></asp:RegularExpressionValidator>
                                    --%>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.2</span>
                                Company Core Members and Major Partners</h2>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.2.1</span>
                                    Core Members’ Profiles
										</h4>
                                <asp:GridView ID="grvCoreMember" runat="server"
                                    ShowFooter="True" AutoGenerateColumns="False"
                                    ShowHeader="false"
                                    GridLines="None" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="Core_Member_ID" runat="server" Value='<%#Eval("Core_Member_ID") %>' />

                                                <div class="form-box form-group" data-id='<%# Container.DataItemIndex+1 %>'>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <p>
                                                                <label>Name</label>
                                                            </p>

                                                            <asp:TextBox CssClass="input-sm" Text='<%#Eval("Name") %>' ID="Name" runat="server" />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <p>
                                                                <label>Positions / Titles</label>
                                                            </p>
                                                            <asp:TextBox CssClass="input-sm" ID="Title" Text='<%#Eval("Position") %>' runat="server" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <p>
                                                            <label>Academic and professional qualifications</label>
                                                        </p>
                                                        <asp:TextBox CssClass="input-sm" ID="Academic" Text='<%#Eval("Professional_Qualifications") %>' runat="server" />
                                                    </div>

                                                    <div class="form-group">
                                                        <p>
                                                            <label>Working experiences</label>
                                                        </p>
                                                        <asp:TextBox CssClass="input-sm" ID="Experience" Text='<%#Eval("Working_Experiences") %>' runat="server" />
                                                    </div>

                                                    <div class="form-group">
                                                        <p>
                                                            <label>Special achievements</label>
                                                        </p>
                                                        <asp:TextBox CssClass="input-sm" ID="Achievements" Text='<%#Eval("Special_Achievements") %>' runat="server" />
                                                    </div>

                                                </div>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                                <div class="form-group">
                                    <asp:ImageButton ID="ButtonAddCoreMembers" ImageUrl="/_layouts/15/Images/CBP_Images/invite.png" runat="server" Text="Add New Row" OnClick="ButtonAddCoreMembers_Click" />
                                    <asp:Label ID="lblCorememberError" CssClass="text-danger" runat="server" />
                                </div>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.2.2</span>
                                    Major Partners’ Profiles
										</h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtPartner_Profiles" TextMode="MultiLine"></asp:TextBox>

                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>3.3</span>
                                    Expenditure Distribution and Forecast Income</h2>
                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.1</span>
                                    Manpower Distribution
										</h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Manpower_Distribution" TextMode="MultiLine"></asp:TextBox>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.2</span>
                                    Equipment Distribution
										</h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Equipment_Distribution" TextMode="MultiLine"></asp:TextBox>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.3</span>
                                    Other Direct Costs
										</h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Other_Costs" TextMode="MultiLine"></asp:TextBox>

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.3.4</span>
                                    Forecast Income
										</h4>
                                <asp:TextBox runat="server" CssClass="form-control" ID="Forecast_Income" TextMode="MultiLine"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep4" Visible="false" runat="server">
                    <div class="box-wrpr">
                        <h2 class="subheading">CONTACT DETAILS</h2>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>4.1</span>
                                Contact Person 1 (Principal Applicant)</h2>
                            <div class="form-group sidemargin">
                                <div class="form-box form-group" data-id="1">

                                    <div class="row">
                                        <div class="col-md-2">
                                            <p>
                                                <label>Salution</label>
                                            </p>
                                            <%--<select name="" id="" class="form-control">
														<option value="">DR</option>
														<option value="">DR2</option>
													</select>--%>
                                            <asp:DropDownList ID="Salutation" runat="server">
                                                <asp:ListItem Text="DR" Value=""></asp:ListItem>
                                                <asp:ListItem Text="DR2" Value=""></asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Last Name</label>
                                            </p>
                                            <input type="text" id="Last_name" runat="server" class="input-sm" />
                                        </div>

                                        <div class="col-md-4">
                                            <p>
                                                <label>First Name</label>
                                            </p>
                                            <input type="text" id="First_Name" runat="server" class="input-sm" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p>
                                            <label>Position</label>
                                        </p>
                                        <input type="text" id="Position" runat="server" class="input-sm" />
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <p>
                                                <label>Contact No. (Home)</label>
                                            </p>
                                            <input type="text" id="Contact_No_Home" runat="server" class="input-sm" />
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Contact No. (Office)</label>
                                            </p>
                                            <input type="text" id="Contact_No_Office" runat="server" class="input-sm" />
                                        </div>

                                        <div class="col-md-4">
                                            <p>
                                                <label>Contact No. (Mobile)</label>
                                            </p>
                                            <input type="text" id="Contact_No_Mobile" runat="server" class="input-sm" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <p>
                                                <label>Fax</label>
                                            </p>
                                            <input type="text" id="Fax" runat="server" class="input-sm" />
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Email</label>
                                            </p>
                                            <input type="text" id="Email" runat="server" class="input-sm" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.2</span>
                                    Contact Person 2 (Principal Applicant)</h2>

                                <div class="form-group sidemargin">
                                    <div class="form-box form-group" data-id="2">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <p>
                                                    <label>Salution</label>
                                                </p>
                                                <asp:DropDownList ID="Salutation1" runat="server">
                                                    <asp:ListItem Text="DR" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="DR2" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Last Name</label>
                                                </p>
                                                <input type="text" class="input-sm" id="Last_Name1" runat="server" />
                                            </div>

                                            <div class="col-md-4">
                                                <p>
                                                    <label>First Name</label>
                                                </p>
                                                <input type="text" class="input-sm" id="First_Name1" runat="server" />
                                            </div>
                                        </div>

                                        <div class="row">
                                            <p>
                                                <label>Position</label>
                                            </p>
                                            <input type="text" class="col-md-4 input-sm" id="Position1" runat="server" />
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Contact No. (Home)</label>
                                                </p>
                                                <input type="text" class="input-sm" id="Contact_No_Home1" runat="server" />
                                            </div>
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Contact No. (Office)</label>
                                                </p>
                                                <input type="text" class="input-sm" id="Contact_No_Office1" runat="server" />
                                            </div>

                                            <div class="col-md-4">
                                                <p>
                                                    <label>Contact No. (Mobile)</label>
                                                </p>
                                                <input type="text" class="input-sm" id="Contact_No_Mobile1" runat="server" />
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Fax</label>
                                                </p>
                                                <input type="text" class="input-sm" id="Fax1" runat="server" />
                                            </div>
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Email</label>
                                                </p>
                                                <input type="text" class="input-sm" id="Email1" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <p>
                                                <label>Mailing Address</label>
                                            </p>
                                            <textarea class="form-control" id="Mailing1" runat="server"></textarea>
                                        </div>

                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep5" Visible="false" runat="server">
                    <div class="box-wrpr" style="width: 700px; margin: 0 auto;">
                        <h2 class="subheading">ATTACHMENT</h2>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.1">BR COPY</label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fu_BrCopy" runat="server" />
                                    <img src="/_layouts/15/Images/CBP_Images/dir.png" alt="">
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.2">Company Annual Return</label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fu_AnnualReturn" runat="server" />
                                    <img src="/_layouts/15/Images/CBP_Images/dir.png" alt="">
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.3">Video Clip</label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:TextBox CssClass="input-sm input-half1 input-trs" Style="width: 63%;" ID="txtVideoClip" runat="server" placeholder="Please input hyperlink of the video" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.4">Presentation Slide</label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuPresentationSlide" runat="server" />
                                    <img src="/_layouts/15/Images/CBP_Images/dir.png" alt="">
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-md-6 lbl" data-id="5.5">Other Attachment</label>
                            <div class="col-md-6">
                                <div class="dirbox">
                                    <asp:FileUpload AllowMultiple="false" accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.png,.jpg,.gif"
                                        CssClass="input-sm input-half input-trs" ID="fuOtherAttachement" runat="server" />
                                    <img src="/_layouts/15/Images/CBP_Images/dir.png" alt="">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-wrpr">

                        <h2 class="subheading">DECLARATION</h2>
                        <div class="form-group green-clr-theme">
                            <asp:CheckBox ID="chkDeclaration" runat="server" />
                            By checking this box, we acknowledge to the below declaration:
                       
                        </div>

                        <p class="form-group">We have observed and agreed with all terms and conditions set out in the Cyberport Incubation Programme (On-site &amp; Off-site Incubatee) Guides and Notes for the Applicants (ENC.RF.010) governing the application of the Cyberport Incubation Programme;</p>

                        <p class="form-group">All information provided in this application as well as the accompanying information reflects the status of affairs as at the date of submission. I shall inform the Administration of the Cyberport Incubation Programme immediately if there are any subsequent changes to the above information;</p>

                        <p class="form-group">The ideas of the proposed project are original without any constituted or potential act of infringement of the intellectual property rights of other individuals and / or organisations;</p>

                        <p class="form-group">We understand the information provided in this application will be disclosed to Cyberport staff for screening purpose. No Confidential Agreement of any kind will be signed with Cyberport or the relevant staff on any occasion; and</p>

                        <p class="form-group">We give our consent to Cyberport staff to carry out necessary due diligence/reference check for the purpose of assessing this application.</p>

                        <div class="row">
                            <div class="col-md-6">
                                <p class="form-group lbl">Full Name of Principle Applicant</p>
                                <asp:TextBox CssClass="input-sm" ID="txtName_PrincipalApplicant" runat="server" />
                            </div>
                            <div class="col-md-6">
                                <p class="form-group lbl">Position Title of Principle Applicant</p>
                                <asp:TextBox CssClass="input-sm" ID="txtPosition_PrincipalApplicant" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="box-wrpr">
                        <h2 class="subheading">PERSONAL INFORMATION COLLECTION STATEMENT</h2>

                        <p class="form-group">Please visit our website http://www.cyberport.hk/en/pics for the Personal Information Collection Statement of Hong Kong Cyberport Management Company Limited.</p>
                        <p class="form-group">I have read the Personal Information Collection Statement of Hong Kong Cyberport Management Company Limited (&quot;HKCMCL&quot;) 香港數碼港管理有限公司, including the information about the use of my personal data in direct marketing, and I understand its contents. By ticking the box below, I signify my consent for HKCMCL to use my personal data (primarily my name and contact details) in direct marketing services, products, facilities, activities and other subjects to me (primarily services, products, facilities, activities, events and subjects offered in relation to HKCMCL or Cyberport or partners of Cyberport or shops or merchants in Cyberport or offered or hosted at Cyberport) as more particularly set out in the Personal Information Collection Statement.</p>

                        <div class="green-clr-theme">
                            <asp:CheckBox runat="server" ID="Marketing_Information" />
                            I agree to receive direct marketing information.
                       
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_Buttons" Visible="false" runat="server">
                    <div style="margin-top: 50px;" class="btn-box sidemargin">

                        <asp:Button ID="btn_StepPrevious" runat="server" Text="Previous" CssClass="apply-btn bluetheme" OnClick="btn_StepPrevious_Click" />

                        <asp:Button ID="btn_StepSave" runat="server" Text="Save" CssClass="apply-btn greentheme" OnClick="btn_StepSave_Click" />

                        <asp:Button ID="btn_StepNext" runat="server" Text="Next" CssClass="apply-btn skytheme" OnClick="btn_StepNext_Click" />
                        <asp:Button ID="btn_Submit" Visible="false" runat="server" Text="Submit" CssClass="apply-btn skytheme"/>
                        <asp:HiddenField ID="hdn_ActiveStep" runat="server" Value="0" />
                    </div>
                </asp:Panel>

                <div id="lbl_Exception" class="text-danger" runat="server"></div>
            </div>
        </div>


        <!---728x90--->
    </div>
    <!---728x90--->
    <!-----//end-main---->
</div>
  <div class="popup" id="submitpopup" style="display: none;">
            <div class="pos-relative card-theme full-width">
                <p class="popup--para">
                	Your submission for intake 201612, deadline is 1 Dec 2016 5:00 (GTM+8).
                	Please input your login email and password for confirmation.
                </p>

                <div class="table full-width">
                    <div class="form-group">
                    	<input type="text" class="input" placeholder="Email">
                    </div>
                    <div class="form-group">
                    	<input type="text" class="input" placeholder="Password">
                    </div>

                    <div class="form-group">
                    	<input type="submit" onclick="myFunction()" value="Cancel" class="apply-btn primary-theme" style="margin-right: 20px;">
                    	<input type="submit" onclick="myFunction()" value="Submit" class="apply-btn theme-green">
                    </div>
                </div>
            </div>
        </div>

