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
using Microsoft.SharePoint.Utilities;

namespace CBP_EMS_SP.ECWebPart.ECWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ECWebPart : WebPart
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
        private String m_insert_Update = "";
        private String m_Role;

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
        public String m_downloadLink;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public ECWebPart()
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
            if (HttpContext.Current.Request.QueryString["ProgName"] == "Cyberport Creative Micro Fund - Hong Kong")
                m_CCMF = 1;
            else if (HttpContext.Current.Request.QueryString["ProgName"] == "Cyberport Creative Micro Fund - GBAYEP")  /*Test Debug*/
                m_CCMF = 1;
            else if (HttpContext.Current.Request.QueryString["ProgName"] == "Cyberport University Partnership Programme") 
                m_CCMF = 1;
            else if (HttpContext.Current.Request.QueryString["ProgName"] == "Cyberport Incubation Programme")
                m_INCUBATION = 1;
            else
            {
                m_CCMF = m_INCUBATION = 0;
            }

            // Convert application ID to application name
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                #region 20220926 uncomment this section for count application type instead of progName
                //using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @ApplicationID", conn))
                //{
                //    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                //    conn.Open();
                //    try
                //    {
                //        m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //    }
                //    finally
                //    {
                //        conn.Close();
                //    }
                //}

                //using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID", conn))
                //{
                //    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                //    conn.Open();
                //    try
                //    {
                //        m_CCMF = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //    }
                //    finally
                //    {
                //        conn.Close();
                //    }
                //}
                #endregion

                if (m_INCUBATION != 0)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Application_Number, Status FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @ApplicationID", conn))
                    {
                        conn.Open();
                        try
                        {
                            cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                            var reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                m_ApplicationNumber = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim();
                                m_Status = reader.GetValue(reader.GetOrdinal("Status")).ToString().Trim();
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    //    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                    //    conn.Open();
                    //    try
                    //    {
                    //        //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                    //        m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                    //        //m_Status = cmd.ExecuteScalar().ToString();
                    //        //var reader = cmd.ExecuteReader();
                    //        //m_ApplicationNumber= reader.GetValue(reader.GetOrdinal("Application_Number")).ToString();
                    //        //m_Status = reader.GetValue(reader.GetOrdinal("Status")).ToString();
                    //    }
                    //    finally
                    //    {
                    //        conn.Close();
                    //    }
                    //}
                    ////txtcommentforapplicants.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;

                    //using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @ApplicationID", conn))
                    //{
                    //    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                    //    conn.Open();
                    //    try
                    //    {
                    //        //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                    //        m_Status = cmd.ExecuteScalar().ToString();
                    //    }
                    //    finally
                    //    {
                    //        conn.Close();
                    //    }
                    //}

                }
                else if (m_CCMF != 0)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Application_Number, Status FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID", conn))
                    {
                        conn.Open();
                        try
                        {
                            cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                            var reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                m_ApplicationNumber = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString().Trim();
                                m_Status = reader.GetValue(reader.GetOrdinal("Status")).ToString().Trim();
                            }
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
                getRadio();
                getBDMComments(m_ApplicationNumber, m_progid);
                lblrole.Text = m_Role;
                lblApplicationStatus.Text = m_Status;
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

        protected void getBDMComments(string m_ApplicationNumber, string m_progid) 
        {

            var m_result = "";
            string sql = "";

            sql = "SELECT top (1) Screening_Comments_ID, Application_Number, Programme_ID, Validation_Result, Comment_For_Applicants, Comment_For_Internal_Use, Created_By, Created_Date "
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
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationNumber));
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lblBDMComments.Text = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();
                            if (reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim() == "BDM Rejected")
                            {
                                lblBDMCommentsTitle.Text = "BDM Rejected Comments";
                            }
                            else
                            {
                                lblBDMCommentsTitle.Text = "BDM Disqualified Comments";
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
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
                if (m_Status == "Waiting for response from applicant" || m_Status == "To be disqualified" || m_Status == "Disqualified" || m_Status == "Eligibility checked" || m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw")
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
            else if (m_Role == "CPIP Coordinator")
            {
                if (m_Status == "Waiting for response from applicant" || m_Status == "To be disqualified" || m_Status == "Disqualified" || m_Status == "Eligibility checked" || m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw")
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
            else
            {
               MainPanel.Visible = false;
            }

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


        protected void getdbdata()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string tempsql = "";

                tempsql = "SELECT [Screening_Comments_ID],[Application_Number],[Programme_ID],[Validation_Result],[Comment_For_Applicants],[Comment_For_Internal_Use],[Created_By],[Created_Date] FROM [TB_SCREENING_HISTORY] "
                + "where Programme_ID=@progid and Application_Number=@ApplicationNumber "
                + "AND (Validation_Result = 'Disqualified' "
                + "OR Validation_Result = 'BDM Rejected' "
                + "OR Validation_Result = 'To be disqualified' "
                + "OR Validation_Result = 'Eligibility checked' "
                + "OR Validation_Result = 'Waiting for response from applicant' ) "
                + "ORDER BY Created_Date DESC ";

                //txtcommentforapplicants.Text = tempsql;

                using (SqlCommand cmd = new SqlCommand(tempsql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@progid", m_progid));
                    cmd.Parameters.Add(new SqlParameter("@ApplicationNumber", m_ApplicationNumber));
                    conn.Open();
                    try
                    {
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
                            if (reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim() == "Eligibility checked" || reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim() == "Waiting for response from applicant" || reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim() == "To be disqualified")
                            {
                                //txtcommentforapplicants.Text = "**************************";
                                //rbtnresult.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim()).Selected = true;
                            }
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

        protected void getRadio()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string tempsql = "";


                tempsql = "SELECT top (1) [Screening_Comments_ID],[Application_Number],[Programme_ID],[Validation_Result],[Comment_For_Applicants],[Comment_For_Internal_Use],[Created_By],[Created_Date] FROM [TB_SCREENING_HISTORY] "
                        + "where Programme_ID=@progid and Application_Number=@ApplicationNumber "
                        + "AND (Validation_Result = 'To be disqualified' "
				        + "OR Validation_Result = 'Eligibility checked' "
				        + "OR Validation_Result = 'Waiting for response from applicant' ) "
                        + "ORDER BY Created_Date DESC ";

                using (SqlCommand cmd = new SqlCommand(tempsql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@progid", m_progid));
                    cmd.Parameters.Add(new SqlParameter("@ApplicationNumber", m_ApplicationNumber));
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                                //txtcommentforapplicants.Text = "**************************";
                                rbtnresult.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim()).Selected = true;
                         
                        }
                        
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
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
                        //lblrole.Text = ogroup.Name;
                        m_Role = ogroup.Name;
                    }
                }
            }
            
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);

            if (Page.IsValid)
            {
                //string Application_Number = m_ApplicationID;
                string newdirection = "";
                string Application_Number = m_ApplicationNumber;
                string Programme_ID = m_progid;
                string Validation_Results = "";
                if (rbtnresult.SelectedValue == "Eligibility checked")
                {
                    Validation_Results = "Eligibility checked";
                }
                else if (rbtnresult.SelectedValue == "Waiting for response from applicant")
                {
                    Validation_Results = "Waiting for response from applicant"; //Waiting for response from applicant
                }
                else if (rbtnresult.SelectedValue == "To be disqualified")
                {
                    Validation_Results = "To be disqualified";
                }
                string Comment_For_Applicants = txtcommentforapplicants.Text.ToString();
                string Comment_For_Internal_Use = txtcommentforinternaluse.Text.ToString();
                string Created_By = m_systemuser;
                DateTime Created_Date = DateTime.Now;
                string sql = "";

                string tempsql = "SELECT Screening_Comments_ID ,Application_Number ,Programme_ID ,Validation_Result ,Comment_For_Applicants ,Comment_For_Internal_Use ,Created_By ,Created_Date FROM [TB_SCREENING_HISTORY] WHERE Application_Number = @ApplicationNumber AND Programme_ID=@Programme_ID and (Validation_Result='Eligibility checked' or Validation_Result='Waiting for response from applicant' or Validation_Result='To be disqualified')";
                using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
                {
                    cmdscore.Parameters.Add(new SqlParameter("@ApplicationNumber", m_ApplicationNumber));
                    cmdscore.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    conn.Open();
                    try
                    {

                        var reader = cmdscore.ExecuteReader();
                        while (reader.Read())
                        {
                            m_insert_Update = reader.GetValue(1).ToString();
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                var parameters = new List<SqlParameter>();
                    sql = "insert into TB_SCREENING_HISTORY(Application_Number,Programme_ID,Validation_Result,Comment_For_Applicants,Comment_For_Internal_Use,Created_By,Created_Date) VALUES ("
                    + "@Application_Number , "
                    + "@Programme_ID , "
                    + "@Validation_Results , "
                    + "@Comment_For_Applicants , "
                    + "@Comment_For_Internal_Use , "
                    + "@Created_By , "
                    + "GETDATE()"
                    + ")";
                    parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                    parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    parameters.Add(new SqlParameter("@Validation_Results", Validation_Results));
                    parameters.Add(new SqlParameter("@Comment_For_Applicants", Comment_For_Applicants));
                    parameters.Add(new SqlParameter("@Comment_For_Internal_Use", Comment_For_Internal_Use));
                    parameters.Add(new SqlParameter("@Created_By", Created_By));



                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.ExecuteNonQuery();

                    //updateStatus();
                    updateStatus(Validation_Results);

                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

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
        }

        protected void updateStatus(string valResult)
        {
            string m_ApplicationID_temp = m_ApplicationID;
            m_ApplicationID = m_ApplicationNumber;
            //txtcommentforapplicants.Text = "updateStatus" + m_ApplicationID + "/";

            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);


            foreach (SPListItem oListItem in collListItems)
            {

                
               //oListItem["Status"] = Validation_Results_SP;
                oListItem["Status"] = valResult;
                oListItem["Coordinator_Comment_for_applicants"] = txtcommentforapplicants.Text.ToString();
                oListItem["Coordinator_Comment_for_internal_use"] = txtcommentforinternaluse.Text.ToString();

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;

                //;

            }

            string sql = "";
            SqlConnection conn = new SqlConnection(connStr);

            if (m_CCMF != 0)
                sql = "UPDATE TB_CCMF_APPLICATION SET Status = @Validation_Results_SP , Modified_Date = GETDATE() , Modified_By = @user WHERE CCMF_ID = @ApplicationID_temp";
            else
                sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = @Validation_Results_SP , Modified_Date = GETDATE() , Modified_By = @user WHERE Incubation_ID = @ApplicationID_temp";
            
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@Validation_Results_SP", valResult));
            cmd.Parameters.Add(new SqlParameter("@user", m_systemuser));
            cmd.Parameters.Add(new SqlParameter("@ApplicationID_temp", m_ApplicationID_temp));
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
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

        protected void rbtnresult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnresult.SelectedValue.ToString() == "Waiting for response from applicant")
            {
                RequiredFieldValidator.Enabled = true;
                RequiredFieldValidator.Visible = true;
            }
            else
            {
                RequiredFieldValidator.Enabled = false;
                RequiredFieldValidator.IsValid = true;
                RequiredFieldValidator.Visible = false;
            }
        }

        protected void getProgrmNameIntakeNumByProgID()
        {
            ConnectOpen();
            try
            {
                var sqlString = "select Programme_Name,Intake_Number from TB_PROGRAMME_INTAKE where Programme_ID=@progid;";

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
        protected void getSYSTEMPARAMETER()
        {
            ConnectOpen();
            try { 
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
                SPSecurity.RunWithElevatedPrivileges(delegate()
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
                            InsertTBDownloadZIP(m_username, m_mail, "ZIP", zipFile, FileName, m_Password, "1");

                            //Completed email  

                            m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
                            //m_body = "Hi, Zip File is ready, please download : " + m_downloadlink + " . <br/>Password is : " + m_Password + " <br/>";
                            //m_body += "Zip Start time : " + m_Starttime + " to End Time :" + m_Endtime + "";
                            m_body = getEmailTemplate("ZipDownloadEndEmail");
                            m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_Application_Number", m_ApplicationNumber).Replace("@@m_FileName", FileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                            sharepointsendemail(m_mail, m_subject, m_body);
                            //sharepointsendemail("andysgi@gmail.com", "hi", "ko");
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
                    //+ "'" + User_Name + "', "
                    //+ "'" + Email + "', "
                    //+ "'" + type + "', "
                    //+ "'" + Path + "', "
                    //+ "'" + File_Name + "', "
                    //+ "'" + Password + "', "
                    //+ "'" + Status + "', "
                    //+ "'" + SPContext.Current.Web.CurrentUser.Name.ToString() + "', "
                    //+ "GETDATE(), "
                    //+ "'" + SPContext.Current.Web.CurrentUser.Name.ToString() + "', "
                    //+ "GETDATE() "
                                    + "@User_Name , "
                                    + "@Email , "
                                    + "@type , "
                                    + "@Path , "
                                    + "@File_Name, "
                                    + "@Password , "
                                    + "@Status, "
                                    + "@CurrentUser, "
                                    + "GETDATE(), "
                                    + "@CurrentUser, "
                                    + "GETDATE() "
                                    + " ) ;";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add("@User_Name", User_Name);
                command.Parameters.Add("@Email", Email);
                command.Parameters.Add("@type", type);
                command.Parameters.Add("@Path", Path);
                command.Parameters.Add("@File_Name", File_Name);
                command.Parameters.Add("@Password", Password);
                command.Parameters.Add("@Status", Status);
                command.Parameters.Add("@CurrentUser", SPContext.Current.Web.CurrentUser.Name.ToString());
                command.ExecuteNonQuery();

                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void RepeaterHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label m_lblCommentforApplicationts = (Label)e.Item.FindControl("lblCommentforApplicationts");
            Label m_lblCommentforApplicationtsLabel = (Label)e.Item.FindControl("lblCommentforApplicationtsLabel");
            Label m_lblCommentforInternualUse = (Label)e.Item.FindControl("lblCommentforInternualUse");
            Label m_lblCommentforInternualUseLabel = (Label)e.Item.FindControl("lblCommentforInternualUseLabel");


            if (String.IsNullOrEmpty(m_lblCommentforApplicationts.Text))
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
