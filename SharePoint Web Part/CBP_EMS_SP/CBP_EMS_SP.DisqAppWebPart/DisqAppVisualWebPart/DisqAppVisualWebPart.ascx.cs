using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace CBP_EMS_SP.DisqAppWebPart.DisqAppVisualWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class DisqAppVisualWebPart : WebPart
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
        private String m_DBData_Role;
        private string m_checkRole = "";
        private String m_insert_Update = "";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public DisqAppVisualWebPart()
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

            m_progid = HttpContext.Current.Request.QueryString["Prog"];
            m_ApplicationID = HttpContext.Current.Request.QueryString["App"];

            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User

            m_DBData_Role = checkrole(m_systemuser);
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @ApplicationID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
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

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
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

                if (Convert.ToInt32(m_INCUBATION) != 0)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @ApplicationID", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                        conn.Open();
                        try { 
                        //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                        m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                    //txtcommentforapplicants.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @ApplicationID", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
                        conn.Open();
                        try { 
                        //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                        m_Status = cmd.ExecuteScalar().ToString();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }

                }
                else if (Convert.ToInt32(m_CCMF) != 0)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
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
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID", conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationID));
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

            AccessControl();


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
                if (m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw")
                {
                    MainPanel.Visible = false;
                }
                else
                {
                    MainPanel.Visible = true;
                }

            }
            else if (m_Role == "CPIP BDM")
            {
                if (m_Status == "BDM Reviewed" || m_Status == "Sr. Mgr. Reviewed" || m_Status == "CPMO Reviewed" || m_Status == "Complete Screening" || m_Status == "Withdraw")
                {
                    MainPanel.Visible = false;
                }
                else
                {
                    MainPanel.Visible = true;
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
            else if (m_Status == "Coordinator Reviewed")
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
                BtnSubmit.Enabled = true;
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
                BtnSubmit.Enabled = true;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
            }
            else if (m_Status == "Complete Screening")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            
        }

        protected void getdbdata()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                //string tempsql = "SELECT CCMF_Scoring_ID , Application_Number , Programme_ID , Reviewer , Role , Management_Team , Business_Model , Creativity , Social_Responsibility , Total_Score , Comments , Remarks , Created_By , Created_Date , Modified_By , Modified_Date FROM [TB_SCREENING_CCMF_SCORE] WHERE Application_Number = '" + m_ApplicationNumber + "' AND Role='" + m_Role + "'";
                string tempsql = "SELECT [Screening_Comments_ID],[Application_Number],[Programme_ID],[Validation_Result],[Comment_For_Applicants],[Comment_For_Internal_Use],[Created_By],[Created_Date] FROM [TB_SCREENING_HISTORY] "
                + "where Programme_ID=@progid and Application_Number=@ApplicationID and Validation_Result='Disqualified' and Created_By like '%BDM%'";

                using (SqlCommand cmd = new SqlCommand(tempsql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@progid", m_progid));
                    cmd.Parameters.Add(new SqlParameter("@ApplicationID", m_ApplicationNumber));
                    conn.Open();
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            txtcomment.Text = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();
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

            if (HttpContext.Current.Request.QueryString["testrole"] != null)
            {
                m_Role = HttpContext.Current.Request.QueryString["testrole"];
                //lblrole.Text = m_Role;
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {

           // var confirmResult = MessageBox.Show("Disqualify Application?", "????", MessageBoxButtons.YesNo);
            
            //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            //string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            try
            {

                string Application_Number = m_ApplicationNumber;
                string Programme_ID = m_progid;
                string Validation_Results = "Disqualified";
                string Comment_For_Applicants = "";
                string Comment_For_Internal_Use = txtcomment.Text.ToString();
                string Created_By = m_systemuser;
                DateTime Created_Date = DateTime.Now;
                string sql = "";

                string tempsql = "SELECT Screening_Comments_ID ,Application_Number ,Programme_ID ,Validation_Result ,Comment_For_Applicants ,Comment_For_Internal_Use ,Created_By ,Created_Date "
                               + "FROM [TB_SCREENING_HISTORY] WHERE Application_Number = @ApplicationNumber AND Programme_ID=@Programme_ID and Validation_Result=@Validation_Results and Created_By like '%BDM%'";
                using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
                {
                    cmdscore.Parameters.Add(new SqlParameter("@ApplicationNumber", m_ApplicationNumber));
                    cmdscore.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    cmdscore.Parameters.Add(new SqlParameter("@Validation_Results", Validation_Results));
                    //conn.Open();
                    //try
                    //{
                        var reader = cmdscore.ExecuteReader();
                        while (reader.Read())
                        {
                            m_insert_Update = reader.GetValue(1).ToString();
                        }
                    //}
                    //finally
                    //{
                    //    conn.Close();
                    //}
                }
                var parameters = new List<SqlParameter>();

                if (m_insert_Update != "")
                {
                    sql = "UPDATE TB_SCREENING_HISTORY SET "
                        + "Validation_Result=@Validation_Results , "
                        + "Comment_For_Applicants=@Comment_For_Applicants , "
                        + "Comment_For_Internal_Use=@Comment_For_Internal_Use  "
                        + "WHERE Application_Number = @Application_Number  "
                        + "AND Programme_ID=@Programme_ID and Validation_Result=@Validation_Results and Created_By like '%BDM%'";

                    parameters.Add(new SqlParameter("@Validation_Results", Validation_Results ));
                    parameters.Add(new SqlParameter("@Comment_For_Applicants" , Comment_For_Applicants ));
                    parameters.Add(new SqlParameter("@Comment_For_Internal_Use" , Comment_For_Internal_Use ));
                    parameters.Add(new SqlParameter("@Application_Number" , Application_Number ));
                    parameters.Add(new SqlParameter("@Programme_ID" , Programme_ID ));
                }
                else 
                {
                    sql = "insert into TB_SCREENING_HISTORY(Application_Number,Programme_ID,Validation_Result,Comment_For_Applicants,Comment_For_Internal_Use,Created_By,Created_Date) VALUES ("
                        + "'" + Application_Number + "' , "
                        + "'" + Programme_ID + "' , "
                        + "'" + Validation_Results + "' , "
                        + "'" + Comment_For_Applicants + "' , "
                        + "'" + Comment_For_Internal_Use + "' , "
                        + "'" + Created_By + "' , "
                        + "GETDATE()"
                        + ")";
                    parameters.Add(new SqlParameter("@Validation_Results", Validation_Results));
                    parameters.Add(new SqlParameter("@Comment_For_Applicants", Comment_For_Applicants));
                    parameters.Add(new SqlParameter("@Comment_For_Internal_Use", Comment_For_Internal_Use));
                    parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                    parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    parameters.Add(new SqlParameter("@Created_By", Created_By));
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();

                updateStatus();

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
            finally
            {

                if (conn != null)
                {
                    conn.Close();
                }
            }
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
            string status_temp = "";
            foreach (SPListItem oListItem in collListItems)
            {
                status_temp = "Disqualified";
                oListItem["Status"] = "Disqualified";
                oListItem["BDM_Comment_for_internal_use"] = txtcomment.Text.ToString(); //For SharePoint Application List Testing Field

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }

            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";

            sql = "UPDATE TB_CCMF_APPLICATION SET Status = @status_temp WHERE CCMF_ID = @ApplicationID_temp ";
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@status_temp", status_temp));
                cmd.Parameters.Add(new SqlParameter("@ApplicationID_temp", m_ApplicationID_temp));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = @status_temp WHERE Incubation_ID = @ApplicationID_temp ";
            conn.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.Parameters.Add(new SqlParameter("@status_temp", status_temp));
                cmd1.Parameters.Add(new SqlParameter("@ApplicationID_temp", m_ApplicationID_temp));
                cmd1.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
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
    }
}
