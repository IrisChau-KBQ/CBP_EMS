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

namespace CBP_EMS_SP.DecInterestWP.DecInterestWP
{
    [ToolboxItemAttribute(false)]
    public partial class DecInterestWP : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        string m_VMID = "";
        string m_systemuser = "";
        string m_Backrurl = "";
        string m_VTemail = "";

        string m_Role = "";
        string m_checkRole = "";
        string m_Programme_ID = "";
        string m_Programme_Name = "";
        string m_Intake_Number = "";
        string m_Time_Interval = "";
        DateTime m_Presentation_From = DateTime.Now;
        DateTime m_Presentation_To = DateTime.Now;
        string m_Programme_Type = "";
        string m_Meeting_status = "";
        DateTime m_VettingDate = DateTime.Now;
        string m_VettingVenue = "";
        DateTime m_VettingMettingFrom = DateTime.Now;
        DateTime m_VettingMeetingTo = DateTime.Now;
        string m_Vetting_Delclaration_ID = "";
        string m_Application_Number = "";


        public String m_path = "";
        public String m_programName;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "";
        public String ImpersonateUser = "";
        public String m_WebsiteUrl_ccmf_form = "";
        public String m_WebsiteUrl_cpip_form = "";

        private SqlConnection connection;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public class TimeSlotList
        {
            public string Application_Number { set; get; }
            public string Company { set; get; }
            public string Project_Name_Eng { set; get; }
            public string Programme_Type { set; get; }
            public string CCMF_Application_Type { set; get; }
            public string Applicant { set; get; }
            public string CoreMember { set; get; }
            public string APPNoURL { set; get; }
            
        }

        public DecInterestWP()
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
            m_VTemail = HttpContext.Current.Request.QueryString["VTemail"];
            m_Backrurl = HttpContext.Current.Request.QueryString["Backurl"];
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();

            if (String.IsNullOrEmpty(m_VTemail))
            { }
            else
            {
                m_systemuser = m_VTemail;   
            }

            if (String.IsNullOrEmpty(m_Backrurl))
            {
                m_Backrurl = "Application%20List%20for%20Vetting%20Team.aspx";
            }
            else
            {
                m_Backrurl = m_Backrurl + "?VMID=" + m_VMID;
            }


            //lbltest.Text += "m_VMID:" + m_VMID + "<br/>";
            //lbltest.Text += "m_VTemail:" + m_VTemail + "<br/>";
            //lbltest.Text += "m_Backrurl" + m_Backrurl + "<br/>";
            //lbltest.Text += "m_systemuser" + m_systemuser + "<br/>";


            if (String.IsNullOrEmpty(m_VMID))
            {
                return;
            }

            getReview();
            getVettingMeetingInfo();
            m_Vetting_Delclaration_ID = getVetting_Delclaration_ID(m_VMID, m_systemuser, m_VettingVenue, lblname.Text.ToString().Trim());

            if (!Page.IsPostBack)
            {
                getdbdata();
                
            }
            else
            {
                //getVettingApplication();
            }

            AccessControl();

            hiddenControl();

            checkConflict();
            
