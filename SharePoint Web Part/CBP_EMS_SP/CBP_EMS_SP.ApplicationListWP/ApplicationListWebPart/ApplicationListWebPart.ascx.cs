using ICSharpCode.SharpZipLib.Zip;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

using System.DirectoryServices.AccountManagement;
using System.Globalization;

using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CBP_EMS_SP.Data.Models;
using iTextSharp.text.pdf.draw;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Collections;

namespace CBP_EMS_SP.ApplicationListWP.ApplicationListWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ApplicationListWebPart : WebPart
    {
        [Personalizable(), WebBrowsable, DefaultValue("https://ems.cyberport.hk/SitePages/CCMF_Internal.aspx"), WebDisplayName("CCMF URL"), Category("Configuration")]
        public String CCMF_URL { get; set; }

        [Personalizable(), WebBrowsable, DefaultValue("https://ems.cyberport.hk/SitePages/CCMFGBAYEP_Internal.aspx"), WebDisplayName("CCMFGBAYEP URL"), Category("Configuration")]
        public String CCMFGBAYEP_URL { get; set; }

        [Personalizable(), WebBrowsable, DefaultValue("https://ems.cyberport.hk/SitePages/IncubationProgram_Internal.aspx"), WebDisplayName("CPIP URL"), Category("Configuration")]
        public String CPIP_URL { get; set; }


        public String CASP_URL { get { return "/SitePages/CASP_Internal.aspx"; } }

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportWMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportEMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
                //return System.Configuration.ConfigurationManager.ConnectionStrings["EmsSqlConStr"].ConnectionString;
            }
        }

        private String m_ProgrammeName;
        private String m_IntakeNum;
        private String m_Cluster;
        private String m_Status;
        private String m_SortCluster;
        private String m_SortClusterOrder;
        private String m_SortApplicationNo;
        private String m_SortApplicationNoOrder;

        private String m_ProgramDeadline = "";
        private int m_AppCount = 0;


        private String m_Role;
        public Dictionary<String, int> GridViewColumnOrder;
        private SqlConnection connection;


        //public String m_path = @"C:\Program Files\Common Files\microsoft shared\Web Server Extensions\15\TEMPLATE\IMAGES\Download";
        public String m_path = "";
        public String m_programName;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink = "";
        public String IsLogEvent;
        public String m_qryStrProgName = "ProgName";
        public String m_qryStrIntakeNo = "IntakeNo";
        public String m_qryStrCluster = "Cluster";
        public String m_qryStrStream = "Stream";
        public String m_qryStrStatus = "Status";
        public String m_qryStrSortColumn1 = "SortColumn1";
        public String m_qryStrSortOrder1 = "SortOrder1";
        public String m_qryStrSortColumn2 = "SortColumn2";
        public String m_qryStrSortOrder2 = "SortOrder2";

        public String m_alertMessage;

        public String m_selectfirstProgrammeNameText = "Select Programme Name";
        public String m_selectfirstIntakeNumberText = "Select Intake Number";
        string m_Programme_Type = "";

        double m_DownloadFileSize = 0;
        //newline
        public string m_ss8;
        public ApplicationListWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            m_alertMessage = "";
            if (checkUser() && AccessControl())
            {
                PanelApplist.Visible = true;
                ConnectOpen();
                try
                {
                    if (!Page.IsPostBack)
                    {
                        GetQueryString();
                        BindDataProgramme();
                        InitDropDownList();
                        //BindDataIntake();
                        //BindDataCluster();
                        BindDataSortColumn();
                        SetDropdowmSelectedItem();



                        CheckButtonVisiable(GridViewApplicationBindData());


                    }
                }
                finally
                {
                    ConnectClose();
                }

                getSYSTEMPARAMETER();

                if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program") ||
                    lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport University Partnerhip Programme") || lstCyberportProgramme.SelectedValue.ToString() == "Cyberport Accelerator Support Programme")
                {
                    //CPIP
                    lststream.Visible = false;
                }
                else
                {
                    //CCMF
                    lststream.Visible = true;

                }
            }
            else
            {
                PanelApplist.Visible = false;
                clearSessionCookies();
            }
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

        private Boolean checkUser()
        {
            var result = true;
            if (HttpContext.Current.Request.QueryString["ischeck"] != null)
            {
                //chcek
                /*
                var roleid = HttpContext.Current.Request.QueryString["roleid"];
                if (roleid != null)
                {
                    getReview();
                    var role = "";
                    if (roleid == "4C0B5391")
                    {
                        role = "Senior Manager";
                    }

                    if (roleid == "60D9E31A")
                    {
                        role = "CPMO";
                    }

                    if (roleid == "D5D1FB1A")
                    {
                        role = "BDM";
                    }

                    if (roleid == "1428B1E7")
                    {
                        role = "Coordinator";
                    }

                    if (m_Role.ToLower().Contains(role.ToLower()))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                */
                result = false;
                //                }
            }

            return result;
        }

        protected Boolean AccessControl()
        {
            var m_result = false;
            getReview();
            // Check Role can display this web part
            //Applicant  //Collaborator  //CCMF Coordinator //CCMF BDM  //CPIP Coordinator  //CPIP BDM  //Senior Manager  //CPMO

            if (m_Role == "Applicant")
            {
                m_result = false;
            }
            else if (m_Role == "Collaborator")
            {
                m_result = false;
            }
            else if (m_Role == "CCMF Coordinator" || m_Role == "CPIP Coordinator" || m_Role == "CASP Coordinator")
            {
                m_result = true;
            }
            else if (m_Role == "CCMF BDM" || m_Role == "CPIP BDM" || m_Role == "CASP BDM")
            {
                m_result = true;
            }
            else if (m_Role == "Senior Manager" || m_Role == "CASP Senior Manager")
            {
                m_result = true;
            }
            else if (m_Role == "CPMO" || m_Role == "CASP CPMO")
            {
                m_result = true;
            }
            else if (m_Role == "CASP Coordinator")
            {
                m_result = true;
            }
            else if (m_Role == "CASP BDM")
            {
                m_result = true;
            }
            else if (m_Role == "CASP Senior Manager")
            {
                m_result = true;
            }

            return m_result;
        }

        private void SetGridViewColumnOrder()
        {


            GridViewColumnOrder = new Dictionary<String, int>();

            var count = 0;
            GridViewColumnOrder.Add("ApplicationNo", count);

            count++;
            GridViewColumnOrder.Add("Cluster", count);

            count++;
            GridViewColumnOrder.Add("CompanyName", count);

            count++;
            GridViewColumnOrder.Add("ProjectName", count);

            count++;
            GridViewColumnOrder.Add("ProgrammeType", count);


            count++;
            GridViewColumnOrder.Add("HongKongProgrammeStream", count);

            count++;
            GridViewColumnOrder.Add("ApplicationType", count);

            count++;
            GridViewColumnOrder.Add("ProjectDescription", count);

            count++;
            GridViewColumnOrder.Add("SubmissionDate", count);

            count++;
            GridViewColumnOrder.Add("Status", count);

            count++;
            GridViewColumnOrder.Add("BDMScore", count);

            count++;
            GridViewColumnOrder.Add("SrManagerScore", count);

            count++;
            GridViewColumnOrder.Add("CPMOScore", count);

            count++;
            GridViewColumnOrder.Add("AveragerScore", count);

            count++;
            GridViewColumnOrder.Add("Remarks", count);

            count++;
            GridViewColumnOrder.Add("RemarksForVetting", count);

            count++;
            GridViewColumnOrder.Add("Shortlisted", count);
            //NewLine

            count++;
            GridViewColumnOrder.Add("SmartSpace", count);
            //NewLine
        }

        private void GetQueryString()
        {
            if (HttpContext.Current.Request.QueryString[m_qryStrProgName] != null)
            {
                m_ProgrammeName = HttpContext.Current.Request.QueryString[m_qryStrProgName];
                m_IntakeNum = HttpContext.Current.Request.QueryString[m_qryStrIntakeNo];
                m_Cluster = HttpContext.Current.Request.QueryString[m_qryStrCluster];
                m_Status = HttpContext.Current.Request.QueryString[m_qryStrStatus];
                m_SortCluster = HttpContext.Current.Request.QueryString[m_qryStrSortColumn1];
                m_SortClusterOrder = HttpContext.Current.Request.QueryString[m_qryStrSortOrder1];
                m_SortApplicationNo = HttpContext.Current.Request.QueryString[m_qryStrSortColumn2];
            }

        }

        private void SetDropdowmSelectedItem()
        {
            if (HttpContext.Current.Request.QueryString[m_qryStrProgName] != null)
            {
                lstCyberportProgramme.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrProgName];
                lstIntakeNumber.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrIntakeNo];
                lstCluster.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrCluster];
                lststream.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrStream];
                lstStatus.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrStatus];
                lstSortCluster.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrSortColumn1];
                radioSortCluster.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrSortOrder1];
                lstSortApplicationNo.SelectedValue = HttpContext.Current.Request.QueryString[m_qryStrSortColumn2];
            }

        }

        private void InitDropDownList()
        {
            List<SearchResult> intakeNumberList = new List<SearchResult>();
            if (m_ProgrammeName == null)
            {
                intakeNumberList.Add(new SearchResult
                {
                    IntakeNumber = m_selectfirstIntakeNumberText
                });
                lstIntakeNumber.DataSource = intakeNumberList;
                lstIntakeNumber.DataBind();
            }
            else
            {
                ConnectOpen();
                try
                {
                    BindDataIntake();
                }
                finally
                {
                    ConnectClose();
                }
                //lbldebug.Visible = true;
                //lbldebug.Text = " m_ProgrammeName : " + m_ProgrammeName;
            }
            List<SearchResult> ClusterList = new List<SearchResult>();
            if (m_IntakeNum == null)
            {
                ClusterList.Add(new SearchResult
                {
                    ClusterValue = "%",
                    ClusterText = "All Cluster"
                });
                lstCluster.DataSource = ClusterList;
                lstCluster.DataBind();
            }
            else
            {
                ConnectOpen();
                try
                {
                    BindDataCluster();
                }
                finally
                {
                    ConnectClose();
                }
                //lbldebug.Visible = true;
                //lbldebug2.Visible = true;
                //lbldebug2.Text = " m_IntakeNum : " + m_IntakeNum;
            }
        }

        private void BindDataProgramme()
        {
            var sqlString = "select distinct Programme_Name from TB_PROGRAMME_INTAKE order by Programme_Name;";
            var command = new SqlCommand(sqlString, connection);
            var reader = command.ExecuteReader();

            List<SearchResult> programmeNameList = new List<SearchResult>();

            programmeNameList.Add(new SearchResult
            {
                ProgrammeName = m_selectfirstProgrammeNameText

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

        }

        private void BindDataIntake()
        {
            String sqlwhere = "";
            if (m_ProgrammeName == null)
            {
                m_ProgrammeName = lstCyberportProgramme.SelectedValue;

            }

            sqlwhere = " where Programme_Name = @m_ProgrammeName and Active = 1";
            var sqlString = "select distinct Intake_Number from TB_PROGRAMME_INTAKE " + sqlwhere + " order by Intake_Number;";

            var command = new SqlCommand(sqlString, connection);
            command.Parameters.Add("@m_ProgrammeName", m_ProgrammeName);

            var reader = command.ExecuteReader();
            List<SearchResult> intakeNumberList = new List<SearchResult>();

            intakeNumberList.Add(new SearchResult
            {
                IntakeNumber = m_selectfirstIntakeNumberText
            });
            while (reader.Read())
            {
                intakeNumberList.Add(new SearchResult
                {
                    IntakeNumber = reader.GetInt32(0).ToString()
                });
            }

            reader.Dispose();
            command.Dispose();

            lstIntakeNumber.DataSource = intakeNumberList;
            lstIntakeNumber.DataBind();
        }

        private void BindDataCluster()
        {
            if (m_ProgrammeName == null)
            {
                m_ProgrammeName = lstCyberportProgramme.SelectedValue;
            }
            else
            {
                lstCyberportProgramme.SelectedValue = m_ProgrammeName;

            }
            if (m_IntakeNum == null)
            {
                m_IntakeNum = lstIntakeNumber.SelectedValue;
            }
            else
            {
                lstIntakeNumber.SelectedValue = m_IntakeNum;

            }
            m_ProgramDeadline = GetIntakeDeadline().ToString("dd MMM yyyy HH:mm");

            List<SearchResult> ClusterList = new List<SearchResult>();
            if (m_ProgrammeName == m_selectfirstProgrammeNameText || m_IntakeNum == m_selectfirstIntakeNumberText)
            {
                ClusterList.Add(new SearchResult
                {
                    ClusterValue = "%",
                    ClusterText = "All Cluster"
                });

                lstCluster.DataSource = ClusterList;
                lstCluster.DataBind();
                return;
            }

            String sqlColumn = "select distinct tbApplicatiion.Business_Area ";
            String sqlFrom = " from TB_PROGRAMME_INTAKE tpi ";
            String sqlWhere = " where tpi.Programme_Name = @m_ProgrammeName and tpi.Intake_Number=@m_IntakeNum and tbApplicatiion.Status <> 'Saved' and tbApplicatiion.Status <> 'Deleted'";


            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
            {
                //CPIP
                sqlFrom += " inner join TB_INCUBATION_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID ";
            }
            else
            {
                //CCMF
                sqlFrom += " inner join TB_CCMF_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID ";

            }


            var sqlString = sqlColumn + sqlFrom + sqlWhere;

            var command = new SqlCommand(sqlString, connection);
            command.Parameters.Add("@m_ProgrammeName", m_ProgrammeName);
            command.Parameters.Add("@m_IntakeNum", m_IntakeNum);

            var reader = command.ExecuteReader();

            ClusterList.Add(new SearchResult
            {
                ClusterValue = "%",
                ClusterText = "All Cluster"
            });

            while (reader.Read())
            {
                if (reader.GetString(0).ToLower() == "open data")
                {
                    ClusterList.Add(new SearchResult
                    {
                        ClusterValue = "AI / Big Data",
                        ClusterText = "AI / Big Data"
                    });
                }
                else
                {
                    ClusterList.Add(new SearchResult
                    {
                        ClusterValue = reader.GetString(0),
                        ClusterText = reader.GetString(0)
                    });
                }
            }

            reader.Dispose();
            command.Dispose();

            lstCluster.DataSource = ClusterList;
            lstCluster.DataBind();

        }

        private void BindDataSortColumn()
        {
            if (m_Status == null)
            {
                m_Status = lstStatus.SelectedValue;
            }
            if (lstCyberportProgramme.SelectedValue.ToString() == "Cyberport Accelerator Support Programme")
            {
                BindDataSortColumnCASP();
                return;
            }

            List<SortColumnClass> sortList = new List<SortColumnClass>();

            sortList.Add(new SortColumnClass
            {
                ColumnValue = "Application_Number",
                ColumnText = "Application No."
            });
            sortList.Add(new SortColumnClass
            {
                ColumnValue = "Business_Area",
                ColumnText = "Cluster"
            });
            sortList.Add(new SortColumnClass
            {
                ColumnValue = "Status",
                ColumnText = "Status"
            });

            getReview();
            if (m_Role == "CCMF BDM" || m_Role == "CPIP BDM" || m_Role == "CPMO" || m_Role == "Senior Manager")
            {
                sortList.Add(new SortColumnClass
                {
                    ColumnValue = "BDM_Score",
                    ColumnText = "BDM Score"
                });
                sortList.Add(new SortColumnClass
                {
                    ColumnValue = "SeniorManager_Score",
                    ColumnText = "Sr. Manager Score"
                });
                sortList.Add(new SortColumnClass
                {
                    ColumnValue = "CPMO_Score",
                    ColumnText = "CPMO Score"
                });
                sortList.Add(new SortColumnClass
                {
                    ColumnValue = "Average_Score",
                    ColumnText = "Average Score"
                });

            }

            //if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
            //{
            //    //CPIP
            //}
            //else
            if (lstCyberportProgramme.SelectedValue.ToString() == "Cyberport Creative Micro Fund - Hong Kong")
            {
                //CCMF
                sortList.Add(new SortColumnClass
                {
                    ColumnValue = "Hong_Kong_Programme_Stream",
                    ColumnText = "Stream"
                });

            }

            lstSortCluster.DataSource = sortList;
            lstSortCluster.DataBind();
            lstSortCluster.SelectedValue = "Application_Number";

            lstSortApplicationNo.DataSource = sortList;
            lstSortApplicationNo.DataBind();
            lstSortApplicationNo.SelectedValue = "Business_Area";

        }
        private void BindDataSortColumnCASP()
        {
            if (m_Status == null)
            {
                m_Status = lstStatus.SelectedValue;
            }

            List<SortColumnClass> sortList = new List<SortColumnClass>();

            sortList.Add(new SortColumnClass
            {
                ColumnValue = "Application_No",
                ColumnText = "Application Number"
            });

            sortList.Add(new SortColumnClass
            {
                ColumnValue = "Status",
                ColumnText = "Status"
            });
            sortList.Add(new SortColumnClass
            {
                ColumnValue = "Submitted_Date",
                ColumnText = "Submission Date"
            });
            getReview();
            //if (m_Role == "CASP BDM" || m_Role == "CPIP BDM" || m_Role == "CPMO" || m_Role == "Senior Manager")
            //{
            //    sortList.Add(new SortColumnClass
            //    {
            //        ColumnValue = "BDM_Score",
            //        ColumnText = "BDM Score"
            //    });
            //    sortList.Add(new SortColumnClass
            //    {
            //        ColumnValue = "SeniorManager_Score",
            //        ColumnText = "Sr. Manager Score"
            //    });
            //    sortList.Add(new SortColumnClass
            //    {
            //        ColumnValue = "CPMO_Score",
            //        ColumnText = "CPMO Score"
            //    });
            //    sortList.Add(new SortColumnClass
            //    {
            //        ColumnValue = "Average_Score",
            //        ColumnText = "Average Score"
            //    });

            //}


            lstSortCluster.DataSource = sortList;
            lstSortCluster.DataBind();
            lstSortCluster.SelectedValue = "Application_No";

            lstSortApplicationNo.DataSource = sortList;
            lstSortApplicationNo.DataBind();
            lstSortApplicationNo.SelectedValue = "Submitted_Date";

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
        private string getCurrentProgramme()
        {
            switch (m_ProgrammeName)
            {
                case "Cyberport Incubation Programme":
                    return "CPIP";
                case "Cyberport Creative Micro Fund - Hong Kong":
                    return "CCMF";
                case "Cyberport University Partnerhip Programme":
                    return "CUPP";
                case "Cyberport Creative Micro Fund - Cross Border":
                    return "CCMF-CB";
                case "Cyberport Accelerator Support Programme":
                    return "CASP";
                case  "Cyberport Creative Micro Fund - GBAYEP":
                    return "CCMFGBAYEP";
            }
            return "";
        }

        private void setGridColumns()
        {
            GridViewApplication.Columns[GridViewColumnOrder["SmartSpace"]].Visible = false;
            string m_Program = getCurrentProgramme();
            if (m_Program == "CPIP")
            {
                GridViewApplication.Columns[GridViewColumnOrder["ProjectName"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["HongKongProgrammeStream"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["ApplicationType"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["CompanyName"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["Shortlisted"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["SmartSpace"]].Visible = false;
            }

            else if (m_Program == "CCMF")
            {
                GridViewApplication.Columns[GridViewColumnOrder["ProjectName"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["HongKongProgrammeStream"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["ApplicationType"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["CompanyName"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["Shortlisted"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["SmartSpace"]].Visible = true;

            }
            else if (m_Program == "CCMFGBAYEP")
            {
                GridViewApplication.Columns[GridViewColumnOrder["ProjectName"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["HongKongProgrammeStream"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["ApplicationType"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["CompanyName"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["Shortlisted"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["SmartSpace"]].Visible = true;

            }

            if (m_Role == "CCMF BDM" || m_Role == "CPIP BDM" || m_Role == "CPMO" || m_Role == "Senior Manager")
            {
                GridViewApplication.Columns[GridViewColumnOrder["BDMScore"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["SrManagerScore"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["CPMOScore"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["AveragerScore"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["Remarks"]].Visible = true;
                GridViewApplication.Columns[GridViewColumnOrder["Shortlisted"]].Visible = true;
            }
            else
            {
                GridViewApplication.Columns[GridViewColumnOrder["BDMScore"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["SrManagerScore"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["CPMOScore"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["AveragerScore"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["Remarks"]].Visible = false;
                GridViewApplication.Columns[GridViewColumnOrder["Shortlisted"]].Visible = false;
            }
            if (m_Role == "CCMF BDM" || m_Role == "CPIP BDM")
            {
                GridViewApplication.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = true;
            }
            else
            {

                GridViewApplication.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = false;
            }

            if (m_Program == "CASP")
            {

                GridViewApplication.Columns[1].Visible =
                GridViewApplication.Columns[2].Visible =
                GridViewApplication.Columns[3].Visible =
                GridViewApplication.Columns[4].Visible =
                GridViewApplication.Columns[5].Visible =
                GridViewApplication.Columns[6].Visible =
                GridViewApplication.Columns[7].Visible =
                GridViewApplication.Columns[10].Visible =
                GridViewApplication.Columns[11].Visible =
                GridViewApplication.Columns[12].Visible =
                GridViewApplication.Columns[13].Visible =
                GridViewApplication.Columns[14].Visible =
                GridViewApplication.Columns[15].Visible =
                GridViewApplication.Columns[16].Visible
                = false;

                GridViewApplication.Columns[8].Visible = true;
                GridViewApplication.Columns[9].Visible = true;


            }
            else
            {
                GridViewApplication.Columns[8].Visible = false;

            }
        }

        private string constructCCMFScript()
        {
            string newSql = "SELECT App.Application_Number as AppNo"
                            + ",Status"
                            + ",Project_Name_Eng"
                            + ",isNull(Project_Name_Chi,'') as Project_Name_Chi"
                            + ",Programme_Type"
                            + ",Hong_Kong_Programme_Stream"
                            + ",CCMF_Application_Type"
                            + ",[SmartSpace]"
                            + ",isNull((Select Total_Score from TB_SCREENING_CCMF_SCORE s where s.Application_Number = App.Application_Number and role = 'CCMF BDM' ),-1) as BDM_Score"
                            + ",isNull((Select Total_Score from TB_SCREENING_CCMF_SCORE s where s.Application_Number = App.Application_Number and role = 'Senior Manager' ),-1) as SeniorManager_Score"
                            + ",isNull((Select Total_Score from TB_SCREENING_CCMF_SCORE s where s.Application_Number = App.Application_Number and role = 'CPMO' ),-1) as CPMO_Score"
                            + ",isNull((Select Remarks from TB_SCREENING_CCMF_SCORE s where s.Application_Number = App.Application_Number and role = 'CCMF BDM' ),'') as BDMRemarks"
                            + ",isNull((Select Remarks from TB_SCREENING_CCMF_SCORE s where s.Application_Number = App.Application_Number and role = 'Senior Manager' ),'') as SrRemarks"
                            + ",isNull((Select Remarks from TB_SCREENING_CCMF_SCORE s where s.Application_Number = App.Application_Number and role = 'CPMO' ),'') as CPMORemarks"
                            + ",IIF(((select count(*) from TB_SCREENING_CCMF_SCORE sa where sa.Application_Number = App.Application_Number and Total_Score <> 0) > 0)"
                            + ",(select Sum([Total_Score]) / "
                            + "(select count(*) from TB_SCREENING_CCMF_SCORE sa where sa.Application_Number = App.Application_Number and Total_Score <> 0) "
                            + "as AvgScore from TB_SCREENING_CCMF_SCORE a where a.Application_Number = App.Application_Number),0 )as Average_Score "
                            + ",CCMF_ID"
                            + ",App.Programme_ID as Programme_ID"
                            + ",isNUll(Business_Area,'') as Business_Area"
                            + ",REPLACE(REPLACE(REPLACE(CAST(Abstract_Eng as NVARCHAR(MAX)),char(13)+char(10),' '),char(9),' '),char(59),',') as PrjDesc"
                            + ",isNUll(sl.Remarks_To_Vetting, '') as Remarks_To_Vetting"
                            + ",isNull(sl.Shortlisted,0) as Shortlisted "
                             + ",App.SmartSpace "
                            + "FROM TB_CCMF_APPLICATION App "
                            + "left join TB_APPLICATION_SHORTLISTING sl on sl.Application_Number = App.Application_Number "
                            + "where App.Intake_Number=@IntakeNumber "
                //+ "and Hong_Kong_Programme_Stream like '%' "
                //+ "and App.Status like @Status "
                            + "and App.Status <> 'Saved' "
                            + "and App.Status <> 'Deleted' ";



            if (m_Status != "%")
                newSql += "and App.Status like '" + m_Status + "' ";
            if (lststream.SelectedValue != "%")
                newSql += "and Hong_Kong_Programme_Stream like '" + lststream.SelectedValue + "'";

            if (m_Cluster != "%")
            {
                if (m_Cluster == "AI / Big Data")
                {
                    newSql += "and Business_Area in ('AI / Big Data','Open Data') ";
                }
                else
                {
                    //newSql += "and Business_Area like (@Cluster) ";
                    newSql += "and Business_Area like '" + m_Cluster + "' ";
                }
            }
            m_ss8 = m_ss8 == null ? "%" : lstSS8.SelectedValue;
            if (m_ss8 != "%")
            {
                newSql += "and App.SmartSpace='" + m_ss8 + "' ";
            }

            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            String sqlOrderBy = "";
            if (m_SortCluster == "Application_Number")
                sqlOrderBy = " order by App.";
            else
                sqlOrderBy = " order by ";
            if (m_SortCluster == m_SortApplicationNo)
            {
                sqlOrderBy += m_SortCluster + " " + m_SortClusterOrder;
            }
            else
            {
                sqlOrderBy += m_SortCluster + " " + m_SortClusterOrder + ", " + m_SortApplicationNo + " " + m_SortClusterOrder;
            }
            //lbldebug.Visible = true;
            //lbldebug.Text = "ccmf order " + sqlOrderBy;
            newSql += sqlOrderBy;

            return newSql;
        }


        private string constructCPIPScript()
        {

            string newSql = "SELECT App.Application_Number as AppNo"
                            + ",Status"
                            + ",Company_Name_Eng"
                            + ",isNUll(Company_Name_Chi, '') as Company_Name_Chi"
                            + ",isNUll((Select Total_Score from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPIP BDM' ),-1) as BDM_Score "
                            + ",isNUll((Select Total_Score from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'Senior Manager' ),-1) as SeniorManager_Score"
                            + ",isNUll((Select Total_Score from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPMO' ),-1) as CPMO_Score"
                            + ",isNUll((Select Remarks from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPIP BDM' ),'') as BDMRemarks"
                            + ",isNUll((Select Remarks from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'Senior Manager' ),'') as SrRemarks"
                            + ",isNUll((Select Remarks from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPMO' ),'') as CPMORemarks"
                            + ",IIF(((select count(*) from TB_SCREENING_INCUBATION_SCORE sa where sa.Application_Number = App.Application_Number and Total_Score <> 0) > 0)"
                            + ",(select Sum([Total_Score]) / "
                            + "(select count(*) from TB_SCREENING_INCUBATION_SCORE sa where sa.Application_Number = App.Application_Number and Total_Score <> 0) "
                            + "as AvgScore from TB_SCREENING_INCUBATION_SCORE a where a.Application_Number = App.Application_Number) , 0) as Average_Score "
                            + ",Incubation_ID"
                            + ",isNUll(App.Programme_ID, '') as Programme_ID"
                            + ",isNUll(Business_Area, '') as Business_Area"
                //+ ",isNUll(Abstract, '') as PrjDesc"
                            + ",REPLACE(REPLACE(REPLACE(CAST(Abstract as NVARCHAR(MAX)),char(13)+char(10),' '),char(9),' '),char(59),',') as PrjDesc"
                            + ",isNUll(sl.Remarks_To_Vetting, '') as Remarks_To_Vetting"
                            + ",isNull(sl.Shortlisted, 0) as Shortlisted "
                            + "FROM TB_INCUBATION_APPLICATION App "
                            + "left join TB_APPLICATION_SHORTLISTING sl on sl.Application_Number = App.Application_Number "
                            + "where App.Intake_Number = @IntakeNumber "
                //+ "and App.Status like @Status "
                            + "and App.Status <> 'Saved' "
                            + "and App.Status <> 'Deleted' ";

            if (m_Status != "%")
                newSql += "and App.Status like '" + m_Status + "' ";

            if (m_Cluster != "%")
            {
                if (m_Cluster == "AI / Big Data")
                {
                    newSql += "and Business_Area in ('AI / Big Data','Open Data') ";
                }
                else
                {
                    //newSql += "and Business_Area like (@Cluster) ";
                    newSql += "and Business_Area like '" + m_Cluster + "' ";

                }
            }

            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            String sqlOrderBy = "";
            if (m_SortCluster == "Application_Number")
                sqlOrderBy = " order by App.";
            else
                sqlOrderBy = " order by ";
            if (m_SortCluster == m_SortApplicationNo)
            {
                sqlOrderBy += m_SortCluster + " " + m_SortClusterOrder;
            }
            else
            {
                sqlOrderBy += m_SortCluster + " " + m_SortClusterOrder + ", " + m_SortApplicationNo + " " + m_SortClusterOrder;
            }
            newSql += sqlOrderBy;

            //lbldebug.Visible = true;
            //lbldebug.Text = "CPIP  order m_Status =" + m_Status + " m_Cluster=" + m_Cluster;
            return newSql;

        }

        private string constructCASPScript()
        {

            string newSql = "SELECT App.CASP_ID, App.Application_No as AppNo"
                            + ",App.Status"
                            + ",App.Submitted_Date"
                //+ ",isNUll(Company_Name_Chi, '') as Company_Name_Chi"
                //+ ",isNUll((Select Total_Score from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPIP BDM' ),-1) as BDM_Score "
                //+ ",isNUll((Select Total_Score from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'Senior Manager' ),-1) as SeniorManager_Score"
                //+ ",isNUll((Select Total_Score from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPMO' ),-1) as CPMO_Score"
                //+ ",isNUll((Select Remarks from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPIP BDM' ),'') as BDMRemarks"
                //+ ",isNUll((Select Remarks from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'Senior Manager' ),'') as SrRemarks"
                //+ ",isNUll((Select Remarks from TB_SCREENING_INCUBATION_SCORE s where s.Application_Number = App.Application_Number and role = 'CPMO' ),'') as CPMORemarks"
                //+ ",(select Sum([Total_Score]) / "
                //+ "(select count(*) from TB_SCREENING_CCMF_SCORE sa where sa.Application_Number = App.Application_Number and Total_Score <> 0) "
                //+ "as AvgScore from TB_SCREENING_CCMF_SCORE a where a.Application_Number = App.Application_Number) as Average_Score "
                //+ ",Incubation_ID"
                // + ",isNUll(Business_Area, '') as Business_Area"
                //+ ",isNUll(Abstract, '') as PrjDesc"
                //+ ",isNUll(sl.Remarks_To_Vetting, '') as Remarks_To_Vetting"

                            + ",isNUll(App.Programme_ID, '') as Programme_ID"

                            + ",isNull(sl.Shortlisted, 0) as Shortlisted "
                            + "FROM TB_CASP_APPLICATION App "
                            + "left join TB_PROGRAMME_INTAKE intake on intake.Programme_ID = App.Programme_ID "
                            + "left join TB_APPLICATION_SHORTLISTING sl on sl.Application_Number = App.Application_No "
                            + "where intake.Intake_Number = @IntakeNumber "
                //+ "and App.Status like @Status "
                            + "and App.Status <> 'Saved' "
                            + "and App.Status <> 'Deleted' ";

            if (m_Status != "%")
                newSql += "and App.Status like '" + m_Status + "' ";

            //if (m_Cluster != "%")
            //{
            //    if (m_Cluster == "AI / Big Data")
            //    {
            //        newSql += "and Business_Area in ('AI / Big Data','Open Data') ";
            //    }
            //    else
            //    {
            //        //newSql += "and Business_Area like (@Cluster) ";
            //        newSql += "and Business_Area like '" + m_Cluster + "' ";

            //    }
            //}

            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            String sqlOrderBy = "";
            if (m_SortCluster == "Application_Number")
                sqlOrderBy = " order by App.";
            else
                sqlOrderBy = " order by ";
            if (m_SortCluster == m_SortApplicationNo)
            {
                sqlOrderBy += m_SortCluster + " " + m_SortClusterOrder;
            }
            else
            {
                sqlOrderBy += m_SortCluster + " " + m_SortClusterOrder;// + ", " + m_SortApplicationNo + " " + m_SortClusterOrder;
            }
            newSql += sqlOrderBy;

            //lbldebug.Visible = true;
            //lbldebug.Text = "CPIP  order m_Status =" + m_Status + " m_Cluster=" + m_Cluster;
            return newSql;

        }
        private string GetApplicationURL(string m_Programme, string ApplicationID, string ProgrammeID)
        {
            string URL = "";
            var qrystring = "";
            if (m_Programme == "CPIP")
            {
                //CPIP
                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue
                            + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue
                            + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue
                            + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue
                            + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue
                            + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue
                            + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
                URL = CPIP_URL;
            }
            else if (m_Programme == "CASP")
            {

                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue
                            + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue
                            + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue
                            + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue
                            + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue
                            + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue
                            + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
                URL = CASP_URL;
            }
            else if (m_Programme == "CCMFGBAYEP")
            {
                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue
                    + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue
                    + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue
                    + "&" + m_qryStrStream + "=" + lststream.SelectedValue
                    + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue
                    + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue
                    + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue
                    + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
                //URL = CCMFGBAYEP_URL;
                URL = CCMF_URL.Replace("CCMF_Internal", "CCMFGBAYEP_Internal");
            }
            else
            {
                //CCMF
                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue
                    + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue
                    + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue
                    + "&" + m_qryStrStream + "=" + lststream.SelectedValue
                    + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue
                    + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue
                    + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue
                    + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
                URL = CCMF_URL;
            }

            URL += "?app=" + ApplicationID + "&prog=" + ProgrammeID + qrystring;
            return URL;
        }

        private List<ApplicationList> fillAppList(SqlDataReader reader, string m_programme)
        {
            List<ApplicationList> applicationList = new List<ApplicationList>();
            m_AppCount = 0;
            while (reader.Read())
            {
                ApplicationList applst = new ApplicationList();

                applst.ApplicationNo = (String)reader.GetValue(reader.GetOrdinal("AppNo"));
                if (m_programme == "CCMF" || m_programme == "CUPP")
                {
                    applst.ProjectName = (String)reader.GetValue(reader.GetOrdinal("Project_Name_Eng"));
                    applst.ProjectNameChinese = (String)reader.GetValue(reader.GetOrdinal("Project_Name_Chi"));
                    applst.ProgrammeType = (String)reader.GetValue(reader.GetOrdinal("Programme_Type"));
                    applst.ApplicationType = (String)reader.GetValue(reader.GetOrdinal("CCMF_Application_Type"));
                    applst.ApplicationID = Convert.ToString(reader.GetGuid(reader.GetOrdinal("CCMF_ID")));
                    applst.SmartSpace = Convert.ToString(reader.GetValue(reader.GetOrdinal("SmartSpace")));
                    if (m_programme != "CUPP")
                    {
                        if ((String)reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")) == "Professional")
                            applst.HongKongProgrammeStream = "PRO";
                        else
                            applst.HongKongProgrammeStream = "YEP";
                    }
                   
                }
                else if (m_programme == "CCMFGBAYEP")
                {
                    applst.ProjectName = (String)reader.GetValue(reader.GetOrdinal("Project_Name_Eng"));
                    applst.ProjectNameChinese = (String)reader.GetValue(reader.GetOrdinal("Project_Name_Chi"));
                    applst.ProgrammeType = (String)reader.GetValue(reader.GetOrdinal("Programme_Type"));
                    applst.ApplicationType = (String)reader.GetValue(reader.GetOrdinal("CCMF_Application_Type"));
                    applst.ApplicationID = Convert.ToString(reader.GetGuid(reader.GetOrdinal("CCMF_ID")));
                    applst.SmartSpace = Convert.ToString(reader.GetValue(reader.GetOrdinal("SmartSpace")));                    
                }
                else if (m_programme == "CASP")
                {
                    applst.Submitted_Date = (DateTime)reader.GetValue(reader.GetOrdinal("Submitted_Date"));
                    applst.ApplicationID = Convert.ToString(reader.GetGuid(reader.GetOrdinal("CASP_ID")));

                }
                else if (m_programme == "CPIP")
                {
                    applst.CompanyName = (String)reader.GetValue(reader.GetOrdinal("Company_Name_Eng"));
                    applst.CompanyNameChinese = (String)reader.GetValue(reader.GetOrdinal("Company_Name_Chi"));
                    applst.ApplicationID = Convert.ToString(reader.GetGuid(reader.GetOrdinal("Incubation_ID")));
                }
                applst.Status = (String)reader.GetValue(reader.GetOrdinal("Status"));
                if (m_programme != "CASP")
                {
                    applst.ProjectDescription = (String)reader.GetValue(reader.GetOrdinal("PrjDesc"));
                    applst.Cluster = (String)reader.GetValue(reader.GetOrdinal("Business_Area"));

                    if (applst.Cluster.ToLower() == "open data")
                        applst.Cluster = "AI / Big Data";


                    if (applst.Status == "Disqualified")
                    {
                        applst.BDMScore = "NA";
                        applst.SrManagerScore = "NA";
                        applst.CPMOScore = "NA";
                        applst.AverageScore = "NA";
                    }
                    else
                    {
                        Decimal score = 0;

                        score = reader.GetDecimal(reader.GetOrdinal("BDM_Score"));
                        if (score == -1)
                            applst.BDMScore = "";
                        else
                        {
                            applst.BDMScore = score.ToString("F3", CultureInfo.InvariantCulture);
                        }

                        score = reader.GetDecimal(reader.GetOrdinal("SeniorManager_Score"));
                        if (score == -1)
                            applst.SrManagerScore = "";
                        else
                        {
                            applst.SrManagerScore = score.ToString("F3", CultureInfo.InvariantCulture);
                        }

                        score = reader.GetDecimal(reader.GetOrdinal("CPMO_Score"));
                        if (score == -1)
                            applst.CPMOScore = "";
                        else
                        {
                            applst.CPMOScore = score.ToString("F3", CultureInfo.InvariantCulture);
                        }

                        if (reader.IsDBNull(reader.GetOrdinal("Average_Score")))
                            applst.AverageScore = "";
                        else
                            applst.AverageScore = (reader.GetDecimal(reader.GetOrdinal("Average_Score"))).ToString("F3", CultureInfo.InvariantCulture);
                    }

                    applst.Shortlisted = (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted"));

                    applst.Remarks = " ";
                    string BDMrmk = (String)reader.GetValue(reader.GetOrdinal("BDMRemarks"));
                    string SrMgrrmk = (String)reader.GetValue(reader.GetOrdinal("SrRemarks"));
                    string CPMOrmk = (String)reader.GetValue(reader.GetOrdinal("CPMORemarks"));

                    if (BDMrmk != "")
                        applst.Remarks += "BDM: " + BDMrmk + "<br>";
                    if (SrMgrrmk != "")
                        applst.Remarks += "Sr Mgr: " + SrMgrrmk + "<br>";
                    if (CPMOrmk != "")
                        applst.Remarks += "CPMO: " + CPMOrmk + "<br>";

                    applst.RemarksForVetting = (String)reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting"));
                    if (applst.RemarksForVetting != "")
                    {
                        applst.RemarksForVetting = applst.RemarksForVetting.Replace("Senior Manager Remark :", "<br>Senior Manager Remark :");
                        applst.RemarksForVetting = applst.RemarksForVetting.Replace("CPMO Remark :", "<br>CPMO Remark :");
                    }
                    else
                        applst.RemarksForVetting = " ";

                }
                applst.APPNoURL = GetApplicationURL(m_programme, applst.ApplicationID, reader.GetInt32(reader.GetOrdinal("Programme_ID")).ToString());

                applicationList.Add(applst);
                m_AppCount++;
            }
            return applicationList;
        }

        private List<ApplicationList> getApplicationList()
        {
            var connection = new SqlConnection(connStr);
            connection.Open();
            List<ApplicationList> applicationList = new List<ApplicationList>();
            string m_Program = getCurrentProgramme();
            m_IntakeNum = lstIntakeNumber.SelectedValue;
            m_ProgrammeName = lstCyberportProgramme.SelectedValue;

            string sql = "";
            //Ccmf-cb added for ss8 
            if (m_Program == "CCMF" || m_Program == "CCMF-CB" || m_Program == "CCMFGBAYEP" || m_Program == "CUPP")
            {
                sql = constructCCMFScript();
            }
            else if (m_Program == "CASP")
            {
                sql = constructCASPScript();

            }
            else// CPIP
            {
                sql = constructCPIPScript();
            }
            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@ProgrammeName", m_ProgrammeName);
            command.Parameters.Add("@IntakeNumber", m_IntakeNum);

            //if (m_Status == "All Status")
            //    command.Parameters.Add("@Status", "*");
            //else
            //    command.Parameters.Add("@Status", m_Status);

            //if (m_Cluster == "All Cluster")
            //    command.Parameters.Add("@Cluster", "*");
            //else if (m_Cluster != "AI / Big Data")
            //    command.Parameters.Add("@Cluster", m_Cluster);

            //lbldebug.Visible = true;
            //lbldebug.Text += " applicationList Count = " + applicationList.Count ;
            try
            {
                var reader = command.ExecuteReader();
                applicationList = fillAppList(reader, m_Program);

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return applicationList;
        }


        private List<ApplicationList> GridViewApplicationBindData()
        {
            List<ApplicationList> applicationList = new List<ApplicationList>();
            m_ProgrammeName = lstCyberportProgramme.SelectedValue;
            m_IntakeNum = lstIntakeNumber.SelectedValue;
            m_Cluster = lstCluster.SelectedValue;
            m_Status = lstStatus.SelectedValue;
            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            if (m_ProgrammeName == m_selectfirstProgrammeNameText || m_IntakeNum == m_selectfirstIntakeNumberText)
            {
                GridViewApplication.DataSource = applicationList;
                GridViewApplication.DataBind();
                return applicationList;
            }

            SetGridViewColumnOrder();
            string m_Program = getCurrentProgramme();

            applicationList = getApplicationList();
            checkExportPDFBtn(applicationList, m_Program == "CASP" ? 0 : applicationList.Count);
            setGridColumns();

            lblcount.Text = m_AppCount.ToString() + " applications";
            lbldatetime.Text = "Deadline: " + m_ProgramDeadline;
            GridViewApplication.DataSource = applicationList;
            GridViewApplication.DataBind();
            return applicationList;
        }

        private void checkExportPDFBtn(List<ApplicationList> applicationList, int count)
        {
            btnExportPDFList.Value = "";
            if (count != 0)
            {
                btnCompleteScreening.Visible = true;
                btnExportPDF.Visible = true;
                string AppNo = "";
                applicationList.ForEach(x => AppNo += x.ApplicationNo + ",");
                btnExportPDF.Text = "Batch print Applications";
                btnExportPDFList.Value = AppNo;
            }
            else
            {
                btnCompleteScreening.Visible = false;
                btnExportPDF.Visible = false;
            }
        }

        //private List<ApplicationList> GridViewApplicationBindData()
        //{
        //    var connection = new SqlConnection(connStr);
        //    connection.Open();
        //    List<ApplicationList> applicationList = new List<ApplicationList>();
        //    try
        //    {
        //        m_ProgrammeName = lstCyberportProgramme.SelectedValue;
        //        m_IntakeNum = lstIntakeNumber.SelectedValue;
        //        m_Cluster = lstCluster.SelectedValue;
        //        m_Status = lstStatus.SelectedValue;
        //        m_SortCluster = lstSortCluster.SelectedValue;
        //        m_SortClusterOrder = radioSortCluster.SelectedValue;
        //        m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

        //        String sqlColumn = "select tbApplicatiion.Application_Number,tbApplicatiion.Status";
        //        String sqlFrom = " from TB_PROGRAMME_INTAKE tpi ";
        //        String sqlWhere = " where tpi.Programme_Name = @ProgrammeName "
        //                            + "and tpi.Intake_Number=@IntakeNumber and tbApplicatiion.Status like @Status "
        //            //+ "and tbApplicatiion.Business_Area like @Cluster "
        //                            + "and tbApplicatiion.Status <> 'Saved' "
        //                            + "and tbApplicatiion.Status <> 'Deleted'";
        //        String sqlOrderBy = "";
        //        String BDMRole = "";
        //        if (m_SortCluster == m_SortApplicationNo)
        //        {
        //            sqlOrderBy = " order by " + m_SortCluster + " " + m_SortClusterOrder;
        //        }
        //        else
        //        {
        //            sqlOrderBy = " order by  " + m_SortCluster + " " + m_SortClusterOrder + ", " + m_SortApplicationNo + " " + m_SortClusterOrder;
        //        }

        //        String sqlScreeningScoreTable = "";
        //        string sqlApplcationTable = "";
        //        String linkURl = "";
        //        String sqlColumnID = "";
        //        String sqlColumnProjectDesc = "";
        //        string m_Program = "";
        //        switch (m_ProgrammeName)
        //        {
        //            case "Cyberport Incubation Programme" :
        //                m_Program = "CPIP";
        //                break;
        //            case "Cyberport Creative Micro Fund - Hong Kong" :
        //                m_Program = "CCMF";
        //                break;
        //            case "Cyberport University Partnerhip Programme" :
        //                m_Program = "CUPP";
        //                break;
        //            case "Cyberport Creative Micro Fund - Cross Border" :
        //                m_Program = "CCMF-CB";
        //                break;
        //        }



        //        SetGridViewColumnOrder();
        //        if (m_Program == "CPIP")
        //        {
        //            //CPIP
        //            sqlColumn += ",isNull(tbApplicatiion.Company_Name_Eng,'') as Company_Name_Eng";
        //            sqlColumnID = " ,tbApplicatiion.Incubation_ID,tbApplicatiion.Programme_ID ";
        //            sqlScreeningScoreTable = "TB_SCREENING_INCUBATION_SCORE";
        //            sqlApplcationTable = "TB_INCUBATION_APPLICATION";
        //            sqlColumnProjectDesc = " ,tbApplicatiion.Abstract ";
        //            BDMRole = "CPIP BDM";

        //            linkURl = CPIP_URL;
        //        }
        //        else
        //        {
        //            //CCMF & CUPP
        //            sqlColumn += ",isNull(tbApplicatiion.Project_Name_Eng,'') as Project_Name_Eng"
        //                        + ",isNull(tbApplicatiion.Programme_Type,'') as Programme_Type"
        //                        + ",isNull(tbApplicatiion.CCMF_Application_Type,'') as CCMF_Application_Type";
        //            sqlColumnID = " ,tbApplicatiion.CCMF_ID,tbApplicatiion.Programme_ID ";
        //            sqlScreeningScoreTable = "TB_SCREENING_CCMF_SCORE";
        //            sqlApplcationTable = "TB_CCMF_APPLICATION";
        //            sqlColumnProjectDesc = " ,tbApplicatiion.Abstract_Eng ";
        //            BDMRole = "CCMF BDM";

        //            linkURl = CCMF_URL;

        //            if (m_Program == "CCMF")
        //            {
        //                sqlColumn += ",isNull(tbApplicatiion.Hong_Kong_Programme_Stream,'') as Hong_Kong_Programme_Stream";
        //                sqlWhere += " and tbApplicatiion.Hong_Kong_Programme_Stream like @Hong_Kong_Programme_Stream ";
        //            }

        //        }


        //         setGridColumns();

        //         sqlColumn += ",case when tbsscoreBDM.BDM_Score is null then -1 else tbsscoreBDM.BDM_Score end as BDM_Score,isNull(tbsscoreBDM.BDM_Remarks,'') as BDM_Remarks,case when tbsscoreSeniorManager.SeniorManager_Score is null then -1 else tbsscoreSeniorManager.SeniorManager_Score end as SeniorManager_Score,isNull(tbsscoreSeniorManager.SeniorManager_Remarks,'') as SeniorManager_Remarks,case when tbsscoreCPMO.CPMO_Score is null then -1 else tbsscoreCPMO.CPMO_Score end as CPMO_Score,isNull(tbsscoreCPMO.CPMO_Remarks,'') as CPMO_Remarks,isNull(case when (tbsscoreBDM.BDM_Score is null and tbsscoreSeniorManager.SeniorManager_Score is null and tbsscoreCPMO.CPMO_Score is null) then -1 when (tbsscoreBDM.BDM_Score + tbsscoreSeniorManager.SeniorManager_Score + tbsscoreCPMO.CPMO_Score) = 0 then 0 else (isNull(tbsscoreBDM.BDM_Score,0) +isNull(tbsscoreSeniorManager.SeniorManager_Score,0) +isNull(tbsscoreCPMO.CPMO_Score,0))/ nullif(((case when isNull(tbsscoreBDM.BDM_Score,0) = 0 then 0 else 1 end)+(case when isNull(tbsscoreSeniorManager.SeniorManager_Score,0) = 0 then 0 else 1 end) + (case when isNull(tbsscoreCPMO.CPMO_Score,0) = 0 then 0 else 1 end)) ,0) end ,-1) as Average_Score ";

        //         sqlFrom += " inner join " + sqlApplcationTable + " tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID "
        //                     + "LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number "
        //                         + "and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";

        //         sqlFrom += " left join (select tbsscore.Total_Score as BDM_Score,tbsscore.Remarks as BDM_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='" + BDMRole + "') tbsscoreBDM on tbsscoreBDM.Application_Number = tbApplicatiion.Application_Number left join (select tbsscore.Total_Score as SeniorManager_Score,tbsscore.Remarks as SeniorManager_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='Senior Manager') tbsscoreSeniorManager on tbsscoreSeniorManager.Application_Number = tbApplicatiion.Application_Number left join (select tbsscore.Total_Score as CPMO_Score,tbsscore.Remarks as CPMO_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPMO') tbsscoreCPMO on tbsscoreCPMO.Application_Number = tbApplicatiion.Application_Number ";

        //         sqlColumn += ",Application_Deadline,isNull(tbAppShortlisting.Remarks_To_Vetting,'') as Remarks_To_Vetting,isNull(tbAppShortlisting.Shortlisted,0) as Shortlisted ";
        //         sqlColumn += sqlColumnID;
        //         sqlColumn += " ,tbApplicatiion.Business_Area ";
        //         sqlColumn += sqlColumnProjectDesc;
        //         if (m_Cluster != "All Cluster")
        //         {
        //             if (m_Cluster == "AI / Big Data")
        //             {
        //                 sqlWhere += "and tbApplicatiion.Business_Area in ('AI / Big Data','Open Data') ";
        //             }
        //             else
        //             {
        //                 sqlWhere += "and tbApplicatiion.Business_Area like (@Cluster) ";

        //             }
        //         }


        //         var sqlString = "select * from( " + sqlColumn + sqlFrom + sqlWhere + ") as result " + sqlOrderBy;
        //        //if (m_Program == "CPIP")
        //        //{
        //        //    linkURl = CPIP_URL;
        //        //    sqlString = constructCPIPScript();
        //        //}
        //        //else
        //        //{
        //        //    linkURl = CCMF_URL;
        //        //    sqlString = constructCCMFScript();
        //        //}
        //        if (m_ProgrammeName == m_selectfirstProgrammeNameText || m_IntakeNum == m_selectfirstIntakeNumberText)
        //        {
        //            GridViewApplication.DataSource = applicationList;
        //            GridViewApplication.DataBind();
        //            return applicationList;
        //        }

        //        //lbltest.Text = sqlString;
        //        var command = new SqlCommand(sqlString, connection);
        //        command.Parameters.Add("@ProgrammeName", m_ProgrammeName);
        //        command.Parameters.Add("@IntakeNumber", m_IntakeNum);
        //        command.Parameters.Add("@Status", m_Status);

        //        if (m_Cluster != "All Cluster" && m_Cluster != "AI / Big Data")
        //            command.Parameters.Add("@Cluster", m_Cluster);

        //        //if (!lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //        if(m_Program == "CCMF")
        //        {
        //            //CCMF
        //            command.Parameters.Add("@Hong_Kong_Programme_Stream", lststream.SelectedValue);
        //        }

        //        //lbltest.Text = m_Program + "<br/>" + sqlString; //return;
        //        //lbltest.Text += "Blind Gride";// +"<br/>" + sqlString; //return;

        //        var reader = command.ExecuteReader();


        //        int count = 0;
        //        int countlatest = 0;
        //        Boolean remarksForVettingvisiable = false;
        //        while (reader.Read())
        //        {
        //            count++;

        //            ApplicationList applst = new ApplicationList();
        //            applst.ApplicationNo = (String)reader.GetValue(0);
        //            countlatest = 1;

        //            applst.ProjectDescription = "";
        //            applst.Status = (String)reader.GetValue(1);

        //            //if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //            if(m_Program == "CPIP")
        //            {
        //                //CPIP
        //                applst.CompanyName = (String)reader.GetValue(2);


        //                countlatest = 2;
        //            }
        //            else
        //            {
        //                //CCMF
        //                countlatest++;
        //                applst.ProjectName = (String)reader.GetValue(countlatest);

        //                countlatest++;
        //                applst.ProgrammeType = (String)reader.GetValue(countlatest);

        //                countlatest++;
        //                applst.ApplicationType = (String)reader.GetValue(countlatest);

        //                if (m_Program == "CCMF")
        //                {
        //                    countlatest++;
        //                    var HongKongProgrammeStream = (String)reader.GetValue(countlatest);
        //                    if (HongKongProgrammeStream == "Professional")
        //                    {
        //                        applst.HongKongProgrammeStream = "PRO";
        //                    }
        //                    if (HongKongProgrammeStream == "Young Entrepreneur")
        //                    {
        //                        applst.HongKongProgrammeStream = "YEP";
        //                    }
        //                }

        //            }

        //            if (applst.Status == "Disqualified")
        //            {
        //                applst.BDMScore = "NA";
        //                applst.SrManagerScore = "NA";
        //                applst.CPMOScore = "NA";
        //                applst.AverageScore = "NA";
        //            }
        //            else
        //            {
        //                var BDMScore = float.Parse(reader.GetValue(countlatest + 1).ToString());
        //                if (BDMScore == -1)
        //                {
        //                    applst.BDMScore = "";
        //                }
        //                else
        //                {
        //                    applst.BDMScore = BDMScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //                var SrManagerScore = float.Parse(reader.GetValue(countlatest + 3).ToString());
        //                if (SrManagerScore == -1)
        //                {
        //                    applst.SrManagerScore = "";
        //                }
        //                else
        //                {

        //                    applst.SrManagerScore = SrManagerScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //                var CPMOScore = float.Parse(reader.GetValue(countlatest + 5).ToString());
        //                if (CPMOScore == -1)
        //                {
        //                    applst.CPMOScore = "";
        //                }
        //                else
        //                {

        //                    applst.CPMOScore = CPMOScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //                var AverageScore = float.Parse(reader.GetValue(countlatest + 7).ToString());
        //                if (AverageScore == -1)
        //                {
        //                    applst.AverageScore = "";
        //                }
        //                else
        //                {

        //                    applst.AverageScore = AverageScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //           }

        //            var Remarks = "";
        //            if (reader.GetString(countlatest + 2) != "")
        //            {
        //                Remarks += "BDM: " + reader.GetString(countlatest + 2) + "<br>";
        //            }

        //            if (reader.GetString(countlatest + 4) != "")
        //            {
        //                Remarks += "sr Mgr: " + reader.GetString(countlatest + 4) + "<br>";
        //            }

        //            if (reader.GetString(countlatest + 6) != "")
        //            {
        //                Remarks += "CPMO: " + reader.GetString(countlatest + 6) + "<br>";
        //            }

        //            applst.Remarks = Remarks;

        //            countlatest = countlatest + 7;

        //            countlatest++;
        //            lbldatetime.Text = "Deadline: " + reader.GetDateTime(countlatest).ToString("d MMM yyyy h:mmtt");

        //            countlatest++;
        //            applst.RemarksForVetting = (String)reader.GetValue(countlatest);
        //            applst.RemarksForVetting = applst.RemarksForVetting.Replace("Senior Manager Remark :", "<br>Senior Manager Remark :");
        //            applst.RemarksForVetting = applst.RemarksForVetting.Replace("CPMO Remark :", "<br>CPMO Remark :");
        //            if (m_Role.Contains("BDM") && (applst.Status == "CPMO Reviewed" || applst.Status == "Complete Screening"))
        //            {
        //                remarksForVettingvisiable = true;

        //            }

        //            countlatest++;
        //            applst.Shortlisted = (Boolean)reader.GetValue(countlatest);

        //            countlatest++;
        //            var app = reader.GetValue(countlatest).ToString(); // CCMF_ID||Incubation_ID

        //            countlatest++;
        //            var prog = reader.GetInt32(countlatest); //Programme_ID
        //            var qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue + "&" + m_qryStrStream + "=" + lststream.SelectedValue + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
        //            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //            {
        //                //CPIP
        //                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
        //            }
        //            else
        //            {
        //                //CCMF
        //                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue + "&" + m_qryStrStream + "=" + lststream.SelectedValue + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;

        //            }
        //            applst.APPNoURL = linkURl + "?app=" + app + "&prog=" + prog + qrystring;

        //            countlatest++;
        //            applst.Cluster = reader.GetString(countlatest);
        //            if (applst.Cluster.ToLower() == "open data")
        //                applst.Cluster = "AI / Big Data";

        //            countlatest++;
        //            applst.ProjectDescription = reader.GetString(countlatest);

        //            applicationList.Add(applst);


        //        }
        //        if (remarksForVettingvisiable)
        //        {
        //            GridViewApplication.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = true;
        //        }
        //        else
        //        {

        //            GridViewApplication.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = false;
        //        }

        //        btnExportPDFList.Value = "";
        //        if (count != 0)
        //        {
        //            // commented in 2018
        //            //btnExportPDF.Visible = true;
        //            btnCompleteScreening.Visible = true;
        //            btnExportPDF.Visible = true;
        //            string AppNo = "";
        //            applicationList.ForEach(x => AppNo += x.ApplicationNo + ",");
        //            btnExportPDF.Text = "Batch print Applications";
        //            btnExportPDFList.Value = AppNo;
        //        }
        //        else
        //        {
        //            btnCompleteScreening.Visible = false;
        //            btnExportPDF.Visible = false;
        //        }

        //        //lbldebug.Text = "Grid [" + m_Cluster + "]";
        //        lblcount.Text = count.ToString() + " applications";
        //        GridViewApplication.DataSource = applicationList;
        //        GridViewApplication.DataBind();



        //        reader.Dispose();
        //        command.Dispose();

        //    }
        //    finally
        //    {
        //        connection.Close();
        //        connection.Dispose();
        //    }
        //    return applicationList;
        //}

        //private List<ApplicationList> GetAllOfApplicationBindData()
        //{
        //    var connection = new SqlConnection(connStr);
        //    connection.Open();
        //    List<ApplicationList> applicationList = new List<ApplicationList>();
        //    try
        //    {
        //        m_ProgrammeName = lstCyberportProgramme.SelectedValue;
        //        m_IntakeNum = lstIntakeNumber.SelectedValue;
        //        //m_Cluster = "%";
        //        m_Cluster = lstCluster.SelectedValue;
        //        m_Status = "%";
        //        m_SortCluster = lstSortCluster.SelectedValue;
        //        m_SortClusterOrder = radioSortCluster.SelectedValue;
        //        m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

        //        String sqlColumn = "select tbApplicatiion.Application_Number,tbApplicatiion.Status";
        //        String sqlFrom = " from TB_PROGRAMME_INTAKE tpi ";
        //        String sqlWhere = " where tpi.Programme_Name = @ProgrammeName "
        //                            + "and tpi.Intake_Number=@IntakeNumber "
        //                            + "and tbApplicatiion.Status like @Status "
        //                            + "and tbApplicatiion.Status <> 'Saved' "
        //                            + "and tbApplicatiion.Status <> 'Deleted'";
        //        String sqlOrderBy = "";
        //        String BDMRole = "";
        //        if (m_SortCluster == m_SortApplicationNo)
        //        {
        //            sqlOrderBy = " order by " + m_SortCluster + " " + m_SortClusterOrder;
        //        }
        //        else
        //        {
        //            sqlOrderBy = " order by  " + m_SortCluster + " " + m_SortClusterOrder + ", " + m_SortApplicationNo + " " + m_SortClusterOrder;
        //        }

        //        //String sqlWhere = "";
        //        String sqlScreeningScoreTable = "";
        //        String linkURl = "";
        //        String sqlColumnID = "";
        //        String sqlColumnProjectDesc = "";


        //        SetGridViewColumnOrder();
        //        if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //        {
        //            //CPIP
        //            sqlColumn += ",isNull(tbApplicatiion.Company_Name_Eng,'') as Company_Name_Eng,isNull(tbApplicatiion.Company_Name_Chi,'') as Company_Name_Chi ";
        //            sqlColumnID = " ,tbApplicatiion.Incubation_ID,tbApplicatiion.Programme_ID ";
        //            sqlFrom += " inner join TB_INCUBATION_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID "
        //                        + "LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting "
        //                        +   "on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";
        //            sqlScreeningScoreTable = "TB_SCREENING_INCUBATION_SCORE";
        //            sqlColumnProjectDesc = " ,tbApplicatiion.Abstract ";
        //            BDMRole = "CPIP BDM";





        //            linkURl = CPIP_URL;
        //        }
        //        else
        //        {
        //            //CCMF
        //            sqlColumn += ",isNull(tbApplicatiion.Project_Name_Eng,'') as Project_Name_Eng,isNull(tbApplicatiion.Project_Name_Chi,'') as Project_Name_Chi"
        //                        + ",isNull(tbApplicatiion.Programme_Type,'') as Programme_Type"
        //                        + ",isNull(tbApplicatiion.Hong_Kong_Programme_Stream,'') as Hong_Kong_Programme_Stream"
        //                        + ",isNull(tbApplicatiion.CCMF_Application_Type,'') as CCMF_Application_Type";
        //            sqlColumnID = " ,tbApplicatiion.CCMF_ID,tbApplicatiion.Programme_ID ";
        //            sqlFrom += " inner join TB_CCMF_APPLICATION tbApplicatiion on tpi.Programme_ID = tbApplicatiion.Programme_ID "
        //                    + "LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting "
        //                    +       "on tbAppShortlisting.Application_Number = tbApplicatiion.Application_Number and tbAppShortlisting.Programme_ID = tpi.Programme_ID ";
        //            sqlScreeningScoreTable = "TB_SCREENING_CCMF_SCORE";
        //            sqlColumnProjectDesc = " ,tbApplicatiion.Abstract_Eng ";
        //            BDMRole = "CCMF BDM";

        //            linkURl = CCMF_URL;

        //            sqlWhere += " and tbApplicatiion.Hong_Kong_Programme_Stream like @Hong_Kong_Programme_Stream ";
        //        }

        //        setGridColumns();

        //        sqlColumn += ",case when tbsscoreBDM.BDM_Score is null then -1 else tbsscoreBDM.BDM_Score end as BDM_Score"
        //                    + ",isNull(tbsscoreBDM.BDM_Remarks,'') as BDM_Remarks"
        //                    + ",case when tbsscoreSeniorManager.SeniorManager_Score is null then -1 else tbsscoreSeniorManager.SeniorManager_Score end as SeniorManager_Score"
        //                    + ",isNull(tbsscoreSeniorManager.SeniorManager_Remarks,'') as SeniorManager_Remarks"
        //                    + ",case when tbsscoreCPMO.CPMO_Score is null then -1 else tbsscoreCPMO.CPMO_Score end as CPMO_Score"
        //                    + ",isNull(tbsscoreCPMO.CPMO_Remarks,'') as CPMO_Remarks"
        //                    + ",isNull(case when (tbsscoreBDM.BDM_Score is null and tbsscoreSeniorManager.SeniorManager_Score is null and tbsscoreCPMO.CPMO_Score is null) then -1 "
        //                    +               "when (tbsscoreBDM.BDM_Score + tbsscoreSeniorManager.SeniorManager_Score + tbsscoreCPMO.CPMO_Score) = 0 then 0 "
        //                    +               "else (isNull(tbsscoreBDM.BDM_Score,0) +isNull(tbsscoreSeniorManager.SeniorManager_Score,0) +isNull(tbsscoreCPMO.CPMO_Score,0))"
        //                    +               "/ nullif(((case when isNull(tbsscoreBDM.BDM_Score,0) = 0 then 0 else 1 end)"
        //                    +               " +(case when isNull(tbsscoreSeniorManager.SeniorManager_Score,0) = 0 then 0 else 1 end) "
        //                    +               " + (case when isNull(tbsscoreCPMO.CPMO_Score,0) = 0 then 0 else 1 end)) ,0) end ,-1) as Average_Score ";

        //        sqlFrom += " left join (select tbsscore.Total_Score as BDM_Score,tbsscore.Remarks as BDM_Remarks,tbsscore.Application_Number "
        //                    +               "from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='" + BDMRole + "') tbsscoreBDM "
        //                    +       "on tbsscoreBDM.Application_Number = tbApplicatiion.Application_Number "
        //                    + "left join (select tbsscore.Total_Score as SeniorManager_Score,tbsscore.Remarks as SeniorManager_Remarks,tbsscore.Application_Number "
        //                    +           "from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='Senior Manager') tbsscoreSeniorManager "
        //                    +       "on tbsscoreSeniorManager.Application_Number = tbApplicatiion.Application_Number "
        //                    + "left join (select tbsscore.Total_Score as CPMO_Score,tbsscore.Remarks as CPMO_Remarks,tbsscore.Application_Number "
        //                    +           "from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPMO') tbsscoreCPMO "
        //                    +       "on tbsscoreCPMO.Application_Number = tbApplicatiion.Application_Number ";

        //        sqlColumn += ",Application_Deadline,isNull(tbAppShortlisting.Remarks_To_Vetting,'') as Remarks_To_Vetting,isNull(tbAppShortlisting.Shortlisted,0) as Shortlisted ";
        //        sqlColumn += sqlColumnID;
        //        sqlColumn += " ,tbApplicatiion.Business_Area ";
        //        sqlColumn += sqlColumnProjectDesc;
        //        if (m_Cluster != "All Cluster")
        //        {
        //            if (m_Cluster.ToLower() == "ai / big data")
        //            {
        //                sqlWhere += " and Business_Area in ('AI / Big Data','Open Data')";
        //            }
        //            else
        //            {
        //                sqlWhere += " and Business_Area like (@Cluster)";
        //            }
        //        }
        //        //lbldebug2.Text = "GetAll SQLWhere [" + sqlWhere + "]";
        //        var sqlString = "select * from( " + sqlColumn + sqlFrom + sqlWhere + ") as result " + sqlOrderBy;

        //        if (m_ProgrammeName == m_selectfirstProgrammeNameText || m_IntakeNum == m_selectfirstIntakeNumberText)
        //        {
        //            GridViewApplication.DataSource = applicationList;
        //            GridViewApplication.DataBind();
        //            return null;
        //        }

        //        var command = new SqlCommand(sqlString, connection);
        //        command.Parameters.Add("@ProgrammeName", m_ProgrammeName);
        //        command.Parameters.Add("@IntakeNumber", m_IntakeNum);
        //        command.Parameters.Add("@Status", m_Status);
        //        if (m_Cluster != "All Cluster" && m_Cluster.ToLower() != "ai / big data")
        //            command.Parameters.Add("@Cluster", m_Cluster);

        //        if (!lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //        {
        //            //CCMF
        //            command.Parameters.Add("@Hong_Kong_Programme_Stream", "%");
        //        }
        //        //lbltest.Text += "Filter - " + sqlString; //return;
        //        var reader = command.ExecuteReader();


        //        int count = 0;
        //        int countlatest = 0;
        //        Boolean remarksForVettingvisiable = false;
        //        while (reader.Read())
        //        {
        //            count++;

        //            ApplicationList applst = new ApplicationList();
        //            applst.ApplicationNo = (String)reader.GetValue(0);
        //            countlatest = 1;

        //            applst.ProjectDescription = "";
        //            applst.Status = (String)reader.GetValue(1);

        //            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //            {
        //                //CPIP
        //                countlatest++;
        //                applst.CompanyName = (String)reader.GetValue(countlatest);

        //                countlatest++;
        //                applst.CompanyNameChinese = (String)reader.GetValue(countlatest);
        //            }
        //            else
        //            {
        //                //CCMF
        //                countlatest++;
        //                applst.ProjectName = (String)reader.GetValue(countlatest);

        //                countlatest++;
        //                applst.ProjectNameChinese = (String)reader.GetValue(countlatest);

        //                countlatest++;
        //                applst.ProgrammeType = (String)reader.GetValue(countlatest);

        //                countlatest++;
        //                var HongKongProgrammeStream = (String)reader.GetValue(countlatest);
        //                if (HongKongProgrammeStream == "Professional")
        //                {
        //                    applst.HongKongProgrammeStream = "PRO";
        //                }
        //                if (HongKongProgrammeStream == "Young Entrepreneur")
        //                {
        //                    applst.HongKongProgrammeStream = "YEP";
        //                }

        //                countlatest++;
        //                applst.ApplicationType = (String)reader.GetValue(countlatest);

        //            }


        //            if (applst.Status == "Disqualified")
        //            {
        //                applst.BDMScore = "NA";
        //                applst.SrManagerScore = "NA";
        //                applst.CPMOScore = "NA";
        //                applst.AverageScore = "NA";
        //            }
        //            else
        //            {
        //                var BDMScore = float.Parse(reader.GetValue(countlatest + 1).ToString());
        //                if (BDMScore == -1)
        //                {
        //                    applst.BDMScore = "";
        //                }
        //                else
        //                {
        //                    applst.BDMScore = BDMScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //                var SrManagerScore = float.Parse(reader.GetValue(countlatest + 3).ToString());
        //                if (SrManagerScore == -1)
        //                {
        //                    applst.SrManagerScore = "";
        //                }
        //                else
        //                {

        //                    applst.SrManagerScore = SrManagerScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //                var CPMOScore = float.Parse(reader.GetValue(countlatest + 5).ToString());
        //                if (CPMOScore == -1)
        //                {
        //                    applst.CPMOScore = "";
        //                }
        //                else
        //                {

        //                    applst.CPMOScore = CPMOScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //                var AverageScore = float.Parse(reader.GetValue(countlatest + 7).ToString());
        //                if (AverageScore == -1)
        //                {
        //                    applst.AverageScore = "";
        //                }
        //                else
        //                {

        //                    applst.AverageScore = AverageScore.ToString("F3", CultureInfo.InvariantCulture);
        //                }

        //            }

        //            var Remarks = "";
        //            if (reader.GetString(countlatest + 2) != "")
        //            {
        //                Remarks += "BDM: " + reader.GetString(countlatest + 2) + "<br>";
        //            }

        //            if (reader.GetString(countlatest + 4) != "")
        //            {
        //                Remarks += "sr Mgr: " + reader.GetString(countlatest + 4) + "<br>";
        //            }

        //            if (reader.GetString(countlatest + 6) != "")
        //            {
        //                Remarks += "CPMO: " + reader.GetString(countlatest + 6) + "<br>";
        //            }

        //            applst.Remarks = Remarks;

        //            countlatest = countlatest + 7;

        //            countlatest++;
        //            lbldatetime.Text = "Deadline: " + reader.GetDateTime(countlatest).ToString("d MMM yyyy h:mmtt");

        //            countlatest++;
        //            applst.RemarksForVetting = (String)reader.GetValue(countlatest);
        //            applst.RemarksForVetting = applst.RemarksForVetting.Replace("Senior Manager Remark :", "<br>Senior Manager Remark :");
        //            applst.RemarksForVetting = applst.RemarksForVetting.Replace("CPMO Remark :", "<br>CPMO Remark :");
        //            if (m_Role.Contains("BDM") && (applst.Status == "CPMO Reviewed" || applst.Status == "Complete Screening"))
        //            {
        //                remarksForVettingvisiable = true;

        //            }

        //            countlatest++;
        //            applst.Shortlisted = (Boolean)reader.GetValue(countlatest);

        //            countlatest++;
        //            var app = reader.GetValue(countlatest).ToString(); // CCMF_ID||Incubation_ID
        //            applst.ApplicationID = app;

        //            countlatest++;
        //            var prog = reader.GetInt32(countlatest); //Programme_ID
        //            var qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue + "&" + m_qryStrStream + "=" + lststream.SelectedValue + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
        //            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //            {
        //                //CPIP
        //                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;
        //            }
        //            else
        //            {
        //                //CCMF
        //                qrystring = "&" + m_qryStrProgName + "=" + lstCyberportProgramme.SelectedValue + "&" + m_qryStrIntakeNo + "=" + lstIntakeNumber.SelectedValue + "&" + m_qryStrCluster + "=" + lstCluster.SelectedValue + "&" + m_qryStrStream + "=" + lststream.SelectedValue + "&" + m_qryStrStatus + "=" + lstStatus.SelectedValue + "&" + m_qryStrSortColumn1 + "=" + lstSortCluster.SelectedValue + "&" + m_qryStrSortColumn2 + "=" + lstSortApplicationNo.SelectedValue + "&" + m_qryStrSortOrder1 + "=" + radioSortCluster.SelectedValue;

        //            }
        //            applst.APPNoURL = linkURl + "?app=" + app + "&prog=" + prog + qrystring;

        //            countlatest++;
        //            applst.Cluster = reader.GetString(countlatest);

        //            countlatest++;
        //            applst.ProjectDescription = reader.GetString(countlatest);

        //            applicationList.Add(applst);


        //        }

        //        if (remarksForVettingvisiable)
        //        {
        //            GridViewApplication.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = true;
        //        }
        //        else
        //        {

        //            GridViewApplication.Columns[GridViewColumnOrder["RemarksForVetting"]].Visible = false;
        //        }


        //        reader.Dispose();
        //        command.Dispose();
        //    }
        //    finally
        //    {

        //        connection.Close();
        //        connection.Dispose();
        //    }
        //    return applicationList;
        //}

        protected void lstCyberportProgramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                BindDataIntake();
                //BindDataCluster();
                BindDataSortColumn();

                CheckButtonVisiable(GridViewApplicationBindData());

            }
            finally
            {
                ConnectClose();
            }

            btnDownload.Enabled = true;

            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))// ||
            //    lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport University Partnerhip Programme"))
            {
                //CPIP
                lststream.Visible = false;
                lstSS8.Visible = false;
            }
            else if (lstCyberportProgramme.SelectedValue.ToString() == "Cyberport Accelerator Support Programme")
            {
                lststream.Visible = false;
                lstCluster.Visible = false;
                lstSS8.Visible = false;

            }
            else
            {
                //CCMF
                lststream.Visible = true;
                lstSS8.Visible = true;

            }
            lblcount.Text = "0 applications";
            lbldatetime.Text = "Deadline: ";
        }

        protected void lstIntakeNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                BindDataCluster();

                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }

            btnDownload.Enabled = true;
        }

        protected void lstStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                BindDataSortColumn();


                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void lstCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {

                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void lstSortCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                ;

                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void radioSortCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                ;

                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void lstSortApplicationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                ;

                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void radioSortApplicationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                ;

                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void CheckButtonVisiable(List<ApplicationList> applicationLists)
        {
            Boolean BDMFinalReviewStatus = false;

            btnCoordinatorproceedtoBDM.Visible = false;
            btnBDMproceedtoSeniorManager.Visible = false;
            btnCompleteScreening.Visible = false;
            btnSeniorManagerproceedtoCPMO.Visible = false;
            btnCPMOproceedtoFinalBDM.Visible = false;

            btnBDMproceedtoSeniorManager.Enabled = true;
            btnCompleteScreening.Enabled = true;
            btnSeniorManagerproceedtoCPMO.Enabled = true;
            btnCPMOproceedtoFinalBDM.Enabled = true;


            btnSaveShortlisted.Visible = false;
            btnDownload.Visible = false;
            btnCSV.Visible = false;
            BtnDuplicated_Submission.Visible = false;
            if (lstCyberportProgramme.SelectedValue.Contains("Cyberport Accelerator Support Programme"))
            {

            }
            else if (lstCyberportProgramme.SelectedValue != m_selectfirstProgrammeNameText && lstIntakeNumber.SelectedValue != m_selectfirstIntakeNumberText)
            {
                //var applicationLists = GetAllOfApplicationBindData();

                var ProgramIntakeStatus = SearchProgramIntakeListStatus();
                btnDownload.Visible = true;
                btnCSV.Visible = true;
                BtnDuplicated_Submission.Visible = true;
                if (applicationLists != null)
                {

                    var btnSaveShortlistedVisiable = false;

                    getReview();
                    if ((m_Role == "CCMF Coordinator" || m_Role == "CPIP Coordinator" || m_Role == "CASP Coordinator"))
                    {
                        btnCoordinatorproceedtoBDM.Visible = false;
                    }


                    if ((m_Role == "CCMF BDM" && checkCPIPOrCCMF() == "CCMF") || (m_Role == "CPIP BDM" && checkCPIPOrCCMF() == "CPIP"))
                    {
                        if (ProgramIntakeStatus == "Programme passed deadline")
                        {
                            var btnCoordinatorproceedtoBDMVisible = true;
                            foreach (ApplicationList row in applicationLists)
                            {
                                if (!(row.Status == "Deleted" || row.Status == "Disqualified" || row.Status == "BDM Reviewed" || row.Status == "CPMO Reviewed" || row.Status == "Sr. Mgr. Reviewed"))
                                {
                                    btnCoordinatorproceedtoBDMVisible = false;
                                }
                            }
                            btnBDMproceedtoSeniorManager.Visible = btnCoordinatorproceedtoBDMVisible;
                        }

                    }

                    if (m_Role == "Senior Manager")
                    {
                        //if (ProgramIntakeStatus == "Programme passed deadline" || ProgramIntakeStatus == "BDM Reviewed")
                        if (ProgramIntakeStatus == "BDM Reviewed")
                        {
                            var btnSeniorManagerproceedtoCPMOVisible = true;
                            foreach (ApplicationList row in applicationLists)
                            {
                                if (!(row.Status == "Deleted" || row.Status == "Disqualified" || row.Status == "Sr. Mgr. Reviewed" || row.Status == "BDM Reviewed" || row.Status == "CPMO Reviewed"))
                                {
                                    btnSeniorManagerproceedtoCPMOVisible = false;
                                }
                            }
                            btnSeniorManagerproceedtoCPMO.Visible = btnSeniorManagerproceedtoCPMOVisible;
                        }

                    }

                    if (m_Role == "CPMO")
                    {
                        //if (ProgramIntakeStatus == "Programme passed deadline" || ProgramIntakeStatus == "BDM Reviewed" || ProgramIntakeStatus == "Sr. Mgr. Reviewed")
                        if (ProgramIntakeStatus == "Sr. Mgr. Reviewed")
                        {
                            var btnCPMOproceedtoFinalBDMVisible = true;
                            foreach (ApplicationList row in applicationLists)
                            {
                                if (!(row.Status == "Deleted" || row.Status == "Disqualified" || row.Status == "Sr. Mgr. Reviewed" || row.Status == "CPMO Reviewed"))
                                {
                                    btnCPMOproceedtoFinalBDMVisible = false;
                                }
                            }
                            btnCPMOproceedtoFinalBDM.Visible = btnCPMOproceedtoFinalBDMVisible;
                        }

                    }


                    if ((m_Role == "CCMF BDM" && checkCPIPOrCCMF() == "CCMF") || (m_Role == "CPIP BDM" && checkCPIPOrCCMF() == "CPIP"))
                    {
                        //if (ProgramIntakeStatus == "Programme passed deadline" || ProgramIntakeStatus == "BDM Reviewed" || ProgramIntakeStatus == "Sr. Mgr. Reviewed" || ProgramIntakeStatus == "CPMO Reviewed")
                        if (ProgramIntakeStatus == "Sr. Mgr. Reviewed" || ProgramIntakeStatus == "CPMO Reviewed")
                        {
                            var btnCompleteScreeningVisible = true;
                            foreach (ApplicationList row in applicationLists)
                            {
                                if (!(row.Status == "Deleted" || row.Status == "Disqualified" || row.Status == "Withdraw" || row.Status == "CPMO Reviewed"))
                                {
                                    btnCompleteScreeningVisible = false;
                                }
                            }
                            btnCompleteScreening.Visible = btnCompleteScreeningVisible;
                        }

                    }


                    foreach (GridViewRow row in GridViewApplication.Rows)
                    {
                        if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Complete Screening" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified"))
                        {
                            btnSaveShortlistedVisiable = true;
                        }
                        if (row.Cells[GridViewColumnOrder["Status"]].Text == "Complete Screening" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Withdraw"
                            || row.Cells[GridViewColumnOrder["Status"]].Text == "Submitted" || row.Cells[GridViewColumnOrder["Status"]].Text == "BDM Rejected"
                            || row.Cells[GridViewColumnOrder["Status"]].Text == "To be disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Waiting for response from applicant"
                            || row.Cells[GridViewColumnOrder["Status"]].Text == "Withdraw")
                        {
                            System.Web.UI.WebControls.CheckBox ShortlistedCheckBox = (row.Cells[GridViewColumnOrder["Shortlisted"]].FindControl("CheckBoxShortlisted") as System.Web.UI.WebControls.CheckBox);
                            ShortlistedCheckBox.Enabled = false;
                        }
                    }
                    if (((m_Role == "CCMF BDM" && checkCPIPOrCCMF() == "CCMF") || (m_Role == "CPIP BDM" && checkCPIPOrCCMF() == "CPIP") || m_Role == "Senior Manager" || m_Role == "CPMO") && btnSaveShortlistedVisiable && ProgramIntakeStatus != "Complete Screening")//&& ProgramIntakeDeadline > DateTime.Now)
                    {
                        //by andy
                        //if (ProgramIntakeStatus == "")
                        //{
                        //    btnSaveShortlisted.Visible = false;
                        //
                        //}
                        //else
                        //{
                        btnSaveShortlisted.Visible = true;
                        //}
                    }
                    else
                    {
                        btnSaveShortlisted.Visible = false;
                        foreach (GridViewRow row in GridViewApplication.Rows)
                        {
                            System.Web.UI.WebControls.CheckBox ShortlistedCheckBox = (row.Cells[GridViewColumnOrder["Shortlisted"]].FindControl("CheckBoxShortlisted") as System.Web.UI.WebControls.CheckBox);
                            ShortlistedCheckBox.Enabled = false;
                        }


                    }
                }

            }
        }

        protected String checkCPIPOrCCMF()
        {
            var result = "";
            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
            {
                //CPIP
                result = "CPIP";
            }
            else
            {
                result = "CCMF";
            }

            return result;
        }

        protected void CheckButtonVisiableBack()
        {
            Boolean BDMFinalReviewStatus = false;

            btnCoordinatorproceedtoBDM.Visible = false;
            btnBDMproceedtoSeniorManager.Visible = false;
            btnCompleteScreening.Visible = false;
            btnSeniorManagerproceedtoCPMO.Visible = false;
            btnCPMOproceedtoFinalBDM.Visible = false;

            //btnCoordinatorproceedtoBDM.Enabled = false;
            //btnBDMproceedtoSeniorManager.Enabled = false;
            //btnCompleteScreening.Enabled = false;
            //btnSeniorManagerproceedtoCPMO.Enabled = false;
            //btnCPMOproceedtoFinalBDM.Enabled = false;

            btnSaveShortlisted.Visible = false;
            btnDownload.Visible = false;
            btnCSV.Visible = false;
            BtnDuplicated_Submission.Visible = false;
            //DateTime ProgramIntakeDeadline = SearchProgramIntakeListDeadline();
            //lbltest.Visible = true;
            //lbltest.Text =  "++++" + ProgramIntakeDeadline.ToString();
            //lbltest.Text = DateTime.Now.ToString();

            if (lstCyberportProgramme.SelectedValue != m_selectfirstProgrammeNameText && lstIntakeNumber.SelectedValue != m_selectfirstIntakeNumberText)
            {
                var ProgramIntakeStatus = SearchProgramIntakeListStatus();
                btnDownload.Visible = true;
                btnCSV.Visible = true;
                BtnDuplicated_Submission.Visible = true;
                if (GridViewApplication.Rows.Count > 0)
                {
                    //var btnCoordinatorproceedtoBDMEnable = true;
                    //var btnBDMproceedtoSeniorManagerEnable = true;
                    //var btnCompleteScreeningEnable = false;
                    //var btnSeniorManagerproceedtoCPMOEnable = true;
                    //var btnCPMOproceedtoFinalBDMEnable = true;

                    //var btnCompleteScreeningVisiable = true;

                    var btnSaveShortlistedVisiable = false;

                    if ((m_Role == "CCMF Coordinator" || m_Role == "CPIP Coordinator"))
                    {
                        btnCoordinatorproceedtoBDM.Visible = false;
                    }


                    if (m_Role == "CCMF BDM" || m_Role == "CPIP BDM")
                    {
                        if (ProgramIntakeStatus == "Programme passed deadline")
                        {
                            var btnCoordinatorproceedtoBDMVisible = true;
                            foreach (GridViewRow row in GridViewApplication.Rows)
                            {
                                if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "BDM Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "Sr. Mgr. Reviewed"))
                                {
                                    btnCoordinatorproceedtoBDMVisible = false;
                                }
                            }
                            btnBDMproceedtoSeniorManager.Visible = btnCoordinatorproceedtoBDMVisible;
                        }

                    }

                    if (m_Role == "Senior Manager")
                    {
                        if (ProgramIntakeStatus == "Programme passed deadline" || ProgramIntakeStatus == "BDM Reviewed")
                        {
                            var btnSeniorManagerproceedtoCPMOVisible = true;
                            foreach (GridViewRow row in GridViewApplication.Rows)
                            {
                                if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Sr. Mgr. Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "BDM Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed"))
                                {
                                    btnSeniorManagerproceedtoCPMOVisible = false;
                                }
                            }
                            btnSeniorManagerproceedtoCPMO.Visible = btnSeniorManagerproceedtoCPMOVisible;
                        }

                    }

                    if (m_Role == "CPMO")
                    {
                        if (ProgramIntakeStatus == "Programme passed deadline" || ProgramIntakeStatus == "BDM Reviewed" || ProgramIntakeStatus == "Sr. Mgr. Reviewed")
                        {
                            var btnCPMOproceedtoFinalBDMVisible = true;
                            foreach (GridViewRow row in GridViewApplication.Rows)
                            {
                                if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Sr. Mgr. Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed"))
                                {
                                    btnCPMOproceedtoFinalBDMVisible = false;
                                }
                            }
                            btnCPMOproceedtoFinalBDM.Visible = btnCPMOproceedtoFinalBDMVisible;
                        }

                    }

                    if (m_Role == "CCMF BDM" || m_Role == "CPIP BDM")
                    {
                        if (ProgramIntakeStatus == "Programme passed deadline" || ProgramIntakeStatus == "BDM Reviewed" || ProgramIntakeStatus == "Sr. Mgr. Reviewed" || ProgramIntakeStatus == "CPMO Reviewed")
                        {
                            var btnCompleteScreeningVisible = true;
                            foreach (GridViewRow row in GridViewApplication.Rows)
                            {
                                if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Withdraw" || row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed"))
                                {
                                    btnCompleteScreeningVisible = false;
                                }
                            }
                            btnCompleteScreening.Visible = btnCompleteScreeningVisible;
                        }

                    }


                    foreach (GridViewRow row in GridViewApplication.Rows)
                    {
                        if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Complete Screening" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified"))
                        {
                            btnSaveShortlistedVisiable = true;
                        }
                        if (row.Cells[GridViewColumnOrder["Status"]].Text == "Complete Screening" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Withdraw"
                            || row.Cells[GridViewColumnOrder["Status"]].Text == "Submitted" || row.Cells[GridViewColumnOrder["Status"]].Text == "BDM Rejected"
                            || row.Cells[GridViewColumnOrder["Status"]].Text == "To be disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Waiting for response from applicant")
                        {
                            System.Web.UI.WebControls.CheckBox ShortlistedCheckBox = (row.Cells[GridViewColumnOrder["Shortlisted"]].FindControl("CheckBoxShortlisted") as System.Web.UI.WebControls.CheckBox);
                            ShortlistedCheckBox.Enabled = false;
                        }
                    }
                    if ((m_Role.Contains("BDM") || m_Role == "Senior Manager" || m_Role == "CPMO") && btnSaveShortlistedVisiable && ProgramIntakeStatus != "Complete Screening")//&& ProgramIntakeDeadline > DateTime.Now)
                    {
                        //by andy
                        //if (ProgramIntakeStatus == "")
                        //{
                        //    btnSaveShortlisted.Visible = false;
                        //
                        //}
                        //else
                        //{
                        btnSaveShortlisted.Visible = true;
                        //}
                    }
                    else
                    {
                        btnSaveShortlisted.Visible = false;
                        foreach (GridViewRow row in GridViewApplication.Rows)
                        {
                            System.Web.UI.WebControls.CheckBox ShortlistedCheckBox = (row.Cells[GridViewColumnOrder["Shortlisted"]].FindControl("CheckBoxShortlisted") as System.Web.UI.WebControls.CheckBox);
                            ShortlistedCheckBox.Enabled = false;
                        }


                    }
                }


                #region
                //foreach (GridViewRow row in GridViewApplication.Rows)
                //{
                //    if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Coordinator Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "To be disqualified"))
                //    {
                //        btnCoordinatorproceedtoBDMEnable = false;
                //    }

                //    if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "BDM Reviewed" ))
                //    {
                //        btnBDMproceedtoSeniorManagerEnable = false;
                //    }

                //    if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Sr. Mgr. Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "BDM Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed"))
                //    {
                //        btnSeniorManagerproceedtoCPMOEnable = false;
                //    }

                //    if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified" || row.Cells[GridViewColumnOrder["Status"]].Text == "Sr. Mgr. Reviewed"))
                //    {
                //        btnCPMOproceedtoFinalBDMEnable = false;
                //    }

                //    if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "CPMO Reviewed" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified"))
                //    {
                //        btnCompleteScreeningVisiable = false;
                //    }

                //    if (row.Cells[GridViewColumnOrder["RemarksForVetting"]].Text != "")
                //    {
                //        btnCompleteScreeningEnable = true;
                //    }

                //    if (!(row.Cells[GridViewColumnOrder["Status"]].Text == "Complete Screening" || row.Cells[GridViewColumnOrder["Status"]].Text == "Disqualified"))
                //    {
                //        btnSaveShortlistedVisiable = true;
                //    }
                //}

                //if ((m_Role == "CCMF BDM" || m_Role == "CPIP BDM") && btnBDMproceedtoSeniorManagerEnable)
                //{
                //    btnBDMproceedtoSeniorManager.Visible = true;
                //}

                //if (m_Role == "Senior Manager" && btnSeniorManagerproceedtoCPMOEnable)
                //{
                //    btnSeniorManagerproceedtoCPMO.Visible = true;
                //}

                //if ((m_Role == "CCMF BDM" || m_Role == "CPIP BDM") && btnCompleteScreeningVisiable)
                //{
                //    btnCompleteScreening.Visible = true;
                //}

                //if (m_Role == "CPMO")
                //{
                //    btnCPMOproceedtoFinalBDM.Visible = true;
                //}

                //btnCoordinatorproceedtoBDM.Enabled = btnCoordinatorproceedtoBDMEnable;
                //btnBDMproceedtoSeniorManager.Enabled = btnBDMproceedtoSeniorManagerEnable;
                //btnCompleteScreening.Enabled = btnCompleteScreeningEnable;
                //btnSeniorManagerproceedtoCPMO.Enabled = btnSeniorManagerproceedtoCPMOEnable;
                //btnCPMOproceedtoFinalBDM.Enabled = btnCPMOproceedtoFinalBDMEnable;
                #endregion


            }
        }
        protected void btnCoordinatorproceedtoBDM_Click(object sender, EventArgs e)
        {
            updateStatus("Notified BDM");
            UpdateShortlisted();
            updateStatusCoordinator();
            UpdateProgramIntakeStatus("Eligibility checked");
            CheckButtonVisiable(GridViewApplicationBindData());

            btnCoordinatorproceedtoBDM.Enabled = false;
        }

        protected void btnBDMproceedtoSeniorManager_Click(object sender, EventArgs e)
        {
            updateStatus("Notified Sr Mgr");
            UpdateShortlisted();
            UpdateProgramIntakeStatus("BDM Reviewed");

            passScore();
            CheckButtonVisiable(GridViewApplicationBindData());

            btnBDMproceedtoSeniorManager.Enabled = false;
        }

        protected void btnSeniorManagerproceedtoCPMO_Click(object sender, EventArgs e)
        {
            updateStatus("Notified CPMO");
            //UpdateSeniorAPPListStatus();

            UpdateIntakeApplicationStatus("BDM Reviewed", "Sr. Mgr. Reviewed");

            passScore();
            UpdateProgramIntakeStatus("Sr. Mgr. Reviewed");
            UpdateShortlisted();

            CheckButtonVisiable(GridViewApplicationBindData());

            btnSeniorManagerproceedtoCPMO.Enabled = false;
        }

        protected void btnCPMOproceedtoFinalBDM_Click(object sender, EventArgs e)
        {
            updateStatus("Notified Final BDM");
            //updateStatusFinal();
            //UpdateCPMOAPPListStatus();
            UpdateIntakeApplicationStatus("Sr. Mgr. Reviewed", "CPMO Reviewed");
            UpdateProgramIntakeStatus("CPMO Reviewed");
            UpdateShortlisted();
            PassRemaksToApplicationShortlisting();

            CheckButtonVisiable(GridViewApplicationBindData());

            btnCPMOproceedtoFinalBDM.Enabled = false;
        }

        protected void btnCompleteScreening_Click(object sender, EventArgs e)
        {
            updateStatus("Notified Vetting Coordinator");
            //updateStatusCompleteScreen();
            UpdateIntakeApplicationStatus("CPMO Reviewed", "Complete Screening");
            UpdateProgramIntakeStatus("Complete Screening");
            UpdateShortlisted();

            CheckButtonVisiable(GridViewApplicationBindData());

            btnCompleteScreening.Enabled = false;
        }

        protected void updateStatusCoordinator()
        {
            string v_status = "Coordinator Reviewed";
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            var program_Id = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
            oQuery.Query = "<Where><And><Eq><FieldRef Name='Programme_ID'  /><Value Type='Text'>" + program_Id + "</Value></Eq><Eq><FieldRef Name='Status'  /><Value Type='Choice'>" + v_status + "</Value></Eq></And></Where>";

            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                oListItem["Status"] = "Eligibility checked";
                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }

            SqlConnection conn = new SqlConnection(connStr);
            string sql = "";

            sql = "UPDATE TB_CCMF_APPLICATION SET Status = '" + "Eligibility checked" + "' WHERE Programme_ID = @program_Id and Status=@v_status";
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@program_Id", program_Id);
                cmd.Parameters.Add("@v_status", v_status);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = '" + "Eligibility checked" + "' WHERE Programme_ID = @program_Id and Status=@v_status";
            conn.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.Parameters.Add("@program_Id", program_Id);
                cmd1.Parameters.Add("@v_status", v_status);
                cmd1.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }


        }

        protected void updateStatus(String status)
        {
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Programme Intake"];
            SPQuery oQuery = new SPQuery();
            var program_Id = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + program_Id + "</Value></Eq> </Where>";

            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                oListItem["Status"] = status;

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }
        }

        //protected void updateStatusFinal()
        //{
        //    string v_status = "CPMO Reviewed";
        //    SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
        //    SPList oList = oWebsiteRoot.Lists["Application List"];
        //    SPQuery oQuery = new SPQuery();
        //    var program_Id = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
        //    oQuery.Query = "<Where><And><Eq><FieldRef Name='Programme_ID'  /><Value Type='Text'>" + program_Id + "</Value></Eq><Eq><FieldRef Name='Status'  /><Value Type='Choice'>" + v_status + "</Value></Eq></And></Where>";

        //    SPListItemCollection collListItems = oList.GetItems(oQuery);

        //    foreach (SPListItem oListItem in collListItems)
        //    {
        //        //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
        //        oListItem["Status"] = "BDM Final Review";
        //        oListItem.Web.AllowUnsafeUpdates = true;
        //        oListItem.Update();
        //        oListItem.Web.AllowUnsafeUpdates = false;
        //    }

        //    SqlConnection conn = new SqlConnection(connStr);
        //    string sql = "";

        //    sql = "UPDATE TB_CCMF_APPLICATION SET Status = '" + "BDM Final Review" + "' WHERE Programme_ID = @program_Id and Status=@v_status";

        //    conn.Open();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.Add("@program_Id", program_Id);
        //        cmd.Parameters.Add("@v_status", v_status);
        //        cmd.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = '" + "BDM Final Review" + "' WHERE Programme_ID = @program_Id and Status=@v_status";
        //    conn.Open();
        //    try
        //    {
        //        SqlCommand cmd1 = new SqlCommand(sql, conn);
        //        cmd1.Parameters.Add("@program_Id", program_Id);
        //        cmd1.Parameters.Add("@v_status", v_status);

        //        cmd1.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }


        //}

        //protected void updateStatusCompleteScreen()
        //{
        //    //var applicationLists = GetAllOfApplicationBindData();
        //    var applicationLists = getApplicationList();
        //    foreach (var row in applicationLists)
        //    {
        //        if (row.RemarksForVetting != "")
        //        {
        //            var applicationNumber = row.ApplicationNo;
        //            string v_status = "CPMO Reviewed";
        //            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
        //            SPList oList = oWebsiteRoot.Lists["Application List"];
        //            SPQuery oQuery = new SPQuery();
        //            //oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
        //            //oQuery.Query = "<Where><And><Eq><FieldRef Name='Programme_Name'  /><Value Type='Choice'>"+lstCyberportProgramme.SelectedValue+"</Value></Eq><Eq><FieldRef Name='Intake_Number'  /><Value Type='Text'>"+lstIntakeNumber.SelectedValue+"</Value></Eq></And></Where>";
        //            //oQuery.Query = "<Where><And><Eq><FieldRef Name='Programme_ID'  /><Value Type='Choice'>" + program_Id + "</Value></Eq></And></Where>";
        //            //oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + program_Id + "</Value></Eq> </Where>";
        //            oQuery.Query = "<Where><And><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + applicationNumber + "</Value></Eq><Eq><FieldRef Name='Status'  /><Value Type='Choice'>" + v_status + "</Value></Eq></And></Where>";

        //            SPListItemCollection collListItems = oList.GetItems(oQuery);

        //            foreach (SPListItem oListItem in collListItems)
        //            {
        //                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
        //                oListItem["Status"] = "Complete Screening";
        //                oListItem.Web.AllowUnsafeUpdates = true;
        //                oListItem.Update();
        //                oListItem.Web.AllowUnsafeUpdates = false;
        //            }



        //            SqlConnection conn = new SqlConnection(connStr);
        //            string sql = "";
        //            if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //            {
        //                //CPIP
        //                sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = '" + "Complete Screening" + "' WHERE Application_Number = @applicationNumber and Status=@v_status";
        //            }
        //            else
        //            {
        //                //CCMF
        //                sql = "UPDATE TB_CCMF_APPLICATION SET Status = '" + "Complete Screening" + "' WHERE Application_Number = @applicationNumber and Status=@v_status ";
        //            }

        //            conn.Open();
        //            try
        //            {
        //                SqlCommand cmd1 = new SqlCommand(sql, conn);
        //                cmd1.Parameters.Add("@applicationNumber", applicationNumber);
        //                cmd1.Parameters.Add("@v_status", v_status);

        //                cmd1.ExecuteNonQuery();
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        }
        //    }
        //}


        protected int AddScoreChecking(string v_Application_Number, string v_Programme_ID, string v_Role, string Scoretype)
        {
            int m_result = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                if (Scoretype.Trim() == "CCMF")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Application_Number') FROM [TB_SCREENING_CCMF_SCORE] where Application_Number=@v_Application_Number and Programme_ID=@v_Programme_ID and Role=@v_Role", conn))
                    {
                        cmd.Parameters.Add("@v_Application_Number", v_Application_Number);
                        cmd.Parameters.Add("@v_Programme_ID", v_Programme_ID);
                        cmd.Parameters.Add("@v_Role", v_Role);

                        conn.Open();
                        try
                        {
                            m_result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Application_Number') FROM [TB_SCREENING_INCUBATION_SCORE] where Application_Number=@v_Application_Number and Programme_ID=@v_Programme_ID and Role=@v_Role", conn))
                    {
                        cmd.Parameters.Add("@v_Application_Number", v_Application_Number);
                        cmd.Parameters.Add("@v_Programme_ID", v_Programme_ID);
                        cmd.Parameters.Add("@v_Role", v_Role);

                        conn.Open();
                        try
                        {
                            m_result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }

            return m_result;

        }
        /**** passScore ***/

        protected void passSoreCPIP(string v_Programme_ID, string v_Role)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlConnection connsub = new SqlConnection(connStr);
            string m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();
            string tempsql_insert = "";
            string tempsql = "SELECT * FROM [TB_SCREENING_INCUBATION_SCORE] WHERE Programme_ID = @v_Programme_ID AND Role=@v_Role";

            // target pass score to which role
            string caseRole = "";

            switch (v_Role)
            {
                case "CCMF BDM":
                    caseRole = "Senior Manager";
                    break;
                case "CPIP BDM":
                    caseRole = "Senior Manager";
                    break;
                case "Senior Manager":
                    caseRole = "CPMO";
                    break;
                default:

                    break;
            }

            //lbltest.Text += tempsql;

            using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
            {
                cmdscore.Parameters.Add("@v_Programme_ID", v_Programme_ID);
                cmdscore.Parameters.Add("@v_Role", v_Role);

                conn.Open();
                try
                {
                    var reader = cmdscore.ExecuteReader();
                    while (reader.Read())
                    {
                        //lbltest.Text += reader.GetValue(1).ToString();
                        if (AddScoreChecking(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(), reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString(), caseRole, "CPIP") == 0)
                        {

                            connsub.Open();
                            try
                            {
                                tempsql_insert = "insert into TB_SCREENING_INCUBATION_SCORE(Application_Number,Programme_ID,Reviewer,Role,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                                + "@Application_Number , "
                                + "@Programme_ID , "
                                + "'passScore' , "
                                + "@caseRole , "
                                + "@Management_Team , "
                                + "@Creativity , "
                                + "@Business_Viability , "
                                + "@Benefit_To_Industry , "
                                + "@Proposal_Milestones , "
                                + "@Total_Score , "
                                + "@Comments , "
                                + "'' , "
                                + "@m_systemuser , "
                                + "GETDATE() , "
                                + "@m_systemuser , "
                                + "GETDATE()"
                                + ")";

                                SqlCommand cmdinsert = new SqlCommand(tempsql_insert, conn);
                                cmdinsert.Parameters.Add("@Application_Number", reader.GetValue(reader.GetOrdinal("Application_Number")).ToString());
                                cmdinsert.Parameters.Add("@Programme_ID", reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString());
                                cmdinsert.Parameters.Add("@caseRole", caseRole);
                                cmdinsert.Parameters.Add("@Management_Team", reader.GetValue(reader.GetOrdinal("Management_Team")).ToString());
                                cmdinsert.Parameters.Add("@Creativity", reader.GetValue(reader.GetOrdinal("Creativity")).ToString());
                                cmdinsert.Parameters.Add("@Business_Viability", reader.GetValue(reader.GetOrdinal("Business_Viability")).ToString());
                                cmdinsert.Parameters.Add("@Benefit_To_Industry", reader.GetValue(reader.GetOrdinal("Benefit_To_Industry")).ToString());
                                cmdinsert.Parameters.Add("@Proposal_Milestones", reader.GetValue(reader.GetOrdinal("Proposal_Milestones")).ToString());
                                //cmdinsert.Parameters.Add("@Total_Score", reader.GetValue(reader.GetOrdinal("Total_Score")).ToString());
                                SqlParameter Total_Score = new SqlParameter("@Total_Score", SqlDbType.Decimal);
                                Total_Score.Precision = 3;
                                Total_Score.Scale = 2;
                                Total_Score.Value = reader.GetValue(reader.GetOrdinal("Total_Score")).ToString();
                                cmdinsert.Parameters.Add(Total_Score);

                                cmdinsert.Parameters.Add("@Comments", reader.GetValue(reader.GetOrdinal("Comments")).ToString());
                                cmdinsert.Parameters.Add("@m_systemuser", m_systemuser);

                                cmdinsert.ExecuteNonQuery();
                            }
                            finally
                            {
                                connsub.Close();
                            }
                        }

                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        protected void passSoreCCMF(string v_Programme_ID, string v_Role)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlConnection connsub = new SqlConnection(connStr);
            string m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString();
            string tempsql_insert = "";


            //string tempsql = "SELECT * FROM [TB_SCREENING_CCMF_SCORE] WHERE Programme_ID = '" + v_Programme_ID + "' AND Role='" + v_Role + "'";
            string tempsql = "SELECT CCMF_Scoring_ID ,Application_Number,Programme_ID,Reviewer,Role,Management_Team,Business_Model,Creativity,Social_Responsibility,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date FROM [TB_SCREENING_CCMF_SCORE] WHERE Programme_ID = @v_Programme_ID AND Role=@v_Role";

            // target pass score to which role
            string caseRole = "";

            switch (v_Role)
            {
                case "CCMF BDM":
                    caseRole = "Senior Manager";
                    break;
                case "CPIP BDM":
                    caseRole = "Senior Manager";
                    break;
                case "Senior Manager":
                    caseRole = "CPMO";
                    break;
                default:

                    break;
            }

            using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
            {
                cmdscore.Parameters.Add("@v_Programme_ID", v_Programme_ID);
                cmdscore.Parameters.Add("@v_Role", v_Role);

                conn.Open();
                try
                {
                    var reader = cmdscore.ExecuteReader();
                    while (reader.Read())
                    {
                        //lbltest.Text += reader.GetValue(1).ToString();
                        //reader.GetOrdinal("Application_Number").ToString();
                        if (AddScoreChecking(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(), reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString(), caseRole, "CCMF") == 0)
                        {

                            connsub.Open();
                            try
                            {
                                //tempsql_insert = "insert into TB_SCREENING_CCMF_SCORE(Application_Number,Programme_ID,Reviewer,Role,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                                tempsql_insert = "insert into TB_SCREENING_CCMF_SCORE(Application_Number,Programme_ID,Reviewer,Role,Management_Team,Business_Model,Creativity,Social_Responsibility,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date) VALUES ("
                                + "@Application_Number , "
                                + "@Programme_ID , "
                                + "'passScore' , "
                                + "@caseRole , "
                                + "@Management_Team , "
                                + "@Business_Model , "
                                + "@Creativity , "
                                + "@Social_Responsibility , "
                                + "@Total_Score , "
                                + "@Comments , "
                                + "'' , "
                                + "@m_systemuser , "
                                + "GETDATE() , "
                                + "@m_systemuser , "
                                + "GETDATE()"
                                + ")";

                                SqlCommand cmdinsert = new SqlCommand(tempsql_insert, conn);
                                cmdinsert.Parameters.Add("@Application_Number", reader.GetValue(reader.GetOrdinal("Application_Number")).ToString());
                                cmdinsert.Parameters.Add("@Programme_ID", reader.GetValue(reader.GetOrdinal("Programme_ID")).ToString());
                                cmdinsert.Parameters.Add("@caseRole", caseRole);
                                cmdinsert.Parameters.Add("@Management_Team", reader.GetValue(reader.GetOrdinal("Management_Team")).ToString());
                                cmdinsert.Parameters.Add("@Business_Model", reader.GetValue(reader.GetOrdinal("Business_Model")).ToString());
                                cmdinsert.Parameters.Add("@Creativity", reader.GetValue(reader.GetOrdinal("Creativity")).ToString());
                                cmdinsert.Parameters.Add("@Social_Responsibility", reader.GetValue(reader.GetOrdinal("Social_Responsibility")).ToString());
                                cmdinsert.Parameters.Add("@Total_Score", reader.GetValue(reader.GetOrdinal("Total_Score")).ToString());
                                cmdinsert.Parameters.Add("@Comments", reader.GetValue(reader.GetOrdinal("Comments")).ToString());
                                cmdinsert.Parameters.Add("@m_systemuser", m_systemuser);

                                cmdinsert.ExecuteNonQuery();
                            }
                            finally
                            {
                                connsub.Close();
                            }
                        }
                        //lbltest.Text += tempsql_insert;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Get Sharepoint Role Name e.g Senior Manager
        protected string get_role()
        {
            string m_Role = "";

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

            //if (HttpContext.Current.Request.QueryString["testrole"] != null)
            //{
            //    m_Role = HttpContext.Current.Request.QueryString["testrole"];
            //    //lblrole.Text = m_Role;
            //}

            return m_Role;

        }

        //Get Programme id 
        protected string get_proid(string Programme_Name, string Intake_Number)
        {
            string m_prog_id = "";
            string sql_string = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql_string = "SELECT * FROM TB_PROGRAMME_INTAKE where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number and Active = 1";
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
            //return sql_string;
            return m_prog_id;
        }

        protected void passScore()
        {
            string v_Programme_Name = lstCyberportProgramme.SelectedValue.ToString();
            string v_Intake_Number = lstIntakeNumber.SelectedValue.ToString();
            if (v_Programme_Name == "Cyberport Incubation Programme")
            {
                passSoreCPIP(get_proid(v_Programme_Name, v_Intake_Number), get_role());
                //lbltest.Text = "CPIP|" + v_Programme_Name + "|" + v_Intake_Number + "|" + get_role();
            }
            else
            {
                passSoreCCMF(get_proid(v_Programme_Name, v_Intake_Number), get_role());
                //lbltest.Text = "CCMF|" + v_Programme_Name + "|" + v_Intake_Number + "|" + get_role();
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            passScore();
        }

        protected void getSYSTEMPARAMETER()
        {
            ConnectOpen();
            try
            {
                var sqlString = "select Config_Code,Value from TB_SYSTEM_PARAMETER;";

                var command = new SqlCommand(sqlString, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == "AttachmentPrimaryFolder")
                    {
                        //Shared Documents
                        m_AttachmentPrimaryFolderName = reader.GetString(1);
                    }

                    if (reader.GetString(0) == "AttachmentSecondaryFolder")
                    {
                        //ApplicationAttachments
                        m_AttachmentSecondaryFolderName = reader.GetString(1);
                    }

                    if (reader.GetString(0) == "zipfiledownloadurl")
                    {
                        /*hhttp://cyberportemssp:10870/*/
                        m_zipfiledownloadurl = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "ApplicationIsInDebug")
                    {
                        //1,0
                        m_ApplicationIsInDebug = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "ApplicationDebugEmailSentTo")
                    {
                        m_ApplicationDebugEmailSentTo = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "zipfiledownloadpath")
                    {
                        m_path = reader.GetString(1);

                    }

                    if (reader.GetString(0) == "IsLogEvent")
                    {
                        IsLogEvent = reader.GetString(1);

                    }
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {

                ConnectClose();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string m_username = SPContext.Current.Web.CurrentUser.Name.ToString();
            string m_mail = "";
            string m_programId = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
            if (m_ApplicationIsInDebug == "1")
            {
                m_mail = m_ApplicationDebugEmailSentTo;
            }
            else
            {
                m_mail = SPContext.Current.Web.CurrentUser.Email;
            }

            m_programName = lstCyberportProgramme.SelectedValue;
            m_intake = lstIntakeNumber.SelectedValue;

            string sql = @"INSERT INTO [TB_DOWNLOAD_REQUEST] (Programme_Name, Programme_ID, Intake_Number, Request_Type, Status, User_Email, Created_By, Created_Date) 
                           VALUES (@Programme_Name, @Programme_ID, @Intake_Number, 'Download Attachments', 0, @User_Email, @User, GETDATE())";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Programme_Name", m_programName);
                    cmd.Parameters.AddWithValue("@Intake_Number", m_intake);
                    cmd.Parameters.AddWithValue("@Programme_ID", m_programId);
                    cmd.Parameters.AddWithValue("@User_Email", m_mail);
                    cmd.Parameters.AddWithValue("@User", m_username);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            string m_subject = "Zip File : " + m_programName + " / " + m_intake + " is processing.";
            string m_body = getEmailTemplate("ZipDownloadStartEmail");
            sharepointsendemail(m_mail, m_subject, m_body);

            lbldownloadmessage.Text = "Download Complete.";
        }



        protected void sharepointsendemail(string toAddress, string subject, string body)
        {
            SPSecurity.RunWithElevatedPrivileges(
                             delegate()
                             {
                                 using (SPSite site = new SPSite(
                                   SPContext.Current.Site.ID,
                                   SPContext.Current.Site.Zone))
                                 {
                                     using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                                     {
                                         SPUtility.SendEmail(web, false, false,
                                                                   toAddress,
                                                                   subject,
                                                                   body);
                                     }
                                 }
                             });
        }
        protected String getEmailTemplate(string emailTemplate)
        {
            ConnectOpen();
            String emailTemplateContent = "";
            try
            {

                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template=@emailTemplate;";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@emailTemplate", emailTemplate);

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

        protected string genRandom(int numerofpassword)
        {
            string stringlist = "01234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*";
            string _num = "";
            int rnd_titleid = 0;

            Random rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 1; i <= numerofpassword; i++)
            {
                rnd_titleid = rnd.Next(0, stringlist.Length);
                _num += stringlist.Substring(rnd_titleid, 1);
            }

            return _num;
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (m_ApplicationIsInDebug == "1")
            {
                if (m_ApplicationDebugEmailSentTo == "" || m_ApplicationDebugEmailSentTo == null)
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDownload.Enabled = false;
                    args.IsValid = true;
                }
            }
            else
            {
                string m_mail = SPContext.Current.Web.CurrentUser.Email;
                if (m_mail == "" || m_mail == null)
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDownload.Enabled = false;
                    args.IsValid = true;
                }
            }

        }

        protected void UpdateShortlisted()
        {
            ConnectOpen();
            try
            {
                SetGridViewColumnOrder();
                foreach (GridViewRow row in GridViewApplication.Rows)
                {
                    System.Web.UI.WebControls.CheckBox ShortlistedCheckBox = (row.Cells[GridViewColumnOrder["Shortlisted"]].FindControl("CheckBoxShortlisted") as System.Web.UI.WebControls.CheckBox);

                    String Shortlisted = "";
                    if (ShortlistedCheckBox.Checked)
                    {
                        Shortlisted = "1";
                    }
                    else
                    {
                        Shortlisted = "0";
                    }

                    var applicationNumber = ((HyperLink)row.Cells[GridViewColumnOrder["ApplicationNo"]].Controls[0]).Text;
                    var remarksForVetting = row.Cells[GridViewColumnOrder["RemarksForVetting"]].Text;
                    var programId = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);



                    var sqlString = "select Application_Number,Programme_ID,Remarks_To_Vetting,Shortlisted,Modified_By,Modified_Date from TB_APPLICATION_SHORTLISTING where Application_Number=@applicationNumber and  Programme_ID=@programId; ";

                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add("@applicationNumber", applicationNumber);
                    command.Parameters.Add("@programId", programId);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        //update
                        var sqlUpdate = "update TB_APPLICATION_SHORTLISTING set "
                                        + "Remarks_To_Vetting = @remarksForVetting, "
                                        + "Shortlisted = @Shortlisted, "
                                        + "Modified_By = @User, "
                                        + "Modified_Date = GETDATE() "
                                        + "where Application_Number=@applicationNumber and Programme_ID=@programId ";
                        command = new SqlCommand(sqlUpdate, connection);
                        command.Parameters.Add("@remarksForVetting", remarksForVetting);
                        command.Parameters.Add("@Shortlisted", Shortlisted);
                        command.Parameters.Add("@User", SPContext.Current.Web.CurrentUser.Name.ToString());
                        command.Parameters.Add("@applicationNumber", applicationNumber);
                        command.Parameters.Add("@programId", programId);


                        command.ExecuteNonQuery();

                    }
                    else
                    {
                        //insert
                        var sqlUpdate = "insert into TB_APPLICATION_SHORTLISTING(Application_Number,Programme_ID,Remarks_To_Vetting,Shortlisted,Created_By,Created_Date,Modified_By,Modified_Date) values("
                                        + "@applicationNumber, "
                                        + "@programId, "
                                        + "@remarksForVetting, "
                                        + "@Shortlisted, "
                                        + "@User, "
                                        + "GETDATE(), "
                                        + "@User, "
                                        + "GETDATE() "
                                        + " ) ;";
                        command = new SqlCommand(sqlUpdate, connection);
                        command.Parameters.Add("@applicationNumber", applicationNumber);
                        command.Parameters.Add("@programId", programId);
                        command.Parameters.Add("@remarksForVetting", remarksForVetting);
                        command.Parameters.Add("@Shortlisted", Shortlisted);
                        command.Parameters.Add("@User", SPContext.Current.Web.CurrentUser.Name.ToString());

                        command.ExecuteNonQuery();
                    }

                    reader.Dispose();
                    command.Dispose();

                }
            }
            finally
            {
                ConnectClose();
            }

        }

        protected void UpdateApplicationStatus(String tableName, String ApplicationNumber, String statusReplaceValue, String statusSearchValue)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "update " + tableName + " set "
                                        + "Status = @statusReplaceValue "
                                        + "where Application_Number=@ApplicationNumber and Status=@statusSearchValue";
                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add("@statusReplaceValue", statusReplaceValue);
                command.Parameters.Add("@ApplicationNumber", ApplicationNumber);
                command.Parameters.Add("@statusSearchValue", statusSearchValue);

                command.ExecuteNonQuery();


                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

        }
        protected void UpdateSharePointApplicationStatus(String ApplicationNumber, String progID, String statusReplaceValue, String statusSearchValue)
        {
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><And><And><Eq><FieldRef Name='Title'  /><Value Type='Text'> " + ApplicationNumber + "</Value></Eq><Eq><FieldRef Name='Programme_ID'  /><Value Type='Text'>" + progID + "</Value></Eq></And><Eq><FieldRef Name='Status'  /><Value Type='Choice'>" + statusSearchValue + "</Value></Eq></And></Where>";

            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                oListItem["Status"] = statusReplaceValue;

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }

        }

        protected void UpdateSPApplistStatus(String Oldstatus, String status)
        {
            var prog_id = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><And><Eq><FieldRef Name='Programme_ID'  /><Value Type='Text'>" + prog_id + "</Value></Eq><Eq><FieldRef Name='Status'  /><Value Type='Choice'>" + Oldstatus + "</Value></Eq></And></Where>";

            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {
                //lblrole.Text += Convert.ToString(oListItem["Applicant"]) + " " + Convert.ToString(oListItem["Status"]) + "<br />";
                oListItem["Status"] = status;

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;
            }
        }

        protected void UpdateIntakeApplicationStatus(String currentStatus, String Status)
        {
            //SetGridViewColumnOrder();
            var prog_id = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);

            ConnectOpen();
            try
            {
                var AppTable = "";
                var AppID = "";
                var sqlString = "";
                if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                {
                    //CPIP
                    AppTable = "TB_INCUBATION_APPLICATION";
                    AppID = "Incubation_ID";
                    sqlString = "SELECT Incubation_ID FROM TB_INCUBATION_APPLICATION WHERE Programme_ID =@prog_id and Status =@current_status";
                }
                else
                {
                    //CCMF
                    AppTable = "TB_CCMF_APPLICATION";
                    AppID = "CCMF_ID";
                    sqlString = "SELECT CCMF_ID FROM TB_CCMF_APPLICATION WHERE Programme_ID =@prog_id and Status =@current_status";
                }
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@current_status", currentStatus);
                command.Parameters.Add("@prog_id", prog_id);
                //lbldebug.Visible = true;
                //lbldebug.Text = "prog_id =  " + prog_id + " SSQL " + sqlString;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    String AppId = reader.GetGuid(0).ToString();
                    //lbldebug.Text += " id = : " + AppId;
                    UpdateAppStatus(AppTable, AppId, AppID, Status);
                }
                UpdateSPApplistStatus(currentStatus, Status);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConnectClose();

            }

        }
        protected void UpdateAppStatus(String applicationTable, String appID, string IDName, string status)
        {
            try
            {
                var sqlUpdate = "update " + applicationTable + " set Status = @statusReplaceValue where " + IDName + " = @App_ID";
                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add("@statusReplaceValue", status);
                command.Parameters.Add("@App_ID", appID);
                command.ExecuteNonQuery();
                command.Dispose();
                //lbldebug.Text += "App_ID :  " + appID + " SSQL " + sqlUpdate;
            }
            catch (Exception ex)
            {
                //lbldebug.Text = "UpdateCPIPApplicationStatus error " + ex.Message;
                throw ex;
            }
            finally
            {
            }
        }

        //protected void UpdateSeniorAPPListStatus()
        //{
        //    SetGridViewColumnOrder();
        //    var applicationTable = "";
        //    if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //    {
        //        //CPIP
        //        applicationTable = "TB_INCUBATION_APPLICATION";
        //    }
        //    else
        //    {
        //        //CCMF
        //        applicationTable = "TB_CCMF_APPLICATION";
        //    }

        //    //var applicationLists = GetAllOfApplicationBindData();
        //    var applicationLists = getApplicationList();
        //    foreach (var row in applicationLists)
        //    {
        //        var applicationNumber = row.ApplicationNo;
        //        var programId = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
        //        var status = row.Status;
        //        if (status == "BDM Reviewed")
        //        {
        //            UpdateApplicationStatus(applicationTable, applicationNumber, "Sr. Mgr. Reviewed", status);
        //        }

        //        UpdateSharePointApplicationStatus(applicationNumber, programId, "Sr. Mgr. Reviewed", status);

        //    }
        //}

        //protected void UpdateCPMOAPPListStatus()
        //{
        //    SetGridViewColumnOrder();
        //    var applicationTable = "";
        //    if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
        //    {
        //        //CPIP
        //        applicationTable = "TB_INCUBATION_APPLICATION";
        //    }
        //    else
        //    {
        //        //CCMF
        //        applicationTable = "TB_CCMF_APPLICATION";
        //    }
        //    //var applicationLists = GetAllOfApplicationBindData();
        //    var applicationLists = getApplicationList();
        //    foreach (var row in applicationLists)
        //    {
        //        var applicationNumber = row.ApplicationNo;
        //        var programId = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);
        //        var status = row.Status;
        //        if (status == "Sr. Mgr. Reviewed")
        //        {
        //            UpdateApplicationStatus(applicationTable, applicationNumber, "CPMO Reviewed", status);
        //        }

        //        UpdateSharePointApplicationStatus(applicationNumber, programId, "CPMO Reviewed", status);

        //    }
        //}

        protected void UpdateProgramIntakeStatus(String Status)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "update TB_PROGRAMME_INTAKE set "
                                        + "Status = @Status "
                                        + "where Programme_ID=@Programme_ID";
                //+"where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number";
                var command = new SqlCommand(sqlUpdate, connection);
                //command.Parameters.Add("@Programme_Name", lstCyberportProgramme.SelectedValue);
                //command.Parameters.Add("@Intake_Number", lstIntakeNumber.SelectedValue);
                command.Parameters.Add("@Programme_ID", get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue));
                command.Parameters.Add("@Status", Status);

                command.ExecuteNonQuery();


                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected String SearchProgramIntakeListStatus()
        {
            ConnectOpen();
            var status = "";
            try
            {
                var sqlString = "select isnull(Status,'') as Status  from TB_PROGRAMME_INTAKE where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@Programme_Name", lstCyberportProgramme.SelectedValue);
                command.Parameters.Add("@Intake_Number", lstIntakeNumber.SelectedValue);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    status = reader.GetString(0);
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            return status;
        }

        protected DateTime GetIntakeDeadline()
        {
            //ConnectOpen();
            var status = "";
            DateTime deadline = Convert.ToDateTime("01/01/2999 00:00:00.00");
            try
            {
                //var sqlString = "select Application_Deadline from TB_PROGRAMME_INTAKE where Programme_Name=@Programme_Name and Intake_Number=" + lstIntakeNumber.SelectedValue ; //@Intake_Number ";
                var sqlString = "select Application_Deadline from TB_PROGRAMME_INTAKE where Programme_Name='" + m_ProgrammeName + "' and Intake_Number=" + m_IntakeNum; //@Intake_Number ";

                var command = new SqlCommand(sqlString, connection);
                //command.Parameters.Add("@Programme_Name", lstCyberportProgramme.SelectedValue);
                //command.Parameters.Add("@Intake_Number", lstIntakeNumber.SelectedValue);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    deadline = reader.GetDateTime(reader.GetOrdinal("Application_Deadline"));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                //ConnectClose();
            }

            return deadline;
        }

        protected void InsertTBDownloadZIP(String User_Name, String Email, String type, String Path, String File_Name, String Password, String Status)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "insert into TB_Download_ZIP(User_Name,Email,type,Path,File_Name,Password,Status,Created_By,Created_Date,Modified_By,Modified_Date) values("
                                    + "@User_Name, "
                                    + "@Email, "
                                    + "@type, "
                                    + "@Path, "
                                    + "@File_Name, "
                                    + "@Password, "
                                    + "@Status, "
                                    + "@User, "
                                    + "GETDATE(), "
                                    + "@User, "
                                    + "GETDATE() "
                                    + " ) ;";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add("@User_Name", User_Name);
                command.Parameters.Add("@Email", Email);
                command.Parameters.Add("@type", type);
                command.Parameters.Add("@Path", Path);
                command.Parameters.Add("@File_Name", File_Name);
                command.Parameters.Add("@Password", Password);
                command.Parameters.Add("@Status", Status);
                command.Parameters.Add("@User", SPContext.Current.Web.CurrentUser.Name.ToString());

                command.ExecuteNonQuery();

                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void btnSaveShortlisted_Click(object sender, EventArgs e)
        {
            UpdateShortlisted();
        }

        protected void PassRemaksToApplicationShortlisting()
        {
            SetGridViewColumnOrder();
            ConnectOpen();
            try
            {
                foreach (GridViewRow row in GridViewApplication.Rows)
                {
                    var applicationNumber = ((HyperLink)row.Cells[GridViewColumnOrder["ApplicationNo"]].Controls[0]).Text;
                    var programId = get_proid(lstCyberportProgramme.SelectedValue, lstIntakeNumber.SelectedValue);

                    var sqlString = "";
                    if (lstCyberportProgramme.SelectedValue.ToString().Contains("Cyberport Incubation Program"))
                    {
                        //CPIP
                        sqlString = "SELECT Incubation_Scoring_ID,Application_Number,Programme_ID,Reviewer,Role,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,Total_Score,Comments,Remarks,Created_By,Created_Date,Modified_By,Modified_Date FROM TB_SCREENING_INCUBATION_SCORE "
                        + "where Programme_ID=@m_progid and Application_Number=@m_ApplicationNumber ";
                    }
                    else
                    {
                        //CCMF
                        sqlString = "SELECT CCMF_Scoring_ID ,Application_Number ,Programme_ID ,Reviewer ,Role ,Management_Team ,Business_Model ,Creativity ,Social_Responsibility ,Total_Score ,Comments ,isnull(Remarks,'') as Remarks,Created_By ,Created_Date ,Modified_By ,Modified_Date FROM TB_SCREENING_CCMF_SCORE "
                        + "where Programme_ID=@m_progid and Application_Number=@m_ApplicationNumber ";

                    }
                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add("@m_progid", programId);
                    command.Parameters.Add("@m_ApplicationNumber", applicationNumber);

                    var reader = command.ExecuteReader();
                    var remark = "";
                    while (reader.Read())
                    {
                        if (reader.GetValue(reader.GetOrdinal("Remarks")).ToString() != "")
                        {
                            remark += reader.GetValue(reader.GetOrdinal("Role")).ToString() + " Remark : ";
                            remark += reader.GetValue(reader.GetOrdinal("Remarks")).ToString() + "\r\n";
                        }
                    }
                    reader.Dispose();

                    if (remark != "")
                    {
                        var sqlUpdate = "update TB_APPLICATION_SHORTLISTING set "
                                        + "Remarks_To_Vetting = @remarksForVetting, "
                                        + "Modified_By = @User, "
                                        + "Modified_Date = GETDATE() "
                                        + "where Application_Number=@applicationNumber and  Programme_ID=@programId ";
                        command = new SqlCommand(sqlUpdate, connection);
                        command.Parameters.Add("@remarksForVetting", remark);
                        command.Parameters.Add("@User", SPContext.Current.Web.CurrentUser.Name.ToString());
                        command.Parameters.Add("@applicationNumber", applicationNumber);
                        command.Parameters.Add("@programId", programId);


                        command.ExecuteNonQuery();
                    }


                    command.Dispose();


                }
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void lststream_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectOpen();
            try
            {
                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }

        protected String getGroupUserEmail(String groupName)
        {

            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.LoginName.ToString(); //"Flora Yeung";            
            //lblrole.Text = SPContext.Current.Web.AllRolesForCurrentUser.ToString(); //"CCMF BDM";
            //lblreviewer.Text = SPContext.Current.Web.CurrentUser.Name.ToString();
            //lblrole.Text = "";
            var UserStr = "";

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPGroup groupObject = null;
                        foreach (SPGroup group in web.Groups)
                        {
                            if (group.Name == groupName)
                            {
                                //lbltest.Text += "[" + group.Name + "]";
                                groupObject = group;
                            }
                        }

                        if (groupObject != null)
                        {
                            foreach (SPUser user in groupObject.Users)
                            {
                                UserStr += user.Email + ";";
                            }
                        }

                    }
                }
            });

            return UserStr;

        }

        protected string checkProgrammeType(string m_Programme_Name, string m_Intake_Number)
        {
            var m_INCUBATION = 0;
            var m_CCMF = 0;
            var m_result = "";
            string sql = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql = "SELECT COUNT(O.Incubation_ID) "
                    + "FROM dbo.TB_INCUBATION_APPLICATION AS O INNER JOIN "
                    + "dbo.TB_PROGRAMME_INTAKE AS S ON O.Programme_ID = S.Programme_ID "
                    + "WHERE (S.Programme_Name = @Programme_Name) AND (O.Intake_Number = @Intake_Number)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Programme_Name", m_Programme_Name));
                    cmd.Parameters.Add(new SqlParameter("@Intake_Number", m_Intake_Number));
                    conn.Open();
                    try
                    {
                        m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    sql = "SELECT COUNT(O.CCMF_ID) "
                        + "FROM dbo.TB_CCMF_APPLICATION AS O INNER JOIN "
                        + "dbo.TB_PROGRAMME_INTAKE AS S ON O.Programme_ID = S.Programme_ID "
                        + "WHERE (S.Programme_Name = @Programme_Name) AND (O.Intake_Number = @Intake_Number)";

                    cmd.Parameters.Add(new SqlParameter("@Programme_Name", m_Programme_Name));
                    cmd.Parameters.Add(new SqlParameter("@Intake_Number", m_Intake_Number));
                    conn.Open();
                    try
                    {
                        m_CCMF = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            if (m_CCMF == 0 && m_INCUBATION > 0)
            {
                m_result = "CPIP";
            }
            else if (m_CCMF > 0 && m_INCUBATION == 0)
            {
                m_result = "CCMF";
            }
            else if (m_CCMF == 0 && m_INCUBATION == 0)
            {
                m_result = "not data";
            }
            return m_result;

        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            string v_Programme_Name = lstCyberportProgramme.SelectedValue.ToString();
            string v_Intake_Number = lstIntakeNumber.SelectedValue.ToString();
            //m_Programme_Type = checkProgrammeType(v_Programme_Name, v_Intake_Number);
            if (v_Programme_Name.Contains("Cyberport Incubation Program"))
            {
                //CPIP
                ExportCSVCPIP();
            }
            else
            {
                //CCMF
                ExportCSVCCMF();
            }

        }

        protected void BtnDuplicated_Submission_Click(object sender, EventArgs e)
        {
            ExportDuplicated_Submission();
        }



        protected string getApplicationNumber_info_backup(string m_HKID)
        {
            string m_result = "";

            var sqlString = "";
            //csv += "\r\n";

            ConnectOpen();
            try
            {
                sqlString = "SELECT [Application_Number], [Company_Name_Eng] as [PrjCompName] "
                          + "from [TB_INCUBATION_APPLICATION], [TB_APPLICATION_COMPANY_CORE_MEMBER] "
                          + "where [TB_INCUBATION_APPLICATION].[Incubation_ID] = [TB_APPLICATION_COMPANY_CORE_MEMBER].[Application_ID] "
                          + "and ([TB_INCUBATION_APPLICATION].[STATUS] <> 'Saved') "
                          + "and ([TB_INCUBATION_APPLICATION].[STATUS] <> 'Deleted') "
                          + "and [TB_APPLICATION_COMPANY_CORE_MEMBER].[HKID] =@HKID "
                          + "union "
                          + "SELECT [Application_Number], [Project_Name_Eng] as [PrjCompName] "
                          + "from [TB_CCMF_APPLICATION], [TB_APPLICATION_COMPANY_CORE_MEMBER] "
                          + "where [TB_CCMF_APPLICATION].[CCMF_ID] = [TB_Application_Company_Core_Member].[Application_ID] "
                          + "and  ([TB_CCMF_APPLICATION].[STATUS] <> 'Saved') "
                          + "and ([TB_CCMF_APPLICATION].[STATUS] <> 'Deleted') "
                          + "and [TB_Application_Company_Core_Member].[HKID] = @HKID "
                          + "union "
                          + "SELECT [TB_PAST_APPLICATION].[Application_Number], [TB_PAST_APPLICATION].[Name] as [PrjCompName] "
                          + "from  [TB_PAST_APPLICATION] "
                          + "where [TB_PAST_APPLICATION].[ID_Number] = @HKID ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@HKID", m_HKID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //csv += reader.GetValue(reader.GetOrdinal("Masked_HKID")).ToString() + "|" + reader.GetValue(reader.GetOrdinal("HKID")).ToString() + "<br>"; 
                    m_result += reader.GetValue(reader.GetOrdinal("Application_Number")).ToString() + "[" + reader.GetValue(reader.GetOrdinal("PrjCompName")).ToString() + "] ,";
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            if (m_result.Length > 0)
            {
                m_result = m_result.Substring(0, m_result.Length - 1);
            }

            return m_result;
        }

        protected ArrayList getApplicationNumber_info(string m_HKID, string m_Application_Number)
        {
            ArrayList m_result = new ArrayList();
            int CCMFCount = 0;
            int CPIPCount = 0;
            int PastCount = 0;
            var sqlString = "";
            //csv += "\r\n";

            ConnectOpen();
            try
            {


                sqlString = "SELECT DISTINCT dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.HKID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Masked_HKID, "
                          + "dbo.TB_CCMF_APPLICATION.Application_Number, dbo.TB_CCMF_APPLICATION.Status, dbo.TB_CCMF_APPLICATION.Project_Name_Eng as PrjCompName "
                          + "FROM dbo.TB_APPLICATION_COMPANY_CORE_MEMBER LEFT OUTER JOIN "
                          + "dbo.TB_CCMF_APPLICATION ON dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID = dbo.TB_CCMF_APPLICATION.CCMF_ID "
                          + "WHERE (dbo.TB_CCMF_APPLICATION.Application_Number IS NOT NULL) AND (dbo.TB_CCMF_APPLICATION.Status <> 'Saved') AND (dbo.TB_CCMF_APPLICATION.Status <> 'Deleted') and (HKID = @HKID) "
                          + "and (dbo.TB_CCMF_APPLICATION.Application_Number <> @m_Application_Number)"
                          + "order by Application_Number asc ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@HKID", m_HKID));
                command.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CCMFCount++;
                    //csv += reader.GetValue(reader.GetOrdinal("Masked_HKID")).ToString() + "|" + reader.GetValue(reader.GetOrdinal("HKID")).ToString() + "<br>"; 
                    m_result.Add(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString() + " [" + reader.GetValue(reader.GetOrdinal("PrjCompName")).ToString() + "]");
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            ConnectOpen();
            try
            {

                sqlString = "SELECT DISTINCT dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.HKID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Masked_HKID, "
                          + "dbo.TB_INCUBATION_APPLICATION.Application_Number, dbo.TB_INCUBATION_APPLICATION.Status, dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng as PrjCompName "
                          + "FROM dbo.TB_APPLICATION_COMPANY_CORE_MEMBER LEFT OUTER JOIN "
                          + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID = dbo.TB_INCUBATION_APPLICATION.Incubation_ID "
                          + "WHERE (dbo.TB_INCUBATION_APPLICATION.Application_Number IS NOT NULL) AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved') AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Deleted') and (HKID = @HKID) "
                          + "and (dbo.TB_INCUBATION_APPLICATION.Application_Number <> @m_Application_Number)"
                          + "order by Application_Number asc";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@HKID", m_HKID));
                command.Parameters.Add(new SqlParameter("@m_Application_Number", m_Application_Number));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CPIPCount++;
                    //csv += reader.GetValue(reader.GetOrdinal("Masked_HKID")).ToString() + "|" + reader.GetValue(reader.GetOrdinal("HKID")).ToString() + "<br>"; 
                    m_result.Add(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString() + " [" + reader.GetValue(reader.GetOrdinal("PrjCompName")).ToString() + "]");
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            ConnectOpen();
            try
            {
                sqlString = "Select [Application_Number], [Name] as [PrjCompName] from TB_PAST_APPLICATION where ID_Number=@HKID";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@HKID", m_HKID));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PastCount++;
                    //csv += reader.GetValue(reader.GetOrdinal("Masked_HKID")).ToString() + "|" + reader.GetValue(reader.GetOrdinal("HKID")).ToString() + "<br>"; 
                    m_result.Add(reader.GetValue(reader.GetOrdinal("Application_Number")).ToString() + " [" + reader.GetValue(reader.GetOrdinal("PrjCompName")).ToString() + "]");
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }


            var sumCount = CCMFCount + CPIPCount + PastCount;
            //m_result += CCMFCount.ToString() + "," + CPIPCount.ToString() + "," + PastCount.ToString() + "," + sumCount.ToString() + ",";
            if (sumCount <= 0)
            {
                m_result.Clear();
            }

            //if (m_result.Length > 0)
            //{
            //    m_result = m_result.Substring(0, m_result.Length - 1);
            //}

            return m_result;
        }

        protected void ExportDuplicated_Submission()
        {
            DataTable dtData = CreateTableSchema();
            var sqlString = "";

            ConnectOpen();
            try
            {
                string v_Programme_Name = lstCyberportProgramme.SelectedValue.ToString();
                string v_Intake_Number = lstIntakeNumber.SelectedValue.ToString();
                m_Programme_Type = checkProgrammeType(v_Programme_Name, v_Intake_Number);

                //if (v_Programme_Name == "Cyberport Incubation Programme")
                if (lstCyberportProgramme.SelectedValue.Contains("Cyberport Incubation Program"))
                {
                    //sqlString = "SELECT DISTINCT  dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.HKID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Masked_HKID, "
                    //          + "dbo.TB_INCUBATION_APPLICATION.Application_Number, dbo.TB_INCUBATION_APPLICATION.Status "
                    //          + "FROM dbo.TB_APPLICATION_COMPANY_CORE_MEMBER LEFT OUTER JOIN "
                    //          + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID = dbo.TB_INCUBATION_APPLICATION.Incubation_ID "
                    //          + "WHERE (dbo.TB_INCUBATION_APPLICATION.Application_Number IS NOT NULL) AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved') AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Deleted') and Application_ID in (select Incubation_ID as Application_ID from TB_INCUBATION_APPLICATION where Status<>'Saved' and Status<>'Deleted' and Programme_ID in "
                    //          + "(Select top 1 Programme_ID from [TB_PROGRAMME_INTAKE] where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number)) "
                    //          + "order by Application_Number asc ";

                    sqlString = "SELECT DISTINCT  dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.HKID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Masked_HKID, "
                              + "dbo.TB_INCUBATION_APPLICATION.Application_Number, dbo.TB_INCUBATION_APPLICATION.Status, dbo.TB_INCUBATION_APPLICATION.Company_Name_Eng as PrjCompName "
                              + "FROM dbo.TB_APPLICATION_COMPANY_CORE_MEMBER LEFT OUTER JOIN "
                              + "dbo.TB_INCUBATION_APPLICATION ON dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID = dbo.TB_INCUBATION_APPLICATION.Incubation_ID "
                              + "WHERE (dbo.TB_INCUBATION_APPLICATION.Application_Number IS NOT NULL) AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Saved') AND (dbo.TB_INCUBATION_APPLICATION.Status <> 'Deleted') and Application_ID in (select Incubation_ID as Application_ID from TB_INCUBATION_APPLICATION where Status<>'Saved' and Status<>'Deleted' and Programme_ID in "
                              + "(Select top 1 Programme_ID from [TB_PROGRAMME_INTAKE] where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number)) "
                              + "order by Application_Number asc ";
                }
                else
                {
                    //          + "dbo.TB_CCMF_APPLICATION.Application_Number, dbo.TB_CCMF_APPLICATION.Status "
                    //          + "FROM dbo.TB_APPLICATION_COMPANY_CORE_MEMBER LEFT OUTER JOIN "
                    //          + "dbo.TB_CCMF_APPLICATION ON dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID = dbo.TB_CCMF_APPLICATION.CCMF_ID "
                    //          + "WHERE (dbo.TB_CCMF_APPLICATION.Application_Number IS NOT NULL) AND (dbo.TB_CCMF_APPLICATION.Status <> 'Saved') AND (dbo.TB_CCMF_APPLICATION.Status <> 'Deleted') and Application_ID in (select CCMF_ID as Application_ID from TB_CCMF_APPLICATION where Status<>'Saved' and Status<>'Deleted' and Programme_ID in "
                    //          + "(Select top 1 Programme_ID from [TB_PROGRAMME_INTAKE] where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number)) "
                    //          + "order by Application_Number asc ";

                    sqlString = "SELECT DISTINCT  dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.HKID, dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Masked_HKID, "
                              + "dbo.TB_CCMF_APPLICATION.Application_Number, dbo.TB_CCMF_APPLICATION.Status, dbo.TB_CCMF_APPLICATION.Project_Name_Eng as PrjCompName "
                              + "FROM dbo.TB_APPLICATION_COMPANY_CORE_MEMBER LEFT OUTER JOIN "
                              + "dbo.TB_CCMF_APPLICATION ON dbo.TB_APPLICATION_COMPANY_CORE_MEMBER.Application_ID = dbo.TB_CCMF_APPLICATION.CCMF_ID "
                              + "WHERE (dbo.TB_CCMF_APPLICATION.Application_Number IS NOT NULL) AND (dbo.TB_CCMF_APPLICATION.Status <> 'Saved') AND (dbo.TB_CCMF_APPLICATION.Status <> 'Deleted') and Application_ID in (select CCMF_ID as Application_ID from TB_CCMF_APPLICATION where Status<>'Saved' and Status<>'Deleted' and Programme_ID in "
                              + "(Select top 1 Programme_ID from [TB_PROGRAMME_INTAKE] where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number)) "
                              + "order by Application_Number asc ";
                }





                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Programme_Name", lstCyberportProgramme.SelectedValue.ToString()));
                command.Parameters.Add(new SqlParameter("@Intake_Number", lstIntakeNumber.SelectedValue.ToString()));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {

                    var Duplicated_list = getApplicationNumber_info(reader.GetValue(reader.GetOrdinal("HKID")).ToString(), reader.GetValue(reader.GetOrdinal("Application_Number")).ToString());
                    int countApp = 0;
                    if (Duplicated_list.Count > 0)
                    {
                        foreach (string projName in Duplicated_list)
                        {
                            if (countApp == 0)
                                dtData.Rows.Add(reader.GetValue(reader.GetOrdinal("Masked_HKID")).ToString(), reader.GetValue(reader.GetOrdinal("Application_Number")).ToString() + " [" + reader.GetValue(reader.GetOrdinal("PrjCompName")).ToString() + "]", projName);
                            else
                                dtData.Rows.Add(" ", " ", projName);
                            countApp++;

                        }

                    }

                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }

            StringBuilder data = ConvertDataTableToCsvFile(dtData);
            //lbltest.Text = csv.Replace("\r\n", "<br>");
            //Download the CSV file.

            if (data.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition",
            "attachment;filename=" + lstCyberportProgramme.SelectedValue + lstIntakeNumber.SelectedValue + "_Duplicated_Submission.csv");
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                System.IO.StreamWriter sw =
                    new System.IO.StreamWriter(
                    HttpContext.Current.Response.OutputStream,
                    System.Text.Encoding.UTF8);
                sw.Write(data);
                sw.Close();
                Context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                HttpContext.Current.Response.End();
                Context.Response.Redirect(Context.Request.RawUrl);

            }


        }

        public DataTable CreateTableSchema()
        {
            DataTable dtData = new DataTable("Application_Duplication");
            dtData.Columns.Add("Masked_HKID", typeof(string));
            dtData.Columns.Add("Application_Number", typeof(string));
            dtData.Columns.Add("Duplicated", typeof(string));
            return dtData;
        }

        public StringBuilder ConvertDataTableToCsvFile(DataTable dtData)
        {
            StringBuilder data = new StringBuilder();

            //Taking the column names.
            for (int column = 0; column < dtData.Columns.Count; column++)
            {
                //Making sure that end of the line, shoould not have comma delimiter.
                if (column == dtData.Columns.Count - 1)
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(",", ";"));
                else
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(",", ";") + ',');
            }

            data.Append(Environment.NewLine);//New line after appending columns.

            for (int row = 0; row < dtData.Rows.Count; row++)
            {
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    ////Making sure that end of the line, shoould not have comma delimiter.
                    if (column == dtData.Columns.Count - 1)
                        data.Append(dtData.Rows[row][column].ToString().Replace(",", ";"));
                    else
                        data.Append(dtData.Rows[row][column].ToString().Replace(",", ";") + ',');
                }

                //Making sure that end of the file, should not have a new line.
                if (row != dtData.Rows.Count - 1)
                    data.Append(Environment.NewLine);
            }
            return data;
        }

        protected void ExportCSVCPIP()
        {
            string csv = "Application No.,Cluster,Company Name,Company Name Chinese,Status,Video Link, "
                       + "BDM - Market viability with milestones (30%),BDM - Quality and competence of the management team (20%),BDM - Business scalability (20%),BDM -Functional prototype or product to solve a real problem (20%),BDM - Innovativeness (10%),BDM Score,Comments,"
                       + "Sr. Manager - Market viability with milestones (30%),Sr. Manager - Quality and competence of the management team (20%),Sr. Manager - Business scalability (20%),Sr. Manager -Functional prototype or product to solve a real problem (20%),Sr. Manager - Innovativeness (10%),Sr. Manager Score,Comments,"
                       + "CPMO - Market viability with milestones (30%),CPMO - Quality and competence of the management team (20%),CPMO -Business scalability (20%),CPMO - Functional prototype or product to solve a real problem (20%),CPMO - Innovativeness (10%),CPMO Score,Comments,"
                       + "Average Score,Remarks,RemarksForVetting,Shortlisted, Project Description ";
            csv += "\r\n";
            //Download the CSV file.
            //var applicationLists = GetAllOfApplicationBindData();
            m_ProgrammeName = lstCyberportProgramme.SelectedValue;
            m_IntakeNum = lstIntakeNumber.SelectedValue;
            m_Status = "%";
            m_Cluster = lstCluster.SelectedValue;
            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            var applicationLists = getApplicationList();
            foreach (ApplicationList row in applicationLists)
            {
                csv += "\"" + row.ApplicationNo + "\",";
                csv += "\"" + row.Cluster + "\",";
                csv += "\"" + row.CompanyName + "\",";
                csv += "\"" + row.CompanyNameChinese + "\",";
                csv += "\"" + row.Status + "\",";
                csv += "\"" + getVideoLink(row.ApplicationID) + "\",";

                csv += getScoreCPIP(row.ApplicationNo);

                csv += "\"" + row.AverageScore + "\",";
                csv += "\"" + row.Remarks.Replace("\n", " ").Replace("\r", " ") + "\",";
                csv += "\"" + row.RemarksForVetting.Replace("\n", " ").Replace("\r", " ") + "\",";
                csv += "\"" + (row.Shortlisted ? "True" : "False") + "\",";
                csv += "\"" + row.ProjectDescription.Replace(System.Environment.NewLine, " ") + "\",";


                csv += "\r\n";
            }


            csv = csv.Replace("<br>", " ").Replace("&nbsp;", " ");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
        "attachment;filename=" + lstCyberportProgramme.SelectedValue + lstIntakeNumber.SelectedValue + ".csv");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.IO.StreamWriter sw =
                new System.IO.StreamWriter(
                HttpContext.Current.Response.OutputStream,
                System.Text.Encoding.UTF8);
            sw.Write(csv);
            sw.Close();
            Context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            HttpContext.Current.Response.End();
            Context.Response.Redirect(Context.Request.RawUrl);

        }


        protected string getScoreCPIP(string m_applicationNumber)
        {
            //CPIP BDM
            //CPMO
            //Senior Manager

            string Score = "";
            string BDMScore = ",,,,,,,";
            string SrScore = ",,,,,,,";
            string CPMOScore = ",,,,,,,";
            string sql = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                sql = "SELECT (Management_Team / 0.3) as Management_Team, "
                     + "(Creativity /0.2) as Creativity, "
                     + "(Business_Viability / 0.2) as Business_Viability, "
                     + "(Benefit_To_Industry / 0.2) as Benefit_To_Industry, "
                     + "(Proposal_Milestones / 0.1) as Proposal_Milestones, "
                     + "Total_Score,Comments, Role "
                     + "FROM dbo.TB_SCREENING_INCUBATION_SCORE "
                     + "where Application_Number=@m_applicationNumber ";


                //sql = "SELECT (Management_Team / 0.2) as Management_Team, "
                //     + "(Creativity /0.2) as Creativity, "
                //     + "(Business_Viability / 0.3) as Business_Viability, "
                //     + "(Benefit_To_Industry / 0.1) as Benefit_To_Industry, "
                //     + "(Proposal_Milestones / 0.2) as Proposal_Milestones, "
                //     + "Total_Score,Comments, Role "
                //     + "FROM dbo.TB_SCREENING_INCUBATION_SCORE "
                //     + "where Application_Number=@m_applicationNumber ";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_applicationNumber", m_applicationNumber));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            //Score = reader.GetValue(reader.GetOrdinal("role")).ToString()+ ",";
                            Score = "\"" + reader.GetDecimal(reader.GetOrdinal("Management_Team")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Creativity")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Business_Viability")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Benefit_To_Industry")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Proposal_Milestones")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Total_Score")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetValue(reader.GetOrdinal("Comments")).ToString() + "\",";

                            if (reader.GetValue(reader.GetOrdinal("role")).ToString() == "CPIP BDM")
                            {
                                BDMScore = Score;
                            }
                            else if (reader.GetValue(reader.GetOrdinal("role")).ToString() == "Senior Manager")
                            {
                                SrScore = Score;
                            }
                            else if (reader.GetValue(reader.GetOrdinal("role")).ToString() == "CPMO")
                            {
                                CPMOScore = Score;
                            }

                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return BDMScore + SrScore + CPMOScore;
        }

        protected string getScoreCCMF(string m_applicationNumber)
        {
            //CCMF BDM
            //CPMO
            //Senior Manager

            string Score = "";
            string BDMScore = ",,,,,,";
            string SrScore = ",,,,,,";
            string CPMOScore = ",,,,,,";
            string sql = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql = "Select (Management_Team /0.3)as Management_Team, "
                    + "(Business_Model /0.3)as Business_Model, "
                    + "(Creativity /0.3)as Creativity, "
                    + "(Social_Responsibility /0.1)as Social_Responsibility, "
                    + "Total_Score,Comments,role from TB_SCREENING_CCMF_SCORE "
                    + "where Application_Number=@m_applicationNumber ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_applicationNumber", m_applicationNumber));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            //Score = reader.GetValue(reader.GetOrdinal("role")).ToString()+ ",";
                            Score = "\"" + reader.GetDecimal(reader.GetOrdinal("Management_Team")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Business_Model")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Creativity")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Social_Responsibility")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetDecimal(reader.GetOrdinal("Total_Score")).ToString("f3") + "\",";
                            Score += "\"" + reader.GetValue(reader.GetOrdinal("Comments")).ToString() + "\",";

                            if (reader.GetValue(reader.GetOrdinal("role")).ToString() == "CCMF BDM")
                            {
                                BDMScore = Score;
                            }
                            else if (reader.GetValue(reader.GetOrdinal("role")).ToString() == "Senior Manager")
                            {
                                SrScore = Score;
                            }
                            else if (reader.GetValue(reader.GetOrdinal("role")).ToString() == "CPMO")
                            {
                                CPMOScore = Score;
                            }

                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return BDMScore + SrScore + CPMOScore;
        }

        protected void ExportCSVCCMF()
        {
            var csv = "Application No.,Cluster,Project Name,Project Name Chinese,Programme Type,Stream,Application Type,Smart Space,Status,Video Link, "
                       + "BDM - Management Team (30%),BDM - Business Model & Time to Market (30%),BDM - Creativty and Innovation of the Proposed Project. Product and Service (30%),BDM - Social Responsibility (10%),BDM Score,Comments,"
                       + "Sr. Manager - Management Team (30%),Sr. Manager - Business Model & Time to Market (30%),Sr. Manager - Creativty and Innovation of the Proposed Project. Product and Service (30%),Sr. Manager - Social Responsibility (10%),Sr. Manager Score,Comments,"
                       + "CPMO - Management Team (30%),CPMO - Business Model & Time to Market (30%),CPMO - Creativty and Innovation of the Proposed Project. Product and Service (30%),CPMO - Social Responsibility (10%),CPMO Score,Comments,"
                       + "Average Score,Remarks,RemarksForVetting,Shortlisted, Project Description ";
            csv += "\r\n";
            //Download the CSV file.
            //var applicationLists = GetAllOfApplicationBindData();
            m_ProgrammeName = lstCyberportProgramme.SelectedValue;
            m_IntakeNum = lstIntakeNumber.SelectedValue;
            m_Status = "%";
            m_Cluster = lstCluster.SelectedValue;
            m_SortCluster = lstSortCluster.SelectedValue;
            m_SortClusterOrder = radioSortCluster.SelectedValue;
            m_SortApplicationNo = lstSortApplicationNo.SelectedValue;

            var applicationLists = getApplicationList();
            foreach (ApplicationList row in applicationLists)
            {

                csv += "\"" + row.ApplicationNo + "\",";
                csv += "\"" + row.Cluster + "\",";
                csv += "\"" + row.ProjectName + "\",";
                csv += "\"" + row.ProjectNameChinese + "\",";
                csv += "\"" + row.ProgrammeType + "\",";
                csv += "\"" + row.HongKongProgrammeStream + "\",";
                csv += "\"" + row.ApplicationType + "\",";
                csv += "\"" + row.SmartSpace + "\",";
                csv += "\"" + row.Status + "\",";
                csv += "\"" + getVideoLink(row.ApplicationID) + "\",";

                csv += getScoreCCMF(row.ApplicationNo);

                csv += "\"" + row.AverageScore + "\",";
                csv += "\"" + row.Remarks.Replace("\n", " ").Replace("\r", " ") + "\",";
                csv += "\"" + row.RemarksForVetting.Replace("\n", " ").Replace("\r", " ") + "\",";
                csv += "\"" + (row.Shortlisted ? "True" : "False") + "\",";
                csv += "\"" + row.ProjectDescription.Replace(System.Environment.NewLine, " ") + "\",";


                csv += "\r\n";
            }


            csv = csv.Replace("<br>", " ").Replace("&nbsp;", " ");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
            "attachment;filename=" + lstCyberportProgramme.SelectedValue + lstIntakeNumber.SelectedValue + ".csv");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.IO.StreamWriter sw =
                new System.IO.StreamWriter(
                HttpContext.Current.Response.OutputStream,
                System.Text.Encoding.UTF8);
            sw.Write(csv);
            sw.Close();
            Context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            HttpContext.Current.Response.End();
            Context.Response.Redirect(Context.Request.RawUrl);


        }

        protected string getRemarksForVetting(string m_applicationNumber)
        {
            string m_result = "";
            string sql = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql = "SELECT Remarks_To_Vetting, Shortlisted "
                    + "FROM TB_APPLICATION_SHORTLISTING "
                    + "where Application_Number=@m_applicationNumber ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@m_applicationNumber", m_applicationNumber));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            m_result = reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString() + ",";
                            m_result += reader.GetValue(reader.GetOrdinal("Shortlisted")).ToString() + ",";
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return m_result;
        }

        protected string getVideoLink(string ApplicationID)
        {
            string m_result = "";
            string sql = "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql = "select Attachment_Path from TB_APPLICATION_ATTACHMENT where Application_ID = @Application_ID and Attachment_Type = 'Video_Clip'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("@Application_ID", ApplicationID));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            m_result = reader.GetValue(reader.GetOrdinal("Attachment_Path")).ToString();
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return m_result;
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(btnExportPDFList.Value))
            {
                string PdfZipDestinationFileName = "";
                string FileName = lstCyberportProgramme.SelectedItem.Text + " " + lstIntakeNumber.SelectedItem.Text;
                if (btnExportPDFList.Value.ToLower().Contains("cpip"))
                {
                    PdfZipDestinationFileName = "PDF_" + "CPIP_" + lstIntakeNumber.SelectedValue + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                }
                else if (btnExportPDFList.Value.ToLower().Contains("cupp")) {
                    PdfZipDestinationFileName = "PDF_" + "CUPP_" + lstIntakeNumber.SelectedValue + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                }
                else if (btnExportPDFList.Value.ToLower().Contains("gbayep")) {
                    PdfZipDestinationFileName = "PDF_" + "GBAYEP_" + lstIntakeNumber.SelectedValue + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                }
                else

                    PdfZipDestinationFileName = "PDF_" + "CCMF_HK_" + lstIntakeNumber.SelectedValue + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                // uncomment
                string PdfZipDestinationFolder = m_path + @"\" + PdfZipDestinationFileName;
                m_downloadLink = m_zipfiledownloadurl;
                string m_username = SPContext.Current.Web.CurrentUser.Name.ToString();
                string m_mail = "";
                if (m_ApplicationIsInDebug == "1")
                {
                    m_mail = m_ApplicationDebugEmailSentTo;
                }
                else
                {
                    m_mail = SPContext.Current.Web.CurrentUser.Email;
                }
                string m_subject = "";
                string m_body = "";
                string m_Programme_Name = lstCyberportProgramme.SelectedItem.Text;
                string m_Intake_Number = lstIntakeNumber.SelectedItem.Text;
                string m_Password;
                string m_downloadlink = m_downloadLink;
                string m_Starttime;
                string m_Endtime;

                //starting email
                m_subject = "Pdf File : " + m_Programme_Name + " / " + m_Intake_Number + " is processing.";
                m_body = getEmailTemplate("ZipPdfStartEmail");
                sharepointsendemail(m_mail, m_subject, m_body);
                m_Starttime = DateTime.Now.ToString();

                byte[] strs = null;
                //List<byte []> sumallpdf = new List<byte []>();

                string dir = GetSysParam("zipfiledownloadpath");

                using (new NetworkConnection(dir, new NetworkCredential(GetSysParam("NetworkDriveUser"), GetSysParam("NetworkDrivePassword"), GetSysParam("NetworkDriveDomain"))))
                {

                    using (var dbcontext = new CyberportEMS_EDM())
                    {
                        List<string> ObjListApplication = btnExportPDFList.Value.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
                        if (btnExportPDFList.Value.ToLower().Contains("cpip"))
                        {
                            ZipOutputStream objzipStream = new ZipOutputStream(File.Create(PdfZipDestinationFolder));
                            //Uncomment 3 line
                            objzipStream.SetLevel(9); //0-9, 9 being the highest level of compression
                            objzipStream.Password = genRandom(8);   // optional. Null is the same as not setting. Required if using AES.
                            m_Password = objzipStream.Password;

                            foreach (var item in ObjListApplication)
                            {

                                try
                                {

                                    //TB_INCUBATION_APPLICATION Application_Parent_id = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item && string.IsNullOrEmpty(x.Application_Parent_ID));
                                    TB_INCUBATION_APPLICATION Application_Parent_id = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
                                    //    Application_Parent_id = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item && string.IsNullOrEmpty(x.Application_Parent_ID));

                                    if (Application_Parent_id != null)
                                    {
                                        using (MemoryStream output = new MemoryStream())
                                        {
                                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);

                                            PdfWriter wri = PdfWriter.GetInstance(doc, output);
                                            Common.ITextEvents ievents = new Common.ITextEvents();
                                            ievents.DocRefNo = "Doc Ref: ENC.SF.040";
                                            ievents.FooterAppName = item;
                                            wri.PageEvent = ievents;
                                            doc.Open();
                                            CreatePDFIncubation(item, ref doc);
                                            doc.Close();
                                            ZipEntry objZipEntry = new ZipEntry(item + ".pdf");
                                            //objZipEntry.DateTime = DateTime.Now;
                                            //objZipEntry.Size = output.Length;
                                            var strm = output.ToArray();
                                            objzipStream.PutNextEntry(objZipEntry);

                                            objzipStream.Write(strm, 0, Convert.ToInt32(strm.Length));
                                            //ShowPdf(output.ToArray());
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                }


                            }




                            //sumallpdf.Add(strs);

                            //objzipStream.Finish();
                            objzipStream.Close();
                            m_Endtime = DateTime.Now.ToString();
                            InsertTBDownloadZIP(m_username, m_mail, "PDF ZIP", PdfZipDestinationFolder, PdfZipDestinationFileName, m_Password, "1");

                            m_subject = "Pdf File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
                            m_body = getEmailTemplate("ZipPdfEndEmail");
                            m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_FileName", PdfZipDestinationFileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                            sharepointsendemail(m_mail, m_subject, m_body);
                            lbldownloadmessage.Text = "Download Complete.";


                            //System.Web.HttpContext.Current.Response.ContentType = "application/zip";
                            //System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".zip");

                            //System.Web.HttpContext.Current.Response.End();
                            //System.Web.HttpContext.Current.Response.Flush();

                        }
                        else
                        {
                            ZipOutputStream objzipStream = new ZipOutputStream(File.Create(PdfZipDestinationFolder));
                            objzipStream.SetLevel(9); //0-9, 9 being the highest level of compression
                            objzipStream.Password = genRandom(8);   // optional. Null is the same as not setting. Required if using AES.
                            m_Password = objzipStream.Password;

                            foreach (var item in ObjListApplication)
                            {
                                try
                                {

                                    TB_CCMF_APPLICATION Application_Parent_id = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == item && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
                                    //TB_INCUBATION_APPLICATION Application_Parent_id = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
                                    if (Application_Parent_id != null)
                                    {
                                        using (MemoryStream output = new MemoryStream())
                                        {
                                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);

                                            PdfWriter wri = PdfWriter.GetInstance(doc, output);
                                            Common.ITextEvents ievents = new Common.ITextEvents();
                                            ievents.FooterAppName = item;
                                            ievents.DocRefNo = "Doc Ref: ENC.SF.041";
                                            wri.PageEvent = ievents;
                                            doc.Open();
                                            CreatePDFCCMF(item, ref doc);
                                            doc.Close();
                                            ZipEntry objZipEntry = new ZipEntry(item + ".pdf");
                                            //objZipEntry.DateTime = DateTime.Now;
                                            //objZipEntry.Size = output.Length;
                                            var strm = output.ToArray();
                                            objzipStream.PutNextEntry(objZipEntry);

                                            objzipStream.Write(strm, 0, Convert.ToInt32(strm.Length));
                                        }
                                        //sumallpdf.Add(strs);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }

                            }
                            objzipStream.Close();
                            m_Endtime = DateTime.Now.ToString();

                            InsertTBDownloadZIP(m_username, m_mail, "PDF ZIP", PdfZipDestinationFolder, PdfZipDestinationFileName, m_Password, "1");


                            m_subject = "Pdf File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
                            m_body = getEmailTemplate("ZipPdfEndEmail");
                            m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_FileName", PdfZipDestinationFileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                            sharepointsendemail(m_mail, m_subject, m_body);
                            lbldownloadmessage.Text = "Download Complete.";



                            //System.Web.HttpContext.Current.Response.ContentType = "application/zip";
                            //System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + ".zip");

                            //System.Web.HttpContext.Current.Response.End();
                            //System.Web.HttpContext.Current.Response.Flush();

                        }
                    }
                }
            }
        }
        private string GetSysParam(string param)
        {
            ConnectOpen();

            var sqlString = "SELECT Value FROM TB_SYSTEM_PARAMETER WHERE Config_Code = @Config";
            var command = new SqlCommand(sqlString, connection);
            var result = string.Empty;

            try
            {
                command.Parameters.AddWithValue("@Config", param);
                result = command.ExecuteScalar().ToString();
            }
            finally
            {
                ConnectClose();
            }
            return result;
        }

        //private static string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
        //                                  "microsoft-jhenghei-5965ec3170036.ttf");
        private static string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),"msjh.ttc,0");
        private static BaseFont fontInternational = BaseFont.CreateFont(arialuniTff, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private Font Font12blue = new Font(fontInternational, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#075CA9")));
        private Font Font12blueLight = new Font(fontInternational, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#145DAA")));
        private Font Font12Green = new Font(fontInternational, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#80C343")));

        private Font Font12black = new Font(fontInternational, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
        private Font Font15Head = new Font(fontInternational, 15, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#075CA9"))); // need bold
        private Font Font18blue = new Font(fontInternational, 18, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#075CA9"))); // need bold
        private Font Font10blue = new Font(fontInternational, 8, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#075CA9"))); // need bold

        //Incubation PDF Generation Code
        private void CreatePDFIncubation(string Application_Number, ref Document doc)
        {

            using (var dbcontext = new CyberportEMS_EDM())
            {

                TB_INCUBATION_APPLICATION objTB_INCUBATION_APPLICATION = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == Application_Number && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
                TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbcontext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == objTB_INCUBATION_APPLICATION.Programme_ID);

                PdfPTable table_2 = new PdfPTable(2);
                table_2.DefaultCell.Border = Rectangle.NO_BORDER;
                table_2.TotalWidth = 500f;
                table_2.LockedWidth = true;
                float[] widths2 = new float[] { 250f, 250f };
                table_2.SetWidths(widths2);


                table_2.AddCell(new Paragraph("Application Name:", Font12blue));
                table_2.AddCell(new Paragraph(objTB_PROGRAMME_INTAKE.Programme_Name, Font12blue));
                table_2.AddCell(new Paragraph("Intake number:", Font12blue));

                table_2.AddCell(new Paragraph(objTB_PROGRAMME_INTAKE.Intake_Number.ToString(), Font12blue));
                table_2.AddCell(new Paragraph("Application Number:", Font12blue));
                table_2.AddCell(new Paragraph(objTB_INCUBATION_APPLICATION.Application_Number, Font12blue));
                table_2.AddCell(new Paragraph("Last Submitted Date:", Font12blue));
                table_2.AddCell(new Paragraph(objTB_INCUBATION_APPLICATION.Last_Submitted.ToString("dd MMM yyyy"), Font12blue));

                doc.Add(table_2);
                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                doc.Add(p);

                Paragraph para = new Paragraph("You are  Required:", Font12Green);
                para.Alignment = Element.ALIGN_LEFT;
                doc.Add(para);
                doc.Add(new Chunk("\n"));
                //Paragraph para1 = new Paragraph("1. To observe the following requirements when filling the application:", Font12blueLight);
                Paragraph para1 = new Paragraph("1. " + TranslateIncubation("Instruction_1"), Font12blueLight);
                doc.Add(para1);
                doc.Add(new Chunk("\n"));
                //Paragraph para2 = new Paragraph("• Use English to complete the application unless otherwise speciofied;", Font12blueLight);
                Paragraph para2 = new Paragraph("• " + TranslateIncubation("Instruction_1_1"), Font12blueLight);

                para2.IndentationLeft = 30;
                doc.Add(para2);
                doc.Add(new Chunk("\n"));
                //Paragraph para3 = new Paragraph("• Present all monetary figures in Hongkong Dollar;", Font12blueLight);
                Paragraph para3 = new Paragraph("• " + TranslateIncubation("Instruction_1_2"), Font12blueLight);
                para3.IndentationLeft = 30;
                doc.Add(para3);
                doc.Add(new Chunk("\n"));
                //Paragraph para4 = new Paragraph("• Use Traditional Chinese characters for Chinese information;", Font12blueLight);
                Paragraph para4 = new Paragraph("• " + TranslateIncubation("Instruction_1_3"), Font12blueLight);
                para4.IndentationLeft = 30;
                doc.Add(para4);
                doc.Add(new Chunk("\n"));
                //Paragraph para5 = new Paragraph("• Put “NA” where the information sought is not applicable or not available;", Font12blueLight);
                Paragraph para5 = new Paragraph("• " + TranslateIncubation("Instruction_1_4"), Font12blueLight);
                para5.IndentationLeft = 30;
                doc.Add(para5);
                doc.Add(new Chunk("\n"));
                //Paragraph para6 = new Paragraph("2. To read the Cyberport Incubation Programme Guides and Notes for the Applicants  (ENC.RF.010)  before filling in this Application Form.", Font12blueLight);
                Paragraph para6 = new Paragraph("2. " + TranslateIncubation("Instruction_2"), Font12blueLight);
                doc.Add(para6);
                doc.Add(new Chunk("\n"));
                //Paragraph para7 = new Paragraph("3. The form must be completed by the applicant. If the applicant is a company, the form must be completed by the director or owner of the applicant organisation. Otherwise it will not be processed.", Font12blueLight);
                Paragraph para7 = new Paragraph("3. " + TranslateIncubation("Instruction_3"), Font12blueLight);
                doc.Add(para7);
                doc.Add(new Chunk("\n"));
                //Paragraph para8 = new Paragraph("4. ONE email address for EACH Cyberport Incubation Programme application , and that email address would receive all the emails in respect of the submitted application.  If you have another project to apply for Cyberport Incubation Programme, please use a separate email address for that application.", Font12blueLight);
                //doc.Add(para8);
                //Paragraph para8 = new Paragraph("4. To submit 1 online application associated with 1 email account is required in applying Cyberport Incubation Programme.", Font12blueLight);
                Paragraph para8 = new Paragraph("4. " + TranslateIncubation("Instruction_4"), Font12blueLight);
                doc.Add(para8);
                doc.Add(new Chunk("\n"));
                Paragraph para9b = new Paragraph("5. " + TranslateIncubation("Instruction_4_1"), Font12blueLight);
                doc.Add(para9b);
                doc.Add(new Chunk("\n"));
                Paragraph para14 = new Paragraph(TranslateIncubation("Instruction_6"), Font12blueLight);
                doc.Add(para14);
                doc.Add(new Chunk("\n"));
                //Paragraph para15 = new Paragraph("Hong Kong Cyberport Management Company Limited.", Font12blueLight);
                Paragraph para15 = new Paragraph(TranslateIncubation("Instruction_7"), Font12blueLight);
                doc.Add(para15);
                doc.Add(new Chunk("\n"));
                Paragraph para16 = new Paragraph(TranslateIncubation("Step_1_Profile"), Font18blue);
                para16.Alignment = Element.ALIGN_CENTER;
                doc.Add(para16);
                doc.Add(new Chunk("\n"));


                PdfPTable table = new PdfPTable(3);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 50f, 400f, 50f };
                table.SetWidths(widths);
                table.AddCell(new Paragraph("1.1", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q1"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_1.ToString() == "True" ? "Yes" : "No", Font12black));
                table.AddCell(new Paragraph("1.2", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q2"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_2.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_2.ToString() == "True" ? "Yes" : "No", Font12black));
                table.AddCell(new Paragraph("1.3", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q3"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_3.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_3.ToString() == "True" ? "Yes" : "No", Font12black));
                table.AddCell(new Paragraph("1.4", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q4"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_4.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_4.ToString() == "True" ? "Yes" : "No", Font12black));
                table.AddCell(new Paragraph("1.5", Font12blue));
                //table.AddCell(new Paragraph("If not, do you agree to set up a company registered as a legal entity and incorporated in Hong Kong upon admission to the Cyberport Incubation Programme?", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q5"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_5.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_5.ToString() == "True" ? "Yes" : "No", Font12black));

                //table.AddCell("");
                //Paragraph para1_5i = new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_1_Profile", "CyberportEMS_Incubation", 1033) "(i)	A company in the same field currently in any incubation programme(s) operated by Hong Kong Cyberport Management Company, Hong Kong Science & Technology Parks Corporation, or Hong Kong Design Centre. (together the “HK Incubators”),or", Font12blue);
                //para1_5i.IndentationLeft = 30;
                //table.AddCell(para1_5i);
                //table.AddCell("");

                //table.AddCell("");

                //Paragraph para1_5ii = new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_1_Profile", "CyberportEMS_Incubation", 1033) "(ii)	A company in the same field that has previously joined and graduated from any incubation programme operated by any of the HK Incubators.", Font12blue);
                //para1_5ii.IndentationLeft = 30;
                //table.AddCell(para1_5ii);
                //table.AddCell("");




                table.AddCell(new Paragraph("1.6", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q6"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_6.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_6.ToString() == "True" ? "Yes" : "No", Font12black));
                table.AddCell(new Paragraph("1.7", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q7"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_7.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_7.ToString() == "True" ? "Yes" : "No", Font12black));
                table.AddCell(new Paragraph("1.8", Font12blue));

                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q8"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_8.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_8.ToString() == "True" ? "Yes" : "No", Font12black));
                //table.AddCell(new Paragraph("1.9", Font12blue));
                //table.AddCell(new Paragraph("If you have answered “yes” to Question 1.8 above, are you applying for the Cyberport Incubation Programme using the same or similar project under CCMF? If similar or different, please clarify with details in Section 2.3.2.4 below to enable our Vetting Team to distinguish this application from any others.", Font12blue));
                //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_9.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_9.ToString() == "True" ? "Yes" : "No", Font12black));

                //table.AddCell(new Paragraph("1.9", Font12blue));
                //table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q8_1"), Font12blue));
                //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_8_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_8_1.ToString() == "True" ? "Yes" : "No", Font12black));

                table.AddCell(new Paragraph("1.9", Font12blue));
                table.AddCell(new Paragraph(TranslateIncubation("Step_1_Q9"), Font12blue));
                table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question1_10.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question1_10.ToString() == "True" ? "Yes" : "No", Font12black));


                doc.Add(table);

                Paragraph para17 = new Paragraph("2. " + TranslateIncubation("Step2_COMPANY_INFORMATION"), Font18blue);
                para17.Alignment = Element.ALIGN_CENTER;
                doc.Add(para17);
                doc.Add(new Chunk("\n"));

                Paragraph para18 = new Paragraph("2.1 " + TranslateIncubation("Step2_Company_Name"), Font15Head);
                doc.Add(para18);
                doc.Add(new Chunk("\n"));
                Chunk glue = new Chunk(new VerticalPositionMark());

                Paragraph para19 = new Paragraph(objTB_INCUBATION_APPLICATION.Company_Name_Eng, Font12black);
                para19.Add(new Chunk(glue));
                para19.Add(objTB_INCUBATION_APPLICATION.Company_Name_Chi);
                //para20.IndentationRight = 30;
                doc.Add(para19);
                doc.Add(new Chunk("\n"));


                doc.Add(new Paragraph("2.1 a " + TranslateIncubation("Step2_Website"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(objTB_INCUBATION_APPLICATION.Website, Font12black));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph("2.1 b " + TranslateIncubation("Step2_2b_Establishment"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(objTB_INCUBATION_APPLICATION.Establishment_Year.HasValue ? objTB_INCUBATION_APPLICATION.Establishment_Year.Value.ToString("MMM-yyyy") : "", Font12black));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph("2.1 c " + TranslateIncubation("Step2_2c_CountryOFOrigin"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(objTB_INCUBATION_APPLICATION.Country_Of_Origin, Font12black));
                doc.Add(new Chunk("\n"));

                Paragraph para21 = new Paragraph("2.2 " + TranslateIncubation("Step_2_Abstract"), Font15Head);
                doc.Add(para21);
                doc.Add(new Chunk("\n"));
                Paragraph para22 = new Paragraph("2.2.1 English", Font12blue);
                doc.Add(para22);
                doc.Add(new Chunk("\n"));
                Paragraph para23 = new Paragraph(objTB_INCUBATION_APPLICATION.Abstract, Font12black);
                doc.Add(para23);
                doc.Add(new Chunk("\n"));
                Paragraph para24 = new Paragraph("2.2.2 Chinese", Font12blue);
                doc.Add(para24);
                doc.Add(new Chunk("\n"));
                Paragraph para25 = new Paragraph(objTB_INCUBATION_APPLICATION.Abstract_Chi, Font12black);
                doc.Add(para25);
                doc.Add(new Chunk("\n"));
                Paragraph para26 = new Paragraph("2.3 " + TranslateIncubation("Step_2_Company_Details"), Font15Head);
                doc.Add(para26);
                doc.Add(new Chunk("\n"));
                Paragraph para27 = new Paragraph("2.3.1 " + TranslateIncubation("Step_2_Objectives"), Font12blue);
                doc.Add(para27);
                doc.Add(new Chunk("\n"));
                Paragraph para28 = new Paragraph(objTB_INCUBATION_APPLICATION.Objective, Font12black);
                doc.Add(para28);
                doc.Add(new Chunk("\n"));
                Paragraph para29 = new Paragraph("2.3.2 " + TranslateIncubation("Step_2_Background"), Font12blue);
                doc.Add(para29);
                doc.Add(new Chunk("\n"));
                Paragraph para31 = new Paragraph("2.3.2.1 " + TranslateIncubation("Step_2_Backgroundsub"), Font12blue);
                doc.Add(para31);
                doc.Add(new Chunk("\n"));
                Paragraph para30 = new Paragraph(objTB_INCUBATION_APPLICATION.Background, Font12black);
                doc.Add(para30);
                doc.Add(new Chunk("\n"));
                Paragraph para32 = new Paragraph("2.3.2.2 " + TranslateIncubation("Step_2_pilot"), Font12blue);
                doc.Add(para32);
                doc.Add(new Chunk("\n"));
                Paragraph para33 = new Paragraph(objTB_INCUBATION_APPLICATION.Pilot_Work_Done, Font12black);

                doc.Add(para33);
                doc.Add(new Chunk("\n"));
                Paragraph para34 = new Paragraph("2.3.2.3 " + TranslateIncubation("Step_4_Funding_Status"), Font12blue);
                doc.Add(para34);
                doc.Add(new Chunk("\n"));
                Paragraph para35 = new Paragraph(TranslateIncubation("Step_4_Funding_Statussub"), Font12blue);
                doc.Add(para35);
                doc.Add(new Chunk("\n"));
                List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbcontext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID).ToList();
                foreach (TB_APPLICATION_FUNDING_STATUS obj in objTB_APPLICATION_FUNDING_STATUS)
                {
                    PdfPTable table1 = new PdfPTable(2);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                    table1.TotalWidth = 500f;
                    table1.LockedWidth = true;
                    float[] width = new float[] { 250f, 250f };
                    table1.SetWidths(width);
                    table1.AddCell(new Paragraph(TranslateIncubation("Step3_Date"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_4_Name_of_Programme"), Font12blue));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(obj.Date.HasValue ? obj.Date.Value.ToString("MMM-yyyy") : "", Font12black));
                    table1.AddCell(new Phrase(obj.Programme_Name.ToString(), Font12black));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_2_Application_status"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_4_Funding_Status"), Font12blue));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(obj.Application_Status, Font12black));

                    table1.AddCell(new Phrase(obj.Funding_Status.ToString(), Font12black));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_2_expendiuture_covered"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_2_Currency"), Font12blue));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(obj.Expenditure_Nature.ToString(), Font12black));

                    table1.AddCell(new Phrase(obj.Currency.ToString(), Font12black));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_2_amount_received"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateIncubation("Step_2_Maximum_amount"), Font12blue));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(""));
                    table1.AddCell(new Phrase(obj.Amount_Received.ToString(), Font12black));

                    table1.AddCell(new Phrase(obj.Maximum_Amount.ToString(), Font12black));
                    doc.Add(table1);
                    doc.Add(new Chunk("\n"));
                }

                //Paragraph para36 = new Paragraph("2.3.2.4 Additional Information", Font12blue);
                Paragraph para36 = new Paragraph("2.3.2.4 " + TranslateIncubation("Step_2_additionalinfo"), Font12blue);
                doc.Add(para36);
                doc.Add(new Chunk("\n"));
                Paragraph para37 = new Paragraph(objTB_INCUBATION_APPLICATION.Additional_Information, Font12black);
                doc.Add(para37);
                doc.Add(new Chunk("\n"));
                //Paragraph para38 = new Paragraph("2.4 Implementation", Font15Head);
                Paragraph para38 = new Paragraph("2.4 " + TranslateIncubation("Step_2_Implementation"), Font15Head);
                doc.Add(para38);
                //Paragraph para39 = new Paragraph("(A summary of your company’s vision, mission and positioning.) ", Font12blue);
                //doc.Add(para39);
                doc.Add(new Chunk("\n"));
                Paragraph para40 = new Paragraph("2.4.1 " + TranslateIncubation("Step_2_proposed_products"), Font12blue);
                doc.Add(para40);
                doc.Add(new Chunk("\n"));
                Paragraph para41 = new Paragraph(objTB_INCUBATION_APPLICATION.Proposed_Products, Font12black);
                doc.Add(para41);
                doc.Add(new Chunk("\n"));
                Paragraph para42 = new Paragraph("2.4.2 " + TranslateIncubation("Step_2_target_market"), Font12blue);
                doc.Add(para42);
                doc.Add(new Chunk("\n"));
                Paragraph para43 = new Paragraph(objTB_INCUBATION_APPLICATION.Target_Market, Font12black);
                doc.Add(para43);
                doc.Add(new Chunk("\n"));
                Paragraph para44 = new Paragraph("2.4.3 " + TranslateIncubation("Step_2_competition_analysis"), Font12blue);
                doc.Add(para44);
                doc.Add(new Chunk("\n"));
                Paragraph para45 = new Paragraph(objTB_INCUBATION_APPLICATION.Competition_Analysis, Font12black);
                doc.Add(para45);
                doc.Add(new Chunk("\n"));
                Paragraph para46 = new Paragraph("2.4.4 " + TranslateIncubation("Step_2_Revenue_model"), Font12blue);
                doc.Add(para46);
                doc.Add(new Chunk("\n"));
                Paragraph para47 = new Paragraph(objTB_INCUBATION_APPLICATION.Revenus_Model, Font12black);
                doc.Add(para47);
                doc.Add(new Chunk("\n"));
                Paragraph para48 = new Paragraph("2.4.5 Exit Strategy (If applicable)", Font12blue);
                doc.Add(para48);
                doc.Add(new Chunk("\n"));
                Paragraph para49 = new Paragraph(objTB_INCUBATION_APPLICATION.Exit_Strategy, Font12black);
                doc.Add(para49);
                doc.Add(new Chunk("\n"));

                Paragraph para50 = new Paragraph(" 2.4.6 " + TranslateIncubation("Step_2_Business_model"), Font12blue);
                doc.Add(para50);
                doc.Add(new Chunk("\n"));

                PdfPTable table4 = new PdfPTable(2);
                table4.DefaultCell.Border = Rectangle.NO_BORDER;
                table4.TotalWidth = 500f;
                table4.LockedWidth = true;
                float[] widths1 = new float[] { 150f, 350f };
                table4.SetWidths(widths1);
                table4.AddCell(new Paragraph(TranslateIncubation("Step_2_Milestone"), Font12blue));
                table4.AddCell(new Paragraph(TranslateIncubation("Step_2_Details"), Font12blue));
                doc.Add(new Chunk("\n"));
                table4.AddCell(new Paragraph(TranslateIncubation("Step_2_Month_1"), Font12blue));

                table4.AddCell(new Phrase(objTB_INCUBATION_APPLICATION.First_6_Months_Milestone, Font12black));
                table4.AddCell(new Paragraph(TranslateIncubation("Step_2_Month_2"), Font12blue));

                table4.AddCell(new Phrase(objTB_INCUBATION_APPLICATION.Second_6_Months_Milestone, Font12black));
                table4.AddCell(new Paragraph(TranslateIncubation("Step_2_Month_3"), Font12blue));

                table4.AddCell(new Phrase(objTB_INCUBATION_APPLICATION.Third_6_Months_Milestone, Font12black));
                table4.AddCell(new Paragraph(TranslateIncubation("Step_2_Month_4"), Font12blue));

                table4.AddCell(new Phrase(objTB_INCUBATION_APPLICATION.Forth_6_Months_Milestone, Font12black));

                doc.Add(table4);
                doc.Add(new Chunk("\n"));

                doc.Add(new Chunk("\n"));
                Paragraph para61 = new Paragraph("2.4.7 " + TranslateIncubation("Step_2_Resubmission"), Font12blue);
                doc.Add(para61);
                doc.Add(new Chunk("\n"));

                if (objTB_INCUBATION_APPLICATION.Resubmission.HasValue)
                {
                    if (objTB_INCUBATION_APPLICATION.Resubmission == true)
                    {
                        Paragraph para62 = new Paragraph("Yes", Font12black);
                        doc.Add(para62);
                        doc.Add(new Chunk("\n"));
                        //Paragraph para62_1 = new Paragraph(objTB_INCUBATION_APPLICATION.Resubmission_Project_Reference, Font12black);
                        //doc.Add(para62_1);
                        // doc.Add(new Chunk("\n"));

                    }
                    else
                    {
                        Paragraph para62 = new Paragraph("No", Font12black);
                        doc.Add(para62);
                        doc.Add(new Chunk("\n"));
                    }
                }
                Paragraph para64 = new Paragraph("2.4.8 " + TranslateIncubation("Step_2_Main_Difference"), Font12blue);
                doc.Add(para64);
                doc.Add(new Chunk("\n"));
                if (objTB_INCUBATION_APPLICATION.Resubmission.HasValue)
                {
                    if (objTB_INCUBATION_APPLICATION.Resubmission == true)
                    {
                        Paragraph para63 = new Paragraph(objTB_INCUBATION_APPLICATION.Resubmission_Main_Differences, Font12black);
                        doc.Add(para63);
                        doc.Add(new Chunk("\n"));

                    }
                    else
                    {
                        Paragraph para63_1 = new Paragraph("NA", Font12black);
                        doc.Add(para63_1);
                        doc.Add(new Chunk("\n"));
                    }
                }
                Paragraph para65 = new Paragraph("2.5 " + TranslateIncubation("Step_2_Classification_of_Company"), Font15Head);
                doc.Add(para65);
                doc.Add(new Chunk("\n"));
                Paragraph para66 = new Paragraph("2.5.1 " + TranslateIncubation("step_2_Company_Type"), Font12blue);
                doc.Add(para66);
                doc.Add(new Chunk("\n"));
                if (!string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Company_Type))
                {
                    string companyType = GetCompanyTypeByValue(objTB_INCUBATION_APPLICATION.Company_Type);
                    if (objTB_INCUBATION_APPLICATION.Company_Type.Trim().ToLower() == "other")
                    {
                        Paragraph para67 = new Paragraph(companyType + " : " + objTB_INCUBATION_APPLICATION.Other_Company_Type, Font12black);
                        doc.Add(para67);
                    }
                    else
                    {
                        Paragraph para67 = new Paragraph(companyType, Font12black);
                        doc.Add(para67);
                    }
                    doc.Add(new Chunk("\n"));


                }
                Paragraph para68 = new Paragraph("2.5.2 Business Area", Font12blue);
                doc.Add(para68);
                doc.Add(new Chunk("\n"));

                if (!string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Business_Area))
                {
                    string businessArea = GetBusinessAreaByValue(objTB_INCUBATION_APPLICATION.Business_Area);
                    if (objTB_INCUBATION_APPLICATION.Business_Area.Trim().ToLower() == "others")
                    {
                        doc.Add(new Paragraph(businessArea + " : " + objTB_INCUBATION_APPLICATION.Other_Bussiness_Area, Font12black));
                    }
                    else
                    {
                        doc.Add(new Paragraph(businessArea, Font12black));
                    }

                    doc.Add(new Chunk("\n"));
                }
                Paragraph para72 = new Paragraph("2.5.3 Positioning", Font12blue);
                doc.Add(para72);
                doc.Add(new Chunk("\n"));
                Paragraph para73 = new Paragraph(objTB_INCUBATION_APPLICATION.Positioning, Font12black);
                doc.Add(para73);
                doc.Add(new Chunk("\n"));
                Paragraph para74 = new Paragraph(TranslateIncubation("Step2_Others") + " " + objTB_INCUBATION_APPLICATION.Other_Positioning, Font12black);
                doc.Add(para74);
                doc.Add(new Chunk("\n"));
                Paragraph para100 = new Paragraph(TranslateIncubation("Step_2_Management") + ": " + objTB_INCUBATION_APPLICATION.Management_Positioning, Font12black);
                doc.Add(para100);
                doc.Add(new Chunk("\n"));
                Paragraph para75 = new Paragraph("2.5.4 " + TranslateIncubation("Step_2_Other_Attributes"), Font12blue);
                doc.Add(para75);
                doc.Add(new Chunk("\n"));
                Paragraph para76 = new Paragraph(objTB_INCUBATION_APPLICATION.Other_Attributes, Font12black);
                doc.Add(para76);
                doc.Add(new Chunk("\n"));
                Paragraph para77 = new Paragraph("2.5.5 Please indicate your preferred track of incubation", Font12blue);
                doc.Add(para77);
                doc.Add(new Chunk("\n"));
                Paragraph para78 = new Paragraph(objTB_INCUBATION_APPLICATION.Preferred_Track, Font12black);
                doc.Add(para78);
                doc.Add(new Chunk("\n"));
                //Paragraph para79 = new Paragraph("Note: (i) Allocation of office space for on-site incubatee are subject to availability. (ii) On-site incubatees should meet the 40%  monthly utilisation rate in any 6 months.", Font12blue);

                Paragraph para79 = new Paragraph(TranslateIncubation("Step_2_5_5_Note"), Font12blue);
                doc.Add(para79);
                doc.Add(new Chunk("\n"));
                Paragraph para80 = new Paragraph("3. " + TranslateIncubation("Step_3_Company_ownership"), Font18blue);
                para80.Alignment = Element.ALIGN_CENTER;
                doc.Add(para80);
                doc.Add(new Chunk("\n"));
                Paragraph para81 = new Paragraph("3.1 " + TranslateIncubation("Step_3_Comapny_ownership_sub1"), Font15Head);


                doc.Add(para81);
                doc.Add(new Chunk("\n"));
                Paragraph para82 = new Paragraph("3.1.1 " + TranslateIncubation("Step_3_Comapny_ownership_sub2"), Font12blue);


                doc.Add(para82);
                doc.Add(new Chunk("\n"));
                Paragraph para84 = new Paragraph(objTB_INCUBATION_APPLICATION.Company_Ownership_Structure, Font12black);


                doc.Add(para84);
                doc.Add(new Chunk("\n"));

                Paragraph para85 = new Paragraph("3.1.2 " + TranslateIncubation("Step_3_Comapny_ownership_sub2"), Font12blue);
                doc.Add(para85);
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID && x.Attachment_Type == "Company_Ownership_Structure").ToList();
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT)
                {
                    Paragraph para86 = new Paragraph(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1), Font12black);
                    doc.Add(para86);
                    doc.Add(new Chunk("\n"));
                }

                doc.Add(new Chunk("\n"));
                Paragraph para87 = new Paragraph("3.2 " + TranslateIncubation("Step_3_Company_Core_Members"), Font15Head);


                doc.Add(para87);
                doc.Add(new Chunk("\n"));
                Paragraph para88 = new Paragraph("3.2.1 " + TranslateIncubation("Step_3_Profiles"), Font12blue);

                doc.Add(para88);
                doc.Add(new Chunk("\n"));

                List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbcontext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID).ToList();
                foreach (TB_APPLICATION_COMPANY_CORE_MEMBER obj in objTB_APPLICATION_COMPANY_CORE_MEMBER)
                {
                    PdfPTable table2 = new PdfPTable(2);
                    table2.DefaultCell.Border = Rectangle.NO_BORDER;
                    table2.TotalWidth = 500f;
                    table2.LockedWidth = true;
                    float[] width = new float[] { 250f, 250f };
                    table2.SetWidths(width);
                    table2.AddCell(new Paragraph(TranslateIncubation("Step_3_Name"), Font12blue));
                    table2.AddCell(new Paragraph(TranslateIncubation("Step_3_Postitions"), Font12blue));
                    table2.AddCell("");
                    table2.AddCell("");
                    table2.AddCell(new Phrase(obj.Name.ToString(), Font12black));

                    table2.AddCell(new Phrase(obj.Position.ToString(), Font12black));
                    table2.AddCell(new Paragraph(TranslateIncubation("Step_3_HKID"), Font12blue));
                    table2.AddCell("");


                    table2.AddCell(new Phrase(obj.Masked_HKID, Font12black));

                    table2.AddCell("");
                    PdfPCell cell = new PdfPCell(new Phrase(TranslateIncubation("Step_3_Academic_and_Professionals").Replace("<i>", "").Replace("</i>", ""), Font12blue));
                    cell.Colspan = 2;
                    cell.Border = Rectangle.NO_BORDER;
                    table2.AddCell(cell);
                    PdfPCell cell1 = new PdfPCell(new Phrase(obj.CoreMember_Profile, Font12black));
                    cell1.Colspan = 2;
                    cell1.Border = Rectangle.NO_BORDER;
                    table2.AddCell(cell1);

                    doc.Add(table2);
                    doc.Add(new Chunk("\n"));
                }

                Paragraph para88_1 = new Paragraph("3.2.2 " + TranslateIncubation("Step_3_Major_Partner_Profiles"), Font12blue);
                doc.Add(para88_1);
                doc.Add(new Chunk("\n"));

                Paragraph para88_2 = new Paragraph(objTB_INCUBATION_APPLICATION.Major_Partners_Profiles, Font12black);
                doc.Add(para88_2);
                doc.Add(new Chunk("\n"));

                Paragraph para89 = new Paragraph("3.3 " + TranslateIncubation("Step_3_Expenditure"), Font15Head);

                doc.Add(para89);
                doc.Add(new Chunk("\n"));
                Paragraph para91 = new Paragraph("3.3.1 " + TranslateIncubation("Step_3_Manpower_Distribution"), Font12blue);

                doc.Add(para91);
                doc.Add(new Chunk("\n"));
                Paragraph para90 = new Paragraph(objTB_INCUBATION_APPLICATION.Manpower_Distribution, Font12black);

                doc.Add(para90);
                doc.Add(new Chunk("\n"));
                Paragraph para92 = new Paragraph("3.3.2 " + TranslateIncubation("Step_3_Equipment"), Font12blue);

                doc.Add(para92);
                doc.Add(new Chunk("\n"));
                Paragraph para93 = new Paragraph(objTB_INCUBATION_APPLICATION.Equipment_Distribution, Font12black);

                doc.Add(para93);
                doc.Add(new Chunk("\n"));
                Paragraph para94 = new Paragraph("3.3.3 " + TranslateIncubation("Step_3_Other_cost"), Font12blue);

                doc.Add(para94);
                doc.Add(new Chunk("\n"));
                Paragraph para95 = new Paragraph(objTB_INCUBATION_APPLICATION.Other_Direct_Costs, Font12black);

                doc.Add(para95);
                doc.Add(new Chunk("\n"));
                Paragraph para96 = new Paragraph("3.3.4 " + TranslateIncubation("Step_3_Forest_Income"), Font12blue);

                doc.Add(para96);
                doc.Add(new Chunk("\n"));
                Paragraph para97 = new Paragraph(objTB_INCUBATION_APPLICATION.Forecast_Income, Font12black);

                doc.Add(para97);
                doc.Add(new Chunk("\n"));
                Paragraph para98 = new Paragraph("4. " + TranslateIncubation("Step_4_CONTACT_DETAILS"), Font18blue);
                para98.Alignment = Element.ALIGN_CENTER;

                doc.Add(para98);
                doc.Add(new Chunk("\n"));
                List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbcontext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID).ToList();
                int i = 1;
                foreach (TB_APPLICATION_CONTACT_DETAIL obj in objTB_APPLICATION_CONTACT_DETAIL)
                {
                    if (i == 1)
                    {
                        Paragraph para99 = new Paragraph("4." + i + " " + TranslateIncubation("Step_4_Contact_Person") + " " + i + TranslateIncubation("Step_4_Principal_Applicant"), Font15Head);
                        doc.Add(para99);
                        doc.Add(new Chunk("\n"));
                    }
                    else
                    {
                        Paragraph para99 = new Paragraph("4." + i + " " + TranslateIncubation("Step_4_Contact_Person") + " " + i, Font15Head);
                        doc.Add(para99);
                        doc.Add(new Chunk("\n"));

                    }
                    i += 1;

                    PdfPTable table3 = new PdfPTable(3);
                    table3.DefaultCell.Border = Rectangle.NO_BORDER;
                    table3.TotalWidth = 450f;
                    table3.LockedWidth = true;
                    float[] widths3 = new float[] { 150f, 150f, 150f };
                    table3.SetWidths(widths3);
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Salution"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Last_Name"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_First_Name"), Font12blue));
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell(new Phrase(obj.Salutation.ToString(), Font12black));
                    table3.AddCell(new Phrase(obj.Last_Name_Eng.ToString(), Font12black));
                    table3.AddCell(new Phrase(obj.First_Name_Eng.ToString(), Font12black));
                    PdfPCell cell = new PdfPCell(new Phrase(TranslateIncubation("Step_4_Position"), Font12blue));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Colspan = 3;
                    table3.AddCell(cell);

                    PdfPCell cell1 = new PdfPCell(new Phrase(obj.Position, Font12black));
                    cell1.Border = Rectangle.NO_BORDER;

                    cell1.Colspan = 3;
                    table3.AddCell(cell1);

                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Contact_Home"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Contact_Office"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Contact_No_Mobile"), Font12blue));
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell(new Phrase(obj.Contact_No_Home, Font12black));
                    table3.AddCell(new Phrase(obj.Contact_No_Office, Font12black));
                    table3.AddCell(new Phrase(obj.Contact_No_Mobile, Font12black));

                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Fax"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Email"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateIncubation("Step_4_Nationality"), Font12blue));

                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell(new Phrase(obj.Fax, Font12black));
                    table3.AddCell(new Phrase(obj.Email, Font12black));
                    table3.AddCell(new Phrase(obj.Nationality, Font12black));

                    PdfPCell cell2 = new PdfPCell(new Phrase(TranslateIncubation("Step_4_Mailing_Address"), Font12blue));

                    cell2.Colspan = 3;
                    cell2.Border = Rectangle.NO_BORDER;
                    table3.AddCell(cell2);

                    PdfPCell cell3 = new PdfPCell(new Phrase(obj.Mailing_Address, Font12black));
                    cell3.Colspan = 3;
                    cell3.Border = Rectangle.NO_BORDER;
                    table3.AddCell(cell3);

                    doc.Add(table3);
                    doc.Add(new Chunk("\n"));
                }
                Paragraph para_1 = new Paragraph("5. " + TranslateIncubation("ATTACHMENT_Title"), Font18blue);
                para_1.Alignment = Element.ALIGN_CENTER;

                doc.Add(para_1);
                doc.Add(new Chunk("\n"));
                Paragraph para_3 = new Paragraph("5.1 " + TranslateIncubation("BRCOPY"), Font12blue);
                doc.Add(para_3);
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_br = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID && x.Attachment_Type == "BR_COPY").ToList();
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_br)
                {
                    Paragraph para_2 = new Paragraph(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1), Font12black);
                    doc.Add(para_2);
                    doc.Add(new Chunk("\n"));
                }
                Paragraph para_4 = new Paragraph("5.2 " + TranslateIncubation("CompanyAnnualReturn"), Font12blue);
                doc.Add(para_4);
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_car = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID && x.Attachment_Type == "Company_Annual_Return").ToList();
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_car)
                {
                    Paragraph para_5 = new Paragraph(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1), Font12black);
                    doc.Add(para_5);
                    doc.Add(new Chunk("\n"));
                }
                Paragraph para_6 = new Paragraph("5.3 " + TranslateIncubation("CompanyAnnualReturn"), Font12blue);
                doc.Add(para_6);
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_videoclip = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID && x.Attachment_Type == "Video_Clip").ToList();
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_videoclip)
                {
                    Paragraph para_7 = new Paragraph(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1), Font12black);
                    doc.Add(para_7);
                    doc.Add(new Chunk("\n"));
                }
                //string pSlide = TranslateIncubation("PresentationSlide");
                //string[] pSlideSplit = pSlide.Split(' ');
                //if (pSlideSplit.Length > 2)
                //{
                //    pSlide = pSlideSplit[0] + " " + pSlideSplit[1];
                //}
                Paragraph para_8 = new Paragraph("5.4 " + TranslateIncubation("PresentationSlide"), Font12blue);
                doc.Add(para_8);
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_preslide = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID && x.Attachment_Type == "Presentation_Slide").ToList();
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_preslide)
                {
                    Paragraph para_9 = new Paragraph(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1), Font12black);
                    doc.Add(para_9);
                    doc.Add(new Chunk("\n"));
                }
                Paragraph para_10 = new Paragraph("5.5 " + TranslateIncubation("OtherAttachment"), Font12blue);
                doc.Add(para_10);
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_otherattach = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_INCUBATION_APPLICATION.Incubation_ID && x.Attachment_Type == "Other_Attachment").ToList();
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_otherattach)
                {
                    Paragraph para_11 = new Paragraph(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1), Font12black);
                    doc.Add(para_11);
                    doc.Add(new Chunk("\n"));
                }

                doc.Add(new Chunk("\n"));

                Paragraph para_DECLARATION = new Paragraph(TranslateIncubation("Declaration_Title"), Font18blue);
                para_DECLARATION.Alignment = Element.ALIGN_CENTER;
                doc.Add(para_DECLARATION);
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(objTB_INCUBATION_APPLICATION.Declaration == true ? "Yes" : "No", Font12black));
                Paragraph para_DECLARATION1 = new Paragraph(TranslateIncubation("Step_5_have_read"), Font12Green);
                doc.Add(para_DECLARATION1);
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration1"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration2"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration3"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration4"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration5"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration7"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_5_Declaration8"), Font12blue));
                //doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("6.8 " + TranslateIncubation("Step_5_Declaration1"), Font12blue));
                //doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("6.9 " + TranslateIncubation("Step_5_Declaration1"), Font12blue));
                //doc.Add(new Chunk("\n"));
                doc.Add(new Chunk("\n"));

                PdfPTable tableapp = new PdfPTable(2);
                tableapp.DefaultCell.Border = Rectangle.NO_BORDER;
                tableapp.TotalWidth = 500f;
                tableapp.LockedWidth = true;
                float[] widths12 = new float[] { 250f, 250f };
                tableapp.SetWidths(widths12);
                tableapp.AddCell(new Paragraph(TranslateIncubation("Step_5_Full_Name"), Font12blue));
                tableapp.AddCell(new Paragraph(TranslateIncubation("Step_5_Title_Principal_Applicant"), Font12blue));
                tableapp.AddCell("");
                tableapp.AddCell("");
                tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Principal_Full_Name) ? objTB_INCUBATION_APPLICATION.Principal_Full_Name : "", Font12black));
                tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Principal_Position_Title) ? objTB_INCUBATION_APPLICATION.Principal_Position_Title : "", Font12black));
                doc.Add(tableapp);
                doc.Add(new Chunk("\n"));


                Paragraph para_pinfo = new Paragraph(TranslateIncubation("Step_6_PERSONAL_INFORMATION"), Font18blue);
                //para_pinfo.Alignment = Element.ALIGN_CENTER;
                doc.Add(para_pinfo);
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_PERSONAL_INFORMATION_1"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Purpose"), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Purpose_1"), Font12blue));
                doc.Add(new Chunk("\n"));

                Paragraph pin1 = new Paragraph(
                    "• " + TranslateIncubation("Step_6_Purpose_2") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Purpose_3") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Purpose_4") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Purpose_5") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Purpose_6") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Purpose_7") + "\n\n"
                , Font12blue);
                pin1.IndentationLeft = 30;
                doc.Add(pin1);
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Purpose_8"), Font12blue));


                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Transfer_data"), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Transfer_data_1"), Font12blue));
                doc.Add(new Chunk("\n"));

                Paragraph pin2 = new Paragraph(
                    "• " + TranslateIncubation("Step_6_Transfer_data_2") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Transfer_data_3") + "\n\n" +
                    "• " + TranslateIncubation("Step_6_Transfer_data_4") + "\n\n"
                , Font12blue);
                pin2.IndentationLeft = 30;
                doc.Add(pin2);
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Transfer_data_note"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Direct_marketing"), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Direct_marketing_1"), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(TranslateIncubation("Step_6_cyberport") +
                    " " + TranslateIncubation("Step_6_cyberport_1"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_cyberport_2"), Font12blue));
                doc.Add(new Chunk("\n"));


                doc.Add(new Paragraph(TranslateIncubation("Step_6_Privacy"), Font15Head));
                doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("Please see our Privacy Policy Statement at http://www.cyberport.hk/en/privacy_policy for our general policy and practices in respect of our collection and use of personal data.", Font12blue));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Privacy_2"), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(TranslateIncubation("Step_6_Access"), Font15Head));
                doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph(@"You have the right to request access to, and correction of, Your Data held by us. We may charge a reasonable fee for administering and processing your data access request. If you need to check whether we hold Your Data or if you wish to have access to, correct any of Your Data which is inaccurate, please write via e-mail to our Data Protection Officer at dpo@cyberport.hk or via mail to Units 1102-04, Level 11, Cyberport 2, 100 Cyberport Road, Hong Kong.", Font12blue));
                doc.Add(new Paragraph(TranslateIncubation("Step_6_Access_1"), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(TranslateIncubation("STEP_6_STATEMENT"), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Chunk("\n"));
                if (objTB_INCUBATION_APPLICATION.Have_Read_Statement.HasValue)
                {
                    doc.Add(new Paragraph(objTB_INCUBATION_APPLICATION.Have_Read_Statement == true ? "Yes" : "No", Font12black));
                }
                doc.Add(new Paragraph(TranslateIncubation("step_6_personal_information_collection"), Font12Green));

                doc.Add(new Chunk("\n"));
                if (objTB_INCUBATION_APPLICATION.Marketing_Information.HasValue)
                {
                    doc.Add(new Paragraph(objTB_INCUBATION_APPLICATION.Marketing_Information == true ? "Yes" : "No", Font12black));
                }
                doc.Add(new Paragraph(TranslateIncubation("step_6_marketing"), Font12Green));
                doc.Add(new Chunk("\n"));

            }
        }

        private void ShowPdf(byte[] strS)
        {
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "Incubation Application.pdf");

            System.Web.HttpContext.Current.Response.BinaryWrite(strS);
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.Clear();
        }

        //CCMF PDF Generation
        private void CreatePDFCCMF(string ApplicationNumber, ref Document doc)
        {
            using (var dbcontext = new CyberportEMS_EDM())
            {

                //TB_CCMF_APPLICATION objTB_INCUBATION_APPLICATION = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == ApplicationNumber);
                TB_CCMF_APPLICATION objTB_TB_CCMF_APPLICATION = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == ApplicationNumber && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
                TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbcontext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == objTB_TB_CCMF_APPLICATION.Programme_ID);

                PdfPTable table_2 = new PdfPTable(2);
                table_2.DefaultCell.Border = Rectangle.NO_BORDER;
                table_2.TotalWidth = 500f;
                table_2.LockedWidth = true;
                float[] widths2 = new float[] { 250f, 250f };
                table_2.SetWidths(widths2);
                table_2.AddCell(new Paragraph("Application Name:", Font12blue));
                table_2.AddCell(new Paragraph(objTB_PROGRAMME_INTAKE.Programme_Name, Font12blue));
                table_2.AddCell(new Paragraph("Intake number:", Font12blue));

                table_2.AddCell(new Paragraph(objTB_PROGRAMME_INTAKE.Intake_Number.ToString(), Font12blue));
                table_2.AddCell(new Paragraph("Application Number:", Font12blue));
                table_2.AddCell(new Paragraph(objTB_TB_CCMF_APPLICATION.Application_Number, Font12blue));
                table_2.AddCell(new Paragraph("Last Submitted Date:", Font12blue));
                table_2.AddCell(new Paragraph(objTB_TB_CCMF_APPLICATION.Last_Submitted.ToString("dd MMM yyyy"), Font12blue));
                doc.Add(table_2);

                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                doc.Add(p);
                Paragraph para = new Paragraph(TranslateCCMF("Instruction_0"), Font12Green);
                para.Alignment = Element.ALIGN_LEFT;
                doc.Add(para);
                doc.Add(new Chunk("\n"));
                Paragraph para1 = new Paragraph("1. " + TranslateCCMF("Instruction_1"), Font12blueLight);
                doc.Add(para1);
                doc.Add(new Chunk("\n"));
                Paragraph para2 = new Paragraph("• " + TranslateCCMF("Instruction_1_1"), Font12blueLight);

                para2.IndentationLeft = 30;
                doc.Add(para2);
                doc.Add(new Chunk("\n"));
                Paragraph para3 = new Paragraph("• " + TranslateCCMF("Instruction_1_2"), Font12blueLight);
                para3.IndentationLeft = 30;
                doc.Add(para3);
                doc.Add(new Chunk("\n"));
                Paragraph para4 = new Paragraph("• " + TranslateCCMF("Instruction_1_3"), Font12blueLight);
                para4.IndentationLeft = 30;
                doc.Add(para4);
                doc.Add(new Chunk("\n"));
                Paragraph para5 = new Paragraph("• " + TranslateCCMF("Instruction_1_4"), Font12blueLight);
                para5.IndentationLeft = 30;
                doc.Add(para5);
                doc.Add(new Chunk("\n"));
                //Paragraph para6 = new Paragraph("2. To read the  CCMF Guides and Notes for the Applicants (ENC.RF.015)  before filling in this Application Form.", Font12blueLight);
                Paragraph para6 = new Paragraph("2. " + TranslateCCMF("Instruction_2"), Font12blueLight);
                doc.Add(para6);
                doc.Add(new Chunk("\n"));
                Paragraph para7 = new Paragraph("3. " + TranslateCCMF("Instruction_3"), Font12blueLight);
                doc.Add(para7);
                doc.Add(new Chunk("\n"));
                //Paragraph para8 = new Paragraph("4. ONE email address for EACH CCMF Programme application , and that email address would receive all the emails in respect of the submitted application.  If you have another project to apply for CCMF Programme, please use a separate email address for that application.", Font12blueLight);
                Paragraph para8 = new Paragraph("4. " + TranslateCCMF("Instruction_4"), Font12blueLight);
                doc.Add(para8);
                doc.Add(new Chunk("\n"));
                //Paragraph para9 = new Paragraph("5. Please prepare and upload the requested supporting documents in Section 6 including, but not limited, to the following: ", Font12blueLight);
                Paragraph para9 = new Paragraph("5. " + TranslateCCMF("Instruction_4_1"), Font12blueLight);
                doc.Add(para9);
                doc.Add(new Chunk("\n"));
                //Paragraph para10 = new Paragraph("• Company documents (e.g. Business Registration, Certificate of Incorporation, etc.) (if applicable)", Font12blueLight);
                //para10.IndentationLeft = 30;
                //doc.Add(para10);
                //doc.Add(new Chunk("\n"));

                //Paragraph paraio = new Paragraph("• Identity documents (e.g. copy of Hong Kong Identity Card, Passport, Student Identity Card, Graduate Certificate etc.) (if applicable))", Font12blueLight);
                //paraio.IndentationLeft = 30;
                //doc.Add(paraio);
                //doc.Add(new Chunk("\n"));

                //Paragraph para11 = new Paragraph("• Other supplementary information such as Corporate Structure / Organisation Chart, Curriculum Vitae of team members, pictures, diagrams and business proposal, etc. for illustration of your project (if any)", Font12blueLight);
                //para11.IndentationLeft = 30;
                //doc.Add(para11);
                //doc.Add(new Chunk("\n"));
                //Paragraph para12 = new Paragraph("• You are recommended to prepare a 1-minute video clip/presentation file/slide show to illustrate your project. Kindly provide the link if there is any.", Font12blueLight);
                //para12.IndentationLeft = 30;
                //doc.Add(para12);
                //doc.Add(new Chunk("\n"));
                //Paragraph para13 = new Paragraph("6. For enquiries, please call (852) 3166 3900 or email to ccmf_enquiry@cyberport.hk.", Font12blueLight);
                Paragraph para13 = new Paragraph("6. " + TranslateCCMF("Instruction_5"), Font12blueLight);
                doc.Add(para13);
                doc.Add(new Chunk("\n"));
                Paragraph para14 = new Paragraph(TranslateCCMF("Instruction_7"), Font12blueLight);
                //Paragraph para14 = new Paragraph("Remarks: This application form has been translated into Chinese for reference only. In case of discrepancy, the English version shall prevail.", Font12blueLight);
                doc.Add(para14);
                doc.Add(new Chunk("\n"));
                //Paragraph para15 = new Paragraph("Hong Kong Cyberport Management Company Limited.", Font12blueLight);
                Paragraph para15 = new Paragraph(TranslateCCMF("Instruction_8"), Font12blueLight);
                doc.Add(para15);
                doc.Add(new Chunk("\n"));

                Paragraph paraType = new Paragraph("1. " + TranslateCCMF("Step1_Types_Of_CCMF"), Font18blue);
                paraType.Alignment = Element.ALIGN_CENTER;
                doc.Add(paraType);
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph("1.1 " + TranslateCCMF("Step1_CCMF_Selection"), Font15Head));
                doc.Add(new Chunk("\n"));
                if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("hongkong") && objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower().Contains("young entrepreneur"))
                {
                    //                   doc.Add(new Paragraph("Hong Kong Young Entrepreneur Programme", Font12black));
                    doc.Add(new Paragraph(TranslateCCMF("Hong_Kong_Young_Entrepreneur"), Font12black));
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("hongkong") && objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower().Contains("professional"))
                {
                    //doc.Add(new Paragraph("Professional Stream", Font12black));
                    doc.Add(new Paragraph(TranslateCCMF("Professional_Stream"), Font12black));
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("shenzhen"))
                {
                    doc.Add(new Paragraph("Cyberport Shenzhen - Hong Kong Young Entrepreneur Programme", Font12black));
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("guangdong"))
                {
                    doc.Add(new Paragraph("Cyberport Guangdong - Hong Kong Young Entrepreneur Programme", Font12black));
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower() == "cupp")
                {
                    doc.Add(new Paragraph("Cyberport University Partnership Programme", Font12black));
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains( "gbayep"))
                {
                    doc.Add(new Paragraph("Cyberport Greater Bay Area Young Entrepreneurship Programme Supported by CCMF", Font12black));
                }
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph("1.2 " + TranslateCCMF("Step_1_CCMF_app"), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(objTB_TB_CCMF_APPLICATION.CCMF_Application_Type + " Application", Font12black));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph("1.3 " + TranslateCCMF("Step1_Question1_3"), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(objTB_TB_CCMF_APPLICATION.Question1_3.HasValue ? (objTB_TB_CCMF_APPLICATION.Question1_3.Value == true ? "Yes" : "No") : "" + " Application", Font12black));
                doc.Add(new Chunk("\n"));
                doc.Add(new Chunk("\n"));

                doc.Add(new Chunk("\n"));


                Paragraph para16 = new Paragraph("2. " + TranslateCCMF("Step2_Heading"), Font18blue);
                para16.Alignment = Element.ALIGN_CENTER;

                doc.Add(para16);
                doc.Add(new Chunk("\n"));

                if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("hongkong") && objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower().Contains("young entrepreneur"))
                {
                    Paragraph para_16 = new Paragraph("2.1 " + TranslateCCMF("Hong_Kong_Programme") + "-" + TranslateCCMF("Hong_Kong_Young_Entrepreneur_Programme"), Font15Head);
                    doc.Add(para_16);
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("hongkong") && objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower().Contains("professional"))
                {
                    Paragraph para_16 = new Paragraph("2.1 " + TranslateCCMF("Hong_Kong_Programme") + "-" + TranslateCCMF("Professional_Stream"), Font15Head);
                    doc.Add(para_16);
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("crossborder"))
                {
                    Paragraph para_16 = new Paragraph("2.1 " + TranslateCCMF("Cross_Border_Programme_Supported_by_CCMF"), Font15Head);
                    doc.Add(para_16);
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("gbayep"))
                {
                    Paragraph para_16 = new Paragraph("2.1 " + TranslateCCMF("CCMF_GBAYEP_Header"), Font15Head);
                    doc.Add(para_16);
                }
                else
                {
                    Paragraph para_16 = new Paragraph("2.1 " + TranslateCCMF("CUPP_Header"), Font15Head);
                    doc.Add(para_16);
                }
                PdfPTable table = new PdfPTable(3);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 50f, 400f, 50f };
                table.SetWidths(widths);

                if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("hongkong"))
                {
                    if (objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower() == "professional" && objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "individual")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "hkpi", ref table, ref doc);
                    }
                    else if (objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower() == "professional" && objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "company")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "hkpc", ref table, ref doc);
                    }
                    else if (objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower() == "young entrepreneur" && objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "individual")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "hkyi", ref table, ref doc);
                    }
                    else if (objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower() == "young entrepreneur" && objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "company")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "hkyc", ref table, ref doc);
                    }

                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("crossborder"))
                {
                    if (objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "individual")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "cbi", ref table, ref doc);
                    }
                    else if (objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "company")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "cbc", ref table, ref doc);
                    }

                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("cupp"))
                {
                    if (objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "individual")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "cbui", ref table, ref doc);
                    }
                    else if (objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "company")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "cbuc", ref table, ref doc);
                    }

                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("gbayep"))
                {
                    if (objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "individual")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "ccmfgbayepi", ref table, ref doc);
                    }
                    else if (objTB_TB_CCMF_APPLICATION.CCMF_Application_Type.ToLower() == "company")
                    {
                        Switch2UI(objTB_TB_CCMF_APPLICATION, "ccmfgbayepc", ref table, ref doc);
                    }

                }
                doc.Add(table);
                doc.Add(new Chunk("\n"));

                //doc.Add(new Paragraph("Note: Applicants should make a true, full and accurate disclosure on information requested. The application can be treated as an ineligible application if applicant violates the rule of application. Cyberport has the absolute right to terminate the admitted project due to violation on CCMF rules while applying and within CCMF project period after admission.", Font12black));

                //doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("If an applicant of the same project applying for Cyberport Creative Micro Fund (CCMF) and Cyberport Incubation Programme (CIP) at the same intake, HKCMCL shall only consider the application of CIP without further notice.", Font12black));

                //doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("Application eligibility of CCMF Hong Kong Programme is stated in the CCMF Guides and Notes for Applicants – Hong Kong Programme.", Font12black));
                doc.Add(new Paragraph(TranslateCCMF("Step2_Note"), Font12black));
                doc.Add(new Chunk("\n"));

                Paragraph para17 = new Paragraph("3. " + TranslateCCMF("Step3_PROJECT_INFORMATION"), Font18blue);
                para17.Alignment = Element.ALIGN_CENTER;
                doc.Add(para17);
                doc.Add(new Chunk("\n"));
                Paragraph para18 = new Paragraph("3.1 " + TranslateCCMF("Step3_Project_Name"), Font15Head);

                doc.Add(para18);
                doc.Add(new Chunk("\n"));
                Chunk glue = new Chunk(new VerticalPositionMark());
                Paragraph para19 = new Paragraph(objTB_TB_CCMF_APPLICATION.Project_Name_Eng, Font12black);
                para19.Add(new Chunk(glue));
                para19.Add(objTB_TB_CCMF_APPLICATION.Project_Name_Chi);
                //para20.IndentationRight = 30;
                doc.Add(para19);
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph("3.2 " + TranslateCCMF("Step3_1b_CompanyName") + TranslateCCMF("Step3_1b_CompanyNameDetail"), Font15Head));
                doc.Add(new Paragraph(objTB_TB_CCMF_APPLICATION.Company_Name, Font12black));


                Paragraph para_yearEsta = new Paragraph("3.2a " + TranslateCCMF("Step3_1c_Establishment"), Font12blue);
                para_yearEsta.Add(new Chunk(glue));
                para_yearEsta.Add("3.2b " + TranslateCCMF("Step3_1d_CountryOFOrigin"));
                doc.Add(para_yearEsta);
                Paragraph para_yearEstaVal = new Paragraph(objTB_TB_CCMF_APPLICATION.Establishment_Year.HasValue ? objTB_TB_CCMF_APPLICATION.Establishment_Year.Value.ToString("dd-MMM-yyyy") : "", Font12black);

                para_yearEstaVal.Add(new Chunk(glue));
                para_yearEstaVal.Add(objTB_TB_CCMF_APPLICATION.Country_Of_Origin);
                doc.Add(para_yearEstaVal);
                doc.Add(new Chunk("\n"));

                Paragraph para_New2HK = new Paragraph("3.2c " + TranslateCCMF("Step3_1d_NewHK"), Font12blue);
                doc.Add(para_New2HK);
                Paragraph para_New2HKval = new Paragraph((objTB_TB_CCMF_APPLICATION.NEW_to_HK.HasValue ? (objTB_TB_CCMF_APPLICATION.NEW_to_HK.Value == true ? "Yes" : "No") : ""), Font12black);
                doc.Add(para_New2HKval);
                doc.Add(new Chunk("\n"));

                para_yearEsta.Add(new Chunk(glue));
                Paragraph para21 = new Paragraph("3.3 " + TranslateCCMF("Step3_Abstract"), Font15Head);
                //para21.Add(" (A summary of your project’s vision, mission and positioning.)", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#075CA9"))));
                doc.Add(para21);
                doc.Add(new Chunk("\n"));
                Paragraph para22 = new Paragraph("3.3.1 English", Font12blue);
                doc.Add(para22);
                doc.Add(new Chunk("\n"));
                Paragraph para23 = new Paragraph(objTB_TB_CCMF_APPLICATION.Abstract_Eng, Font12black);
                doc.Add(para23);
                doc.Add(new Chunk("\n"));
                Paragraph para24 = new Paragraph("3.3.2 Chinese", Font12blue);
                doc.Add(para24);
                doc.Add(new Chunk("\n"));
                Paragraph para25 = new Paragraph(objTB_TB_CCMF_APPLICATION.Abstract_Chi, Font12black);
                doc.Add(para25);
                doc.Add(new Chunk("\n"));
                Paragraph para26 = new Paragraph("3.4 " + TranslateCCMF("Step3_BusinessArea"), Font15Head);
                doc.Add(para26);
                doc.Add(new Chunk("\n"));
                //Paragraph para27 = new Paragraph("2.3.1 Objectives - the “why” of the company ", Font12blue);
                //doc.Add(para27);
                //doc.Add(new Chunk("\n"));
                if (!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Business_Area))
                {
                    string businessArea = GetBusinessAreaByValue(objTB_TB_CCMF_APPLICATION.Business_Area);
                    if (objTB_TB_CCMF_APPLICATION.Business_Area.Trim().ToLower() == "others")
                    {
                        Paragraph para28 = new Paragraph(businessArea + " : " + objTB_TB_CCMF_APPLICATION.Other_Business_Area, Font12black);
                        doc.Add(para28);
                    }
                    else
                    {
                        Paragraph para28 = new Paragraph(businessArea, Font12black);
                        doc.Add(para28);
                    }

                    doc.Add(new Chunk("\n"));
                }
                Paragraph para_26 = new Paragraph("3.5 " + TranslateCCMF("Step3_AnticipatedDate"), Font15Head);
                doc.Add(para_26);
                doc.Add(new Chunk("\n"));
                Chunk glue1 = new Chunk(new VerticalPositionMark());
                Paragraph para_20 = new Paragraph(TranslateCCMF("step3_Commencement"), Font12blue);

                para_20.Add(new Chunk(glue));
                para_20.Add(TranslateCCMF("Step3_Complete_Date"));
                doc.Add(para_20);
                Paragraph para_19 = new Paragraph(objTB_TB_CCMF_APPLICATION.Commencement_Date.HasValue ? objTB_TB_CCMF_APPLICATION.Commencement_Date.Value.ToString("dd-MMM-yyyy") : "", Font12black);

                para_19.Add(new Chunk(glue));
                para_19.Add(objTB_TB_CCMF_APPLICATION.Completion_Date.HasValue ? objTB_TB_CCMF_APPLICATION.Completion_Date.Value.ToString("dd-MMM-yyyy") : "");
                doc.Add(para_19);
                doc.Add(new Chunk("\n"));


                doc.Add(new Paragraph("3.6 " + TranslateCCMF("Step_3_Smart_Space"), Font15Head));
                doc.Add(new Chunk("\n"));
                string SmartSpace = objTB_TB_CCMF_APPLICATION.SmartSpace;
                doc.Add(new Paragraph(SmartSpace == "Other" ? TranslateCCMF("Step_3_Smart_Space_Others") : (SmartSpace == "SS8" ? TranslateCCMF("Step_3_Smart_Space_SS8") : SmartSpace), Font12black));
                doc.Add(new Chunk("\n"));

                Paragraph para20 = new Paragraph("4. " + TranslateCCMF("Step_4_Applicatin_Information"), Font18blue);
                para20.Alignment = Element.ALIGN_CENTER;
                doc.Add(para20);
                doc.Add(new Chunk("\n"));
                // Paragraph para_21 = new Paragraph("4.1(a) Project Management Team", Font15Head);
                Paragraph para_21 = new Paragraph("4.1(a) " + TranslateCCMF("Step_4_project_management"), Font15Head);

                doc.Add(para_21);
                doc.Add(new Chunk("\n"));

                Paragraph para34 = new Paragraph(TranslateCCMF("Step_4_project_managementsub"), Font10blue);
                doc.Add(para34);
                doc.Add(new Chunk("\n"));
                List<TB_APPLICATION_COMPANY_CORE_MEMBER> objTB_APPLICATION_COMPANY_CORE_MEMBER = dbcontext.TB_APPLICATION_COMPANY_CORE_MEMBER.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID).ToList();
                foreach (TB_APPLICATION_COMPANY_CORE_MEMBER obj in objTB_APPLICATION_COMPANY_CORE_MEMBER)
                {
                    PdfPTable table2 = new PdfPTable(2);
                    table2.DefaultCell.Border = Rectangle.NO_BORDER;
                    table2.TotalWidth = 500f;
                    table2.LockedWidth = true;
                    float[] widths123 = new float[] { 250f, 250f };
                    table2.SetWidths(widths123);
                    table2.AddCell(new Paragraph(TranslateCCMF("Step_3_Name"), Font12blue));
                    table2.AddCell(new Paragraph(TranslateCCMF("Step_3_Position"), Font12blue));
                    table2.AddCell("");
                    table2.AddCell("");
                    table2.AddCell(new Phrase(obj.Name.ToString(), Font12black));
                    table2.AddCell(new Phrase(obj.Position.ToString(), Font12black));

                    table2.AddCell(new Paragraph(TranslateCCMF("Step_4_Email"), Font12blue));
                    table2.AddCell(new Paragraph(TranslateCCMF("Step_4_Nationality"), Font12blue));
                    table2.AddCell("");
                    table2.AddCell("");
                    table2.AddCell(new Phrase(obj.Email, Font12black));
                    table2.AddCell(new Phrase(obj.Nationality, Font12black));



                    table2.AddCell(new Paragraph(TranslateCCMF("Step_3_Hkid"), Font12blue));
                    table2.AddCell("");
                    table2.AddCell(new Phrase(obj.Masked_HKID, Font12black));
                    table2.AddCell("");

                    PdfPCell cell = new PdfPCell(new Phrase(TranslateCCMF("Step_3_Background"), Font12blue));

                    cell.Colspan = 2;
                    cell.Border = Rectangle.NO_BORDER;
                    table2.AddCell(cell);

                    PdfPCell cell1 = new PdfPCell(new Phrase(obj.Background_Information, Font12black));
                    cell1.Colspan = 2;
                    cell1.Border = Rectangle.NO_BORDER;
                    table2.AddCell(cell1);

                    doc.Add(table2);
                    doc.Add(new Chunk("\n"));
                }

                Paragraph para36 = new Paragraph("4.1(b) " + TranslateCCMF("Step_4_Advisor"), Font15Head);
                doc.Add(para36);
                doc.Add(new Chunk("\n"));
                Paragraph para37 = new Paragraph(objTB_TB_CCMF_APPLICATION.Advisor_Info, Font12black);
                doc.Add(para37);
                doc.Add(new Chunk("\n"));
                Paragraph para38 = new Paragraph("4.2 " + TranslateCCMF("Step_4_Business_model"), Font15Head);
                doc.Add(para38);
                Paragraph para39 = new Paragraph(TranslateCCMF("Step_4_Business_modelsub"), Font10blue);
                doc.Add(para39);
                doc.Add(new Chunk("\n"));
                Paragraph para40 = new Paragraph(objTB_TB_CCMF_APPLICATION.Business_Model, Font12black);
                doc.Add(para40);
                doc.Add(new Chunk("\n"));

                Paragraph para42 = new Paragraph("4.3 " + TranslateCCMF("Step_4_creativity"), Font15Head);
                doc.Add(para42);
                doc.Add(new Chunk("\n"));
                Paragraph para43 = new Paragraph(TranslateCCMF("Step_4_creativitysub"), Font10blue);
                doc.Add(para43);
                doc.Add(new Chunk("\n"));
                Paragraph para44 = new Paragraph(objTB_TB_CCMF_APPLICATION.Innovation, Font12black);
                doc.Add(para44);
                doc.Add(new Chunk("\n"));
                Paragraph para45 = new Paragraph("4.4 Social Responsibility ", Font15Head);
                doc.Add(para45);
                doc.Add(new Chunk("\n"));
                Paragraph para46 = new Paragraph(TranslateCCMF("Step_4_socialresponsibilitysub"), Font10blue);
                doc.Add(para46);
                doc.Add(new Chunk("\n"));
                Paragraph para47 = new Paragraph(objTB_TB_CCMF_APPLICATION.Social_Responsibility, Font12black);
                doc.Add(para47);
                doc.Add(new Chunk("\n"));
                Paragraph para48 = new Paragraph("4.5 Competition Analysis", Font15Head);
                doc.Add(para48);
                doc.Add(new Chunk("\n"));
                Paragraph para49 = new Paragraph(objTB_TB_CCMF_APPLICATION.Competition_Analysis, Font12black);
                doc.Add(para49);
                doc.Add(new Chunk("\n"));

                Paragraph para50 = new Paragraph("4.6 " + TranslateCCMF("Step_4_Projectmilestone"), Font15Head);
                doc.Add(para50);

                PdfPTable table4 = new PdfPTable(2);
                table4.DefaultCell.Border = Rectangle.NO_BORDER;
                table4.TotalWidth = 500f;
                table4.LockedWidth = true;
                float[] widths1 = new float[] { 150f, 350f };
                table4.SetWidths(widths1);
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Milestone"), Font12blue));
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Details"), Font12blue));
                doc.Add(new Chunk("\n"));
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Month_1"), Font12blue));

                table4.AddCell(new Phrase(objTB_TB_CCMF_APPLICATION.Project_Milestone_M1, Font12black));
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Month_2"), Font12blue));

                table4.AddCell(new Phrase(objTB_TB_CCMF_APPLICATION.Project_Milestone_M2, Font12black));
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Month_3"), Font12blue));

                table4.AddCell(new Phrase(objTB_TB_CCMF_APPLICATION.Project_Milestone_M3, Font12black));
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Month_4"), Font12blue));

                table4.AddCell(new Phrase(objTB_TB_CCMF_APPLICATION.Project_Milestone_M4, Font12black));

                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Month_5"), Font12blue));

                table4.AddCell(new Phrase(objTB_TB_CCMF_APPLICATION.Project_Milestone_M5, Font12black));
                table4.AddCell(new Paragraph(TranslateCCMF("Step_4_Month_6"), Font12blue));

                table4.AddCell(new Phrase(objTB_TB_CCMF_APPLICATION.Project_Milestone_M6, Font12black));

                doc.Add(table4);
                doc.Add(new Chunk("\n"));

                doc.Add(new Chunk("\n"));
                Paragraph para61 = new Paragraph("4.7 " + TranslateCCMF("Step_4_Cost_Projections"), Font15Head);
                doc.Add(para61);
                doc.Add(new Chunk("\n"));
                Paragraph para62 = new Paragraph(objTB_TB_CCMF_APPLICATION.Cost_Projection, Font12black);
                doc.Add(para62);
                doc.Add(new Chunk("\n"));
                Paragraph para_61 = new Paragraph("4.8 " + TranslateCCMF("Step_4_Funding_Status"), Font15Head);
                doc.Add(para_61);
                doc.Add(new Chunk("\n"));
                //Paragraph para35 = new Paragraph("List out in detail(i) all grants and funding received / to be received from other publicly and / or privately funded organizations / programmes which the applicant(or companies established by he / she / it) has applied for, will receive or will be entitled to receive in the coming 18 months, or have received in the past 18 months; (ii)the nature of expenditure covered / to be covered by such funding sources; and(iii) the amount and the maximum amount received / to be received under such funding sources ) ", Font10blue);
                Paragraph para35 = new Paragraph(TranslateCCMF("Step_4_Funding_Statussub"), Font10blue);
                doc.Add(para35);
                doc.Add(new Chunk("\n"));
                List<TB_APPLICATION_FUNDING_STATUS> objTB_APPLICATION_FUNDING_STATUS = dbcontext.TB_APPLICATION_FUNDING_STATUS.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID).ToList();
                foreach (TB_APPLICATION_FUNDING_STATUS obj in objTB_APPLICATION_FUNDING_STATUS)
                {
                    PdfPTable table1 = new PdfPTable(2);
                    table1.DefaultCell.Border = Rectangle.NO_BORDER;
                    table1.TotalWidth = 500f;
                    table1.LockedWidth = true;
                    float[] width = new float[] { 250f, 250f };
                    table1.SetWidths(width);
                    table1.AddCell(new Paragraph(TranslateCCMF("Step3_Date"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_Name_of_Programme"), Font12blue));
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell(new Phrase(obj.Date.HasValue ? obj.Date.Value.ToString("MMM-yyyy") : "", Font12black));

                    table1.AddCell(new Phrase(obj.Programme_Name.ToString(), Font12black));
                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_Application_Status"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_Funding_Status"), Font12blue));
                    table1.AddCell(""); table1.AddCell("");
                    table1.AddCell(new Phrase(obj.Application_Status, Font12black));
                    table1.AddCell(new Phrase(obj.Funding_Status.ToString(), Font12black));

                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_expenditure"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_Currency"), Font12blue));
                    table1.AddCell(""); table1.AddCell("");
                    table1.AddCell(new Phrase(obj.Expenditure_Nature.ToString(), Font12black));

                    table1.AddCell(new Phrase(obj.Currency.ToString(), Font12black));
                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_Amount_received"), Font12blue));
                    table1.AddCell(new Paragraph(TranslateCCMF("Step_4_Maximum_amount"), Font12blue));
                    table1.AddCell(""); table1.AddCell("");
                    table1.AddCell(new Phrase(obj.Amount_Received.ToString(), Font12black));

                    table1.AddCell(new Phrase(obj.Maximum_Amount.ToString(), Font12black));
                    doc.Add(table1);
                    doc.Add(new Chunk("\n"));
                }
                //doc.Add(para62);
                //doc.Add(new Chunk("\n"));
                Paragraph para64 = new Paragraph("4.9 " + TranslateCCMF("Step_4_Exit_strategy"), Font15Head);
                doc.Add(para64);
                doc.Add(new Chunk("\n"));
                Paragraph para63 = new Paragraph(objTB_TB_CCMF_APPLICATION.Exit_Stategy, Font12black);
                doc.Add(para63);
                doc.Add(new Chunk("\n"));
                Paragraph para65 = new Paragraph("4.10 " + TranslateCCMF("Step_4_additional_information"), Font15Head);
                doc.Add(para65);
                doc.Add(new Chunk("\n"));
                Paragraph para66 = new Paragraph(objTB_TB_CCMF_APPLICATION.Additional_Information, Font12black);
                doc.Add(para66);
                doc.Add(new Chunk("\n"));

                Paragraph para98 = new Paragraph("5. " + TranslateCCMF("Step_5_CONTACT_DETAILS"), Font18blue);
                para98.Alignment = Element.ALIGN_CENTER;

                doc.Add(para98);
                doc.Add(new Chunk("\n"));
                List<TB_APPLICATION_CONTACT_DETAIL> objTB_APPLICATION_CONTACT_DETAIL = dbcontext.TB_APPLICATION_CONTACT_DETAIL.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID).ToList();
                int i = 1;
                foreach (TB_APPLICATION_CONTACT_DETAIL obj in objTB_APPLICATION_CONTACT_DETAIL)
                {
                    if (i == 1)
                    {
                        Paragraph para99 = new Paragraph("5." + i + " " + TranslateCCMF("Step_5_Contact_Person") + " " + i + TranslateCCMF("Step_5_Principal_Applicant"), Font15Head);
                        doc.Add(para99);
                        doc.Add(new Chunk("\n"));
                    }
                    else
                    {
                        Paragraph para99 = new Paragraph("5." + i + " " + TranslateCCMF("Step_5_Contact_Person") + " " + i, Font15Head);
                        doc.Add(para99);
                        doc.Add(new Chunk("\n"));

                    }
                    i += 1;

                    PdfPTable table3 = new PdfPTable(3);
                    table3.DefaultCell.Border = Rectangle.NO_BORDER;
                    table3.TotalWidth = 450f;
                    table3.LockedWidth = true;
                    float[] widths3 = new float[] { 150f, 150f, 150f };
                    table3.SetWidths(widths3);
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Salution"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Last_Name"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_First_Name"), Font12blue));
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell(new Phrase(obj.Salutation.ToString(), Font12black));
                    table3.AddCell(new Phrase(obj.Last_Name_Eng.ToString(), Font12black));
                    table3.AddCell(new Phrase(obj.First_Name_Eng.ToString(), Font12black));


                    table3.AddCell(new Paragraph(""));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Last_Name_Chi"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_First_Name_Chi"), Font12blue));
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell("");
                    table3.AddCell(new Phrase(obj.Last_Name_Chi.ToString(), Font12black));
                    table3.AddCell(new Phrase(obj.First_Name_Chi.ToString(), Font12black));



                    PdfPCell cell = new PdfPCell(new Phrase(TranslateCCMF("Step_5_Contact_No"), Font12blue));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Colspan = 3;
                    table3.AddCell(cell);

                    PdfPCell cell1 = new PdfPCell(new Phrase(obj.Contact_No, Font12black));
                    cell1.Colspan = 3;
                    cell1.Border = Rectangle.NO_BORDER;

                    cell1.Colspan = 3;
                    table3.AddCell(cell1);

                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Fax"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Email"), Font12blue));
                    table3.AddCell("");

                    table3.AddCell(new Phrase(obj.Fax, Font12black));
                    table3.AddCell(new Phrase(obj.Email, Font12black));
                    table3.AddCell("");

                    PdfPCell cell2 = new PdfPCell(new Phrase(TranslateCCMF("Step_5_Mailing_Address"), Font12blue));
                    cell2.Colspan = 3;
                    cell2.Border = Rectangle.NO_BORDER;
                    table3.AddCell(cell2);
                    PdfPCell cell3 = new PdfPCell(new Phrase(obj.Mailing_Address, Font12black));
                    cell3.Colspan = 3;
                    cell3.Border = Rectangle.NO_BORDER;
                    table3.AddCell(cell3);


                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Institutiuon"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Student_idcard"), Font12blue));
                    table3.AddCell("");

                    table3.AddCell(new Phrase(obj.Education_Institution_Eng, Font12black));
                    table3.AddCell(new Phrase(obj.Student_ID_Number, Font12black));
                    table3.AddCell("");

                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Programme_Enrolled_Eng"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_dateofgrad"), Font12blue));
                    table3.AddCell("");

                    table3.AddCell(new Phrase(obj.Programme_Enrolled_Eng, Font12black));
                    string hdn_year = obj.Graduation_Year.HasValue ? obj.Graduation_Year.ToString() : "";
                    string hdn_month = obj.Graduation_Month.HasValue ? obj.Graduation_Month.ToString() : "";

                    if (!string.IsNullOrEmpty(hdn_year) && !string.IsNullOrEmpty(hdn_month))
                    {
                        string monthName = new DateTime(Convert.ToInt32(hdn_year), Convert.ToInt32(hdn_month), 1).ToString("MMM", CultureInfo.InvariantCulture);
                        table3.AddCell(new Phrase(monthName + "-" + hdn_year, Font12black));
                    }
                    else
                    {
                        table3.AddCell("");
                    }

                    table3.AddCell("");

                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Org_name"), Font12blue));
                    table3.AddCell(new Paragraph(TranslateCCMF("Step_5_Position"), Font12blue));
                    table3.AddCell("");

                    table3.AddCell(new Phrase(obj.Organisation_Name, Font12black));

                    table3.AddCell(new Phrase(obj.Position, Font12black));
                    table3.AddCell("");
                    doc.Add(table3);
                    doc.Add(new Chunk("\n"));
                }

                Paragraph para_1 = new Paragraph("6. " + TranslateCCMF("Step6_Attachment_Header"), Font18blue);
                para_1.Alignment = Element.ALIGN_CENTER;
                doc.Add(para_1);
                doc.Add(new Chunk("\n"));

                string apptype = objTB_TB_CCMF_APPLICATION.CCMF_Application_Type;


                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_br = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID && x.Attachment_Type == "BR_COPY").ToList();
                Paragraph brCopy = new Paragraph();
                brCopy.Font = Font12black;
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_br)
                {
                    brCopy.Add(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1) + "\n");
                }

                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_car = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID && x.Attachment_Type == "Student_ID").ToList();
                Paragraph studentID = new Paragraph();
                studentID.Font = Font12black;
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_car)
                {
                    studentID.Add(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1) + "\n");


                }

                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_videoclip = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID && x.Attachment_Type == "Video_Clip").ToList();
                Paragraph vdoClip = new Paragraph();
                vdoClip.Font = Font12black;
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_videoclip)
                {
                    vdoClip.Add(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1) + "\n");


                }
                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_preslide = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID && x.Attachment_Type == "Presentation_Slide").ToList();
                Paragraph prsntatnAttach = new Paragraph();
                prsntatnAttach.Font = Font12black;
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_preslide)
                {
                    prsntatnAttach.Add(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1) + "\n");

                }

                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_hkid = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID && x.Attachment_Type == "HK_ID").ToList();
                Paragraph hkIDAttach = new Paragraph();
                hkIDAttach.Font = Font12black;
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_hkid)
                {
                    hkIDAttach.Add(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1) + "\n");

                }


                List<TB_APPLICATION_ATTACHMENT> objTB_APPLICATION_ATTACHMENT_otherattach = dbcontext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objTB_TB_CCMF_APPLICATION.CCMF_ID && x.Attachment_Type == "Other_Attachment").ToList();
                Paragraph otherAttach = new Paragraph();
                otherAttach.Font = Font12black;
                foreach (TB_APPLICATION_ATTACHMENT item in objTB_APPLICATION_ATTACHMENT_otherattach)
                {
                    otherAttach.Add(item.Attachment_Path.Remove(0, (item.Attachment_Path).LastIndexOf("/") + 1) + "\n");

                }

                if (!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream))
                {
                    if (objTB_TB_CCMF_APPLICATION.Hong_Kong_Programme_Stream.ToLower() != "professional")
                    {
                        if (apptype.ToLower() == "company")
                        {

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "BRCOPY", "CyberportEMS_CCMF", 1033), Font12blue));
                            //doc.Add(new Paragraph("6.1 BR Copy ", Font12blue));
                            doc.Add(brCopy);
                            doc.Add(new Chunk("\n"));

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "StudentID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(studentID);
                            doc.Add(new Chunk("\n"));

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "VideoClip", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(vdoClip);
                            doc.Add(new Chunk("\n"));

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "PresentationSlide", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(prsntatnAttach);
                            doc.Add(new Chunk("\n"));


                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "OtherAttachment", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(otherAttach);
                            doc.Add(new Chunk("\n"));


                        }
                        else
                        {


                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "StudentID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(studentID);
                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(hkIDAttach);
                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "VideoClip", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(vdoClip);
                            doc.Add(new Chunk("\n"));

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "PresentationSlide", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(prsntatnAttach);
                            doc.Add(new Chunk("\n"));

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "OtherAttachment", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(otherAttach);

                        }

                    }
                    else
                    {
                        if (apptype.ToLower() == "company")
                        {

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "BRCOPY", "CyberportEMS_CCMF", 1033), Font12blue));
                            //doc.Add(new Paragraph(BR Copy ", Font12blue));
                            doc.Add(brCopy);

                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "VideoClip", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(vdoClip);

                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "PresentationSlide", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(prsntatnAttach);


                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "OtherAttachment", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(otherAttach);

                        }
                        else
                        {

                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(hkIDAttach);
                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "VideoClip", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(vdoClip);

                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "PresentationSlide", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(prsntatnAttach);

                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "OtherAttachment", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(otherAttach);
                        }
                    }
                }
                else if (objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower() != "hongkong")
                {
                    if (apptype.ToLower() == "company")
                    {
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "BRCOPY", "CyberportEMS_CCMF", 1033), Font12blue));
                        //doc.Add(new Paragraph("6.1 BR Copy ", Font12blue));
                        doc.Add(brCopy);
                        if (!objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("gbayep"))
                        {
                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "StudentID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(studentID);
                        }
                        

                        doc.Add(new Chunk("\n"));
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "VideoClip", "CyberportEMS_CCMF", 1033), Font12blue));
                        doc.Add(vdoClip);

                        doc.Add(new Chunk("\n"));
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "PresentationSlide", "CyberportEMS_CCMF", 1033), Font12blue));
                        doc.Add(prsntatnAttach);


                        doc.Add(new Chunk("\n"));
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "OtherAttachment", "CyberportEMS_CCMF", 1033), Font12blue));
                        doc.Add(otherAttach);
                    }
                    else
                    {
                        if (!objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("gbayep"))
                        {
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "StudentID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(studentID);
                            doc.Add(new Chunk("\n"));
                            doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKID", "CyberportEMS_CCMF", 1033), Font12blue));
                            doc.Add(hkIDAttach);
                            doc.Add(new Chunk("\n"));
                       
                        }
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "VideoClip", "CyberportEMS_CCMF", 1033), Font12blue));
                        doc.Add(vdoClip);

                        doc.Add(new Chunk("\n"));
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "PresentationSlide", "CyberportEMS_CCMF", 1033), Font12blue));
                        doc.Add(prsntatnAttach);


                        doc.Add(new Chunk("\n"));
                        doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "OtherAttachment", "CyberportEMS_CCMF", 1033), Font12blue));
                        doc.Add(otherAttach);
                    }

                }

                doc.Add(new Chunk("\n"));

                Paragraph para_DECLARATION = new Paragraph(TranslateCCMF("Step_6_DECLARATION"), Font18blue);
                para_DECLARATION.Alignment = Element.ALIGN_CENTER;
                doc.Add(para_DECLARATION);
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(objTB_TB_CCMF_APPLICATION.Declaration == true ? "Yes" : "No", Font12black));
                Paragraph para_DECLARATION1 = new Paragraph(TranslateCCMF("Step_6_Consideration"), Font12Green);
                doc.Add(para_DECLARATION1);
                doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("7.1 We agree with all the terms and conditions set out in the CCMF Guides (http://www.cyberport.hk/files/ccmf/ENC_RF_015a_CCMF_Guides_and_Notes_for_Applicants_CCMF.pdf) and Notes for the Applicants (ENC.RF.O15) governing the application of the Cyberport Creative Micro Fund Scheme.", Font12blue));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_1"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_2"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_3"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_4"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_5"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_6"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_7"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_8"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Consideration_9"), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Chunk("\n"));

                PdfPTable tableapp = new PdfPTable(2);
                tableapp.DefaultCell.Border = Rectangle.NO_BORDER;
                tableapp.TotalWidth = 500f;
                tableapp.LockedWidth = true;
                float[] widths12 = new float[] { 250f, 250f };
                tableapp.SetWidths(widths12);
                if (!objTB_TB_CCMF_APPLICATION.Programme_Type.ToLower().Contains("gbayep"))
                {
                    tableapp.AddCell(new Paragraph(TranslateCCMF("Step_6_Full_Name"), Font12blue));
                    tableapp.AddCell(new Paragraph(TranslateCCMF("Step_6_Title_Principal_Applicant"), Font12blue));
                    tableapp.AddCell("");
                    tableapp.AddCell("");
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_Full_Name) ? objTB_TB_CCMF_APPLICATION.Principal_Full_Name : "", Font12black));
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_Position_Title) ? objTB_TB_CCMF_APPLICATION.Principal_Position_Title : "", Font12black));
                

                }
                
                else
                {
                    tableapp.AddCell(new Paragraph(TranslateGBAYEP("Step_6_Full_Name"), Font12blue));
                    tableapp.AddCell(new Paragraph(TranslateGBAYEP("Step_6_Title_Principal_Applicant"), Font12blue));
                    tableapp.AddCell("");
                    tableapp.AddCell("");
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_Full_Name) ? objTB_TB_CCMF_APPLICATION.Principal_Full_Name : "", Font12black));
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_Position_Title) ? objTB_TB_CCMF_APPLICATION.Principal_Position_Title : "", Font12black));

                    tableapp.AddCell(new Paragraph(TranslateGBAYEP("Step_6_Full_Name_2"), Font12blue));
                    tableapp.AddCell(new Paragraph(TranslateGBAYEP("Step_6_Title_2nd_Applicant"), Font12blue));
                    tableapp.AddCell("");
                    tableapp.AddCell("");
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_2nd_Full_Name) ? objTB_TB_CCMF_APPLICATION.Principal_2nd_Full_Name : "", Font12black));
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_2nd_Position_Title) ? objTB_TB_CCMF_APPLICATION.Principal_2nd_Position_Title : "", Font12black));

                    tableapp.AddCell(new Paragraph(TranslateGBAYEP("Step_6_2nd_Email"), Font12blue));
                    tableapp.AddCell(new Phrase(!string.IsNullOrEmpty(objTB_TB_CCMF_APPLICATION.Principal_2nd_Email) ? objTB_TB_CCMF_APPLICATION.Principal_2nd_Email : "", Font12black));

                }
                doc.Add(tableapp);
                doc.Add(new Chunk("\n"));

                Paragraph para_pinfo = new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_PERSONAL_INFORMATION", "CyberportEMS_CCMF", 1033), Font18blue);
                //para_pinfo.Alignment = Element.ALIGN_CENTER;
                doc.Add(para_pinfo);
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_PERSONAL_INFORMATION_1", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose", "CyberportEMS_CCMF", 1033), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_1", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));

                Paragraph pin1 = new Paragraph(
                   "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_2", "CyberportEMS_CCMF", 1033) + "\n\n" +
                   "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_3", "CyberportEMS_CCMF", 1033) + "\n\n" +
                   "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_4", "CyberportEMS_CCMF", 1033) + "\n\n" +
                   "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_5", "CyberportEMS_CCMF", 1033) + "\n\n" +
                   "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_6", "CyberportEMS_CCMF", 1033) + "\n\n" +
                   "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_7", "CyberportEMS_CCMF", 1033) + "\n\n"
                , Font12blue);
                pin1.IndentationLeft = 30;
                doc.Add(pin1);
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Purpose_8", "CyberportEMS_CCMF", 1033), Font12blue));


                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Transfer_data", "CyberportEMS_CCMF", 1033), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Transfer_data_1", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));

                Paragraph pin2 = new Paragraph(
                  "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Transfer_data_2", "CyberportEMS_CCMF", 1033) + "\n\n" +
                  "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Transfer_data_3", "CyberportEMS_CCMF", 1033) + "\n\n" +
                  "• " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_Transfer_data_4", "CyberportEMS_CCMF", 1033) + "\n\n"
                , Font12blue);
                pin2.IndentationLeft = 30;
                doc.Add(pin2);
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Transfer_data_note", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Direct_marketing", "CyberportEMS_CCMF", 1033), Font15Head));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Direct_marketing_1", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_cyberport", "CyberportEMS_CCMF", 1033) +
                    " " + SPUtility.GetLocalizedString("$Resources:" + "Step_6_cyberport_1", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_cyberport_2", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Privacy", "CyberportEMS_CCMF", 1033), Font15Head));
                doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph("Please see our Privacy Policy Statement at http://www.cyberport.hk/en/privacy_policy for our general policy and practices in respect of our collection and use of personal data.", Font12blue));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Privacy_2"), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "Step_6_Access", "CyberportEMS_CCMF", 1033), Font15Head));
                doc.Add(new Chunk("\n"));
                //doc.Add(new Paragraph(@"You have the right to request access to, and correction of, Your Data held by us. We may charge a reasonable fee for administering and processing your data access request. If you need to check whether we hold Your Data or if you wish to have access to, correct any of Your Data which is inaccurate, please write via e-mail to our Data Protection Officer at dpo@cyberport.hk or via mail to Units 1102-04, Level 11, Cyberport 2, 100 Cyberport Road, Hong Kong.", Font12blue));
                doc.Add(new Paragraph(TranslateCCMF("Step_6_Access_1"), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "STEP_6_STATEMENT", "CyberportEMS_CCMF", 1033), Font12blue));
                doc.Add(new Chunk("\n"));

                doc.Add(new Chunk("\n"));
                if (objTB_TB_CCMF_APPLICATION.Have_Read_Statement.HasValue)
                {
                    doc.Add(new Paragraph(objTB_TB_CCMF_APPLICATION.Have_Read_Statement == true ? "Yes" : "No", Font12black));
                }

                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "step_6_personal_information_collection", "CyberportEMS_CCMF", 1033), Font12Green));
                doc.Add(new Chunk("\n"));

                if (objTB_TB_CCMF_APPLICATION.Marketing_Information.HasValue)
                {
                    doc.Add(new Paragraph(objTB_TB_CCMF_APPLICATION.Marketing_Information == true ? "Yes" : "No", Font12black));
                }
                doc.Add(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "step_6_marketing", "CyberportEMS_CCMF", 1033), Font12Green));
                doc.Add(new Chunk("\n"));


            }
        }

        private void Switch2UI(TB_CCMF_APPLICATION objTB_INCUBATION_APPLICATION, string ProgType, ref PdfPTable table, ref Document doc)
        {
            switch (ProgType)
            {
                case "cbi":
                    {
                        doc.Add(new Paragraph("2.1.1 " + TranslateCCMF("Individual_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("CCMF_Ind_1_A"), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));



                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");


                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("CCMF_IndComp_2_A"), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_G", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("h)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_H", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("i)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_I", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("j)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_J", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString() == "True" ? "Yes" : "No", Font12black));


                    }
                    break;
                case "cbc":
                    {

                        doc.Add(new Paragraph("2.1.1 Company Applicant", Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_Comp_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_Comp_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));


                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(" Individual and Company Applicant", Font12blue));
                        table.AddCell("");


                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_G", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("h)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_H", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("i)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_I", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("j)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMF_IndComp_2_J", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString() == "True" ? "Yes" : "No", Font12black));
                    }

                    break;
                case "cbuc":
                    {
                        doc.Add(new Paragraph("2.1.1 Company Applicant", Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Comp_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Comp_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(" Individual and Company Applicant", Font12blue));
                        table.AddCell("");


                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_G", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("h)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_H", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));


                    }
                    break;
                case "cbui":
                    {
                        doc.Add(new Paragraph("2.1.1 Individual Applicant", Font12blue));
                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Ind_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Ind_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));



                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(" Individual and Company Applicant", Font12blue));
                        table.AddCell("");


                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_G", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("h)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CUPP_IndComp_2_H", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));

                    }
                    break;
                case "hkyc":
                    {
                        doc.Add(new Paragraph("2.1.1 " + TranslateCCMF("Company_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Comp_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Comp_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Comp_1_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1c.ToString() == "True" ? "Yes" : "No", Font12black));



                        table.AddCell(new Paragraph("2.1.2 ", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");


                        //table.AddCell(new Paragraph("a)", Font12blue));
                        //table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        //table.AddCell(new Paragraph("b)", Font12blue));
                        //table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_G", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_H", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));
                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_F_1", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString() == "True" ? "Yes" : "No", Font12black));

                    }
                    break;
                case "hkyi":
                    {
                        doc.Add(new Paragraph("2.1.1 " + TranslateCCMF("Individual_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Ind_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Ind_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));

                        //table.AddCell(new Paragraph("c)", Font12blue));
                        //table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_Ind_1_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1c.ToString() == "True" ? "Yes" : "No", Font12black));


                        table.AddCell(new Paragraph("2.1.2 ", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");


                        //table.AddCell(new Paragraph("a)", Font12blue));
                        //table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        //table.AddCell(new Paragraph("b)", Font12blue));
                        //table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        //table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_G", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_H", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Young_IndComp_2_F_1", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString() == "True" ? "Yes" : "No", Font12black));
                    }
                    break;
                case "hkpc":
                    {
                        doc.Add(new Paragraph("2.1.1 " + TranslateCCMF("Company_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_Comp_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_Comp_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("2.1.2 ", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");


                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_F_1", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString() == "True" ? "Yes" : "No", Font12black));


                    }
                    break;
                case "hkpi":
                    {

                        doc.Add(new Paragraph("2.1.1 " + TranslateCCMF("Individual_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_Ind_1_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_Ind_1_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");


                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_A", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_B", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_C", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_D", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_E", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_F", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "HKP_Pro_Stream_IndComp_2_F_1", "CyberportEMS_CCMF", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString() == "True" ? "Yes" : "No", Font12black));

                    }
                    break;
                case "ccmfgbayepc":
                    {
                        doc.Add(new Paragraph("2.1.1 " + TranslateGBAYEP("Company_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_Comp_1_A", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_Comp_1_B", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_C", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2c.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_D", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_E", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_F", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_G", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f_1.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("h)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_H", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("i)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_I", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("j)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_J", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("k)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_K", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2k.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2k.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("l)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_L", "CyberportEMS_CCMFGBAYEP", 1033).Replace("2.2 (j)", "2.2 (i)"), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2l.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2l.ToString() == "True" ?
"Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("m)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_M", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2m.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2m.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("n)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_N", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2n.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2n.ToString() == "True" ? "Yes" : "No", Font12black));
                    }
                    break;
                case "ccmfgbayepi":
                    {
                        doc.Add(new Paragraph("2.1.1 " + TranslateGBAYEP("Individual_Applicant"), Font12blue));

                        table.AddCell(new Paragraph("a)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_Ind_1_A", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("2.1.2", Font12blue));
                        table.AddCell(new Paragraph(TranslateCCMF("Individual_and_Company_Applicant"), Font12blue));
                        table.AddCell("");
                        
                       /* table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_Comp_1_B", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_1b.ToString() == "True" ? "Yes" : "No", Font12black));*/


                        table.AddCell(new Paragraph("b)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_C", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2a.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("c)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_D", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2d.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("d)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_E", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2e.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("e)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_F", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2f.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("f)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_G", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2g.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("g)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_H", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2h.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("h)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_I", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2i.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("i)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_J", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2j.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("j)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_K", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2k.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2k.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("k)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_L", "CyberportEMS_CCMFGBAYEP", 1033).Replace("2.2 (j)", "2.2 (i)"), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2l.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2l.ToString() == "True" ? 
"Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("l)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_M", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2m.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2m.ToString() == "True" ? "Yes" : "No", Font12black));

                        table.AddCell(new Paragraph("m)", Font12blue));
                        table.AddCell(new Paragraph(SPUtility.GetLocalizedString("$Resources:" + "CCMFGBAYEP_IndComp_2_N", "CyberportEMS_CCMFGBAYEP", 1033), Font12blue));
                        table.AddCell(new Paragraph(string.IsNullOrEmpty(objTB_INCUBATION_APPLICATION.Question2_1_2n.ToString()) ? "" : objTB_INCUBATION_APPLICATION.Question2_1_2n.ToString() == "True" ? "Yes" : "No", Font12black));
                    }
                    break;
                default:
                    break;
            }

        }

        private string GetBusinessAreaByValue(string value)
        {
            string businessArea = string.Empty;

            switch (value)
            {
                case "E-Commerce":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_ECommerce", "CyberportEMS_CCMF", 1033);
                    break;
                case "Edutech":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Edutech", "CyberportEMS_CCMF", 1033);
                    break;
                case "Open Data":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_OpenData", "CyberportEMS_CCMF", 1033);
                    break;
                case "AI / Big Data":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_OpenData", "CyberportEMS_CCMF", 1033);
                    break;
                case "Fintech":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Fintech", "CyberportEMS_CCMF", 1033);
                    break;
                case "Gaming":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Gaming", "CyberportEMS_CCMF", 1033);
                    break;
                case "Healthcare":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Healthcare", "CyberportEMS_CCMF", 1033);
                    break;
                case "Wearable":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Wearable", "CyberportEMS_CCMF", 1033);
                    break;
                case "Social-Media":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Social_Media", "CyberportEMS_CCMF", 1033);
                    break;
                case "Others":
                    businessArea = SPUtility.GetLocalizedString("$Resources:" + "Step3_Others", "CyberportEMS_CCMF", 1033);
                    break;
                default:
                    businessArea = value;
                    break;
            }
            return businessArea;
        }

        private string GetCompanyTypeByValue(string value)
        {
            string companyType = string.Empty;

            switch (value.Trim().ToLower())
            {
                case "private":
                    companyType = SPUtility.GetLocalizedString("$Resources:" + "step_2_Unlimited", "CyberportEMS_Incubation", 1033);
                    break;
                case "limited":
                    companyType = SPUtility.GetLocalizedString("$Resources:" + "Step_2_Limited", "CyberportEMS_Incubation", 1033);
                    break;
                case "public":
                    companyType = SPUtility.GetLocalizedString("$Resources:" + "Step_2_Publicly_listed", "CyberportEMS_Incubation", 1033);
                    break;
                case "others":
                    companyType = SPUtility.GetLocalizedString("$Resources:" + "Step2_Others", "CyberportEMS_Incubation", 1033);
                    break;
                default:
                    companyType = value;
                    break;
            }
            return companyType;
        }

        public string ProcessMyDataItem(object myValue)
        {
            if (myValue == null)
            {
                return "";
            }

            return myValue.ToString().Replace(Environment.NewLine, "<br />");
        }

        private void ShowPdfCCMF(byte[] strS)
        {

            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "CCMF Application.pdf");

            System.Web.HttpContext.Current.Response.BinaryWrite(strS);
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.Clear();


        }

        private string TranslateIncubation(string key)
        {
            string strValue = SPUtility.GetLocalizedString("$Resources:" + key, "CyberportEMS_Incubation", 1033);
            strValue = strValue.Replace("<mark>", "").Replace("</mark>", "").Replace("<small>", "").Replace("</small>", "").Replace("<span class=\"bold graylbl-2\">", "").Replace("<span>", "").Replace("<mark class=\"mark-2\">", "");
            strValue = strValue.Replace("<Blockquote>", "").Replace("</Blockquote>", "").Replace("<i>", "").Replace("</br>", " ").Replace("<u>", "").Replace("</u>", "").Replace("</span>", "").Replace("</p>", "");
            strValue = strValue.Replace("</a>", "").Replace(" target=\"_blank\">", "").Replace("<a href=\"https://www.cyberport.hk/guides_and_notes/cip/ENC_RF_010_eng.pdf\"", "").Replace("</i>", "");
            strValue = strValue.Replace("<li>", "  • ").Replace("</li>", "\n").Replace("<ul>", "").Replace("</ul>", "").Replace("<br>", "\n").Replace("<p style=\"color:#808080\">", "");
            strValue = strValue.Replace("<a href=\"mailto:cip_enquiry@cyberport.hk\">", "");

            return strValue;
        }
        private string TranslateCCMF(string key)
        {
            string strValue = SPUtility.GetLocalizedString("$Resources:" + key, "CyberportEMS_CCMF", 1033);
            strValue = strValue.Replace("<mark>", "").Replace("</mark>", "").Replace("<small>", "").Replace("</small>", "").Replace("<span class=\"bold graylbl-2\">", "").Replace("<span>", "").Replace("<mark class=\"mark-2\">", "");
            strValue = strValue.Replace("<li>", "  • ").Replace("</li>", "\n").Replace("<ul>", "").Replace("</ul>", "").Replace("<br>", "\n").Replace("<br/>", "\n").Replace("</br>", "\n").Replace("</span>", "");
            strValue = strValue.Replace("</a>", "").Replace("<a href=\"https://www.cyberport.hk/guides_and_notes/ccmf-hk/ENC_RF_015a_eng.pdf\"", "").Replace("target=\"_blank\">", "").Replace("</i>", "");
            strValue = strValue.Replace("<a href= \"mailto:cip_enquiry@cyberport.hk\">", "");
            strValue = strValue.Replace("<a href=\"http://www.cyberport.hk/files/ccmf/ENC_RF_015a_CCMF_Guides_and_Notes_for_Applicants_CCMF.pdf\"", "");
            return strValue;
        }

        private string TranslateGBAYEP(string key)
        {
            string strValue = SPUtility.GetLocalizedString("$Resources:" + key, "CyberportEMS_CCMFGBAYEP", 1033);
            strValue = strValue.Replace("<mark>", "").Replace("</mark>", "").Replace("<small>", "").Replace("</small>", "").Replace("<span class=\"bold graylbl-2\">", "").Replace("<span>", "").Replace("<mark class=\"mark-2\">", "");
            strValue = strValue.Replace("<li>", "  • ").Replace("</li>", "\n").Replace("<ul>", "").Replace("</ul>", "").Replace("<br>", "\n").Replace("<br/>", "\n").Replace("</br>", "\n").Replace("</span>", "");
            strValue = strValue.Replace("</a>", "").Replace("<a href=\"https://www.cyberport.hk/guides_and_notes/ccmf-hk/ENC_RF_015a_eng.pdf\"", "").Replace("target=\"_blank\">", "").Replace("</i>", "");
            strValue = strValue.Replace("<a href= \"mailto:cip_enquiry@cyberport.hk\">", "");
            strValue = strValue.Replace("<a href=\"http://www.cyberport.hk/files/ccmf/ENC_RF_015a_CCMF_Guides_and_Notes_for_Applicants_CCMF.pdf\"", "");
            return strValue;
        }
        public class SearchResult
        {
            public string ProgrammeName { get; set; }
            public string IntakeNumber { get; set; }
            public string ClusterValue { get; set; }
            public string ClusterText { get; set; }

        }

        public class SortColumnClass
        {
            public string ColumnValue { get; set; }
            public string ColumnText { get; set; }

        }
        public class ApplicationList
        {
            public string ApplicationNo { get; set; }
            public string Cluster { get; set; }
            public string CompanyName { get; set; }
            public string CompanyNameChinese { get; set; }
            public string ProjectName { get; set; }
            public string ProjectNameChinese { get; set; }
            public string ProgrammeType { get; set; }
            public string ApplicationType { get; set; }
            public string ProjectDescription { get; set; }
            public string Status { get; set; }
            public string BDMScore { get; set; }
            public string SrManagerScore { get; set; }
            public string CPMOScore { get; set; }
            public string AverageScore { get; set; }
            public string Remarks { get; set; }
            public string RemarksForVetting { get; set; }
            public Boolean Shortlisted { get; set; }
            public string APPNoURL { get; set; }
            public string HongKongProgrammeStream { get; set; }

            public string ApplicationID { get; set; }

            public DateTime Submitted_Date { get; set; }

            //For ss8
            public string SmartSpace { get; set; }
        }

        protected void lstSS8_SelectedIndexChanged(object sender, EventArgs e)
        {

            ConnectOpen();
            try
            {
                m_ss8 = lstSS8.SelectedValue;
                CheckButtonVisiable(GridViewApplicationBindData());
            }
            finally
            {
                ConnectClose();
            }
        }
    }
}

