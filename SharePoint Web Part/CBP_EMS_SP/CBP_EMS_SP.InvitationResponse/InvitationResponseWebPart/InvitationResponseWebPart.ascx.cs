using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.Web.Hosting.Administration;
using System.Text.RegularExpressions;
using CBP_EMS_SP.Common;
using System.Web.UI.WebControls;
using CBP_EMS_SP.Data.Models;

namespace CBP_EMS_SP.InvitationResponse.InvitationResponseWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class InvitationResponseWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportWMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }
        private SqlConnection connection;

        private string m_VMID;
        private string m_VAID;

        public String m_path;
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "http://cyberportemssp:10869/SitePages/DownloadPage.aspx";

        private SearchResult searchResult = new SearchResult();

        private Boolean m_PopUpStatus;
        private Boolean m_SubmitValidation = false;
        private Boolean m_SubmitPresention = false;
        private Boolean m_SubmitVideo = false;

        public InvitationResponseWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_VMID = HttpContext.Current.Request.QueryString["VMID"];
            m_VAID = HttpContext.Current.Request.QueryString["VAID"];
            if (m_VAID != null && m_VMID != null)
            {
                var status = CheckIsSubmit();
                if (status == 2 || status == 3)
                {
                    ////not submited, submited 
                    PanelIR.Visible = true;
                    m_PopUpStatus = false;

                    DateTime cdeadLine = GetConfirmDeadline();
                    if (cdeadLine == DateTime.MinValue)
                    {
                        lblConfirmationDeadline.Text = "Confirmation Deadline: ";
                    }
                    else
                    {
                        lblConfirmationDeadline.Text = "Confirmation Deadline: " + cdeadLine.ToString("d MMM yyyy HH:mm tt");
                    }
                    if (cdeadLine <= DateTime.Today)
                    {
                        //submited
                        txtNameofPrincipalApplicant.ReadOnly = true;
                        txtMobileNumber.ReadOnly = true;
                        radioAttend.Enabled = false;
                        txtNameofAttendees.ReadOnly = true;
                        txtTypeofPresentationTools.ReadOnly = true;
                        txtSpecialRequest.ReadOnly = true;
                        CheckBoxCF.Enabled = false;
                        btnsubmit.Enabled = false;
                    }

                    if (!Page.IsPostBack)
                    {
                        SetScreenValue();
                    }

                    
                }
                else if (status == 1)
                {
                    //not find application
                    PanelIR.Visible = false;
                    m_PopUpStatus = true;
                    lblPopUptext.Text = "You are not invited for the presentation, please response with applicant account.";
                }
            }
        }

        protected DateTime GetConfirmDeadline()
        {
            DateTime cDeadline = new DateTime();
            ConnectOpen();
            try
            {
                var sqlString = "select isnull(ConfirmDeadline,'') as ConfirmDeadline from TB_VETTING_MEETING where Vetting_Meeting_ID = @Vetting_Meeting_ID  ";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_VMID));
                SqlDataReader  reader = command.ExecuteReader();
                if (reader.Read())
                {
                    cDeadline = reader.GetDateTime(reader.GetOrdinal("ConfirmDeadline"));
                }
                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            return cDeadline;
        }

        public int CheckIsSubmit()
        {
            var status = 0;
            ConnectOpen();
            try
            {
                var sqlString = "select tva.Attend from TB_VETTING_MEETING tvm inner join TB_VETTING_APPLICATION tva on tvm.Vetting_Meeting_ID = tva.Vetting_Meeting_ID  " +
                                "where tva.Vetting_Application_ID = @Vetting_Application_ID and tva.Vetting_Meeting_ID = @Vetting_Meeting_ID and tva.Email = @Email and tvm.Meeting_status in ('Email Sended', 'Invite Email Sent') ";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Vetting_Application_ID", m_VAID));
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_VMID));
                command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Name.ToString()));
                //command.Parameters.Add(new SqlParameter("@Meeting_status", "('Email Sended', 'Invite Email Sent')"));

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var Attend = reader.GetInt32(0);

                    if (Attend == 0)
                    {
                        //not submit,don't pop up
                        status = 2;
                    }
                    else
                    {
                        //submited,pop up
                        status = 3;
                    }

                }
                else
                {
                    //not find application,pop up
                    status = 1;
                }


                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            return status;
        }

        public void SetScreenValue()
        {
            if (m_VAID != null && m_VMID != null)
            {
                ConnectOpen();
                try
                {
                    searchResult = new SearchResult();
                    var sqlString = "SELECT TB_VETTING_MEETING.Date, TB_VETTING_MEETING.Venue,TB_PROGRAMME_INTAKE.Programme_Name, TB_PROGRAMME_INTAKE.Intake_Number, TB_VETTING_APPLICATION.Application_Number,TB_PROGRAMME_INTAKE.Programme_ID, isnull(TB_VETTING_APPLICATION.Email,'') as Email, isnull(TB_VETTING_APPLICATION.Mobile_Number,'') as Mobile_Number, isnull(TB_VETTING_APPLICATION.Attend,1) as Attend, case when TB_VETTING_APPLICATION.Name_of_Attendees is null then '' else TB_VETTING_APPLICATION.Name_of_Attendees end as Name_of_Attendees, case when TB_VETTING_APPLICATION.Presentation_Tools is null then '' else TB_VETTING_APPLICATION.Presentation_Tools end as Presentation_Tools,case when TB_VETTING_APPLICATION.Special_Request is null then '' else TB_VETTING_APPLICATION.Special_Request end as Special_Request,isnull(TB_VETTING_APPLICATION.Name_of_Principal_Applicationt,'') as Name_of_Principal_Applicationt,case when TB_VETTING_APPLICATION.Receive_Marketing_Informatioin is null then 3 else cast(TB_VETTING_APPLICATION.Receive_Marketing_Informatioin as int) end Receive_Marketing_Informatioin,TB_VETTING_APPLICATION.Presentation_From,TB_VETTING_APPLICATION.Presentation_To,isnull(TB_VETTING_MEETING.Deadline,'') as Deadline  FROM TB_VETTING_MEETING INNER JOIN TB_VETTING_APPLICATION ON TB_VETTING_MEETING.Vetting_Meeting_ID = TB_VETTING_APPLICATION.Vetting_Meeting_ID INNER JOIN TB_PROGRAMME_INTAKE ON TB_VETTING_MEETING.Programme_ID = TB_PROGRAMME_INTAKE.Programme_ID WHERE (TB_VETTING_MEETING.Vetting_Meeting_ID = @m_VMID) AND (TB_VETTING_APPLICATION.Vetting_Application_ID = @m_VAID)";

                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                    command.Parameters.Add(new SqlParameter("@m_VAID", m_VAID));

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var columnOrder = 0;
                        searchResult.Date = reader.GetDateTime(columnOrder).ToString("dd MMM yyyy");

                        columnOrder++;
                        searchResult.Venue = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.ProgrammeName = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.IntakeNo = reader.GetInt32(columnOrder).ToString();

                        columnOrder++;
                        searchResult.ApplicationNo = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.ProgrammeID = reader.GetInt32(columnOrder).ToString();

                        columnOrder++;
                        searchResult.Email = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.Mobile_Number = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.Attend = reader.GetInt32(columnOrder).ToString();

                        columnOrder++;
                        searchResult.NameofAttendees = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.PresentationTools = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.SpecialRequest = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.Name_of_Principal_Applicationt = reader.GetString(columnOrder);

                        columnOrder++;
                        var agreechecked = reader.GetInt32(columnOrder);
                        if (agreechecked == 1)
                        {
                            CheckBoxCF.Checked = true;
                        }

                        columnOrder++;
                        var presentFrom = reader.GetDateTime(columnOrder).ToString("HH:mm tt");

                        columnOrder++;
                        var presentTo = reader.GetDateTime(columnOrder).ToString("HH:mm tt");
                        var TimeSlot = " (" + presentFrom + " - " + presentTo + ")";
                        searchResult.Date += TimeSlot;

                        columnOrder++;
                        var deadline = reader.GetDateTime(columnOrder).ToString("d MMM yyyy");
                        var deadlineDatatime = reader.GetDateTime(columnOrder);
                        if (deadline == "1 Jan 1900")
                        {
                            lbldeadline.Text = "Presentation Material Submission Deadline: ";
                        }
                        else
                        {
                            lbldeadline.Text = "Presentation Material Submission Deadline: " + reader.GetDateTime(columnOrder).ToString("d MMM yyyy HH:mm tt");
                        }
                        if (DateTime.Now.Date > deadlineDatatime)
                        {
                            btnVideo.Enabled = false;
                            FileUploadPresentation.Enabled = false;
                            btnImageUpload.Enabled = false;
                            btnImageUpload.ImageUrl = "/_layouts/15/Images/CBP_Images/dir-1.png";
                            HiddenFieldisOverDeadline.Value = "1";
                        }
                        else
                        {
                            btnVideo.Enabled = true;
                            FileUploadPresentation.Enabled = true; 
                            btnImageUpload.Enabled = true;
                            btnImageUpload.ImageUrl = "/_layouts/15/Images/CBP_Images/dir.png";
                            HiddenFieldisOverDeadline.Value = "0";
                        }

                    }


                    reader.Dispose();
                    command.Dispose();

                    if (searchResult != null)
                    {
                        lblDate.Text = searchResult.Date;
                        lblVenue.Text = searchResult.Venue;
                        lblProgrammeName.Text = searchResult.ProgrammeName;
                        lblIntakeNo.Text = searchResult.IntakeNo;
                        lblApplicationNo.Text = searchResult.ApplicationNo;
                        HiddenFieldProgrammeID.Value = searchResult.ProgrammeID;
                        txtEmail.Text = searchResult.Email;
                        txtMobileNumber.Text = searchResult.Mobile_Number;
                        radioAttend.SelectedValue = searchResult.Attend;
                        txtNameofAttendees.Text = searchResult.NameofAttendees;
                        txtTypeofPresentationTools.Text = searchResult.PresentationTools;
                        txtSpecialRequest.Text = searchResult.SpecialRequest;
                        txtNameofPrincipalApplicant.Text = searchResult.Name_of_Principal_Applicationt;

                        var sqlColumn = "";
                        var sqlFrom = "";
                        if (searchResult.ProgrammeName.Contains("Cyberport Incubation Program"))
                        {
                            //CPIP
                            sqlColumn = "select isNull(tbApplicatiion.Company_Name_Chi,'') as Company_Name_Chi,Incubation_ID ";
                            sqlFrom = " from TB_INCUBATION_APPLICATION tbApplicatiion ";

                            lblCompanyName.Visible = true;
                            lblCompanyNameLabel.Visible = true;

                            lblProjectName.Visible = false;
                            lblProjectNameLabel.Visible = false;
                            lblProgrammeType.Visible = false;
                            lblProgrammeTypeLabel.Visible = false;
                            lblApplicationType.Visible = false;
                            lblApplicationTypeLabel.Visible = false;
                        }
                        else
                        {
                            //CCMF
                            sqlColumn = "select isNull(tbApplicatiion.Project_Name_Eng,'') as Project_Name_Eng,isNull(tbApplicatiion.Programme_Type,'') as Programme_Type,isNull(tbApplicatiion.CCMF_Application_Type,'') as CCMF_Application_Type,CCMF_ID ";
                            sqlFrom = " from TB_CCMF_APPLICATION tbApplicatiion ";

                            lblCompanyName.Visible = false;
                            lblCompanyNameLabel.Visible = false;

                            lblProjectName.Visible = true;
                            lblProjectNameLabel.Visible = true;
                            lblProgrammeType.Visible = true;
                            lblProgrammeTypeLabel.Visible = true;
                            lblApplicationType.Visible = true;
                            lblApplicationTypeLabel.Visible = true;
                        }
                        sqlColumn += " ,isnull(am.MobilePIN,'') as MobilePIN,isnull(am.Email,'') as Email ";
                        sqlFrom += " inner join aspnet_Users au on au.UserName = tbApplicatiion.Applicant inner join aspnet_Membership am on am.ApplicationId = au.ApplicationId and am.UserId = au.UserId ";
                        var sqlWhere = " where tbApplicatiion.Application_Number=@ApplicationNo;";
                        sqlString = sqlColumn + sqlFrom + sqlWhere;
                        command = new SqlCommand(sqlString, connection);
                        command.Parameters.Add(new SqlParameter("@ApplicationNo", searchResult.ApplicationNo));

                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var columnOrder = 0;
                            if (searchResult.ProgrammeName.Contains("Cyberport Incubation Program"))
                            {
                                //CPIP
                                searchResult.CompanyName = reader.GetString(columnOrder);

                                columnOrder++;
                                searchResult.ApplicationID = reader.GetGuid(columnOrder).ToString();
                            }
                            else
                            {
                                //CCMF
                                searchResult.ProjectName = reader.GetString(columnOrder);

                                columnOrder++;
                                searchResult.ProgrammeType = reader.GetString(columnOrder);

                                columnOrder++;
                                searchResult.ApplicationType = reader.GetString(columnOrder);

                                columnOrder++;
                                searchResult.ApplicationID = reader.GetGuid(columnOrder).ToString();

                            }

                            columnOrder++;
                            if (String.IsNullOrEmpty(searchResult.Mobile_Number))
                            {
                                searchResult.Mobile_Number = reader.GetString(columnOrder);
                            }

                            columnOrder++;
                            if (String.IsNullOrEmpty(searchResult.Email))
                            {
                                searchResult.Email = reader.GetString(columnOrder);
                            }
                        }
                        lblCompanyName.Text = searchResult.CompanyName;
                        lblProjectName.Text = searchResult.ProjectName;
                        lblProgrammeType.Text = searchResult.ProgrammeType;
                        lblApplicationType.Text = searchResult.ApplicationType;
                        HiddenFieldApplicationID.Value = searchResult.ApplicationID;
                        txtEmail.Text = searchResult.Email;
                        txtMobileNumber.Text = searchResult.Mobile_Number;

                        reader.Dispose();
                        command.Dispose();

                        if (searchResult.ApplicationID != null)
                        {
                            BindRepeaterLinksAndVideoClip(searchResult.ApplicationID);
                        }
                    }
                }
                finally
                {
                    ConnectClose();
                }
            }
        }

        public void BindRepeaterLinksAndVideoClip(String ApplicationID)
        {
            ConnectOpen();
            try{
                var sqlString = "SELECT Attachment_Type,Attachment_Path,Attachment_ID FROM TB_APPLICATION_ATTACHMENT where Application_ID = @ApplicationID and (Attachment_Type = 'Video_Clip' or Attachment_Type = 'Presentation_Slide_Response') ";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@ApplicationID", ApplicationID));

                var reader = command.ExecuteReader();
                List<linkClass> links = new List<linkClass>();
                while (reader.Read())
                {
                    if (reader.GetString(0) == "Video_Clip")
                    {
                        txtVideoClip.Text = reader.GetString(1);
                    }

                    if (reader.GetString(0) == "Presentation_Slide_Response")
                    {
                        var filePath = reader.GetString(1);
                        var filename = filePath.Split('/');

                        linkClass link = new linkClass();
                        link.NavigateUrl = @"/" + filePath;
                        link.Text = filename[filename.Length - 1];

                        link.AttachmentId = reader.GetInt32(reader.GetOrdinal("Attachment_ID"));
                        link.AttachmentPath = filePath;


                        links.Add(link);
                    }
                }

                RepeaterLinks.DataSource = links;
                RepeaterLinks.DataBind();

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        public void ConnectOpen()
        {
            connection = new SqlConnection(connStr);
            connection.Open();
        }

        private void ConnectClose()
        {
            connection.Close();
            connection.Dispose();
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {

            if (Validation() && Page.IsValid)
            {
                if (m_VAID != null && m_VMID != null)
                {
                    updateVettingApplication();

                    lblMsg.Text = "Submit Success";
                    lblerror.Text = "";
                    Context.Response.Redirect("MyApplications.aspx");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("MyApplications.aspx");
        }

        public String getApplicationVersion()
        {
            var version = "";
            ConnectOpen();
            try
            {
                var sqlString = "";
                if (lblProgrammeName.Text.Contains("Cyberport Incubation Program"))
                {
                    //CPIP
                    sqlString = "select Version_Number from TB_INCUBATION_APPLICATION where Application_Number = @Application_Number and status = 'Complete Screening'";
                }
                else
                {
                    //CCMF
                    sqlString = "select Version_Number from TB_CCMF_APPLICATION where Application_Number = @Application_Number and status = 'Complete Screening'";

                }


                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Application_Number", lblApplicationNo.Text));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    version = reader.GetString(0);
                }


                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            return version;
        }

        public Boolean ValidationVideoClip()
        {
            Regex URLregex = new Regex(@"^(https?)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.IgnoreCase);
            if (txtVideoClip.Text == "")
            {
                lblMvideo.Text = "Video Clip must be input";
                return false;
            }
            else if (!URLregex.IsMatch(txtVideoClip.Text.Trim()))
            {
                lblMvideo.Text = "Video Clip format is incorrect";
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean ValidationFileUpload()
        {
            if (!FileUploadPresentation.HasFile)
            {
                lblmsgUpload.Text = "Upload Presentation Slide file must be uploaded";
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean Validation()
        {
            bool noError = true;
            lblerrorMoble.Text = lblerrorAttendee.Text =lblerrorTool.Text = lblerrorNameOfApplicant.Text = lblerror.Text = "";
            if (radioAttend.SelectedItem.Text == "Yes")
            {
                //Regex URLregex = new Regex(@"^(https?)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.IgnoreCase);

                /*if (txtEmail.Text.Trim() == "")
                {
                    lblerror.Text = "Emal must be input";
                    result = false;
                }
*/
                if (txtMobileNumber.Text == "")
                {
                    lblerrorMoble.Text = "Mobile number must be input";
                    noError = false;
                }
                if (txtNameofAttendees.Text == "")
                {
                    lblerrorAttendee.Text = "Name of Attendees must be input";
                    noError = false;
                }
                if (txtTypeofPresentationTools.Text == "")
                {
                    lblerrorTool.Text = "Type of Presentation Tools must be input";
                    noError = false;
                }
                if (txtNameofPrincipalApplicant.Text == "")
                {
                    lblerrorNameOfApplicant.Text = "Name of principal applicant must be input";
                    noError = false;
            }
                if (!noError)
                    lblerror.Text = "Input error";

            }
            return noError;

        }

        public void updateVettingApplication()
        {
            ConnectOpen();
            try
            {
                var Checked = 0;
                if (CheckBoxCF.Checked)
                {
                    Checked = 1;
                }

                var sqlUpdate = "update TB_VETTING_APPLICATION set "
                                        + "Name_of_Principal_Applicationt = @Name_of_Principal_Applicationt, "
                                        + "Email = @Email, "
                                        + "Mobile_Number = @Mobile_Number, "
                                        + "Attend = @Attend, "
                                        + "Name_of_Attendees = @Name_of_Attendees, "
                                        + "Presentation_Tools = @Presentation_Tools, "
                                        + "Special_Request = @Special_Request, "
                                        + "Receive_Marketing_Informatioin = @Receive_Marketing_Informatioin "
                                        + "where Application_Number=@Application_Number; ";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add(new SqlParameter("@Name_of_Principal_Applicationt", txtNameofPrincipalApplicant.Text));
                command.Parameters.Add(new SqlParameter("@Email", txtEmail.Text));
                command.Parameters.Add(new SqlParameter("@Mobile_Number", txtMobileNumber.Text));
                command.Parameters.Add(new SqlParameter("@Attend", radioAttend.SelectedValue));
                command.Parameters.Add(new SqlParameter("@Name_of_Attendees", txtNameofAttendees.Text));
                command.Parameters.Add(new SqlParameter("@Presentation_Tools", txtTypeofPresentationTools.Text));
                command.Parameters.Add(new SqlParameter("@Special_Request", txtSpecialRequest.Text));
                command.Parameters.Add(new SqlParameter("@Receive_Marketing_Informatioin", Checked));
                command.Parameters.Add(new SqlParameter("@Application_Number", lblApplicationNo.Text));

                var reader = command.ExecuteNonQuery();
                command.Dispose();


            }
            finally
            {
                ConnectClose();
            }
        }

        public void UpdateApplicationAttachment(String Attachment_Type, String Application_ID, String Attachment_Path)
        {
            ConnectOpen();
            try
            {
                var sqlString = "SELECT Attachment_ID,Application_ID,Programme_ID,Attachment_Type,Attachment_Path FROM TB_APPLICATION_ATTACHMENT where Application_ID = @Application_ID and Attachment_Type = @Attachment_Type";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Application_ID", Application_ID));
                command.Parameters.Add(new SqlParameter("@Attachment_Type", Attachment_Type));

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //update
                    var sqlUpdate = "update TB_APPLICATION_ATTACHMENT set "
                                        + "Attachment_Path = @Attachment_Path, "
                                        + "Modified_By = @Modified_By, "
                                        + "Modified_Date = GETDATE() "
                                        + "where Attachment_ID=@Attachment_ID; ";

                    command = new SqlCommand(sqlUpdate, connection);
                    command.Parameters.Add(new SqlParameter("@Attachment_Path", Attachment_Path));
                    command.Parameters.Add(new SqlParameter("@Modified_By", SPContext.Current.Web.CurrentUser.Name.ToString()));
                    command.Parameters.Add(new SqlParameter("@Attachment_ID", reader.GetInt32(0).ToString()));

                    command.ExecuteNonQuery();
                }
                else
                {
                    //insert
                    var sqlInsert = "insert into TB_APPLICATION_ATTACHMENT(Application_ID,Programme_ID,Attachment_Type,Attachment_Path,Created_By,Created_Date,Modified_By,Modified_Date) values( "
                                        + "@Application_ID, "
                                        + "@Programme_ID, "
                                        + "@Attachment_Type, "
                                        + "@Attachment_Path, "
                                        + "@user, "
                                        + "GETDATE(), "
                                        + "@user, "
                                        + "GETDATE() "
                                        + " ) ;";

                    command = new SqlCommand(sqlInsert, connection);
                    command.Parameters.Add(new SqlParameter("@Application_ID", Application_ID));
                    command.Parameters.Add(new SqlParameter("@Programme_ID", HiddenFieldProgrammeID.Value));
                    command.Parameters.Add(new SqlParameter("@Attachment_Type", Attachment_Type));
                    command.Parameters.Add(new SqlParameter("@Attachment_Path", Attachment_Path));
                    command.Parameters.Add(new SqlParameter("@user", SPContext.Current.Web.CurrentUser.Name.ToString()));

                    command.ExecuteNonQuery();
                }

                reader.Dispose();
                command.Dispose();


            }
            finally
            {
                ConnectClose();
            }
        }

        public void InsertApplicationAttachment(String Attachment_Type, String Application_ID, String Attachment_Path)
        {
            ConnectOpen();
            try
            {
                //insert
                var sqlInsert = "insert into TB_APPLICATION_ATTACHMENT(Application_ID,Programme_ID,Attachment_Type,Attachment_Path,Created_By,Created_Date,Modified_By,Modified_Date) values( "
                                    + "@Application_ID, "
                                    + "@Programme_ID, "
                                    + "@Attachment_Type, "
                                    + "@Attachment_Path, "
                                    + "@user, "
                                    + "GETDATE(), "
                                    + "@user, "
                                    + "GETDATE() "
                                    + " ) ;";

                var command = new SqlCommand(sqlInsert, connection);
                command.Parameters.Add(new SqlParameter("@Application_ID", Application_ID));
                command.Parameters.Add(new SqlParameter("@Programme_ID", HiddenFieldProgrammeID.Value));
                command.Parameters.Add(new SqlParameter("@Attachment_Type", Attachment_Type));
                command.Parameters.Add(new SqlParameter("@Attachment_Path", Attachment_Path));
                command.Parameters.Add(new SqlParameter("@user", SPContext.Current.Web.CurrentUser.Name.ToString()));

                command.ExecuteNonQuery();

                command.Dispose();


            }
            finally
            {
                ConnectClose();
            }
        }

        public void UploadFiletoSharepointDocument(String PrimaryFolderName, String FileName)
        {
            if (FileUploadPresentation.HasFile)
            {
                SPSite site = SPContext.Current.Site;
                SPWeb web = site.OpenWeb();

                SPFolder myLibrary = web.GetFolder(PrimaryFolderName);
                var fileNames = FileName.Split('/');
                for (int i = 0; i < fileNames.Length - 1; i++)
                {
                    myLibrary = checkFolderexist(myLibrary, fileNames[i]);
                }

                // Prepare to upload  
                Boolean replaceExistingFiles = true;
                // Upload document  
                var fileByteArray = FileUploadPresentation.FileBytes;
                SPFile spfile = myLibrary.Files.Add(FileUploadPresentation.FileName, fileByteArray, replaceExistingFiles);
                // Commit   
                myLibrary.Update();
            }
        }
        public SPFolder checkFolderexist(SPFolder folder, String folderName)
        {
            SPFolder folderResult = null;
            Boolean exit = false;
            foreach (SPFolder subfolder in folder.SubFolders)
            {
                if (subfolder.Name == folderName)
                {
                    exit = true;
                    folderResult = subfolder;
                    break;
                }
            }

            if (!exit)
            {
                folder.SubFolders.Add(folderName);
                folderResult = folder.SubFolders[folderName];
            }

            return folderResult;
        }


        protected void getSYSTEMPARAMETER()
        {
            ConnectOpen();
            try
            {
                var sqlString = "select Config_Code,Value from TB_SYSTEM_PARAMETER;";

                var command = new SqlCommand(sqlString, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == "AttachmentPrimaryFolder")
                    {
                        //Shared Documents
                        m_AttachmentPrimaryFolderName = reader.GetString(1);
                    }

                    if (reader.GetString(0) == "AttachmentSecondaryFolder")
                    {
                        //ApplicationAttachments
                        m_AttachmentSecondaryFolderName = reader.GetString(1);
                    }

                    if (reader.GetString(0) == "zipfiledownloadurl")
                    {
                        /*hhttp://cyberportemssp:10870/*/
                        m_zipfiledownloadurl = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "ApplicationIsInDebug")
                    {
                        //1,0
                        m_ApplicationIsInDebug = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "ApplicationDebugEmailSentTo")
                    {
                        m_ApplicationDebugEmailSentTo = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "zipfiledownloadpath")
                    {
                        m_path = reader.GetString(1);

                    }
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void CustomValidatorFileUpload_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string fileName = FileUploadPresentation.FileName;
            string extension = System.IO.Path.GetExtension(fileName);
            Boolean validation = true;
            if (!((extension.ToLower() == ".ppt") || (extension.ToLower() == ".pptx") || (extension.ToLower() == ".pdf") || (extension.ToLower() == ".png") ||
                (extension.ToLower() == ".jpg") || (extension.ToLower() == ".gif") || (extension.ToLower() == ".tiff") || (extension.ToLower() == ".html") ||
                (extension.ToLower() == ".odt") || (extension.ToLower() == ".pages") || (extension.ToLower() == ".wmv") || (extension.ToLower() == ".avi") ||
                (extension.ToLower() == ".mp4") || (extension.ToLower() == ".m4a") || (extension.ToLower() == ".mov")))
            {
                CustomValidatorFileUpload.ErrorMessage = "File Type is limited to PPT, PPTX, PDF, PNG, JPG, GIF";
                validation = false;
            }

            int fileSize = FileUploadPresentation.PostedFile.ContentLength;
            if (fileSize > 5242880)
            {
                CustomValidatorFileUpload.ErrorMessage = "Filesize is limited to 5MB";
                validation = false;
            }

            if (validation)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }

        }

        protected void CustomValidatorMobileNumber_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            

            //Matches	123-12-1233 | (02717)230464 | +919427237800 | +9427237800 | 02717-230464
            //Non-Matches	23046 | (027)17-230a4d64a
            //Regex rgx = new Regex(@"[\+]{0,1}(\d{10,13}|[\(][\+]{0,1}\d{2,}[\13)]*\d{5,13}|\d{2,6}[\-]{1}\d{2,13}[\-]*\d{3,13})");
            Regex rgx = new Regex(@"[\d-+()]");

            //Regex rgx = new Regex(@"^(\(\+\d*\)[- ]\d*|\+\d*[- ]\d*|\d*)$");
            
            if (rgx.IsMatch(args.Value.ToString()))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void CustomValidatorEmail_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            //Regex rgx = new Regex(@"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$");
            Regex rgx = new Regex(@"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$");
            
            if (rgx.IsMatch(args.Value.ToString()))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void btnPopUpIR_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("MyApplications.aspx");
        }

        protected void btnVideo_Click(object sender, EventArgs e)
        {
            if (HiddenFieldisOverDeadline.Value == "0")
            {
                if (ValidationVideoClip() && Page.IsValid)
                {
                    if (m_VAID != null && m_VMID != null)
                    {
                        UpdateApplicationAttachment("Video_Clip", HiddenFieldApplicationID.Value, txtVideoClip.Text);

                        lblMvideo.Text = "";
                    }
                    m_SubmitVideo = true;
                }
            }
        }

        protected void btnImageUpload_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (HiddenFieldisOverDeadline.Value == "0")
            {
                if (ValidationFileUpload() && Page.IsValid)
                {
                    if (m_VAID != null && m_VMID != null)
                    {
                        getSYSTEMPARAMETER();

                        if (FileUploadPresentation.HasFile)
                        {
                            var PrimaryFolderName = m_AttachmentPrimaryFolderName;
                            var FileName = "";
                            if (m_AttachmentSecondaryFolderName != "")
                            {
                                FileName += m_AttachmentSecondaryFolderName + "/";
                            }
                            FileName += lblProgrammeName.Text + " " + lblIntakeNo.Text + "/" + lblApplicationNo.Text + "/" + FileUploadPresentation.FileName;

                            SPFunctions objSPFunctions = new SPFunctions();
                            CBP_EMS_SP.Data.Models.TB_PROGRAMME_INTAKE objProgram = new Data.Models.TB_PROGRAMME_INTAKE();
                            objProgram.Programme_Name = lblProgrammeName.Text;
                            objProgram.Intake_Number = int.Parse(lblIntakeNo.Text);
                            //var version = getApplicationVersion();
                            //version = (float.Parse(version) + 0.1).ToString("N1");
                            var Maxversion = 0;
                            foreach (RepeaterItem item in RepeaterLinks.Items)
                            {
                                var fname = (HyperLink)item.FindControl("HyperLinkFileName");
                                
                                var Fname1 = fname.Text.Split('.')[0];
                                var Responseindex = Fname1.IndexOf("Slide_Response");
                                var number = int.Parse(Fname1.Substring(Responseindex + 14));
                                if (Maxversion < number)
                                {
                                    Maxversion = number;
                                }
                            }
                            var version = Maxversion.ToString();
                            var fileUrl = objSPFunctions.AttachmentSave(lblApplicationNo.Text, objProgram, FileUploadPresentation, enumAttachmentType.Presentation_Slide_Response, version);
                            InsertApplicationAttachment("Presentation_Slide_Response", HiddenFieldApplicationID.Value, fileUrl);

                            BindRepeaterLinksAndVideoClip(HiddenFieldApplicationID.Value);

                            //var fArray = fileUrl.Split('/');
                            //HyperLinkFileName.NavigateUrl = @"/" + fileUrl;
                            //HyperLinkFileName.Text = fArray[fArray.Length - 1];
                            //UploadFiletoSharepointDocument(PrimaryFolderName, FileName);
                            //UpdateApplicationAttachment("Presentation_Slide", HiddenFieldApplicationID.Value, PrimaryFolderName + "/" + FileName);

                            lblmsgUpload.Text = "";
                            m_SubmitPresention = true;
                        }
                     }
                }
            }
       }

        protected void RepeaterLinks_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {

            var itemindex = int.Parse(e.CommandArgument.ToString());
            if (itemindex >= 0)
            {
                RepeaterItem Repeateritem = RepeaterLinks.Items[itemindex];
                var AttachmentId = (HiddenField)Repeateritem.FindControl("HiddenFieldAttachmentId");
                var AttachmentPath = (HiddenField)Repeateritem.FindControl("HiddenFieldAttachmentPath");

                DeleteApplicationAttachment(AttachmentId.Value);

                TB_APPLICATION_ATTACHMENT applicationAttchment = new TB_APPLICATION_ATTACHMENT();
                applicationAttchment.Attachment_ID = int.Parse(AttachmentId.Value);
                applicationAttchment.Attachment_Path = AttachmentPath.Value;

                //delete share point file
                SPFunctions.DeleteAttachmentfromlist(applicationAttchment);

                BindRepeaterLinksAndVideoClip(HiddenFieldApplicationID.Value);
               
            }
            
        }

        public void DeleteApplicationAttachment(String AttachmentId)
        {
            ConnectOpen();
            try
            {
                //update
                var sqlUpdate = "delete FROM TB_APPLICATION_ATTACHMENT where "
                                    + "Attachment_ID = @Attachment_ID; ";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add(new SqlParameter("@Attachment_ID", AttachmentId));
                command.ExecuteNonQuery();

                command.Dispose();


            }
            finally
            {
                ConnectClose();
            }
        }

        protected void btnsubmit_Click1(object sender, EventArgs e)
        {
            if (Validation() && Page.IsValid)
            {
                m_SubmitValidation = true;
            }
            else
            {
                m_SubmitValidation = false;
            }
        }
    }

    public class SearchResult
    {
        public string Date { get; set; }
        public string Venue { get; set; }
        public string ProgrammeName { get; set; }
        public string IntakeNo { get; set; }
        public string ApplicationNo { get; set; }
        public string CompanyName	 { get; set; }
        public string ProjectName { get; set; }
        public string ProgrammeType { get; set; }
        public string ApplicationType { get; set; }
        public string VideoClip { get; set; }
        public string PresentationSlide { get; set; }
        public string ApplicationID { get; set; }
        public string ProgrammeID { get; set; }
        public string Email { get; set; }
        public string Mobile_Number { get; set; }
        public string Attend { get; set; }
        public string NameofAttendees { get; set; }
        public string PresentationTools { get; set; }
        public string SpecialRequest { get; set; }
        public string Name_of_Principal_Applicationt { get; set; }
        public Boolean Receive_Marketing_Informatioin { get; set; }
        
        
    }

    public class linkClass
    {
        public string NavigateUrl { get; set; }
        public string Text { get; set; }

        public string AttachmentPath { get; set; }
        public int AttachmentId { get; set; }
    }
}
