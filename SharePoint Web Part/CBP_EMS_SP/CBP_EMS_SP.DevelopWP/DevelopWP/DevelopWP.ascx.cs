using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data.Models;
using System.Data.SqlClient;

namespace CBP_EMS_SP.DevelopWP.DevelopWP
{
    [ToolboxItemAttribute(false)]
    public partial class DevelopWP : WebPart
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

        public DevelopWP()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbltest.Text = "ASF";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
         
            using (var cydb = new CyberportEMS_EDM())
            {
                var setdb = cydb.Set<TB_SCREENING_HISTORY>();


                setdb.Add(new TB_SCREENING_HISTORY
                { 
                    Application_Number = "CPIP-201703-0004",
                    Programme_ID = 16,
                    Validation_Result = "Eligibility checked",
                    Comment_For_Applicants = "",
                    Comment_For_Internal_Use ="",
                    Created_By ="Andy",
                    Created_Date = DateTime.Now
                });

                    //Application_Number = "CPIP-201703-0004",
                    //Programme_ID = 16,
                    //Validation_Result = "Eligibility checked",
                    //Comment_For_Applicants = "",
                    //Comment_For_Internal_Use ="",
                    //Created_By ="Andy",
                    //Created_Date = DateTime.Now

            }

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

        protected void Button1_Click(object sender, EventArgs e)
        {
            lbltest.Text = AddScoreChecking("CCFM-201704-0003", "184", "Senior Manager","CCMF").ToString();
        }

        protected int AddScoreChecking(string v_Application_Number, string v_Programme_ID, string v_Role,string Scoretype) 
        {
            int m_result = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                if (Scoretype.Trim() == "CCMF")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Application_Number') FROM [TB_SCREENING_CCMF_SCORE] where Application_Number='" + v_Application_Number + "' and Programme_ID='" + v_Programme_ID + "' and Role='" + v_Role + "'", conn))
                    {
                        conn.Open();
                        m_result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        conn.Close();
                    }                
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Application_Number') FROM [TB_SCREENING_INCUBATION_SCORE] where Application_Number='" + v_Application_Number + "' and Programme_ID='" + v_Programme_ID + "' and Role='" + v_Role + "'", conn))
                    {
                        conn.Open();
                        m_result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        conn.Close();
                    }
                }
            }

            return m_result;

        }
    }
}
