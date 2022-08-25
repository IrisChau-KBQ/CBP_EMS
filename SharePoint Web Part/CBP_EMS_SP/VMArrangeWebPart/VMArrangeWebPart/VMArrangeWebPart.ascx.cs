using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.SharePoint;
using System.Web;

namespace VMArrangeWebPart.VMArrangeWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class VMArrangeWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public VMArrangeWebPart()
        {
        }

        List<SearchResult> lstData = new List<SearchResult>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
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
            //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            //string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                sql = "SELECT dbo.TB_PROGRAMME_INTAKE.Programme_Name, dbo.TB_PROGRAMME_INTAKE.Intake_Number, dbo.TB_VETTING_MEETING.Vetting_Meeting_ID, dbo.TB_VETTING_MEETING.Programme_ID, "
                             + "dbo.TB_VETTING_MEETING.Date, dbo.TB_VETTING_MEETING.Venue, dbo.TB_VETTING_MEETING.Vetting_Meeting_From, dbo.TB_VETTING_MEETING.Vetting_Meeting_To, "
                             + "dbo.TB_VETTING_MEETING.Presentation_From, dbo.TB_VETTING_MEETING.Presentation_To, dbo.TB_VETTING_MEETING.Vetting_Team_Leader, dbo.TB_VETTING_MEETING.No_of_Attendance, "
                             + "dbo.TB_VETTING_MEETING.Created_By, dbo.TB_VETTING_MEETING.Created_Date, dbo.TB_VETTING_MEETING.Modified_By, dbo.TB_VETTING_MEETING.Modified_Date, dbo.TB_VETTING_MEETING.Time_Interval, dbo.TB_VETTING_MEETING.Meeting_status "
                             + "FROM dbo.TB_VETTING_MEETING INNER JOIN "
                             + "dbo.TB_PROGRAMME_INTAKE ON dbo.TB_VETTING_MEETING.Programme_ID = dbo.TB_PROGRAMME_INTAKE.Programme_ID order by dbo.TB_VETTING_MEETING.Created_Date desc ";


                //var command = new SqlCommand("SELECT Vetting_Meeting_ID,Programme_ID,Date,Venue,Vetting_Meeting_From,Vetting_Meeting_To,Presentation_From,Presentation_To,Vetting_Team_Leader,No_of_Attendance FROM TB_VETTING_MEETING WHERE Date > GETDATE()-1;", connection);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstData.Add(new SearchResult
                    {
                        Vetting_Meeting_ID = reader.GetGuid(reader.GetOrdinal("Vetting_Meeting_ID")).ToString(),
                        Programme_Name = reader.GetString(reader.GetOrdinal("Programme_Name")),
                        Programme_ID = reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString(),
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("VM_create.aspx");
            
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
                HiddenField HiddenMeeting_status = (HiddenField)row.FindControl("HiddenMeeting_status");

                urlstring = HFVMID.Value.ToString();
                Context.Response.Redirect("VM_create.aspx?VMID=" + urlstring);
                //if (HiddenMeeting_status.Value.ToString() == "Saved" )
                //{
                //    Context.Response.Redirect("VM_create.aspx?VMID=" + urlstring);
                //}
                ////if (HiddenMeeting_status.Value.ToString() == "Vetting Meeting Create" 
                ////   || HiddenMeeting_status.Value.ToString() == "Email Sended"
                ////   || HiddenMeeting_status.Value.ToString() == "Invite Email Sent")
                //else
                //{
                //    //Context.Response.Redirect("VM_Edit.aspx?VMID=" + urlstring);
                //    Context.Response.Redirect("VM_create.aspx?VMID=" + urlstring + "&VMStatus=1");
                //}
         
                
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
                Context.Response.Redirect("PresentationList.aspx?VMID=" + urlstring);
            }
            else if (e.CommandName == "Programme Name")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTable.Rows[index];
                string urlstring = row.Cells[0].Text;

                //HiddenField HiddenProgramme_ID = (HiddenField)row.FindControl("HiddenProgramme_ID");
                //urlstring = HiddenProgramme_ID.Value.ToString();
                //Context.Response.Redirect("invitation_response_summary.aspx?prog=" + urlstring);
                HiddenField HFVMID = (HiddenField)row.FindControl("HiddenVetting_Meeting_ID");
                urlstring = HFVMID.Value.ToString();
                Context.Response.Redirect("invitation_response_summary.aspx?VMID=" + urlstring);
            }
        }

        protected void gvTable_DataBound(object sender, EventArgs e)
        {
            
        }

        protected bool recordcountcheck(string m_Vetting_Meeting_ID)
        {
            string sql = "";
            int m_count=0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql = "SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = @m_Vetting_Meeting_ID ";

                //sql = "SELECT Vetting_Meeting_ID ,Programme_ID,Date,Venue,Vetting_Meeting_From,Vetting_Meeting_To,Presentation_From,Presentation_To,Vetting_Team_Leader,No_of_Attendance,Created_By,Created_Date,Modified_By,Modified_Date,Time_Interval,Meeting_status FROM TB_VETTING_MEETING";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                    conn.Open();
                    try
                    {
                        m_count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            if (m_count == 0)
            { return false;  }
            else
            { return true; }

        }

        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //bool showbutton = true; 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hex = "#80C343";
                Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
                e.Row.Cells[0].ForeColor = _color;
                //e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].CssClass = "emscontent datatable first-child";

                //e.Row.Cells[0].Text = "<B>" + e.Row.Cells[0].Text + "</B>";
                //ImageButton btn = (ImageButton)e.Row.Cells[8].FindControl("EditButton");
                HiddenField HFVMID = (HiddenField)e.Row.FindControl("HiddenVetting_Meeting_ID");
                HiddenField HFMeeting_status = (HiddenField)e.Row.FindControl("HiddenMeeting_status");
                LinkButton m_btnview = (LinkButton)e.Row.FindControl("btnview");
                m_btnview.ForeColor = _color;
                
                //if (recordcountcheck(HFVMID.Value.ToString().Trim()))
                if (HFMeeting_status.Value.ToString().Trim() == "Vetting Meeting Create")
                {
                    ImageButton btnadd = (ImageButton)e.Row.FindControl("AddButton");
                    ImageButton btnedit = (ImageButton)e.Row.FindControl("EditButton");
                    btnadd.Visible = true;
                    btnedit.Visible = false;
                }
                else if (HFMeeting_status.Value.ToString().Trim() == "Saved")
                {
                    ImageButton btnadd = (ImageButton)e.Row.FindControl("AddButton");
                    ImageButton btnedit = (ImageButton)e.Row.FindControl("EditButton");
                    btnadd.Visible = false;
                    btnedit.Visible = false;
                }
                else
                {
                    ImageButton btnadd = (ImageButton)e.Row.FindControl("AddButton");
                    ImageButton btnedit = (ImageButton)e.Row.FindControl("EditButton");
                    btnadd.Visible = false;
                    btnedit.Visible = true;
                }
                
            }
           
        }
    }
}
