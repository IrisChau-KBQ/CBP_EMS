<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossBorderFormWebPart.ascx.cs" Inherits="CBP_EMS_SP.CrossBorderForm.CrossBorderFormWebPart" %>
<%--<asp:HiddenField ID="hdn_ProgramID" runat="server" />
<asp:HiddenField ID="hdn_ApplicationID" runat="server" />
<asp:Label ID="lbj" Text="text" runat="server" />
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
                                <li>Use <mark>English</mark> to complete the application unless otherwise specified;
                                </li>
                                <li>Present all monetary figures in <mark>Hong Kong Dollar;</mark>
                                </li>
                                <li>Use <mark>Traditional Chinese characters</mark> for Chinese information;
                                </li>
                                <li>Put <mark>“NA”</mark> where the information sought is not applicable or not available;
                                </li>
                            </ol>
                        </li>
                        <li class="eligibility__list">To read the <mark>CCMF Guides and Notes for the Applicants</mark> (ENC.RF.015) before filling in this Application Form.
                        </li>

                        <li class="eligibility__list">The form must be signed by the applicant. If the applicant is a company, the form must be signed by the director or owner of the applicant organisation. Otherwise it will not be processed.
                        </li>

                        <li class="eligibility__list">To submit 1 online application first, then send <mark>1 signed hardcopy of CCMF application form to the following address within 5 working days after online submission:</mark>
                            <div class="form-group sidemargin" style="padding-top: 10px;">
                                <p class="form-group"><strong>Cyberport Creative Micro Fund</strong></p>
                                <p class="form-group">Hong Kong Cyberport Management Company Limited</p>
                                <p class="form-group">Entrepreneurship Centre</p>
                                <p class="form-group">Level 5, Cyberport 3 (Core F), 100 Cyberport Road, Hong Kong</p>
                            </div>
                        </li>

                        <li class="eligibility__list">For enquiries, please call (852) 3166 3900 or email to <mark>ecentre@cyberport.hk.</mark>
                        </li>
                    </ol>
                    <div style="padding-top: 10px;">
                        <p class="form-group">You are recommended to prepare a 1-minute video clip/presentation file/slide show to illustrate the idea of your project if applicable. Kindly provide the link in Section 2.3.2.4, if there is any.</p>
                        <p class="from-group bold">Remarks: This application form has been translated into Chinese for reference only. In case of discrepancy, the English version shall prevail.</p>
                        <p class="from-group bold">Hong kong Cyberpost Company Private Limited</p>
                    </div>

                </div>


                <div style="margin-top: 50px; text-align: center;">
                    <asp:Button runat="server" ID="btnIncubationForm" CssClass="btn-green login-btn" Text="Continue" OnClick="btnIncubationForm_Click" />
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
                    <asp:ListItem Value="6" Text="" />
                </asp:BulletedList>
                <asp:Panel ID="pnl_IncubationStep1" Visible="false" runat="server">

                    <div class="form-group">

                        <div class="box-wrpr">

                            <h2 class="subheading">TYPES OF CCMS</h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>1.1</span>
                                    Please choose one of the types of CCMF below:</h2>

                                <div class="sidemargin">
                                    <div class="form-group">
                                        <asp:RadioButton ID="rdo_HK" runat="server" CssClass="width90" Text="Hong Kong Programme" GroupName="rdoformopt" Enable="false" />
                                    </div>
                                    <%--<div class="sidemargin">
                                        <asp:RadioButtonList ID="rdo_HK_Option" runat="server" RepeatDirection="Vertical" CssClass="width90">
                                            <asp:ListItem Value="True" Text="">Professional Stream</asp:ListItem>
                                            <asp:ListItem Value="False" Text="">Hong Kong Young Entrepreneur Programme</asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>>
                                    <div class="form-group">
                                        <asp:RadioButton ID="rdo_Crossborder" runat="server" CssClass="width90" Text="Cross-Border Programme(s) Supported by CCMF" GroupName="rdoformopt" Enable="false" />
                                    </div>
                                    <%--<asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Vertical" CssClass="width90">
                                        <asp:ListItem Value="True" Text="">Cross-Border Programme(s) Supported by CCMF</asp:ListItem>
                                        <asp:ListItem Value="False" Text="">Cyberport University Partnership Programme Supported by CCMF</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>1.2</span>
                                    Please choose one of the types of CCMF application below:</h2>

                                <div class="sidemargin">
                                    <asp:RadioButtonList ID="RadioButtonList4" runat="server" RepeatDirection="Vertical" CssClass="width90">
                                        <asp:ListItem Value="True" Text="">Individual Application</asp:ListItem>
                                        <asp:ListItem Value="False" Text="">Company Application</asp:ListItem>
                                    </asp:RadioButtonList>

                                </div>
                            </div>

                        </div>
                    </div>




                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep2" Visible="false" runat="server">
                    <asp:Panel ID="pnl_HK" runat="server" Visible="false"></asp:Panel>
                    <asp:Panel ID="pnl_crossborder" Visible="false" runat="server"></asp:Panel>
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
                                        <asp:TextBox ID="txtOther_Bussiness_Area1" Style="display: none" runat="server" CssClass="input-sm input-half" placeholder="Please specify"></asp:TextBox>

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
                        <h2 class="subheading">PROJECT INFORMATION</h2>


                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.1</span>
                                Project Name</h2>

                            <div class="row sidemargin">

                                <div class="col-md-6">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="English" placeholder="English"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="Chinese" placeholder="Chinese"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.2</span>
                                Abstract
                               
                                <small>(A summary of your company’s vision, mission and positioning.)</small></h2>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.2.1</span>
                                    English <small>(Within 300 words)</small>
                                </h4>
                                <asp:TextBox runat="server" ID="txtProjectInfoAbsEng" TextMode="MultiLine" CssClass="form-control" />

                            </div>

                            <div class="form-group sidemargin">
                                <h4 class="subheading2">
                                    <span>3.2.2</span>
                                    Chinese <small>(Within 300 words)</small>
                                </h4>
                                <asp:TextBox runat="server" ID="txtProjectInfoAbschi" TextMode="MultiLine" CssClass="form-control" />
                            </div>
                        </div>


                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.3</span>
                                Business Area</h2>
                            <p class="subheading2 sidemargin">(Please choose one closest to your nature)</p>

                            <div class="form-group sidemargin">
                                <div class="form-group">
                                    <asp:RadioButtonList ID="RadioButtonList1" CssClass="rdoBusiness_Area" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                                        <asp:ListItem Text="App Design/ Web Production" Value="App Design/ Web Production"></asp:ListItem>
                                        <asp:ListItem Text="E-Commerce" Value="E-Commerce"></asp:ListItem>
                                        <asp:ListItem Text="Edutech" Value="Edutech"></asp:ListItem>
                                        <asp:ListItem Text="Open Data" Value="Open Data"></asp:ListItem>
                                        <asp:ListItem Text="Fintech" Value="Fintech"></asp:ListItem>
                                        <asp:ListItem Text="Gaming" Value="Gaming"></asp:ListItem>
                                        <asp:ListItem Text="Healthcare" Value="Healthcare"></asp:ListItem>
                                        <asp:ListItem Text="Wearable" Value="Wearable"></asp:ListItem>
                                        <asp:ListItem Text="Socila-Media" Value="Socila-Media"></asp:ListItem>
                                        <asp:ListItem Text="Others" Value="Others"></asp:ListItem>


                                    </asp:RadioButtonList>


                                </div>

                                <div class="form-group">

                                    <asp:TextBox runat="server" CssClass="input-sm input-half" ID="txtOther_Bussiness_Area" placeholder="Please specify" Style="display: none" />

                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <h2 class="subheading text-left">
                                <span>3.4</span>
                                Anticipated Commencement and Completion Date</h2>

                            <div class="row">
                                <div class="col-md-6">
                                    <p class="form-group">Date (day - month - year)</p>
                                    <asp:TextBox runat="server" CssClass="input-sm " ID="txtantisdate"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <p class="form-group">Complete Date (day - month - year)</p>
                                    <asp:TextBox runat="server" CssClass="input-sm" ID="txtanticdate"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep4" Visible="false" runat="server">
                    <div class="form-group">
                        <div class="box-wrpr">
                            <h2 class="subheading">APPLICATION INFORMATION</h2>

                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.1</span>
                                    Project Management Team <small>(Vetting Weighting: 30%)</small> </h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">The ability of the project management team reflected from its prior track record, execution ability, good probability of launch to market, individual qualifications, and team job allocation.
										</h4>
                                    <asp:TextBox runat="server" ID="txtpromanagteam" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>




                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.2</span>
                                    Business Model & Time to Market <small>(Vetting Weighting: 30%)</small></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">Effective communication and the qualities of vision, direction, short and long term objectives, market need for product, target market and marketing strategy, and realistic assessment of the Project viability.  In addition, a product launch within three months from grant disbursement will be encouraged.
										</h4>
                                    <asp:TextBox runat="server" ID="txtbusinessmodelteam" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.3</span>
                                    Creativity and Innovation of the Proposed Project, Product and Service <small>(Vetting Weighting： 30%)</small></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">Factors such as the use of innovative technologies, creative solutions, disruptive technology capability, emerging, or breakthrough problem-solving technologies.
										</h4>
                                    <asp:TextBox runat="server" ID="txtcreativity" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.4</span>
                                    Social Responsibility <small>(Vetting Weighting: 10%)</small></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">Preference will be given to projects that incorporate social responsibility objectives as a project focus (including contribution to open source, creative commons and other “progressive” technologies with a social focus), demonstration of ethical decision making, or contribution towards solving problems that originate from the social environment.
										</h4>
                                    <asp:TextBox runat="server" ID="txtsocialrespon" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.5</span>
                                    Competition Analysis <small>(If applicable)</small></h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">The ability of the project management team reflected from its prior track record, execution ability, good probability of launch to market, individual qualifications, and team job allocation.
										</h4>
                                    <asp:TextBox runat="server" ID="txtcompanalysis" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>


                            <div class="form-group sidemargin">
                                <h2 class="subheading text-left">
                                    <span>4.6</span>
                                    6-Month Project Milestone
									
                                    <p class="subheading2"><small>(The Milestone proposed will form as a basis for continuous project review and evaluation if the Grant is awarded )</small></p>
                                </h2>



                                <div class="row">
                                    <div class="col-md-3 bluelbl">Milestone</div>
                                    <div class="col-md-9 bluelbl">Details</div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl">Months - 1</div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth1" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl">Months - 2</div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth2" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl">Months - 3</div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth3" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl">Months - 4</div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth4" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl">Months - 5</div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth5" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 graylbl">Months - 6</div>
                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtmonth6" TextMode="MultiLine" CssClass="form-control" />
                                    </div>
                                </div>

                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.7</span>
                                    Cost Projections:</h2>

                                <div class="form-group sidemargin">
                                    <h4 class="subheading2">(Cost of development and on-going operations )
										</h4>
                                    <asp:TextBox runat="server" ID="txtcostprojection" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.8</span>
                                    Funding Status</h2>
                                <p class="sidemargin subheading2">(List out in detail (i) all grants and funding received/to be received from other publicly and/or privately funded organizations/ programmes which the applicant (or companies established by he/she/it) has applied for, will receive or will be entitled to receive in the coming 18 months, or have received in the past 18 months; (ii) the nature of expenditure covered/to be covered by such funding sources; and (iii) the amount and the maximum amount received/to be received under such funding sources ) </p>

                                <div class="form-group sidemargin">
                                    <asp:GridView ID="Grd_FundingStatus" runat="server" ShowFooter="True" AutoGenerateColumns="False"
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


                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.9</span>
                                    Exit Strategy <small>(If applicable):</small></h2>

                                <div class="form-group sidemargin">
                                    <asp:TextBox runat="server" ID="txtexitstrategy" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>


                            <div class="form-group">
                                <h2 class="subheading text-left">
                                    <span>4.10</span>
                                    Additional Information <small>(If applicable):</small></h2>

                                <div class="form-group sidemargin">
                                    <asp:TextBox runat="server" ID="txtaddinformation" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep5" Visible="false" runat="server">
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
                                           
                                            <asp:DropDownList ID="Salutation" runat="server">
                                                <asp:ListItem Text="DR" Value=""></asp:ListItem>
                                                <asp:ListItem Text="DR2" Value=""></asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Last Name</label>
                                            </p>
                                            <asp:TextBox ID="txtContactLast_name" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <p>
                                                <label>First Name</label>
                                            </p>
                                            <asp:TextBox ID="txtContactFirst_name" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <p>
                                            <label>Position</label>
                                        </p>
                                        <asp:TextBox ID="txtContactPostition" runat="server" CssClass="input-sm"></asp:TextBox>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <p>
                                                <label>Contact No. (Home)</label>
                                            </p>
                                            <asp:TextBox ID="txtContactNoHome" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Contact No. (Office)</label>
                                            </p>
                                            <asp:TextBox ID="txtContactNoOffice" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <p>
                                                <label>Contact No. (Mobile)</label>
                                            </p>
                                            <asp:TextBox ID="txtContactNoMobile" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <p>
                                                <label>Fax</label>
                                            </p>
                                            <asp:TextBox ID="txtContactFax" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Email</label>
                                            </p>
                                            <asp:TextBox ID="txtContactEmail" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                        <p>
                                            <label>Mailing Address</label>
                                        </p>
                                        <asp:TextBox ID="txtContactAddress" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
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
                                                <asp:TextBox ID="txtContactLast_name1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <p>
                                                    <label>First Name</label>
                                                </p>
                                                <asp:TextBox ID="txtContactFirst_name1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <p>
                                                <label>Position</label>
                                            </p>
                                            <asp:TextBox ID="txtContactPostition1" runat="server" CssClass="input-sm"></asp:TextBox>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Contact No. (Home)</label>
                                                </p>
                                                <asp:TextBox ID="txtContactNoHome1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Contact No. (Office)</label>
                                                </p>
                                                <asp:TextBox ID="txtContactNoOffice1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <p>
                                                    <label>Contact No. (Mobile)</label>
                                                </p>
                                                <asp:TextBox ID="txtContactNoMobile1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Fax</label>
                                                </p>
                                                <asp:TextBox ID="txtContactFax1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <p>
                                                    <label>Email</label>
                                                </p>
                                                <asp:TextBox ID="txtContactEmail1" runat="server" CssClass="input-sm"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <p>
                                                <label>Mailing Address</label>
                                            </p>
                                            <asp:TextBox ID="txtContactAddress1" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_IncubationStep6" Visible="false" runat="server">
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
                <div class="form-group">
                    <h2 class="subheading text-center" style="margin: 20px 0;">DECLARATION</h2>

                    <div class="form-group green-clr-theme">
                        <asp:CheckBox ID="chkDeclaration" runat="server" />
                        In consideration of Hong Kong Cyberport Management Company Limited (“HKCMCL”) considering and processing our application for funding from CCMF, we confirm our agreement and undertake the following by checking this box
                    </div>

                    <p class="form-group">We agree with all the terms and conditions set out in the CCMF Guides and Notes for the Applicants (ENC.RF.O15) governing the application of the Cyberport Creative Micro Fund Scheme.</p>

                    <p class="form-group">All information provided in this application as well as the accompanying information is true, not misleading in any manner and reflects the status of affairs as at the date of submission. I shall inform the Administration of the Cyberport Creative Micro Fund immediately if there are any changes to the above information. </p>

                    <p class="form-group">We are the creators of our proposed project the details of which have been described in Section 3 of this CCMF Application Form ("Project") and our Project is original. To the best of our knowledge, the Project and the product(s) and/or service(s) to be developed as a part of the Project are not under development or available anywhere in the world. </p>

                    <p class="form-group">The development, completion and use of the Project and the product(s) and/or service(s) to be developed as a part of the Project will not infringe the rights or intellectual property rights of any other party (whether registered or not), including but not limited to patent rights, trade mark rights, and copyright. </p>

                    <p class="form-group">Other than the funding programme(s) specified in Section 4.8 of this Application Form, we (whether individually or as part of any team) have not applied for or received any other grant or funding from any publicly and/or privately funded organisations/programmes for the Project in the past 18 months and we are not applying for nor will we be entitled to receive any other grant or funding from any publicly and/or privately funded organisations/programmes for the Project in the future 18 months. We will notify Cyberport immediately if we apply for or become entitled to any grant or funding from any publicly and/or privately funded organisations/programmes for the Project and when requested, provide evidence of payments made or received in respect of such programmes. </p>

                    <p class="form-group">We will use any grant to be received from the CCMF (if any) for the sole purpose of developing and completing the Project. We will not give away, share or othen/vise use any grant to be received from the CCMF in contravention of the law or any of the terms which apply to application for funding from the CCMF. </p>

                    <p class="form-group">We understand the information provided in this application will be disclosed to Cyberport staff for screening purpose. No Confidential Agreement of any kind will be signed with Cyberport or the relevant staff on any occasion. </p>

                    <p class="form-group">We give our consent to Cyberport staff to carry out necessary due diligence/reference check for the purpose of assessing this application. </p>

                    <p class="form-group">We acknowledge that HKCMCL has absolute discretion to revoke this application and call for refund of the full amount of grant received by the applicant if any member of the team breaches any of the terms and conditions in the CCMF Guides and Notes for the Applicants (ENC.RF.O15), this CCMF Application Form and any other relevant agreement(s) between us and HKCMCL. If any of our conduct amounts to or may amount to a breach of criminal law, HKCMCL would report our conduct to the relevant law enforcement body. </p>
                </div>


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
                <h2 class="subheading text-center" style="margin: 20px 0;">PERSONAL INFORMATION COLLECTION STATEMENT
