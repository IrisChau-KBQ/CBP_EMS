using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data.Models;
using System.Linq;
using System.Collections.Generic;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Common;
using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using System.Threading;
using System.Web;
using Microsoft.ApplicationServer.Caching;
using System.Web.Configuration;


namespace CBP_EMS_SP.IncubationFormsWebPart.IncubationFormInstructions
{
    [ToolboxItemAttribute(false)]
    public partial class IncubationFormInstructions : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]



        public IncubationFormInstructions()
        {
        }
        private string defaultNationality = "Hong Kong";
        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            InitializeControl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);

            //lblQ19.Text = SPFunctions.LocalizeUI("Step_1_Q9", "CyberportEMS_Incubation");//Localize("Step_1_Q9");
            //lblQ15.Text = SPFunctions.LocalizeUI("Step_1_Q5", "CyberportEMS_Incubation");//Localize("Step_1_Q5");

            string ctrlname = HttpContext.Current.Request.Params.Get("__EVENTTARGET");
            if (!Page.IsPostBack && string.IsNullOrEmpty(ctrlname))
            {



                if (Context.Request.UrlReferrer != null)
                {
                    btn_Back.PostBackUrl = Context.Request.UrlReferrer.ToString();

                }

                if (string.IsNullOrEmpty(Context.Request.QueryString["prog"]) || string.IsNullOrEmpty(Context.Request.QueryString["app"]))
                {
                    Context.Response.Redirect("~/SitePages/MyApplications.aspx");
                }
                else
                {
                    IncubationSubmitPopup.Visible = false;
                    FillIntakeDetails();
                }


            }
            string CompanyTypeSelected = rbtnCompany_Type.SelectedValue;

            List<ListItem> Companytype = new List<ListItem>();
            Companytype.Add(new ListItem() { Value = "private", Text = Localize("step_2_Unlimited") });
            Companytype.Add(new ListItem() { Value = "Limited", Text = Localize("Step_2_Limited") });
            Companytype.Add(new ListItem() { Value = "Public", Text = Localize("Step_2_Publicly_listed") });
            Companytype.Add(new ListItem() { Value = "Others", Text = Localize("Step2_Others") });
            rbtnCompany_Type.DataSource = Companytype;
            rbtnCompany_Type.DataTextField = "Text";
            rbtnCompany_Type.DataValueField = "Value";
            rbtnCompany_Type.DataBind();
            if (!string.IsNullOrEmpty(CompanyTypeSelected))
            {
                rbtnCompany_Type.SelectedValue = CompanyTypeSelected;
                if (CompanyTypeSelected.ToLower() == "others")
                {
                    txtOther_Company_Type.Visible = true;

                }
            }

            CompanyTypeSelected = rbtnPreferred_Track.SelectedValue;
            List<ListItem> Sites = new List<ListItem>();
            Sites.Add(new ListItem() { Value = "On-site incubation", Text = Localize("Step_2_On_site_incubation") });
            Sites.Add(new ListItem() { Value = "Off-site incubation", Text = Localize("Step_2_Off_site_incubation") });

            rbtnPreferred_Track.DataSource = Sites;
            rbtnPreferred_Track.DataTextField = "Text";
            rbtnPreferred_Track.DataValueField = "Value";
            rbtnPreferred_Track.DataBind();
            if (!string.IsNullOrEmpty(CompanyTypeSelected))
            {
                rbtnPreferred_Track.SelectedValue = CompanyTypeSelected;
            }

            CompanyTypeSelected = rbtnList_Business_Area.SelectedValue;
            List<ListItem> busniessareaoption = new List<ListItem>();

            busniessareaoption.Add(new ListItem() { Value = "AI / Big Data", Text = Localize("Step2_OpenData") });
            busniessareaoption.Add(new ListItem() { Value = "Application Development", Text = Localize("Step2_AppDesign") });
            busniessareaoption.Add(new ListItem() { Value = "Blockchain", Text = Localize("Step2_Blockchain") });
            busniessareaoption.Add(new ListItem() { Value = "Cybersecurity", Text = Localize("Step2_Cybersecurity") });
            busniessareaoption.Add(new ListItem() { Value = "EdTech", Text = Localize("Step2_Edutech") });
            busniessareaoption.Add(new ListItem() { Value = "EnvironmenTech", Text = Localize("Step2_EnvironmenTech") });
            busniessareaoption.Add(new ListItem() { Value = "E-sport/Digital Entertainment", Text = Localize("Step2_Gaming") });
            busniessareaoption.Add(new ListItem() { Value = "FinTech", Text = Localize("Step2_Fintech") });
            busniessareaoption.Add(new ListItem() { Value = "HealthTech", Text = Localize("Step2_Healthcare") });
            busniessareaoption.Add(new ListItem() { Value = "MarTech", Text = Localize("Step2_MarTech") });
            busniessareaoption.Add(new ListItem() { Value = "RetailTech / E-commerce", Text = Localize("Step2_ECommerce") });
            busniessareaoption.Add(new ListItem() { Value = "Robotics / IoT", Text = Localize("Step2_Wearable") });
            busniessareaoption.Add(new ListItem() { Value = "Smart Building", Text = Localize("Step2_Smart_Building") });
            busniessareaoption.Add(new ListItem() { Value = "Smart Mobility", Text = Localize("Step2_Smart_Mobility") });
            busniessareaoption.Add(new ListItem() { Value = "Others", Text = Localize("Step2_Others") });

            rbtnList_Business_Area.DataSource = busniessareaoption;
            rbtnList_Business_Area.DataTextField = "Text";
            rbtnList_Business_Area.DataValueField = "Value";
            rbtnList_Business_Area.DataBind();
            if (!string.IsNullOrEmpty(CompanyTypeSelected))
            {
                rbtnList_Business_Area.SelectedValue = CompanyTypeSelected;
                if (CompanyTypeSelected.ToLower() == "others")
                {
                    txtOther_Bussiness_Area.Visible = true;
                }
            }

            string Positioning = string.Empty;
            foreach (ListItem objList in chkPositioning.Items)
            {
                if (objList.Selected)
                {
                    Positioning += objList.Value.Trim() + ";";
                }
            }

            List<ListItem> positioning = new List<ListItem>();
            positioning.Add(new ListItem() { Value = "Content creation", Text = Localize("Step_2_Content_creation") });
            positioning.Add(new ListItem() { Value = "Production / post-production", Text = Localize("tep_2_Production") });
            positioning.Add(new ListItem() { Value = "Publishing / distribution / delivery", Text = Localize("Step_2_Publslishing") });
            positioning.Add(new ListItem() { Value = "Platform / device development", Text = Localize("Step_2_Platform") });
            positioning.Add(new ListItem() { Value = "Management / trading / service", Text = Localize("Step_2_Management") });
            positioning.Add(new ListItem() { Value = "Others", Text = Localize("Step2_Others") });
            chkPositioning.DataSource = positioning;
            chkPositioning.DataTextField = "Text";
            chkPositioning.DataValueField = "Value";
            chkPositioning.DataBind();
            if (!string.IsNullOrEmpty(Positioning))
            {
                string[] Positionings = Positioning.Split(';');
                foreach (string strPos in Positionings.Where(x => !string.IsNullOrEmpty(x)))
                {
                    foreach (ListItem items in chkPositioning.Items)
                    {
                        if (items.Value.ToLower() == strPos.Trim().ToLower())
                        {
                            items.Selected = true;

                            if (items.Value.ToLower() == "others")
                            {
                                txtPositioningOther.Visible = true;
                            }
                            if (items.Value.Contains("trading"))
                            {
                                txtManagementOther.Visible = true;
                            }
                        }
                    }
                }
            }

        }
        protected void FillIntakeDetails()
        {
            try
            {
                int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
                Guid objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objSp = new SPFunctions();
                using (var Intake = new CyberportEMS_EDM())
                {
                    TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                    if (objProgram == null)
                    {
                        ShowbottomMessage("No Intake program found selected", false);
                    }
                    else
                    {
                        lblIntake.Text = objProgram.Intake_Number.ToString();
                        lblDeadline.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
                        //lblDeadlinePopup.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
                        lblDeadlinePopup.Text = HttpUtility.HtmlEncode(dbTextbyLanguage(objProgram.Application_Deadline_Eng, objProgram.Application_Deadline_SimpChin, objProgram.Application_Deadline_TradChin));//objProgram.Application_Deadline.ToString("dd MMM yyyy");

                        hdn_ProgramID.Value = objProgram.Programme_ID.ToString();

                    }

                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(Intake, progId);
                    CBPCommonConstants oCommonFunction = new CBPCommonConstants();
                    ddlCountryOrigin.DataSource = oCommonFunction.GetNationalityList;
                    ddlCountryOrigin.DataBind();
                    ddlCountryOrigin.SelectedValue = defaultNationality;

                    //Intake.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

                    if (objIncubation != null)
                    {
                        bool isDisabled = false;
                        //if (objProgram.Application_Deadline <= DateTime.Now && (objIncubation.Status != formsubmitaction.Waiting_for_response_from_applicant.ToString() || objIncubation.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                        if (objProgram.Application_Deadline <= DateTime.Now && (objIncubation.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                        {
                            isDisabled = true;
                            DisableControls();
                            if (objSp.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"], true))
                            { btn_Back.Visible = true; }
                        }
                        if (objIncubation.Status.Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " "))
                        {
                            btn_StepSave.Visible = false;
                        }
                        /*else if (objIncubation.Submission_Date.HasValue)
                        {
                            if (HttpContext.Current.Request.QueryString["resubmitversion"] != null && HttpContext.Current.Request.QueryString["resubmitversion"].ToString() == "Y")
                            {
                            ReSubmitVersionCopy();
                            objIncubation = GetExistingIncubation(Intake, progId);
                        }
                        }*/
                        lblApplicant.Text = objIncubation.Created_By;
                        hdn_ApplicationID.Value = objIncubation.Incubation_ID.ToString();
                        if (objIncubation.Last_Submitted != null)
                            lblLastSubmitted.Text = objIncubation.Last_Submitted.ToString("dd MMM yyyy");
                        lblApplicationNo.Text = HttpUtility.HtmlEncode(objIncubation.Application_Number);
                        rbtnPanel1Q1.SelectedValue = Convert.ToString(objIncubation.Question1_1);
                        rbtnPanel1Q2.SelectedValue = Convert.ToString(objIncubation.Question1_2);
                        rbtnPanel1Q3.SelectedValue = Convert.ToString(objIncubation.Question1_3);
                        rbtnPanel1Q4.SelectedValue = Convert.ToString(objIncubation.Question1_4);
                        rbtnPanel1Q5.SelectedValue = Convert.ToString(objIncubation.Question1_5);
                        rbtnPanel1Q6.SelectedValue = Convert.ToString(objIncubation.Question1_6);
                        rbtnPanel1Q7.SelectedValue = Convert.ToString(objIncubation.Question1_7);
                        rbtnPanel1Q8.SelectedValue = Convert.ToString(objIncubation.Question1_8);
                        //rbtnPanel1Q8_1.SelectedValue = Convert.ToString(objIncubation.Question1_8_1);
                        rbtnPanel1Q9.SelectedValue = Convert.ToString(objIncubation.Question1_9);
                        //rbtnPanel1Q10.SelectedValue = Convert.ToString(objIncubation.Question1_10);
                        //if (Convert.ToString(objIncubation.Question1_4) != "")
                        //{
                        //    if (objIncubation.Question1_4 == false)
                        //    {
                        //        lblQ15.Enabled = true;
                        //        lblQ15td.Enabled = true;
                        //        rbtnPanel1Q5.Enabled = true;
                        //    }
                        //}
                        //if (Convert.ToString(objIncubation.Question1_8) != "")
                        //{
                        //    if (objIncubation.Question1_8 == true)
                        //    {
                        //        lblQ19.Enabled = true;
                        //        lblQ19td.Enabled = true;

                        //        rbtnPanel1Q9.Enabled = true;
                        //    }
                        //}
                        txtCompanyNameEnglish.Text = objIncubation.Company_Name_Eng;
                        lblCompanyNameEnglish.Text = objIncubation.Company_Name_Eng;
                        txtCompanyNameChinese.Text = objIncubation.Company_Name_Chi;
                        lblCompanyNameChinese.Text = objIncubation.Company_Name_Chi;
                        txtAbstractEnglish.Text = objIncubation.Abstract;
                        lblAbstractEnglish.Text = string.IsNullOrEmpty(objIncubation.Abstract) ? "" : objIncubation.Abstract.Replace(Environment.NewLine, "<br>");
                        txtObjective.Text = objIncubation.Objective;
                        lblObjective.Text = string.IsNullOrEmpty(objIncubation.Objective) ? "" : objIncubation.Objective.Replace(Environment.NewLine, "<br>");
                        txtbackground.Text = objIncubation.Background;
                        lblbackground.Text = string.IsNullOrEmpty(objIncubation.Background) ? "" : objIncubation.Background.Replace(Environment.NewLine, "<br>");
                        txtPilot_Work_Done.Text = objIncubation.Pilot_Work_Done;
                        lblPilot_Work_Done.Text = string.IsNullOrEmpty(objIncubation.Pilot_Work_Done) ? "" : objIncubation.Pilot_Work_Done.Replace(Environment.NewLine, "<br>");
                        TxtAbstractChinese.Text = objIncubation.Abstract_Chi;
                        lblAbstractChinese.Text = string.IsNullOrEmpty(objIncubation.Abstract_Chi) ? "" : objIncubation.Abstract_Chi.Replace(Environment.NewLine, "<br>");
                        txtAdditionalInformation.Text = objIncubation.Additional_Information;
                        lblAdditionalInformation.Text = string.IsNullOrEmpty(objIncubation.Additional_Information) ? "" : objIncubation.Additional_Information.Replace(Environment.NewLine, "<br>");
                        txtProposedProducts.Text = objIncubation.Proposed_Products;
                        lblProposedProducts.Text = string.IsNullOrEmpty(objIncubation.Proposed_Products) ? "" : objIncubation.Proposed_Products.Replace(Environment.NewLine, "<br>");
                        txtTargetMarket.Text = objIncubation.Target_Market;
                        lblTargetMarket.Text = string.IsNullOrEmpty(objIncubation.Target_Market) ? "" : objIncubation.Target_Market.Replace(Environment.NewLine, "<br>");
                        txtCompetitionAnalysis.Text = objIncubation.Competition_Analysis;
                        lblCompetitionAnalysis.Text = string.IsNullOrEmpty(objIncubation.Competition_Analysis) ? "" : objIncubation.Competition_Analysis.Replace(Environment.NewLine, "<br>");
                        txtRevenueModel.Text = objIncubation.Revenus_Model;
                        lblRevenueModel.Text = string.IsNullOrEmpty(objIncubation.Revenus_Model) ? "" : objIncubation.Revenus_Model.Replace(Environment.NewLine, "<br>");
                        txtFirst6Months.Text = objIncubation.First_6_Months_Milestone;
                        txtSecond6Months.Text = objIncubation.Second_6_Months_Milestone;
                        txtThird6Months.Text = objIncubation.Third_6_Months_Milestone;
                        txtForth6Months.Text = objIncubation.Forth_6_Months_Milestone;
                        lblFirst6Months.Text = string.IsNullOrEmpty(objIncubation.First_6_Months_Milestone) ? "" : objIncubation.First_6_Months_Milestone.Replace(Environment.NewLine, "<br>");
                        lblSecond6Months.Text = string.IsNullOrEmpty(objIncubation.Second_6_Months_Milestone) ? "" : objIncubation.Second_6_Months_Milestone.Replace(Environment.NewLine, "<br>");
                        lblThird6Months.Text = string.IsNullOrEmpty(objIncubation.Third_6_Months_Milestone) ? "" : objIncubation.Third_6_Months_Milestone.Replace(Environment.NewLine, "<br>");
                        lblForth6Months.Text = string.IsNullOrEmpty(objIncubation.Forth_6_Months_Milestone) ? "" : objIncubation.Forth_6_Months_Milestone.Replace(Environment.NewLine, "<br>");
                        txtExitStrategy.Text = objIncubation.Exit_Strategy;
                        lblExitStrategy.Text = string.IsNullOrEmpty(objIncubation.Exit_Strategy) ? "" : objIncubation.Exit_Strategy.Replace(Environment.NewLine, "<br>"); ;
                        if (objIncubation.Resubmission != null)
                            rbtnResubmission.SelectedValue = Convert.ToString(objIncubation.Resubmission);
                        //commented on 26102018 cpip changes
                        // txtResubmission_Project_Reference.Text = objIncubation.Resubmission_Project_Reference;
                        //  lblResubmission_Project_Reference.Text = string.IsNullOrEmpty(objIncubation.Resubmission_Project_Reference) ? "" : objIncubation.Resubmission_Project_Reference.Replace(Environment.NewLine, "<br>");
                        txtResubmission_Main_Differences.Text = objIncubation.Resubmission_Main_Differences;
                        lblResubmission_Main_Differences.Text = string.IsNullOrEmpty(objIncubation.Resubmission_Main_Differences) ? "" : objIncubation.Resubmission_Main_Differences.Replace(Environment.NewLine, "<br>");

                        if (!string.IsNullOrEmpty(objIncubation.Company_Type.Trim()))
                        {
                            rbtnCompany_Type.SelectedValue = objIncubation.Company_Type.Trim();

                            if (objIncubation.Company_Type.ToLower() == "others")
                            {
                                txtOther_Company_Type.Text = objIncubation.Other_Company_Type;
                                lblOther_Company_Type.Text = objIncubation.Other_Company_Type;
                                txtOther_Company_Type.Visible = true;
                                txtOther_Company_Type.Style.Add("display", "inline-block");
                                if (isDisabled)
                                {
                                    txtOther_Company_Type.Visible = false;
                                    lblOther_Company_Type.Visible = true;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(objIncubation.Business_Area))
                        {

                            if (objIncubation.Business_Area.Trim().ToLower() == "open data")
                            {
                                //rbtnList_Business_Area.SelectedIndex = 0;
                                rbtnList_Business_Area.SelectedValue = "AI / Big Data";
                            }
                            else
                            {
                                rbtnList_Business_Area.SelectedValue = objIncubation.Business_Area.Trim();
                            }
                            //rbtnList_Business_Area.SelectedValue = objIncubation.Business_Area.Trim();
                        }
                        if (!string.IsNullOrEmpty(objIncubation.Business_Area))
                        {
                            if (objIncubation.Business_Area.ToLower() == "others")
                            {
                                txtOther_Bussiness_Area.Text = objIncubation.Other_Bussiness_Area;
                                lblOther_Bussiness_Area.Text = objIncubation.Other_Bussiness_Area;
                                txtOther_Bussiness_Area.Visible = true;
                                txtOther_Bussiness_Area.Style.Add("display", "inline-block");
                                if (isDisabled)
                                {
                                    txtOther_Bussiness_Area.Visible = false;
                                    lblOther_Bussiness_Area.Visible = true;
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(objIncubation.Positioning))
                        {
                            string[] Positioning = objIncubation.Positioning.Split(';');
                            foreach (string strPos in Positioning.Where(x => !string.IsNullOrEmpty(x)))
                            {
                                foreach (ListItem items in chkPositioning.Items)
                                {
                                    if (items.Value.ToLower() == strPos.Trim().ToLower())
                                    {
                                        items.Selected = true;

                                        if (items.Value.ToLower() == "others")
                                        {
                                            txtPositioningOther.Text = objIncubation.Other_Positioning;
                                            lblPositioningOther.Text = objIncubation.Other_Positioning;
                                            txtPositioningOther.Visible = true;
                                            txtPositioningOther.Style.Add("display", "inline-block");
                                            if (isDisabled)
                                            {
                                                txtPositioningOther.Visible = false;
                                                lblDisPositioningOther.Visible = true;
                                                lblDisPositioningOther.Text = Localize("Step2_Others");
                                                lblPositioningOther.Visible = true;
                                            }
                                        }
                                        if (items.Value.Contains("trading"))
                                        {
                                            txtManagementOther.Text = objIncubation.Management_Positioning;
                                            lblManagementOther.Text = objIncubation.Management_Positioning;
                                            txtManagementOther.Visible = true;
                                            txtManagementOther.Style.Add("display", "inline-block");
                                            if (isDisabled)
                                            {
                                                txtManagementOther.Visible = false;
                                                lblDisManagementOther.Visible = true;
                                                lblDisManagementOther.Text = Localize("Step_2_Management") + ":";
                                                lblManagementOther.Visible = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        txtPositioningOther.Text = objIncubation.Other_Positioning;
                        txtManagementOther.Text = objIncubation.Management_Positioning;

                        txtOtherAttributes.Text = objIncubation.Other_Attributes;
                        lblOtherAttributes.Text = string.IsNullOrEmpty(objIncubation.Other_Attributes) ? "" : objIncubation.Other_Attributes.Replace(Environment.NewLine, "<br>");
                        if (!string.IsNullOrEmpty(objIncubation.Preferred_Track))
                            rbtnPreferred_Track.SelectedValue = objIncubation.Preferred_Track;
                        txtCompany_Ownership_1.Text = objIncubation.Company_Ownership_Structure;
                        lblCompany_Ownership_1.Text = string.IsNullOrEmpty(objIncubation.Company_Ownership_Structure) ? "" : objIncubation.Company_Ownership_Structure.Replace(Environment.NewLine, "<br>");
                        txtPartner_Profiles.Text = objIncubation.Major_Partners_Profiles;
                        lblPartner_Profiles.Text = string.IsNullOrEmpty(objIncubation.Major_Partners_Profiles) ? "" : objIncubation.Major_Partners_Profiles.Replace(Environment.NewLine, "<br>");
                        Manpower_Distribution.Text = objIncubation.Manpower_Distribution;
                        lblManpower_Distribution.Text = string.IsNullOrEmpty(objIncubation.Manpower_Distribution) ? "" : objIncubation.Manpower_Distribution.Replace(Environment.NewLine, "<br>");
                        Equipment_Distribution.Text = objIncubation.Equipment_Distribution;
                        lblEquipment_Distribution.Text = string.IsNullOrEmpty(objIncubation.Equipment_Distribution) ? "" : objIncubation.Equipment_Distribution.Replace(Environment.NewLine, "<br>");
                        Other_Costs.Text = objIncubation.Other_Direct_Costs;
                        lblOther_Costs.Text = string.IsNullOrEmpty(objIncubation.Other_Direct_Costs) ? "" : objIncubation.Other_Direct_Costs.Replace(Environment.NewLine, "<br>");
                        Forecast_Income.Text = objIncubation.Forecast_Income;
                        lblForecast_Income.Text = string.IsNullOrEmpty(objIncubation.Forecast_Income) ? "" : objIncubation.Forecast_Income.Replace(Environment.NewLine, "<br>");
                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Declaration)))
                            chkDeclaration.Checked = Convert.ToBoolean(objIncubation.Declaration);
                        txtName_PrincipalApplicant.Text = objIncubation.Principal_Full_Name;
                        lblName_PrincipalApplicant.Text = objIncubation.Principal_Full_Name;
                        txtPosition_PrincipalApplicant.Text = objIncubation.Principal_Position_Title;
                        lblPosition_PrincipalApplicant.Text = objIncubation.Principal_Position_Title;
                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Marketing_Information)))
                            Marketing_Information.Checked = Convert.ToBoolean(objIncubation.Marketing_Information);
                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Have_Read_Statement)))
                            Personal_Information.Checked = Convert.ToBoolean(objIncubation.Have_Read_Statement);

                        //txtProjectName.Text = lblProjectName.Text = objIncubation.Project_Name;


                        if (ddlCountryOrigin.Items.FindByValue(objIncubation.Country_Of_Origin) != null)
                        {
                            ddlCountryOrigin.SelectedValue = objIncubation.Country_Of_Origin.Trim();
                        }
                        else
                        {
                            ddlCountryOrigin.SelectedValue = defaultNationality;
                        }

                        txtWebsiteName.Text = lblWebsiteName.Text = objIncubation.Website;
                        txtestablishmentyear.Text = lblestablishmentyear.Text = objIncubation.Establishment_Year.HasValue ? objIncubation.Establishment_Year.Value.ToString("MMM-yyyy") : "";
                        //if (objIncubation.NEW_to_HK.HasValue)
                        //    rdbNEWHK.SelectedValue = objIncubation.NEW_to_HK.Value.ToString();

                        //List<TB_APPLICATION_CONTACT_DETAIL> objCoreMembers = IncubationContext.APPLICATION_CONTACT_DETAIL_GET(Guid.Parse(hdn_ApplicationID.Value));
                        //foreach(TB_APPLICATION_CONTACT_DETAIL item in objCoreMembers)
                        //{

                        //    txtContactLast_name.Text = item.Last_Name_Eng;
                        //    txtContactFirst_name.Text = item.First_Name_Eng;
                        //    txtContactPostition.Text = item.Position;
                        //    txtContactNoHome.Text = item.Contact_No_Home;
                        //    txtContactNoOffice.Text = item.Contact_No_Office;
                        //    txtContactNoMobile.Text = item.Contact_No_Mobile;
                        //    txtContactFax.Text = Convert.ToString(item.Fax);
                        //    txtContactEmail.Text = item.Email;
                        //    txtContactAddress.Text = item.Mailing_Address;

                        //    Salutation.SelectedValue = item.Salutation;

                        //}
                        InitializeFundingStatus();
                        InitialCoreMembers();
                        FillContact();
                        InitializeUploadsDocument();
                        if (isDisabled)
                        {
                            DisableListTextbox();
                        }
                        //FillContact();



                    }
                    else if (objProgram.Application_Deadline >= DateTime.Now && objProgram.Application_Start <= DateTime.Now)
                    {
                        lblApplicant.Text = objSp.GetCurrentUser();
                        hdn_ApplicationID.Value = "";
                        InitializeFundingStatus();
                        InitialCoreMembers();
                        FillContact();
                        InitializeUploadsDocument();

                        //int count = (Intake.TB_INCUBATION_APPLICATION.Where(x=>x.Programme_ID== progId).Count() + 1);
                        int count = 0;
                        var result = Intake.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == progId).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                        if (result != null)
                        {
                            count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                        }
                        else
                        {
                            count = 1;
                        }
                        lblApplicationNo.Text = HttpUtility.HtmlEncode(Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));

                    }
                    else
                    {
                        DisableControls();
                        pnlRestricted.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);
            }
        }

        protected void FillContact()
        {

            List<TB_APPLICATION_CONTACT_DETAIL> ObjContactDetails = new List<TB_APPLICATION_CONTACT_DETAIL>();
            if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                ObjContactDetails = IncubationContext.APPLICATION_CONTACT_DETAIL_GET(Guid.Parse(hdn_ApplicationID.Value));
            if (ObjContactDetails.Count == 0)
            {
                ObjContactDetails = new List<TB_APPLICATION_CONTACT_DETAIL>();
                ObjContactDetails.Add(new TB_APPLICATION_CONTACT_DETAIL() { CONTACT_DETAILS_ID = 0 });

            }

            gv_CONTACT_DETAIL.DataSource = ObjContactDetails;
            gv_CONTACT_DETAIL.DataBind();
        }
        protected void DisableControls()
        {
            fuOtherAttachement.Enabled = false;
            fuPresentationSlide.Enabled = false;
            fu_AnnualReturn.Enabled = false;
            fu_BrCopy.Enabled = false;
            fu_Company_Ownership_2.Enabled = false;
            btnannual.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnotherattach.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnpresenattion.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btncompanyownership.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnbr.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnannual.Enabled = false;
            btnotherattach.Enabled = false;
            btnpresenattion.Enabled = false;
            btncompanyownership.Enabled = false;
            btnbr.Enabled = false;
            pnl_IncubationStep1.Enabled = false;
            pnl_IncubationStep1.Enabled = false;
            pnl_IncubationStep2.Enabled = false;
            pnl_IncubationStep3.Enabled = false;
            pnl_IncubationStep4.Enabled = false;
            pnl_IncubationStep5.Enabled = false;
            btn_Submit.Enabled = false;
            btn_Submit.Visible = false;
            btn_submitFinal.Visible = false;
            btn_StepSave.Visible = false;
            ShowLabels();
        }

        private void ShowLabels()
        {
            txtCompanyNameEnglish.Visible = false;
            lblCompanyNameEnglish.Visible = true;
            txtCompanyNameChinese.Visible = false;
            lblCompanyNameChinese.Visible = true;
            txtAbstractEnglish.Visible = false;
            lblAbstractEnglish.Visible = true;
            TxtAbstractChinese.Visible = false;
            lblAbstractChinese.Visible = true;
            txtObjective.Visible = false;
            lblObjective.Visible = true;
            txtbackground.Visible = false;
            lblbackground.Visible = true;
            txtPilot_Work_Done.Visible = false;
            lblPilot_Work_Done.Visible = true;
            txtAdditionalInformation.Visible = false;
            lblAdditionalInformation.Visible = true;
            txtProposedProducts.Visible = false;
            lblProposedProducts.Visible = true;
            txtTargetMarket.Visible = false;
            lblTargetMarket.Visible = true;
            txtCompetitionAnalysis.Visible = false;
            lblCompetitionAnalysis.Visible = true;
            txtExitStrategy.Visible = false;
            lblExitStrategy.Visible = true;
            txtRevenueModel.Visible = false;
            lblRevenueModel.Visible = true;

            txtFirst6Months.Visible = false;
            txtSecond6Months.Visible = false;
            txtThird6Months.Visible = false;
            txtForth6Months.Visible = false;
            lblFirst6Months.Visible = true;
            lblSecond6Months.Visible = true;
            lblThird6Months.Visible = true;
            lblForth6Months.Visible = true;

            //commented on 26102018 cpip changes

            //txtResubmission_Project_Reference.Visible = false;
            //lblResubmission_Project_Reference.Visible = true;
            txtResubmission_Main_Differences.Visible = false;
            lblResubmission_Main_Differences.Visible = true;

            txtCompany_Ownership_1.Visible = false;
            lblCompany_Ownership_1.Visible = true;

            txtOtherAttributes.Visible = false;
            lblOtherAttributes.Visible = true;
            txtName_PrincipalApplicant.Visible = false;
            lblName_PrincipalApplicant.Visible = true;
            txtPosition_PrincipalApplicant.Visible = false;
            lblPosition_PrincipalApplicant.Visible = true;

            //txtProjectName.Visible = false;
            //lblProjectName.Visible = true;

            txtWebsiteName.Visible = false;
            lblWebsiteName.Visible = true;

            ddlCountryOrigin.Enabled = false;

            txtestablishmentyear.Visible = imgEstablishmentyear.Visible = false;
            lblestablishmentyear.Visible = true;
            //rdbNEWHK.Enabled = false;
        }

        private void DisableListTextbox()
        {
            for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
            {
                TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
                TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
                TextBox txtApplicationStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationStatus");
                TextBox txtFundingStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtFundingStatus");
                TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");

                Label lblNameofProgram = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblNameofProgram");
                Label lblApplicationDate = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblApplicationDate");
                Label lblApplicationStatus = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblApplicationStatus");
                Label lblFundingStatus = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblFundingStatus");
                Label lblNature = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblNature");
                Label lblAmountReceived = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblAmountReceived");
                Label lblApplicationMaximumAmount = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblApplicationMaximumAmount");

                txtNameofProgram.Visible = false;
                txtApplicationDate.Visible = false;
                txtApplicationStatus.Visible = false;
                txtFundingStatus.Visible = false;
                txtNature.Visible = false;
                txtAmountReceived.Visible = false;
                txtApplicationMaximumAmount.Visible = false;

                lblNameofProgram.Visible = true;
                lblApplicationDate.Visible = true;
                lblApplicationStatus.Visible = true;
                lblFundingStatus.Visible = true;
                lblNature.Visible = true;
                lblAmountReceived.Visible = true;
                lblApplicationMaximumAmount.Visible = true;

                txtPartner_Profiles.Visible = false;
                lblPartner_Profiles.Visible = true;
                Manpower_Distribution.Visible = false;
                lblManpower_Distribution.Visible = true;
                Equipment_Distribution.Visible = false;
                lblEquipment_Distribution.Visible = true;
                Other_Costs.Visible = false;
                lblOther_Costs.Visible = true;
                Forecast_Income.Visible = false;
                lblForecast_Income.Visible = true;
            }

            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMembersProfile");
                TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");

                Label lblBoxAddress = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblName");
                Label lblNOE = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblTitle");
                Label lblCoreMembersProfile = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblCoreMembersProfile");
                Label lblHKID = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblHKID");

                TextBoxAddress.Visible = false;
                txtNOE.Visible = false;
                txtCoreMembersProfile.Visible = false;
                txtHKID.Visible = false;

                lblBoxAddress.Visible = true;
                lblNOE.Visible = true;
                lblCoreMembersProfile.Visible = true;
                lblHKID.Visible = true;
            }

            for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
            {
                TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactPostition");
                TextBox Contact_No_Home = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoHome");
                TextBox Contact_No_Office = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoOffice");
                TextBox Contact_No_Mobile = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoMobile");
                TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                DropDownList Nationality = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("ddlContactNationality");
                TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");


                Label lblContactLast_name = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactLast_name");
                Label lblContactFirst_name = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactFirst_name");
                Label lblPosition = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactPostition");
                Label lblContact_No_Home = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactNoHome");
                Label lblContact_No_Office = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactNoOffice");
                Label lblContact_No_Mobile = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactNoMobile");
                Label lblFax = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactFax");
                Label lblEmail = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactEmail");
                Label lblNationality = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactNationality");
                Label lblMailing_Address = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactAddress");

                txtContactLast_name.Visible = false;
                txtContactFirst_name.Visible = false;
                Position.Visible = false;
                Contact_No_Home.Visible = false;
                Contact_No_Office.Visible = false;
                Contact_No_Mobile.Visible = false;
                Fax.Visible = false;
                Email.Visible = false;
                Nationality.Visible = false;
                Mailing_Address.Visible = false;

                lblContactLast_name.Visible = true;
                lblContactFirst_name.Visible = true;
                lblPosition.Visible = true;
                lblContact_No_Home.Visible = true;
                lblContact_No_Office.Visible = true;
                lblContact_No_Mobile.Visible = true;
                lblFax.Visible = true;
                lblEmail.Visible = true;
                lblNationality.Visible = true;
                lblMailing_Address.Visible = true;
            }

        }

        protected void btnIncubationForm_Click(object sender, EventArgs e)
        {
            SetPanelVisibilityOfStep(1);
        }
        protected void btn_StepPrevious_Click(object sender, EventArgs e)
        {
            lbl_Exception.InnerHtml = "";
            lblgrouperror.Visible = false;
            ShowHideControlsBasedUponUserData();
            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) - 1);
        }
        protected void btn_StepNext_Click(object sender, EventArgs e)
        {


            ShowHideControlsBasedUponUserData();
            lblgrouperror.Visible = false;
            ////            lbl_Exception.InnerHtml = "teating";
            //if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2)
            //{

            //    SaveStep1Data();
            //    InitializeFundingStatus();
            //}
            //else
            //    if ((Convert.ToInt16(hdn_ActiveStep.Value) + 1) == 3)
            //{
            //    InitialCoreMembers();
            //}
            //else if ((Convert.ToInt16(hdn_ActiveStep.Value) + 1) == 3)
            //{
            //    FillContact();
            //}
            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) + 1);


        }
        protected void btn_StepSave_Click(object sender, EventArgs e)
        {

            bool IsError = false;
            lbl_Exception.InnerHtml = "";
            lblgrouperror.Visible = false;
            int ActiveStep = Convert.ToInt16(hdn_ActiveStep.Value);
            if (ActiveStep > 0)
            {
                quicklnk_1.CssClass = "";
                quicklnk_2.CssClass = "";
                quicklnk_3.CssClass = "";
                quicklnk_4.CssClass = "";
                quicklnk_5.CssClass = "";
                for (int i = ActiveStep; i > 0; i--)
                {
                    ((LinkButton)(this.FindControl("quicklnk_" + i))).CssClass = "active";
                }
            }
            check_db_validations(false);
            //if (ActiveStep == 1)
            //{
            //    SaveStep1Data();
            //}
            //else if (ActiveStep == 2)
            //{
            //    SaveStep2Data();
            //}
            //else if (ActiveStep == 3)
            //{
            //    SaveStep3Data();
            //}
            //else if (ActiveStep == 4)
            //{
            //    SaveStep4Data(out IsError);
            //}
            //else if (ActiveStep == 5)
            //{
            //    SaveStep5Data();
            //}
            ShowHideControlsBasedUponUserData();
        }
        protected void SetPanelVisibilityOfStep(int ActiveStep)
        {
            lblgrouperror.Visible = false;
            ShowbottomMessage("", true);
            hdn_ActiveStep.Value = ActiveStep.ToString();
            pnl_programDetail.Visible = true;
            pnl_Buttons.Visible = true;
            progressList.Visible = true;
            btn_StepNext.Visible = true;
            btn_Submit.Visible = false;
            IncubationSubmitPopup.Visible = false;
            if (ActiveStep > 0)
            {

                quicklnk_1.CssClass = "";
                quicklnk_2.CssClass = "";
                quicklnk_3.CssClass = "";
                quicklnk_4.CssClass = "";
                quicklnk_5.CssClass = "";
                for (int i = ActiveStep; i > 0; i--)
                {
                    ((LinkButton)(this.FindControl("quicklnk_" + i))).CssClass = "active";
                }
            }
            switch (ActiveStep)
            {
                case 0:
                    {
                        pnl_InstructionForm.Visible = true;

                        pnl_IncubationStep1.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep5.Visible = false;
                        pnl_programDetail.Visible = false;
                        pnl_Buttons.Visible = false;
                        progressList.Visible = false;


                    }
                    break;
                case 1:
                    {
                        pnl_IncubationStep1.Visible = true;



                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep5.Visible = false;

                    }
                    break;
                case 2:
                    {
                        pnl_IncubationStep2.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep5.Visible = false;
                        // InitializeFundingStatus();

                    }
                    break;
                case 3:
                    {
                        pnl_IncubationStep3.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep5.Visible = false;
                        //InitialCoreMembers();

                    }
                    break;
                case 4:
                    {
                        pnl_IncubationStep4.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        pnl_IncubationStep5.Visible = false;
                        // FillContact();

                    }
                    break;
                case 5:
                    {
                        pnl_IncubationStep5.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        btn_StepNext.Visible = false;
                        btn_Submit.Visible = true;

                    }
                    break;
                default:
                    break;
            }
        }

        protected void ButtonAddCoreMembers_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddNewCoreMemberRow();
        }
        private void InitialCoreMembers()
        {
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();
            if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                objCoreMembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(Guid.Parse(hdn_ApplicationID.Value));

            if (objCoreMembers.Count == 0)
            {
                objCoreMembers.Add(new TB_APPLICATION_COMPANY_CORE_MEMBER() { Core_Member_ID = 0 });
            }

            grvCoreMember.DataSource = objCoreMembers;
            grvCoreMember.DataBind();
        }

        private void AddNewCoreMemberRow()
        {
            bool IsError = false;
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();
            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                try
                {
                    HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
                    TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                    TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                    TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMembersProfile");
                    TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");
                    TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                    objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                    objMember.Name = TextBoxAddress.Text;
                    objMember.Position = txtNOE.Text;
                    objMember.CoreMember_Profile = txtCoreMembersProfile.Text;
                    objMember.HKID = txtHKID.Text;
                    objCoreMembers.Add(objMember);
                }
                catch (Exception ex)
                {
                    IsError = true;
                    lblCorememberError.Text = ex.Message;
                }
            }
            if (IsError == false)
            {
                objCoreMembers.Add(new TB_APPLICATION_COMPANY_CORE_MEMBER() { Core_Member_ID = 0 });
                grvCoreMember.DataSource = objCoreMembers;
                grvCoreMember.DataBind();

            }
            //  SetPreviousData();
        }

        private List<TB_APPLICATION_COMPANY_CORE_MEMBER> GetCoreMemberForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();
            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                string titleerror = "Core member" + (i + 1) + " : ";

                try
                {

                    HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
                    TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                    TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                    TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMembersProfile");
                    TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");
                    if (TextBoxAddress.Text != "" || txtNOE.Text != "" || txtCoreMembersProfile.Text != "" || txtHKID.Text != "")
                    {
                        TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                        objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);

                        if ((TextBoxAddress.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), TextBoxAddress.Text))
                            || (IsSubmitClick && TextBoxAddress.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), TextBoxAddress.Text)))
                            objerror.Add(titleerror + Localize("Core_member_name"));
                        else objMember.Name = TextBoxAddress.Text;


                        if ((txtNOE.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNOE.Text))
                            || (IsSubmitClick && txtNOE.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNOE.Text)))
                            objerror.Add(titleerror + Localize("Core_member_position"));
                        else objMember.Position = txtNOE.Text;

                        if ((txtHKID.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtHKID.Text))
                            || (IsSubmitClick && txtHKID.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtHKID.Text)))
                            objerror.Add(titleerror + Localize("Core_member_HKID"));
                        else objMember.HKID = MD5Encryption.EncryptData(txtHKID.Text);
                        if (txtHKID.Text.Length > 0)
                        {
                            if (txtHKID.Text.Length > 4)
                            {
                                string masked = new string('*', txtHKID.Text.Length - 4);
                                objMember.Masked_HKID = txtHKID.Text.Substring(0, 4) + masked;
                            }
                            else
                            {
                                objMember.Masked_HKID = txtHKID.Text;
                            }


                        }
                        objMember.CoreMember_Profile = txtCoreMembersProfile.Text;
                        objCoreMembers.Add(objMember);
                    }
                }
                catch (Exception ex)
                {
                    objerror.Add(titleerror + " " + ex.Message);
                }
            }

            return objCoreMembers;

            //  SetPreviousData();
        }

        private void InitializeFundingStatus()
        {
            List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = new List<TB_APPLICATION_FUNDING_STATUS>();
            if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(Guid.Parse(hdn_ApplicationID.Value));
            if (ObjFundingStatus.Count == 0)
            {
                ObjFundingStatus = new List<TB_APPLICATION_FUNDING_STATUS>();
                ObjFundingStatus.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = null });

            }
            Grd_FundingStatus.DataSource = ObjFundingStatus;
            Grd_FundingStatus.DataBind();
        }
        private void AddNewFundingStatus()
        {
            bool IsError = false;
            lbl_fundingError.Text = string.Empty;

            List<TB_APPLICATION_FUNDING_STATUS> objCoreMembers = new List<TB_APPLICATION_FUNDING_STATUS>();

            for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
            {
                try
                {
                    HiddenField FundingID = (HiddenField)Grd_FundingStatus.Rows[i].Cells[0].FindControl("FundingID");
                    TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
                    TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
                    TextBox txtApplicationStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationStatus");
                    TextBox txtFundingStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtFundingStatus");
                    TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                    TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                    TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
                    DropDownList Currency = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("Currency");

                    TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                    objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                    objMember.Programme_Name = txtNameofProgram.Text;
                    objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
                    objMember.Funding_Status = txtFundingStatus.Text;
                    objMember.Application_Status = txtApplicationStatus.Text;
                    objMember.Expenditure_Nature = txtNature.Text;
                    objMember.Currency = Currency.SelectedValue;
                    if (txtAmountReceived.Text != "")
                    {
                        objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);
                    }
                    if (txtApplicationMaximumAmount.Text != "")
                    {
                        objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);
                    }
                    objCoreMembers.Add(objMember);
                }
                catch (Exception ex)
                {
                    IsError = true;
                    lbl_fundingError.Text = ex.Message;
                }
            }
            if (IsError == false)
            {
                objCoreMembers.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = null });


                Grd_FundingStatus.DataSource = objCoreMembers;
                Grd_FundingStatus.DataBind();
            }

            //  SetPreviousData();
        }

        private List<TB_APPLICATION_FUNDING_STATUS> GetFundingStatusForSave(bool IsSubmitClick, ref List<string> objErrors)
        {
            List<TB_APPLICATION_FUNDING_STATUS> objCoreMembers = new List<TB_APPLICATION_FUNDING_STATUS>();
            for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
            {
                string titleerror = "Funding status " + (i + 1) + " : ";
                try
                {
                    HiddenField FundingID = (HiddenField)Grd_FundingStatus.Rows[i].Cells[0].FindControl("FundingID");
                    TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
                    TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
                    TextBox txtApplicationStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationStatus");
                    TextBox txtFundingStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtFundingStatus");
                    TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                    TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                    TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
                    DropDownList Currency = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("Currency");
                    if (txtNameofProgram.Text != "" || txtApplicationStatus.Text != "" || txtFundingStatus.Text != "" || txtNature.Text != "" || txtAmountReceived.Text != "" || txtApplicationMaximumAmount.Text != "")
                    {
                        TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                        objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                        if ((txtNameofProgram.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNameofProgram.Text))
                            || (IsSubmitClick && txtNameofProgram.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNameofProgram.Text)))
                            objErrors.Add(titleerror + Localize("Funding_Programme_name"));

                        else objMember.Programme_Name = txtNameofProgram.Text;

                        if ((txtApplicationStatus.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtApplicationStatus.Text))
                            || (IsSubmitClick && txtApplicationStatus.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtApplicationStatus.Text))
                            )
                            objErrors.Add(titleerror + Localize("Funding_Application_status"));
                        else objMember.Application_Status = txtApplicationStatus.Text;

                        if (txtApplicationDate.Text != "")
                        {
                            DateTime dDate;

                            if (DateTime.TryParse(txtApplicationDate.Text, out dDate))
                            {
                                String.Format("{0:M-yy}", dDate);
                                objMember.Date = Convert.ToDateTime(txtApplicationDate.Text.Trim());

                            }
                            else
                            {
                                objErrors.Add(Localize("Funding_Date") + (i + 1)); // <-- Control flow goes here
                            }
                        }

                        else
                            objErrors.Add(titleerror + Localize("Funding_Date_Empty"));

                        if ((txtFundingStatus.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtFundingStatus.Text))
                            || (IsSubmitClick && txtFundingStatus.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtFundingStatus.Text))
                            )
                            objErrors.Add(titleerror + Localize("Funding_status"));
                        else objMember.Funding_Status = txtFundingStatus.Text;

                        if ((txtNature.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNature.Text)) ||
                            (IsSubmitClick && txtNature.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNature.Text))
                            )
                            objErrors.Add(titleerror + Localize("Funding_Expenditure"));
                        else objMember.Expenditure_Nature = txtNature.Text;

                        if ((txtAmountReceived.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmountReceived.Text))
                            || (IsSubmitClick && txtAmountReceived.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmountReceived.Text)))
                            objErrors.Add(titleerror + Localize("Funding_Amount"));
                        else if (txtAmountReceived.Text.Length > 0)
                            objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);

                        if ((txtApplicationMaximumAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtApplicationMaximumAmount.Text))
                            || (IsSubmitClick && txtApplicationMaximumAmount.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtApplicationMaximumAmount.Text)))
                            objErrors.Add(titleerror + Localize("Funding_MaximumAmount"));
                        else if (txtApplicationMaximumAmount.Text.Length > 0)
                            objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);


                        objMember.Currency = Currency.SelectedValue;
                        objCoreMembers.Add(objMember);
                    }
                }
                catch (Exception ex)
                {
                    objErrors.Add(titleerror + " " + ex.Message);
                }

            }

            return objCoreMembers;

            //  SetPreviousData();
        }
        private List<TB_APPLICATION_CONTACT_DETAIL> GetContactDetailsForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            List<TB_APPLICATION_CONTACT_DETAIL> objContactDetails = new List<TB_APPLICATION_CONTACT_DETAIL>();
            for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
            {
                string titleerror = "Contact Detail " + (i + 1) + " : ";
                try
                {
                    HiddenField contactId = (HiddenField)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("CONTACT_DETAILS_ID");
                    TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                    TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                    TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactPostition");
                    TextBox Contact_No_Home = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoHome");
                    TextBox Contact_No_Office = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoOffice");
                    TextBox Contact_No_Mobile = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoMobile");
                    TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                    TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                    DropDownList Nationality = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("ddlContactNationality");
                    TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                    DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");

                    if (txtContactLast_name.Text != "" || txtContactFirst_name.Text != "" || Position.Text != "" || Contact_No_Home.Text != "" || Contact_No_Office.Text != "" || Contact_No_Mobile.Text != "" ||
                        Fax.Text != "" || Email.Text != "" || Mailing_Address.Text != "")
                    {
                        TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                        objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                        if (txtContactLast_name.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactLast_name.Text))
                            objerror.Add(titleerror + Localize("Contact_lastname"));
                        else objMember.Last_Name_Eng = txtContactLast_name.Text;
                        if (txtContactFirst_name.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactFirst_name.Text))
                            objerror.Add(titleerror + Localize("Contact_firstname"));
                        else objMember.First_Name_Eng = txtContactFirst_name.Text;

                        if (Position.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Position.Text))
                            objerror.Add(titleerror + Localize("Contact_Position"));
                        else objMember.Position = Position.Text;

                        if (Contact_No_Home.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", Contact_No_Home.Text))
                            objerror.Add(titleerror + Localize("Contact_Home"));
                        else objMember.Contact_No_Home = Contact_No_Home.Text;

                        if (Contact_No_Office.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", Contact_No_Office.Text))
                            objerror.Add(titleerror + Localize("Contact_Office"));
                        else objMember.Contact_No_Office = Contact_No_Office.Text;

                        if (Contact_No_Mobile.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", Contact_No_Mobile.Text))
                            objerror.Add(titleerror + Localize("Contact_Mobile"));
                        else objMember.Contact_No_Mobile = Contact_No_Mobile.Text;

                        if (Fax.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", Fax.Text))
                            objerror.Add(titleerror + Localize("Contact_Fax"));
                        else objMember.Fax = Fax.Text;

                        objMember.Mailing_Address = Mailing_Address.Text;
                        objMember.Salutation = Salutation.Text;

                        if (Email.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Email.Text))
                            objerror.Add(titleerror + Localize("Contact_Email"));
                        else objMember.Email = Email.Text;

                        objMember.Nationality = Nationality.SelectedValue;

                        objContactDetails.Add(objMember);
                    }
                }
                catch (Exception ex)
                {
                    objerror.Add(titleerror + " " + ex.Message);
                }
            }

            return objContactDetails;

            //  SetPreviousData();
        }



        protected void btn_FundingAddNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddNewFundingStatus();

        }


        public bool SaveAttachmentUrl(string Url, enumAttachmentType objAttachmentType, Guid appId, int progId)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                string FileName = string.Empty;
                SPFunctions objSp = new SPFunctions();
                FileName = Url;
                TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
                {
                    Application_ID = appId,
                    Attachment_Path = FileName,
                    Attachment_Type = objAttachmentType.ToString(),
                    Created_By = objSp.GetCurrentUser(),
                    Created_Date = DateTime.Now,
                    Modified_By = objSp.GetCurrentUser(),
                    Modified_Date = DateTime.Now,
                    Programme_ID = progId
                };
                IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach);
                dbContext.SaveChanges();
                return true;
            }

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

        }

        protected void btn_submitFinal_Click(object sender, EventArgs e)
        {

            try
            {
                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtLoginUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                {
                    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtLoginUserName.Text, txtLoginPassword.Text);

                    if (!status)
                    {
                        IncubationSubmitPopup.Visible = true;
                        UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                    }
                    else
                    {
                        using (var dbContext = new CyberportEMS_EDM())
                        {
                            int progId = Convert.ToInt32(hdn_ProgramID.Value);
                            Guid appId = Guid.Parse(hdn_ApplicationID.Value);
                            TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId); //dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == appId);

                            if (objIncubation != null)
                            {

                                //objIncubation.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objIncubation.Version_Number)) + Convert.ToDecimal("1")).ToString("F2");
                                //objIncubation.Modified_Date = DateTime.Now;
                                //objIncubation.Status = "Submitted";
                                //    objIncubation.Last_Submitted = DateTime.Now;
                                //if (objIncubation.Application_Parent_ID == null)
                                //{
                                //    objIncubation.Submission_Date = DateTime.Now;
                                //}
                                bool isrequestor = false;
                                if (objIncubation.Application_Parent_ID == null)
                                {
                                    if (objIncubation.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                    {
                                        objIncubation.Status = "Resubmitted information";//formsubmitaction.Resubmitted_information.ToString().Replace("_", " ");
                                        isrequestor = true;
                                    }
                                    else
                                    {
                                        objIncubation.Status = formsubmitaction.Submitted.ToString();
                                        objIncubation.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objIncubation.Version_Number)) + 1).ToString("F2");
                                        isrequestor = false;
                                    }
                                    objIncubation.Modified_Date = DateTime.Now;
                                    objIncubation.Last_Submitted = DateTime.Now;
                                    //objIncubation.Status = "Submitted";
                                    objIncubation.Submission_Date = DateTime.Now;
                                }
                                else
                                {
                                    dbContext.TB_INCUBATION_APPLICATION.Remove(objIncubation);
                                    Guid ParentId = Guid.Parse(objIncubation.Application_Parent_ID);
                                    TB_INCUBATION_APPLICATION oldobjIncubation = GetDatabyParentId(dbContext, ParentId);
                                    oldobjIncubation.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objIncubation.Version_Number)) + 1).ToString("F2");
                                    oldobjIncubation.Abstract = objIncubation.Abstract;
                                    oldobjIncubation.Abstract_Chi = objIncubation.Abstract_Chi;
                                    oldobjIncubation.Additional_Information = objIncubation.Additional_Information;
                                    oldobjIncubation.Applicant = objIncubation.Applicant;
                                    oldobjIncubation.Background = objIncubation.Background;
                                    oldobjIncubation.Business_Area = objIncubation.Business_Area;
                                    oldobjIncubation.Company_Name_Chi = objIncubation.Company_Name_Chi;
                                    oldobjIncubation.Company_Name_Eng = objIncubation.Company_Name_Eng;
                                    oldobjIncubation.Establishment_Year = objIncubation.Establishment_Year;
                                    oldobjIncubation.Company_Ownership_Structure = objIncubation.Company_Ownership_Structure;
                                    oldobjIncubation.Company_Type = objIncubation.Company_Type;
                                    oldobjIncubation.Competition_Analysis = objIncubation.Competition_Analysis;
                                    oldobjIncubation.Core_Members_Profiles = objIncubation.Core_Members_Profiles;
                                    oldobjIncubation.Created_By = objIncubation.Created_By;
                                    oldobjIncubation.Created_Date = objIncubation.Created_Date;
                                    oldobjIncubation.Declaration = objIncubation.Declaration;
                                    oldobjIncubation.Equipment_Distribution = objIncubation.Equipment_Distribution;
                                    oldobjIncubation.Exit_Strategy = objIncubation.Exit_Strategy;
                                    oldobjIncubation.First_6_Months_Milestone = objIncubation.First_6_Months_Milestone;
                                    oldobjIncubation.Forecast_Income = objIncubation.Forecast_Income;
                                    oldobjIncubation.Forth_6_Months_Milestone = objIncubation.Forth_6_Months_Milestone;
                                    oldobjIncubation.Have_Read_Statement = objIncubation.Have_Read_Statement;
                                    oldobjIncubation.Intake_Number = objIncubation.Intake_Number;
                                    oldobjIncubation.Major_Partners_Profiles = objIncubation.Major_Partners_Profiles;
                                    oldobjIncubation.Last_Submitted = DateTime.Now;
                                    oldobjIncubation.Management_Positioning = objIncubation.Management_Positioning;
                                    oldobjIncubation.Manpower_Distribution = objIncubation.Manpower_Distribution;
                                    oldobjIncubation.Marketing_Information = objIncubation.Marketing_Information;
                                    oldobjIncubation.Modified_By = objIncubation.Modified_By;
                                    // oldobjIncubation.Modified_Date = DateTime.Now;
                                    oldobjIncubation.Objective = objIncubation.Objective;
                                    oldobjIncubation.Other_Attributes = objIncubation.Other_Attributes;
                                    oldobjIncubation.Other_Bussiness_Area = objIncubation.Other_Bussiness_Area;
                                    oldobjIncubation.Other_Company_Type = objIncubation.Other_Company_Type;
                                    oldobjIncubation.Other_Direct_Costs = objIncubation.Other_Direct_Costs;
                                    oldobjIncubation.Other_Positioning = objIncubation.Other_Positioning;
                                    oldobjIncubation.Pilot_Work_Done = objIncubation.Pilot_Work_Done;
                                    oldobjIncubation.Positioning = objIncubation.Positioning;
                                    oldobjIncubation.Preferred_Track = objIncubation.Preferred_Track;
                                    oldobjIncubation.Principal_Full_Name = objIncubation.Principal_Full_Name;
                                    oldobjIncubation.Principal_Position_Title = objIncubation.Principal_Position_Title;
                                    oldobjIncubation.Programme_ID = objIncubation.Programme_ID;
                                    oldobjIncubation.Proposed_Products = objIncubation.Proposed_Products;
                                    oldobjIncubation.Question1_1 = objIncubation.Question1_1;

                                    oldobjIncubation.Question1_2 = objIncubation.Question1_2;
                                    oldobjIncubation.Question1_3 = objIncubation.Question1_3;
                                    oldobjIncubation.Question1_4 = objIncubation.Question1_4;
                                    oldobjIncubation.Question1_5 = objIncubation.Question1_5;
                                    oldobjIncubation.Question1_6 = objIncubation.Question1_6;
                                    oldobjIncubation.Question1_7 = objIncubation.Question1_7;
                                    oldobjIncubation.Question1_8 = objIncubation.Question1_8;
                                    //oldobjIncubation.Question1_8_1 = objIncubation.Question1_8_1;
                                    oldobjIncubation.Question1_9 = objIncubation.Question1_9;
                                    //oldobjIncubation.Question1_10 = objIncubation.Question1_10;
                                    oldobjIncubation.Resubmission = objIncubation.Resubmission;
                                    oldobjIncubation.Resubmission_Main_Differences = objIncubation.Resubmission_Main_Differences;
                                    oldobjIncubation.Resubmission_Project_Reference = objIncubation.Resubmission_Project_Reference;
                                    oldobjIncubation.Revenus_Model = objIncubation.Revenus_Model;
                                    oldobjIncubation.Second_6_Months_Milestone = objIncubation.Second_6_Months_Milestone;
                                    oldobjIncubation.Submission_Date = DateTime.Now;
                                    oldobjIncubation.Status = "Submitted"; //oldobjIncubation.Status = objIncubation.Status;

                                    oldobjIncubation.Target_Market = objIncubation.Target_Market;
                                    oldobjIncubation.Third_6_Months_Milestone = objIncubation.Third_6_Months_Milestone;
                                    dbContext.SaveChanges();

                                    List<TB_APPLICATION_ATTACHMENT> objIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_ATTACHMENT.RemoveRange(objIncubationAttachement);

                                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.RemoveRange(objTB_APPLICATION_COMPANY_CORE_MEMBER);

                                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_CONTACT_DETAIL.RemoveRange(objTB_APPLICATION_CONTACT_DETAIL);

                                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_FUNDING_STATUS.RemoveRange(objTB_APPLICATION_FUNDING_STATUS);
                                    List<TB_APPLICATION_ATTACHMENT> addobjIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objIncubation.Incubation_ID && x.Programme_ID == objIncubation.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_ATTACHMENT.AddRange(addobjIncubationAttachement);
                                    foreach (TB_APPLICATION_ATTACHMENT objAttach in addobjIncubationAttachement)
                                    {
                                        objAttach.Application_ID = oldobjIncubation.Incubation_ID;
                                    }
                                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> addobjTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objIncubation.Incubation_ID && x.Programme_ID == objIncubation.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.AddRange(addobjTB_APPLICATION_COMPANY_CORE_MEMBER);
                                    foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objAttach in addobjTB_APPLICATION_COMPANY_CORE_MEMBER)
                                    {
                                        objAttach.Application_ID = oldobjIncubation.Incubation_ID;
                                    }
                                    List<TB_APPLICATION_CONTACT_DETAIL> addobjTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objIncubation.Incubation_ID && x.Programme_ID == objIncubation.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_CONTACT_DETAIL.AddRange(addobjTB_APPLICATION_CONTACT_DETAIL);
                                    foreach (TB_APPLICATION_CONTACT_DETAIL objAttach in addobjTB_APPLICATION_CONTACT_DETAIL)
                                    {
                                        objAttach.Application_ID = oldobjIncubation.Incubation_ID;
                                    }
                                    List<TB_APPLICATION_FUNDING_STATUS> addobjTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objIncubation.Incubation_ID && x.Programme_ID == objIncubation.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_FUNDING_STATUS.AddRange(addobjTB_APPLICATION_FUNDING_STATUS);
                                    foreach (TB_APPLICATION_FUNDING_STATUS objAttach in addobjTB_APPLICATION_FUNDING_STATUS)
                                    {
                                        objAttach.Application_ID = oldobjIncubation.Incubation_ID;
                                    }



                                }
                                dbContext.SaveChanges();
                                string requestor = "";
                                string strEmailContent = "";
                                string strEmailsubject = "";

                                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                                objTbParams = dbContext.TB_SYSTEM_PARAMETER;

                                TB_PROGRAMME_INTAKE oIntake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == objIncubation.Programme_ID);
                                string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                                WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;

                                string applicationType = "IncubationProgram.aspx";
                                string token = "/SitePages/" + applicationType + "?prog=" + objIncubation.Programme_ID + "&app=" + objIncubation.Incubation_ID;

                                if (isrequestor == true)
                                {

                                    List<TB_SCREENING_HISTORY> objTB_SCREENING_HISTORY1 = new List<TB_SCREENING_HISTORY>();
                                    TB_SCREENING_HISTORY objTB_SCREENING_HISTORY = new TB_SCREENING_HISTORY();
                                    objTB_SCREENING_HISTORY1 = dbContext.TB_SCREENING_HISTORY.OrderByDescending(x => x.Created_Date).ToList();
                                    objTB_SCREENING_HISTORY = objTB_SCREENING_HISTORY1.FirstOrDefault(x => x.Application_Number == objIncubation.Application_Number && x.Programme_ID == objIncubation.Programme_ID);

                                    requestor = objIncubation.Created_By;

                                    strEmailContent = CBPEmail.GetEmailTemplate("Requestor_Applicant_Resubmitted");
                                    strEmailContent = strEmailContent.Replace("@@intakeno", oIntake.Intake_Number.ToString()).Replace("@@IntakeNumber", oIntake.Intake_Number.ToString());
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objIncubation.Application_Number);
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted_Requestor").Replace("@@Applicationnumber", objIncubation.Application_Number);
                                    strEmailsubject = strEmailsubject.Replace("@@ProgramName", oIntake.Programme_Name);


                                    int IsEmailSent = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);

                                    requestor = objTB_SCREENING_HISTORY.Created_By;

                                    strEmailContent = CBPEmail.GetEmailTemplate("Application_Updated_Coordinator");
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objIncubation.Application_Number);
                                    strEmailContent = strEmailContent.Replace("@@Comment", objTB_SCREENING_HISTORY.Comment_For_Applicants);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_Resubmitted_Coordinator").Replace("@@Applicationnumber", objIncubation.Application_Number);
                                }
                                else
                                {
                                    requestor = objIncubation.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("Application_Applicant_Submitted");

                                    strEmailContent = strEmailContent.Replace("@@intakeno", oIntake.Intake_Number.ToString()).Replace("@@IntakeNumber", oIntake.Intake_Number.ToString());
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objIncubation.Application_Number);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objIncubation.Application_Number);
                                }

                                int IsEmailed = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);
                                IncubationSubmitPopup.Visible = false;
                                pnlsubmissionpopup.Visible = true;
                                if (isrequestor == true)
                                {
                                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Re-submitted");
                                }
                                else
                                {
                                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Submitted");
                                }
                                Fill_Programelist(objIncubation.Application_Number, objIncubation.Programme_ID, objIncubation.Intake_Number, objIncubation.Version_Number, objIncubation.Business_Area, objIncubation.Status, objIncubation.Applicant);

                            }
                            else
                            {
                                IncubationSubmitPopup.Visible = true;
                                UserCustomerrorLogin.InnerHtml = Localize("Releventdata_error");
                            }

                        }
                    }
                }
                else
                {
                    IncubationSubmitPopup.Visible = true;
                    UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                }
            }
            catch (Exception ex)
            {
                IncubationSubmitPopup.Visible = true;
                UserCustomerrorLogin.InnerText = ex.Message;
            }
        }

        protected void btn_Submit_Click1(object sender, EventArgs e)
        {
            int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
            int errors = check_db_validations(true);
            if (errors == 0)
            {
                if (!SubmitValidationError())
                {

                    IncubationSubmitPopup.Visible = true;
                    SPFunctions objFUnction = new SPFunctions();
                    txtLoginUserName.Text = objFUnction.GetCurrentUser();
                    using (var Intake = new CyberportEMS_EDM())
                    {
                        TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                        lblintakeno.Text = objProgram.Intake_Number.ToString();
                    }
                }

                else
                {
                    lblgrouperror.Visible = true;
                }
            }
            ShowHideControlsBasedUponUserData();

        }

        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            IncubationSubmitPopup.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message">Empty message to hide the element</param>
        /// <param name="Success"></param>
        protected void ShowbottomMessage(string Message, bool Success)
        {
            lbl_success.InnerHtml = "";
            lbl_Exception.InnerHtml = "";
            if (Message.Length > 0)
            {
                if (Success)
                    lbl_success.InnerHtml = Message;
                else
                    lbl_Exception.InnerHtml = Message;
            }
        }

        protected void ShowHideControlsBasedUponUserData()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);

                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    if (objIncubation != null)
                    {
                        bool Activation = false;
                        //if (Convert.ToString(objIncubation.Question1_4) != "")
                        //{
                        //    Activation = Convert.ToBoolean(objIncubation.Question1_4);
                        //    if (Activation == false)
                        //    {
                        //        lblQ15.Enabled = true;
                        //        lblQ15td.Enabled = true;
                        //        rbtnPanel1Q5.Enabled = true;
                        //    }
                        //}


                        if (Convert.ToString(objIncubation.Question1_8) != "")
                        {
                            Activation = Convert.ToBoolean(objIncubation.Question1_8);
                            if (Activation)
                            {
                                //  ss8 changes
                                //  lblQ19.Enabled = true;
                                // lblQ19td.Enabled = true;
                                //  rbtnPanel1Q9.Enabled = true;
                            }
                        }

                        if (Convert.ToString(objIncubation.Resubmission) != "")
                        {

                            if ((bool)objIncubation.Resubmission)
                            {

                                //commented on 26102018 cpip changes
                                // txtResubmission_Project_Reference.Enabled = true;
                                txtResubmission_Main_Differences.Enabled = true;

                            }
                        }

                        if (!string.IsNullOrEmpty(objIncubation.Company_Type))
                        {
                            if (objIncubation.Company_Type.ToLower().Trim() == "others")
                            {
                                //  txtOther_Company_Type.Text = objIncubation.Other_Company_Type;
                                txtOther_Company_Type.Style.Remove("display");
                                txtOther_Company_Type.Visible = true;
                            }
                            else
                            {

                                txtOther_Company_Type.Style.Add("display", "none");
                            }

                        }
                        else
                        {

                            txtOther_Company_Type.Style.Add("display", "none");
                        }




                        if (!string.IsNullOrEmpty(objIncubation.Business_Area))
                        {
                            if (objIncubation.Business_Area.ToLower().Trim() == "others")
                            {
                                // txtOther_Bussiness_Area.Text = objIncubation.Other_Bussiness_Area;
                                txtOther_Bussiness_Area.Style.Remove("display");
                                txtOther_Bussiness_Area.Visible = true;
                            }
                            else
                            {

                                txtOther_Bussiness_Area.Style.Add("display", "none");
                            }
                        }
                        else
                        {

                            txtOther_Bussiness_Area.Style.Add("display", "none");
                        }


                        if (!string.IsNullOrEmpty(objIncubation.Positioning))
                        {
                            if (objIncubation.Positioning.ToLower().Contains("trading"))
                            {
                                // txtManagementOther.Text = objIncubation.Management_Positioning;
                                txtManagementOther.Style.Remove("display");
                                txtManagementOther.Visible = true;

                            }
                            else
                            {

                                txtManagementOther.Style.Add("display", "none");
                            }
                            if (objIncubation.Positioning.ToLower().Contains("others"))
                            {
                                // txtPositioningOther.Text = objIncubation.Other_Positioning;
                                txtPositioningOther.Style.Remove("display");
                                txtPositioningOther.Visible = true;

                            }
                            else
                            {

                                txtPositioningOther.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            txtPositioningOther.Style.Add("display", "none");
                            txtManagementOther.Style.Add("display", "none");
                        }


                    }

                    foreach (ListItem items in chkPositioning.Items)
                    {


                        if (items.Value.ToLower() == "others" && items.Selected)
                        {
                            txtPositioningOther.Visible = true;
                            txtPositioningOther.Style.Remove("display");
                        }
                        else if (items.Value.ToLower() == "others" && !items.Selected)
                        {

                            txtPositioningOther.Style.Add("display", "none");

                        }
                        if (items.Value.ToLower().Contains("trading") && items.Selected)
                        {
                            txtManagementOther.Visible = true;
                            txtManagementOther.Style.Remove("display");

                        }
                        else if (items.Value.ToLower().Contains("trading") && !items.Selected)
                        {

                            txtManagementOther.Style.Add("display", "none");

                        }

                    }

                    //if (chkPositioning.SelectedValue != "")
                    //{
                    //    if (chkPositioning.SelectedValue.ToLower().Contains("trading"))
                    //    {

                    //        txtManagementOther.Style.Remove("display");
                    //        txtManagementOther.Visible = true;

                    //    }
                    //    else
                    //    {

                    //        txtManagementOther.Style.Add("display", "none");
                    //    }
                    //    if (chkPositioning.SelectedValue.ToLower().Contains("others"))
                    //    {

                    //        txtPositioningOther.Style.Remove("display");
                    //        txtPositioningOther.Visible = true;

                    //    }
                    //    else
                    //    {

                    //        txtPositioningOther.Style.Add("display", "none");
                    //    }
                    //}
                    //else
                    //{

                    //    txtPositioningOther.Style.Add("display", "none");
                    //    txtManagementOther.Style.Add("display", "none");
                    //}
                    if (rbtnList_Business_Area.SelectedValue != "")
                    {
                        if (rbtnList_Business_Area.SelectedValue.ToLower().Trim() == "others")
                        {
                            txtOther_Bussiness_Area.Style.Remove("display");
                            txtOther_Bussiness_Area.Visible = true;
                        }
                        else
                        {

                            txtOther_Bussiness_Area.Style.Add("display", "none");
                        }
                    }
                    else
                    {

                        txtOther_Bussiness_Area.Style.Add("display", "none");
                    }
                    if (rbtnCompany_Type.SelectedValue != "")
                    {
                        if (rbtnCompany_Type.SelectedValue.ToLower().Trim() == "others")
                        {
                            txtOther_Company_Type.Style.Remove("display");
                            txtOther_Company_Type.Visible = true;
                        }
                        else
                        {

                            txtOther_Company_Type.Style.Add("display", "none");
                        }
                    }
                    else
                    {

                        txtOther_Company_Type.Style.Add("display", "none");
                    }


                }
            }
            catch (Exception)
            {



            }

        }
        protected bool SubmitValidationError()
        {
            bool IsError = false;

            List<string> errlist = new List<string>();
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    SPFunctions objFn = new SPFunctions();
                    string strCurrentUser = objFn.GetCurrentUser();
                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(objIncubation.Incubation_ID);
                    List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT = IncubationContext.ListofTB_APPLICATION_ATTACHMENTGGet(objIncubation.Incubation_ID, objIncubation.Programme_ID);
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> Objcoremembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(objIncubation.Incubation_ID);
                    List<TB_APPLICATION_CONTACT_DETAIL> ObjTB_APPLICATION_CONTACT_DETAIL = IncubationContext.APPLICATION_CONTACT_DETAIL_GET(objIncubation.Incubation_ID);
                    if (objIncubation != null)
                    {

                        if (Convert.ToString(objIncubation.Question1_1) == "")
                        {

                            IsError = true;
                            errlist.Add(Localize("Question_1_1"));

                        }
                        else if (objIncubation.Question1_1.Value == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Question_1_1_SubmitError"));

                        }

                        if (Convert.ToString(objIncubation.Question1_2) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_2"));

                        }
                        else if (objIncubation.Question1_2.Value == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Question_1_2_SubmitError"));

                        }

                        if (Convert.ToString(objIncubation.Question1_3) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_3"));

                        }
                        else if (objIncubation.Question1_3.Value == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Question_1_3_SubmitError"));

                        }


                        if (Convert.ToString(objIncubation.Question1_4) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_4"));

                        }
                        else if (objIncubation.Question1_4.Value == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Question_1_4_SubmitError"));

                        }

                        if (Convert.ToString(objIncubation.Question1_5) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_5"));

                        }
                        else if (objIncubation.Question1_5.Value == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Question_1_5_SubmitError"));

                        }

                        if (Convert.ToString(objIncubation.Question1_6) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_6"));

                        }
                        if (Convert.ToString(objIncubation.Question1_7) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_7"));
                        }
                        bool Activation = Convert.ToBoolean(objIncubation.Question1_8);
                        if (Convert.ToString(objIncubation.Question1_8) != "")
                        {
                            Activation = Convert.ToBoolean(objIncubation.Question1_8);
                            //if (Activation && Convert.ToString(objIncubation.Question1_9) == "")
                            //{
                            //    IsError = true;

                            //    errlist.Add(Localize("Question_1_9"));
                            //}
                        }
                        else
                        {
                            IsError = true;

                            errlist.Add(Localize("Question_1_8"));
                        }
                        //if (Convert.ToString(objIncubation.Question1_8_1) == "")
                        //{

                        //    IsError = true;

                        //    errlist.Add(Localize("Question_1_8_1"));

                        //}
                        if (Convert.ToString(objIncubation.Question1_9) == "")
                        {

                            IsError = true;

                            errlist.Add(Localize("Question_1_9"));

                        }
                        //if (Convert.ToString(objIncubation.Question1_10) == "")
                        //{

                        //    IsError = true;

                        //    errlist.Add(Localize("Question_1_10"));

                        //}
                        if (objIncubation.Company_Name_Eng == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Comapny_Name_Error"));
                        }
                        if (objIncubation.Abstract == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Abstract_Error"));
                        }
                        if (objIncubation.Abstract_Chi == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Abstract_Chinese_Error"));
                        }
                        if (objIncubation.Objective == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Objective_Error"));
                        }
                        if (objIncubation.Background == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_General_Background"));
                        }
                        if (objIncubation.Pilot_Work_Done == "")
                        {
                            IsError = true;
                            errlist.Add(

                                Localize("Error_Pilot_Work"));
                        }

                        //if (ObjFundingStatus.Count == 0)
                        //{
                        //    IsError = true;
                        //    errlist.Add("At least one Funding Status required");
                        //}
                        //if (Convert.ToString(objIncubation.Question1_7) != null && Convert.ToString(objIncubation.Question1_8) != null)
                        //{
                        //    bool que6 = Convert.ToBoolean(objIncubation.Question1_6);
                        //    bool que7 = Convert.ToBoolean(objIncubation.Question1_7);
                        //    bool que8 = Convert.ToBoolean(objIncubation.Question1_8);
                        //    if ((que8 == true || que6 == true || que7 == true) && ObjFundingStatus.Count == 0)
                        //    {
                        //        IsError = true;

                        //        errlist.Add("Funding status"+count +"Please fill in at least one record in the required field: 2.3.2.3 Funding Status, if Question 1.6 or Question 1.7 or Question 1.8 answer is Yes.");
                        //    }
                        //else if (que8 == false && que9 == true && ObjFundingStatus.Count == 0)
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in at least one record in the required field: 2.3.2.3 Funding Status, if Question 1.7 or Question 1.8 answer is Yes.");
                        //}
                        //else if (que8 == true && que9 == true && ObjFundingStatus.Count == 0)
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in at least one record in the required field: 2.3.2.3 Funding Status, if Question 1.7 or Question 1.8 answer is Yes.");
                        //}
                        //}
                        int count = 1;
                        if (Convert.ToString(objIncubation.Question1_7) != null || Convert.ToString(objIncubation.Question1_8) != null || Convert.ToString(objIncubation.Question1_6) != null)
                        {
                            bool que6 = Convert.ToBoolean(objIncubation.Question1_6);
                            bool que8 = Convert.ToBoolean(objIncubation.Question1_8);
                            bool que7 = Convert.ToBoolean(objIncubation.Question1_7);
                            if ((que6 == true || que8 == true || que7 == true) && (ObjFundingStatus.Count == 0))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_Funding_Required"));
                            }
                            else if (que6 == true || que8 == true || que7 == true)
                            {

                                foreach (TB_APPLICATION_FUNDING_STATUS obj1 in ObjFundingStatus)
                                {
                                    if ((obj1.Currency == null) || (Convert.ToString(obj1.Date) == null) || (obj1.Programme_Name == "")
                                        || (obj1.Expenditure_Nature == "") || (obj1.Funding_Status == "") || (Convert.ToString(obj1.Maximum_Amount) == null)
                                        || (Convert.ToString(obj1.Amount_Received) == null))
                                    {
                                        IsError = true;
                                        errlist.Add(Localize("Error_Funding_Allfields"));
                                    }

                                }
                            }
                        }

                        /*
                        if (Convert.ToString(objIncubation.Question1_8) != null && Convert.ToString(objIncubation.Question1_9) != null)
                        {
                            bool que8 = Convert.ToBoolean(objIncubation.Question1_8);
                            bool que9 = Convert.ToBoolean(objIncubation.Question1_9);
                            if ((que8 == true) && (que9 == true) && (objIncubation.Additional_Information == ""))
                            {
                                IsError = true;
                                errlist.Add("Please fill in the required field: 2.3.2.4 Additional Information, if Question 1.8 and Question 1.9 answer is Yes.");
                            }
                        }

                        if (Convert.ToString(objIncubation.Question1_10) != null)
                        {
                            bool que10 = Convert.ToBoolean(objIncubation.Question1_10);
                            if ((que10 == true) && (objIncubation.Additional_Information == ""))
                            {
                                IsError = true;
                                errlist.Add("Please fill in the required field: 2.3.2.4 Additional Information, if Question 1.10 answer is Yes.");
                            }
                        }
                        if (Convert.ToString(objIncubation.Question1_10) != "")
                        {
                            if (objIncubation.Question1_10.Value == true && objIncubation.Additional_Information == "")
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_Addidtiona_Info"));
                            }
                        }
                        */
                        if (objIncubation.Proposed_Products == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Propsed_Products"));
                        }
                        if (objIncubation.Target_Market == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Target_Market"));
                        }
                        if (objIncubation.Competition_Analysis == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Competition_Analysis"));
                        }
                        if (objIncubation.Revenus_Model == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Revenue_Model"));
                        }

                        if (objIncubation.First_6_Months_Milestone == "" || objIncubation.Second_6_Months_Milestone == "" || objIncubation.Third_6_Months_Milestone == ""
                            || objIncubation.Forth_6_Months_Milestone == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Project_Milestone"));
                        }

                        //if (objIncubation.Second_6_Months_Milestone == "")
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 2.4.6 Second 6 months Milestone.");
                        //}
                        //if (objIncubation.Third_6_Months_Milestone == "")
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 2.4.6 Third 6 months Milestone.");
                        //}
                        //if (objIncubation.Forth_6_Months_Milestone == "")
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 2.4.6 Forth 6 months Milestone.");
                        //}
                        if (Convert.ToString(objIncubation.Resubmission) == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Resubmission"));
                        }
                        if (string.IsNullOrEmpty(objIncubation.Company_Type) || objIncubation.Company_Type.Trim() == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Company_Type"));
                        }
                        if (!string.IsNullOrEmpty(objIncubation.Company_Type))
                        {
                            if (objIncubation.Company_Type.ToLower().Trim() == "others")
                            {
                                if (string.IsNullOrEmpty(objIncubation.Other_Company_Type))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Other_Company_Type"));
                                }
                            }
                        }
                        if (objIncubation.Business_Area == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Business_Area"));
                        }
                        if (objIncubation.Business_Area != "")
                        {
                            if (objIncubation.Business_Area.ToLower().Trim() == "others")
                            {
                                if (string.IsNullOrEmpty(objIncubation.Other_Bussiness_Area))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Other_Business_Area"));
                                }
                            }
                        }
                        if (objIncubation.Positioning == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Positioning"));
                        }


                        if (objIncubation.Positioning != "")
                        {
                            string[] Positioning = objIncubation.Positioning.Split(';');
                            foreach (string strPos in Positioning.Where(x => !string.IsNullOrEmpty(x)))
                            {

                                if (strPos == "Management / trading / service")
                                {
                                    if (string.IsNullOrEmpty(objIncubation.Management_Positioning))
                                    {
                                        IsError = true;
                                        errlist.Add(Localize("Error_Positioning_Management"));
                                    }
                                }
                                if (strPos.ToLower() == "others")
                                {
                                    if (string.IsNullOrEmpty(objIncubation.Other_Positioning))
                                    {
                                        IsError = true;
                                        errlist.Add(Localize("Error_Positioning_Management_others"));
                                    }
                                }

                            }
                        }
                        if (objIncubation.Preferred_Track == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Preferred_track"));
                        }


                        //if (Convert.ToBoolean(objIncubation.Resubmission) == true)
                        //{
                        //    if (objIncubation.Resubmission_Project_Reference == "" || objIncubation.Resubmission_Main_Differences == "")
                        //    {
                        //        IsError = true;
                        //        errlist.Add(Localize("Error_Resubmission_option"));
                        //    }
                        //}


                        //if ((objIncubation.Question1_8 == true || objIncubation.Question1_9 == true) && string.IsNullOrEmpty(objIncubation.Additional_Information.Trim()))
                        //{
                        //    IsError = true;
                        //    errlist.Add("2.3.2.4 Additional Information can not be empty.");
                        //}
                        //if (objIncubation.Core_Members_Profiles == null)
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 3.2.1 Core Members' Profiles.");
                        //}



                        if (Convert.ToString(objIncubation.Resubmission) != "")
                        {

                            if ((bool)objIncubation.Resubmission)
                            {

                                if (string.IsNullOrEmpty(objIncubation.Resubmission_Main_Differences))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Resubmission_option"));

                                }


                            }
                        }
                        if (objIncubation.Company_Ownership_Structure == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Company_structure"));
                        }
                        foreach (TB_APPLICATION_COMPANY_CORE_MEMBER obj1 in Objcoremembers)
                        {
                            if (obj1.Name == "" || obj1.Position == "" || string.IsNullOrEmpty(obj1.HKID)
                                || string.IsNullOrEmpty(obj1.CoreMember_Profile))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_core_member"));
                            }
                        }
                        if (objTB_APPLICATION_ATTACHMENT != null)
                        {
                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Company_Ownership_Structure.ToString().ToLower()))
                            {
                                IsError = true;

                                errlist.Add(Localize("Error_company_ownership_structure"));
                            }
                        }
                        if (Objcoremembers.Count == 0 && objIncubation.Major_Partners_Profiles == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_atleast_coremember"));
                        }
                        else if (Objcoremembers.Count == 0)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_atleast_coremember"));
                        }
                        if (objIncubation.Major_Partners_Profiles == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Major_Partners"));
                        }
                        if (objIncubation.Manpower_Distribution == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Manpower_Distribution"));
                        }
                        if (objIncubation.Equipment_Distribution == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_equipment_distribution"));
                        }
                        if (objIncubation.Other_Direct_Costs == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_other_direct_cost"));
                        }
                        if (objIncubation.Forecast_Income == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Forecast_income"));
                        }
                        if (ObjTB_APPLICATION_CONTACT_DETAIL.Count == 0)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_atleast_contact"));
                        }
                        int i = 1;
                        foreach (TB_APPLICATION_CONTACT_DETAIL obj1 in ObjTB_APPLICATION_CONTACT_DETAIL)
                        {
                            if (obj1.First_Name_Eng == "" && obj1.Last_Name_Eng == "" && obj1.Position == ""
                                && obj1.Contact_No_Home == "" && obj1.Contact_No_Mobile == "" && obj1.Contact_No_Office == "" && obj1.Email == ""
                                && obj1.Mailing_Address == "")
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_Contact_fillall"));
                                break;
                            }

                            if (string.IsNullOrEmpty(obj1.First_Name_Eng))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_First_Name_Eng"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Last_Name_Eng))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Last_Name_Eng"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Position))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Position"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Email))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Email"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Mailing_Address))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Mailing_Address"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Contact_No_Home))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Contact_Home"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Contact_No_Mobile))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Contact_Mobile"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj1.Contact_No_Office))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Contact_Office"), i.ToString()));
                            }

                            i++;

                        }
                        //if (Convert.ToString(objIncubation.Question1_4) != "")
                        //{
                        //    Activation = Convert.ToBoolean(objIncubation.Question1_4);

                        //    if (Activation == true)
                        //    {
                        if (objTB_APPLICATION_ATTACHMENT != null)
                        {
                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.CI_COPY.ToString().ToLower()))
                            {
                                IsError = true;

                                errlist.Add(Localize("Error_CI_Copy"));
                            }
                        }
                        else
                        {
                            IsError = true;

                            errlist.Add(Localize("Error_CI_Copy"));
                        }
                        //    }
                        //}


                        if (objIncubation.Have_Read_Statement == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Personal_information"));
                        }

                        if (objIncubation.Declaration == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Declaration"));
                        }
                        //if (objIncubation.Marketing_Information == false)
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please answer if you have read statements in page 5");
                        //}
                        if (objIncubation.Principal_Full_Name == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Applicant_fullname"));
                        }
                        if (objIncubation.Principal_Position_Title == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Position_Applicant"));
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                errlist.Add(ex.Message);

            }
            //if (IsError == true)
            //{
            lblgrouperror.DataSource = errlist;
            lblgrouperror.DataBind();
            ShowbottomMessage("", false);
            //}
            //else
            //{
            //    ShowbottomMessage("", true);
            //}
            return IsError;

        }

        protected void gv_CONTACT_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hdnSalutation = ((HiddenField)e.Row.FindControl("hdnSalutation")).Value;
                //hdnSalutation
                DropDownList Salutation = (DropDownList)e.Row.FindControl("Salutation");
                Salutation.SelectedValue = hdnSalutation;

                string hdnNationality = ((HiddenField)e.Row.FindControl("hdnNationality")).Value;
                DropDownList ddlNationality = (DropDownList)e.Row.FindControl("ddlContactNationality");
                CBPCommonConstants oCommonCOnstant = new CBPCommonConstants();
                ddlNationality.DataSource = oCommonCOnstant.GetNationalityList;
                ddlNationality.DataBind();
                //hdnSalutation
                if (ddlNationality.Items.FindByValue(hdnNationality) != null)
                {
                    ddlNationality.SelectedValue = hdnNationality;
                }
                else
                {
                    ddlNationality.SelectedValue = defaultNationality;
                }

            }
        }

        protected TB_INCUBATION_APPLICATION GetExistingIncubation(CyberportEMS_EDM dbContext, int programId)
        {
            TB_INCUBATION_APPLICATION objIncubation = null;
            List<TB_INCUBATION_APPLICATION> objIncubations = new List<TB_INCUBATION_APPLICATION>();
            Guid objUserProgramId;
            if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
            else
                objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();
            TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = new TB_PROGRAMME_INTAKE();
            objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);
            if (objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
            {
                if (Context.Request.QueryString["type"] == "fed46de9f573")
                {
                    objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Incubation_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                    if (objIncubation != null)
                    {
                        objIncubations = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == programId && !string.IsNullOrEmpty(x.Application_Parent_ID) && x.Application_Parent_ID.ToLower() == objUserProgramId.ToString().ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                        if (objIncubations.Count > 0)
                        {
                            objIncubation = objIncubations.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                        }
                        else objIncubation = dbContext.TB_INCUBATION_APPLICATION.OrderByDescending(x => x.Modified_Date).FirstOrDefault(x => x.Programme_ID == programId && x.Incubation_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                    }
                    else
                    {
                        DisableControls();
                        pnlRestricted.Visible = true;
                    }
                    if (objIncubation == null)
                        Context.Response.Redirect("~/SitePages/Home.aspx");
                    else
                    {
                        UserCollaborator();

                    }
                }
                else
                {
                    if (Context.Request.QueryString["new"] == "Y")
                    {
                        objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());

                    }
                    else
                    {
                        objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Incubation_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                        if (objIncubation != null)
                        {
                            if (objTB_PROGRAMME_INTAKE.Application_Deadline < DateTime.Now)
                            {
                                objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (string.IsNullOrEmpty(x.Application_Parent_ID)) && (x.Created_By.ToLower() == strCurrentUser) && !(x.Status == formsubmitaction.Saved.ToString() || x.Status == formsubmitaction.Deleted.ToString()));
                            }
                            else
                            {

                                objIncubations = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == programId && !string.IsNullOrEmpty(x.Application_Parent_ID) && x.Created_By.ToLower() == strCurrentUser.ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                                if (objIncubations.Count > 0)
                                {
                                    objIncubation = objIncubations.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                                }
                                else
                                    objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());
                            }

                        }
                        else
                        {
                            DisableControls();
                            pnlRestricted.Visible = true;
                        }
                    }

                }
            }
            else
            {
                //objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                //if (objIncubations.Count > 0)
                //{
                //    objIncubation = objIncubations.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                //}
                //else

                //    objIncubation = dbContext.TB_INCUBATION_APPLICATION.OrderByDescending(x => x.Modified_Date).FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()));
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                    objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);


                objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && x.Incubation_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                UserCollaborator();

            }
            if (objIncubation != null)
            {
                hdn_ApplicationID.Value = objIncubation.Incubation_ID.ToString();
            }
            return objIncubation;
        }

        protected TB_INCUBATION_APPLICATION GetDatabyParentId(CyberportEMS_EDM dbContext, Guid ParentId)
        {
            TB_INCUBATION_APPLICATION objIncubation = null;

            objIncubation = dbContext.TB_INCUBATION_APPLICATION.OrderBy(x => x.Modified_Date).FirstOrDefault(x => x.Incubation_ID == ParentId);
            return objIncubation;
        }

        protected void UserCollaborator()
        {
            btn_Submit.Enabled = false;
        }

        protected void quicklnk_1_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            ShowHideControlsBasedUponUserData();
            //using (var dbContext = new CyberportEMS_EDM())
            //{

            //    int progId = Convert.ToInt32(hdn_ProgramID.Value);

            //    SPFunctions objfunction = new SPFunctions();
            //    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);
            //    if (objIncubation == null)
            //    {
            //        SaveStep1Data();
            //    }
            //}
            SetPanelVisibilityOfStep(Convert.ToInt32(id));


        }

        protected void SaveAttachment_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SPFunctions objfunction = new SPFunctions();
            string FileName = string.Empty;
            bool IsError = false;
            string ErrorMessage = string.Empty;
            var argument = ((ImageButton)sender).CommandName;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    if (objIncubation == null)
                    {
                        objIncubation = new TB_INCUBATION_APPLICATION();
                        objIncubation.Incubation_ID = NewProgramId();
                        hdn_ApplicationID.Value = objIncubation.Incubation_ID.ToString();
                        objIncubation.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
                        //int count = (dbContext.TB_INCUBATION_APPLICATION.Where(x=>x.Programme_ID == objIncubation.Programme_ID).Count() + 1);
                        int count = 0;
                        int programId = Convert.ToInt32(hdn_ProgramID.Value);
                        var result = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == programId).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                        if (result != null)
                        {
                            count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                        }
                        else
                        {
                            count = 1;
                        }
                        lblApplicationNo.Text = HttpUtility.HtmlEncode(dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                        objIncubation.Application_Number = lblApplicationNo.Text;
                        objIncubation.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                        objIncubation.Applicant = objfunction.GetCurrentUser();
                        objIncubation.Last_Submitted = DateTime.Now;
                        objIncubation.Status = "Saved";
                        objIncubation.Version_Number = "0.01";
                        objIncubation.Created_By = objfunction.GetCurrentUser();
                        objIncubation.Created_Date = DateTime.Now;
                        objIncubation.Modified_By = objfunction.GetCurrentUser();
                        objIncubation.Modified_Date = DateTime.Now;
                        dbContext.TB_INCUBATION_APPLICATION.Add(objIncubation);
                        dbContext.SaveChanges();
                        objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    }
                    if (objIncubation != null)
                    {
                        if (objIncubation.Submission_Date.HasValue && objIncubation.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                        {
                            ReSubmitVersionCopy();
                            objIncubation = GetExistingIncubation(dbContext, progId);
                            decimal inc = (decimal)0.01;
                            objIncubation.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + inc).ToString("F2");
                            //objIncubation.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal(".01")).ToString();
                            dbContext.SaveChanges();
                        }


                        //SaveAttachment(fu_BrCopy, enumAttachmentType.BR_COPY, objIncubation.CCMF_ID, progId);
                        SPFunctions objSPFunctions = new SPFunctions();

                        string _fileUrl = string.Empty;
                        switch (Convert.ToInt32(argument))
                        {
                            case 1:
                                if (fu_BrCopy.HasFile)
                                {

                                    if (fu_BrCopy.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_BrCopy.FileName.Remove(0, fu_BrCopy.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                    fu_BrCopy, enumAttachmentType.CI_COPY, objIncubation.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.CI_COPY, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                            BRCOPY.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            BRCOPY.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        BRCOPY.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    BRCOPY.Text = Localize("Error_file_upload");
                                }
                                break;

                            case 2:
                                if (fu_AnnualReturn.HasFile)
                                {
                                    if (fu_AnnualReturn.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_AnnualReturn.FileName.Remove(0, fu_AnnualReturn.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {

                                            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                    fu_AnnualReturn, enumAttachmentType.Company_Annual_Return, objIncubation.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Company_Annual_Return, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                            AnnualReturn.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            AnnualReturn.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        AnnualReturn.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    AnnualReturn.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 3:
                                if (fuPresentationSlide.HasFile)
                                {
                                    if (fuPresentationSlide.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fuPresentationSlide.FileName.Remove(0, fuPresentationSlide.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {

                                            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                            fuPresentationSlide, enumAttachmentType.Presentation_Slide, objIncubation.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Presentation_Slide, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                            PresentationSlide.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            PresentationSlide.Text = Localize("File_type_presentation");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        PresentationSlide.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    PresentationSlide.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 4:
                                if (fuOtherAttachement.HasFile)
                                {
                                    if (fuOtherAttachement.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fuOtherAttachement.FileName.Remove(0, fuOtherAttachement.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objIncubation.Incubation_ID).ToList();
                                            attachments = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Other_Attachment.ToString().ToLower()).ToList();

                                            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                        fuOtherAttachement, enumAttachmentType.Other_Attachment, attachments.Count().ToString());
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Other_Attachment, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                            OtherAttachement.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            OtherAttachement.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        OtherAttachement.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    OtherAttachement.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 5:
                                if (fu_Company_Ownership_2.HasFile)
                                {
                                    if (fu_Company_Ownership_2.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_Company_Ownership_2.FileName.Remove(0, fu_Company_Ownership_2.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {

                                            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                    fu_Company_Ownership_2, enumAttachmentType.Company_Ownership_Structure, objIncubation.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Company_Ownership_Structure, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                            lblcompanyownership.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lblcompanyownership.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lblcompanyownership.Text = Localize("File_size");
                                    }

                                }
                                else
                                {
                                    IsError = true;
                                    lblcompanyownership.Text = Localize("Error_file_upload");
                                }
                                break;
                        }

                    }

                }

            }

            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);

            }
        }
        private void InitializeUploadsDocument()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    if (objIncubation != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objIncubation.Incubation_ID).ToList();
                        rptrbrcopy.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.CI_COPY.ToString().ToLower());
                        rptrbrcopy.DataBind();
                        rptrannual.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Company_Annual_Return.ToString().ToLower());
                        rptrannual.DataBind();
                        rptrpresentation.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Presentation_Slide.ToString().ToLower());
                        rptrpresentation.DataBind();
                        rptrOtherAttachement.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Other_Attachment.ToString().ToLower());
                        rptrOtherAttachement.DataBind();
                        rptrcompanyownership.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Company_Ownership_Structure.ToString().ToLower());
                        rptrcompanyownership.DataBind();

                        if (attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Video_Clip.ToString().ToLower()).Count() > 0)
                            txtVideoClip.Text = attachments.FirstOrDefault(x => x.Attachment_Type.ToLower() == enumAttachmentType.Video_Clip.ToString().ToLower()).Attachment_Path;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        private Guid NewProgramId()
        {
            Guid objNewId = Guid.NewGuid();
            while (new CyberportEMS_EDM().TB_INCUBATION_APPLICATION.Where(x => x.Incubation_ID == objNewId).Count() == 0)
            {
                objNewId = Guid.NewGuid();
                break;
            }
            return objNewId;
        }
        protected void ReSubmitVersionCopy()
        {

            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    SPFunctions objfunction = new SPFunctions();
                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);
                    TB_INCUBATION_APPLICATION objIncubationNew = objIncubation;
                    Guid objinbubationExitingId = objIncubation.Incubation_ID;
                    dbContext.TB_INCUBATION_APPLICATION.Add(objIncubationNew);
                    objIncubationNew.Submission_Date = null;
                    objIncubationNew.Modified_Date = DateTime.Now;
                    objIncubationNew.Last_Submitted = DateTime.Now;
                    //objIncubationNew.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal("1")).ToString("F2");
                    objIncubationNew.Status = "Saved";
                    objIncubationNew.Application_Parent_ID = !string.IsNullOrEmpty(objIncubation.Application_Parent_ID) ? objIncubation.Application_Parent_ID : objIncubation.Incubation_ID.ToString();
                    objIncubationNew.Incubation_ID = NewProgramId();
                    dbContext.Entry(objIncubationNew).State = System.Data.Entity.EntityState.Added;


                    List<TB_APPLICATION_ATTACHMENT> objIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_ATTACHMENT.AddRange(objIncubationAttachement);
                    foreach (TB_APPLICATION_ATTACHMENT objAttach in objIncubationAttachement)
                    {
                        objAttach.Application_ID = objIncubationNew.Incubation_ID;
                    }
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.AddRange(objTB_APPLICATION_COMPANY_CORE_MEMBER);
                    foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objAttach in objTB_APPLICATION_COMPANY_CORE_MEMBER)
                    {
                        objAttach.Application_ID = objIncubationNew.Incubation_ID;
                    }
                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_CONTACT_DETAIL.AddRange(objTB_APPLICATION_CONTACT_DETAIL);
                    foreach (TB_APPLICATION_CONTACT_DETAIL objAttach in objTB_APPLICATION_CONTACT_DETAIL)
                    {
                        objAttach.Application_ID = objIncubationNew.Incubation_ID;
                    }
                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_FUNDING_STATUS.AddRange(objTB_APPLICATION_FUNDING_STATUS);
                    foreach (TB_APPLICATION_FUNDING_STATUS objAttach in objTB_APPLICATION_FUNDING_STATUS)
                    {
                        objAttach.Application_ID = objIncubationNew.Incubation_ID;
                    }


                    dbContext.SaveChanges();
                    InitializeUploadsDocument();

                }


            }
            catch (Exception ex)
            {
            }
        }

        //protected void rbtnPanel1Q4_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selectedValue = rbtnPanel1Q4.SelectedValue;
        //    if (Convert.ToBoolean(selectedValue) == false)
        //    {
        //        lblQ15.Enabled = true;
        //        lblQ15td.Enabled = true;
        //        rbtnPanel1Q5.Enabled = true;
        //        rbtnPanel1Q5.SelectedIndex = -1;
        //    }
        //    else
        //    {
        //        lblQ15.Enabled = false;
        //        lblQ15td.Enabled = false;
        //        rbtnPanel1Q5.Enabled = false;
        //        rbtnPanel1Q5.SelectedIndex = -1;
        //    }
        //}

        //protected void rbtnPanel1Q8_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selectedValue = rbtnPanel1Q8.SelectedValue;
        //    if (Convert.ToBoolean(selectedValue) == true)
        //    {
        //        lblQ19.Enabled = true;
        //        lblQ19td.Enabled = true;
        //        rbtnPanel1Q9.Enabled = true;
        //        rbtnPanel1Q9.SelectedIndex = -1;
        //    }
        //    else
        //    {
        //        lblQ19td.Enabled = false;
        //        lblQ19.Enabled = false;
        //        rbtnPanel1Q9.Enabled = false;
        //        rbtnPanel1Q9.SelectedIndex = -1;

        //    }

        //}

        protected void btn_ContactsAddNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddNewContactDetails();
        }
        private void AddNewContactDetails()
        {
            bool IsError = false;
            lblcontactdetails.Text = string.Empty;


            List<TB_APPLICATION_CONTACT_DETAIL> objContactDetails = new List<TB_APPLICATION_CONTACT_DETAIL>();


            for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
            {
                try
                {


                    HiddenField contactId = (HiddenField)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("CONTACT_DETAILS_ID");
                    TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                    TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                    TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactPostition");
                    TextBox Contact_No_Home = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoHome");
                    TextBox Contact_No_Office = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoOffice");
                    TextBox Contact_No_Mobile = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoMobile");
                    TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                    TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                    DropDownList Nationality = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("ddlContactNationality");
                    TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                    DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");


                    TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                    objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                    objMember.Last_Name_Eng = txtContactLast_name.Text;
                    objMember.First_Name_Eng = txtContactFirst_name.Text;
                    objMember.Position = Position.Text;
                    objMember.Contact_No_Home = Contact_No_Home.Text;
                    objMember.Contact_No_Office = Contact_No_Office.Text;
                    objMember.Contact_No_Mobile = Contact_No_Mobile.Text;
                    if (Fax.Text != "")
                    {
                        objMember.Fax = Fax.Text;
                    }
                    objMember.Mailing_Address = Mailing_Address.Text;
                    objMember.Salutation = Salutation.Text;
                    objMember.Email = Email.Text;
                    objMember.Nationality = Nationality.SelectedValue;
                    objContactDetails.Add(objMember);




                }
                catch (Exception ex)
                {
                    IsError = true;
                    lblcontactdetails.Text = "Please fill in Last Name, First Name, Saluation, Position, Contact No., Contact No. Type, Email, Nationality and Mailing Address in " + (i + 1).ToString() + "  Contact Detailss, or leave all fields blank to remove the contact. ";
                    break;

                }
            }
            if (IsError == false)
            {
                objContactDetails.Add(new TB_APPLICATION_CONTACT_DETAIL() { CONTACT_DETAILS_ID = 0 });


                gv_CONTACT_DETAIL.DataSource = objContactDetails;
                gv_CONTACT_DETAIL.DataBind();
            }

            //  SetPreviousData();
        }

        protected void Grd_FundingStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hdnCurrency = ((HiddenField)e.Row.FindControl("hdnCurrency")).Value;

                DropDownList Currency = (DropDownList)e.Row.FindControl("Currency");
                Currency.SelectedValue = hdnCurrency;

            }
        }
        protected int check_db_validations(bool IsSubmitClick)
        {

            List<String> ErrorLIst = new List<string>();
            using (var dbContext = new CyberportEMS_EDM())
            {

                int progId = Convert.ToInt32(hdn_ProgramID.Value);

                SPFunctions objfunction = new SPFunctions();
                TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                                                                                                   //added
                TB_PROGRAMME_INTAKE objprogram = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(k => k.Programme_ID == progId);
                if (objprogram.Application_Deadline >= DateTime.Now || objIncubation.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                {
                    bool isnewobj = false;
                    if (objIncubation == null)
                    {
                        isnewobj = true;
                        objIncubation = new TB_INCUBATION_APPLICATION();
                    }
                    else if (objIncubation.Submission_Date.HasValue && objIncubation.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                    {
                        ReSubmitVersionCopy();
                        objIncubation = GetExistingIncubation(dbContext, progId);
                    }

                    if (!string.IsNullOrEmpty(rbtnPanel1Q1.SelectedValue))
                        objIncubation.Question1_1 = Convert.ToBoolean(rbtnPanel1Q1.SelectedValue);

                    if (!string.IsNullOrEmpty(rbtnPanel1Q2.SelectedValue))
                        objIncubation.Question1_2 = Convert.ToBoolean(rbtnPanel1Q2.SelectedValue);
                    if (!string.IsNullOrEmpty(rbtnPanel1Q3.SelectedValue))
                        objIncubation.Question1_3 = Convert.ToBoolean(rbtnPanel1Q3.SelectedValue);
                    if (!string.IsNullOrEmpty(rbtnPanel1Q4.SelectedValue))
                        objIncubation.Question1_4 = Convert.ToBoolean(rbtnPanel1Q4.SelectedValue);
                    if (!string.IsNullOrEmpty(rbtnPanel1Q5.SelectedValue))
                        objIncubation.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
                    else
                        objIncubation.Question1_5 = null;
                    if (!string.IsNullOrEmpty(rbtnPanel1Q6.SelectedValue))
                        objIncubation.Question1_6 = Convert.ToBoolean(rbtnPanel1Q6.SelectedValue);
                    if (!string.IsNullOrEmpty(rbtnPanel1Q7.SelectedValue))
                        objIncubation.Question1_7 = Convert.ToBoolean(rbtnPanel1Q7.SelectedValue);
                    if (!string.IsNullOrEmpty(rbtnPanel1Q8.SelectedValue))
                        objIncubation.Question1_8 = Convert.ToBoolean(rbtnPanel1Q8.SelectedValue);
                    if (!string.IsNullOrEmpty(rbtnPanel1Q9.SelectedValue))
                        objIncubation.Question1_9 = Convert.ToBoolean(rbtnPanel1Q9.SelectedValue);
                    else
                        objIncubation.Question1_9 = null;
                    //if (!string.IsNullOrEmpty(rbtnPanel1Q8_1.SelectedValue))
                    //    objIncubation.Question1_8_1 = Convert.ToBoolean(rbtnPanel1Q8_1.SelectedValue);
                    //if (!string.IsNullOrEmpty(rbtnPanel1Q10.SelectedValue))
                    //    objIncubation.Question1_10 = Convert.ToBoolean(rbtnPanel1Q10.SelectedValue);
                    objIncubation.Company_Name_Eng = txtCompanyNameEnglish.Text.Trim();
                    if (objIncubation.Company_Name_Eng.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Company_Name_Eng))
                    {
                        ErrorLIst.Add(Localize("Error_company_name_length"));
                    }

                    //if (!string.IsNullOrEmpty(txtProjectName.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtProjectName.Text))
                    //    ErrorLIst.Add(Localize("Error_ProjectName_length"));
                    //else objIncubation.Project_Name = txtProjectName.Text.Trim();


                    if (!string.IsNullOrEmpty(txtWebsiteName.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtWebsiteName.Text))
                        ErrorLIst.Add(Localize("Error_Website_length"));
                    else objIncubation.Website = txtWebsiteName.Text.Trim();

                    if (txtestablishmentyear.Text != "")
                    {
                        DateTime dDate;
                        if (DateTime.TryParse(txtestablishmentyear.Text, out dDate))
                        {
                            String.Format("{0:M-yy}", dDate);
                            objIncubation.Establishment_Year = Convert.ToDateTime(txtestablishmentyear.Text.Trim());
                        }
                        else
                        {
                            ErrorLIst.Add(Localize("Error_Year_of_EstablishmentInvalid"));
                        }

                    }
                    //objIncubation.Establishment_Year = Convert.ToDateTime(txtestablishmentyear.Text.Trim());
                    //if (rdbNEWHK.SelectedValue != "")
                    //{
                    //    objIncubation.NEW_to_HK = Convert.ToBoolean(rdbNEWHK.SelectedValue);
                    //}

                    objIncubation.Country_Of_Origin = ddlCountryOrigin.SelectedValue;


                    objIncubation.Company_Name_Chi = txtCompanyNameChinese.Text.Trim();
                    if (objIncubation.Company_Name_Chi.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Company_Name_Chi))
                    {
                        ErrorLIst.Add(Localize("Error_company_name_chinese"));
                    }
                    var text = txtAbstractEnglish.Text.Trim();
                    int wordCount = 0, index = 0;

                    while (index < text.Length)
                    {
                        // check if current char is part of a word
                        while (index < text.Length && !char.IsWhiteSpace(text[index]))
                            index++;
                        wordCount++;
                        // skip whitespace until next word
                        while (index < text.Length && char.IsWhiteSpace(text[index]))
                            index++;
                    }
                    if (wordCount > 300)
                        ErrorLIst.Add(Localize("Error_Abstract_english_length"));
                    else objIncubation.Abstract = txtAbstractEnglish.Text.Trim();
                    var text1 = TxtAbstractChinese.Text.Trim();
                    int wordCount1 = 0, index1 = 0;

                    while (index1 < text1.Length)
                    {
                        // check if current char is part of a word
                        while (index1 < text1.Length && !char.IsWhiteSpace(text1[index1]))
                            index1++;

                        wordCount1++;

                        // skip whitespace until next word
                        while (index1 < text1.Length && char.IsWhiteSpace(text1[index1]))
                            index1++;
                    }
                    if (wordCount1 > 500)
                        ErrorLIst.Add(Localize("Error_Abstract_chinese_length"));
                    else
                        objIncubation.Abstract_Chi = TxtAbstractChinese.Text.Trim();
                    //    if (txtAbstractEnglish.Text.Trim().Length > 300)
                    //  ErrorLIst.Add("Abstract(English) should be within 300 words.");
                    //else objIncubation.Abstract = txtAbstractEnglish.Text.Trim();
                    //    if (TxtAbstractChinese.Text.Trim().Length > 300)
                    //        ErrorLIst.Add("Abstract(Chinese) should be within 300 words.");
                    //    else
                    //        objIncubation.Abstract_Chi = TxtAbstractChinese.Text.Trim();
                    objIncubation.Objective = txtObjective.Text.Trim();
                    objIncubation.Background = txtbackground.Text.Trim();
                    objIncubation.Pilot_Work_Done = txtPilot_Work_Done.Text.Trim();
                    objIncubation.Additional_Information = txtAdditionalInformation.Text.Trim();
                    objIncubation.Proposed_Products = txtProposedProducts.Text.Trim();
                    objIncubation.Target_Market = txtTargetMarket.Text.Trim();
                    objIncubation.Competition_Analysis = txtCompetitionAnalysis.Text.Trim();
                    objIncubation.Revenus_Model = txtRevenueModel.Text.Trim();
                    objIncubation.First_6_Months_Milestone = txtFirst6Months.Text.Trim();
                    objIncubation.Second_6_Months_Milestone = txtSecond6Months.Text.Trim();
                    objIncubation.Third_6_Months_Milestone = txtThird6Months.Text.Trim();
                    objIncubation.Forth_6_Months_Milestone = txtForth6Months.Text.Trim();
                    objIncubation.Exit_Strategy = txtExitStrategy.Text.Trim();
                    if (rbtnResubmission.SelectedValue != "")
                    {
                        objIncubation.Resubmission = Convert.ToBoolean(rbtnResubmission.SelectedValue);
                        if ((bool)objIncubation.Resubmission)
                        {
                            //commented on 26102018 cpip changes
                            //objIncubation.Resubmission_Project_Reference = txtResubmission_Project_Reference.Text.Trim(); 
                            objIncubation.Resubmission_Main_Differences = txtResubmission_Main_Differences.Text.Trim();
                        }
                    }
                    objIncubation.Company_Type = rbtnCompany_Type.SelectedValue.Trim();
                    if (objIncubation.Company_Type.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 10, true, AllowAllSymbol: true), objIncubation.Company_Type))
                        ErrorLIst.Add(Localize("Error_company_type_length"));
                    if (rbtnCompany_Type.SelectedValue.ToLower() == "others")
                    {
                        objIncubation.Other_Company_Type = txtOther_Company_Type.Text.Trim();
                        if (objIncubation.Other_Company_Type.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Other_Company_Type))
                            ErrorLIst.Add(Localize("Error_company_name_other"));
                    }


                    objIncubation.Business_Area = rbtnList_Business_Area.SelectedValue.Trim();
                    if (objIncubation.Business_Area.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 40, true, AllowAllSymbol: true), objIncubation.Business_Area))
                        ErrorLIst.Add(Localize("Error_Business_Area_length"));
                    if (rbtnList_Business_Area.SelectedValue.ToLower() == "others")
                    {
                        objIncubation.Other_Bussiness_Area = txtOther_Bussiness_Area.Text.Trim();
                        if (objIncubation.Other_Bussiness_Area.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 40, true, AllowAllSymbol: true), objIncubation.Other_Bussiness_Area))
                            ErrorLIst.Add(Localize("Error_Busniess_Area_Other_lenth"));
                    }

                    string Positioning = string.Empty;
                    string PositioningOther = string.Empty;
                    string ManagementOther = string.Empty;
                    foreach (ListItem objList in chkPositioning.Items)
                    {
                        if (objList.Selected)
                        {
                            Positioning += objList.Value.Trim() + ";";
                            if (objList.Value.ToLower() == "others")
                            {
                                PositioningOther = txtPositioningOther.Text.Trim();

                            }
                            if (objList.Value.ToLower() == "management / trading / service")
                            {
                                ManagementOther = txtManagementOther.Text.Trim();

                            }
                        }
                    }
                    objIncubation.Positioning = Positioning;
                    objIncubation.Other_Positioning = PositioningOther;
                    if (objIncubation.Other_Positioning.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Other_Positioning))
                        ErrorLIst.Add(Localize("Error_positioning_other_lenghth"));
                    objIncubation.Management_Positioning = ManagementOther;
                    if (objIncubation.Management_Positioning.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Management_Positioning))
                        ErrorLIst.Add(Localize("Error_positioning_management_lenghth"));
                    objIncubation.Other_Attributes = txtOtherAttributes.Text.Trim();
                    objIncubation.Preferred_Track = rbtnPreferred_Track.SelectedValue;
                    if (objIncubation.Preferred_Track.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 30, true, AllowAllSymbol: true), objIncubation.Preferred_Track))
                        ErrorLIst.Add(Localize("Error_preferred_length"));
                    objIncubation.Company_Ownership_Structure = txtCompany_Ownership_1.Text.Trim();


                    objIncubation.Major_Partners_Profiles = txtPartner_Profiles.Text.Trim();
                    objIncubation.Manpower_Distribution = Manpower_Distribution.Text.Trim();
                    objIncubation.Equipment_Distribution = Equipment_Distribution.Text.Trim();
                    objIncubation.Other_Direct_Costs = Other_Costs.Text.Trim();
                    objIncubation.Forecast_Income = Forecast_Income.Text.Trim();
                    objIncubation.Principal_Full_Name = txtName_PrincipalApplicant.Text.Trim();
                    if (objIncubation.Principal_Full_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Principal_Full_Name))
                        ErrorLIst.Add(Localize("Error_Principal_title_length"));
                    objIncubation.Principal_Position_Title = txtPosition_PrincipalApplicant.Text.Trim();
                    if (objIncubation.Principal_Position_Title.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objIncubation.Principal_Position_Title))
                        ErrorLIst.Add(Localize("Error_principal_position_length"));

                    objIncubation.Declaration = chkDeclaration.Checked;
                    objIncubation.Marketing_Information = Marketing_Information.Checked;
                    objIncubation.Have_Read_Statement = Personal_Information.Checked;

                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = GetFundingStatusForSave(IsSubmitClick, ref ErrorLIst);
                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = GetContactDetailsForSave(IsSubmitClick, ref ErrorLIst);
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = GetCoreMemberForSave(IsSubmitClick, ref ErrorLIst);
                    if (txtVideoClip.Text.Length > 0 && !Common.CBPRegularExpression.RegExValidate(CBPRegularExpression.Url, txtVideoClip.Text.Trim()))
                        ErrorLIst.Add(Localize("Error_Video_link_url"));
                    else if (txtVideoClip.Text.Length > 0)
                        SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, objIncubation.Incubation_ID, progId);
                    try
                    {
                        objIncubation.Last_Submitted = DateTime.Now;
                        objIncubation.Modified_By = objfunction.GetCurrentUser();
                        objIncubation.Modified_Date = DateTime.Now;

                        if (ErrorLIst.Count == 0)
                        {
                            lblgrouperror.Visible = false;
                            if (isnewobj)
                            {

                                objIncubation.Incubation_ID = NewProgramId();
                                hdn_ApplicationID.Value = objIncubation.Incubation_ID.ToString();
                                objIncubation.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
                                //int count = (dbContext.TB_INCUBATION_APPLICATION.Where(x=>x.Programme_ID== objIncubation.Programme_ID).Count() + 1);
                                int count = 0;
                                int programId = Convert.ToInt32(hdn_ProgramID.Value);
                                var result = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == programId).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                                if (result != null)
                                {
                                    count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                                }
                                else
                                {
                                    count = 1;
                                }
                                lblApplicationNo.Text = HttpUtility.HtmlEncode(dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                                objIncubation.Application_Number = lblApplicationNo.Text;
                                objIncubation.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                                objIncubation.Applicant = objfunction.GetCurrentUser();
                                objIncubation.Status = "Saved";
                                objIncubation.Version_Number = "0.01";
                                objIncubation.Created_By = objfunction.GetCurrentUser();
                                objIncubation.Created_Date = DateTime.Now;
                                dbContext.TB_INCUBATION_APPLICATION.Add(objIncubation);
                            }
                            else
                            {
                                if (objIncubation.Status.ToLower().Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                {
                                    objIncubation.Status = formsubmitaction.Saved.ToString();
                                    decimal inc = (decimal)0.01;
                                    objIncubation.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + inc).ToString("F2");
                                    //objIncubation.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal(".01")).ToString();
                                }
                            }
                            dbContext.SaveChanges();

                            objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                            objTB_APPLICATION_FUNDING_STATUS.ForEach(x => x.Application_ID = objIncubation.Incubation_ID);
                            objTB_APPLICATION_FUNDING_STATUS.ForEach(x => x.Programme_ID = objIncubation.Programme_ID);
                            IncubationContext.APPLICATION_FUNDING_STATUS_ADDUPDATE(dbContext, objTB_APPLICATION_FUNDING_STATUS, objIncubation.Incubation_ID);
                            objTB_APPLICATION_CONTACT_DETAIL.ForEach(x => x.Application_ID = objIncubation.Incubation_ID);
                            objTB_APPLICATION_CONTACT_DETAIL.ForEach(x => x.Programme_ID = objIncubation.Programme_ID);
                            IncubationContext.TB_APPLICATION_CONTACTDETAILSADDUPDATE(dbContext, objTB_APPLICATION_CONTACT_DETAIL, objIncubation.Incubation_ID);
                            objTB_APPLICATION_COMPANY_CORE_MEMBER.ForEach(x => x.Application_ID = objIncubation.Incubation_ID);
                            objTB_APPLICATION_COMPANY_CORE_MEMBER.ForEach(x => x.Programme_ID = progId);
                            IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_ADDUPDATE(dbContext, objTB_APPLICATION_COMPANY_CORE_MEMBER, objIncubation.Incubation_ID);

                            dbContext.SaveChanges();
                            InitializeFundingStatus();
                            InitialCoreMembers();
                            FillContact();
                            InitializeUploadsDocument();
                            ShowbottomMessage("Saved Successfully", true);
                        }
                        else
                        {
                            lblgrouperror.Visible = true;
                            lblgrouperror.DataSource = ErrorLIst;
                            lblgrouperror.DataBind();
                        }
                    }

                    catch (Exception e)
                    { }
                    ShowHideControlsBasedUponUserData();
                }
                else
                {
                    DisableControls();
                    ErrorLIst.Add("Deadline has been passed," + Localize("Error6_Deadline_Message"));
                    ErrorLIst.Add("Current Time: " + DateTime.Now + "," + Localize("Error6_Deadline_Message"));
                    //ErrorLIst.Add(  "changes are not saved or submitted");
                    lblgrouperror.Visible = true;
                    lblgrouperror.DataSource = ErrorLIst;
                    lblgrouperror.DataBind();
                    ShowbottomMessage("", false);
                }
            }
            return ErrorLIst.Count;
        }
        //private string Localize(string Key)
        //{
        //    return SPUtility.GetLocalizedString("$Resources:" + Key, "CyberportEMS_Incubation", (uint)SPContext.Current.Web.Locale.LCID);

        //}
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Incubation");
        }
        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }


        protected void SetPanel1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SetPanelVisibilityOfStep(0);
        }


        protected void Attachments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveAttachment")
            {
                int AttachmentId = 0;
                Int32.TryParse(e.CommandArgument.ToString(), out AttachmentId);
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_APPLICATION_ATTACHMENT objAttach = dbContext.TB_APPLICATION_ATTACHMENT.FirstOrDefault(x => x.Attachment_ID == AttachmentId);
                    if (objAttach != null)
                    {
                        //SPFunctions.DeleteAttachmentfromlist(objAttach);


                        dbContext.TB_APPLICATION_ATTACHMENT.Remove(objAttach);
                        dbContext.SaveChanges();
                    }
                }
            }
            InitializeUploadsDocument();
        }

        public static string LocalizeUI(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Incubation");
        }
        public static SPListItem GetItemByBdcId(SPList list, string bdcIdentity, string IdentityName, string bdcIdentity1, string IdentityName1)
        {
            SPListItem myitem = null;
            foreach (SPListItem item in list.Items)
            {
                if (item[IdentityName].ToString() == bdcIdentity && item[IdentityName1].ToString() == bdcIdentity1)
                {
                    myitem = item;
                }
            }

            return myitem;
        }
        private void Fill_Programelist(string Application_number, int Programme_Id, int Intake, string version, string Businessarea, string status, string Applicant)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPWeb site = SPFunctions.GetCurrentWeb)
                    {
                        site.AllowUnsafeUpdates = true;
                        SPList list = site.Lists["Application List"];

                        SPListItem itemAttachment = GetItemByBdcId(list, Application_number.ToString(), "Application_Number", Programme_Id.ToString(), "Programme_ID");

                        if (itemAttachment == null)
                        {

                            itemAttachment = list.Items.Add();
                        }

                        itemAttachment["Application_Number"] = Application_number;

                        itemAttachment["Programme_ID"] = Programme_Id;
                        itemAttachment["Programme_Name"] = "Cyberport Incubation Programme";
                        itemAttachment["Intake_Number"] = Intake;
                        itemAttachment["Applicant"] = Applicant;
                        itemAttachment["Version_Number"] = version;
                        itemAttachment["Business_Area"] = Businessarea;
                        itemAttachment["Status"] = "Submitted";
                        //itemAttachment["Programme_Name_Full"] = "Cyberport Incubation Programme";
                        itemAttachment.Update();
                        site.AllowUnsafeUpdates = false;
                    }
                });

            }
            catch (Exception e)
            {

            }
            //finally
            //{
            //    Context.Response.Redirect("~/SitePages/Home.aspx", false);
            //}
        }
        protected void Grd_FundingStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                bool IsError = false;
                lbl_fundingError.Text = string.Empty;

                List<TB_APPLICATION_FUNDING_STATUS> objCoreMembers = new List<TB_APPLICATION_FUNDING_STATUS>();

                for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
                {
                    try
                    {


                        HiddenField FundingID = (HiddenField)Grd_FundingStatus.Rows[i].Cells[0].FindControl("FundingID");
                        if (i != id)
                        {
                            TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
                            TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
                            TextBox txtApplicationStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationStatus");
                            TextBox txtFundingStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtFundingStatus");
                            TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                            TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                            TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
                            DropDownList Currency = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("Currency");

                            TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                            objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                            objMember.Programme_Name = txtNameofProgram.Text;
                            objMember.Currency = Currency.SelectedValue;
                            try
                            {
                                if (txtAmountReceived.Text != "")
                                {
                                    objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);
                                }
                                if (txtApplicationMaximumAmount.Text != "")
                                {
                                    objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);
                                }
                                objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
                            }
                            catch (Exception)
                            {
                                objMember.Amount_Received = null;
                                objMember.Maximum_Amount = null;
                                objMember.Date = null;
                            }
                            objMember.Funding_Status = txtFundingStatus.Text;
                            objMember.Application_Status = txtApplicationStatus.Text;
                            objMember.Expenditure_Nature = txtNature.Text;
                            objCoreMembers.Add(objMember);
                        }

                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        lbl_fundingError.Text = ex.Message;
                    }
                }
                if (IsError == false)
                {
                    if (objCoreMembers.Count == 0)
                    {
                        objCoreMembers.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = null });
                    }
                    Grd_FundingStatus.DataSource = objCoreMembers;
                    Grd_FundingStatus.DataBind();
                }


            }
        }

        protected void gv_CONTACT_DETAIL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                bool IsError = false;
                lblcontactdetails.Text = string.Empty;

                List<TB_APPLICATION_CONTACT_DETAIL> objContactDetails = new List<TB_APPLICATION_CONTACT_DETAIL>();
                for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
                {
                    try
                    {


                        HiddenField contactId = (HiddenField)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("CONTACT_DETAILS_ID");
                        if (i != id)
                        {
                            TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                            TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                            TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactPostition");
                            TextBox Contact_No_Home = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoHome");
                            TextBox Contact_No_Office = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoOffice");
                            TextBox Contact_No_Mobile = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoMobile");
                            TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                            TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                            DropDownList Nationality = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("ddlContactNationality");
                            TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                            DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");


                            TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                            objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                            objMember.Last_Name_Eng = txtContactLast_name.Text;
                            objMember.First_Name_Eng = txtContactFirst_name.Text;
                            objMember.Position = Position.Text;
                            objMember.Contact_No_Home = Contact_No_Home.Text;
                            objMember.Contact_No_Office = Contact_No_Office.Text;
                            objMember.Contact_No_Mobile = Contact_No_Mobile.Text;
                            if (Fax.Text != "")
                            {
                                objMember.Fax = Fax.Text;
                            }
                            objMember.Mailing_Address = Mailing_Address.Text;
                            objMember.Salutation = Salutation.Text;
                            objMember.Email = Email.Text;
                            objMember.Nationality = Nationality.SelectedValue;
                            objContactDetails.Add(objMember);


                        }

                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        lblcontactdetails.Text = "Please fill in Last Name, First Name, Saluation, Position, Contact No., Contact No. Type, Email, Nationality and Mailing Address in " + (i + 1).ToString() + "  Contact Detailss, or leave all fields blank to remove the contact. ";
                        break;

                    }
                }
                if (IsError == false)
                {
                    if (objContactDetails.Count == 0)
                    {
                        objContactDetails.Add(new TB_APPLICATION_CONTACT_DETAIL() { CONTACT_DETAILS_ID = 0 });
                    }

                    gv_CONTACT_DETAIL.DataSource = objContactDetails;
                    gv_CONTACT_DETAIL.DataBind();
                }

            }
        }

        protected void grvCoreMember_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                bool IsError = false;
                lblCorememberError.Text = string.Empty;
                List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();
                for (int i = 0; i < grvCoreMember.Rows.Count; i++)
                {
                    try
                    {
                        HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
                        if (i != id)
                        {
                            TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                            TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                            TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMembersProfile");
                            TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");
                            TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                            objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                            objMember.Name = TextBoxAddress.Text;
                            objMember.Position = txtNOE.Text;
                            objMember.CoreMember_Profile = txtCoreMembersProfile.Text;
                            objMember.HKID = txtHKID.Text;
                            objCoreMembers.Add(objMember);
                        }
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        lblCorememberError.Text = ex.Message;
                    }
                }
                if (IsError == false)
                {
                    if (objCoreMembers.Count == 0)
                    {
                        objCoreMembers.Add(new TB_APPLICATION_COMPANY_CORE_MEMBER() { Core_Member_ID = 0 });
                    }
                    grvCoreMember.DataSource = objCoreMembers;
                    grvCoreMember.DataBind();

                }
            }
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;

            Context.Response.Redirect("~/SitePages/Home.aspx", false);


        }

        public string dbTextbyLanguage(string English, string HKSimplify, string HKTrad)
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "zh-HK")
            {
                return HKTrad;
            }
            else if (Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
            {
                return HKSimplify;
            }
            else
            {
                return English;
            }

        }

        public string ProcessMyDataItem(object myValue)
        {
            if (myValue == null)
            {
                return "";
            }

            return myValue.ToString().Replace(Environment.NewLine, "<br>");
        }

        protected void rptrOtherAttachement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_INCUBATION_APPLICATION objIncubation = GetExistingIncubation(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

                    SPFunctions objSp = new SPFunctions();
                    string AccessUser = lblApplicant.Text.Trim();
                    string CurrentUser = objSp.GetCurrentUser();
                    if (objIncubation.Applicant.ToLower() != objSp.GetCurrentUser().ToLower() && objIncubation.Modified_By.ToLower() != objSp.GetCurrentUser().ToLower())
                    {
                        HyperLink hypNavigation = (HyperLink)e.Item.FindControl("hypNavigation");
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        hypNavigation.Enabled = false;
                        lnkAttachmentDelete.Enabled = false;

                    }
                }
            }
        }
    }
}