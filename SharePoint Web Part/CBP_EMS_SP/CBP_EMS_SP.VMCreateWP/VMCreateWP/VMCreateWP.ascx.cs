using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SP = Microsoft.SharePoint.Client;
using System.Linq;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using System.Net.Mail;
using System.IO;
using Microsoft.SharePoint.Administration;
using System.Net;

namespace CBP_EMS_SP.VMCreateWP.VMCreateWP
{
    [ToolboxItemAttribute(false)]
    public partial class VMCreateWP : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        private String m_progid;
        private String m_ProgrammeName;
        private string m_IntakeNum;

        private string selectedProg;
        private string selectedIntake;

        private string m_VMID = null;
        private SqlConnection connection;


        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        string m_systemuser = "";
        string m_Role = "";
        string m_Programme_ID = "";
        string m_Programme_Name = "";
        string m_Intake_Number = "";
        string m_Time_Interval = "";
        DateTime m_Presentation_From = DateTime.Now;
        DateTime m_Presentation_To = DateTime.Now;
        string m_Programme_Type = "";
        string m_Meeting_status = "";
        DateTime m_VettingDate = DateTime.Now;
        DateTime m_VettingDeadline = DateTime.Now;
        DateTime m_ConfirmDeadline = DateTime.Now;
        DateTime m_PakingDealine = DateTime.Now;
        string m_VettingVenue = "";
        DateTime m_VettingMettingFrom = DateTime.Now;
        DateTime m_VettingMeetingTo = DateTime.Now;



        string m_WebsiteUrl = "";
        string m_WebsiteUrl_VettingTeam = "";
        string m_WebsiteUrl_InvitationResponse = "";
        string m_mail = "";
        string m_subject = "";
        string m_body = "";
        string m_VMStatus = "";

        String m_SelectProgrammeName = "Select Programme Name";
        String m_SelectIntakeNumber = "Select Intake Number";
        String m_SelectVettingTeamLeaderName = "Select Leader";

        String m_iniProgrammeName = "";
        String m_iniIntakeNumber = "";
        

        int m_totalEligibleApplication;
        int m_totalShortlisted;

        DateTime m_ApplicationDeadline;
        string m_PreviewCCMFInvitEmailTo = "";
        string m_PreviewCPIPInvitEmailTo = "";
        string m_PreviewCUPPInvitEmailTo = "";
        string m_PreviewGBAYEPInvitEmailTo = "";
        bool m_InviteEmailSent = false;
        
        private string _CCEmailCCMFVetting = "";
        private string _CCEmailCPIPVetting = "";
        private string _CCEmailCUPPVetting = "";
        private string _CCEmailGBAYEPVetting = "";
        private string _VettingInvitationAttachment = "";
        private ListItem m_oldLeader;

      
        public Dictionary<String, int> GridViewColumnOrder;

        public VMCreateWP()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private void BindData()
        {

            Bind_lstRight();

            Bind_Progddlst();
            if (!string.IsNullOrEmpty(m_VMID))
            {
                screenValueSet();
                Bind_Intakeddlst();
            }
            else
                Init_Intakeddlst();

            //BindDataCluster();


        }

