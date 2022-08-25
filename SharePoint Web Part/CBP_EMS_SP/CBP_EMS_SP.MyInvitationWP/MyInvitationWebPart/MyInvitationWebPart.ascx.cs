using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.MyInvitationWP.MyInvitationWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class MyInvitationWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public MyInvitationWebPart()
        {
        }

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        private SqlConnection connection;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            if (web.UICulture.Name.ToLower() == "en-us")
                            {
                                lblTitle.Text = "My Invitation";
                            }
                            else
                            {
                                lblTitle.Text = "我的邀請";
                            }
                        }
                    }
                });

                BindGridViewMyInvitation();
            }
        }

        protected void BindGridViewMyInvitation()
        {
            ConnectOpen();
            try
            {
                //var sqlString = "select tpi.Programme_Name,tpi.Intake_Number,tva.Application_Number,tvm.Vetting_Meeting_From,tvm.Vetting_Meeting_To,tvmInfo.Email,tva.Vetting_Meeting_ID,tva.Vetting_Application_ID from TB_VETTING_MEETING tvm left join TB_PROGRAMME_INTAKE tpi on tpi.Programme_ID = tvm.Programme_ID left join TB_VETTING_APPLICATION tva on tvm.Vetting_Meeting_ID = tva.Vetting_Meeting_ID left join TB_VETTING_MEMBER tvmember on tvmember.Vetting_Meeting_ID = tvm.Vetting_Meeting_ID left join TB_VETTING_MEMBER_INFO tvmInfo on tvmInfo.Vetting_Member_ID = tvmember.Vetting_Member_ID where tvm.Meeting_status = 'Email Sended' and tva.Application_Number <> 'Time Break' and tvmInfo.Disabled = 0  and tvmInfo.Email = @Email";
                //var sqlString = "select distinct IT.Programme_Name, IT.Intake_Number , AP.Application_Number, VM.[Vetting_Meeting_ID], AP.Vetting_Application_ID, VM.Meeting_status, VM.[Date],VM.[Vetting_Meeting_From],VM.[Vetting_Meeting_To], [Venue],[Vetting_Team_Leader], AP.Email from [dbo].[TB_VETTING_MEETING] VM ";
                var sqlString = "select distinct IT.Programme_Name, IT.Intake_Number , AP.Application_Number, VM.[Vetting_Meeting_ID], AP.Vetting_Application_ID, ";
                sqlString += "VM.Meeting_status, VM.[Date],AP.[Presentation_From], AP.[Presentation_To], [Venue],[Vetting_Team_Leader], AP.Email from [dbo].[TB_VETTING_MEETING] VM ";
                sqlString += "left join[dbo].[TB_VETTING_MEMBER] ME on ME.Vetting_Meeting_ID = VM.[Vetting_Meeting_ID] ";
                sqlString += "left join[dbo].[TB_VETTING_MEMBER_INFO] MI on ME.Vetting_Member_ID = MI.Vetting_Member_ID ";
                sqlString += "left join[dbo].[TB_PROGRAMME_INTAKE] IT on IT.Programme_ID = VM.Programme_ID ";
                sqlString += "left join[dbo].[TB_VETTING_APPLICATION] AP on AP.Vetting_Meeting_ID = VM.Vetting_Meeting_ID ";
                sqlString += "where  VM.Meeting_status in ('Email Sended', 'Invite Email Sent') and AP.Email = @Email and AP.Email != '' ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@Email", SPContext.Current.Web.CurrentUser.Email);

                var reader = command.ExecuteReader();
                List<SearchResult> invitationList = new List<SearchResult>();
                while (reader.Read())
                {
                    SearchResult item = new SearchResult();

                    var programName = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                    var Intake_Number = reader.GetValue(reader.GetOrdinal("Intake_Number")).ToString();
                    var Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString();
                    //var Vetting_Meeting_From = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_From"));
                    //var Vetting_Meeting_To = reader.GetDateTime(reader.GetOrdinal("Vetting_Meeting_To"));
                    var Vetting_Meeting_From = reader.GetDateTime(reader.GetOrdinal("Presentation_From"));
                    var Vetting_Meeting_To = reader.GetDateTime(reader.GetOrdinal("Presentation_To"));
                    var Vetting_Meeting_ID = reader.GetValue(reader.GetOrdinal("Vetting_Meeting_ID")).ToString();
                    var Vetting_Application_ID = reader.GetValue(reader.GetOrdinal("Vetting_Application_ID")).ToString();

                    item.ItemText = programName + " - " + Intake_Number + " - " + Application_Number + " - " + Vetting_Meeting_From.ToString("d MMM yyyy") 
                                    + " (" + Vetting_Meeting_From.ToString("HH:mm") + " - " + Vetting_Meeting_To.ToString("HH:mm") + ")";
                    item.VMID = Vetting_Meeting_ID;
                    item.VAID = Vetting_Application_ID;

                    invitationList.Add(item);
                }

                reader.Dispose();
                command.Dispose();

                GridViewMyInvitation.DataSource = invitationList;
                GridViewMyInvitation.DataBind();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void GridViewMyInvitation_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = GridViewMyInvitation.Rows[index];
                var VMID = row.FindControl("HiddenFieldVMID") as HiddenField;
                var VAID = row.FindControl("HiddenFieldVAID") as HiddenField;

                Context.Response.Redirect("Invitation%20Response.aspx?VMID=" + VMID.Value + "&VAID=" + VAID.Value);
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

    public class SearchResult
    {
        public string ItemText { get; set; }
        public string VMID { get; set; }
        public string VAID { get; set; }
    }
}
