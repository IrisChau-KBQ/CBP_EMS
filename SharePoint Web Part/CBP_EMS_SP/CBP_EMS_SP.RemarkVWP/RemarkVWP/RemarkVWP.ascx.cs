using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using System;
using System.Data;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.Specialized;
namespace CBP_EMS_SP.RemarkVWP.RemarkVWP
{
    [ToolboxItemAttribute(false)]
    public partial class RemarkVWP : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        private String m_progid;
        private String m_ApplicationID;
        private String m_systemuser;
        private String m_Role;
        private int m_CCMF;
        private int m_INCUBATION;
        private String m_ApplicationNumber;
        private String m_Status;
        private String m_insert_Update = "";
        public String m_path;
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "";

        //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        private SqlConnection connection;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public RemarkVWP()
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
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User

            m_progid = HttpContext.Current.Request.QueryString["Prog"];
            m_ApplicationID = HttpContext.Current.Request.QueryString["App"];

            // Convert application ID to application name
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @m_ApplicationID ", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
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

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @m_ApplicationID ", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
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



                //if (get_role().ToString().Trim() == "CPIP BDM" && Convert.ToInt32(m_INCUBATION) != 0)
                //{
                //    CPIPScorePanel.Visible = true;
                //    MainPanel.Visible = true;
                //}
                //else if (get_role().ToString().Trim() == "CCMF BDM" && Convert.ToInt32(m_CCMF) != 0)
                //{
                //    CPIPScorePanel.Visible = false;
                //    MainPanel.Visible = true;
                //}
                //else 
                //{
                //    CCMFScorePanel.Visible = false;
                //    CPIPScorePanel.Visible = false;
                //}


                //lbltest.Text = "afasdfsfsa|" + m_INCUBATION + "CCMF" + m_CCMF;
                CCMFScorePanel.Visible = false;
                CPIPScorePanel.Visible = false;

