using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using System.Web.Security;
using Microsoft.SharePoint.Administration.Claims;
using CBP_EMS_SP.Common;
using System.Web.Configuration;
using Microsoft.ApplicationServer.Caching;
using System.Globalization;
using System.Threading;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Web.UI.WebControls;

namespace CBP_EMS_SP.PublicUserControls.MyApplications
{
    [ToolboxItemAttribute(false)]
    public partial class MyApplications : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public MyApplications()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            //CacheManager objCommonFunctions = new CacheManager();
            //string macaddress = SPFunctions.GetMacAddress();
            //DataCache emsuserlanguage = objCommonFunctions.DefaultCache;
            //if (emsuserlanguage[macaddress] != null)
            //{
            //    var CultureName = emsuserlanguage[macaddress];

            //    CultureInfo culture = new CultureInfo(CultureName.ToString());
            //    Thread.CurrentThread.CurrentCulture = culture;
            //    Thread.CurrentThread.CurrentUICulture = culture;

            //}


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
            lblheading.InnerText = Localize("My_Applications");
            lblheadincol.InnerText = Localize("My_App_Collaboration");
            deletepopup.InnerText = Localize("My_App_deletepopup");
            lnkyes.Text = Localize("lbl_yes");
            lnkno.Text = Localize("lbl_no");
            if (!Page.IsPostBack)
            {

                FillProgramDetail();
            }

            foreach (RepeaterItem item in rptrMyApplicationsCCMF.Items)
            {

                LinkButton btncontrol = item.FindControl("lnkCollaboration") as LinkButton;
                if (btncontrol != null)
                {

                    btncontrol.Text = Localize("btn_Collaboration");
                }
            }