</h2>

                <p class="form-group">This Personal Information Collection Statement of Hong Kong Cyberport Management Company Limited ("us" or "we") applies to the personal data collected in this CCMF Application Form ("Personal Data"). </p>

                <p class="form-group bold">Purpose and Use of Your Data</p>

                <ul class="form-group bult">
                    <li>The Personal Data will be used for one or more of the following purposes:</li>
                    <li>processing your application;</li>
                    <li>communicating with you if we have any/or question and update about your application;</li>
                    <li>providing updates, services, products, facilities or activities and matters relating to our administration, management, operation and maintenance; </li>
                    <li>designing, reviewing, evaluating and enhancing our services, products, facilities or activities (including conducting research, survey and analysis for those purposes);</li>
                    <li>direct marketing (please see "Direct Marketing" section below); and</li>
                    <li>handling complaints, legal, safety and security purposes.</li>
                </ul>

                <p class="form-group">It is mandatory to provide the requested personal data, or we will not be able to process your application.</p>

                                <p class="form-group bold">Transfer of Your Data</p>
                <p class="form-group">Your Data will not generally be disclosed or transferred to any other party in a form that would identify you, except in the following circumstances: </p>

                <ul class="form-group  bult">
                    <li>If we use third party suppliers or service providers who provide services to support our business operation or facilitate provision of our services, products, facilities or activities, we may provide Your Data to these suppliers or service providers. </li>
                    <li>We may disclose Your Data to any governmental or law enforcement agency, judicial body or regulatory authority to whom we are required to make disclosure according to any law, regulation, court order, direction, code or guideline. </li>
                    <li>In addition, Your Data may be accessed by, disclosed or transferred to any person under a duty of confidentiality to us (including our accountants, auditors, legal advisers or other professional advisers) for the purposes described in the "Purpose and Use of Your Data" section above. </li>
                </ul>


                <p class="form-group bold">Direct Marketing</p>
                <p class="form-group">In connection with direct marketing, we intend to use your name and email address to market and promote the services, products, facilities, activities, contests, conferences, lucky draws, events and promotional campaigns from time to time available relating to us or "Cyberport", or shops or merchants or hotel in the "Cyberport" or offered or hosted at "Cyberport" (including consumer goods, food and beverages, books and stationery, children goods and services, fashion and accessories, optical products, watches, jewellery, luxurious goods, personal care, health and beauty products and services, furniture and lifestyle goods, home furnishing and appliances, florists, sports gear and products, electronic products and appliances, banking and financial services, cinemas and theatres, cultural, entertainment facilities and activities, charity activities, leisure activities and performances, gift redemptions and car parking facilities) </p>

                <p class="form-group">“Cyberport” means the community or development located at Telegraph Bay, Pokfulam including its offices, centres and arcade. </p>
                <p class="form-group">We cannot use Your Data for direct marketing unless we have received your consent or indication of no objection. You may write to our Data Protection Officer or contact us via the contact information provided in our direct marketing materials to opt out from direct marketing at any time. </p>

                <p class="form-group bold">Our Privacy Policy and Practices</p>
                <p class="form-group">Please see our Privacy Policy Statement at http://www.cyberport.hklenlprivacy_policy for our general policy and practices in respect of our collection and use of personal data. </p>

                <p class="form-group bold">Access to and Correction of Your Data</p>
                <p class="form-group">You have the right to request access to, and correction of, Your Data held by us. We may charge a reasonable fee for administering and processing your data access request. If you need to check whether we hold Your Data or if you wish to have access to, correct any of Your Data which is inaccurate, please write via e-mail to our Data Protection Officer at dpo@cyberport.hk or via mail to Units 1102-04, Level 11, Cyberport 2, 100 Cyberport Road, Hong Kong. </p>

                <p class="form-group">This Statement is written in the English language and may be translated into other languages. In the event of any inconsistency between the English version and the translated version of this Statement, the English version shall prevail. </p>


                <div class="green-clr-theme">
                    <asp:CheckBox runat="server" ID="Personal_Information" />
                    l have read the Personal Information Collection Statement above.
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
                    <asp:Button ID="btn_Submit" Visible="false" OnClick="btn_Submit_Click1" runat="server" Text="Submit" CssClass="btnSubmitIncubation apply-btn skytheme" />
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


<asp:Panel ID="IncubationSubmitPopup" runat="server">
    <div class="popup-overlay"></div>
    <div class="popup IncubationSubmitPopup">
        <div class="pos-relative card-theme full-width">
            <p class="popup--para">
                Your submission for intake 201612, deadline is
                <asp:Label ID="lblDeadlinePopup" runat="server" />.
                	Please input your login email and password for confirmation.
               
            </p>

            <div class="table full-width">
                <div class="form-group">
                    <asp:TextBox CssClass="input" placeholder="Email" ID="txtLoginUserName" runat="server"></asp:TextBox>
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
</asp:Panel>--%>
