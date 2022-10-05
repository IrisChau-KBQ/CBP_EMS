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
using System.Linq;

namespace CBP_EMS_SP.AppProgIntake.AppProgIntake
{
    [ToolboxItemAttribute(false)]
    public partial class AppProgIntake : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        string m_prog = "";
        string m_mid = "";
        string m_systemuser = "";
        string m_Role = "";
        string m_Programme_Type = "";
        string m_Programme_ID = "";
        string m_VMID = "";
        private SqlConnection connection;

        public String m_path = "";
        public String m_programName;
        public String m_meeting;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "";
        public String ImpersonateUser = "";
        public String IsLogEvent;

        public List<String> Video_ClipPresentation_SlidePathList;

        public double m_DownloadFileSize = 0;

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
            public string Application_ID { set; get; }
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
            public DateTime ? DT_Presentation_From { set; get; }
            public DateTime ? DT_Presentation_To { set; get; }
            public string Vetting_Meeting_Date { set; get; }
            public string Total_Score { set; get; }
            public string Remarks { set; get; }
            public string Go { set; get; }
            public string Projectdescription { set; get; }
            public string Remarksforvettingteam { set; get; }
            public Boolean ShortlistedChecked { set; get; }
            public string AverageScore { set; get; }
            public string Hong_Kong_Programme_Stream { set; get; }
        }

        public AppProgIntake()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            m_prog = HttpContext.Current.Request.QueryString["prog"];
            m_meeting = HttpContext.Current.Request.QueryString["mid"];
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();

            if (string.IsNullOrEmpty(m_prog))
            {
                return;
            }

            getReview();
            getSYSTEMPARAMETER();
            getProgrammeIntakeInfo();

            if (!Page.IsPostBack)
            {
                getdbdata();                
            }
        }

        protected void getdbdata()
        {
            ShortlistedDDL(ddlShortlisted);
            StreamDDL(ddlStream);

            getApplication(ddlDateFilter);
            
        }

        protected void ClusterDDl(DropDownList m_ddlCluster, string m_ddlDateFilter)
        {
            m_ddlCluster.Items.Clear();
            m_ddlCluster.Items.Add(new ListItem("All Date", "All Date"));
            string m_Cluster = "";
            string sql = "";

            if (m_Programme_Type == "CCMF")
            {


                sql = "SELECT DISTINCT TOP (100) PERCENT dbo.TB_VETTING_MEETING.Date "
                    + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                    + "dbo.TB_VETTING_MEETING ON dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = dbo.TB_VETTING_MEETING.Vetting_Meeting_ID INNER JOIN "
                    + "dbo.TB_CCMF_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_CCMF_APPLICATION.Application_Number INNER JOIN "
                    + "dbo.TB_VETTING_MEMBER ON dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = dbo.TB_VETTING_MEMBER.Vetting_Meeting_ID INNER JOIN "
                    + "dbo.TB_VETTING_MEMBER_INFO ON dbo.TB_VETTING_MEMBER.Vetting_Member_ID = dbo.TB_VETTING_MEMBER_INFO.Vetting_Member_ID LEFT OUTER JOIN "
                    + "dbo.TB_PRESENTATION_INCUBATION_SCORE ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_PRESENTATION_INCUBATION_SCORE.Application_Number "
                    + "WHERE (dbo.TB_CCMF_APPLICATION.Status <> 'Saved') AND (dbo.TB_CCMF_APPLICATION.Programme_ID = @Programme_ID) AND (dbo.TB_CCMF_APPLICATION.Status = 'Complete Screening') AND "
                    + "(dbo.TB_VETTING_MEMBER_INFO.Email = @Email) "
                    + "ORDER BY dbo.TB_VETTING_MEETING.Date ";

            }
            else
            {
                //CPIP
                sql = "SELECT DISTINCT TOP (100) PERCENT dbo.TB_VETTING_MEETING.Date "
                    + "FROM dbo.TB_VETTING_APPLICATION INNER JOIN "
                    + "dbo.TB_VETTING_MEETING ON dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = dbo.TB_VETTING_MEETING.Vetting_Meeting_ID INNER JOIN "
                    + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_INCUBATION_APPLICATION.Application_Number INNER JOIN "
                    + "dbo.TB_VETTING_MEMBER ON dbo.TB_VETTING_APPLICATION.Vetting_Meeting_ID = dbo.TB_VETTING_MEMBER.Vetting_Meeting_ID INNER JOIN "
                    + "dbo.TB_VETTING_MEMBER_INFO ON dbo.TB_VETTING_MEMBER.Vetting_Member_ID = dbo.TB_VETTING_MEMBER_INFO.Vetting_Member_ID LEFT OUTER JOIN "
                    + "dbo.TB_PRESENTATION_INCUBATION_SCORE ON dbo.TB_VETTING_APPLICATION.Application_Number = dbo.TB_PRESENTATION_INCUBATION_SCORE.Application_Number "
                    + "WHERE (dbo.TB_VETTING_MEMBER_INFO.Email = @Email) AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved') AND (dbo.TB_INCUBATION_APPLICATION.Programme_ID = @Programme_ID) AND "
                    + "(dbo.TB_INCUBATION_APPLICATION.Status = 'Complete Screening') "
                    + "ORDER BY dbo.TB_VETTING_MEETING.Date";

            }
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Programme_ID", m_prog);
                    cmd.Parameters.AddWithValue("@Email", m_systemuser);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //lbltest.Text += reader.GetDateTime(reader.GetOrdinal("Date")).ToString("dd MMMM, yyyy") + " " + reader.GetDateTime(reader.GetOrdinal("Date")).ToString() + " ||";

                        m_ddlCluster.Items.Add(new ListItem(reader.GetDateTime(reader.GetOrdinal("Date")).ToString("dd MMMM, yyyy"), reader.GetDateTime(reader.GetOrdinal("Date")).ToString()));
                    }
                    conn.Close();
                }
            }

            //set default selectvalue
            var Datestatus = false;
            var DatestatusTest = "";
            foreach(ListItem item in m_ddlCluster.Items){
                if (item.Value != "All Date")
                {
                    var itemdate = Convert.ToDateTime(item.Value);
                    if (DateTime.Now.Date <= itemdate.Date)
                    {
                        m_ddlCluster.SelectedValue = item.Value;
                        Datestatus = true;
                        break;
                    }
                }

                DatestatusTest = item.Value;
            }
            if (!Datestatus)
            {
                m_ddlCluster.SelectedValue = DatestatusTest;
            }

        }

        protected void ShortlistedDDL(DropDownList m_ddlShortlisted) 
        {
            m_ddlShortlisted.Items.Clear();
            m_ddlShortlisted.Items.Add(new ListItem("All ", "All"));
            m_ddlShortlisted.Items.Add(new ListItem("Shortlisted", "Shortlisted"));
            m_ddlShortlisted.Items.Add(new ListItem("Non Shortlisted", "Non Shortlisted"));
             //shortlisted, non shortlisted and All,
        }

        protected void StreamDDL(DropDownList m_ddlStream)
        {
            m_ddlStream.Items.Clear();
            m_ddlStream.Items.Add(new ListItem("All ", "All"));
            m_ddlStream.Items.Add(new ListItem("YEP", "YEP"));
            m_ddlStream.Items.Add(new ListItem("PRO", "PRO"));
            //shortlisted, non shortlisted and All,
        }



        protected void getReview()
        {

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
                        m_Role = ogroup.Name;
                    }
                }
            }
        }

        protected void getProgrammeIntakeInfo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                sql = "SELECT Programme_ID,Programme_Name,Intake_Number,Application_No_Prefix,Application_Start,Application_Deadline,"
                    + "Application_Deadline_Eng,Application_Deadline_TradChin,Application_Deadline_SimpChin,Vetting_Session_Eng,Vetting_Session_TradChin,"
                    + "Vetting_Session_SimpChin,Result_Announce_Eng,Result_Announce_TradChin,Result_Announce_Simp_Chin,Active,Created_By,Created_Date,Modified_By,Modified_Date "
                    + "FROM TB_PROGRAMME_INTAKE "
                    + "WHERE Programme_ID = @Programme_ID";

                //lbltest.Text = sql;

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Programme_ID", m_prog);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                        lblIntakeNo.Text = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                        m_Programme_ID = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString();
                    }
                    conn.Close();
                }

                m_Programme_Type = checkProgrammeType(m_Programme_ID);
                //lbltest.Text += m_Programme_Type + "<br/>";
                if (m_Programme_Type == "CPIP")
                {
                    GridView_App.Columns[4].Visible = false;
                    GridView_App.Columns[5].Visible = false;
                    GridView_App.Columns[6].Visible = false;
                    GridView_App.Columns[7].Visible = false;
                    ddlStream.Visible = false;
                }
                else
                {
                    GridView_App.Columns[3].Visible = false;
                    ddlStream.Visible = true;
                }
                lblMeetingDate.Text = getVettingDate();
                
            }
        }
        protected string getProjectDec(string appNo)
        {
            string PrjDesc = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";
                if (m_Programme_Type== "CCMF")
                {
                    sql = "Select isNull(Abstract_Eng,'') as description from TB_CCMF_APPLICATION where Application_Number = @Application_Number and status = 'Complete Screening'";
                    //lbltest.Text += "CCMF ";
                }
                else
                {
                    sql = "Select isNull(Abstract,'') as description from TB_INCUBATION_APPLICATION where Application_Number = @Application_Number and status = 'Complete Screening'";
                    //lbltest.Text += "CPIP ";
                }
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@Application_Number", appNo);

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            PrjDesc = reader.GetValue(reader.GetOrdinal("description")).ToString().Replace("\r\n", "<br/>").Replace("\n\r", "<br/>");
                        }
                        reader.Dispose();
                        cmd.Dispose();
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return PrjDesc;
       }
        protected string getVettingComment(string appNo)
        {
            string remarks = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "Select isNull(Remarks_To_Vetting,'') as Remarks_To_Vetting from TB_APPLICATION_SHORTLISTING where Application_Number = @AppNo";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@AppNo", appNo);

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            remarks = reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString();
                        }
                        reader.Dispose();
                        cmd.Dispose();
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return remarks;
        }
        protected String getVettingDate()
        {
            String VMDate = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "Select Date as Meeting_Date from TB_VETTING_MEETING where [Vetting_Meeting_ID] = @MeetingID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@MeetingID", m_meeting);

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            VMDate = reader.GetDateTime(reader.GetOrdinal("Meeting_Date")).ToString("dd MMM, yyyy");
                        }
                        reader.Dispose();
                        cmd.Dispose();
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return VMDate;
        }

        protected void getApplication(DropDownList m_ddlDateFilter)
        {
            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "";

                if (m_Programme_Type == "CCMF")
                {

                    sql = @"SELECT * FROM (
                            SELECT vetting.Vetting_Application_ID, 
                            isNull(vetting.Presentation_From, '') as Presentation_From,
                            isNull(vetting.Presentation_To, '') as Presentation_To,
                            CCMF.Application_Number,
                            CCMF.CCMF_ID,
                            CCMF.Project_Name_Eng,
                            CCMF.Programme_Type,
                            CCMF.Hong_Kong_Programme_Stream,
                            CCMF.CCMF_Application_Type,
                            CCMF.Business_Area,
                            CCMF.Programme_ID,
                            CCMF.Status,
                            isNull(meeting.Date,'') as Date,
                            meeting.Vetting_Meeting_ID as Vetting_Meeting_ID
                            ,((isNull((select Total_Score from TB_SCREENING_CCMF_SCORE where Role='CCMF BDM' and Application_Number = vetting.Application_Number),0)
                            + isNull((select Total_Score from TB_SCREENING_CCMF_SCORE where Role='Senior Manager'and Application_Number = vetting.Application_Number),0)
                            + isNull((select Total_Score from TB_SCREENING_CCMF_SCORE where Role='CPMO'and Application_Number = vetting.Application_Number),0))) /
                            ((case when ((select Total_Score from TB_SCREENING_CCMF_SCORE where Application_Number = vetting.Application_Number and Role='CCMF BDM') = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_CCMF_SCORE where Application_Number = vetting.Application_Number and Role='Senior Manager') = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_CCMF_SCORE where Application_Number = vetting.Application_Number and Role='CPMO') = 0) then 0 else 1 end)) as avgscore 
                            ,Shortlisted
                            FROM TB_CCMF_APPLICATION CCMF
                            LEFT JOIN TB_VETTING_APPLICATION vetting ON CCMF.Application_Number = vetting.Application_Number
                            LEFT JOIN TB_VETTING_MEETING meeting ON meeting.Vetting_Meeting_ID = vetting.Vetting_Meeting_ID
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CCMF.Application_Number
                            WHERE isnull(CONVERT(varchar(50),meeting.Vetting_Meeting_ID),'') = @MeetingID AND CCMF.Status = 'Complete Screening'

                            UNION ALL

                            SELECT 
                            null,
                            null as Presentation_From,
                            null as Presentation_To,
                            CCMF.Application_Number,
                            CCMF.CCMF_ID,
                            CCMF.Project_Name_Eng,
                            CCMF.Programme_Type,
                            CCMF.Hong_Kong_Programme_Stream,
                            CCMF.CCMF_Application_Type,
                            CCMF.Business_Area,
                            CCMF.Programme_ID,
                            CCMF.Status,
                            null as Date,
                            null as Vetting_Meeting_ID,
                            ((isNull((select Total_Score from TB_SCREENING_CCMF_SCORE where Role='CCMF BDM' and Application_Number = CCMF.Application_Number),0)
                            + isNull((select Total_Score from TB_SCREENING_CCMF_SCORE where Role='Senior Manager'and Application_Number = CCMF.Application_Number),0)
                            + isNull((select Total_Score from TB_SCREENING_CCMF_SCORE where Role='CPMO'and Application_Number = CCMF.Application_Number),0))) /
                            ((case when ((select Total_Score from TB_SCREENING_CCMF_SCORE where Application_Number = CCMF.Application_Number and Role='CCMF BDM') = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_CCMF_SCORE where Application_Number = CCMF.Application_Number and Role='Senior Manager') = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_CCMF_SCORE where Application_Number = CCMF.Application_Number and Role='CPMO') = 0) then 0 else 1 end)) as avgscore 
                            ,Shortlisted
                            FROM TB_CCMF_APPLICATION CCMF
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CCMF.Application_Number
                            WHERE CCMF.Application_Number NOT IN (SELECT Application_Number FROM TB_VETTING_APPLICATION)
                            AND CCMF.Status = 'Complete Screening'
                            AND CCMF.Programme_ID = @ProgrammeID
                            AND Shortlisted = 0 
                            ) as CCMF
                            ORDER BY CASE WHEN Presentation_From is null then 1 else 0 end, Presentation_From, Application_Number";                        

                }
                else
                {
                    sql = @"SELECT * FROM (
                            SELECT 
                            vetting.Vetting_Application_ID,
                            vetting.Presentation_From as Presentation_From, 
                            vetting.Presentation_To as Presentation_To,
                            isnull(shortlist.Application_Number,'') as Application_Number, 
                            CPIP.Incubation_ID,
                            CPIP.Company_Name_Eng, CPIP.Business_Area, 
                            shortlist.[Programme_ID], isNull(meeting.Date,'') as Date,
                            meeting.Vetting_Meeting_ID, CPIP.Status,
                            ((isNull((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Application_Number = CPIP.Application_Number and Role='CPIP BDM'),0) 
                            + isNull((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Application_Number = CPIP.Application_Number and Role='Senior Manager'),0) 
                            + isNull((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Application_Number = CPIP.Application_Number and Role='CPMO'),0)) )/
                            ((case when ((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Role='CCMF BDM' and Application_Number = CPIP.Application_Number) = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Role='Senior Manager' and Application_Number = CPIP.Application_Number) = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Role='CPMO' and Application_Number = CPIP.Application_Number) = 0) then 0 else 1 end)) as Average_Score,
                            isnull(Shortlisted,0) as Shortlisted
                            FROM TB_VETTING_MEETING meeting
                            LEFT JOIN TB_VETTING_APPLICATION vetting ON meeting.Vetting_Meeting_ID = vetting.Vetting_Meeting_ID
                            LEFT JOIN TB_INCUBATION_APPLICATION CPIP ON vetting.Application_Number = CPIP.Application_Number
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CPIP.Application_Number
                            WHERE meeting.Vetting_Meeting_ID = @MeetingID AND CPIP.status = 'Complete Screening' 

                            UNION ALL

                            SELECT 
                            null as Vetting_Application_ID,
                            null as Presentation_From, 
                            null as Presentation_To,
                            shortlist.Application_Number, CPIP.Incubation_ID,
                            CPIP.Company_Name_Eng, CPIP.Business_Area, 
                            shortlist.[Programme_ID], null as Date,
                            null as Vetting_Meeting_ID, CPIP.Status,
                            ((isNull((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Application_Number = CPIP.Application_Number and Role='CPIP BDM'),0) 
                            + isNull((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Application_Number = CPIP.Application_Number and Role='Senior Manager'),0) 
                            + isNull((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Application_Number = CPIP.Application_Number and Role='CPMO'),0)) )/
                            ((case when ((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Role='CCMF BDM' and Application_Number = CPIP.Application_Number) = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Role='Senior Manager' and Application_Number = CPIP.Application_Number) = 0) then 0 else 1 end
                            + case when ((select Total_Score from TB_SCREENING_INCUBATION_SCORE where Role='CPMO' and Application_Number = CPIP.Application_Number) = 0) then 0 else 1 end)) as Average_Score,
                            Shortlisted
                            FROM TB_INCUBATION_APPLICATION CPIP
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CPIP.Application_Number
                            WHERE CPIP.Application_Number NOT IN (SELECT Application_Number FROM TB_VETTING_APPLICATION)
                            AND CPIP.Status = 'Complete Screening'
                            AND CPIP.Programme_ID = @ProgrammeID
                            AND Shortlisted = 0 
                            ) as CPIP
                            ORDER BY CASE WHEN Presentation_From is null then 1 else 0 end, Presentation_From, Application_Number";

                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@MeetingID", m_meeting);
                    cmd.Parameters.AddWithValue("@ProgrammeID", m_Programme_ID);

                    m_ddlDateFilter.SelectedItem.Text = getVettingDate();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string timeslot = string.Empty;
                        int iPresentFrom = reader.GetOrdinal("Presentation_From");
                        string presentFrom = string.Empty;
                        DateTime? dtPresentFrom = null;

                        int iPresentTo = reader.GetOrdinal("Presentation_To");
                        string presentTo = string.Empty;
                        DateTime? dtPresentTo = null;

                        int iVettingDate = reader.GetOrdinal("Date");
                        string vettingDate = string.Empty;
                        int iVettingID = reader.GetOrdinal("Vetting_Meeting_ID");
                        string vettingID = string.Empty;
                        int iVettingAppID = reader.GetOrdinal("Vetting_Application_ID");
                        string vettingAppID = string.Empty;


                        //if (m_ddlDateFilter.SelectedItem.Text.Trim() == reader.GetDateTime(reader.GetOrdinal("Date")).ToString("dd MMM, yyyy") || (m_ddlDateFilter.SelectedItem.Text.Trim() == "All Date"))
                        //{
                            if (ddlShortlisted.SelectedItem.Text.Trim() == "All" ||
                               (ddlShortlisted.SelectedItem.Text.Trim() == "Shortlisted" && (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted")) == true) ||
                               (ddlShortlisted.SelectedItem.Text.Trim() == "Non Shortlisted" && (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted")) == false)
                                )
                            {
                                

                                if (!reader.IsDBNull(iPresentFrom))
                                {
                                    dtPresentFrom = reader.GetDateTime(iPresentFrom);
                                    presentFrom = dtPresentFrom.ToString();
                                }
                                if (!reader.IsDBNull(iPresentTo))
                                {
                                    dtPresentTo = reader.GetDateTime(iPresentTo);
                                    presentTo = dtPresentTo.ToString();
                                }
                                if (!reader.IsDBNull(iVettingDate))
                                {
                                    vettingDate = reader.GetDateTime(iVettingDate).ToString("dd MMM, yyyy");
                                }
                                if (!reader.IsDBNull(iVettingID))
                                {
                                    vettingID = reader.GetValue(iVettingID).ToString();
                                }
                                if (!reader.IsDBNull(iVettingAppID))
                                {
                                    vettingAppID = reader.GetValue(iVettingAppID).ToString();
                                }
                                if (!reader.IsDBNull(iPresentFrom) && !reader.IsDBNull(iPresentTo))
                                {
                                    timeslot = reader.GetDateTime(iPresentFrom).ToString("hh:mm tt") + " - " + reader.GetDateTime(iPresentTo).ToString("hh:mm tt");
                                }

                                if (m_Programme_Type == "CCMF")
                                {


                                    if (ddlStream.SelectedItem.Text.Trim() == "All" ||
                                    (ddlStream.SelectedItem.Text.Trim() == reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")).ToString().Replace("Professional", "PRO").Replace("Young Entrepreneur", "YEP")))
                                    {

                                        lstTimeSlotList.Add(new TimeSlotList
                                        {
                                            TimeSlot = timeslot,
                                            Presentation_From = presentFrom,
                                            DT_Presentation_From = dtPresentFrom,
                                            Presentation_To = presentTo,
                                            DT_Presentation_To = dtPresentTo,
                                            Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                            Application_ID = reader.GetValue(reader.GetOrdinal("CCMF_ID")).ToString(),
                                            Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                            Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                                            CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                                            Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                            Vetting_Application_ID = vettingAppID,
                                            Vetting_Meeting_ID = vettingID,
                                            Vetting_Meeting_Date = vettingDate,
                                            Hong_Kong_Programme_Stream = reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")).ToString().Replace("Professional", "PRO").Replace("Young Entrepreneur", "YEP"),
                                            Projectdescription = getProjectDec(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString()),
                                            AverageScore = float.Parse(reader.GetValue(reader.GetOrdinal("avgscore")).ToString()).ToString("N3"),
                                            Remarksforvettingteam = getVettingComment(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString()),
                                            ShortlistedChecked = (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted"))
                                        });
                                    }
                                }
                                else
                                {                                   
                                    
                                    lstTimeSlotList.Add(new TimeSlotList
                                    {
                                        TimeSlot = timeslot,
                                        Presentation_From = presentFrom,
                                        DT_Presentation_From = dtPresentFrom,
                                        Presentation_To = presentTo,
                                        DT_Presentation_To = dtPresentTo,
                                        Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                        Application_ID = reader.GetValue(reader.GetOrdinal("Incubation_ID")).ToString(),
                                        Company = reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),
                                        Cluster = reader.GetValue(reader.GetOrdinal("Business_Area")).ToString(),
                                        Vetting_Application_ID = vettingAppID,
                                        Vetting_Meeting_ID = vettingID,
                                        Vetting_Meeting_Date = vettingDate,
                                        Projectdescription = getProjectDec(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString()),
                                        AverageScore = float.Parse(reader.GetValue(reader.GetOrdinal("Average_Score")).ToString()).ToString("N3"),
                                        Remarksforvettingteam = getVettingComment(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString()),
                                        ShortlistedChecked = (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted"))
                                    });
                                }
                            }



                       // }
                    }
                    conn.Close();
                }
            }


            GridView_App.DataSource = lstTimeSlotList;
            GridView_App.DataBind();

        }


        protected string checkProgrammeType(string m_Programme_ID)
        {
            var m_INCUBATION = 0;
            var m_CCMF = 0;
            var m_result = "";
            var sqlStr1 = "SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Programme_ID = @Programme_ID";
            var sqlStr2 = "SELECT COUNT('Incubation_ID') FROM [TB_CCMF_APPLICATION] WHERE Programme_ID = @Programme_ID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStr1, conn))
                {
                    cmd.Parameters.Add("@Programme_ID", m_Programme_ID);

                    conn.Open();
                    m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
                }

                using (SqlCommand cmd = new SqlCommand(sqlStr2, conn))
                {
                    cmd.Parameters.Add("@Programme_ID", m_Programme_ID);

                    conn.Open();
                    m_CCMF = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
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
            if (e.CommandName == "Application_Number")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView_App.Rows[index];
                string urlstring = row.Cells[0].Text;

                HiddenField HiddenApplication_ID = (HiddenField)row.FindControl("HiddenApplication_ID");
                urlstring = m_prog;

                if (m_Programme_Type == "CCMF" && lblProName.Text.Equals("Cyberport Creative Micro Fund - GBAYEP"))
                {
                    Context.Response.Redirect("/SitePages/CCMFGBAYEP.aspx?prog=" + urlstring + "&app=" + HiddenApplication_ID.Value.ToString());
                }
                else if (m_Programme_Type == "CCMF")
                {
                    Context.Response.Redirect("/SitePages/CCMF.aspx?prog=" + urlstring + "&app=" + HiddenApplication_ID.Value.ToString());
                }
                else
                {
                    Context.Response.Redirect("/SitePages/IncubationProgram.aspx?prog=" + urlstring + "&app=" + HiddenApplication_ID.Value.ToString());
                }
                
            }
        }

        protected void GridView_App_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void ddlDateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;
            string value = (string)list.SelectedValue;
            string Txt = (string)list.SelectedItem.Text;

            getApplication(ddlDateFilter);
            //SortVettingApplcationList();
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");
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

            string sql = @"INSERT INTO [TB_DOWNLOAD_REQUEST] (Programme_Name, Programme_ID, Intake_Number, Request_Type, Status, User_Email, Vetting_Meeting_ID, ShortListFilter, StreamFilter, Created_By, Created_Date) 
                           VALUES (@Programme_Name, @Programme_ID, @Intake_Number, 'Download Attachments - Application for Programme Intake', 0, @User_Email, @vmID, @shortlist, @stream, @User, GETDATE())";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Programme_Name", m_programName);
                    cmd.Parameters.AddWithValue("@Intake_Number", m_intake);
                    cmd.Parameters.AddWithValue("@Programme_ID", m_programId);
                    cmd.Parameters.AddWithValue("@User_Email", m_mail);
                    cmd.Parameters.AddWithValue("@vmID", m_meeting);
                    cmd.Parameters.AddWithValue("@shortlist", ddlShortlisted.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@stream", ddlStream.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@User", m_username);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            string m_subject = "Zip File for shortlisted applications : " + m_programName + " - " + m_intake + " is processing.";
            string m_body = getEmailTemplate("ZipDownloadStartEmail");

            sharepointsendemail(m_mail, m_subject, m_body);
            lbldownloadmessage.Text = "Download Complete.";

        }
        
        public void processFolder(String Destination, String FileName)
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



            if (Page.IsValid)
            {
                var ApplicationIDs = new List<String>();
                //var ApplicationID = "";
                foreach (GridViewRow row in GridView_App.Rows)
                {
                    var AppID = row.FindControl("HiddenApplication_ID") as HiddenField;
                    //ApplicationID += "'" + AppID.Value + "',";
                    ApplicationIDs.Add(AppID.Value);
                }
                //ApplicationID = ApplicationID.Substring(0, ApplicationID.Length - 1);
                var filePaths = GetPresentationSlideAttachment(ApplicationIDs);

                if (filePaths.Count > 0)
                {
                    m_downloadLink = m_zipfiledownloadurl;
                    string m_subject = "";
                    string m_body = "";
                    string m_Programme_Name = m_programName;
                    string m_Intake_Number = lblIntakeNo.Text;
                    string m_Password;
                    string m_downloadlink = m_downloadLink;
                    string m_Starttime;
                    string m_Endtime;
                    //string m_zipstatus = "done";



                    //starting email
                    m_subject = "Zip File for shortlisted applications : " + m_Programme_Name + " - " + m_Intake_Number + " is processing.";
                    //m_body = "Hi, Zip File is processing, please wait next email to confirm Zip File is ready.";
                    m_body = getEmailTemplate("ZipDownloadStartEmail");
                    sharepointsendemail(m_mail, m_subject, m_body);


                    /*************************/
                    //zip programm in here:
                    m_Starttime = DateTime.Now.ToString();
                    //SPSite site = SPContext.Current.Site;
                    //SPWeb web = site.OpenWeb();
                    SPUtility.ValidateFormDigest();
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                            {
                                ErrorLog("Start", "Download started");

                                ZipOutputStream zipStream = new ZipOutputStream(File.Create(Destination));

                                zipStream.SetLevel(9); //0-9, 9 being the highest level of compression

                                zipStream.Password = genRandom(8);	// optional. Null is the same as not setting. Required if using AES.
                                m_Password = zipStream.Password;
                                //lbldatetime.Text = lbldatetime.Text + "   "+zipStream.Password;
                                CompressFolder(web, zipStream, filePaths);
                                zipStream.Finish();
                                //zipStream.IsStreamOwner = false;	
                                zipStream.Close();

                                m_Endtime = DateTime.Now.ToString();
                                /*************************/

                                //insert into TB_Download_ZIP
                                InsertTBDownloadZIP(m_username, m_mail, "ZIP", Destination, FileName, m_Password, "1");

                                //Completed email  

                                m_subject = "Zip File for shortlisted applications : " + m_Programme_Name + " - " + m_Intake_Number + " is completed.";
                                m_body = getEmailTemplate("ZipDownloadEndEmailApp");
                                m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_FileName", FileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                                sharepointsendemail(m_mail, m_subject, m_body);
                                //sharepointsendemail("andysgi@gmail.com", "hi", "ko");
                                lbldownloadmessage.Text = "Download Complete.";

                                ErrorLog("End", "Download completed");
                            }
                        }
                    });

                }
                else
                {
                    lbldownloadmessage.Text = "No found files";
                }

            }
        }
        
        private void CompressFolder(SPWeb web, ZipOutputStream zipStream, List<String> filePaths)
        {
            ErrorLog("Paths", filePaths.Count.ToString());
            foreach (var file in filePaths)
            {
                String entryName = "";
                try
                {
                    var DeletepathName = "";
                    if (!string.IsNullOrEmpty(m_AttachmentPrimaryFolderName))
                    {
                        DeletepathName += m_AttachmentPrimaryFolderName + @"\";
                    }
                    if (!string.IsNullOrEmpty(m_AttachmentSecondaryFolderName))
                    {
                        DeletepathName += m_AttachmentSecondaryFolderName + @"\";
                    }

                    entryName = file.Substring(DeletepathName.Length);
                    ZipEntry entry = new ZipEntry(entryName);
                    entry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(entry);
                    var size = file.Length / 1024;
                    //SPUser ensure = web.EnsureUser(ImpersonateUser);
                    //SPSite impSite = new SPSite(web.Site.Url, ensure.UserToken);
                    //SPWeb impWeb = impSite.OpenWeb();

                    var filObject = web.GetFile(file);
                    if (filObject.Exists)
                    {

                        ErrorLog("File name", entryName + " (" + size.ToString("N2") + " KB)");

                        byte[] binary = filObject.OpenBinary();
                        zipStream.Write(binary, 0, binary.Length);

                        m_DownloadFileSize = m_DownloadFileSize + size;
                        ErrorLog("Total file size", m_DownloadFileSize.ToString("N2") + " KB");
                    }
                }
                catch(Exception ex)
                {

                    ErrorLog("Error", ex.Message);
                    var str = entryName + " ";
                    ErrorLog("Error file", str);
                }
                
            }

        }

        protected List<String> getApplicationAttachVideo_ClipAndPresentation_SlidePath()
        {
            ConnectOpen();
            List<String> list = new List<String>();
            try
            {
                var appNums = "";
                foreach (GridViewRow row in GridView_App.Rows)
                {
                    Label applicationNoLabel = row.FindControl("lblApplication_Number") as Label;
                    if (appNums == "")
                    {
                        appNums += "'" + applicationNoLabel.Text + "'";
                    }
                    else
                    {
                        appNums += ",'" + applicationNoLabel.Text + "'";
                    }

                }

                String sqlColumn = "select taattachment.Attachment_Path ";
                String sqlFrom = "";
                String sqlWhere = " where tbApplicatiion.Application_Number in (" + appNums + ")  "
                                + " and tbApplicatiion.Status <> 'Saved' "
                                + " and (taattachment.Attachment_Type <> 'HK_ID'"
                                + " and taattachment.Attachment_Type <> 'BR_COPY'"
                                + " and taattachment.Attachment_Type <> 'Video_Clip'"
                                + " and taattachment.Attachment_Type <> 'Student_ID' "
                                + " and taattachment.Attachment_Type <> 'Company_Annual_Return'"
                                + " and  taattachment.Attachment_Type <> 'Presentation_Slide_Response')";

                if (lblProName.Text.Contains("Cyberport Incubation Program"))
                {
                    //CPIP
                    sqlFrom += "from TB_INCUBATION_APPLICATION tbApplicatiion inner join TB_APPLICATION_ATTACHMENT taattachment on taattachment.Application_ID = tbApplicatiion.Incubation_ID ";
                }
                else
                {
                    //CCMF
                    sqlFrom += "from TB_CCMF_APPLICATION tbApplicatiion inner join TB_APPLICATION_ATTACHMENT taattachment on taattachment.Application_ID = tbApplicatiion.CCMF_ID ";

                }

                var sqlString = sqlColumn + sqlFrom + sqlWhere;
                var command = new SqlCommand(sqlString, connection);

                var reader = command.ExecuteReader();

                
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {

                ConnectClose();
            }

            return list;
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
            try { 
                    var command = new SqlCommand(sqlUpdate, connection);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                finally 
                { 
                    ConnectClose();
                }
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

            try
            {
                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template='" + emailTemplate + "';";

                var command = new SqlCommand(sqlString, connection);
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

                    if (reader.GetString(0) == "ImpersonateUser")
                    {
                        ImpersonateUser = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "IsLogEvent")
                    {
                        IsLogEvent = reader.GetString(1);
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

        private List<String> GetPresentationSlideAttachment(List<String> ApplicationIDS)
        {
            List<String> AttachmentPaths = new List<String>();
            ConnectOpen();
            try
            {
                //var sqlString = String.Format("select Attachment_Type,Attachment_Path from TB_APPLICATION_ATTACHMENT where Application_ID in ({0}) and Attachment_Type <> 'Video_Clip' and Attachment_Type <> 'Presentation_Slide_Response' and Attachment_Type <> 'HK_ID' and Attachment_Type <> 'BR_COPY' and Attachment_Type <> 'Student_ID' and Attachment_Type <> 'Company_Annual_Return';", String.Join(",", ApplicationIDS.Select((x, i) => "@ApplicationID" + i).ToArray()));
                var sqlString = String.Format("select Attachment_Type,Attachment_Path from TB_APPLICATION_ATTACHMENT where Application_ID in ({0}) and Attachment_Type <> 'HK_ID' and Attachment_Type <> 'BR_COPY' and Attachment_Type <> 'Video_Clip' and Attachment_Type <> 'Student_ID' and Attachment_Type <> 'Company_Annual_Return' and Attachment_Type <> 'Presentation_Slide_Response';", String.Join(",", ApplicationIDS.Select((x, i) => "@ApplicationID" + i).ToArray()));
                var command = new SqlCommand(sqlString, connection);
                for (int i = 0; i < ApplicationIDS.Count; i++)
                    command.Parameters.Add("@ApplicationID" + i, ApplicationIDS[i]);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AttachmentPaths.Add(reader.GetString(1));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            return AttachmentPaths;

        }


    }
}
