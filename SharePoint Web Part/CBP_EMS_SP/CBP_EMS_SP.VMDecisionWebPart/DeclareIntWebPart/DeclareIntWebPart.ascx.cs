using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.DeclareIntWebPart.DeclareIntWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class DeclareIntWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DeclareIntWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private String m_progid;
        private String m_ApplicationID;
        //private String m_Role;
        private String m_vmID; //from URL

        private String m_Member_Email = "";

        private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";


        protected void Page_Load(object sender, EventArgs e)
        {
            m_progid = Context.Request.QueryString["ProgNo"];

            //int ApplicationID = Convert.ToInt32(Context.Request.QueryString["AppNo"]);
            m_ApplicationID = Context.Request.QueryString["AppNo"];
            m_vmID = Context.Request.QueryString["vmID"];

            //getReview();
            screenValueSet();
        }

        protected void screenValueSet()
        {
            //double lstMT_Num = Convert.ToDouble(lstMT.SelectedValue);
            //double lstBMTM_Num = Convert.ToDouble(lstBMTM.SelectedValue);
            //double lstCIPP_Num = Convert.ToDouble(lstCIPP.SelectedValue);
            //double lstSR_Num = Convert.ToDouble(lstSR.SelectedValue);
            //double TotalScore = ((lstMT_Num * scorepresent("lstMT_Num")) + (lstBMTM_Num * scorepresent("lstBMTM_Num")) + (lstCIPP_Num * scorepresent("lstCIPP_Num")) + (lstSR_Num * scorepresent("lstSR_Num")));
            //lblTotalScore.Text = TotalScore.ToString("F2");

            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            //lblApplicationNo.Text = m_ApplicationID;


            var connection = new SqlConnection(connStr);
            connection.Open();
            // var command = new SqlCommand("SELECT Company_Name_Eng,Company_Name_Chi,Applicant FROM TB_INCUBATION_APPLICATION where Application_Number='" + m_ApplicationID + "'", connection);
            var command = new SqlCommand("SELECT Date, Venue FROM TB_VETTING_MEETING where Vetting_Metting_ID ='" + m_vmID + "'", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lblDate.Text = reader.GetString(0);
                lblVenue.Text = reader.GetString(1);
            }

            reader.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";
            string sql2 = "";

            try
            {

                //SPBasePermissions bp = SPContext.Current.Web.GetUserEffectivePermissions(SPContext.Current.Web.CurrentUser.LoginName);

                string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //SPContext.Current.Web.CurrentUser.Name.ToString();

                string Application_Number = m_ApplicationID;  //"4C0B5391-D2C7-444D-8B6B-16F1B5EADE7D";
                string Programme_ID = m_progid;
                //DateTime Date = lblDate.Text;
                string Date = lblDate.Text;
                string Venue = lblVenue.Text;
                string Name = lblName.Text;

                string NoConflict = ckbNoConflict.Checked ? "1" : "0";
                string HaveConflict = ckbHaveConflict.Checked ? "1" : "0";

                //string Created_By = systemuser;
                //DateTime Created_Date = DateTime.Now;
                //string Modified_By = systemuser;
                //DateTime Modified_Date = DateTime.Now;

                sql = "insert into TB_VETTING_DECLARATION(Vetting_Delclaration_ID,Vetting_Meeting_ID,Member_Email,DateTime,Venue,Name,No_Conflict_Application,Abstained_Voting_Application) VALUES ("
                        + "'" + "4C0B5391-D2C7-444D-8B6B-16F1B5EADE7A" + "' , "
                        + "'" + m_vmID + "' , "
                        + "'" + m_Member_Email + "' , "
                        + "'" + Date + "' , "
                        + "'" + Venue + "' , "
                        + "'" + Name + "' , "
                        + "'" + NoConflict + "' , "
                        + "'" + HaveConflict + "'); ";


                if (HaveConflict == "1")
                {

                    sql2 = "insert into TB_DECLARATION_APPLICATION(Vetting_Delclaration_ID,Vetting_Meeting_ID,Application_Number) VALUES ("
                        + "'" + "4C0B5391-D2C7-444D-8B6B-16F1B5EADE7A" + "' , "
                        + "'" + m_vmID + "' , "
                        + "'" + Application_Number + "');";

                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                if (sql2 != "")
                {
                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    cmd2.ExecuteNonQuery();
                }



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


    }
}
