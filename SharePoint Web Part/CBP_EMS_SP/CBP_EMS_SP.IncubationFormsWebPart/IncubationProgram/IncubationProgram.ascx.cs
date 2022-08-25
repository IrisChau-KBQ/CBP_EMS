using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Data.Models;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.IncubationFormsWebPart.IncubationProgram
{
    [ToolboxItemAttribute(false)]
    public partial class IncubationProgram : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public IncubationProgram()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillIntakeDetails();
            }
        }

        public static SPListItem GetItemByBdcId(SPList list, string CCMF_ID)
        {
            SPListItem myitem = null;
            foreach (SPListItem item in list.Items)
            {

                if (item["CCMF_ID"].ToString() == CCMF_ID)
                {
                    myitem = item;
                    break;
                }
            }
            return myitem;
        }




        protected void FillIntakeDetails()
        {
            try
            {
                int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
                Guid objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);

                using (var Intake = new CyberportEMS_EDM())
                {
                    TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                    if (objProgram == null)
                    {
                        Context.Response.Redirect("~/SitePages/Home.aspx");
                    }
                    else
                    {
                        lblIntake.Text = objProgram.Intake_Number.ToString();
                        lblDeadline.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
                        lblApplicant.Text = GetCurrentUser();
                        lblApplicationNo.Text = objProgram.Application_No_Prefix + "-";
                        hdn_ApplicationID.Value = objUserProgramId.ToString();
                        hdn_ProgramID.Value = objProgram.Programme_ID.ToString();

                    }
                }

                //SPSecurity.RunWithElevatedPrivileges(delegate ()
                //{
                SPSite site = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPServiceContext context = SPServiceContext.GetContext(site);
                SPServiceContextScope contextScope = new SPServiceContextScope(context);

                SPWeb web = site.OpenWeb();
                SPList list = web.Lists["Incubation"];
                SPListItem item = GetItemByBdcId(list, objUserProgramId.ToString());
                if (item != null)
                {
                    rbtnPanel1Q1.SelectedValue = Convert.ToString(item["Question1_1"]);
                    rbtnPanel1Q2.SelectedValue = Convert.ToString(item["Question1_2"]);
                    rbtnPanel1Q3.SelectedValue = Convert.ToString(item["Question1_3"]);
                    rbtnPanel1Q4.SelectedValue = Convert.ToString(item["Question1_4"]);
                    rbtnPanel1Q5.SelectedValue = Convert.ToString(item["Question1_5"]);
                    rbtnPanel1Q6.SelectedValue = Convert.ToString(item["Question1_6"]);
                    rbtnPanel1Q7.SelectedValue = Convert.ToString(item["Question1_7"]);
                    rbtnPanel1Q8.SelectedValue = Convert.ToString(item["Question1_8"]);
                    rbtnPanel1Q9.SelectedValue = Convert.ToString(item["Question1_9"]);
                    rbtnPanel1Q10.SelectedValue = Convert.ToString(item["Question1_10"]);
                }
                //});

                //using (var Intake = new CyberportEMS_EDM())
                //{
                //    TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
                //    if (objProgram == null)
                //    {
                //        Context.Response.Redirect("~/SitePages/Home.aspx");
                //    }
                //    else
                //    {
                //        lblIntake.Text = objProgram.Intake_Number.ToString();
                //        lblDeadline.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
                //        lblApplicant.Text = GetCurrentUser();
                //        lblApplicationNo.Text = objProgram.Application_No_Prefix + "-";
                //        hdn_ApplicationID.Value = objUserProgramId.ToString();
                //        hdn_ProgramID.Value = objProgram.Programme_ID.ToString();

                //    }

                //    TB_INCUBATION_APPLICATION objIncubation = Intake.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == objUserProgramId);
                //    if (objIncubation != null)
                //    {
                //        lblLastSubmitted.Text = objIncubation.Last_Submitted.ToString("dd MMM yyyy");

                //        rbtnPanel1Q1.SelectedValue = Convert.ToString(objIncubation.Question1_1);
                //        rbtnPanel1Q2.SelectedValue = Convert.ToString(objIncubation.Question1_2);
                //        rbtnPanel1Q3.SelectedValue = Convert.ToString(objIncubation.Question1_3);
                //        rbtnPanel1Q4.SelectedValue = Convert.ToString(objIncubation.Question1_4);
                //        rbtnPanel1Q5.SelectedValue = Convert.ToString(objIncubation.Question1_5);
                //        rbtnPanel1Q6.SelectedValue = Convert.ToString(objIncubation.Question1_6);
                //        rbtnPanel1Q7.SelectedValue = Convert.ToString(objIncubation.Question1_7);
                //        rbtnPanel1Q8.SelectedValue = Convert.ToString(objIncubation.Question1_8);
                //        rbtnPanel1Q9.SelectedValue = Convert.ToString(objIncubation.Question1_9);
                //        rbtnPanel1Q10.SelectedValue = Convert.ToString(objIncubation.Question1_10);

                //        txtCompanyNameEnglish.Text = objIncubation.Company_Name_Eng;
                //        txtCompanyNameChinese.Text = objIncubation.Company_Name_Chi;
                //        txtAbstractEnglish.Text = objIncubation.Abstract;
                //        txtObjective.Text = objIncubation.Objective;
                //        txtbackground.Text = objIncubation.Background;
                //        txtPilot_Work_Done.Text = objIncubation.Pilot_Work_Done;

                //        txtAdditionalInformation.Text = objIncubation.Additional_Information;
                //        txtProposedProducts.Text = objIncubation.Proposed_Products;
                //        txtTargetMarket.Text = objIncubation.Target_Market;
                //        txtCompetitionAnalysis.Text = objIncubation.Competition_Analysis;
                //        txtRevenueModel.Text = objIncubation.Revenus_Model;
                //        txtFirst6Months.Text = objIncubation.First_6_Months_Milestone;
                //        txtSecond6Months.Text = objIncubation.Second_6_Months_Milestone;
                //        txtThird6Months.Text = objIncubation.Third_6_Months_Milestone;
                //        txtForth6Months.Text = objIncubation.Forth_6_Months_Milestone;
                //        txtExitStrategy.Text = objIncubation.Exit_Strategy;
                //        if (objIncubation.Resubmission != null)
                //            rbtnResubmission.SelectedValue = Convert.ToString(objIncubation.Resubmission);
                //        txtResubmission_Project_Reference.Text = objIncubation.Resubmission_Project_Reference;
                //        txtResubmission_Main_Differences.Text = objIncubation.Resubmission_Main_Differences;
                //        rbtnCompany_Type.SelectedValue = objIncubation.Company_Type;
                //        if (objIncubation.Company_Type.ToLower() == "others")
                //        {
                //            txtOther_Company_Type.Text = objIncubation.Other_Company_Type;
                //            txtOther_Company_Type.Visible = true;
                //        }

                //        rbtnList_Business_Area.SelectedValue = objIncubation.Business_Area;
                //        if (objIncubation.Business_Area.ToLower() == "others")
                //        {
                //            txtOther_Bussiness_Area.Text = objIncubation.Other_Bussiness_Area;
                //            txtOther_Bussiness_Area.Visible = true;
                //        }
                //        string[] Positioning = objIncubation.Positioning.Split(';');
                //        txtPositioningOther.Text = objIncubation.Other_Positioning;
                //        foreach (string strPos in Positioning.Where(x => !string.IsNullOrEmpty(x)))
                //        {
                //            chkPositioning.SelectedValue = strPos;
                //        }
                //        txtOtherAttributes.Text = objIncubation.Other_Attributes;
                //        rbtnPreferred_Track.SelectedValue = objIncubation.Preferred_Track;
                //        txtCompany_Ownership_1.Text = objIncubation.Company_Ownership_Structure;
                //        txtPartner_Profiles.Text = objIncubation.Major_Partners_Profiles;
                //        Manpower_Distribution.Text = objIncubation.Manpower_Distribution;
                //        Equipment_Distribution.Text = objIncubation.Equipment_Distribution;
                //        Other_Costs.Text = objIncubation.Other_Direct_Costs;
                //        Forecast_Income.Text = objIncubation.Forecast_Income;
                //    }
                //}
            }
            catch (Exception ex)
            {
                lbl_Exception.InnerText = ex.InnerException.ToString();
            }
        }
        protected string GetCurrentUser()
        {
            string UserName = string.Empty;
            using (SPSite oSPsite = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url))
            {

                using (SPWeb oSPweb = oSPsite.OpenWeb())
                {
                    SPUser CurrentUser = oSPweb.CurrentUser;
                    UserName = CurrentUser.Name;
                }
            }
            return UserName;
        }
        protected void btnIncubationForm_Click(object sender, EventArgs e)
        {
            SetPanelVisibilityOfStep(1);
        }
        protected void btn_StepPrevious_Click(object sender, EventArgs e)
        {
            lbl_Exception.InnerHtml = "";
            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) - 1);
        }
        protected void btn_StepNext_Click(object sender, EventArgs e)
        {
            lbl_Exception.InnerHtml = "";
            if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2)
            {
                SaveStep1Data();
                InitializeFundingStatus();
            }
            else
            if ((Convert.ToInt16(hdn_ActiveStep.Value) + 1) == 3)
            {
                InitialCoreMembers();
            }

            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) + 1);

        }
        protected void btn_StepSave_Click(object sender, EventArgs e)
        {
            lbl_Exception.InnerHtml = "";
            int ActiveStep = Convert.ToInt16(hdn_ActiveStep.Value);
            if (ActiveStep == 1)
            {
                SaveStep1Data();
            }
            else if (ActiveStep == 2)
            {
                SaveStep2Data();
            }
            else if (ActiveStep == 3)
            {
                SaveStep3Data();
            }
            else if (ActiveStep == 5)
            {
                SaveStep5Data();
            }
        }
        protected void SetPanelVisibilityOfStep(int ActiveStep)
        {
            hdn_ActiveStep.Value = ActiveStep.ToString();
            pnl_programDetail.Visible = true;
            pnl_Buttons.Visible = true;
            progressList.Visible = true;
            btn_StepNext.Visible = true;
            btn_Submit.Visible = false;
            if (ActiveStep > 0)
            {

                foreach (ListItem objList in progressList.Items)
                {
                    if (Convert.ToInt32(objList.Value) <= ActiveStep)
                    {
                        objList.Attributes.Add("class", "active");
                    }
                    else
                        objList.Attributes.Remove("class");

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
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(Guid.Parse(hdn_ApplicationID.Value));

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
                    TextBox txtAmountR = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Academic");
                    TextBox txtAmountMax = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Experience");
                    TextBox txtAchievements = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Achievements");

                    TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                    objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                    objMember.Name = TextBoxAddress.Text;
                    objMember.Position = txtNOE.Text;
                    objMember.Professional_Qualifications = txtAmountR.Text;
                    objMember.Working_Experiences = txtAmountMax.Text;
                    objMember.Special_Achievements = txtAchievements.Text;
                    objCoreMembers.Add(objMember);
                }
                catch (Exception)
                {

                    IsError = true;
                    lblCorememberError.Text = "Fill all the record in Member " + (i + 1).ToString();
                    break;


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

        private List<TB_APPLICATION_COMPANY_CORE_MEMBER> GetCoreMemberForSave(out bool IsError)
        {
            IsError = false;
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();



            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                try
                {

                    HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
                    TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
                    TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
                    TextBox txtAmountR = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Academic");
                    TextBox txtAmountMax = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Experience");
                    TextBox txtAchievements = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Achievements");
                    if (TextBoxAddress.Text != "" && txtNOE.Text != "" && txtAmountR.Text != "" && txtAmountMax.Text != "" && txtAchievements.Text != "")
                    {
                        TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                        objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                        objMember.Name = TextBoxAddress.Text;
                        objMember.Position = txtNOE.Text;
                        objMember.Professional_Qualifications = txtAmountR.Text;
                        objMember.Working_Experiences = txtAmountMax.Text;
                        objMember.Special_Achievements = txtAchievements.Text;
                        objCoreMembers.Add(objMember);
                    }
                }
                catch (Exception)
                {
                    IsError = true;
                    lbl_Exception.InnerHtml = "Fill the all values in " + (i + 1).ToString() + " Core members ";
                    break;

                }
            }

            return objCoreMembers;

            //  SetPreviousData();
        }

        private void InitializeFundingStatus()
        {
            List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(Guid.Parse(hdn_ApplicationID.Value));
            if (ObjFundingStatus.Count == 0)
            {
                ObjFundingStatus = new List<TB_APPLICATION_FUNDING_STATUS>();
                ObjFundingStatus.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = DateTime.Now });

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

                    TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                    objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                    objMember.Programme_Name = txtNameofProgram.Text;
                    objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
                    objMember.Funding_Status = txtFundingStatus.Text;
                    objMember.Application_Status = txtApplicationStatus.Text;
                    objMember.Expenditure_Nature = txtNature.Text;
                    objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);
                    objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);
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
                objCoreMembers.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = DateTime.Now });


                Grd_FundingStatus.DataSource = objCoreMembers;
                Grd_FundingStatus.DataBind();
            }

            //  SetPreviousData();
        }

        private List<TB_APPLICATION_FUNDING_STATUS> GetFundingStatusForSave(out bool IsError)
        {
            IsError = false;
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
                    if (txtNameofProgram.Text != "" && txtApplicationStatus.Text != "" && txtFundingStatus.Text != "" && txtNature.Text != "" && txtAmountReceived.Text != "" && txtApplicationMaximumAmount.Text != "")
                    {
                        TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                        objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                        objMember.Programme_Name = txtNameofProgram.Text;
                        objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
                        objMember.Funding_Status = txtFundingStatus.Text;
                        objMember.Application_Status = txtApplicationStatus.Text;
                        objMember.Expenditure_Nature = txtNature.Text;
                        objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);
                        objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);
                        objMember.Application_ID = Guid.Parse(hdn_ApplicationID.Value);
                        objCoreMembers.Add(objMember);
                    }
                }
                catch (Exception)
                {
                    IsError = true;
                    lbl_Exception.InnerHtml = "Fill the all values in " + (i + 1).ToString() + " Funding status";
                    break;

                }
            }

            return objCoreMembers;

            //  SetPreviousData();
        }

        protected void GetSPProgramList()
        {
            SPList objUserProgram = null;
            using (SPSite oSPsite = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url))
            {

                using (SPWeb oSPweb = oSPsite.OpenWeb())
                {
                    objUserProgram = oSPweb.Lists["Incubation"];

                }
            }
        }

        protected void SaveStep1Data()
        {
            string ErrorMessage = string.Empty;

            bool IsError = false;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    Guid appId = Guid.Parse(hdn_ApplicationID.Value);
                    TB_INCUBATION_APPLICATION objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == appId);
                    if (objIncubation == null)
                    {
                        TB_INCUBATION_APPLICATION objNewApplication = new TB_INCUBATION_APPLICATION();
                        objNewApplication.CCMF_ID = Guid.Parse(hdn_ApplicationID.Value);
                        objNewApplication.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
                        objNewApplication.Application_Number = Convert.ToString(lblApplicationNo.Text);
                        objNewApplication.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                        objNewApplication.Applicant = GetCurrentUser();
                        objNewApplication.Last_Submitted = DateTime.Now;
                        objNewApplication.Status = "Saved";
                        objNewApplication.Version_Number = "0.1";
                        if (rbtnPanel1Q1.SelectedValue != "")
                            objNewApplication.Question1_1 = Convert.ToBoolean(rbtnPanel1Q1.SelectedValue);
                        if (rbtnPanel1Q2.SelectedValue != "")
                            objNewApplication.Question1_2 = Convert.ToBoolean(rbtnPanel1Q2.SelectedValue);
                        if (rbtnPanel1Q3.SelectedValue != "")
                            objNewApplication.Question1_3 = Convert.ToBoolean(rbtnPanel1Q3.SelectedValue);
                        if (rbtnPanel1Q4.SelectedValue != "")
                        {
                            objNewApplication.Question1_4 = Convert.ToBoolean(rbtnPanel1Q4.SelectedValue);
                            if (objNewApplication.Question1_4 == false && rbtnPanel1Q5.SelectedValue == "")
                            {
                                rbtnPanel1Q5.Enabled = true;

                                ErrorMessage += "Please select Question 1.5<br>";
                                IsError = true;
                            }
                            else if (rbtnPanel1Q5.SelectedValue != "")
                                objNewApplication.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
                        }
                        if (rbtnPanel1Q6.SelectedValue != "")
                            objNewApplication.Question1_6 = Convert.ToBoolean(rbtnPanel1Q6.SelectedValue);
                        if (rbtnPanel1Q7.SelectedValue != "")
                            objNewApplication.Question1_7 = Convert.ToBoolean(rbtnPanel1Q7.SelectedValue);
                        if (rbtnPanel1Q8.SelectedValue != "")
                        {
                            objNewApplication.Question1_8 = Convert.ToBoolean(rbtnPanel1Q8.SelectedValue);
                            if (objNewApplication.Question1_8 == true && rbtnPanel1Q9.SelectedValue == "")
                            {
                                rbtnPanel1Q9.Enabled = true;

                                ErrorMessage += "Please select Question 1.9<br>";
                                IsError = true;
                            }
                            else if (rbtnPanel1Q9.SelectedValue == "")
                                objNewApplication.Question1_9 = Convert.ToBoolean(rbtnPanel1Q9.SelectedValue);
                        }
                        if (rbtnPanel1Q10.SelectedValue != "")
                            objNewApplication.Question1_10 = Convert.ToBoolean(rbtnPanel1Q10.SelectedValue);


                        objNewApplication.Created_By = GetCurrentUser();
                        objNewApplication.Created_Date = DateTime.Now;
                        objNewApplication.Modified_By = GetCurrentUser();
                        objNewApplication.Modified_Date = DateTime.Now;
                        if (IsError == false)
                        {
                            dbContext.TB_INCUBATION_APPLICATION.Add(objNewApplication);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            lbl_Exception.InnerHtml = ErrorMessage;
                        }
                    }
                    else
                    {
                        objIncubation.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
                        objIncubation.Applicant = GetCurrentUser();
                        objIncubation.Last_Submitted = DateTime.Now;
                        objIncubation.Status = "Saved";
                        objIncubation.Version_Number = "0.1";
                        if (rbtnPanel1Q1.SelectedValue != "")
                            objIncubation.Question1_1 = Convert.ToBoolean(rbtnPanel1Q1.SelectedValue);
                        if (rbtnPanel1Q2.SelectedValue != "")
                            objIncubation.Question1_2 = Convert.ToBoolean(rbtnPanel1Q2.SelectedValue);
                        if (rbtnPanel1Q3.SelectedValue != "")
                            objIncubation.Question1_3 = Convert.ToBoolean(rbtnPanel1Q3.SelectedValue);
                        if (rbtnPanel1Q4.SelectedValue != "")
                        {
                            objIncubation.Question1_4 = Convert.ToBoolean(rbtnPanel1Q4.SelectedValue);
                            if (objIncubation.Question1_4 == false && rbtnPanel1Q5.SelectedValue == "")
                            {
                                rbtnPanel1Q5.Enabled = true;
                                ErrorMessage += "Please select Question 1.5<br>";
                                IsError = true;
                            }
                            else if (rbtnPanel1Q5.SelectedValue == "")
                                objIncubation.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
                        }
                        if (rbtnPanel1Q5.SelectedValue != "")
                            objIncubation.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
                        if (rbtnPanel1Q6.SelectedValue != "")
                            objIncubation.Question1_6 = Convert.ToBoolean(rbtnPanel1Q6.SelectedValue);
                        if (rbtnPanel1Q7.SelectedValue != "")
                            objIncubation.Question1_7 = Convert.ToBoolean(rbtnPanel1Q7.SelectedValue);
                        if (rbtnPanel1Q8.SelectedValue != "")
                        {
                            objIncubation.Question1_8 = Convert.ToBoolean(rbtnPanel1Q8.SelectedValue);
                            if (objIncubation.Question1_8 == true && rbtnPanel1Q9.SelectedValue == "")
                            {
                                rbtnPanel1Q9.Enabled = true;

                                ErrorMessage += "Please select Question 1.9<br>";
                                IsError = true;
                            }
                            else if (rbtnPanel1Q9.SelectedValue == "")
                                objIncubation.Question1_9 = Convert.ToBoolean(rbtnPanel1Q9.SelectedValue);
                        }
                        if (rbtnPanel1Q10.SelectedValue != "")
                            objIncubation.Question1_10 = Convert.ToBoolean(rbtnPanel1Q10.SelectedValue);
                        if (IsError == false)
                        {
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            lbl_Exception.InnerHtml = ErrorMessage;
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                lbl_Exception.InnerHtml = ex.Message;
            }
        }

        protected void SaveStep2Data()
        {
            string ErrorMessage = string.Empty;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    Guid appId = Guid.Parse(hdn_ApplicationID.Value);
                    TB_INCUBATION_APPLICATION objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == appId);
                    if (objIncubation != null)
                    {
                        bool IsError = false;

                        List<TB_APPLICATION_FUNDING_STATUS> objFunds = GetFundingStatusForSave(out IsError);
                        if ((objIncubation.Question1_7 == true || objIncubation.Question1_8 == true) && objFunds.Count == 0)
                        {
                            IsError = true;
                            ErrorMessage = "At least one Funding Status required.";
                        }


                        if (IsError == false)
                        {
                            objFunds.ForEach(x => x.Application_ID = Guid.Parse(hdn_ApplicationID.Value));
                            objFunds.ForEach(x => x.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value));
                            IncubationContext.APPLICATION_FUNDING_STATUS_ADDUPDATE(dbContext, objFunds);
                            objIncubation.Last_Submitted = DateTime.Now;

                            objIncubation.Company_Name_Eng = txtCompanyNameEnglish.Text.Trim();
                            objIncubation.Company_Name_Chi = txtCompanyNameChinese.Text.Trim();

                            objIncubation.Abstract = txtAbstractEnglish.Text.Trim();

                            objIncubation.Objective = txtObjective.Text.Trim();
                            objIncubation.Background = txtbackground.Text.Trim();
                            objIncubation.Pilot_Work_Done = txtPilot_Work_Done.Text.Trim();
                            if ((objIncubation.Question1_8 == true || objIncubation.Question1_9 == true) && string.IsNullOrEmpty(txtAdditionalInformation.Text.Trim()))
                            {
                                IsError = true;
                                ErrorMessage = "2.3.2.4 Additional Information can not be empty.";
                            }
                            else
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
                                    if (string.IsNullOrEmpty(txtResubmission_Project_Reference.Text.Trim()) || string.IsNullOrEmpty(txtResubmission_Main_Differences.Text.Trim()))
                                    {
                                        IsError = true;
                                        ErrorMessage = "Section 2.4.7 and 2.4.8 can not be empty";
                                        txtResubmission_Project_Reference.Enabled = true;
                                        txtResubmission_Main_Differences.Enabled = true;
                                    }
                                    else
                                    {
                                        objIncubation.Resubmission_Project_Reference = txtResubmission_Project_Reference.Text.Trim();
                                        objIncubation.Resubmission_Main_Differences = txtResubmission_Main_Differences.Text.Trim();
                                    }
                                }
                            }



                            objIncubation.Company_Type = rbtnCompany_Type.SelectedValue;
                            if (rbtnCompany_Type.SelectedValue.ToLower() == "others")
                            {
                                objIncubation.Other_Company_Type = txtOther_Company_Type.Text.Trim();
                            }


                            objIncubation.Business_Area = rbtnList_Business_Area.SelectedValue;
                            if (rbtnList_Business_Area.SelectedValue.ToLower() == "others")
                            {
                                objIncubation.Other_Bussiness_Area = txtOther_Bussiness_Area.Text.Trim();
                            }

                            string Positioning = string.Empty;
                            string PositioningOther = string.Empty;
                            foreach (ListItem objList in chkPositioning.Items)
                            {
                                if (objList.Selected)
                                {
                                    Positioning += objList.Value + ";";
                                    if (objList.Value.ToLower() == "other")
                                    {
                                        PositioningOther = txtPositioningOther.Text.Trim();
                                    }
                                }
                            }
                            objIncubation.Positioning = Positioning;
                            objIncubation.Other_Positioning = PositioningOther;
                            objIncubation.Other_Attributes = txtOtherAttributes.Text.Trim();
                            objIncubation.Preferred_Track = rbtnPreferred_Track.SelectedValue;
                            if (IsError == false)
                                dbContext.SaveChanges();
                            else
                            {
                                lbl_Exception.InnerHtml = ErrorMessage;
                            }
                        }
                    }
                    else
                    {
                        lbl_Exception.InnerHtml = "Could not found the relevent data.";
                    }
                }

            }

            catch (Exception ex)
            {
                lbl_Exception.InnerHtml = ex.Message;
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
                    Guid appId = Guid.Parse(hdn_ApplicationID.Value);
                    TB_INCUBATION_APPLICATION objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == appId);
                    if (objIncubation != null)
                    {
                        bool IsError = false;
                        string FileName = string.Empty;
                        if (fu_Company_Ownership_2.HasFile)
                        {
                            if (fu_Company_Ownership_2.PostedFile.ContentLength <= (5 * 1024 * 1024))
                            {
                                string Extension = fu_Company_Ownership_2.FileName.Remove(0, fu_Company_Ownership_2.FileName.LastIndexOf("."));
                                FileName = appId + Extension;
                                fu_Company_Ownership_2.SaveAs(CBPCommonConstants.GetAttachementFolderPhysical(enumAttachmentType.Company_Ownership_Structure) + FileName);
                                TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
                                {
                                    Application_ID = appId,
                                    Attachment_Path = FileName,
                                    Attachment_Type = enumAttachmentType.Company_Ownership_Structure.ToString(),
                                    Created_By = GetCurrentUser(),
                                    Created_Date = DateTime.Now,
                                    Modified_By = GetCurrentUser(),
                                    Modified_Date = DateTime.Now,
                                    Programme_ID = progId
                                };
                                IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                IsError = true;
                                ErrorMessage += "Uploaded file is greater then 5MB";
                            }

                        }

                        List<TB_APPLICATION_COMPANY_CORE_MEMBER> objFunds = GetCoreMemberForSave(out IsError);
                        if (objFunds.Count == 0)
                        {
                            IsError = true;
                            ErrorMessage += "At least one company core members required.<br/>";
                        }
                        else
                        {
                            objIncubation.Last_Submitted = DateTime.Now;

                            objFunds.ForEach(x => x.Application_ID = appId);
                            objFunds.ForEach(x => x.Programme_ID = progId);
                            objFunds.ForEach(x => x.HKID = x.Position);
                            IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_ADDUPDATE(dbContext, objFunds);

                            objIncubation.Company_Ownership_Structure = txtCompany_Ownership_1.Text.Trim();


                            objIncubation.Major_Partners_Profiles = txtPartner_Profiles.Text.Trim();
                            objIncubation.Manpower_Distribution = Manpower_Distribution.Text.Trim();
                            objIncubation.Equipment_Distribution = Equipment_Distribution.Text.Trim();
                            objIncubation.Other_Direct_Costs = Other_Costs.Text.Trim();
                            objIncubation.Forecast_Income = Forecast_Income.Text.Trim();
                        }

                        if (IsError == false)
                            dbContext.SaveChanges();
                        else
                        {
                            lbl_Exception.InnerHtml = ErrorMessage;
                        }
                    }

                    else
                    {
                        lbl_Exception.InnerHtml = "Could not found the relevent data.";
                    }
                }

            }

            catch (Exception ex)
            {
                lbl_Exception.InnerHtml = ex.Message;
            }
        }

        protected void btn_FundingAddNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddNewFundingStatus();

        }

        protected void SaveStep5Data()
        {
            string ErrorMessage = string.Empty;
            try
            {
                int progId = Convert.ToInt32(hdn_ProgramID.Value);
                Guid appId = Guid.Parse(hdn_ApplicationID.Value);

                SaveAttachment(fu_BrCopy, enumAttachmentType.BR_COPY, appId, progId);
                SaveAttachment(fu_AnnualReturn, enumAttachmentType.Company_Annual_Return, appId, progId);
                SaveAttachment(fuPresentationSlide, enumAttachmentType.Presentation_Slide, appId, progId);
                SaveAttachment(fuOtherAttachement, enumAttachmentType.Other_Attachment, appId, progId);

                SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, appId, progId);

                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_INCUBATION_APPLICATION objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == appId);
                    if (objIncubation != null)
                    {
                        objIncubation.Last_Submitted = DateTime.Now;
                        objIncubation.Declaration = chkDeclaration.Checked;
                        objIncubation.Marketing_Information = Marketing_Information.Checked;
                        objIncubation.Principal_Full_Name = txtName_PrincipalApplicant.Text.Trim();
                        objIncubation.Principal_Position_Title = txtPosition_PrincipalApplicant.Text.Trim();
                        dbContext.SaveChanges();

                    }
                    else
                    {
                        lbl_Exception.InnerHtml = "Could not found the relevent data.";
                    }
                }

            }

            catch (Exception ex)
            {
                lbl_Exception.InnerHtml = ex.Message;
            }
        }

        public bool SaveAttachment(FileUpload objUploder, enumAttachmentType objAttachmentType, Guid appId, int progId)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                string FileName = string.Empty;

                if (objUploder.HasFile)
                {
                    if (objUploder.PostedFile.ContentLength <= (5 * 1024 * 1024))
                    {
                        string Extension = objUploder.FileName.Remove(0, objUploder.FileName.LastIndexOf("."));
                        FileName = appId + "_" + objAttachmentType.ToString() + Extension;
                        fu_Company_Ownership_2.SaveAs(CBPCommonConstants.GetAttachementFolderPhysical(objAttachmentType) + FileName);
                        TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
                        {
                            Application_ID = appId,
                            Attachment_Path = FileName,
                            Attachment_Type = objAttachmentType.ToString(),
                            Created_By = GetCurrentUser(),
                            Created_Date = DateTime.Now,
                            Modified_By = GetCurrentUser(),
                            Modified_Date = DateTime.Now,
                            Programme_ID = progId
                        };
                        IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach);
                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                    return true;
            }

        }

        public bool SaveAttachmentUrl(string Url, enumAttachmentType objAttachmentType, Guid appId, int progId)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                string FileName = string.Empty;

                FileName = Url;
                TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
                {
                    Application_ID = appId,
                    Attachment_Path = FileName,
                    Attachment_Type = objAttachmentType.ToString(),
                    Created_By = GetCurrentUser(),
                    Created_Date = DateTime.Now,
                    Modified_By = GetCurrentUser(),
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
    }
}
