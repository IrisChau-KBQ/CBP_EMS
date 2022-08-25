using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.PresentResultSummary.PresentResultSummaryWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class PresentResultSummaryWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public PresentResultSummaryWebPart()
        {
        }


        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportWMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }
        private SqlConnection connection;
        private string m_VMID;
        private string m_Role;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            m_VMID = HttpContext.Current.Request.QueryString["VMID"];

            if (CheckUser())
            {
                if (!Page.IsPostBack)
                {
                    BindGridViewPresentation();
                }
            }
        }

        public Boolean CheckUser()
        {
            if ( m_VMID != null)
            {
                getReview();

                if (m_Role.Contains("Vetting Team") && CheckisMemberLeader())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
                        m_Role = ogroup.Name;
                    }
                }
            }

        }
        public Boolean CheckisMemberLeader()
        {
            ConnectOpen();

            var sqlString = "select tmInfo.Email,tmInfo.Full_Name,tmInfo.Disabled from TB_VETTING_MEETING tvm inner join TB_VETTING_MEMBER tvmember on tvmember.Vetting_Meeting_ID = tvm.Vetting_Meeting_ID inner join TB_VETTING_MEMBER_INFO tmInfo on tmInfo.Vetting_Member_ID = tvmember.Vetting_Member_ID where tvm.Vetting_Meeting_ID = @m_VMID and tmInfo.Disabled=0 and tvmember.isLeader=1 and tmInfo.Email = @Email";

            var command = new SqlCommand(sqlString, connection);
            command.Parameters.Add("@m_VMID", m_VMID);
            command.Parameters.Add("@Email", SPContext.Current.Web.CurrentUser.Name.ToString());
            var reader = command.ExecuteReader();
            var status = false;
            if (reader.Read())
            {
                status = true;
            }


            reader.Dispose();
            command.Dispose();

            ConnectClose();

            return status;
        }

        public void BindGridViewPresentation()
        {
            ConnectOpen();

            var sqlString = "SELECT tva.Application_Number,tvmInfo.Email,case when tvdecision.Go is null then 3 else tvdecision.Go END as Go,case when tScoreCCMF.CCMF_Scoring_ID is null then isnull(tScoreCPIP.Total_Score,0.01) else isnull(tScoreCCMF.Total_Score,0.01)  end Total_Score, case when tScoreCCMF.CCMF_Scoring_ID is null then isnull(tScoreCPIP.Remarks,'') else isnull(tScoreCCMF.Remarks,'')  end Remarks,case when tapplicationCCMF.CCMF_ID is null then isnull(tapplicationCPIP.Company_Name_Chi,'') else isnull(tapplicationCCMF.Project_Name_Chi,'')  end name FROM TB_VETTING_MEETING tvm INNER JOIN TB_VETTING_APPLICATION tva ON tvm.Vetting_Meeting_ID = tva.Vetting_Meeting_ID LEFT OUTER JOIN TB_VETTING_MEMBER tvmember ON tvm.Vetting_Meeting_ID = tvmember.Vetting_Meeting_ID LEFT OUTER JOIN TB_VETTING_MEMBER_INFO tvmInfo ON tvmember.Vetting_Member_ID = tvmInfo.Vetting_Member_ID LEFT OUTER JOIN TB_PROGRAMME_INTAKE tpi ON tvm.Programme_ID = tpi.Programme_ID LEFT OUTER JOIN TB_VETTING_DECISION tvdecision ON tvm.Vetting_Meeting_ID = tvdecision.Vetting_Meeting_ID and tvdecision.Member_Email = tvmInfo.Email and tvdecision.Application_Number = tva.Application_Number LEFT OUTER JOIN TB_PRESENTATION_CCMF_SCORE tScoreCCMF ON tva.Application_Number = tScoreCCMF.Application_Number and tScoreCCMF.Member_Email = tvmInfo.Email  and tpi.Programme_ID = tScoreCCMF.Programme_ID LEFT OUTER JOIN TB_CCMF_APPLICATION tapplicationCCMF ON tpi.Programme_ID = tapplicationCCMF.Programme_ID and tva.Application_Number = tapplicationCCMF.Application_Number  LEFT OUTER JOIN TB_PRESENTATION_INCUBATION_SCORE tScoreCPIP ON tva.Application_Number = tScoreCPIP.Application_Number and tScoreCPIP.Member_Email = tvmInfo.Email and tpi.Programme_ID = tScoreCPIP.Programme_ID LEFT OUTER JOIN TB_INCUBATION_APPLICATION tapplicationCPIP ON tpi.Programme_ID = tapplicationCPIP.Programme_ID and tva.Application_Number = tapplicationCPIP.Application_Number where tvm.Vetting_Meeting_ID = @m_VMID order by tva.Application_Number,tvmInfo.Email";

            var command = new SqlCommand(sqlString, connection);
            command.Parameters.Add("@m_VMID", m_VMID);
            var reader = command.ExecuteReader();

            List<searchResult> Summarylist = new List<searchResult>();

            while (reader.Read())
            {
                searchResult item = new searchResult();

                var count = 0;
                item.ApplicationNo = reader.GetString(count);

                count++;
                item.Email = reader.GetString(count);

                count++;
                item.Email = reader.GetString(count);

                count++;
                var gonotgo = (float)reader.GetValue(count);
                if (gonotgo == 3)
                {
                    item.GoNotGo = "";
                }
                else
                {
                    if (gonotgo == 0)
                    {
                        item.GoNotGo = "Go";
                    }
                    else
                    {
                        item.GoNotGo = "NG";
                    }
                    
                }

                count++;
                var score = (float)reader.GetValue(count);
                if (score == 0.01)
                {
                    item.Score = "";
                }
                else
                {
                    item.Score = score.ToString();
                }

                count++;
                item.Remarks = reader.GetString(count);

                count++;
                item.CompanyNameProjectName = reader.GetString(count);



                //Summarylist.Add(item);
            }


            reader.Dispose();
            command.Dispose();

            ConnectClose();


            GridViewPresentation.DataSource = Summarylist;
            GridViewPresentation.DataBind();
        }

        public void GetMemberInfo()
        {
            ConnectOpen();

            var sqlString = "select tmInfo.Email,tmInfo.Full_Name,tmInfo.Disabled from TB_VETTING_MEETING tvm inner join TB_VETTING_MEMBER tvmember on tvmember.Vetting_Meeting_ID = tvm.Vetting_Meeting_ID inner join TB_VETTING_MEMBER_INFO tmInfo on tmInfo.Vetting_Member_ID = tvmember.Vetting_Member_ID where tvm.Vetting_Meeting_ID = @m_VMID and tmInfo.Disabled=0";

            var command = new SqlCommand(sqlString, connection);
            command.Parameters.Add("@m_VMID", m_VMID);
            var reader = command.ExecuteReader();

            List<searchResult> memberlist = new List<searchResult>();

            while (reader.Read())
            {
                searchResult item = new searchResult();

                var count = 0;
                item.Email = reader.GetString(count);

                count++;
                item.fullName = reader.GetString(count);

                memberlist.Add(item);
            }


            reader.Dispose();
            command.Dispose();

            ConnectClose();


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {

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

    public class searchResult
    {
        public string Sequence { get; set; }
        public string ApplicationNo { get; set; }
        public string CompanyNameProjectName { get; set; }
        public string Scoreofeachvettingmember { get; set; }
        public string Score { get; set; }
        public string TotalScore { get; set; }
        public string AverageScore { get; set; }
        public string GoNotGo { get; set; }
        public string Recommend { get; set; }
        public string NotRecommend { get; set; }
        public string NoofVote { get; set; }
        public string Remarks { get; set; }

        public string Email { get; set; }
        public string fullName { get; set; }
    }
}
