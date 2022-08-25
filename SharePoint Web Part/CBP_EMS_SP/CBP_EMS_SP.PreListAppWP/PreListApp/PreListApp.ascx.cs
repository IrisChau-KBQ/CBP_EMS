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
using System.Collections.Specialized;

namespace CBP_EMS_SP.PreListAppWP.PreListApp
{
    [ToolboxItemAttribute(false)]
    public partial class PreListApp : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        string m_VMID = "";
        string m_systemuser = "";
        string m_Role = "";
        string m_Programme_ID = "";
        string m_Programme_Name = "";
        string m_Intake_Number = "";
        string m_Time_Interval = "";
        DateTime m_Presentation_From = DateTime.Now;
        DateTime m_Presentation_To = DateTime.Now;
        string m_Programme_Type = "";
        string m_Meeting_status = "";
        private SqlConnection connection;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;

        DateTime m_VettingDate  = DateTime.Now;
        string m_VettingVenue = "";
        DateTime m_VettingMettingFrom  = DateTime.Now;
        DateTime m_VettingMeetingTo  = DateTime.Now;
        string m_WebsiteUrl = "";
        string m_WebsiteUrl_VettingTeam = "";
        string m_WebsiteUrl_InvitationResponse = "";
        string m_mail = "";
        string m_subject = "";
        string m_body = "";
        DateTime m_ConfirmDeadline;
        DateTime m_Deadline;
        DateTime m_DecentralizeProduct;
        bool  m_DecisionCompleted;
        bool m_MeetingCompleted;


        private string _CCEmailCCMF = "";
        private string _CCEmailCPIP = "";

        //string m_username = SPContext.Current.Web.CurrentUser.Name.ToString();

        Boolean isShowWithdrawPopUp = false;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
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
            public string Hong_Kong_Programme_Stream { set; get; }
            public string avgscore  { set; get; }
            public string Remarks_To_Vetting  { set; get; }

