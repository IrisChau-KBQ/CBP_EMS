using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Common;
using Microsoft.SharePoint.Utilities;
using CBP_EMS_SP.Data.Models;
using System.Linq;
using Novacode;

namespace CBP_EMS_SP.ECWebPartCASP.VisualWebPart1
{
    [ToolboxItemAttribute(false)]
    public partial class VisualWebPart1 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        private String m_progid;
        private String m_ApplicationID;
        private String m_systemuser;
        private String m_ApplicationNumber;

        private String m_Status;

        private String m_Role;


        public String m_path = System.Configuration.ConfigurationManager.AppSettings["DownloadPath"] ?? @"D:\\tmp";
        public String m_programName;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink;

        public TB_CASP_APPLICATION objAppInit;


        public VisualWebPart1()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //m_progid = Context.Request.QueryString["ProgNo"]; //Get Programme ID from URL
            //m_ApplicationID = Context.Request.QueryString["AppNo"]; //Get Application Number from URL
            //m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User
            //lblCoordinator.Text = m_systemuser;

            //m_progid = Context.Request.QueryString["Prog"]; //Get Programme ID from URL
            //m_ApplicationID = Context.Request.QueryString["App"]; //Get Application Number from URL
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User
            //FlblCoordinator.Text = m_systemuser;

            m_progid = HttpContext.Current.Request.QueryString["Prog"];
            m_ApplicationID = HttpContext.Current.Request.QueryString["App"];
            if (HttpContext.Current.Request.QueryString["ProgName"] != "Cyberport Accelerator Support Programme")
            {
                RedirectPageToBack();
            }
            objAppInit = GetUserApplication();

            m_ApplicationNumber = objAppInit.Application_No;
            m_Status = objAppInit.Status;


            getReview();
            if (!Page.IsPostBack)
            {
                AccessControl();
                getdbdata();
                getRadio();
                getBDMComments(m_ApplicationNumber, m_progid);
                lblrole.Text = m_Role;
                lblApplicationStatus.Text = m_Status;
            }



