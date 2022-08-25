using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.SharePoint.Utilities;

namespace CBP_EMS_SP.RejectBDMWP.RejectBDMWP
{
    [ToolboxItemAttribute(false)]
    public partial class RejectBDMWP : WebPart
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
        private int m_CCMF;
        private int m_INCUBATION;
        private String m_Status;
        private String m_Role;
        string m_Programme_Type = "";
        private SqlConnection connection;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        string m_WebsiteUrl = "";
        string m_WebsiteUrl_VettingTeam = "";
        string m_WebsiteUrl_InvitationResponse = "";
        string m_WebsiteUrl_Coordinator = "";
        
        string m_mail = "";
        string m_subject = "";
        string m_body = "";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public RejectBDMWP()
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

            //m_progid = Context.Request.QueryString["ProgNo"]; //Get Programme ID from URL
            //m_ApplicationID = Context.Request.QueryString["AppNo"]; //Get Application Number from URL
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User


            m_Programme_Type = checkProgrammeType(m_progid);


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = '" + m_ApplicationID + "'", conn))
                //{
                //    conn.Open();
                //    m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //    conn.Close();
                //}
                //
                //using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                //{
                //    conn.Open();
                //    m_CCMF = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //    conn.Close();
                //}

                if (m_Programme_Type == "CPIP")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @m_ApplicationID", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
                        conn.Open();
                        try
                        {
                            //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    //txtcommentforapplicants.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @m_ApplicationID ", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
                        try
                        {
                            conn.Open();
                            //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_Status = cmd.ExecuteScalar().ToString();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }

                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @m_ApplicationID ", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
                        conn.Open();
                        try
                        {
                            ////cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                    //txtcommentforinternaluse.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @m_ApplicationID ", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
                        conn.Open();
                        try
                        {
                            //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_Status = cmd.ExecuteScalar().ToString();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }

                }


            }

            getReview();

            if (!Page.IsPostBack)
            {
                getdbdata();
            }
            else
            {

            }

            lblrole.Text = m_Role;
            lblApplicationStatus.Text = m_Status;


            
            AccessControl();

            //lbltest.Text += SPContext.Current.Web.CurrentUser.Email.ToString();
        }


        protected void getdbdata()
        {
            getCommentsFromCoordinator(m_ApplicationID, m_progid, m_Programme_Type);
            setRadio(m_ApplicationNumber, m_progid);
            getCommentsFromRejectDisqualified(m_ApplicationNumber, m_progid);
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
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@m_Programme_ID", m_Programme_ID);
                        m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }

                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE Programme_ID = @m_Programme_ID", conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@m_Programme_ID", m_Programme_ID);
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

