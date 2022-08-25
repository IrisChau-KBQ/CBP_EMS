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
using System.Web.Configuration;

namespace CBP_EMS_SP.CASPApplicationForm.CASPApplicationForm
{
    [ToolboxItemAttribute(false)]
    public partial class CASPApplicationForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CASPApplicationForm()
        {
        }
        public bool IsApplicantUser = false;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        private Guid NewProgramId()
        {
            Guid objNewId = Guid.NewGuid();
            while (new CyberportEMS_EDM().TB_CASP_APPLICATION.Where(x => x.CASP_ID == objNewId).Count() == 0)
            {
                objNewId = Guid.NewGuid();
                break;
            }
            return objNewId;
        }
        public static string LocalizeUI(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_CASP");
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_CASP");
        }
        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;

            }
            SPFunctions.LocalizeUIForPage(UserLanguage);

            btn_StepNext.Text = SPFunctions.LocalizeUI("Next", "CyberportEMS_Common");
            btn_StepPrevious.Text = SPFunctions.LocalizeUI("Previous", "CyberportEMS_Common");
            //btn_HideSubmitPopup.Text = SPFunctions.LocalizeUI("Cancel", "CyberportEMS_Common");
            btn_StepSave.Text = SPFunctions.LocalizeUI("Save", "CyberportEMS_Common");
            btn_Submit.Text = SPFunctions.LocalizeUI("Submit", "CyberportEMS_Common");
            //btn_submitFinal.Text = SPFunctions.LocalizeUI("Submit", "CyberportEMS_Common");
            btnCASPForm.Text = SPFunctions.LocalizeUI("btn_Continue_Text", "CyberportEMS_Common");

            string ctrlname = HttpContext.Current.Request.Params.Get("__EVENTTARGET");

            if (!Page.IsPostBack && string.IsNullOrEmpty(ctrlname))
            {
                if (Context.Request.UrlReferrer != null)
                {
                    //  btn_Back.PostBackUrl = Context.Request.UrlReferrer.ToString();

                }

                if (string.IsNullOrEmpty(Context.Request.QueryString["prog"]) || string.IsNullOrEmpty(Context.Request.QueryString["app"]))
                {
                    Context.Response.Redirect("~/SitePages/MyApplications.aspx");
                }
                else
                {
                    // IncubationSubmitPopup.Visible = false;

                    FillIntakeDetails();
                }
            }

            string EndosProg = rbtnProgrammeEndorsed.SelectedValue;
            List<ListItem> EndosProgs = new List<ListItem>();
            EndosProgs.Add(new ListItem() { Value = "true", Text = Localize("Step_2_1_ProgrammeEndorsed") });
            EndosProgs.Add(new ListItem() { Value = "false", Text = Localize("Step_2_1_ProgrammeNotEndorsed") });
            if (!string.IsNullOrEmpty(EndosProg))
            {
                rbtnProgrammeEndorsed.SelectedValue = EndosProg;
            }

            rbtnProgrammeEndorsed.DataSource = EndosProgs;
            rbtnProgrammeEndorsed.DataTextField = "Text";
            rbtnProgrammeEndorsed.DataValueField = "Value";
            rbtnProgrammeEndorsed.DataBind();
            if (!string.IsNullOrEmpty(EndosProg))
            {
                rbtnProgrammeEndorsed.SelectedValue = EndosProg;
            }
            chkDeclaration.Text = SPFunctions.LocalizeUI("Step_4_declarationcheckbox ", "CyberportEMS_CASP");


        }

        protected void FillUserCompany(bool IsApplicantUser, TB_CASP_APPLICATION objApp)
        {

            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();
            if (!IsApplicantUser)
                strCurrentUser = objApp.Applicant;
            List<CASP_CompanyList> objCASP_CompanyList = IncubationContext.GetCompanyProjectNameForUser(strCurrentUser);
            if (objCASP_CompanyList.Count > 0)
            {
                ddl_companyProjects.DataSource = objCASP_CompanyList;
                ddl_companyProjects.DataTextField = "CompanyName";
                ddl_companyProjects.DataValueField = "CompanyIdNumber";
                ddl_companyProjects.DataBind();
                ddl_companyProjects.Items.Insert(0, new ListItem() { Text = "Select Company", Value = "" });
            }
            else
            {
                Context.Response.Redirect("~/SitePages/Home.aspx", false);
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

                        TB_CASP_APPLICATION objApp = GetExistingApplication(Intake, progId);

                        FillUserCompany(IsApplicantUser, objApp);


                        hdn_ProgramID.Value = objProgram.Programme_ID.ToString();
                        if (objApp != null)
                        {
                            //   bool isDisabled = false;



                            if (objApp.Status.Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " "))
                            {
                                btn_StepSave.Visible = false;
                            }

                            lblApplicant.Text = objApp.Created_By;
                            hdn_ApplicationID.Value = objApp.CASP_ID.ToString();
                            if (objApp.Last_Submitted.HasValue)
                                lblLastSubmitted.Text = objApp.Last_Submitted.Value.ToString("dd MMM yyyy");
                            lblApplicationNo.Text = HttpUtility.HtmlEncode(objApp.Application_No);



                            ddl_companyProjects.SelectedValue = objApp.Company_Project + ":" + objApp.CCMF_CPIP_App_No;
                            GetCompanydetails(ddl_companyProjects.SelectedValue.ToString());


                            txtCompanyRegAdd.Text = objApp.Company_Address;
                            lblCompanyRegAdd.Text = objApp.Company_Address;

                            txtabstract.Text = objApp.Abstract;
                            lblabstract.Text = objApp.Abstract;

                            txtownership.Text = objApp.Company_Ownership_Structure;
                            lblownership.Text = objApp.Company_Ownership_Structure;

                            InitializeFundingStatus();
                            InitialCoreMembers();
                            InitializeContact();
                            InitializeUploadsDocument();

                            txtAdditionalInformation.Text = objApp.Additional_Info;
                            lblAdditionalInformation.Text = objApp.Additional_Info;

                            txtprogramme.Text = objApp.Accelerator_Name;
                            lblprogramme.Text = objApp.Accelerator_Name;



                            if (objApp.Endorsed_by_Cyberport.HasValue)
                            {
                                rbtnProgrammeEndorsed.SelectedValue = objApp.Endorsed_by_Cyberport.Value.ToString().ToLower();
                            }

                            if (objApp.Commencement_Date.HasValue)
                            {
                                txtcommencementdate.Text = objApp.Commencement_Date.Value.ToString("dd-MMM-yyyy");
                                lblcommencementdate.Text = objApp.Commencement_Date.Value.ToString("dd-MMM-yyyy");
                            }

                            if (objApp.Duration.HasValue)
                            {
                                txtprogrammeduration.Text = objApp.Duration.ToString();
                                //lblprogrammeduration.Text = objApp.Duration.ToString() + " " + "month(s)";
                                lblprogrammeduration.Text = objApp.Duration.ToString();
                            }
                            txtbackground.Text = objApp.Background;
                            lblbackground.Text = objApp.Background;

                            txtoffering.Text = objApp.Offer;
                            lbloffering.Text = objApp.Offer;

                            txtfundraising.Text = objApp.Fund_Raising_Capabilities;
                            lblfundraising.Text = objApp.Fund_Raising_Capabilities;

                            txtalumnisize.Text = objApp.Size_of_Alumni;
                            lblalumnisize.Text = objApp.Size_of_Alumni;

                            txtreputation.Text = objApp.Reputation;
                            lblreputation.Text = objApp.Reputation;

                            txtwebsite.Text = objApp.Website;
                            lblwebsite.Text = objApp.Website;

                            if (objApp.Declaration.HasValue)
                                chkDeclaration.Checked = objApp.Declaration.Value;

                            txtPrinciple_Full_Name.Text = objApp.Principle_Full_Name;
                            lblPrinciple_Full_Name.Text = objApp.Principle_Full_Name;

                            txtPrinciple_Title.Text = objApp.Principle_Title;
                            lblPrinciple_Title.Text = objApp.Principle_Title;

                            if (((objApp.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) && (objApp.Status.Replace("_", " ") != formsubmitaction.Saved.ToString().Replace("_", " "))) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                            {
                                // isDisabled = true;
                                DisableControls();
                                DisableListTextbox();

                                if (objSp.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"], true))
                                { btn_Back.Visible = true; }
                            }
                        }
                        else
                        { // if there is no application for this user
                            //int count = 0;
                            //lblApplicationNo.Text = HttpUtility.HtmlEncode(objProgram.Application_No_Prefix + "-" + objProgram.Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                            lblApplicant.Text = objSp.GetCurrentUser();
                            hdn_ApplicationID.Value = "";
                            InitializeUploadsDocument();
                            InitialCoreMembers();
                            InitializeFundingStatus();
                            InitializeContact();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);
            }
        }


        protected void DisableControls()
        {
            fu_Company_Ownership_2.Enabled = false;

            FuAdmissionRecord.Enabled = false;

            btnAdmissionAttachement.Enabled = false;
            ImageButton1.Enabled = false;
            pnl_CASPStep1.Enabled = false;
            pnl_CASPStep2.Enabled = false;
            pnl_CASPStep3.Enabled = false;
            pnl_CASPStep4.Enabled = false;

            ddl_companyProjects.Enabled = false;
            chkDeclaration.Enabled = false;
            rbtnProgrammeEndorsed.Enabled = false;
            ButtonAddCoreMembers.Enabled = false;
            btn_Submit.Enabled = false;
            btn_Submit.Visible = false;
            btn_submitFinal.Visible = false;
            btn_StepSave.Visible = false;

            pnl_CASPStep1.Enabled = rptrcompanyownership.EnableTheming;
            pnl_CASPStep2.Enabled = rptradmission.EnableTheming;

            ShowLabels();
        }


        protected void disablepanelControls()
        {

        }

        private void ShowLabels()
        {

            //txtcorename.Visible = false;
            //lblcorename.Visible = true;

            //txtcorePosition.Visible = false;
            //lblcorePosition.Visible = true;

            //txtCoreMemberProfile.Visible = false;
            //lblCoreMemberProfile.Visible = true;

            //txtApplicationDate.Visible = false;
            //lblApplicationDate.Visible = true;

            //txtApplicationStatus.Visible = false;
            //lblApplicationStatus.Visible = true;

            //txtFundingStatus.Visible = false;
            //lblFundingStatus.Visible = true;


            txtprogramme.Visible = false;
            lblprogramme.Visible = true;


            txtownership.Visible = false;
            lblownership.Visible = true;

            txtabstract.Visible = false;
            lblabstract.Visible = true;

            txtCompanyRegAdd.Visible = false;
            lblCompanyRegAdd.Visible = true;

            txtPrinciple_Full_Name.Visible = false;
            lblPrinciple_Full_Name.Visible = true;

            txtPrinciple_Title.Visible = false;
            lblPrinciple_Title.Visible = true;

            txtwebsite.Visible = false;
            lblwebsite.Visible = true;

            txtreputation.Visible = false;
            lblreputation.Visible = true;

            txtwebsite.Visible = false;
            lblwebsite.Visible = true;

            txtreputation.Visible = false;
            lblreputation.Visible = true;

            txtalumnisize.Visible = false;
            lblalumnisize.Visible = true;

            txtfundraising.Visible = false;
            lblfundraising.Visible = true;

            txtoffering.Visible = false;
            lbloffering.Visible = true;

            txtbackground.Visible = false;
            lblbackground.Visible = true;

            txtprogrammeduration.Visible = false;
            lblprogrammeduration.Visible = true;
            txtcommencementdate.Visible = false;
            lblcommencementdate.Visible = true;
            txtAdditionalInformation.Visible = false;
            lblAdditionalInformation.Visible = true;

        }



        private void DisableListTextbox()
        {
            Grd_FundingStatus.Enabled = false;
            pnl_FundingAddNew.Visible = false;
            for (int i = 0; i < Grd_FundingStatus.Rows.Count; i++)
            {

                TextBox txtNameofProgram = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNameofProgram");
                TextBox txtApplicationDate = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationDate");
                DropDownList ddlApplication = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlApplication");
                DropDownList ddlFunding = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlFunding");
                TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
                DropDownList Currency = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("Currency");
                Image imgCalender = (Image)Grd_FundingStatus.Rows[i].Cells[0].FindControl("imgCalender");
                ImageButton btn_FundingRemoveNew = (ImageButton)Grd_FundingStatus.Rows[i].Cells[0].FindControl("btn_FundingRemoveNew");



                Label lblNameofProgram = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblNameofProgram");
                Label lblApplicationDate = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblApplicationDate");

                Label lblApplicationStatue = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblApplicationStatue");
                Label lblFundingStatus = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblFundingStatus");
                Label lblCurrency = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblCurrency");

                Label lblNature = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblNature");
                Label lblAmountReceived = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblAmountReceived");
                Label lblApplicationMaximumAmount = (Label)Grd_FundingStatus.Rows[i].Cells[0].FindControl("lblApplicationMaximumAmount");

                txtNameofProgram.Visible = btn_FundingRemoveNew.Visible = false;
                txtApplicationDate.Visible = false;
                ddlApplication.Visible = false;
                ddlFunding.Visible = false;
                txtNature.Visible = false;
                txtAmountReceived.Visible = false;
                txtApplicationMaximumAmount.Visible = false;
                Currency.Visible = false;
                imgCalender.Visible = false;


                lblNameofProgram.Visible = true;
                lblApplicationDate.Visible = true;
                lblApplicationStatue.Visible = true;
                lblFundingStatus.Visible = true;
                lblNature.Visible = true;
                lblAmountReceived.Visible = true;
                lblApplicationMaximumAmount.Visible = true;
                lblCurrency.Visible = true;

            }

            grvCoreMember.Enabled = false;
            pnl_CoreMemberAddNew.Visible = false;
            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                TextBox txtcorename = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorename");
                TextBox txtcorePosition = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorePosition");
                TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMemberProfile");

                ImageButton btn_CoreRemoveNew = (ImageButton)grvCoreMember.Rows[i].Cells[0].FindControl("btn_CoreRemoveNew");

                Label lblcorename = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblcorename");
                Label lblcorePosition = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblcorePosition");
                Label lblCoreMemberProfile = (Label)grvCoreMember.Rows[i].Cells[0].FindControl("lblCoreMemberProfile");


                txtcorename.Visible = btn_CoreRemoveNew.Visible = false;
                txtcorePosition.Visible = false;
                txtCoreMembersProfile.Visible = false;


                lblcorename.Visible = true;
                lblcorePosition.Visible = true;
                lblCoreMemberProfile.Visible = true;

            }



            pnl_ContactAddNew.Visible = false;
            for (int i = 0; i < gv_CONTACT_DETAIL.Rows.Count; i++)
            {
                DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");
                TextBox txtContactLast_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactLast_name");
                TextBox txtContactFirst_name = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactFirst_name");
                TextBox txtContactPostition = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactPostition");
                TextBox txtContactNoHome = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactNoHome");
                TextBox txtContactEmail = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                TextBox txtContactAddress = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                ImageButton btn_ContactRemoveNew = (ImageButton)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("btn_ContactRemoveNew");

                Salutation.Visible = txtContactLast_name.Visible = txtContactFirst_name.Visible = txtContactPostition.Visible =
txtContactNoHome.Visible = txtContactEmail.Visible = txtContactAddress.Visible = btn_ContactRemoveNew.Visible = false;


                Label lblSalutation = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblSalutation");
                Label lblContactLast_name = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactLast_name");
                Label lblContactFirst_name = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactFirst_name");
                Label lblContactPostition = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactPostition");
                Label lblContactNoHome = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactNoHome");
                Label lblContactEmail = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactEmail");
                Label lblContactAddress = (Label)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("lblContactAddress");
                lblSalutation.Visible = lblContactLast_name.Visible = lblContactFirst_name.Visible = lblContactPostition.Visible = lblContactNoHome.Visible = lblContactEmail.Visible = lblContactAddress.Visible = true;
            }


        }


        protected TB_CASP_APPLICATION GetExistingApplication(CyberportEMS_EDM dbContext, int programId)
        {
            TB_CASP_APPLICATION objApp = null;
            List<TB_CASP_APPLICATION> objApps = new List<TB_CASP_APPLICATION>();
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
                    objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && x.CASP_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                    //if (objApps != null)
                    //{
                    //    objApps = dbContext.TB_CASP_APPLICATION.Where(x => x.Programme_ID == programId && !string.IsNullOrEmpty(x.Application_Parent_ID) && x.Application_Parent_ID.ToLower() == objUserProgramId.ToString().ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                    //    if (objApps.Count > 0)
                    //    {
                    //        objApp = objApps.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                    //    }
                    //    else objApp = dbContext.TB_CASP_APPLICATION.OrderByDescending(x => x.Modified_Date).FirstOrDefault(x => x.Programme_ID == programId && x.CASP_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                    //}
                    //else
                    //{
                    //    pnlRestricted.Visible = true;
                    //}
                    if (objApps == null)
                        Context.Response.Redirect("~/SitePages/Home.aspx");
                    else if (objApp.Status.ToLower() == "saved")
                    {
                        btn_Submit.Enabled = false;
                    }

                }
                else
                {
                    IsApplicantUser = true;
                    objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.CASP_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());


                    //if (Context.Request.QueryString["new"] == "Y")
                    //{


                    //}
                    //else
                    //{
                    //    objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.CASP_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                    //    if (objApp != null)
                    //    {
                    //        if (objTB_PROGRAMME_INTAKE.Application_Deadline < DateTime.Now)
                    //        {
                    //            objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (string.IsNullOrEmpty(x.Application_Parent_ID)) && (x.Created_By.ToLower() == strCurrentUser) && !(x.Status == formsubmitaction.Saved.ToString() || x.Status == formsubmitaction.Deleted.ToString()));
                    //        }
                    //        else
                    //        {

                    //            objApps = dbContext.TB_CASP_APPLICATION.Where(x => x.Programme_ID == programId && !string.IsNullOrEmpty(x.Application_Parent_ID) && x.Created_By.ToLower() == strCurrentUser.ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                    //            if (objApps.Count > 0)
                    //            {
                    //                objApp = objApps.OrderByDescending(x => Convert.ToDecimal(x.Version_Number)).FirstOrDefault();
                    //            }
                    //            else
                    //                objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());
                    //        }

                    //    }
                    //    else
                    //    {
                    //        //DisableControls();
                    //        pnlRestricted.Visible = true;
                    //    }
                    //}

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                    objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);


                objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId && x.CASP_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());
                //UserCollaborator();

            }
            if (objApp != null)
            {
                hdn_ApplicationID.Value = objApp.CASP_ID.ToString();
            }
            return objApp;
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
                    TB_CASP_APPLICATION objApp = GetExistingApplication(dbContext, progId);
                    if (objApp == null)
                    {
                        if (check_db_validations(false) == 0)
                        {
                            objApp = GetExistingApplication(dbContext, progId);

                        }
                    }
                    if (objApp != null)
                    {

                        //SaveAttachment(fu_BrCopy, enumAttachmentType.BR_COPY, objIncubation.CCMF_ID, progId);
                        SPFunctions objSPFunctions = new SPFunctions();

                        string _fileUrl = string.Empty;
                        switch (Convert.ToInt32(argument))
                        {
                            case 1:
                                if (fu_Company_Ownership_2.HasFile)
                                {
                                    if (fu_Company_Ownership_2.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_Company_Ownership_2.FileName.Remove(0, fu_Company_Ownership_2.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {

                                            _fileUrl = objSPFunctions.AttachmentSave(objApp.Application_No, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                    fu_Company_Ownership_2, enumAttachmentType.Company_Ownership_Structure, objApp.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Company_Ownership_Structure, objApp.CASP_ID, objApp.Programme_ID);
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
                            case 2:
                                if (FuAdmissionRecord.HasFile)
                                {
                                    if (FuAdmissionRecord.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = FuAdmissionRecord.FileName.Remove(0, FuAdmissionRecord.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {

                                            _fileUrl = objSPFunctions.AttachmentSave(objApp.Application_No, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                    FuAdmissionRecord, enumAttachmentType.Accelerator_Admission_Record, objApp.Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Accelerator_Admission_Record, objApp.CASP_ID, objApp.Programme_ID);
                                            lbladmissionrecord.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lbladmissionrecord.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lbladmissionrecord.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    lbladmissionrecord.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 3:
                                //if (fuPresentationSlide.HasFile)
                                //{
                                //    if (fuPresentationSlide.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                //    {
                                //        string Extension = fuPresentationSlide.FileName.Remove(0, fuPresentationSlide.FileName.LastIndexOf(".") + 1);
                                //        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                //            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                //        {

                                //            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                //            fuPresentationSlide, enumAttachmentType.Presentation_Slide, objIncubation.Version_Number);
                                //            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Presentation_Slide, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                //            PresentationSlide.Text = "";
                                //            InitializeUploadsDocument();
                                //        }
                                //        else
                                //        {
                                //            IsError = true;
                                //            PresentationSlide.Text = Localize("File_type_presentation");
                                //        }
                                //    }
                                //    else
                                //    {
                                //        IsError = true;
                                //        PresentationSlide.Text = Localize("File_size");
                                //    }
                                //}
                                //else
                                //{
                                //    IsError = true;
                                //    PresentationSlide.Text = Localize("Error_file_upload");
                                //}
                                break;
                            case 4:
                                //if (fuOtherAttachement.HasFile)
                                //{
                                //    if (fuOtherAttachement.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                //    {
                                //        string Extension = fuOtherAttachement.FileName.Remove(0, fuOtherAttachement.FileName.LastIndexOf(".") + 1);
                                //        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                //            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                //            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                //        {
                                //            List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objIncubation.Incubation_ID).ToList();
                                //            attachments = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Other_Attachment.ToString().ToLower()).ToList();

                                //            _fileUrl = objSPFunctions.AttachmentSave(objIncubation.Application_Number, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                                //        fuOtherAttachement, enumAttachmentType.Other_Attachment, attachments.Count().ToString());
                                //            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Other_Attachment, objIncubation.Incubation_ID, objIncubation.Programme_ID);
                                //            OtherAttachement.Text = "";
                                //            InitializeUploadsDocument();
                                //        }
                                //        else
                                //        {
                                //            IsError = true;
                                //            OtherAttachement.Text = Localize("File_type");
                                //        }
                                //    }
                                //    else
                                //    {
                                //        IsError = true;
                                //        OtherAttachement.Text = Localize("File_size");
                                //    }
                                //}
                                //else
                                //{
                                //    IsError = true;
                                //    OtherAttachement.Text = Localize("Error_file_upload");
                                //}
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

        //protected void ReSubmitVersionCopy()
        //{

        //    try
        //    {
        //        using (var dbContext = new CyberportEMS_EDM())
        //        {
        //            int progId = Convert.ToInt32(hdn_ProgramID.Value);
        //            SPFunctions objfunction = new SPFunctions();
        //            TB_CASP_APPLICATION objApp = GetExistingApplication(dbContext, progId);
        //            TB_CASP_APPLICATION objAppNew = objApp;
        //            Guid objinbubationExitingId = objApp.CASP_ID;
        //            dbContext.TB_CASP_APPLICATION.Add(objAppNew);
        //            objAppNew.Submitted_Date = null;
        //            objAppNew.Modified_Date = DateTime.Now;
        //            objAppNew.Last_Submitted = DateTime.Now;
        //            //objIncubationNew.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal("1")).ToString("F2");
        //            objAppNew.Status = "Saved";
        //            objAppNew.Application_Parent_ID = !string.IsNullOrEmpty(objApp.Application_Parent_ID) ? objAppNew.Application_Parent_ID : objAppNew.CASP_ID.ToString();
        //            objAppNew.CASP_ID = NewProgramId();
        //            dbContext.Entry(objAppNew).State = System.Data.Entity.EntityState.Added;


        //            List<TB_APPLICATION_ATTACHMENT> objIncubationAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objApp.Programme_ID).ToList();
        //            dbContext.TB_APPLICATION_ATTACHMENT.AddRange(objIncubationAttachement);
        //            foreach (TB_APPLICATION_ATTACHMENT objAttach in objIncubationAttachement)
        //            {
        //                objAttach.Application_ID = objAppNew.CASP_ID;
        //            }
        //            //Enable later
        //            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objApp.Programme_ID).ToList();
        //            dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.AddRange(objTB_APPLICATION_COMPANY_CORE_MEMBER);
        //            foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objAttach in objTB_APPLICATION_COMPANY_CORE_MEMBER)
        //            {
        //                objAttach.Application_ID = objAppNew.CASP_ID;
        //            }
        //            List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objApp.Programme_ID).ToList();
        //            dbContext.TB_APPLICATION_CONTACT_DETAIL.AddRange(objTB_APPLICATION_CONTACT_DETAIL);
        //            foreach (TB_APPLICATION_CONTACT_DETAIL objAttach in objTB_APPLICATION_CONTACT_DETAIL)
        //            {
        //                objAttach.Application_ID = objAppNew.CASP_ID;
        //            }
        //            List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objinbubationExitingId && x.Programme_ID == objApp.Programme_ID).ToList();
        //            dbContext.TB_APPLICATION_FUNDING_STATUS.AddRange(objTB_APPLICATION_FUNDING_STATUS);
        //            foreach (TB_APPLICATION_FUNDING_STATUS objAttach in objTB_APPLICATION_FUNDING_STATUS)
        //            {
        //                objAttach.Application_ID = objAppNew.CASP_ID;
        //            }


        //            dbContext.SaveChanges();
        //            InitializeUploadsDocument();

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
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
        private void InitializeUploadsDocument()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_CASP_APPLICATION ObjApp = GetExistingApplication(dbContext, progId);
                    if (ObjApp != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == ObjApp.CASP_ID).ToList();
                        rptrcompanyownership.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Company_Ownership_Structure.ToString().ToLower());
                        rptrcompanyownership.DataBind();
                        rptradmission.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Accelerator_Admission_Record.ToString().ToLower());
                        rptradmission.DataBind();

                    }
                }
            }
            catch (Exception e)
            {

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

        protected void rptrOtherAttachement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    TB_CASP_APPLICATION objApp = GetExistingApplication(dbContext, progId);

                    SPFunctions objSp = new SPFunctions();
                    string AccessUser = lblApplicant.Text.Trim();
                    string CurrentUser = objSp.GetCurrentUser();
                    if (objApp.Applicant.ToLower() != objSp.GetCurrentUser().ToLower() && objApp.Modified_By.ToLower() != objSp.GetCurrentUser().ToLower())
                    {
                        HyperLink hypNavigation = (HyperLink)e.Item.FindControl("hypNavigation");
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        hypNavigation.Enabled = false;
                        lnkAttachmentDelete.Enabled = false;

                    }

                    if (((objApp.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) && (objApp.Status.Replace("_", " ") != formsubmitaction.Saved.ToString().Replace("_", " "))) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))

                    {
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        lnkAttachmentDelete.Visible = false;

                    }
                }
            }
        }


        #region Core Member
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
        protected void ButtonAddCoreMembers_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddNewCoreMemberRow();
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
                    TextBox txtcorename = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorename");
                    TextBox txtcorePosition = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorePosition");
                    TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMemberProfile");

                    TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                    objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                    objMember.Name = txtcorename.Text;
                    objMember.Position = txtcorePosition.Text;
                    objMember.CoreMember_Profile = txtCoreMembersProfile.Text;
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
                            TextBox TextBoxAddress = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorename");
                            TextBox txtNOE = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorePosition");
                            TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMemberProfile");
                            TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                            objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);
                            objMember.Name = TextBoxAddress.Text;
                            objMember.Position = txtNOE.Text;
                            objMember.CoreMember_Profile = txtCoreMembersProfile.Text;
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

        private List<TB_APPLICATION_COMPANY_CORE_MEMBER> GetCoreMemberForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            List<TB_APPLICATION_COMPANY_CORE_MEMBER> objCoreMembers = new List<TB_APPLICATION_COMPANY_CORE_MEMBER>();
            for (int i = 0; i < grvCoreMember.Rows.Count; i++)
            {
                string titleerror = "Core member" + (i + 1) + " : ";

                try
                {

                    HiddenField Core_Member_ID = (HiddenField)grvCoreMember.Rows[i].Cells[0].FindControl("Core_Member_ID");
                    TextBox txtcorename = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorename");
                    TextBox txtcorePosition = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtcorePosition");
                    TextBox txtCoreMembersProfile = (TextBox)grvCoreMember.Rows[i].Cells[0].FindControl("txtCoreMemberProfile");

                    if (txtcorename.Text != "" || txtcorePosition.Text != "" || txtCoreMembersProfile.Text != "" || txtCoreMembersProfile.Text != "")
                    {
                        TB_APPLICATION_COMPANY_CORE_MEMBER objMember = new TB_APPLICATION_COMPANY_CORE_MEMBER();
                        objMember.Core_Member_ID = Convert.ToInt32(Core_Member_ID.Value);

                        if ((txtcorename.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtcorename.Text))
                            || (IsSubmitClick && txtcorename.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtcorename.Text)))
                            objerror.Add(titleerror + Localize("Core_member_name"));
                        else objMember.Name = txtcorename.Text;


                        if ((txtcorePosition.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtcorePosition.Text))
                            || (IsSubmitClick && txtcorePosition.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtcorePosition.Text)))
                            objerror.Add(titleerror + Localize("Core_member_position"));
                        else objMember.Position = txtcorePosition.Text;

                        if ((txtCoreMembersProfile.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtCoreMembersProfile.Text))
                           || (IsSubmitClick && txtCoreMembersProfile.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 500, true, AllowAllSymbol: true), txtCoreMembersProfile.Text)))
                            objerror.Add(titleerror + Localize("Core_MembersProfile"));
                        else objMember.CoreMember_Profile = txtCoreMembersProfile.Text;
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

        #endregion


        #region Funding Status
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

        protected void btn_FundingAddNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddNewFundingStatus();

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

                    TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                    TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                    TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
                    DropDownList Currency = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("Currency");

                    DropDownList ddlApplication = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlApplication");
                    DropDownList ddlFunding = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlFunding");

                    TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                    objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                    objMember.Programme_Name = txtNameofProgram.Text;
                    objMember.Date = Convert.ToDateTime(txtApplicationDate.Text);
                    objMember.Funding_Status = ddlFunding.SelectedValue;
                    objMember.Application_Status = ddlApplication.SelectedValue;
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
        protected void Grd_FundingStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hdnCurrency = ((HiddenField)e.Row.FindControl("hdnCurrency")).Value;
                string hdnApplicationStatus = ((HiddenField)e.Row.FindControl("hdnApplicationStatus")).Value;
                string hdnFunding = ((HiddenField)e.Row.FindControl("hdnFunding")).Value;
                DropDownList ddlApplication = (DropDownList)e.Row.FindControl("ddlApplication");
                DropDownList ddlFunding = (DropDownList)e.Row.FindControl("ddlFunding");

                List<ListItem> applicationstatus = new List<ListItem>();
                applicationstatus.Add(new ListItem() { Value = "Pending for result", Text = Localize("AppStatus_Pending") });
                applicationstatus.Add(new ListItem() { Value = "Successful", Text = Localize("AppStatus_Success") });
                applicationstatus.Add(new ListItem() { Value = "Being Declined", Text = Localize("AppStatus_Declined") });
                ddlApplication.DataSource = applicationstatus;
                ddlApplication.DataTextField = "Text";
                ddlApplication.DataValueField = "Value";
                ddlApplication.DataBind();
                ddlApplication.SelectedValue = hdnApplicationStatus;

                List<ListItem> fundingStatus = new List<ListItem>();
                fundingStatus.Add(new ListItem() { Value = "Pending for process", Text = Localize("FundStatus_Pending") });
                fundingStatus.Add(new ListItem() { Value = "Received", Text = Localize("FundStatus_Received") });
                fundingStatus.Add(new ListItem() { Value = "Being Declined", Text = Localize("FundStatus_Declined") });
                ddlFunding.DataSource = fundingStatus;
                ddlFunding.DataTextField = "Text";
                ddlFunding.DataValueField = "Value";
                ddlFunding.DataBind();

                ddlFunding.SelectedValue = hdnFunding;
                DropDownList Currency = (DropDownList)e.Row.FindControl("Currency");
                Currency.SelectedValue = hdnCurrency;

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

                            TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                            TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                            TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");

                            DropDownList ddlApplication = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlApplication");
                            DropDownList ddlFunding = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlFunding");

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
                            objMember.Funding_Status = ddlFunding.SelectedValue;
                            objMember.Application_Status = ddlApplication.SelectedValue;
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

                    TextBox txtNature = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtNature");
                    TextBox txtAmountReceived = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtAmountReceived");
                    TextBox txtApplicationMaximumAmount = (TextBox)Grd_FundingStatus.Rows[i].Cells[0].FindControl("txtApplicationMaximumAmount");
                    DropDownList ddlApplication = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlApplication");
                    DropDownList ddlFunding = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("ddlFunding");


                    DropDownList Currency = (DropDownList)Grd_FundingStatus.Rows[i].Cells[0].FindControl("Currency");
                    if (txtNameofProgram.Text != "" || txtNature.Text != "" || txtAmountReceived.Text != "" || txtApplicationMaximumAmount.Text != "")
                    {
                        TB_APPLICATION_FUNDING_STATUS objMember = new TB_APPLICATION_FUNDING_STATUS();
                        objMember.Funding_ID = Convert.ToInt32(FundingID.Value);
                        if ((txtNameofProgram.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNameofProgram.Text))
                            || (IsSubmitClick && txtNameofProgram.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNameofProgram.Text)))
                            objErrors.Add(titleerror + Localize("Funding_Programme_name"));

                        else objMember.Programme_Name = txtNameofProgram.Text;



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

                        objMember.Application_Status = ddlApplication.SelectedValue;
                        objMember.Funding_Status = ddlFunding.SelectedValue;

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
        #endregion

        #region Contact

        protected void InitializeContact()
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
                    TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                    TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                    DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");


                    TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                    objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                    objMember.Last_Name_Eng = txtContactLast_name.Text;
                    objMember.First_Name_Eng = txtContactFirst_name.Text;
                    objMember.Position = Position.Text;
                    objMember.Contact_No = Contact_No_Home.Text;
                    objMember.Mailing_Address = Mailing_Address.Text;
                    objMember.Salutation = Salutation.Text;
                    objMember.Email = Email.Text;
                    objContactDetails.Add(objMember);




                }
                catch (Exception ex)
                {
                    IsError = true;
                    lblcontactdetails.Text = "Please fill in Last Name, First Name, Saluation, Position, Contact No., Email and Mailing Address in " + (i + 1).ToString() + "  Contact Details, or leave all fields blank to remove the contact. ";
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
                            TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                            TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                            DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");


                            TB_APPLICATION_CONTACT_DETAIL objMember = new TB_APPLICATION_CONTACT_DETAIL();
                            objMember.CONTACT_DETAILS_ID = Convert.ToInt32(contactId.Value);
                            objMember.Last_Name_Eng = txtContactLast_name.Text;
                            objMember.First_Name_Eng = txtContactFirst_name.Text;
                            objMember.Position = Position.Text;
                            objMember.Contact_No = Contact_No_Home.Text;
                            objMember.Mailing_Address = Mailing_Address.Text;
                            objMember.Salutation = Salutation.Text;
                            objMember.Email = Email.Text;
                            objContactDetails.Add(objMember);


                        }

                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        lblcontactdetails.Text = "Please fill in Last Name, First Name, Saluation, Position, Contact No.,Email and Mailing Address in " + (i + 1).ToString() + "  Contact Details, or leave all fields blank to remove the contact. ";
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
                    TextBox Email = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactEmail");
                    TextBox Mailing_Address = (TextBox)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("txtContactAddress");
                    DropDownList Salutation = (DropDownList)gv_CONTACT_DETAIL.Rows[i].Cells[0].FindControl("Salutation");

                    if (txtContactLast_name.Text != "" || txtContactFirst_name.Text != "" || Position.Text != "" || Contact_No_Home.Text != "" || Email.Text != "" || Mailing_Address.Text != "")
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
                        else objMember.Contact_No = Contact_No_Home.Text;

                        objMember.Mailing_Address = Mailing_Address.Text;
                        objMember.Salutation = Salutation.Text;

                        if (Email.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), Email.Text))
                            objerror.Add(titleerror + Localize("Contact_Email"));
                        else objMember.Email = Email.Text;
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
        protected void gv_CONTACT_DETAIL_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hdnSalutation = ((HiddenField)e.Row.FindControl("hdnSalutation")).Value;
                //hdnSalutation
                DropDownList Salutation = (DropDownList)e.Row.FindControl("Salutation");
                Salutation.SelectedValue = hdnSalutation;

            }
        }
        #endregion


        protected void btn_StepSave_Click(object sender, EventArgs e)
        {
            lbl_Exception.InnerHtml = "";
            lblgrouperror.Visible = false;
            int ActiveStep = Convert.ToInt16(hdn_ActiveStep.Value);
            if (ActiveStep > 0)
            {
                quicklnk_1.CssClass = "";
                quicklnk_2.CssClass = "";
                quicklnk_3.CssClass = "";
                quicklnk_4.CssClass = "";

                for (int i = ActiveStep; i > 0; i--)
                {
                    ((LinkButton)(this.FindControl("quicklnk_" + i))).CssClass = "active";
                }
            }
            check_db_validations(false);
            //enable later ShowHideControlsBasedUponUserData();

        }
        protected void btn_Submit_Click1(object sender, EventArgs e)
        {
            int progId = Convert.ToInt32(Context.Request.QueryString["prog"]);
            int errors = check_db_validations(true);
            if (errors == 0)
            {
                if (!SubmitValidationError())
                {
                    UserSubmitPasswordPopup.Visible = true;
                    SPFunctions objFUnction = new SPFunctions();
                    txtLoginUserName.Text = objFUnction.GetCurrentUser();
                    //lblSubmissionApplication.Text = objProgram.Intake_Number.ToString();
                }
                else
                {
                    lblgrouperror.Visible = true;
                }
            }
            //enable later ShowHideControlsBasedUponUserData();
        }

        /// <summary>
        /// Submit From Popup Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_submitFinal_Click(object sender, EventArgs e)
        {

            try
            {
                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtLoginUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                {
                    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtLoginUserName.Text, txtLoginPassword.Text);

                    if (!status)
                    {
                        UserSubmitPasswordPopup.Visible = true;
                        UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                    }
                    else
                    {
                        using (var dbContext = new CyberportEMS_EDM())
                        {
                            int progId = Convert.ToInt32(hdn_ProgramID.Value);
                            TB_PROGRAMME_INTAKE objIntake = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == progId).ToList().FirstOrDefault();
                            Guid appId = Guid.Parse(hdn_ApplicationID.Value);
                            TB_CASP_APPLICATION objApp = GetExistingApplication(dbContext, progId);

                            if (objApp != null)
                            {
                                bool isrequestor = false;
                                if (objApp.Application_Parent_ID == null)
                                {
                                    if (objApp.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                    {
                                        objApp.Status = "Resubmitted information";//formsubmitaction.Resubmitted_information.ToString().Replace("_", " ");
                                        isrequestor = true;
                                    }
                                    else
                                    {
                                        objApp.Status = formsubmitaction.Submitted.ToString();
                                        objApp.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objApp.Version_Number)) + Convert.ToDecimal("1")).ToString("F2");
                                        isrequestor = false;
                                    }
                                    objApp.Modified_Date = DateTime.Now;
                                    objApp.Last_Submitted = DateTime.Now;
                                    //objIncubation.Status = "Submitted";
                                    objApp.Submitted_Date = DateTime.Now;
                                }
                                else
                                {
                                    dbContext.TB_CASP_APPLICATION.Remove(objApp);
                                    Guid ParentId = Guid.Parse(objApp.Application_Parent_ID);
                                    TB_CASP_APPLICATION objAppOld = GetDatabyParentId(dbContext, ParentId);
                                    objAppOld.Version_Number = (Decimal.Truncate(Convert.ToDecimal(objApp.Version_Number)) + Convert.ToDecimal("1")).ToString("F2");
                                    objAppOld.Applicant = objApp.Applicant;
                                    objAppOld.Created_By = objApp.Created_By;
                                    objAppOld.Created_Date = objApp.Created_Date;
                                    objAppOld.Last_Submitted = DateTime.Now;
                                    objAppOld.Modified_By = objAppOld.Modified_By;
                                    objAppOld.Submitted_Date = DateTime.Now;
                                    objAppOld.Status = "Submitted"; //oldobjIncubation.Status = objIncubation.Status;

                                    //objAppOld.Abstract = objApp.Abstract;
                                    //objAppOld.Declaration = objApp.Declaration;
                                    //objAppOld.Background = objApp.Background;



                                    dbContext.SaveChanges();

                                    List<TB_APPLICATION_ATTACHMENT> objAppAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_ATTACHMENT.RemoveRange(objAppAttachement);

                                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.RemoveRange(objTB_APPLICATION_COMPANY_CORE_MEMBER);

                                    List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_CONTACT_DETAIL.RemoveRange(objTB_APPLICATION_CONTACT_DETAIL);

                                    List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == ParentId).ToList();
                                    dbContext.TB_APPLICATION_FUNDING_STATUS.RemoveRange(objTB_APPLICATION_FUNDING_STATUS);
                                    List<TB_APPLICATION_ATTACHMENT> addobjAppAttachement = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_ID && x.Programme_ID == objApp.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_ATTACHMENT.AddRange(addobjAppAttachement);
                                    foreach (TB_APPLICATION_ATTACHMENT objAttach in addobjAppAttachement)
                                    {
                                        objAttach.Application_ID = objAppOld.CASP_ID;
                                    }
                                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> addobjTB_APPLICATION_COMPANY_CORE_MEMBER = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objApp.CASP_ID && x.Programme_ID == objApp.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.AddRange(addobjTB_APPLICATION_COMPANY_CORE_MEMBER);
                                    foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objAttach in addobjTB_APPLICATION_COMPANY_CORE_MEMBER)
                                    {
                                        objAttach.Application_ID = objAppOld.CASP_ID;
                                    }
                                    List<TB_APPLICATION_CONTACT_DETAIL> addobjTB_APPLICATION_CONTACT_DETAIL = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objApp.CASP_ID && x.Programme_ID == objApp.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_CONTACT_DETAIL.AddRange(addobjTB_APPLICATION_CONTACT_DETAIL);
                                    foreach (TB_APPLICATION_CONTACT_DETAIL objAttach in addobjTB_APPLICATION_CONTACT_DETAIL)
                                    {
                                        objAttach.Application_ID = objAppOld.CASP_ID;
                                    }
                                    List<TB_APPLICATION_FUNDING_STATUS> addobjTB_APPLICATION_FUNDING_STATUS = dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objApp.CASP_ID && x.Programme_ID == objApp.Programme_ID).ToList();
                                    dbContext.TB_APPLICATION_FUNDING_STATUS.AddRange(addobjTB_APPLICATION_FUNDING_STATUS);
                                    foreach (TB_APPLICATION_FUNDING_STATUS objAttach in addobjTB_APPLICATION_FUNDING_STATUS)
                                    {
                                        objAttach.Application_ID = objAppOld.CASP_ID;
                                    }
                                }
                                dbContext.SaveChanges();
                                string requestor = "";
                                string strEmailContent = "";
                                string strEmailsubject = "";

                                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                                objTbParams = dbContext.TB_SYSTEM_PARAMETER;

                                TB_PROGRAMME_INTAKE oIntake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == objApp.Programme_ID);
                                string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                                WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;

                                string applicationType = "CASPProgram.aspx";
                                string token = "/SitePages/" + applicationType + "?prog=" + objApp.Programme_ID + "&app=" + objApp.CASP_ID;

                                if (isrequestor == true)
                                {

                                    List<TB_SCREENING_HISTORY> objTB_SCREENING_HISTORY1 = new List<TB_SCREENING_HISTORY>();
                                    TB_SCREENING_HISTORY objTB_SCREENING_HISTORY = new TB_SCREENING_HISTORY();
                                    objTB_SCREENING_HISTORY1 = dbContext.TB_SCREENING_HISTORY.OrderByDescending(x => x.Created_Date).ToList();
                                    objTB_SCREENING_HISTORY = objTB_SCREENING_HISTORY1.FirstOrDefault(x => x.Application_Number == objApp.Application_No && x.Programme_ID == objApp.Programme_ID);

                                    requestor = objApp.Created_By;

                                    strEmailContent = CBPEmail.GetEmailTemplate("Requestor_Applicant_Resubmitted");
                                    strEmailContent = strEmailContent.Replace("@@intakeno", oIntake.Intake_Number.ToString()).Replace("@@IntakeNumber", oIntake.Intake_Number.ToString());
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted_Requestor").Replace("@@Applicationnumber", objApp.Application_No);
                                    strEmailsubject = strEmailsubject.Replace("@@ProgramName", oIntake.Programme_Name);


                                    int IsEmailSent = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);

                                    //requestor = objTB_SCREENING_HISTORY.Created_By;

                                    //strEmailContent = CBPEmail.GetEmailTemplate("Application_Updated_Coordinator");
                                    //strEmailContent = strEmailContent.Replace("@@AppNumber", objIncubation.Application_Number);
                                    //strEmailContent = strEmailContent.Replace("@@Comment", objTB_SCREENING_HISTORY.Comment_For_Applicants);
                                    //strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    //strEmailsubject = LocalizeCommon("Mail_App_Resubmitted_Coordinator").Replace("@@Applicationnumber", objIncubation.Application_Number);

                                }
                                else
                                {
                                    requestor = objApp.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("Application_Applicant_Submitted");

                                    strEmailContent = strEmailContent.Replace("@@intakeno", oIntake.Intake_Number.ToString()).Replace("@@IntakeNumber", oIntake.Intake_Number.ToString());
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objApp.Application_No);
                                    int IsEmailed = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);
                                }

                                UserSubmitPasswordPopup.Visible = false;
                                pnlsubmissionpopup.Visible = true;
                                if (isrequestor == true)
                                {
                                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Re-submitted");
                                }
                                else
                                {
                                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Submitted");
                                }
                                Fill_Programelist(objApp.Application_No, objApp.Programme_ID, objIntake.Intake_Number, objApp.Version_Number, objApp.Status, objApp.Applicant);

                            }
                            else
                            {
                                UserSubmitPasswordPopup.Visible = true;
                                UserCustomerrorLogin.InnerHtml = Localize("Releventdata_error");
                            }

                        }
                    }
                }
                else
                {
                    UserSubmitPasswordPopup.Visible = true;
                    UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                }
            }
            catch (Exception ex)
            {
                UserSubmitPasswordPopup.Visible = true;
                UserCustomerrorLogin.InnerText = ex.Message;
            }
        }

        protected int check_db_validations(bool IsSubmitClick)
        {

            List<String> ErrorLIst = new List<string>();
            using (var dbContext = new CyberportEMS_EDM())
            {

                int progId = Convert.ToInt32(hdn_ProgramID.Value);

                SPFunctions objfunction = new SPFunctions();
                TB_CASP_APPLICATION objApp = GetExistingApplication(dbContext, progId);
                TB_PROGRAMME_INTAKE objprogram = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(k => k.Programme_ID == progId);
                bool isnewobj = false;
                if (objApp == null)
                {
                    isnewobj = true;
                    objApp = new TB_CASP_APPLICATION();
                }
                //else if (objApp.Submitted_Date.HasValue && objApp.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                //{
                //   // ReSubmitVersionCopy();
                //    objApp = GetExistingApplication(dbContext, progId);
                //}


                if (ddl_companyProjects.SelectedValue == "")
                {
                    ErrorLIst.Add(Localize("Error_CompanySelect"));
                }
                else
                {
                    string Company = ddl_companyProjects.SelectedValue;
                    objApp.Company_Project = Company.Split(':')[0];
                    objApp.CCMF_CPIP_App_No = Company.Split(':')[1];
                }

                objApp.Company_Address = txtCompanyRegAdd.Text.Trim();

                objApp.Abstract = txtabstract.Text.Trim();
                objApp.Company_Ownership_Structure = txtownership.Text.Trim();

                List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = GetCoreMemberForSave(IsSubmitClick, ref ErrorLIst);
                List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = GetFundingStatusForSave(IsSubmitClick, ref ErrorLIst);
                List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = GetContactDetailsForSave(IsSubmitClick, ref ErrorLIst);

                if (WordCount(txtAdditionalInformation.Text.Trim()) > 300)
                    ErrorLIst.Add(Localize("Error_Additional_Information_length"));
                else objApp.Additional_Info = txtAdditionalInformation.Text.Trim();

                if (txtprogramme.Text.Trim().Length > 255)
                    ErrorLIst.Add(Localize("Error_ProgramNameLength"));
                else objApp.Accelerator_Name = txtprogramme.Text.Trim();
                if (!string.IsNullOrEmpty(rbtnProgrammeEndorsed.SelectedValue))
                    objApp.Endorsed_by_Cyberport = Convert.ToBoolean(rbtnProgrammeEndorsed.SelectedValue);

                if (!string.IsNullOrEmpty(txtcommencementdate.Text.Trim()))
                {
                    DateTime dDate;

                    if (DateTime.TryParse(txtcommencementdate.Text.Trim(), out dDate))
                    {
                        objApp.Commencement_Date = Convert.ToDateTime(txtcommencementdate.Text.Trim());
                    }
                    else
                    {

                        ErrorLIst.Add(Localize("Error_Commencement_Date_Invalid"));

                    }
                }


                if ((txtprogrammeduration.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.IntergerValue, txtprogrammeduration.Text)))
                    ErrorLIst.Add(Localize("Error_Program_Duration_Type"));
                else if (!string.IsNullOrEmpty(txtprogrammeduration.Text))
                    objApp.Duration = Convert.ToInt32(txtprogrammeduration.Text);

                if (WordCount(txtbackground.Text.Trim()) > 300)
                    ErrorLIst.Add(Localize("Error_General_Background"));
                else objApp.Background = txtbackground.Text.Trim();

                objApp.Offer = txtoffering.Text.Trim();
                objApp.Fund_Raising_Capabilities = txtfundraising.Text.Trim();
                objApp.Size_of_Alumni = txtalumnisize.Text.Trim();
                objApp.Reputation = txtreputation.Text.Trim();

                if ((txtwebsite.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.Url, txtwebsite.Text)))
                    ErrorLIst.Add(Localize("Error_Website_Format"));
                else if (!string.IsNullOrEmpty(txtwebsite.Text))
                    objApp.Website = txtwebsite.Text.Trim();

                objApp.Declaration = chkDeclaration.Checked;
                objApp.Principle_Full_Name = txtPrinciple_Full_Name.Text.Trim();

                if (objApp.Principle_Full_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Principle_Full_Name))
                    ErrorLIst.Add(Localize("Error_Principal_title_length"));

                objApp.Principle_Title = txtPrinciple_Title.Text.Trim();
                if (objApp.Principle_Title.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Principle_Title))
                    ErrorLIst.Add(Localize("Error_principal_position_length"));

                try
                {
                    objApp.Last_Submitted = DateTime.Now;
                    objApp.Modified_By = objfunction.GetCurrentUser();
                    objApp.Modified_Date = DateTime.Now;
                    if (ErrorLIst.Count == 0)
                    {
                        lblgrouperror.Visible = false;
                        if (isnewobj)
                        {

                            objApp.CASP_ID = NewProgramId();
                            hdn_ApplicationID.Value = objApp.CASP_ID.ToString();
                            objApp.Programme_ID = Convert.ToInt32(hdn_ProgramID.Value);

                            int count = 0;
                            int programId = Convert.ToInt32(hdn_ProgramID.Value);
                            var result = dbContext.TB_CASP_APPLICATION.Where(x => x.Programme_ID == programId).OrderByDescending(x => x.Application_No).FirstOrDefault();
                            if (result != null)
                            {
                                count = Convert.ToInt32(result.Application_No.Substring(result.Application_No.Length - 4, 4)) + 1;
                            }
                            else
                            {
                                count = 1;
                            }
                            lblApplicationNo.Text = HttpUtility.HtmlEncode(dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Application_No_Prefix + "-" + dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId).Intake_Number + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                            objApp.Application_No = lblApplicationNo.Text;
                            objApp.Applicant = objfunction.GetCurrentUser();
                            objApp.Status = "Saved";
                            objApp.Version_Number = "0.01";
                            objApp.Created_By = objfunction.GetCurrentUser();
                            objApp.Created_Date = DateTime.Now;
                            dbContext.TB_CASP_APPLICATION.Add(objApp);
                        }
                        else
                        {
                            //objIncubation.Version_Number = (Convert.ToDecimal(objIncubation.Version_Number) + Convert.ToDecimal(".01")).ToString();
                            if (objApp.Status.ToLower().Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                            {
                                objApp.Status = formsubmitaction.Saved.ToString();
                                objApp.Version_Number = (Convert.ToDecimal(objApp.Version_Number) + Convert.ToDecimal(".01")).ToString();
                            }

                        }


                        dbContext.SaveChanges();


                        objApp = GetExistingApplication(dbContext, progId);
                        objTB_APPLICATION_FUNDING_STATUS.ForEach(x => x.Application_ID = objApp.CASP_ID);
                        objTB_APPLICATION_FUNDING_STATUS.ForEach(x => x.Programme_ID = objApp.Programme_ID);
                        IncubationContext.APPLICATION_FUNDING_STATUS_ADDUPDATE(dbContext, objTB_APPLICATION_FUNDING_STATUS, objApp.CASP_ID);
                        objTB_APPLICATION_CONTACT_DETAIL.ForEach(x => x.Application_ID = objApp.CASP_ID);
                        objTB_APPLICATION_CONTACT_DETAIL.ForEach(x => x.Programme_ID = objApp.Programme_ID);
                        IncubationContext.TB_APPLICATION_CONTACTDETAILSADDUPDATE(dbContext, objTB_APPLICATION_CONTACT_DETAIL, objApp.CASP_ID);
                        objTB_APPLICATION_COMPANY_CORE_MEMBER.ForEach(x => x.Application_ID = objApp.CASP_ID);
                        objTB_APPLICATION_COMPANY_CORE_MEMBER.ForEach(x => x.Programme_ID = progId);


                        IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_ADDUPDATE(dbContext, objTB_APPLICATION_COMPANY_CORE_MEMBER, objApp.CASP_ID);




                        dbContext.SaveChanges();
                        objApp = GetExistingApplication(dbContext, progId);
                        InitializeFundingStatus();
                        InitialCoreMembers();
                        InitializeContact();
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
                catch (Exception)
                {

                    throw;
                }
                //enable  ShowHideControlsBasedUponUserData();
            }
            return ErrorLIst.Count;
        }

        protected int WordCount(string text)
        {
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
            return wordCount;
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
                    TB_CASP_APPLICATION objApp = GetExistingApplication(dbContext, progId);
                    lblSubmissionApplication.Text = objApp.Application_No;
                    List<TB_APPLICATION_FUNDING_STATUS> ObjFundingStatus = IncubationContext.APPLICATION_FUNDING_STATUS_GET(objApp.CASP_ID);
                    List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT = IncubationContext.ListofTB_APPLICATION_ATTACHMENTGGet(objApp.CASP_ID, objApp.Programme_ID);
                    List<TB_APPLICATION_COMPANY_CORE_MEMBER> Objcoremembers = IncubationContext.APPLICATION_COMPANY_CORE_MEMBER_GET(objApp.CASP_ID);
                    List<TB_APPLICATION_CONTACT_DETAIL> ObjTB_APPLICATION_CONTACT_DETAIL = IncubationContext.APPLICATION_CONTACT_DETAIL_GET(objApp.CASP_ID);
                    if (objApp != null)
                    {
                        if (string.IsNullOrEmpty(objApp.Company_Address))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Company_Registered_Address"));
                        }

                        if (string.IsNullOrEmpty(objApp.Abstract))
                        {
                            IsError = true;
                            errlist.Add(Localize("Abstract_Error"));
                        }
                        if (string.IsNullOrEmpty(objApp.Company_Ownership_Structure))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Company_structure"));
                        }

                        if (objTB_APPLICATION_ATTACHMENT != null)
                        {
                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Company_Ownership_Structure.ToString().ToLower()))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_company_ownership_structure"));
                            }

                            if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Accelerator_Admission_Record.ToString().ToLower()))
                            {
                                IsError = true;
                                errlist.Add(Localize("Error_Accelerator_Admission_RecordFile"));
                            }

                        }

                        if (Objcoremembers.Count == 0)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_atleast_coremember"));
                        }
                        if (ObjTB_APPLICATION_CONTACT_DETAIL.Count == 0)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_atleast_contact"));
                        }

                        if (string.IsNullOrEmpty(objApp.Accelerator_Name))
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Accelerator_Name"));
                        }

                        if (!objApp.Endorsed_by_Cyberport.HasValue)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Endorsed_by_Cyberport"));
                        }


                        if (!objApp.Commencement_Date.HasValue)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Commencement_Date"));
                        }

                        if (!objApp.Duration.HasValue)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Duration"));
                        }
                        if (objApp.Endorsed_by_Cyberport.HasValue)
                        {
                            if (objApp.Endorsed_by_Cyberport.Value == false)
                            {
                                if (string.IsNullOrEmpty(objApp.Background))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_General_Background"));
                                }
                                if (string.IsNullOrEmpty(objApp.Offer))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Offerings_To_Admitted"));
                                }

                                if (string.IsNullOrEmpty(objApp.Fund_Raising_Capabilities))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Fund_Raising_Capabilities"));
                                }

                                if (string.IsNullOrEmpty(objApp.Size_of_Alumni))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Size_of_Alumni"));
                                }

                                if (string.IsNullOrEmpty(objApp.Reputation))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Reputation"));
                                }

                                if (string.IsNullOrEmpty(objApp.Website))
                                {
                                    IsError = true;
                                    errlist.Add(Localize("Error_Website"));
                                }
                            }
                        }



                        int i = 1;
                        foreach (TB_APPLICATION_CONTACT_DETAIL obj1 in ObjTB_APPLICATION_CONTACT_DETAIL)
                        {
                            if (obj1.First_Name_Eng == "" && obj1.Last_Name_Eng == "" && obj1.Position == ""
                                && obj1.Contact_No == "" && obj1.Email == ""
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

                            if (string.IsNullOrEmpty(obj1.Contact_No))
                            {
                                IsError = true;
                                errlist.Add(string.Format(Localize("Error_Contact_Home"), i.ToString()));
                            }
                            i++;
                        }


                        if (objApp.Declaration == false)
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Declaration"));
                        }
                        if (objApp.Principle_Full_Name == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("Error_Applicant_fullname"));
                        }
                        if (objApp.Principle_Title == "")
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

        protected TB_CASP_APPLICATION GetDatabyParentId(CyberportEMS_EDM dbContext, Guid ParentId)
        {
            TB_CASP_APPLICATION objApp = dbContext.TB_CASP_APPLICATION.OrderBy(x => x.Modified_Date).FirstOrDefault(x => x.CASP_ID == ParentId);
            return objApp;
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
        private void Fill_Programelist(string Application_number, int Programme_Id, int Intake, string version, string status, string Applicant)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPWeb site = SPFunctions.GetCurrentWeb)
                    {
                        site.AllowUnsafeUpdates = true;
                        SPList list = site.Lists["Application List CASP"];

                        SPListItem itemAttachment = GetItemByBdcId(list, Application_number.ToString(), "Application_Number", Programme_Id.ToString(), "Programme_ID");

                        if (itemAttachment == null)
                        {

                            itemAttachment = list.Items.Add();
                        }

                        itemAttachment["Application_Number"] = Application_number;

                        itemAttachment["Programme_ID"] = Programme_Id;
                        itemAttachment["Programme_Name"] = "Cyberport Accelerator Support Programme";
                        itemAttachment["Intake_Number"] = Intake;
                        itemAttachment["Applicant"] = Applicant;
                        itemAttachment["Version_Number"] = version;

                        itemAttachment["Status"] = status;
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








        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            UserSubmitPasswordPopup.Visible = false;
        }
        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;

            Context.Response.Redirect("~/SitePages/Home.aspx", false);


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

        protected void SetPanel1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SetPanelVisibilityOfStep(0);
        }
        protected void btnCASPForm_Click(object sender, EventArgs e)
        {
            SetPanelVisibilityOfStep(1);
        }
        protected void btn_StepPrevious_Click(object sender, EventArgs e)
        {
            lbl_Exception.InnerHtml = "";
            lblgrouperror.Visible = false;
            //enable ShowHideControlsBasedUponUserData();
            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) - 1);
        }
        protected void btn_StepNext_Click(object sender, EventArgs e)
        {


            lblgrouperror.Visible = false;
            //enable ShowHideControlsBasedUponUserData();
            SetPanelVisibilityOfStep(Convert.ToInt16(hdn_ActiveStep.Value) + 1);


        }
        protected void quicklnk_1_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            //enable ShowHideControlsBasedUponUserData();

            SetPanelVisibilityOfStep(Convert.ToInt32(id));


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
            UserSubmitPasswordPopup.Visible = false;
            if (ActiveStep > 0)
            {

                quicklnk_1.CssClass = "";
                quicklnk_2.CssClass = "";
                quicklnk_3.CssClass = "";
                quicklnk_4.CssClass = "";

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

                        pnl_CASPStep1.Visible = false;
                        pnl_CASPStep2.Visible = false;
                        pnl_CASPStep3.Visible = false;
                        pnl_CASPStep4.Visible = false;
                        pnl_programDetail.Visible = false;
                        pnl_Buttons.Visible = false;
                        progressList.Visible = false;


                    }
                    break;
                case 1:
                    {
                        pnl_CASPStep1.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_CASPStep2.Visible = false;
                        pnl_CASPStep3.Visible = false;
                        pnl_CASPStep4.Visible = false;
                    }
                    break;
                case 2:
                    {
                        pnl_CASPStep2.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_CASPStep1.Visible = false;
                        pnl_CASPStep3.Visible = false;
                        pnl_CASPStep4.Visible = false;
                    }
                    break;
                case 3:
                    {
                        pnl_CASPStep3.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_CASPStep2.Visible = false;
                        pnl_CASPStep1.Visible = false;
                        pnl_CASPStep4.Visible = false;
                    }
                    break;
                case 4:
                    {
                        pnl_CASPStep4.Visible = true;

                        pnl_InstructionForm.Visible = false;
                        pnl_CASPStep2.Visible = false;
                        pnl_CASPStep3.Visible = false;
                        pnl_CASPStep1.Visible = false;
                        btn_StepNext.Visible = false;
                        btn_Submit.Visible = true;
                    }
                    break;
                default:
                    break;
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

        protected void ddl_companyProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_companyProjects.SelectedIndex != 0)
            {
                GetCompanydetails(ddl_companyProjects.SelectedValue);
            }
            else
            {
                lblPeriod.Text = string.Empty;
                lblCategory.Text = string.Empty;

            }
        }


        protected void GetCompanydetails(string CompanyProject)
        {
            if (!string.IsNullOrEmpty(CompanyProject))
            {
                string Project_Name = CompanyProject.Split(':')[0];
                string Application_No = CompanyProject.Split(':')[1];


                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_INCUBATION_APPLICATION objcompMap = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Company_Name_Eng.ToLower() == Project_Name.ToLower() && x.Application_Number == Application_No);

                    string type = "";
                    DateTime Period = DateTime.Now;
                    if (objcompMap != null)
                    {
                        type = "CPIP";
                        Period = objcompMap.Created_Date;
                    }
                    else
                    {
                        TB_CCMF_APPLICATION objcompMapCCMF = dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Project_Name_Eng.ToLower() == Project_Name.ToLower() && x.Application_Number == Application_No);
                        if (objcompMapCCMF != null)
                        {
                            type = "CCMF";
                            Period = objcompMapCCMF.Created_Date;
                        }
                    }



                    if (!string.IsNullOrEmpty(type))
                    {
                        lblPeriod.Text = Period.ToString("dd MMM yyyy") + " - " + Period.AddYears(5).ToString("dd MMM yyyy");
                        lblCategory.Text = "Existing" + " " + type;
                    }
                }
            }
            else
            {
                lblPeriod.Text = "";
                lblCategory.Text = "";


            }

        }
    }
}