                if (Convert.ToInt32(m_INCUBATION) != 0)
                {
                    CPIPScorePanel.Visible = true;

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @m_ApplicationID ", conn))
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

                    CPIPGridViewScore.Visible = true;
                    CCMFGridViewScore.Visible = false;
                }
                else if (Convert.ToInt32(m_CCMF) != 0)
                {
                    CCMFScorePanel.Visible = true;

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
                    //txtcommentforinternaluse.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;
                    CPIPGridViewScore.Visible = false;
                    CCMFGridViewScore.Visible = true;
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

            checkvettingteam();

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
                //if (m_Status == "CPMO Reviewed" && Convert.ToInt32(m_CCMF) != 0)
                if (m_CCMF > 0)
                {
                    if (m_Status == "CPMO Reviewed")
                    {
                        MainPanel.Visible = true;
                    }
                    else
                    {
                        MainPanel.Visible = false;
                    }
                }
                else
                {
                    MainPanel.Visible = false;
                }
            }
            else if (m_Role == "CPIP BDM")
            {
                //if (m_Status == "CPMO Reviewed" && Convert.ToInt32(m_INCUBATION) != 0)
                if (m_INCUBATION > 0)
                {
                    if (m_Status == "CPMO Reviewed")
                    {
                        MainPanel.Visible = true;
                    }
                    else
                    {
                        MainPanel.Visible = false;
                    }
                }
                else
                {
                    MainPanel.Visible = false;
                }
            }
            else
                MainPanel.Visible = false;
            //else if (m_Role == "Senior Manager")
            //{
            //    MainPanel.Visible = false;
            //    //MainPanel.Visible = true;// for testing 
            //}
            //else if (m_Role == "CPMO")
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
                BtnSubmit.Enabled = false;
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
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "Disqualified")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
            }
            else if (m_Status == "BDM Reviewed")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Rejected")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Sr. Mgr. Reviewed")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "CPMO Reviewed")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
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

        protected void CCMFScoreTable()
        {
            string[] ScoreTitlearr = new string[] { "Management Team (30%)", 
                                                    "Business Model & Time to Market (30%)", 
                                                    "Creativty and Innovation of the Proposed Project , Product and Service (30%)", 
                                                    "Social Responsibility (10%)", 
                                                    "Total Score", 
                                                    "Comments",
                                                    "Remarks"};

            string[] tempScore = new string[10];
            tempScore[0] = "";
            tempScore[1] = "";
            tempScore[2] = "";
            int z = 0;

            List<ScoreList> lstScore = new List<ScoreList>();
            ScoreList Slist = new ScoreList();

            List<ScoreList> lstScoreTotal = new List<ScoreList>();
            ScoreList SlistTotal = new ScoreList();

            string listscoresql = "";
            Double suntemp = 0;


            for (int i = 0; i <= 6; i++)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    listscoresql = "SELECT CCMF_Scoring_ID ,Application_Number ,Programme_ID ,Reviewer ,Role ,Management_Team ,Business_Model ,Creativity ,Social_Responsibility ,Total_Score ,Comments ,Remarks ,Created_By ,Created_Date ,Modified_By ,Modified_Date FROM TB_SCREENING_CCMF_SCORE "
                    + "where Programme_ID=@m_progid and Application_Number=@m_ApplicationNumber";

                    using (SqlCommand cmd = new SqlCommand(listscoresql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                        z = 0;
                        conn.Open();
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader.GetValue(reader.GetOrdinal("Role")).ToString().Trim() == "CPMO")
                                {
                                    z = 2;
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.3;
                                    lblCPMO_lstMT.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Model"))) / 0.3;
                                    lblCPMO_lstBMTM.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.3;
                                    lblCPMO_lstCIPP_CCMF.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Social_Responsibility"))) / 0.1;
                                    lblCPMO_lstSR.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Total_Score")));
                                    lblCPMO_TotalScore_CCMF.Text = suntemp.ToString("N3");
                                    //lblCPMO_TotalScore_CCMF.Text = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString().Trim();
                                    lblCPMO_Comments_CCMF.Text = reader.GetValue(reader.GetOrdinal("Comments")).ToString().Trim();
                                }
                                else if (reader.GetValue(reader.GetOrdinal("Role")).ToString().Trim() == "Senior Manager")
                                {
                                    z = 1;
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.3;
                                    lblSrMagr_lstMT.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Model"))) / 0.3;
                                    lblSrMagr_lstBMTM.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.3;
                                    lblSrMagr_lstCIPP_CCMF.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Social_Responsibility"))) / 0.1;
                                    lblSrMagr_lstSR.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Total_Score")));
                                    lblSrMagr_TotalScore_CCMF.Text = suntemp.ToString("N3");
                                    //lblSrMagr_TotalScore_CCMF.Text = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString().Trim();
                                    lblSrMagr_Comments_CCMF.Text = reader.GetValue(reader.GetOrdinal("Comments")).ToString().Trim();
                                }
                                else if (reader.GetValue(reader.GetOrdinal("Role")).ToString().Trim() == "CCMF BDM")
                                {
                                    z = 0;
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.3;
                                    lstMT.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Model"))) / 0.3;
                                    lstBMTM.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.3;
                                    lstCIPP_CCMF.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Social_Responsibility"))) / 0.1;
                                    lstSR.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Total_Score")));
                                    lblBDM_TotalScore_CCMF.Text = suntemp.ToString("N3");
                                    //lblBDM_TotalScore_CCMF.Text = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString().Trim();
                                    lstBDM_Comments_CCMF.SelectedValue = reader.GetValue(reader.GetOrdinal("Comments")).ToString().Trim();
                                }


                                if ((i + 5) == reader.GetOrdinal("Management_Team"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.3;
                                    tempScore[z] = suntemp.ToString();


                                }
                                else if ((i + 5) == reader.GetOrdinal("Business_Model"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Model"))) / 0.3;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else if ((i + 5) == reader.GetOrdinal("Creativity"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.3;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else if ((i + 5) == reader.GetOrdinal("Social_Responsibility"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Social_Responsibility"))) / 0.1;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else
                                {
                                    tempScore[z] = reader.GetValue(i + 5).ToString();
                                }
                                z++;
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }

                
                lstScore.Add(new ScoreList
                {
                    ScoreTitle = ScoreTitlearr[i],
                    ScoreBDM = Convert.ToString(tempScore[0]),
                    ScoreSrMagr = Convert.ToString(tempScore[1]),
                    ScoreCPMO = Convert.ToString(tempScore[2])
                });
            }

            //average score
            var count = (double.Parse(lstMT.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstMT.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_lstMT.Text) == 0 ? 0 : 1);
            lblAverage_lstMT.Text = ((double.Parse(lstMT.SelectedValue) + double.Parse(lblSrMagr_lstMT.Text) + double.Parse(lblCPMO_lstMT.Text)) / count).ToString("N3");

            count = (double.Parse(lstBMTM.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstBMTM.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_lstBMTM.Text) == 0 ? 0 : 1);
            lblAverage_lstBMTM.Text = ((double.Parse(lstBMTM.SelectedValue) + double.Parse(lblSrMagr_lstBMTM.Text) + double.Parse(lblCPMO_lstBMTM.Text)) / count).ToString("N3");

            count = (double.Parse(lstCIPP_CCMF.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstCIPP_CCMF.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_lstCIPP_CCMF.Text) == 0 ? 0 : 1);
            lblAverage_lstCIPP_CCMF.Text = ((double.Parse(lstCIPP_CCMF.SelectedValue) + double.Parse(lblSrMagr_lstCIPP_CCMF.Text) + double.Parse(lblCPMO_lstCIPP_CCMF.Text)) / count).ToString("N3");

            count = (double.Parse(lstSR.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstSR.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_lstSR.Text) == 0 ? 0 : 1);
            lblAverage_lstSR.Text = ((double.Parse(lstSR.SelectedValue) + double.Parse(lblSrMagr_lstSR.Text) + double.Parse(lblCPMO_lstSR.Text)) / count).ToString("N3");

            count = (double.Parse(lblBDM_TotalScore_CCMF.Text) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_TotalScore_CCMF.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_TotalScore_CCMF.Text) == 0 ? 0 : 1);
            lblAverage_TotalScore_CCMF.Text = ((double.Parse(lblBDM_TotalScore_CCMF.Text) + double.Parse(lblSrMagr_TotalScore_CCMF.Text) + double.Parse(lblCPMO_TotalScore_CCMF.Text)) / count).ToString("N3");


            CCMFGridViewScore.DataSource = lstScore;
            CCMFGridViewScore.DataBind();
        }

        protected void CPIPScoreTable()
        {
            string[] ScoreTitlearr = new string[] { "Quality and competence of the management team (20%)", 
                                                    "Creativity and innovation of the proposed project, product and service (20%)", 
                                                    "Market and business viability (30%)", 
                                                    "Benefit to Hong Kong’s digital tech industry (10%)", 
                                                    "Proposed six-monthly milestones for the project or business after admission (20%)", 
                                                    "Total Score", 
                                                    "Comments",
                                                    "Remarks"};

            string[] tempScore = new string[10];
            tempScore[0] = "";
            tempScore[1] = "";
            tempScore[2] = "";
            int z = 0;

            List<ScoreList> lstScore = new List<ScoreList>();
            ScoreList Slist = new ScoreList();

            List<ScoreList> lstScoreTotal = new List<ScoreList>();
            ScoreList SlistTotal = new ScoreList();

            string listscoresql = "";
            Double suntemp = 0;

            for (int i = 0; i <= 6; i++)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    //listscoresql = "SELECT CCMF_Scoring_ID ,Application_Number ,Programme_ID ,Reviewer ,Role ,Management_Team ,Business_Model ,Creativity ,Social_Responsibility ,Total_Score ,Comments ,Remarks ,Created_By ,Created_Date ,Modified_By ,Modified_Date FROM TB_SCREENING_CCMF_SCORE "
                    //+ "where Programme_ID='" + m_progid + "' and Application_Number='" + m_ApplicationNumber + "'";                    
                    listscoresql = "SELECT Incubation_Scoring_ID,Application_Number,Programme_ID,Reviewer,Role,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date FROM TB_SCREENING_INCUBATION_SCORE "
                    + "where Programme_ID=@m_progid and Application_Number=@m_ApplicationNumber ";

                    using (SqlCommand cmd = new SqlCommand(listscoresql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                        z = 0;
                        conn.Open();
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader.GetValue(reader.GetOrdinal("Role")).ToString().Trim() == "CPMO")
                                {
                                    z = 2;
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.2;
                                    lblCPMO_lstqcmt.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.2;
                                    lblCPMP_lstcipp.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Viability"))) / 0.3;
                                    lblCPMP_lstmbv.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Benefit_To_Industry"))) / 0.1;
                                    lblCPMP_lstbhkdti.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Proposal_Milestones"))) / 0.2;
                                    lblCPMP_lstpsmpb.Text = suntemp.ToString("N1");
                                    lblCPMO_TotalScore_CPIP.Text = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString().Trim();
                                    lblCPMO_Comments_CPIP.Text = reader.GetValue(reader.GetOrdinal("Comments")).ToString().Trim();
                                }
                                else if (reader.GetValue(reader.GetOrdinal("Role")).ToString().Trim() == "Senior Manager")
                                {
                                    z = 1;
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.2;
                                    lblSrMagr_lstqcmt.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.2;
                                    lblSrMagr_lstcipp.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Viability"))) / 0.3;
                                    lblSrMagr_lstmbv.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Benefit_To_Industry"))) / 0.1;
                                    lblSrMagr_lstbhkdti.Text = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Proposal_Milestones"))) / 0.2;
                                    lblSrMagr_lstpsmpb.Text = suntemp.ToString("N1");
                                    lblSrMagr_TotalScore_CPIP.Text = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString().Trim();
                                    lblSrMagr_Comments_CPIP.Text = reader.GetValue(reader.GetOrdinal("Comments")).ToString().Trim();
                                }
                                else if (reader.GetValue(reader.GetOrdinal("Role")).ToString().Trim().ToUpper() == "CPIP BDM")
                                {
                                    z = 0;
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.2;
                                    lstqcmt.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.2;
                                    lstcipp.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Viability"))) / 0.3;
                                    lstmbv.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Benefit_To_Industry"))) / 0.1;
                                    lstbhkdti.SelectedValue = suntemp.ToString("N1");
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Proposal_Milestones"))) / 0.2;
                                    lstpsmpb.SelectedValue = suntemp.ToString("N1");
                                    lblBDM_TotalScore_CPIP.Text = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString().Trim();
                                    lstBDM_Comments_CPIP.SelectedValue = reader.GetValue(reader.GetOrdinal("Comments")).ToString().Trim();
                                }



                                if ((i + 5) == reader.GetOrdinal("Management_Team"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Management_Team"))) / 0.2;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else if ((i + 5) == reader.GetOrdinal("Creativity"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Creativity"))) / 0.2;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else if ((i + 5) == reader.GetOrdinal("Business_Viability"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Business_Viability"))) / 0.3;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else if ((i + 5) == reader.GetOrdinal("Benefit_To_Industry"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Benefit_To_Industry"))) / 0.1;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else if ((i + 5) == reader.GetOrdinal("Proposal_Milestones"))
                                {
                                    suntemp = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("Proposal_Milestones"))) / 0.2;
                                    tempScore[z] = suntemp.ToString();
                                }
                                else
                                {
                                    tempScore[z] = reader.GetValue(i + 5).ToString();
                                }
                                z++;
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }


                if (string.IsNullOrEmpty(ScoreTitlearr[i])) { }

                if (i <= 4)
                {

                    lstScore.Add(new ScoreList
                    {
                        ScoreTitle = ScoreTitlearr[i],
                        ScoreBDM = Convert.ToString(tempScore[0]),
                        ScoreSrMagr = Convert.ToString(tempScore[1]),
                        ScoreCPMO = Convert.ToString(tempScore[2])
                    });
                }
                else
                {
                    lstScoreTotal.Add(new ScoreList
                    {
                        ScoreTitle = ScoreTitlearr[i],
                        ScoreBDM = Convert.ToString(tempScore[0]),
                        ScoreSrMagr = Convert.ToString(tempScore[1]),
                        ScoreCPMO = Convert.ToString(tempScore[2])
                    });

                    //                    rbtnresult.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString().Trim()).Selected = true;
                }

                if (ScoreTitlearr[i] == "Total Score")
                {
                    lblBDMTotal.Text = Convert.ToString(tempScore[0]);
                    lblSrMgrTotal.Text = Convert.ToString(tempScore[1]);
                    lblCPMOTotal.Text = Convert.ToString(tempScore[2]);
                }
                else if (ScoreTitlearr[i] == "Comments")
                {
                    lblBDMComments.Text = Convert.ToString(tempScore[0]);
                    lblSrMgrComments.Text = Convert.ToString(tempScore[1]);
                    lblCPMOComments.Text = Convert.ToString(tempScore[2]);
                }

            }

            //average score
            var count = (double.Parse(lstqcmt.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstqcmt.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_lstqcmt.Text) == 0 ? 0 : 1);
            lblAverage_lstqcmt.Text = ((double.Parse(lstqcmt.SelectedValue) + double.Parse(lblSrMagr_lstqcmt.Text) + double.Parse(lblCPMO_lstqcmt.Text)) / count).ToString("N1");

            count = (double.Parse(lstcipp.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstcipp.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMP_lstcipp.Text) == 0 ? 0 : 1);
            lblAverage_lstcipp.Text = ((double.Parse(lstcipp.SelectedValue) + double.Parse(lblSrMagr_lstcipp.Text) + double.Parse(lblCPMP_lstcipp.Text)) / count).ToString("N1");

            count = (double.Parse(lstmbv.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstmbv.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMP_lstmbv.Text) == 0 ? 0 : 1);
            lblAverage_lstmbv.Text = ((double.Parse(lstmbv.SelectedValue) + double.Parse(lblSrMagr_lstmbv.Text) + double.Parse(lblCPMP_lstmbv.Text)) / count).ToString("N1");

            count = (double.Parse(lstbhkdti.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstbhkdti.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMP_lstbhkdti.Text) == 0 ? 0 : 1);
            lblAverage_lstbhkdti.Text = ((double.Parse(lstbhkdti.SelectedValue) + double.Parse(lblSrMagr_lstbhkdti.Text) + double.Parse(lblCPMP_lstbhkdti.Text)) / count).ToString("N1");

            count = (double.Parse(lstpsmpb.SelectedValue) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_lstpsmpb.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMP_lstpsmpb.Text) == 0 ? 0 : 1);
            lblAverage_lstpsmpb.Text = ((double.Parse(lstpsmpb.SelectedValue) + double.Parse(lblSrMagr_lstpsmpb.Text) + double.Parse(lblCPMP_lstpsmpb.Text)) / count).ToString("N1");

            count = (double.Parse(lblBDM_TotalScore_CPIP.Text) == 0 ? 0 : 1) + (double.Parse(lblSrMagr_TotalScore_CPIP.Text) == 0 ? 0 : 1) + (double.Parse(lblCPMO_TotalScore_CPIP.Text) == 0 ? 0 : 1);
            lblAverage_TotalScore_CPIP.Text = ((double.Parse(lblBDM_TotalScore_CPIP.Text) + double.Parse(lblSrMagr_TotalScore_CPIP.Text) + double.Parse(lblCPMO_TotalScore_CPIP.Text)) / count).ToString("N3");


            CPIPGridViewScore.DataSource = lstScore;
            CPIPGridViewScore.DataBind();

            CPIPGridViewScoreTotal.DataSource = lstScoreTotal;
            CPIPGridViewScoreTotal.DataBind();
        }

        protected void getdbdata()
        {

            CCMFScoreTable();
            CPIPScoreTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                remark.Text = "";
                string sql = "";

                //                SELECT  where Application_Number='CCFM-201704-0006' and Programme_ID='18'
                string tempsql = "SELECT Application_Shortlisting_ID ,Application_Number ,Programme_ID ,Remarks_To_Vetting ,Shortlisted ,Created_By ,Created_Date ,Modified_By ,Modified_Date FROM TB_APPLICATION_SHORTLISTING WHERE Application_Number = @m_ApplicationNumber  AND Programme_ID=@m_progid ";

                using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
                {
                    cmdscore.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                    cmdscore.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                    conn.Open();
                    try
                    {
                        var reader = cmdscore.ExecuteReader();
                        while (reader.Read())
                        {
                            m_insert_Update = reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString();
                            //rbtnresult.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString()).Selected = true;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }

                }

                if (!string.IsNullOrEmpty(m_insert_Update.Trim()))
                {
                    remark.Text += m_insert_Update;
                    //lbltest.Text = "[" + m_insert_Update.ToString() + "]";
                }
                else
                {

                    string scoresql = "";

                    scoresql = "SELECT CCMF_Scoring_ID ,Application_Number ,Programme_ID ,Reviewer ,Role ,Management_Team ,Business_Model ,Creativity ,Social_Responsibility ,Total_Score ,Comments ,isnull(Remarks,'') as Remarks,Created_By ,Created_Date ,Modified_By ,Modified_Date FROM TB_SCREENING_CCMF_SCORE "
                    + "where Programme_ID=@m_progid and Application_Number=@m_ApplicationNumber ";
                    //lbltest.Text = "scoresql[" + scoresql + "]";

                    using (SqlCommand cmd = new SqlCommand(scoresql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                        conn.Open();
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader.GetValue(reader.GetOrdinal("Remarks")).ToString() != "")
                                {
                                    remark.Text += reader.GetValue(reader.GetOrdinal("Role")).ToString() + " Remark : ";
                                    remark.Text += reader.GetValue(reader.GetOrdinal("Remarks")).ToString() + "\r\n";
                                }
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }

                    scoresql = "SELECT Incubation_Scoring_ID,Application_Number,Programme_ID,Reviewer,Role,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,isnull(Remarks,'') as Remarks,Created_By,Created_Date,Modified_By,Modified_Date FROM TB_SCREENING_INCUBATION_SCORE "
                    + "where Programme_ID=@m_progid and Application_Number=@m_ApplicationNumber ";

                    using (SqlCommand cmd = new SqlCommand(scoresql, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_progid", m_progid));
                        cmd.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                        conn.Open();
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader.GetValue(reader.GetOrdinal("Remarks")).ToString() != "")
                                {
                                    remark.Text += reader.GetValue(reader.GetOrdinal("Role")).ToString() + " Remark : ";
                                    remark.Text += reader.GetValue(reader.GetOrdinal("Remarks")).ToString() + "\r\n";
                                }
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }

                }
                if (!remark.Text.Contains("Final BDM Remark"))
                    remark.Text += "Final BDM Remark : ";
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

            /*if (HttpContext.Current.Request.QueryString["testrole"] != null)
            {
                m_Role = HttpContext.Current.Request.QueryString["testrole"];
                //lblrole.Text = m_Role;
            }*/
        }

        protected void checkvettingteam() 
        {
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";

            string tempsql = "SELECT ApplicationId,RoleId,RoleName,LoweredRoleName,Description FROM aspnet_Roles where RoleName<>'Admin'";
            using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
            {   

                conn.Open();
                try
                {
                    var reader = cmdscore.ExecuteReader();
                    while (reader.Read())
                    {
                        //m_insert_Update = reader.GetValue(1).ToString();
                        if (m_Role.ToLower().Trim() == reader.GetValue(reader.GetOrdinal("RoleName")).ToString().ToLower().Trim())
                        {
                            MainPanel.Visible = false;
                            clearSessionCookies();
                        }
                    }
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        private void clearSessionCookies()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Clear();
                }

                int cookieCount = HttpContext.Current.Request.Cookies.Count;
                for (var i = 0; i < cookieCount; i++)
                {
                    var cookie = HttpContext.Current.Request.Cookies[i];
                    if (cookie != null)
                    {
                        var cookieName = cookie.Name;
                        var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                        HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();
            }

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            //string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                string Application_Number = m_ApplicationNumber;
                string Programme_ID = m_progid;
                string Remarks_To_Vetting = remark.Text.ToString().Trim();
                if (Remarks_To_Vetting.EndsWith("Final BDM Remark :"))
                {
                   Remarks_To_Vetting = Remarks_To_Vetting.Replace("Final BDM Remark :", "");
                }
                string Shortlisted = "1";
                string Created_By = m_systemuser;
                DateTime Created_Date = DateTime.Now;
                string Modified_By = m_systemuser;
                DateTime Modified_Date = DateTime.Now;

                string Reviewer = SPContext.Current.Web.CurrentUser.Name.ToString();
                double Management_Team = Convert.ToDouble(lstqcmt.SelectedValue) * 0.2;//scorepresent("lstMT_Num");                
                double Creativity = Convert.ToDouble(lstcipp.SelectedValue) * 0.2;//scorepresent("lstBMTM_Num");
                double Business_Viability = Convert.ToDouble(lstmbv.SelectedValue) * 0.3;//scorepresent("lstCIPP_Num");
                double Benefit_To_Industry = Convert.ToDouble(lstbhkdti.SelectedValue) * 0.1;// * scorepresent("lstSR_Num");
                double Proposal_Milestones = Convert.ToDouble(lstpsmpb.SelectedValue) * 0.2;// * scorepresent("lstpsmpb");
                double Total_Score = ((Management_Team) + (Creativity) + (Business_Viability) + (Benefit_To_Industry) + (Proposal_Milestones));

                string sql = "";

                string tempsql = "SELECT Application_Shortlisting_ID ,Application_Number ,Programme_ID ,Remarks_To_Vetting ,Shortlisted ,Created_By ,Created_Date ,Modified_By ,Modified_Date FROM TB_APPLICATION_SHORTLISTING TB_SCREENING_HISTORY WHERE Application_Number = @m_ApplicationNumber AND Programme_ID=@Programme_ID ";
                using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
                {
                    cmdscore.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    cmdscore.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));

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
                List<SqlParameter> parameters;

                parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(m_insert_Update))
                {
                    sql = "UPDATE TB_APPLICATION_SHORTLISTING SET "
                    + "Remarks_To_Vetting=@Remarks_To_Vetting, "
                    + "Modified_By=@Modified_By, "
                    + "Modified_Date = GETDATE()"
                    + "WHERE Application_Number = @Application_Number "
                    + "AND Programme_ID=@Programme_ID";

                    parameters.Add(new SqlParameter("@Remarks_To_Vetting", Remarks_To_Vetting));
                    parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                    parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                    parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                }
                else
                {

                    sql = "insert into TB_APPLICATION_SHORTLISTING(Application_Number,Programme_ID,Remarks_To_Vetting,Shortlisted,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                        + "@Application_Number , "
                        + "@Programme_ID , "
                        + "@Remarks_To_Vetting , "
                        + "@Shortlisted , "
                        + "@Created_By , "
                        + "GETDATE() , "
                        + "@Modified_By , "
                        + "GETDATE()"
                        + ")";
                    parameters.Add(new SqlParameter("@Remarks_To_Vetting", Remarks_To_Vetting));
                    parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                    parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                    parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    parameters.Add(new SqlParameter("@Shortlisted", Shortlisted));
                    parameters.Add(new SqlParameter("@Created_By", Created_By));
                }
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }

                var m_Programme_Type = checkProgrammeType(Programme_ID);
                if (m_Programme_Type == "CPIP")
                {
                    //CPIP
                    var sqlString = "select * from TB_SCREENING_INCUBATION_SCORE WHERE Application_Number = @m_ApplicationNumber AND Role=@m_Role ";
                    using (SqlCommand cmdscore = new SqlCommand(sqlString, conn))
                    {
                        cmdscore.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                        cmdscore.Parameters.Add(new SqlParameter("@m_Role", m_Role));

                        conn.Open();
                        try
                        {
                            var reader = cmdscore.ExecuteReader();
                            if (reader.Read())
                            {
                                //update CPIP
                                parameters = new List<SqlParameter>();
                                sql = "UPDATE TB_SCREENING_INCUBATION_SCORE SET "
                                   + "Reviewer=@Reviewer, "
                                   + "Management_Team=@Management_Team, "
                                   + "Creativity=@Creativity, "
                                   + "Business_Viability=@Business_Viability, "
                                   + "Benefit_To_Industry=@Benefit_To_Industry, "
                                   + "Proposal_Milestones=@Proposal_Milestones, "
                                   + "Total_Score=@Total_Score, "
                                   + "Comments=@Comments, "
                                   + "Modified_By=@Modified_By, "
                                   + "Modified_Date= GETDATE() "
                                   + "WHERE Application_Number = @m_ApplicationNumber "
                                   + "AND Role=@m_Role";
                                parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                                parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                                parameters.Add(new SqlParameter("@Creativity", Creativity));
                                parameters.Add(new SqlParameter("@Business_Viability", Business_Viability));
                                parameters.Add(new SqlParameter("@Benefit_To_Industry", Benefit_To_Industry));
                                parameters.Add(new SqlParameter("@Proposal_Milestones", Proposal_Milestones));
                                parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                                parameters.Add(new SqlParameter("@Comments", lstBDM_Comments_CPIP.SelectedValue));
                                parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                                parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                                parameters.Add(new SqlParameter("@m_Role", m_Role));

                                var cmd = new SqlCommand(sql, conn);
                                cmd.Parameters.AddRange(parameters.ToArray());
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                //insert CPIP
                                parameters = new List<SqlParameter>();
                                sql = "insert into TB_SCREENING_INCUBATION_SCORE(Application_Number,Programme_ID,Reviewer,Role,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,Created_By,Created_Date,Modified_By,Modified_Date) values( "
                                   + "@m_ApplicationNumber, "
                                   + "@Programme_ID, " 
                                   + "@Reviewer, "
                                   + "@m_Role, "
                                   + "@Management_Team, "
                                   + "@Creativity, "
                                   + "@Business_Viability, "
                                   + "@Benefit_To_Industry, "
                                   + "@Proposal_Milestones, "
                                   + "@Total_Score, "
                                   + "@Comments, "
                                   + "@Modified_By, "
                                   + "GETDATE(), "
                                   + "@Modified_By, "
                                   + "GETDATE() "
                                   + ")";
                                parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                                parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                                parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                                parameters.Add(new SqlParameter("@m_Role", m_Role));
                                parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                                parameters.Add(new SqlParameter("@Creativity", Creativity));
                                parameters.Add(new SqlParameter("@Business_Viability", Business_Viability));
                                parameters.Add(new SqlParameter("@Benefit_To_Industry", Benefit_To_Industry));
                                parameters.Add(new SqlParameter("@Proposal_Milestones", Proposal_Milestones));
                                parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                                parameters.Add(new SqlParameter("@Comments", lstBDM_Comments_CPIP.SelectedValue));
                                parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                                
                                

                                var cmd = new SqlCommand(sql, conn);
                                cmd.Parameters.AddRange(parameters.ToArray());
                                cmd.ExecuteNonQuery();
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }
                else
                {
                    //CCMF  
                    var sqlString = "select * from TB_SCREENING_CCMF_SCORE WHERE Application_Number = @m_ApplicationNumber AND Role=@m_Role ";
                    using (SqlCommand cmdscore = new SqlCommand(sqlString, conn))
                    {
                        cmdscore.Parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                        cmdscore.Parameters.Add(new SqlParameter("@m_Role", m_Role));

                        conn.Open();
                        try
                        {
                            var reader = cmdscore.ExecuteReader();

                            parameters = new List<SqlParameter>();
                            Management_Team = Convert.ToDouble(lstMT.SelectedValue) * 0.3;
                            double Business_Model = Convert.ToDouble(lstBMTM.SelectedValue) * 0.3;
                            Creativity = Convert.ToDouble(lstCIPP_CCMF.SelectedValue) * 0.3;
                            double Social_Responsibility = Convert.ToDouble(lstSR.SelectedValue) * 0.1;
                            //Total_Score = Convert.ToDouble(lblBDM_TotalScore_CCMF.Text);
                            Total_Score = ((Management_Team) + (Business_Model) + (Creativity) + (Social_Responsibility));

                            if (reader.Read())
                            {
                                // update CCMF
                                sql = "UPDATE TB_SCREENING_CCMF_SCORE SET "
                                     + "Reviewer=@Reviewer, "
                                     + "Management_Team=@Management_Team, "
                                     + "Business_Model=@Business_Model, "
                                     + "Creativity=@Creativity, "
                                     + "Social_Responsibility=@Social_Responsibility, "
                                     + "Total_Score=@Total_Score, "
                                     + "Comments=@Comments, "
                                     + "Modified_By=@Modified_By, "
                                     + "Modified_Date= GETDATE() "
                                     + "WHERE Application_Number = @m_ApplicationNumber "
                                     + "AND Role=@m_Role";
                                parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                                parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                                parameters.Add(new SqlParameter("@Creativity", Creativity));
                                parameters.Add(new SqlParameter("@Business_Model", Business_Model));
                                parameters.Add(new SqlParameter("@Social_Responsibility", Social_Responsibility));
                                parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                                parameters.Add(new SqlParameter("@Comments", lstBDM_Comments_CCMF.SelectedValue));
                                parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                                parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                                parameters.Add(new SqlParameter("@m_Role", m_Role));


                                var cmd = new SqlCommand(sql, conn);
                                cmd.Parameters.AddRange(parameters.ToArray());
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                //insert CPIP
                                parameters = new List<SqlParameter>();
                                sql = "insert into TB_SCREENING_CCMF_SCORE(Application_Number,Programme_ID,Reviewer,Role,Management_Team,Business_Model,Creativity,Social_Responsibility,Total_Score,Comments,Created_By,Created_Date,Modified_By,Modified_Date) values( "
                                   + "@m_ApplicationNumber, "
                                   + "@Programme_ID, "
                                   + "@Reviewer, "
                                   + "@m_Role, "
                                   + "@Management_Team, "
                                   + "@Business_Model, "
                                   + "@Creativity, "
                                   + "@Social_Responsibility, "
                                   + "@Total_Score, "
                                   + "@Comments, "
                                   + "@Modified_By, "
                                   + "GETDATE(), "
                                   + "@Modified_By, "
                                   + "GETDATE() "
                                   + ")";
                                parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                                parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                                parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                                parameters.Add(new SqlParameter("@m_Role", m_Role));
                                parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                                parameters.Add(new SqlParameter("@Business_Model", Business_Model));
                                parameters.Add(new SqlParameter("@Creativity", Creativity));
                                parameters.Add(new SqlParameter("@Social_Responsibility", Social_Responsibility));
                                parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                                parameters.Add(new SqlParameter("@Comments", lstBDM_Comments_CCMF.SelectedValue));
                                parameters.Add(new SqlParameter("@Modified_By", Modified_By));



                                var cmd = new SqlCommand(sql, conn);
                                cmd.Parameters.AddRange(parameters.ToArray());
                                cmd.ExecuteNonQuery();
                            }
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }

                #region
                //parameters = new List<SqlParameter>();
                //// update CPIP
                //sql = "UPDATE TB_SCREENING_INCUBATION_SCORE SET "
                //   + "Reviewer=@Reviewer, "
                //   + "Management_Team=@Management_Team, "
                //   + "Creativity=@Creativity, "
                //   + "Business_Viability=@Business_Viability, "
                //   + "Benefit_To_Industry=@Benefit_To_Industry, "
                //   + "Proposal_Milestones=@Proposal_Milestones, "
                //   + "Total_Score=@Total_Score, "
                //   + "Modified_By=@Modified_By, "
                //   + "Modified_Date= GETDATE() "
                //   + "WHERE Application_Number = @m_ApplicationNumber "
                //   + "AND Role=@m_Role";
                //parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                //parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                //parameters.Add(new SqlParameter("@Creativity", Creativity));
                //parameters.Add(new SqlParameter("@Business_Viability", Business_Viability));
                //parameters.Add(new SqlParameter("@Benefit_To_Industry", Benefit_To_Industry));
                //parameters.Add(new SqlParameter("@Proposal_Milestones", Proposal_Milestones));
                //parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                //parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                //parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                //parameters.Add(new SqlParameter("@m_Role", m_Role));
                //conn.Open();
                //try
                //{
                //    SqlCommand cmdBDMScore = new SqlCommand(sql, conn);
                //    cmdBDMScore.Parameters.AddRange(parameters.ToArray());
                //    cmdBDMScore.ExecuteNonQuery();
                //}
                //finally
                //{
                //    conn.Close();
                //}
                #endregion

                #region
                //parameters = new List<SqlParameter>();
                //Management_Team = Convert.ToDouble(lstMT.SelectedValue) * 0.3;
                //double Business_Model = Convert.ToDouble(lstBMTM.SelectedValue) * 0.3;
                //Creativity = Convert.ToDouble(lstCIPP_CCMF.SelectedValue) * 0.3;
                //double Social_Responsibility = Convert.ToDouble(lstSR.SelectedValue) * 0.1;
                ////Total_Score = Convert.ToDouble(lblBDM_TotalScore_CCMF.Text);
                //Total_Score = ((Management_Team) + (Business_Model) + (Creativity) + (Social_Responsibility));

                //// update CCMF
                //sql = "UPDATE TB_SCREENING_CCMF_SCORE SET "
                //     + "Reviewer=@Reviewer, "
                //     + "Management_Team=@Management_Team, "
                //     + "Business_Model=@Business_Model, "
                //     + "Creativity=@Creativity, "
                //     + "Social_Responsibility=@Social_Responsibility, "
                //     + "Total_Score=@Total_Score, "
                //     + "Modified_By=@Modified_By, "
                //     + "Modified_Date= GETDATE() "
                //     + "WHERE Application_Number = @m_ApplicationNumber "
                //     + "AND Role=@m_Role";
                //parameters.Add(new SqlParameter("@Reviewer", Reviewer));
                //parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                //parameters.Add(new SqlParameter("@Creativity", Creativity));
                //parameters.Add(new SqlParameter("@Business_Model", Business_Model));
                //parameters.Add(new SqlParameter("@Social_Responsibility", Social_Responsibility));
                //parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                //parameters.Add(new SqlParameter("@Modified_By", Modified_By));
                //parameters.Add(new SqlParameter("@m_ApplicationNumber", m_ApplicationNumber));
                //parameters.Add(new SqlParameter("@m_Role", m_Role));

                //conn.Open();
                //try
                //{
                //    SqlCommand cmdBDMScore = new SqlCommand(sql, conn);
                //    cmdBDMScore.Parameters.AddRange(parameters.ToArray());
                //    cmdBDMScore.ExecuteNonQuery();
                //}
                //finally
                //{
                //    conn.Close();
                //}
                #endregion



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
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                oListItem["Status"] = "Complete Screening";
                //oListItem["Remarks_for_Vetting"] = remark.Text.ToString(); //For SharePoint Application List Testing Field

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }
        }

        protected void BtnPassScore_Click(object sender, EventArgs e)
        {

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

        //Get Sharepoint Role Name e.g Senior Manager
        protected string get_role()
        {
            string m_Role = "";

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

            return m_Role;

        }

        //Get Programme id 
        protected string get_proid(string Programme_Name, string Intake_Number)
        {
            string m_prog_id = "";
            string sql_string = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql_string = "SELECT * FROM TB_PROGRAMME_INTAKE where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number ";
                using (SqlCommand cmd = new SqlCommand(sql_string, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Programme_Name", Programme_Name));
                    cmd.Parameters.Add(new SqlParameter("@Intake_Number", Intake_Number));
                    conn.Open();
                    try
                    {
                        m_prog_id = cmd.ExecuteScalar().ToString();
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
            //return sql_string;
            return m_prog_id;
        }


        public class ScoreList
        {
            public string ScoreTitle { set; get; }
            public string ScoreBDM { set; get; }
            public string ScoreSrMagr { set; get; }
            public string ScoreCPMO { set; get; }
        }

        protected void CPIPGridViewScore_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string urlstring = "";
            // //bool showbutton = true; 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0].Text = "<B>" + e.Row.Cells[0].Text + "</B>";
                //ddlBDM 
                HiddenField HiddenddlBDM = (HiddenField)e.Row.FindControl("HiddenddlBDM");
                urlstring = HiddenddlBDM.Value.ToString();

                DropDownList ddlDepartment = e.Row.FindControl("ddlBDM") as DropDownList;

                //     //ImageButton btn = (ImageButton)e.Row.Cells[8].FindControl("EditButton");
                //     HiddenField HFVMID = (HiddenField)e.Row.FindControl("HiddenVetting_Meeting_ID");

                // 
                //     if (recordcountcheck(HFVMID.Value.ToString().Trim()))
                //     {
                //         ImageButton btnadd = (ImageButton)e.Row.FindControl("AddButton");
                //         ImageButton btnedit = (ImageButton)e.Row.FindControl("EditButton");
                //         btnadd.Visible = true;
                //         btnedit.Visible = false;
                //     }
                //     else
                //     {
                //         ImageButton btnadd = (ImageButton)e.Row.FindControl("AddButton");
                //         ImageButton btnedit = (ImageButton)e.Row.FindControl("EditButton");
                //         btnadd.Visible = false;
                //         btnedit.Visible = true;
                //     }
                // 
            }
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

                            string newdirection = "";
                            newdirection = "Application%20List.aspx?";
                            newdirection += "ProgName=" + HttpContext.Current.Request.QueryString["ProgName"];
                            newdirection += "&IntakeNo=" + HttpContext.Current.Request.QueryString["IntakeNo"];
                            newdirection += "&Cluster=" + HttpContext.Current.Request.QueryString["Cluster"];
                            newdirection += "&Stream=" + HttpContext.Current.Request.QueryString["Stream"];
                            newdirection += "&Status=" + HttpContext.Current.Request.QueryString["Status"];
                            newdirection += "&SortColumn1=" + HttpContext.Current.Request.QueryString["SortColumn1"];
                            newdirection += "&SortOrder1=" + HttpContext.Current.Request.QueryString["SortOrder1"];
                            newdirection += "&SortColumn2=" + HttpContext.Current.Request.QueryString["SortColumn2"];
                            newdirection += "&SortOrder2=" + HttpContext.Current.Request.QueryString["SortOrder2"];


                            //newdirection = "Application%20List.aspx";
                            Context.Response.Redirect(newdirection);
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
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
                                        SPUtility.SendEmail(web,headers, body);
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
                command.Parameters.Add("@emailTemplate", emailTemplate);

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
    }
}