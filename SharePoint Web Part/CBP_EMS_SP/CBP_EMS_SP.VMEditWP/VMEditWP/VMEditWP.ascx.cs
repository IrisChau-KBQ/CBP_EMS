using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using System.Linq;

namespace CBP_EMS_SP.VMEditWP.VMEditWP
{
    [ToolboxItemAttribute(false)]
    public partial class VMEditWP : WebPart
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

        private String m_progid;
        //private int m_progid;
        //private String m_ApplicationID;
        private String m_ProgrammeName;
        //private Int32 m_IntakeNum;
        private string m_IntakeNum;
        private string m_VMID;

        // private String VettingMeeting_ID;
        private string selectedProg;
        private string selectedIntake;
        //private string trace_Msg = "";

        public Dictionary<String, int> GridViewColumnOrder;

        public VMEditWP()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private void BindData()
        {
            //BindDataCluster();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            // m_progid = Context.Request.QueryString["ProgNo"];

            // int ApplicationID = Convert.ToInt32(Context.Request.QueryString["AppNo"]);
            // m_ApplicationID = Context.Request.QueryString["AppNo"];

            // screenValueSet();            // no use yet for create

            // To automatic resolve the path of the current SP site, and show the calendar properly. - stanley 
            m_VMID = HttpContext.Current.Request.QueryString["VMID"];
            if (m_VMID != null)
            {
                SetGridViewColumnOrder();
                screenValueSet();
                if (!Page.IsPostBack)
                {

                    BindData();
                    //gen_AppList(); 

                }

                //BindddlSelectedList();

            }

        }

        // not called for VMcreate
        protected void screenValueSet()
        {
            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                var sqlString = "SELECT tvmeeting.Date, tvmeeting.Venue, tvmeeting.Vetting_Meeting_From, tvmeeting.Vetting_Meeting_To,tvmeeting.Presentation_From, tvmeeting.Presentation_To, tvmeeting.Vetting_Team_Leader, tvmeeting.No_of_Attendance, tvmeeting.Time_Interval, isnull(TB_VETTING_MEMBER_INFO.Email,'') as Email,TB_PROGRAMME_INTAKE.Programme_Name, TB_PROGRAMME_INTAKE.Intake_Number FROM TB_VETTING_MEETING tvmeeting left JOIN TB_VETTING_MEMBER tvmember ON tvmeeting.Vetting_Meeting_ID = tvmember.Vetting_Meeting_ID left JOIN TB_VETTING_MEMBER_INFO ON tvmember.Vetting_Member_ID = TB_VETTING_MEMBER_INFO.Vetting_Member_ID INNER JOIN TB_PROGRAMME_INTAKE ON tvmeeting.Programme_ID = TB_PROGRAMME_INTAKE.Programme_ID where tvmember.isLeader=0 and tvmeeting.Vetting_Meeting_ID = @m_VMID ;";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@m_VMID", m_VMID);

                var reader = command.ExecuteReader();
                VettingMetting vm = new VettingMetting();
                vm.VettingTeamMenber = "";
                while (reader.Read())
                {
                    var countFieldOrder = 0;
                    vm.Date = reader.GetDateTime(countFieldOrder).ToString("dd MMM yyyy");

                    countFieldOrder++;
                    vm.Venue = reader.GetString(countFieldOrder);

                    countFieldOrder++;
                    vm.VMFrom = reader.GetDateTime(countFieldOrder).ToString("h : mm tt");

                    countFieldOrder++;
                    vm.VMto = reader.GetDateTime(countFieldOrder).ToString("h : mm tt");

                    countFieldOrder++;
                    vm.PresentationFrom = reader.GetDateTime(countFieldOrder).ToString("h : mm tt");

                    countFieldOrder++;
                    vm.Presentationto = reader.GetDateTime(countFieldOrder).ToString("h : mm tt");

                    countFieldOrder++;
                    vm.VettingTeamLeader = reader.GetString(countFieldOrder);

                    countFieldOrder++;
                    vm.NoofAttendance = reader.GetInt32(countFieldOrder).ToString();

                    countFieldOrder++;
                    vm.TimeInteval = reader.GetString(countFieldOrder) + " mins";

                    countFieldOrder++;
                    vm.VettingTeamMenber += reader.GetString(countFieldOrder) + "<br>";

                    countFieldOrder++;
                    vm.ProgrammeName = reader.GetString(countFieldOrder);

                    countFieldOrder++;
                    vm.IntakeNumber = reader.GetInt32(countFieldOrder).ToString();
                }

                lblCyberportProgramme.Text = vm.ProgrammeName;
                lblIntakeNumber.Text = vm.IntakeNumber;
                lblDate.Text = vm.Date;
                lblVenue.Text = vm.Venue;
                lblTimeInterval.Text = vm.TimeInteval;
                lblVMFrom.Text = vm.VMFrom;
                lblVMTo.Text = vm.VMto;
                lblPresentFm.Text = vm.PresentationFrom;
                lblPresentTo.Text = vm.Presentationto;
                lblVettingTeamLeader.Text = vm.VettingTeamLeader;
                lblVettingTeamMember.Text = vm.VettingTeamMenber;

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }


        //protected void btnsubmit_Click(object sender, EventArgs e)
        //{
        //    if (m_VMID != null)
        //    {
        //        if (Val_SelectCnt() == false)           // check the application selected counts
        //            return;
        //        //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //        //string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
        //        SqlConnection conn = new SqlConnection(connStr);
        //        conn.Open();
        //        try
        //        {
        //            //delete Application

        //            var sqlDelete = "delete from TB_VETTING_APPLICATION where Vetting_Meeting_ID = '" + m_VMID + "';";
        //            var cmd = new SqlCommand(sqlDelete, conn);
        //            cmd.ExecuteNonQuery();

        //            //  loop the Application list below and insert those *SELECTED* into TB_VETTING_APPLICATION:
        //            var PresentationFrom = Convert.ToDateTime(lblDate.Text + " " + lblPresentFm.Text);
        //            var TimeInterval = int.Parse(lblTimeInterval.Text.Replace("mins", "").Trim());
        //            foreach (ListItem SelectedItem in ddlSelectedList.Items)
        //            {

        //                string Application_Number = SelectedItem.Text;

        //                var Presentationto = PresentationFrom.AddMinutes(TimeInterval);
        //                // (CheckBox)row.FindControl("CheckBoxselecton") = true;
        //                var sql = "INSERT INTO TB_VETTING_APPLICATION(Vetting_Application_ID, Vetting_Meeting_ID, Application_Number, Presentation_From, Presentation_To) VALUES("
        //                    + "NEWID(), "
        //                    + "'" + m_VMID + "', "
        //                    + "'" + Application_Number + "',"
        //                    + "'" + PresentationFrom + "' , "
        //                    + "'" + Presentationto + "');";

        //                cmd = new SqlCommand(sql, conn);
        //                cmd.ExecuteNonQuery();

        //                PresentationFrom = Presentationto;


        //            }

        //            cmd.Dispose();
        //            conn.Close();
        //            conn.Dispose();

        //            Context.Response.Redirect("Vetting Meeting Arrangement.aspx");
        //        }
        //        finally
        //        {

        //            if (conn != null)
        //            {
        //                conn.Close();
        //            }
        //        }
        //    }
        //}

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

        //protected void gen_AppList()
        //{
        //    // gen the selected / current programme name and intake number for filtering

        //    var connection = new SqlConnection(connStr);
        //    connection.Open();

        //    m_ProgrammeName = lblCyberportProgramme.Text;
        //    m_IntakeNum = lblIntakeNumber.Text;

        //    String sqlColumn = "select tbApplicatiion.Application_Number,tbApplicatiion.Status,tbApplicatiion.Business_Area,isNull(tbAppShortlisting.Shortlisted,0) as Shortlisted";
        //    String sqlFrom = " from TB_PROGRAMME_INTAKE tpi ";
        //    String sqlWhere = " where tpi.Programme_Name = '" + m_ProgrammeName + "' and tpi.Intake_Number='" + m_IntakeNum + "' and tbApplicatiion.Business_Area like '" + lstCluster.SelectedValue + "'  and tbApplicatiion.Status <> 'Saved' and tbApplicatiion.Status = 'Complete Screening'";
        //    String sqlOrderBy = " order by Application_Number asc ";

        //    List<ApplicationList> applicationList = new List<ApplicationList>();
        //    if (m_ProgrammeName.Contains("Cyberport Incubation Program"))
        //    {
        //        //CPIP
        //        sqlColumn += ",isNull(tbApplicatiion.Company_Name_Eng,'') as Company_Name_Eng";
        //        sqlFrom += " inner join TB_INCUBATION_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";
               

        //        gvAppl.Columns[GridViewColumnOrder["ProjectName"]].Visible = false;
        //        gvAppl.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = false;
        //        gvAppl.Columns[GridViewColumnOrder["ApplicationType"]].Visible = false;
        //        gvAppl.Columns[GridViewColumnOrder["CompanyName"]].Visible = true;
        //    }
        //    else
        //    {
        //        //CCMF
        //        sqlColumn += ",isNull(tbApplicatiion.Project_Name_Eng,'') as Project_Name_Eng,isNull(tbApplicatiion.Programme_Type,'') as Programme_Type,isNull(tbApplicatiion.CCMF_Application_Type,'') as CCMF_Application_Type";
        //        sqlFrom += " inner join TB_CCMF_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";