            //lbltest.Text += "checkrole function : " + checkrole("CCMF Coordinator User").ToString(); ;
            
        }

        protected void AccessControl()
        {
            // Check Role can display this web part
            //Applicant  //Collaborator  //CCMF Coordinator //CCMF BDM  //CPIP Coordinator  //CPIP BDM  //Senior Manager  //CPMO

            //checkvettingteam();

            if (m_Role == "Applicant")
            {
                mainPanel.Visible = false;
            }
            else if (m_Role == "Collaborator")
            {
                mainPanel.Visible = false;
            }
            else if (m_Role == "CCMF Coordinator")
            {
                mainPanel.Visible = true;
            }
            else if (m_Role == "CPIP Coordinator")
            {
                mainPanel.Visible = true;
            }
            else if (m_Role == "CCMF BDM")
            {
                mainPanel.Visible = true;
            }
            else if (m_Role == "CPIP BDM")
            {
                mainPanel.Visible = true;
            }
            else if (m_Role == "Senior Manager")
            {
                mainPanel.Visible = true;
            }
            else if (m_Role == "CPMO")
            {
                mainPanel.Visible = true;
            }


        }

        protected void hiddenControl() 
        {

            if (String.IsNullOrEmpty(m_VTemail)) 
            {   
                   
                rbtDec1.Enabled = true;
                rbtDec2.Enabled = true;
                btnSubmit.Visible = true;
            }
            else
            {
                rbtDec1.Enabled = false;
                rbtDec2.Enabled = false;
                btnSubmit.Visible = false;
            }

            if (CheckMeetingIsCompleted())
            {
                btnSubmit.Enabled = false;
            }else
            {
                btnSubmit.Enabled = true;
            }

        }

        private void checkConflict()
        {
            EnableConflictCheckbox(rbtDec1.Checked);            
        }

        protected void getdbdata()
        {
            getVettingApplication();
            if (m_Vetting_Delclaration_ID !="") 
            { 
                getVetting_Delclaration();
            }
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


        protected string checkrole(string m_username)
        {   
            m_checkRole = "";
            SPSite oSiteCollection = SPContext.Current.Site;
            using (SPWeb oWebsite = oSiteCollection.OpenWeb())
            {
                SPUser userName = oWebsite.EnsureUser(m_username); //Getting the Current User Login Name
                SPGroupCollection collGroups = userName.Groups;
                if (collGroups.Count > 0)
                {
                    foreach (SPGroup ogroup in collGroups)   //Looping the group collection and adding to the list 
                    {
                        //lblrole.Text = ogroup.Name;
                        m_checkRole = ogroup.Name;
                    }
                }
            }

            return m_checkRole;
        }

        protected string checkVettingMemberFullName(string m_username)
        {
            string m_result = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                sql = "SELECT Salutation, First_Name, Full_Name "
                    + "FROM TB_VETTING_MEMBER_INFO "
                    + "where Email = @Email";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@Email", m_username);
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            m_result = reader.GetValue(reader.GetOrdinal("Salutation")).ToString().Trim();
                            m_result += " " + reader.GetValue(reader.GetOrdinal("Full_Name")).ToString().Trim();
                            m_result += ", " + reader.GetValue(reader.GetOrdinal("First_Name")).ToString().Trim();
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }

            return m_result;
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
                    + "WHERE (dbo.TB_VETTING_MEETING.Vetting_Meeting_ID = @VMID )";

                //lbltest.Text = sql;

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@VMID", m_VMID));
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lblDate.Text = reader.GetDateTime(reader.GetOrdinal("Date")).ToString("dd MMMM, yyyy");
                            lblTime.Text = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_From")).ToString("h:mmtt") + " - " + reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_To")).ToString("h:mmtt") + " (Vetting Meeting), "
                                         + reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("h:mmtt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("h:mmtt") + " (Presentation Session)";
                            m_Time_Interval = reader.GetValue(reader.GetOrdinal("Time_Interval")).ToString();
                            lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                            m_Programme_Name = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                            lblIntakeNo.Text = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                            m_Intake_Number = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                            m_Programme_ID = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString();
                            //lblm_Programme_ID.Text = m_Programme_ID;
                            m_VettingDate = reader.GetDateTime(reader.GetOrdinal("Date"));
                            m_VettingVenue = reader.GetValue(reader.GetOrdinal("Venue")).ToString();
                            lblvenus.Text = m_VettingVenue;
                            m_Presentation_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From"));
                            m_Presentation_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To"));
                            m_VettingMettingFrom = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_From"));
                            m_VettingMeetingTo = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_To"));
                            //lblPresentation_From.Text = m_Presentation_From.ToString();
                            //lblPresentation_To.Text = m_Presentation_To.ToString();
                            m_Meeting_status = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString();

                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                lblname.Text = checkVettingMemberFullName(m_systemuser);

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

        protected void getVettingApplication()
        {
            //lbltest.Text += "getVettingApplication<br/>"+ DateTime.Now.ToString();
            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "";
                string m_Attendance = "";

                if (m_Programme_Type == "CCMF")
                {
                    sql = @"SELECT Application_Number, ccmf.Project_Name_Eng, ccmf.Programme_Type, ccmf.CCMF_Application_Type, ccmf.Applicant, ccmf.CCMF_ID, intake.Programme_ID,
                            LTRIM(isnull(taccm.Name,'')) as CoreMember FROM TB_PROGRAMME_INTAKE intake
                            LEFT JOIN TB_CCMF_APPLICATION ccmf ON intake.Programme_ID = ccmf.Programme_ID
                            LEFT JOIN 
                            ( 
	                            SELECT 
		                              Application_ID, 
		                              STUFF( 
			                              (SELECT ', ' + Name 
			                               FROM TB_APPLICATION_COMPANY_CORE_MEMBER 
			                               WHERE Application_ID = a.Application_ID 
			                               FOR XML PATH ('')) 
			                               , 1, 1, '')  AS Name 
	                             FROM TB_APPLICATION_COMPANY_CORE_MEMBER AS a 
	                             GROUP BY Application_ID 
	                             ) as taccm on taccm.Application_ID = ccmf.CCMF_ID 
	                            WHERE intake.Programme_ID = @ProgrammeID
                            AND ccmf.Status <> 'Saved' and ccmf.Status = 'Complete Screening'
                            ORDER BY Application_Number";
                }
                else
                {
                    sql = @"SELECT Application_Number, cpip.Company_Name_Eng, cpip.Applicant, Incubation_ID, intake.Programme_ID,
                            LTRIM(isnull(taccm.Name,'')) as CoreMember FROM TB_PROGRAMME_INTAKE intake
                            LEFT JOIN TB_INCUBATION_APPLICATION cpip ON intake.Programme_ID = cpip.Programme_ID
                            LEFT JOIN 
                            ( 
	                            SELECT 
		                              Application_ID, 
		                              STUFF( 
			                              (SELECT ', ' + Name 
			                               FROM TB_APPLICATION_COMPANY_CORE_MEMBER 
			                               WHERE Application_ID = a.Application_ID 
			                               FOR XML PATH ('')) 
			                               , 1, 1, '')  AS Name 
	                             FROM TB_APPLICATION_COMPANY_CORE_MEMBER AS a 
	                             GROUP BY Application_ID 
	                             ) as taccm on taccm.Application_ID = cpip.Incubation_ID 
	                            WHERE intake.Programme_ID = @ProgrammeID
                            AND cpip.Status <> 'Saved' and cpip.Status = 'Complete Screening'
                            ORDER BY Application_Number";
                }


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.Add(new SqlParameter("@ProgrammeID", m_Programme_ID));

                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        getSYSTEMPARAMETER();
                        while (reader.Read())
                        {
                            if (m_Programme_Type == "CCMF")
                            {
                                lstTimeSlotList.Add(new TimeSlotList
                                {
                                    
                                    Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                    Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                    Project_Name_Eng = reader.GetValue(reader.GetOrdinal("Project_Name_Eng")).ToString(),
                                    CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),

                                    Applicant = reader.GetValue(reader.GetOrdinal("Applicant")).ToString(),
                                    CoreMember = reader.GetValue(reader.GetOrdinal("CoreMember")).ToString(),
                                    APPNoURL = "/" + m_WebsiteUrl_ccmf_form + "?app=" + reader.GetValue(reader.GetOrdinal("CCMF_ID")).ToString() + "&prog=" + reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString()
                                    
                                });

                            }
                            else
                            {
                                lstTimeSlotList.Add(new TimeSlotList
                                {
                                    Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                    Company = reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),
                                    Applicant = reader.GetValue(reader.GetOrdinal("Applicant")).ToString(),
                                    CoreMember = reader.GetValue(reader.GetOrdinal("CoreMember")).ToString(),
                                    APPNoURL = "/" + m_WebsiteUrl_cpip_form + "?app=" + reader.GetValue(reader.GetOrdinal("Incubation_ID")).ToString() + "&prog=" + reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString()
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
            //
            //lstTimeSlotList.Sort((x, y) => DateTime.Compare(x.DT_Presentation_From, y.DT_Presentation_From));

            GridView_App.DataSource = lstTimeSlotList;
            GridView_App.DataBind();

        }

        protected string checkProgrammeType(string m_Programme_ID)
        {
            var m_INCUBATION = 0;
            var m_CCMF = 0;
            var m_result = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Programme_ID = '" + m_Programme_ID + "'", conn))
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Programme_ID = @Programme_ID ", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Programme_ID", m_Programme_ID));
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

                //using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE Programme_ID = '" + m_Programme_ID + "'", conn))
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE Programme_ID = @Programme_ID ", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Programme_ID", m_Programme_ID));
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

        protected void GridView_App_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GridView_App_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox m_chkcoi = (CheckBox)e.Row.FindControl("chkcoi");
                TextBox m_TxtReson = (TextBox)e.Row.FindControl("TxtReson");
                Label m_lblReson = (Label)e.Row.FindControl("lblReson");
                HiddenField HiddenApplication_Number = (HiddenField)e.Row.FindControl("HiddenApplication_Number");



                if (!string.IsNullOrEmpty(m_Vetting_Delclaration_ID))
                { 
             
                    using (SqlConnection conn = new SqlConnection(connStr))
                        {
                            string sql = "";

                            sql = "SELECT Vetting_Delclaration_ID,Application_Number,Conflict_Of_Interest,Reason "
                                + "FROM TB_DECLARATION_APPLICATION "
                                + "Where Vetting_Delclaration_ID=@m_Vetting_Delclaration_ID and Application_Number=@m_Application_Number ";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.Add("@m_Vetting_Delclaration_ID", m_Vetting_Delclaration_ID);
                                cmd.Parameters.Add("@m_Application_Number", HiddenApplication_Number.Value.ToString().Trim());
                                conn.Open();
                                try
                                {
                                    var reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        if (Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Conflict_Of_Interest"))) > 0) { m_chkcoi.Checked = true; }
                                        m_TxtReson.Text = reader.GetValue(reader.GetOrdinal("Reason")).ToString().Trim();
                                    }
                                }
                                finally
                                {
                                    conn.Close();
                                }
                            }

                        }
                }

                if (!String.IsNullOrEmpty(m_VTemail))
                {
                    m_chkcoi.Enabled = false;
                    m_TxtReson.Enabled = false;
                }

            }

        }

        protected void GridView_App_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect(m_Backrurl);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool m_checkGV = false;
            
            lblmessageerror1.Text = "";
            lblmessageerror2.Text = "";

            if (CheckMeetingIsCompleted())
            {
                pnlWarning.Visible = true;
                return;
            }
            
            //lbltest.Text += "chkDec2.Checked" + chkDec2.Checked.ToString() + "<br/>";
            foreach (GridViewRow row in GridView_App.Rows)
            {   
                CheckBox m_chkcoi = (CheckBox)row.FindControl("chkcoi");
                TextBox m_TxtReson = (TextBox)row.FindControl("TxtReson");



                if (rbtDec1.Checked == true) 
                {
                    if (m_chkcoi.Checked == true) 
                    {
                        lblmessageerror2.Text = "If you select Conflict of Interest, please select other one option.";
                        break;
                    }
                    if (!string.IsNullOrEmpty(m_TxtReson.Text))
                    {
                        lblmessageerror2.Text = "If you fill the reason, please select other one option.";
                        break;
                    }
                } 
                else 
                {
                    //m_checkGV = (m_checkGV && m_chkcoi.Checked);
                    if (m_chkcoi.Checked == true) { m_checkGV = true; }

                    if ((m_chkcoi.Checked == true) && (string.IsNullOrEmpty(m_TxtReson.Text)))
                    {
                        lblmessageerror2.Text = "Please fill the Reason";
                        break;
                    }
                    else if ((m_chkcoi.Checked == false) && (!string.IsNullOrEmpty(m_TxtReson.Text))) 
                    {
                        lblmessageerror2.Text = "Please select Conflict of Interest";
                        break;
                    }

                }

            }

            if (m_checkGV == false && rbtDec1.Checked == false) 
            {
                lblmessageerror2.Text = "Please select Conflict of Interest";
            }
            
            //lbltest.Text += "m_checkGV" + m_checkGV.ToString() + "<br/>";
            //lbltest.Text += "m_checkTxt" + m_checkTxt.ToString() + "<br/>";

            if (string.IsNullOrEmpty(lblmessageerror2.Text))
            {
                if (string.IsNullOrEmpty(m_Vetting_Delclaration_ID))
                {
                    insertVettDeclaration(m_VMID, m_systemuser, m_VettingVenue, lblname.Text.ToString().Trim());
                    m_Vetting_Delclaration_ID = getVetting_Delclaration_ID(m_VMID, m_systemuser, m_VettingVenue, lblname.Text.ToString().Trim());
                    insertDeclartationApp();
                }
                else
                {
                    updateVettDeclaration(m_Vetting_Delclaration_ID, m_VMID, m_systemuser, m_VettingVenue, lblname.Text.ToString().Trim());
                    insertDeclartationApp();
                }

                Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");
            }           
            
        }

        protected string getVetting_Delclaration_ID(string m_Vetting_Meeting_ID, string m_systemuser, string m_VettingVenue, string m_userName) 
        {
            string m_Vetting_Delclaration_ID = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                sql = "SELECT Vetting_Delclaration_ID,Vetting_Meeting_ID,Member_Email,DateTime,Venue,Name,No_Conflict_Application,Abstained_Voting_Application "
                    + "FROM TB_VETTING_DECLARATION "
                    + "Where Vetting_Meeting_ID=@m_Vetting_Meeting_ID and Member_Email=@m_systemuser and Venue=@m_VettingVenue and Name=@m_userName ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID);
                    cmd.Parameters.Add("@m_systemuser", m_systemuser);
                    cmd.Parameters.Add("@m_VettingVenue", m_VettingVenue);
                    cmd.Parameters.Add("@m_userName", m_userName);
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            m_Vetting_Delclaration_ID = reader.GetValue(reader.GetOrdinal("Vetting_Delclaration_ID")).ToString().Trim();
                            //chkDec1.Checked = reader.GetBoolean(reader.GetOrdinal("No_Conflict_Application"));
                            //chkDec2.Checked = reader.GetBoolean(reader.GetOrdinal("Abstained_Voting_Application"));
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }

            return m_Vetting_Delclaration_ID;
        }

        protected void getVetting_Delclaration()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                sql = "SELECT Vetting_Delclaration_ID,Vetting_Meeting_ID,Member_Email,DateTime,Venue,Name,No_Conflict_Application,Abstained_Voting_Application "
                    + "FROM TB_VETTING_DECLARATION "
                    + "Where Vetting_Delclaration_ID = @m_Vetting_Delclaration_ID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@m_Vetting_Delclaration_ID", m_Vetting_Delclaration_ID);
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            //chkDec1.Checked = reader.GetBoolean(reader.GetOrdinal("No_Conflict_Application"));
                            //chkDec2.Checked = reader.GetBoolean(reader.GetOrdinal("Abstained_Voting_Application"));
                            if (reader.GetBoolean(reader.GetOrdinal("No_Conflict_Application")))
                            {
                                rbtDec1.Checked = true;
                                rbtDec2.Checked = false;
                            }
                            else 
                            {
                                rbtDec1.Checked = false;
                                rbtDec2.Checked = true;
                            }
                            
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }
        }

        protected void insertDeclartationApp() 
        {
            SqlConnection conn = new SqlConnection(connStr);

            string sql = "";
            //var m_Presentation_To = m_Presentation_From.AddMinutes(int.Parse(m_Time_Interval));
            sql = "DELETE FROM TB_DECLARATION_APPLICATION "
                        + "Where Vetting_Delclaration_ID = @m_Vetting_Delclaration_ID ";

            

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@m_Vetting_Delclaration_ID", m_Vetting_Delclaration_ID);
                cmd.ExecuteNonQuery();
                //conn.Close();


                foreach (GridViewRow row in GridView_App.Rows)
                {
                    HiddenField HiddenApplication_Number = (HiddenField)row.FindControl("HiddenApplication_Number");
                    CheckBox m_chkcoi = (CheckBox)row.FindControl("chkcoi");
                    TextBox m_TxtReson = (TextBox)row.FindControl("TxtReson");
                    m_Application_Number = HiddenApplication_Number.Value.ToString().Trim();

                    sql = "INSERT INTO TB_DECLARATION_APPLICATION(Vetting_Delclaration_ID,Application_Number,Conflict_Of_Interest,Reason) VALUES("
                            + "@m_Vetting_Delclaration_ID, "
                            + "@Application_Number,"
                            + "@Conflict_Of_Interest,"
                            + "@Reason);";

                    //lbltest.Text += sql + "<br/>";

                    //conn.Open();
                    SqlCommand cmdGV = new SqlCommand(sql, conn);
                    cmdGV.Parameters.Add("@m_Vetting_Delclaration_ID", m_Vetting_Delclaration_ID);
                    cmdGV.Parameters.Add("@Application_Number", m_Application_Number);
                    cmdGV.Parameters.Add("@Conflict_Of_Interest", m_chkcoi.Checked);
                    cmdGV.Parameters.Add("@Reason", m_TxtReson.Text.ToString().Trim());
                    cmdGV.ExecuteNonQuery();
                    //conn.Close();

                }

            }
            finally
            {
                conn.Close();
            }



        }

        protected void insertVettDeclaration(string m_Vetting_Meeting_ID, string m_systemuser, string m_VettingVenue, string m_userName) 
        {
            string sql = "";
            
            sql = "INSERT INTO TB_VETTING_DECLARATION(Vetting_Delclaration_ID, Vetting_Meeting_ID, Member_Email, DateTime, Venue,Name,No_Conflict_Application,Abstained_Voting_Application) VALUES("
                        + "NEWID(), "
                        + "@m_Vetting_Meeting_ID, "
                        + "@m_systemuser,"
                        + "GETDATE() , "
                        + "@m_VettingVenue,"
                        + "@m_userName,"
                        + "@chkDec1 , "
                        + "@chkDec2);";
                        
            
            //lbltest.Text += sql;

            SqlConnection conn = new SqlConnection(connStr);
            
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID);
                cmd.Parameters.Add("@m_systemuser", m_systemuser);
                cmd.Parameters.Add("@m_VettingVenue", m_VettingVenue);
                cmd.Parameters.Add("@m_userName", m_userName);
                //cmd.Parameters.Add("@chkDec1", chkDec1.Checked);
                //cmd.Parameters.Add("@chkDec2", chkDec2.Checked);
                cmd.Parameters.Add("@chkDec1", rbtDec1.Checked);
                cmd.Parameters.Add("@chkDec2", rbtDec2.Checked);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        protected void updateVettDeclaration(string m_Vetting_Delclaration_ID , string m_Vetting_Meeting_ID, string m_systemuser, string m_VettingVenue, string m_userName)
        {
            string sql = "";
            //var m_Presentation_To = m_Presentation_From.AddMinutes(int.Parse(m_Time_Interval));
            sql = "UPDATE TB_VETTING_DECLARATION SET "
                        + "No_Conflict_Application = @chkDec1, "
                        + "Abstained_Voting_Application = @chkDec2 "
                        + "Where Vetting_Delclaration_ID = @m_Vetting_Delclaration_ID "
                        + "and Vetting_Meeting_ID = @m_Vetting_Meeting_ID "
                        + "and Member_Email = @m_systemuser ";

            //lbltest.Text += sql;
            //
            //
            //lbltest.Text += "||" + chkDec1.Checked.ToString() + "||";

            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@m_Vetting_Delclaration_ID", m_Vetting_Delclaration_ID);
                cmd.Parameters.Add("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID);
                cmd.Parameters.Add("@m_systemuser", m_systemuser);
                //cmd.Parameters.Add("@chkDec1", chkDec1.Checked);
                //cmd.Parameters.Add("@chkDec2", chkDec2.Checked);
                cmd.Parameters.Add("@chkDec1", rbtDec1.Checked);
                cmd.Parameters.Add("@chkDec2", rbtDec2.Checked);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
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

        private bool CheckMeetingIsCompleted()
        {
            bool result = false;
            string sql = "SELECT Meeting_Completed FROM TB_VETTING_MEETING WHERE Vetting_Meeting_ID = @vmID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@vmID", m_VMID);

                    if (command.ExecuteScalar() != null && DBNull.Value != command.ExecuteScalar())
                    {
                        result = (bool)command.ExecuteScalar();
                    }

                    conn.Close();
                }
            }
            return result;
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

                    if (reader.GetString(0) == "WebsiteUrl_ccmf_form")
                    {
                        m_WebsiteUrl_ccmf_form = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "WebsiteUrl_cpip_form")
                    {
                        m_WebsiteUrl_cpip_form = reader.GetString(1);

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

        protected void imbClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlWarning.Visible = false;

            Context.Response.Redirect("~/SitePages/Application%20List%20for%20Vetting%20Team.aspx", false);
        }

        protected void rbtDec1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;

            EnableConflictCheckbox(radioBtn.Checked == true);         


        }

        protected void rbtDec2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;

            EnableConflictCheckbox(radioBtn.Checked == false);
           
        }

        private void EnableConflictCheckbox(bool IsNoConflict)
        {
            if (IsNoConflict)
            {
                foreach (GridViewRow row in GridView_App.Rows)
                {
                    CheckBox m_chkcoi = (CheckBox)row.FindControl("chkcoi");
                    TextBox m_TxtReson = (TextBox)row.FindControl("TxtReson");

                    m_chkcoi.Enabled = false;
                    m_TxtReson.Enabled = false;

                    m_chkcoi.Checked = false;
                    m_TxtReson.Text = string.Empty;
                }
            }
            else
            {
                foreach (GridViewRow row in GridView_App.Rows)
                {
                    CheckBox m_chkcoi = (CheckBox)row.FindControl("chkcoi");
                    TextBox m_TxtReson = (TextBox)row.FindControl("TxtReson");

                    m_chkcoi.Enabled = true;
                    m_TxtReson.Enabled = true;
                }
            }
        }
    }
}
