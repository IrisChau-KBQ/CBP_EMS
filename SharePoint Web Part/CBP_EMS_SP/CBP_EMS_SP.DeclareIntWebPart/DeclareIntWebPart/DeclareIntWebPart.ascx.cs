using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
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


        List<SearchResult> lstData = new List<SearchResult>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private String m_progid;
        private String m_ApplicationID;
        private Guid m_vmID = Guid.NewGuid();
        private String m_Member_Email = "";

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        protected void Page_Load(object sender, EventArgs e)
        {
            m_progid = Context.Request.QueryString["ProgNo"];
            m_ApplicationID = Context.Request.QueryString["AppNo"];
            if (Context.Request.QueryString["vmID"] == null || Context.Request.QueryString["vmID"] == "")
            {
                m_vmID = Guid.NewGuid();
            }else{
                m_vmID = new Guid(Context.Request.QueryString["vmID"]);
            }
            

            screenValueSet();
        }

        protected void screenValueSet()
        {

            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

            var connection = new SqlConnection(connStr);
            connection.Open();
            // var command = new SqlCommand("SELECT Company_Name_Eng,Company_Name_Chi,Applicant FROM TB_INCUBATION_APPLICATION where Application_Number='" + m_ApplicationID + "'", connection);
            var command = new SqlCommand("SELECT Date, Venue FROM TB_VETTING_MEETING where Vetting_Meeting_ID ='" + m_vmID + "'", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lblDate.Text = reader.GetDateTime(0).ToString();
                lblVenue.Text = reader.GetString(1);
            }

            reader.Dispose();
            command.Dispose();
   
            // select and populate application list
            var cmd = new SqlCommand("SELECT isNull(b.Application_NUMBER,'') as Application_NUMBER, isNull(c.company_name_eng,'') as CoOrPrjName, isNull(c.Applicant,'') as Applicant, cast(isNull(a.No_ConflicT_Application,0) as bit) as No_ConflicT_Application "
                                       + "FROM TB_Vetting_Declaration a, "
                                       + "TB_VETTING_APPLICATION b, "
                                       + "TB_INCUBATION_APPLICATION c "
                                       + "where a.Vetting_Meeting_ID = b.Vetting_Meeting_ID "
                                       + "and   b.Application_Number = c.Application_Number "
                                       + "and   a.Vetting_Meeting_ID = '" + m_vmID + "'"
                                       + " union " +
                                      "SELECT isNull(b.Application_NUMBER,'') as Application_NUMBER, isNull(c.Project_Name_Eng,'') as CoOrPrjName, isNull(c.Applicant,'') as Applicant , cast(isNull(a.No_ConflicT_Application,0) as bit) as No_ConflicT_Application "
                                       + "FROM TB_Vetting_Declaration a, "
                                       + "TB_VETTING_APPLICATION b, "
                                       + "TB_CCMF_APPLICATION c "
                                       + "where a.Vetting_Meeting_ID = b.Vetting_Meeting_ID "
                                       + "and   b.Application_Number = c.Application_Number "
                                       + "and   a.Vetting_Meeting_ID = '" + m_vmID + "';", connection);

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lstData.Add(new SearchResult
                {
                    CoOrprjName = rdr.GetString(1),
                    Applicant = rdr.GetString(2),
                    ConflictOfInt = (Boolean)rdr.GetValue(3)
                });
            }

            gvAppl.DataSource = lstData;
            gvAppl.DataBind();

            rdr.Dispose();                       
            cmd.Dispose();

            connection.Close();
            connection.Dispose();

        }


        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";
            string sql2 = "";

            try
            {
                string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); 

                string Application_Number = m_ApplicationID;
                DateTime Date = DateTime.Parse(lblDate.Text);
                string Venue = lblVenue.Text;
                string Name = lblName.Text;

                string NoConflict = ckbNoConflict.Checked ? "1" : "0";
                string HaveConflict = ckbHaveConflict.Checked ? "1" : "0";

                sql = "insert into TB_VETTING_DECLARATION(Vetting_Meeting_ID,Member_Email,DateTime,Venue,Name,No_Conflict_Application,Abstained_Voting_Application) VALUES ("
                        + "'" + m_vmID + "' , "
                        + "'" + m_Member_Email + "' , "
                        + "'" + Date + "' , "
                        + "'" + Venue + "' , "
                        + "'" + Name + "' , "
                        + "'" + NoConflict + "' , "
                        + "'" + HaveConflict + "'); ";


                if (HaveConflict == "1")
                {

                    sql2 = "insert into TB_DECLARATION_APPLICATION(Vetting_Meeting_ID,Application_Number) VALUES ("
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

        public class SearchResult
        {
            public string Application_Number { get; set; }
            public string CoOrprjName { get; set; }
            public string Applicant { get; set; }
            public Boolean ConflictOfInt { get; set; }

        }


    }
}