        //        gvAppl.Columns[GridViewColumnOrder["ProjectName"]].Visible = true;
        //        gvAppl.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = true;
        //        gvAppl.Columns[GridViewColumnOrder["ApplicationType"]].Visible = true;
        //        gvAppl.Columns[GridViewColumnOrder["CompanyName"]].Visible = false;
        //    }
        //    sqlColumn += " ,isnull(tva.Application_Number,'') as ApplicationNumber,tva.Presentation_From,tva.Presentation_To ";
        //    sqlFrom += " left join TB_VETTING_APPLICATION tva on tva.Application_Number = tbApplicatiion.Application_Number ";

        //    var selectedList = GetVettingApplicationNumber();
        //    var sqlString = "select * from( " + sqlColumn + sqlFrom + sqlWhere + ") as result " + sqlOrderBy;
        //    //var sqlString = "SELECT cast(0 as bit) as selection, b.Application_Number, isnull(b.Company_name_Eng,'') as CoOrPrjName, isnull(b.business_area,'') as Cluster, '' as ProgTyep, b.Status "
        //    //               + "from TB_INCUBATION_APPLICATION b, "
        //    //               + "TB_PROGRAMME_INTAKE c "
        //    //               + " where b.Programme_ID = c.Programme_ID "
        //    //               + " and  c.Programme_Name = '" + m_ProgrammeName + "' and c.Intake_Number = '" + m_IntakeNum + "' "
        //    //               + " UNION " +
        //    //                 "SELECT cast(0 as bit) as selection, b.Application_Number, isnull(b.Project_Name_Eng,''), isnull(b.business_area,'') as Cluster, b.Programme_Type as ProgType, b.Status "
        //    //               + "from TB_CCMF_APPLICATION b, "
        //    //               + "TB_PROGRAMME_INTAKE c "
        //    //               + "where b.Programme_ID = c.Programme_ID "
        //    //               + " and  c.Programme_Name = '" + m_ProgrammeName + "' and c.Intake_Number = '" + m_IntakeNum + "';";

        //    var command = new SqlCommand(sqlString, connection);
        //    var reader = command.ExecuteReader();
        //    int countlatest = 0;
        //    while (reader.Read())
        //    {
        //        countlatest = 0;
        //        ApplicationList applst = new ApplicationList();
        //        applst.ApplicationNo = (String)reader.GetValue(countlatest);

        //        countlatest++;
        //        applst.Status = reader.GetString(countlatest);

        //        countlatest++;
        //        applst.Cluster = reader.GetString(countlatest);

        //        countlatest++;
        //        applst.Shortlisted = (Boolean)reader.GetValue(countlatest);

        //        if (m_ProgrammeName.Contains("Cyberport Incubation Program"))
        //        {
        //            //CPIP
        //            countlatest++;
        //            applst.CompanyName = reader.GetString(countlatest);
        //        }
        //        else
        //        {
        //            //CCMF
        //            countlatest++;
        //            applst.ProjectName = reader.GetString(countlatest);

        //            countlatest++;
        //            applst.ProgrammeType = reader.GetString(countlatest);

        //            countlatest++;
        //            applst.ApplicationType = reader.GetString(countlatest);
        //        }

        //        countlatest++;
        //        if (reader.GetString(countlatest) != "")
        //        {
        //            countlatest++;
        //            var presentationFrom = reader.GetDateTime(countlatest);

        //            countlatest++;
        //            var presentationTo = reader.GetDateTime(countlatest);

        //            if (presentationFrom.ToString("dd MMM yyyy") == presentationTo.ToString("dd MMM yyyy"))
        //            {
        //                applst.PresentationDetails = presentationFrom.ToString("dd MMM yyyy,HH:mm tt") + " - " + presentationTo.ToString("HH:mm tt");
        //            }
        //            else
        //            {
        //                applst.PresentationDetails = presentationFrom.ToString("dd MMM yyyy,HH:mm tt") + " - " + presentationTo.ToString("dd MMM yyyy,HH:mm tt");
        //            }
        //        }
        //        else
        //        {
        //            applst.PresentationDetails = "";
        //        }

                
        //        if (selectedList.Contains(applst.ApplicationNo))
        //        {
        //            applst.selection = true;
        //        }

        //        applicationList.Add(applst);
        //    }
        //    gvAppl.DataSource = applicationList;
        //    gvAppl.DataBind();