        private void Bind_Progddlst()               // setup the programme name ddlist
        {
            var connection = new SqlConnection(connStr);
            connection.Open();

            try
            {
                var sqlString = "select distinct  [Programme_Name]from [dbo].[TB_PROGRAMME_INTAKE] where status = 'Complete Screening' order by Programme_Name";
                var command = new SqlCommand(sqlString, connection);
                var reader = command.ExecuteReader();
                List<SearchResult> programmeNameList = new List<SearchResult>();

                programmeNameList.Add(new SearchResult
                {
                    ProgrammeName = m_SelectProgrammeName

                });

                while (reader.Read())
                {
                    programmeNameList.Add(new SearchResult
                    {
                        ProgrammeName = reader.GetString(0)

                    });
                }
                reader.Dispose();
                command.Dispose();

                lstCyberportProgramme.DataSource = programmeNameList;
                lstCyberportProgramme.DataBind();
                if (m_iniProgrammeName != "")
                {
                    lstCyberportProgramme.SelectedValue = m_iniProgrammeName;
                }
                selectedProg = lstCyberportProgramme.SelectedValue;                  // get selected Programme Name
                //if (selectedProg == null || selectedProg == "")
                //    selectedProg = lstCyberportProgramme.Text;

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private void Bind_lstRight()             // setup the progaramme intake ddlist
        {

            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                //var sqlString = "SELECT Vetting_Member_ID,Email FROM TB_VETTING_MEMBER_INFO where Disabled=0";
                var sqlString = "SELECT Vetting_Member_ID,Email,isNull(Salutation, '') +' '+Full_Name+ ', '+ isnull(First_Name,'') as Full_Name FROM TB_VETTING_MEMBER_INFO where Disabled=0 order by Full_Name";
                var command = new SqlCommand(sqlString, connection);
                var reader = command.ExecuteReader();
                List<SearchResult> memberEmailList = new List<SearchResult>();
                while (reader.Read())
                {
                    memberEmailList.Add(new SearchResult
                    {                        
                        //FieldText = reader.GetString(1),
                        FieldText = reader.GetString(reader.GetOrdinal("Full_Name")),
                        FieldValue = reader.GetGuid(0).ToString()
                    });
                }

                lstRight.DataSource = memberEmailList;
                lstRight.DataBind();

                List<SearchResult> TmemberEmailList = new List<SearchResult>();
                TmemberEmailList.Add(new SearchResult
                {
                    FieldText = m_SelectVettingTeamLeaderName,
                    FieldValue = m_SelectVettingTeamLeaderName
                });
                TmemberEmailList.AddRange(memberEmailList);

                ddlVettingTeamLeader.DataSource = TmemberEmailList;
                ddlVettingTeamLeader.DataBind();

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private void Bind_lstRightLeftExceptLeader()             // setup the progaramme intake ddlist
        {
            if (ddlVettingTeamLeader.SelectedValue != m_SelectVettingTeamLeaderName)
            {
                var connection = new SqlConnection(connStr);
                connection.Open();
                try
                {
                    var sqlString = "SELECT Vetting_Member_ID,Email,isNull(Salutation, '') +' '+ Full_Name+ ', '+ isnull(First_Name,'') as Full_Name FROM TB_VETTING_MEMBER_INFO where Disabled=0 and Vetting_Member_ID <> @Vetting_Member_ID order by Full_Name";
                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add(new SqlParameter("@Vetting_Member_ID", ddlVettingTeamLeader.SelectedValue));
                    var reader = command.ExecuteReader();
                    List<SearchResult> memberEmailList = new List<SearchResult>();
                    while (reader.Read())
                    {
                        memberEmailList.Add(new SearchResult
                        {
                            //FieldText = reader.GetString(1),
                            FieldText = reader.GetString(reader.GetOrdinal("Full_Name")),
                            FieldValue = reader.GetGuid(0).ToString()
                        });
                    }

                    lstRight.DataSource = memberEmailList;
                    lstRight.DataBind();

                    lstLeft.Items.Remove(ddlVettingTeamLeader.SelectedItem);
                    for (int i = 0; i < lstLeft.Items.Count; i++)
                    {
                        lstRight.Items.Remove(lstLeft.Items[i]);
                    }

                    lstLeft.SelectedIndex = -1;

                    reader.Dispose();
                    command.Dispose();
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private void Init_Intakeddlst()             // setup the progaramme intake ddlist
        {
            List<SearchResult> intakeNumberList = new List<SearchResult>();
            intakeNumberList.Add(new SearchResult
            {
                IntakeNumber = m_SelectIntakeNumber
            });
            lstIntakeNumber.DataSource = intakeNumberList;
            lstIntakeNumber.DataBind();
        }


        private void Bind_Intakeddlst()             // setup the progaramme intake ddlist
        {

            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {

                selectedProg = lstCyberportProgramme.SelectedValue;
                //var sqlString = "select distinct tpi.Intake_Number from TB_PROGRAMME_INTAKE tpi "
                //                + "left join TB_INCUBATION_APPLICATION tia on tia.Programme_ID = tpi.Programme_ID "
                //                + "left join TB_CCMF_APPLICATION tca on tca.Programme_ID = tpi.Programme_ID  "
                //                + "where (tca.Programme_ID is not null or tia.Programme_ID is not null) "
                //                + "and (tca.Status = 'Complete Screening' or tia.Status = 'Complete Screening') "
                //                + "and tpi.Programme_Name = @selectedProg order by tpi.Intake_Number;";     // show only intake found for that program
                var sqlString = "select distinct intake_number, status from [dbo].[TB_PROGRAMME_INTAKE]  "
                                + "where Programme_Name = @selectedProg and status = 'Complete Screening' order by Intake_Number"; // show only intake found for that program


                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@selectedProg", selectedProg));

                var reader = command.ExecuteReader();
                List<SearchResult> intakeNumberList = new List<SearchResult>();
                intakeNumberList.Add(new SearchResult
                {
                    IntakeNumber = m_SelectIntakeNumber
                });
                while (reader.Read())
                {
                    intakeNumberList.Add(new SearchResult
                    {
                        IntakeNumber = reader.GetInt32(0).ToString()
                    });
                }

                lstIntakeNumber.DataSource = intakeNumberList;
                lstIntakeNumber.DataBind();
                if (m_iniIntakeNumber != "")
                {
                    lstIntakeNumber.SelectedValue = m_iniIntakeNumber;
                }
                selectedIntake = lstIntakeNumber.SelectedValue;  // get selected inTake Number
                //if (selectedIntake == null || selectedIntake == "")
                //    selectedIntake = lstIntakeNumber.Text;

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        protected void screenValueSet()
        {
            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                var sqlString = "SELECT tvmeeting.Date, "
                                    + "tvmeeting.Venue, "
                                    + "tvmeeting.Vetting_Meeting_From, "
                                    + "tvmeeting.Vetting_Meeting_To, "
                                    + "tvmeeting.Presentation_From, tvmeeting.Presentation_To, "
                                    + "tvmeeting.Vetting_Team_Leader, "
                                    + "tvmeeting.No_of_Attendance, "
                                    + "tvmeeting.Time_Interval, "
                                    + "case when tvmember.isLeader is null then 3 else cast(tvmember.isLeader as int) end isLeader, "
                                    + "isnull(vminfo.Email,'') as Email,"
                                    + "TB_PROGRAMME_INTAKE.Programme_Name, "
                                    + "TB_PROGRAMME_INTAKE.Intake_Number, "
                                    + "isnull(tvmeeting.Deadline,'') as Deadline, "
                                    + "isnull(tvmeeting.ConfirmDeadline,'') as ConfirmDeadline, "
                                    + "isnull(tvmeeting.ParkingDeadline, '') as ParkingDeadline, "
                                    //+ "isnull(TB_VETTING_MEMBER_INFO.Full_Name,'') as FullName "
                                    + "isNull(vminfo.Salutation, '') +' '+vminfo.Full_Name+', '+ isnull(vminfo.First_Name,'') as FullName "
                    + "FROM TB_VETTING_MEETING tvmeeting left JOIN TB_VETTING_MEMBER tvmember ON tvmeeting.Vetting_Meeting_ID = tvmember.Vetting_Meeting_ID "
                    + "left JOIN TB_VETTING_MEMBER_INFO vminfo ON tvmember.Vetting_Member_ID = vminfo.Vetting_Member_ID "
                                    + "INNER JOIN TB_PROGRAMME_INTAKE ON tvmeeting.Programme_ID = TB_PROGRAMME_INTAKE.Programme_ID "
                                + "where tvmeeting.Vetting_Meeting_ID = @m_VMID order by isLeader asc";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                var reader = command.ExecuteReader();
                VettingMetting vm = new VettingMetting();
                List<string> VettingTeamMenber = new List<string>();
                List<SearchResult> VettingTeam = new List<SearchResult>();
                while (reader.Read())
                {
                    var countFieldOrder = 0;
                    vm.Date = reader.GetDateTime(countFieldOrder);

                    countFieldOrder++;
                    vm.Venue = reader.GetString(countFieldOrder);

                    countFieldOrder++;
                    vm.VMFrom = reader.GetDateTime(countFieldOrder);

                    countFieldOrder++;
                    vm.VMto = reader.GetDateTime(countFieldOrder);

                    countFieldOrder++;
                    vm.PresentationFrom = reader.GetDateTime(countFieldOrder);

                    countFieldOrder++;
                    vm.Presentationto = reader.GetDateTime(countFieldOrder);

                    countFieldOrder++;
                    vm.VettingTeamLeader = reader.GetString(countFieldOrder);
                    //vm.VettingTeamLeader = reader.GetString(reader.GetOrdinal("FullName"));

                    countFieldOrder++;
                    vm.NoofAttendance = reader.GetInt32(countFieldOrder).ToString();

                    countFieldOrder++;
                    vm.TimeInteval = reader.GetString(countFieldOrder);

                    countFieldOrder++;
                    var isleader = reader.GetInt32(countFieldOrder);

                    countFieldOrder++;
                    if (isleader == 0)
                    {
                        //VettingTeamMenber.Add(reader.GetString(countFieldOrder));
                        VettingTeamMenber.Add(reader.GetString(reader.GetOrdinal("FullName")));
                    }


                    countFieldOrder++;
                    vm.ProgrammeName = reader.GetString(countFieldOrder);

                    countFieldOrder++;
                    vm.IntakeNumber = reader.GetInt32(countFieldOrder).ToString();

                    //vm.Meeting_status = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString();
                    vm.Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline"));

                    vm.ConfirmDeadline = reader.GetDateTime(reader.GetOrdinal("ConfirmDeadline"));

                    vm.ParkingDeadline = reader.GetDateTime(reader.GetOrdinal("ParkingDeadline"));

                    vm.FullName = reader.GetString(reader.GetOrdinal("FullName"));
                }
                vm.VettingTeamMenber = VettingTeamMenber;

                lstCyberportProgramme.SelectedValue = vm.ProgrammeName;
                m_iniProgrammeName = vm.ProgrammeName;
                //lstIntakeNumber.SelectedValue = vm.IntakeNumber;
                m_iniIntakeNumber = vm.IntakeNumber;
                DatePicker.SelectedDate = vm.Date;
                SubDeadlinePicker.SelectedDate = vm.Deadline;
                SubDeadlineTimePicker.SelectedDate = vm.Deadline;
                ConfirmDeadlinePicker.SelectedDate = vm.ConfirmDeadline;
                ConfirmDeadlineTimePicker.SelectedDate = vm.ConfirmDeadline;
                ParkingDeadlinePicker.SelectedDate = vm.ParkingDeadline;
                TxtVenue.Text = vm.Venue;
                ddlTimeInterval.Text = vm.TimeInteval;
                VMFrom.SelectedDate = vm.VMFrom;
                VMTo.SelectedDate = vm.VMto;
                PresentFm.SelectedDate = vm.PresentationFrom;
                PresentTo.SelectedDate = vm.Presentationto;

                ListItem leader = ddlVettingTeamLeader.Items.FindByText(vm.VettingTeamLeader);
                if (leader != null)
                {
                    ddlVettingTeamLeader.SelectedValue = leader.Value;
                    lstRight.Items.Remove(leader);
                    m_oldLeader = leader;
                    hdn_Old_Leader.Value = leader.Value;
                    }

                foreach (ListItem item in lstRight.Items)
                    {
                    if (vm.VettingTeamMenber.Contains(item.Text))
                        {
                        //lstRight.Items.Remove(item);
                        lstLeft.Items.Add(item);
                        lstoldlist.Items.Add(item);
                        }
                    }
                foreach (ListItem item in lstLeft.Items)
                {
                    if (lstRight.Items.Contains(item))
                        lstRight.Items.Remove(item);
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

        private void Refresh_choices()
        {

            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                // get the Programme ID using programme Name and intake Number. Programme ID for adding into TB_Vetting_Meeting.
                var sqlString = "SELECT distinct Programme_ID from TB_PROGRAMME_INTAKE " +
                                " Where Programme_Name = @selectedProg " +
                                "   and Intake_Number = @selectedIntake ;";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@selectedProg", selectedProg));
                command.Parameters.Add(new SqlParameter("@selectedIntake", selectedIntake));

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    m_progid = reader.GetInt32(0).ToString();
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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (AccessControl())
            {
                panel1.Visible = true;
                DatePicker.DatePickerFrameUrl = ResolveUrl(SPContext.Current.Site.Url + "/_layouts/15/iframe.aspx");
                SetGridViewColumnOrder();
                m_VMID = HttpContext.Current.Request.QueryString["VMID"]!=null? (HttpContext.Current.Request.QueryString["VMID"]).ToString(): String.Empty ;
                m_VMStatus = HttpContext.Current.Request.QueryString["VMStatus"];
                lblInviteStatus.Text = "";

                btnPreviewEmail.Visible = true;
                btnSendInvitation.Visible = true;

                if (string.IsNullOrEmpty(m_VMID))
                {
                    btnDelete.Visible = false;
                    btnPreviewEmail.Visible = false;
                    btnSendInvitation.Visible = false;

                }
                else
                {
                    m_Meeting_status = GetMeetingStatus().Trim();

                    switch (m_Meeting_status)
                    {
                        case "Invite Email Sent":
                            m_InviteEmailSent = true;
                            //lblInviteStatus.Text = "Invitation Email already sent";
                            btnPreviewEmail.Enabled = true;
                            btnSendInvitation.Enabled = true;
                            btnDelete.Visible = false;
                            btnConfirm.Text = "Save";
                            DisableMeeitngInputs();
                            break;

                        case "Email Sended":
                            btnPreviewEmail.Enabled = true;
                            btnSendInvitation.Enabled = true;
                            btnDelete.Visible = false;
                            btnConfirm.Text = "Save";
                            DisableMeeitngInputs();
                            break;

                        case "Vetting Meeting Create":
                            btnPreviewEmail.Enabled = false;
                            btnSendInvitation.Enabled = false;
                            m_InviteEmailSent = false;
                            btnDelete.Visible = true;
                            btnConfirm.Text = "Save";
                            DisableMeeitngInputs();
                            break;

                        case "Saved":
                            btnPreviewEmail.Enabled = false;
                            btnSendInvitation.Enabled = false;
                            m_InviteEmailSent = false;
                            btnDelete.Visible = true;
                            btnConfirm.Text = "Confirm";
                            break;

                        default:
                            btnPreviewEmail.Enabled = false;
                            btnSendInvitation.Enabled = false;
                            m_InviteEmailSent = false;
                            btnConfirm.Text = "Confirm";
                            break;
                    }
                }
                if (!Page.IsPostBack)
                {

                    BindData();

                }
                else
                {
                    setdefaultValue();
                }
            }
            else
            {
                panel1.Visible = false;
            }

        }

        protected string GetMeetingStatus ()
        {
            string meetingStatus = "";
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                string slqText = "";
                conn.Open();

                slqText = "Select Meeting_status from TB_VETTING_MEETING where Vetting_Meeting_ID = @m_VMID";
                SqlCommand cmd;
                cmd = new SqlCommand(slqText, conn);
                cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    meetingStatus = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString();
                }

                reader.Dispose();
                cmd.Dispose();
            }
            finally
            {
                conn.Close();
            }
            return meetingStatus;
        }

        protected Boolean AccessControl()
        {
            var m_result = false;
            getReview();
            // Check Role can display this web part

            if (m_Role == "CCMF Coordinator")
            {
                m_result = true;
            }
            else if (m_Role == "CPIP Coordinator")
            {
                m_result = true;
            }
            else if (m_Role.Contains("BDM"))
            {
                m_result = true;
            }
            else if (m_Role.Contains("Senior Manager"))
            {
                m_result = true;
            }
            else if (m_Role.Contains("CPMO"))
            {
                m_result = true;
            }

            return m_result;
        }

        protected void getReview()
        {

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
                        m_Role = ogroup.Name;
                    }
                }
            }

        }

        protected void DisableMeeitngInputs() 
        {
            lstCyberportProgramme.Enabled = false;
            lstIntakeNumber.Enabled = false;
            DatePicker.Enabled = false;
            SubDeadlinePicker.Enabled = false;
            SubDeadlineTimePicker.Enabled = false;
            ConfirmDeadlinePicker.Enabled = false;
            ConfirmDeadlineTimePicker.Enabled = false;
            ParkingDeadlinePicker.Enabled = false;
            TxtVenue.Enabled = false;
            ddlTimeInterval.Enabled = false;
            VMFrom.Enabled = false;
            VMTo.Enabled = false;
            PresentFm.Enabled = false;
            PresentTo.Enabled = false;
            btnsubmit.Visible = false;
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            //lbltest.Text += "Saveed and Click Save";
            SaveconfirmVM("Saved");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //lbltest.Text += "Saveed and Click Confirm";
            SaveconfirmVM("Vetting Meeting Create");
        }

        protected void btnPreviewtEmail_Click(object sender, EventArgs e)
        {
            SendInviteTeamMemberEmail(0);
        }

        protected void btnSendInvitation_Click(object sender, EventArgs e)
        {
            SendInviteTeamMemberEmail(1);
            UpdateVettingMeetingStatus(m_VMID, "Invite Email Sent");
        }

        protected void btnDeletePop_Click(object sender, EventArgs e)
        {
          

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                string slqText = "";
                conn.Open();
                SqlCommand cmd;

                slqText = "Delete TB_VETTING_APPLICATION where Vetting_Meeting_ID = @m_VMID";
                //lbltest.Text += "SQL1 -" + slqText;
                cmd = new SqlCommand(slqText, conn);
                cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                slqText = "Delete TB_VETTING_MEMBER where Vetting_Meeting_ID = @m_VMID";
                //lbltest.Text += "SQL1 -" + slqText;
                cmd = new SqlCommand(slqText, conn);
                cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                slqText = "Delete TB_VETTING_MEETING where Vetting_Meeting_ID = @m_VMID";
                //lbltest.Text += "<br/>SQL2 -" + slqText;
                cmd = new SqlCommand(slqText, conn);
                cmd.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();
                }
            }
            Context.Response.Redirect("Vetting Meeting Arrangement.aspx");
       }


        protected void isVettingTeamUpdated()
        {

            //lbltest.Text = "";
            List<ListItem> CurrentEmail = getCurrentTeam();
            List<ListItem> oldEmail = getOldTeam();

            List<ListItem> toBeRemoved = getTobeRemovedMembers(oldEmail,CurrentEmail);
            List<ListItem> toBeAdded = getNewMembers(CurrentEmail,oldEmail);
            List<ListItem> emailList = new List<ListItem>();
            bool NeedLeaderUpdate = false;


           //lbltest.Text += "IsUpdatedTeam init - New Member Count - [" +toBeAdded.Count() + "] </br>";
           
           if (hdn_Old_Leader.Value != ddlVettingTeamLeader.SelectedValue)
           {
               NeedLeaderUpdate = true;
               //lbltest.Text += "IsUpdatedTeamm - Leader Updated - [" + ddlVettingTeamLeader.SelectedValue + "] </br>";
               if (! IsMemberExist(ddlVettingTeamLeader.SelectedValue))
               {
                    emailList.Add(ddlVettingTeamLeader.SelectedItem);
                    //lbltest.Text += "IsUpdatedTeam - after add team lead count - [" + toBeAdded.Count() + "] - ";

               }
               //else
               //     lbltest.Text += "IsUpdatedTeam - not added leader - ";
               UpdateTeamLeader(ddlVettingTeamLeader.SelectedItem); 
           }
           if (toBeRemoved.Count() > 0)
           {
                RemoveVettingDeclarationRecord(toBeRemoved);
                RemoveVettingMemberVettingScoreRecord(toBeRemoved);
                RemoveVettingMember(toBeRemoved);
           }
           if (toBeAdded.Count() > 0)
           {
                AddNewVettingMember(toBeAdded);
            }

           //lbltest.Text += "Old lead - [" + hdn_Old_Leader.Value + "]</br>";
           //lbltest.Text += "IsUpdatedTeam - after check add count - [" +toBeAdded.Count() + "] - ";

           if (NeedLeaderUpdate)
               toBeAdded.AddRange(emailList);


           if (toBeAdded.Count() > 0 )
           {
                //lbltest.Text += "IsUpdatedTeam - need send invite email [" + m_Meeting_status + "] </br>";
                m_Programme_Name = lstCyberportProgramme.SelectedValue;
                bool CCOn = true;
  
                // not newly created meeting 
                if (m_Meeting_status == "Invite Email Sent")
                {
                    string emailTo = "";
                    emailTo = getNewMemberEmail(toBeAdded);
                    //lbltest.Text += "IsUpdatedTeam - To email list - " + emailTo + "</br>";

                    if (m_ApplicationIsInDebug == "1")
                        CCOn = false; // turn off cc when debug

                    getNumberOfApplication();
                    getNumberOfShortedApp(m_VMID);

                    sendSingleVettingTeamEmail(emailTo, "", CCOn);
                }
            }
            //else
            //   lbltest.Text += "IsUpdatedTeam - No email [" + m_Meeting_status + "] </br>";
        }

 
        protected List<ListItem> getTobeRemovedMembers ( List<ListItem> OldMembers ,List<ListItem> CurrentMembers)
        {
            return OldMembers.Except(CurrentMembers).ToList();
            }

        protected List<ListItem> getNewMembers(List<ListItem> CurrentMembers, List<ListItem> OldMembers)
        {
            return CurrentMembers.Except(OldMembers).ToList(); ;
        }

        protected List<ListItem> getCurrentTeam()
        {
            //lbltest.Text += "<br/> Current - ";
            List<ListItem> CurrentItem = new List<ListItem>();
            //CurrentItem.Add(ddlVettingTeamLeader.SelectedItem);
            foreach (ListItem item in lstLeft.Items)
            {
                //lbltest.Text += " CurrentTeam value [" + item.Value + "] text [" + item.Text + "]; </br>";
                CurrentItem.Add(item);
            }
            return CurrentItem;
        }

        protected List<ListItem> getOldTeam()
        {
            //lbltest.Text += "<br/> Old - ";
            List<ListItem> oldEmail = new List<ListItem>();
            foreach (ListItem item in lstoldlist.Items)
            {
                //lbltest.Text += " OldTeam value [" + item.Value + "] text [" + item.Text + "];</br>";
                oldEmail.Add(item);
            }
            return oldEmail;
        }

        //protected List<SearchResult> getNewMemberEmail(List<ListItem> toBeAdded)
        protected string getNewMemberEmail(List<ListItem> toBeAdded)
        {
            //List<SearchResult> memberEmailList = new List<SearchResult>();
            //lbltest.Text += "getNewMemberEmail  Add Count - " + toBeAdded.Count() + "</br>";
            string memberEmailList = "";
            SqlConnection conn = new SqlConnection(connStr);
            //var sql = "Select Email, Vetting_Member_ID, Full_name from TB_VETTING_MEMBER_INFO where Vetting_Member_ID in (";
            var sql = "Select Email, Vetting_Member_ID, Full_name from TB_VETTING_MEMBER_INFO where Vetting_Member_ID in (";
            //int memberCount = toBeAdded.Count;
            int count = 0;
            foreach (ListItem id in toBeAdded)
            {
                //lbltest.Text += "getNewMemberEmail  Add - Team - " + id.Value + "</br>";
                if (++count == 1)
                    sql += "'" + id.Value + "'";
                else
                    sql += ",'" + id.Value + "'";
            }
            sql += ")";
            conn.Open();
            try
            {
                var command = new SqlCommand(sql, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    memberEmailList += reader.GetString(reader.GetOrdinal("Email")) + ";";
                    //memberEmailList.Add(new SearchResult
                    //{
                    //    //FieldText = reader.GetString(1),
                    //    FieldText = reader.GetString(reader.GetOrdinal("Email")),
                    //    FieldValue = reader.GetGuid(1).ToString(),
                    //    ClusterText = reader.GetString(reader.GetOrdinal("Full_name"))
                    //});
                }
                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                conn.Close();
            }

            return memberEmailList;
        }


        protected List<string> getMemberEmail(List<ListItem> toBeRemoved)
        {
            List<string> Emails = new List<string>();
            SqlConnection conn = new SqlConnection(connStr);

            var sql = "Select Email from TB_VETTING_MEMBER_INFO where Vetting_Member_ID in (";
            int itemCount = 0;

            foreach (ListItem item in toBeRemoved)
            {
                if (++itemCount == 1)
                {
                    sql += "'" + item.Value + "'";
                }
                else
                {
                    sql += ",'" + item.Value + "'";
                }
            }
            sql += ")";
            try
            {

                conn.Open();
                var command = new SqlCommand(sql, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                    Emails.Add(reader.GetValue(reader.GetOrdinal("Email")).ToString());
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return Emails;
        }
        
        protected void RemoveVettingMemberVettingScoreRecord(List<ListItem> toBeRemoved)
        {
            SqlConnection conn = new SqlConnection(connStr);
            string ScoreTable = "TB_PRESENTATION_CCMF_SCORE";

            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                ScoreTable = "TB_PRESENTATION_INCUBATION_SCORE";

            var sql = "delete " + ScoreTable + " where  Programme_ID = @ProgramID and Member_Email in (";

            List<string> memberEmail = getMemberEmail(toBeRemoved);

            lbltest.Text += "<br/> RemoveVettingMemberVettingScoreRecord memberEmail.Count [" + memberEmail.Count + "]";

            int itemCount = 0;

            foreach (string item in memberEmail)
            {
                if (++itemCount == 1)
                {
                    sql += "'" + item + "'";
                }
                else
                {
                    sql += ",'" + item + "'";
                }
            }
            sql += ")";

            conn.Open();
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@ProgramID", m_progid));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch(Exception ex)
            {
                lbltest.Text += "Exception: " + ex.Message;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void RemoveVettingDeclarationRecord(List<ListItem> toBeRemoved)
        {
            SqlConnection conn = new SqlConnection(connStr);
           
            var sql1 = "delete TB_DECLARATION_APPLICATION where Vetting_Delclaration_ID in (";
            var sql2 = "delete TB_VETTING_DECLARATION where  Vetting_Meeting_ID = @MeetingID and name in (";

            List<SearchResult> Declarations = GetMemberDeclarations(toBeRemoved);

            lbltest.Text += "<br/> remove member (Declarations.Count [" + Declarations.Count + "]";
            
            if (Declarations.Count > 0)
            {
                int itemCount = 0;
                foreach (SearchResult Declare in Declarations)
                {
                    if (++itemCount == 1)
                    {
                        sql1 += "'" + Declare.FieldValue + "'";
                        sql2 += "'" + Declare.FieldText + "'";
                    }
                    else
                    {
                        sql1 += ",'" + Declare.FieldValue + "'";
                        sql2 += ",'" + Declare.FieldText + "'";
                    }
                }
                sql1 += ")";
                sql2 += ")";

                lbltest.Text += "<br/> remove member sql [" + sql1 + "]";
                lbltest.Text += "<br/> sql2 [" + sql1 + "]";

                conn.Open();
                try
                {
                    SqlCommand cmd;
                    cmd = new SqlCommand(sql1, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    cmd = new SqlCommand(sql2, conn);
                    cmd.Parameters.Add(new SqlParameter("@MeetingID", m_VMID));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        protected List<SearchResult> GetMemberDeclarations(List<ListItem> toBeRemoved)
        {
            SqlConnection conn = new SqlConnection(connStr);
            List<SearchResult> result = new List<SearchResult>();
            var sql = "select Vetting_Delclaration_ID, Name, Member_Email "
                        + "from TB_VETTING_DECLARATION "
                        + "where Vetting_Meeting_ID = @MeetingID and name in ";
            int Counter = 0;
            foreach (ListItem Member in toBeRemoved)
            {
                if(Counter++ == 0)
                    sql += "('" + Member.Text + "'";
                else
                    sql += ",'" + Member.Text + "'";
            }
            sql +=")";

            try
            {

                conn.Open();
                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("@MeetingID", m_VMID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                    SearchResult Mem = new SearchResult();
                    Mem.FieldValue = reader.GetValue(reader.GetOrdinal("Vetting_Delclaration_ID")).ToString();
                    Mem.FieldText = reader.GetValue(reader.GetOrdinal("Name")).ToString();
                    Mem.ClusterText = reader.GetValue(reader.GetOrdinal("Member_Email")).ToString();
                    result.Add(Mem);
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return result;
                
        }

        protected void RemoveVettingMember(List<ListItem> toBeRemoved)
        {
            //lbltest.Text += "<br/> remove member sql [";
            SqlConnection conn = new SqlConnection(connStr);
            var sql = "delete from TB_VETTING_MEMBER where Vetting_Meeting_ID = @Vetting_Meeting_ID and Vetting_Member_ID ";
            int itemCount = toBeRemoved.Count();
            if (itemCount == 1)
                sql += " = '" + toBeRemoved.First().Value + "'";
            else
            {
                sql += "in (";
                foreach (ListItem id in toBeRemoved)
                {
                    sql += "'" + id.Value + "'";
                    if (--itemCount > 0)
                        sql += ",";
                    else
                        sql += ")";
                }
            }
            //lbltest.Text += "<br/> remove member sql ["+ sql + "]";
            conn.Open();
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_VMID));

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void UpdateTeamLeader(ListItem newLeader)
        {
            string LeaderID = newLeader.Value;
            //lbltest.Text += "Update Team Leader [" + newLeader.Text + "] old [" + m_oldLeader.Text + "]";
            
            UpdateMeetingTeamLeader(ddlVettingTeamLeader.SelectedItem.Text, m_VMID);
            
            //lbltest.Text += "<insert>";
            if (!string.IsNullOrEmpty(hdn_Old_Leader.Value))
            {
                RemoveTB_VETTING_MEMBER(m_VMID, hdn_Old_Leader.Value);
            }
            InsertTB_VETTING_MEMBER(m_VMID, LeaderID, true);
            
        }

        protected void AddNewVettingMember(List<ListItem> toBeAdded)
        {

            //string emailTo = "";
            getSYSTEMPARAMETER();
            getMeeintgInformation();
            getProgramIntakeApplicationDeadline();
            string MemberID = "";

            string MeetingID = m_VMID;
            //bool leader = false;
            //bool CCOn = true;
            
            foreach (ListItem id in toBeAdded)
            {
                MemberID = id.Value;
                if (!IsMemberExist(MemberID))
                {
                //lbltest.Text += "<br/> AddNewVettingMember - Meeting ID [" + MeetingID + "] Leader [" + leader + "] ID [" + MemberID + "]";
                    InsertTB_VETTING_MEMBER(MeetingID, MemberID, false);
                }
                else
                {
                    //lbltest.Text += "<br/> AddNewVettingMember - Update Meeting ID [" + MeetingID + "] Leader [" + leader + "] ID [" + MemberID + "]";
                    UpdateTeamMember(MemberID, false);
                }
            }

            //if (m_Meeting_status == "Invitation Email Sent")
            //{
            //    emailTo = getNewMemberEmail(toBeAdded);
            //    if (m_ApplicationIsInDebug == "1")
            //        CCOn = false; // turn off cc when debug

            //    getNumberOfApplication();
            //    getNumberOfShortedApp(m_VMID);

            //    //sendSingleVettingTeamEmail(emailTo, "", true);
            //    sendSingleVettingTeamEmail(emailTo, "", CCOn);
            //}
        }


        protected void SaveconfirmVM(String MeetingStatus)
        {
            if (Page.IsValid)
            {
                if (btnConfirm.Text == "Save")
                {
                    //lbltest.Text = "Meeting is alreadty confirmed Update on vetting Team";

                    m_ProgrammeName = lstCyberportProgramme.Text;
                    m_IntakeNum = lstIntakeNumber.Text;
                    m_progid = get_proid(m_ProgrammeName, m_IntakeNum);
                    if (string.IsNullOrEmpty(m_VMID))
                    {
                        m_VMID =  HttpContext.Current.Request.QueryString["VMID"];
                    }

                    isVettingTeamUpdated();

                    Context.Response.Redirect("Vetting Meeting Arrangement.aspx");
 
                }
                else
                {
                    SqlConnection conn = new SqlConnection(connStr);
                    getSYSTEMPARAMETER();
                        // need to: 
                        // Update 1) TB_Vetting_Meeting, 2) Vetting team member store in TB_VETTING_MEMBER, 3) List of Application store in TB_VETTING_APPLICATION

                        // * Validation: * //
                        if (Valid(MeetingStatus) == false)
                        {
                            return;
                        }
                    try
                    {
                        string systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //SPContext.Current.Web.CurrentUser.Name.ToString();

                        m_ProgrammeName = lstCyberportProgramme.Text;
                        m_IntakeNum = lstIntakeNumber.Text;
                        m_progid = get_proid(m_ProgrammeName, m_IntakeNum);

                        DateTime VMDate = DatePicker.SelectedDate;
                        DateTime VMDeadline = new DateTime(SubDeadlinePicker.SelectedDate.Year, SubDeadlinePicker.SelectedDate.Month, SubDeadlinePicker.SelectedDate.Day, SubDeadlineTimePicker.SelectedDate.Hour, SubDeadlineTimePicker.SelectedDate.Minute, 59);
                        DateTime VMConfirmDeadline = new DateTime(ConfirmDeadlinePicker.SelectedDate.Year, ConfirmDeadlinePicker.SelectedDate.Month, ConfirmDeadlinePicker.SelectedDate.Day, ConfirmDeadlineTimePicker.SelectedDate.Hour, ConfirmDeadlineTimePicker.SelectedDate.Minute, 59);
                        DateTime VMParkingDeadline = ParkingDeadlinePicker.SelectedDate;
                        string Venue = TxtVenue.Text;
                        DateTime Vetting_Meeting_From = VMDate.Date + VMFrom.SelectedDate.TimeOfDay;
                        DateTime Vetting_Meeting_To = VMDate.Date + VMTo.SelectedDate.TimeOfDay;
                        DateTime Presentation_From = VMDate.Date + PresentFm.SelectedDate.TimeOfDay;
                        DateTime Presentation_To = VMDate.Date + PresentTo.SelectedDate.TimeOfDay;

                        string Vetting_Team_Leader = ddlVettingTeamLeader.SelectedItem.Text;

                        string Modified_By = systemuser;
                        string Created_By = systemuser;

                        int VTMbrCnt = lstLeft.Items.Count;          // get the member counts
                        int No_of_Attendance = VTMbrCnt + 1;

                        if (!string.IsNullOrEmpty(hdn_VMID.Value))
                        {
                            m_VMID = hdn_VMID.Value;
                        }

                        // All checked OK, starting to create:
                        Guid vm_uid;
                        if (string.IsNullOrEmpty(m_VMID))
                        {
                            vm_uid = Guid.NewGuid();
                        }
                        else
                        {
                            vm_uid = new Guid(m_VMID);
                        }


                        // insert new record into TB_VETTING_MEETING:
                        // get a new ID first:
                        if (!string.IsNullOrEmpty(m_VMID))
                        {
                            //update
                            string MettingStatuschange = "";
                            if (Convert.ToInt32(m_VMStatus) == 1)
                            {
                                MettingStatuschange = getVettingMeetingStatus(vm_uid.ToString());
                            }
                            else
                            {
                                MettingStatuschange = MeetingStatus;
                            }

                            UpdateTBVETTINGMEETING(vm_uid.ToString(), m_progid, VMDate, VMDeadline, VMConfirmDeadline, VMParkingDeadline, Venue, Vetting_Meeting_From, Vetting_Meeting_To, Presentation_From, Presentation_To, Vetting_Team_Leader, No_of_Attendance.ToString(), ddlTimeInterval.SelectedValue, MettingStatuschange);

                            isVettingTeamUpdated();

                            hdn_VMID.Value = vm_uid.ToString();
                        }
                        else
                        {
                            //insert
                            InsertTBVETTINGMEETING(vm_uid.ToString(), m_progid, VMDate, VMDeadline, VMConfirmDeadline, VMParkingDeadline, Venue, Vetting_Meeting_From, Vetting_Meeting_To, Presentation_From, Presentation_To, Vetting_Team_Leader, No_of_Attendance.ToString(), ddlTimeInterval.SelectedValue, MeetingStatus);
                            m_VMID = vm_uid.ToString();
                            AddNewVettingMember(getCurrentTeam());
                            UpdateTeamLeader(ddlVettingTeamLeader.SelectedItem);
                            hdn_VMID.Value = m_VMID;

                        }
                    }
                    finally
                    {

                        if (conn != null)
                        {
                            conn.Close();
                        }
                        Context.Response.Redirect("Vetting Meeting Arrangement.aspx");
                    }
                }
            }
        }

        protected void SendInviteTeamMemberEmail(int action)
        {
            string emailTo = "";
            getSYSTEMPARAMETER();
            getMeeintgInformation();
            getNumberOfApplication();
            getNumberOfShortedApp(m_VMID);
            getProgramIntakeApplicationDeadline();
            bool ccOn = false;
            if(action == 0) // preview
            {
                if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                    emailTo = m_PreviewCPIPInvitEmailTo;
                else
                    emailTo = m_PreviewCCMFInvitEmailTo;
                //lbltest.Text += "Preview - [" + emailTo + "]";
            }
            else  // invitation email
            {
                //if (m_ApplicationIsInDebug == "1")
                //    emailTo = m_ApplicationDebugEmailSentTo;
                //else
                //{
                //    emailTo = GetVettingTeamEmailList(m_VMID);
                //    ccOn = true;
                //}
               //lbltest.Text += "Inviate - [" + emailTo + "]";
                emailTo = GetVettingTeamEmailList(m_VMID);
                if (m_ApplicationIsInDebug != "1")
                ccOn = true;
                else
                    lblInviteStatus.Text = "Invitation Email already sent";
            }
            sendSingleVettingTeamEmail(emailTo, "Vetting Team", ccOn);
            //if(ccOn)
            //    lblInviteStatus.Text = "Invitation Email already sent";

        }

        protected bool IsMemberExist(string MemberID)
        {
            bool exist = false;
            //lbltest.Text += "IsMemberExist Meb_ID [" + MemberID + "] [" +  m_VMID + "]</br>";
            ConnectOpen();
            try
            {
                var sqlString = "Select Vetting_Member_ID from TB_VETTING_MEMBER where Vetting_Member_ID = @MemberID and Vetting_Meeting_ID = @MeetingID";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@MemberID", MemberID));
                command.Parameters.Add(new SqlParameter("@MeetingID", m_VMID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                    exist = !reader.IsDBNull(0);
                }
                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            //lbltest.Text += "IsMemberExist  = D [" + exist + "] </br>";

            return exist;
        }

        protected void UpdateMeetingTeamLeader(string leader, string vm_id)
        {
            ConnectOpen();
            try
            {
                var sqlString = "Update TB_VETTING_MEETING set Vetting_Team_Leader = @leader Where Vetting_Meeting_ID = @vm_id";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.AddWithValue("@leader", leader);
                command.Parameters.AddWithValue("@vm_id", vm_id);
                
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

        }

        protected void UpdateTeamMember(string MemberID, bool Leader)
        {
            ConnectOpen();
            try
            {
                var sqlString = "Update TB_VETTING_Member set isLeader = @leader where Vetting_Member_ID = @MemberID and Vetting_Meeting_ID = @MeetingID";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@leader", Leader));
                command.Parameters.Add(new SqlParameter("@MemberID", MemberID));
                command.Parameters.Add(new SqlParameter("@MeetingID", m_VMID));
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

        }

        protected string getVettingMeetingStatus(string m_Vetting_Meeting_ID) 
        {
            string m_result = "";

            ConnectOpen();
            try
            {
                var sqlString = "SELECT Meeting_status "
                              + "FROM TB_VETTING_MEETING where "
                              + "Vetting_Meeting_ID = @Vetting_Meeting_ID ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_Vetting_Meeting_ID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                    m_result = reader.GetValue(reader.GetOrdinal("Meeting_status")).ToString();
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            return m_result;
        }

        public void getMeeintgInformation()
        {
            m_Programme_Name = lstCyberportProgramme.SelectedValue;
            m_Intake_Number = lstIntakeNumber.SelectedValue;
            m_VettingDate = DatePicker.SelectedDate;
            m_VettingDeadline = new DateTime(SubDeadlinePicker.SelectedDate.Year, SubDeadlinePicker.SelectedDate.Month, SubDeadlinePicker.SelectedDate.Day, SubDeadlineTimePicker.SelectedDate.Hour, SubDeadlineTimePicker.SelectedDate.Minute, 59);
            m_ConfirmDeadline = new DateTime(ConfirmDeadlinePicker.SelectedDate.Year, ConfirmDeadlinePicker.SelectedDate.Month, ConfirmDeadlinePicker.SelectedDate.Day, ConfirmDeadlineTimePicker.SelectedDate.Hour, ConfirmDeadlineTimePicker.SelectedDate.Minute, 59);
            m_PakingDealine = ParkingDeadlinePicker.SelectedDate;
            m_VettingVenue = TxtVenue.Text;
            m_VettingMettingFrom = VMFrom.SelectedDate;
            m_VettingMeetingTo = VMTo.SelectedDate;
            m_Presentation_From = PresentFm.SelectedDate;
            m_Presentation_To = PresentTo.SelectedDate;


        }
        protected void getNumberOfShortedApp(string MeetingID)
        {
            ConnectOpen();
            try
            {
                var sqlString = "SELECT Count(Vetting_Application_ID) as noOfShortListed FROM TB_VETTING_APPLICATION  where Vetting_Meeting_ID = @MeetingID AND Application_Number <> 'Time Break'";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@MeetingID", MeetingID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    m_totalShortlisted = reader.GetInt32(reader.GetOrdinal("noOfShortListed"));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

        }
        protected void getNumberOfApplication()
        {
            ConnectOpen();
            try
            {
                var sqlString = "";
                if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                {
                    //CPIP
                    sqlString = " SELECT count(Incubation_ID) as NoOfApplicaiton " +
                                " FROM TB_INCUBATION_APPLICATION " +
                                " WHERE Programme_ID = @Programm_ID " +
                                "and Status in ('Complete Screening', 'Presentation Withdraw', 'Awarded')";
                }
                else
                {
                    //CCMF
                    sqlString = " SELECT Count(CCMF_ID) as NoOfApplicaiton " +
                                " FROM TB_CCMF_APPLICATION " +
                                " WHERE Programme_ID = @Programm_ID "+
                                " and Status in ('Complete Screening', 'Presentation Withdraw', 'Awarded') ";
                }
                //lbltest.Text += "getNumberOfApplication - prog  [" + m_Programme_Name + "] intake [" + lstIntakeNumber.SelectedValue +"]";


                string progID = get_proid(m_Programme_Name, lstIntakeNumber.SelectedValue);
                
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Programm_ID", progID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    m_totalEligibleApplication = reader.GetInt32(reader.GetOrdinal("NoOfApplicaiton"));
                }

                reader.Dispose();
                command.Dispose();
                //lbltest.Text = "<br/> getOnApp PrgID [" + m_Programme_Name + "] intake [" + lstIntakeNumber.SelectedValue + "] count [" + m_totalEligibleApplication +"]";
            }
            finally
            {
                ConnectClose();
        }

        }

        protected void getProgramIntakeApplicationDeadline()
        {
            ConnectOpen();
            try
            {
                //var sqlString = "select Programme_Name,Intake_Number from TB_PROGRAMME_INTAKE where Programme_ID='" + m_progid + "';";
                var sqlString = "select isnull(Application_Deadline,'') as Application_Deadline from TB_PROGRAMME_INTAKE tpi"
                                + " where tpi.Programme_Name = @Programme_Name and tpi.Intake_Number=@Intake_Number";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Programme_Name", lstCyberportProgramme.SelectedValue));
                command.Parameters.Add(new SqlParameter("@Intake_Number", lstIntakeNumber.SelectedValue));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //lblProName.Text = reader.GetValue(reader.GetOrdinal("Programme_Name")).ToString();
                    m_ApplicationDeadline = reader.GetDateTime(reader.GetOrdinal("Application_Deadline"));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

        }

        private Boolean Valid(string m_MeetingStatus)
        {
            DateTime VMDate = DatePicker.SelectedDate;
            DateTime Vetting_Meeting_From = VMDate.Date + VMFrom.SelectedDate.TimeOfDay;
            DateTime Vetting_Meeting_To = VMDate.Date + VMTo.SelectedDate.TimeOfDay;
            DateTime Presentation_From = VMDate.Date + PresentFm.SelectedDate.TimeOfDay;
            DateTime Presentation_To = VMDate.Date + PresentTo.SelectedDate.TimeOfDay;
            DateTime VettingDeadline = new DateTime(SubDeadlinePicker.SelectedDate.Year, SubDeadlinePicker.SelectedDate.Month, SubDeadlinePicker.SelectedDate.Day, SubDeadlineTimePicker.SelectedDate.Hour, SubDeadlineTimePicker.SelectedDate.Minute, 59);
            DateTime ConfirmDeadline = new DateTime(ConfirmDeadlinePicker.SelectedDate.Year, ConfirmDeadlinePicker.SelectedDate.Month, ConfirmDeadlinePicker.SelectedDate.Day, ConfirmDeadlineTimePicker.SelectedDate.Hour, ConfirmDeadlineTimePicker.SelectedDate.Minute, 59);
            DateTime ParkingDeadline = ParkingDeadlinePicker.SelectedDate;

            // * Validation: * //
            var status = true;

            if (m_Meeting_status != "Invite Email Sent" && m_Meeting_status != "Email Sended" )
            {

                if (VMDate.Date <= DateTime.Now.Date)
                {
                    lblerrordateinfo.Text = "Vetting Meeting should not allow create meeting with the date today or the date before";
                    status = false;
                }
                else
                {
                    lblerrordateinfo.Visible = false;
                }

                if (Vetting_Meeting_From >= Vetting_Meeting_To)
                {
                    validate_msg1.Text = "Vetting Meeting To time should be later than From time.";
                    validate_msg1.Visible = true;
                    status = false;
                }
                else
                {
                    validate_msg1.Visible = false;
                }

                
                if (Presentation_From >= Presentation_To)
                {

                    validate_msg2.Text = "Presentation To time should be later than From time.";
                    validate_msg2.Visible = true;
                    status = false;
                }
                else
                {
                    validate_msg2.Visible = false;
                }

                if (!((Vetting_Meeting_To <= Presentation_From))) // && (Presentation_To <= Vetting_Meeting_To)))
                {

                    validate_msgMP.Text = "Presentation time should after Vetting Meeting time.";
                    validate_msgMP.Visible = true;
                    status = false;
                }
                else
                {
                    validate_msgMP.Visible = false;
                }

                if (ConfirmDeadline >= VMDate)
                {
                    lblerrorCondateinfo.Text = "Invitation confirm deadline should be before Vetting Meeting date.";
                    status = false;
                }
                else
                {
                    lblerrorCondateinfo.Visible = false;
                }

                if (VettingDeadline >= VMDate)
                {
                    lblerrorSubdateinfo.Text = "Presentation Slide submission deadline should be before Vetting Meeting date ";
                    status = false;
                }
                else
                {
                    lblerrorSubdateinfo.Visible = false;
                }
                if (ParkingDeadline >= VMDate)
                {
                    lblerrorParkingdateinfo.Text = "Parking request deadline should be before Vetting Meeting date.";
                    status = false;
                }
                else
                {
                    lblerrorParkingdateinfo.Visible = false;
                }
                // Create by Andy
                if (m_MeetingStatus == "Vetting Meeting Create" && lstLeft.Items.Count == 0)
                {
                    validate_msg3.Text = "Vetting Team Member cannot empty when you confirm.";
                    //validate_msg3.Text = m_VMID + m_MeetingStatus;
                    validate_msg3.Visible = true;
                    status = false;
                }
                else
                {
                    validate_msg3.Visible = false;
                }

                if (lstCyberportProgramme.SelectedValue == m_SelectProgrammeName)
                {
                    lblErrorlstCyberportProgramme.Text = "Please select a valid Programme Name";
                    status = false;
                }
                else
                {
                    lblErrorlstCyberportProgramme.Visible = false;
                }

                if (lstIntakeNumber.SelectedValue == m_SelectIntakeNumber)
                {
                    lblErrorIntakeNumber.Text = "Please select a valid Intake Number";
                    status = false;
                }
                else
                {
                    lblErrorIntakeNumber.Visible = false;
                }
            }


            //lbltest.Text = ddlVettingTeamLeader.SelectedValue;
            if (ddlVettingTeamLeader.SelectedValue == m_SelectVettingTeamLeaderName)
            {
                lblerrorVettingTeamLeader.Text = "Please select a Team Leader";
                status = false;
            }
            else
            {
                lblerrorVettingTeamLeader.Visible = false;
            }

            return status;
        }

        private void InsertTBVETTINGMEETING(String Vetting_Meeting_ID, String Programme_ID, DateTime Date, DateTime Deadline, DateTime ConfirmDeadline, DateTime ParkingDeadline, String Venue, DateTime Vetting_Meeting_From, DateTime Vetting_Meeting_To, DateTime Presentation_From, DateTime Presentation_To, String Vetting_Team_Leader, String No_of_Attendance, String Time_Interval, String Meeting_status)
        {
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "insert into TB_VETTING_MEETING(Vetting_Meeting_ID, Programme_ID, Date, Deadline, ConfirmDeadline, ParkingDeadline, Venue, Vetting_Meeting_From, Vetting_Meeting_To, Presentation_From, Presentation_To, " +
                                                 " Vetting_Team_Leader, No_of_Attendance, Created_By, Created_Date, Modified_By, Modified_Date,Time_Interval,Meeting_status) VALUES ("
                            + "@Vetting_Meeting_ID, "
                            + "@Programme_ID, "
                            + "@Date, "
                            + "@Deadline, "
                            + "@ConfirmDeadline, "
                            + "@ParkingDeadline, "
                            + "@Venue, "
                            + "@Vetting_Meeting_From, "
                            + "@Vetting_Meeting_To, "
                            + "@Presentation_From, "
                            + "@Presentation_To, "
                            + "@Vetting_Team_Leader, "
                            + "@No_of_Attendance, "
                            + "@Created_By, "
                            + "GETDATE() , "
                            + "@Created_By, "
                            + "GETDATE(),"
                            + "@Time_Interval, "
                            + "@Meeting_status "
                            + ")";

            conn.Open();
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                cmd.Parameters.Add(new SqlParameter("@Date", Date));
                cmd.Parameters.Add(new SqlParameter("@Deadline", Deadline));
                cmd.Parameters.Add(new SqlParameter("@ConfirmDeadline", ConfirmDeadline));
                cmd.Parameters.Add(new SqlParameter("@ParkingDeadline", ParkingDeadline));
                cmd.Parameters.Add(new SqlParameter("@Venue", Venue));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_From", Vetting_Meeting_From));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_To", Vetting_Meeting_To));
                cmd.Parameters.Add(new SqlParameter("@Presentation_From", Presentation_From));
                cmd.Parameters.Add(new SqlParameter("@Presentation_To", Presentation_To));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Team_Leader", Vetting_Team_Leader));
                cmd.Parameters.Add(new SqlParameter("@No_of_Attendance", No_of_Attendance));
                cmd.Parameters.Add(new SqlParameter("@Created_By", SPContext.Current.Web.CurrentUser.Name.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Time_Interval", Time_Interval));
                cmd.Parameters.Add(new SqlParameter("@Meeting_status", Meeting_status));

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        private void UpdateVettingMeetingStatus(string Vetting_Meeting_ID, string status)
        {
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "update TB_VETTING_MEETING set Meeting_status = @status, Modified_date = GETDATE(), Modified_By = @CurrentUser where Vetting_Meeting_ID = @Vetting_Meeting_ID";
            conn.Open();
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@status", status));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@CurrentUser", SPContext.Current.Web.CurrentUser.Name.ToString()));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void UpdateTBVETTINGMEETING(String Vetting_Meeting_ID, String Programme_ID, DateTime Date, DateTime Deadline, DateTime ConfirmDeadline, DateTime ParkingDeadline, String Venue, DateTime Vetting_Meeting_From,
            DateTime Vetting_Meeting_To, DateTime Presentation_From, DateTime Presentation_To, String Vetting_Team_Leader, String No_of_Attendance, String Time_Interval, String Meeting_status)
        {
            SqlConnection conn = new SqlConnection(connStr);
            string sql = "update TB_VETTING_MEETING set "
                            + "Programme_ID = @Programme_ID, "
                            + "Date = @Date, "
                            + "Deadline = @Deadline, "
                            + "ConfirmDeadline = @ConfirmDeadline, "
                            + "ParkingDeadline = @ParkingDeadline, "
                            + "Venue = @Venue, "
                            + "Vetting_Meeting_From = @Vetting_Meeting_From, "
                            + "Vetting_Meeting_To = @Vetting_Meeting_To, "
                            + "Presentation_From = @Presentation_From, "
                            + "Presentation_To = @Presentation_To, "
                            + "Vetting_Team_Leader = @Vetting_Team_Leader, "
                            + "No_of_Attendance = @No_of_Attendance, "
                            + "Modified_By = @Created_By, "
                            + "Modified_Date = GETDATE(),"
                            + "Time_Interval = @Time_Interval, "
                            + "Meeting_status = @Meeting_status "
                            + " where Vetting_Meeting_ID = @Vetting_Meeting_ID";

            conn.Open();
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@Programme_ID", Programme_ID));
                cmd.Parameters.Add(new SqlParameter("@Date", Date));
                cmd.Parameters.Add(new SqlParameter("@Deadline", Deadline));
                cmd.Parameters.Add(new SqlParameter("@ConfirmDeadline", ConfirmDeadline));
                cmd.Parameters.Add(new SqlParameter("@ParkingDeadline", ParkingDeadline));
                cmd.Parameters.Add(new SqlParameter("@Venue", Venue));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_From", Vetting_Meeting_From));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_To", Vetting_Meeting_To));
                cmd.Parameters.Add(new SqlParameter("@Presentation_From", Presentation_From));
                cmd.Parameters.Add(new SqlParameter("@Presentation_To", Presentation_To));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Team_Leader", Vetting_Team_Leader));
                cmd.Parameters.Add(new SqlParameter("@No_of_Attendance", No_of_Attendance));
                cmd.Parameters.Add(new SqlParameter("@Created_By", SPContext.Current.Web.CurrentUser.Name.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Time_Interval", Time_Interval));
                cmd.Parameters.Add(new SqlParameter("@Meeting_status", Meeting_status));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void RemoveTB_VETTING_MEMBER(string Vetting_Meeting_ID, string Vetting_Member_ID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE  FROM TB_VETTING_MEMBER WHERE Vetting_Meeting_ID = @vm_id and Vetting_Member_ID = @member_id";
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@vm_id", Vetting_Meeting_ID);
                    command.Parameters.AddWithValue("@member_id", Vetting_Member_ID);
                    command.ExecuteNonQuery();
                conn.Close();
                    command.Dispose();
                conn.Dispose();
            }
        }
        }

        //private void RemoveTB_VETTING_MEMBER(string Vetting_Meeting_ID, string Vetting_Member_ID)
        //{
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        string sql = "DELETE  FROM TB_VETTING_MEMBER WHERE Vetting_Meeting_ID = @vm_id and Vetting_Member_ID = @member_id";
        //        using (SqlCommand command = new SqlCommand(sql, conn))
        //        {
        //            conn.Open();
        //            command.Parameters.AddWithValue("@vm_id", Vetting_Meeting_ID);
        //            command.Parameters.AddWithValue("@member_id", Vetting_Member_ID);
        //            command.ExecuteNonQuery();
        //            conn.Close();
        //            command.Dispose();
        //            conn.Dispose();
        //        }
        //    }
        //}

        private void InsertTB_VETTING_MEMBER(String Vetting_Meeting_ID, String Vetting_Member_ID, bool isLeader)
        {
            //lbltest.Text += "<br/> insert Vetting_Meeting_ID [" + Vetting_Meeting_ID + "] Vetting_Member_ID [" + Vetting_Member_ID + "] isLeader [" + isLeader + "]";
            SqlConnection conn = new SqlConnection(connStr);
            var sql = "INSERT INTO TB_VETTING_MEMBER(Vetting_Meeting_ID, Vetting_Member_ID,isLeader) VALUES("
                                  + "@Vetting_Meeting_ID, "
                                  + "@Vetting_Member_ID, "
                                  + "@isLeader);";
            conn.Open();
            try
            {
                SqlCommand cmd;
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", Vetting_Meeting_ID));
                cmd.Parameters.Add(new SqlParameter("@Vetting_Member_ID", Vetting_Member_ID));
                cmd.Parameters.Add(new SqlParameter("@isLeader", isLeader));

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {

                conn.Close();
                conn.Dispose();
            }
        }

        private void InsertSharePointVettingMeeting(String Vetting_Meeting_ID, String Programme_ID, String Programme_Name, String Intake_Number)
        {
            // add to SP list
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Vetting Meeting"];
            SPListItem oListItem = oList.AddItem();

            oListItem["Vetting_Meeting_ID"] = Vetting_Meeting_ID;
            oListItem["Programme_ID"] = Programme_ID;
            oListItem["Programme_Name"] = Programme_Name;
            oListItem["Intake_Number"] = Intake_Number;

            oListItem.Web.AllowUnsafeUpdates = true;
            oListItem.Update();
            oListItem.Web.AllowUnsafeUpdates = false;
            //  end add to SP list
        }

        protected void updateSharePointVettingMeeting(String Vetting_Meeting_ID, String Programme_ID, String Programme_Name, String Intake_Number)
        {
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Vetting Meeting"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + Vetting_Meeting_ID + "</Value></Eq> </Where>";

            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                oListItem["Programme_ID"] = Programme_ID;
                oListItem["Programme_Name"] = Programme_Name;
                oListItem["Intake_Number"] = Intake_Number;

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }
        }

        private void SetGridViewColumnOrder()
        {
            GridViewColumnOrder = new Dictionary<String, int>();
            GridViewColumnOrder.Add("Select", 0);
            GridViewColumnOrder.Add("ApplicationNo", 1);
            GridViewColumnOrder.Add("CompanyName", 2);
            GridViewColumnOrder.Add("ProjectName", 3);
            GridViewColumnOrder.Add("ProgrammeType", 4);
            GridViewColumnOrder.Add("ApplicationType", 5);
            GridViewColumnOrder.Add("Cluster", 6);
            GridViewColumnOrder.Add("Status", 7);
            GridViewColumnOrder.Add("Shortlisted", 8);
            GridViewColumnOrder.Add("PresentationDetails", 9);
        }



        public class AppLstResult
        {
            public Boolean selection { get; set; }
            public string Application_Number { get; set; }
            public string CoOrPrjName { get; set; }
            public string Cluster { get; set; }
            public string ProgType { get; set; }
            public string Status { get; set; }

        }


        protected void lstCyberportProgramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_Intakeddlst();
            //BindDataCluster();
            //ClearddlGridviewSelected();
            //gen_AppList();
        }

        protected void lstIntakeNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindDataCluster();
            //ClearddlGridviewSelected();
            //gen_AppList();
        }

        protected void DatePicker_DateChanged(object sender, EventArgs e)
        {
            // need to assign the selected date to all other time input fields b'cos time only, assume the same day.

        }

        protected void VMFrom_DateChanged(object sender, EventArgs e)
        {
            // clear the msg and make it invisible again
            validate_msg1.Text = "";
            validate_msg1.Visible = false;
        }

        protected void VMTo_DateChanged(object sender, EventArgs e)
        {
            // clear the msg and make it invisible again
            validate_msg1.Text = "";
            validate_msg1.Visible = false;
        }

        protected void PresentFm_DateChanged(object sender, EventArgs e)
        {
            // clear the msg and make it invisible again
            validate_msg2.Text = "";
            validate_msg2.Visible = false;
        }

        protected void PresentTo_DateChanged(object sender, EventArgs e)
        {
            // clear the msg and make it invisible again
            validate_msg2.Text = "";
            validate_msg2.Visible = false;
        }

        protected void ConfirmDeadlineTimePicker_DateChanged(object sender, EventArgs e)
        {

        }

        protected void SubDeadlineTimePicker_DateChanged(object sender, EventArgs e)
        {

        }

        public void MsgBox(String ex, Page pg, Object obj)
        {
            string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
            Type cstype = obj.GetType();
            ClientScriptManager cs = pg.ClientScript;
            cs.RegisterClientScriptBlock(cstype, s, s.ToString());
        }


        protected string get_proid(string Programme_Name, string Intake_Number)
        {
            string m_prog_id = "";
            string sql_string = ""; using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql_string = "SELECT * FROM TB_PROGRAMME_INTAKE where Programme_Name= @Programme_Name and Intake_Number= @Intake_Number ";
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
            return m_prog_id;
        }

        protected void ListBoxSorting(ref ListBox listBox)
        {
            List<ListItem> list = new List<ListItem>(listBox.Items.Cast<ListItem>());
            list = list.OrderBy(x => x.Text).ToList<ListItem>();
            listBox.Items.Clear();
            listBox.Items.AddRange(list.ToArray<ListItem>());
        }

        protected void DropDownSorting(ref DropDownList ddl)
        {
            List<ListItem> list = new List<ListItem>(ddl.Items.Cast<ListItem>());
            list = list.OrderBy(x => x.Text).ToList<ListItem>();
            ddl.Items.Clear();
            ddl.Items.AddRange(list.ToArray<ListItem>());
        }

        protected void btnRight_Click(object sender, EventArgs e)
        {
            lstRight.Items.AddRange(
                        (from ListItem LI in lstLeft.Items where LI.Selected select LI).ToArray<ListItem>());

            ListBoxSorting(ref lstRight);

            for (int i = 0; i < lstLeft.Items.Count; i++)
            {
                if (lstLeft.Items[i].Selected)
                {
                    lstLeft.Items[i].Selected = false;
                    //ddlVettingTeamLeader.Items.Add(lstLeft.Items[i]);
                    lstLeft.Items.RemoveAt(i);
                    i--;
                }
            }

            DropDownSorting(ref ddlVettingTeamLeader);
        }

        protected void btnLeft_Click(object sender, EventArgs e)
        {
            lstLeft.Items.AddRange(
                        (from ListItem LI in lstRight.Items where LI.Selected select LI).ToArray<ListItem>());

            ListBoxSorting(ref lstLeft);

            for (int i = 0; i < lstRight.Items.Count; i++)
            {
                if (lstRight.Items[i].Selected)
                {
                    lstRight.Items.RemoveAt(i);
                    i--;
                }
            }

            lstLeft.SelectedIndex = -1;
            foreach (ListItem item in lstLeft.Items)
            {
                if (ddlVettingTeamLeader.Items.Contains(item))
                    ddlVettingTeamLeader.Items.Remove(item);
            }

        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("/SitePages/Vetting Meeting Arrangement.aspx");

        }

        private void setdefaultValue()
        {
            VMFrom.SelectedDate = VMFrom.SelectedDate;
            VMTo.SelectedDate = VMTo.SelectedDate;
            PresentFm.SelectedDate = PresentFm.SelectedDate;
            PresentTo.SelectedDate = PresentTo.SelectedDate;
            ConfirmDeadlineTimePicker.SelectedDate = ConfirmDeadlineTimePicker.SelectedDate;
            SubDeadlineTimePicker.SelectedDate = SubDeadlineTimePicker.SelectedDate;
        }

        protected void CustomValidatorVettingTeamMember_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var status = true;
            foreach (ListItem Items in lstLeft.Items)
            {
                if (ddlVettingTeamLeader.SelectedItem.Text == Items.Text)
                {
                    status = false;
                    break;
                }
            }

            args.IsValid = status;
        }

        protected void gvAppl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SetGridViewColumnOrder();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                var PresentationDetails = e.Row.Cells[GridViewColumnOrder["PresentationDetails"]].Text;
                var selectbox = e.Row.FindControl("CheckBoxselection") as CheckBox;
                if (PresentationDetails.Trim() != "&nbsp;")
                {
                    selectbox.Visible = false;
                }
                else
                {
                    selectbox.Visible = true;
                }

            }
        }

        protected string GetVettingTeamEmailList(string m_Vetting_Meeting_ID)
        {
            //lbltest.Text  += "VettingTeamEmailList<br/>";
            string emailToList = "";

            ConnectOpen();
            try
            {
                var sqlString = "";
                sqlString = "SELECT dbo.TB_VETTING_MEMBER.Vetting_Meeting_ID, dbo.TB_VETTING_MEMBER.Vetting_Member_ID, dbo.TB_VETTING_MEMBER.isLeader, dbo.TB_VETTING_MEMBER_INFO.Email, "
                    + "dbo.TB_VETTING_MEMBER_INFO.Full_Name, dbo.TB_VETTING_MEMBER_INFO.Registered, dbo.TB_VETTING_MEMBER_INFO.Disabled "
                    + "FROM dbo.TB_VETTING_MEMBER INNER JOIN dbo.TB_VETTING_MEMBER_INFO ON dbo.TB_VETTING_MEMBER.Vetting_Member_ID = dbo.TB_VETTING_MEMBER_INFO.Vetting_Member_ID "
                    + "WHERE dbo.TB_VETTING_MEMBER_INFO.Disabled = 0 and dbo.TB_VETTING_MEMBER.Vetting_Meeting_ID = @m_Vetting_Meeting_ID ";

                //lbltest.Text += sqlString;

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_Vetting_Meeting_ID", m_Vetting_Meeting_ID));

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    emailToList += reader.GetValue(reader.GetOrdinal("Email")).ToString() + ";";
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            return emailToList;
        }


        protected void sendSingleVettingTeamEmail(string m_EmailList, string fullName, bool ccOn)
        {

            StringDictionary headers = new StringDictionary();

            m_mail = m_EmailList;
            m_subject = "";
            if (m_ApplicationIsInDebug == "1")
            {
                //m_body = "Email Send to : " + m_EmailList + "<br/></br>";
                m_body = "Email Send to : " + m_mail + "<br/></br>";
                m_mail = m_ApplicationDebugEmailSentTo;
                if (m_Programme_Name.Contains("Cyberport Incubation Program"))
                    m_body += "Email CC to : " + _CCEmailCPIPVetting + "<br/></br>";
                else
                    m_body += "Email CC to : " + _CCEmailCCMFVetting + "<br/></br>";
            }
            else
                m_body = "";

            string _ApplicationType="";
            if (m_Programme_Name.Contains("Cyberport Incubation Program"))
            {
                //CPIP
                m_subject = m_VettingDate.ToString("d MMMM yyyy (ddd)") + " - Vetting Meeting and Presentation Session for Cyberport Incubation Programme(CPIP) - " + m_Intake_Number ;
                m_body += getEmailTemplate("Vetting_Team_Invitaion_CPIP");
                if (!string.IsNullOrEmpty(_CCEmailCPIPVetting) && ccOn)
                    headers.Add("cc", _CCEmailCPIPVetting);
                _ApplicationType = "CPIP";
            }
            else if (m_Programme_Name.Contains("Cyberport University Partnership Programme"))
            {
                //CUPP
                m_subject = m_VettingDate.ToString("d MMMM yyyy (ddd)") + " - Vetting Meeting and Presentation Session for Cyberport University Partnership PRogramme(CUPP) - " + m_Intake_Number ;
                m_body += getEmailTemplate("Vetting_Team_Invitaion_CUPP");
                if (!string.IsNullOrEmpty(_CCEmailCUPPVetting) && ccOn)
                    headers.Add("cc", _CCEmailCUPPVetting);
                _ApplicationType="CCMF";
            }
            else if (m_Programme_Name.ToLower().Contains("gbayep"))
            {
                //GBAYEP
                m_subject = m_VettingDate.ToString("d MMMM yyyy (ddd)") + " - Vetting Meeting and Presentation Session for Cyberport Creative Micro Fund - Greater Bay Area Young Entrepreneurship Programme - " + m_Intake_Number;
                m_body += getEmailTemplate("Vetting_Team_Invitaion_GBAYEP");
                if (!string.IsNullOrEmpty(_CCEmailGBAYEPVetting) && ccOn)
                    headers.Add("cc", _CCEmailGBAYEPVetting);
                _ApplicationType = "CCMF";
            }
            else
            {
                //CCMF
                m_subject = m_VettingDate.ToString("d MMMM yyyy (ddd)") + " - Vetting Meeting and Presentation Session for Cyberport Creative Micro Fund(CCMF) - " + m_Intake_Number ;
                m_body += getEmailTemplate("Vetting_Team_Invitaion_CCMF");
                if (!string.IsNullOrEmpty(_CCEmailCCMFVetting) && ccOn)
                    headers.Add("cc", _CCEmailCCMFVetting);
                _ApplicationType="CCMF";
            }

            m_body = m_body
                    .Replace("@@IntakeNumber", m_Intake_Number)
                    .Replace("@@VettingDate", m_VettingDate.ToString("d MMMM, yyyy (ddd)"))
                    .Replace("@@VettingMettingFrom", m_VettingMettingFrom.ToString("h:mmtt"))
                    .Replace("@@VettingMeetingTo", m_VettingMeetingTo.ToString("h:mmtt"))
                    .Replace("@@PresentFrom", m_Presentation_From.ToString("h:mmtt"))
                    .Replace("@@PresentTo", m_Presentation_To.ToString("h:mmtt"))
                    .Replace("@@VettingVenue", m_VettingVenue)

                    .Replace("@@TotalEligibleApplications", m_totalEligibleApplication.ToString())
                    .Replace("@@ApplicationDeadline", m_ApplicationDeadline.ToString("d MMMM yyyy"))
                    .Replace("@@TotalShortlistedApplications", m_totalShortlisted.ToString())

                    .Replace("@@CarParkDeadline", m_PakingDealine.ToString("d MMMM yyyy"));
            //lbltest.Text += "<br/> in sendSingleVettingTeamEmail - <br/> Name [" + fullName + "] to list [" + m_EmailList + "]";
            headers.Add("to", m_mail);
            headers.Add("subject", m_subject);

            //lbltest.Text += "Sending email";

            //sharepointsendemail(m_mail, m_subject, m_body);
            sharepointsendemail(headers, m_body, _ApplicationType);

        }


        protected void sendToVettingTeam(string m_EmailList, string fullName)
        {

            StringDictionary headers = new StringDictionary();
            string _ApplicationType = "";

            //m_WebsiteUrl += m_WebsiteUrl_VettingTeam;
            var Debugstr = "";
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
                Debugstr = "send to (" + m_EmailList + ") <br>";
            }
            else
            {
                m_mail = m_EmailList;
            }

            m_subject = "";
            m_body = "";
            if (m_Programme_Name.Contains("Cyberport Incubation Program"))
            {
                //CPIP
                m_subject = m_VettingDate.ToString("d MMMM (ddd) yyyy") + " - Vetting Meeting and Presentation Session for Cyberport Incubation Programme(CPIP) (" + m_Intake_Number + ")";
                m_body = getEmailTemplate("Vetting_Team_Invitaion_CPIP");
                if (!string.IsNullOrEmpty(_CCEmailCPIPVetting))
                    headers.Add("cc", _CCEmailCPIPVetting);
                _ApplicationType = "CPIP";

            }
            else
            {
                //CCMF
                m_subject = m_VettingDate.ToString("d MMMM (ddd) yyyy") + " - Vetting Meeting and Presentation Session for Cyberport Creative Micro Fund(CCMF) (" + m_Intake_Number + ")";
                m_body = getEmailTemplate("Vetting_Team_Invitaion_CCMF");
                if (!string.IsNullOrEmpty(_CCEmailCCMFVetting))
                    headers.Add("cc", _CCEmailCCMFVetting);
                _ApplicationType = "CCMF";

            }

           
            //m_subject = "Vetting Meetin Team Notification[" + m_EmailList + "]";
            //m_body = getEmailTemplate("Vetting_Meeting_Invitation");
            m_body = Debugstr + m_body;
            m_body = m_body
                    .Replace("@@IntakeNumber", m_Intake_Number)
                    .Replace("@@VettingDate", m_VettingDate.ToString("d MMMM, yyyy (ddd)"))
                    .Replace("@@VettingMettingFrom", m_VettingMettingFrom.ToString("h:mmtt"))
                    .Replace("@@VettingMeetingTo", m_VettingMeetingTo.ToString("h:mmtt"))
                    .Replace("@@PresentFrom", m_Presentation_From.ToString("h:mmtt"))
                    .Replace("@@PresentTo", m_Presentation_To.ToString("h:mmtt"))
                    .Replace("@@VettingVenue", m_VettingVenue)

                    .Replace("@@TotalEligibleApplications", m_totalEligibleApplication.ToString())
                    .Replace("@@ApplicationDeadline", m_ApplicationDeadline.ToString("d MMMM yyyy"))
                    .Replace("@@TotalShortlistedApplications", m_totalShortlisted.ToString())

                    .Replace("@@CarParkDeadline", m_PakingDealine.ToString("d MMMM yyyy"));

            headers.Add("to", m_mail);
            headers.Add("subject", m_subject);
            //sharepointsendemail(m_mail, m_subject, m_body);
            sharepointsendemail(headers, m_body, _ApplicationType);
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

        protected String getEmailTemplate(string emailTemplate)
        {
            String emailTemplateContent = "";
            ConnectOpen();
            try
            {
                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template = @emailTemplate ;";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@emailTemplate", emailTemplate));

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

        protected void sharepointsendemail(StringDictionary headers, string body, string ApplicationType)
        {
            string folderURL = _VettingInvitationAttachment + "/" + ApplicationType;

            //lbltest.Text += "sharepointsendemail - Address [" + toAddress + "] Subject [" + subject + "]" ;
            SPSecurity.RunWithElevatedPrivileges(
            delegate()
            {
                using (SPSite site = new SPSite(
                  SPContext.Current.Site.ID,
                  SPContext.Current.Site.Zone))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPWebApplication webApp = web.Site.WebApplication;
                        string smtpServerAddress = webApp.OutboundMailServiceInstance.Server.Address;
                        string fromAddress = webApp.OutboundMailSenderAddress;
                        MailMessage email = new MailMessage();
                        email.From = new MailAddress(fromAddress);
                        email.Subject = headers["subject"];
                        email.IsBodyHtml = true;
                        email.Body = body;

                        if (!string.IsNullOrEmpty(headers["to"]))
                        {
                            foreach (string toAddress in headers["to"].Split(';').ToList().Where(x => !string.IsNullOrEmpty(x)))
                            {
                                email.To.Add(toAddress);
                            }
                        }
                        if (!string.IsNullOrEmpty(headers["cc"]))
                        {
                            foreach (string ccAddress in headers["cc"].Split(';').ToList().Where(x => !string.IsNullOrEmpty(x)))
                            {
                                email.CC.Add(ccAddress);
                            }
                        }

                        SPFolder folder = web.GetFolder(folderURL);
                        foreach (SPFile objFile in folder.Files)
                        {
                            Stream contentStream = objFile.OpenBinaryStream();
                            Attachment attachment = new Attachment(contentStream, objFile.Name);
                            email.Attachments.Add(attachment);
                        }


                        try
                        {

                            SmtpClient mailServer = new SmtpClient(smtpServerAddress);
                            //mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;
                            mailServer.Send(email);

                        }
                        catch (Exception ex) {
                            lblInviteStatus.Text = "smtpServerAddress: " + smtpServerAddress + "|From: " + fromAddress + "|Error: " + ex.ToString();
                        }

                        

                    }
                }
            });
        }

        protected void getSYSTEMPARAMETER()
        {
            ConnectOpen();
            try
            {
                var sqlString = "select Config_Code,Value from TB_SYSTEM_PARAMETER where Config_Code in "
                                + "('ApplicationIsInDebug','ApplicationDebugEmailSentTo','WebsiteUrl',"
                                + "'WebsiteUrl_VettingTeam', 'WebsiteUrl_InvitationResponse','PreviewCPIPInvitEmailTo','PreviewCCMFInvitEmailTo',"
                                + "'CCEmailCPIPVetting','CCEmailCCMFVetting','VettingInvitationAttachment')";

                var command = new SqlCommand(sqlString, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == "ApplicationIsInDebug")
                    {
                        //1,0
                        m_ApplicationIsInDebug = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "ApplicationDebugEmailSentTo")
                    {
                        m_ApplicationDebugEmailSentTo = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "WebsiteUrl")
                    {
                        m_WebsiteUrl = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "WebsiteUrl_VettingTeam")
                    {
                        m_WebsiteUrl_VettingTeam = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "WebsiteUrl_InvitationResponse")
                    {
                        m_WebsiteUrl_InvitationResponse = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "PreviewCCMFInvitEmailTo")
                    {
                        m_PreviewCCMFInvitEmailTo = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "PreviewCPIPInvitEmailTo")
                    {
                        m_PreviewCPIPInvitEmailTo = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "PreviewCUPPInvitEmailTo")
                    {
                        m_PreviewCUPPInvitEmailTo = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "PreviewGBAYEPInvitEmailTo")
                    {
                        m_PreviewGBAYEPInvitEmailTo = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "CCEmailCPIPVetting")
                    {
                        _CCEmailCPIPVetting = reader.GetString(1);

                    }
                    if (reader.GetString(0) == "CCEmailCCMFVetting")
                    {
                        _CCEmailCCMFVetting = reader.GetString(1);
                    }
                    if (reader.GetString(0) == "CCEmailCUPPVetting")
                    {
                        _CCEmailCUPPVetting = reader.GetString(1);
                    }
                    if (reader.GetString(0) == "CCEmailGBAYEPVetting")
                    {
                        _CCEmailGBAYEPVetting = reader.GetString(1);
                    }
                    if (reader.GetString(0) == "VettingInvitationAttachment")
                    {
                        _VettingInvitationAttachment = reader.GetString(1);
                }


                }
                //lbltest.Text = "GetSysParam - m_PreviewCCMFInvitEmailTo[" + m_PreviewCCMFInvitEmailTo + "] m_PreviewCPIPInvitEmailTo [" + m_PreviewCPIPInvitEmailTo + "]"; 
                reader.Dispose();
                command.Dispose();

            }
            finally
            {

                ConnectClose();
            }
        }

        protected void ddlVettingTeamLeader_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_lstRightLeftExceptLeader();
        }


    }

    public class SearchResult
    {
        public string ProgrammeName { get; set; }
        public string IntakeNumber { get; set; }
        public string ClusterValue { get; set; }
        public string ClusterText { get; set; }
        public string FieldText { get; set; }
        public string FieldValue { get; set; }

    }

    public class ApplicationList
    {
        public Boolean selection { get; set; }
        public string ApplicationNo { get; set; }
        public string Cluster { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string ProgrammeType { get; set; }
        public string ApplicationType { get; set; }
        public string Status { get; set; }
        public Boolean Shortlisted { get; set; }
        public string PresentationDetails { get; set; }

    }

    public class VettingMetting
    {
        public string ProgrammeName { get; set; }
        public string IntakeNumber { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string TimeInteval { get; set; }
        public DateTime VMFrom { get; set; }
        public DateTime VMto { get; set; }
        public DateTime PresentationFrom { get; set; }
        public DateTime Presentationto { get; set; }
        public string VettingTeamLeader { get; set; }
        public List<string> VettingTeamMenber { get; set; }
        public string NoofAttendance { get; set; }
        public string Meeting_status { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime ConfirmDeadline { get; set; }
        public DateTime ParkingDeadline { get; set; }
        public string FullName { get; set; }


    }

}