            public string ApplicationId { set; get; }
            public string ProgramId { set; get; }

        }

        public class ApplicationList
        {
            public string Incubation_ID  { set; get; }
            public string Programme_ID { set; get; }
            public string Application_Number { set; get; }
            public string Intake_Number { set; get; }
            public string Applicant { set; get; }
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AccessControl())
            {
                mainPanel.Visible = true;

                m_VMID = HttpContext.Current.Request.QueryString["VMID"];
                m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();
                lblMessageConfirm.Text = "";

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
                    //getVettingApplication();
                }

                checkStatusControlBtn();

                //pnlContent.Height = GridView_CPIP.Height;
            }
            else
            {
                mainPanel.Visible = false;
            }

        }

        protected Boolean AccessControl()
        {
            var m_result = false;
            getReview();
            // Check Role can display this web part

            if (m_Role == "CCMF Coordinator")
            {
                m_result = true;
            }
            else if (m_Role == "CPIP Coordinator")
            {
                m_result = true;
            }
            else if (m_Role.Contains("BDM"))
            {
                m_result = true;
            }
            else if (m_Role.Contains("Senior Manager"))
            {
                m_result = true;
            }
            else if (m_Role.Contains("CPMO"))
            {
                m_result = true;
            }

            return m_result;
        }

        protected void getdbdata()
        {
            getVettingApplication();
            UnassignedApplicants();
            ClusterDDl(ddlCluster,GridViewUA,"");
            //checkTimeSlotoutofrang();
        }

        protected void checkStatusControlBtn()
        {
            if (StatusConvertNum(m_Meeting_status) > 1)
            {
                BtnConfirm.Visible = false;
                ddlCluster.Visible = false;
                btnReset.Visible = false;
                btnInsert.Visible = false;
                BtnPRS.Visible = true;
                btnDecisionSummary.Visible = true;
            }
            else
            {
                BtnPRS.Visible = false;
                btnDecisionSummary.Visible = false;
            }
        }

        protected void getVettingMeetingInfo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";
                sql = "SELECT dbo.TB_PROGRAMME_INTAKE.Programme_ID,dbo.TB_PROGRAMME_INTAKE.Programme_Name, dbo.TB_PROGRAMME_INTAKE.Intake_Number, dbo.TB_VETTING_MEETING.Vetting_Meeting_ID, dbo.TB_VETTING_MEETING.Programme_ID, "
                    + "dbo.TB_VETTING_MEETING.Date, dbo.TB_VETTING_MEETING.Venue, dbo.TB_VETTING_MEETING.Vetting_Meeting_From, dbo.TB_VETTING_MEETING.Vetting_Meeting_To, dbo.TB_VETTING_MEETING.Decision_Completed,"
                    + "dbo.TB_VETTING_MEETING.Presentation_From, dbo.TB_VETTING_MEETING.Presentation_To, dbo.TB_VETTING_MEETING.Vetting_Team_Leader, dbo.TB_VETTING_MEETING.No_of_Attendance, "
                    + "dbo.TB_VETTING_MEETING.Meeting_Completed, dbo.TB_VETTING_MEETING.Created_By, dbo.TB_VETTING_MEETING.Created_Date, dbo.TB_VETTING_MEETING.Modified_By, dbo.TB_VETTING_MEETING.Modified_Date, "
                    + "dbo.TB_VETTING_MEETING.Time_Interval, dbo.TB_VETTING_MEETING.Meeting_status, "
                    + "isnull(dbo.TB_VETTING_MEETING.Deadline,'') as Deadline, isnull(dbo.TB_VETTING_MEETING.ConfirmDeadline,'') as ConfirmDeadline "
                    + "FROM dbo.TB_VETTING_MEETING INNER JOIN dbo.TB_PROGRAMME_INTAKE ON dbo.TB_VETTING_MEETING.Programme_ID = dbo.TB_PROGRAMME_INTAKE.Programme_ID "
                    + "WHERE (dbo.TB_VETTING_MEETING.Vetting_Meeting_ID = @m_VMID )";

                //lbltest.Text = sql;

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lblDate.Text = reader.GetDateTime(reader.GetOrdinal("Date")).ToString("dd MMMM, yyyy");
                            lblTime_Interval.Text = reader.GetValue(reader.GetOrdinal("Time_Interval")).ToString() + " mins";
                            m_Time_Interval = reader.GetValue(reader.GetOrdinal("Time_Interval")).ToString();
                            lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                            m_Programme_Name = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                            lblIntakeNo.Text = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                            m_Intake_Number = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                            m_Programme_ID = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString();
                            lblm_Programme_ID.Text = m_Programme_ID;
                            m_VettingDate = reader.GetDateTime(reader.GetOrdinal("Date"));
                            lblVenue.Text = m_VettingVenue = reader.GetValue(reader.GetOrdinal("Venue")).ToString();
                            m_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From"));
                            m_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To"));
                            m_VettingMettingFrom = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_From"));
                            m_VettingMeetingTo = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_To"));
                            lblPresentation_From.Text = m_Presentation_From.ToString();
                            lblPresentation_To.Text = m_Presentation_To.ToString();
                            m_Meeting_status = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString();

                            m_Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline"));
                            m_ConfirmDeadline = reader.GetDateTime(reader.GetOrdinal("ConfirmDeadline"));
                            if (reader.IsDBNull(reader.GetOrdinal("Decision_Completed")))
                            {
                                m_DecisionCompleted = false;
                            }
                            else
                            {
                                m_DecisionCompleted = reader.GetBoolean(reader.GetOrdinal("Decision_Completed"));
                            }
                            if (reader.IsDBNull(reader.GetOrdinal("Meeting_Completed")))
                            {
                                m_MeetingCompleted = false;
                            }
                            else
                            {
                                m_MeetingCompleted = reader.GetBoolean(reader.GetOrdinal("Meeting_Completed"));
                            }
                            if (m_MeetingCompleted)
                                lblMeetingStatus.Text = "Meeting Completed";
                            else if (m_DecisionCompleted)
                                lblMeetingStatus.Text = "Stage 1 Completed";
                            else
                                lblMeetingStatus.Text = "Pending to Start";


                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                m_Programme_Type = checkProgrammeType(m_Programme_ID);
                //lbltest.Text += m_Programme_Type + "<br/>";
                if (m_Programme_Type == "CPIP")
                {
                    GridView_CPIP.Columns[3].Visible = false;
                    GridView_CPIP.Columns[4].Visible = false;
                    GridView_CPIP.Columns[5].Visible = false;
                    GridView_CPIP.Columns[6].Visible = false;
                }
                else
                {
                    GridView_CPIP.Columns[2].Visible = false;
                }
            }
        }

        protected string Remarklinebreak(string m_string)
        {
            string m_result = "";
            string m_replacework = "<BR>";

            m_string = m_string.Replace("CCMF BDM Remark :", "<BR>CCMF BDM Remark :")
                               .Replace("Senior Manager Remark :", "<BR>Senior Manager Remark :")
                               .Replace("CPMO Remark :", "<BR>CPMO Remark :")
                               .Replace("Final remark :", "<BR>Final remark :");

            if (m_string.StartsWith(m_replacework))
            {
                //m_result += m_replacework.Length.ToString();
                //m_result += m_string.StartsWith(m_replacework).ToString();
                //m_result += m_string.IndexOf(m_replacework).ToString();

                m_result += m_string.Substring(m_replacework.Length);
            }

            return m_result;
        }

        protected void getVettingApplication(){
            //lbltest.Text += "getVettingApplication<br/>"+ DateTime.Now.ToString();
            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "";
                string m_Attendance = "";
                string v_Remarks_To_Vetting = "";

                if (m_Programme_Type == "CCMF")
                {
                    sql = "SELECT dbo.TB_VETTING_APPLICATION.Vetting_Application_ID, dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID, dbo.TB_VETTING_APPLICATION.Application_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Presentation_From, dbo.TB_VETTING_APPLICATION.Presentation_To, dbo.TB_VETTING_APPLICATION.Email, dbo.TB_VETTING_APPLICATION.Mobile_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Attend, dbo.TB_VETTING_APPLICATION.Name_of_Attendees, dbo.TB_VETTING_APPLICATION.Presentation_Tools, dbo.TB_VETTING_APPLICATION.Special_Request, "
                    + "dbo.TB_CCMF_APPLICATION.Project_Name_Eng, dbo.TB_CCMF_APPLICATION.Programme_Type, dbo.TB_CCMF_APPLICATION.CCMF_Application_Type, dbo.TB_CCMF_APPLICATION.Business_Area, dbo.TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream "
                    + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                    + "dbo.TB_CCMF_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_CCMF_APPLICATION.Application_Number "
                    + "WHERE dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = @m_VMID AND dbo.TB_CCMF_APPLICATION.Status <> 'Saved' AND dbo.TB_CCMF_APPLICATION.Status = 'Complete Screening' "
                    + "";
                    //+ "ORDER BY Presentation_From ASC";

                    sql = "SELECT O.Vetting_Application_ID, O.Vetting_Meeting_ID, O.Application_Number, O.Presentation_From, O.Presentation_To, O.Email, O.Mobile_Number, O.Attend, O.Name_of_Attendees, O.Presentation_Tools, "
                    + "O.Special_Request, dbo.TB_CCMF_APPLICATION.Project_Name_Eng, dbo.TB_CCMF_APPLICATION.Programme_Type, dbo.TB_CCMF_APPLICATION.CCMF_Application_Type, "
                    + "dbo.TB_CCMF_APPLICATION.Business_Area, dbo.TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream, "
                    + "(SELECT AVG(Total_Score) AS Expr1 "
                    + "FROM dbo.TB_SCREENING_CCMF_SCORE AS C "
                    + "WHERE (Application_Number = O.Application_Number) AND (Total_Score <> 0)) AS avgscore, ISNULL(dbo.TB_APPLICATION_SHORTLISTING.Remarks_To_Vetting, N'') AS Remarks_To_Vetting "
                    + "FROM dbo.TB_VETTING_APPLICATION AS O INNER JOIN "
                    + "dbo.TB_CCMF_APPLICATION ON O.Application_Number = dbo.TB_CCMF_APPLICATION.Application_Number LEFT OUTER JOIN "
                    + "dbo.TB_APPLICATION_SHORTLISTING ON O.Application_Number = dbo.TB_APPLICATION_SHORTLISTING.Application_Number "
                    + "WHERE (O.Vetting_Meeting_ID = @m_VMID) AND (dbo.TB_CCMF_APPLICATION.Status <> 'Saved') AND (dbo.TB_CCMF_APPLICATION.Status = 'Complete Screening' or TB_CCMF_APPLICATION.Status = 'Presentation Withdraw') ";


                }
                else
                {
                    sql = "SELECT dbo.TB_VETTING_APPLICATION.Vetting_Application_ID, dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID, dbo.TB_VETTING_APPLICATION.Application_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Presentation_From, dbo.TB_VETTING_APPLICATION.Presentation_To, dbo.TB_VETTING_APPLICATION.Email, dbo.TB_VETTING_APPLICATION.Mobile_Number, "
                    + "dbo.TB_VETTING_APPLICATION.Attend, dbo.TB_VETTING_APPLICATION.Name_of_Attendees, dbo.TB_VETTING_APPLICATION.Presentation_Tools, dbo.TB_VETTING_APPLICATION.Special_Request, "
                    + "dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng, dbo.TB_INCUBATION_APPLICATION.Business_Area "
                    + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                    + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_INCUBATION_APPLICATION.Application_Number "
                    + "WHERE dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = @m_VMID AND dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved' AND dbo.TB_INCUBATION_APPLICATION.Status = 'Complete Screening' "
                    + "";
                    //+ "ORDER BY Presentation_From ASC";

                    sql = "SELECT O.Vetting_Application_ID, O.Vetting_Meeting_ID, O.Application_Number, O.Presentation_From, O.Presentation_To, O.Email, O.Mobile_Number, O.Attend, O.Name_of_Attendees, O.Presentation_Tools, "
                        + "O.Special_Request, dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng, dbo.TB_INCUBATION_APPLICATION.Business_Area, "
                        + "(SELECT AVG(Total_Score) AS Expr1 "
                        + "FROM dbo.TB_SCREENING_INCUBATION_SCORE AS C "
                        + "WHERE (Application_Number = O.Application_Number) AND (Total_Score <> 0)) AS avgscore, ISNULL(dbo.TB_APPLICATION_SHORTLISTING.Remarks_To_Vetting, N'') AS Remarks_To_Vetting "
                        + "FROM dbo.TB_VETTING_APPLICATION AS O INNER JOIN "
                        + "dbo.TB_INCUBATION_APPLICATION ON O.Application_Number = dbo.TB_INCUBATION_APPLICATION.Application_Number LEFT OUTER JOIN "
                        + "dbo.TB_APPLICATION_SHORTLISTING ON O.Application_Number = dbo.TB_APPLICATION_SHORTLISTING.Application_Number "
                        + "WHERE (dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved') AND (dbo.TB_INCUBATION_APPLICATION.Status = 'Complete Screening' or dbo.TB_INCUBATION_APPLICATION.Status = 'Presentation Withdraw') AND (O.Vetting_Meeting_ID = @m_VMID) ";


                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (m_Programme_Type == "CCMF")
                            {
                                //v_Remarks_To_Vetting = reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString();
                                //        .Replace("CCMF BDM Remark :", "<BR>CCMF BDM Remark :")
                                //        .Replace("Senior Manager Remark :", "<BR>Senior Manager Remark :")
                                //        .Replace("CPMO Remark :", "<BR>CPMO Remark :")
                                //        .Replace("Final remark :", "<BR>Final remark :");

                                //if (v_Remarks_To_Vetting.StartsWith("<BR>")) 
                                //{
                                //    v_Remarks_To_Vetting = v_Remarks_To_Vetting.Substring(4, v_Remarks_To_Vetting.Length-4);
                                //}

                                //v_Remarks_To_Vetting = Remarklinebreak(v_Remarks_To_Vetting);

                                lstTimeSlotList.Add(new TimeSlotList
                                {
                                    TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                                    Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                                    DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                                    Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                                    DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                                    Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                    Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                    Hong_Kong_Programme_Stream = reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")).ToString().Replace("Professional", "PRO").Replace("Young Entrepreneur", "YEP"),
                                    Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                                    CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                                    Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                    avgscore = reader.GetDecimal(reader.GetOrdinal("avgscore")).ToString("f3"),
                                    Remarks_To_Vetting = Remarklinebreak(reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString()),
                                    Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                                    Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                                    Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString()
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
                                    avgscore = reader.GetDecimal(reader.GetOrdinal("avgscore")).ToString("f3"),
                                    Remarks_To_Vetting = (reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString()),
                                    Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                                    Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                                    Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString()
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



            lstTimeSlotList = insertTimeBreak(lstTimeSlotList);

            lstTimeSlotList = insertEmptySlot(lstTimeSlotList);

            lstTimeSlotList.Sort((x, y) => DateTime.Compare(x.DT_Presentation_From, y.DT_Presentation_From));

            GridView_CPIP.DataSource = lstTimeSlotList;
            GridView_CPIP.DataBind();


        }

        protected string AttendanceStatus(int m_Attend)
        {
            string m_AttendanceStatus = "";
            if (m_Attend == 0)
            {
                m_AttendanceStatus= "Pending";
            }
            else if (m_Attend == 1)
            {
                m_AttendanceStatus= "Yes";
            }
            else if (m_Attend == 2)
            {
                m_AttendanceStatus= "No";
            }

            return m_AttendanceStatus;
        }

        protected void ApplicationDDL(DropDownList m_ddlApplicationNo,string selectRecord)
        {
            List<ApplicationList> lstApplicationList = new List<ApplicationList>();
            List<TimeSlotList> lstVettingApplication = checkApplicationNo();
            m_ddlApplicationNo.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";
                bool addRecord = false;
                if (m_Programme_Type == "CCMF")
                {
                    sql = "SELECT CCMF_ID ,Programme_ID ,Application_Number ,Intake_Number ,Applicant, Project_Name_Eng, Programme_Type,CCMF_Application_Type, Business_Area "
                        + "FROM TB_CCMF_APPLICATION "
                        + "where Status <> 'Saved' AND Status ='Complete Screening' "
                        + "and Programme_ID = @m_Programme_ID "
                        + "and Intake_Number = @m_Intake_Number "
                        + "order by Application_Number asc";
                }
                else
                {
                    sql = "SELECT Incubation_ID ,Programme_ID ,Application_Number ,Intake_Number ,Applicant, Company_Name_Eng, Business_Area "
                            + "FROM TB_INCUBATION_APPLICATION "
                            + "where Status <> 'Saved' AND Status ='Complete Screening' "
                        + "and Programme_ID = @m_Programme_ID "
                        + "and Intake_Number = @m_Intake_Number "
                            + "order by Application_Number asc";

                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_Programme_ID", m_Programme_ID));
                        cmd.Parameters.Add(new SqlParameter("@m_Intake_Number", m_Intake_Number));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            addRecord = true;

                            foreach (GridViewRow row in GridView_CPIP.Rows)
                            {
                                Label lblApplication_Number = (Label)row.FindControl("lblApplication_Number");
                                if (lblApplication_Number.Text.ToString().Trim() == reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim() && selectRecord.ToString().Trim() != reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim())
                                {
                                    addRecord = false;
                                }
                            }

                            //checking Vetting_Metting Record
                            foreach (var v_VettingApplication in lstVettingApplication)
                            {
                                if (v_VettingApplication.Application_Number.ToString().Trim() == reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim() && selectRecord.ToString().Trim() != reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim())
                                {
                                    addRecord = false;
                                }
                            }

                            if (addRecord) { m_ddlApplicationNo.Items.Add(new ListItem(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(), reader.GetValue(reader.GetOrdinal("Application_Number")).ToString())); }

                        }   //
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }

            m_ddlApplicationNo.Items.Add(new ListItem("Time Break", "Time Break"));

            //ddlsample.SelectedIndex = ddlsample.Items.IndexOf(ddlsample.Items.FindByText("x"))

            if (m_ddlApplicationNo.Items.FindByValue(selectRecord) != null)
            {
                m_ddlApplicationNo.SelectedValue = selectRecord;
            }

        }

        protected void ClusterDDl(DropDownList m_ddlCluster,GridView m_GridView, string selectRecord)
        {
            m_ddlCluster.Items.Clear();
            string m_Cluster = "";
            m_ddlCluster.Items.Add(new ListItem("Cluster", "Cluster"));
            foreach (GridViewRow row in m_GridView.Rows)
            {
                HiddenField HiddenCluster = (HiddenField)row.FindControl("HiddenCluster");
                HiddenField HiddenShortlisted = (HiddenField)row.FindControl("HiddenShortlisted");

                m_Cluster = HiddenCluster.Value.ToString();

                if (m_ddlCluster.Items.FindByText(m_Cluster.ToString()) == null && HiddenShortlisted.Value.ToString().Trim() == "True")
                {
                    m_ddlCluster.Items.Add(new ListItem(HiddenCluster.Value.ToString(), HiddenCluster.Value.ToString()));
                }
            }

            if (m_ddlCluster.Items.Count <= 0)
            {

                btnInsert.Enabled = false;
            }
            else
            {
                btnInsert.Enabled = true;
            }

        }

        protected void UnassignedApplicants()
        {
            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();
            List<TimeSlotList> lstVettingApplication = checkApplicationNo();


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";
                bool addRecord = false;
                int m_RowNum = 0;

                if (m_Programme_Type == "CCMF")
                {
                    sql = "SELECT CCMF_ID, dbo.TB_CCMF_APPLICATION.Programme_ID, dbo.TB_CCMF_APPLICATION.Application_Number, dbo.TB_CCMF_APPLICATION.Intake_Number, dbo.TB_CCMF_APPLICATION.Applicant, "
                        + "dbo.TB_CCMF_APPLICATION.Project_Name_Eng, dbo.TB_CCMF_APPLICATION.Programme_Type, dbo.TB_CCMF_APPLICATION.CCMF_Application_Type, dbo.TB_CCMF_APPLICATION.Business_Area, dbo.TB_CCMF_APPLICATION.Status,"
                        + "ISNULL(dbo.TB_APPLICATION_SHORTLISTING.Shortlisted, 0) AS Shortlisted "
                        + "FROM dbo.TB_CCMF_APPLICATION LEFT OUTER JOIN "
                        + "dbo.TB_APPLICATION_SHORTLISTING ON dbo.TB_CCMF_APPLICATION.Application_Number = dbo.TB_APPLICATION_SHORTLISTING.Application_Number "
                        + "where dbo.TB_CCMF_APPLICATION.Status <> 'Saved' AND dbo.TB_CCMF_APPLICATION.Status ='Complete Screening' "
                        + "and dbo.TB_CCMF_APPLICATION.Programme_ID = @m_Programme_ID "
                        + "and dbo.TB_CCMF_APPLICATION.Intake_Number = @m_Intake_Number "
                        + "order by dbo.TB_APPLICATION_SHORTLISTING.Shortlisted DESC, dbo.TB_CCMF_APPLICATION.Application_Number asc";

                    sql = "SELECT O.CCMF_ID, O.Programme_ID, O.Application_Number, O.Intake_Number, O.Applicant, O.Project_Name_Eng, O.Programme_Type, O.Hong_Kong_Programme_Stream, O.CCMF_Application_Type, O.Business_Area, O.Status, "
                      + "ISNULL(dbo.TB_APPLICATION_SHORTLISTING.Shortlisted, 0) AS Shortlisted, "
                      + "(SELECT isnull(AVG(Total_Score),0) AS Expr1 "
                      + "FROM dbo.TB_SCREENING_CCMF_SCORE AS C "
                      + "WHERE (Application_Number = O.Application_Number) AND (Total_Score <> 0)) AS avgscore "
                      + "FROM dbo.TB_CCMF_APPLICATION AS O LEFT OUTER JOIN "
                      + "dbo.TB_APPLICATION_SHORTLISTING ON O.Application_Number = dbo.TB_APPLICATION_SHORTLISTING.Application_Number "
                      + "WHERE (O.Status <> 'Saved') AND (O.Status = 'Complete Screening') AND (O.Programme_ID = @m_Programme_ID) AND (O.Intake_Number = @m_Intake_Number) "
                      + "ORDER BY Shortlisted DESC, O.Business_Area ASC, O.Hong_Kong_Programme_Stream ASC, avgscore DESC ";
                }
                else
                {

                    sql = "SELECT Incubation_ID, dbo.TB_INCUBATION_APPLICATION.Programme_ID, dbo.TB_INCUBATION_APPLICATION.Application_Number, dbo.TB_INCUBATION_APPLICATION.Intake_Number, dbo.TB_INCUBATION_APPLICATION.Applicant, "
                            + "dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng, dbo.TB_INCUBATION_APPLICATION.Business_Area, dbo.TB_INCUBATION_APPLICATION.Status,"
                            + "ISNULL(dbo.TB_APPLICATION_SHORTLISTING.Shortlisted, 0) AS Shortlisted "
                            + "FROM dbo.TB_INCUBATION_APPLICATION LEFT OUTER JOIN "
                            + "dbo.TB_APPLICATION_SHORTLISTING ON dbo.TB_INCUBATION_APPLICATION.Application_Number = dbo.TB_APPLICATION_SHORTLISTING.Application_Number "
                            + "where dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved' AND dbo.TB_INCUBATION_APPLICATION.Status ='Complete Screening' "
                            + "and dbo.TB_INCUBATION_APPLICATION.Programme_ID = @m_Programme_ID "
                            + "and dbo.TB_INCUBATION_APPLICATION.Intake_Number = @m_Intake_Number "
                            + "order by dbo.TB_APPLICATION_SHORTLISTING.Shortlisted DESC, dbo.TB_INCUBATION_APPLICATION.Application_Number asc";

                    sql = "SELECT O.Incubation_ID, O.Programme_ID, O.Application_Number, O.Intake_Number, O.Applicant, O.Company_Name_Eng, O.Business_Area, O.Status, "
                        + "ISNULL(dbo.TB_APPLICATION_SHORTLISTING.Shortlisted, 0) AS Shortlisted, "
                        + "(SELECT isnull(AVG(Total_Score),0) AS Expr1 "
                        + "FROM dbo.TB_SCREENING_INCUBATION_SCORE AS C "
                        + "WHERE (Application_Number = O.Application_Number) AND (Total_Score <> 0)) AS avgscore "
                        + "FROM dbo.TB_INCUBATION_APPLICATION AS O LEFT OUTER JOIN "
                        + "dbo.TB_APPLICATION_SHORTLISTING ON O.Application_Number = dbo.TB_APPLICATION_SHORTLISTING.Application_Number "
                        + "WHERE (O.Status <> 'Saved') AND (O.Status = 'Complete Screening') AND (O.Programme_ID = @m_Programme_ID) AND (O.Intake_Number = @m_Intake_Number) "
                        + "ORDER BY Shortlisted DESC, O.Business_Area ASC, avgscore DESC ";


                }



                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_Programme_ID", m_Programme_ID));
                        cmd.Parameters.Add(new SqlParameter("@m_Intake_Number", m_Intake_Number));
                        var reader = cmd.ExecuteReader();


                        while (reader.Read())
                        {



                            addRecord = true;
                            foreach (GridViewRow row in GridView_CPIP.Rows)
                            {
                                Label lblApplication_Number = (Label)row.FindControl("lblApplication_Number");
                                if (lblApplication_Number.Text.ToString().Trim() == reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim())
                                {
                                    addRecord = false;
                                }
                            }

                            //checking Vetting_Metting Record
                            foreach (var v_VettingApplication in lstVettingApplication)
                            {
                                if (v_VettingApplication.Application_Number.ToString().Trim() == reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim())
                                {
                                    addRecord = false;
                                }
                            }

                            if (addRecord)
                            {
                                m_RowNum++;

                                if (m_Programme_Type == "CCMF")
                                {
                                    lstTimeSlotList.Add(new TimeSlotList
                                    {
                                        Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                        Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                        Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                        Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                                        CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                                        Shortlisted = reader.GetValue(reader.GetOrdinal("Shortlisted")).ToString(),
                                        RowNumber = m_RowNum.ToString(),
                                        CustDisplay = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString() + "<br/>"
                                        + reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString() + "<br/>"
                                        + reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString() + "<br/>"
                                        + reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")).ToString().Replace("Professional", "PRO").Replace("Young Entrepreneur", "YEP") + "<br/>"
                                            //+ reader.GetValue(reader.GetOrdinal("avgscore")).ToString("f4") + "<br/>"
                                        + reader.GetDecimal(reader.GetOrdinal("avgscore")).ToString("f3") + "<br/>"
                                        + reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),

                                        ApplicationId = reader.GetValue(reader.GetOrdinal("CCMF_ID")).ToString(),
                                        ProgramId = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString()

                                    });
                                }
                                else
                                {
                                    lstTimeSlotList.Add(new TimeSlotList
                                    {
                                        Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                        Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                        Company = reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),
                                        Shortlisted = reader.GetValue(reader.GetOrdinal("Shortlisted")).ToString(),
                                        RowNumber = m_RowNum.ToString(),
                                        CustDisplay = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString() + "<br/>"
                                        + reader.GetDecimal(reader.GetOrdinal("avgscore")).ToString("f3") + "<br/>"
                                        + reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),

                                        ApplicationId = reader.GetValue(reader.GetOrdinal("Incubation_ID")).ToString(),
                                        ProgramId = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString()

                                    });
                                }

                            }



                        }   //
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                lblcount.Text = " ( " + m_RowNum.ToString() + " ) ";

            }




            GridViewUA.DataSource = lstTimeSlotList;
            GridViewUA.DataBind();
        }

        protected List<TimeSlotList> checkApplicationNo()
        {
            string sql = "";
            List<TimeSlotList> lstVetting_Application = new List<TimeSlotList>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql = "SELECT Application_Number FROM TB_VETTING_APPLICATION";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lstVetting_Application.Add(new TimeSlotList
                            {
                                Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString()
                            });
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return lstVetting_Application;

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
            //m_Role = "CCMF BDM";

        }

        protected void GridView_CPIP_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            // Retrieve the row index stored in the 
            // CommandArgument property.
            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button 
            // from the Rows collection.
            GridViewRow row = GridView_CPIP.Rows[index];
            DropDownList ddlApplicationNo = (DropDownList)row.FindControl("ddlApplicationNo");
            Label lblApplication_Number = (Label)row.FindControl("lblApplication_Number");
            ImageButton btnsave = (ImageButton)row.FindControl("SaveButton");
            ImageButton btncancel = (ImageButton)row.FindControl("CancelButton");
            ImageButton btnedit = (ImageButton)row.FindControl("EditButton");
            ImageButton btndelete = (ImageButton)row.FindControl("DeleteButton");
            HiddenField HiddenVetting_Application_ID = (HiddenField)row.FindControl("HiddenVetting_Application_ID");
            HiddenField HiddenVetting_Meeting_ID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
            HiddenField HiddenPresentation_From = (HiddenField)row.FindControl("HiddenPresentation_From");
            HiddenField HiddenPresentation_To = (HiddenField)row.FindControl("HiddenPresentation_To");





            if (e.CommandName == "EditStatus")
            {

                ApplicationDDL(ddlApplicationNo, lblApplication_Number.Text);


                ddlApplicationNo.Visible = true;
                lblApplication_Number.Visible = false;
                btnsave.Visible = true;
                btncancel.Visible = true;
                btnedit.Visible = false;
                btndelete.Visible = false;

                //row.Cells[2].Text = ""; //Company
                //row.Cells[3].Text = ""; //Project Name
                //row.Cells[4].Text = ""; //Programme Type
                //row.Cells[5].Text = ""; //Application Type
                //row.Cells[6].Text = ""; //Cluster
            }
            else if (e.CommandName == "CancelStatus")
            {
                ddlApplicationNo.Visible = false;
                lblApplication_Number.Visible = true;
                btnsave.Visible = false;
                btncancel.Visible = false;
                btnedit.Visible = true;
                btndelete.Visible = true;

                if (StatusConvertNum(m_Meeting_status) > 1)
                {
                    btndelete.Visible = false;
                }
                else
                {
                    btndelete.Visible = true;
                }
            }
            else if (e.CommandName == "SaveStatus")
            {

                ddlApplicationNo.Visible = false;
                lblApplication_Number.Visible = true;
                btnsave.Visible = false;
                btncancel.Visible = false;
                btnedit.Visible = true;
                btndelete.Visible = true;

                //lbltest.Text += " Vetting_Application_ID:"+ HiddenVetting_Application_ID.Value.ToString() + "<br/>";
                //lbltest.Text += " Vetting_Meeting_ID:" + HiddenVetting_Meeting_ID.Value.ToString() + "<br/>";
                //lbltest.Text += " Application_Number:" + lblApplication_Number.Text.ToString() + "<br/>";
                //lbltest.Text += " ddlApplicationNo:" + ddlApplicationNo.SelectedValue.ToString() + "<br/>";
                //lbltest.Text += " HiddenPresentation_From:" + HiddenPresentation_From.Value.ToString().Trim() + "<br/>";
                //lbltest.Text += " HiddenPresentation_To:" + HiddenPresentation_To.Value.ToString().Trim() + "<br/>";

                if (HiddenVetting_Application_ID.Value.ToString().Trim() == "" )
                {
                    if (ddlApplicationNo.SelectedValue.ToString() != "")
                    {
                        insertRowData(HiddenVetting_Meeting_ID.Value.ToString(), ddlApplicationNo.SelectedValue.ToString(), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim());
                        //HiddenPresentation_To.Value = getVetting_Application_ID(m_VMID, ddlApplicationNo.SelectedValue.ToString());

                        //lbltest.Text += " Vetting_Meeting_ID:" + m_VMID + "<br/>";
                        //lbltest.Text += " ddlApplicationNo:" + ddlApplicationNo.SelectedValue.ToString() + "<br/>";

                        //lbltest.Text += "getVetting_Application_ID  " + getVetting_Application_ID(m_VMID, ddlApplicationNo.SelectedValue.ToString());

                        HiddenVetting_Application_ID.Value = getVetting_Application_ID(m_VMID, ddlApplicationNo.SelectedValue.ToString());
                    }
                }
                else
                {
                    if (lblApplication_Number.Text.ToString() != ddlApplicationNo.SelectedValue.ToString())
                    {
                        updateRowData(HiddenVetting_Application_ID.Value.ToString(), HiddenVetting_Meeting_ID.Value.ToString(), lblApplication_Number.Text.ToString(), ddlApplicationNo.SelectedValue.ToString());
                    }

                }

                if (StatusConvertNum(m_Meeting_status) > 1)
                {
                    if (m_Programme_Type == "CPIP")
                    {
                        if (lblApplication_Number.Text.ToString() != ddlApplicationNo.SelectedValue.ToString())
                        {
                            if (ddlApplicationNo.SelectedValue.ToString().Trim() != "")
                            {
                                sendToApplicant(CPIPIDGetEmail(ddlApplicationNo.SelectedValue.ToString()), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                            }
                            if (lblApplication_Number.Text.ToString().Trim() != "")
                            {
                                sendToApplicant_cancel(CPIPIDGetEmail(lblApplication_Number.Text.ToString()), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                            }
                            //lbltest.Text += " Email : " + CPIPIDGetEmail(lblApplication_Number.Text) + "<br/>";
                        }

                    }
                    else
                    {
                        if (lblApplication_Number.Text.ToString() != ddlApplicationNo.SelectedValue.ToString())
                        {
                            if (ddlApplicationNo.SelectedValue.ToString().Trim() != "")
                            {
                                sendToApplicant(CCMFIDGetEmail(ddlApplicationNo.SelectedValue.ToString()), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                            }
                            if (lblApplication_Number.Text.ToString().Trim() != "")
                            {
                                sendToApplicant_cancel(CCMFIDGetEmail(lblApplication_Number.Text.ToString()), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                            }
                            //lbltest.Text += " Email : " + CCMFIDGetEmail(lblApplication_Number.Text) + "<br/>";
                        }

                    }
                }

                //if (HiddenVetting_Application_ID.Value.ToString().Trim() == "")
                //{
                //    insertRowData(HiddenVetting_Meeting_ID.Value.ToString(), ddlApplicationNo.SelectedValue.ToString(), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim());
                //}
                //else
                //{
                //    if (lblApplication_Number.Text.ToString() != ddlApplicationNo.SelectedValue.ToString())
                //    {
                //        updateRowData(HiddenVetting_Application_ID.Value.ToString(), HiddenVetting_Meeting_ID.Value.ToString(), lblApplication_Number.Text.ToString(), ddlApplicationNo.SelectedValue.ToString());
                //    }
                //    
                //}

                getdbdata();
            }
            else if (e.CommandName == "DeleteStatus")
            {



                ddlApplicationNo.Visible = false;
                lblApplication_Number.Visible = true;
                btnsave.Visible = false;
                btncancel.Visible = false;
                btnedit.Visible = true;
                btndelete.Visible = true;

                if (!String.IsNullOrEmpty(HiddenVetting_Application_ID.Value.ToString()))
                {
                    deleteRowData(HiddenVetting_Application_ID.Value.ToString(), HiddenVetting_Meeting_ID.Value.ToString(), lblApplication_Number.Text.ToString());

                    if (StatusConvertNum(m_Meeting_status) > 1)
                    {
                        if (m_Programme_Type == "CPIP")
                        {
                            sendToApplicant_cancel(CPIPIDGetEmail(lblApplication_Number.Text.ToString()), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                        }
                        else
                        {
                            sendToApplicant_cancel(CCMFIDGetEmail(lblApplication_Number.Text.ToString()), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                        }
                    }
                }
                getdbdata();

            }

        }

        protected string getVetting_Application_ID(string m_VMID, string m_Application_Number)
        {
            string m_VAID = "";

            ConnectOpen();
            try
            {
                var sqlString = "SELECT TOP (1) Vetting_Application_ID FROM TB_VETTING_APPLICATION "
                              + "WHERE (Application_Number = @Application_Number) AND (Vetting_Meeting_ID = @Vetting_Meeting_ID )";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Application_Number", m_Application_Number));
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_VMID));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    m_VAID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString();
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            return m_VAID;
        }

        protected void GridView_CPIP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnsave = (ImageButton)e.Row.FindControl("SaveButton");
                ImageButton btncancel = (ImageButton)e.Row.FindControl("CancelButton");
                ImageButton btnedit = (ImageButton)e.Row.FindControl("EditButton");
                ImageButton btndelete = (ImageButton)e.Row.FindControl("DeleteButton");
                HiddenField HiddenAttendance = (HiddenField)e.Row.FindControl("HiddenAttendance");
                DropDownList ddlApplicationNo = (DropDownList)e.Row.FindControl("ddlApplicationNo");
                Label lblApplication_Number = (Label)e.Row.FindControl("lblApplication_Number");

                btnsave.Visible = false;
                btncancel.Visible = false;
                ddlApplicationNo.Visible = false;

                //Control Font Color
                if (HiddenAttendance.Value.ToString().Trim() == "No")
                {
                    string hex = "#000000";
                    Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

                    lblApplication_Number.ForeColor = _color;
                    lblApplication_Number.Font.Bold = true;

                    for (int j = 0; j < e.Row.Cells.Count - 1; j++)
                    {
                        e.Row.Cells[j].ForeColor = _color;
                        e.Row.Cells[j].Font.Bold = true;

                    }
                }
                else if (HiddenAttendance.Value.ToString().Trim() == "Yes")
                {
                    string hex = "#8AD5F7";
                    Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                    e.Row.Cells[10].ForeColor = _color;
                }
                else if (HiddenAttendance.Value.ToString().Trim() == "Pending")
                {
                }
                else if (HiddenAttendance.Value.ToString().Trim() == "" && lblApplication_Number.Text.ToString().Trim() == "Time Break")
                {
                    //Delete button disable
                    btndelete.Visible = true;
                }
                else if (HiddenAttendance.Value.ToString().Trim() == "")
                {
                    //Delete button disable
                    btndelete.Visible = false;
                }



                // Control Button
                if (StatusConvertNum(m_Meeting_status) > 1)
                {
                    if (HiddenAttendance.Value.ToString().Trim() == "No")
                    {
                        btnedit.Visible = true;
                    }
                    else if (HiddenAttendance.Value.ToString().Trim() == "Yes")
                    {
                        btnsave.Visible = false;
                        btnedit.Visible = true;
                        btndelete.Visible = false;
                    }
                    else if (HiddenAttendance.Value.ToString().Trim() == "Pending")
                    {
                        btnsave.Visible = false;
                        btnedit.Visible = true;
                        btndelete.Visible = false;
                    }
                    else if (HiddenAttendance.Value.ToString().Trim() == "" && lblApplication_Number.Text.ToString().Trim() == "Time Break")
                    {
                        btnsave.Visible = false;
                        if (lblApplication_Number.Text.ToString().Trim() != "")
                        {
                            btnedit.Visible = true;
                        }

                        btndelete.Visible = true;

                    }
                    else if (HiddenAttendance.Value.ToString().Trim() == "")
                    {
                        btnsave.Visible = false;
                        if (lblApplication_Number.Text.ToString().Trim() != "")
                        {
                            btnedit.Visible = true;
                        }

                        btndelete.Visible = false;
                        //btncancel.Visible = false;
                    }



                }
                if (m_DecisionCompleted || m_MeetingCompleted)
                {
                    btnedit.Enabled = false;
                    btnedit.ImageUrl = "~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Edit-disable.png";
                    btndelete.Enabled = false;
                    btndelete.ImageUrl = "~/_layouts/15/Images/CBP_EMS_SP.PreListAppWP/Del-disable.png";
                }

            }
        }

        protected void checkTimeSlotoutofrang()
        {
            BtnConfirm.Enabled = true;
            lblMessageConfirm.Text = "";

            foreach (GridViewRow row in GridView_CPIP.Rows)
            {
                HiddenField HiddenPresentation_To = (HiddenField)row.FindControl("HiddenPresentation_To");
                //lbltest.Text += Convert.ToDateTime(HiddenPresentation_To.Value).ToString() + "<br/>";

                if (Convert.ToDateTime(HiddenPresentation_To.Value).CompareTo(m_Presentation_To) > 0)
                {
                    row.Cells[0].Font.Bold = true;
                    row.Cells[0].ForeColor = Color.FromName("red");

                    BtnConfirm.Enabled = false;
                    lblMessageConfirm.Text = "Time Slot cannot out of " + m_Presentation_From.ToString("hh:mm tt") + " To " + m_Presentation_To.ToString("hh:mm tt");
                }
            }

        }

        protected void GridView_CPIP_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            deleteAllData(m_VMID);
            getdbdata();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {

            insertSameCluster(ddlCluster, GridViewUA, GridView_CPIP);
            getdbdata();


        }

        protected void updateRowData(string m_Vetting_Application_ID, string m_Vetting_Meeting_ID, string m_Application_Number, string m_Application_Number_update)
        {
            string sql = "";
            string applicant_email = "";

            if (m_Programme_Type == "CPIP")
            {
                applicant_email = CPIPIDGetEmail(m_Application_Number_update);
            }
            else
            {
                applicant_email = CCMFIDGetEmail(m_Application_Number_update);
            }

            if (StatusConvertNum(m_Meeting_status) > 1)
            {
                sql = "UPDATE TB_VETTING_APPLICATION SET "
                    + "Application_Number= @m_Application_Number_update, "
                    + "Attend='0' , "
                    + "Mobile_Number='' , "
                    + "Name_of_Attendees='' , "
                    + "Presentation_Tools='' , "
                    + "Special_Request='' , "
                    + "Name_of_Principal_Applicationt='' , "
                    + "Email= @applicant_email "
                    + "WHERE Application_Number = @m_Application_Number "
                    + "AND Vetting_Meeting_ID= @m_Vetting_Meeting_ID "
                    + "AND Vetting_Application_ID= @m_Vetting_Application_ID ";

            }
            else
            {
                sql = "UPDATE TB_VETTING_APPLICATION SET "
                    + "Application_Number= @m_Application_Number_update, "
                    + "Attend='0' , "
                    + "Mobile_Number='' , "
                    + "Name_of_Attendees='' , "
                    + "Presentation_Tools='' , "
                    + "Special_Request='' , "
                    + "Name_of_Principal_Applicationt='' , "
                    + "Email= @applicant_email "
                    + "WHERE Application_Number = @m_Application_Number "
                    + "AND Vetting_Meeting_ID= @m_Vetting_Meeting_ID "
                    + "AND Vetting_Application_ID= @m_Vetting_Application_ID ";
            }


            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@m_Application_Number_update", m_Application_Number_update));
                cmd.Parameters.Add(new SqlParameter("@applicant_email", applicant_email));
                cmd.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Application_ID", m_Vetting_Application_ID));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void insertRowData(string m_Vetting_Meeting_ID, string m_Application_Number, string m_Presentation_From, string m_Presentation_To)
        {
            string sql = "";
            string applicant_email = "";

            if (m_Programme_Type == "CPIP")
            {
                applicant_email = CPIPIDGetEmail(m_Application_Number);
            }
            else
            {
                applicant_email = CCMFIDGetEmail(m_Application_Number);
            }


            if (StatusConvertNum(m_Meeting_status) > 1)
            {
                //sql = "UPDATE TB_VETTING_APPLICATION SET "
                //    + "Application_Number='" + m_Application_Number_update + "', "
                //    + "Attend='0' "
                //    + "WHERE Application_Number = '" + m_Application_Number + "' "
                //    + "AND Vetting_Meeting_ID='" + m_Vetting_Meeting_ID + "' "
                //    + "AND Vetting_Application_ID='" + m_Vetting_Application_ID + "'";

                sql = "INSERT INTO TB_VETTING_APPLICATION(Vetting_Application_ID, Vetting_Meeting_ID, Application_Number, Presentation_From, Presentation_To, Attend, Email ) VALUES("
                            + "NEWID(), @m_Vetting_Meeting_ID, @m_Application_Number, @m_Presentation_From, @m_Presentation_To, '0', @m_applicant_email )";


            }
            else
            {
                //sql = "UPDATE TB_VETTING_APPLICATION SET "
                //    + "Application_Number='" + m_Application_Number_update + "' "
                //    + "WHERE Application_Number = '" + m_Application_Number + "' "
                //    + "AND Vetting_Meeting_ID='" + m_Vetting_Meeting_ID + "' "
                //    + "AND Vetting_Application_ID='" + m_Vetting_Application_ID + "'";

                sql = "INSERT INTO TB_VETTING_APPLICATION(Vetting_Application_ID, Vetting_Meeting_ID, Application_Number, Presentation_From, Presentation_To, Attend, Email ) VALUES("
                            + "NEWID(), @m_Vetting_Meeting_ID, @m_Application_Number, @m_Presentation_From, @m_Presentation_To, '0', @m_applicant_email )";

            }
            //lbltest.Text = "m_Presentation_From : " + Convert.ToDateTime(m_Presentation_From).ToString("yyyy-MM-dd HH:mm:ss.fff") + " m_Presentation_To : " + Convert.ToDateTime(m_Presentation_To).ToString("yyyy-MM-dd HH:mm:ss.fff");
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                cmd.Parameters.Add(new SqlParameter("@m_applicant_email", applicant_email));
                //cmd.Parameters.Add("@m_Presentation_From", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_From);
                //cmd.Parameters.Add("@m_Presentation_To", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_To);
                //cmd.Parameters.Add("@m_Presentation_From", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_From).ToString("yyyy-MM-dd HH:mm:ss.fff");
                //cmd.Parameters.Add("@m_Presentation_To", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_To).ToString("yyyy-MM-dd HH:mm:ss.fff");
                //cmd.Parameters.Add(new SqlParameter("@m_Presentation_From", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_From).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                //cmd.Parameters.Add(new SqlParameter("@m_Presentation_To", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_To).ToString("yyyy-MM-dd HH:mm:ss.fff"));

                cmd.Parameters.Add(new SqlParameter("@m_Presentation_From", Convert.ToDateTime(m_Presentation_From)));
                cmd.Parameters.Add(new SqlParameter("@m_Presentation_To", Convert.ToDateTime(m_Presentation_To)));

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected List<TimeSlotList> insertEmptySlot(List<TimeSlotList> lstinput)
        {

            //List<TimeSlotList> lstResult = new List<TimeSlotList>();

            bool m_result = true;

            DateTime m_resulttime = m_Presentation_From ;


            while (m_resulttime < m_Presentation_To)
            {

                m_result = true;
                foreach (var v_VettingApplication in lstinput)
                {
                    //m_Presentation_From
                    //m_Presentation_To

                    //if (Convert.ToDateTime(HiddenPresentation_To.Value).CompareTo(m_Presentation_To) > 0)
                    if (Convert.ToDateTime(v_VettingApplication.Presentation_From).CompareTo(m_resulttime) == 0)
                    {
                        //lbltest.Text += v_VettingApplication.Presentation_From + v_VettingApplication.Presentation_To + "<br/>";
                        m_result = false;

                    }

                }



                if (m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).CompareTo(m_Presentation_To) > 0)
                {
                    //lbltest.Text += v_VettingApplication.Presentation_From + v_VettingApplication.Presentation_To + "<br/>";
                    m_result = false;

                }

                if (m_result)
                {
                    //lbltest.Text += "ADD:" + m_resulttime.ToString() + " To " + m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString() + "<br/>";
                    lstinput.Add(new TimeSlotList
                    {
                        TimeSlot = m_resulttime.ToString("hh:mm tt") + " - " + m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString("hh:mm tt"),
                        Vetting_Meeting_ID = m_VMID,
                        DT_Presentation_From = m_resulttime,
                        DT_Presentation_To = m_resulttime.AddMinutes(int.Parse(m_Time_Interval)),
                        Presentation_From = m_resulttime.ToString(),
                        Presentation_To = m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString()
                    });
                }
                else
                {

                }

                m_resulttime = m_resulttime.AddMinutes(int.Parse(m_Time_Interval));
            }


            //lstinput.Sort((x, y) => DateTime.Compare(x.DT_Presentation_From, y.DT_Presentation_From));
            return lstinput;
        }

        protected List<TimeSlotList> insertTimeBreak(List<TimeSlotList> lstinput)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "";
                string m_Attendance = "";

                sql = "SELECT dbo.TB_VETTING_APPLICATION.Vetting_Application_ID, dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID, dbo.TB_VETTING_APPLICATION.Application_Number, "
                + "dbo.TB_VETTING_APPLICATION.Presentation_From, dbo.TB_VETTING_APPLICATION.Presentation_To, dbo.TB_VETTING_APPLICATION.Email, dbo.TB_VETTING_APPLICATION.Mobile_Number, "
                + "dbo.TB_VETTING_APPLICATION.Attend, dbo.TB_VETTING_APPLICATION.Name_of_Attendees, dbo.TB_VETTING_APPLICATION.Presentation_Tools, dbo.TB_VETTING_APPLICATION.Special_Request, "
                + "dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng, dbo.TB_INCUBATION_APPLICATION.Business_Area "
                + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_INCUBATION_APPLICATION.Application_Number "
                + "WHERE dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = @m_VMID AND dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved' AND dbo.TB_INCUBATION_APPLICATION.Status = 'Complete Screening' "
                + "";
                //+ "ORDER BY Presentation_From ASC";

                sql = "SELECT Vetting_Application_ID, Vetting_Meeting_ID, Application_Number, Presentation_From, Presentation_To, Email, Mobile_Number, Attend, Name_of_Attendees, Presentation_Tools, Special_Request, Name_of_Principal_Applicationt, Receive_Marketing_Informatioin "
                   + "FROM TB_VETTING_APPLICATION "
                   + "WHERE (Vetting_Meeting_ID = @m_VMID) AND (Application_Number = 'Time Break')";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lstinput.Add(new TimeSlotList
                             {
                                 TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                                 Vetting_Meeting_ID = m_VMID,
                                 Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                 DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                                 DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                                 Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                                 Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                                 Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString()
                             });


                            //TimeSlot = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("hh:mm tt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("hh:mm tt"),
                            //Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString(),
                            //DT_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From")),
                            //Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString(),
                            //DT_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To")),
                            //Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                            //Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                            //Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                            //CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                            //Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                            //Attendance = AttendanceStatus(reader.GetInt32(reader.GetOrdinal("Attend"))),
                            //Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString(),
                            //Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString()


                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            //List<TimeSlotList> lstResult = new List<TimeSlotList>();

            bool m_result = true;

            //DateTime m_resulttime = m_Presentation_From;
            //
            //
            //while (m_resulttime < m_Presentation_To)
            //{
            //
            //    m_result = true;
            //    foreach (var v_VettingApplication in lstinput)
            //    {
            //        //m_Presentation_From
            //        //m_Presentation_To
            //
            //        //if (Convert.ToDateTime(HiddenPresentation_To.Value).CompareTo(m_Presentation_To) > 0)
            //        if (Convert.ToDateTime(v_VettingApplication.Presentation_From).CompareTo(m_resulttime) == 0)
            //        {
            //            //lbltest.Text += v_VettingApplication.Presentation_From + v_VettingApplication.Presentation_To + "<br/>";
            //            m_result = false;
            //
            //        }
            //
            //    }
            //
            //
            //
            //    if (m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).CompareTo(m_Presentation_To) > 0)
            //    {
            //        //lbltest.Text += v_VettingApplication.Presentation_From + v_VettingApplication.Presentation_To + "<br/>";
            //        m_result = false;
            //
            //    }
            //
            //    if (m_result)
            //    {
            //        //lbltest.Text += "ADD:" + m_resulttime.ToString() + " To " + m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString() + "<br/>";
            //        lstinput.Add(new TimeSlotList
            //        {
            //            TimeSlot = m_resulttime.ToString("hh:mm tt") + " - " + m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString("hh:mm tt"),
            //            Vetting_Meeting_ID = m_VMID,
            //            DT_Presentation_From = m_resulttime,
            //            DT_Presentation_To = m_resulttime.AddMinutes(int.Parse(m_Time_Interval)),
            //            Presentation_From = m_resulttime.ToString(),
            //            Presentation_To = m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString()
            //        });
            //    }
            //    else
            //    {
            //
            //    }
            //
            //    m_resulttime = m_resulttime.AddMinutes(int.Parse(m_Time_Interval));
            //}



            return lstinput;
        }

        protected void insertSameCluster(DropDownList m_ddlCluster, GridView m_GridView, GridView m_MainGridView)
        {
            string Selected_Cluster = m_ddlCluster.SelectedValue.ToString().Trim();
            lblMessageConfirm.Text = "";
            bool m_result = true;
            List<TimeSlotList> lstinsert = new List<TimeSlotList>();
            List<TimeSlotList> lstUSinsert = new List<TimeSlotList>();

            var m_resulttime = DateTime.Now;
            m_resulttime = m_Presentation_From;

            foreach (GridViewRow row in GridViewUA.Rows)
            {
                m_result = true;
                HiddenField HiddenApplication_Number = (HiddenField)row.FindControl("HiddenApplication_Number");
                HiddenField HiddenCluster = (HiddenField)row.FindControl("HiddenCluster");
                HiddenField HiddenShortlisted = (HiddenField)row.FindControl("HiddenShortlisted");


                //lbltest.Text += m_ddlCluster.SelectedValue.ToString().Trim();
                //lbltest.Text += HiddenCluster.Value.ToString().Trim();
                //lbltest.Text += (m_ddlCluster.SelectedValue.ToString().Trim() == HiddenCluster.Value.ToString().Trim());

                if (m_ddlCluster.SelectedValue.ToString().Trim() == HiddenCluster.Value.ToString().Trim() && HiddenShortlisted.Value.ToString().Trim() == "True")
                {
                    //lbltest.Text += "<br/> UA:" + HiddenApplication_Number.Value.ToString().Trim() + "<br/>";
                    lstUSinsert.Add(new TimeSlotList
                    {
                        Application_Number = HiddenApplication_Number.Value.ToString().Trim(),
                    });

                    foreach (GridViewRow Mainrow in m_MainGridView.Rows)
                    {
                        HiddenField HiddenPresentation_From = (HiddenField)Mainrow.FindControl("HiddenPresentation_From");
                        HiddenField HiddenPresentation_To = (HiddenField)Mainrow.FindControl("HiddenPresentation_To");
                        Label lblApplication_Number = (Label)Mainrow.FindControl("lblApplication_Number");

                        //lbltest.Text += "<br/>******************* UA: m_resulttime = " + m_resulttime.ToString().Trim() + " | HiddenPresentation_From = " + HiddenPresentation_From.Value.ToString().Trim() + "<br/>";
                        //lbltest.Text += "<br/>******************* 1 = " + (m_resulttime.ToString().Trim() == HiddenPresentation_From.Value.ToString().Trim()) + " | 2 = " + (lblApplication_Number.Text.ToString().Trim() == "") + " m_result=" + (m_result.ToString()) + "<br/>";


                        if (m_resulttime.ToString().Trim() == HiddenPresentation_From.Value.ToString().Trim() && lblApplication_Number.Text.ToString().Trim() == "" && m_result)
                        {
                            //lbltest.Text += "Target Insert : " + m_resulttime.ToString().Trim() + " " + m_resulttime.AddMinutes(int.Parse(m_Time_Interval)).ToString()
                            //             + HiddenApplication_Number.Value.ToString().Trim() +  "<br/>";

                            insertVettingApplicationData(m_VMID, HiddenApplication_Number.Value.ToString().Trim(), m_resulttime);

                            lstinsert.Add(new TimeSlotList
                            {
                                Application_Number = HiddenApplication_Number.Value.ToString().Trim(),
                            });

                            m_result = false;
                        }
                        else if (m_resulttime.ToString().Trim() == HiddenPresentation_From.Value.ToString().Trim() && lblApplication_Number.Text.ToString().Trim() != "" && m_result)
                        {
                            m_resulttime = m_resulttime.AddMinutes(int.Parse(m_Time_Interval));
                        }

                    }

                    m_resulttime = m_resulttime.AddMinutes(int.Parse(m_Time_Interval));
                }


            }

            //lblMessageConfirm.Text = lstinsert.Count.ToString() + " / " + lstUSinsert.Count.ToString();
            if (lstinsert.Count < lstUSinsert.Count && lstinsert.Count != 0)
            {
                lblMessageConfirm.Text = "Because of lack of free time slots, " + lstinsert.Count + " applications are inserted";
            }
            else if (lstinsert.Count < lstUSinsert.Count)
            {
                // Normail status , should not display.
            }
            else if (lstinsert.Count == 0  && lstUSinsert.Count > 0)
            {
                lblMessageConfirm.Text = "Timeslots are full. No applications are inserted.";
            }
            //if (lstinsert.Count > 0 && lstUninsert.Count > 0)
            //{
            //    lblMessageConfirm.Text = "Because of lack of free time slots, " + lstinsert.Count + " applications are inserted";
            //}
            //else if (lstinsert.Count == 0 && lstUninsert.Count > 0)
            //{
            //    lblMessageConfirm.Text = "Timeslots are full. No applications are inserted.";
            //}

        }

        protected void insertSameCluster_backup(DropDownList m_ddlCluster, GridView m_GridView, GridView m_MainGridView)
        {
            string Selected_Cluster = m_ddlCluster.SelectedValue.ToString().Trim();
            lblMessageConfirm.Text = "";

            List<TimeSlotList> lstinsert = new List<TimeSlotList>();
            List<TimeSlotList> lstUninsert = new List<TimeSlotList>();

            var m_resulttime = DateTime.Now;
            m_resulttime = m_Presentation_From;

            foreach (GridViewRow row in GridViewUA.Rows)
            {
                HiddenField HiddenApplication_Number = (HiddenField)row.FindControl("HiddenApplication_Number");
                HiddenField HiddenCluster = (HiddenField)row.FindControl("HiddenCluster");
                //lbltest.Text += m_ddlCluster.SelectedValue.ToString().Trim();
                //lbltest.Text += HiddenCluster.Value.ToString().Trim();
                //lbltest.Text += (m_ddlCluster.SelectedValue.ToString().Trim() == HiddenCluster.Value.ToString().Trim());

                if (m_ddlCluster.SelectedValue.ToString().Trim() == HiddenCluster.Value.ToString().Trim())
                {
                    foreach (GridViewRow Mainrow in m_MainGridView.Rows)
                    {
                        HiddenField HiddenPresentation_From = (HiddenField)Mainrow.FindControl("HiddenPresentation_From");
                        HiddenField HiddenPresentation_To = (HiddenField)Mainrow.FindControl("HiddenPresentation_To");
                        if (m_resulttime.ToString().Trim() == HiddenPresentation_From.Value.ToString().Trim())
                        {

                            m_resulttime = m_resulttime.AddMinutes(int.Parse(m_Time_Interval));
                        }
                    }


                    //m_Presentation_From
                    //m_Presentation_To
                    //if (Convert.ToDateTime(HiddenPresentation_To.Value).CompareTo(m_Presentation_To) > 0)
                    if (m_resulttime.CompareTo(m_Presentation_To) >= 0)
                    {
                        //lbltest2.Text += HiddenApplication_Number.Value.ToString().Trim();
                        lstUninsert.Add(new TimeSlotList
                        {
                            Application_Number = HiddenApplication_Number.Value.ToString().Trim(),
                        });
                    }
                    else
                    {
                        insertVettingApplicationData(m_VMID, HiddenApplication_Number.Value.ToString().Trim(), m_resulttime);
                        //lbltest.Text += HiddenApplication_Number.Value.ToString().Trim();
                        lstinsert.Add(new TimeSlotList
                        {
                            Application_Number = HiddenApplication_Number.Value.ToString().Trim(),
                        });

                    }
                    //lbltest.Text += "<br/>Insert Time:" + m_resulttime.ToString() + "Application No:" + HiddenApplication_Number.Value.ToString() ;
                    m_resulttime = m_resulttime.AddMinutes(int.Parse(m_Time_Interval));
                }


            }

            //lblMessageConfirm.Text = lstinsert.Count.ToString() + " / " + lstUninsert.Count.ToString();

            //if (lstinsert.Count > 0 && lstUninsert.Count > 0)
            //{
            //    lblMessageConfirm.Text = "Because of lack of free time slots, " + lstinsert.Count + " applications are inserted";
            //}
            //else if (lstinsert.Count == 0 && lstUninsert.Count > 0)
            //{
            //    lblMessageConfirm.Text = "Timeslots are full. No applications are inserted.";
            //}

        }

        protected void insertVettingApplicationData(string m_Vetting_Meeting_ID, string m_Application_Number, DateTime m_Presentation_From)
        {
            string applicant_email = "";

            if (m_Programme_Type == "CPIP")
            {
                applicant_email = CPIPIDGetEmail(m_Application_Number);
            }
            else
            {
                applicant_email = CCMFIDGetEmail(m_Application_Number);
            }
            //andy
            string sql="";
            var m_Presentation_To = m_Presentation_From.AddMinutes(int.Parse(m_Time_Interval));
            //sql = "INSERT INTO TB_VETTING_APPLICATION(Vetting_Application_ID, Vetting_Meeting_ID, Application_Number, Presentation_From, Presentation_To,Attend) VALUES("
            //            + "NEWID(), "
            //            + "'" + m_Vetting_Meeting_ID + "', "
            //            + "'" + m_Application_Number + "',"
            //            + "'" + m_Presentation_From + "' , "
            //            + "'" + m_Presentation_To + "' , "
            //            + "'" + 0 + "');";

            sql = "INSERT INTO TB_VETTING_APPLICATION(Vetting_Application_ID, Vetting_Meeting_ID, Application_Number, Presentation_From, Presentation_To, Attend, Email ) VALUES("
                            + "NEWID(), @m_Vetting_Meeting_ID, @m_Application_Number, @m_Presentation_From, @m_Presentation_To, '0', @m_applicant_email )";

            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                cmd.Parameters.Add(new SqlParameter("@m_applicant_email", applicant_email));
                //cmd.Parameters.Add(new SqlParameter("@m_Presentation_From", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_From).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                //cmd.Parameters.Add(new SqlParameter("@m_Presentation_To", SqlDbType.DateTime).Value = Convert.ToDateTime(m_Presentation_To).ToString("yyyy-MM-dd HH:mm:ss.fff"));

                cmd.Parameters.Add(new SqlParameter("@m_Presentation_From", Convert.ToDateTime(m_Presentation_From)));
                cmd.Parameters.Add(new SqlParameter("@m_Presentation_To", Convert.ToDateTime(m_Presentation_To)));

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void deleteRowData(string m_Vetting_Application_ID, string m_Vetting_Meeting_ID, string m_Application_Number)
        {
            string sql = "";

            sql = "DELETE FROM TB_VETTING_APPLICATION "
                + "WHERE Application_Number = @m_Application_Number "
                + "AND Vetting_Meeting_ID= @m_Vetting_Meeting_ID "
                + "AND Vetting_Application_ID= @m_Vetting_Application_ID ";

            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Application_ID", m_Vetting_Application_ID));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void deleteAllData(string m_Vetting_Meeting_ID)
        {
            string sql = "";

            sql = "DELETE FROM TB_VETTING_APPLICATION "
                + "WHERE Vetting_Meeting_ID=@m_Vetting_Meeting_ID ";

            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected string checkProgrammeType(string m_Programme_ID)
        {
            var m_INCUBATION = 0;
            var m_CCMF = 0;
            var m_result = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Programme_ID = @m_Programme_ID ", conn))
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
                    try
                    {
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

        protected int StatusConvertNum(string m_Meeting_status)
        {
            int StatusNum = 0;

            if (m_Meeting_status == "")
            {
                StatusNum = 0;
            }
            else if (m_Meeting_status == "Vetting Meeting Create")
            {
                StatusNum = 1;
            }
            else if (m_Meeting_status == "Email Sended")
            {
                StatusNum = 2;
            }
            else if (m_Meeting_status == "Invite Email Sent")
            {
                StatusNum = 3;
            }

            return StatusNum;
        }

        protected void btnSubmitPop_Click(object sender, EventArgs e)
        {


            getSYSTEMPARAMETER();
            //VettingTeamEmailList(m_VMID);
            ApplicantEmailList();
            updateMeetingStatus(m_VMID);


            getReview();
            getVettingMeetingInfo();
            getdbdata();
            checkStatusControlBtn();

        }

        protected void updateMeetingStatus(string m_Vetting_Meeting_ID)
        {
            //Email Sended

            string sql = "";

            sql = "UPDATE TB_VETTING_MEETING SET "
                    + "Meeting_status='Email Sended', "
                    + "Modified_Date= GETDATE(), "
                    + "Modified_By= @m_systemuser "
                    + "WHERE Vetting_Meeting_ID = @m_Vetting_Meeting_ID ";

            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@m_systemuser", m_systemuser));
                cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void sendToApplicant(string m_EmailList, string m_Presentation_From, string m_Presentation_To, string m_Vetting_Application_ID)
        {

            //m_WebsiteUrl += m_WebsiteUrl_VettingTeam;
            string m_passvalue = "?VAID=" + m_Vetting_Application_ID + "&VMID=" + m_VMID;
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
            }
            else
            {
                m_mail = m_EmailList;
            }

            //m_mail = "m62737166@gmail.com";
            //m_mail = "m62737166@gmail.com;andy.choi@mouxidea.com.hk";
            //lbltest.Text += m_EmailList;
            //starting email
            //m_subject = "Presentation Notification[" + m_EmailList + "]";
            //var m_tempdate = m_VettingDate.ToString("dd MMMM yyyy");
            m_subject = "Invitation to Presentation Session for " + m_Intake_Number;

            m_body = "";
            if (m_Programme_Name.Contains("Cyberport Incubation Program"))
            {
                //CPIP
                m_subject = "Invitation to Presentation Session for CPIP" + m_Intake_Number;
                m_body = getEmailTemplate("Presentation_Invitation_CPIP");
            }
            else
            {
                //CCMF
                m_subject = "Invitation to Presentation Session for CCMF " + m_Intake_Number;
                m_body = getEmailTemplate("Presentation_Invitation_CCMF");
            }
            //m_body = m_body.Replace("@@ProgramName", m_Programme_Name)
            //        .Replace("@@Email", m_EmailList)
            //        .Replace("@@IntakeNumber", m_Intake_Number)
            //        .Replace("@@VettingDate", m_VettingDate.ToString("dd MMMM yyyy") )
            //        .Replace("@@VettingVenue ", m_VettingVenue)
            //        .Replace("@@PresentationFrom", Convert.ToDateTime(m_Presentation_From).ToString("hh:mm tt"))
            //        .Replace("@@PresentationTo", Convert.ToDateTime(m_Presentation_To).ToString("hh:mm tt"))
            //        .Replace("@@WebsiteUrl", m_WebsiteUrl + m_WebsiteUrl_InvitationResponse + m_passvalue);

            m_body = m_body
                    .Replace("@@IntakeNumber", m_Intake_Number)
                    .Replace("@@VettingDate", m_VettingDate.ToString("dddd, d MMMM yyyy"))
                    .Replace("@@VettingVenue", m_VettingVenue)
                    .Replace("@@PresentationFrom", Convert.ToDateTime(m_Presentation_From).ToString("hh:mm tt"))
                    .Replace("@@WebsiteUrl", m_WebsiteUrl + m_WebsiteUrl_InvitationResponse + m_passvalue)
                    .Replace("@@ConfirmDeadline", m_ConfirmDeadline.ToString("h:mm tt, d MMMM yyyy"))
                    .Replace("@@Deadline", m_Deadline.ToString("h:mm tt, d MMMM yyyy"))
                    .Replace("@@DecentralizeProduct", "Decentralize Product");

            if (m_ApplicationIsInDebug == "1")
            {
                m_body = "(" + m_EmailList + " " + DateTime.Now.ToString() +" )" + m_body;
            }

            if (!String.IsNullOrEmpty(m_EmailList))
            {
                var CCUsers = GetSendToApplicantCCEmail(m_Vetting_Application_ID);
                sharepointsendemail(m_mail, CCUsers, m_subject, m_body);
            }
            //lbltest.Text += "m_ApplicationDebugEmailSentTo" + m_ApplicationDebugEmailSentTo + "<br/>";
            //lbltest.Text += "sendToApplicant:" + m_mail + m_Presentation_From + m_Presentation_To + m_Vetting_Application_ID + "<br/>";
        }

        protected void sendToApplicant_cancel(string m_EmailList, string m_Presentation_From, string m_Presentation_To, string m_Vetting_Application_ID)
        {

            string m_passvalue = "?VAID=" + m_Vetting_Application_ID + "&VMID=" + m_VMID;
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
            }
            else
            {
                m_mail = m_EmailList;
            }

            m_subject = "Presentation Notification Cancel";
            m_body = getEmailTemplate("Presentation_Invitation_cancel");
            m_body = m_body.Replace("@@ProgramName", m_Programme_Name)
                    .Replace("@@Email", m_EmailList)
                    .Replace("@@IntakeNumber", m_Intake_Number)
                    .Replace("@@VettingDate", m_VettingDate.ToString("dd MMMM yyyy"))
                    .Replace("@@VettingVenue ", m_VettingVenue)
                    .Replace("@@PresentationFrom", Convert.ToDateTime(m_Presentation_From).ToString("hh:mm tt"))
                    .Replace("@@PresentationTo", Convert.ToDateTime(m_Presentation_To).ToString("hh:mm tt"))
                    .Replace("@@WebsiteUrl", m_WebsiteUrl + m_WebsiteUrl_InvitationResponse + m_passvalue);


            if (m_ApplicationIsInDebug == "1")
            {
                m_body = "(" + m_EmailList + " " + DateTime.Now.ToString() + ")" + m_body;
            }
            else
            {

            }

            if (!String.IsNullOrEmpty(m_EmailList))
            {
                sharepointsendemail(m_mail, m_subject, m_body);
            }
        }


        protected void ApplicantEmailList()
        {
            // All of GridView Applicant
            foreach (GridViewRow Mainrow in GridView_CPIP.Rows)
            {
                Label lblApplication_Number = (Label)Mainrow.FindControl("lblApplication_Number");
                HiddenField HiddenPresentation_From = (HiddenField)Mainrow.FindControl("HiddenPresentation_From");
                HiddenField HiddenPresentation_To = (HiddenField)Mainrow.FindControl("HiddenPresentation_To");
                HiddenField HiddenVetting_Application_ID = (HiddenField)Mainrow.FindControl("HiddenVetting_Application_ID");



                if (lblApplication_Number.Text.ToString().Trim() != "")
                {

                    if (m_Programme_Type == "CPIP")
                    {
                        sendToApplicant(CPIPIDGetEmail(lblApplication_Number.Text), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                    }
                    else
                    {
                        sendToApplicant(CCMFIDGetEmail(lblApplication_Number.Text), HiddenPresentation_From.Value.ToString().Trim(), HiddenPresentation_To.Value.ToString().Trim(), HiddenVetting_Application_ID.Value.ToString().Trim());
                    }
                }


            }
        }

        protected string CPIPIDGetEmail(string m_Application_Number)
        {
            var tempEmail = "";
            ConnectOpen();
            try
            {
                var sqlString = "";

                /****** Script for SelectTopNRows command from SSMS  ******/
                //sqlString = "SELECT dbo.TB_INCUBATION_APPLICATION.Applicant, dbo.aspnet_Membership.Email, dbo.aspnet_Membership.LoweredEmail, dbo.TB_INCUBATION_APPLICATION.Status, "
                //            + "dbo.TB_INCUBATION_APPLICATION.Application_Number,dbo.TB_INCUBATION_APPLICATION.Incubation_ID "
                //            + "FROM dbo.TB_INCUBATION_APPLICATION INNER JOIN dbo.aspnet_Users ON dbo.TB_INCUBATION_APPLICATION.Applicant = dbo.aspnet_Users.UserName INNER JOIN "
                //            + "dbo.aspnet_Membership ON dbo.aspnet_Users.UserId = dbo.aspnet_Membership.UserId "
                //            + "WHERE dbo.TB_INCUBATION_APPLICATION.Status = 'Complete Screening' and dbo.TB_INCUBATION_APPLICATION.Application_Number ='" + m_Application_Number + "' "
                //            + "ORDER BY dbo.TB_INCUBATION_APPLICATION.Application_Number ";

                sqlString = "SELECT Incubation_ID,Programme_ID,Application_Number,Intake_Number,Applicant,Last_Submitted,Status "
                          + "FROM TB_INCUBATION_APPLICATION "
                          + "where Status = 'Complete Screening' and Application_Number= @m_Application_Number";

                //lbltest.Text += sqlString;

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //tempEmail = reader.GetValue(reader.GetOrdinal("Email")).ToString();
                    tempEmail = reader.GetValue(reader.GetOrdinal("Applicant")).ToString();
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            return tempEmail;
        }

        protected string CCMFIDGetEmail(string m_Application_Number)
        {
            var tempEmail = "";
            ConnectOpen();
            try
            {
                var sqlString = "";

                /****** Script for SelectTopNRows command from SSMS  ******/
                //sqlString = "SELECT dbo.TB_CCMF_APPLICATION.Applicant, dbo.aspnet_Membership.Email, dbo.aspnet_Membership.LoweredEmail, dbo.TB_CCMF_APPLICATION.Status, "
                //            + "dbo.TB_CCMF_APPLICATION.Application_Number,dbo.TB_CCMF_APPLICATION.CCMF_ID "
                //            + "FROM dbo.TB_CCMF_APPLICATION INNER JOIN dbo.aspnet_Users ON dbo.TB_CCMF_APPLICATION.Applicant = dbo.aspnet_Users.UserName INNER JOIN "
                //            + "dbo.aspnet_Membership ON dbo.aspnet_Users.UserId = dbo.aspnet_Membership.UserId "
                //            + "WHERE dbo.TB_CCMF_APPLICATION.Status = 'Complete Screening' and dbo.TB_CCMF_APPLICATION.Application_Number ='" + m_Application_Number + "' "
                //            + "ORDER BY dbo.TB_CCMF_APPLICATION.Application_Number ";
                //lbltest.Text += sqlString;

                sqlString = "SELECT CCMF_ID ,Programme_ID ,Application_Number ,Intake_Number ,Applicant ,Last_Submitted ,Status "
                          + "FROM TB_CCMF_APPLICATION "
                          + "where Status = 'Complete Screening' and Application_Number= @m_Application_Number";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tempEmail = reader.GetValue(reader.GetOrdinal("Applicant")).ToString();
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            return tempEmail;
        }

        protected String getEmailTemplate(string emailTemplate)
        {
            String emailTemplateContent = "";
            ConnectOpen();
            try {
                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template=@emailTemplate ;";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@emailTemplate",emailTemplate));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emailTemplateContent = reader.GetString(1);
                }

                reader.Dispose();
                command.Dispose();


            }
            finally
            {
                ConnectClose();
            }

            return emailTemplateContent;


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
                    if (reader.GetString(0) == "ApplicationIsInDebug")
                    {
                        //1,0
                        m_ApplicationIsInDebug = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "ApplicationDebugEmailSentTo")
                    {
                        m_ApplicationDebugEmailSentTo = reader.GetString(1);
                    }

                    if (reader.GetString(0) == "WebsiteUrl")
                    {
                        m_WebsiteUrl = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "WebsiteUrl_VettingTeam")
                    {
                        m_WebsiteUrl_VettingTeam = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "WebsiteUrl_InvitationResponse")
                    {
                        m_WebsiteUrl_InvitationResponse = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "CCEmailCPIP")
                    {
                        _CCEmailCPIP = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "CCEmailCCMF")
                    {
                        _CCEmailCCMF = reader.GetString(1);

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

        protected void GridViewUA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Image ImgSh = (System.Web.UI.WebControls.Image)e.Row.FindControl("ImgSh");
                Label lblRowNumber = (Label)e.Row.FindControl("lblRowNumber");
                HiddenField HiddenShortlisted = (HiddenField)e.Row.FindControl("HiddenShortlisted");
                Label lblApplication_Number = (Label)e.Row.FindControl("lblApplication_Number");

                string hex = "#80C343";
                Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                lblApplication_Number.ForeColor = _color;

                if (HiddenShortlisted.Value.ToString().Trim() == "True")
                {
                    ImgSh.Visible = true;

                    //string hex = "#8AD5F7";
                    //Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                    //e.Row.Cells[0].ForeColor = _color;

                    //string hex = "#8AD5F7";
                    //Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                    //lblRowNumber.ForeColor = _color;

                    //lblRowNumber.Attributes.Add("class", "active");
                    //lblRowNumber.CssClass = "active";
                }
                else
                {

                }

            }
        }

        protected void BtnPRS_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Presentation Result Summary.aspx?VMID=" + m_VMID);
        }

        protected void GridViewUA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            // Retrieve the row that contains the button 
            // from the Rows collection.
            GridViewRow row = GridViewUA.Rows[index];
            HiddenField hiddenApplication_Number = (HiddenField)row.FindControl("HiddenApplication_Number");
            HiddenField hiddenApplicationId = (HiddenField)row.FindControl("HiddenApplicationId");
            HiddenField hiddenProgramId = (HiddenField)row.FindControl("HiddenProgramId");

            HiddenWithdrawApplication_Number.Value = hiddenApplication_Number.Value;
            HiddenWithdrawApplicationId.Value = hiddenApplicationId.Value;
            HiddenWithdrawProgramId.Value = hiddenProgramId.Value;

            lblWithdrawMessageboxbody.Text = "Are you confirm withdraw " + hiddenApplication_Number.Value + "?";

            isShowWithdrawPopUp = true;
        }

        protected void unshortlistWithdrawApplicaiton(string AppNo, string ProgID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //var sql = "Update TB_APPLICATION_SHORTLISTING set Shortlisted = 0 where Appliction_Number = '" + AppNo + "' and Programme_ID = '" + ProgID + "'";
                var sqlStr = "Update TB_APPLICATION_SHORTLISTING set Shortlisted = 0 where Application_Number = @AppNo and Programme_ID = @ProgID";

                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);

                    cmd.Parameters.Add("@AppNo", AppNo);
                    cmd.Parameters.Add("@ProgID", ProgID);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                }
                finally
                {
                    ConnectClose();
                }
            }
        }

        protected void btnWithdraw_Click(object sender, EventArgs e)
        {
            var m_ApplicationNumber = HiddenWithdrawApplication_Number.Value;
            var m_ApplicationID = HiddenWithdrawApplicationId.Value;
            var m_progid = HiddenWithdrawProgramId.Value;
            if (!String.IsNullOrEmpty(m_ApplicationID))
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var sql = "select tba.Applicant,tpi.Programme_Name,tpi.Intake_Number,tacd.First_Name_Eng,tacd.Last_Name_Eng from TB_CCMF_APPLICATION  tba left join TB_PROGRAMME_INTAKE tpi on tpi.Programme_ID = tba.Programme_ID left join TB_APPLICATION_CONTACT_DETAIL tacd on tacd.Application_ID = tba.CCMF_ID where tba.CCMF_ID=@Application_ID and tba.Programme_ID = @Programme_ID "
                                + " union all "
                                + "select tba.Applicant,tpi.Programme_Name,tpi.Intake_Number,tacd.First_Name_Eng,tacd.Last_Name_Eng from TB_INCUBATION_APPLICATION tba left join TB_PROGRAMME_INTAKE tpi on tpi.Programme_ID = tba.Programme_ID left join TB_APPLICATION_CONTACT_DETAIL tacd on tacd.Application_ID = tba.Incubation_ID where tba.Incubation_ID = @Application_ID and tba.Programme_ID = @Programme_ID ";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Application_ID", m_ApplicationID));
                        cmd.Parameters.Add(new SqlParameter("@Programme_ID", m_progid));
                        conn.Open();
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                var Programme_Name = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                                var Intake_Number = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                                var Applicant = reader.GetValue(reader.GetOrdinal("Applicant")).ToString();
                                var FirstName = reader.GetValue(reader.GetOrdinal("First_Name_Eng")).ToString();
                                var LastName = reader.GetValue(reader.GetOrdinal("Last_Name_Eng")).ToString();
                                var CoordinatorGroupName = "";

                                getSYSTEMPARAMETER();
                                if (m_ApplicationIsInDebug == "1")
                                {
                                    Applicant = m_ApplicationDebugEmailSentTo;
                                }
                                if (Programme_Name.Contains("Cyberport Incubation Programme"))
                                {
                                    //CPIP
                                    CoordinatorGroupName = "CPIP Coordinator";
                                    UpdateApplicationStatus("TB_INCUBATION_APPLICATION", m_ApplicationID, "Withdraw");
                                    unshortlistWithdrawApplicaiton(m_ApplicationNumber, m_progid);
                                }
                                else
                                {
                                    //CCMF
                                    CoordinatorGroupName = "CCMF Coordinator";
                                    UpdateApplicationStatus("TB_CCMF_APPLICATION", m_ApplicationID, "Withdraw");
                                    unshortlistWithdrawApplicaiton(m_ApplicationNumber, m_progid);
                                }
                                var Users = getGroupUserEmail(CoordinatorGroupName);
                                //lbltest.Text = m_ApplicationNumber +"-"+ m_progid;

                                var m_subject = "Confirmed Withdrawal (@@AppNumber) from Programme @@programName - @@porgrammeIntake";
                                m_subject = m_subject.Replace("@@AppNumber", m_ApplicationNumber).Replace("@@programName", Programme_Name).Replace("@@porgrammeIntake", Intake_Number);
                                var m_body = getEmailTemplate("WithdrawEmail");
                                m_body = m_body.Replace("@@appllicant", FirstName + " " + LastName).Replace("@@AppNumber", m_ApplicationNumber).Replace("@@ProgramName", Programme_Name).Replace("@@IntakeNumber", Intake_Number);
                                sharepointsendemail(Applicant, Users, m_subject, m_body);


                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }
            }
            getdbdata();
        }

        protected void UpdateApplicationStatus(String tableName, String ApplicationID, String statusReplaceValue)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "";
                if (tableName == "TB_CCMF_APPLICATION")
                {
                    sqlUpdate = "update " + tableName + " set "
                                            + "Status = @statusReplaceValue "
                                            + "where CCMF_ID=@ApplicationID";
                }
                else
                {
                    sqlUpdate = "update " + tableName + " set "
                                            + "Status = @statusReplaceValue "
                                            + "where Incubation_ID=@ApplicationID";
                }

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add("@statusReplaceValue", statusReplaceValue);
                command.Parameters.Add("@ApplicationID", ApplicationID);

                command.ExecuteNonQuery();


                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

        }

        protected String getGroupUserEmail(String groupName)
        {

            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.LoginName.ToString(); //"Flora Yeung";            
            //lblrole.Text = SPContext.Current.Web.AllRolesForCurrentUser.ToString(); //"CCMF BDM";
            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.Name.ToString();
            //lblrole.Text = "";
            var UserStr = "";

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPGroup groupObject = null;
                        foreach (SPGroup group in web.Groups)
                        {
                            if (group.Name == groupName)
                            {
                                //lbltest.Text += "[" + group.Name + "]";
                                groupObject = group;
                            }
                        }

                        if (groupObject != null)
                        {
                            foreach (SPUser user in groupObject.Users)
                            {
                                UserStr += user.Email + ";";
                            }
                        }

                    }
                }
            });

            return UserStr;

        }

        protected void sharepointsendemail(string toAddress, string ccAddress, string subject, string body)
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
                                         //SPUtility.SendEmail(web, false, false,
                                         //toAddress,
                                         //subject,
                                         //body);

                                         StringDictionary headers = new StringDictionary();
                                         headers.Add("to", toAddress);
                                         headers.Add("cc", ccAddress);
                                         headers.Add("bcc", "");
                                         //headers.Add("from", "admin@test.com");
                                         headers.Add("subject", subject);
                                         headers.Add("content-type", "text/html");
                                         SPUtility.SendEmail(web, headers, body);
                                     }
                                 }
                             });
        }

        protected void btnDecisionSummary_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("DecisionSummary.aspx?VMID=" + m_VMID);

        }

        protected String GetSendToApplicantCCEmail(string VPID)
        {
            String emails = "";
            ConnectOpen();
            try
            {
                var sqlString = "";
                if (lblProName.Text.Contains("Cyberport Incubation Programme"))
                {
                    //CPIP
                    sqlString = "select distinct isnull(Email,'') Email from ( "
                                + " select tbacollaborator.Email from TB_VETTING_APPLICATION tbva inner join TB_APPLICATION_COLLABORATOR tbacollaborator on tbva.Application_Number = tbacollaborator.Application_Number where tbva.Vetting_Application_ID = @Vetting_Application_ID "
                                + " union all"
                                + " select tbacd.Email from TB_VETTING_APPLICATION tbva inner join TB_INCUBATION_APPLICATION tapp on tbva.Application_Number = tapp.Application_Number left join TB_APPLICATION_CONTACT_DETAIL tbacd on tapp.Incubation_ID = tbacd.Application_ID where tbva.Vetting_Application_ID = @Vetting_Application_ID "
                                + ") emails ";
                    emails = !string.IsNullOrEmpty(_CCEmailCPIP) ? _CCEmailCPIP + ";" : "";
                }
                else
                {
                    //CCMF
                    sqlString = "select distinct isnull(Email,'') Email from ( "
                                + " select tbacollaborator.Email from TB_VETTING_APPLICATION tbva inner join TB_APPLICATION_COLLABORATOR tbacollaborator on tbva.Application_Number = tbacollaborator.Application_Number where tbva.Vetting_Application_ID = @Vetting_Application_ID "
                                + " union all"
                                + " select tbacd.Email from TB_VETTING_APPLICATION tbva inner join TB_CCMF_APPLICATION tapp on tbva.Application_Number = tapp.Application_Number left join TB_APPLICATION_CONTACT_DETAIL tbacd on tapp.CCMF_ID = tbacd.Application_ID where tbva.Vetting_Application_ID = @Vetting_Application_ID "
                                + ") emails ";
                    emails = !string.IsNullOrEmpty(_CCEmailCCMF) ? _CCEmailCCMF + ";" : "";
                }

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Vetting_Application_ID", VPID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //tempEmail = reader.GetValue(reader.GetOrdinal("Email")).ToString();
                    var tempEmail = reader.GetValue(reader.GetOrdinal("Email")).ToString();
                    if (!String.IsNullOrEmpty(tempEmail))
                    {
                        emails += tempEmail + ";";
                    }
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            
            return emails;
        }

    }
}
