using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace CPIPPresentScoreWP.CPIPPresentScoreWebPark
{
    [ToolboxItemAttribute(false)]
    public partial class CPIPPresentScoreWebPark : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CPIPPresentScoreWebPark()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private String m_progid;
        private String m_ApplicationID;
        private String m_Role;
        private String m_Member_Email = "";
        private String m_ApplicationNumber;

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }
      

        protected void Page_Load(object sender, EventArgs e)
        {
            //m_progid = Context.Request.QueryString["ProgNo"];
            
            //int ApplicationID = Convert.ToInt32(Context.Request.QueryString["AppNo"]);
            //m_ApplicationID = Context.Request.QueryString["AppNo"];

            m_progid = HttpContext.Current.Request.QueryString["Prog"];
            m_ApplicationID = HttpContext.Current.Request.QueryString["App"];

            using (SqlConnection conn = new SqlConnection(connStr))
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
            }

            m_ApplicationID = m_ApplicationNumber;

            screenValueSet();
            
        }

        protected void getReview()
        {

            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.LoginName.ToString(); //"Flora Yeung";            
            //lblrole.Text = SPContext.Current.Web.AllRolesForCurrentUser.ToString(); //"CCMF BDM";
            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.Name.ToString();
            //lblrole.Text = "";
            //m_Role = "";
            //SPSite oSiteCollection = SPContext.Current.Site;
            //using (SPWeb oWebsite = oSiteCollection.OpenWeb())
            //{
            //    SPUser userName = oWebsite.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName); //Getting the Current User Login Name
            //    SPGroupCollection collGroups = userName.Groups;
            //    if (collGroups.Count > 0)
            //    {
            //        foreach (SPGroup ogroup in collGroups)   //Looping the group collection and adding to the list 
            //        {
            //            lblrole.Text += "[" + ogroup.Name + "] ";
            //            m_Role += ogroup.Name;
            //        }
            //    }
            //}

        }

        protected void screenValueSet()
        {
            double lstqcmt_Num = Convert.ToDouble(lstqcmt.SelectedValue);
            double lstcipp_Num = Convert.ToDouble(lstcipp.SelectedValue);
            double lstmbv_Num = Convert.ToDouble(lstmbv.SelectedValue);
            double lstbhkdti_Num = Convert.ToDouble(lstbhkdti.SelectedValue);
            double lstpsmpb_Num = Convert.ToDouble(lstpsmpb.SelectedValue);
            double TotalScore = ((lstqcmt_Num * 0.2) + (lstcipp_Num * 0.2) + (lstmbv_Num * 0.3) + (lstbhkdti_Num * 0.1) + (lstpsmpb_Num * 0.2));
            lblTotalScore.Text = TotalScore.ToString("F2");

            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblApplicationNo.Text = m_ApplicationID;


            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                var command = new SqlCommand("SELECT * FROM TB_INCUBATION_APPLICATION where Application_Number=@m_ApplicationID ", connection);
                command.Parameters.Add(new SqlParameter("@m_ApplicationID", m_ApplicationID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lblCompanyName.Text = reader.GetString(18);
                    m_Member_Email = reader.GetString(4);
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);

                //SPBasePermissions bp = SPContext.Current.Web.GetUserEffectivePermissions(SPContext.Current.Web.CurrentUser.LoginName);

                string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();// SPContext.Current.Web.CurrentUser.LoginName.ToString();

                string Application_Number = m_ApplicationID;  //"4C0B5391-D2C7-444D-8B6B-16F1B5EADE7D";
                string Programme_ID = m_progid;
                double Management_Team = Convert.ToDouble(lstqcmt.SelectedValue) * scorepresent("lstMT_Num");
                double Business_Model = Convert.ToDouble(lstmbv.SelectedValue) * scorepresent("lstBMTM_Num");
                double Creativity = Convert.ToDouble(lstcipp.SelectedValue) * scorepresent("lstCIPP_Num");
                double Social_Responsibility = Convert.ToDouble(lstbhkdti.SelectedValue) * scorepresent("lstSR_Num");
                double Proposed_six_monthly = Convert.ToDouble(lstpsmpb.SelectedValue) * scorepresent("lstpsmpb");
                double Total_Score = ((Management_Team) + (Business_Model) + (Creativity) + (Social_Responsibility) + (Proposed_six_monthly));
                string Comments = "";
                string Remarks = txtremarks.Text;
                string Created_By = systemuser;
                DateTime Created_Date = DateTime.Now;
                string Modified_By = systemuser;
                DateTime Modified_Date = DateTime.Now;


                string sql = "insert into TB_PRESENTATION_INCUBATION_SCORE(Application_Number,Programme_ID,Member_Email,Management_Team,Business_Viability,Creativity,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                    + "@Application_Number, "
                    + "@Programme_ID, "
                    + "@m_Member_Email, "
                    + "@Management_Team, "
                    + "@Business_Model, "
                    + "@Creativity, "
                    + "@Social_Responsibility, "
                    + "@Proposed_six_monthly,"
                    + "@Total_Score, "
                    + "@Comments, "
                    + "@Remarks, "
                    + "@Created_By, "
                    + "GETDATE() , "
                    + "@Modified_By, "
                    + "GETDATE()"
                    + ")";

                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                    cmd.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                    cmd.Parameters.Add(new SqlParameter("@m_Member_Email", m_Member_Email));
                    cmd.Parameters.Add(new SqlParameter("@Management_Team", Management_Team));
                    cmd.Parameters.Add(new SqlParameter("@Business_Model", Business_Model));
                    cmd.Parameters.Add(new SqlParameter("@Creativity", Creativity));
                    cmd.Parameters.Add(new SqlParameter("@Social_Responsibility", Social_Responsibility));
                    cmd.Parameters.Add(new SqlParameter("@Proposed_six_monthly", Proposed_six_monthly));
                    cmd.Parameters.Add(new SqlParameter("@Total_Score", Total_Score));
                    cmd.Parameters.Add(new SqlParameter("@Comments", Comments));
                    cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                    cmd.Parameters.Add(new SqlParameter("@Created_By", Created_By));
                    cmd.Parameters.Add(new SqlParameter("@Modified_By", Modified_By));

                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
                Context.Response.Redirect("Application%20List.aspx");
                //Context.Response.Redirect("http://www.google.com.hk");

        }

        protected static double scorepresent(string num)
        {
            double _num = 0.0;

            switch (num)
            {
                case "lstMT_Num":
                    _num = 0.2;
                    break;

                case "lstBMTM_Num":
                    _num = 0.2;
                    break;

                case "lstCIPP_Num":
                    _num = 0.3;
                    break;

                case "lstSR_Num":
                    _num = 0.1;
                    break;

                case "lstpsmpb":
                    _num = 0.2;
                    break;

                default:
                    break;
            }

            return _num;
        }
    }
}
