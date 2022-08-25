using Microsoft.SharePoint;
using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using System.Drawing;

namespace CBP_EMS_SP.VMListWP.VMListWP
{
    [ToolboxItemAttribute(false)]
    public partial class VMListWP : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        private String m_Role;
        private String m_systemuser;
        private SqlConnection connection;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        List<SearchResult> lstData = new List<SearchResult>();
        private String m_useremail;

        public VMListWP()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User
            getReview();
            //lbltest.Text = "Debug mode: " + m_systemuser + "||" +  m_Role;
             m_useremail = HttpContext.Current.Request.QueryString["user"];
             if (checkUserByEmail(m_useremail))
             {
                 MainPanel.Visible = true;

                 if (!Page.IsPostBack)
                 {
                     BindData();
                 }

                 AccessControl();
             }
             else
             {
                 MainPanel.Visible = false;
                 clearSessionCookies();
             }
        }

        private Boolean checkUserByEmail(String email)
        {
            var status = true;
            if (email != null)
            {
                if (email.Contains(SPContext.Current.Web.CurrentUser.Email))
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }


            return status;
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

        protected void AccessControl()
        {
            // Check Role can display this web part
            //Applicant  //Collaborator  //CCMF Coordinator //CCMF BDM  //CPIP Coordinator  //CPIP BDM  //Senior Manager  //CPMO

            ///            if (m_Role == "CBP EMS DEV Vetting Team")
            ///            {
            ///                MainPanel.Visible = true;
            ///            }
            ///            else 
            ///            {
            ///                MainPanel.Visible = false;
            ///            }

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

        private void BindData()
        {
            GenerateData();
            gvTable.DataSource = lstData;
            gvTable.DataBind();
        }




        private void GenerateData()
        {
            string sql = "";
            lstData.Clear();
            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                sql = @"SELECT progintake.Programme_Name, progintake.Intake_Number, meeting.Vetting_Meeting_ID, meeting.Programme_ID,
                        meeting.[Date], meeting.Venue, meeting.Vetting_Meeting_From, meeting.Vetting_Meeting_To,
                        meeting.Presentation_From, meeting.Presentation_To, meeting.Vetting_Team_Leader, meeting.No_of_Attendance,
                        meeting.Created_By, meeting.Created_Date, meeting.Modified_By, meeting.Modified_Date, meeting.Time_Interval, meeting.Meeting_status
                        FROM TB_VETTING_MEETING AS meeting 
                        INNER JOIN TB_PROGRAMME_INTAKE AS progintake ON meeting.Programme_ID = progintake.Programme_ID
                        INNER JOIN TB_VETTING_MEMBER AS member ON member.Vetting_Meeting_ID = meeting.Vetting_Meeting_ID
                        INNER JOIN TB_VETTING_MEMBER_INFO AS memberInfo ON memberInfo.Vetting_Member_ID = member.Vetting_Member_ID
                        WHERE memberInfo.Email = @Email and meeting.Meeting_status = 'Invite Email Sent'
                        ORDER BY  meeting.[Date] DESC";
               
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@Email", m_systemuser));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstData.Add(new SearchResult
                    {
                        Vetting_Meeting_ID = reader.GetGuid(reader.GetOrdinal("Vetting_Meeting_ID")).ToString(),
                        Programme_Name = reader.GetString(reader.GetOrdinal("Programme_Name")),
                        Programme_ID = Convert.ToString(reader.GetValue(reader.GetOrdinal("Programme_ID"))),
                        Intake_No = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString(),
                        Date = reader.GetDateTime(reader.GetOrdinal("Date")).ToString("dd/MM/yyyy"),
                        Venue = reader.GetString(reader.GetOrdinal("Venue")),
                        Vetting_Meeting_Time = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_From")).ToString("h:mmtt") + " - " + reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_To")).ToString("h:mmtt"),
                        Presentation_Time = reader.GetDateTime(reader.GetOrdinal("Presentation_From")).ToString("h:mmtt") + " - " + reader.GetDateTime(reader.GetOrdinal("Presentation_To")).ToString("h:mmtt"),
                        Time_Interval = reader.GetValue(reader.GetOrdinal("Time_Interval")).ToString() + " min",
                        Meeting_status = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString()
                    });               
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

        protected int StatusConvertNum(string m_Meeting_status)
        {
            int StatusNum = 0;

            if (string.IsNullOrEmpty(m_Meeting_status))
            {
                StatusNum = 0;
            }
            else if (m_Meeting_status == "Vetting Meeting Create")
            {
                StatusNum = 1;
            }
            else if (m_Meeting_status == "Email Sended")
            {
                StatusNum = 2;
            }

            return StatusNum;
        }        

        protected void rblSort1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        public class SearchResult
        {
            public string Programme_Name { get; set; }
            public string Intake_No { get; set; }
            public string Vetting_Meeting_ID { get; set; }
            public string Programme_ID { get; set; }
            public string Date { get; set; }
            public string Venue { get; set; }
            public string Vetting_Meeting_Time { get; set; }
            public string Presentation_Time { get; set; }
            public string Time_Interval { get; set; }
            public string Meeting_status { get; set; }
        }



        protected void gvTable_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = gvTable.Rows[index];
                string urlstring = row.Cells[0].Text;

                HiddenField HFVMID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
                urlstring = HFVMID.Value.ToString();
                Context.Response.Redirect("VM_Edit.aspx?VMID=" + urlstring);

            }
            else if (e.CommandName == "Add")
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = gvTable.Rows[index];
                string urlstring = row.Cells[0].Text;

                HiddenField HFVMID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
                urlstring = HFVMID.Value.ToString();
                Context.Response.Redirect("PresentationList.aspx?VMID=" + urlstring);
            }
            else if (e.CommandName == "Edit")
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = gvTable.Rows[index];
                string urlstring = row.Cells[0].Text;

                HiddenField HFVMID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
                urlstring = HFVMID.Value.ToString();
                Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + urlstring);
            }
            else if (e.CommandName == "Programme Name")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTable.Rows[index];
                string urlstring = row.Cells[0].Text;

                HiddenField HiddenProgramme_ID = (HiddenField)row.FindControl("HiddenProgramme_ID");
                HiddenField HFVMID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
                urlstring = "applications_for_programme_intake.aspx?prog=" + HiddenProgramme_ID.Value.ToString();
                urlstring += "&mid=" + HFVMID.Value.ToString();
                Context.Response.Redirect(urlstring);
           }
            else if (e.CommandName == "Declaration")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTable.Rows[index];
                string urlstring = row.Cells[0].Text;

                HiddenField HFVMID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
                urlstring = HFVMID.Value.ToString();
                Context.Response.Redirect("declarationofinterest.aspx?VMID=" + urlstring);
            }

        }

        protected void gvTable_DataBound(object sender, EventArgs e)
        {

        }

        protected string checkVettDeclarationCount(string m_Vetting_Meeting_ID, string m_systemuser)
        {
            string m_Vetting_Delclaration_ID = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                sql = "SELECT Vetting_Delclaration_ID,Vetting_Meeting_ID,Member_Email,DateTime,Venue,Name,No_Conflict_Application,Abstained_Voting_Application "
                    + "FROM TB_VETTING_DECLARATION "
                    + "Where Vetting_Meeting_ID=@m_Vetting_Meeting_ID and Member_Email=@m_systemuser";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID);
                        cmd.Parameters.Add("@m_systemuser", m_systemuser);
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            m_Vetting_Delclaration_ID = reader.GetValue(reader.GetOrdinal("Vetting_Delclaration_ID")).ToString().Trim();
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }

            return m_Vetting_Delclaration_ID;
        }

        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //bool showbutton = true; 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string hex = "#80C343";
                Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                e.Row.Cells[0].ForeColor = _color;
                //e.Row.Cells[0].CssClass = "emscontent datatable first-child";

                //e.Row.Cells[0].Text = "<B>" + e.Row.Cells[0].Text + "</B>";
                //ImageButton btn = (ImageButton)e.Row.Cells[8].FindControl("EditButton");
                HiddenField HFVMID = (HiddenField)e.Row.FindControl("HiddenVetting_Meeting_ID");
                HiddenField HFMeeting_status = (HiddenField)e.Row.FindControl("HiddenMeeting_status");
                HiddenField HiddenProgramme_Name = (HiddenField)e.Row.FindControl("HiddenProgramme_Name");
                LinkButton btnDeclaration = (LinkButton)e.Row.FindControl("btnDeclaration");
                LinkButton m_btnview = (LinkButton)e.Row.FindControl("btnview");

                m_btnview.ForeColor = _color;
                m_btnview.Text = HiddenProgramme_Name.Value.ToString();

                if (string.IsNullOrEmpty(checkVettDeclarationCount(HFVMID.Value.ToString().Trim(), m_systemuser)))
                {
                    btnDeclaration.Text = "Declare now";
                    btnDeclaration.CssClass = "textLightBlueFont";
                }
                else
                {
                    btnDeclaration.Text = "Declared";
                    btnDeclaration.CssClass = "";
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

    }
}