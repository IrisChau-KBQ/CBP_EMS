using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.ECResultWP.ECResultWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ECResultWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private String m_ProgrammeName;
        private String m_IntakeNum;
        private String m_Cluster;
        private String m_Status;
        private String m_SortCluster;
        private String m_SortClusterOrder;
        private String m_SortApplicationNo;
        private String m_SortApplicationNoOrder;

        private String m_Role;

        public ECResultWebPart()
        {
        }

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

            GridViewApplicationBindData();
        }

        private void BindData()
        {
            var connection = new SqlConnection(connStr);
            connection.Open();


            var sqlString = "select distinct Programme_Name from TB_PROGRAMME_INTAKE order by Programme_Name;";
            var command = new SqlCommand(sqlString, connection);
            var reader = command.ExecuteReader();
            List<SearchResult> programmeNameList = new List<SearchResult>();
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

            sqlString = "select distinct Intake_Number from TB_PROGRAMME_INTAKE order by Intake_Number;";
            command = new SqlCommand(sqlString, connection);
            reader = command.ExecuteReader();
            List<SearchResult> intakeNumberList = new List<SearchResult>();
            while (reader.Read())
            {
                intakeNumberList.Add(new SearchResult
                {
                    IntakeNumber = reader.GetInt32(0).ToString()
                });
            }
            lstIntakeNumber.DataSource = intakeNumberList;
            lstIntakeNumber.DataBind();

            reader.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();

            var SampleStr = "Hi," + Environment.NewLine
                + "Please noted that your application for " + lstCyberportProgramme.SelectedValue + " intake " + lstIntakeNumber.SelectedValue + " is successed. Further Information will be provided in the coming email." + Environment.NewLine
                + "Regards," + Environment.NewLine
                +"EMS Administrator";


            txtConfirmtionEmailSample.Text = SampleStr;
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
                        m_Role += ogroup.Name;
                    }
                }
            }

        }

        private void GridViewApplicationBindData()
        {
            var connection = new SqlConnection(connStr);
            connection.Open();

            m_ProgrammeName = lstCyberportProgramme.SelectedValue;
            m_IntakeNum = lstIntakeNumber.SelectedValue;
            m_Cluster = lstCluster.SelectedValue;
            m_Status = lstStatus.SelectedValue;
            m_Cluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;
            m_SortApplicationNoOrder = radioSortApplicationNo.SelectedValue;

            String sqlColumn = "select tbApplicatiion.Application_Number,tbApplicatiion.Status";
            String sqlFrom = " from TB_PROGRAMME_INTAKE tpi ";
            String sqlWhere = " where tpi.Programme_Name = '" + m_ProgrammeName + "' and tpi.Intake_Number='" + m_IntakeNum + "' and tbApplicatiion.Status like '" + m_Status + "'  ";
            String sqlOrderBy = " order by " + m_SortApplicationNo + " " + m_SortApplicationNoOrder + " ; ";
            //String sqlWhere = "";

            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
            {
                //CPIP
                sqlColumn += ",tbApplicatiion.Company_Name_Eng";
                sqlFrom += " inner join TB_INCUBATION_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID  ";
            }
            else
            {
                //CCMF
                sqlColumn += ",tbApplicatiion.Project_Name_Eng ";
                sqlFrom += " inner join TB_CCMF_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID ";
            }

            sqlColumn += ",Application_Deadline ";
            var sqlString = sqlColumn + sqlFrom + sqlWhere;
            sqlString = "select * from (" + sqlString + ") as result " + sqlOrderBy;

            var command = new SqlCommand(sqlString, connection);
            var reader = command.ExecuteReader();
            List<ApplicationList> applicationList = new List<ApplicationList>();

            Dictionary<String, int> GridViewColumn = new Dictionary<String, int>();
            GridViewColumn.Add("ApplicationNo",0);
            GridViewColumn.Add("Cluster", 1);
            GridViewColumn.Add("CompanyName", 2);
            GridViewColumn.Add("ProjectName", 3);
            GridViewColumn.Add("Status", 4);
            GridViewColumn.Add("ECConfirmed", 5);


            int count = 0;
            while (reader.Read())
            {
                count++;
                ApplicationList appList = new ApplicationList(); 


                appList.ApplicationNo = reader.GetString(0); //applicationNo
                appList.Status = reader.GetString(1);
                appList.Cluster = ""; //Cluster
                appList.ECConfirmed = false; 


                if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                {
                    //CPIP
                    appList.CompanyName = reader.GetString(2);
                    GridViewApplication.Columns[GridViewColumn["ProjectName"]].Visible = false; //project name

                }
                else
                {
                    //CCMF

                    appList.ProjectName = reader.GetString(2);
                    GridViewApplication.Columns[GridViewColumn["CompanyName"]].Visible = false; //company name

                }


                lbldatetime.Text = reader.GetDateTime(3).ToString("d MMM yyyy h:mmtt") + "(GMT +8)";

                applicationList.Add(appList);

            }

            lblcount.Text = count.ToString() + " applications";
            GridViewApplication.DataSource = applicationList;
            GridViewApplication.DataBind();




            reader.Dispose();
            command.Dispose();


            connection.Close();
            connection.Dispose();
        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //updateStatus();
            foreach (GridViewRow row in GridViewApplication.Rows)
            {
                string accessType = row.Cells[3].Text;
            }
        }

        protected void updateStatus(String m_ApplicationID)
        {
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            //SPList oList = oSiteCollection.AllWebs["SharePoint Development Site Collection"].Lists["Application List"];
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            //oQuery.Query = "<Where><Eq><FieldRef Name='Programme_ID'/><Value Type='Number'>1</Value></Eq></Where>";
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                oListItem["Status"] = "BDM Reviewed";
                oListItem["BDM_Score"] = "";

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }
        }
    }

    public class SearchResult
    {
        public string ProgrammeName { get; set; }
        public string IntakeNumber { get; set; }

    }
    public class ApplicationList
    {
        public string ApplicationNo { get; set; }
        public string Cluster { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public Boolean ECConfirmed { get; set; }
    }
}
