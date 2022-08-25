using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CBP_EMS_SP.CCMFPresentScoreWP.CCMFPresentScoreWebpart
{
    [ToolboxItemAttribute(false)]
    public partial class CCMFPresentScoreWebpart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CCMFPresentScoreWebpart()
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
        private String m_ApplicationNumber;

        private String m_Member_Email = "";

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
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
                //using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = @ApplicationID ", conn))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ApplicationID", m_ApplicationID));
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
            //getReview();
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
            double lstMT_Num = Convert.ToDouble(lstMT.SelectedValue);
            double lstBMTM_Num = Convert.ToDouble(lstBMTM.SelectedValue);
            double lstCIPP_Num = Convert.ToDouble(lstCIPP.SelectedValue);
            double lstSR_Num = Convert.ToDouble(lstSR.SelectedValue);
            double TotalScore = ((lstMT_Num * scorepresent("lstMT_Num")) + (lstBMTM_Num * scorepresent("lstBMTM_Num")) + (lstCIPP_Num * scorepresent("lstCIPP_Num")) + (lstSR_Num * scorepresent("lstSR_Num")));
            lblTotalScore.Text = TotalScore.ToString("F2");

            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblApplicationNo.Text = m_ApplicationID;


            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                //var command = new SqlCommand("SELECT Company_Name_Eng,Company_Name_Chi,Applicant FROM TB_INCUBATION_APPLICATION where Application_Number='" + m_ApplicationID + "'", connection);
                var command = new SqlCommand("SELECT Company_Name_Eng,Company_Name_Chi,Applicant FROM TB_INCUBATION_APPLICATION where Application_Number=@ApplicationID ", connection);
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ApplicationID", m_ApplicationID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lblCompanyName.Text = reader.GetString(0);
                    m_Member_Email = reader.GetString(2);
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
            //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {

                //SPBasePermissions bp = SPContext.Current.Web.GetUserEffectivePermissions(SPContext.Current.Web.CurrentUser.LoginName);

                string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //SPContext.Current.Web.CurrentUser.Name.ToString();

                string Application_Number = m_ApplicationID;  //"4C0B5391-D2C7-444D-8B6B-16F1B5EADE7D";
                string Programme_ID = m_progid;
                string Reviewer = systemuser;
                double Management_Team = Convert.ToDouble(lstMT.SelectedValue) * scorepresent("lstMT_Num");
                double Business_Model = Convert.ToDouble(lstBMTM.SelectedValue) * scorepresent("lstBMTM_Num");
                double Creativity = Convert.ToDouble(lstCIPP.SelectedValue) * scorepresent("lstCIPP_Num");
                double Social_Responsibility = Convert.ToDouble(lstSR.SelectedValue) * scorepresent("lstSR_Num");
                double Total_Score = Convert.ToDouble(lblTotalScore.Text);
                string Comments = "";
                string Remarks = txtremarks.Text;
                string Created_By = systemuser;
                DateTime Created_Date = DateTime.Now;
                string Modified_By = systemuser;
                DateTime Modified_Date = DateTime.Now;


                string sql = "insert into TB_PRESENTATION_CCMF_SCORE(Application_Number,Programme_ID,Member_Email,Management_Team,Business_Model,Creativity,Social_Responsibility,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                    //+ "'" + Application_Number + "' , "
                    //+ "'" + Programme_ID + "' , "
                    //+ "'" + m_Member_Email + "' , "
                    //+ "'" + Management_Team + "' , "
                    //+ "'" + Business_Model + "' , "
                    //+ "'" + Creativity + "' , "
                    //+ "'" + Social_Responsibility + "' , "
                    //+ "'" + Total_Score + "' , "
                    //+ "'" + Comments + "' , "
                    //+ "'" + Remarks + "' , "
                    //+ "'" + Created_By + "' , "
                    //+ "GETDATE() , "
                    //+ "'" + Modified_By + "' , "
                    //+ "GETDATE()"
                    + "@Application_Number , "
                    + "@Programme_ID , "
                    + "@Member_Email , "
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

                //conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Application_Number",Application_Number));
                cmd.Parameters.Add(new SqlParameter("@Programme_ID",Programme_ID));
                cmd.Parameters.Add(new SqlParameter("@Member_Email",m_Member_Email));
                cmd.Parameters.Add(new SqlParameter("@Management_Team",Management_Team));
                cmd.Parameters.Add(new SqlParameter("@Business_Model",Business_Model));
                cmd.Parameters.Add(new SqlParameter("@Creativity",Creativity));
                cmd.Parameters.Add(new SqlParameter("@Social_Responsibility",Social_Responsibility));
                cmd.Parameters.Add(new SqlParameter("@Total_Score",Total_Score));
                cmd.Parameters.Add(new SqlParameter("@Comments",Comments));
                cmd.Parameters.Add(new SqlParameter("@Remarks",Remarks));
                cmd.Parameters.Add(new SqlParameter("@Created_By",Created_By));
                cmd.Parameters.Add(new SqlParameter("@Modified_By",Modified_By));
                cmd.ExecuteNonQuery();


                Context.Response.Redirect("Application%20List.aspx");
                //Context.Response.Redirect("http://www.google.com.hk");

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

        
    }
}