            getSYSTEMPARAMETER();
            //if (m_ApplicationIsInDebug == "1")
            //{
            //    btnDownload.OnClientClick = "return confirm('Start processing now, an email will be sent to (" + m_ApplicationDebugEmailSentTo + "). Please wait.')";
            //}
            //else
            //{
            //    btnDownload.OnClientClick = "return confirm('Start processing now, an email will be sent to (" + SPContext.Current.Web.CurrentUser.Email + "). Please wait.')";
            //}

        }


        protected TB_CASP_APPLICATION GetUserApplication()
        {
            TB_CASP_APPLICATION objApp = new TB_CASP_APPLICATION();
            try
            {
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    Guid applicationId = Guid.Parse(m_ApplicationID);
                    objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.CASP_ID == applicationId);
                }
                if (objApp == null)
                {
                    RedirectPageToBack();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return objApp;
        }

        protected void RedirectPageToBack()
        {

            string newdirection = "Application%20List.aspx?";
            newdirection += "ProgName=" + HttpContext.Current.Request.QueryString["ProgName"];
            newdirection += "&IntakeNo=" + HttpContext.Current.Request.QueryString["IntakeNo"];
            newdirection += "&Cluster=" + HttpContext.Current.Request.QueryString["Cluster"];
            newdirection += "&Status=" + HttpContext.Current.Request.QueryString["Status"];
            newdirection += "&SortColumn1=" + HttpContext.Current.Request.QueryString["SortColumn1"];
            newdirection += "&SortOrder1=" + HttpContext.Current.Request.QueryString["SortOrder1"];
            newdirection += "&SortColumn2=" + HttpContext.Current.Request.QueryString["SortColumn2"];
            newdirection += "&SortOrder2=" + HttpContext.Current.Request.QueryString["SortOrder2"];
            //newdirection = "Application%20List.aspx";
            Context.Response.Redirect(newdirection);
        }



        protected void getBDMComments(string m_ApplicationNumber, string m_progid)
        {

            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                int m_progidInt = Convert.ToInt32(m_progid);
                TB_SCREENING_HISTORY objScreening = dbContext.TB_SCREENING_HISTORY.Where(
                    x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                && (x.Validation_Result == "Disqualified"
                || x.Validation_Result == "BDM Rejected")
                    ).OrderByDescending(x => x.Created_Date).ToList().FirstOrDefault();

                if (objScreening != null)
                {

                    lblBDMComments.Text = objScreening.Comment_For_Internal_Use;
                    if (objScreening.Validation_Result.Trim() == "BDM Rejected")
                    {
                        lblBDMCommentsTitle.Text = "BDM Rejected Comments";
                    }
                    else
                    {
                        lblBDMCommentsTitle.Text = "BDM Disqualified Comments";
                    }
                }
            }

            if (lblBDMCommentsTitle.Text.Trim() == "")
            {
                lblBDMCommentsTitle.Visible = true;
                lblBDMComments.Visible = true;
            }

        }

        protected void AccessControl()
        {
            // Check Role can display this web part
            //Applicant  //Collaborator  //CCMF Coordinator //CCMF BDM  //CPIP Coordinator  //CPIP BDM  //Senior Manager  //CPMO
            List<ListItem> objRadioActions = new List<ListItem>();

            string SubmittedDate = objAppInit.Completed_Date.HasValue ? objAppInit.Completed_Date.Value.ToString("dd-MMM-yyyy") : "";
            lblApplicationComplete.Text = "Complete Date : " + SubmittedDate;
            txtApplicationComplete.Text = SubmittedDate;
            if (m_Status == stCompleted)
            {
                txtApplicationComplete.Visible = false;
                imgCalender.Visible = false;
                lblApplicationComplete.Visible = true;
                hdnApplicationStatus.Value = stCompleted;
            }

            if (m_Role == "Applicant" || m_Role == "Collaborator")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "CASP Coordinator")
            {
                //objRadioActions.Add(new ListItem() { Text = "Confirm", Value = stEligibility_checked});
                //objRadioActions.Add(new ListItem() { Text = "Require additional information", Value = stWaiting_for_response });
                //objRadioActions.Add(new ListItem() { Text = "To be disqualified", Value = stTo_be_disqualified });
                if (m_Status == stCPMO_Approved || m_Status == stCompleted)
                {
                    objRadioActions.Add(new ListItem() { Text = "Complete", Value = stCompleted, Selected = true });
                    hdnCanFactFileSubmit.Value = "0";
                }
                else
                {
                    objRadioActions.Add(new ListItem() { Text = "Confirm", Value = stEligibility_checked, Selected = true });
                    objRadioActions.Add(new ListItem() { Text = "Require additional information", Value = stWaiting_for_response });
                    objRadioActions.Add(new ListItem() { Text = "To be disqualified", Value = stTo_be_disqualified });
                }
                rbtnresult.DataSource = objRadioActions;
                rbtnresult.DataBind();

                if (m_Status == stWaiting_for_response || m_Status == stTo_be_disqualified || m_Status == stDisqualified || m_Status == stEligibility_checked ||
                    m_Status == stBDM_Approved || m_Status == stBDM_Disqualified ||
                    m_Status == stSenior_Manager_Approved || m_Status == stSenior_Manager_Disqualified ||
                    m_Status == stCompleted)
                {
                    hdnCanFactFileSubmit.Value = "0";
                    MainPanel.Visible = true;
                    BtnSubmit.Enabled = false;
                    lblmessage.Text = "Cannot submit on " + m_Status + " status.";
                    rbtnresult.Enabled = false;
                    txtcommentforapplicants.Enabled = false;
                    txtcommentforinternaluse.Enabled = false;
                }
                else
                {
                    MainPanel.Visible = true;
                }
                btnFactFileGenerate.Visible = true;
            }
            else if (m_Role == "CASP BDM")
            {
                objRadioActions.Add(new ListItem() { Text = "BDM Approve", Value = stBDM_Approved });
                objRadioActions.Add(new ListItem() { Text = "BDM Reject", Value = stReject_to_Coordinator, Selected = true });
                objRadioActions.Add(new ListItem() { Text = "BDM Disqualify", Value = stBDM_Disqualified });

                rbtnresult.DataSource = objRadioActions;
                rbtnresult.DataBind();

                if (m_Status == stSubmitted || m_Status == stResubmitted_information || m_Status == stWaiting_for_response || m_Status == stDisqualified || m_Status == stReject_to_Coordinator ||
                 m_Status == stBDM_Approved || m_Status == stBDM_Disqualified ||
                 m_Status == stSenior_Manager_Approved || m_Status == stSenior_Manager_Disqualified ||
                 m_Status == stCPMO_Approved || m_Status == stCompleted)

                {
                    MainPanel.Visible = true;
                    BtnSubmit.Enabled = false;
                    lblmessage.Text = "Cannot submit on " + m_Status + " status.";
                    rbtnresult.Enabled = false;
                    txtcommentforapplicants.Enabled = false;
                    txtcommentforinternaluse.Enabled = false;
                }
                else
                {
                    MainPanel.Visible = true;
                }
            }
            else if (m_Role == "CASP Senior Manager")
            {
                objRadioActions.Add(new ListItem() { Text = "Approve", Value = stSenior_Manager_Approved });
                objRadioActions.Add(new ListItem() { Text = "Senior Manager Reject", Value = stReject_to_Coordinator, Selected = true });
                objRadioActions.Add(new ListItem() { Text = "Disqualify", Value = stSenior_Manager_Disqualified });

                rbtnresult.DataSource = objRadioActions;
                rbtnresult.DataBind();

                if (m_Status == stSubmitted || m_Status == stResubmitted_information || m_Status == stWaiting_for_response || m_Status == stTo_be_disqualified || m_Status == stDisqualified || m_Status == stEligibility_checked || m_Status == stReject_to_Coordinator ||
                m_Status == stEligibility_checked || m_Status == stBDM_Disqualified ||
                m_Status == stSenior_Manager_Approved || m_Status == stSenior_Manager_Disqualified ||
                m_Status == stCPMO_Approved || m_Status == stCompleted)
                {
                    MainPanel.Visible = true;
                    BtnSubmit.Enabled = false;
                    lblmessage.Text = "Cannot submit on " + m_Status + " status.";
                    rbtnresult.Enabled = false;
                    txtcommentforapplicants.Enabled = false;
                    txtcommentforinternaluse.Enabled = false;
                }
                else
                {
                    MainPanel.Visible = true;
                }
            }
            else if (m_Role == "CPMO")
            {
                objRadioActions.Add(new ListItem() { Text = "CPMO Approve", Value = stCPMO_Approved });
                objRadioActions.Add(new ListItem() { Text = "CPMO Reject", Value = stReject_to_Coordinator, Selected = true });

                rbtnresult.DataSource = objRadioActions;
                rbtnresult.DataBind();

                if (m_Status == stSubmitted || m_Status == stResubmitted_information || m_Status == stWaiting_for_response || m_Status == stTo_be_disqualified || m_Status == stDisqualified || m_Status == stEligibility_checked || m_Status == stReject_to_Coordinator ||
                m_Status == stEligibility_checked || m_Status == stBDM_Approved || m_Status == stBDM_Disqualified ||
                m_Status == stSenior_Manager_Disqualified ||
                m_Status == stCPMO_Approved || m_Status == stCompleted)
                {
                    MainPanel.Visible = true;
                    BtnSubmit.Enabled = false;
                    lblmessage.Text = "Cannot submit on " + m_Status + " status.";
                    rbtnresult.Enabled = false;
                    txtcommentforapplicants.Enabled = false;
                    txtcommentforinternaluse.Enabled = false;
                }
                else
                {
                    MainPanel.Visible = true;
                }
            }
            else { MainPanel.Visible = false; }

            //if (getProgrmNameIntakeafterDeadline() == false)
            //{
            //    MainPanel.Visible = false;
            //}

        }

        /// <summary>
        /// Getting all the history and bind to text box
        /// </summary>
        protected void getdbdata()
        {
            List<SearchResult> historys = new List<SearchResult>();
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                int m_progidInt = Convert.ToInt32(m_progid);
                List<TB_SCREENING_HISTORY> objScreening = dbContext.TB_SCREENING_HISTORY.Where(
                    x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                    //&& (x.Validation_Result == "Disqualified" || x.Validation_Result == "BDM Rejected"
                    //|| x.Validation_Result == "To be disqualified"
                    //|| x.Validation_Result == "Eligibility checked"
                    //|| x.Validation_Result == "Waiting for response from applicant" || x.Validation_Result == "Approved")
                    ).OrderByDescending(x => x.Created_Date).ToList();

                objScreening.ForEach(x => historys.Add(new SearchResult()
                {
                    User = x.Created_By,
                    datetime = x.Created_Date.ToString("dd MM yyyy, HH:mm:ss"),
                    Result = x.Validation_Result,
                    CommentForApplicants = x.Comment_For_Applicants,
                    CommentForInternualUse = x.Comment_For_Internal_Use
                }));
                RepeaterHistory.DataSource = historys;
                RepeaterHistory.DataBind();
            }

        }

        //Get by Role and rbtnresult selected value will be set
        protected void getRadio()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                int m_progidInt = Convert.ToInt32(m_progid);
                TB_SCREENING_HISTORY objScreening = null;//new TB_SCREENING_HISTORY();

                if (m_Role == "CASP Coordinator")
                {
                    objScreening = dbContext.TB_SCREENING_HISTORY.Where(
                        x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                    && (x.Validation_Result == stTo_be_disqualified
                    || x.Validation_Result == stEligibility_checked
                    || x.Validation_Result == stWaiting_for_response
                    || x.Validation_Result == stResubmitted_information || x.Validation_Result == stCPMO_Approved || x.Validation_Result == stCompleted
                    )
                        ).OrderByDescending(x => x.Created_Date).ToList().FirstOrDefault();

                }
                else if (m_Role == "CASP BDM")
                {
                    objScreening = dbContext.TB_SCREENING_HISTORY.Where(
                        x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                    && (x.Validation_Result == stBDM_Approved
                    || x.Validation_Result == stBDM_Disqualified
                    || x.Validation_Result == stReject_to_Coordinator
                    )).OrderByDescending(x => x.Created_Date).ToList().FirstOrDefault();

                }
                else if (m_Role == "CASP Senior Manager")
                {
                    objScreening = dbContext.TB_SCREENING_HISTORY.Where(
                        x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                    && (x.Validation_Result == stSenior_Manager_Approved
                    || x.Validation_Result == stSenior_Manager_Disqualified
                    || x.Validation_Result == stReject_to_Coordinator
                    )).OrderByDescending(x => x.Created_Date).ToList().FirstOrDefault();

                }
                else if (m_Role == "CPMO")
                {
                    objScreening = dbContext.TB_SCREENING_HISTORY.Where(
                        x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                    && (x.Validation_Result == stCPMO_Approved
                    || x.Validation_Result == stReject_to_Coordinator
                    )).OrderByDescending(x => x.Created_Date).ToList().FirstOrDefault();

                }
                //    dbContext.TB_SCREENING_HISTORY.Where(
                //    x => x.Programme_ID == m_progidInt && x.Application_Number == m_ApplicationNumber
                //&& x.Created_By.Contains(m_Role)).OrderByDescending(x => x.Created_Date).ToList().FirstOrDefault();


                if (objScreening != null)
                {
                    if(rbtnresult.Items.FindByValue(objScreening.Validation_Result.Trim())!=null)  rbtnresult.Items.FindByValue(objScreening.Validation_Result.Trim()).Selected = true;
                }


                InitializeUploadsDocument();


            }
        }

        //Getting RoleName of user m_Role
        protected void getReview()
        {

            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.LoginName.ToString(); //"Flora Yeung";            
            //lblrole.Text = SPContext.Current.Web.AllRolesForCurrentUser.ToString(); //"CCMF BDM";

            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.Name.ToString();
            //lblrole.Text = "";
            m_Role = "";
            SPSite oSiteCollection = SPContext.Current.Site;
            using (SPWeb oWebsite = oSiteCollection.OpenWeb())
            {
                SPUser userName = oWebsite.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName); //Getting the Current User Login Name
                SPGroupCollection collGroups = userName.Groups;
                if (collGroups.Count > 0)
                {
                    foreach (SPGroup ogroup in collGroups)   //Looping the group collection and adding to the list 
                    {
                        //lblrole.Text = ogroup.Name;
                        m_Role = ogroup.Name;
                    }
                }
            }
            hdnCurrentRole.Value = m_Role;

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            List<string> errlist = new List<string>();
            if (string.IsNullOrEmpty(rbtnresult.SelectedValue))
            {
                errlist.Add("Please select result status");
            }
            else if ((rbtnresult.SelectedValue == stWaiting_for_response || rbtnresult.SelectedValue == stBDM_Disqualified) && string.IsNullOrEmpty(txtcommentforapplicants.Text.Trim()))
            {
                errlist.Add("Comment for applicants is required.");
            }
            else if ((rbtnresult.SelectedValue != stWaiting_for_response && rbtnresult.SelectedValue != stEligibility_checked && rbtnresult.SelectedValue != stBDM_Approved 
                && rbtnresult.SelectedValue != stSenior_Manager_Approved && rbtnresult.SelectedValue != stCPMO_Approved
                && rbtnresult.SelectedValue != stCompleted && rbtnresult.SelectedValue != stBDM_Disqualified) && string.IsNullOrEmpty(txtcommentforinternaluse.Text.Trim()))
            {
                errlist.Add("Comment for internal use is required.");
            }
            //else if (rbtnresult.SelectedValue != stEligibility_checked && string.IsNullOrEmpty(txtcommentforinternaluse.Text))
            //{
            //    errlist.Add("Comment for internal use is required.");
            //}
            else if (rbtnresult.SelectedValue == stCompleted && string.IsNullOrEmpty(txtApplicationComplete.Text))
            {
                errlist.Add("Complete date is required.");
            }
            else if (rbtnresult.SelectedValue == stCompleted && !string.IsNullOrEmpty(txtApplicationComplete.Text))
            {
                DateTime dDate;

                if (DateTime.TryParse(txtApplicationComplete.Text, out dDate))
                {
                    String.Format("{0:M-yy}", dDate);
                    Convert.ToDateTime(txtApplicationComplete.Text.Trim());
                }
                else
                {
                    errlist.Add("Complete date is invalid.");
                }
            }



            try
            {
                if (Page.IsValid)
                {
                    if (m_Role == "CASP Coordinator" && (rbtnresult.SelectedValue != stCompleted || rbtnresult.SelectedValue != stCPMO_Approved ))
                    {
                        TB_CASP_APPLICATION objApp = GetUserApplication();
                        if (objApp.Endorsed_by_Cyberport.HasValue)
                        {
                            if (objApp.Endorsed_by_Cyberport.Value == false)
                            {
                                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT =
                                IncubationContext.ListofTB_APPLICATION_ATTACHMENTGGet(objApp.CASP_ID, objApp.Programme_ID);
                                if (objTB_APPLICATION_ATTACHMENT != null)
                                {
                                    if (!objTB_APPLICATION_ATTACHMENT.Exists(x => x.Attachment_Type.ToLower() == enumAttachmentType.Fact_Sheet.ToString().ToLower()))
                                    {
                                        errlist.Add("Fact Sheet is required");
                                    }
                                }
                            }
                        }
                    }

                    if (errlist.Count == 0)
                    {

                        //string Application_Number = m_ApplicationID;
                        string newdirection = "";
                        string Application_Number = m_ApplicationNumber;
                        string Programme_ID = m_progid;

                        string Validation_Results = rbtnresult.SelectedValue;

                        string Comment_For_Applicants = txtcommentforapplicants.Text.ToString();
                        string Comment_For_Internal_Use = txtcommentforinternaluse.Text.ToString();
                        Comment_For_Internal_Use = string.IsNullOrEmpty(Comment_For_Internal_Use) ? " " : Comment_For_Internal_Use;
                        string Created_By = m_systemuser;

                        using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                        {
                            TB_SCREENING_HISTORY objScreening = new TB_SCREENING_HISTORY();
                            objScreening.Application_Number = Application_Number;
                            objScreening.Programme_ID = Convert.ToInt32(Programme_ID);
                            objScreening.Validation_Result = Validation_Results;
                            objScreening.Comment_For_Applicants = Comment_For_Applicants;
                            objScreening.Comment_For_Internal_Use = Comment_For_Internal_Use;
                            objScreening.Created_By = Created_By;
                            objScreening.Created_Date = DateTime.Now;
                            dbContext.TB_SCREENING_HISTORY.Add(objScreening);

                            



                            dbContext.SaveChanges();

                        }

                        updateStatus(Validation_Results);
                        RedirectPageToBack();

                    }

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException et)
            {
                foreach (var eve in et.EntityValidationErrors)
                {


                    foreach (var ve in eve.ValidationErrors)
                    {
                        errlist.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                errlist.Add(ex.Message);
            }
            lblgrouperror.DataSource = errlist;
            lblgrouperror.DataBind();
        }
        protected void SaveAttachment_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SPFunctions objfunction = new SPFunctions();
            string FileName = string.Empty;

            string ErrorMessage = string.Empty;
            var argument = ((ImageButton)sender).CommandName;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_CASP_APPLICATION objApp = GetUserApplication();
                    int progId = objApp.Programme_ID;
                    if (objApp != null)
                    {
                        //SaveAttachment(fu_BrCopy, enumAttachmentType.BR_COPY, objIncubation.CCMF_ID, progId);
                        SPFunctions objSPFunctions = new SPFunctions();

                        string _fileUrl = string.Empty;

                        if (Fu_FactSheet.HasFile)
                        {
                            if (Fu_FactSheet.PostedFile.ContentLength <= (5 * 1024 * 1024))
                            {
                                string Extension = Fu_FactSheet.FileName.Remove(0, Fu_FactSheet.FileName.LastIndexOf(".") + 1);
                                if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx")
                                {

                                    _fileUrl = objSPFunctions.AttachmentSave(objApp.Application_No, dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == progId),
                            Fu_FactSheet, enumAttachmentType.Fact_Sheet, objApp.Version_Number);
                                    SaveAttachmentUrl(_fileUrl, enumAttachmentType.Fact_Sheet, objApp.CASP_ID, objApp.Programme_ID);
                                    lblFactsheet.Text = "";
                                    InitializeUploadsDocument();
                                }
                                else
                                {
                                    lblFactsheet.Text = "File should be in Doc or PDF format";
                                }
                            }
                            else
                            {

                                lblFactsheet.Text = "File size should be less then 5MB";

                            }

                        }
                        else
                        {
                            lblFactsheet.Text = "Fach sheet is required.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblFactsheet.Text = ex.Message;

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

        protected void updateStatus(string valResult)
        {

            string m_ApplicationID_temp = m_ApplicationID;

            //txtcommentforapplicants.Text = "updateStatus" + m_ApplicationID + "/";
            TB_CASP_APPLICATION objApp = GetUserApplication();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPWeb site = SPFunctions.GetCurrentWeb)
                {
                    site.AllowUnsafeUpdates = true;
                    SPList list = site.Lists["Application List CASP"];

                    SPListItem itemAttachment = GetItemByBdcId(list, objApp.Application_No.ToString(), "Application_Number", objApp.Programme_ID.ToString(), "Programme_ID");
                    itemAttachment["Status"] = valResult;
                    itemAttachment.Update();
                    site.AllowUnsafeUpdates = false;
                }
            });


            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                objApp = dbContext.TB_CASP_APPLICATION.FirstOrDefault(x => x.CASP_ID == objApp.CASP_ID);
                objApp.Status = valResult;
                objApp.Modified_By = m_systemuser;
                objApp.Modified_Date = DateTime.Now;
                if (rbtnresult.SelectedValue == stCompleted && !string.IsNullOrEmpty(txtApplicationComplete.Text))
                {
                    objApp.Completed_Date = Convert.ToDateTime(txtApplicationComplete.Text);


                    TB_COMPANY_APPLICATION_MAP ObjMap = dbContext.TB_COMPANY_APPLICATION_MAP.FirstOrDefault(x => x.Application_No == objApp.CCMF_CPIP_App_No);

                    if (ObjMap!=null)
                    {
                        TB_COMPANY_APPLICATION_MAP CASPCompany = new TB_COMPANY_APPLICATION_MAP()
                        {
                            Applicaition_Type = "CASP",
                            Application_ID = objApp.CASP_ID,
                            Application_No = objApp.Application_No,
                            Created_By = m_systemuser,
                            Created_Date = DateTime.Now,
                            Company_Profile_ID = ObjMap.Company_Profile_ID
                            
                        };

                        dbContext.TB_COMPANY_APPLICATION_MAP.Add(CASPCompany);

                    }



                }
                dbContext.SaveChanges();
               //if(rbtnresult.SelectedValue==stCompleted)
            }


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





        protected void getProgrmNameIntakeNumByProgID()
        {

            try
            {
                using (CyberportEMS_EDM dbcontext = new CyberportEMS_EDM())
                {
                    int ProgId = Convert.ToInt32(m_progid);
                    TB_PROGRAMME_INTAKE objProgram = dbcontext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == ProgId);
                    m_programName = objProgram.Programme_Name;
                    m_intake = objProgram.Intake_Number.ToString();

                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        protected void getSYSTEMPARAMETER()
        {

            try
            {
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {

                    List<TB_SYSTEM_PARAMETER> objParams = dbContext.TB_SYSTEM_PARAMETER.ToList();
                    m_AttachmentPrimaryFolderName = objParams.FirstOrDefault(x => x.Config_Code == "AttachmentPrimaryFolder").Value;
                    m_AttachmentSecondaryFolderName = objParams.FirstOrDefault(x => x.Config_Code == "AttachmentSecondaryFolder").Value;
                    m_zipfiledownloadurl = objParams.FirstOrDefault(x => x.Config_Code == "zipfiledownloadurl").Value;

                    m_ApplicationIsInDebug = objParams.FirstOrDefault(x => x.Config_Code == "ApplicationIsInDebug").Value;
                    m_ApplicationDebugEmailSentTo = objParams.FirstOrDefault(x => x.Config_Code == "ApplicationDebugEmailSentTo").Value;

                    m_path = objParams.FirstOrDefault(x => x.Config_Code == "zipfiledownloadpath").Value;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //m_programName = "Cyberport Incubation Programme";
            //m_intake = "201705";
            getProgrmNameIntakeNumByProgID();
            //String Source = m_AttachmentPrimaryFolderName  + m_AttachmentSecondaryFolderName  + m_programName + " " + m_intake;
            String Source = "";
            if (!string.IsNullOrEmpty(m_AttachmentPrimaryFolderName))
            {
                Source += m_AttachmentPrimaryFolderName + "/";
            }
            if (!string.IsNullOrEmpty(m_AttachmentSecondaryFolderName))
            {
                Source += m_AttachmentSecondaryFolderName + "/";
            }
            Source += m_programName + " " + m_intake;
            Source += "/" + m_ApplicationNumber;
            var FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            String Destination = m_path + @"\" + FileName;

            m_downloadLink = m_zipfiledownloadurl;

            processFolder(Source, Destination, FileName);
        }
        public void processFolder(string folderURL, string zipFile, String FileName)
        {
            string m_username = SPContext.Current.Web.CurrentUser.Name.ToString();
            string m_mail = "";
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
            }
            else
            {
                m_mail = SPContext.Current.Web.CurrentUser.Email;
            }

            //string m_mail = "Blue.Qiu@mouxidea.com.hk";

            if (Page.IsValid)
            {
                string m_subject = "";
                string m_body = "";
                string m_Programme_Name = m_programName;
                string m_Intake_Number = m_intake;
                string m_Password;
                string m_downloadlink = m_downloadLink;
                string m_Starttime;
                string m_Endtime;
                //string m_zipstatus = "done";



                //starting email
                m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is processing.";
                //m_body = "Hi, Zip File is processing, please wait next email to confirm Zip File is ready.";
                m_body = getEmailTemplate("ZipDownloadStartEmail");
                sharepointsendemail(m_mail, m_subject, m_body);


                /*************************/
                //zip programm in here:

                m_Starttime = DateTime.Now.ToString();
                //SPSite site = SPContext.Current.Site;
                //SPWeb web = site.OpenWeb();

                SPUtility.ValidateFormDigest();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            SPFolder folder = web.GetFolder(folderURL);

                            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));
                            //MemoryStream ms = new MemoryStream();
                            //ZipOutputStream zipStream = new ZipOutputStream(ms);

                            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression

                            zipStream.Password = genRandom(8);	// optional. Null is the same as not setting. Required if using AES.
                            m_Password = zipStream.Password;
                            //lbldatetime.Text = lbldatetime.Text + "   "+zipStream.Password;

                            CompressFolder(folder, zipStream);

                            zipStream.Finish();

                            //zipStream.IsStreamOwner = false;	

                            zipStream.Close();

                            ////var libName = System.Configuration.ConfigurationManager.AppSettings["AssetLibraryName"];
                            ////Label3.Text = "libName: "+libName;
                            //var path = @"Shared Documents\temp";
                            //SPFolder myLibrary = web.GetFolder(path); 
                            //// Prepare to upload  
                            //Boolean replaceExistingFiles = true;
                            //// Upload document  
                            //var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                            //SPFile spfile = myLibrary.Files.Add(filename, ms, replaceExistingFiles);
                            //// Commit   
                            //myLibrary.Update();

                            m_Endtime = DateTime.Now.ToString();
                            /*************************/


                            //insert into TB_Download_ZIP
                            InsertTBDownloadZIP(m_username, m_mail, "ZIP", zipFile, FileName, m_Password, true);

                            //Completed email  

                            m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
                            //m_body = "Hi, Zip File is ready, please download : " + m_downloadlink + " . <br/>Password is : " + m_Password + " <br/>";
                            //m_body += "Zip Start time : " + m_Starttime + " to End Time :" + m_Endtime + "";
                            m_body = getEmailTemplate("ZipDownloadEndEmail");
                            m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_Application_Number", m_ApplicationNumber).Replace("@@m_FileName", FileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                            sharepointsendemail(m_mail, m_subject, m_body);
                            //sharepointsendemail("andysgi@gmail.com", "hi", "ko");
                            lbldownloadmessage.Text = "Download Completed. Please check email.";
                        }
                    }
                });
            }
        }

        //compress file and folder
        private void CompressFolder(SPFolder folder, ZipOutputStream zipStream)
        {
            foreach (SPFile file in folder.Files)
            {
                //var DeletepathName = m_AttachmentPrimaryFolderName + @"\" + m_AttachmentSecondaryFolderName + @"\";
                var DeletepathName = "";
                if (!string.IsNullOrEmpty(m_AttachmentPrimaryFolderName))
                {
                    DeletepathName += m_AttachmentPrimaryFolderName + @"\";
                }
                if (!string.IsNullOrEmpty(m_AttachmentSecondaryFolderName))
                {
                    DeletepathName += m_AttachmentSecondaryFolderName + @"\";
                }
                String entryName = file.Url.Substring(DeletepathName.Length);
                ZipEntry entry = new ZipEntry(entryName);
                entry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(entry);

                byte[] binary = file.OpenBinary();
                zipStream.Write(binary, 0, binary.Length);
            }

            foreach (SPFolder subfoldar in folder.SubFolders)
            {
                CompressFolder(subfoldar, zipStream);
            }
        }

        protected void sharepointsendemail(string toAddress, string subject, string body)
        {
            SPSecurity.RunWithElevatedPrivileges(
                             delegate ()
                             {
                                 using (SPSite site = new SPSite(
                                   SPContext.Current.Site.ID,
                                   SPContext.Current.Site.Zone))
                                 {
                                     using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                                     {
                                         SPUtility.SendEmail(web, false, false,
                                                                   toAddress,
                                                                   subject,
                                                                   body);
                                     }
                                 }
                             });
        }
        protected String getEmailTemplate(string emailTemplate)
        {

            String emailTemplateContent = "";
            try
            {
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_EMAIL_TEMPLATE objTemplate = dbContext.TB_EMAIL_TEMPLATE.FirstOrDefault(x => x.Email_Template == emailTemplate);
                    emailTemplateContent = objTemplate.Email_Template_Content;
                }


            }
            catch (Exception)
            {
                throw;
            }

            return emailTemplateContent;


        }

        protected string genRandom(int numerofpassword)
        {
            string stringlist = "01234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*";
            string _num = "";
            int rnd_titleid = 0;

            Random rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 1; i <= numerofpassword; i++)
            {
                rnd_titleid = rnd.Next(0, stringlist.Length);
                _num += stringlist.Substring(rnd_titleid, 1);
            }

            return _num;
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (m_ApplicationIsInDebug == "1")
            {
                if (string.IsNullOrEmpty(m_ApplicationDebugEmailSentTo))
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDownload.Enabled = true;
                    args.IsValid = true;
                }
            }
            else
            {
                string m_mail = SPContext.Current.Web.CurrentUser.Email;
                if (string.IsNullOrEmpty(m_mail))
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDownload.Enabled = true;
                    args.IsValid = true;
                }
            }


        }

        private void InitializeUploadsDocument()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    if (objAppInit != null)
                    {
                        if (objAppInit.Endorsed_by_Cyberport.HasValue)
                        {
                            if (objAppInit.Endorsed_by_Cyberport.Value == false)
                            {
                                List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objAppInit.CASP_ID).ToList();
                                List<TB_APPLICATION_ATTACHMENT> objApplicationAttach = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Fact_Sheet.ToString().ToLower()).ToList();

                                if (objApplicationAttach.Count > 0)
                                {
                                    rptrFactFile.DataSource = objApplicationAttach;
                                    rptrFactFile.DataBind();
                                }

                                if (m_Role == "CASP Coordinator" && rbtnresult.SelectedValue == stWaiting_for_response)
                                {
                                    pnlFactFile.Visible = true;
                                }
                                else
                                {
                                    pnlFactFile.Visible = true;
                                    Fu_FactSheet.Enabled = ImageButton9.Enabled = false;
                                }

                            }
                            else
                            {
                                pnlFactFile.Visible = false;

                            }
                        }
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
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");

                using (var dbContext = new CyberportEMS_EDM())
                {

                    TB_CASP_APPLICATION objApp = GetUserApplication();

                    SPFunctions objSp = new SPFunctions();

                    if (m_Role == "CASP Coordinator" && rbtnresult.SelectedValue == stResubmitted_information)
                    {
                        lnkAttachmentDelete.Enabled = true;
                    }
                    else
                    {
                        lnkAttachmentDelete.Visible = false;
                    }
                }
            }
        }


        protected void InsertTBDownloadZIP(String User_Name, String Email, String type, String Path, String File_Name, String Password, bool Status)
        {
            List<string> errlist = new List<string>();
            try
            {
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {

                    TB_Download_ZIP objDownloadZip = new TB_Download_ZIP();
                    objDownloadZip.User_Name = User_Name;
                    objDownloadZip.Email = Email;
                    objDownloadZip.type = type;
                    objDownloadZip.Path = Path;
                    objDownloadZip.File_Name = File_Name;
                    objDownloadZip.Password = Password;
                    objDownloadZip.Status = Status;
                    objDownloadZip.Created_By = SPContext.Current.Web.CurrentUser.Name.ToString();
                    objDownloadZip.Modified_By = SPContext.Current.Web.CurrentUser.Name.ToString();
                    objDownloadZip.Created_Date = DateTime.Now;
                    objDownloadZip.Modified_Date = DateTime.Now;
                    dbContext.TB_Download_ZIP.Add(objDownloadZip);
                    dbContext.SaveChanges();
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {


                    foreach (var ve in eve.ValidationErrors)
                    {
                        errlist.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            catch (Exception e)
            {
                errlist.Add(e.Message);
            }
            lblgrouperror.DataSource = errlist;
            lblgrouperror.DataBind();

        }

        protected void RepeaterHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                Label m_lblCommentforApplicationts = (Label)e.Item.FindControl("lblCommentforApplicationts");
                Label m_lblCommentforApplicationtsLabel = (Label)e.Item.FindControl("lblCommentforApplicationtsLabel");
                Label m_lblCommentforInternualUse = (Label)e.Item.FindControl("lblCommentforInternualUse");
                Label m_lblCommentforInternualUseLabel = (Label)e.Item.FindControl("lblCommentforInternualUseLabel");


                if (String.IsNullOrEmpty(m_lblCommentforApplicationts.Text.Trim()))
                {
                    m_lblCommentforApplicationts.Visible = false;
                    m_lblCommentforApplicationtsLabel.Visible = false;
                }

                if (String.IsNullOrEmpty(m_lblCommentforInternualUse.Text.Trim()))
                {
                    m_lblCommentforInternualUse.Visible = false;
                    m_lblCommentforInternualUseLabel.Visible = false;
                }
            }
        }

        private string stSubmitted = "Submitted";
        private string stResubmitted_information = "Resubmitted information";
        private string stReject_to_Coordinator = "Reject to Coordinator";
        private string stWaiting_for_response = "Waiting for response from applicant";
        private string stTo_be_disqualified = "To be disqualified";
        private string stDisqualified = "Disqualified";
        private string stEligibility_checked = "Eligibility checked";
        private string stBDM_Approved = "BDM Approved";
        private string stBDM_Disqualified = "BDM Disqualified";
        private string stSenior_Manager_Approved = "Senior Manager Approved";
        private string stSenior_Manager_Disqualified = "Senior Manager Disqualified";
        private string stCPMO_Approved = "CPMO Approved";
        private string stCompleted = "Completed";

        protected void btnFactFileGenerate_Click(object sender, EventArgs e)
        {
            PrepareFactSheet();
        }

        protected void PrepareFactSheet()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                using (DocX doc = DocX.Create(string.Format("FactSheet-{0}.doc", DateTime.Now.Ticks)))
                {

                    int programId = objAppInit.Programme_ID;

                    //TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);

                    // Title Formatting:
                    var titleFormat = new Formatting();
                    titleFormat.FontFamily = new System.Drawing.FontFamily("Arial");
                    titleFormat.Size = 15D;
                    titleFormat.Position = 12;
                    titleFormat.Bold = true;

                    // Body Formatting
                    var paraFormat = new Formatting();
                    paraFormat.FontFamily = new System.Drawing.FontFamily("Arial");
                    paraFormat.Size = 11D;
                    titleFormat.Position = 12;

                    doc.MarginLeft = 10f;
                    doc.MarginRight = 10f;

                    //DocX letter = this.GetRejectionLetterTemplate(doc);
                    //string headerText = objAppInit.Company_Project;
                    string headerText = objAppInit.Accelerator_Name;
                    doc.InsertParagraph(Environment.NewLine);
                    Paragraph title = doc.InsertParagraph(headerText, false, titleFormat);
                    title.Alignment = Alignment.center;
                    doc.InsertParagraph(Environment.NewLine);


                    var list = doc.AddList(listType: Novacode.ListItemType.Numbered, startNumber: 1);

                    doc.AddListItem(list, "Background", 0, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, objAppInit.Background, 1, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, "Offerings to admitted start-ups ", 0, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, objAppInit.Offer, 1, listType: Novacode.ListItemType.Bulleted);

                    doc.AddListItem(list, "Fund raising capabilities of admitted start-ups", 0, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, objAppInit.Fund_Raising_Capabilities, 1, listType: Novacode.ListItemType.Bulleted);

                    doc.AddListItem(list, "Size of alumni network", 0, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, objAppInit.Size_of_Alumni, 1, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, "Info for Reputation with VCs", 0, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, objAppInit.Reputation, 1, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, "Website URL", 0, listType: Novacode.ListItemType.Bulleted);
                    doc.AddListItem(list, objAppInit.Website, 1, listType: Novacode.ListItemType.Bulleted);

                    doc.InsertList(list);
                    doc.InsertParagraph(Environment.NewLine);

                    MemoryStream ms = new MemoryStream();
                    doc.SaveAs(ms);

                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=CASP_FactSheet_" + objAppInit.Company_Project.Replace(" ", "_") + ".docx");
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    ms.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }
    }

    public class SearchResult
    {
        public string User { get; set; }
        public string Result { get; set; }
        public string CommentForApplicants { get; set; }
        public string CommentForInternualUse { get; set; }
        public string datetime { get; set; }

    }
}
