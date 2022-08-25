using CBP_EMS_SP.Common;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.IdentityModel;
using System.Globalization;
using System.Linq;

namespace CBP_EMS_SP.PresentationOfAppWP.PresentationListOfAppWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class PresentationListOfAppWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public PresentationListOfAppWebPart()
        {
        }

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportWMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private string connStr
        {
            get
            {
             //   return System.Configuration.ConfigurationManager.ConnectionStrings["EmsSqlConStr"].ConnectionString;

               return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        private SqlConnection connection;

        private string m_VMID;
        public Dictionary<String, int> GridViewColumnOrder;

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
        public String ImpersonateUser = "";
        public String IsLogEvent;

        private String m_Role;
        public double m_DownloadFileSize = 0;

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
                Panelplof.Visible = true;

                SetGridViewColumnOrder();

                if (!Page.IsPostBack)
                {
                    bool MeetingConfirmed = CheckWhetherConfirm();
                    CheckBtnDisable();
                    screenValueSet();
                    BindGridViewApplicationList(MeetingConfirmed);
                    btnConfirm.Enabled = MeetingConfirmed;

                    if (IsPreListConfirmedByMember())
                    {

                        btnConfirm.Text = "Reconfirm Presentation List";

                    }

                }

                getSYSTEMPARAMETER();

                if (CheckDecisionIsCompleted() == true)
                {
                    btnConfirm.Enabled = false;
                }
                else btnConfirm.Enabled = true;
            }
            else
            {
                Panelplof.Visible = false;
            }
        }

        public Boolean CheckUser()
        {
            if (m_VMID != null)
            {
                getReview();

                if (m_Role.Contains("Vetting Team") && GetMemberInfo())
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

        private bool CheckDecisionIsCompleted()
        {
            bool result = false;
            string sql = "SELECT Decision_Completed FROM TB_VETTING_MEETING WHERE Vetting_Meeting_ID = @vmID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@vmID", m_VMID);

                    //if (command.ExecuteScalar() != null && DBNull.Value != command.ExecuteScalar())
                    //{
                    string strresult = command.ExecuteScalar().ToString();
                    Boolean.TryParse(strresult, out result);
                    //}

                    conn.Close();
                }
            }
            return result;
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

        public Boolean GetMemberInfo()
        {
            var status = false;
            ConnectOpen();
            try
            {
                var sqlString = "select tmInfo.Email,tmInfo.Full_Name,tmInfo.Disabled from TB_VETTING_MEETING tvm inner join TB_VETTING_MEMBER tvmember on tvmember.Vetting_Meeting_ID = tvm.Vetting_Meeting_ID inner join TB_VETTING_MEMBER_INFO tmInfo on tmInfo.Vetting_Member_ID = tvmember.Vetting_Member_ID where tvm.Vetting_Meeting_ID = @m_VMID and tmInfo.Disabled=0 and tmInfo.Email = @Email";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Name.ToString()));
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    status = true;
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

        private void CheckBtnDisable()
        {
            ConnectOpen();
            try
            {
                var sqlString = " select TB_VETTING_MEMBER.isLeader  from TB_VETTING_MEMBER inner join TB_VETTING_MEMBER_INFO on TB_VETTING_MEMBER_INFO.Vetting_Member_ID = TB_VETTING_MEMBER.Vetting_Member_ID where TB_VETTING_MEMBER_INFO.Email = @Email and TB_VETTING_MEMBER.Vetting_Meeting_ID = @m_VMID";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Email));
                command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var Btnstatus = (Boolean)reader.GetValue(0);
                    if (Btnstatus)
                    {
                        btnPresentationResultSummary.Visible = true;
                        btnDecisionSummary.Visible = true;
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

        private void screenValueSet()
        {
            ConnectOpen();
            try
            {
                var sqlString = "select tvm.Date,tvm.Presentation_From,tvm.Presentation_To,tpi.Programme_Name,tpi.Intake_Number,tvm.Venue,tvm.Decision_Completed, tvm.Meeting_Completed "
                                + "from TB_VETTING_MEETING tvm inner join TB_PROGRAMME_INTAKE tpi on tpi.Programme_ID = tvm.Programme_ID "
                                + "where tvm.Vetting_Meeting_ID=@m_VMID";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var count = 0;
                    lblDate.Text = reader.GetDateTime(count).ToString("dd MMM yyyy");

                    count++;
                    var Presentation_From = reader.GetDateTime(count).ToString("hh:mm tt");

                    count++;
                    var Presentation_To = reader.GetDateTime(count).ToString("hh:mm tt");

                    lblTime.Text = Presentation_From + " - " + Presentation_To;

                    count++;
                    lblProgrammeName.Text = reader.GetString(count);

                    count++;
                    lblIntakeNo.Text = reader.GetInt32(count).ToString();

                    count++;
                    lblVenue.Text = reader.GetString(count);
                    bool m_DecisionCompleted = false;
                    bool m_MeetingCompleted = false;
                    if (!reader.IsDBNull(reader.GetOrdinal("Meeting_Completed")))
                        lblMeetingStatus.Text = "Meeting Completed";
                    else if (!reader.IsDBNull(reader.GetOrdinal("Decision_Completed")))
                        lblMeetingStatus.Text = "Stage 1 Completed";
                    else
                        lblMeetingStatus.Text = "Pending to Start";
                }
                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        private void BindGridViewApplicationList(bool MeetingConfirmed)
        {
            ConnectOpen();
            try
            {
                var sqlColumn = "select tba.Presentation_From,"
                                + "tba.Presentation_To,"
                                + "tba.Application_Number,"
                                + "isnull(tApp.Business_Area,'') as Business_Area,"
                                + "isnull(tsScore.Total_Score,-1) as Total_Score,"
                                + "tba.Vetting_Application_ID ";
                var sqlFrom = " from TB_VETTING_APPLICATION tba ";
                var sqlWhere = " where tba.Vetting_Meeting_ID = @m_VMID and tApp.Status <> 'Saved' and (tApp.Status = 'Complete Screening' or tApp.Status = 'Presentation Withdraw')";
                var sqlOrder = " order by tba.Presentation_From ";

                var sqlColumnProjectDesc = "";
                var sqlScreeningScoreTable = "";
                var BDMRole = "";

                if (lblProgrammeName.Text.Contains("Cyberport Incubation Program"))
                {
                    //CPIP
                    sqlColumn += " ,isNull(tApp.Company_Name_Eng,'') as Company_Name_Eng,"
                                + "tApp.Incubation_ID,tApp.Programme_ID";
                    sqlFrom += " inner join TB_INCUBATION_APPLICATION tApp on tApp.Application_Number = tba.Application_Number "
                                + "left join TB_PRESENTATION_INCUBATION_SCORE tsScore on tsScore.Application_Number = tba.Application_Number and tsScore.Member_Email = @Email";

                    sqlColumnProjectDesc = " ,tApp.Abstract as ProjectDesc ";
                    sqlScreeningScoreTable = "TB_SCREENING_INCUBATION_SCORE";
                    BDMRole = "CPIP BDM";

                    GridViewApplicationList.Columns[GridViewColumnOrder["ProjectName"]].Visible = false;
                    GridViewApplicationList.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = false;
                    GridViewApplicationList.Columns[GridViewColumnOrder["ApplicationType"]].Visible = false;
                    GridViewApplicationList.Columns[GridViewColumnOrder["CompanyName"]].Visible = true;
                }
                else
                {
                    //CCMF
                    sqlColumn += " ,isNull(tApp.Project_Name_Eng,'') as Project_Name_Eng,"
                                + "isNull(tApp.Programme_Type,'') as Programme_Type,"
                                + "isNull(tApp.CCMF_Application_Type,'') as CCMF_Application_Type,"
                                + "tApp.CCMF_ID,tApp.Programme_ID";
                    sqlFrom += " inner join TB_CCMF_APPLICATION tApp on tApp.Application_Number = tba.Application_Number "
                                + "left join TB_PRESENTATION_CCMF_SCORE tsScore on tsScore.Application_Number = tba.Application_Number and tsScore.Member_Email = @Email ";

                    sqlColumnProjectDesc = " ,tApp.Abstract_Eng as ProjectDesc ";
                    sqlScreeningScoreTable = "TB_SCREENING_CCMF_SCORE";
                    BDMRole = "CCMF BDM";

                    GridViewApplicationList.Columns[GridViewColumnOrder["ProjectName"]].Visible = true;
                    GridViewApplicationList.Columns[GridViewColumnOrder["ProgrammeType"]].Visible = true;
                    GridViewApplicationList.Columns[GridViewColumnOrder["ApplicationType"]].Visible = true;
                    GridViewApplicationList.Columns[GridViewColumnOrder["CompanyName"]].Visible = false;
                }

                sqlColumn += " ,case when tsScore.Go is null then 3 else tsScore.Go end as go,isnull(tsScore.Remarks,'') as Remarks ";
                sqlColumn += ",tsScore.Comments as Comments";

                sqlColumn += sqlColumnProjectDesc;
                sqlColumn += " ,isNull(tbAppShortlisting.Remarks_To_Vetting,'') as Remarks_To_Vetting ";
                sqlColumn += " ,isNull(case when (tbsscoreBDM.BDM_Score is null and tbsscoreSeniorManager.SeniorManager_Score is null and tbsscoreCPMO.CPMO_Score is null) then -1 when (tbsscoreBDM.BDM_Score + tbsscoreSeniorManager.SeniorManager_Score + tbsscoreCPMO.CPMO_Score) = 0 then 0 else (isNull(tbsscoreBDM.BDM_Score,0) +isNull(tbsscoreSeniorManager.SeniorManager_Score,0) +isNull(tbsscoreCPMO.CPMO_Score,0))/ nullif(((case when isNull(tbsscoreBDM.BDM_Score,0) = 0 then 0 else 1 end)+(case when isNull(tbsscoreSeniorManager.SeniorManager_Score,0) = 0 then 0 else 1 end) + (case when isNull(tbsscoreCPMO.CPMO_Score,0) = 0 then 0 else 1 end)) ,0) end ,-1) as Average_Score ";
                sqlColumn += " ,case when tvDECISION.Go is null then 3 else tvDECISION.Go end as tvDECISIONGo ";

                sqlFrom += " LEFT JOIN TB_APPLICATION_SHORTLISTING tbAppShortlisting on tbAppShortlisting.Application_Number = tApp.Application_Number and tbAppShortlisting.Programme_ID = tApp.Programme_ID ";
                sqlFrom += " left join (select tbsscore.Total_Score as BDM_Score,tbsscore.Remarks as BDM_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='" + BDMRole + "') tbsscoreBDM on tbsscoreBDM.Application_Number = tApp.Application_Number left join (select tbsscore.Total_Score as SeniorManager_Score,tbsscore.Remarks as SeniorManager_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='Senior Manager') tbsscoreSeniorManager on tbsscoreSeniorManager.Application_Number = tApp.Application_Number left join (select tbsscore.Total_Score as CPMO_Score,tbsscore.Remarks as CPMO_Remarks,tbsscore.Application_Number from " + sqlScreeningScoreTable + " tbsscore where tbsscore.Role='CPMO') tbsscoreCPMO on tbsscoreCPMO.Application_Number = tApp.Application_Number ";
                //sqlFrom += " left join TB_VETTING_DECISION tvd on tvd.Application_Number = tba.Application_Number and tvd.Vetting_Meeting_ID = tba.Vetting_Meeting_ID and tvd.Member_Email = @Email ";

                sqlFrom += " left join TB_VETTING_DECISION tvDECISION on  tvDECISION.Vetting_Meeting_ID = tba.Vetting_Meeting_ID and tvDECISION.Application_Number = tba.Application_Number and tvDECISION.Member_Email = @Email ";

                var sqlString = sqlColumn + sqlFrom + sqlWhere + sqlOrder;

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Name.ToString()));
                List<ApplicationList> applicationList = new List<ApplicationList>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ApplicationList app = new ApplicationList();
                    var count = 0;
                    var Presentation_From = reader.GetDateTime(count).ToString("hh:mm");

                    count++;
                    var Presentation_To = reader.GetDateTime(count).ToString("hh:mm tt");
                    app.TimeSlot = Presentation_From + " - " + Presentation_To;

                    count++;
                    app.ApplicationNo = reader.GetString(count);

                    count++;
                    app.Cluster = reader.GetString(count);

                    count++;
                    app.Score = float.Parse(reader.GetDecimal(count).ToString()).ToString();

                    count++;
                    app.VAID = reader.GetGuid(count).ToString();

                    if (lblProgrammeName.Text.Contains("Cyberport Incubation Program"))
                    {
                        //CPIP
                        count++;
                        app.CompanyName = reader.GetString(count);

                        count++;
                        app.ApplicationID = reader.GetGuid(count).ToString();

                        count++;
                        var programId = reader.GetInt32(count).ToString();

                        app.APPNoURL = "/SitePages/IncubationProgram.aspx?app=" + app.ApplicationID + "&prog=" + programId;


                    }
                    else
                    {
                        //CCMF
                        count++;
                        app.ProjectName = reader.GetString(count);

                        count++;
                        app.ProgrammeType = reader.GetString(count);

                        count++;
                        app.ApplicationType = reader.GetString(count);

                        count++;
                        app.ApplicationID = reader.GetGuid(count).ToString();

                        count++;
                        var programId = reader.GetInt32(count).ToString();

                        app.APPNoURL = "/SitePages/CCMF.aspx?app=" + app.ApplicationID + "&prog=" + programId;


                    }

                    count++;
                    app.GoOrNotgo = reader.GetInt32(count).ToString();
                    if (int.Parse(app.GoOrNotgo) == 3)
                    {
                        app.GoOrNotgo = "";
                    }
                    else if (int.Parse(app.GoOrNotgo) > 0)
                    {
                        app.GoOrNotgo = "Yes";
                    }
                    else
                    {
                        app.GoOrNotgo = "No";
                    }
                    count++;
                    app.Remarks = reader.GetString(count);
                    app.ProjectDescription = reader.GetValue(reader.GetOrdinal("ProjectDesc")).ToString().Replace("\r\n", "<br/>").Replace("\n\r", "<br/>");
                    app.RemarksForVetting = reader.GetValue(reader.GetOrdinal("Remarks_To_Vetting")).ToString();
                    app.CommentsForVetting = reader.GetValue(reader.GetOrdinal("Comments")).ToString();

                    var AverageScore = float.Parse(reader.GetValue(reader.GetOrdinal("Average_Score")).ToString());
                    app.AverageScore = AverageScore.ToString("F3", CultureInfo.InvariantCulture);

                    var DecisionGoNotGO = reader.GetInt32(reader.GetOrdinal("tvDECISIONGo")).ToString();
                    if (int.Parse(DecisionGoNotGO) == 3)
                    {
                        //go is null, defalut value true
                        app.DecisionGoNotGO = true;
                        // app.EnabledDecisionGoNotGO = true;
                    }
                    else if (int.Parse(DecisionGoNotGO) > 0)
                    {
                        app.DecisionGoNotGO = true;
                        //app.EnabledDecisionGoNotGO = true;
                    }
                    else
                    {
                        app.DecisionGoNotGO = false;
                    }
                    app.EnabledDecisionGoNotGO = MeetingConfirmed;

                    applicationList.Add(app);
                }
                hdnVAIDList.Value = string.Join(",", applicationList.Select(x => x.VAID));
               
                GridViewApplicationList.DataSource = applicationList;
                GridViewApplicationList.DataBind();

                reader.Dispose();
                command.Dispose();

            }
            finally
            {
                ConnectClose();
            }
        }

        private void SetGridViewColumnOrder()
        {
            GridViewColumnOrder = new Dictionary<String, int>();
            GridViewColumnOrder.Add("TimeSlot", 0);
            GridViewColumnOrder.Add("ApplicationNo", 1);
            GridViewColumnOrder.Add("CompanyName", 2);
            GridViewColumnOrder.Add("ProjectName", 3);
            GridViewColumnOrder.Add("ProgrammeType", 4);
            GridViewColumnOrder.Add("ApplicationType", 5);
            GridViewColumnOrder.Add("ProjectDescription", 6);
            GridViewColumnOrder.Add("Cluster", 7);
            GridViewColumnOrder.Add("AverageScore", 8);
            GridViewColumnOrder.Add("RemarksforVetting", 9);
            GridViewColumnOrder.Add("Score", 10);
            GridViewColumnOrder.Add("GoOrNotgo", 11);
            GridViewColumnOrder.Add("Comments", 12);
            GridViewColumnOrder.Add("Remarks", 13);
            GridViewColumnOrder.Add("DecisionGoNotGO", 14);
        }

        protected void GridViewApplicationList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            SetGridViewColumnOrder();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var LinkButtonScore = e.Row.FindControl("LinkButtonScore") as LinkButton;
                var ImageButtonScore = e.Row.FindControl("ImageButtonEdit") as ImageButton;
                if (LinkButtonScore.Text == "-1")
                {
                    LinkButtonScore.Text = "";
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (CheckDecisionIsCompleted() == true)
            {
                //pnlWarning.Visible = true;
                PopupTeamConfirmed.Visible = true;
                btnConfirm.Enabled = false;
                //return;
            }
            else
            {
                FinalSubmit();



                //Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");
            }

            //SubmitPopup.Visible = true;
            //comented in 2018
            //txtUserName.Text = objFUnction.GetCurrentUser();            
        }

        protected void imbClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlWarning.Visible = false;

            Context.Response.Redirect("~/SitePages/Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID, false);
        }

        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            SubmitPopup.Visible = false;
        }

        protected string HasVettDecision(string AppNo)
        {
            string AppId = null;
            ConnectOpen();
            try
            {
                var sqlString = "SELECT Application_Number FROM [TB_VETTING_DECISION] where Vetting_Meeting_ID = @m_VMID and Member_Email= @Email and Application_Number = @App_No";
                var command = new SqlCommand(sqlString, connection);

                command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Name.ToString()));
                command.Parameters.Add(new SqlParameter("@App_No", AppNo));
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AppId = reader.GetString(0);
                }

                reader.Dispose();
                command.Dispose();

            }
            finally
            {
                ConnectClose();
            }
            return AppId;
        }
        protected void btn_submitFinal_Click(object sender, EventArgs e)
        {
            try
            {
                //comented in 2018
                //if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                //{
                //    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtUserName.Text, txtLoginPassword.Text);

                //    if (!status)
                //    {
                //        SubmitPopup.Visible = true;
                //        UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                //    }
                //    else
                //    {
                SubmitPopup.Visible = false;

                ConnectOpen();
                try
                {

                    var sqlString = "select Vetting_Member_ID from TB_VETTING_MEMBER_INFO where Email=@Email and Disabled = 0";

                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add("@Email", SPContext.Current.Web.CurrentUser.Name.ToString());

                    var reader = command.ExecuteReader();

                    String memberID = "";
                    while (reader.Read())
                    {
                        memberID = reader.GetGuid(0).ToString();
                    }

                    reader.Dispose();
                    command.Dispose();

                    if (memberID != "" && m_VMID != null)
                    {
                        var sqlUpdate = "update TB_VETTING_MEMBER set "
                                                + "PList_Confirmed = 1 "
                                                + "where Vetting_Meeting_ID=@Vetting_Meeting_ID and Vetting_Member_ID=@Vetting_Member_ID";
                        command = new SqlCommand(sqlUpdate, connection);
                        command.Parameters.Add("@Vetting_Meeting_ID", m_VMID);
                        command.Parameters.Add("@Vetting_Member_ID", memberID);

                        command.ExecuteNonQuery();


                        command.Dispose();
                    }

                    //insert to TB_VETTING_DECISION
                    SetGridViewColumnOrder();

                    foreach (GridViewRow row in GridViewApplicationList.Rows)
                    {
                        var CheckBoxGonotGo = (CheckBox)row.FindControl("CheckBoxGonotGo");
                        var CheckedGonotGo = "0";
                        if (CheckBoxGonotGo.Checked)
                        {
                            CheckedGonotGo = "1";
                        }
                        //CheckBoxGonotGo.Enabled = false;
                        var ScrAappNo = ((HyperLink)row.Cells[GridViewColumnOrder["ApplicationNo"]].Controls[0]).Text;
                        string appNo = HasVettDecision(ScrAappNo);
                        if (appNo != null)
                            UpdateTB_VETTING_DECISION(m_VMID, appNo, CheckedGonotGo);
                        else
                            InsertTB_VETTING_DECISION(m_VMID, ScrAappNo, CheckedGonotGo);
                    }

                    btnConfirm.Text = "Reconfirm Presentation List";

                    //Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");

                }
                finally
                {
                    ConnectClose();
                }
                // commented in2018
                //    }
                //}
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ext)
            {
                string rs = "";
                //foreach (var eve in ext.EntityValidationErrors)
                //{
                //    rs = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    Console.WriteLine(rs);

                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        rs += "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //    }
                //  }
                // commented in2018
                // UserCustomerrorLogin.InnerText = rs;
            }
            catch (Exception ex)
            {
                // commented in2018
                // UserCustomerrorLogin.InnerText = ex.Message;
            }

        }

        protected void FinalSubmit()
        {
            try
            {
                //comented in 2018
                //if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                //{
                //    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtUserName.Text, txtLoginPassword.Text);

                //    if (!status)
                //    {
                //        SubmitPopup.Visible = true;
                //        UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                //    }
                //    else
                //    {

                ConnectOpen();
                try
                {

                    var sqlString = "select Vetting_Member_ID from TB_VETTING_MEMBER_INFO where Email=@Email and Disabled = 0";

                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add("@Email", SPContext.Current.Web.CurrentUser.Name.ToString());

                    var reader = command.ExecuteReader();

                    String memberID = "";
                    while (reader.Read())
                    {
                        memberID = reader.GetGuid(0).ToString();
                    }

                    reader.Dispose();
                    command.Dispose();

                    if (memberID != "" && m_VMID != null)
                    {
                        var sqlUpdate = "update TB_VETTING_MEMBER set "
                                                + "PList_Confirmed = 1 "
                                                + "where Vetting_Meeting_ID=@Vetting_Meeting_ID and Vetting_Member_ID=@Vetting_Member_ID";
                        command = new SqlCommand(sqlUpdate, connection);
                        command.Parameters.Add("@Vetting_Meeting_ID", m_VMID);
                        command.Parameters.Add("@Vetting_Member_ID", memberID);

                        command.ExecuteNonQuery();


                        command.Dispose();
                    }

                    //insert to TB_VETTING_DECISION
                    SetGridViewColumnOrder();

                    foreach (GridViewRow row in GridViewApplicationList.Rows)
                    {
                        var CheckBoxGonotGo = (CheckBox)row.FindControl("CheckBoxGonotGo");
                        var CheckedGonotGo = "0";
                        if (CheckBoxGonotGo.Checked)
                        {
                            CheckedGonotGo = "1";
                        }
                        //CheckBoxGonotGo.Enabled = false;
                        var ScrAappNo = ((HyperLink)row.Cells[GridViewColumnOrder["ApplicationNo"]].Controls[0]).Text;
                        string appNo = HasVettDecision(ScrAappNo);
                        if (appNo != null)
                            UpdateTB_VETTING_DECISION(m_VMID, appNo, CheckedGonotGo);
                        else
                            InsertTB_VETTING_DECISION(m_VMID, ScrAappNo, CheckedGonotGo);
                    }
                    SubmitPopup.Visible = true;


                    btnConfirm.Text = "Reconfirm Presentation List";

                    //Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");

                }
                finally
                {
                    ConnectClose();
                }
                // commented in2018
                //    }
                //}
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ext)
            {
                string rs = "";
                //foreach (var eve in ext.EntityValidationErrors)
                //{
                //    rs = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    Console.WriteLine(rs);

                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        rs += "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //    }
                //  }
                // commented in2018
                // UserCustomerrorLogin.InnerText = rs;
            }
            catch (Exception ex)
            {
                // commented in2018
                // UserCustomerrorLogin.InnerText = ex.Message;
            }

        }

        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Incubation");
        }

        public bool IsPreListConfirmedByMember()
        {
            ConnectOpen();
            bool Confirmed = false;
            try
            {
                var sqlString = "select case when TB_VETTING_MEMBER.PList_Confirmed is null then 3 else cast(TB_VETTING_MEMBER.PList_Confirmed as int) end PList_Confirmed  from  TB_VETTING_MEMBER inner join TB_VETTING_MEMBER_INFO on TB_VETTING_MEMBER.Vetting_Member_ID = TB_VETTING_MEMBER_INFO.Vetting_Member_ID where TB_VETTING_MEMBER.Vetting_Meeting_ID = @Vetting_Meeting_ID and TB_VETTING_MEMBER_INFO.Email = @Email and TB_VETTING_MEMBER_INFO.Disabled = 0 ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@Vetting_Meeting_ID", m_VMID);
                command.Parameters.Add("@Email", SPContext.Current.Web.CurrentUser.Name.ToString());

                var reader = command.ExecuteReader();
                var status = true;
                while (reader.Read())
                {
                    var PList_Confirmed = reader.GetInt32(0);
                    if (PList_Confirmed == 1)
                    {
                        Confirmed = true;
                    }

                }

                reader.Dispose();
                command.Dispose();

            }
            finally
            {
                ConnectClose();
            }
            return Confirmed;
        }

        public bool CheckWhetherConfirm()
        {
            ConnectOpen();
            var status = true;
            try
            {

                //var sqlString = "select case when TB_VETTING_MEMBER.PList_Confirmed is null then 3 else cast(TB_VETTING_MEMBER.PList_Confirmed as int) end PList_Confirmed  from  TB_VETTING_MEMBER inner join TB_VETTING_MEMBER_INFO on TB_VETTING_MEMBER.Vetting_Member_ID = TB_VETTING_MEMBER_INFO.Vetting_Member_ID where TB_VETTING_MEMBER.Vetting_Meeting_ID = @Vetting_Meeting_ID and TB_VETTING_MEMBER_INFO.Email = @Email and TB_VETTING_MEMBER_INFO.Disabled = 0 ";
                var sqlString = "select [Decision_Completed] from  [dbo].[TB_VETTING_MEETING] where Vetting_Meeting_ID = @Vetting_Meeting_ID ";

                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add("@Vetting_Meeting_ID", m_VMID);
                //command.Parameters.Add("@Email", SPContext.Current.Web.CurrentUser.Name.ToString());

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //var PList_Confirmed = reader.GetInt32(0);
                    //if (PList_Confirmed == 1)
                    if (!reader.IsDBNull(0))
                    {
                        status = false;
                    }

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");
        }

        protected void GridViewApplicationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button 
                // from the Rows collection.
                GridViewRow row = GridViewApplicationList.Rows[index];
                var hiddenFieldVAID = row.FindControl("HiddenFieldVAID") as HiddenField;
               
                Context.Response.Redirect("Presentation Scoring Form.aspx?VMID=" + m_VMID + "&VAID=" + hiddenFieldVAID.Value+(
                    string.IsNullOrEmpty( hdnVAIDList.Value)?string.Empty:"&pagging="+ hdnVAIDList.Value
                    ));
            }
        }

        protected void btnDowmloadAllApplications_Click(object sender, EventArgs e)
        {


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
            m_programName = lblProgrammeName.Text.Trim();
            m_intake = lblIntakeNo.Text.Trim();

            string m_programId = get_proid(m_programName, m_intake);

            string sql = @"INSERT INTO [TB_DOWNLOAD_REQUEST] (Programme_Name, Programme_ID, Intake_Number, Request_Type, Status, User_Email, Vetting_Meeting_ID, Created_By, Created_Date) 
                           VALUES (@Programme_Name, @Programme_ID, @Intake_Number, 'Download Shortlisted Application Files', 0, @User_Email, @vmID, @User, GETDATE())";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Programme_Name", m_programName);
                    cmd.Parameters.AddWithValue("@Intake_Number", m_intake);
                    cmd.Parameters.AddWithValue("@Programme_ID", m_programId);
                    cmd.Parameters.AddWithValue("@User_Email", m_mail);
                    cmd.Parameters.AddWithValue("@vmID", m_VMID);
                    cmd.Parameters.AddWithValue("@User", m_username);

                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            string m_subject = "Zip File for shortlisted applications : " + m_programName + " - " + m_intake + " is processing.";
            string m_body = getEmailTemplate("ZipDownloadStartEmail");

            sharepointsendemail(m_mail, m_subject, m_body);
            lbldownloadmessage.Text = "Download Complete.";
            //


            //string m_intake = lblIntakeNo.Text.Trim();
            //string m_programName = lblProgrammeName.Text.Trim();

            //if (m_programName.Contains("Incubation"))
            //    m_programName = "CPIP";
            //else if (m_programName.Contains("Micro"))
            //    m_programName = "CCMF";
            //else
            //    m_programName = "CUPP";


            //var FileName = "SA_" + m_programName + "_" + m_intake + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            //String Destination = m_path + @"\" + FileName;
            //processFolder(Destination, FileName);
        }

        //Get Programme id 
        private string get_proid(string Programme_Name, string Intake_Number)
        {
            string m_prog_id = "";
            string sql_string = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                sql_string = "SELECT * FROM TB_PROGRAMME_INTAKE where Programme_Name=@Programme_Name and Intake_Number=@Intake_Number";
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

        public void processFolder(String Destination, String FileName)
        {
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



            if (Page.IsValid)
            {
                var ApplicationIDs = new List<String>();
                //var ApplicationID = "";
                foreach (GridViewRow row in GridViewApplicationList.Rows)
                {
                    var AppID = row.FindControl("HiddenFieldApplicationID") as HiddenField;
                    //ApplicationID += "'" + AppID.Value + "',";
                    ApplicationIDs.Add(AppID.Value);
                }
                //ApplicationID = ApplicationID.Substring(0, ApplicationID.Length - 1);
                var filePaths = GetPresentationSlideAttachment(ApplicationIDs);
                if (filePaths.Count > 0)
                {
                    m_downloadLink = m_zipfiledownloadurl;
                    string m_subject = "";
                    string m_body = "";
                    string m_Programme_Name = lblProgrammeName.Text;
                    string m_Intake_Number = lblIntakeNo.Text;
                    string m_Password;
                    string m_downloadlink = m_downloadLink;
                    string m_Starttime;
                    string m_Endtime;
                    //string m_zipstatus = "done";



                    //starting email
                    m_subject = "Zip File for shortlisted applications : " + m_Programme_Name + " - " + m_Intake_Number + " is processing.";
                    //m_body = "Hi, Zip File is processing, please wait next email to confirm Zip File is ready.";
                    m_body = getEmailTemplate("ZipDownloadStartEmail");
                    sharepointsendemail(m_mail, m_subject, m_body);


                    /*************************/
                    //zip programm in here:
                    m_Starttime = DateTime.Now.ToString();
                    //SPSite site = SPContext.Current.Site;
                    //SPWeb web = site.OpenWeb();
                    SPUtility.ValidateFormDigest();
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                            {
                                ErrorLog("Start", "Download started");
                                ZipOutputStream zipStream = new ZipOutputStream(File.Create(Destination));

                                zipStream.SetLevel(9); //0-9, 9 being the highest level of compression

                                zipStream.Password = genRandom(8);	// optional. Null is the same as not setting. Required if using AES.
                                m_Password = zipStream.Password;
                                //lbldatetime.Text = lbldatetime.Text + "   "+zipStream.Password;

                                CompressFolder(web, zipStream, filePaths);
                                zipStream.Finish();
                                //zipStream.IsStreamOwner = false;	
                                zipStream.Close();

                                m_Endtime = DateTime.Now.ToString();
                                /*************************/

                                //insert into TB_Download_ZIP
                                InsertTBDownloadZIP(m_username, m_mail, "ZIP", Destination, FileName, m_Password, "1");

                                //Completed email  

                                m_subject = "Zip File for shortlisted applications : " + m_Programme_Name + " - " + m_Intake_Number + " is completed.";
                                m_body = getEmailTemplate("ZipDownloadEndEmailApp");
                                m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Programme_Name", m_Programme_Name).Replace("@@m_Intake_Number", m_Intake_Number).Replace("@@m_FileName", FileName).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                                sharepointsendemail(m_mail, m_subject, m_body);
                                //sharepointsendemail("andysgi@gmail.com", "hi", "ko");
                                lbldownloadmessage.Text = "Download Complete.";

                                ErrorLog("End", "Download completed");
                            }
                        }
                    });

                }
                else
                {
                    lbldownloadmessage.Text = "No found files";
                }

            }
        }

        private void CompressFolder(SPWeb web, ZipOutputStream zipStream, List<String> filePaths)
        {

            ErrorLog("Paths", filePaths.Count.ToString());
            foreach (var file in filePaths)
            {
                String entryName = "";
                try
                {
                    var DeletepathName = "";
                    if (!string.IsNullOrEmpty(m_AttachmentPrimaryFolderName))
                    {
                        DeletepathName += m_AttachmentPrimaryFolderName + @"\";
                    }
                    if (!string.IsNullOrEmpty(m_AttachmentSecondaryFolderName))
                    {
                        DeletepathName += m_AttachmentSecondaryFolderName + @"\";
                    }

                    entryName = file.Substring(DeletepathName.Length);
                    ZipEntry entry = new ZipEntry(entryName);
                    entry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(entry);
                    var size = file.Length / 1024;

                    //SPUser ensure = web.EnsureUser(ImpersonateUser);
                    //SPSite impSite = new SPSite(web.Site.Url, ensure.UserToken);
                    //SPWeb impWeb = impSite.OpenWeb();

                    var filObject = web.GetFile(file);
                    if (filObject.Exists)
                    {

                        ErrorLog("File name", entryName + " (" + size.ToString("N2") + " KB)");
                        byte[] binary = filObject.OpenBinary();
                        zipStream.Write(binary, 0, binary.Length);

                        m_DownloadFileSize = m_DownloadFileSize + size;
                        ErrorLog("Total file size", m_DownloadFileSize.ToString("N2") + " KB");
                    }

                }
                catch (Exception ex)
                {
                    ErrorLog("Error", ex.Message);
                    var str = entryName + " ";
                    ErrorLog("Error file", str);
                }

            }

        }

        private List<String> GetPresentationSlideAttachment(List<String> ApplicationIDS)
        {
            List<String> AttachmentPaths = new List<String>();
            ConnectOpen();
            try
            {
                //var sqlString = String.Format("select Attachment_Type,Attachment_Path from TB_APPLICATION_ATTACHMENT where Application_ID in ({0}) and Attachment_Type <> 'Video_Clip' and Attachment_Type <> 'Presentation_Slide_Response' and Attachment_Type <> 'HK_ID' and Attachment_Type <> 'BR_COPY' and Attachment_Type <> 'Student_ID' and Attachment_Type <> 'Company_Annual_Return';", String.Join(",", ApplicationIDS.Select((x, i) => "@ApplicationID" + i).ToArray()));
                var sqlString = String.Format("select Attachment_Type,Attachment_Path from TB_APPLICATION_ATTACHMENT where Application_ID in ({0}) and Attachment_Type <> 'HK_ID' and Attachment_Type <> 'BR_COPY' and Attachment_Type <> 'Video_Clip' and Attachment_Type <> 'Student_ID' and Attachment_Type <> 'Company_Annual_Return' and Attachment_Type <> 'Presentation_Slide_Response';", String.Join(",", ApplicationIDS.Select((x, i) => "@ApplicationID" + i).ToArray()));
                var command = new SqlCommand(sqlString, connection);
                for (int i = 0; i < ApplicationIDS.Count; i++)
                    command.Parameters.Add("@ApplicationID" + i, ApplicationIDS[i]);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AttachmentPaths.Add(reader.GetString(1));
                }

                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            return AttachmentPaths;

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (m_ApplicationIsInDebug == "1")
            {
                if (string.IsNullOrEmpty(m_ApplicationDebugEmailSentTo))
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDowmloadAllApplications.Enabled = false;
                    args.IsValid = true;
                }
            }
            else
            {
                string m_mail = SPContext.Current.Web.CurrentUser.Email;
                if (string.IsNullOrEmpty(m_mail))
                {
                    args.IsValid = false;
                }
                else
                {
                    btnDowmloadAllApplications.Enabled = false;
                    args.IsValid = true;
                }
            }

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

                    if (reader.GetString(0) == "ImpersonateUser")
                    {
                        ImpersonateUser = reader.GetString(1);

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

        protected String getEmailTemplate(string emailTemplate)
        {
            String emailTemplateContent = "";
            ConnectOpen();
            try
            {
                var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template=@emailTemplate;";

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

        protected void sharepointsendemail(string toAddress, string subject, string body)
        {
            SPSecurity.RunWithElevatedPrivileges(
                             delegate ()
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
                command.Parameters.Add(new SqlParameter("@User_Name", User_Name));
                command.Parameters.Add(new SqlParameter("@Email", Email));
                command.Parameters.Add(new SqlParameter("@type", type));
                command.Parameters.Add(new SqlParameter("@Path", Path));
                command.Parameters.Add(new SqlParameter("@File_Name", File_Name));
                command.Parameters.Add(new SqlParameter("@Password", Password));
                command.Parameters.Add(new SqlParameter("@Status", Status));
                command.Parameters.Add(new SqlParameter("@User", SPContext.Current.Web.CurrentUser.Name.ToString()));

                command.ExecuteNonQuery();

                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void InsertTB_VETTING_DECISION(String Vetting_Meeting_ID, String Application_Number, String Go)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "insert into TB_VETTING_DECISION(Vetting_Delclaration_ID,Vetting_Meeting_ID,Application_Number,Member_Email,Go) values("
                                    + "@Vetting_Delclaration_ID, "
                                    + "@Vetting_Meeting_ID, "
                                    + "@Application_Number, "
                                    + "@Member_Email, "
                                    + "@Go"
                                    + " ) ;";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add(new SqlParameter("@Vetting_Delclaration_ID", Guid.NewGuid()));
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", Vetting_Meeting_ID));
                command.Parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                command.Parameters.Add(new SqlParameter("@Member_Email", SPContext.Current.Web.CurrentUser.Name.ToString()));
                command.Parameters.Add(new SqlParameter("@Go", Go));

                command.ExecuteNonQuery();

                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void UpdateTB_VETTING_DECISION(String Vetting_Meeting_ID, String Application_Number, String Go)
        {
            ConnectOpen();
            try
            {
                var sqlUpdate = "Update TB_VETTING_DECISION Set Go = @Go where Vetting_Meeting_ID = @Vetting_Meeting_ID and Application_Number = @Application_Number and Member_Email = @Member_Email";

                var command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.Add(new SqlParameter("@Go", Go));
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", Vetting_Meeting_ID));
                command.Parameters.Add(new SqlParameter("@Application_Number", Application_Number));
                command.Parameters.Add(new SqlParameter("@Member_Email", SPContext.Current.Web.CurrentUser.Name.ToString()));

                command.ExecuteNonQuery();

                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void btnPresentationResultSummary_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Presentation Result Summary.aspx?VMID=" + m_VMID);
        }

        protected void btnDecisionSummary_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("DecisionSummary.aspx?VMID=" + m_VMID);

        }

        public void ErrorLog(string type, string message)
        {
            getSYSTEMPARAMETER();
            if (IsLogEvent == "1")
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(m_path + @"\logDownload.txt", true))
                {
                    try
                    {
                        file.WriteLine(string.Format("{0}  [{1}] {2}", DateTime.Now, type, message));
                    }
                    catch
                    {

                    }
                }
            }
        }

        protected void ImageButtonClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SubmitPopup.Visible = false;
            //  Context.Response.Redirect("Application%20List%20for%20Vetting%20Team.aspx");
        }

        protected void ImageButtonTemClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            PopupTeamConfirmed.Visible = false;
            return;
        }
    }

    public class ApplicationList
    {
        public string TimeSlot { get; set; }
        public string ApplicationNo { get; set; }
        public string Cluster { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string ProgrammeType { get; set; }
        public string ApplicationType { get; set; }
        public string Score { get; set; }
        public string VAID { get; set; }
        public string ApplicationID { get; set; }
        public string GoOrNotgo { get; set; }
        public string Remarks { get; set; }
        public string APPNoURL { get; set; }

        public string ProjectDescription { get; set; }
        public string AverageScore { get; set; }
        public string RemarksForVetting { get; set; }
        public string CommentsForVetting { get; set; }

        public Boolean DecisionGoNotGO { get; set; }
        public Boolean EnabledDecisionGoNotGO { get; set; }


    }
}
