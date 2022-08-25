using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.InvResWP.InvResWP
{
    [ToolboxItemAttribute(false)]
    public partial class InvResWP : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public InvResWP()
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
        private String m_VA_ID;

        private String m_Member_Email = "";

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        protected void Page_Load(object sender, EventArgs e)
        {
            m_progid = Context.Request.QueryString["ProgNo"];
            m_ApplicationID = Context.Request.QueryString["AppNo"];
            m_VA_ID = Context.Request.QueryString["VAID"];

            screenValueSet();
        }

        protected void screenValueSet()
        {
            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblApplicationNo.Text = m_ApplicationID;


            var connection = new SqlConnection(connStr);
            connection.Open();
            var command = new SqlCommand("SELECT Vetting_Application_ID, Vetting_Meeting_ID, Application_Number FROM TB_VETTING_APPLICATION where Application_Number='" + m_ApplicationID + "'", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {


                m_VA_ID = reader.GetGuid(0).ToString();
                m_ApplicationID = reader.GetString(2);

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

            try
            {
                string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); 

                string Vetting_Application_ID = m_VA_ID;
                string Application_Number = m_ApplicationID;  
                string Programme_ID = m_progid;
                string Email = txtEmail.Text;
                string MobileNumber = txtMobile.Text;
                string Attend = radioAttend.SelectedValue.ToString();
                string NameOfAttendees = txtNameOfAttendees.Text;
                string TypeOfPresent = txtPresentTools.Text;
                string SpecialRequest = txtSpecialRequest.Text;
                string VideoClip = txtVideoClip.Text;
                string PresentSlide = upPresentSlide.FileName.ToString();

                string Created_By = systemuser;
                DateTime Created_Date = DateTime.Now;
                string Modified_By = systemuser;
                DateTime Modified_Date = DateTime.Now;

                string sql = "Update TB_VETTING_APPLICATION set Email = '" + Email + "' , "
                    + "Mobile_Number ='" + MobileNumber + "' , "
                    + "Attend = '" + Attend + "' , "
                    + "Name_of_Attendees = '" + NameOfAttendees + "' , "
                    + "Presentation_Tools = '" + TypeOfPresent + "' , "
                    + "Special_Request = '" + SpecialRequest + "' "
                    + "where Vetting_Application_ID = '" + m_VA_ID + "';";


                string sql2 = "Update TB_APPLICATION_ATTACHMENT set Attachment_Type = '" + VideoClip + "' , "
                    + "Attachment_Path = '" + PresentSlide + "' , "
                    + "Modified_By = '" + Modified_By + "', "
                    + "Modified_Date = GETDATE() "
                    + "where Application_ID = '" + m_VA_ID + "';";


                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                cmd2.ExecuteNonQuery();

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