        protected string checkDisqualified(string m_Application_Number, string m_Programme_ID, string Validation_Results)
        {
            var m_result = "";
            string sql = "";

            sql = "SELECT Screening_Comments_ID, Application_Number, Programme_ID, Validation_Result, Comment_For_Applicants, Comment_For_Internal_Use, Created_By, Created_Date "
                + "FROM TB_SCREENING_HISTORY "
                + "WHERE (Application_Number = @m_ApplicationID) AND (Programme_ID = @m_progid) AND (Validation_Result = 'Disqualified' OR "
                //+ "Validation_Result = 'BDM Rejected') and Created_By like '%BDM%'"
                + "Validation_Result = 'BDM Rejected') "
                + "ORDER BY Created_Date DESC ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_Application_Number));
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_Programme_ID));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            m_result = reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString();
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

        protected void getCommentsFromCoordinator(string m_ApplicationID, string m_progid, string m_Programme_Type)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                if (m_Programme_Type == "CPIP")
                {
                    sql = "SELECT dbo.TB_SCREENING_HISTORY.Screening_Comments_ID, dbo.TB_SCREENING_HISTORY.Application_Number, dbo.TB_SCREENING_HISTORY.Programme_ID, dbo.TB_SCREENING_HISTORY.Validation_Result, "
                        + "dbo.TB_SCREENING_HISTORY.Comment_For_Applicants, dbo.TB_SCREENING_HISTORY.Comment_For_Internal_Use, dbo.TB_SCREENING_HISTORY.Created_By, dbo.TB_SCREENING_HISTORY.Created_Date, "
                        + "dbo.TB_INCUBATION_APPLICATION.Incubation_ID "
                        + "FROM dbo.TB_SCREENING_HISTORY LEFT OUTER JOIN "
                        + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_SCREENING_HISTORY.Application_Number = dbo.TB_INCUBATION_APPLICATION.Application_Number "
                        + "WHERE (Incubation_ID = @m_ApplicationID) AND (dbo.TB_SCREENING_HISTORY.Programme_ID = @m_progid) AND (dbo.TB_SCREENING_HISTORY.Validation_Result = 'Eligibility checked' OR "
                        + "dbo.TB_SCREENING_HISTORY.Validation_Result = 'Waiting for response from applicant' OR "
                        + "dbo.TB_SCREENING_HISTORY.Validation_Result = 'To be disqualified') ";
                }
                else
                {
                    sql = "SELECT dbo.TB_SCREENING_HISTORY.Screening_Comments_ID, dbo.TB_SCREENING_HISTORY.Application_Number, dbo.TB_SCREENING_HISTORY.Programme_ID, dbo.TB_SCREENING_HISTORY.Validation_Result, "
                            + "dbo.TB_SCREENING_HISTORY.Comment_For_Applicants, dbo.TB_SCREENING_HISTORY.Comment_For_Internal_Use, dbo.TB_SCREENING_HISTORY.Created_By, dbo.TB_SCREENING_HISTORY.Created_Date, "
                            + "dbo.TB_CCMF_APPLICATION.CCMF_ID "
                            + "FROM dbo.TB_SCREENING_HISTORY LEFT OUTER JOIN "
                            + "dbo.TB_CCMF_APPLICATION ON dbo.TB_SCREENING_HISTORY.Application_Number = dbo.TB_CCMF_APPLICATION.Application_Number "
                            + "WHERE (CCMF_ID = @m_ApplicationID) AND (dbo.TB_SCREENING_HISTORY.Programme_ID = @m_progid) AND (dbo.TB_SCREENING_HISTORY.Validation_Result = 'Eligibility checked' OR "
                            + "dbo.TB_SCREENING_HISTORY.Validation_Result = 'Waiting for response from applicant' OR "
                            + "dbo.TB_SCREENING_HISTORY.Validation_Result = 'To be disqualified') ";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@m_ApplicationID", m_ApplicationID);
                        cmd.Parameters.Add("@m_progid", m_progid);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lblCommentsCoordinator.Text = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        protected void getCommentsFromRejectDisqualified(string m_ApplicationNumber, string m_progid)
        {
            var m_result = "";
            string sql = "";

            sql = "SELECT Screening_Comments_ID, Application_Number, Programme_ID, Validation_Result, Comment_For_Applicants, Comment_For_Internal_Use, Created_By, Created_Date "
                + "FROM TB_SCREENING_HISTORY "
                + "WHERE (Application_Number = @m_ApplicationID) AND (Programme_ID = @m_progid) AND (Validation_Result = 'Disqualified' OR "
                + "Validation_Result = 'BDM Rejected') and (Created_By like '%BDM%' or Created_By like '%Coordinator%')"
                + "ORDER BY Created_Date DESC ";

            sql = "SELECT Screening_Comments_ID, Application_Number, Programme_ID, Validation_Result, Comment_For_Applicants, Comment_For_Internal_Use, Created_By, Created_Date "
                + "FROM TB_SCREENING_HISTORY "
                + "WHERE (Application_Number = @m_ApplicationID) AND (Programme_ID = @m_progid) "
                + "AND (Validation_Result = 'Disqualified' "
                + "OR Validation_Result = 'BDM Rejected' "
                + "OR Validation_Result = 'To be disqualified' "
                + "OR Validation_Result = 'Eligibility checked' "
                + "OR Validation_Result = 'Waiting for response from applicant' ) "
                //+ "AND (Created_By like '%BDM%' or Created_By like '%Coordinator%') "
                + "ORDER BY Created_Date DESC ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationNumber));
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        var reader = cmd.ExecuteReader();
                        List<SearchResult> historys = new List<SearchResult>();

                        while (reader.Read())
                        {
                            SearchResult item = new SearchResult();

                            item.User = reader.GetValue(reader.GetOrdinal("Created_By")).ToString();
                            item.datetime = reader.GetDateTime(reader.GetOrdinal("Created_Date")).ToString("dd MM yyyy, HH:mm:ss");
                            item.Result = reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString();
                            item.CommentForApplicants = reader.GetValue(reader.GetOrdinal("Comment_For_Applicants")).ToString();
                            item.CommentForInternualUse = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();

                            historys.Add(item);

                            //txtcommentforinternaluse.Text  = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();
                            ///rbt_Reject_Disqualify.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim()).Selected = true;
                        }
                        RepeaterHistory.DataSource = historys;
                        RepeaterHistory.DataBind();
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
            
        }

        protected void setRadio(string m_ApplicationNumber, string m_progid)
        {
            var m_result = "";
            string sql = "";
            
            sql = "SELECT top (1) Screening_Comments_ID, Application_Number, Programme_ID, Validation_Result, Comment_For_Applicants, Comment_For_Internal_Use, Created_By, Created_Date "
                + "FROM TB_SCREENING_HISTORY "
                + "WHERE (Application_Number = @m_ApplicationID) AND (Programme_ID = @m_progid) "
                + "AND (Validation_Result = 'Disqualified' "
                + "OR Validation_Result = 'BDM Rejected') "
                //+ "AND (Created_By like '%BDM%' ) "
                + "ORDER BY Created_Date DESC";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationNumber));
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        var reader = cmd.ExecuteReader();
                        

                        while (reader.Read())
                        {
                            //txtcommentforinternaluse.Text  = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();
                            rbt_Reject_Disqualify.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim()).Selected = true;
                        }
                        
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }

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
                //if (m_Status == "Eligibility checked" || m_Status == "Coordinator Reviewed" || m_Status == "To be disqualified") { MainPanel.Visible = true; } else { MainPanel.Visible = false; }
                //if (m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw")
                if (m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw")
                {
                    MainPanel.Visible = false;
                }
                else
                {
                    MainPanel.Visible = true;
                }


                if (m_Status == "CPMO Reviewed")
                {
                    rbt_Reject_Disqualify.Items.Remove(rbt_Reject_Disqualify.Items.FindByValue("BDM Rejected"));
                    rbt_Reject_Disqualify.Items[0].Selected = true;

                }
                

            }
            else if (m_Role == "CPIP BDM")
            {
                //if (m_Status == "Eligibility checked" || m_Status == "Coordinator Reviewed" || m_Status == "To be disqualified") { MainPanel.Visible = true; } else { MainPanel.Visible = false; }
                //if ((m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw"))
                if ((m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" ||  m_Status == "Complete Screening" || m_Status == "Withdraw"))
                {
                    MainPanel.Visible = false;
                }
                else
                {
                    
                    if (m_Programme_Type == "CPIP")
                    {   
                        MainPanel.Visible = true;
                    }
                    else 
                    {
                        MainPanel.Visible = false;
                    }

                    if (m_Status == "CPMO Reviewed")
                    {
                        rbt_Reject_Disqualify.Items.Remove(rbt_Reject_Disqualify.Items.FindByValue("BDM Rejected"));
                        rbt_Reject_Disqualify.Items[0].Selected = true;
                    }

                }
            }
            else if (m_Role == "Senior Manager")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "CPMO")
            {
                MainPanel.Visible = false;
            }
            //else if (m_Status == "Waiting for response from applicant" || m_Status == "Resubmitted information" || m_Status == "To be disqualified" || m_Status == "Disqualified")
            //{
            //    MainPanel.Visible = false;
            //}




            // Check Status can display submit button
            if (m_Status == "Submitted")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Waiting for response from applicant")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Resubmitted information")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Eligibility checked")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "To be disqualified")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Disqualified")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Reviewed")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Rejected")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Sr. Mgr. Reviewed")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "CPMO Reviewed")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Final Review")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
            }
            else if (m_Status == "Complete Screening")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }

            // Programme Intake Deadline
            if (getProgrmNameIntakeafterDeadline() == false)
            {
                MainPanel.Visible = false;
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
                //var sqlString = "select Programme_Name,Intake_Number from TB_PROGRAMME_INTAKE where Programme_ID='" + m_progid + "';";
                sqlString = "select Programme_Name,Intake_Number from TB_PROGRAMME_INTAKE where Programme_ID=@progid and Application_Deadline < GETDATE()";

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
                    //m_programName = reader.GetString(0);
                    //m_intake = reader.GetInt32(1).ToString();
                    //reader.GetValue(reader.GetOrdinal("Application_Deadline")).ToString();
                }

                reader.Dispose();
                command.Dispose();

            }
            finally
            {
                ConnectClose();
            }

            if (countNum == 0)
            {
                m_result = false;
            }
            else
            {
                m_result = true;
            };

            return m_result;
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

        protected void updateStatus_Reject()
        {
            string m_ApplicationID_temp = m_ApplicationID;
            m_ApplicationID = m_ApplicationNumber;

            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);
            string status_temp = "";
            status_temp = "BDM Rejected";
            foreach (SPListItem oListItem in collListItems)
            {   
                oListItem["Status"] = "BDM Rejected";
                oListItem["BDM_Comment_for_internal_use"] = txtcommentforinternaluse.Text.ToString(); //For SharePoint Application List Testing Field

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;

            }

            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";

            if (m_Programme_Type == "CPIP")
            {
                sql = "UPDATE TB_INCUBATION_APPLICATION SET "
                  + "Modified_By=@m_systemuser , "
                  + "Modified_Date= GETDATE() , "
                  + "Status = @status_temp "
                  + "WHERE Incubation_ID = @m_ApplicationID_temp ";
            }
            else
            {
                sql = "UPDATE TB_CCMF_APPLICATION SET "
                  + "Modified_By= @m_systemuser , "
                  + "Modified_Date= GETDATE() , "
                  + "Status = @status_temp "
                  + "WHERE CCMF_ID = @m_ApplicationID_temp ";
            }

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@m_systemuser", m_systemuser);
                cmd.Parameters.Add("@status_temp", status_temp);
                cmd.Parameters.Add("@m_ApplicationID_temp", m_ApplicationID_temp);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }


        protected void updateStatus_Disqualify()
        {
            string m_ApplicationID_temp = m_ApplicationID;
            m_ApplicationID = m_ApplicationNumber;

            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);
            string status_temp = "";
            status_temp = "Disqualified";

            foreach (SPListItem oListItem in collListItems)
            {
                
                oListItem["Status"] = "Disqualified";
                oListItem["BDM_Comment_for_internal_use"] = txtcommentforinternaluse.Text.ToString(); //For SharePoint Application List Testing Field

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }

            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";

            sql = "UPDATE TB_CCMF_APPLICATION SET Status = @status_temp WHERE CCMF_ID = @m_ApplicationID_temp ";
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@status_temp", status_temp));
                cmd.Parameters.Add(new SqlParameter("@m_ApplicationID_temp", m_ApplicationID_temp));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }


            sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = @status_temp WHERE Incubation_ID = @m_ApplicationID_temp ";
            conn.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.Parameters.Add(new SqlParameter("@status_temp", status_temp));
                cmd1.Parameters.Add(new SqlParameter("@m_ApplicationID_temp", m_ApplicationID_temp));
                cmd1.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }


        }

        protected void addCommenttoCoordinator(string m_Comment_For_Internal_Use, string m_Application_Number, string m_Programme_ID) 
        {
            var m_result = "";
            string sql = "";

            sql = "SELECT Screening_Comments_ID, Application_Number, Programme_ID, Validation_Result, Comment_For_Applicants, Comment_For_Internal_Use, Created_By, Created_Date "
                + "FROM TB_SCREENING_HISTORY "
                + "WHERE (Application_Number = @Application_Number) AND (Programme_ID = @m_Programme_ID) AND (Validation_Result = 'Disqualified' OR "
                //+ "Validation_Result = 'BDM Rejected') and Created_By like '%Coordinator%'"
                + "Validation_Result = 'BDM Rejected') "
                + "ORDER BY Created_Date DESC ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@Application_Number", m_Application_Number));
                        cmd.Parameters.Add(new SqlParameter("@m_Programme_ID", m_Programme_ID));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            txtcommentforinternaluse.Text = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();
                            rbt_Reject_Disqualify.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim()).Selected = true;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }

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

                    if (reader.GetString(0) == "WebsiteUrl_Coordinator")
                    {
                        m_WebsiteUrl_Coordinator = reader.GetString(1);

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

        protected void sendToCoordinator(string m_EmailList, string m_AppNumber, string m_Comment)
        {

            //m_WebsiteUrl += m_WebsiteUrl_VettingTeam;
            //Application%20List.aspx?ischeck=2&roleid=D5D1FB1A
            //string m_passvalue = "?ischeck=2&roleid=D5D1FB1A";
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
            }
            else
            {
                m_mail = m_EmailList;
            }

            m_subject = "Notification Email for Reject";

            m_body = getEmailTemplate("BDM_Reject_Validation");
            m_body = m_body.Replace("@@AppNumber", m_AppNumber)
                    .Replace("@@Comment", m_Comment)
                    .Replace("@@WebsiteUrl", m_WebsiteUrl);

            if (m_ApplicationIsInDebug == "1")
            {
                m_body = "(" + m_EmailList + " " + DateTime.Now.ToString() + " )" + m_body;
            }
            else
            {

            }

            sharepointsendemail(m_mail, m_subject, m_body);
            //lbltest.Text += "m_ApplicationDebugEmailSentTo" + m_ApplicationDebugEmailSentTo + "<br/>";
            //lbltest.Text += "sendToApplicant:" + m_mail + m_Presentation_From + m_Presentation_To + m_Vetting_Application_ID + "<br/>";
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
            String emailTemplateContent = "";
            ConnectOpen();
            try
            {
                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template=@emailTemplate ;";

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

        protected void btnSubmitPop_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);

            string Application_Number = m_ApplicationNumber;
            string Programme_ID = m_progid;
            //string Validation_Results = "Rejected by BDM";
            string Validation_Results = "BDM Rejected";
            string Comment_For_Applicants = "";
            string Comment_For_Internal_Use = txtcommentforinternaluse.Text.ToString();
            string Created_By = m_systemuser;
            DateTime Created_Date = DateTime.Now;
            string m_checkDisqualified = "";
            string sql = "";


            m_checkDisqualified = checkDisqualified(Application_Number, Programme_ID, Validation_Results);

            if (rbt_Reject_Disqualify.SelectedValue == "BDM Rejected")
            //if (Convert.ToInt32(rbt_Reject_Disqualify.SelectedIndex) == 0)
            {
                Validation_Results = "BDM Rejected";
                updateStatus_Reject();
                
                //sendToCoordinator();
                //lbltest.Text = SPContext.Current.Web.CurrentUser.Email.ToString();
                
            }
            else
            {
                Validation_Results = "Disqualified";
                updateStatus_Disqualify();
                updateShortlisted(Application_Number, Programme_ID, "0");
            }

            //lblrole.Text = Validation_Results + " " + rbt_Reject_Disqualify.SelectedIndex.ToString();
                
                //if (m_checkDisqualified == "" )
                //{
                    sql = "insert into TB_SCREENING_HISTORY(Application_Number,Programme_ID,Validation_Result,Comment_For_Applicants,Comment_For_Internal_Use,Created_By,Created_Date) VALUES ("
                    + "@Application_Number , "
                    + "@Programme_ID , "
                    + "@Validation_Results , "
                    + "@Comment_For_Applicants , "
                    + "@Comment_For_Internal_Use , "
                    + "@Created_By , "
                    + "GETDATE()"
                    + ")";

                    conn.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@Application_Number", Application_Number);
                        cmd.Parameters.Add("@Programme_ID", Programme_ID);
                        cmd.Parameters.Add("@Validation_Results", Validation_Results);
                        cmd.Parameters.Add("@Comment_For_Applicants", Comment_For_Applicants);
                        cmd.Parameters.Add("@Comment_For_Internal_Use", Comment_For_Internal_Use);
                        cmd.Parameters.Add("@Created_By", Created_By);
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        conn.Close();
                    }

                //}
                //else
                //{
                //
                //    sql = "UPDATE TB_SCREENING_HISTORY SET "
                //        + "Comment_For_Internal_Use= @Comment_For_Internal_Use, "
                //        + "Validation_Result= @Validation_Results "
                //        + "Where (Application_Number = @Application_Number) AND (Programme_ID = @Programme_ID) and Created_By like '%BDM%'";
                //
                //    conn.Open();
                //    try
                //    {
                //        SqlCommand cmd = new SqlCommand(sql, conn);
                //        cmd.Parameters.Add("@Application_Number", Application_Number);
                //        cmd.Parameters.Add("@Programme_ID", Programme_ID);
                //        cmd.Parameters.Add("@Validation_Results", Validation_Results);
                //        cmd.Parameters.Add("@Comment_For_Internal_Use", Comment_For_Internal_Use);
                //        cmd.ExecuteNonQuery();
                //    }
                //    finally
                //    {
                //        conn.Close();
                //    }
                //
                //}


                //addCommenttoCoordinator(Comment_For_Internal_Use, Application_Number, Programme_ID);


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


            //newdirection = "Application%20List.aspx";
            Context.Response.Redirect(newdirection);

        }

        protected void RepeaterHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label m_lblCommentforApplicationts = (Label)e.Item.FindControl("lblCommentforApplicationts");
            Label m_lblCommentforApplicationtsLabel = (Label)e.Item.FindControl("lblCommentforApplicationtsLabel");
            Label m_lblCommentforInternualUse = (Label)e.Item.FindControl("lblCommentforInternualUse");
            Label m_lblCommentforInternualUseLabel = (Label)e.Item.FindControl("lblCommentforInternualUseLabel");

            
            if (String.IsNullOrEmpty( m_lblCommentforApplicationts.Text)) 
            {
                m_lblCommentforApplicationts.Visible = false;
                m_lblCommentforApplicationtsLabel.Visible = false;
            }

            if (String.IsNullOrEmpty(m_lblCommentforInternualUse.Text))
            {
                m_lblCommentforInternualUse.Visible = false;
                m_lblCommentforInternualUseLabel.Visible = false;
            }



        }

        protected void updateShortlisted(String ApplicationNumber, String ProgramId, String ShortlistedStatus)
        {
            SqlConnection conn = new SqlConnection(connStr);
            var sql = "update TB_APPLICATION_SHORTLISTING set Shortlisted = @Shortlisted where Application_Number = @Application_Number and Programme_ID = @Programme_ID ";
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Shortlisted", ShortlistedStatus));
                cmd.Parameters.Add(new SqlParameter("@Application_Number", ApplicationNumber));
                cmd.Parameters.Add(new SqlParameter("@Programme_ID", ProgramId));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
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
