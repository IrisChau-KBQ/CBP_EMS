using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Data.Models;

using Microsoft.ApplicationServer.Caching;
using Microsoft.SharePoint;
using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.CrossBorderForm.CCMF
{
    [ToolboxItemAttribute(false)]
    public partial class CCMF : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        public CCMF()
        {
        }

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            InitializeControl();
        }
        private string defaultNationality =  "Hong Kong";


        protected void Page_Load(object sender, EventArgs e)
        {

            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            // Added by Tim Ng @ 20170316
            SetUpHeader();

            btnIncubationForm.Text = SPFunctions.LocalizeUI("btn_Continue_Text", "CyberportEMS_Common");
            btn_StepNext.Text = SPFunctions.LocalizeUI("Next", "CyberportEMS_Common");
            btn_StepPrevious.Text = SPFunctions.LocalizeUI("Previous", "CyberportEMS_Common");
            btn_HideSubmitPopup.Text = SPFunctions.LocalizeUI("Cancel", "CyberportEMS_Common");
            btn_Submit.Text = SPFunctions.LocalizeUI("Submit", "CyberportEMS_Common");
            btn_submitFinal.Text = SPFunctions.LocalizeUI("Submit", "CyberportEMS_Common");
            btn_StepSave.Text = SPFunctions.LocalizeUI("Save", "CyberportEMS_Common");
            rdo_HK.Text = SPFunctions.LocalizeUI("Hong_Kong_Programme", "CyberportEMS_CCMF");
            rdo_Crossborder.Text = SPFunctions.LocalizeUI("Cross_Border_Programme_Supported_by_CCMF", "CyberportEMS_CCMF");
            rdo_CUPP.Text = SPFunctions.LocalizeUI("Cyberport_University_Partnership_Programme", "CyberportEMS_CCMF");

            string ctrlname = HttpContext.Current.Request.Params.Get("__EVENTTARGET");
            if (!Page.IsPostBack && string.IsNullOrEmpty(ctrlname))
            {
                if (Context.Request.UrlReferrer != null)
                {
                    btn_Back.PostBackUrl = Context.Request.UrlReferrer.ToString();

                }

                List<ListItem> crossborderoptions = new List<ListItem>();
                crossborderoptions.Add(new ListItem() { Value = "CrossBorder-Shenzhen", Text = SPFunctions.LocalizeUI("CyberportShenzhen_Hong_Kong_Young_Entrepreneur_Programme1", "CyberportEMS_CCMF") });
                crossborderoptions.Add(new ListItem() { Value = "CrossBorder-Guangdong", Text = SPFunctions.LocalizeUI("CyberportGuangdong__Hong_Kong_Young_Entrepreneur_Programme1", "CyberportEMS_CCMF") });
                rdo_CrossborderOptions.DataSource = crossborderoptions;
                rdo_CrossborderOptions.DataTextField = "Text";
                rdo_CrossborderOptions.DataValueField = "Value";
                rdo_CrossborderOptions.DataBind();

                List<ListItem> Contactarea = new List<ListItem>();
                Contactarea.Add(new ListItem() { Value = "HongKong", Text = SPFunctions.LocalizeUI("Contact_HongKong", "CyberportEMS_CCMF") });
                Contactarea.Add(new ListItem() { Value = "China", Text = SPFunctions.LocalizeUI("Contact_China", "CyberportEMS_CCMF") });

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
            else if (!string.IsNullOrEmpty(hdn_ProgramID.Value))
            {
                ShowHideControlsBasedUponUserData(true);
            }

            string HKSelectedValue = rdo_HK_Option.SelectedValue;

            List<ListItem> HKOptions = new List<ListItem>();
            HKOptions.Add(new ListItem() { Value = "Professional", Text = SPFunctions.LocalizeUI("Professional_Stream", "CyberportEMS_CCMF") });
            HKOptions.Add(new ListItem() { Value = "Young Entrepreneur", Text = SPFunctions.LocalizeUI("Hong_Kong_Young_Entrepreneur", "CyberportEMS_CCMF") });
            rdo_HK_Option.DataSource = HKOptions;
            rdo_HK_Option.DataTextField = "Text";
            rdo_HK_Option.DataValueField = "Value";
            rdo_HK_Option.DataBind();
            if (!string.IsNullOrEmpty(HKSelectedValue))
                rdo_HK_Option.SelectedValue = HKSelectedValue;
            //rdo_HK_Option.Items[1].Attributes.CssStyle.Add("visibility", "hidden");

            string HKSelectedValueType = rdo_CCMFApplication.SelectedValue;

            List<ListItem> CCMFApplication = new List<ListItem>();
            CCMFApplication.Add(new ListItem() { Value = "Individual", Text = SPFunctions.LocalizeUI("Individual_Application", "CyberportEMS_CCMF") });
            CCMFApplication.Add(new ListItem() { Value = "Company", Text = SPFunctions.LocalizeUI("Company_Application", "CyberportEMS_CCMF") });
            rdo_CCMFApplication.DataSource = CCMFApplication;
            rdo_CCMFApplication.DataTextField = "Text";
            rdo_CCMFApplication.DataValueField = "Value";
            rdo_CCMFApplication.DataBind();
            if (!string.IsNullOrEmpty(HKSelectedValueType))
                rdo_CCMFApplication.SelectedValue = HKSelectedValueType;

            string BusinessAreaSelected = RadioButtonList1.SelectedValue;
            List<ListItem> busniessareaoption = new List<ListItem>();
            busniessareaoption.Add(new ListItem() { Value = "AI/Big data", Text = SPFunctions.LocalizeUI("Step3_OpenData", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Application Development", Text = SPFunctions.LocalizeUI("Step3_AppDesign", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Blockchain", Text = SPFunctions.LocalizeUI("Step3_Blockchain", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Cybersecurity", Text = SPFunctions.LocalizeUI("Step3_Cybersecurity", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Edtech", Text = SPFunctions.LocalizeUI("Step3_Edutech", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "EnvironmenTech", Text = SPFunctions.LocalizeUI("Step3_EnvironmenTech", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "E-sport/Digital Entertainment", Text = SPFunctions.LocalizeUI("Step3_Gaming", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "FinTech", Text = SPFunctions.LocalizeUI("Step3_Fintech", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "HealthTech", Text = SPFunctions.LocalizeUI("Step3_Healthcare", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "MarTech", Text = SPFunctions.LocalizeUI("Step3_MarTech", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "RetailTech / E-commerce", Text = SPFunctions.LocalizeUI("Step3_ECommerce", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Robotics / IoT", Text = SPFunctions.LocalizeUI("Step3_Wearable", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Smart Building", Text = SPFunctions.LocalizeUI("Step3_Smart_Building", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Smart Mobility", Text = SPFunctions.LocalizeUI("Step3_Smart_Mobility", "CyberportEMS_CCMF") });
            busniessareaoption.Add(new ListItem() { Value = "Others", Text = SPFunctions.LocalizeUI("Step3_Others", "CyberportEMS_CCMF") });
            RadioButtonList1.DataSource = busniessareaoption;
            RadioButtonList1.DataTextField = "Text";
            RadioButtonList1.DataValueField = "Value";
            RadioButtonList1.DataBind();
            if (!string.IsNullOrEmpty(BusinessAreaSelected))
            {
                RadioButtonList1.SelectedValue = BusinessAreaSelected;
            }

            //new SmartSpace
            string smartSpace = ddlsmartspace.SelectedValue;
            List<ListItem> smartspacelist = new List<ListItem>();
            smartspacelist.Add(new ListItem() { Value = "No", Text = SPFunctions.LocalizeUI("Step_3_Smart_Space_No", "CyberportEMS_CCMF") });
            smartspacelist.Add(new ListItem() { Value = "SS8", Text = SPFunctions.LocalizeUI("Step_3_Smart_Space_SS8", "CyberportEMS_CCMF") });
            smartspacelist.Add(new ListItem() { Value = "Others", Text = SPFunctions.LocalizeUI("Step_3_Smart_Space_Others", "CyberportEMS_CCMF") });
            ddlsmartspace.DataSource = smartspacelist;
            ddlsmartspace.DataTextField = "Text";
            ddlsmartspace.DataValueField = "Value";
            ddlsmartspace.DataBind();
            if (!string.IsNullOrEmpty(smartSpace))
            {
                ddlsmartspace.SelectedValue = smartSpace;
            }



            /*Page UI Step Set*/
            if (hdn_ActiveStep.Value == "2")
            {
                if (rdo_HK.Checked)
                {
                    // Professional Stream
                    if (rdo_HK_Option.SelectedValue == "Professional")
                    {
                        if (rdo_CCMFApplication.SelectedValue == "Company")
                            switchStep2UI("hkpc");
                        else
                            switchStep2UI("hkpi");
                    }
                    // Hong Kong Young Entrepreneur Programme
                    else
                    {
                        if (rdo_CCMFApplication.SelectedValue == "Company")
                            switchStep2UI("hkyc");
                        else
                            switchStep2UI("hkyi");
                    }

                }
                else if (rdo_Crossborder.Checked)
                {
                    if (rdo_CCMFApplication.SelectedValue == "Company")
                        switchStep2UI("cbc");
                    else
                        switchStep2UI("cbi");
                }
                else if (rdo_CUPP.Checked)
                {
                    if (rdo_CCMFApplication.SelectedValue == "Company")
                        switchStep2UI("cbuc");
                    else
                        switchStep2UI("cbui");
                }
            }
            HideShowProfOrYEP();
        }

        /// <summary>
        /// Set up CCMF Header
        /// </summary>
        protected void SetUpHeader()
        {
            int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
            using (var Intake = new CyberportEMS_EDM())
            {
                TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                if (objProgram == null)
                {
                    Context.Response.Redirect("~/SitePages/Home.aspx");
                }
                else
                {
                    if (objProgram.Programme_Name.ToLower().Contains("cross border"))
                    {
                        mainpageheading.Text = SPFunctions.LocalizeUI("CCMF_CrossBorder_Header", "CyberportEMS_CCMF");
                    }
                    else if (objProgram.Programme_Name.ToLower().Contains("hong kong"))
                    {
                        mainpageheading.Text = SPFunctions.LocalizeUI("CCMF_HK_Header", "CyberportEMS_CCMF");
                    }
                    else
                    {
                        mainpageheading.Text = SPFunctions.LocalizeUI("CUPP_Header", "CyberportEMS_CCMF");
                    }
                }
            }
        }

        protected void HideShowProfOrYEP()
        {
            int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
            using (var Intake = new CyberportEMS_EDM())
            {
                TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                if (!objProgram.ProfShow  )
                {
                    rdo_HK_Option.Items[0].Attributes.CssStyle.Add("visibility", "hidden");
                    //rdo_HK_Option.Visible = false;
                   
                }
                if (!objProgram.YEPShow)
                {
                    rdo_HK_Option.Items[1].Attributes.CssStyle.Add("visibility", "hidden");
                   // rdo_HK_Option.Visible = false;

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
                    //mainpageheading.Text = objProgram.Programme_Name;

                    if (objProgram == null)
                    {
                        Context.Response.Redirect("~/SitePages/Home.aspx");
                    }
                    else
                    {
                        if (objProgram.Programme_Name.ToLower().Contains("cross border"))
                        {
                            rdo_Crossborder.Checked = true;
                            rdo_CrossborderOptions.Enabled = true;

                            rdo_HK_Option.Visible = false;
                            rdo_HK.Visible = false;
                            rdo_CUPP.Visible = false;
                            mainpagelogo.ImageUrl = "images/_layouts/15/Images/CBP_Images/Cross Border.png";
                            mainpagelogo.CssClass = "head-logo";
                        }
                        else if (objProgram.Programme_Name.ToLower().Contains("hong kong"))
                        {
                            rdo_CUPP.Visible = false;
                            rdo_Crossborder.Visible = false;
                            rdo_CrossborderOptions.Visible = false;

                            rdo_HK.Checked = true;
                            rdo_HK_Option.Enabled = true;
                            mainpagelogo.ImageUrl = "images/_layouts/15/Images/CBP_Images/HK.png";
                            mainpagelogo.CssClass = "head-logo";
                        }
                        else
                        {
                            rdo_CUPP.Enabled = true;
                            rdo_Crossborder.Visible = false;
                            rdo_CrossborderOptions.Visible = false;
                            rdo_CUPP.Checked = true;
                            rdo_HK_Option.Visible = false;
                            rdo_HK.Visible = false;
                            mainpagelogo.ImageUrl = "images/_layouts/15/Images/CBP_Images/HK.png";
                            mainpagelogo.CssClass = "head-logo";
                        }
                    }

                    lblIntake.Text = objProgram.Intake_Number.ToString();
                    lblDeadline.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
                    //lblDeadlinePopup.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
                    lblDeadlinePopup.Text = HttpUtility.HtmlEncode(dbTextbyLanguage(objProgram.Application_Deadline_Eng, objProgram.Application_Deadline_SimpChin, objProgram.Application_Deadline_TradChin));//objProgram.Application_Deadline.ToString("dd MMM yyyy");

                    hdn_ProgramID.Value = objProgram.Programme_ID.ToString();

                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(Intake, progId);
                    CBPCommonConstants oCommonFunction = new CBPCommonConstants();
                    ddlCountryOrigin.DataSource = oCommonFunction.GetNationalityList;
                    ddlCountryOrigin.DataBind();
                    ddlCountryOrigin.SelectedValue = defaultNationality;

                    //Intake.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

                    if (objIncubation != null)
                    {
                        SPFunctions objFUnction = new SPFunctions();
                        bool isDisabled = false;
                        if (objProgram.Application_Deadline <= DateTime.Now && (objIncubation.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) || !objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                        {
                            isDisabled = true;
                            DisableControls();
                            if (objSp.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"], true))
                            { btn_Back.Visible = true; }
                        }
                        else if (objIncubation.Status.Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " "))
                        {
                            btn_StepSave.Visible = false;
                        }
                        //                       else if (objIncubation.Submission_Date.HasValue)
                        //                       {
                        //                           if (HttpContext.Current.Request.QueryString["resubmitversion"] != null && HttpContext.Current.Request.QueryString["resubmitversion"].ToString() == "Y")
                        //                           {
                        //                           ReSubmitVersionCopy();
                        //                           objIncubation = GetExistingCCMF(Intake, progId);
                        //                           }
                        //                       }
                        lblApplicant.Text = SPHttpUtility.HtmlEncode(objIncubation.Created_By);
                        hdn_ApplicationID.Value = objIncubation.CCMF_ID.ToString();

                        lblApplicationNo.Text = SPHttpUtility.HtmlEncode(objIncubation.Application_Number);
                        if (objIncubation.Programme_Type.ToLower() == "hongkong")
                        {
                            rdo_HK.Checked = true;
                            rdo_HK_Option.SelectedValue = Convert.ToString(objIncubation.Hong_Kong_Programme_Stream);
                            rdo_HK_Option.Enabled = false;
                        }
                        else if (objIncubation.Programme_Type.ToLower() == "cupp")
                        {
                            rdo_CUPP.Checked = true;
                        }
                        else if (objIncubation.Programme_Type.ToLower().Contains("crossborder"))
                        {
                            if (!(objIncubation.Programme_Type.ToLower() == "crossborder"))
                            {
                                string[] crossporderoptions = objIncubation.Programme_Type.Split('-');
                                if (crossporderoptions[1].ToLower() == "shenzhen")
                                {
                                    rdo_CrossborderOptions.SelectedValue = "CrossBorder-Shenzhen";
                                }
                                else
                                {
                                    rdo_CrossborderOptions.SelectedValue = "CrossBorder-Guangdong";
                                }
                            }
                            rdo_Crossborder.Checked = true;
                        }

                        rdo_CCMFApplication.SelectedValue = Convert.ToString(objIncubation.CCMF_Application_Type);

                        if (objIncubation.Question1_3.HasValue)
                        {
                            rdo1_3.SelectedValue = Convert.ToString(objIncubation.Question1_3);
                            if (rdo1_3.SelectedItem.Value == "True")
                                div1_3Remark.Visible = true;
                            else
                                div1_3Remark.Visible = false;
                        }
                        rdo211a.SelectedValue = Convert.ToString(objIncubation.Question2_1_1a);
                        rdo211b.SelectedValue = Convert.ToString(objIncubation.Question2_1_1b);
                        rdo212a.SelectedValue = Convert.ToString(objIncubation.Question2_1_2a);
                        rdo212b.SelectedValue = Convert.ToString(objIncubation.Question2_1_2b);
                        rdo212c.SelectedValue = Convert.ToString(objIncubation.Question2_1_2c);
                        rdo212d.SelectedValue = Convert.ToString(objIncubation.Question2_1_2d);
                        rdo212e.SelectedValue = Convert.ToString(objIncubation.Question2_1_2e);
                        rdo212f.SelectedValue = Convert.ToString(objIncubation.Question2_1_2f);
                        rdo212g.SelectedValue = Convert.ToString(objIncubation.Question2_1_2g);
                        rdo212h.SelectedValue = Convert.ToString(objIncubation.Question2_1_2h);
                        rdo212f_1.SelectedValue = Convert.ToString(objIncubation.Question2_1_2f_1);

                        rdo212i.Text = Convert.ToString(objIncubation.Question2_1_2i);
                        rdo212j.Text = Convert.ToString(objIncubation.Question2_1_2j);
                        rdo211c.Text = Convert.ToString(objIncubation.Question2_1_1c);
                        English.Text = objIncubation.Project_Name_Eng;
                        lblEnglish.Text = objIncubation.Project_Name_Eng;
                        Chinese.Text = objIncubation.Project_Name_Chi;
                        lblChinese.Text = objIncubation.Project_Name_Chi;

                        txtProjectInfoAbsEng.Text = objIncubation.Abstract_Eng;
                        lblProjectInfoAbsEng.Text = string.IsNullOrEmpty(objIncubation.Abstract_Eng) ? "" : objIncubation.Abstract_Eng.Replace(Environment.NewLine, "<br>");
                        txtProjectInfoAbschi.Text = objIncubation.Abstract_Chi;
                        lblProjectInfoAbschi.Text = string.IsNullOrEmpty(objIncubation.Abstract_Chi) ? "" : objIncubation.Abstract_Chi.Replace(Environment.NewLine, "<br>");
                        //new line smartspace
                        if (!string.IsNullOrEmpty(objIncubation.SmartSpace))
                            ddlsmartspace.SelectedValue = objIncubation.SmartSpace;
                        lblsmartspace.Text = objIncubation.SmartSpace;
                        txtestablishmentyear.Text = objIncubation.Establishment_Year.HasValue ? objIncubation.Establishment_Year.Value.ToString("MMM-yyyy") : "";
                        lblestablishmentyear.Text = objIncubation.Establishment_Year.HasValue ? objIncubation.Establishment_Year.Value.ToString("MMM-yyyy") : "";

                        txtCompanyName.Text = objIncubation.Company_Name;
                        lblCompanyName.Text = objIncubation.Company_Name;

                        if (ddlCountryOrigin.Items.FindByValue(objIncubation.Country_Of_Origin) != null)
                        {
                            ddlCountryOrigin.SelectedValue = objIncubation.Country_Of_Origin.Trim();
                        }
                        else
                        {
                            ddlCountryOrigin.SelectedValue = defaultNationality;
                        }

                        rdbNEWHK.SelectedValue = Convert.ToString(objIncubation.NEW_to_HK);

                        if (!string.IsNullOrEmpty(objIncubation.Business_Area))
                        {
                            if (objIncubation.Business_Area.ToLower
                                () == "open data")
                            {
                                //RadioButtonList1.SelectedIndex = 0;
                                RadioButtonList1.SelectedValue = "AI / Big Data";
                            }
                            else
                            {
                                RadioButtonList1.SelectedValue = objIncubation.Business_Area;
                            }
                            //RadioButtonList1.SelectedValue = objIncubation.Business_Area;
                            if (objIncubation.Business_Area.ToLower() == "others")
                            {
                                txtOther_Bussiness_Area.Text = objIncubation.Other_Business_Area;
                                lblOther_Bussiness_Area.Text = objIncubation.Other_Business_Area;
                                txtOther_Bussiness_Area.Visible = true;
                                if (isDisabled)
                                {
                                    txtOther_Bussiness_Area.Visible = false;
                                    lblOther_Bussiness_Area.Visible = true;
                                }
                            }
                        }
                        if (objIncubation.Commencement_Date.HasValue)
                        {
                            txtantisdate.Text = objIncubation.Commencement_Date.Value.ToString("dd-MMM-yyyy");
                            lblantisdate.Text = objIncubation.Commencement_Date.Value.ToString("dd-MMM-yyyy");
                        }
                        if (objIncubation.Completion_Date.HasValue)
                        {
                            txtanticdate.Text = objIncubation.Completion_Date.Value.ToString("dd-MMM-yyyy");
                            lblanticdate.Text = objIncubation.Completion_Date.Value.ToString("dd-MMM-yyyy");
                        }
                        //txtpromanagteam.Text = objIncubation.Project_Management_Team;
                        txtbusinessmodelteam.Text = objIncubation.Business_Model;
                        lblbusinessmodelteam.Text = string.IsNullOrEmpty(objIncubation.Business_Model) ? "" : objIncubation.Business_Model.Replace(Environment.NewLine, "<br>");
                        txtAdvisor.Text = objIncubation.Advisor_Info;
                        lblAdvisor.Text = string.IsNullOrEmpty(objIncubation.Advisor_Info) ? "" : objIncubation.Advisor_Info.Replace(Environment.NewLine, "<br>");
                        txtcreativity.Text = objIncubation.Innovation;
                        lblcreativity.Text = string.IsNullOrEmpty(objIncubation.Innovation) ? "" : objIncubation.Innovation.Replace(Environment.NewLine, "<br>");
                        txtsocialrespon.Text = objIncubation.Social_Responsibility;
                        lblsocialrespon.Text = string.IsNullOrEmpty(objIncubation.Social_Responsibility) ? "" : objIncubation.Social_Responsibility.Replace(Environment.NewLine, "<br>");
                        txtcompanalysis.Text = objIncubation.Competition_Analysis;
                        lblcompanalysis.Text = string.IsNullOrEmpty(objIncubation.Competition_Analysis) ? "" : objIncubation.Competition_Analysis.Replace(Environment.NewLine, "<br>");
                        txtmonth1.Text = objIncubation.Project_Milestone_M1;
                        lblmonth1.Text = string.IsNullOrEmpty(objIncubation.Project_Milestone_M1) ? "" : objIncubation.Project_Milestone_M1.Replace(Environment.NewLine, "<br>");
                        txtmonth2.Text = objIncubation.Project_Milestone_M2;
                        lblmonth2.Text = string.IsNullOrEmpty(objIncubation.Project_Milestone_M2) ? "" : objIncubation.Project_Milestone_M2.Replace(Environment.NewLine, "<br>");
                        txtmonth3.Text = objIncubation.Project_Milestone_M3;
                        lblmonth3.Text = string.IsNullOrEmpty(objIncubation.Project_Milestone_M3) ? "" : objIncubation.Project_Milestone_M3.Replace(Environment.NewLine, "<br>");
                        txtmonth4.Text = objIncubation.Project_Milestone_M4;
                        lblmonth4.Text = string.IsNullOrEmpty(objIncubation.Project_Milestone_M4) ? "" : objIncubation.Project_Milestone_M4.Replace(Environment.NewLine, "<br>");
                        txtmonth5.Text = objIncubation.Project_Milestone_M5;
                        lblmonth5.Text = string.IsNullOrEmpty(objIncubation.Project_Milestone_M5) ? "" : objIncubation.Project_Milestone_M5.Replace(Environment.NewLine, "<br>");
                        txtmonth6.Text = objIncubation.Project_Milestone_M6;
                        lblmonth6.Text = string.IsNullOrEmpty(objIncubation.Project_Milestone_M6) ? "" : objIncubation.Project_Milestone_M6.Replace(Environment.NewLine, "<br>");

                        txtExitStrategy.Text = objIncubation.Exit_Stategy;
                        lblExitStrategy.Text = string.IsNullOrEmpty(objIncubation.Exit_Stategy) ? "" : objIncubation.Exit_Stategy.Replace(Environment.NewLine, "<br>");
                        txtaddinformation.Text = objIncubation.Additional_Information;
                        lbladdinformation.Text = string.IsNullOrEmpty(objIncubation.Additional_Information) ? "" : objIncubation.Additional_Information.Replace(Environment.NewLine, "<br>");

                        txtcostprojection.Text = objIncubation.Cost_Projection;
                        lblcostprojection.Text = string.IsNullOrEmpty(objIncubation.Cost_Projection) ? "" : objIncubation.Cost_Projection.Replace(Environment.NewLine, "<br>");
                        txtName_PrincipalApplicant.Text = objIncubation.Principal_Full_Name;
                        lblName_PrincipalApplicant.Text = objIncubation.Principal_Full_Name;

                        txtPosition_PrincipalApplicant.Text = objIncubation.Principal_Position_Title;
                        lblPosition_PrincipalApplicant.Text = objIncubation.Principal_Position_Title;
                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Declaration)))
                            chkDeclaration.Checked = Convert.ToBoolean(objIncubation.Declaration);
                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Marketing_Information)))
                            Marketing_Information.Checked = Convert.ToBoolean(objIncubation.Marketing_Information);
                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Have_Read_Statement)))
                            Personal_Information.Checked = Convert.ToBoolean(objIncubation.Have_Read_Statement);
                        //FillContact(objIncubation.Incubation_ID);
                        InitializeUploadsDocument();
                        if (objIncubation.Last_Submitted != null)
                            lblLastSubmitted.Text = objIncubation.Last_Submitted.ToString("dd MMM yyyy");
                        InitializeFundingStatus();
                        InitialCoreMembers();
                        FillContact();
                        if (isDisabled)
                        {
                            DisableListTextbox();
                        }
                        InitializeUploadsDocument();
                    }
                    else if (objProgram.Application_Deadline >= DateTime.Now && objProgram.Application_Start <= DateTime.Now)
                    {
                        lblApplicant.Text = objSp.GetCurrentUser();
                        hdn_ApplicationID.Value = string.Empty;
                        //FillContact(Guid.NewGuid());
                        int count = 0;

                        var result = Intake.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == progId).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                        if (result != null)
                        {
                            count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                        }
                        else
                        {
                            count = 1;
                        }
                        lblApplicationNo.Text = HttpUtility.HtmlEncode(Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                        InitializeFundingStatus();
                        InitialCoreMembers();
                        FillContact();
                        InitializeUploadsDocument();
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
                //Context.Response.Redirect("~/SitePages/Home.aspx");
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
            fu_BrCopy.Enabled = false;
            fu_HKID.Enabled = false;
            //ss8changes   //   fu_StudentID.Enabled = false;
            btnOtherAttachmnets.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnbr.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnHKID.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            //ss8changes    //   btnstudentid.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnPresentation.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
            btnHKID.Enabled = false;
            btnbr.Enabled = false;
            btnOtherAttachmnets.Enabled = false;
            btnPresentation.Enabled = false;
            //ss8changes  //    btnstudentid.Enabled = false;
            pnl_IncubationStep1.Enabled = false;
            pnl_IncubationStep1.Enabled = false;
            pnl_IncubationStep2.Enabled = false;
            pnl_IncubationStep3.Enabled = false;
            pnl_IncubationStep4.Enabled = false;
            pnl_IncubationStep5.Enabled = false;
            pnl_IncubationStep6.Enabled = false;
            btn_Submit.Visible = false;
            btn_submitFinal.Visible = false;
            btn_StepSave.Visible = false;
            btn_Submit.Enabled = false;

            ShowLabel();
        }

        private void ShowLabel()
        {

            /* Show label instead of textbox when disabled*/
            txtProjectInfoAbsEng.Visible = false;
            lblProjectInfoAbsEng.Visible = true;

            txtProjectInfoAbschi.Visible = false;
            lblProjectInfoAbschi.Visible = true;

            txtAdvisor.Visible = false;
            lblAdvisor.Visible = true;

            txtbusinessmodelteam.Visible = false;
            lblbusinessmodelteam.Visible = true;

            txtcreativity.Visible = false;
            lblcreativity.Visible = true;

            txtsocialrespon.Visible = false;
            lblsocialrespon.Visible = true;

            txtcompanalysis.Visible = false;
            lblcompanalysis.Visible = true;

            txtmonth1.Visible = false;
            txtmonth2.Visible = false;
            txtmonth3.Visible = false;
            txtmonth4.Visible = false;
            txtmonth5.Visible = false;
            txtmonth6.Visible = false;

            lblmonth1.Visible = true;
            lblmonth2.Visible = true;
            lblmonth3.Visible = true;
            lblmonth4.Visible = true;
            lblmonth5.Visible = true;
            lblmonth6.Visible = true;

            txtExitStrategy.Visible = false;
            lblExitStrategy.Visible = true;

            txtcostprojection.Visible = false;
            lblcostprojection.Visible = true;

            txtaddinformation.Visible = false;
            lbladdinformation.Visible = true;

            txtanticdate.Visible = false;
            lblanticdate.Visible = true;
            txtantisdate.Visible = false;
            lblantisdate.Visible = true;

            English.Visible = false;
            lblEnglish.Visible = true;
            Chinese.Visible = false;
            lblChinese.Visible = true;

            txtName_PrincipalApplicant.Visible = false;
            lblName_PrincipalApplicant.Visible = true;

            txtPosition_PrincipalApplicant.Visible = false;
            lblPosition_PrincipalApplicant.Visible = true;
            ddlsmartspace.Visible = false;
            lblsmartspace.Visible = true;

            txtCompanyName.Visible = txtestablishmentyear.Visible = imgEstablishmentyear.Visible = false;
            lblCompanyName.Visible = lblestablishmentyear.Visible = true;
            ddlCountryOrigin.Enabled = false;
            rdbNEWHK.Enabled = false;
        }

        private void DisableListTextbox()
        {
            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {

                TextBox txtBackgroundinfo = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Backgroundinfo");
                Label lblBackgroundinfo = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblBackgroundinfo");

                TextBox txtFullname = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                Label lblFullName = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblName");

                TextBox txtTitle = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                Label lblTitle = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblTitle");

                TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");
                Label lblHKID = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblHKID");

                TextBox txtEmail = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtEmail");
                Label lblEmail = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblEmail");

                DropDownList txtNationality = (DropDownList)grvCoreMember.Rows[i].Cells[0].FindControl("ddlContactNationality");
                Label lblNationality = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblNationality");

                txtBackgroundinfo.Visible = false;
                lblBackgroundinfo.Visible = true;

                txtFullname.Visible = false;
                lblFullName.Visible = true;

                txtTitle.Visible = false;
                lblTitle.Visible = true;

                txtHKID.Visible = false;
                lblHKID.Visible = true;

                txtEmail.Visible = false;
                lblEmail.Visible = true;

                txtNationality.Visible = false;
                lblNationality.Visible = true;
            }

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
                lblNameofProgram.Visible = true;
                txtApplicationDate.Visible = false;
                lblApplicationDate.Visible = true;
                txtApplicationStatus.Visible = false;
                lblApplicationStatus.Visible = true;
                txtFundingStatus.Visible = false;
                lblFundingStatus.Visible = true;
                txtNature.Visible = false;
                lblNature.Visible = true;
                txtAmountReceived.Visible = false;
                lblAmountReceived.Visible = true;
                txtApplicationMaximumAmount.Visible = false;
                lblApplicationMaximumAmount.Visible = true;

            }

            for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
            {
                TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                TextBox txtlastchiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtlast_chiname");
                TextBox txtFisrtChiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtFisrt_Chiname");
                TextBox txtContactNo = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNo");
                TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                TextBox Nameofinsititute = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Nameofinsititute");
                TextBox Studentidcard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Studentidcard");
                TextBox Programmerolled = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Programmerolled");
                TextBox Dateofgraduation = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Dateofgraduation");
                TextBox OrganisationName = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("OrganisationName");
                TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Position");

                Label lblContactLast_name = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactLast_name");
                Label lblContactFirst_name = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactFirst_name");
                Label lbllastchiname = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lbllast_chiname");
                Label lblFisrtChiname = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblFisrt_Chiname");
                Label lblContactNo = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactNo");
                Label lblFax = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactFax");
                Label lblEmail = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactEmail");
                Label lblMailing_Address = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactAddress");
                Label lblNameofinsititute = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblNameofinsititute");
                Label lblStudentidcard = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblStudentidcard");
                Label lblProgrammerolled = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblProgrammerolled");
                Label lblDateofgraduation = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblDateofgraduation");
                Label lblOrganisationName = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblOrganisationName");
                Label lblPosition = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblPosition");

                txtContactLast_name.Visible = false;
                txtContactFirst_name.Visible = false;
                txtlastchiname.Visible = false;
                txtFisrtChiname.Visible = false;
                txtContactNo.Visible = false;
                Fax.Visible = false;
                Email.Visible = false;
                Mailing_Address.Visible = false;
                Nameofinsititute.Visible = false;
                Studentidcard.Visible = false;
                Programmerolled.Visible = false;
                Dateofgraduation.Visible = false;
                OrganisationName.Visible = false;
                Position.Visible = false;

                lblContactLast_name.Visible = true;
                lblContactFirst_name.Visible = true;
                lbllastchiname.Visible = true;
                lblFisrtChiname.Visible = true;
                lblContactNo.Visible = true;
                lblFax.Visible = true;
                lblEmail.Visible = true;
                lblMailing_Address.Visible = true;
                lblNameofinsititute.Visible = true;
                lblStudentidcard.Visible = true;
                lblProgrammerolled.Visible = true;
                lblDateofgraduation.Visible = true;
                lblOrganisationName.Visible = true;
                lblPosition.Visible = true;
            }


        }


        protected void btnIncubationForm_Click(object sender, EventArgs e)
        {
            SetPanelVisibilityOfStep(1);
        }
        protected void btn_StepPrevious_Click(object sender, EventArgs e)
        {

            lblgrouperror.Visible = false;
            lbl_Exception.InnerHtml = "";

            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) - 1);
            if (Convert.ToInt16(hdn_ActiveStep.Value) > 0)
            {
                ShowHideControlsBasedUponUserData();

            }

        }
        protected void btn_StepNext_Click(object sender, EventArgs e)
        {
            bool IsError = false;
            lbl_Exception.InnerHtml = "";
            lblgrouperror.Visible = false;

            if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2)
            {
                if (rdo_CCMFApplication.SelectedValue == "" && ((!rdo_HK.Checked) || (!rdo_Crossborder.Checked)))
                {
                    ShowbottomMessage("Select at least one option", false);
                    IsError = true;
                }
                else if (rdo_HK.Checked)
                {
                    if (rdo_HK_Option.SelectedValue == "")
                    {
                        ShowbottomMessage("Select an options from Hong Kong Programme", false);
                        IsError = true;
                    }
                }
                else if (rdo_Crossborder.Checked)
                {
                    if (rdo_CrossborderOptions.SelectedValue == "")
                    {
                        ShowbottomMessage("Select an options from Crossborder Programme", false);
                        IsError = true;
                    }
                }

                if (!IsError)
                    SaveStep1Data();
            }

            //else if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 4)
            //{

            //    InitializeFundingStatus();
            //}
            //else
            //if ((Convert.ToInt16(hdn_ActiveStep.Value) + 1) == 3)
            //{
            //    InitialCoreMembers();
            //}
            //else if ((Convert.ToInt16(hdn_ActiveStep.Value) + 1) == 3)
            //{
            //    FillContact();
            //}
            if (IsError == false)
            {
                SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) + 1);
            }
            ShowHideControlsBasedUponUserData();
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_CCMF");
        }
        protected void btn_StepSave_Click(object sender, EventArgs e)
        {
            bool IsError = false;
            lblgrouperror.Visible = false;
            lbl_Exception.InnerHtml = "";
            int ActiveStep = Convert.ToInt16(hdn_ActiveStep.Value);

            if (ActiveStep > 0)
            {
                quicklnk_1.CssClass = "";
                quicklnk_2.CssClass = "";
                quicklnk_3.CssClass = "";
                quicklnk_4.CssClass = "";
                quicklnk_5.CssClass = "";
                quicklnk_6.CssClass = "";
                for (int i = ActiveStep; i > 0; i--)
                {
                    ((LinkButton)(this.FindControl("quicklnk_" + i))).CssClass = "active";
                }
            }

            check_db_validations(false);
            if (ActiveStep == 1)
            {
                if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2)
                {
                    if (rdo_CCMFApplication.SelectedValue == "" && ((!rdo_HK.Checked) || (!rdo_Crossborder.Checked) || (!rdo_CUPP.Checked)))
                    {
                       // ShowbottomMessage(Localize("Error_CCMF_oprtions"), false);
                        IsError = true;
                    }
                    else if (rdo_HK.Checked)
                    {
                        if (rdo_HK_Option.SelectedValue == "")
                        {
                            //ShowbottomMessage(Localize("Error_hongkong_options"), false);
                            IsError = true;
                        }
                        else if ((rdo_CCMFApplication.SelectedValue != "" && ((rdo_HK.Checked && rdo_HK_Option.SelectedValue != "") || (rdo_Crossborder.Checked))))
                        {
                            SaveStep1Data();
                        }
                    }
                    else if (rdo_Crossborder.Checked)
                    {
                        if (rdo_CrossborderOptions.SelectedValue == "")
                        {
                            ShowbottomMessage(Localize("Error_crossborder_options"), false);
                            IsError = true;
                        }
                        else if ((rdo_CCMFApplication.SelectedValue != "" && ((rdo_Crossborder.Checked && rdo_CrossborderOptions.SelectedValue != "") || (rdo_HK.Checked))))
                        {
                            SaveStep1Data();
                        }
                    }
                    else if (rdo_CCMFApplication.SelectedValue != "" && (rdo_CUPP.Checked))
                    {
                        SaveStep1Data();
                    }
                }
            }
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
            //    SaveStep4Data();
            //}
            //else if (ActiveStep == 5)
            //{
            //    SaveStep5Data(out IsError);
            //}
            //else if (ActiveStep == 6)
            //{
            //    SaveStep6Data();
            //}
            ShowHideControlsBasedUponUserData();

        }
        protected void SetPanelVisibilityOfStep(int ActiveStep)
        {
            ShowbottomMessage("", true);
            lblgrouperror.Visible = false;
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
                quicklnk_6.CssClass = "";
                for (int i = ActiveStep; i > 0; i--)
                {
                    ((LinkButton)(this.FindControl("quicklnk_" + i))).CssClass = "active";
                }
                //foreach (ListItem objList in progressList.Items)
                //{
                //    if (Convert.ToInt32(objList.Value) <= ActiveStep)
                //    {
                //        objList.Attributes.Add("class", "active");
                //    }
                //    else
                //        objList.Attributes.Remove("class");

                //}
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
                        pnl_IncubationStep6.Visible = false;
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
                        pnl_IncubationStep6.Visible = false;
                        //switchStep2UI();
                        if (rdo_HK.Checked) //HKP_Pro_Young_Comp_1_A
                        {
                            // Professional Stream
                            if (rdo_HK_Option.SelectedValue == "Professional")
                            {
                                if (rdo_CCMFApplication.SelectedValue == "Company")
                                    switchStep2UI("hkpc");
                                else
                                    switchStep2UI("hkpi");
                            }
                            // Hong Kong Young Entrepreneur Programme
                            else
                            {
                                if (rdo_CCMFApplication.SelectedValue == "Company")
                                    switchStep2UI("hkyc");
                                else
                                    switchStep2UI("hkyi");
                            }

                        }
                        else if (rdo_Crossborder.Checked)
                        {
                            if (rdo_CCMFApplication.SelectedValue == "Company")
                                switchStep2UI("cbc");
                            else
                                switchStep2UI("cbi");
                        }
                        else if (rdo_CUPP.Checked)
                        {
                            if (rdo_CCMFApplication.SelectedValue == "Company")
                                switchStep2UI("cbuc");
                            else
                                switchStep2UI("cbui");
                        }
                    }
                    break;
                case 3:
                    {
                        pnl_IncubationStep3.Visible = true;
                        pnl_IncubationStep6.Visible = false;
                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep5.Visible = false;
                    }
                    break;
                case 4:
                    {
                        pnl_IncubationStep4.Visible = true;
                        pnl_IncubationStep6.Visible = false;
                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        pnl_IncubationStep5.Visible = false;
                        //InitializeFundingStatus();
                    }
                    break;
                case 5:
                    {
                        pnl_IncubationStep5.Visible = true;
                        pnl_IncubationStep6.Visible = false;
                        pnl_InstructionForm.Visible = false;
                        pnl_IncubationStep2.Visible = false;
                        pnl_IncubationStep3.Visible = false;
                        pnl_IncubationStep4.Visible = false;
                        pnl_IncubationStep1.Visible = false;
                        btn_StepNext.Visible = true;
                        btn_Submit.Visible = false;
                        //FillContact();
                    }
                    break;
                case 6:
                    {
                        pnl_IncubationStep5.Visible = false;
                        pnl_IncubationStep6.Visible = true;
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
                    TextBox txtBootcampno = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Bootcampno");
                    TextBox txtBackgroundinfo = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Backgroundinfo");

                    TextBox txtEmail = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtEmail");
                    DropDownList ddlContactNationality = (DropDownList)grvCoreMember.Rows[i].Cells[0].FindControl("ddlContactNationality");

                    TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");

                    TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                    objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                    objMember.Name = TextBoxAddress.Text;
                    objMember.Position = txtNOE.Text;
                    objMember.Bootcamp_Eligible_Number = txtBootcampno.Text;
                    objMember.Background_Information = txtBackgroundinfo.Text;
                    objMember.Email = txtEmail.Text;
                    objMember.Nationality = ddlContactNationality.SelectedValue;
                    objMember.HKID = txtHKID.Text;
                    objCoreMembers.Add(objMember);
                }
                catch (Exception)
                {

                    IsError = true;
                    lblCorememberError.Text = "Fill Correct values in " + (i + 1).ToString() + " Project Management ";
                    break;
                }
            }
            if (IsError == false)
            {
                objCoreMembers.Add(new TB_APPLICATION_COMPANY_CORE_MEMBER() { Core_Member_ID = 0 });

                grvCoreMember.DataSource = objCoreMembers;
                grvCoreMember.DataBind();

            }
        }

        private List<TB_APPLICATION_COMPANY_CORE_MEMBER> GetCoreMemberForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            bool IsError = false;
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();

            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                string titleerror = "Core member" + (i + 1) + " : ";
                try
                {

                    HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
                    TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                    TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                    TextBox txtBootcampno = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Bootcampno");
                    TextBox txtBackgroundinfo = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Backgroundinfo");
                    TextBox txtEmail = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtEmail");

                    DropDownList ddlContactNationality = (DropDownList)grvCoreMember.Rows[i].Cells[0].FindControl("ddlContactNationality");

                    TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");
                    if (TextBoxAddress.Text != "" || txtNOE.Text != "" || txtBootcampno.Text != "" || txtBackgroundinfo.Text != "" || txtHKID.Text != "")
                    {
                        TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                        objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                        if ((TextBoxAddress.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), TextBoxAddress.Text))
                                         || (IsSubmitClick && TextBoxAddress.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), TextBoxAddress.Text)))
                            objerror.Add(titleerror + Localize("Error_core_name"));
                        else objMember.Name = TextBoxAddress.Text;

                        if ((txtNOE.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNOE.Text))
                            || (IsSubmitClick && txtNOE.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNOE.Text)))
                            objerror.Add(titleerror + Localize("Error_core_position"));
                        else objMember.Position = txtNOE.Text;

                        if (txtEmail.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtEmail.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_email"));
                        else objMember.Email = txtEmail.Text;

                        objMember.Nationality = ddlContactNationality.SelectedValue;

                        if ((txtHKID.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtHKID.Text))
                            || (IsSubmitClick && txtHKID.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtHKID.Text)))
                            objerror.Add(titleerror + Localize("Error_core_hkid"));
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
                        if (rdo_CUPP.Checked || rdo_Crossborder.Checked)
                        {
                            //if ((txtBootcampno.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtBootcampno.Text))
                            //    || (IsSubmitClick && txtBootcampno.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtBootcampno.Text)))
                            //    objerror.Add(titleerror + Localize("Error_core_bootcamp"));
                            //else 
                            objMember.Bootcamp_Eligible_Number = txtBootcampno.Text;
                        }
                        objMember.Background_Information = txtBackgroundinfo.Text;

                        objCoreMembers.Add(objMember);
                    }
                }
                catch (Exception ex)
                {
                    objerror.Add(titleerror + " " + ex.Message);
                }
            }
            return objCoreMembers;
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
                    objMember.Application_ID = Guid.Parse(hdn_ApplicationID.Value);

                    objCoreMembers.Add(objMember);
                }
                catch (Exception)
                {
                    IsError = true;
                    lbl_fundingError.Text = "Fill all the record in funding " + (i + 1).ToString();
                    break;
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
            bool IsError = false;
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
                            objErrors.Add(titleerror + Localize("Error_Funding_Programmename"));
                        else objMember.Programme_Name = txtNameofProgram.Text;

                        if ((txtApplicationStatus.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtApplicationStatus.Text))
                            || (IsSubmitClick && txtApplicationStatus.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtApplicationStatus.Text))
                            )
                            objErrors.Add(titleerror + Localize("Error_Funding_appstatus"));
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
                                objErrors.Add(Localize("Error_Funding_date") + (i + 1)); // <-- Control flow goes here
                            }
                        }
                        else
                            objErrors.Add(titleerror + Localize("Error_Funding_daterequired"));

                        if ((txtFundingStatus.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtFundingStatus.Text))
                            || (IsSubmitClick && txtFundingStatus.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtFundingStatus.Text))
                            )
                            objErrors.Add(titleerror + Localize("Error_funding_status"));
                        else objMember.Funding_Status = txtFundingStatus.Text;

                        if ((txtNature.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNature.Text)) ||
                            (IsSubmitClick && txtNature.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNature.Text))
                            )
                            objErrors.Add(titleerror + Localize("Error_funding_expenditure"));
                        else objMember.Expenditure_Nature = txtNature.Text;

                        if ((txtAmountReceived.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmountReceived.Text))
                            || (IsSubmitClick && txtAmountReceived.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmountReceived.Text)))
                            objErrors.Add(titleerror + Localize("Error_Funding_Amount"));
                        else if (txtAmountReceived.Text.Length > 0)
                            objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);

                        if ((txtApplicationMaximumAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtApplicationMaximumAmount.Text))
                            || (IsSubmitClick && txtApplicationMaximumAmount.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtApplicationMaximumAmount.Text)))
                            objErrors.Add(titleerror + Localize("Error_Funding_MaximumAmount"));
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

        protected void SaveStep1Data()
        {
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();
            if (objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
            {
                string ErrorMessage = string.Empty;
                SPFunctions objSp = new SPFunctions();
                bool IsError = false;
                try
                {
                    using (var dbContext = new CyberportEMS_EDM())
                    {
                        int progId = Convert.ToInt32(hdn_ProgramID.Value);
                        TB_PROGRAMME_INTAKE objProgram = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                        TB_CCMF_APPLICATION objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

                        if (objCCMF == null && objProgram.Application_Deadline >= DateTime.Now)
                        {
                            TB_CCMF_APPLICATION objTB_CCMF_APPLICATION = new TB_CCMF_APPLICATION();
                            objTB_CCMF_APPLICATION.CCMF_ID = Guid.NewGuid();
                            hdn_ApplicationID.Value = objTB_CCMF_APPLICATION.CCMF_ID.ToString();

                            objTB_CCMF_APPLICATION.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);

                            //int count = (dbContext.TB_INCUBATION_APPLICATION.Where(x=>x.Programme_ID== objTB_CCMF_APPLICATION.Programme_ID).Count() + 1);
                            int count = 0;
                            int programID = Convert.ToInt32(hdn_ProgramID.Value);
                            var result = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programID).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                            if (result != null)
                            {
                                count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                            }
                            else
                            {
                                count = 1;
                            }
                            lblApplicationNo.Text = HttpUtility.HtmlEncode(dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                            objTB_CCMF_APPLICATION.Application_Number = lblApplicationNo.Text;
                            objTB_CCMF_APPLICATION.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                            objTB_CCMF_APPLICATION.Applicant = objSp.GetCurrentUser();
                            objTB_CCMF_APPLICATION.Last_Submitted = DateTime.Now;
                            objTB_CCMF_APPLICATION.Status = "Saved";

                            objTB_CCMF_APPLICATION.Version_Number = "0.01";
                            //if (rdo_HK.Checked)
                            //    objTB_CCMF_APPLICATION.Programme_Type = "HongKong";
                            //if (rdo_Crossborder.Checked)
                            //    objTB_CCMF_APPLICATION.Programme_Type = "CrossBorder";
                            //if (rdo_HK_Option.SelectedValue != "")
                            //    objTB_CCMF_APPLICATION.Hong_Kong_Programme_Stream = (rdo_HK_Option.SelectedValue);
                            //if (rdo_CCMFApplication.SelectedValue != "")
                            //    objTB_CCMF_APPLICATION.CCMF_Application_Type = rdo_CCMFApplication.SelectedValue;
                            if (rdo_HK.Checked)
                            {
                                objTB_CCMF_APPLICATION.Programme_Type = "HongKong";
                                if (rdo_HK_Option.SelectedValue != "")
                                {
                                    rdo_HK_Option.Enabled = false;
                                    objTB_CCMF_APPLICATION.Hong_Kong_Programme_Stream = (rdo_HK_Option.SelectedValue);
                                }
                            }
                            else if (rdo_Crossborder.Checked)
                            {
                                if (rdo_CrossborderOptions.SelectedValue != "")
                                {
                                    objTB_CCMF_APPLICATION.Programme_Type = rdo_CrossborderOptions.SelectedValue;
                                }
                            }
                            else if (rdo_CUPP.Checked)
                                objTB_CCMF_APPLICATION.Programme_Type = "CUPP";
                            if (rdo_CCMFApplication.SelectedValue != "")
                                objTB_CCMF_APPLICATION.CCMF_Application_Type = rdo_CCMFApplication.SelectedValue;

                            objTB_CCMF_APPLICATION.Created_By = objSp.GetCurrentUser();
                            objTB_CCMF_APPLICATION.Created_Date = DateTime.Now;
                            objTB_CCMF_APPLICATION.Modified_By = objSp.GetCurrentUser();
                            objTB_CCMF_APPLICATION.Modified_Date = DateTime.Now;
                            if (IsError == false)
                            {
                                dbContext.TB_CCMF_APPLICATION.Add(objTB_CCMF_APPLICATION);
                                dbContext.SaveChanges();
                                ShowbottomMessage("Saved Successfully", true);
                            }
                            else
                            {
                                lbl_Exception.InnerHtml = ErrorMessage;
                            }
                        }
                        //else if ((objProgram.Application_Deadline <= DateTime.Now && objCCMF.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) || objProgram.Application_Deadline >= DateTime.Now)
                        else if ((objProgram.Application_Deadline <= DateTime.Now && objCCMF.Status.Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) || objProgram.Application_Deadline >= DateTime.Now)
                        {
                            TB_CCMF_APPLICATION objTB_CCMF_APPLICATION = GetExistingCCMF(dbContext, progId);
                            if (objCCMF.Submission_Date.HasValue && objCCMF.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                            {
                                ReSubmitVersionCopy();
                                objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                                decimal inCreament = (decimal)0.01;
                                objCCMF.Version_Number =  (Convert.ToDecimal(objCCMF.Version_Number) + inCreament).ToString("F2");
                                dbContext.SaveChanges();
                            }
                            hdn_ApplicationID.Value = objCCMF.CCMF_ID.ToString();
                            if (rdo_HK.Checked)
                            {
                                objTB_CCMF_APPLICATION.Programme_Type = "HongKong";
                                if (rdo_HK_Option.SelectedValue != "")
                                    objTB_CCMF_APPLICATION.Hong_Kong_Programme_Stream = (rdo_HK_Option.SelectedValue);
                            }
                            else if (rdo_Crossborder.Checked)
                            {
                                if (rdo_CrossborderOptions.SelectedValue != "")
                                    objTB_CCMF_APPLICATION.Programme_Type = rdo_CrossborderOptions.SelectedValue;
                            }
                            else if (rdo_CUPP.Checked)
                                objTB_CCMF_APPLICATION.Programme_Type = "CUPP";
                            if (rdo_CCMFApplication.SelectedValue != "")
                                objTB_CCMF_APPLICATION.CCMF_Application_Type = rdo_CCMFApplication.SelectedValue;
                            objCCMF.Modified_Date = DateTime.Now;

                            //if (objCCMF.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ") && objCCMF.Status.Replace("_", " ") != formsubmitaction.Resubmitted_information.ToString().Replace("_", " "))
                            //{
                            //    objCCMF.Status = formsubmitaction.Saved.ToString();
                            //}

                            //objCCMF.Status = "Saved";

                            dbContext.SaveChanges();
                            //ShowbottomMessage("Saved Successfully", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowbottomMessage(ex.Message, false);
                }
            }
        }

        protected void SaveStep4Data()
        {
            string ErrorMessage = string.Empty;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    // Guid appId = Guid.Parse(hdn_ApplicationID.Value);

                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);//dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    if (objIncubation != null)
                    {
                        bool IsError = false;

                        List<TB_APPLICATION_FUNDING_STATUS> objFunds = new List<TB_APPLICATION_FUNDING_STATUS>(); /* GetFundingStatusForSave(out IsError)*/;
                        List<TB_APPLICATION_COMPANY_CORE_MEMBER> objFunds1 = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();  /*GetCoreMemberForSave(out IsError);*/



                        if (IsError == false)
                        {
                            objFunds.ForEach(x => x.Application_ID = objIncubation.CCMF_ID);
                            objFunds.ForEach(x => x.Programme_ID = objIncubation.Programme_ID);
                            IncubationContext.APPLICATION_FUNDING_STATUS_ADDUPDATE(dbContext, objFunds, objIncubation.CCMF_ID);
                            if (objFunds1.Count > 0)
                            {
                                objFunds1.ForEach(x => x.Application_ID = objIncubation.CCMF_ID);
                                objFunds1.ForEach(x => x.Programme_ID = progId);
                                objFunds1.ForEach(x => x.HKID = x.Position);
                                foreach (TB_APPLICATION_COMPANY_CORE_MEMBER item in objFunds1)
                                    if (item.Position == "" || item.Name == "")
                                    {
                                        IsError = true;
                                        ErrorMessage += "Position and Name cannot be empty in Funding status";

                                    }
                                IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_ADDUPDATE(dbContext, objFunds1, objIncubation.CCMF_ID);
                            }

                            //objIncubation.Project_Management_Team = txtpromanagteam.Text.Trim();
                            objIncubation.Business_Model = txtbusinessmodelteam.Text.Trim();
                            objIncubation.Advisor_Info = txtAdvisor.Text.Trim();

                            objIncubation.Innovation = txtcreativity.Text.Trim();

                            objIncubation.Social_Responsibility = txtsocialrespon.Text.Trim();
                            objIncubation.Competition_Analysis = txtcompanalysis.Text.Trim();
                            objIncubation.Project_Milestone_M1 = txtmonth1.Text.Trim();

                            objIncubation.Project_Milestone_M2 = txtmonth2.Text.Trim();
                            objIncubation.Project_Milestone_M3 = txtmonth3.Text.Trim();
                            objIncubation.Project_Milestone_M4 = txtmonth4.Text.Trim();
                            objIncubation.Project_Milestone_M5 = txtmonth5.Text.Trim();
                            objIncubation.Project_Milestone_M6 = txtmonth6.Text.Trim();
                            objIncubation.Cost_Projection = txtcostprojection.Text.Trim();
                            objIncubation.Exit_Stategy = txtExitStrategy.Text.Trim();
                            objIncubation.Additional_Information = txtaddinformation.Text.Trim();




                            if (IsError == false)
                            {

                                dbContext.SaveChanges();
                                ShowbottomMessage("Saved Successfully", true);
                                //InitializeFundingStatus();
                            }
                            else
                            {
                                ShowbottomMessage(ErrorMessage, false);
                            }
                        }
                    }
                    else
                    {
                        ShowbottomMessage("Could not found the relevent data.", false);
                    }
                }

            }

            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);
            }
        }


        protected void SaveStep3Data()
        {
            string ErrorMessage = string.Empty;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                    if (objIncubation != null)
                    {
                        bool IsError = false;
                        string FileName = string.Empty;
                        objIncubation.SmartSpace = ddlsmartspace.SelectedValue;
                        objIncubation.Project_Name_Eng = English.Text.Trim();
                        objIncubation.Project_Name_Chi = Chinese.Text.Trim();
                        objIncubation.Abstract_Eng = txtProjectInfoAbsEng.Text.Trim();
                        objIncubation.Abstract_Chi = txtProjectInfoAbschi.Text.Trim();
                        objIncubation.Business_Area = RadioButtonList1.SelectedValue;
                        objIncubation.Company_Name = txtCompanyName.Text.Trim();
                        if (txtestablishmentyear.Text != "")
                        {
                            DateTime dDate;
                            if (DateTime.TryParse(txtestablishmentyear.Text, out dDate))
                            {
                                String.Format("{0:M-yy}", dDate);
                                objIncubation.Establishment_Year = Convert.ToDateTime(txtestablishmentyear.Text.Trim());
                            }

                        }
                        objIncubation.NEW_to_HK = Convert.ToBoolean(rdbNEWHK.SelectedValue);

                        if (RadioButtonList1.SelectedValue.ToLower() == "others")
                        {
                            objIncubation.Other_Business_Area = txtOther_Bussiness_Area.Text.Trim();
                        }
                        if (txtantisdate.Text != "")
                        {
                            objIncubation.Commencement_Date = Convert.ToDateTime(txtantisdate.Text.Trim());
                        }
                        if (txtanticdate.Text != "")
                        {
                            objIncubation.Completion_Date = Convert.ToDateTime(txtanticdate.Text.Trim());
                        }


                        if (IsError == false)
                        {
                            dbContext.SaveChanges();
                            ShowbottomMessage("Saved Successfully", true);
                        }
                        else
                        {
                            ShowbottomMessage(ErrorMessage, false);
                        }
                    }

                    else
                    {
                        ShowbottomMessage("Could not found the relevent data.", false);
                    }
                }

            }

            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);
            }
        }
        protected void SaveStep2Data()
        {
            string ErrorMessage = string.Empty;

            bool IsError = false;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);

                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                    if (objIncubation != null)
                    {

                        //objIncubation.Incubation_ID = Guid.NewGuid();
                        //objIncubation.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
                        //objIncubation.Application_Number = Convert.ToString(lblApplicationNo.Text);
                        //objIncubation.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                        //objIncubation.Applicant = GetCurrentUser();
                        //objIncubation.Last_Submitted = DateTime.Now;
                        //objIncubation.Status = "Saved";
                        //objIncubation.Version_Number = "0.1";

                        if (rdo211a.SelectedValue != "")
                            objIncubation.Question2_1_1a = Convert.ToBoolean(rdo211a.SelectedValue);
                        if (rdo211b.SelectedValue != "")
                            objIncubation.Question2_1_1b = Convert.ToBoolean(rdo211b.SelectedValue);
                        if (rdo212a.SelectedValue != "")
                            objIncubation.Question2_1_2a = Convert.ToBoolean(rdo212a.SelectedValue);
                        if (rdo212b.SelectedValue != "")
                            objIncubation.Question2_1_2b = Convert.ToBoolean(rdo212b.SelectedValue);
                        if (rdo212c.SelectedValue != "")
                            objIncubation.Question2_1_2c = Convert.ToBoolean(rdo212c.SelectedValue);
                        if (rdo212d.SelectedValue != "")
                            objIncubation.Question2_1_2d = Convert.ToBoolean(rdo212d.SelectedValue);
                        if (rdo212e.SelectedValue != "")
                            objIncubation.Question2_1_2e = Convert.ToBoolean(rdo212e.SelectedValue);
                        if (rdo212f.SelectedValue != "")
                            objIncubation.Question2_1_2f = Convert.ToBoolean(rdo212f.SelectedValue);
                        if (rdo212g.SelectedValue != "")
                            objIncubation.Question2_1_2g = Convert.ToBoolean(rdo212g.SelectedValue);
                        if (rdo212h.SelectedValue != "")
                            objIncubation.Question2_1_2h = Convert.ToBoolean(rdo212h.SelectedValue);
                        if (rdo212i.SelectedValue != "")
                            objIncubation.Question2_1_2i = Convert.ToBoolean(rdo212i.SelectedValue);
                        if (rdo212j.SelectedValue != "")
                            objIncubation.Question2_1_2j = Convert.ToBoolean(rdo212j.SelectedValue);
                        if (rdo212f_1.SelectedValue != "")
                            objIncubation.Question2_1_2f_1 = Convert.ToBoolean(rdo212f_1.SelectedValue);

                        objIncubation.Modified_By = new SPFunctions().GetCurrentUser();
                        objIncubation.Modified_Date = DateTime.Now;
                        if (IsError == false)
                        {
                            //dbContext.TB_CCMF_APPLICATION.Add(objIncubation);
                            ShowbottomMessage("Saved Successfully", true);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            ShowbottomMessage("Saved Incubation form not found", false);
                        }
                    }
                    else
                    {
                        ShowbottomMessage("Saved Incubation form not found", false);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);
            }
        }
        protected void SaveStep5Data(out bool IsError)
        {
            IsError = false;
            string ErrorMessage = string.Empty;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                    if (objIncubation != null)
                    {
                        hdn_ApplicationID.Value = objIncubation.CCMF_ID.ToString();
                        List<TB_APPLICATION_CONTACT_DETAIL> objFunds = new List<TB_APPLICATION_CONTACT_DETAIL>();/* GetContactDetailsForSave(out IsError)*/;
                        if (IsError == false)
                        {
                            objFunds.ForEach(x => x.Application_ID = objIncubation.CCMF_ID);
                            objFunds.ForEach(x => x.Programme_ID = objIncubation.Programme_ID);
                            IncubationContext.TB_APPLICATION_CONTACTDETAILSADDUPDATE(dbContext, objFunds, objIncubation.CCMF_ID);
                            ShowbottomMessage("Saved Successfully", true);
                            dbContext.SaveChanges();
                            //FillContact();
                        }

                        if (IsError == false)
                        {

                        }
                        else
                        {
                            ShowbottomMessage("Fill all the detials", false);

                        }
                    }

                    else
                    {
                        ShowbottomMessage("Saved Incubation form not found", false);
                    }
                }

            }
            catch (Exception ex)
            {
                IsError = true;
                ShowbottomMessage("Please fill correct values  in 4.1 Contact Details, or leave all fields blank to remove the contact.", false);
            }
        }
        private List<TB_APPLICATION_CONTACT_DETAIL> GetContactDetailsForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            bool IsError = false;
            List<TB_APPLICATION_CONTACT_DETAIL> objContactDetails = new List<TB_APPLICATION_CONTACT_DETAIL>();


            for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
            {
                string titleerror = "Contact Detail " + (i + 1) + " : ";
                try
                {


                    HiddenField contactId = (HiddenField)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("CONTACT_DETAILS_ID");
                    RadioButtonList Area = (RadioButtonList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("rdo_Area");
                    TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                    TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                    TextBox txtlastchiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtlast_chiname");
                    TextBox txtFisrtChiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtFisrt_Chiname");
                    //TextBox identitycard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("identity_card");
                    TextBox txtContactNo = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNo");
                    TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                    TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                    TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                    TextBox Nameofinsititute = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Nameofinsititute");
                    TextBox Studentidcard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Studentidcard");
                    TextBox Programmerolled = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Programmerolled");
                    TextBox Dateofgraduation = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Dateofgraduation");
                    TextBox OrganisationName = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("OrganisationName");
                    TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Position");
                    DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");


                    if (txtContactLast_name.Text != "" || txtContactFirst_name.Text != "" || Position.Text != "" || txtlastchiname.Text != "" || txtFisrtChiname.Text != "" || //identitycard.Text != "" ||
                        txtContactNo.Text != "" || Fax.Text != "" || Email.Text != "" || Mailing_Address.Text != "" || Nameofinsititute.Text != "" || Studentidcard.Text != "" ||
                         Programmerolled.Text != "" || Dateofgraduation.Text != "" || OrganisationName.Text != "" || Position.Text != "")
                    {
                        TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                        objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                        if (txtContactLast_name.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactLast_name.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_Lastname"));
                        else objMember.Last_Name_Eng = txtContactLast_name.Text;
                        if (txtContactFirst_name.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactFirst_name.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_firstname"));
                        else objMember.First_Name_Eng = txtContactFirst_name.Text;

                        if (txtlastchiname.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtlastchiname.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_lastnamechi"));
                        else
                            objMember.Last_Name_Chi = txtlastchiname.Text;
                        if (txtFisrtChiname.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtFisrtChiname.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_firstnamechi"));
                        else
                            objMember.First_Name_Chi = txtFisrtChiname.Text;
                        //if (identitycard.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), identitycard.Text))
                        //    objerror.Add(titleerror + " Identity card length should be less than 255 characters.");
                        //else
                        //    objMember.ID_Number = MD5Encryption.EncryptData(identitycard.Text);
                        if (txtContactNo.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", txtContactNo.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_contact"));
                        else objMember.Contact_No = txtContactNo.Text;
                        if (Fax.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", Fax.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_fax"));
                        else objMember.Fax = Fax.Text;
                        if (Email.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Email.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_email"));
                        else objMember.Email = Email.Text;
                        objMember.Mailing_Address = Mailing_Address.Text;
                        if (Nameofinsititute.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Nameofinsititute.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_education"));
                        else
                            objMember.Education_Institution_Eng = Nameofinsititute.Text;
                        if (Studentidcard.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Studentidcard.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_studentid"));
                        else
                            objMember.Student_ID_Number = Studentidcard.Text;
                        if (Programmerolled.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Programmerolled.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_programme"));
                        else
                            objMember.Programme_Enrolled_Eng = Programmerolled.Text;
                        if (Dateofgraduation.Text != "")
                        {

                            DateTime dDate;

                            if (DateTime.TryParse(Dateofgraduation.Text, out dDate))
                            {
                                String.Format("{0:M-yy}", dDate);
                                var date = Dateofgraduation.Text.Split(new char[] { '-', ' ' });
                                //Convert.ToInt32(date[0]);

                                objMember.Graduation_Month = DateTime.ParseExact(date[0], "MMM", CultureInfo.CurrentCulture).Month;
                                //objMember.Graduation_Month = DateTime.ParseExact(date[0], "MMM", CultureInfo.InvariantCulture).Month;

                                //objMember.Graduation_Month = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(date[0]) + 1;
                                objMember.Graduation_Year = Convert.ToInt32(date[1]);

                            }
                            else
                            {
                                objerror.Add(Localize("Error_Contact_dateformat") + (i + 1));
                            }
                        }



                        if (OrganisationName.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), OrganisationName.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_organisation"));
                        else
                            objMember.Organisation_Name = OrganisationName.Text;
                        if (Position.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Position.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_position"));

                        else
                            objMember.Position = Position.Text;
                        if (Salutation.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Salutation.Text))
                            objerror.Add(titleerror + Localize("Error_Contact_salutation"));
                        else
                            objMember.Salutation = Salutation.Text;
                        if (Area.SelectedValue != "")
                        {
                            objMember.Area = Area.SelectedValue;
                        }

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

        protected void SaveStep6Data()
        {
            string ErrorMessage = string.Empty;
            bool IsError = false;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                    if (objIncubation != null)
                    {


                        if (!string.IsNullOrEmpty(txtVideoClip.Text))
                        {
                            if (Common.CBPRegularExpression.RegExValidate(CBPRegularExpression.Url, txtVideoClip.Text.Trim()))
                                SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, objIncubation.CCMF_ID, progId);
                            else
                            {
                                IsError = true;
                                ErrorMessage = "Invalid Video clip url.";
                            }

                        }
                        //                SPFunctions objSPFunctions = new SPFunctions();

                        //                string _fileUrl = string.Empty;

                        //                if (fu_BrCopy.HasFile)
                        //                {
                        //                    _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                        //                    fu_BrCopy, enumAttachmentType.BR_COPY);
                        //                    SaveAttachmentUrl(_fileUrl, enumAttachmentType.BR_COPY, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                        //                }
                        //                if (fu_StudentID.HasFile)
                        //                {
                        //                    _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                        //                    fu_StudentID, enumAttachmentType.Student_ID);
                        //                    SaveAttachmentUrl(_fileUrl, enumAttachmentType.Student_ID, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                        //                }

                        //                if (fu_HKID.HasFile)
                        //                {
                        //                    _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                        //fu_HKID, enumAttachmentType.HK_ID);
                        //                    SaveAttachmentUrl(_fileUrl, enumAttachmentType.HK_ID, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                        //                }

                        //                if (fuPresentationSlide.HasFile)
                        //                {
                        //                    _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                        //                        fuPresentationSlide, enumAttachmentType.Presentation_Slide);
                        //                    SaveAttachmentUrl(_fileUrl, enumAttachmentType.Presentation_Slide, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                        //                }

                        //                if (fuOtherAttachement.HasFile)
                        //                {
                        //                    _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                        //                    fuOtherAttachement, enumAttachmentType.Other_Attachment);
                        //                    SaveAttachmentUrl(_fileUrl, enumAttachmentType.Other_Attachment, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                        //                }

                        //                if (!string.IsNullOrEmpty(txtVideoClip.Text))
                        //                    SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, objIncubation.Incubation_ID, progId);

                        if (!IsError)
                        {

                            objIncubation.Declaration = chkDeclaration.Checked;
                            objIncubation.Marketing_Information = Marketing_Information.Checked;
                            objIncubation.Have_Read_Statement = Personal_Information.Checked;
                            objIncubation.Principal_Full_Name = txtName_PrincipalApplicant.Text.Trim();
                            objIncubation.Principal_Position_Title = txtPosition_PrincipalApplicant.Text.Trim();
                            dbContext.SaveChanges();
                            ShowbottomMessage("Saved Successfully", true);

                        }
                        else
                            ShowbottomMessage(ErrorMessage, false);

                    }
                    else
                    {
                        ShowbottomMessage("Saved Incubation form not found", false);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);

            }
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
                        UserCustomerrorLogin.InnerText = Localize("Error_Finalsubmit_emalandpass");
                    }
                    else
                    {
                        using (var dbContext = new CyberportEMS_EDM())
                        {
                            int progId = Convert.ToInt32(hdn_ProgramID.Value);
                            Guid appId = Guid.Parse(hdn_ApplicationID.Value);
                            TB_CCMF_APPLICATION objCCMF = GetExistingCCMF(dbContext, progId);
                            // dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.Incubation_ID == appId);

                            if (objCCMF != null)
                            {
                                bool isrequestor = false;
                                if (objCCMF.Application_Parent_ID == null)
                                {
                                    if (objCCMF.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                    {
                                        objCCMF.Status = formsubmitaction.Resubmitted_information.ToString().Replace("_", " ");
                                        isrequestor = true;
                                    }
                                    else
                                    {
                                        objCCMF.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objCCMF.Version_Number)) + 1 ).ToString("F2");
                                        objCCMF.Status = formsubmitaction.Submitted.ToString();
                                        isrequestor = false;
                                    }
                                    objCCMF.Modified_Date = DateTime.Now;
                                    objCCMF.Last_Submitted = DateTime.Now;
                                    objCCMF.Submission_Date = DateTime.Now;
                                }
                                if (objCCMF.Application_Parent_ID != null)
                                {
                                    dbContext.TB_CCMF_APPLICATION.Remove(objCCMF);
                                    Guid ParentId = Guid.Parse(objCCMF.Application_Parent_ID);
                                    TB_CCMF_APPLICATION oldObjCCMF = GetDatabyParentId(dbContext, ParentId);
                                    oldObjCCMF.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objCCMF.Version_Number)) + 1 ).ToString("F2");
                                    oldObjCCMF.Abstract_Chi = objCCMF.Abstract_Chi;
                                    oldObjCCMF.Abstract_Eng = objCCMF.Abstract_Eng;
                                    oldObjCCMF.Additional_Information = objCCMF.Additional_Information;
                                    oldObjCCMF.Applicant = objCCMF.Applicant;
                                    oldObjCCMF.Application_Number = objCCMF.Application_Number;
                                    oldObjCCMF.Business_Area = objCCMF.Business_Area;
                                    oldObjCCMF.Business_Model = objCCMF.Business_Model;
                                    oldObjCCMF.CCMF_Application_Type = objCCMF.CCMF_Application_Type;
                                    oldObjCCMF.Commencement_Date = objCCMF.Commencement_Date;

                                    //2021-12-03
                                    oldObjCCMF.Company_Name = objCCMF.Company_Name;
                                    oldObjCCMF.Establishment_Year = objCCMF.Establishment_Year;

                                    oldObjCCMF.Competition_Analysis = objCCMF.Competition_Analysis;
                                    oldObjCCMF.Completion_Date = objCCMF.Completion_Date;
                                    oldObjCCMF.Cost_Projection = objCCMF.Cost_Projection;
                                    oldObjCCMF.Created_By = objCCMF.Created_By;
                                    oldObjCCMF.Created_Date = DateTime.Now;
                                    oldObjCCMF.CrossBorder_Programme_Type = objCCMF.CrossBorder_Programme_Type;
                                    oldObjCCMF.Declaration = objCCMF.Declaration;
                                    oldObjCCMF.Exit_Stategy = objCCMF.Exit_Stategy;
                                    oldObjCCMF.Have_Read_Statement = objCCMF.Have_Read_Statement;
                                    oldObjCCMF.Hong_Kong_Programme_Stream = objCCMF.Hong_Kong_Programme_Stream;
                                    oldObjCCMF.Advisor_Info = objCCMF.Advisor_Info;
                                    oldObjCCMF.Innovation = objCCMF.Innovation;//objIncubation.Business_Area;
                                    oldObjCCMF.Intake_Number = objCCMF.Intake_Number;
                                    oldObjCCMF.Last_Submitted = DateTime.Now;//objIncubation.Last_Submitted;
                                    oldObjCCMF.Submission_Date = DateTime.Now;
                                    oldObjCCMF.Modified_Date = DateTime.Now;
                                    oldObjCCMF.Marketing_Information = objCCMF.Marketing_Information;
                                    oldObjCCMF.Modified_By = objCCMF.Modified_By;
                                    oldObjCCMF.Other_Business_Area = objCCMF.Other_Business_Area;
                                    oldObjCCMF.Principal_Full_Name = objCCMF.Principal_Full_Name;
                                    oldObjCCMF.Principal_Position_Title = objCCMF.Principal_Position_Title;
                                    //oldObjCCMF.Modified_Date = DateTime.Now;
                                    oldObjCCMF.Programme_ID = objCCMF.Programme_ID;
                                    oldObjCCMF.Programme_Type = objCCMF.Programme_Type;
                                    oldObjCCMF.Project_Management_Team = objCCMF.Project_Management_Team;
                                    oldObjCCMF.Project_Milestone_M1 = objCCMF.Project_Milestone_M1;
                                    oldObjCCMF.Project_Milestone_M2 = objCCMF.Project_Milestone_M2;
                                    oldObjCCMF.Project_Milestone_M3 = objCCMF.Project_Milestone_M3;
                                    oldObjCCMF.Project_Milestone_M4 = objCCMF.Project_Milestone_M4;
                                    oldObjCCMF.Project_Milestone_M5 = objCCMF.Project_Milestone_M5;
                                    oldObjCCMF.Project_Milestone_M6 = objCCMF.Project_Milestone_M6;
                                    oldObjCCMF.Project_Name_Chi = objCCMF.Project_Name_Chi;
                                    oldObjCCMF.Project_Name_Eng = objCCMF.Project_Name_Eng;
                                    oldObjCCMF.Question2_1_1a = objCCMF.Question2_1_1a;
                                    oldObjCCMF.Question2_1_1b = objCCMF.Question2_1_1b;
                                    oldObjCCMF.Question2_1_1c = objCCMF.Question2_1_1c;
                                    oldObjCCMF.Question2_1_1d = objCCMF.Question2_1_1d;
                                    oldObjCCMF.Question2_1_1e = objCCMF.Question2_1_1e;
                                    oldObjCCMF.Question2_1_1f = objCCMF.Question2_1_1f;
                                    oldObjCCMF.Question2_1_1g = objCCMF.Question2_1_1g;
                                    oldObjCCMF.Question2_1_1h = objCCMF.Question2_1_1h;
                                    oldObjCCMF.Question2_1_1i = objCCMF.Question2_1_1i;
                                    oldObjCCMF.Question2_1_1j = objCCMF.Question2_1_1j;
                                    //oldObjCCMF.Question2_1_2a = objIncubation.Question2_1_1c;
                                    //oldObjCCMF.Question2_1_2b = objIncubation.Question2_1_1d;
                                    //oldObjCCMF.Question2_1_2c = objIncubation.Question2_1_1e;
                                    //oldObjCCMF.Question2_1_2d = objIncubation.Question2_1_1f;
                                    //oldObjCCMF.Question2_1_2e = objIncubation.Question2_1_1g;
                                    //oldObjCCMF.Question2_1_2f = objIncubation.Question2_1_1h;
                                    //oldObjCCMF.Question2_1_2g = objIncubation.Question2_1_1i;
                                    //oldObjCCMF.Question2_1_2h = objIncubation.Question2_1_2h;
                                    //oldObjCCMF.Question2_1_2i = objIncubation.Question2_1_2i;
                                    //oldObjCCMF.Question2_1_2j = objIncubation.Question2_1_2j;
                                    oldObjCCMF.Question2_1_2a = objCCMF.Question2_1_2a;
                                    oldObjCCMF.Question2_1_2b = objCCMF.Question2_1_2b;
                                    oldObjCCMF.Question2_1_2c = objCCMF.Question2_1_2c;
                                    oldObjCCMF.Question2_1_2d = objCCMF.Question2_1_2d;
                                    oldObjCCMF.Question2_1_2e = objCCMF.Question2_1_2e;
                                    oldObjCCMF.Question2_1_2f = objCCMF.Question2_1_2f;
                                    oldObjCCMF.Question2_1_2g = objCCMF.Question2_1_2g;
                                    oldObjCCMF.Question2_1_2h = objCCMF.Question2_1_2h;
                                    oldObjCCMF.Question2_1_2i = objCCMF.Question2_1_2i;
                                    oldObjCCMF.Question2_1_2j = objCCMF.Question2_1_2j;
                                    oldObjCCMF.Question2_1_2k = objCCMF.Question2_1_2k;
                                    oldObjCCMF.Social_Responsibility = objCCMF.Social_Responsibility;
                                    oldObjCCMF.Status = "Submitted"; //oldObjCCMF.Status = objIncubation.Status;

                                    //oldObjCCMF.Modified_Date = DateTime.Now;
                                    //oldObjCCMF.Last_Submitted = DateTime.Now;
                                    //oldObjCCMF.Status = "Submitted";
                                    //oldObjCCMF.Submission_Date = DateTime.Now;

                                    dbContext.SaveChanges();

                                    List<TB_APPLICATION_ATTACHMENT> objIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_ATTACHMENT.RemoveRange(objIncubationAttachement);

                                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.RemoveRange(objTB_APPLICATION_COMPANY_CORE_MEMBER);

                                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_CONTACT_DETAIL.RemoveRange(objTB_APPLICATION_CONTACT_DETAIL);

                                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_FUNDING_STATUS.RemoveRange(objTB_APPLICATION_FUNDING_STATUS);
                                    List<TB_APPLICATION_ATTACHMENT> addobjIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objCCMF.CCMF_ID && x.Programme_ID == objCCMF.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_ATTACHMENT.AddRange(addobjIncubationAttachement);
                                    foreach (TB_APPLICATION_ATTACHMENT objAttach in addobjIncubationAttachement)
                                    {
                                        objAttach.Application_ID = oldObjCCMF.CCMF_ID;
                                    }
                                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> addobjTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objCCMF.CCMF_ID && x.Programme_ID == objCCMF.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.AddRange(addobjTB_APPLICATION_COMPANY_CORE_MEMBER);
                                    foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objAttach in addobjTB_APPLICATION_COMPANY_CORE_MEMBER)
                                    {
                                        objAttach.Application_ID = oldObjCCMF.CCMF_ID;
                                    }
                                    List<TB_APPLICATION_CONTACT_DETAIL> addobjTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objCCMF.CCMF_ID && x.Programme_ID == objCCMF.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_CONTACT_DETAIL.AddRange(addobjTB_APPLICATION_CONTACT_DETAIL);
                                    foreach (TB_APPLICATION_CONTACT_DETAIL objAttach in addobjTB_APPLICATION_CONTACT_DETAIL)
                                    {
                                        objAttach.Application_ID = oldObjCCMF.CCMF_ID;
                                    }
                                    List<TB_APPLICATION_FUNDING_STATUS> addobjTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objCCMF.CCMF_ID && x.Programme_ID == objCCMF.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_FUNDING_STATUS.AddRange(addobjTB_APPLICATION_FUNDING_STATUS);
                                    foreach (TB_APPLICATION_FUNDING_STATUS objAttach in addobjTB_APPLICATION_FUNDING_STATUS)
                                    {
                                        objAttach.Application_ID = oldObjCCMF.CCMF_ID;
                                    }
                                }
                                dbContext.SaveChanges();
                                string requestor = "";
                                string strEmailContent = "";
                                string strEmailsubject = "";
                                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                                objTbParams = dbContext.TB_SYSTEM_PARAMETER;

                                TB_PROGRAMME_INTAKE oIntake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == objCCMF.Programme_ID);

                                string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                                WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;

                                string applicationType = "CCMF.aspx";
                                string token = "/SitePages/" + applicationType + "?prog=" + objCCMF.Programme_ID + "&app=" + objCCMF.CCMF_ID;
                                if (isrequestor == true)
                                {
                                    List<TB_SCREENING_HISTORY> objTB_SCREENING_HISTORY1 = new List<TB_SCREENING_HISTORY>();
                                    TB_SCREENING_HISTORY objTB_SCREENING_HISTORY = new TB_SCREENING_HISTORY();
                                    objTB_SCREENING_HISTORY1 = dbContext.TB_SCREENING_HISTORY.OrderByDescending(x => x.Created_Date).ToList();
                                    objTB_SCREENING_HISTORY = objTB_SCREENING_HISTORY1.FirstOrDefault(x => x.Application_Number == objCCMF.Application_Number && x.Programme_ID == objCCMF.Programme_ID);

                                    requestor = objCCMF.Created_By;
                                    //requestor = objTB_SCREENING_HISTORY.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("Requestor_Applicant_Resubmitted");
                                    strEmailContent = strEmailContent.Replace("@@intakeno", oIntake.Intake_Number.ToString()).Replace("@@IntakeNumber", oIntake.Intake_Number.ToString());
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objCCMF.Application_Number);
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted_Requestor").Replace("@@Applicationnumber", objCCMF.Application_Number);
                                    strEmailsubject = strEmailsubject.Replace("@@ProgramName", oIntake.Programme_Name);

                                    int IsEmailSent = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);

                                    requestor = objTB_SCREENING_HISTORY.Created_By;

                                    strEmailContent = CBPEmail.GetEmailTemplate("Application_Updated_Coordinator");
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objCCMF.Application_Number);
                                    strEmailContent = strEmailContent.Replace("@@Comment", objTB_SCREENING_HISTORY.Comment_For_Applicants);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_Resubmitted_Coordinator").Replace("@@Applicationnumber", objCCMF.Application_Number);
                                }
                                else
                                {
                                    requestor = objCCMF.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("Application_Applicant_Submitted");

                                    strEmailContent = strEmailContent.Replace("@@intakeno", oIntake.Intake_Number.ToString()).Replace("@@IntakeNumber", oIntake.Intake_Number.ToString());
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objCCMF.Application_Number);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objCCMF.Application_Number);
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
                                Fill_Programelist(objCCMF.Application_Number, objCCMF.Programme_ID, objCCMF.Intake_Number, objCCMF.Version_Number, objCCMF.Business_Area, objCCMF.Status, objCCMF.Applicant, objCCMF.Programme_Type);
                            }
                            else
                            {
                                IncubationSubmitPopup.Visible = true;
                                UserCustomerrorLogin.InnerHtml = "Could not found the relevent data.";
                            }
                        }

                    }
                }
                else
                {
                    IncubationSubmitPopup.Visible = true;
                    UserCustomerrorLogin.InnerText = "Invalid Email and Password";
                }
            }
            catch (Exception ex)
            {
                IncubationSubmitPopup.Visible = true;
                UserCustomerrorLogin.InnerText = ex.Message;
            }
        }
        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
        protected void btn_Submit_Click1(object sender, EventArgs e)
        {
            ShowHideControlsBasedUponUserData();
            int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
            ShowbottomMessage("", true);
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
        }

        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            IncubationSubmitPopup.Visible = false;
            SPFunctions objFUnction = new SPFunctions();
            txtLoginUserName.Text = objFUnction.GetCurrentUser();
        }
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
        protected TB_CCMF_APPLICATION GetDatabyParentId(CyberportEMS_EDM dbContext, Guid ParentId)
        {
            TB_CCMF_APPLICATION objIncubation = null;

            objIncubation = dbContext.TB_CCMF_APPLICATION.OrderBy(x => x.Modified_Date).FirstOrDefault(x => x.CCMF_ID == ParentId);
            return objIncubation;
        }
        protected void switchStep2UI(string ProgType)
        {


            switch (ProgType)
            {
                case "cbi":
                    {
                        //heading1.Text = "-" + SPFunctions.LocalizeUI("Cyberport_Shenzhen_Hong_Kong_Young_Entrepreneur_Programme");
                        //heading2.Text = "-" + SPFunctions.LocalizeUI("Cyberport_Guangdong__Hong_Kong_Young_Entrepreneur_Programme");
                        hkorcrossborder.Text = SPFunctions.LocalizeUI("Cross_Border_Programme_Supported_by_CCMF", "CyberportEMS_CCMF");
                        Applicant.Text = SPFunctions.LocalizeUI("Individual_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        lbl211a.Text = SPFunctions.LocalizeUI("CCMF_Ind_1_A", "CyberportEMS_CCMF");
                        //lbl211b.Text = SPFunctions.LocalizeUI("CCMF_Comp_1_B", "CyberportEMS_CCMF");
                        div211b.Visible = false;// Added by Tim Ng
                        div211b.Disabled = false;
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;
                        div212a.Visible = false;
                        lbl211btd.Disabled = true;
                        lbl211b.Enabled = false;
                        rdo211b.Enabled = false;
                        //rdo211b.SelectedIndex = -1;

                        //   lbl212a.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_A", "CyberportEMS_CCMF");
                        //   lbl212b.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_B", "CyberportEMS_CCMF");
                        lbl212c.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_F", "CyberportEMS_CCMF");
                        lbl212g.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_G", "CyberportEMS_CCMF");
                        lbl212h.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_H", "CyberportEMS_CCMF");
                        lbl212i.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_I", "CyberportEMS_CCMF");
                        lbl212j.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_J", "CyberportEMS_CCMF");


                        //div212f_1.Visible = false;
                        rdo211b_SetUpDisable();
                    }
                    break;
                case "cbc":
                    {

                        //heading1.Text = "-" + SPFunctions.LocalizeUI("Cyberport_Shenzhen_Hong_Kong_Young_Entrepreneur_Programme");
                        //heading2.Text = "-" + SPFunctions.LocalizeUI("Cyberport_Guangdong__Hong_Kong_Young_Entrepreneur_Programme");
                        hkorcrossborder.Text = SPFunctions.LocalizeUI("Cross_Border_Programme_Supported_by_CCMF", "CyberportEMS_CCMF");
                        Applicant.Text = SPFunctions.LocalizeUI("Company_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        lbl211a.Text = SPFunctions.LocalizeUI("CCMF_Comp_1_A", "CyberportEMS_CCMF");
                        lbl211b.Text = SPFunctions.LocalizeUI("CCMF_Comp_1_B", "CyberportEMS_CCMF");
                        div211b.Visible = true;// added by TimNg
                        div211b.Disabled = false;
                        div212a.Visible = false;
                        lbl211btd.Disabled = true;
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;
                        //rdo211b.SelectedIndex = -1;
                        //     lbl212a.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_A", "CyberportEMS_CCMF");
                        //    lbl212b.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_B", "CyberportEMS_CCMF");
                        lbl212c.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_F", "CyberportEMS_CCMF");
                        lbl212g.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_G", "CyberportEMS_CCMF");
                        lbl212h.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_H", "CyberportEMS_CCMF");
                        lbl212i.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_I", "CyberportEMS_CCMF");
                        lbl212j.Text = SPFunctions.LocalizeUI("CCMF_IndComp_2_J", "CyberportEMS_CCMF");

                        //div212f_1.Visible = false;
                        rdo211b_SetUpDisable();
                    }

                    break;
                case "cbuc":
                    {

                        //heading1.Text = "-" + SPFunctions.LocalizeUI("Cyberport_University_Partnership_Programme", "CyberportEMS_CCMF");
                        Applicant.Text = SPFunctions.LocalizeUI("Company_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        hkorcrossborder.Text = SPFunctions.LocalizeUI("CUPP_heading", "CyberportEMS_CCMF");
                        lbl211a.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_1_A", "CyberportEMS_CCMF");
                        lbl211b.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_1_B", "CyberportEMS_CCMF");
                        div211b.Disabled = false;

                        lbl211btd.Disabled = false;
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;
                        //rdo211b.SelectedIndex = -1;

                        // reopen for CUPP change 20230302
                           lbl212a.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_A", "CyberportEMS_CCMF");
                            lbl212b.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_B", "CyberportEMS_CCMF");
                        lbl212c.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_F", "CyberportEMS_CCMF");
                        lbl212g.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_G", "CyberportEMS_CCMF");
                        lbl212h.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_H", "CyberportEMS_CCMF");

                        div212i.Visible = false;
                        div212j.Visible = false;

                        // reopen for CUPP change 20230302
                        div212a.Visible = true;
                        div212b.Visible = true;
                        //div212a.Visible = false;
                        //div212f_1.Visible = false;
                        rdo211b_SetUpDisable();
                    }
                    break;
                case "cbui":
                    {
                        //heading1.Text = "-" + SPFunctions.LocalizeUI("Cyberport_University_Partnership_Programme", "CyberportEMS_CCMF");
                        Applicant.Text = SPFunctions.LocalizeUI("Individual_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        lbl211a.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Ind_1_A", "CyberportEMS_CCMF");
                        lbl211b.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Ind_1_B", "CyberportEMS_CCMF");
                        hkorcrossborder.Text = SPFunctions.LocalizeUI("CUPP_heading", "CyberportEMS_CCMF");
                        div211b.Disabled = false;// always enable individual 
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;
                        lbl211btd.Disabled = false;
                        //rdo211b.SelectedIndex = -1;
                        // reopen for CUPP change 20230302
                              lbl212a.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_A", "CyberportEMS_CCMF");
                              lbl212b.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_B", "CyberportEMS_CCMF");
                        lbl212c.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_F", "CyberportEMS_CCMF");
                        lbl212g.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_G", "CyberportEMS_CCMF");
                        lbl212h.Text = SPFunctions.LocalizeUI("CUPP_IndComp_2_H", "CyberportEMS_CCMF");
                        div212i.Visible = false;
                        div212j.Visible = false;
                        // reopen for CUPP change 20230302
                        div212a.Visible = true;
                        div212b.Visible = true;
                        //div212a.Visible = false;
                        //div212f_1.Visible = false;
                        rdo211b_SetUpDisable();
                    }
                    break;
                case "hkyc": //HK YEP Com
                    {

                        Applicant.Text = SPFunctions.LocalizeUI("Company_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        hkorcrossborder.Text = SPFunctions.LocalizeUI("Hong_Kong_Young_Entrepreneur_Programme", "CyberportEMS_CCMF");
                        lbl211a.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_1_A", "CyberportEMS_CCMF");
                        lbl211b.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_1_B", "CyberportEMS_CCMF");
                        div211b.Visible = true; // Added by Tim Ng @ 20170316
                        div211b.Disabled = false;

                        lbl211cYepComp.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_1_C_note", "CyberportEMS_CCMF"); ;


                        if (!string.IsNullOrEmpty(rdo211c.SelectedValue))
                        {
                            if (!Convert.ToBoolean(rdo211c.SelectedValue))
                            {
                                lbl211cYepComp.Visible = true;
                                lbl211cYepInd.Visible = false;
                            }
                        }

                        lbl211btd.Disabled = false;
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;
                        Tr11c.Visible = true;
                        lbl211c.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_1_C", "CyberportEMS_CCMF");
                        //rdo211b.SelectedIndex = -1;
                        //        lbl212a.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_A", "CyberportEMS_CCMF");
                        //        lbl212b.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_B", "CyberportEMS_CCMF");                        
                        lbl212c.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_F", "CyberportEMS_CCMF");
                        lbl212g.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_G", "CyberportEMS_CCMF");
                        lbl212h.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_H", "CyberportEMS_CCMF");
                        div212i.Visible = false;
                        div212j.Visible = false;
                        div212a.Visible = false;
                        div212b.Visible = false;
                        rdo211b_SetUpDisable();
                        spn212c.InnerHtml = "a)";
                        lbl212dtd.Text = "b)";
                        lbl212etdd.Text = "c)";
                        lbl212ftd.Text = "d)";
                        lbl212gtd.Text = "e)";
                        spn212h.InnerHtml = "f)";
                        div212f_1.Visible = true;
                        lbl212f_1.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_F_1", "CyberportEMS_CCMF");
                        spn212f_1.InnerHtml = "g)";

                        if (!string.IsNullOrEmpty(rdo212f.SelectedValue))
                        {
                            if (Convert.ToBoolean(rdo212f.SelectedValue) == true)
                            {
                                rdo212g.Enabled = true;
                                lbl212gtd.Enabled = true;
                                lbl212g.Enabled = true;

                            }
                            else
                            {
                                lbl212gtd.Enabled = false;
                                lbl212g.Enabled = false;
                                rdo212g.Enabled = false;
                                rdo212g.SelectedIndex = -1;

                            }
                        }
                        else
                        {
                            lbl212gtd.Enabled = false;
                            lbl212g.Enabled = false;
                            rdo212g.Enabled = false;
                            rdo212g.SelectedIndex = -1;

                        }


                    }
                    break;
                case "hkyi": // HK YEP Ind
                    {
                        Applicant.Text = SPFunctions.LocalizeUI("Individual_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        lbl211a.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Ind_1_A", "CyberportEMS_CCMF");
                        hkorcrossborder.Text = SPFunctions.LocalizeUI("Hong_Kong_Young_Entrepreneur_Programme", "CyberportEMS_CCMF");
                        div211b.Visible = true; // Added by Tim Ng @ 20170316
                        div211b.Disabled = false;
                        lbl211b.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Ind_1_B", "CyberportEMS_CCMF");
                        lbl211btd.Disabled = false;
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;

                        //Turn off YEP ind  2.1.1 c
                        //Tr11c.Visible = true;
                        lbl211c.Visible = false;
                        Tr11c.Visible = false;

                        lbl211cYepInd.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Ind_1_C_note", "CyberportEMS_CCMF");
                        if (!string.IsNullOrEmpty(rdo211c.SelectedValue))
                        {
                            if (!Convert.ToBoolean(rdo211c.SelectedValue))
                            {
                                lbl211cYepComp.Visible = false;
                                //lbl211cYepInd.Visible = true;
                                lbl211cYepInd.Visible = false;
                            }
                        }

                        //rdo211b.SelectedIndex = -1;

                        //         lbl212a.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_A", "CyberportEMS_CCMF");
                        //          lbl212b.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_B", "CyberportEMS_CCMF");
                        div212a.Visible = false;
                        div212b.Visible = false;
                        lbl212c.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_F", "CyberportEMS_CCMF");
                        lbl212g.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_G", "CyberportEMS_CCMF");
                        lbl212h.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_H", "CyberportEMS_CCMF");

                        div212i.Visible = false;
                        div212j.Visible = false;
                        rdo211b_SetUpDisable();
                        spn212c.InnerHtml = "a)";
                        lbl212dtd.Text = "b)";
                        lbl212etdd.Text = "c)";
                        lbl212ftd.Text = "d)";
                        lbl212gtd.Text = "e)";

                        div212h.Visible = true;
                        spn212h.InnerHtml = "f)";
                        div212f_1.Visible = true;
                        lbl212f_1.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_F_1", "CyberportEMS_CCMF");
                        spn212f_1.InnerHtml = "g)";
                        if (!string.IsNullOrEmpty(rdo212f.SelectedValue))
                        {
                            if (Convert.ToBoolean(rdo212f.SelectedValue) == true)
                            {
                                rdo212g.Enabled = true;
                                lbl212gtd.Enabled = true;
                                lbl212g.Enabled = true;

                            }
                            else
                            {
                                lbl212gtd.Enabled = false;
                                lbl212g.Enabled = false;
                                rdo212g.Enabled = false;
                                rdo212g.SelectedIndex = -1;

                            }
                        }
                        else
                        {
                            lbl212gtd.Enabled = false;
                            lbl212g.Enabled = false;
                            rdo212g.Enabled = false;
                            rdo212g.SelectedIndex = -1;

                        }
                    }
                    break;
                case "hkpc": // HK pro com
                    {
                        Applicant.Text = SPFunctions.LocalizeUI("Company_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        hkorcrossborder.Text =  SPFunctions.LocalizeUI("Professional_Stream", "CyberportEMS_CCMF");
                        div212a.Visible = true;
                        div212b.Visible = true;
                        lbl211a.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_Comp_1_A", "CyberportEMS_CCMF");
                        lbl211b.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_Comp_1_B", "CyberportEMS_CCMF");
                        div211b.Visible = true; // Added by Tim Ng @ 20170316

                        lbl212a.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_A", "CyberportEMS_CCMF");
                        lbl212b.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_B", "CyberportEMS_CCMF");
                        lbl212c.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_F", "CyberportEMS_CCMF");

                        div212f_1.Visible = true;
                        lbl212f_1.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_F_1", "CyberportEMS_CCMF");
                        spn212f_1.InnerHtml = "g)";

                        div212g.Visible = false;
                        //div212g.Visible = false;
                        //lbl212g.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_G", "CyberportEMS_CCMF");
                        div212h.Visible = false;
                        div212i.Visible = false;
                        div212j.Visible = false;
                        rdo211b_SetUpDisable();



                        if (!string.IsNullOrEmpty(rdo212d.SelectedValue))
                        {
                            if (Convert.ToBoolean(rdo212d.SelectedValue) == true)
                            {
                                rdo212e.Enabled = true;
                                lbl212etdd.Enabled = true;
                                lbl212e.Enabled = true;

                            }
                            else
                            {
                                rdo212e.Enabled = false;
                                lbl212etdd.Enabled = false;
                                lbl212e.Enabled = false;
                                rdo212e.SelectedIndex = -1;

                            }
                        }
                        else
                        {
                            rdo212e.Enabled = false;
                            lbl212etdd.Enabled = false;
                            lbl212e.Enabled = false;
                            rdo212e.SelectedIndex = -1;

                        }


                    }
                    break;
                case "hkpi": //HK pro ind
                    {
                        Applicant.Text = SPFunctions.LocalizeUI("Individual_Applicant", "CyberportEMS_CCMF");
                        BothApplicants.Text = SPFunctions.LocalizeUI("Individual_and_Company_Applicant", "CyberportEMS_CCMF");
                        hkorcrossborder.Text =  SPFunctions.LocalizeUI("Professional_Stream", "CyberportEMS_CCMF");
                        div212a.Visible = true;
                        div212b.Visible = true;
                        lbl211a.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_Ind_1_A", "CyberportEMS_CCMF");
                        lbl211b.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_Ind_1_B", "CyberportEMS_CCMF");
                        div211b.Visible = true; // Added by Tim Ng @ 20170316
                        div211b.Disabled = false;
                        lbl211btd.Disabled = false;
                        lbl211b.Enabled = true;
                        rdo211b.Enabled = true;

                        lbl212a.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_A", "CyberportEMS_CCMF");
                        lbl212b.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_B", "CyberportEMS_CCMF");
                        lbl212c.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_C", "CyberportEMS_CCMF");
                        lbl212d.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_D", "CyberportEMS_CCMF");
                        lbl212e.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_E", "CyberportEMS_CCMF");
                        lbl212f.Text = SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_2_F", "CyberportEMS_CCMF");
                        div212f_1.Visible = true;
                        lbl212f_1.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_IndComp_2_F_1", "CyberportEMS_CCMF");
                        spn212f_1.InnerHtml = "g)";

                        div212g.Visible = false;
                        div212h.Visible = false;
                        div212i.Visible = false;
                        div212j.Visible = false;
                        rdo211b_SetUpDisable();

                        if (!string.IsNullOrEmpty(rdo212d.SelectedValue))
                        {
                            if (Convert.ToBoolean(rdo212d.SelectedValue) == true)
                            {
                                rdo212e.Enabled = true;
                                lbl212etdd.Enabled = true;
                                lbl212e.Enabled = true;

                            }
                            else
                            {
                                rdo212e.Enabled = false;
                                lbl212etdd.Enabled = false;
                                lbl212e.Enabled = false;

                                rdo212e.SelectedIndex = -1;

                            }
                        }
                        else
                        {
                            rdo212e.Enabled = false;
                            lbl212etdd.Enabled = false;
                            lbl212e.Enabled = false;

                            rdo212e.SelectedIndex = -1;

                        }
                    }
                    break;
                default:
                    break;
            }

        }
        protected bool SubmitValidationError()
        {
            bool IsError = false;
            string ErrorMessage = string.Empty;
            List<string> errlist = new List<string>();

            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    SPFunctions objFn = new SPFunctions();
                    string strCurrentUser = objFn.GetCurrentUser();


                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);//dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    //List<TB_APPLICATION_COMPANY_CORE_MEMBER> Objcoremembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(objIncubation.Incubation_ID);
                    List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(objIncubation.CCMF_ID);
                    List<TB_APPLICATION_CONTACT_DETAIL> ObjTB_APPLICATION_CONTACT_DETAIL = IncubationContext.APPLICATION_CONTACT_DETAIL_GET(objIncubation.CCMF_ID);
                    List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT = IncubationContext.ListofTB_APPLICATION_ATTACHMENTGGet(objIncubation.CCMF_ID, objIncubation.Programme_ID);
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> Objcoremembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(objIncubation.CCMF_ID);
                    if (objIncubation != null)
                    {
                        String Activation = "";
                        bool CheckActivation = false;
                        bool IsYepApplication = false;
                        if (objIncubation.Programme_Type.ToLower() == "hongkong" || objIncubation.Programme_Type.ToLower() == "cupp")
                        {
                            //if (objTB_APPLICATION_ATTACHMENT.Attachment_Type.ToLower() != enumAttachmentType.Student_ID.ToString().ToLower())
                            //{
                            //    IsError = true;

                            //    errlist.Add("Please upload document in the required field:  BR Copy.");
                            //}
                            if (!string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                            {
                                //ss8 Validation
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question1_3)))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_1_3"));
                                }
                                else if (objIncubation.Question1_3 == true)
                                {
                                    IsError = true;

                                    errlist.Add(Localize("Error_1_3Ageval"));
                                }


                                if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "professional")
                                {
                                    if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                                    {
                                        Activation = objIncubation.CCMF_Application_Type;

                                        //Added 2.1.1a is required for all Prof
                                        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                        {
                                            IsError = true;

                                            errlist.Add(Localize("Error_2_1_1a"));
                                        }

                                        //for prof, only is company application and 2.1.1a is yes the 2.1.1b can be empty
                                        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                        {
                                            if (Activation.ToLower() == "company")
                                            {
                                                if (objIncubation.Question2_1_1a == false)
                                                {
                                                    IsError = true;

                                                    errlist.Add(Localize("Error_2_1_1b"));
                                                }

                                            }
                                            else
                                            {
                                                IsError = true;

                                                errlist.Add(Localize("Error_2_1_1b"));

                                            }
                                        }

                                        //if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                        //{
                                        //    if (Activation.ToLower() == "company")
                                        //    {
                                        //        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)) && objIncubation.Question2_1_1a == false)
                                        //        {
                                        //            IsError = true;

                                        //            errlist.Add(Localize("Error_2_1_1b"));
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                        //        {
                                        //            IsError = true;

                                        //            errlist.Add(Localize("Error_2_1_1b"));

                                        //        }
                                        //    }
                                        //}
                                    }


                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2a)))
                                    {
                                        IsError = true;

                                        errlist.Add(Localize("Error_2_1_2a"));
                                    }


                                    if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                                    {
                                        Activation = objIncubation.CCMF_Application_Type;
                                        if (Activation.ToLower() == "company" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2b)))
                                        {
                                            IsError = true;

                                            errlist.Add(Localize("Error_2_1_2b"));
                                        }
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2c)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2c"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2d)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2d"));

                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2d)))
                                    {
                                        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2e)) && objIncubation.Question2_1_2d == true)
                                        {

                                            IsError = true;
                                            errlist.Add(Localize("Error_2_1_2dreq"));

                                        }
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2f"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f_1)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2g"));

                                    }

                                    //if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1h)))
                                    //{
                                    //    CheckActivation = Convert.ToBoolean(objIncubation.Question2_1_1h);
                                    //    if (CheckActivation == false && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1i)))
                                    //    {
                                    //        IsError = true;
                                    //        errlist.Add("Please select Question 2.1.1(I)");
                                    //    }
                                    //}



                                    //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1j)))
                                    //{

                                    //    IsError = true;
                                    //    errlist.Add("Please select Question 2.1.1(J)");

                                    //}
                                }

                                else
                                {

                                    if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                                    {
                                        IsYepApplication = true;
                                        Activation = objIncubation.CCMF_Application_Type;
                                        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                        {
                                            IsError = true;

                                            errlist.Add(Localize("Error_2_1_1a"));
                                        }
                                        //Activation.ToLower() == "company" && 
                                        //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)) && objIncubation.Question2_1_1a == true)
                                        //{

                                        //    IsError = true;

                                        //    errlist.Add("Please select yes / no in the required field: 2.1.1(b)");
                                        //}

                                        if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                        {
                                            if (Activation.ToLower() == "company")
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)) && objIncubation.Question2_1_1a == false)
                                                {
                                                    IsError = true;

                                                    errlist.Add(Localize("Error_2_1_1b"));
                                                }
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                                {
                                                    IsError = true;

                                                    errlist.Add(Localize("Error_2_1_2breq"));
                                                }
                                            }
                                        }
                                        //if (!objIncubation.Question2_1_1c.HasValue)
                                        //{
                                        //    IsError = true;
                                        //    errlist.Add(Localize("Error_2_1_1c"));
                                        //}
                                        //else if (objIncubation.Question2_1_1c.Value == false)
                                        //{
                                        //    IsError = true;
                                        //    errlist.Add(Localize("Error_Age_Validation"));
                                        //}

                                        //if (objTB_APPLICATION_ATTACHMENT != null && Activation.ToLower() == "company" && !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                        //{
                                        //    if (Convert.ToBoolean(objIncubation.Question2_1_1b) == true && (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.BR_COPY.ToString().ToLower())))

                                        //    {



                                        //        IsError = true;

                                        //        errlist.Add("Please upload document in the required field:  BR Copy if 2.1.2(B) is yes .");
                                        //    }

                                        //}
                                    }


                                    //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2a)))
                                    //{
                                    //    IsError = true;
                                    //    errlist.Add(Localize("Error_2_1_2a"));
                                    //}

                                    //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2b)))
                                    //{

                                    //    IsError = true;
                                    //    errlist.Add(Localize("Error_2_1_2b"));

                                    //}
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2c)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2a"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2d)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2b"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2e)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2c"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2d"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2g)) && Convert.ToBoolean(objIncubation.Question2_1_2f) == true)
                                        if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2g)))
                                        {

                                            IsError = true;
                                            errlist.Add(Localize("Error_2_1_2e"));

                                        }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2h)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2f"));

                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f_1)))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2g"));

                                    }
                                    //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2k)))
                                    //{

                                    //    IsError = true;
                                    //    errlist.Add("Please select Question 2.1.2(K)");

                                    //}
                                }
                            }
                            if (objIncubation.Programme_Type.ToLower() == "cupp")
                            {

                                if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                                {
                                    Activation = objIncubation.CCMF_Application_Type;
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                    {
                                        IsError = true;

                                        errlist.Add(Localize("Error_2_1_1a"));
                                    }

                                    if (Activation.ToLower() == "company" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                    {
                                        IsError = true;

                                        errlist.Add(Localize("Error_2_1_1b"));
                                    }
                                    //if (objTB_APPLICATION_ATTACHMENT != null && Activation.ToLower() == "company" && !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                    //{
                                    //    if (Convert.ToBoolean(objIncubation.Question2_1_1b) == true && (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.BR_COPY.ToString().ToLower())))

                                    //    {


                                    //        IsError = true;

                                    //        errlist.Add("Please upload document in the required field:  BR Copy if 2.1.2(B) is yes .");
                                    //    }

                                    //}
                                }


                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2a)))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2a"));
                                }

                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2b)))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2b"));

                                }
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2c)))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2c"));

                                }
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2d)))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2d"));

                                }
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2e)))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2e"));

                                }
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2f"));

                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)))
                                {
                                    if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2g)) && objIncubation.Question2_1_2f == true)
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_2_1_2greq"));

                                    }
                                }
                                //if(string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2g)))
                                //{

                                //    IsError = true;
                                //    errlist.Add("Please select yes / no in the required field: Question 2.1.2(g)");

                                //}
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2h)))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_2_1_2h"));

                                }
                                //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2k)))
                                //{

                                //    IsError = true;
                                //    errlist.Add("Please select Question 2.1.2(K)");

                                //}
                            }
                        }

                        else if (objIncubation.Programme_Type.ToLower().Contains("crossborder"))
                        {
                            if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                            {
                                Activation = objIncubation.CCMF_Application_Type;
                                if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                {
                                    IsError = true;

                                    errlist.Add(Localize("Error_2_1_1a"));
                                }

                                if (Activation.ToLower() == "company" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                {
                                    IsError = true;

                                    errlist.Add(Localize("Error_2_1_1b"));
                                }
                                // if (objTB_APPLICATION_ATTACHMENT != null && Activation.ToLower() == "company" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                //{
                                //    if (Convert.ToBoolean(objIncubation.Question2_1_1b) == true && (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.BR_COPY.ToString().ToLower())))
                                //    {
                                //        IsError = true;

                                //        errlist.Add("Please upload document in the required field:  BR Copy if 2.1.2(B) is yes");
                                //    }
                                //}
                            }

                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2a)))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2a"));
                            }


                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2b)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2b"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2c)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2c"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2d)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2d"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2e)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2e"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2f"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2g)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2g"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2h)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2h"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2i)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2i"));

                            }
                            if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2j)))
                            {

                                IsError = true;
                                errlist.Add(Localize("Error_2_1_2j"));

                            }
                            //if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2k)))
                            //{
                            //    CheckActivation = Convert.ToBoolean(objIncubation.Question2_1_2k);
                            //    if (CheckActivation == false && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2l)))
                            //    {
                            //        IsError = true;
                            //        errlist.Add("Please select Question 2.2(L)");
                            //    }
                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2m)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.2(M)");

                            //}
                        }
                        else
                        {
                            //if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                            //{
                            //    Activation = objIncubation.CCMF_Application_Type;
                            //    if (Activation.ToLower() == "individual" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1a)))
                            //    {
                            //        IsError = true;

                            //        errlist.Add("Please select Question 2.3.1(A)");
                            //    }
                            //}
                            //if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                            //{
                            //    Activation = objIncubation.CCMF_Application_Type;
                            //    if (Activation.ToLower() == "company" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1b)))
                            //    {
                            //        IsError = true;

                            //        errlist.Add("Please select Question  2.3.2)(B)");
                            //    }
                            //    else
                            //    {
                            //        if (Convert.ToBoolean(objIncubation.Question2_1_2b) == true && objTB_APPLICATION_ATTACHMENT.Attachment_Type.ToLower() != enumAttachmentType.BR_COPY.ToString().ToLower())
                            //        {
                            //            IsError = true;

                            //            errlist.Add("Please upload document in the required field:  BR Copy.");
                            //        }
                            //    }
                            //}
                            //if (!string.IsNullOrEmpty(objIncubation.CCMF_Application_Type))
                            //{
                            //    Activation = objIncubation.CCMF_Application_Type;
                            //    if (Activation.ToLower() == "company" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1c)))
                            //    {
                            //        IsError = true;
                            //        errlist.Add("Please select Question 2.3.1(C)");
                            //    }
                            //}
                            //if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2i)))
                            //{
                            //    CheckActivation = Convert.ToBoolean(objIncubation.Question2_1_2i);
                            //    if (CheckActivation == true && string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1j)))
                            //    {
                            //        IsError = true;
                            //        errlist.Add("Please select Question 2.3.1(J)");
                            //    }
                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1d)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(D)");

                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1e)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(E)");

                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1f)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(F)");

                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1g)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(G)");

                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1h)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(H)");

                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1i)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(I)");

                            //}
                            //if (string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1k)))
                            //{

                            //    IsError = true;
                            //    errlist.Add("Please select Question 2.3.1(K)");

                            //}
                        }



                        if (string.IsNullOrEmpty(objIncubation.Project_Name_Eng))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Projname_eng"));
                        }

                        string apptype = objIncubation.CCMF_Application_Type;


                        if (objIncubation.Programme_Type == "HongKong")
                        {
                            if (apptype == "Company")
                            {

                                if (string.IsNullOrEmpty(objIncubation.Company_Name))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Company_Name_Required"));
                                }

                                //if (!objIncubation.Establishment_Year.HasValue)
                                //{
                                //    IsError = true;
                                //    errlist.Add(Localize("Error_Establishment_Year_Required"));
                                //}

                                //if (!objIncubation.NEW_to_HK.HasValue)
                                //{
                                //    IsError = true;
                                //    errlist.Add(Localize("Error_New_to_HK_Required"));
                                //}


                            }
                        }


                        if (string.IsNullOrEmpty(objIncubation.Abstract_Eng))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Abstract_eng"));
                        }
                        if (string.IsNullOrEmpty(objIncubation.Business_Area))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Businessarea"));
                        }
                        if (objIncubation.Commencement_Date == null)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_commencement"));
                        }
                        if (objIncubation.Completion_Date == null)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Completion"));
                        }
                        if ((objIncubation.Commencement_Date != null && objIncubation.Completion_Date != null) && objIncubation.Commencement_Date > objIncubation.Completion_Date)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_completion_and_commencemnt"));
                        }



                        if (objIncubation.Programme_Type == "HongKong")
                        {
                            if (string.IsNullOrEmpty(objIncubation.SmartSpace))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_3_6"));
                            }
                        }


                        if (Objcoremembers.Count == 0)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Projectmember_atleast"));
                        }
                        if (!string.IsNullOrEmpty(objIncubation.Business_Area))
                        {
                            if (objIncubation.Business_Area.ToLower() == "others")
                            {
                                Activation = objIncubation.Business_Area;
                                if (objIncubation.Business_Area.ToLower() == "others" && string.IsNullOrEmpty(Convert.ToString(objIncubation.Other_Business_Area)))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Businessarea_other"));
                                }
                            }
                        }
                        /*
                        if (string.IsNullOrEmpty(objIncubation.Advisor_Info))
                        {
                            IsError = true;
                            errlist.Add("Please fill in the required field 4.1(b) Advisor Info");
                        }
                        */
                        if (string.IsNullOrEmpty(objIncubation.Business_Model))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Businessmodel"));
                        }
                        if (string.IsNullOrEmpty(objIncubation.Innovation))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Innovation"));
                        }
                        if (string.IsNullOrEmpty(objIncubation.Social_Responsibility))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_social_responsibility"));
                        }
                        //if (string.IsNullOrEmpty(objIncubation.Competition_Analysis))
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field 4.5 Competition Analysis");
                        //}
                        if (string.IsNullOrEmpty(objIncubation.Project_Milestone_M1) || string.IsNullOrEmpty(objIncubation.Project_Milestone_M2) || string.IsNullOrEmpty(objIncubation.Project_Milestone_M3) ||
                           string.IsNullOrEmpty(objIncubation.Project_Milestone_M4) || string.IsNullOrEmpty(objIncubation.Project_Milestone_M5) ||
                           string.IsNullOrEmpty(objIncubation.Project_Milestone_M6))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Project_Milestone"));
                        }
                        //if (string.IsNullOrEmpty(objIncubation.Project_Milestone_M2))
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 4.4.6 Second 6 months Project Milestone.");
                        //}
                        //if (string.IsNullOrEmpty(objIncubation.Project_Milestone_M3))
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 4.4.6 Second 6 months Project Milestone.");
                        //}
                        //if (string.IsNullOrEmpty(objIncubation.Project_Milestone_M4))
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 4.4.6 Forth 6 months Project Milestone.");
                        //}
                        //if (string.IsNullOrEmpty(objIncubation.Project_Milestone_M4))
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 4.4.6 Second 6 months Project Milestone.");
                        //}
                        //if (string.IsNullOrEmpty(objIncubation.Project_Milestone_M6))
                        //{
                        //    IsError = true;
                        //    errlist.Add("Please fill in the required field: 4.4.6 Forth 6 months Project Milestone.");
                        //}
                        if (string.IsNullOrEmpty(objIncubation.Cost_Projection))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Cost_Projection"));
                        }



                        foreach (TB_APPLICATION_FUNDING_STATUS obj in ObjFundingStatus)
                        {
                            if (obj.Date == null || obj.Programme_Name == null || obj.Application_Status == null || obj.Funding_Status == null || obj.Expenditure_Nature == null
                                || obj.Amount_Received == null || obj.Maximum_Amount == null)
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_Fundingall"));
                            }

                        }
                        //if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1j)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2j))
                        //    || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2l)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2m))
                        //   || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1j)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1k)))
                        //{

                        //    if ((Convert.ToBoolean(objIncubation.Question2_1_2e) == true || Convert.ToBoolean(objIncubation.Question2_1_2f) == true) && string.IsNullOrEmpty(Convert.ToString(objIncubation.Additional_Information)))
                        //    {
                        //        IsError = true;
                        //        errlist.Add("Please fill in the required field: 4.10 Additional Information, if Question 2.1.2(e) or 2.1.2(f) answer is Yes.");
                        //    }
                        //}

                        foreach (TB_APPLICATION_COMPANY_CORE_MEMBER obj1 in Objcoremembers)
                        {
                            if (string.IsNullOrEmpty(obj1.Name) || string.IsNullOrEmpty(obj1.Position) || string.IsNullOrEmpty(obj1.HKID)
                                || string.IsNullOrEmpty(obj1.Background_Information))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_Projectmember_atleast"));
                            }

                        }
                        //if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1f)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1g))
                        //   || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1h)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2g))
                        //  || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2h)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2i))
                        //   || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2i)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2j))
                        //    || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_2k)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1g))
                        //     || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1h)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_3_1i)))
                        //{

                        //    if ((Convert.ToBoolean(objIncubation.Question2_1_1f) == true || Convert.ToBoolean(objIncubation.Question2_1_1g) == true || Convert.ToBoolean(objIncubation.Question2_1_1h) == true
                        //        || Convert.ToBoolean(objIncubation.Question2_1_2g) == true || Convert.ToBoolean(objIncubation.Question2_1_2h) == true || Convert.ToBoolean(objIncubation.Question2_1_2i) == true
                        //        || Convert.ToBoolean(objIncubation.Question2_2i) == true || Convert.ToBoolean(objIncubation.Question2_2j) == true || Convert.ToBoolean(objIncubation.Question2_2k) == true
                        //         || Convert.ToBoolean(objIncubation.Question2_3_1g) == true || Convert.ToBoolean(objIncubation.Question2_3_1h) == true || Convert.ToBoolean(objIncubation.Question2_3_1i) == true)
                        //         && (ObjFundingStatus.Count == 0))

                        //    {
                        //        IsError = true;
                        //        errlist.Add("Please Add at least one Funding status in 3.8");
                        //    }
                        //}

                        if (objIncubation.Programme_Type.ToLower() == "hongkong")
                        {
                            if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "professional")
                            {
                                if (Convert.ToString(objIncubation.Question2_1_2b) != null || Convert.ToString(objIncubation.Question2_1_2c) != null || Convert.ToString(objIncubation.Question2_1_2d) != null)
                                {
                                    bool queb = Convert.ToBoolean(objIncubation.Question2_1_2b);
                                    bool quec = Convert.ToBoolean(objIncubation.Question2_1_2c);
                                    bool qued = Convert.ToBoolean(objIncubation.Question2_1_2d);
                                    if ((queb == true || quec == true || qued == true) && (ObjFundingStatus.Count == 0))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_fundingatleast"));

                                    }

                                    else if (queb == true || quec == true || qued == true)
                                    {

                                        foreach (TB_APPLICATION_FUNDING_STATUS obj1 in ObjFundingStatus)
                                        {
                                            if ((obj1.Currency == null) || (Convert.ToString(obj1.Date) == null) || (obj1.Programme_Name == "")
                                                || (obj1.Expenditure_Nature == "") || (obj1.Funding_Status == "") || (Convert.ToString(obj1.Maximum_Amount) == null)
                                                || (Convert.ToString(obj1.Amount_Received) == null))
                                            {
                                                IsError = true;
                                                errlist.Add(Localize("Error_Fundingall"));
                                            }

                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2d)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)))
                                {

                                    if ((Convert.ToBoolean(objIncubation.Question2_1_2d) == true || Convert.ToBoolean(objIncubation.Question2_1_2f) == true) && string.IsNullOrEmpty(objIncubation.Additional_Information))
                                    {
                                        IsError = true;
                                        errlist.Add(Localize("Error_additinal_info"));
                                    }
                                }

                            }
                            else
                            {
                                if (Convert.ToString(objIncubation.Question2_1_2d) != null || Convert.ToString(objIncubation.Question2_1_2e) != null || Convert.ToString(objIncubation.Question2_1_2f) != null)
                                {
                                    bool qued = Convert.ToBoolean(objIncubation.Question2_1_2d);
                                    bool quee = Convert.ToBoolean(objIncubation.Question2_1_2e);
                                    bool quef = Convert.ToBoolean(objIncubation.Question2_1_2f);
                                    if ((qued == true || quee == true || quef == true) && (ObjFundingStatus.Count == 0))
                                    {

                                        IsError = true;
                                        errlist.Add(Localize("Error_fundingatleast"));
                                    }
                                    else if (qued == true || quee == true || quef == true)
                                    {

                                        foreach (TB_APPLICATION_FUNDING_STATUS obj1 in ObjFundingStatus)
                                        {
                                            if ((obj1.Currency == null) || (Convert.ToString(obj1.Date) == null) || (obj1.Programme_Name == "")
                                                || (obj1.Expenditure_Nature == "") || (obj1.Funding_Status == "") || (Convert.ToString(obj1.Maximum_Amount) == null)
                                                || (Convert.ToString(obj1.Amount_Received) == null))
                                            {
                                                IsError = true;
                                                errlist.Add(Localize("Error_Fundingall"));
                                            }

                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2f)) || !string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_2h)))
                                {

                                    if ((Convert.ToBoolean(objIncubation.Question2_1_2f) == true || Convert.ToBoolean(objIncubation.Question2_1_2h) == true) && string.IsNullOrEmpty(objIncubation.Additional_Information))
                                    {
                                        IsError = true;
                                        errlist.Add(Localize("Error_additinalreq").Replace("2.1.2(g)", "2.1.2(e)").Replace("2.1.2(h)", "2.1.2(f)"));
                                    }
                                }

                            }
                        }

                        if (objIncubation.Programme_Type.ToLower().Contains("crossborder"))
                        {
                            if (Convert.ToString(objIncubation.Question2_1_2f) != null || Convert.ToString(objIncubation.Question2_1_2g) != null || Convert.ToString(objIncubation.Question2_1_2h) != null)
                            {
                                bool quef = Convert.ToBoolean(objIncubation.Question2_1_2f);
                                bool queg = Convert.ToBoolean(objIncubation.Question2_1_2g);
                                bool queh = Convert.ToBoolean(objIncubation.Question2_1_2h);
                                if ((quef == true || queg == true || queh == true) && (ObjFundingStatus.Count == 0))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_fundingreq1"));

                                }
                                else if (quef == true || queg == true || queh == true)
                                {

                                    foreach (TB_APPLICATION_FUNDING_STATUS obj1 in ObjFundingStatus)
                                    {
                                        if ((obj1.Currency == null) || (Convert.ToString(obj1.Date) == null) || (obj1.Programme_Name == "")
                                            || (obj1.Expenditure_Nature == "") || (obj1.Funding_Status == "") || (Convert.ToString(obj1.Maximum_Amount) == null)
                                            || (Convert.ToString(obj1.Amount_Received) == null))
                                        {
                                            IsError = true;
                                            errlist.Add(Localize("Error_Fundingall"));
                                        }

                                    }
                                }
                            }

                            //if(Convert.ToString(objIncubation.Question2_1_2i) != null || Convert.ToString(objIncubation.Question2_1_2j)!= null)
                            //{
                            //    bool quei = Convert.ToBoolean(objIncubation.Question2_1_2i);
                            //    bool quej = Convert.ToBoolean(objIncubation.Question2_1_2j);

                            //    if (quei == true || quej == true)
                            //    {

                            //        IsError = true;
                            //        errlist.Add("Additional Information, if 2.1.2(i) or 2.1.2(j) is Yes ");

                            //    }
                            //}
                        }

                        if (objIncubation.Programme_Type.ToLower() == "cupp")
                        {
                            if (Convert.ToString(objIncubation.Question2_1_2d) != null || Convert.ToString(objIncubation.Question2_1_2e) != null || Convert.ToString(objIncubation.Question2_1_2f) != null)
                            {
                                bool qued = Convert.ToBoolean(objIncubation.Question2_1_2d);
                                bool quee = Convert.ToBoolean(objIncubation.Question2_1_2e);
                                bool quef = Convert.ToBoolean(objIncubation.Question2_1_2f);
                                if ((qued == true || quee == true || quef == true) && (ObjFundingStatus.Count == 0))
                                {

                                    IsError = true;
                                    errlist.Add(Localize("Error_fundingreq"));

                                }
                                else if (quef == true || qued == true || quee == true)
                                {

                                    foreach (TB_APPLICATION_FUNDING_STATUS obj1 in ObjFundingStatus)
                                    {
                                        if ((obj1.Currency == null) || (Convert.ToString(obj1.Date) == null) || (obj1.Programme_Name == "")
                                            || (obj1.Expenditure_Nature == "") || (obj1.Funding_Status == "") || (Convert.ToString(obj1.Maximum_Amount) == null)
                                            || (Convert.ToString(obj1.Amount_Received) == null))
                                        {
                                            IsError = true;
                                            errlist.Add(Localize("Error_Fundingall"));
                                        }

                                    }
                                }
                            }
                        }
                        //if (!string.IsNullOrEmpty(objIncubation.Programme_Type) && !string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                        //{
                        //    Activation = objIncubation.Programme_Type;
                        //    string activation1 = objIncubation.Hong_Kong_Programme_Stream;
                        //    if (Activation.ToLower() == "hongkong" && activation1.ToLower() == "Young Entrepreneur")
                        //    {
                        //        if (objTB_APPLICATION_ATTACHMENT != null)
                        //        {
                        //            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Student_ID.ToString().ToLower()))
                        //            {
                        //                IsError = true;

                        //                errlist.Add("Please upload document in the required field:  Student ID");
                        //            }
                        //        }
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(objIncubation.Programme_Type) && !string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                        //{
                        //    Activation = objIncubation.Programme_Type;
                        //    string activation1 = objIncubation.Hong_Kong_Programme_Stream;
                        //    if (Activation.ToLower() == "cross_border")
                        //    {
                        //        if (objTB_APPLICATION_ATTACHMENT != null)
                        //        {
                        //            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Student_ID.ToString().ToLower()))
                        //            {
                        //                IsError = true;

                        //                errlist.Add("Please upload document in the required field:  Student ID");
                        //            }
                        //        }
                        //    }
                        //}
                        if (ObjTB_APPLICATION_CONTACT_DETAIL.Count == 0)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_contact_atleast"));
                        }
                        int i = 1;
                        foreach (TB_APPLICATION_CONTACT_DETAIL obj in ObjTB_APPLICATION_CONTACT_DETAIL)
                        {

                            if (string.IsNullOrEmpty(obj.First_Name_Eng) && string.IsNullOrEmpty(obj.Last_Name_Eng) && string.IsNullOrEmpty(obj.Salutation) && string.IsNullOrEmpty(obj.Contact_No)
                                && string.IsNullOrEmpty(obj.Email) && string.IsNullOrEmpty(obj.Mailing_Address))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_contact"));
                                break;
                            }

                            if (string.IsNullOrEmpty(obj.First_Name_Eng))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_First_Name_Eng"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj.Last_Name_Eng))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Last_Name_Eng"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj.Contact_No))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Contact_Empty"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj.Email))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Email_Empty"), i.ToString()));
                            }

                            if (string.IsNullOrEmpty(obj.Mailing_Address))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Mailing_Address"), i.ToString()));
                            }
                            i++;
                        }
                        if (objIncubation.Programme_Type.ToLower().Contains("crossborder") && ObjTB_APPLICATION_CONTACT_DETAIL.Count != 0)
                        {
                            if ((ObjTB_APPLICATION_CONTACT_DETAIL.Where(x => x.Area == "HongKong")).Count() == 0 || (ObjTB_APPLICATION_CONTACT_DETAIL.Where(x => x.Area == "China")).Count() == 0)
                            {
                                errlist.Add(Localize("Error_contact_area"));
                            }

                        }
                        //string apptype = objIncubation.CCMF_Application_Type;
                        if (apptype.ToLower() == "company")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                            {
                                if (Convert.ToBoolean(objIncubation.Question2_1_1a) == true && !objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.BR_COPY.ToString().ToLower()))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_brcopy"));
                                }
                            }
                        }

                        //if (!string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                        //{
                        //    if (objIncubation.Hong_Kong_Programme_Stream.ToLower() != "professional")
                        //    {

                        //        if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Student_ID.ToString().ToLower()))
                        //        {
                        //            IsError = true;
                        //            errlist.Add(Localize("Error_studentid"));
                        //        }

                        //    }
                        //}
                        if (objIncubation.Programme_Type.ToLower() != "hongkong")
                        {

                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Student_ID.ToString().ToLower()))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_studentid"));
                            }


                        }
                        string apptype1 = objIncubation.CCMF_Application_Type;
                        if (apptype1.ToLower() == "individual" || IsYepApplication == true)
                        {

                            if (!string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream) && !objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                            {
                                // HK ID Changes for Professional with question
                                if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "professional")
                                {
                                    if (objIncubation.Question2_1_1b.HasValue)
                                    {
                                        if (objIncubation.Question2_1_1b.Value == true)
                                        {
                                            IsError = true;
                                            errlist.Add(Localize("Error_hkid"));
                                        }
                                    }
                                }
                                else // For all individual and YEP
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_hkid"));
                                }


                                /* if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "professional")
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1b)))
                                    {
                                        if (Convert.ToBoolean(objIncubation.Question2_1_1b) == true && !objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                                        {
                                            IsError = true;
                                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                                            {

                                                errlist.Add("Please upload document in the required field: HK ID");
                                            }
                                        }
                                    }
                                }
                                else if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "young entrepreneur")
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                    {
                                        if (Convert.ToBoolean(objIncubation.Question2_1_1a) == true && !objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                                        {
                                            IsError = true;
                                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                                            {

                                                errlist.Add("Please upload document in the required field: HK ID");
                                            }
                                        }
                                    }

                                }*/

                            }
                            else if (objIncubation.Programme_Type.ToLower() == "cupp")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(objIncubation.Question2_1_1a)))
                                {
                                    if (Convert.ToBoolean(objIncubation.Question2_1_1a) == true && !objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                                    {
                                        IsError = true;
                                        if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()))
                                        {

                                            errlist.Add(Localize("Error_hkid"));
                                        }
                                    }
                                }
                            }

                        }

                        if (objIncubation.Declaration == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_declaration"));
                        }
                        if (objIncubation.Have_Read_Statement == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_have_read"));
                        }
                        if (objIncubation.Principal_Full_Name == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_full_applicant"));
                        }
                        if (objIncubation.Principal_Position_Title == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_applicant_position"));
                        }
                        //if (Convert.ToString(objIncubation.CCMF_Application_Type) != "")
                        //{
                        //    Activation = objIncubation.CCMF_Application_Type;
                        //    if (Activation.ToLower() == "individual" && Convert.ToString(objIncubation.Question2_3_1a) == "")
                        //    {
                        //        IsError = true;

                        //        ErrorMessage += "Please select Question 2.3.1(A)<br>";
                        //    }
                        //}
                        //if (Convert.ToString(objIncubation.CCMF_Application_Type) != "")
                        //{
                        //    Activation = objIncubation.CCMF_Application_Type;
                        //    if (Activation.ToLower() == "company" && Convert.ToString(objIncubation.Question2_3_1b) == "")
                        //    {
                        //        IsError = true;

                        //        ErrorMessage += "Please select Question 2.3.1(B)<br>";
                        //    }
                        //}
                        //if (Convert.ToString(objIncubation.CCMF_Application_Type) != "")
                        //{
                        //    Activation = objIncubation.CCMF_Application_Type;
                        //    if (Activation.ToLower() == "company" && Convert.ToString(objIncubation.Question2_3_1c) == "")
                        //    {
                        //        IsError = true;
                        //        ErrorMessage += "Please select Question 2.3.1(C)<br>";
                        //    }
                        //}
                        //    List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(objIncubation.CCMF_ID);
                        //    if (ObjFundingStatus.Count == 0)
                        //    {
                        //        IsError = true;
                        //        ErrorMessage += "At least one Funding Status required<br>";
                        //    }
                        //    if ((objIncubation.Question1_8 == true || objIncubation.Question1_9 == true) && string.IsNullOrEmpty(objIncubation.Additional_Information.Trim()))
                        //    {
                        //        IsError = true;
                        //        ErrorMessage += "2.3.2.4 Additional Information can not be empty<br>";
                        //    }

                        //    if (Convert.ToString(objIncubation.Resubmission) != "")
                        //    {

                        //        if ((bool)objIncubation.Resubmission)
                        //        {

                        //            if (string.IsNullOrEmpty(objIncubation.Resubmission_Project_Reference.Trim()) ||
                        //                string.IsNullOrEmpty(objIncubation.Resubmission_Main_Differences.Trim()))
                        //            {
                        //                IsError = true;
                        //                ErrorMessage += "Section 2.4.7 and 2.4.8 can not be empty<br>";

                        //            }


                        //        }
                        //    }


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
                string hdn_year = ((HiddenField)e.Row.FindControl("hdn_year")).Value;
                string hdn_month = ((HiddenField)e.Row.FindControl("hdn_month")).Value;
                string hdn_area = ((HiddenField)e.Row.FindControl("hdn_Area")).Value;
                TextBox Dateofgraduation = (TextBox)e.Row.FindControl("Dateofgraduation");
                Label lblDateofgraduation = (Label)e.Row.FindControl("lblDateofgraduation");
                if (!string.IsNullOrEmpty(hdn_year) && !string.IsNullOrEmpty(hdn_month))
                {
                    string monthName = new DateTime(Convert.ToInt32(hdn_year), Convert.ToInt32(hdn_month), 1).ToString("MMM", CultureInfo.InvariantCulture);
                    Dateofgraduation.Text = monthName + "-" + hdn_year;
                    lblDateofgraduation.Text = Dateofgraduation.Text;
                }
                //hdnSalutation
                DropDownList Salutation = (DropDownList)e.Row.FindControl("Salutation");
                Salutation.SelectedValue = hdnSalutation;
                RadioButtonList Area1 = (RadioButtonList)e.Row.FindControl("rdo_Area");
                Area1.SelectedValue = hdn_area;
                int progId = Convert.ToInt32(hdn_ProgramID.Value);
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                    if (objIncubation != null)
                    {
                        if (objIncubation.Programme_Type.ToLower().Contains("crossborder"))
                        {
                            RadioButtonList Area = (RadioButtonList)e.Row.FindControl("rdo_Area");
                            Area.Visible = true;
                        }
                    }
                }
            }
        }

        protected TB_CCMF_APPLICATION GetExistingCCMF(CyberportEMS_EDM dbContext, int programId)
        {
            TB_CCMF_APPLICATION objCCMF = null;
            List<TB_CCMF_APPLICATION> objCCFMList = new List<TB_CCMF_APPLICATION>();
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
                    objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && x.CCMF_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                    if (objCCFMList.Count > 0)
                    {
                        objCCMF = objCCFMList.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                    }
                    else objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && x.CCMF_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());

                    if (objCCMF == null)
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
                        objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());

                    }
                    else
                    {
                        objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.CCMF_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                        if (objCCMF != null)
                        {
                            if (objTB_PROGRAMME_INTAKE.Application_Deadline < DateTime.Now)
                            {
                                objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (string.IsNullOrEmpty(x.Application_Parent_ID)) && (x.Created_By.ToLower() == strCurrentUser) && !(x.Status == formsubmitaction.Saved.ToString() || x.Status == formsubmitaction.Deleted.ToString()));
                            }
                            else
                            {

                                objCCFMList = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId && !string.IsNullOrEmpty(x.Application_Parent_ID) && x.Created_By.ToLower() == strCurrentUser.ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                                if (objCCFMList.Count > 0)
                                {
                                    objCCMF = objCCFMList.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                                }
                                else
                                    objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());
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
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                    objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);


                objCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && x.CCMF_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                UserCollaborator();
            }

            if (objCCMF != null)
            {
                hdn_ApplicationID.Value = objCCMF.CCMF_ID.ToString();
            }
            return objCCMF;
        }
        protected void UserCollaborator()
        {
            btn_Submit.Enabled = false;
        }
        protected void quicklnk_1_Click(object sender, EventArgs e)
        {

            using (var dbContext = new CyberportEMS_EDM())
            {
                bool IsError = false;
                int progId = Convert.ToInt32(hdn_ProgramID.Value);
                string id = ((LinkButton)sender).CommandArgument;
                SPFunctions objfunction = new SPFunctions();
                TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2 && (!rdo_Crossborder.Checked) && (!rdo_HK.Checked) && (!rdo_CUPP.Checked))
                {
                    ShowbottomMessage(Localize("Error_CCMF_oprtions"), false);
                    IsError = true;
                }
                else if (objIncubation == null)
                {
                    if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2)
                    {

                        if (rdo_CCMFApplication.SelectedValue == "" && ((!rdo_HK.Checked) || (!rdo_Crossborder.Checked) || (!rdo_CUPP.Checked)))
                        {
                            ShowbottomMessage(Localize("Error_CCMF_oprtions"), false);
                            IsError = true;
                        }

                        else if (rdo_HK.Checked)
                        {
                            if (rdo_HK_Option.SelectedValue == "")
                            {
                                ShowbottomMessage(Localize("Error_CCMF_oprtions"), false);
                                IsError = true;
                            }
                            else
                            {
                                SaveStep1Data();
                            }

                        }
                        else if (rdo_Crossborder.Checked)
                        {
                            if (rdo_CrossborderOptions.SelectedValue == "")
                            {
                                ShowbottomMessage(Localize("Error_CCMF_oprtions"), false);
                                IsError = true;
                            }
                            else
                            {
                                SaveStep1Data();
                            }

                        }
                        else
                        {
                            SaveStep1Data();
                        }



                    }

                }
                else
                {
                    SaveStep1Data();
                }
                if (IsError == true)
                {
                    id = "1";
                }

                SetPanelVisibilityOfStep(Convert.ToInt32(id));
                ShowHideControlsBasedUponUserData();

            }

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
                    TB_CCMF_APPLICATION objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    if (objCCMF == null)
                    {
                        objCCMF = new TB_CCMF_APPLICATION();
                        objCCMF.CCMF_ID = NewProgramId();
                        hdn_ApplicationID.Value = objCCMF.CCMF_ID.ToString();
                        objCCMF.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
                        //int count = (dbContext.TB_INCUBATION_APPLICATION.Where(x=>x.Programme_ID == objIncubation.Programme_ID).Count() + 1);
                        //int count = (dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == objIncubation.Programme_ID).Count());
                        int count = 0;
                        int programId = Convert.ToInt32(hdn_ProgramID.Value);
                        var result = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                        if (result != null)
                        {
                            count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                        }
                        else
                        {
                            count = 1;
                        }
                        lblApplicationNo.Text = HttpUtility.HtmlEncode(dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                        objCCMF.Application_Number = lblApplicationNo.Text;
                        objCCMF.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                        objCCMF.Applicant = objfunction.GetCurrentUser();
                        objCCMF.Last_Submitted = DateTime.Now;
                        objCCMF.Status = "Saved";
                        objCCMF.Version_Number = "0.01";
                        objCCMF.Created_By = objfunction.GetCurrentUser();
                        objCCMF.Created_Date = DateTime.Now;
                        objCCMF.Modified_By = objfunction.GetCurrentUser();
                        objCCMF.Modified_Date = DateTime.Now;
                        dbContext.TB_CCMF_APPLICATION.Add(objCCMF);
                        dbContext.SaveChanges();
                        objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    }
                    if (objCCMF != null)
                    {
                        if (objCCMF.Submission_Date.HasValue && objCCMF.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                        {
                            ReSubmitVersionCopy();
                            objCCMF = GetExistingCCMF(dbContext, progId);
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
                                            _fileUrl = objSPFunctions.AttachmentSave(objCCMF.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                    fu_BrCopy, enumAttachmentType.BR_COPY, objCCMF.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.BR_COPY, objCCMF.CCMF_ID, objCCMF.Programme_ID);
                                            BRCOPY.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            BRCOPY.Text = Localize("Error_file_format");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        BRCOPY.Text = Localize("Error_file_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    BRCOPY.Text = Localize("Error_upload_file");
                                }
                                break;

                            case 2:
                                if (fu_StudentID.HasFile)
                                {
                                    if (fu_StudentID.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_StudentID.FileName.Remove(0, fu_StudentID.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {

                                            List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objCCMF.CCMF_ID).ToList();
                                            attachments = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Student_ID.ToString().ToLower()).ToList();

                                            _fileUrl = objSPFunctions.AttachmentSave(objCCMF.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                             fu_StudentID, enumAttachmentType.Student_ID, attachments.Count().ToString());
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Student_ID, objCCMF.CCMF_ID, objCCMF.Programme_ID);
                                            lblStudentID.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lblStudentID.Text = Localize("Error_file_format");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lblStudentID.Text = Localize("Error_file_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    lblStudentID.Text = Localize("Error_upload_file");
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

                                            _fileUrl = objSPFunctions.AttachmentSave(objCCMF.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                            fuPresentationSlide, enumAttachmentType.Presentation_Slide, objCCMF.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Presentation_Slide, objCCMF.CCMF_ID, objCCMF.Programme_ID);
                                            PresentationSlide.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            PresentationSlide.Text = Localize("Error_file_presentation_format");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        PresentationSlide.Text = Localize("Error_file_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    PresentationSlide.Text = Localize("Error_upload_file");
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
                                            List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objCCMF.CCMF_ID).ToList();
                                            attachments = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Other_Attachment.ToString().ToLower()).ToList();

                                            _fileUrl = objSPFunctions.AttachmentSave(objCCMF.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                            fuOtherAttachement, enumAttachmentType.Other_Attachment, attachments.Count().ToString());
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Other_Attachment, objCCMF.CCMF_ID, objCCMF.Programme_ID);
                                            OtherAttachement.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            OtherAttachement.Text = Localize("Error_file_format");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        OtherAttachement.Text = Localize("Error_file_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    OtherAttachement.Text = Localize("Error_upload_file");
                                }
                                break;
                            case 5:
                                if (fu_HKID.HasFile)
                                {
                                    if (fu_HKID.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_HKID.FileName.Remove(0, fu_HKID.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objCCMF.CCMF_ID).ToList();
                                            attachments = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower()).ToList();
                                            _fileUrl = objSPFunctions.AttachmentSave(objCCMF.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                    fu_HKID, enumAttachmentType.HK_ID, attachments.Count().ToString());
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.HK_ID, objCCMF.CCMF_ID, objCCMF.Programme_ID);
                                            lblHKID.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lblHKID.Text = Localize("Error_file_format");
                                        }
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    lblHKID.Text = Localize("Error_upload_file");
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
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    if (objIncubation != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objIncubation.CCMF_ID).ToList();
                        rptrbrcopy.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.BR_COPY.ToString().ToLower());
                        rptrbrcopy.DataBind();
                        rptrHKID.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.HK_ID.ToString().ToLower());
                        rptrHKID.DataBind();
                        rptrpresentation.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Presentation_Slide.ToString().ToLower());
                        rptrpresentation.DataBind();
                        rptrOtherAttachement.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Other_Attachment.ToString().ToLower());
                        rptrOtherAttachement.DataBind();

                        rptrStudentID.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Student_ID.ToString().ToLower());
                        rptrStudentID.DataBind();

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
            while (new CyberportEMS_EDM().TB_CCMF_APPLICATION.Where(x => x.CCMF_ID == objNewId).Count() == 0)
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
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                    //TB_CCMF_APPLICATION objIncubationNew = objIncubation;
                    //Guid objinbubationExitingId = objIncubation.CCMF_ID;
                    //dbContext.TB_CCMF_APPLICATION.Add(objIncubationNew);
                    //objIncubationNew.Submission_Date = null;
                    //objIncubationNew.Last_Submitted = DateTime.Now;
                    ////objIncubationNew.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal("1")).ToString("F2");
                    //objIncubationNew.Status = "Saved";
                    //objIncubationNew.Application_Parent_ID = !string.IsNullOrEmpty(objIncubation.Application_Parent_ID) ? objIncubation.Application_Parent_ID : objIncubation.CCMF_ID.ToString();
                    //objIncubationNew.CCMF_ID = NewProgramId();
                    //dbContext.Entry(objIncubationNew).State = System.Data.Entity.EntityState.Added;

                    TB_CCMF_APPLICATION objIncubationNew = objIncubation;
                    Guid objinbubationExitingId = objIncubation.CCMF_ID;
                    dbContext.TB_CCMF_APPLICATION.Add(objIncubationNew);
                    objIncubationNew.Submission_Date = null;
                    objIncubationNew.Modified_Date = DateTime.Now;
                    objIncubationNew.Last_Submitted = DateTime.Now;
                    //objIncubationNew.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal("1")).ToString("F2");
                    objIncubationNew.Status = "Saved";
                    objIncubationNew.Application_Parent_ID = !string.IsNullOrEmpty(objIncubation.Application_Parent_ID) ? objIncubation.Application_Parent_ID : objIncubation.CCMF_ID.ToString();
                    objIncubationNew.CCMF_ID = NewProgramId();
                    dbContext.Entry(objIncubationNew).State = System.Data.Entity.EntityState.Added;
                    List<TB_APPLICATION_ATTACHMENT> objIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_ATTACHMENT.AddRange(objIncubationAttachement);
                    foreach (TB_APPLICATION_ATTACHMENT objAttach in objIncubationAttachement)
                    {
                        objAttach.Application_ID = objIncubationNew.CCMF_ID;
                    }
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.AddRange(objTB_APPLICATION_COMPANY_CORE_MEMBER);
                    foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objAttach in objTB_APPLICATION_COMPANY_CORE_MEMBER)
                    {
                        objAttach.Application_ID = objIncubationNew.CCMF_ID;
                    }
                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_CONTACT_DETAIL.AddRange(objTB_APPLICATION_CONTACT_DETAIL);
                    foreach (TB_APPLICATION_CONTACT_DETAIL objAttach in objTB_APPLICATION_CONTACT_DETAIL)
                    {
                        objAttach.Application_ID = objIncubationNew.CCMF_ID;
                    }
                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objIncubation.Programme_ID).ToList();
                    dbContext.TB_APPLICATION_FUNDING_STATUS.AddRange(objTB_APPLICATION_FUNDING_STATUS);
                    foreach (TB_APPLICATION_FUNDING_STATUS objAttach in objTB_APPLICATION_FUNDING_STATUS)
                    {
                        objAttach.Application_ID = objIncubationNew.CCMF_ID;
                    }


                    dbContext.SaveChanges();
                    InitializeUploadsDocument();

                }


            }
            catch (Exception ex)
            {
            }
        }
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
                    //Label lblcontactSubTitle = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactSubTitle");
                    RadioButtonList Area = (RadioButtonList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("rdo_Area");
                    TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                    TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                    TextBox txtlastchiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtlast_chiname");
                    TextBox txtFisrtChiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtFisrt_Chiname");
                    //TextBox identitycard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("identity_card");
                    TextBox txtContactNo = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNo");
                    TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                    TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                    TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                    TextBox Nameofinsititute = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Nameofinsititute");
                    TextBox Studentidcard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Studentidcard");
                    TextBox Programmerolled = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Programmerolled");
                    TextBox Dateofgraduation = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Dateofgraduation");
                    TextBox OrganisationName = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("OrganisationName");
                    TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Position");
                    DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");

                    //lblcontactSubTitle.Visible = false;


                    TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                    objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);


                    objMember.Last_Name_Eng = txtContactLast_name.Text;
                    objMember.First_Name_Eng = txtContactFirst_name.Text;
                    objMember.Last_Name_Chi = txtlastchiname.Text;
                    objMember.First_Name_Chi = txtFisrtChiname.Text;
                    //objMember.ID_Number = identitycard.Text;
                    objMember.Area = Area.SelectedValue;

                    objMember.Contact_No = txtContactNo.Text;

                    objMember.Fax = Fax.Text;

                    objMember.Email = Email.Text;
                    objMember.Mailing_Address = Mailing_Address.Text;
                    objMember.Education_Institution_Eng = Nameofinsititute.Text;
                    objMember.Student_ID_Number = Studentidcard.Text;
                    objMember.Programme_Enrolled_Eng = Programmerolled.Text;
                    if (Dateofgraduation.Text != "")
                    {
                        var date = Dateofgraduation.Text.Split(new char[] { '-', ' ' });

                        objMember.Graduation_Month = DateTime.ParseExact(date[0], "MMM", CultureInfo.CurrentCulture).Month;
                        //objMember.Graduation_Month = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(date[0]) + 1;
                        objMember.Graduation_Year = Convert.ToInt32(date[1]);
                    }

                    objMember.Organisation_Name = OrganisationName.Text;
                    objMember.Position = Position.Text;
                    objMember.Salutation = Salutation.Text;

                    objMember.Application_ID = Guid.Parse(hdn_ApplicationID.Value);
                    objContactDetails.Add(objMember);




                }
                catch (Exception ex)
                {
                    IsError = true;

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


        protected void ShowHideControlsBasedUponUserData(bool ForLanguageOnly = false)
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);

                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

                    if (objIncubation != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT = IncubationContext.ListofTB_APPLICATION_ATTACHMENTGGet(objIncubation.CCMF_ID, objIncubation.Programme_ID);
                        bool isDisabled = lblEnglish.Visible;

                        string apptype = objIncubation.CCMF_Application_Type;
                        if (apptype.ToLower() == "company")
                        {
                            attachbrcopy.Visible = true;
                            attachhkid.Visible = false;
                            br_copy.InnerText = "6.1 " + SPFunctions.LocalizeUI("BRCOPY", "CyberportEMS_CCMF");//BR Copy";


                            studnt_id.InnerText = "6.2 " + SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMF");//Student ID";

                            video_clip.InnerText = "6.3 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                            presentation_slide.InnerText = "6.4 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                            other.InnerText = "6.5 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";


                        }
                        else
                        {
                            attachbrcopy.Visible = false;
                            attachhkid.Visible = true;


                            studnt_id.InnerText = "6.1 " + SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMF");//Student ID";
                            hk_id.InnerText = "6.2 " + SPFunctions.LocalizeUI("HKID", "CyberportEMS_CCMF");//HK ID";
                            video_clip.InnerText = "6.3 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                            presentation_slide.InnerText = "6.4 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                            other.InnerText = "6.5 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";
                        }

                        if (!string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                        {
                            if (objIncubation.Hong_Kong_Programme_Stream.ToLower() != "professional")
                            {
                                if (apptype.ToLower() == "company")
                                {
                                    attachstudentid.Visible = false;
                                    attachbrcopy.Visible = true;
                                    attachhkid.Visible = true;

                                    br_copy.InnerText = "6.1 " + SPFunctions.LocalizeUI("BRCOPY", "CyberportEMS_CCMF");//BR Copy";
                                    hk_id.InnerText = "6.2 " + SPFunctions.LocalizeUI("HKID", "CyberportEMS_CCMF");//HK ID";                                                                               //ss8 changes
                                                                                                                   //  studnt_id.InnerText = "6.2 " + SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMF");//Student ID";

                                    video_clip.InnerText = "6.3 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                                    presentation_slide.InnerText = "6.4 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                                    other.InnerText = "6.5 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";


                                }
                                else
                                {
                                    attachstudentid.Visible = false;
                                    attachbrcopy.Visible = false;
                                    attachhkid.Visible = true;
                                    //ss8 changes
                                    //  studnt_id.InnerText = "6.1 " + SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMF");//Student ID";
                                    hk_id.InnerText = "6.1 " + SPFunctions.LocalizeUI("HKID", "CyberportEMS_CCMF");//HK ID";
                                    video_clip.InnerText = "6.2 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                                    presentation_slide.InnerText = "6.3 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                                    other.InnerText = "6.4 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";
                                }

                                //ss8 changes
                                //  attachstudentid.Visible = true;
                                attachstudentid.Visible = false;
                            }
                            else
                            {
                                if (apptype.ToLower() == "company")
                                {

                                    br_copy.InnerText = "6.1 " + SPFunctions.LocalizeUI("BRCOPY", "CyberportEMS_CCMF");//BR Copy";


                                    video_clip.InnerText = "6.2 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                                    presentation_slide.InnerText = "6.3 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                                    other.InnerText = "6.4 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";


                                }
                                else
                                {



                                    hk_id.InnerText = "6.1 " + SPFunctions.LocalizeUI("HKID", "CyberportEMS_CCMF");//HK ID";
                                    video_clip.InnerText = "6.2 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                                    presentation_slide.InnerText = "6.3 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                                    other.InnerText = "6.4 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";
                                }
                            }
                        }
                        else if (objIncubation.Programme_Type.ToLower() != "hongkong")
                        {
                            if (apptype.ToLower() == "company")
                            {

                                br_copy.InnerText = "6.1 " + SPFunctions.LocalizeUI("BRCOPY", "CyberportEMS_CCMF");//BR Copy";
                                studnt_id.InnerText = "6.2 " + SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMF");//Student ID";

                                video_clip.InnerText = "6.3 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                                presentation_slide.InnerText = "6.4 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                                other.InnerText = "6.5 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";


                            }
                            else
                            {


                                studnt_id.InnerText = "6.1 " + SPFunctions.LocalizeUI("StudentID", "CyberportEMS_CCMF");//Student ID";
                                hk_id.InnerText = "6.2 " + SPFunctions.LocalizeUI("HKID", "CyberportEMS_CCMF");//HK ID";
                                video_clip.InnerText = "6.3 " + SPFunctions.LocalizeUI("VideoClip", "CyberportEMS_CCMF");//Video Clip";
                                presentation_slide.InnerText = "6.4 " + SPFunctions.LocalizeUI("PresentationSlide", "CyberportEMS_CCMF");//Presentation Slide";
                                other.InnerText = "6.5 " + SPFunctions.LocalizeUI("OtherAttachment", "CyberportEMS_CCMF");//Other Attachment";
                            }

                            attachstudentid.Visible = true;


                        }
                        if (ForLanguageOnly == false)
                        {

                            //if (!string.IsNullOrEmpty(objIncubation.Business_Area))
                            //{
                            //    if (objIncubation.Business_Area.ToLower().Trim() == "others")
                            //    {
                            //        txtOther_Bussiness_Area.Text = objIncubation.Other_Business_Area;
                            //        txtOther_Bussiness_Area.Style.Remove("display");
                            //        txtOther_Bussiness_Area.Visible = true;
                            //        if (isDisabled)
                            //        {
                            //            txtOther_Bussiness_Area.Style.Add("display", "none");
                            //            lblOther_Bussiness_Area.Visible = true;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        txtOther_Bussiness_Area.Style.Add("display", "none");
                            //        lblOther_Bussiness_Area.Visible = false;
                            //    }
                            //}
                            //else
                            //{
                            //    txtOther_Bussiness_Area.Style.Add("display", "none");
                            //    lblOther_Bussiness_Area.Visible = false;
                            //}

                            if (RadioButtonList1.SelectedValue != "")
                            {
                                if (RadioButtonList1.SelectedValue.ToLower().Trim() == "others")
                                {
                                    txtOther_Bussiness_Area.Style.Remove("display");
                                    txtOther_Bussiness_Area.Visible = true;
                                    if (isDisabled)
                                    {
                                        txtOther_Bussiness_Area.Style.Add("display", "none");
                                        lblOther_Bussiness_Area.Visible = true;
                                    }
                                }
                                else
                                {
                                    txtOther_Bussiness_Area.Style.Add("display", "none");
                                    lblOther_Bussiness_Area.Visible = false;
                                }
                            }
                            else
                            {
                                txtOther_Bussiness_Area.Style.Add("display", "none");
                                lblOther_Bussiness_Area.Visible = false;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {



            }

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

        //private string SPFunctions.LocalizeUI(string Key)
        //{
        //    return SPUtility.GetSPFunctions.LocalizeUIdString("$Resources:" + Key, "CyberportEMS_CCMF", (uint)SPContext.Current.Web.Locale.LCID);

        //}
        //public static string LocalizeUI(string Key)
        //{
        //    return SPFunctions.LocalizeUI(Key, "CyberportEMS_CCMF");
        //}
        protected int check_db_validations(bool IsSubmitClick)
        {

            List<String> ErrorLIst = new List<string>();
            using (var dbContext = new CyberportEMS_EDM())
            {

                int progId = Convert.ToInt32(hdn_ProgramID.Value);

                SPFunctions objfunction = new SPFunctions();
                TB_CCMF_APPLICATION objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                //added
                TB_PROGRAMME_INTAKE objprogram = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(k => k.Programme_ID == progId);
                if (objprogram.Application_Deadline >= DateTime.Now || objCCMF.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                {
                    bool isnewobj = false;
                    if (objCCMF == null)
                    {
                        isnewobj = true;
                        objCCMF = new TB_CCMF_APPLICATION();
                    }
                    else if (objCCMF.Submission_Date.HasValue && objCCMF.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                    {
                        ReSubmitVersionCopy();
                        objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                    }

                    if (rdo_HK.Checked)
                    {
                        objCCMF.Programme_Type = "HongKong";
                        if (rdo_HK_Option.SelectedValue != "")
                        {
                            //if (rdo_HK_Option.SelectedValue.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 251, true, AllowAllSymbol: true), rdo_HK_Option.SelectedValue))
                            //    ErrorLIst.Add(Localize("Error_hongkngstream"));
                            //else
                            objCCMF.Hong_Kong_Programme_Stream = (rdo_HK_Option.SelectedValue);
                        }
                        else
                            ErrorLIst.Add(Localize("Error_CCMF_oprtions"));
                    }
                    else if (rdo_Crossborder.Checked)
                        if (rdo_CrossborderOptions.SelectedValue != "")
                            objCCMF.Programme_Type = (rdo_CrossborderOptions.SelectedValue);
                        else
                            ErrorLIst.Add(Localize("Error_CCMF_oprtions"));


                    if (rdo_CCMFApplication.SelectedValue != "")
                    {
                        if (rdo_CCMFApplication.SelectedValue.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 251, true, AllowAllSymbol: true), rdo_CCMFApplication.SelectedValue))
                            ErrorLIst.Add(Localize("Error_ccmfapplication"));
                        else
                            objCCMF.CCMF_Application_Type = rdo_CCMFApplication.SelectedValue;
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error_hongkong_options"));
                    }
                    if (rdo1_3.SelectedValue != "")
                    {
                        objCCMF.Question1_3 = Convert.ToBoolean(rdo1_3.SelectedValue);
                    }
                    if (rdo211a.SelectedValue != "")
                        objCCMF.Question2_1_1a = Convert.ToBoolean(rdo211a.SelectedValue);
                    if (rdo211b.SelectedValue != "")
                        objCCMF.Question2_1_1b = Convert.ToBoolean(rdo211b.SelectedValue);
                    if (rdo211c.SelectedValue != "")
                        objCCMF.Question2_1_1c = Convert.ToBoolean(rdo211c.SelectedValue);
                    if (rdo212a.SelectedValue != "")
                        objCCMF.Question2_1_2a = Convert.ToBoolean(rdo212a.SelectedValue);
                    if (rdo212b.SelectedValue != "")
                        objCCMF.Question2_1_2b = Convert.ToBoolean(rdo212b.SelectedValue);
                    if (rdo212c.SelectedValue != "")
                        objCCMF.Question2_1_2c = Convert.ToBoolean(rdo212c.SelectedValue);
                    if (rdo212d.SelectedValue != "")
                        objCCMF.Question2_1_2d = Convert.ToBoolean(rdo212d.SelectedValue);
                    if (rdo212e.SelectedValue != "")
                        objCCMF.Question2_1_2e = Convert.ToBoolean(rdo212e.SelectedValue);
                    if (rdo212f.SelectedValue != "")
                        objCCMF.Question2_1_2f = Convert.ToBoolean(rdo212f.SelectedValue);
                    if (rdo212g.SelectedValue != "")
                        objCCMF.Question2_1_2g = Convert.ToBoolean(rdo212g.SelectedValue);
                    if (rdo212h.SelectedValue != "")
                        objCCMF.Question2_1_2h = Convert.ToBoolean(rdo212h.SelectedValue);
                    if (rdo212i.SelectedValue != "")
                        objCCMF.Question2_1_2i = Convert.ToBoolean(rdo212i.SelectedValue);
                    if (rdo212j.SelectedValue != "")
                        objCCMF.Question2_1_2j = Convert.ToBoolean(rdo212j.SelectedValue);
                    if (rdo211c.SelectedValue != "")
                        objCCMF.Question2_1_2c = Convert.ToBoolean(rdo211c.SelectedValue);
                    //new question
                    if (rdo212f_1.SelectedValue != "")
                        objCCMF.Question2_1_2f_1 = Convert.ToBoolean(rdo212f_1.SelectedValue);

                    if (!string.IsNullOrEmpty(English.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), English.Text))
                        ErrorLIst.Add(Localize("Error_proname_eng"));
                    else objCCMF.Project_Name_Eng = English.Text.Trim();
                    if (!string.IsNullOrEmpty(Chinese.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Chinese.Text))
                        ErrorLIst.Add(Localize("Error_proname_chi"));
                    else objCCMF.Project_Name_Chi = Chinese.Text.Trim();
                    //objCCMF.Abstract_Eng = txtProjectInfoAbsEng.Text.Trim();
                    //objCCMF.Abstract_Chi = txtProjectInfoAbschi.Text.Trim();

                    if (!string.IsNullOrEmpty(txtCompanyName.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCompanyName.Text))
                        ErrorLIst.Add(Localize("Error_CompanyName_length"));
                    else objCCMF.Company_Name = txtCompanyName.Text.Trim();

                    if (txtestablishmentyear.Text != "")
                    {
                        DateTime dDate;
                        if (DateTime.TryParse(txtestablishmentyear.Text, out dDate))
                        {
                            String.Format("{0:M-yy}", dDate);
                            objCCMF.Establishment_Year = Convert.ToDateTime(txtestablishmentyear.Text.Trim());
                        }
                        else
                        {
                            ErrorLIst.Add(Localize("Error_Year_of_EstablishmentInvalid"));
                        }
                    }
                    else objCCMF.Establishment_Year = null;

                    if (rdbNEWHK.SelectedValue != "")
                        objCCMF.NEW_to_HK = Convert.ToBoolean(rdbNEWHK.SelectedValue);

                    objCCMF.Country_Of_Origin = ddlCountryOrigin.SelectedValue;

                    var text = txtProjectInfoAbsEng.Text.Trim();
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
                        ErrorLIst.Add(Localize("Error_Abstract_eng_length"));
                    else
                        objCCMF.Abstract_Eng = txtProjectInfoAbsEng.Text.Trim();
                    var text1 = txtProjectInfoAbschi.Text.Trim();
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
                        ErrorLIst.Add(Localize("Error_Abstract_chi_length"));
                    else
                        objCCMF.Abstract_Chi = txtProjectInfoAbschi.Text.Trim();
                    if (!string.IsNullOrEmpty(RadioButtonList1.SelectedValue) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 40, true, AllowAllSymbol: true), RadioButtonList1.SelectedValue))
                        ErrorLIst.Add(Localize("Error_businessarea_length"));
                    else objCCMF.Business_Area = RadioButtonList1.SelectedValue;

                    if (RadioButtonList1.SelectedValue.ToLower() == "others")
                    {
                        if (!string.IsNullOrEmpty(txtOther_Bussiness_Area.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtOther_Bussiness_Area.Text))
                            ErrorLIst.Add(Localize("Error_otherbusiness_length"));
                        else objCCMF.Other_Business_Area = txtOther_Bussiness_Area.Text.Trim();
                    }
                    if (txtantisdate.Text != "")
                    {
                        DateTime dDate;

                        if (DateTime.TryParse(txtantisdate.Text, out dDate))
                        {
                            String.Format("{0:dd-M-yy}", dDate);
                            objCCMF.Commencement_Date = Convert.ToDateTime(txtantisdate.Text.Trim());
                            //objCCMF.Commencement_Date = dDate;
                            ////objCCMF.Commencement_Date = Convert.ToDateTime(txtantisdate.Text.Trim());
                        }
                        else
                        {
                            ErrorLIst.Add(Localize("Error_Commenbcemenbtdate")); // <-- Control flow goes here
                        }

                    }
                    if (txtanticdate.Text != "")
                    {
                        DateTime dDate;

                        if (DateTime.TryParse(txtanticdate.Text, out dDate))
                        {
                            String.Format("{0:dd-M-yy}", dDate);
                            objCCMF.Completion_Date = Convert.ToDateTime(txtanticdate.Text.Trim());
                        }
                        else
                        {
                            ErrorLIst.Add(Localize("Error_Completiondate")); // <-- Control flow goes here
                        }

                    }
                    //newLine Added
                    objCCMF.SmartSpace = ddlsmartspace.SelectedValue;
                    objCCMF.Business_Model = txtbusinessmodelteam.Text.Trim();
                    objCCMF.Advisor_Info = txtAdvisor.Text.Trim();

                    objCCMF.Innovation = txtcreativity.Text.Trim();

                    objCCMF.Social_Responsibility = txtsocialrespon.Text.Trim();
                    objCCMF.Competition_Analysis = txtcompanalysis.Text.Trim();

                    /*if (!string.IsNullOrEmpty(txtmonth1.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtmonth1.Text))
                        ErrorLIst.Add("Project Milestone M1  should be less than 500 characters.");
                    else*/
                    objCCMF.Project_Milestone_M1 = txtmonth1.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtmonth2.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtmonth2.Text))
                    //    ErrorLIst.Add("Project Milestone M2  should be less than 500 characters.");
                    //else
                    objCCMF.Project_Milestone_M2 = txtmonth2.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtmonth3.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtmonth3.Text))
                    //    ErrorLIst.Add("Project Milestone M3  should be less than 500 characters.");
                    //else
                    objCCMF.Project_Milestone_M3 = txtmonth3.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtmonth4.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtmonth4.Text))
                    //    ErrorLIst.Add("Project Milestone M4  should be less than 500 characters.");
                    //else
                    objCCMF.Project_Milestone_M4 = txtmonth4.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtmonth5.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtmonth5.Text))
                    //    ErrorLIst.Add("Project Milestone M5  should be less than 500 characters.");
                    //else
                    objCCMF.Project_Milestone_M5 = txtmonth5.Text.Trim();
                    //if (!string.IsNullOrEmpty(txtmonth6.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtmonth6.Text))
                    //    ErrorLIst.Add("Project Milestone M6  should be less than 500 characters.");
                    //else
                    objCCMF.Project_Milestone_M6 = txtmonth6.Text.Trim();
                    objCCMF.Cost_Projection = txtcostprojection.Text.Trim();
                    objCCMF.Exit_Stategy = txtExitStrategy.Text.Trim();
                    objCCMF.Additional_Information = txtaddinformation.Text.Trim();
                    if (txtVideoClip.Text != "")
                    {
                        if (Common.CBPRegularExpression.RegExValidate(CBPRegularExpression.Url, txtVideoClip.Text.Trim()))

                            SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, objCCMF.CCMF_ID, progId);

                        else ErrorLIst.Add(Localize("Error_hyperlink"));
                    }
                    objCCMF.Declaration = chkDeclaration.Checked;
                    objCCMF.Marketing_Information = Marketing_Information.Checked;
                    objCCMF.Have_Read_Statement = Personal_Information.Checked;
                    if (!string.IsNullOrEmpty(txtName_PrincipalApplicant.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtName_PrincipalApplicant.Text))
                        ErrorLIst.Add(Localize("Error_Applicant_name_title"));
                    else
                        objCCMF.Principal_Full_Name = txtName_PrincipalApplicant.Text.Trim();

                    if (!string.IsNullOrEmpty(txtPosition_PrincipalApplicant.Text) && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtPosition_PrincipalApplicant.Text))
                        ErrorLIst.Add(Localize("Error_Applicant_position_lenghth"));
                    else
                        objCCMF.Principal_Position_Title = txtPosition_PrincipalApplicant.Text.Trim();
                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = GetFundingStatusForSave(IsSubmitClick, ref ErrorLIst);
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = GetCoreMemberForSave(IsSubmitClick, ref ErrorLIst);
                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = GetContactDetailsForSave(IsSubmitClick, ref ErrorLIst);
                    //if (txtVideoClip.Text.Length > 0 && !Common.CBPRegularExpression.RegExValidate(CBPRegularExpression.Url, txtVideoClip.Text.Trim()))
                    //    ErrorLIst.Add("Invalid Video clip url.");
                    try
                    {
                        if (ErrorLIst.Count == 0)
                        {


                            //objCCMF.Question1_1.Value
                            if (isnewobj)
                            {
                                objCCMF.CCMF_ID = NewProgramId();
                                hdn_ApplicationID.Value = objCCMF.CCMF_ID.ToString();
                                objCCMF.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
                                int count = 0;
                                int programId = Convert.ToInt32(hdn_ProgramID.Value);
                                var result = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId).OrderByDescending(x => x.Application_Number).FirstOrDefault();
                                if (result != null)
                                {
                                    count = Convert.ToInt32(result.Application_Number.Substring(result.Application_Number.Length - 4, 4)) + 1;
                                }
                                else
                                {
                                    count = 1;
                                }
                                lblApplicationNo.Text = HttpUtility.HtmlEncode(dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                                objCCMF.Application_Number = lblApplicationNo.Text;
                                objCCMF.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                                objCCMF.Applicant = objfunction.GetCurrentUser();
                                objCCMF.Last_Submitted = DateTime.Now;
                                objCCMF.Status = "Saved";
                                objCCMF.Version_Number = "0.01";
                                objCCMF.Created_By = objfunction.GetCurrentUser();
                                objCCMF.Created_Date = DateTime.Now;
                                objCCMF.Modified_By = objfunction.GetCurrentUser();
                                objCCMF.Modified_Date = DateTime.Now;
                                dbContext.TB_CCMF_APPLICATION.Add(objCCMF);
                            }
                            else
                            {
                                if (objCCMF.Status.ToLower().Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                {
                                    objCCMF.Status = formsubmitaction.Saved.ToString();
                                    decimal inc = (decimal)0.01;
                                    objCCMF.Version_Number = (Convert.ToDecimal(objCCMF.Version_Number) + inc).ToString("F2");
                                    // objCCMF.Version_Number = (Convert.ToDecimal(objCCMF.Version_Number) + Convert.ToDecimal(".01")).ToString();
                                }
                            }
                            dbContext.SaveChanges();

                            objCCMF = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
                            objTB_APPLICATION_FUNDING_STATUS.ForEach(x => x.Application_ID = objCCMF.CCMF_ID);
                            objTB_APPLICATION_FUNDING_STATUS.ForEach(x => x.Programme_ID = objCCMF.Programme_ID);
                            IncubationContext.APPLICATION_FUNDING_STATUS_ADDUPDATE(dbContext, objTB_APPLICATION_FUNDING_STATUS, objCCMF.CCMF_ID);
                            objTB_APPLICATION_CONTACT_DETAIL.ForEach(x => x.Application_ID = objCCMF.CCMF_ID);
                            objTB_APPLICATION_CONTACT_DETAIL.ForEach(x => x.Programme_ID = objCCMF.Programme_ID);
                            IncubationContext.TB_APPLICATION_CONTACTDETAILSADDUPDATE(dbContext, objTB_APPLICATION_CONTACT_DETAIL, objCCMF.CCMF_ID);
                            objTB_APPLICATION_COMPANY_CORE_MEMBER.ForEach(x => x.Application_ID = objCCMF.CCMF_ID);
                            objTB_APPLICATION_COMPANY_CORE_MEMBER.ForEach(x => x.Programme_ID = progId);


                            IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_ADDUPDATE(dbContext, objTB_APPLICATION_COMPANY_CORE_MEMBER, objCCMF.CCMF_ID);
                            if (txtVideoClip.Text.Length > 0 && !Common.CBPRegularExpression.RegExValidate(CBPRegularExpression.Url, txtVideoClip.Text.Trim()))
                                ErrorLIst.Add("Invalid Video clip url.");
                            else if (txtVideoClip.Text.Length > 0)
                                SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, objCCMF.CCMF_ID, progId);
                            dbContext.SaveChanges();
                            InitializeFundingStatus();
                            InitialCoreMembers();
                            FillContact();
                            ShowbottomMessage("Saved Successfully", true);
                        }
                        else
                        {
                            lblgrouperror.Visible = true;
                            lblgrouperror.DataSource = ErrorLIst;


                            lblgrouperror.DataBind();
                        }
                        string eventname = dbContext.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "App_Event_Logging_Name").Value;
                        string eventvalue = dbContext.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "App_Event_logging_Enable").Value;
                        //EventInstance eventInstance = new EventInstance(0, 0) { EntryType = EventLogEntryType.Information };


                        //string[] eventLog = EventLogger.BuildEventLog(objCCMF);
                        if (!EventLog.SourceExists(eventvalue))
                            EventLog.CreateEventSource(eventvalue, "test");
                        if (eventvalue == "1")
                        {
                            //EventLog log = new EventLog { Source = eventname };
                            //log.WriteEvent(eventInstance, eventLog);
                            EventLog.WriteEvent(eventname, new EventInstance(0, 0, EventLogEntryType.Information), objCCMF);
                            // EventLog.WriteEvent(eventname, , EventLogEntryType.Information);
                        }
                    }

                    catch (Exception e)
                    { }

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
        protected void SetPanel1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SetPanelVisibilityOfStep(0);
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
        private void Fill_Programelist(string Application_number, int Programme_Id, int Intake, string version, string Businessarea, string status, string Applicant, string ProgrammeName)
        {
            //bool originalCatchValue = SPSecurity.CatchAccessDeniedException;
            //SPSecurity.CatchAccessDeniedException = false;
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
                        if (!ProgrammeName.ToLower().Contains("cupp") && !ProgrammeName.ToLower().Contains("hongkong"))
                        {
                            itemAttachment["Programme_Name"] = "CCMF – Cross Border";
                        }
                        else if (!ProgrammeName.ToLower().Contains("crossborder") && !ProgrammeName.ToLower().Contains("cupp"))
                        {
                            itemAttachment["Programme_Name"] = "CCMF – Hong Kong";
                        }
                        else
                        {
                            itemAttachment["Programme_Name"] = "CCMF – CUPP";
                        }
                        itemAttachment["Programme_Name"] = "CCMF-" + ProgrammeName;
                        itemAttachment["Intake_Number"] = Intake;
                        itemAttachment["Applicant"] = Applicant;
                        itemAttachment["Version_Number"] = version;
                        itemAttachment["Business_Area"] = Businessarea;
                        itemAttachment["Status"] = "Submitted";
                        //if (!ProgrammeName.ToLower().Contains("university"))
                        //{
                        //    itemAttachment["Programme_Name_Full"] = "Cyberport Creative Micro-" + ProgrammeName;
                        //}
                        //else
                        //{
                        //    itemAttachment["Programme_Name_Full"] = "Cyberport University Partnership Programme";
                        //}

                        itemAttachment.Update();
                        site.AllowUnsafeUpdates = false;
                    }

                });

            }
            catch (Exception e)
            {
                throw;
            }
            //finally
            //{
            //    Context.Response.Redirect("~/SitePages/Home.aspx", false);
            //}

        }

        protected void rdo212d_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                int progId = Convert.ToInt32(hdn_ProgramID.Value);
                TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                string selectedValue = rdo212d.SelectedValue;
                if (!string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                {
                    if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "professional" && objIncubation.Programme_Type.ToLower() == "hongkong")
                    {
                        if (Convert.ToBoolean(selectedValue) == true)
                        {
                            rdo212e.Enabled = true;
                            lbl212etdd.Enabled = true;
                            lbl212e.Enabled = true;
                            rdo212e.SelectedIndex = -1;
                        }
                        else
                        {
                            lbl212etdd.Enabled = false;
                            lbl212e.Enabled = false;
                            rdo212e.Enabled = false;
                            rdo212e.SelectedIndex = -1;

                        }
                    }
                }
            }
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

        protected void rdo212h_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                int progId = Convert.ToInt32(hdn_ProgramID.Value);
                TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                string selectedValue = rdo212h.SelectedValue;

                if (objIncubation.Programme_Type.ToLower() == "crossborder")
                {
                    if (Convert.ToBoolean(selectedValue) == true)
                    {
                        rdo212i.Enabled = true;
                        lbl212itd.Enabled = true;
                        lbl212i.Enabled = true;
                        rdo212i.SelectedIndex = -1;
                    }
                    else
                    {
                        lbl212itd.Enabled = false;
                        lbl212i.Enabled = false;
                        rdo212i.Enabled = false;
                        rdo212i.SelectedIndex = -1;

                    }
                }
            }
        }


        protected void rdo212f_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                int progId = Convert.ToInt32(hdn_ProgramID.Value);
                TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);
                string selectedValue = rdo212f.SelectedValue;

                if (objIncubation.Programme_Type.ToLower() == "cupp")
                {
                    if (Convert.ToBoolean(selectedValue) == true)
                    {
                        rdo212g.Enabled = true;
                        lbl212gtd.Enabled = true;
                        lbl212g.Enabled = true;
                        rdo212g.SelectedIndex = -1;
                    }
                    else
                    {
                        lbl212gtd.Enabled = false;
                        lbl212g.Enabled = false;
                        rdo212g.Enabled = false;
                        rdo212g.SelectedIndex = -1;

                    }
                }

                if (!string.IsNullOrEmpty(objIncubation.Hong_Kong_Programme_Stream))
                {
                    if (objIncubation.Hong_Kong_Programme_Stream.ToLower() == "young entrepreneur" && objIncubation.Programme_Type.ToLower() == "hongkong")
                    {
                        if (Convert.ToBoolean(selectedValue) == true)
                        {
                            rdo212g.Enabled = true;
                            lbl212gtd.Enabled = true;
                            lbl212g.Enabled = true;
                            rdo212g.SelectedIndex = -1;
                        }
                        else
                        {
                            lbl212gtd.Enabled = false;
                            lbl212g.Enabled = false;
                            rdo212g.Enabled = false;
                            rdo212g.SelectedIndex = -1;

                        }
                    }
                }
            }
        }

        protected void rdo_CUPP_CheckedChanged(object sender, EventArgs e)
        {
            rdo_HK_Option.Enabled = false;
            rdo_HK_Option.SelectedIndex = -1;
        }

        protected void rdo211a_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdo211b_SetUpDisable();
        }

        protected void rdo211b_SetUpDisable()
        {
            string selectedValue = rdo211a.SelectedValue;
            bool isTrue = false;
            if (Boolean.TryParse(selectedValue, out isTrue))
            {
                if (rdo_CCMFApplication.SelectedValue == "Company")
                {
                    if (Convert.ToBoolean(selectedValue) == false)
                    {
                        lbl211b.Enabled = true;
                        //div211b.Enabled = true;
                        rdo211b.Enabled = true;
                        //rdo211b.SelectedIndex = -1;
                    }
                    else
                    {
                        lbl211b.Enabled = false;
                        //div211b.Enabled = false;
                        rdo211b.Enabled = false;
                        rdo211b.SelectedIndex = -1;
                    }
                }
                else
                {
                    //if (Convert.ToBoolean(selectedValue) == false)
                    //{
                    //    lbl211b.Enabled = false;
                    //    //div211b.Enabled = false;
                    //    rdo211b.Enabled = false;
                    //    rdo211b.SelectedIndex = -1;
                    //}
                    //else
                    //{
                    // Always enable
                    lbl211b.Enabled = true;
                    //div211b.Enabled = true;
                    rdo211b.Enabled = true;
                    //rdo211b.SelectedIndex = -1;
                    //}
                }
            }
            else
            {
                // Always enable
                lbl211b.Enabled = true;
                //div211b.Enabled = true;
                rdo211b.Enabled = true;
            }
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
                            //Label lblContactSubTitle = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactSubTitle");
                            RadioButtonList Area = (RadioButtonList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("rdo_Area");
                            TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                            TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                            TextBox txtlastchiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtlast_chiname");
                            TextBox txtFisrtChiname = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtFisrt_Chiname");
                            //TextBox identitycard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("identity_card");
                            TextBox txtContactNo = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNo");
                            TextBox Fax = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFax");
                            TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                            TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                            TextBox Nameofinsititute = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Nameofinsititute");
                            TextBox Studentidcard = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Studentidcard");
                            TextBox Programmerolled = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Programmerolled");
                            TextBox Dateofgraduation = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Dateofgraduation");
                            TextBox OrganisationName = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("OrganisationName");
                            TextBox Position = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Position");
                            DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");


                            //if (i > 0)
                            //    lblContactSubTitle.Visible = false;
                            TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                            objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                            objMember.Last_Name_Eng = txtContactLast_name.Text;
                            objMember.First_Name_Eng = txtContactFirst_name.Text;
                            objMember.Last_Name_Chi = txtlastchiname.Text;
                            objMember.First_Name_Chi = txtFisrtChiname.Text;
                            //objMember.ID_Number = identitycard.Text;
                            objMember.Area = Area.SelectedValue;

                            objMember.Contact_No = txtContactNo.Text;

                            objMember.Fax = Fax.Text;

                            objMember.Email = Email.Text;
                            objMember.Mailing_Address = Mailing_Address.Text;
                            objMember.Education_Institution_Eng = Nameofinsititute.Text;
                            objMember.Student_ID_Number = Studentidcard.Text;
                            objMember.Programme_Enrolled_Eng = Programmerolled.Text;
                            try
                            {
                                if (Dateofgraduation.Text != "")
                                {
                                    var date = Dateofgraduation.Text.Split(new char[] { '-', ' ' });

                                    objMember.Graduation_Month = DateTime.ParseExact(date[0], "MMM", CultureInfo.CurrentCulture).Month;
                                    //objMember.Graduation_Month = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(date[0]) + 1;
                                    objMember.Graduation_Year = Convert.ToInt32(date[1]);
                                }
                            }
                            catch (Exception)
                            {
                                objMember.Graduation_Month = null;
                                objMember.Graduation_Year = null;
                            }

                            objMember.Organisation_Name = OrganisationName.Text;
                            objMember.Position = Position.Text;
                            objMember.Salutation = Salutation.Text;

                            objMember.Application_ID = Guid.Parse(hdn_ApplicationID.Value);
                            objContactDetails.Add(objMember);





                        }

                    }
                    catch (Exception ex)
                    {
                        IsError = true;

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
                            TextBox txtBootcampno = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Bootcampno");
                            TextBox txtBackgroundinfo = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Backgroundinfo");
                            TextBox txtEmail = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtEmail");
                            DropDownList ddlContactNationality = (DropDownList)grvCoreMember.Rows[i].Cells[0].FindControl("ddlContactNationality");

                            TextBox txtHKID = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("HKID");

                            TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                            objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                            objMember.Name = TextBoxAddress.Text;
                            objMember.Position = txtNOE.Text;
                            objMember.Bootcamp_Eligible_Number = txtBootcampno.Text;
                            objMember.Background_Information = txtBackgroundinfo.Text;
                            objMember.Email = txtEmail.Text;
                            objMember.Nationality = ddlContactNationality.SelectedValue;
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
                    TB_CCMF_APPLICATION objIncubation = GetExistingCCMF(dbContext, progId);//dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

                    SPFunctions objSp = new SPFunctions();
                    string AccessUser = lblApplicant.Text.Trim();
                    string CurrentUser = objSp.GetCurrentUser();
                    if (objIncubation.Applicant.ToLower() != objSp.GetCurrentUser().ToLower() && objIncubation.Modified_By.ToLower() != objSp.GetCurrentUser().ToLower())
                    {

                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        lnkAttachmentDelete.Enabled = false;
                        if (e.Item.FindControl("hypNavigation") != null)
                        {
                            HyperLink hypNavigation = (HyperLink)e.Item.FindControl("hypNavigation");
                            hypNavigation.Enabled = false;
                        }

                    }
                }
            }
        }

        protected void rdo1_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo1_3.SelectedItem.Value == "True")
                div1_3Remark.Visible = true;
            else
                div1_3Remark.Visible = false;
        }

        protected void rdo211c_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (rdo_HK.Checked && !Convert.ToBoolean(rdo211c.SelectedValue) && rdo_CCMFApplication.SelectedValue == "Company")
            {
                lbl211cYepComp.Visible = true;
            }
            else if (rdo_HK.Checked && !Convert.ToBoolean(rdo211c.SelectedValue))
            {
                lbl211cYepInd.Visible = true;
            }
            else
            {
                lbl211cYepComp.Visible = false;
                lbl211cYepInd.Visible = false;



            }
        }

        protected void grvCoreMember_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

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

        //protected void rdo_CCMFApplication_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rdo_HK.Checked)
        //    {
        //        // Professional Stream
        //        if (rdo_HK_Option.SelectedValue == "Professional")
        //        {
        //            lblstep12Note.Visible = true;
        //            lblstep12Note.Text =SPFunctions.LocalizeUI("HKP_Pro_Stream_IndComp_Step_1_2_note", "CyberportEMS_CCMF");

        //        }
        //        // Hong Kong Young Entrepreneur Programme
        //        else
        //        {
        //            if (rdo_CCMFApplication.SelectedValue == "Company")
        //            {
        //                lblstep12Note.Visible = true;
        //                lblstep12Note.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Comp_Step_1_2_note", "CyberportEMS_CCMF");
        //            }

        //            else
        //            {
        //                lblstep12Note.Visible = true;
        //                lblstep12Note.Text = SPFunctions.LocalizeUI("HKP_Pro_Young_Ind_Step_1_2_note", "CyberportEMS_CCMF");
        //            }

        //        }

        //    }

        //}
    }


}

