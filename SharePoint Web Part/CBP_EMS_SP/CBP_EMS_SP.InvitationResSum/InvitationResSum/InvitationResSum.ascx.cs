using ICSharpCode.SharpZipLib.Zip;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using Microsoft.SharePoint.Utilities;
using System.Data;
using System.IO;


namespace CBP_EMS_SP.InvitationResSum.InvitationResSum
{
    [ToolboxItemAttribute(false)]
    public partial class InvitationResSum : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        string m_prog = "";
        string m_systemuser = "";
        string m_Role = "";
        string m_Programme_Type = "";
        string m_Meeting_status = "";
        string m_Programme_ID = "";
        string m_Programme_Name = "";
        string m_VMID = "";
        DateTime m_Presentation_From = DateTime.Now;
        DateTime m_Presentation_To = DateTime.Now;

        DateTime m_VettingDate = DateTime.Now;
        string m_VettingVenue = "";
        DateTime m_VettingMettingFrom = DateTime.Now;
        DateTime m_VettingMeetingTo = DateTime.Now;
        private SqlConnection connection;

        public String m_programName;
        public String m_intake;
        public String m_path = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "";

        public String IsLogEvent;
        public double m_DownloadFileSize = 0;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public class AttachmentList
        { 
            public string Attachment_Type { set; get; }
            public string Attachment_Path { set; get; }
        }

        public class TimeSlotList
        {
            public string Vetting_Application_ID { set; get; }
            public string Vetting_Meeting_ID { set; get; }
            public string TimeSlot { set; get; }
            public string Application_Number { set; get; }
            public string Company { set; get; }
            public string Project_Name_Eng { set; get; }
            public string Programme_Type { set; get; }
            public string CCMF_Application_Type { set; get; }
            public string Cluster { set; get; }
            public string Attendance { set; get; }
            public string CustDisplay { set; get; }
            public string Presentation_From { set; get; }
            public string Presentation_To { set; get; }
            public string Shortlisted { set; get; }
            public string RowNumber { set; get; }
            public DateTime DT_Presentation_From { set; get; }
            public DateTime DT_Presentation_To { set; get; }
            public string Email { set; get; }
            public string Mobile_Number { set; get; }
            public string Name_of_Principal_Applicant { set; get; }
            public string Name_of_Attendees { set; get; }
            public string Presentation_Tools { set; get; }
            public string Special_Request { set; get; }
            public string Agreement { set; get; }

        }

        public InvitationResSum()
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
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();

            if (string.IsNullOrEmpty(m_VMID))
            {
                return;
            }

            getSYSTEMPARAMETER();
            getReview();
            getVettingMeetingInfo();

