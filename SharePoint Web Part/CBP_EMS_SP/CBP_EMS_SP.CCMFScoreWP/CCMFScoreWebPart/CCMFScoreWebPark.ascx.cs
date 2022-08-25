using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Microsoft.SharePoint.Utilities;
using System.Text;

namespace CBP_EMS_SP.CCMFScoreWP.CCMFScoreWebPark
{
    [ToolboxItemAttribute(false)]
    public partial class CCMFScoreWebPark : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]


        private String m_progid;
        private String m_ApplicationID;
        private String m_Role;
        private String m_ApplicationNumber;
        private String m_Status;
        private String m_insert_Update = "";

        private SqlConnection connection;
        public String m_path = System.Configuration.ConfigurationManager.AppSettings["DownloadPath"] ?? @"D:\\tmp";
        public String m_programName;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "http://cyberportemssp:10869/SitePages/DownloadPage.aspx";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public CCMFScoreWebPark()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            m_progid = HttpContext.Current.Request.QueryString["Prog"];
            m_ApplicationID = HttpContext.Current.Request.QueryString["App"];

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID ", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                    conn.Open();
                    //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                    try
                    {
                        m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID ", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                    conn.Open();
                    //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                    try
                    {
                        m_Status = cmd.ExecuteScalar().ToString();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            
            getReview();
            screenValueSet();

            if (!Page.IsPostBack)
            {
                getdbdata();
                screenValueSet();

            }

            AccessControl();
            

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

        protected string ProIntakeStatus(string m_progid)
        {
            string m_result="";

            string sql = "SELECT Programme_ID, Programme_Name, Intake_Number, Application_No_Prefix, Application_Start, Application_Deadline, Application_Deadline_Eng, Application_Deadline_TradChin, "
                       + "Application_Deadline_SimpChin, Vetting_Session_Eng, Vetting_Session_TradChin, Vetting_Session_SimpChin, Result_Announce_Eng, Result_Announce_TradChin, Result_Announce_Simp_Chin, Active, "
                       + "Created_By, Created_Date, Modified_By, Modified_Date, Status "
                       + "FROM TB_PROGRAMME_INTAKE "
                       + "WHERE (Programme_ID = @Programme_ID) ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Programme_ID", m_progid));
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            m_result  = reader.GetValue(reader.GetOrdinal("Status")).ToString();

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

        protected void AccessControl() 
        {
            // Check Role can display this web part
            //Applicant  //Collaborator  //CCMF Coordinator //CCMF BDM  //CPIP Coordinator  //CPIP BDM  //Senior Manager  //CPMO

            if (m_Role == "Applicant")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "Collaborator")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "CCMF Coordinator")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "CPIP Coordinator")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "CCMF BDM")
            {
                if (m_Status == "To be disqualified" || m_Status == "Eligibility checked" || m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" )
                {
                    MainPanel.Visible = true;
                }
                else 
                {
                    MainPanel.Visible = false;
                }
            }
            else if (m_Role == "CPIP BDM")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "Senior Manager")
            {
                if (m_Status == "Eligibility checked" || m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed")
                {
                    MainPanel.Visible = true;
                }
                else
                {
                    MainPanel.Visible = false;
                }
            }
            else if (m_Role == "CPMO")
            {
                if (m_Status == "Eligibility checked" || m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed")
                {
                    MainPanel.Visible = true;
                }
                else
                {
                    MainPanel.Visible = false;
                }
            }

            // Check Status can display submit button
            if (m_Status == "Submitted")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "Cannot submit CCMF Score on " + m_Status + " status.";
                lstMT.Enabled = false;
                lstBMTM.Enabled = false;
                lstCIPP.Enabled = false;
                lstSR.Enabled = false;
                lstcomment.Enabled = false;
                txtremarks.Enabled = false;
            }
            else if (m_Status == "Waiting for response from applicant")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
                lstMT.Enabled = false;
                lstBMTM.Enabled = false;
                lstCIPP.Enabled = false;
                lstSR.Enabled = false;
                lstcomment.Enabled = false;
                txtremarks.Enabled = false;
            }
            else if (m_Status == "Resubmitted information")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "Coordinator Reviewed")
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Eligibility checked")
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "To be disqualified")
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Disqualified")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
                lstMT.Enabled = false;
                lstBMTM.Enabled = false;
                lstCIPP.Enabled = false;
                lstSR.Enabled = false;
                lstcomment.Enabled = false;
                txtremarks.Enabled = false;
            }
            else if (m_Status == "BDM Reviewed")
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Rejected")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
                lstMT.Enabled = false;
                lstBMTM.Enabled = false;
                lstCIPP.Enabled = false;
                lstSR.Enabled = false;
                lstcomment.Enabled = false;
                txtremarks.Enabled = false;
            }
            else if (m_Status == "Sr. Mgr. Reviewed")
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "CPMO Reviewed")
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Final Review")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
                lstMT.Enabled = false;
                lstBMTM.Enabled = false;
                lstCIPP.Enabled = false;
                lstSR.Enabled = false;
                lstcomment.Enabled = false;
                txtremarks.Enabled = false;
            }
            else if (m_Status == "Complete Screening")
            {
                btnsubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
                lstMT.Enabled = false;
                lstBMTM.Enabled = false;
                lstCIPP.Enabled = false;
                lstSR.Enabled = false;
                lstcomment.Enabled = false;
                txtremarks.Enabled = false;
            }
            else
            {
                btnsubmit.Enabled = true;
                lblmessage.Text = "";
            }

            // Programme Intake Deadline
            //lblrole.Text = getProgrmNameIntakeafterDeadline().ToString();
            if (getProgrmNameIntakeafterDeadline() == false) 
            {
                MainPanel.Visible = false;
            }

        }

        protected void getdbdata()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                conn.Open();
                try
                {
                    string tempsql = "SELECT CCMF_Scoring_ID , Application_Number , Programme_ID , Reviewer , Role , Management_Team , Business_Model , Creativity , Social_Responsibility , Total_Score , Comments , Remarks , Created_By , Created_Date , Modified_By , Modified_Date FROM [TB_SCREENING_CCMF_SCORE] WHERE Application_Number = @ApplicationNumber AND Role=@Role ";
                    using (SqlCommand cmd = new SqlCommand(tempsql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ApplicationNumber", m_ApplicationNumber));
                        cmd.Parameters.Add(new SqlParameter("@Role", m_Role));
                        double dropdown;
                        double dbtemp;

                        //conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            for (int i = 0; i <= lstMT.Items.Count - 1; i++)
                            {
                                //txtremarks.Text += "||| " + lstqcmt.Items[i].Value;
                                dropdown = Convert.ToDouble(lstMT.Items[i].Value);
                                dbtemp = Convert.ToDouble(reader.GetValue(5)) / 0.3;
                                dbtemp = Convert.ToDouble(dbtemp);

                                if (Convert.ToString(dropdown) == Convert.ToString(dbtemp))
                                {
                                    lstMT.SelectedIndex = i;
                                }

                            }

                            for (int i = 0; i <= lstBMTM.Items.Count - 1; i++)
                            {
                                dropdown = Convert.ToDouble(lstBMTM.Items[i].Value);
                                dbtemp = Convert.ToDouble(reader.GetValue(6)) / 0.3;
                                dbtemp = Convert.ToDouble(dbtemp);

                                if (Convert.ToString(dropdown) == Convert.ToString(dbtemp))
                                {
                                    lstBMTM.SelectedIndex = i;
                                }

                            }

                            for (int i = 0; i <= lstCIPP.Items.Count - 1; i++)
                            {
                                //txtremarks.Text += "||| " + lstqcmt.Items[i].Value;
                                dropdown = Convert.ToDouble(lstCIPP.Items[i].Value);
                                dbtemp = Convert.ToDouble(reader.GetValue(7)) / 0.3;
                                dbtemp = Convert.ToDouble(dbtemp);

                                if (Convert.ToString(dropdown) == Convert.ToString(dbtemp))
                                {
                                    lstCIPP.SelectedIndex = i;
                                }

                            }

                            for (int i = 0; i <= lstSR.Items.Count - 1; i++)
                            {
                                //txtremarks.Text += "||| " + lstqcmt.Items[i].Value;
                                dropdown = Convert.ToDouble(lstSR.Items[i].Value);
                                dbtemp = Convert.ToDouble(reader.GetValue(8)) / 0.1;
                                dbtemp = Convert.ToDouble(dbtemp);

                                if (Convert.ToString(dropdown) == Convert.ToString(dbtemp))
                                {
                                    lstSR.SelectedIndex = i;
                                }

                            }

                            for (int i = 0; i <= lstcomment.Items.Count - 1; i++)
                            {

                                if (reader.GetValue(10).ToString() == lstcomment.Items[i].Value)
                                {
                                    lstcomment.SelectedIndex = i;
                                }
                            }

                            txtremarks.Text = reader.GetValue(11).ToString();

                        }
                        //conn.Close();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        protected void getReview()
        {

            lblrole.Text = "";
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
                        lblrole.Text = ogroup.Name;
                        m_Role = ogroup.Name;
                    }
                }
            }
            
        }

        protected void screenValueSet()
        {
            double lstMT_Num = Convert.ToDouble(lstMT.SelectedValue);
            double lstBMTM_Num = Convert.ToDouble(lstBMTM.SelectedValue);
            double lstCIPP_Num = Convert.ToDouble(lstCIPP.SelectedValue);
            double lstSR_Num = Convert.ToDouble(lstSR.SelectedValue);
            double TotalScore = ((lstMT_Num * scorepresent("lstMT_Num")) + (lstBMTM_Num * scorepresent("lstBMTM_Num")) + (lstCIPP_Num * scorepresent("lstCIPP_Num")) + (lstSR_Num * scorepresent("lstSR_Num")));
            lblTotalScore.Text = TotalScore.ToString("F3");
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {

                string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();

                string Application_Number = m_ApplicationNumber;  //"4C0B5391-D2C7-444D-8B6B-16F1B5EADE7D";
                string Programme_ID = m_progid;
                string Reviewer = systemuser;
                string Role = m_Role; //m_Role
                double Management_Team = Convert.ToDouble(lstMT.SelectedValue) * scorepresent("lstMT_Num");
                double Business_Model = Convert.ToDouble(lstBMTM.SelectedValue) * scorepresent("lstBMTM_Num");
                double Creativity = Convert.ToDouble(lstCIPP.SelectedValue) * scorepresent("lstCIPP_Num");
                double Social_Responsibility = Convert.ToDouble(lstSR.SelectedValue) * scorepresent("lstSR_Num");
                double Total_Score = Convert.ToDouble(lblTotalScore.Text);
                string Comments = lstcomment.SelectedValue.ToString();
                string Remarks = txtremarks.Text;
                
                string Created_By = systemuser;
                DateTime Created_Date = DateTime.Now;
                string Modified_By = systemuser;
                DateTime Modified_Date = DateTime.Now;
                string sql = "";
                
                string tempsql = "SELECT CCMF_Scoring_ID , Application_Number , Programme_ID , Reviewer , Role , Management_Team , Business_Model , Creativity , Social_Responsibility , Total_Score , Comments , Remarks , Created_By , Created_Date , Modified_By , Modified_Date FROM [TB_SCREENING_CCMF_SCORE] WHERE Application_Number = @ApplicationNumber AND Role=@Role ";
                using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
                {
                    cmdscore.Parameters.Add(new SqlParameter("@ApplicationNumber", m_ApplicationNumber));
                    cmdscore.Parameters.Add(new SqlParameter("@Role", m_Role));
                    var reader = cmdscore.ExecuteReader();
                    while (reader.Read())
                    {
                        m_insert_Update = reader.GetValue(1).ToString();                        
                    }
                }


                if (!string.IsNullOrEmpty(m_insert_Update))
                {

                    sql = "UPDATE TB_SCREENING_CCMF_SCORE SET "
                    + "Reviewer=@Reviewer, "
                    + "Management_Team=@Management_Team, "
                    + "Business_Model=@Business_Model, "
                    + "Creativity=@Creativity, "
                    + "Social_Responsibility=@Social_Responsibility, "
                    + "Total_Score=@Total_Score, "
                    + "Comments=@Comments, "
                    + "Remarks=@Remarks, "
                    + "Modified_By=@Modified_By, "
                    + "Modified_Date= GETDATE() "
                    + "WHERE Application_Number = @Application_Number "
                    + "AND Role=@Role";

                }
                else 
                {

                sql = "insert into TB_SCREENING_CCMF_SCORE(Application_Number,Programme_ID,Reviewer,Role,Management_Team,Business_Model,Creativity,Social_Responsibility,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                    + "@Application_Number , "
                    + "@Programme_ID , "
                    + "@Reviewer , "
                    + "@Role , "
                    + "@Management_Team , "
                    + "@Business_Model , "
                    + "@Creativity , "
                    + "@Social_Responsibility , "
                    + "@Total_Score , "
                    + "@Comments , "
                    + "@Remarks , "
                    + "@Created_By , "
                    + "GETDATE() , "
                    + "@Modified_By , "
                    + "GETDATE()"
                    + ")";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                cmd.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                cmd.Parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                cmd.Parameters.Add(new SqlParameter("@Role", m_Role));
                cmd.Parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                cmd.Parameters.Add(new SqlParameter("@Business_Model", Business_Model));
                cmd.Parameters.Add(new SqlParameter("@Creativity", Creativity));
                cmd.Parameters.Add(new SqlParameter("@Social_Responsibility", Social_Responsibility));
                cmd.Parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                cmd.Parameters.Add(new SqlParameter("@Comments", Comments));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@Created_By", Created_By));
                cmd.Parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                cmd.ExecuteNonQuery();

                
                string m_ProIntakeStatus = ProIntakeStatus(m_progid);

                if (m_Role == "CCMF BDM")
                {
                    if (m_ProIntakeStatus == "Sr. Mgr. Reviewed" || m_ProIntakeStatus == "CPMO Reviewed" || m_ProIntakeStatus == "Complete Screening") 
                    {

                    }
                    else
                    {
                        updateStatus();    
                    }
                }
                else if (m_Role == "Senior Manager")
                {
                    if (m_ProIntakeStatus == "CPMO Reviewed" || m_ProIntakeStatus == "Complete Screening")
                    {

                    }
                    else
                    {
                        updateStatus();
                    }
                }
                else if (m_Role == "CPMO")
                {
                    if (m_ProIntakeStatus == "Complete Screening")
                    {

                    }
                    else
                    {
                        updateStatus();
                    }
                }
                


                string newdirection = "";
                newdirection = "Application%20List.aspx?";
                newdirection += "ProgName=" + HttpContext.Current.Request.QueryString["ProgName"];
                newdirection += "&IntakeNo=" + HttpContext.Current.Request.QueryString["IntakeNo"];
                newdirection += "&Cluster=" + HttpContext.Current.Request.QueryString["Cluster"];
                newdirection += "&Status=" + HttpContext.Current.Request.QueryString["Status"];
                newdirection += "&SortColumn1=" + HttpContext.Current.Request.QueryString["SortColumn1"];
                newdirection += "&SortOrder1=" + HttpContext.Current.Request.QueryString["SortOrder1"];
                newdirection += "&SortColumn2=" + HttpContext.Current.Request.QueryString["SortColumn2"];
                newdirection += "&SortOrder2=" + HttpContext.Current.Request.QueryString["SortOrder2"];


                Context.Response.Redirect(newdirection);

            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        protected static double scorepresent(string num)
        {
            double _num = 0.0;

            switch (num)
            {
                case "lstMT_Num":
                    _num = 0.3;
                    break;

                case "lstBMTM_Num":
                    _num = 0.3;
                    break;

                case "lstCIPP_Num":
                    _num = 0.3;
                    break;

                case "lstSR_Num":
                    _num = 0.1;
                    break;

                default:
                    break;
            }

            return _num;
        }

        protected void updateStatus()
        {
            string m_ApplicationID_temp = m_ApplicationID;
            m_ApplicationID = m_ApplicationNumber;

            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                string status_temp = "";
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                if (m_Role == "CCMF BDM")
                {
                    if (m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed") { status_temp = m_Status; } else { status_temp = "BDM Reviewed"; }
                    //status_temp = "BDM Reviewed";
                    oListItem["Status"] = status_temp;
                    oListItem["BDM_Score"] = lblTotalScore.Text;
                }
                else if (m_Role == "Senior Manager")
                {
                    if (m_Status == "CPMO Reviewed") { status_temp = m_Status; } else { status_temp = "Sr. Mgr. Reviewed"; }
                    //status_temp = "Sr. Mgr. Reviewed";
                    oListItem["Status"] = status_temp;
                    oListItem["Sr_Manager_Score"] = lblTotalScore.Text;
                }
                else if (m_Role == "CPMO")
                {
                    status_temp = "CPMO Reviewed";
                    oListItem["Status"] = "CPMO Reviewed";
                    oListItem["CPMO_Score"] = lblTotalScore.Text;
                }

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;

                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                try
                {
                    string sql = "";

                    sql = "UPDATE TB_CCMF_APPLICATION SET Status = @status_temp  WHERE CCMF_ID = @ApplicationID_temp ";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@status_temp", status_temp));
                    cmd.Parameters.Add(new SqlParameter("@ApplicationID_temp", m_ApplicationID_temp));
                    cmd.ExecuteNonQuery();

                }
                finally
                {
                    conn.Close();
                }
            }
        }

        protected void getProgrmNameIntakeNumByProgID()
        {
            ConnectOpen();
            try
            {
                var sqlString = "select Programme_Name,Intake_Number from TB_PROGRAMME_INTAKE where Programme_ID=@progid ;";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@progid", m_progid));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    m_programName = reader.GetString(0);
                    m_intake = reader.GetInt32(1).ToString();
                }

                reader.Dispose();
                command.Dispose();

            }
            finally
            {
                ConnectClose();
            }
        }

        protected bool getProgrmNameIntakeafterDeadline()
        {
            bool m_result = true;
            var sqlString = "";
            var countNum = 0;

            ConnectOpen();
            try
            {

                sqlString = "SELECT COUNT(Programme_ID) "
                          + "FROM TB_PROGRAMME_INTAKE "
                          + "where Programme_ID=@progid and "
                          + "Application_Deadline < GETDATE() ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@progid", m_progid));
                var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    countNum = reader.GetInt32(0);
                }

                reader.Dispose();
                command.Dispose();

            }
            finally
            {
                ConnectClose();
            }
            
            //lblrole.Text = countNum.ToString();
            if (countNum == 0)
            { 
                m_result = false; 
            }
             else
            {
                m_result = true; 
            };

            //lblrole.Text += m_result.ToString();
            return m_result;
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            getProgrmNameIntakeNumByProgID();
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



                //starting email
                m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is processing.";
                m_body = getEmailTemplate("ZipDownloadStartEmail");
                sharepointsendemail(m_mail, m_subject, m_body);


                /*************************/
                //zip programm in here:

                m_Starttime = DateTime.Now.ToString();

                SPUtility.ValidateFormDigest();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
	                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
	                {
		                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
		                {
                            SPFolder folder = web.GetFolder(folderURL);

                            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));

                            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression

                            zipStream.Password = genRandom(8);	// optional. Null is the same as not setting. Required if using AES.
                            m_Password = zipStream.Password;
                            //lbldatetime.Text = lbldatetime.Text + "   "+zipStream.Password;

                            CompressFolder(folder, zipStream);

                            zipStream.Finish();

                            //zipStream.IsStreamOwner = false;	

                            zipStream.Close();


                            m_Endtime = DateTime.Now.ToString();
                            /*************************/

                            //insert into TB_Download_ZIP
                            InsertTBDownloadZIP(m_username, m_mail, "ZIP", zipFile, FileName, m_Password, "1");

                            //Completed email  

                            m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
                            m_body = getEmailTemplate("ZipDownloadEndEmail");
                            m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_Application_Number", m_ApplicationNumber).Replace("@@m_FileName", FileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime); 
                            sharepointsendemail(m_mail, m_subject, m_body);
                            lbldownloadmessage.Text = "Download Complete.";
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
                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template=@emailTemplate;";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@emailTemplate", emailTemplate));
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
                if (string.IsNullOrEmpty(m_ApplicationDebugEmailSentTo))
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
                if (string.IsNullOrEmpty(m_mail))
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

        protected void InsertTBDownloadZIP(String User_Name, String Email, String type, String Path, String File_Name, String Password, String Status)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "insert into TB_Download_ZIP(User_Name,Email,type,Path,File_Name,Password,Status,Created_By,Created_Date,Modified_By,Modified_Date) values("
                                    + "@User_Name , "
                                    + "@Email , "
                                    + "@type , "
                                    + "@Path  , "
                                    + "@File_Name , "
                                    + "@Password , "
                                    + "@Status, "
                                    + "@user, "
                                    + "GETDATE(), "
                                    + "@user, "
                                    + "GETDATE() "
                                    + " ) ;";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add(new SqlParameter("@User_Name", User_Name));
                command.Parameters.Add(new SqlParameter("@Email", Email));
                command.Parameters.Add(new SqlParameter("@type", type));
                command.Parameters.Add(new SqlParameter("@Path", Path));
                command.Parameters.Add(new SqlParameter("@File_Name", File_Name));
                command.Parameters.Add(new SqlParameter("@Password", Password));
                command.Parameters.Add(new SqlParameter("@Status", Status));
                command.Parameters.Add(new SqlParameter("@user", SPContext.Current.Web.CurrentUser.Name.ToString()));
                command.ExecuteNonQuery();

                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }
    }
}
