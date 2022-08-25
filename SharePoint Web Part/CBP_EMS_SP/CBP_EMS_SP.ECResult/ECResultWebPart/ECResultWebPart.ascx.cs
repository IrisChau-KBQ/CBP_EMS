using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data.Models;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.ECResult.ECResultWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ECResultWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ECResultWebPart()
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
                if (Context.Request.UrlReferrer != null)
                {
                    btn_Cancel.PostBackUrl = Context.Request.UrlReferrer.ToString();

                }
                FillPrograms();
            }

        }
        public void FillPrograms()
        {

            lblgrouperror.Visible = false;

            if (!string.IsNullOrEmpty(Context.Request.QueryString["program_id"]))
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    var program_id = Context.Request.QueryString["program_id"];
                    int programId = Convert.ToInt32(program_id);

                    List<TB_EC_RESULT> objTB_VETTING_APPLICATION = dbContext.TB_EC_RESULT.Where(x => x.Programme_ID == programId).ToList();

                    if (objTB_VETTING_APPLICATION.Count > 0)
                    {
                        //mailarea.Visible = true;
                        if (objTB_VETTING_APPLICATION.Where(x => x.Status == formsubmitaction.Completed.ToString()).Count() == 0)
                        {
                            btn_Confirm.Visible = true;
                        }
                        //btn_CancelIntake.Visible = true;
                    }


                    TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);
                    lblprogramtype.Text = objTB_PROGRAMME_INTAKE.Programme_Name;
                    lblintake.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.Intake_Number);
                    lbldeadline.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.Application_Deadline);
                    lbltotalapplications.Text = Convert.ToString(objTB_VETTING_APPLICATION.Count());
                    var obj = objTB_VETTING_APPLICATION.OrderBy(x => x.Recommended == true);
                    rptrprogrammesummary.DataSource = obj;
                    rptrprogrammesummary.DataBind();

                }
            }
        }
        protected void btn_Confirm_Click1(object sender, EventArgs e)
        {

            List<AwardedApplicationSelected> objSelectedApps = GetAwardedApplicationsWithConflicts();
            pnlCompanyConflicts.Visible = false;

            if (objSelectedApps.Where(x => x.HasConflict == true).Count() > 0)
            {
                pnlCompanyConflicts.Visible = true;
                rptrConflictCompanies.DataSource = objSelectedApps;
                rptrConflictCompanies.DataBind();
            }
            else {
                ShowFinalSubmitPopup();
            }

        }

        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            SubmitPopup.Visible = false;
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Incubation");
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;

            //Context.Response.Redirect("~/SitePages/Home.aspx", false);
            Context.Response.Redirect("~/SitePages/ECProgramList.aspx", false);


        }
        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }

        protected void rptrprogrammesummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            using (var dbContext = new CyberportEMS_EDM())
            {
                var program_id = Context.Request.QueryString["program_id"];
                int programId = Convert.ToInt32(program_id);
                TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);
                if (!(objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation")))
                {
                    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        Label lblProgramtype = (Label)e.Item.FindControl("lblProgramtype");
                        lblProgramtype.Visible = true;
                        Label lblApplicationtype = (Label)e.Item.FindControl("lblApplicationtype");
                        lblApplicationtype.Visible = true;

                        HiddenField Hdn_Application_Number = (HiddenField)e.Item.FindControl("Hdn_Application_Number");

                        TB_VETTING_APPLICATION objVettingApplication = dbContext.TB_VETTING_APPLICATION.Where(x => x.Application_Number == Hdn_Application_Number.Value).FirstOrDefault();
                        bool IsWithDraw = false;
                        if (objVettingApplication != null)
                        {
                            TB_PRESENTATION_APPLICATION_REMARKS objTB_PRESENTATION_APPLICATION_REMARKS = dbContext.TB_PRESENTATION_APPLICATION_REMARKS.Where(x => x.Vetting_Appilcation_ID == objVettingApplication.Vetting_Application_ID).FirstOrDefault();
                            IsWithDraw = objTB_PRESENTATION_APPLICATION_REMARKS != null ? objTB_PRESENTATION_APPLICATION_REMARKS.Withdraw : IsWithDraw;
                        }



                        Label lblrecommended = (Label)e.Item.FindControl("lblrecommended");
                        bool IsRecommended = (bool)DataBinder.Eval(e.Item.DataItem, "Recommended");

                        if (IsWithDraw)
                        {
                            lblrecommended.Text = "Withdraw";
                        }
                        else if (IsRecommended)
                        {
                            lblrecommended.Text = "Recommended";
                        }
                        else
                        {
                            lblrecommended.Text = "Not Recommended";
                        }
                        if (IsRecommended)
                        {
                            CheckBox chKECconfirmed = (CheckBox)e.Item.FindControl("chKECconfirmed");
                            chKECconfirmed.Checked = true;
                        }
                    }
                    if (e.Item.ItemType == ListItemType.Header)
                    {
                        Label thcompanyname = (Label)e.Item.FindControl("thcompanyname");
                        thcompanyname.Text = "Programme Name";
                        Label thprogramname = (Label)e.Item.FindControl("thprogramname");
                        thprogramname.Visible = true;
                        Label thapplicationame = (Label)e.Item.FindControl("thapplicationame");
                        thapplicationame.Visible = true;
                    }



                }
                else
                {
                    if (e.Item.ItemType == ListItemType.Header)
                    {

                        Label thcompanyname = (Label)e.Item.FindControl("thcompanyname");
                        thcompanyname.Text = "Company Name";
                        Label thprogramname = (Label)e.Item.FindControl("thprogramname");
                        thprogramname.Visible = false;
                        Label thapplicationame = (Label)e.Item.FindControl("thapplicationame");
                        thapplicationame.Visible = false;
                    }


                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {

                    HiddenField Hdn_Application_Number = (HiddenField)e.Item.FindControl("Hdn_Application_Number");

                    TB_VETTING_APPLICATION objVettingApplication = dbContext.TB_VETTING_APPLICATION.Where(x => x.Application_Number == Hdn_Application_Number.Value).FirstOrDefault();
                    bool IsWithDraw = false;
                    if (objVettingApplication != null)
                    {
                        TB_PRESENTATION_APPLICATION_REMARKS objTB_PRESENTATION_APPLICATION_REMARKS = dbContext.TB_PRESENTATION_APPLICATION_REMARKS.Where(x => x.Vetting_Appilcation_ID == objVettingApplication.Vetting_Application_ID).FirstOrDefault();
                        IsWithDraw = objTB_PRESENTATION_APPLICATION_REMARKS != null ? objTB_PRESENTATION_APPLICATION_REMARKS.Withdraw : IsWithDraw;
                    }



                    Label lblrecommended = (Label)e.Item.FindControl("lblrecommended");
                    bool IsRecommended = (bool)DataBinder.Eval(e.Item.DataItem, "Recommended");

                    if (IsWithDraw)
                    {
                        lblrecommended.Text = "Withdraw";
                    }
                    else if (IsRecommended)
                    {
                        lblrecommended.Text = "Recommended";
                    }
                    else
                    {
                        lblrecommended.Text = "Not Recommended";
                    }
                    if (IsRecommended)
                    {
                        CheckBox chKECconfirmed = (CheckBox)e.Item.FindControl("chKECconfirmed");
                        chKECconfirmed.Checked = true;
                    }
                }
            }
        }
        protected bool FillCompanyForCPIP(CyberportEMS_EDM dbContext, List<TB_INCUBATION_APPLICATION> objApps, AwardedApplicationSelected ConflictsItems)
        {
            try
            {
                SPFunctions objFUnction = new SPFunctions();
                string currentUser = objFUnction.GetCurrentUser();
                foreach (TB_INCUBATION_APPLICATION objApp in objApps)
                {
                    TB_COMPANY_APPLICATION_MAP objComApp = dbContext.TB_COMPANY_APPLICATION_MAP.FirstOrDefault(x => x.Application_ID == objApp.Incubation_ID && x.Application_No == objApp.Application_Number);
                    if (objComApp == null)
                    {
                        Guid Company_Profile_ID = NewProgramId();
                        Guid CompanyProfileIdUserSelected = ConflictsItems.CompaniesConflicts.FirstOrDefault(x => x.IsSelectedCompany == true)
                            .CompanyProfileId;

                        TB_COMPANY_PROFILE_BASIC objComp = new TB_COMPANY_PROFILE_BASIC();
                        if (CompanyProfileIdUserSelected == default(Guid))
                        {
                            objComp = new TB_COMPANY_PROFILE_BASIC()
                            {
                                Company_Profile_ID = Company_Profile_ID,
                                Company_Name = objApp.Company_Name_Eng,
                                Name_Eng = objApp.Company_Name_Eng,
                                Name_Chi = objApp.Company_Name_Chi,
                                CPIP_Custer = objApp.Business_Area,
                                CPIP_Abstract = objApp.Abstract,
                                CPIP_Abstract_Chi = objApp.Abstract_Chi,
                                Company_Ownership_Structure = objApp.Company_Ownership_Structure,
                                Remarks = objApp.Additional_Information,
                                Created_By = objApp.Created_By,
                                Created_Date = DateTime.Now,
                                Modified_By = currentUser,
                                Modified_Date = DateTime.Now,
                            };
                            dbContext.Entry(objComp).State = System.Data.Entity.EntityState.Added;
                        }
                        else {
                            objComp = dbContext.TB_COMPANY_PROFILE_BASIC.FirstOrDefault(x => x.Company_Profile_ID == CompanyProfileIdUserSelected);
                            objComp.CPIP_Custer = objApp.Business_Area;
                            objComp.CPIP_Abstract = objApp.Abstract;
                            objComp.CPIP_Abstract_Chi = objApp.Abstract_Chi;
                            objComp.Company_Ownership_Structure = objApp.Company_Ownership_Structure;
                            objComp.Remarks = objApp.Additional_Information;
                            objComp.Created_By = objApp.Created_By;
                            objComp.Created_Date = DateTime.Now;
                            objComp.Modified_By = currentUser;
                            objComp.Modified_Date = DateTime.Now;
                            dbContext.Entry(objComp).State = System.Data.Entity.EntityState.Modified;
                        }


                        // objComp.TB_COMPANY_CONTACT = new List<TB_COMPANY_CONTACT>();

                        List<TB_APPLICATION_CONTACT_DETAIL> objContactList = dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objApp.Incubation_ID).ToList();

                        foreach (TB_APPLICATION_CONTACT_DETAIL objContact in objContactList)
                        {
                            TB_COMPANY_CONTACT ObjTBContact =
                                objComp.TB_COMPANY_CONTACT.FirstOrDefault(x => x.Name_Eng == objContact.First_Name_Eng + ":" + objContact.Last_Name_Eng);

                            if (ObjTBContact == null)
                            {
                                objComp.TB_COMPANY_CONTACT.Add(new TB_COMPANY_CONTACT()
                                {
                                    Contact_No = objContact.Contact_No_Mobile,
                                    Email = objContact.Email,
                                    Fax_No = objContact.Fax,
                                    Salutation = objContact.Salutation,
                                    Contact_No_Home = objContact.Contact_No_Home,
                                    Contact_No_Office = objContact.Contact_No_Office,
                                    Created_By = currentUser,
                                    Created_Date = DateTime.Now,
                                    Modified_By = currentUser,
                                    Modified_Date = DateTime.Now,
                                    Mailing_Address = objContact.Mailing_Address,
                                    Name_Eng = objContact.First_Name_Eng + ":" + objContact.Last_Name_Eng,
                                    Position = objContact.Position,
                                    No_Edit = true
                                });
                            }
                            else {

                                ObjTBContact.Contact_No = objContact.Contact_No_Mobile;
                                ObjTBContact.Email = objContact.Email;
                                ObjTBContact.Fax_No = objContact.Fax;
                                ObjTBContact.Salutation = objContact.Salutation;
                                ObjTBContact.Contact_No_Home = objContact.Contact_No_Home;
                                ObjTBContact.Contact_No_Office = objContact.Contact_No_Office;
                                ObjTBContact.Created_By = currentUser;
                                ObjTBContact.Created_Date = DateTime.Now;
                                ObjTBContact.Modified_By = currentUser;
                                ObjTBContact.Modified_Date = DateTime.Now;
                                ObjTBContact.Mailing_Address = objContact.Mailing_Address;
                                ObjTBContact.Name_Eng = objContact.First_Name_Eng + ":" + objContact.Last_Name_Eng;
                                ObjTBContact.Position = objContact.Position;
                                ObjTBContact.No_Edit = true;

                                dbContext.Entry(ObjTBContact).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        //CoreMembersProfile 
                        //   objComp.TB_COMPANY_MEMBER = new List<TB_COMPANY_MEMBER>();

                        List<TB_APPLICATION_COMPANY_CORE_MEMBER> ObjCoreMemberApplication = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objApp.Incubation_ID).ToList();
                        foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objCoreMemberApp in ObjCoreMemberApplication)
                        {
                            TB_COMPANY_MEMBER objCoreMember =
                                objComp.TB_COMPANY_MEMBER.FirstOrDefault(x => x.Name == objCoreMemberApp.Name);

                            if (objCoreMember == null)
                            {
                                objComp.TB_COMPANY_MEMBER.Add(new TB_COMPANY_MEMBER()
                                {
                                    Created_By = currentUser,
                                    Created_Date = DateTime.Now,
                                    Modified_By = currentUser,
                                    Modified_Date = DateTime.Now,
                                    Background_Information = objCoreMemberApp.Background_Information,
                                    Bootcamp_Eligible_Number = objCoreMemberApp.Bootcamp_Eligible_Number,
                                    CASP = false,
                                    CCMF = false,
                                    CoreMember_Profile = objCoreMemberApp.CoreMember_Profile,
                                    CPIP = true,
                                    HKID = objCoreMemberApp.HKID,
                                    Masked_HKID = objCoreMemberApp.Masked_HKID,
                                    Name = objCoreMemberApp.Name,
                                    Position = objCoreMemberApp.Position,
                                    Professional_Qualifications = objCoreMemberApp.Professional_Qualifications,
                                    Special_Achievements = objCoreMemberApp.Special_Achievements,
                                    Working_Experiences = objCoreMemberApp.Working_Experiences,
                                    No_Edit = true
                                });
                            }
                            else {

                                objCoreMember.Created_By = currentUser;
                                objCoreMember.Created_Date = DateTime.Now;
                                objCoreMember.Modified_By = currentUser;
                                objCoreMember.Modified_Date = DateTime.Now;
                                objCoreMember.Background_Information = objCoreMemberApp.Background_Information;
                                objCoreMember.Bootcamp_Eligible_Number = objCoreMemberApp.Bootcamp_Eligible_Number;
                                objCoreMember.CoreMember_Profile = objCoreMemberApp.CoreMember_Profile;
                                objCoreMember.CPIP = true;
                                objCoreMember.HKID = objCoreMemberApp.HKID;
                                objCoreMember.Masked_HKID = objCoreMemberApp.Masked_HKID;
                                objCoreMember.Name = objCoreMemberApp.Name;
                                objCoreMember.Position = objCoreMemberApp.Position;
                                objCoreMember.Professional_Qualifications = objCoreMemberApp.Professional_Qualifications;
                                objCoreMember.Special_Achievements = objCoreMemberApp.Special_Achievements;
                                objCoreMember.Working_Experiences = objCoreMemberApp.Working_Experiences;
                                objCoreMember.No_Edit = true;

                                dbContext.Entry(objCoreMember).State = System.Data.Entity.EntityState.Modified;
                            }
                        }

                        //FUnding
                        dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objApp.Incubation_ID).ToList().ForEach(x =>

                           objComp.TB_COMPANY_FUND.Add(new TB_COMPANY_FUND()
                           {
                               Created_By = currentUser,
                               Created_Date = DateTime.Now,
                               Modified_By = currentUser,
                               Modified_Date = DateTime.Now,
                               Amount_Received = x.Amount_Received,
                               Programme_Name = x.Programme_Name,
                               Application_No = objApp.Application_Number,
                               Application_Status = x.Application_Status,
                               Currency = x.Currency,
                               Expenditure_Nature = x.Expenditure_Nature,
                               Funding_Status = x.Funding_Status,
                               Maximum_Amount = x.Maximum_Amount,
                               Programme_Type = "CPIP",
                               No_Edit = true
                           }));

                       
                        objComp.TB_COMPANY_APPLICATION_MAP.Add(new TB_COMPANY_APPLICATION_MAP()
                        {

                            Applicaition_Type = "CPIP",
                            Application_ID = objApp.Incubation_ID,
                            Application_No = objApp.Application_Number,
                            Created_By = currentUser,
                            Created_Date = DateTime.Now,
                        });
                       
                        dbContext.SaveChanges();

                    }
                }
                return false;
            }
            catch (DbEntityValidationException exp)
            {
                List<string> ErrorLIst = new List<string>();
                foreach (var s in exp.EntityValidationErrors)
                {
                    foreach (var d in s.ValidationErrors)
                    {
                        ErrorLIst.Add(d.PropertyName + " - " + d.ErrorMessage);
                    }

                }
                lblgrouperror.Visible = true;
                lblgrouperror.DataSource = ErrorLIst;
                lblgrouperror.DataBind();
                return true;

            }

        }


        protected bool FillCompanyForCCMF(CyberportEMS_EDM dbContext, List<TB_CCMF_APPLICATION> objApps, AwardedApplicationSelected ConflictsItems)
        {
            try
            {
                SPFunctions objFUnction = new SPFunctions();
                string currentUser = objFUnction.GetCurrentUser();

                foreach (TB_CCMF_APPLICATION objApp in objApps)
                {

                    TB_COMPANY_APPLICATION_MAP objComApp = dbContext.TB_COMPANY_APPLICATION_MAP.FirstOrDefault(x => x.Application_ID == objApp.CCMF_ID && x.Application_No == objApp.Application_Number);
                    if (objComApp == null)
                    {
                        Guid Company_Profile_ID = NewProgramId();
                        Guid CompanyProfileIdUserSelected = ConflictsItems.CompaniesConflicts.FirstOrDefault(x => x.IsSelectedCompany == true)
                          .CompanyProfileId;
                        TB_COMPANY_PROFILE_BASIC objComp = new TB_COMPANY_PROFILE_BASIC();
                        if (CompanyProfileIdUserSelected == default(Guid))
                        {
                            objComp = new TB_COMPANY_PROFILE_BASIC()
                            {
                                Company_Profile_ID = Company_Profile_ID,
                                Company_Name = objApp.Project_Name_Eng,
                                Name_Eng = objApp.Project_Name_Eng,
                                Name_Chi = objApp.Project_Name_Chi,
                                CCMF_Custer = objApp.Business_Area,
                                CCMF_Abstract = objApp.Abstract_Eng,
                                CCMF_Abstract_Chi = objApp.Abstract_Chi,
                                Remarks = objApp.Additional_Information,
                                Created_By = objApp.Created_By,
                                Created_Date = DateTime.Now,
                                Modified_By = currentUser,
                                Modified_Date = DateTime.Now,
                            };
                            dbContext.Entry(objComp).State = System.Data.Entity.EntityState.Added;
                        }
                        else {
                            objComp = dbContext.TB_COMPANY_PROFILE_BASIC.FirstOrDefault(x => x.Company_Profile_ID == CompanyProfileIdUserSelected);
                            objComp.Company_Name = objApp.Project_Name_Eng;
                            objComp.Name_Eng = objApp.Project_Name_Eng;
                            objComp.Name_Chi = objApp.Project_Name_Chi;
                            objComp.CCMF_Custer = objApp.Business_Area;
                            objComp.CCMF_Abstract = objApp.Abstract_Eng;
                            objComp.CCMF_Abstract_Chi = objApp.Abstract_Chi;
                            objComp.Remarks = objApp.Additional_Information;
                            objComp.Created_By = objApp.Created_By;
                            objComp.Created_Date = DateTime.Now;
                            objComp.Modified_By = currentUser;
                            objComp.Modified_Date = DateTime.Now;
                            dbContext.Entry(objComp).State = System.Data.Entity.EntityState.Modified;
                        }

                        // objComp.TB_COMPANY_CONTACT = new List<TB_COMPANY_CONTACT>();
                        List<TB_APPLICATION_CONTACT_DETAIL> objContactList =
                            dbContext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objApp.CCMF_ID).ToList();
                        foreach (TB_APPLICATION_CONTACT_DETAIL objContact in objContactList)
                        {
                            TB_COMPANY_CONTACT ObjTBContact =
                               objComp.TB_COMPANY_CONTACT.FirstOrDefault(x => x.Name_Eng == objContact.First_Name_Eng + ":" + objContact.Last_Name_Eng);

                            if (ObjTBContact == null)
                            {

                                objComp.TB_COMPANY_CONTACT.Add(new TB_COMPANY_CONTACT()
                                {

                                    Contact_No = objContact.Contact_No,
                                    Email = objContact.Email,
                                    Fax_No = objContact.Fax,
                                    Salutation = objContact.Salutation,
                                    Area = objContact.Area,

                                    Education_Institution = objContact.Education_Institution_Eng,
                                    Graduation_Date = (objContact.Graduation_Month.HasValue && objContact.Graduation_Year.HasValue) ? (new DateTime(objContact.Graduation_Year.Value, objContact.Graduation_Month.Value, 1)) : (DateTime?)null,
                                    Programme_Enrolled = objContact.Programme_Enrolled_Eng,
                                    Student_ID = objContact.Student_ID_Number,
                                    Organization_Name = objContact.Organisation_Name,
                                    Created_By = currentUser,
                                    Created_Date = DateTime.Now,
                                    Modified_By = currentUser,
                                    Modified_Date = DateTime.Now,
                                    Mailing_Address = objContact.Mailing_Address,
                                    Name_Chi = objContact.First_Name_Chi + ":" + objContact.Last_Name_Chi,
                                    Name_Eng = objContact.First_Name_Eng + ":" + objContact.First_Name_Eng,

                                    Position = objContact.Position,
                                    No_Edit = true
                                });
                            }
                            else {
                                ObjTBContact.Contact_No = objContact.Contact_No;
                                ObjTBContact.Email = objContact.Email;
                                ObjTBContact.Fax_No = objContact.Fax;
                                ObjTBContact.Salutation = objContact.Salutation;
                                ObjTBContact.Area = objContact.Area;
                                ObjTBContact.Education_Institution = objContact.Education_Institution_Eng;
                                ObjTBContact.Graduation_Date = (objContact.Graduation_Month.HasValue && objContact.Graduation_Year.HasValue) ? (new DateTime(objContact.Graduation_Year.Value, objContact.Graduation_Month.Value, 1)) : (DateTime?)null;
                                ObjTBContact.Programme_Enrolled = objContact.Programme_Enrolled_Eng;
                                ObjTBContact.Student_ID = objContact.Student_ID_Number;
                                ObjTBContact.Organization_Name = objContact.Organisation_Name;
                                ObjTBContact.Created_By = currentUser;
                                ObjTBContact.Created_Date = DateTime.Now;
                                ObjTBContact.Modified_By = currentUser;
                                ObjTBContact.Modified_Date = DateTime.Now;
                                ObjTBContact.Mailing_Address = objContact.Mailing_Address;
                                ObjTBContact.Name_Chi = objContact.First_Name_Chi + ":" + objContact.Last_Name_Chi;
                                ObjTBContact.Name_Eng = objContact.First_Name_Eng + ":" + objContact.First_Name_Eng;

                                ObjTBContact.Position = objContact.Position;
                                ObjTBContact.No_Edit = true;
                                dbContext.Entry(ObjTBContact).State = System.Data.Entity.EntityState.Modified;
                            }

                        }
                            //CoreMembersProfile 
                            //objComp.TB_COMPANY_MEMBER = new List<TB_COMPANY_MEMBER>();
                            List<TB_APPLICATION_COMPANY_CORE_MEMBER> ObjCoreMemberApplication = dbContext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objApp.CCMF_ID).ToList();

                            foreach (TB_APPLICATION_COMPANY_CORE_MEMBER objCoreMemberApp in ObjCoreMemberApplication)
                            {
                                TB_COMPANY_MEMBER objCoreMember =
                                objComp.TB_COMPANY_MEMBER.FirstOrDefault(x => x.Name == objCoreMemberApp.Name);
                                if (objCoreMember == null)
                                {
                                    objComp.TB_COMPANY_MEMBER.Add(new TB_COMPANY_MEMBER()
                                    {
                                        Created_By = currentUser,
                                        Created_Date = DateTime.Now,
                                        Modified_By = currentUser,
                                        Modified_Date = DateTime.Now,
                                        Background_Information = objCoreMemberApp.Background_Information,
                                        Bootcamp_Eligible_Number = objCoreMemberApp.Bootcamp_Eligible_Number,
                                        CASP = false,
                                        CCMF = true,
                                        CoreMember_Profile = objCoreMemberApp.CoreMember_Profile,
                                        CPIP = false,
                                        HKID = objCoreMemberApp.HKID,
                                        Masked_HKID = objCoreMemberApp.Masked_HKID,
                                        Name = objCoreMemberApp.Name,
                                        Position = objCoreMemberApp.Position,
                                        Professional_Qualifications = objCoreMemberApp.Professional_Qualifications,
                                        Special_Achievements = objCoreMemberApp.Special_Achievements,
                                        Working_Experiences = objCoreMemberApp.Working_Experiences,
                                        No_Edit = true
                                    });
                                }
                                else {

                                    objCoreMember.Created_By = currentUser;
                                    objCoreMember.Created_Date = DateTime.Now;
                                    objCoreMember.Modified_By = currentUser;
                                    objCoreMember.Modified_Date = DateTime.Now;
                                    objCoreMember.Background_Information = objCoreMemberApp.Background_Information;
                                    objCoreMember.Bootcamp_Eligible_Number = objCoreMemberApp.Bootcamp_Eligible_Number;
                                    objCoreMember.CCMF = true;
                                    objCoreMember.CoreMember_Profile = objCoreMemberApp.CoreMember_Profile;
                                    objCoreMember.HKID = objCoreMemberApp.HKID;
                                    objCoreMember.Masked_HKID = objCoreMemberApp.Masked_HKID;
                                    objCoreMember.Name = objCoreMemberApp.Name;
                                    objCoreMember.Position = objCoreMemberApp.Position;
                                    objCoreMember.Professional_Qualifications = objCoreMemberApp.Professional_Qualifications;
                                    objCoreMember.Special_Achievements = objCoreMemberApp.Special_Achievements;
                                    objCoreMember.Working_Experiences = objCoreMemberApp.Working_Experiences;
                                    objCoreMember.No_Edit = true;

                                    dbContext.Entry(objCoreMember).State = System.Data.Entity.EntityState.Modified;
                                }
                            }


                            //FUnding

                            dbContext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objApp.CCMF_ID).ToList().ForEach(x =>

                               objComp.TB_COMPANY_FUND.Add(new TB_COMPANY_FUND()
                               {
                                   Created_By = currentUser,
                                   Created_Date = DateTime.Now,
                                   Modified_By = currentUser,
                                   Modified_Date = DateTime.Now,
                                   Amount_Received = x.Amount_Received,
                                   Programme_Name = x.Programme_Name,
                                   Application_No = objApp.Application_Number,
                                   Application_Status = x.Application_Status,
                                   Currency = x.Currency,
                                   Expenditure_Nature = x.Expenditure_Nature,
                                   Funding_Status = x.Funding_Status,
                                   Maximum_Amount = x.Maximum_Amount,
                                   Programme_Type = "CCMF",
                                   No_Edit = true

                               }));
                          
                            objComp.TB_COMPANY_APPLICATION_MAP.Add(new TB_COMPANY_APPLICATION_MAP()
                            {
                                Applicaition_Type = "CCMF",
                                Application_ID = objApp.CCMF_ID,
                                Application_No = objApp.Application_Number,
                                Created_By = currentUser,
                                Created_Date = DateTime.Now,
                            });
                          
                            dbContext.SaveChanges();
                        }

                    }
                    return false;
                }

            catch (DbEntityValidationException exp)
            {
                List<string> ErrorLIst = new List<string>();
                foreach (var s in exp.EntityValidationErrors)
                {
                    foreach (var d in s.ValidationErrors)
                    {
                        ErrorLIst.Add(d.PropertyName + " - " + d.ErrorMessage);
                    }

                }
                lblgrouperror.Visible = true;
                lblgrouperror.DataSource = ErrorLIst;
                lblgrouperror.DataBind();
                return true;
            }


        }
        private Guid NewProgramId()
        {
            Guid objNewId = Guid.NewGuid();
            while (new CyberportEMS_EDM().TB_COMPANY_PROFILE_BASIC.Where(x => x.Company_Profile_ID == objNewId).Count() == 0)
            {
                objNewId = Guid.NewGuid();
                break;
            }
            return objNewId;
        }

        protected void GetCompanyConflictUserSelection(ref List<AwardedApplicationSelected> ObjSelections)
        {
            foreach (RepeaterItem Items in rptrConflictCompanies.Items)
            {
                HiddenField hdnApplicationId = (HiddenField)Items.FindControl("hdnApplicationId");
                Guid ApplicationId = Guid.Parse(hdnApplicationId.Value);
                DropDownList ddlConflictCompanies = (DropDownList)Items.FindControl("ddlConflictCompanies");
                Guid Companyid = Guid.Parse(ddlConflictCompanies.SelectedValue);
                ObjSelections.Where(x => x.ApplicationId == ApplicationId).ToList().ForEach(z => z.CompaniesConflicts.FirstOrDefault(x => x.CompanyProfileId == Companyid).IsSelectedCompany = true);
            }
        }
        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            bool IsErrorOccured = false;
            using (var dbContext = new CyberportEMS_EDM())
            {
                List<AwardedApplicationSelected> objSelectedApps = GetAwardedApplicationsWithConflicts();
                GetCompanyConflictUserSelection(ref objSelectedApps);

                foreach (AwardedApplicationSelected aItem in objSelectedApps)
                {
                    //HiddenField hdn_ProgramID = (HiddenField)aItem.FindControl("hdn_ProgramID");
                    //string ProgramID = hdn_ProgramID.Value;
                    //int ProgramId = Convert.ToInt16(ProgramID);
                    TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == aItem.ProgramId);
                    //Label App_number = (Label)aItem.FindControl("lblapplicationnumber");
                    //string appno = App_number.Text;
                    if (aItem.Programme_Name.ToLower().Contains("incubation"))
                    {
                        List<TB_INCUBATION_APPLICATION> objTB_INCUBATION_APPLICATION = new List<TB_INCUBATION_APPLICATION>();
                        objTB_INCUBATION_APPLICATION = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Application_Number == aItem.ApplicationNo).ToList();

                        objTB_INCUBATION_APPLICATION.ForEach(x => x.Awarded = true);

                        //objTB_INCUBATION_APPLICATION.ForEach(x => x.Status = "Awarded");
                        objTB_PROGRAMME_INTAKE.Status = formsubmitaction.Completed.ToString();


                        IsErrorOccured = FillCompanyForCPIP(dbContext, objTB_INCUBATION_APPLICATION, aItem);
                    }
                    else
                    {

                        List<TB_CCMF_APPLICATION> objCCMFApplication = new List<TB_CCMF_APPLICATION>();
                        objCCMFApplication = dbContext.TB_CCMF_APPLICATION.Where(x => x.Application_Number == aItem.ApplicationNo).ToList();
                        objCCMFApplication.ForEach(x => x.Awarded = true);
                        objTB_PROGRAMME_INTAKE.Status = formsubmitaction.Completed.ToString();

                        IsErrorOccured = FillCompanyForCCMF(dbContext, objCCMFApplication, aItem);


                    }
                    List<TB_EC_RESULT> objTB_VETTING_APPLICATION = dbContext.TB_EC_RESULT.Where(x => x.Programme_ID == aItem.ProgramId).ToList();

                    objTB_VETTING_APPLICATION.ForEach(x => x.Status = formsubmitaction.Completed.ToString());
                    SPFunctions objFUnction = new SPFunctions();
                    string currentUser = objFUnction.GetCurrentUser();
                    objTB_VETTING_APPLICATION.ForEach(x => x.Confirm_By = currentUser);

                }
                dbContext.SaveChanges();

                if (!IsErrorOccured)
                {
                    SubmitPopup.Visible = false;
                    pnlsubmissionpopup.Visible = true;
                }
            }
        }

        //protected void btn_Confirm_Click(object sender, EventArgs e)
        //{
        //    bool IsErrorOccured = false;
        //    using (var dbContext = new CyberportEMS_EDM())
        //    {
        //        List<AwardedApplicationSelected> objSelectedApps = GetAwardedApplicationsWithConflicts();
        //        GetCompanyConflictUserSelection(ref objSelectedApps);

        //        foreach (RepeaterItem aItem in rptrprogrammesummary.Items)
        //        {
        //            CheckBox chkDisplayTitle = (CheckBox)aItem.FindControl("chKECconfirmed");
        //            if (chkDisplayTitle.Checked)
        //            {
        //                HiddenField hdn_ProgramID = (HiddenField)aItem.FindControl("hdn_ProgramID");
        //                string ProgramID = hdn_ProgramID.Value;
        //                int ProgramId = Convert.ToInt16(ProgramID);
        //                TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == ProgramId);
        //                Label App_number = (Label)aItem.FindControl("lblapplicationnumber");
        //                string appno = App_number.Text;
        //                if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
        //                {
        //                    List<TB_INCUBATION_APPLICATION> objTB_INCUBATION_APPLICATION = new List<TB_INCUBATION_APPLICATION>();
        //                    objTB_INCUBATION_APPLICATION = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Application_Number == appno).ToList();
        //                    //foreach (TB_INCUBATION_APPLICATION item in objTB_INCUBATION_APPLICATION)
        //                    //{
        //                    //    string strEmailContent = "";
        //                    //    strEmailContent = textmail.InnerText;
        //                    //    strEmailContent = strEmailContent.Replace("Cyberport Incubation Programme", objTB_PROGRAMME_INTAKE.Programme_Name);
        //                    //    strEmailContent = strEmailContent.Replace("201612", Convert.ToString(objTB_PROGRAMME_INTAKE.Intake_Number));
        //                    //    int IsEmailed = CBPEmail.SendMail(item.Applicant, LocalizeCommon("ECResultMail_To_Applicants"), strEmailContent);

        //                    //}

        //                    objTB_INCUBATION_APPLICATION.ForEach(x => x.Awarded = true);

        //                    //objTB_INCUBATION_APPLICATION.ForEach(x => x.Status = "Awarded");
        //                    objTB_PROGRAMME_INTAKE.Status = formsubmitaction.Completed.ToString();
        //                    var program_id = Context.Request.QueryString["program_id"];
        //                    int programId = Convert.ToInt32(program_id);

        //                    List<TB_EC_RESULT> objTB_VETTING_APPLICATION = dbContext.TB_EC_RESULT.Where(x => x.Programme_ID == programId).ToList();

        //                    objTB_VETTING_APPLICATION.ForEach(x => x.Status = formsubmitaction.Completed.ToString());
        //                    SPFunctions objFUnction = new SPFunctions();
        //                    string currentUser = objFUnction.GetCurrentUser();
        //                    objTB_VETTING_APPLICATION.ForEach(x => x.Confirm_By = currentUser);

        //                    dbContext.SaveChanges();
        //                    IsErrorOccured = FillCompanyForCPIP(dbContext, objTB_INCUBATION_APPLICATION);
        //                }
        //                else
        //                {

        //                    List<TB_CCMF_APPLICATION> objTB_INCUBATION_APPLICATION = new List<TB_CCMF_APPLICATION>();
        //                    objTB_INCUBATION_APPLICATION = dbContext.TB_CCMF_APPLICATION.Where(x => x.Application_Number == appno).ToList();
        //                    //foreach (TB_CCMF_APPLICATION item in objTB_INCUBATION_APPLICATION)
        //                    //{
        //                    //    string strEmailContent = "";
        //                    //    strEmailContent = textmail.InnerText;
        //                    //    strEmailContent = strEmailContent.Replace("Cyberport Incubation Programme", objTB_PROGRAMME_INTAKE.Programme_Name);
        //                    //    strEmailContent = strEmailContent.Replace("201612", Convert.ToString(objTB_PROGRAMME_INTAKE.Intake_Number));
        //                    //    int IsEmailed = CBPEmail.SendMail(item.Applicant, LocalizeCommon("ECResultMail_To_Applicants"), strEmailContent);
        //                    //}
        //                    objTB_INCUBATION_APPLICATION.ForEach(x => x.Awarded = true);
        //                    //objTB_INCUBATION_APPLICATION.ForEach(x => x.Status = "Awarded");
        //                    objTB_PROGRAMME_INTAKE.Status = formsubmitaction.Completed.ToString();


        //                    var program_id = Context.Request.QueryString["program_id"];
        //                    int programId = Convert.ToInt32(program_id);

        //                    List<TB_EC_RESULT> objTB_VETTING_APPLICATION = dbContext.TB_EC_RESULT.Where(x => x.Programme_ID == programId).ToList();

        //                    objTB_VETTING_APPLICATION.ForEach(x => x.Status = formsubmitaction.Completed.ToString());
        //                    SPFunctions objFUnction = new SPFunctions();
        //                    string currentUser = objFUnction.GetCurrentUser();
        //                    objTB_VETTING_APPLICATION.ForEach(x => x.Confirm_By = currentUser);
        //                    dbContext.SaveChanges();
        //                    IsErrorOccured = FillCompanyForCCMF(dbContext, objTB_INCUBATION_APPLICATION);


        //                }

        //            }
        //        }
        //        if (!IsErrorOccured)
        //        {
        //            SubmitPopup.Visible = false;
        //            pnlsubmissionpopup.Visible = true;
        //        }
        //    }
        //}

        protected List<AwardedApplicationSelected> GetAwardedApplicationsWithConflicts()
        {
            List<AwardedApplicationSelected> objApps = new List<AwardedApplicationSelected>();
            using (var dbContext = new CyberportEMS_EDM())
            {
                List<TB_COMPANY_PROFILE_BASIC> objCompanies = dbContext.TB_COMPANY_PROFILE_BASIC.ToList();

                foreach (RepeaterItem aItem in rptrprogrammesummary.Items)
                {
                    CheckBox chkDisplayTitle = (CheckBox)aItem.FindControl("chKECconfirmed");
                    if (chkDisplayTitle.Checked)
                    {
                        AwardedApplicationSelected objAppSelected = new AwardedApplicationSelected();
                        HiddenField hdn_ProgramID = (HiddenField)aItem.FindControl("hdn_ProgramID");
                        string ProgramID = hdn_ProgramID.Value;
                        objAppSelected.ProgramId = Convert.ToInt16(ProgramID);

                        TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == objAppSelected.ProgramId);
                        Label App_number = (Label)aItem.FindControl("lblapplicationnumber");
                        objAppSelected.Programme_Name = objTB_PROGRAMME_INTAKE.Programme_Name;
                        objAppSelected.ApplicationNo = App_number.Text;
                        if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
                        {
                            TB_INCUBATION_APPLICATION objIncub = dbContext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == objAppSelected.ApplicationNo && x.Status == "Complete Screening");
                            objAppSelected.ApplicationId = objIncub.Incubation_ID;
                            objAppSelected.Applicant = objIncub.Applicant;

                        }
                        else
                        {
                            TB_CCMF_APPLICATION objCCMFApp =
                            dbContext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == objAppSelected.ApplicationNo && (x.Status == "Complete Screening"));
                            objAppSelected.ApplicationId = objCCMFApp.CCMF_ID;
                            objAppSelected.Applicant = objCCMFApp.Applicant;
                        }


                        //checking conflicts at the same time
                        List<TB_COMPANY_PROFILE_BASIC> objExistingCompany
                               = objCompanies.Where(x => x.Created_By == objAppSelected.Applicant).ToList();

                        if (objExistingCompany.Count > 0)
                        {
                            objAppSelected.CompaniesConflicts.AddRange(objExistingCompany.Select(x => new CompanyConflictsList()
                            {
                                Company_ApplicationNo = x.Company_Name + " - "
                                + x.TB_COMPANY_APPLICATION_MAP.FirstOrDefault().Application_No,
                                CompanyProfileId = x.Company_Profile_ID
                            }));
                            objAppSelected.CompaniesConflicts.Add(new CompanyConflictsList() { CompanyProfileId = default(Guid), Company_ApplicationNo = "New Company" });
                            objAppSelected.HasConflict = true;
                        }
                        else {
                            objAppSelected.CompaniesConflicts.Add(new CompanyConflictsList() { CompanyProfileId = default(Guid), Company_ApplicationNo = "New Company",IsSelectedCompany=true });
                            objAppSelected.HasConflict = false;
                            
                        }

                        objApps.Add(objAppSelected);
                    }
                }
            }
            return objApps;
        }



        protected void btn_CompanyConflictsHide_Click(object sender, EventArgs e)
        {
            pnlCompanyConflicts.Visible = false;
        }

        protected void btn_CompanyConflictsConfirm_Click(object sender, EventArgs e)
        {
            pnlCompanyConflicts.Visible = false;
            ShowFinalSubmitPopup();
        }
        protected void ShowFinalSubmitPopup()
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                SubmitPopup.Visible = true;
                var program_id = Context.Request.QueryString["program_id"];
                int programid = Convert.ToInt32(program_id);
                TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programid);
                lblintakeno.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.Intake_Number);
                lblDeadlinePopup.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.Application_Deadline);
            }
        }
        public class AwardedApplicationSelected
        {
            public AwardedApplicationSelected()
            {
                CompaniesConflicts = new List<CompanyConflictsList>();
            }
            public int ProgramId { get; set; }
            public string Programme_Name { get; set; }
            public Guid ApplicationId { get; set; }
            public string ApplicationNo { get; set; }
            public string Applicant { get; set; }
            public bool HasConflict { get; set; }
            public List<CompanyConflictsList> CompaniesConflicts { get; set; }
        }
        public class CompanyConflictsList
        {
            public Guid CompanyProfileId { get; set; }
            public string Company_ApplicationNo { get; set; }
            public bool IsSelectedCompany { get; set; }
        }

    }
}