        //    SetGridviewSelected();

        //    // to enable or disable save button
        //    if (gvAppl.Rows.Count > 0)
        //        btnsubmit.Enabled = true;
        //    else
        //        btnsubmit.Enabled = false;

        //    reader.Dispose();
        //    command.Dispose();
        //    connection.Close();
        //    connection.Dispose();
        //}

       

        public class AppLstResult
        {
            public Boolean selection { get; set; }
            public string Application_Number { get; set; }
            public string CoOrPrjName { get; set; }
            public string Cluster { get; set; }
            public string ProgType { get; set; }
            public string Status { get; set; }

        }


       

        protected string get_proid(string Programme_Name, string Intake_Number)
        {
            string m_prog_id = "";
            string sql_string = ""; using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql_string = "SELECT * FROM TB_PROGRAMME_INTAKE where Programme_Name = @Programme_Name and Intake_Number=@Intake_Number";
                using (SqlCommand cmd = new SqlCommand(sql_string, conn))
                {
                    cmd.Parameters.Add("@Programme_Name", Programme_Name);
                    cmd.Parameters.Add("@Intake_Number", Intake_Number);

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

        //protected bool Val_SelectCnt()
        //{
        //    int CntSelect = 0;
        //    foreach (GridViewRow row in gvAppl.Rows)
        //    {
        //        CheckBox chk = (row.Cells[0].FindControl("CheckBoxselection") as CheckBox);
        //        if (chk.Checked)
        //        {
        //            CntSelect++;
        //        }
        //    }

        //    var PresentationFrom = Convert.ToDateTime(lblDate.Text + " " + lblPresentFm.Text);
        //    var PresentationTo = Convert.ToDateTime(lblDate.Text + " " + lblPresentTo.Text);
        //    int TimeInterval = int.Parse(lblTimeInterval.Text.Replace("mins", "").Trim());
        //    var count = Math.Ceiling((PresentationTo - PresentationFrom).TotalMinutes / TimeInterval);
        //    if (CntSelect <= 0)
        //    {

        //        lbl_validate_SelectCnt.Text = "At least One Application should be selected.";
        //        lbl_validate_SelectCnt.Visible = true;
        //        return false;

        //    }
        //    else if (CntSelect > count)
        //    {
        //        lbl_validate_SelectCnt.Text = "The Applications cannot be more than " + count;
        //        lbl_validate_SelectCnt.Visible = true;
        //        return false;
        //    }
        //    else
        //    {
        //        lbl_validate_SelectCnt.Text = "";
        //        lbl_validate_SelectCnt.Visible = false;
        //        return true;
        //    }
        //}


        //private void BindDataCluster()
        //{
        //    m_ProgrammeName = lblCyberportProgramme.Text;
        //    m_IntakeNum = lblIntakeNumber.Text;

        //    String sqlColumn = "select distinct tbApplicatiion.Business_Area ";
        //    String sqlFrom = " from TB_PROGRAMME_INTAKE tpi ";
        //    String sqlWhere = " where tpi.Programme_Name = '" + m_ProgrammeName + "' and tpi.Intake_Number='" + m_IntakeNum + "' and tbApplicatiion.Status <> 'Saved'";


        //    if (m_ProgrammeName.Contains("Cyberport Incubation Program"))
        //    {
        //        //CPIP
        //        sqlFrom += " inner join TB_INCUBATION_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID ";
        //    }
        //    else
        //    {
        //        //CCMF
        //        sqlFrom += " inner join TB_CCMF_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID ";

        //    }

        //    var connection = new SqlConnection(connStr);
        //    connection.Open();

        //    var sqlString = sqlColumn + sqlFrom + sqlWhere;

        //    var command = new SqlCommand(sqlString, connection);
        //    var reader = command.ExecuteReader();
        //    List<SearchResult> ClusterList = new List<SearchResult>();
        //    ClusterList.Add(new SearchResult
        //    {
        //        ClusterValue = "%",
        //        ClusterText = "All Cluster"
        //    });

        //    while (reader.Read())
        //    {
        //        ClusterList.Add(new SearchResult
        //        {
        //            ClusterValue = reader.GetString(0),
        //            ClusterText = reader.GetString(0)
        //        });
        //    }

        //    reader.Dispose();
        //    command.Dispose();
        //    connection.Close();
        //    connection.Dispose();

        //    lstCluster.DataSource = ClusterList;
        //    lstCluster.DataBind();

        //}

        //private void BindddlSelectedList()
        //{
        //    foreach (GridViewRow row in gvAppl.Rows)
        //    {
        //        System.Web.UI.WebControls.CheckBox SelectBox = (row.Cells[GridViewColumnOrder["Select"]].FindControl("CheckBoxselection") as System.Web.UI.WebControls.CheckBox);
        //        var ApplicationNunber = row.Cells[GridViewColumnOrder["ApplicationNo"]].Text;
        //        var Cluster = row.Cells[GridViewColumnOrder["Cluster"]].Text;
        //        ListItem item = new ListItem(ApplicationNunber, Cluster);
        //        if (SelectBox.Checked)
        //        {
        //            if (!ddlSelectedList.Items.Contains(item))
        //            {
        //                ddlSelectedList.Items.Add(item);
        //            }

        //        }
        //        else
        //        {
        //            if (ddlSelectedList.Items.Contains(item))
        //            {
        //                ddlSelectedList.Items.Remove(item);
        //            }
        //        }

        //    }

        //    var selectedCountLabel = "<span class='SelectedCount'>All Cluster: " + ddlSelectedList.Items.Count + "</span>";

        //    Dictionary<String, String> SelectedList = new Dictionary<String, String>();
        //    foreach (ListItem item in ddlSelectedList.Items)
        //    {
        //        SelectedList.Add(item.Text, item.Value);
        //    }

        //    var ClusterList = SelectedList.GroupBy(s => s.Value).Select(g => new { Cluster = g.Key, Count = g.Count() });
        //    foreach (var item in ClusterList)
        //    {
        //        selectedCountLabel += "<span class='SelectedCount'>" + item.Cluster + ": " + item.Count + "</span>";
        //    }
        //    lblSelectedCount.Text = selectedCountLabel;
        //}

        //private void SetGridviewSelected()
        //{
        //    foreach (GridViewRow row in gvAppl.Rows)
        //    {
        //        System.Web.UI.WebControls.CheckBox SelectBox = (row.Cells[GridViewColumnOrder["Select"]].FindControl("CheckBoxselection") as System.Web.UI.WebControls.CheckBox);
        //        var ApplicationNunber = row.Cells[GridViewColumnOrder["ApplicationNo"]].Text;
        //        var Cluster = row.Cells[GridViewColumnOrder["Cluster"]].Text;
        //        ListItem item = new ListItem(ApplicationNunber, Cluster);
        //        if (ddlSelectedList.Items.Contains(item))
        //        {
        //            SelectBox.Checked = true;
        //        }

        //    }
        //}

        //private void ClearddlGridviewSelected()
        //{
        //    ddlSelectedList.Items.Clear();
        //}

        //protected void lstCluster_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    gen_AppList();

        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("/SitePages/Vetting Meeting Arrangement.aspx");

        }
        private List<String> GetVettingApplicationNumber()
        {
            List<String> ApplicationNumberSelectedList = new List<String>();

            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                var sqlString = "select  Application_Number from TB_VETTING_APPLICATION where Vetting_Meeting_ID=@m_VMID;";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@m_VMID", m_VMID);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ApplicationNumberSelectedList.Add(reader.GetString(0));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {

                connection.Close();
                connection.Dispose();
            }

            return ApplicationNumberSelectedList;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {
                var sqlDelete = "delete from TB_VETTING_APPLICATION where Vetting_Meeting_ID = @m_VMID;";
                var cmd = new SqlCommand(sqlDelete, conn);
                cmd.Parameters.Add("@m_VMID", m_VMID);

                cmd.ExecuteNonQuery();

                sqlDelete = "delete from TB_VETTING_MEMBER where Vetting_Meeting_ID = @m_VMID;";
                cmd = new SqlCommand(sqlDelete, conn);
                cmd.Parameters.Add("@m_VMID", m_VMID);

                cmd.ExecuteNonQuery();

                sqlDelete = "delete from TB_VETTING_MEETING where Vetting_Meeting_ID = @m_VMID;";
                cmd = new SqlCommand(sqlDelete, conn);
                cmd.Parameters.Add("@m_VMID", m_VMID);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            finally
            {

                conn.Close();
                conn.Dispose();
            }
            Context.Response.Redirect("Vetting Meeting Arrangement.aspx");
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

                
                if (selectbox.Checked)
                {
                    selectbox.Visible = true;
                }

            }
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
        public string Date { get; set; }
        public string Venue { get; set; }
        public string TimeInteval { get; set; }
        public string VMFrom { get; set; }
        public string VMto { get; set; }
        public string PresentationFrom { get; set; }
        public string Presentationto { get; set; }
        public string VettingTeamLeader { get; set; }
        public string VettingTeamMenber { get; set; }
        public string NoofAttendance { get; set; }

    }
}
