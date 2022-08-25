using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.APPListBYVTWP.APPListBYVTWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class APPListBYVTWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public APPListBYVTWebPart()
        {
        }
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

            DataTable dataSourceApplication = new DataTable();

            dataSourceApplication.Columns.Add("Application No.", typeof(string));
            dataSourceApplication.Columns.Add("Cluster", typeof(string));

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
            String sqlWhere = " where tpi.Programme_Name = '" + m_ProgrammeName + "' and tpi.Intake_Number='" + m_IntakeNum + "' and tbApplicatiion.Status like '" + m_Status + "'   ";
            String sqlOederBy = " order by " + m_SortApplicationNo + " " + m_SortApplicationNoOrder + " ";
            //String sqlWhere = "";
            String sqlScreeningScoreTable = "";
            String linkURl = "";
            String sqlColumnID = "";

            Dictionary<String, int> GridViewColumnOrder = new Dictionary<String, int>();
            GridViewColumnOrder.Add("ApplicationNo", 0);
            GridViewColumnOrder.Add("Cluster", 1);
            GridViewColumnOrder.Add("CompanyName", 2);
            GridViewColumnOrder.Add("ProjectName", 3);
            GridViewColumnOrder.Add("ProgrammeType", 4);
            GridViewColumnOrder.Add("ApplicationType", 5);
            GridViewColumnOrder.Add("ProjectDescription", 6);
            GridViewColumnOrder.Add("Status", 7);
            GridViewColumnOrder.Add("BDMScore", 8);
            GridViewColumnOrder.Add("SrManagerScore", 9);
            GridViewColumnOrder.Add("CPMOScore", 10);
            GridViewColumnOrder.Add("AveragerScore", 11);
            GridViewColumnOrder.Add("Remarks", 12);
            GridViewColumnOrder.Add("RemarksForVetting", 13);
            GridViewColumnOrder.Add("Shortlisted", 14);
            GridViewColumnOrder.Add("PresentationFrom", 15);
            GridViewColumnOrder.Add("PresentationTo", 16);
            GridViewColumnOrder.Add("Email", 17);
            GridViewColumnOrder.Add("MobileNumber", 18);
            GridViewColumnOrder.Add("Attend", 19);
            GridViewColumnOrder.Add("NameOfAttendees", 20);
            GridViewColumnOrder.Add("PresentationTools", 21);
            GridViewColumnOrder.Add("SpecialRequest", 22);
            GridViewColumnOrder.Add("GoNotGo", 23);

            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
            {
                //CPIP
                sqlColumn += ",tbApplicatiion.Company_Name_Eng";
                sqlColumnID = " ,tbApplicatiion.Incubation_ID,tbApplicatiion.Programme_ID ";
                sqlFrom += " inner join TB_INCUBATION_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";
                sqlScreeningScoreTable = "TB_SCREENING_INCUBATION_SCORE";

                GridViewApplicationVT.Columns[GridViewColumnOrder["ProjectName"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["ApplicationType"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["CompanyName"]].Visible = true;

                linkURl = "http://cyberportemssp:10869/SitePages/IncubationProgram.aspx";
            }
            else
            {
                //CCMF
                sqlColumn += ",tbApplicatiion.Project_Name_Eng,tbApplicatiion.Programme_Type,tbApplicatiion.CCMF_Application_Type";
                sqlColumnID = " ,tbApplicatiion.CCMF_ID,tbApplicatiion.Programme_ID ";
                sqlFrom += " inner join TB_CCMF_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";
                sqlScreeningScoreTable = "TB_SCREENING_CCMF_SCORE";

                GridViewApplicationVT.Columns[GridViewColumnOrder["ProjectName"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["ApplicationType"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["CompanyName"]].Visible = false;

                linkURl = "http://cyberportemssp:10869/SitePages/CCMF.aspx";
            }

            if (lstStatus.SelectedItem.Text.ToString() == "Eligibility checked")
            {
                GridViewApplicationVT.Columns[GridViewColumnOrder["BDMScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["SrManagerScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["CPMOScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["AveragerScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["Remarks"]].Visible = true;

                sqlColumn += ",isNull(tbsscoreBDM.BDM_Score,0) as BDM_Score,isNull(tbsscoreBDM.BDM_Remarks,'') as BDM_Remarks ,isNull(tbsscoreBDM.BDM_Score,0)  as Average_Score ";
                sqlFrom += " left  join (select tbsscore.Total_Score as BDM_Score,tbsscore.Remarks as BDM_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPIP BDM') tbsscoreBDM on tbsscoreBDM.Application_Number = tbApplicatiion.Application_Number ";
            }
            else if (lstStatus.SelectedItem.Text.ToString() == "BDM Reviewed")
            {
                GridViewApplicationVT.Columns[GridViewColumnOrder["BDMScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["SrManagerScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["CPMOScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["AveragerScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["Remarks"]].Visible = true;

                sqlColumn += ",isNull(tbsscoreBDM.BDM_Score,0) as BDM_Score,isNull(tbsscoreBDM.BDM_Remarks,'') as BDM_Remarks,isNull(tbsscoreSeniorManager.SeniorManager_Score,0) as SeniorManager_Score,isNull(tbsscoreSeniorManager.SeniorManager_Remarks,'') as SeniorManager_Remarks,(isNull(tbsscoreBDM.BDM_Score,0) +isNull(tbsscoreSeniorManager.SeniorManager_Score,0) )/2 as Average_Score ";
                sqlFrom += " left join (select tbsscore.Total_Score as BDM_Score,tbsscore.Remarks as BDM_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPIP BDM') tbsscoreBDM on tbsscoreBDM.Application_Number = tbApplicatiion.Application_Number left join (select tbsscore.Total_Score as SeniorManager_Score,tbsscore.Remarks as SeniorManager_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='Senior Manager') tbsscoreSeniorManager on tbsscoreSeniorManager.Application_Number = tbApplicatiion.Application_Number ";

            }
            else if (lstStatus.SelectedItem.Text.ToString() == "Sr. Mgr. Reviewed" || lstStatus.SelectedItem.Text.ToString() == "All Status")
            {
                GridViewApplicationVT.Columns[GridViewColumnOrder["BDMScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["SrManagerScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["CPMOScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["AveragerScore"]].Visible = true;
                GridViewApplicationVT.Columns[GridViewColumnOrder["Remarks"]].Visible = true;

                sqlColumn += ",isNull(tbsscoreBDM.BDM_Score,0) as BDM_Score,isNull(tbsscoreBDM.BDM_Remarks,'') as BDM_Remarks,isNull(tbsscoreSeniorManager.SeniorManager_Score,0) as SeniorManager_Score,isNull(tbsscoreSeniorManager.SeniorManager_Remarks,'') as SeniorManager_Remarks,isNull(tbsscoreCPMO.CPMO_Score,0) as CPMO_Score,isNull(tbsscoreCPMO.CPMO_Remarks,'') as CPMO_Remarks,(isNull(tbsscoreBDM.BDM_Score,0) +isNull(tbsscoreSeniorManager.SeniorManager_Score,0) +isNull(tbsscoreCPMO.CPMO_Score,0))/3 as Average_Score ";
                sqlFrom += " left join (select tbsscore.Total_Score as BDM_Score,tbsscore.Remarks as BDM_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPIP BDM') tbsscoreBDM on tbsscoreBDM.Application_Number = tbApplicatiion.Application_Number left join (select tbsscore.Total_Score as SeniorManager_Score,tbsscore.Remarks as SeniorManager_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='Senior Manager') tbsscoreSeniorManager on tbsscoreSeniorManager.Application_Number = tbApplicatiion.Application_Number left join (select tbsscore.Total_Score as CPMO_Score,tbsscore.Remarks as CPMO_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPMO') tbsscoreCPMO on tbsscoreCPMO.Application_Number = tbApplicatiion.Application_Number ";
            }
            else
            {
                GridViewApplicationVT.Columns[GridViewColumnOrder["BDMScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["SrManagerScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["CPMOScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["AveragerScore"]].Visible = false;
                GridViewApplicationVT.Columns[GridViewColumnOrder["Remarks"]].Visible = false;
            }

            getReview();

            sqlColumn += ",Application_Deadline,isNull(tbAppShortlisting.Remarks_To_Vetting,'') as Remarks_To_Vetting,isNull(tbAppShortlisting.Shortlisted,0) as Shortlisted ";
            
            sqlColumn += " ,isNull(tbva.Presentation_From,'') as Presentation_From,isNull(tbva.Presentation_To,'') as Presentation_To,isNull(tbva.Email,'') as Email,isNull(tbva.Mobile_Number,'') as Mobile_Number,isNull(tbva.Attend,0) as Attend,isNull(tbva.Name_of_Attendees,'') as Name_of_Attendees,isNull(tbva.Presentation_Tools,'') as Presentation_Tools,isNull(tbva.Special_Request,'') as Special_Request,isNull(tbvd.Go,0) as Go ";
            sqlColumn += sqlColumnID;
            sqlFrom += " left join TB_VETTING_APPLICATION tbva on tbva.Application_Number=tbApplicatiion.Application_Number left join TB_VETTING_DECISION tbvd on tbvd.Application_Number= tbApplicatiion.Application_Number ";

            var sqlString = "select * from( " + sqlColumn + sqlFrom + sqlWhere + ") as result " + sqlOederBy;
            var command = new SqlCommand(sqlString, connection);
            var reader = command.ExecuteReader();
            List<ApplicationListVT> applicationListVT = new List<ApplicationListVT>();

            

            int count = 0;
            int countlatest = 0;

            if (m_Role.Contains("Vetting Team"))
            {

                while (reader.Read())
                {
                    count++;

                    ApplicationListVT applst = new ApplicationListVT();
                    applst.ApplicationNo = (String)reader.GetValue(0);
                    applst.Cluster = "";
                    countlatest = 1;

                    applst.ProjectDescription = "";
                    applst.Status = (String)reader.GetValue(1);

                    if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                    {
                        //CPIP
                        applst.CompanyName = (String)reader.GetValue(2);

                        countlatest = 2;
                    }
                    else
                    {
                        //CCMF
                        applst.ProjectName = (String)reader.GetValue(2);
                        applst.ProgrammeType = (String)reader.GetValue(3);
                        applst.ApplicationType = (String)reader.GetValue(4);

                        countlatest = 4;

                    }

                    if (lstStatus.SelectedItem.Text.ToString() == "Eligibility checked")
                    {
                        applst.BDMScore = float.Parse(reader.GetValue(countlatest + 1).ToString());
                        applst.AverageScore = float.Parse(reader.GetValue(countlatest + 3).ToString());


                        var Remarks = "BDM:" + reader.GetString(countlatest + 2);
                        applst.Remarks = Remarks; //BDM Remarks

                        countlatest = countlatest + 3;
                    }
                    else if (lstStatus.SelectedItem.Text.ToString() == "BDM Reviewed")
                    {
                        applst.BDMScore = float.Parse(reader.GetValue(countlatest + 1).ToString());
                        applst.SrManagerScore = float.Parse(reader.GetValue(countlatest + 3).ToString());
                        applst.AverageScore = float.Parse(reader.GetValue(countlatest + 5).ToString());

                        var Remarks = "BDM: " + reader.GetString(countlatest + 2) + "<br>"
                                      + "sr Mgr: " + reader.GetString(countlatest + 4);
                        applst.Remarks = Remarks;

                        countlatest = countlatest + 5;

                    }
                    else if (lstStatus.SelectedItem.Text.ToString() == "Sr. Mgr. Reviewed" || lstStatus.SelectedItem.Text.ToString() == "All Status")
                    {
                        applst.BDMScore = float.Parse(reader.GetValue(countlatest + 1).ToString());
                        applst.SrManagerScore = float.Parse(reader.GetValue(countlatest + 3).ToString());
                        applst.CPMOScore = float.Parse(reader.GetValue(countlatest + 5).ToString());
                        applst.AverageScore = float.Parse(reader.GetValue(countlatest + 7).ToString());

                        var Remarks = "BDM: " + reader.GetString(countlatest + 2) + "<br>"
                                      + "sr Mgr: " + reader.GetString(countlatest + 4) + "<br>"
                                      + "CPMO: " + reader.GetString(countlatest + 6);
                        applst.Remarks = Remarks;

                        countlatest = countlatest + 7;
                    }

                    countlatest++;
                    lbldatetime.Text = reader.GetDateTime(countlatest).ToString("d MMM yyyy h:mmtt") + "(GMT +8)";


                    if (m_Role.Contains("Vetting Team"))
                    {
                        countlatest++;
                        applst.RemarksForVetting = (String)reader.GetValue(countlatest);

                    }
                    else
                    {
                        countlatest++;
                        GridViewApplicationVT.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = false;
                    }

                    countlatest++;
                    applst.Shortlisted = (Boolean)reader.GetValue(countlatest);


                    countlatest++;
                    applst.PresentationFrom = reader.GetDateTime(countlatest).ToString("h:mmtt");

                    countlatest++;
                    applst.PresentationTo = reader.GetDateTime(countlatest).ToString("h:mmtt");

                    countlatest++;
                    applst.Email = reader.GetString(countlatest);

                    countlatest++;
                    applst.MobileNumber = reader.GetString(countlatest); 

                    countlatest++;
                    applst.Attend = (Boolean)reader.GetValue(countlatest); 

                    countlatest++;
                    applst.NameOfAttendees = reader.GetString(countlatest); 

                    countlatest++;
                    applst.PresentationTools = reader.GetString(countlatest); 

                    countlatest++;
                    applst.SpecialRequest = reader.GetString(countlatest); 

                    countlatest++;
                    applst.GoNotGo = (Boolean)reader.GetValue(countlatest) ? "Go":"No Go";

                    countlatest++;
                    var app = reader.GetValue(countlatest).ToString(); // CCMF_ID||Incubation_ID

                    countlatest++;
                    var prog = reader.GetInt32(countlatest); //Programme_ID
                    applst.APPNoURL = linkURl + "?app=" + app + "&prog=" + prog;


                    applicationListVT.Add(applst);


                }
            }
            lblcount.Text = count.ToString() + " applications";
            GridViewApplicationVT.DataSource = applicationListVT;
            GridViewApplicationVT.DataBind();
            

            

            reader.Dispose();
            command.Dispose();


            connection.Close();
            connection.Dispose();
        }
    }

    public class SearchResult
    {
        public string ProgrammeName { get; set; }
        public string IntakeNumber { get; set; }

    }
    public class ApplicationListVT
    {
        public string ApplicationNo { get; set; }
        public string Cluster { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string ProgrammeType { get; set; }
        public string ApplicationType { get; set; }
        public string ProjectDescription { get; set; }
        public string Status { get; set; }
        public float BDMScore { get; set; }
        public float SrManagerScore { get; set; }
        public float CPMOScore { get; set; }
        public float AverageScore { get; set; }
        public string Remarks { get; set; }
        public string RemarksForVetting { get; set; }
        public Boolean Shortlisted { get; set; }
        public string APPNoURL { get; set; }
        public string PresentationFrom { get; set; }
        public string PresentationTo { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public Boolean Attend { get; set; }
        public String NameOfAttendees { get; set; }
        public string PresentationTools { get; set; }
        public string SpecialRequest { get; set; }
        public string GoNotGo { get; set; }

    }
}