            foreach (RepeaterItem item in rptrMyApplicationsIncubation.Items)
            {

                LinkButton btncontrol = item.FindControl("lnkCollaboration") as LinkButton;
                if (btncontrol != null)
                {

                    btncontrol.Text = Localize("btn_Collaboration");
                }
            }
        }

        protected void FillProgramDetail()
        {
            string strCurrentUser = new Common.SPFunctions().GetCurrentUser().ToLower();
            using (var dbContext = new CyberportEMS_EDM())
            {
                var objInc = (from oIncub in dbContext.TB_INCUBATION_APPLICATION
                              join oIntake in dbContext.TB_PROGRAMME_INTAKE on oIncub.Programme_ID equals oIntake.Programme_ID
                              where oIncub.Created_By.ToLower() == strCurrentUser && oIncub.Application_Parent_ID == null && oIncub.Status != formsubmitaction.Deleted.ToString()
                              select new
                              {
                                  Programme_guid = oIncub.Incubation_ID,
                                  Programme_ID = oIntake.Programme_ID,
                                  Programme_Name = oIntake.Programme_Name,
                                  Intake_Number = oIntake.Intake_Number,
                                  ApplicationNumber = oIncub.Application_Number,
                                  ProjectName = oIncub.Company_Name_Eng,
                                  Application_Deadline = oIntake.Application_Deadline,
                                  Status = oIncub.Status
                              });


                rptrMyApplicationsIncubation.DataSource = objInc.Where(x => !objInc.Any(y => x.Programme_guid == y.Programme_guid && x.Application_Deadline < DateTime.Now && x.Status.ToLower() == "saved")).ToList();
                rptrMyApplicationsIncubation.DataBind();

                var objCCMF = (from oIncub in dbContext.TB_CCMF_APPLICATION
                               join oIntake in dbContext.TB_PROGRAMME_INTAKE on oIncub.Programme_ID equals oIntake.Programme_ID
                               where oIncub.Created_By.ToLower() == strCurrentUser && oIncub.Application_Parent_ID == null && oIncub.Status != formsubmitaction.Deleted.ToString() && oIntake.Programme_Name != "Cyberport Creative Micro Fund - GBAYEP"
                               select new
                               {
                                   Programme_guid = oIncub.CCMF_ID,
                                   Programme_ID = oIntake.Programme_ID,
                                   Programme_Name = oIntake.Programme_Name,
                                   Intake_Number = oIntake.Intake_Number,
                                   ApplicationNumber = oIncub.Application_Number,
                                   ProjectName = oIncub.Project_Name_Eng,
                                   Application_Deadline = oIntake.Application_Deadline,
                                   Status = oIncub.Status
                               });
                rptrMyApplicationsCCMF.DataSource = objCCMF.Where(x => !objCCMF.Any(y => x.Programme_guid == y.Programme_guid && x.Application_Deadline < DateTime.Now && x.Status.ToLower() == "saved")).ToList();
                rptrMyApplicationsCCMF.DataBind();

                var objCCMFGBAYEP = (from oIncub in dbContext.TB_CCMF_APPLICATION
                               join oIntake in dbContext.TB_PROGRAMME_INTAKE on oIncub.Programme_ID equals oIntake.Programme_ID
                                     where oIncub.Created_By.ToLower() == strCurrentUser && oIncub.Application_Parent_ID == null && oIncub.Status != formsubmitaction.Deleted.ToString() && oIntake.Programme_Name == "Cyberport Creative Micro Fund - GBAYEP"
                               select new
                               {
                                   Programme_guid = oIncub.CCMF_ID,
                                   Programme_ID = oIntake.Programme_ID,
                                   Programme_Name = oIntake.Programme_Name,
                                   Intake_Number = oIntake.Intake_Number,
                                   ApplicationNumber = oIncub.Application_Number,
                                   ProjectName = oIncub.Project_Name_Eng,
                                   Application_Deadline = oIntake.Application_Deadline,
                                   Status = oIncub.Status
                               });
                rptrMyApplicationsCCMFGBAYEP.DataSource = objCCMFGBAYEP.Where(x => !objCCMF.Any(y => x.Programme_guid == y.Programme_guid && x.Application_Deadline < DateTime.Now && x.Status.ToLower() == "saved")).ToList();
                rptrMyApplicationsCCMFGBAYEP.DataBind();

                var objIncCol = (from oIncubcol in dbContext.TB_APPLICATION_COLLABORATOR
                                 join oIncub in dbContext.TB_INCUBATION_APPLICATION on new { oIncubcol.Programme_ID, oIncubcol.Application_Number } equals new { oIncub.Programme_ID, oIncub.Application_Number }
                                 join oIntake in dbContext.TB_PROGRAMME_INTAKE on oIncub.Programme_ID equals oIntake.Programme_ID
                                 where oIncubcol.Email.ToLower() == strCurrentUser && !string.IsNullOrEmpty(oIncub.Application_Number)
                                  && oIncub.Application_Parent_ID == null

                                 select new
                                 {
                                     Programme_guid = oIncub.Incubation_ID,
                                     Programme_ID = oIntake.Programme_ID,
                                     Programme_Name = oIntake.Programme_Name,
                                     Intake_Number = oIntake.Intake_Number,
                                     ApplicationNumber = oIncub.Application_Number,
                                     ProjectName = oIncub.Company_Name_Eng,
                                     Application_Deadline = oIntake.Application_Deadline,
                                     Status = oIncub.Status
                                 });
                rptrMyApplicationsIncubationColb.DataSource = objIncCol.Where(x => !objIncCol.Any(y => x.Programme_guid == y.Programme_guid && x.Application_Deadline < DateTime.Now && x.Status.ToLower() == "saved")).ToList();
                rptrMyApplicationsIncubationColb.DataBind();

                var objCCMFCol = (from oIncubcol in dbContext.TB_APPLICATION_COLLABORATOR
                                  join oIncub in dbContext.TB_CCMF_APPLICATION on new { oIncubcol.Programme_ID, oIncubcol.Application_Number } equals new { oIncub.Programme_ID, oIncub.Application_Number }
                                  join oIntake in dbContext.TB_PROGRAMME_INTAKE on oIncub.Programme_ID equals oIntake.Programme_ID
                                  where oIncubcol.Email.ToLower() == strCurrentUser && !string.IsNullOrEmpty(oIncub.Application_Number) && oIntake.Programme_Name != "Cyberport Creative Micro Fund - GBAYEP"
                                  && oIncub.Application_Parent_ID == null
                                  select new
                                  {
                                      Programme_guid = oIncub.CCMF_ID,
                                      Programme_ID = oIntake.Programme_ID,
                                      Programme_Name = oIntake.Programme_Name,
                                      Intake_Number = oIntake.Intake_Number,
                                      ApplicationNumber = oIncub.Application_Number,
                                      ProjectName = oIncub.Project_Name_Eng,
                                      Application_Deadline = oIntake.Application_Deadline,
                                      Status = oIncub.Status
                                  });
                rptrMyApplicationsCCMFColb.DataSource = objCCMFCol.Where(x => !objCCMFCol.Any(y => x.Programme_guid == y.Programme_guid && x.Application_Deadline < DateTime.Now && x.Status.ToLower() == "saved")).ToList();
                rptrMyApplicationsCCMFColb.DataBind();

                var objCCMFGBAYEPCol = (from oIncubcol in dbContext.TB_APPLICATION_COLLABORATOR
                                  join oIncub in dbContext.TB_CCMF_APPLICATION on new { oIncubcol.Programme_ID, oIncubcol.Application_Number } equals new { oIncub.Programme_ID, oIncub.Application_Number }
                                  join oIntake in dbContext.TB_PROGRAMME_INTAKE on oIncub.Programme_ID equals oIntake.Programme_ID
                                        where oIncubcol.Email.ToLower() == strCurrentUser && !string.IsNullOrEmpty(oIncub.Application_Number) && oIntake.Programme_Name == "Cyberport Creative Micro Fund - GBAYEP"
                                  && oIncub.Application_Parent_ID == null
                                  select new
                                  {
                                      Programme_guid = oIncub.CCMF_ID,
                                      Programme_ID = oIntake.Programme_ID,
                                      Programme_Name = oIntake.Programme_Name,
                                      Intake_Number = oIntake.Intake_Number,
                                      ApplicationNumber = oIncub.Application_Number,
                                      ProjectName = oIncub.Project_Name_Eng,
                                      Application_Deadline = oIntake.Application_Deadline,
                                      Status = oIncub.Status
                                  });
                rptrMyApplicationsCCMFGBAYEPColb.DataSource = objCCMFGBAYEPCol.Where(x => !objCCMFCol.Any(y => x.Programme_guid == y.Programme_guid && x.Application_Deadline < DateTime.Now && x.Status.ToLower() == "saved")).ToList();
                rptrMyApplicationsCCMFGBAYEPColb.DataBind();


                var objCASP = (from oCASP in dbContext.TB_CASP_APPLICATION
                               join oIntake in dbContext.TB_PROGRAMME_INTAKE on oCASP.Programme_ID equals oIntake.Programme_ID
                               where oCASP.Created_By.ToLower() == strCurrentUser && oCASP.Application_Parent_ID == null && oCASP.Status != formsubmitaction.Deleted.ToString()
                               select new
                               {
                                   Programme_guid = oCASP.CASP_ID,
                                   Programme_ID = oIntake.Programme_ID,
                                   Programme_Name = oIntake.Programme_Name,
                                   Intake_Number = oIntake.Intake_Number,
                                   ApplicationNumber = oCASP.Application_No,
                                   ProjectName = "",
                                   Application_Deadline = oIntake.Application_Deadline,
                                   Status = oCASP.Status
                               });
                rptrMyApplicationCASP.DataSource = objCASP.ToList();
                rptrMyApplicationCASP.DataBind();


                var objCASPCol = (from ocol in dbContext.TB_APPLICATION_COLLABORATOR
                                  join oCasp in dbContext.TB_CASP_APPLICATION on new { Programme_ID = ocol.Programme_ID, Application_Number = ocol.Application_Number } equals new { Programme_ID = oCasp.Programme_ID, Application_Number = oCasp.Application_No }
                                  join oIntake in dbContext.TB_PROGRAMME_INTAKE on oCasp.Programme_ID equals oIntake.Programme_ID
                                  where ocol.Email.ToLower() == strCurrentUser && !string.IsNullOrEmpty(oCasp.Application_No)
                                  && oCasp.Application_Parent_ID == null
                                  select new
                                  {
                                      Programme_guid = oCasp.CASP_ID,
                                      Programme_ID = oIntake.Programme_ID,
                                      Programme_Name = oIntake.Programme_Name,
                                      Intake_Number = oIntake.Intake_Number,
                                      ApplicationNumber = oCasp.Application_No,
                                      ProjectName = oCasp.Accelerator_Name,
                                      Application_Deadline = oIntake.Application_Deadline,
                                      Status = oCasp.Status
                                  });
                rptrMyApplicationsCASPColb.DataSource = objCASPCol.ToList();
                rptrMyApplicationsCASPColb.DataBind();
            }
        }


        protected void rptrMyApplicationsIncubation_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            hdnProgramID.Value = string.Empty;
            hdnApplicationNumber.Value = string.Empty;
            hdnApplicationId.Value = string.Empty;
            string[] strData = e.CommandArgument.ToString().Split(':');
            if (e.CommandName == "Delete")
            {
                pnldeleteapplication.Visible = true;


                hdnProgramID.Value = strData[0];
                hdnApplicationNumber.Value = strData[1];
            }
            else
            {
                pnlcollaboratorPopup.Visible = true;

                FillCollaborator(strData[0], strData[1], strData[2]);
            }

        }
        protected void FillCollaborator(string ProgramId, string ProgramNumber, string appid)
        {

            using (var dbContext = new CyberportEMS_EDM())
            {
                hdnProgramID.Value = ProgramId;
                hdnApplicationNumber.Value = ProgramNumber;
                hdnApplicationId.Value = appid;
                int ProgId = Convert.ToInt32(ProgramId);
                rptrCollaboratorList.DataSource = dbContext.TB_APPLICATION_COLLABORATOR.Where(x => x.Application_Number == ProgramNumber && x.Programme_ID == ProgId).ToList();
                rptrCollaboratorList.DataBind();
                lblInvitation.Text = "";
                txtCollaboratorEmail.Text = "";

            }
        }

        protected void btn_InviteCollaborator_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            bool isSuccess = true;
            try
            {


                if (Common.CBPRegularExpression.RegExValidate(Common.CBPRegularExpression.Email, txtCollaboratorEmail.Text.Trim()))
                {
                    if (!txtCollaboratorEmail.Text.ToLower().Trim().EndsWith("@cyberport.hk"))
                    {

                        int ProgId = Convert.ToInt32(hdnProgramID.Value);
                        string Appid = hdnApplicationId.Value;
                        string ProgramNumber = hdnApplicationNumber.Value;
                        SPFunctions objfunction = new SPFunctions();
                        using (var dbContext = new CyberportEMS_EDM())
                        {
                            TB_PROGRAMME_INTAKE oIntake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == ProgId);

                            TB_APPLICATION_COLLABORATOR objCollaborator = dbContext.TB_APPLICATION_COLLABORATOR.
                                FirstOrDefault(x => x.Email == txtCollaboratorEmail.Text.Trim() && x.Programme_ID == ProgId && x.Application_Number == hdnApplicationNumber.Value);
                            if (objCollaborator != null)
                            {
                                lblInvitation.Text = Localize("Error_MyApp_Collabratoralreadyexsist");
                                lblInvitation.CssClass = "text-danger";
                                isSuccess = false;
                            }
                            else if (txtCollaboratorEmail.Text.ToLower().Trim() == objfunction.GetCurrentUser().ToLower().Trim())
                            {
                                lblInvitation.Text = Localize("Error_MyApp_sameuser");
                                lblInvitation.CssClass = "text-danger";
                                isSuccess = false;
                            }
                            else
                            {

                                aspnet_Users objUser = dbContext.aspnet_Users.FirstOrDefault(x => x.UserName.ToLower() == txtCollaboratorEmail.Text.ToLower());
                                if (objUser == null)
                                {
                                    isSuccess = CreateUser(txtCollaboratorEmail.Text.Trim(), oIntake.Programme_Name);

                                }
                                else
                                {
                                    string strEmailContent = CBPEmail.GetEmailTemplate("CollaborationExistsInvitation");
                                    strEmailContent = strEmailContent.Replace("@@ApplicantEmail", new SPFunctions().GetCurrentUser());
                                    CBPTokenGeneration objCBPTokenGeneration = new CBPTokenGeneration();
                                    IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                                    objTbParams = dbContext.TB_SYSTEM_PARAMETER;

                                    string UserSharepointEmailSend = objTbParams.FirstOrDefault(x => x.Config_Code == "UserSharepointEmailSend").Value;
                                    string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                                    WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;

                                    objCBPTokenGeneration.Add("ProgrammeId", Convert.ToString(ProgId));
                                    objCBPTokenGeneration.Add("AppId", Convert.ToString(Appid));
                                    objCBPTokenGeneration.Add("Collaboratoremail", txtCollaboratorEmail.Text.ToLower().Trim());
                                    //string applicationType = oIntake.Programme_Name.Contains("Cyberport Incubation Program") ? "IncubationProgram.aspx" : "CCMF.aspx";
                                    string applicationType = oIntake.Programme_Name.Contains("Cyberport Incubation Program") ? "IncubationProgram.aspx" : (oIntake.Programme_Name.Contains("Cyberport Creative Micro Fund - GBAYEP") ? "CCMFGBAYEP.aspx" : "CCMF.aspx");
                                    string token = "/SitePages/" + applicationType + "?prog=" + ProgId + "&app=" + Appid;
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", oIntake.Programme_Name);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    //strEmailContent = strEmailContent.Replace("@@WebsiteUrl", WebsiteUrl + token);



                                    int IsEmailed = CBPEmail.SendMail(txtCollaboratorEmail.Text.Trim(), Localize("Mail_Invite_Collaborator"), strEmailContent);

                                }
                                if (isSuccess)
                                {
                                    string CurrentUser = new SPFunctions().GetCurrentUser();
                                    dbContext.TB_APPLICATION_COLLABORATOR.Add(new TB_APPLICATION_COLLABORATOR()
                                    {
                                        Application_Number = hdnApplicationNumber.Value,
                                        Created_By = CurrentUser,
                                        Modified_By = CurrentUser,
                                        Email = txtCollaboratorEmail.Text,
                                        Created_Date = DateTime.Now,
                                        Modified_Date = DateTime.Now,
                                        Programme_ID = ProgId
                                    });
                                    dbContext.SaveChanges();
                                    txtCollaboratorEmail.Text = string.Empty;

                                    lblInvitation.Text = Localize("MyApp_successfullyinvited");
                                    lblInvitation.CssClass = "text-success";
                                    FillCollaborator(hdnProgramID.Value, hdnApplicationNumber.Value, hdnApplicationId.Value);
                                }

                            }
                        }

                    }
                    else
                    {
                        lblInvitation.Text = Localize("Error_MyApp_emaillimitation");
                        lblInvitation.CssClass = "text-danger";

                    }
                }
                else
                {
                    lblInvitation.Text = Localize("Forgot_pass_emailaddress");
                    lblInvitation.CssClass = "text-danger";
                }
            }
            catch (Exception ex)
            {
                lblInvitation.Text = ex.Message;
                lblInvitation.CssClass = "text-danger";
            }
        }

        protected void rptrCollaboratorList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int ProgId = Convert.ToInt32(hdnProgramID.Value);

                    TB_APPLICATION_COLLABORATOR objCollaborator = dbContext.TB_APPLICATION_COLLABORATOR.
                        FirstOrDefault(x => x.Email == e.CommandArgument.ToString() && x.Programme_ID == ProgId && x.Application_Number == hdnApplicationNumber.Value);
                    dbContext.TB_APPLICATION_COLLABORATOR.Remove(objCollaborator);
                    dbContext.SaveChanges();
                }
                FillCollaborator(hdnProgramID.Value, hdnApplicationNumber.Value, hdnApplicationId.Value);

            }
        }


        private bool CreateUser(string UserEmail, string ProgramName)
        {
            bool IsValid = false;
            SPWeb site = null;
            SPGroup oGroup = null;
            try
            {
                MembershipCreateStatus UserState;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string spUserRoleGroup = (String)WebConfigurationManager.AppSettings["SPExternalUserGroupName"];
                    SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    site = siteCollection.OpenWeb();
                    site.AllowUnsafeUpdates = true;
                    oGroup = site.SiteGroups[spUserRoleGroup];


                    // Attempt to create the user
                    Membership.CreateUser(UserEmail, "dummypassowrd@123", UserEmail
                                            , "SecurityQuestion"
                        , "SecurityAnswer"
                        , true
                        , out UserState);

                    // Validate user creation
                    string Message = "";
                    #region User account creation status handling
                    switch (UserState)
                    {
                        case MembershipCreateStatus.Success:
                            IsValid = true;
                            break;

                        case MembershipCreateStatus.DuplicateEmail:
                            Message = Localize("Registration_email_dublication");
                            break;

                        case MembershipCreateStatus.DuplicateUserName:
                            Message = Localize("Registration_email_dublication");
                            break;

                        case MembershipCreateStatus.InvalidAnswer:
                            Message = Localize("Registration_invalid_security_ans");
                            break;

                        case MembershipCreateStatus.InvalidEmail:
                            Message = Localize("Registration_Invalid_email");
                            break;

                        case MembershipCreateStatus.InvalidPassword:
                            Message = Localize("Registration_Pass_notsecure");
                            break;

                        case MembershipCreateStatus.InvalidQuestion:
                            Message = Localize("Registration_invalid_question");
                            break;

                        default:
                            Message = Localize("Registration_internal_error");
                            break;
                    }
                    #endregion

                    if (!IsValid)
                    {
                        lblInvitation.Text = Message;
                        lblInvitation.CssClass = "text-danger";
                    }

                    if (IsValid)
                    {
                        Roles.AddUserToRole(UserEmail, spUserRoleGroup);

                        SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                        if (mgr != null)
                        {
                            SPUser oUser = site.EnsureUser(UserEmail);
                            oUser.Email = UserEmail;
                            oUser.Name = UserEmail;
                            oGroup.AddUser(oUser);
                        }
                        using (var dbcotext = new CyberportEMS_EDM())
                        {
                            int ProgId = Convert.ToInt32(hdnProgramID.Value);
                            string Appid = hdnApplicationId.Value;
                            string ProgramNumber = hdnApplicationNumber.Value;

                            string UserID = dbcotext.aspnet_Users.FirstOrDefault(x => x.UserName.ToLower() == UserEmail.ToLower()).UserId.ToString();
                            string strEmailContent = CBPEmail.GetEmailTemplate("CollaborationNewInvitation");
                            IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                            objTbParams = dbcotext.TB_SYSTEM_PARAMETER;
                            string UserSharepointEmailSend = objTbParams.FirstOrDefault(x => x.Config_Code == "UserSharepointEmailSend").Value;
                            string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;

                            strEmailContent = strEmailContent.Replace("@@WebsiteUrl", WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl);
                            strEmailContent = strEmailContent.Replace("@@ApplicantEmail", new SPFunctions().GetCurrentUser());
                            strEmailContent = strEmailContent.Replace("@@ProgramName", ProgramName);
                            strEmailContent = strEmailContent.Replace("@@ForgotPasswordLink", UserID);
                            //string applicationType = ProgramName.Contains("Cyberport Incubation Program") ? "IncubationProgram.aspx" : "CCMF.aspx";
                            string applicationType = ProgramName.Contains("Cyberport Incubation Program") ? "IncubationProgram.aspx" : (ProgramName.Contains("Cyberport Creative Micro Fund - GBAYEP") ? "CCMFGBAYEP.aspx" : "CCMF.aspx");
                            string token = "/SitePages/" + applicationType + "?prog=" + ProgId + "&app=" + Appid;
                            strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                            int IsEmailed = CBPEmail.SendMail(UserEmail, Localize("Mail_Invite_Collaborator"), strEmailContent);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                IsValid = false;
                Membership.DeleteUser(UserEmail);
                SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                if (mgr != null)
                {
                    SPUser oUser = site.EnsureUser(UserEmail);
                    oGroup.RemoveUser(oUser);
                }
                lblInvitation.Text = ex.Message;
                lblInvitation.CssClass = "text-danger";
            }
            return IsValid;
        }

        protected void imgPopupClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlcollaboratorPopup.Visible = false;
            hdnApplicationNumber.Value = string.Empty;
            hdnProgramID.Value = string.Empty;

        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnldeleteapplication.Visible = false;
            Hdnprogid.Value = string.Empty;
            hdnappno.Value = string.Empty;
        }


        protected void lnkyes_Click(object sender, EventArgs e)
        {
            int Programid = Convert.ToInt32(hdnProgramID.Value);
            //Guid appId = new Guid(hdnApplicationId.Value);
            string applicationNumber = hdnApplicationNumber.Value;

            using (var dbContext = new CyberportEMS_EDM())
            {
                TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == Programid);
                if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
                {
                    List<TB_INCUBATION_APPLICATION> objTB_INCUBATION_APPLICATION = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Application_Number == applicationNumber).ToList();
                    foreach (TB_INCUBATION_APPLICATION item in objTB_INCUBATION_APPLICATION)
                    {
                        item.Status = formsubmitaction.Deleted.ToString();
                        Fill_Programelist(item.Application_Number, item.Programme_ID, item.Intake_Number, item.Version_Number, item.Business_Area, item.Applicant);
                    }
                    //dbContext.TB_INCUBATION_APPLICATION.RemoveRange(objTB_INCUBATION_APPLICATION);
                }
                else if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("cyberport creative micro fund"))
                {
                    List<TB_CCMF_APPLICATION> objTB_INCUBATION_APPLICATION = dbContext.TB_CCMF_APPLICATION.Where(x => x.Application_Number == applicationNumber).ToList();
                    foreach (TB_CCMF_APPLICATION item in objTB_INCUBATION_APPLICATION)
                    {
                        item.Status = formsubmitaction.Deleted.ToString();
                        Fill_Programelist(item.Application_Number, item.Programme_ID, item.Intake_Number, item.Version_Number, item.Business_Area, item.Applicant);
                    }
                    // dbContext.TB_CCMF_APPLICATION.RemoveRange(objTB_INCUBATION_APPLICATION);

                }
                else if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("cyberport accelerator"))
                {
                    List<TB_CASP_APPLICATION> objCaspApps = dbContext.TB_CASP_APPLICATION.Where(x => x.Application_No == applicationNumber).ToList();
                    foreach (TB_CASP_APPLICATION item in objCaspApps)
                    {
                        TB_PROGRAMME_INTAKE objIntake = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == item.Programme_ID).ToList().FirstOrDefault();
                        item.Status = formsubmitaction.Deleted.ToString();
                        Fill_ProgramelistCASP(item.Application_No, item.Programme_ID, objIntake.Intake_Number, item.Version_Number, item.Applicant);
                    }
                    // dbContext.TB_CCMF_APPLICATION.RemoveRange(objTB_INCUBATION_APPLICATION);

                }
                dbContext.SaveChanges();
            }
            pnldeleteapplication.Visible = false;
            FillProgramDetail();

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
        private void Fill_Programelist(string Application_number, int Programme_Id, int Intake, string version, string Businessarea, string Applicant)
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
                        itemAttachment["Status"] = formsubmitaction.Deleted.ToString();

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

        private void Fill_ProgramelistCASP(string Application_number, int Programme_Id, int Intake, string version, string Applicant)
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
                        itemAttachment["Status"] = formsubmitaction.Deleted.ToString();

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


        protected void lnkno_Click(object sender, EventArgs e)
        {
            hdnProgramID.Value = "0";
            pnldeleteapplication.Visible = false;
            FillProgramDetail();
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }

    }
}