            if (!Page.IsPostBack)
            {
                getdbdata();
            }
            else
            {
              
            }


        }

        protected bool checkAttFileZip(string m_filenamePath)
        {
            bool m_answer = false;

            foreach (GridViewRow row in GridView_App.Rows)
            {
                Label lblUploadPresentationSlide_full = (Label)row.FindControl("lblUploadPresentationSlide_full");

                if (lblUploadPresentationSlide_full.Text.IndexOf(m_filenamePath) >= 0)
                {
                    m_answer = true;
                    break;
                }


            }

            return m_answer;
        }

        protected void getdbdata()
        {
            getVettingApplication();
            //UnassignedApplicants();
            //ClusterDDl(ddlCluster, GridViewUA, "");
            //checkTimeSlotoutofrang();
        }

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
           
        }

        protected void getVettingMeetingInfo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";
                sql = "SELECT dbo.TB_PROGRAMME_INTAKE.Programme_ID,dbo.TB_PROGRAMME_INTAKE.Programme_Name, dbo.TB_PROGRAMME_INTAKE.Intake_Number, dbo.TB_VETTING_MEETING.Vetting_Meeting_ID, dbo.TB_VETTING_MEETING.Programme_ID, "
                    + "dbo.TB_VETTING_MEETING.Date, dbo.TB_VETTING_MEETING.Venue, dbo.TB_VETTING_MEETING.Vetting_Meeting_From, dbo.TB_VETTING_MEETING.Vetting_Meeting_To, "
                    + "dbo.TB_VETTING_MEETING.Presentation_From, dbo.TB_VETTING_MEETING.Presentation_To, dbo.TB_VETTING_MEETING.Vetting_Team_Leader, dbo.TB_VETTING_MEETING.No_of_Attendance, "
                    + "dbo.TB_VETTING_MEETING.Created_By, dbo.TB_VETTING_MEETING.Created_Date, dbo.TB_VETTING_MEETING.Modified_By, dbo.TB_VETTING_MEETING.Modified_Date, "
                    + "dbo.TB_VETTING_MEETING.Time_Interval, dbo.TB_VETTING_MEETING.Meeting_status "
                    + "FROM dbo.TB_VETTING_MEETING INNER JOIN dbo.TB_PROGRAMME_INTAKE ON dbo.TB_VETTING_MEETING.Programme_ID = dbo.TB_PROGRAMME_INTAKE.Programme_ID "
                    + "WHERE (dbo.TB_VETTING_MEETING.Vetting_Meeting_ID = @m_VMID)";

                //lbltest.Text = sql;

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@m_VMID", m_VMID);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        //lblTime_Interval.Text = reader.GetValue(reader.GetOrdinal("Time_Interval")).ToString() + " mins";
                        //m_Time_Interval = reader.GetValue(reader.GetOrdinal("Time_Interval")).ToString();
                        lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                        m_Programme_Name = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                        lblIntakeNo.Text = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                        //m_Intake_Number = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                        m_Programme_ID = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString();
                        //lblm_Programme_ID.Text = m_Programme_ID;
                        m_VettingDate = reader.GetDateTime(reader.GetOrdinal("Date"));
                        lblDate.Text = m_VettingDate.ToString("dd MMMM, yyyy");
                        m_VettingVenue = reader.GetValue(reader.GetOrdinal("Venue")).ToString();
                        lblVenue.Text = m_VettingVenue;
                        m_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From"));
                        m_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To"));
                        m_VettingMettingFrom = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_From"));
                        m_VettingMeetingTo = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_To"));
                        //lblPresentation_From.Text = m_Presentation_From.ToString();
                        //lblPresentation_To.Text = m_Presentation_To.ToString();
                        m_Meeting_status = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString();
                    }
                    conn.Close();
                }

                m_Programme_Type = checkProgrammeType(m_Programme_ID);
                //lbltest.Text += m_Programme_Type + "<br/>";
                if (m_Programme_Type == "CPIP")
                {
                    GridView_App.Columns[2].Visible = false;
                    GridView_App.Columns[3].Visible = false;
                    GridView_App.Columns[4].Visible = false;
                }
                else
                {
                    GridView_App.Columns[1].Visible = false;
                }
            }
        }

        protected List<TimeSlotList> getVettingApplicationList()
        {

            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();

            string sql = string.Empty;

            if (m_Programme_Type == "CCMF")
            {
                sql = "SELECT vApp.Vetting_Application_ID, "
                        + "vApp.Vetting_Meeting_ID, "
                        + "vApp.Application_Number, "
                        + "vApp.Presentation_From, "
                        + "vApp.Presentation_To, "
                        + "vApp.Email, "
                        + "vApp.Mobile_Number, "
                        + "vApp.Attend, "
                        + "vApp.Name_of_Attendees, "
                        + "vApp.Presentation_Tools, "
                        + "vApp.Special_Request, "
                        + "vApp.Name_of_Principal_Applicationt, "
                        + "vApp.Receive_Marketing_Informatioin, "
                        + "CCMF.Project_Name_Eng, "
                        + "CCMF.Programme_Type, "
                        + "CCMF.CCMF_Application_Type, "
                        + "CCMF.Business_Area "
                    + "FROM TB_VETTING_APPLICATION vApp "
                    + "INNER JOIN TB_CCMF_APPLICATION CCMF ON vApp.Application_Number = CCMF.Application_Number "
                    + "WHERE vApp.Vetting_Meeting_ID = @m_VMID "
                        + "AND CCMF.Status <> 'Saved' "
                        + "AND CCMF.Status IN ('Complete Screening', 'Presentation Withdraw', 'Awarded') "
                    + "ORDER BY Presentation_From";
            }
            if (m_Programme_Type == "CPIP")
            {
                sql = "SELECT vApp.Vetting_Application_ID, "
                        + "vApp.Vetting_Meeting_ID, "
                        + "vApp.Application_Number, "
                        + "vApp.Presentation_From, "
                        + "vApp.Presentation_To, "
                        + "vApp.Email, "
                        + "vApp.Mobile_Number, "
                        + "vApp.Attend, "
                        + "vApp.Name_of_Attendees, "
                        + "vApp.Presentation_Tools, "
                        + "vApp.Special_Request, "
                        + "vApp.Name_of_Principal_Applicationt, "
                        + "vApp.Receive_Marketing_Informatioin, "
                        + "CPIP.Company_Name_Eng, "
                        + "CPIP.Business_Area "
                   + "FROM TB_VETTING_APPLICATION vApp "
                   + "INNER JOIN TB_INCUBATION_APPLICATION CPIP ON vApp.Application_Number = CPIP.Application_Number "
                   + "WHERE vApp.Vetting_Meeting_ID = @m_VMID "
                        + "AND CPIP.Status <> 'Saved' "
                        + "AND CPIP.Status IN ('Complete Screening', 'Presentation Withdraw', 'Awarded') "
                   + "ORDER BY Presentation_From";
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@m_VMID", m_VMID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (m_Programme_Type == "CCMF")
                            {
                                lstTimeSlotList.Add(new TimeSlotList
                                {
                                    TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                                    Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                                    DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                                    Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                                    DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                                    Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                    Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                    Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                                    CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                                    Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                    Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                                    Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                                    Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString(),
                                    Email = reader.GetValue(reader.GetOrdinal("Email")).ToString(),
                                    Mobile_Number = reader.GetValue(reader.GetOrdinal("Mobile_Number")).ToString(),
                                    Name_of_Attendees = reader.GetValue(reader.GetOrdinal("Name_of_Attendees")).ToString(),
                                    Presentation_Tools = reader.GetValue(reader.GetOrdinal("Presentation_Tools")).ToString(),
                                    Special_Request = reader.GetValue(reader.GetOrdinal("Special_Request")).ToString(),
                                    Agreement = reader.IsDBNull(reader.GetOrdinal("Receive_Marketing_Informatioin")) ? "" : AgreementStatus(reader.GetBoolean(reader.GetOrdinal("Receive_Marketing_Informatioin"))),
                                    Name_of_Principal_Applicant = reader.GetValue(reader.GetOrdinal("Name_of_Principal_Applicationt")).ToString()
                                });
                            }


                            if (m_Programme_Type == "CPIP")
                            {
                                lstTimeSlotList.Add(new TimeSlotList
                                {
                                    TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                                    Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                                    DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                                    Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                                    DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                                    Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                    Company = reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),
                                    Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                    Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                                    Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                                    Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString(),
                                    Email = reader.GetValue(reader.GetOrdinal("Email")).ToString(),
                                    Mobile_Number = reader.GetValue(reader.GetOrdinal("Mobile_Number")).ToString(),
                                    Name_of_Attendees = reader.GetValue(reader.GetOrdinal("Name_of_Attendees")).ToString(),
                                    Presentation_Tools = reader.GetValue(reader.GetOrdinal("Presentation_Tools")).ToString(),
                                    Special_Request = reader.GetValue(reader.GetOrdinal("Special_Request")).ToString(),
                                    Agreement = reader.IsDBNull(reader.GetOrdinal("Receive_Marketing_Informatioin")) ? "" : AgreementStatus(reader.GetBoolean(reader.GetOrdinal("Receive_Marketing_Informatioin"))),
                                    Name_of_Principal_Applicant = reader.GetValue(reader.GetOrdinal("Name_of_Principal_Applicationt")).ToString()
                                });
                            }


                        }
                    }
                    conn.Close();
                }
            }
            return lstTimeSlotList;
        }

        protected void getVettingApplication()
        {
            //lbltest.Text += "getVettingApplication<br/>"+ DateTime.Now.ToString();
            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "";

                if (m_Programme_Type == "CCMF")
                {
                    sql = "SELECT dbo.TB_VETTING_APPLICATION.Vetting_Application_ID, dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID, dbo.TB_VETTING_APPLICATION.Application_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Presentation_From, dbo.TB_VETTING_APPLICATION.Presentation_To, dbo.TB_VETTING_APPLICATION.Email, dbo.TB_VETTING_APPLICATION.Mobile_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Attend, Name_of_Principal_Applicationt, Receive_Marketing_Informatioin, dbo.TB_VETTING_APPLICATION.Name_of_Attendees, dbo.TB_VETTING_APPLICATION.Presentation_Tools, dbo.TB_VETTING_APPLICATION.Special_Request, "
                    + "dbo.TB_CCMF_APPLICATION.Project_Name_Eng, dbo.TB_CCMF_APPLICATION.Programme_Type, dbo.TB_CCMF_APPLICATION.CCMF_Application_Type, dbo.TB_CCMF_APPLICATION.Business_Area "
                    + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                    + "dbo.TB_CCMF_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_CCMF_APPLICATION.Application_Number "
                    + "WHERE dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = @m_VMID AND dbo.TB_CCMF_APPLICATION.Status <> 'Saved' AND dbo.TB_CCMF_APPLICATION.Status IN ('Complete Screening', 'Presentation Withdraw', 'Awarded')"
                    + "ORDER BY Presentation_From";
                }
                else
                {
                    sql = "SELECT dbo.TB_VETTING_APPLICATION.Vetting_Application_ID, dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID, dbo.TB_VETTING_APPLICATION.Application_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Presentation_From, dbo.TB_VETTING_APPLICATION.Presentation_To, dbo.TB_VETTING_APPLICATION.Email, dbo.TB_VETTING_APPLICATION.Mobile_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Attend, Name_of_Principal_Applicationt, Receive_Marketing_Informatioin, dbo.TB_VETTING_APPLICATION.Name_of_Attendees, dbo.TB_VETTING_APPLICATION.Presentation_Tools, dbo.TB_VETTING_APPLICATION.Special_Request, "
                    + "dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng, dbo.TB_INCUBATION_APPLICATION.Business_Area "
                    + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                    + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_INCUBATION_APPLICATION.Application_Number "
                    + "WHERE dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = @m_VMID AND dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved' AND dbo.TB_INCUBATION_APPLICATION.Status IN ('Complete Screening', 'Presentation Withdraw', 'Awarded')"
                    + "ORDER BY Presentation_From";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                    conn.Open();
                    try 
                    { 
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (m_Programme_Type == "CCMF")
                        {
                            lstTimeSlotList.Add(new TimeSlotList
                            {
                                TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                                Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                                DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                                Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                                DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                                Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                                CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                                Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                                Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                                Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString(),
                                Email = reader.GetValue(reader.GetOrdinal("Email")).ToString(),
                                Mobile_Number = reader.GetValue(reader.GetOrdinal("Mobile_Number")).ToString(),
                                Name_of_Attendees = reader.GetValue(reader.GetOrdinal("Name_of_Attendees")).ToString(),
                                Presentation_Tools = reader.GetValue(reader.GetOrdinal("Presentation_Tools")).ToString(),
                                Special_Request = reader.GetValue(reader.GetOrdinal("Special_Request")).ToString(),
                                Agreement = reader.IsDBNull(reader.GetOrdinal("Receive_Marketing_Informatioin")) ? "" : AgreementStatus(reader.GetBoolean(reader.GetOrdinal("Receive_Marketing_Informatioin"))),
                                Name_of_Principal_Applicant = reader.GetValue(reader.GetOrdinal("Name_of_Principal_Applicationt")).ToString()
                            });
                            
                        }
                        else
                        {
                            lstTimeSlotList.Add(new TimeSlotList
                            {
                                TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                                Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                                DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                                Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                                DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                                Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                Company = reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),
                                Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                                Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                                Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString(),
                                Email = reader.GetValue(reader.GetOrdinal("Email")).ToString(),
                                Mobile_Number = reader.GetValue(reader.GetOrdinal("Mobile_Number")).ToString(),
                                Name_of_Attendees = reader.GetValue(reader.GetOrdinal("Name_of_Attendees")).ToString(),
                                Presentation_Tools = reader.GetValue(reader.GetOrdinal("Presentation_Tools")).ToString(),
                                Special_Request = reader.GetValue(reader.GetOrdinal("Special_Request")).ToString(),
                                Agreement = reader.IsDBNull(reader.GetOrdinal("Receive_Marketing_Informatioin")) ? "" : AgreementStatus(reader.GetBoolean(reader.GetOrdinal("Receive_Marketing_Informatioin"))),
                                Name_of_Principal_Applicant = reader.GetValue(reader.GetOrdinal("Name_of_Principal_Applicationt")).ToString()

                            });
                        }

                    }

                    }
                    finally
                    {
                        conn.Close();
                    }
                    
                }
            }




            //lstTimeSlotList = insertEmptySlot(lstTimeSlotList);

            //lstTimeSlotList.Sort((x, y) => DateTime.Compare(x.DT_Presentation_From, y.DT_Presentation_From));

            GridView_App.DataSource = lstTimeSlotList;
            GridView_App.DataBind();

        }

        protected List<AttachmentList> getAttachment(string m_Application_Number)
        {
            List<AttachmentList> lstAttachmentList = new List<AttachmentList>();

                string sql = "";

                if (m_Programme_Type == "CCMF")
                {
                    sql = "SELECT dbo.TB_CCMF_APPLICATION.[CCMF_ID], dbo.TB_CCMF_APPLICATION.Application_Number, "
                     + "dbo.TB_CCMF_APPLICATION.Status, dbo.TB_APPLICATION_ATTACHMENT.Attachment_Type, "
                    + "dbo.TB_APPLICATION_ATTACHMENT.Attachment_Path "
                    + "FROM dbo.TB_CCMF_APPLICATION INNER JOIN "
                    + "dbo.TB_APPLICATION_ATTACHMENT ON dbo.TB_CCMF_APPLICATION.[CCMF_ID] = dbo.TB_APPLICATION_ATTACHMENT.Application_ID "
                    + "WHERE (dbo.TB_CCMF_APPLICATION.Status <> 'Saved') AND (dbo.TB_CCMF_APPLICATION.Status = 'Complete Screening') AND "
                    + "(dbo.TB_CCMF_APPLICATION.Application_Number = @Application_Number) AND (dbo.TB_APPLICATION_ATTACHMENT.Attachment_Type IN ('Video_Clip', 'Presentation_Slide_Response')) ";
                    //+ "(dbo.TB_CCMF_APPLICATION.Application_Number = @Application_Number) AND (dbo.TB_APPLICATION_ATTACHMENT.Attachment_Type IN ('Video_Clip', 'Presentation_Slide')) ";

                }
                else
                {
                    sql = "SELECT dbo.TB_INCUBATION_APPLICATION.[Incubation_ID], dbo.TB_INCUBATION_APPLICATION.Application_Number, "
                      + "dbo.TB_INCUBATION_APPLICATION.Status, dbo.TB_APPLICATION_ATTACHMENT.Attachment_Type, "
                     + "dbo.TB_APPLICATION_ATTACHMENT.Attachment_Path "
                     + "FROM dbo.TB_INCUBATION_APPLICATION INNER JOIN "
                     + "dbo.TB_APPLICATION_ATTACHMENT ON dbo.TB_INCUBATION_APPLICATION.[Incubation_ID] = dbo.TB_APPLICATION_ATTACHMENT.Application_ID "
                     + "WHERE dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved' AND dbo.TB_INCUBATION_APPLICATION.Status = 'Complete Screening' AND "
                     + "dbo.TB_INCUBATION_APPLICATION.Application_Number = @Application_Number AND dbo.TB_APPLICATION_ATTACHMENT.Attachment_Type IN ('Video_Clip', 'Presentation_Slide_Response') ";
                    //+ "dbo.TB_INCUBATION_APPLICATION.Application_Number = @Application_Number AND dbo.TB_APPLICATION_ATTACHMENT.Attachment_Type IN ('Video_Clip', 'Presentation_Slide') ";

                }

                
                
                using (SqlConnection conn = new SqlConnection(connStr))
                {

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Application_Number", m_Application_Number));
                        conn.Open();
                        try
                        {
                            var reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                lstAttachmentList.Add(new AttachmentList
                                {
                                    Attachment_Path = reader.GetValue(reader.GetOrdinal("Attachment_Path")).ToString(),
                                    Attachment_Type = reader.GetValue(reader.GetOrdinal("Attachment_Type")).ToString()
                                });
                            }
                        }
                        finally 
                        {
                            conn.Close();
                        }
                        
                    }
                }

                return lstAttachmentList;

        }

        protected string findFilename(string filepath)
        {
            var position = filepath.LastIndexOf(@"/");
            return filepath.Substring(position + 1, filepath.Length - position - 1);
        }
        protected string AttendanceStatus(int m_Attend)
        {
            string m_AttendanceStatus = "";
            if (m_Attend == 0)
            {
                m_AttendanceStatus = "Pending";
            }
            else if (m_Attend == 1)
            {
                m_AttendanceStatus = "Yes";
            }
            else if (m_Attend == 2)
            {
                m_AttendanceStatus = "No";
            }

            return m_AttendanceStatus;
        }

        protected string AgreementStatus(bool m_agree) {
            string m_AgreementStatus = "";

            if (m_agree)
            {
                m_AgreementStatus = "agree";
            }
            else
            {
                m_AgreementStatus = "disagree";
            }

            return m_AgreementStatus;
        }

        protected string checkProgrammeType(string m_Programme_ID)
        {
            var m_INCUBATION = 0;
            var m_CCMF = 0;
            var m_result = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Programme_ID = @m_Programme_ID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@m_Programme_ID", m_Programme_ID));
                    conn.Open();
                    try 
                    { 
                        m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    finally 
                    { 
                        conn.Close();
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE Programme_ID = @m_Programme_ID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@m_Programme_ID", m_Programme_ID));
                    conn.Open();
                    try { 
                        m_CCMF = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                    
                }
            }

            if (m_CCMF == 0 && m_INCUBATION > 0)
            {
                m_result = "CPIP";
            }
            else if (m_CCMF > 0 && m_INCUBATION == 0)
            {
                m_result = "CCMF";
            }
            else if (m_CCMF == 0 && m_INCUBATION == 0)
            {
                m_result = "not data";
            }
            return m_result;

        }

        protected void GridView_App_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GridView_App_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblApplication_Number = (Label)e.Row.FindControl("lblApplication_Number");
                Label lblVideoClip = (Label)e.Row.FindControl("lblVideoClip");
                Label lblUploadPresentationSlide = (Label)e.Row.FindControl("lblUploadPresentationSlide");
                Label lblUploadPresentationSlide_full = (Label)e.Row.FindControl("lblUploadPresentationSlide_full");

                List<AttachmentList> lstAttachmentList = new List<AttachmentList>();
                lstAttachmentList = getAttachment(lblApplication_Number.Text.ToString().Trim());

                foreach (var v_lstAttachmentList in lstAttachmentList)
                {
                    if (v_lstAttachmentList.Attachment_Type.ToString() == "Video_Clip")
                    {
                        lblVideoClip.Text = v_lstAttachmentList.Attachment_Path.ToString();
                    }
                    else if (v_lstAttachmentList.Attachment_Type.ToString() == "Presentation_Slide_Response")
                    //else if (v_lstAttachmentList.Attachment_Type.ToString() == "Presentation_Slide")
                    {
                        lblUploadPresentationSlide.Text += findFilename(v_lstAttachmentList.Attachment_Path.ToString()) + "<br/>";
                        lblUploadPresentationSlide_full.Text += v_lstAttachmentList.Attachment_Path.ToString() + "<br/>";
                    }
                }
                
            }
        }

        protected void GridView_App_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Vetting%20Meeting%20Arrangement.aspx");
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {

            string m_username = SPContext.Current.Web.CurrentUser.Name.ToString();
            string m_mail = "";
            string m_programId = m_Programme_ID;
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
            }
            else
            {
                m_mail = SPContext.Current.Web.CurrentUser.Email;
            }

            m_programName = lblProName.Text.Trim();
            m_intake = lblIntakeNo.Text.Trim();

            string sql = @"INSERT INTO [TB_DOWNLOAD_REQUEST] (Programme_Name, Programme_ID, Intake_Number, Request_Type, Status, User_Email, Vetting_Meeting_ID, Created_By, Created_Date) 
                           VALUES (@Programme_Name, @Programme_ID, @Intake_Number, 'Download Attachments - Invitation Response Summary', 0, @User_Email, @vmID, @User, GETDATE())";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Programme_Name", m_programName);
                    cmd.Parameters.AddWithValue("@Intake_Number", m_intake);
                    cmd.Parameters.AddWithValue("@Programme_ID", m_programId);
                    cmd.Parameters.AddWithValue("@User_Email", m_mail);
                    cmd.Parameters.AddWithValue("@vmID", m_VMID);
                    cmd.Parameters.AddWithValue("@User", m_username);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            string m_subject = "Zip File : " + m_programName + " / " + m_intake + " is processing.";
            string m_body = getEmailTemplate("ZipDownloadStartEmail");
            sharepointsendemail(m_mail, m_subject, m_body);

            lbldownloadmessage.Text = "Download Complete.";
        }

        private string getVideoClip(string appNum)
        {
            string video = string.Empty;

            List<AttachmentList> lstAttachmentList = getAttachment(appNum.Trim());
            foreach (var attachment in lstAttachmentList)
            {
                if (attachment.Attachment_Type.ToString() == "Video_Clip")
                {
                    video = attachment.Attachment_Path.ToString();
                }
            }
            return video;
        }

        private string getPresentationSlide(string appNum)
        {
            string presentation = string.Empty;
            List<AttachmentList> lstAttachmentList = getAttachment(appNum.Trim());
            foreach (var attachment in lstAttachmentList)
            {
                if (attachment.Attachment_Type.ToString() == "Presentation_Slide_Response")
                { 
                    presentation += findFilename(attachment.Attachment_Path.ToString()) + " ";
                }
            }
            return presentation;
        }

        private string getPrincipalFullName(string appNum, string appType){
            string fullName = string.Empty;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = string.Empty;
                if (appType == "CCMF")
                {
                    sql = "SELECT Principal_Full_Name FROM TB_CCMF_APPLICATION WHERE Application_Number = @appNum";
                } 
                if (appType == "CPIP")
                {
                    sql = "SELECT Principal_Full_Name FROM TB_INCUBATION_APPLICATION WHERE Application_Number = @appNum";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@appNum", appNum));
                    conn.Open();
                    fullName = cmd.ExecuteScalar().ToString();                 
                    conn.Close();                    
                }
            }

            return fullName;
        }

        protected void sharepointsendemail(string toAddress, string subject, string body)
        {
            SPSecurity.RunWithElevatedPrivileges(
                             delegate()
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
            ConnectOpen();
            String emailTemplateContent = "";
            var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template=@emailTemplate ";

            var command = new SqlCommand(sqlString, connection);
            command.Parameters.Add(new SqlParameter("@emailTemplate", emailTemplate));

            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emailTemplateContent = reader.GetString(1);
                }
                reader.Dispose();

            }
            finally
            {
                command.Dispose();
                ConnectClose();
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

        protected void getSYSTEMPARAMETER()
        {
            ConnectOpen();

            var sqlString = "select Config_Code,Value from TB_SYSTEM_PARAMETER;";

            var command = new SqlCommand(sqlString, connection);

            try { 
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

                        if (reader.GetString(0) == "IsLogEvent")
                        {
                            IsLogEvent = reader.GetString(1);
                        }
                    }

                    reader.Dispose();
            }
            finally 
            { 
                command.Dispose();
                ConnectClose();
            }
        }

        protected void InsertTBDownloadZIP(String User_Name, String Email, String type, String Path, String File_Name, String Password, String Status)
        {

            ConnectOpen();
            var sqlUpdate = "insert into TB_Download_ZIP(User_Name,Email,type,Path,File_Name,Password,Status,Created_By,Created_Date,Modified_By,Modified_Date) values("
                                + "'" + User_Name + "', "
                                + "'" + Email + "', "
                                + "'" + type + "', "
                                + "'" + Path + "', "
                                + "'" + File_Name + "', "
                                + "'" + Password + "', "
                                + "'" + Status + "', "
                                + "'" + SPContext.Current.Web.CurrentUser.Name.ToString() + "', "
                                + "GETDATE(), "
                                + "'" + SPContext.Current.Web.CurrentUser.Name.ToString() + "', "
                                + "GETDATE() "
                                + " ) ;";

            var command = new SqlCommand(sqlUpdate, connection);
            try 
            { 
                command.ExecuteNonQuery();

            }
            finally
            {
                command.Dispose();
                ConnectClose();
            }
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            ExportCSV();
        }

        private void ExportCSV()
        {
            string csv = string.Empty;
            if (m_Programme_Type == "CCMF")
            {
                csv = "Application No.,Presentation Time,Project Name,Name of Principal Applicant,Email,Mobile No.,Attend," +
                      "Name of Attendees,Type of Presentation Tools,Special Request,Video Clip,Upload Presentation Slide,Agreement";
                csv += "\r\n";

                List<TimeSlotList> ccmfList = getVettingApplicationList();

                foreach (TimeSlotList item in ccmfList)
                {
                    csv += "\"" + item.Application_Number + "\",";
                    csv += "\"" + item.Presentation_From + "\",";
                    csv += "\"" + item.Project_Name_Eng + "\",";
                    //csv += "\"" + getPrincipalFullName(item.Application_Number, "CCMF") + "\",";
                    csv += "\"" + item.Name_of_Principal_Applicant + "\",";
                    csv += "\"" + item.Email + "\",";
                    csv += "\"" + item.Mobile_Number + "\",";
                    csv += "\"" + item.Attendance + "\",";
                    csv += "\"" + item.Name_of_Attendees + "\",";
                    csv += "\"" + item.Presentation_Tools + "\",";
                    csv += "\"" + item.Special_Request + "\",";
                    csv += "\"" + getVideoClip(item.Application_Number) + "\",";
                    csv += "\"" + getPresentationSlide(item.Application_Number) + "\",";
                    csv += "\"" + item.Agreement + "\",";           

                    csv += "\r\n";
                }
            }

            if (m_Programme_Type == "CPIP")
            {
                csv = "Application No.,Presentation Time,Company Name,Name of Principal Applicant,Email,Mobile No.,Attend," +
                      "Name of Attendees,Type of Presentation Tools,Special Request,Video Clip,Upload Presentation Slide,Agreement";      
                csv += "\r\n";
                
                List<TimeSlotList> cpipList = getVettingApplicationList();

                foreach (TimeSlotList item in cpipList)
                {
                    csv += "\"" + item.Application_Number + "\",";
                    csv += "\"" + item.Presentation_From + "\",";
                    csv += "\"" + item.Company + "\",";
                    //csv += "\"" + getPrincipalFullName(item.Application_Number, "CPIP") + "\",";
                    csv += "\"" + item.Name_of_Principal_Applicant + "\",";
                    csv += "\"" + item.Email + "\",";
                    csv += "\"" + item.Mobile_Number + "\",";
                    csv += "\"" + item.Attendance + "\",";
                    csv += "\"" + item.Name_of_Attendees + "\",";
                    csv += "\"" + item.Presentation_Tools + "\",";
                    csv += "\"" + item.Special_Request + "\",";
                    csv += "\"" + getVideoClip(item.Application_Number) + "\",";
                    csv += "\"" + getPresentationSlide(item.Application_Number) + "\",";
                    csv += "\"" + item.Agreement + "\",";              

                    csv += "\r\n";
                }

            }


            csv = csv.Replace("<br>", " ").Replace("&nbsp;", " ");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + lblProName.Text + lblIntakeNo.Text + ".csv");

            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.IO.StreamWriter sw =
                new System.IO.StreamWriter(
                HttpContext.Current.Response.OutputStream,
                System.Text.Encoding.UTF8);
            sw.Write(csv);
            sw.Close();

            Context.ApplicationInstance.CompleteRequest(); 
            HttpContext.Current.Response.End();
            Context.Response.Redirect(Context.Request.RawUrl);
        }

        //compress file and folder
        private void CompressFolder(SPFolder folder, ZipOutputStream zipStream)
        {
            foreach (SPFile file in folder.Files)
            {
                String entryName = "";
                try
                {
                    //var DeletepathName = m_AttachmentPrimaryFolderName + @"\" + m_AttachmentSecondaryFolderName + @"\";
                    var DeletepathName = "";
                    if (m_AttachmentPrimaryFolderName != "")
                    {
                        DeletepathName += m_AttachmentPrimaryFolderName + @"\";
                    }
                    if (m_AttachmentSecondaryFolderName != "")
                    {
                        DeletepathName += m_AttachmentSecondaryFolderName + @"\";
                    }
                    entryName = file.Url.Substring(DeletepathName.Length);

                    //lbltest.Text += entryName + "<br/>";

                    if (checkAttFileZip(file.Url))
                    {
                        //lbltest.Text += "True: " + entryName + "<br/>";

                        var size = file.Length / 1024;
                        ErrorLog("File name", entryName + " (" + size.ToString("N2") + " KB)");

                        ZipEntry entry = new ZipEntry(entryName);
                        entry.DateTime = DateTime.Now;
                        zipStream.PutNextEntry(entry);
                        byte[] binary = file.OpenBinary();
                        zipStream.Write(binary, 0, binary.Length);

                        m_DownloadFileSize = m_DownloadFileSize + size;
                        ErrorLog("Total file size", m_DownloadFileSize.ToString("N2") + " KB");
                    }
                }
                catch (Exception e)
                {
                    ErrorLog("Error", e.Message);
                    var str = entryName + " ";
                    ErrorLog("Error file", str);

                }

            }

            foreach (SPFolder subfoldar in folder.SubFolders)
            {
                CompressFolder(subfoldar, zipStream);
            }
        }

        public void ErrorLog(string type, string message)
        {
            getSYSTEMPARAMETER();
            if (IsLogEvent == "1")
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(m_path + @"\logDownload.txt", true))
                {
                    try
                    {
                        file.WriteLine(string.Format("{0}  [{1}] {2}", DateTime.Now, type, message));
                    }
                    catch
                    {

                    }
                }
            }
        }



        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (m_ApplicationIsInDebug == "1")
            {
                if (m_ApplicationDebugEmailSentTo == "" || m_ApplicationDebugEmailSentTo == null)
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDownload.Enabled = false;
                    args.IsValid = true;
                }
            }
            else
            {
                string m_mail = SPContext.Current.Web.CurrentUser.Email;
                if (m_mail == "" || m_mail == null)
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDownload.Enabled = false;
                    args.IsValid = true;
                }
            }

        }

    }
}
