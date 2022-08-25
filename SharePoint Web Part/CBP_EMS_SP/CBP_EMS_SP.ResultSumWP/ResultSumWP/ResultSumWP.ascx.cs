using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;


namespace CBP_EMS_SP.ResultSumWP.ResultSumWP
{
    [ToolboxItemAttribute(false)]
    public partial class ResultSumWP : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ResultSumWP()
        {
        }

        List<SearchResult> lstData = new List<SearchResult>();

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
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
        }

        private void BindData()
        {
            GenerateData();
            gvTable.DataSource = lstData;
            gvTable.DataBind();
        }

        private void GenerateData()
        {

            lstData.Clear();

            //var connection = new SqlConnection("Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;");
            //var connection = new SqlConnection("Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;");
            var connection = new SqlConnection(connStr);
            connection.Open();
            try
            {
                var command = new SqlCommand("SELECT A.[Application_Number] as [App No], A.[Company_Name_Eng] as [Comp/Proj Name], CAST(B.Total_Score as decimal) as [Total Score],  CAST(C.[Recommend] as int) as [Recommend],  CAST(F.[Not Recommend] as int) as [Not Recommend], CAST(C.[Recommend]+F.[Not Recommend] as int) as [No. of Vote] FROM [TB_INCUBATION_APPLICATION] as A JOIN [TB_PRESENTATION_INCUBATION_SCORE] as B ON A.[Application_Number] = B.[Application_Number] JOIN (SELECT D.Application_Number, CAST(COUNT(*) as int) as [Recommend] FROM [TB_VETTING_DECISION] as D WHERE D.[Go] = 1 GROUP BY D.[Application_Number]) as C ON A.[Application_Number] = C.[Application_Number] JOIN (SELECT E.Application_Number, CAST(COUNT(*) as int) as [Not Recommend] FROM [TB_VETTING_DECISION] as E WHERE E.[Go] = 0 GROUP BY E.[Application_Number]) as F ON A.[Application_Number] = F.[Application_Number] UNION SELECT A.[Application_Number] as [App No], A.[Project_Name_Eng] as [Comp/Proj Name],CAST(B.Total_Score as decimal) as [Total Score], CAST(C.[Recommend] as int) as [Recommend], CAST(F.[Not Recommend] as int) as [Not Recommend],	CAST(C.[Recommend]+F.[Not Recommend] as int) as [No. of Vote] FROM [TB_CCMF_APPLICATION] as A JOIN [TB_PRESENTATION_CCMF_SCORE] as B ON A.[Application_Number] = B.[Application_Number] JOIN (SELECT D.Application_Number, CAST(COUNT(*) as int) as [Recommend] FROM [TB_VETTING_DECISION] as D WHERE D.[Go] = 1 GROUP BY D.[Application_Number]) as C ON A.[Application_Number] = C.[Application_Number] JOIN (SELECT E.Application_Number, CAST(COUNT(*) as int) as [Not Recommend] FROM [TB_VETTING_DECISION] as E WHERE E.[Go] = 0 GROUP BY E.[Application_Number]) as F ON A.[Application_Number] = F.[Application_Number]", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstData.Add(new SearchResult
                    {
                        Sequence = "",
                        Application_Number = reader.GetString(0),
                        CompProjName = reader.GetString(1),
                        ScoreOfEachVT = "",
                        TotalScore = reader.GetDecimal(2),
                        AvgScore = "",
                        GoNotGoEachVT = "",
                        Recommend = reader.GetInt32(3),
                        NotRecommend = reader.GetInt32(4),
                        NoOfVote = reader.GetInt32(5),
                        Remarks = ""

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
            public string Sequence { get; set; }
            public string Application_Number { get; set; }
            public string CompProjName { get; set; }
            public string ScoreOfEachVT { get; set; }
            public decimal TotalScore { get; set; }
            public string AvgScore { get; set; }
            public string GoNotGoEachVT { get; set; }
            public int Recommend { get; set; }
            public int NotRecommend { get; set; }
            public int NoOfVote { get; set; }
            public string Remarks { get; set; }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("http://www.google.com.hk");
        }
    }
}
