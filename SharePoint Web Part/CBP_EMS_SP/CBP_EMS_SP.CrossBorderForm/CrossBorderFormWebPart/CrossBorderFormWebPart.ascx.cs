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
using Microsoft.SharePoint.IdentityModel;

namespace CBP_EMS_SP.CrossBorderForm.CrossBorderFormWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class CrossBorderFormWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CrossBorderFormWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        IncubationSubmitPopup.Visible = false;
        //        FillIntakeDetails();
        //    }
        //}
        //protected void FillIntakeDetails()
        //{
            
        //    try
        //    {
        //        int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
        //        Guid objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);

        //        using (var Intake = new CyberportEMS_EDM())
        //        {
        //            TB_PROGRAMME_INTAKE objProgram = Intake.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId);
        //            if (objProgram == null)
        //            {
        //                Context.Response.Redirect("~/SitePages/Home.aspx");
        //            }
        //            else
        //            {
        //                if (objProgram.Programme_Name.ToLower().Contains("cross border"))
        //                {
        //                    rdo_Crossborder.Checked = true;

        //                }
        //                else
        //                {
        //                    rdo_HK.Checked = true;
        //                }




        //                lblIntake.Text = objProgram.Intake_Number.ToString();
        //                lblDeadline.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
        //                lblDeadlinePopup.Text = objProgram.Application_Deadline.ToString("dd MMM yyyy");
        //                lblApplicant.Text = GetCurrentUser();
        //                lblApplicationNo.Text = objProgram.Application_No_Prefix + "-";
        //                hdn_ApplicationID.Value = objUserProgramId.ToString();
        //                hdn_ProgramID.Value = objProgram.Programme_ID.ToString();

        //            }
        //            string strCurrentUser = GetCurrentUser();
        //            TB_CCMF_APPLICATION objIncubation = Intake.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));

        //            // if (objIncubation != null)

        //            //{
        //            //    lblLastSubmitted.Text = objIncubation.Last_Submitted.ToString("dd MMM yyyy");

        //            //    rbtnPanel1Q1.SelectedValue = Convert.ToString(objIncubation.Question1_1);
        //            //    rbtnPanel1Q2.SelectedValue = Convert.ToString(objIncubation.Question1_2);
        //            //    rbtnPanel1Q3.SelectedValue = Convert.ToString(objIncubation.Question1_3);
        //            //    rbtnPanel1Q4.SelectedValue = Convert.ToString(objIncubation.Question1_4);
        //            //    rbtnPanel1Q5.SelectedValue = Convert.ToString(objIncubation.Question1_5);
        //            //    rbtnPanel1Q6.SelectedValue = Convert.ToString(objIncubation.Question1_6);
        //            //    rbtnPanel1Q7.SelectedValue = Convert.ToString(objIncubation.Question1_7);
        //            //    rbtnPanel1Q8.SelectedValue = Convert.ToString(objIncubation.Question1_8);
        //            //    rbtnPanel1Q9.SelectedValue = Convert.ToString(objIncubation.Question1_9);
        //            //    rbtnPanel1Q10.SelectedValue = Convert.ToString(objIncubation.Question1_10);

        //            //    txtCompanyNameEnglish.Text = objIncubation.Company_Name_Eng;
        //            //    txtCompanyNameChinese.Text = objIncubation.Company_Name_Chi;
        //            //    txtAbstractEnglish.Text = objIncubation.Abstract;
        //            //    txtObjective.Text = objIncubation.Objective;
        //            //    txtbackground.Text = objIncubation.Background;
        //            //    txtPilot_Work_Done.Text = objIncubation.Pilot_Work_Done;

        //            //    txtAdditionalInformation.Text = objIncubation.Additional_Information;
        //            //    txtProposedProducts.Text = objIncubation.Proposed_Products;
        //            //    txtTargetMarket.Text = objIncubation.Target_Market;
        //            //    txtCompetitionAnalysis.Text = objIncubation.Competition_Analysis;
        //            //    txtRevenueModel.Text = objIncubation.Revenus_Model;
        //            //    txtFirst6Months.Text = objIncubation.First_6_Months_Milestone;
        //            //    txtSecond6Months.Text = objIncubation.Second_6_Months_Milestone;
        //            //    txtThird6Months.Text = objIncubation.Third_6_Months_Milestone;
        //            //    txtForth6Months.Text = objIncubation.Forth_6_Months_Milestone;
        //            //    txtExitStrategy.Text = objIncubation.Exit_Strategy;
        //            //    if (objIncubation.Resubmission != null)
        //            //        rbtnResubmission.SelectedValue = Convert.ToString(objIncubation.Resubmission);
        //            //    txtResubmission_Project_Reference.Text = objIncubation.Resubmission_Project_Reference;
        //            //    txtResubmission_Main_Differences.Text = objIncubation.Resubmission_Main_Differences;
        //            //    rbtnCompany_Type.SelectedValue = objIncubation.Company_Type;
        //            //    if (objIncubation.Company_Type.ToLower() == "others")
        //            //    {
        //            //        txtOther_Company_Type.Text = objIncubation.Other_Company_Type;
        //            //        txtOther_Company_Type.Visible = true;
        //            //    }

        //            //    rbtnList_Business_Area.SelectedValue = objIncubation.Business_Area;
        //            //    if (objIncubation.Business_Area.ToLower() == "others")
        //            //    {
        //            //        txtOther_Bussiness_Area.Text = objIncubation.Other_Bussiness_Area;
        //            //        txtOther_Bussiness_Area.Visible = true;
        //            //    }
        //            //    string[] Positioning = objIncubation.Positioning.Split(';');
        //            //    txtPositioningOther.Text = objIncubation.Other_Positioning;
        //            //    foreach (string strPos in Positioning.Where(x => !string.IsNullOrEmpty(x)))
        //            //    {
        //            //        chkPositioning.SelectedValue = strPos;
        //            //    }
        //            //    txtOtherAttributes.Text = objIncubation.Other_Attributes;
        //            //    rbtnPreferred_Track.SelectedValue = objIncubation.Preferred_Track;
        //            //    txtCompany_Ownership_1.Text = objIncubation.Company_Ownership_Structure;
        //            //    txtPartner_Profiles.Text = objIncubation.Major_Partners_Profiles;
        //            //    Manpower_Distribution.Text = objIncubation.Manpower_Distribution;
        //            //    Equipment_Distribution.Text = objIncubation.Equipment_Distribution;
        //            //    Other_Costs.Text = objIncubation.Other_Direct_Costs;
        //            //    Forecast_Income.Text = objIncubation.Forecast_Income;
        //            //}
        //        }
        //    }

        //    catch (Exception)
        //    {
        //        Context.Response.Redirect("~/SitePages/Home.aspx");
        //    }
        //}
        //protected string GetCurrentUser()
        //{

        //    return new SPFunctions().GetCurrentUser().ToLower();
        //}
        //protected void btnIncubationForm_Click(object sender, EventArgs e)
        //{
        //    SetPanelVisibilityOfStep(1);
        //}
        //protected void btn_StepPrevious_Click(object sender, EventArgs e)
        //{
        //    lbl_Exception.InnerHtml = "";
        //    SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) - 1);
        //}
        //protected void btn_StepNext_Click(object sender, EventArgs e)
        //{
        //    lbl_Exception.InnerHtml = "";
        //    if ((Convert.ToInt32(hdn_ActiveStep.Value) + 1) == 2)
        //    {
        //        SaveStep1Data();
        //        InitializeFundingStatus();
        //    }
        //    else
        //    if ((Convert.ToInt16(hdn_ActiveStep.Value) + 1) == 3)
        //    {
        //        InitialCoreMembers();
        //    }

        //    SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) + 1);

        //}
        //protected void btn_StepSave_Click(object sender, EventArgs e)
        //{
        //    lbl_Exception.InnerHtml = "";
        //    int ActiveStep = Convert.ToInt16(hdn_ActiveStep.Value);
        //    if (ActiveStep == 1)
        //    {
        //        SaveStep1Data();
        //    }
        //    else if (ActiveStep == 2)
        //    {
        //        SaveStep2Data();
        //    }
        //    else if (ActiveStep == 3)
        //    {
        //        SaveStep3Data();
        //    }
        //    else if (ActiveStep == 4)
        //    {
        //        SaveStep4Data();
        //    }
        //    else if (ActiveStep == 5)
        //    {
        //        SaveStep5Data();
        //    }
        //    else if (ActiveStep == 6)
        //    {
        //        SaveStep6Data();
        //    }
        //}
        //protected void SetPanelVisibilityOfStep(int ActiveStep)
        //{
        //    hdn_ActiveStep.Value = ActiveStep.ToString();
        //    pnl_programDetail.Visible = true;
        //    pnl_Buttons.Visible = true;
        //    progressList.Visible = true;
        //    btn_StepNext.Visible = true;
        //    btn_Submit.Visible = false;
        //    IncubationSubmitPopup.Visible = false;
        //    if (ActiveStep > 0)
        //    {

        //        foreach (ListItem objList in progressList.Items)
        //        {
        //            if (Convert.ToInt32(objList.Value) <= ActiveStep)
        //            {
        //                objList.Attributes.Add("class", "active");
        //            }
        //            else
        //                objList.Attributes.Remove("class");

        //        }
        //    }
        //    switch (ActiveStep)
        //    {
        //        case 0:
        //            {
        //                pnl_InstructionForm.Visible = true;

        //                pnl_IncubationStep1.Visible = false;
        //                pnl_IncubationStep2.Visible = false;
        //                pnl_IncubationStep3.Visible = false;
        //                pnl_IncubationStep4.Visible = false;
        //                pnl_IncubationStep5.Visible = false;
        //                pnl_programDetail.Visible = false;
        //                pnl_Buttons.Visible = false;
        //                progressList.Visible = false;


        //            }
        //            break;
        //        case 1:
        //            {
        //                pnl_IncubationStep1.Visible = true;



        //                pnl_InstructionForm.Visible = false;
        //                pnl_IncubationStep2.Visible = false;
        //                pnl_IncubationStep3.Visible = false;
        //                pnl_IncubationStep4.Visible = false;
        //                pnl_IncubationStep5.Visible = false;

        //            }
        //            break;
        //        case 2:
        //            {
        //                pnl_IncubationStep2.Visible = true;
        //                if (rdo_HK.Checked)
        //                {
        //                    pnl_HK.Visible = true;
        //                    pnl_crossborder.Visible = false;
        //                }
        //                else
        //                {
        //                    pnl_HK.Visible = false;
        //                    pnl_crossborder.Visible = true;

        //                }
        //                pnl_InstructionForm.Visible = false;
        //                pnl_IncubationStep1.Visible = false;
        //                pnl_IncubationStep3.Visible = false;
        //                pnl_IncubationStep4.Visible = false;
        //                pnl_IncubationStep5.Visible = false;

        //            }
        //            break;
        //        case 3:
        //            {
        //                pnl_IncubationStep3.Visible = true;

        //                pnl_InstructionForm.Visible = false;
        //                pnl_IncubationStep2.Visible = false;
        //                pnl_IncubationStep1.Visible = false;
        //                pnl_IncubationStep4.Visible = false;
        //                pnl_IncubationStep5.Visible = false;

        //            }
        //            break;
        //        case 4:
        //            {
        //                pnl_IncubationStep4.Visible = true;

        //                pnl_InstructionForm.Visible = false;
        //                pnl_IncubationStep2.Visible = false;
        //                pnl_IncubationStep3.Visible = false;
        //                pnl_IncubationStep1.Visible = false;
        //                pnl_IncubationStep5.Visible = false;

        //            }
        //            break;
        //        case 5:
        //            {
        //                pnl_IncubationStep5.Visible = true;

        //                pnl_InstructionForm.Visible = false;
        //                pnl_IncubationStep2.Visible = false;
        //                pnl_IncubationStep3.Visible = false;
        //                pnl_IncubationStep4.Visible = false;
        //                pnl_IncubationStep1.Visible = false;
        //                btn_StepNext.Visible = false;
        //                btn_Submit.Visible = true;

        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //protected void ButtonAddCoreMembers_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    AddNewCoreMemberRow();
        //}
        //private void InitialCoreMembers()
        //{
        //    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(Guid.Parse(hdn_ApplicationID.Value));

        //    if (objCoreMembers.Count == 0)
        //    {
        //        objCoreMembers.Add(new TB_APPLICATION_COMPANY_CORE_MEMBER() { Core_Member_ID = 0 });
        //    }

        //    grvCoreMember.DataSource = objCoreMembers;
        //    grvCoreMember.DataBind();
        //}
        //private void AddNewCoreMemberRow()
        //{
        //    bool IsError = false;
        //    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();

        //    for (int i = 0; i < grvCoreMember.Rows.Count; i++)
        //    {
        //        try
        //        {


        //            HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
        //            TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
        //            TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
        //            TextBox txtAmountR = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Academic");
        //            TextBox txtAmountMax = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Experience");
        //            TextBox txtAchievements = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Achievements");

        //            TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
        //            objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
        //            objMember.Name = TextBoxAddress.Text;
        //            objMember.Position = txtNOE.Text;
        //            objMember.Professional_Qualifications = txtAmountR.Text;
        //            objMember.Working_Experiences = txtAmountMax.Text;
        //            objMember.Special_Achievements = txtAchievements.Text;
        //            objCoreMembers.Add(objMember);
        //        }
        //        catch (Exception)
        //        {

        //            IsError = true;
        //            lblCorememberError.Text = "Fill all the record in Member " + (i + 1).ToString();
        //            break;


        //        }
        //    }
        //    if (IsError == false)
        //    {
        //        objCoreMembers.Add(new TB_APPLICATION_COMPANY_CORE_MEMBER() { Core_Member_ID = 0 });


        //        grvCoreMember.DataSource = objCoreMembers;
        //        grvCoreMember.DataBind();

        //    }
        //    //  SetPreviousData();
        //}

        //private List<TB_APPLICATION_COMPANY_CORE_MEMBER> GetCoreMemberForSave(out bool IsError)
        //{
        //    IsError = false;
        //    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();



        //    for (int i = 0; i < grvCoreMember.Rows.Count; i++)
        //    {
        //        try
        //        {

        //            HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
        //            TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Name");
        //            TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Title");
        //            TextBox txtAmountR = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Academic");
        //            TextBox txtAmountMax = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Experience");
        //            TextBox txtAchievements = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("Achievements");
        //            if (TextBoxAddress.Text != "" && txtNOE.Text != "" && txtAmountR.Text != "" && txtAmountMax.Text != "" && txtAchievements.Text != "")
        //            {
        //                TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
        //                objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
        //                objMember.Name = TextBoxAddress.Text;
        //                objMember.Position = txtNOE.Text;
        //                objMember.Professional_Qualifications = txtAmountR.Text;
        //                objMember.Working_Experiences = txtAmountMax.Text;
        //                objMember.Special_Achievements = txtAchievements.Text;
        //                objCoreMembers.Add(objMember);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            IsError = true;
        //            lbl_Exception.InnerHtml = "Fill the all values in " + (i + 1).ToString() + " Core members ";
        //            break;

        //        }
        //    }

        //    return objCoreMembers;

        //    //  SetPreviousData();
        //}

        //private void InitializeFundingStatus()
        //{
        //    List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(Guid.Parse(hdn_ApplicationID.Value));
        //    if (ObjFundingStatus.Count == 0)
        //    {
        //        ObjFundingStatus = new List<TB_APPLICATION_FUNDING_STATUS>();
        //        ObjFundingStatus.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = DateTime.Now });

        //    }
        //    Grd_FundingStatus.DataSource = ObjFundingStatus;
        //    Grd_FundingStatus.DataBind();
        //}
        //private void AddNewFundingStatus()
        //{
        //    bool IsError = false;
        //    lbl_fundingError.Text = string.Empty;

        //    List<TB_APPLICATION_FUNDING_STATUS> objCoreMembers = new List<TB_APPLICATION_FUNDING_STATUS>();

        //    for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
        //    {
        //        try
        //        {


        //            HiddenField FundingID = (HiddenField)Grd_FundingStatus.Rows[i].Cells[0].FindControl("FundingID");
        //            TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
        //            TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
        //            TextBox txtApplicationStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationStatus");
        //            TextBox txtFundingStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtFundingStatus");
        //            TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
        //            TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
        //            TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");

        //            TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
        //            objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
        //            objMember.Programme_Name = txtNameofProgram.Text;
        //            objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
        //            objMember.Funding_Status = txtFundingStatus.Text;
        //            objMember.Application_Status = txtApplicationStatus.Text;
        //            objMember.Expenditure_Nature = txtNature.Text;
        //            objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);
        //            objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);
        //            objMember.Application_ID = Guid.Parse(hdn_ApplicationID.Value);



        //            objCoreMembers.Add(objMember);
        //        }
        //        catch (Exception)
        //        {
        //            IsError = true;
        //            lbl_fundingError.Text = "Fill all the record in funding " + (i + 1).ToString();
        //            break;

        //        }
        //    }
        //    if (IsError == false)
        //    {
        //        objCoreMembers.Add(new TB_APPLICATION_FUNDING_STATUS() { Funding_ID = 0, Date = DateTime.Now });


        //        Grd_FundingStatus.DataSource = objCoreMembers;
        //        Grd_FundingStatus.DataBind();
        //    }

        //    //  SetPreviousData();
        //}

        //private List<TB_APPLICATION_FUNDING_STATUS> GetFundingStatusForSave(out bool IsError)
        //{
        //    IsError = false;
        //    List<TB_APPLICATION_FUNDING_STATUS> objCoreMembers = new List<TB_APPLICATION_FUNDING_STATUS>();


        //    for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
        //    {
        //        try
        //        {


        //            HiddenField FundingID = (HiddenField)Grd_FundingStatus.Rows[i].Cells[0].FindControl("FundingID");
        //            TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
        //            TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
        //            TextBox txtApplicationStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationStatus");
        //            TextBox txtFundingStatus = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtFundingStatus");
        //            TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
        //            TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
        //            TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
        //            if (txtNameofProgram.Text != "" && txtApplicationStatus.Text != "" && txtFundingStatus.Text != "" && txtNature.Text != "" && txtAmountReceived.Text != "" && txtApplicationMaximumAmount.Text != "")
        //            {
        //                TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
        //                objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
        //                objMember.Programme_Name = txtNameofProgram.Text;
        //                objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
        //                objMember.Funding_Status = txtFundingStatus.Text;
        //                objMember.Application_Status = txtApplicationStatus.Text;
        //                objMember.Expenditure_Nature = txtNature.Text;
        //                objMember.Amount_Received = Convert.ToDecimal(txtAmountReceived.Text);
        //                objMember.Maximum_Amount = Convert.ToDecimal(txtApplicationMaximumAmount.Text);
        //                objMember.Application_ID = Guid.Parse(hdn_ApplicationID.Value);
        //                objCoreMembers.Add(objMember);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            IsError = true;
        //            lbl_Exception.InnerHtml = "Fill the all values in " + (i + 1).ToString() + " Funding status";
        //            break;

        //        }
        //    }

        //    return objCoreMembers;

        //    //  SetPreviousData();
        //}

        //protected void GetSPProgramList()
        //{
        //    SPList objUserProgram = null;
        //    using (SPSite oSPsite = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url))
        //    {

        //        using (SPWeb oSPweb = oSPsite.OpenWeb())
        //        {
        //            objUserProgram = oSPweb.Lists["Incubation"];

        //        }
        //    }
        //}

        //protected void SaveStep1Data()
        //{
        //    //string ErrorMessage = string.Empty;

        //    //bool IsError = false;
        //    //try
        //    //{
        //    //    using (var dbContext = new CyberportEMS_EDM())
        //    //    {
        //    //        int progId = Convert.ToInt32(hdn_ProgramID.Value);

        //    //        string strCurrentUser = GetCurrentUser();
        //    //        TB_INCUBATION_APPLICATION objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
        //    //        if (objIncubation == null)
        //    //        {
        //    //            TB_CCMF_APPLICATION objTB_CCMF_APPLICATION = new TB_CCMF_APPLICATION();
        //    //            objTB_CCMF_APPLICATION.CCMF_ID = Guid.NewGuid();
        //    //            objTB_CCMF_APPLICATION.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);
        //    //            objTB_CCMF_APPLICATION.Application_Number = Convert.ToString(lblApplicationNo.Text);
        //    //            objTB_CCMF_APPLICATION.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
        //    //            objTB_CCMF_APPLICATION.Applicant = GetCurrentUser();
        //    //            objTB_CCMF_APPLICATION.Last_Submitted = DateTime.Now;
        //    //            objTB_CCMF_APPLICATION.Status = "Saved";
        //    //            objTB_CCMF_APPLICATION.Version_Number = "0.1";
        //    //            if (rdo_HK.SelectedValue != "")
        //    //                objTB_CCMF_APPLICATION.Have_Read_Statement = Convert.ToBoolean(rbtnPanel1Q1.SelectedValue);
        //    //            if (rbtnPanel1Q2.SelectedValue != "")
        //    //                objNewApplication.Question1_2 = Convert.ToBoolean(rbtnPanel1Q2.SelectedValue);
        //    //            if (rbtnPanel1Q3.SelectedValue != "")
        //    //                objNewApplication.Question1_3 = Convert.ToBoolean(rbtnPanel1Q3.SelectedValue);
        //    //            if (rbtnPanel1Q4.SelectedValue != "")
        //    //            {
        //    //                objNewApplication.Question1_4 = Convert.ToBoolean(rbtnPanel1Q4.SelectedValue);
        //    //                if (objNewApplication.Question1_4 == false && rbtnPanel1Q5.SelectedValue == "")
        //    //                {
        //    //                    rbtnPanel1Q5.Enabled = true;

        //    //                    ErrorMessage += "Please select Question 1.5<br>";
        //    //                    IsError = true;
        //    //                }
        //    //                else if (rbtnPanel1Q5.SelectedValue != "")
        //    //                    objNewApplication.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
        //    //            }
        //    //            if (rbtnPanel1Q6.SelectedValue != "")
        //    //                objNewApplication.Question1_6 = Convert.ToBoolean(rbtnPanel1Q6.SelectedValue);
        //    //            if (rbtnPanel1Q7.SelectedValue != "")
        //    //                objNewApplication.Question1_7 = Convert.ToBoolean(rbtnPanel1Q7.SelectedValue);
        //    //            if (rbtnPanel1Q8.SelectedValue != "")
        //    //            {
        //    //                objNewApplication.Question1_8 = Convert.ToBoolean(rbtnPanel1Q8.SelectedValue);
        //    //                if (objNewApplication.Question1_8 == true && rbtnPanel1Q9.SelectedValue == "")
        //    //                {
        //    //                    rbtnPanel1Q9.Enabled = true;

        //    //                    ErrorMessage += "Please select Question 1.9<br>";
        //    //                    IsError = true;
        //    //                }
        //    //                else if (rbtnPanel1Q9.SelectedValue == "")
        //    //                    objNewApplication.Question1_9 = Convert.ToBoolean(rbtnPanel1Q9.SelectedValue);
        //    //            }
        //    //            if (rbtnPanel1Q10.SelectedValue != "")
        //    //                objNewApplication.Question1_10 = Convert.ToBoolean(rbtnPanel1Q10.SelectedValue);


        //    //            objNewApplication.Created_By = GetCurrentUser();
        //    //            objNewApplication.Created_Date = DateTime.Now;
        //    //            objNewApplication.Modified_By = GetCurrentUser();
        //    //            objNewApplication.Modified_Date = DateTime.Now;
        //    //            if (IsError == false)
        //    //            {
        //    //                dbContext.TB_INCUBATION_APPLICATION.Add(objNewApplication);
        //    //                dbContext.SaveChanges();
        //    //            }
        //    //            else
        //    //            {
        //    //                lbl_Exception.InnerHtml = ErrorMessage;
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            objIncubation.Intake_Number = Convert.ToInt32(lblIntake.Text.Trim());
        //    //            objIncubation.Applicant = GetCurrentUser();
        //    //            objIncubation.Last_Submitted = DateTime.Now;
        //    //            objIncubation.Status = "Saved";
        //    //            objIncubation.Version_Number = "0.1";
        //    //            if (rbtnPanel1Q1.SelectedValue != "")
        //    //                objIncubation.Question1_1 = Convert.ToBoolean(rbtnPanel1Q1.SelectedValue);
        //    //            if (rbtnPanel1Q2.SelectedValue != "")
        //    //                objIncubation.Question1_2 = Convert.ToBoolean(rbtnPanel1Q2.SelectedValue);
        //    //            if (rbtnPanel1Q3.SelectedValue != "")
        //    //                objIncubation.Question1_3 = Convert.ToBoolean(rbtnPanel1Q3.SelectedValue);
        //    //            if (rbtnPanel1Q4.SelectedValue != "")
        //    //            {
        //    //                objIncubation.Question1_4 = Convert.ToBoolean(rbtnPanel1Q4.SelectedValue);
        //    //                if (objIncubation.Question1_4 == false && rbtnPanel1Q5.SelectedValue == "")
        //    //                {
        //    //                    rbtnPanel1Q5.Enabled = true;
        //    //                    ErrorMessage += "Please select Question 1.5<br>";
        //    //                    IsError = true;
        //    //                }
        //    //                else if (rbtnPanel1Q5.SelectedValue == "")
        //    //                    objIncubation.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
        //    //            }
        //    //            if (rbtnPanel1Q5.SelectedValue != "")
        //    //                objIncubation.Question1_5 = Convert.ToBoolean(rbtnPanel1Q5.SelectedValue);
        //    //            if (rbtnPanel1Q6.SelectedValue != "")
        //    //                objIncubation.Question1_6 = Convert.ToBoolean(rbtnPanel1Q6.SelectedValue);
        //    //            if (rbtnPanel1Q7.SelectedValue != "")
        //    //                objIncubation.Question1_7 = Convert.ToBoolean(rbtnPanel1Q7.SelectedValue);
        //    //            if (rbtnPanel1Q8.SelectedValue != "")
        //    //            {
        //    //                objIncubation.Question1_8 = Convert.ToBoolean(rbtnPanel1Q8.SelectedValue);
        //    //                if (objIncubation.Question1_8 == true && rbtnPanel1Q9.SelectedValue == "")
        //    //                {
        //    //                    rbtnPanel1Q9.Enabled = true;

        //    //                    ErrorMessage += "Please select Question 1.9<br>";
        //    //                    IsError = true;
        //    //                }
        //    //                else if (rbtnPanel1Q9.SelectedValue == "")
        //    //                    objIncubation.Question1_9 = Convert.ToBoolean(rbtnPanel1Q9.SelectedValue);
        //    //            }
        //    //            if (rbtnPanel1Q10.SelectedValue != "")
        //    //                objIncubation.Question1_10 = Convert.ToBoolean(rbtnPanel1Q10.SelectedValue);
        //    //            if (IsError == false)
        //    //            {
        //    //                dbContext.SaveChanges();
        //    //            }
        //    //            else
        //    //            {
        //    //                lbl_Exception.InnerHtml = ErrorMessage;
        //    //            }
        //    //        }
        //    //    }

        //    //}

        //    //catch (Exception ex)
        //    //{
        //    //    lbl_Exception.InnerHtml = ex.Message;
        //    //}
        //}

        //protected void SaveStep4Data()
        //{
        //    string ErrorMessage = string.Empty;
        //    try
        //    {
        //        using (var dbContext = new CyberportEMS_EDM())
        //        {
        //            int progId = Convert.ToInt32(hdn_ProgramID.Value);
        //            // Guid appId = Guid.Parse(hdn_ApplicationID.Value);
        //            string strCurrentUser = GetCurrentUser();
        //            TB_CCMF_APPLICATION objIncubation = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
        //            if (objIncubation != null)
        //            {
        //                bool IsError = false;

        //                List<TB_APPLICATION_FUNDING_STATUS> objFunds = GetFundingStatusForSave(out IsError);



        //                if (IsError == false)
        //                {
        //                    objFunds.ForEach(x => x.Application_ID = objIncubation.Incubation_ID);
        //                    objFunds.ForEach(x => x.Programme_ID = objIncubation.Programme_ID);
        //                    IncubationContext.APPLICATION_FUNDING_STATUS_ADDUPDATE(dbContext, objFunds);
        //                    objIncubation.Last_Submitted = DateTime.Now;

        //                    objIncubation.Project_Management_Team = txtpromanagteam.Text.Trim();
        //                    objIncubation.Business_Model = txtbusinessmodelteam.Text.Trim();

        //                    objIncubation.Innovation = txtcreativity.Text.Trim();

        //                    objIncubation.Social_Responsibility = txtsocialrespon.Text.Trim();
        //                    objIncubation.Competition_Analysis = txtcompanalysis.Text.Trim();
        //                    objIncubation.Project_Milestone_M1 = txtmonth1.Text.Trim();
                           
        //                     objIncubation.Project_Milestone_M2 = txtmonth2.Text.Trim();
        //                    objIncubation.Project_Milestone_M3 = txtmonth3.Text.Trim();
        //                    objIncubation.Project_Milestone_M4 = txtmonth4.Text.Trim();
        //                    objIncubation.Project_Milestone_M5 = txtmonth5.Text.Trim();
        //                    objIncubation.Project_Milestone_M6 = txtmonth6.Text.Trim();
        //                    objIncubation.Cost_Projection = txtcostprojection.Text.Trim();
        //                    objIncubation.Exit_Stategy = txtexitstrategy.Text.Trim();
        //                    objIncubation.Additional_Information = txtaddinformation.Text.Trim();
                           
                          


        //                    if (IsError == false)
        //                        dbContext.SaveChanges();
        //                    else
        //                    {
        //                        lbl_Exception.InnerHtml = ErrorMessage;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                lbl_Exception.InnerHtml = "Could not found the relevent data.";
        //            }
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        lbl_Exception.InnerHtml = ex.Message;
        //    }
        //}


        //protected void SaveStep3Data()
        //{
        //    string ErrorMessage = string.Empty;
        //    try
        //    {
        //        using (var dbContext = new CyberportEMS_EDM())
        //        {
        //            int progId = Convert.ToInt32(hdn_ProgramID.Value);
        //            string strCurrentUser = GetCurrentUser();

        //            TB_CCMF_APPLICATION objIncubation = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
        //            if (objIncubation != null)
        //            {
        //                bool IsError = false;
        //                string FileName = string.Empty;

        //                objIncubation.Project_Name_Eng = English.Text.Trim();
        //                objIncubation.Project_Name_Chi = Chinese.Text.Trim();
        //                objIncubation.Abstract_Eng = txtProjectInfoAbsEng.Text.Trim();
        //                objIncubation.Abstract_Chi = txtProjectInfoAbschi.Text.Trim();
        //                objIncubation.Business_Area = RadioButtonList1.SelectedValue;
        //                if (RadioButtonList1.SelectedValue.ToLower() == "others")
        //                {
        //                    objIncubation.Other_Business_Area = txtOther_Bussiness_Area.Text.Trim();
        //                }

        //                objIncubation.Submission_Date = Convert.ToDateTime(txtantisdate.Text.Trim());
        //                objIncubation.Completion_Date = Convert.ToDateTime(txtanticdate.Text.Trim());


        //                if (IsError == false)
        //                    dbContext.SaveChanges();
        //                else
        //                {
        //                    lbl_Exception.InnerHtml = ErrorMessage;
        //                }
        //            }

        //            else
        //            {
        //                lbl_Exception.InnerHtml = "Could not found the relevent data.";
        //            }
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        lbl_Exception.InnerHtml = ex.Message;
        //    }
        //}
        //protected void SaveStep2Data()
        //{ }
        //protected void SaveStep5Data()
        //{
        //    string ErrorMessage = string.Empty;
        //    try
        //    {
        //        using (var dbContext = new CyberportEMS_EDM())
        //        {
        //            int progId = Convert.ToInt32(hdn_ProgramID.Value);
        //            string strCurrentUser = GetCurrentUser();

        //            TB_CCMF_APPLICATION objIncubation = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
        //            if (objIncubation != null)
        //            {
        //                List<TB_APPLICATION_CONTACT_DETAIL> objcontactdetails = new List<TB_APPLICATION_CONTACT_DETAIL>();
        //                TB_APPLICATION_CONTACT_DETAIL objTB_APPLICATION_CONTACT_DETAIL = new TB_APPLICATION_CONTACT_DETAIL();
        //                bool IsError = false;
        //                string FileName = string.Empty;

        //                objTB_APPLICATION_CONTACT_DETAIL.Application_ID = objIncubation.CCMF_ID;
        //                objTB_APPLICATION_CONTACT_DETAIL.Programme_ID = progId;
        //                objTB_APPLICATION_CONTACT_DETAIL.Last_Name_Eng = txtContactLast_name.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.First_Name_Eng = txtContactFirst_name.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Position = txtContactPostition.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Contact_No_Home = txtContactNoHome.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Contact_No_Office = txtContactNoOffice.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Contact_No_Mobile = txtContactNoMobile.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Fax = Convert.ToInt32(txtContactFax.Text.Trim());
        //                objTB_APPLICATION_CONTACT_DETAIL.Email = txtContactEmail.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Mailing_Address = txtContactAddress.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL.Salutation = Salutation.Text.Trim();
        //                objcontactdetails.Add(objTB_APPLICATION_CONTACT_DETAIL);
        //                TB_APPLICATION_CONTACT_DETAIL objTB_APPLICATION_CONTACT_DETAIL1 = new TB_APPLICATION_CONTACT_DETAIL();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Application_ID = objIncubation.CCMF_ID;
        //                objTB_APPLICATION_CONTACT_DETAIL1.Last_Name_Eng = txtContactLast_name1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.First_Name_Eng = txtContactFirst_name1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Position = txtContactPostition1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Contact_No_Home = txtContactNoHome1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Contact_No_Office = txtContactNoOffice1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Contact_No_Mobile = txtContactNoMobile1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Fax = Convert.ToInt32(txtContactFax1.Text.Trim());
        //                objTB_APPLICATION_CONTACT_DETAIL1.Email = txtContactEmail1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Mailing_Address = txtContactAddress1.Text.Trim();
        //                objTB_APPLICATION_CONTACT_DETAIL1.Salutation = Salutation.Text.Trim();
        //                objcontactdetails.Add(objTB_APPLICATION_CONTACT_DETAIL1);
        //                IncubationContext.TB_APPLICATION_CONTACTDETAILSADDUPDATE(dbContext, objIncubation.CCMF_ID, progId, objcontactdetails);




        //                if (IsError == false)
        //                    dbContext.SaveChanges();
        //                else
        //                {
        //                    lbl_Exception.InnerHtml = ErrorMessage;
        //                }
        //            }

        //            else
        //            {
        //                lbl_Exception.InnerHtml = "Could not found the relevent data.";
        //            }
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        lbl_Exception.InnerHtml = ex.Message;
        //    }
        //}
        //protected void btn_FundingAddNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    AddNewFundingStatus();

        //}

        //protected void SaveStep6Data()
        //{
        //    string ErrorMessage = string.Empty;
        //    try
        //    {
        //        using (var dbContext = new CyberportEMS_EDM())
        //        {
        //            int progId = Convert.ToInt32(hdn_ProgramID.Value);
        //            string strCurrentUser = GetCurrentUser();

        //            TB_CCMF_APPLICATION objIncubation = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && (x.Created_By.ToLower() == strCurrentUser || x.Modified_By.ToLower() == strCurrentUser));
        //            if (objIncubation != null)
        //            {


        //                SaveAttachment(fu_BrCopy, enumAttachmentType.BR_COPY, objIncubation.CCMF_ID, progId);
        //                SaveAttachment(fu_AnnualReturn, enumAttachmentType.Company_Annual_Return, objIncubation.CCMF_ID, progId);
        //                SaveAttachment(fuPresentationSlide, enumAttachmentType.Presentation_Slide, objIncubation.CCMF_ID, progId);
        //                SaveAttachment(fuOtherAttachement, enumAttachmentType.Other_Attachment, objIncubation.CCMF_ID, progId);

        //                SaveAttachmentUrl(txtVideoClip.Text, enumAttachmentType.Video_Clip, objIncubation.CCMF_ID, progId);


        //                objIncubation.Last_Submitted = DateTime.Now;
        //                objIncubation.Declaration = chkDeclaration.Checked;
        //                objIncubation.Marketing_Information = Marketing_Information.Checked;
        //                objIncubation.Have_Read_Statement = Personal_Information.Checked;
        //                objIncubation.Principal_Full_Name = txtName_PrincipalApplicant.Text.Trim();
        //                objIncubation.Principal_Position_Title = txtPosition_PrincipalApplicant.Text.Trim();
        //                dbContext.SaveChanges();

        //            }
        //            else
        //            {
        //                lbl_Exception.InnerHtml = "Could not found the relevent data.";
        //            }
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        lbl_Exception.InnerHtml = ex.Message;
        //    }
        //}

        //public bool SaveAttachment(FileUpload objUploder, enumAttachmentType objAttachmentType, Guid appId, int progId)
        //{
        //    //using (var dbContext = new CyberportEMS_EDM())
        //    //{
        //    //    string FileName = string.Empty;

        //    //    if (objUploder.HasFile)
        //    //    {
        //    //        if (objUploder.PostedFile.ContentLength <= (5 * 1024 * 1024))
        //    //        {
        //    //            string Extension = objUploder.FileName.Remove(0, objUploder.FileName.LastIndexOf("."));
        //    //            FileName = appId + "_" + objAttachmentType.ToString() + Extension;
        //    //            fu_Company_Ownership_2.SaveAs(CBPCommonConstants.GetAttachementFolderPhysical(objAttachmentType) + FileName);
        //    //            TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
        //    //            {
        //    //                Application_ID = appId,
        //    //                Attachment_Path = FileName,
        //    //                Attachment_Type = objAttachmentType.ToString(),
        //    //                Created_By = GetCurrentUser(),
        //    //                Created_Date = DateTime.Now,
        //    //                Modified_By = GetCurrentUser(),
        //    //                Modified_Date = DateTime.Now,
        //    //                Programme_ID = progId
        //    //            };
        //    //            IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach);
        //    //            dbContext.SaveChanges();
        //    //            return true;
        //    //        }
        //    //        else
        //    //        {
        //    //            return false;
        //    //        }

        //    //    }
        //    //    else
        //    //        return true;
        //    //}
        //    return true;
        //}

        //public bool SaveAttachmentUrl(string Url, enumAttachmentType objAttachmentType, Guid appId, int progId)
        //{
        //    using (var dbContext = new CyberportEMS_EDM())
        //    {
        //        string FileName = string.Empty;

        //        FileName = Url;
        //        TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
        //        {
        //            Application_ID = appId,
        //            Attachment_Path = FileName,
        //            Attachment_Type = objAttachmentType.ToString(),
        //            Created_By = GetCurrentUser(),
        //            Created_Date = DateTime.Now,
        //            Modified_By = GetCurrentUser(),
        //            Modified_Date = DateTime.Now,
        //            Programme_ID = progId
        //        };
        //        IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach);
        //        dbContext.SaveChanges();
        //        return true;
        //    }

        //}

        //protected void btn_Submit_Click(object sender, EventArgs e)
        //{

        //}

        //protected void btn_submitFinal_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtLoginUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
        //        {
        //            bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtLoginUserName.Text, txtLoginPassword.Text);

        //            if (!status)
        //            {
        //                IncubationSubmitPopup.Visible = true;
        //                UserCustomerrorLogin.InnerText = "Invalid Email and Password";
        //            }
        //            else
        //            {
        //                using (var dbContext = new CyberportEMS_EDM())
        //                {
        //                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
        //                    Guid appId = Guid.Parse(hdn_ApplicationID.Value);
        //                    TB_INCUBATION_APPLICATION objIncubation = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == progId && x.CCMF_ID == appId);
        //                    if (objIncubation != null)
        //                    {
        //                        objIncubation.Last_Submitted = DateTime.Now;
        //                        objIncubation.Submission_Date = DateTime.Now;
        //                        dbContext.SaveChanges();
        //                        Context.Response.Redirect("~/SitePages/Home.aspx");
        //                    }
        //                    else
        //                    {
        //                        IncubationSubmitPopup.Visible = true;
        //                        UserCustomerrorLogin.InnerHtml = "Could not found the relevent data.";
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            IncubationSubmitPopup.Visible = true;
        //            UserCustomerrorLogin.InnerText = "Invalid Email and Password";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IncubationSubmitPopup.Visible = true;
        //        UserCustomerrorLogin.InnerText = ex.Message;
        //    }
        //}

        //protected void btn_Submit_Click1(object sender, EventArgs e)
        //{
        //    IncubationSubmitPopup.Visible = true;
        //}

        //protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        //{
        //    IncubationSubmitPopup.Visible = false;
        //}
    }
}
