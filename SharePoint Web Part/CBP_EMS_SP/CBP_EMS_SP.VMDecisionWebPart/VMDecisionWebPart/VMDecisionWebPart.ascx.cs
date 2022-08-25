using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using CBP_EMS_SP.Data.Models;
using System.Linq;

namespace CBP_EMS_SP.VMDecisionWebPart.VMDecisionWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class VMDecisionWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public VMDecisionWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        //private String m_progid;
        private String m_VM_De_ID;
        private String m_VM_ID;
        private String m_ApplicationID;
        //private String m_systemuser;
        //private String m_Member_Email = "";

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
        private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";
        protected void Page_Load(object sender, EventArgs e)
        {
            //m_progid = Context.Request.QueryString["ProgNo"];
            m_VM_De_ID = Context.Request.QueryString["VMDeID"];
            m_ApplicationID = Context.Request.QueryString["AppNo"];
            m_VM_ID = Context.Request.QueryString["vmID"];

            screenValueSet();
        }

        protected void screenValueSet()
        {
           // double lstMT_Num = Convert.ToDouble(lstMT.SelectedValue);
           // double lstBMTM_Num = Convert.ToDouble(lstBMTM.SelectedValue);
           // double lstCIPP_Num = Convert.ToDouble(lstCIPP.SelectedValue);
           // double lstSR_Num = Convert.ToDouble(lstSR.SelectedValue);
           // double TotalScore = ((lstMT_Num * scorepresent("lstMT_Num")) + (lstBMTM_Num * scorepresent("lstBMTM_Num")) + (lstCIPP_Num * scorepresent("lstCIPP_Num")) + (lstSR_Num * scorepresent("lstSR_Num")));
           // lblTotalScore.Text = TotalScore.ToString("F2");
           //
            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblApplicationNo.Text = m_ApplicationID;
           //
           //
           // var connection = new SqlConnection(connStr);
           // connection.Open();
           // var command = new SqlCommand("SELECT Company_Name_Eng,Company_Name_Chi,Applicant FROM TB_INCUBATION_APPLICATION where Application_Number='" + m_ApplicationID + "'", connection);
           // var reader = command.ExecuteReader();
           // while (reader.Read())
           // {
           //     lblCompanyName.Text = reader.GetString(0);
           //     m_Member_Email = reader.GetString(2);
           // }
           //
           // reader.Dispose();
           // command.Dispose();
           // connection.Close();
           // connection.Dispose();
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                //string Programme_ID = m_progid;
                string VM_ID = m_VM_ID;

                string VM_De_ID = m_VM_De_ID;
                string Application_Number = m_ApplicationID;
                string Member_Email = txtboxName.Text;
                string GoOrNotGo = radioGoNotGo.SelectedValue.ToString();
                 using (var dbcontext = new CyberportEMS_EDM())
                {
                    bool? metting_complted = dbcontext.TB_VETTING_MEETING.FirstOrDefault(k => k.Vetting_Meeting_ID == Guid.Parse( VM_ID)).Meeting_Completed;
                    if (metting_complted==false)
                    {
                string sql = "insert into TB_VETTING_DECISION(Vetting_Delclaration_ID,Vetting_Meeting_ID,Application_Number,Member_Email,Go) VALUES ("
                    + "'" + VM_De_ID + "' , "
                    + "'" + VM_ID + "' , "
                    + "'" + Application_Number + "' , "
                    + "'" + Member_Email + "' , "
                    + "'" + GoOrNotGo + "')";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                    }
                     else
                    {
                        
                        PopupAlreadyconfirmed.Visible = true;

                    }
                 }
                //Context.Response.Redirect("http://www.google.com.hk");
                
            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();
                }
            }
        
        }
          protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            PopupAlreadyconfirmed.Visible = false;
            Page.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            btnsubmit.Enabled = false;
        }
    }
}
