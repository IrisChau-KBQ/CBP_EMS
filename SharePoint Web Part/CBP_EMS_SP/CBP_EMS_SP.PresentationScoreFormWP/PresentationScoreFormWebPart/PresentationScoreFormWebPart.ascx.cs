using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace CBP_EMS_SP.PresentationScoreFormWP.PresentationScoreFormWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class PresentationScoreFormWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public PresentationScoreFormWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportWMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private string connStr
        {
            get
            {
               // return System.Configuration.ConfigurationManager.ConnectionStrings["EmsSqlConStr"].ConnectionString;

                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }
        private SqlConnection connection;
        private string m_VMID;
        private string m_VAID;
        private string m_PaggingVAID;
        private String m_Role;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_VMID = HttpContext.Current.Request.QueryString["VMID"];
            m_VAID = HttpContext.Current.Request.QueryString["VAID"];
            m_PaggingVAID = HttpContext.Current.Request.QueryString["pagging"];
            if (CheckUser())
            {
                if (!Page.IsPostBack)
                {
                    SetScreenValue();
                    CheckDisable();
                }
            }
        }

        public Boolean CheckUser()
        {
            if (m_VAID != null && m_VMID != null)
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

        public void CheckDisable()
        {
            if (lblProgrammeName.Text.Contains("Cyberport Incubation Program"))
            {
                //CPIP
                UpdatePanelCPIP.Visible = true;
                UpdatePanelCCMF.Visible = false;

            }
            else
            {
                //CCMF
                UpdatePanelCPIP.Visible = false;
                UpdatePanelCCMF.Visible = true;
            }

            if (CheckMeetingIsCompleted())
            {
                btnCPIPSubmit.Enabled = false;
                btnCCMFSubmit.Enabled = false;
            }
            else
            {
                btnCPIPSubmit.Enabled = true;
                btnCCMFSubmit.Enabled = true;
            }
        }

        public void SetScreenValue()
        {
            if (m_VAID != null && m_VMID != null)
            {
                ConnectOpen();
                try
                {
                    var searchResult = new SearchResult();
                    var sqlColumn = "SELECT  TB_PROGRAMME_INTAKE.Programme_Name, TB_PROGRAMME_INTAKE.Intake_Number,TB_VETTING_MEETING.Date,TB_VETTING_MEETING.Presentation_From,TB_VETTING_MEETING.Presentation_To, TB_VETTING_APPLICATION.Application_Number,TB_PROGRAMME_INTAKE.Programme_ID,isnull(TB_VETTING_APPLICATION.Email,'') as Email ";
                    var sqlFrom = " FROM TB_VETTING_MEETING INNER JOIN TB_VETTING_APPLICATION ON TB_VETTING_MEETING.Vetting_Meeting_ID = TB_VETTING_APPLICATION.Vetting_Meeting_ID INNER JOIN TB_PROGRAMME_INTAKE ON TB_VETTING_MEETING.Programme_ID = TB_PROGRAMME_INTAKE.Programme_ID  ";
                    var sqlWhere = " WHERE (TB_VETTING_MEETING.Vetting_Meeting_ID = @m_VMID) AND (TB_VETTING_APPLICATION.Vetting_Application_ID = @m_VAID) ";

                    var sqlString = sqlColumn + sqlFrom + sqlWhere;

                    var command = new SqlCommand(sqlString, connection);
                    command.Parameters.Add(new SqlParameter("@m_VMID", m_VMID));
                    command.Parameters.Add(new SqlParameter("@m_VAID", m_VAID));
                    command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Name.ToString()));

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var columnOrder = 0;
                        searchResult.ProgrammeName = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.IntakeNo = reader.GetInt32(columnOrder).ToString();

                        columnOrder++;
                        searchResult.Date = reader.GetDateTime(columnOrder).ToString("dd MMM yyyy");

                        columnOrder++;
                        var PresentFM = reader.GetDateTime(columnOrder).ToString("hh:mm tt");

                        columnOrder++;
                        var PresentTO = reader.GetDateTime(columnOrder).ToString("hh:mm tt");

                        searchResult.Time = PresentFM + " - " + PresentTO;

                        columnOrder++;
                        searchResult.ApplicationNo = reader.GetString(columnOrder);

                        columnOrder++;
                        searchResult.ProgrammeID = reader.GetInt32(columnOrder).ToString();

                        columnOrder++;
                        searchResult.MemberEmail = SPContext.Current.Web.CurrentUser.Name.ToString();
                    }


                    reader.Dispose();
                    command.Dispose();

                    if (searchResult.ProgrammeID != null)
                    {

                        lblProgrammeName.Text = searchResult.ProgrammeName;
                        lblIntakeNo.Text = searchResult.IntakeNo;
                        lblDate.Text = searchResult.Date;
                        lblTime.Text = searchResult.Time;
                        lblApplicationNo.Text = searchResult.ApplicationNo;

                        HiddenFieldMemberEmail.Value = searchResult.MemberEmail;
                        HiddenFieldProgramID.Value = searchResult.ProgrammeID;

                        sqlColumn = "";
                        sqlFrom = "";
                        if (searchResult.ProgrammeName.Contains("Cyberport Incubation Program"))
                        {
                            //CPIP
                            hdnApplicationName.Value = "cpip";
                            //sqlColumn = "select isNull(tbApplicatiion.Company_Name_Chi,'') as Company_Name_Chi,isnull(tpsore.Management_Team,0.01) as Management_Team,isnull(tpsore.Creativity,0.01) as Creativity,isnull(tpsore.Business_Viability,0.01) as Business_Viability,isnull(tpsore.Benefit_To_Industry,0.01) as Benefit_To_Industry,isnull(tpsore.Proposal_Milestones,0.01) as Proposal_Milestones,isnull(tpsore.Total_Score,0) as Total_Score,isnull(tpsore.Remarks,'') as Remarks,cast(tpsore.Go as int) go ";
                            sqlColumn = "select isNull(tbApplicatiion.Company_Name_Eng,'') as Company_Name_Eng,"
                                        + "isnull(tpsore.Management_Team,0.01) as Management_Team,"
                                        + "isnull(tpsore.Creativity,0.01) as Creativity,"
                                        + "isnull(tpsore.Business_Viability,0.01) as Business_Viability,"
                                        + "isnull(tpsore.Benefit_To_Industry,0.01) as Benefit_To_Industry,"
                                        + "isnull(tpsore.Proposal_Milestones,0.01) as Proposal_Milestones,"
                                        + "isnull(tpsore.Comments,'') as CPIP_Comment,"
                                        + "isnull(tpsore.Total_Score,0) as Total_Score,"
                                        + "isnull(tpsore.Remarks,'') as Remarks,cast(tpsore.Go as int) go ";
                            sqlFrom = " from TB_INCUBATION_APPLICATION tbApplicatiion left join TB_PRESENTATION_INCUBATION_SCORE tpsore on tpsore.Programme_ID = tbApplicatiion.Programme_ID and tpsore.Application_Number = tbApplicatiion.Application_Number and tpsore.Member_Email = @Email ";

                            lblCompanyName.Visible = true;
                            lblCompanyNameLabel.Visible = true;

                            lblProjectName.Visible = false;
                            lblProjectNameLabel.Visible = false;
                            lblProgrammeType.Visible = false;
                            lblProgrammeTypeLabel.Visible = false;
                            lblApplicationType.Visible = false;
                            lblApplicationTypeLabel.Visible = false;
                        }
                        else
                        {
                            //CCMF
                            hdnApplicationName.Value = "ccmf";
                            sqlColumn = "select isNull(tbApplicatiion.Project_Name_Eng,'') as Project_Name_Eng,"
                                        + "isNull(tbApplicatiion.Programme_Type,'') as Programme_Type,"
                                        + "isNull(tbApplicatiion.CCMF_Application_Type,'') as CCMF_Application_Type,"
                                        + "isnull(tpsore.Management_Team,0.01) as Management_Team,"
                                        + "isnull(tpsore.Business_Model,0.01) as Business_Model,"
                                        + "isnull(tpsore.Creativity,0.01) as Creativity,"
                                        + "isnull(tpsore.Social_Responsibility,0.01) as Social_Responsibility,"
                                        + "isnull(tpsore.Comments,'') as CCMF_Comment,"
                                        + "isnull(tpsore.Total_Score,0) as Total_Score,"
                                        + "isnull(tpsore.Remarks,'') as Remarks,cast(tpsore.Go as int) go ";
                            sqlFrom = " from TB_CCMF_APPLICATION tbApplicatiion left join TB_PRESENTATION_CCMF_SCORE tpsore on tpsore.Programme_ID = tbApplicatiion.Programme_ID and tpsore.Application_Number = tbApplicatiion.Application_Number and tpsore.Member_Email = @Email ";

                            lblCompanyName.Visible = false;
                            lblCompanyNameLabel.Visible = false;

                            lblProjectName.Visible = true;
                            lblProjectNameLabel.Visible = true;
                            lblProgrammeType.Visible = true;
                            lblProgrammeTypeLabel.Visible = true;
                            lblApplicationType.Visible = true;
                            lblApplicationTypeLabel.Visible = true;
                        }

                        sqlWhere = " where tbApplicatiion.Application_Number=@Application_Number;";
                        sqlString = sqlColumn + sqlFrom + sqlWhere;
                        command = new SqlCommand(sqlString, connection);
                        command.Parameters.Add(new SqlParameter("@Application_Number", searchResult.ApplicationNo));
                        command.Parameters.Add(new SqlParameter("@Email", SPContext.Current.Web.CurrentUser.Name.ToString()));


                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var columnOrder = 0;
                            if (searchResult.ProgrammeName.Contains("Cyberport Incubation Program"))
                            {
                                //CPIP
                                searchResult.CompanyName = reader.GetString(columnOrder);

                                columnOrder++;
                                var Management_Team = reader.GetDecimal(columnOrder);
                                if (Management_Team != (decimal)0.01)
                                {
                                    lstqcmt.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CPIPscorepresent("lstqcmt_Num")).ToString("N1");

                                    columnOrder++;
                                    lstcipp.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CPIPscorepresent("lstcipp_Num")).ToString("N1");

                                    columnOrder++;
                                    lstmbv.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CPIPscorepresent("lstmbv_Num")).ToString("N1");

                                    columnOrder++;
                                    lstbhkdti.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CPIPscorepresent("lstbhkdti_Num")).ToString("N1");

                                    columnOrder++;
                                    lstpsmpb.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CPIPscorepresent("lstpsmpb_Num")).ToString("N1");

                                    columnOrder++;
                                    //lstCPIPComment.SelectedValue = reader.GetString(columnOrder).ToString();

                                    string strSelectedComments = reader.GetString(columnOrder).ToString();
                                    if (!string.IsNullOrEmpty(strSelectedComments))
                                    {
                                        foreach (string strComment in strSelectedComments.Split(',').Where(x => !string.IsNullOrEmpty(x)))
                                        {
                                            System.Web.UI.WebControls.ListItem chkListItem = chkCPIPComments.Items.FindByValue(strComment);
                                            if (chkListItem != null)
                                                chkListItem.Selected = true;
                                        }
                                    }

                                    columnOrder++;
                                    lblTotalScore.Text = reader.GetDecimal(columnOrder).ToString();
                                    HiddenFieldTatalScore.Value = lblTotalScore.Text;

                                    columnOrder++;
                                    txtremarks.Text = reader.GetString(columnOrder);

                                    columnOrder++;
                                    RadioButtonListGoNotGo.SelectedValue = (reader.GetInt32(columnOrder)).ToString();
                                    hdnApplicationGoState.Value = RadioButtonListGoNotGo.SelectedValue;
                                }

                            }
                            else
                            {
                                //CCMF
                                searchResult.ProjectName = reader.GetString(columnOrder);

                                columnOrder++;
                                searchResult.ProgrammeType = reader.GetString(columnOrder);

                                columnOrder++;
                                searchResult.ApplicationType = reader.GetString(columnOrder);


                                columnOrder++;
                                var Management_Team = reader.GetDecimal(columnOrder);
                                if (Management_Team != (decimal)0.01)
                                {
                                    lstMT.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CCMFscorepresent("lstMT_Num")).ToString("N1");

                                    columnOrder++;
                                    lstBMTM.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CCMFscorepresent("lstBMTM_Num")).ToString("N1");

                                    columnOrder++;
                                    lstCCMFcipp.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CCMFscorepresent("lstCCMFcipp_Num")).ToString("N1");

                                    columnOrder++;
                                    lstSR.SelectedValue = (Convert.ToDouble(reader.GetDecimal(columnOrder)) / CCMFscorepresent("lstSR_Num")).ToString("N1");

                                    columnOrder++;
                                    //lstCCMFComment.SelectedValue = reader.GetString(columnOrder).ToString();
                                    string strSelectedComments = reader.GetString(columnOrder).ToString();
                                    if (!string.IsNullOrEmpty(strSelectedComments))
                                    {
                                        foreach (string strComment in strSelectedComments.Split(',').Where(x => !string.IsNullOrEmpty(x)))
                                        {
                                            System.Web.UI.WebControls.ListItem chkListItem = chkCCMFComments.Items.FindByValue(strComment);
                                            if (chkListItem != null)
                                                chkListItem.Selected = true;
                                        }
                                    }
                                    columnOrder++;
                                    LabelCCMFTotal.Text = reader.GetDecimal(columnOrder).ToString();
                                    HiddenFieldTatalScore.Value = LabelCCMFTotal.Text;

                                    columnOrder++;
                                    txtCCMFRemart.Text = reader.GetString(columnOrder);

                                    columnOrder++;
                                    RadioButtonListCCMFGoNotGo.SelectedValue = (reader.GetInt32(columnOrder)).ToString();
                                    hdnApplicationGoState.Value = RadioButtonListCCMFGoNotGo.SelectedValue;

                                }
                            }

                        }
                        lblCompanyName.Text = searchResult.CompanyName;
                        lblProjectName.Text = searchResult.ProjectName;
                        lblProgrammeType.Text = searchResult.ProgrammeType;
                        lblApplicationType.Text = searchResult.ApplicationType;

                        reader.Dispose();
                        command.Dispose();


                    }
                }
                finally
                {
                    ConnectClose();
                }
            }
        }

        protected void imbClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlWarning.Visible = false;

            Context.Response.Redirect("~/SitePages/Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID, false);
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

        private bool CheckMeetingIsCompleted()
        {
            bool result = false;
            string sql = "SELECT Meeting_Completed FROM TB_VETTING_MEETING WHERE Vetting_Meeting_ID = @vmID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@vmID", m_VMID);

                    if (command.ExecuteScalar() != null && DBNull.Value != command.ExecuteScalar())
                    {
                        result = (bool)command.ExecuteScalar();
                    }

                    conn.Close();
                }
            }
            return result;
        }

        protected void btnCPIPSubmit_Click(object sender, EventArgs e)
        {
            if (CheckUser())
            {
                if (CheckMeetingIsCompleted())
                {
                    pnlWarning.Visible = true;
                    return;
                }

                if (!string.IsNullOrEmpty(RadioButtonListGoNotGo.SelectedValue))
                {
                    string strCommentItems = "";
                    if (RadioButtonListGoNotGo.SelectedValue == "0")
                    {
                        foreach (System.Web.UI.WebControls.ListItem chkCommentItem in chkCPIPComments.Items)
                        {
                            if (chkCommentItem.Selected)
                                strCommentItems += chkCommentItem.Value + ",";
                        }
                    }
                    if (RadioButtonListGoNotGo.SelectedValue == "1" || (RadioButtonListGoNotGo.SelectedValue == "0" && !string.IsNullOrEmpty(strCommentItems)))
                    {
                        var parametersObject = new SearchResult();
                        parametersObject.ProgrammeName = lblProgrammeName.Text;
                        parametersObject.ApplicationNo = lblApplicationNo.Text;
                        parametersObject.ProgrammeID = HiddenFieldProgramID.Value;
                        parametersObject.MemberEmail = HiddenFieldMemberEmail.Value;
                        parametersObject.CPIP_Management_Team = (Convert.ToDouble(lstqcmt.SelectedValue) * CPIPscorepresent("lstqcmt_Num")).ToString("N2");
                        parametersObject.CPIP_Creativity = (Convert.ToDouble(lstcipp.SelectedValue) * CPIPscorepresent("lstcipp_Num")).ToString("N2");
                        parametersObject.CPIP_Business_Viability = (Convert.ToDouble(lstmbv.SelectedValue) * CPIPscorepresent("lstmbv_Num")).ToString("N2");
                        parametersObject.CPIP_Benefit_To_Industry = (Convert.ToDouble(lstbhkdti.SelectedValue) * CPIPscorepresent("lstbhkdti_Num")).ToString("N2");
                        parametersObject.CPIP_Proposal_Milestones = (Convert.ToDouble(lstpsmpb.SelectedValue) * CPIPscorepresent("lstpsmpb_Num")).ToString("N2");
                        // parametersObject.CPIP_Comment = lstCPIPComment.SelectedValue;

                        parametersObject.CPIP_Comment = strCommentItems.Length > 0 ? strCommentItems.Remove(strCommentItems.Length - 1) : strCommentItems;
                        //parametersObject.Total_Score = HiddenFieldTatalScore.Value;
                        //parametersObject.Total_Score = parametersObject.CPIP_Management_Team
                        //                            + parametersObject.CPIP_Creativity
                        //                            + parametersObject.CPIP_Business_Viability
                        //                            + parametersObject.CPIP_Benefit_To_Industry
                        //                            + parametersObject.CPIP_Proposal_Milestones;
                        parametersObject.Total_Score = ((Convert.ToDouble(lstqcmt.SelectedValue) * CPIPscorepresent("lstqcmt_Num"))
                                                       + (Convert.ToDouble(lstcipp.SelectedValue) * CPIPscorepresent("lstcipp_Num"))
                                                       + (Convert.ToDouble(lstmbv.SelectedValue) * CPIPscorepresent("lstmbv_Num"))
                                                       + (Convert.ToDouble(lstbhkdti.SelectedValue) * CPIPscorepresent("lstbhkdti_Num"))
                                                       + (Convert.ToDouble(lstpsmpb.SelectedValue) * CPIPscorepresent("lstpsmpb_Num"))).ToString("N2");                       
                        parametersObject.GoNotGo = RadioButtonListGoNotGo.SelectedValue;
                        parametersObject.Remarks = txtremarks.Text;

                        UpdateTB_PRESENTATION_INCUBATION_SCORE(parametersObject);
                        //UpdateTB_VETTING_DECISION(parametersObject);
                        PaggingNavigation();
                        // Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID);
                    }
                    else
                        lblError.Text = "Not recommended application must select comment(s)";
                }
                else
                    lblError.Text = "Please select Recommend / Not Recommend option";

            }

        }

        protected void btnCCMFSubmit_Click(object sender, EventArgs e)
        {
            if (CheckUser())
            {
                if (CheckMeetingIsCompleted())
                {
                    pnlWarning.Visible = true;
                    return;
                }


                if (!string.IsNullOrEmpty(RadioButtonListCCMFGoNotGo.SelectedValue))
                {
                    string strCommentItems = "";
                    if (RadioButtonListCCMFGoNotGo.SelectedValue == "0")
                    {
                        foreach (System.Web.UI.WebControls.ListItem chkCommentItem in chkCCMFComments.Items)
                        {
                            if (chkCommentItem.Selected)
                                strCommentItems += chkCommentItem.Value + ",";
                        }
                    }
                    if (RadioButtonListCCMFGoNotGo.SelectedValue == "1" || (RadioButtonListCCMFGoNotGo.SelectedValue == "0" && !string.IsNullOrEmpty(strCommentItems)))
                    {

                        var parametersObject = new SearchResult();
                        parametersObject.ProgrammeName = lblProgrammeName.Text;
                        parametersObject.ApplicationNo = lblApplicationNo.Text;
                        parametersObject.ProgrammeID = HiddenFieldProgramID.Value;
                        parametersObject.MemberEmail = HiddenFieldMemberEmail.Value;
                        parametersObject.CCMF_Management_Team = (Convert.ToDouble(lstMT.SelectedValue) * CCMFscorepresent("lstMT_Num")).ToString("N2");
                        parametersObject.CCMF_Business_Model = (Convert.ToDouble(lstBMTM.SelectedValue) * CCMFscorepresent("lstBMTM_Num")).ToString("N2");
                        parametersObject.CCMF_Creativity = (Convert.ToDouble(lstCCMFcipp.SelectedValue) * CCMFscorepresent("lstCCMFcipp_Num")).ToString("N2");
                        parametersObject.CCMF_Social_Responsibility = (Convert.ToDouble(lstSR.SelectedValue) * CCMFscorepresent("lstSR_Num")).ToString("N2");
                        //parametersObject.CCMF_Comment = lstCCMFComment.SelectedValue;
                        parametersObject.CCMF_Comment = strCommentItems.Length > 0 ? strCommentItems.Remove(strCommentItems.Length - 1) : strCommentItems;
                        //parametersObject.Total_Score = HiddenFieldTatalScore.Value;
                        parametersObject.Total_Score = ((Convert.ToDouble(lstMT.SelectedValue) * CCMFscorepresent("lstMT_Num"))
                                                    + (Convert.ToDouble(lstBMTM.SelectedValue) * CCMFscorepresent("lstBMTM_Num"))
                                                    + (Convert.ToDouble(lstCCMFcipp.SelectedValue) * CCMFscorepresent("lstCCMFcipp_Num"))
                                                    + (Convert.ToDouble(lstSR.SelectedValue) * CCMFscorepresent("lstSR_Num"))).ToString("N2");
                        parametersObject.GoNotGo = RadioButtonListCCMFGoNotGo.SelectedValue;
                        parametersObject.Remarks = txtCCMFRemart.Text;




                        UpdateTB_TB_PRESENTATION_CCMF_SCORE(parametersObject);
                        //UpdateTB_VETTING_DECISION(parametersObject);
                        PaggingNavigation();
                        // Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID);
                    }
                    else
                        lblError.Text = "Not recommended application must select comment(s)";
                }
                else
                    lblError.Text = "Please select Recommend / Not Recommend option";

            }

        }
        private void PaggingNavigation()
        {
            if (!string.IsNullOrEmpty(m_PaggingVAID))
            {
                char[] delimiters = new char[] { ',' };
                List<string> strPaggingsVAID = m_PaggingVAID.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
                int curIndex = strPaggingsVAID.FindIndex(x => x == m_VAID);
                if (curIndex + 1 != strPaggingsVAID.Count)
                {
                    Context.Response.Redirect("Presentation Scoring Form.aspx?VMID=" + m_VMID + "&VAID=" + strPaggingsVAID[curIndex + 1] + "&pagging=" + m_PaggingVAID);
                }
                else
                    Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID);
            }
            else
                Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("Presentation%20List%20of%20Applications.aspx?VMID=" + m_VMID);
        }




        public void UpdateTB_PRESENTATION_INCUBATION_SCORE(SearchResult Parameters)
        {
            ConnectOpen();
            try
            {
                var sqlString = "SELECT tScore.Incubation_Scoring_ID from TB_PRESENTATION_INCUBATION_SCORE tScore where tScore.Application_Number = @Application_Number and tScore.Programme_ID = @Programme_ID and tScore.Member_Email = @Member_Email ";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Application_Number", Parameters.ApplicationNo));
                command.Parameters.Add(new SqlParameter("@Programme_ID", Parameters.ProgrammeID));
                command.Parameters.Add(new SqlParameter("@Member_Email", SPContext.Current.Web.CurrentUser.Name.ToString()));

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //update
                    var sqlUpdate = "update TB_PRESENTATION_INCUBATION_SCORE set "
                                        + "Management_Team = @Management_Team, "
                                        + "Creativity = @Creativity, "
                                        + "Business_Viability = @Business_Viability, "
                                        + "Benefit_To_Industry = @Benefit_To_Industry, "
                                        + "Proposal_Milestones = @Proposal_Milestones, "
                                        + "Total_Score = @Total_Score, "
                                        + "Remarks = @Remarks, "
                                        + "Go = @Go, "
                                        + "Comments = @Comment,"
                                        + "Modified_By = @Modified_By, "
                                        + "Modified_Date = GETDATE() "
                                        + "where Incubation_Scoring_ID=@Incubation_Scoring_ID; ";

                    command = new SqlCommand(sqlUpdate, connection);

                    command.Parameters.Add(new SqlParameter("@Management_Team", Decimal.Parse(Parameters.CPIP_Management_Team)));
                    command.Parameters.Add(new SqlParameter("@Creativity", Decimal.Parse(Parameters.CPIP_Creativity)));
                    command.Parameters.Add(new SqlParameter("@Business_Viability", Decimal.Parse(Parameters.CPIP_Business_Viability)));
                    command.Parameters.Add(new SqlParameter("@Benefit_To_Industry", Decimal.Parse(Parameters.CPIP_Benefit_To_Industry)));
                    command.Parameters.Add(new SqlParameter("@Proposal_Milestones", Decimal.Parse(Parameters.CPIP_Proposal_Milestones)));

                    SqlParameter Total_Score = new SqlParameter("@Total_Score", SqlDbType.Decimal);
                    Total_Score.Precision = 3;
                    Total_Score.Scale = 2;
                    Total_Score.Value = Parameters.Total_Score;
                    command.Parameters.Add(Total_Score);

                    command.Parameters.Add(new SqlParameter("@Remarks", Parameters.Remarks));
                    command.Parameters.Add(new SqlParameter("@Go", Parameters.GoNotGo));
                    command.Parameters.Add(new SqlParameter("@Comment", Parameters.CPIP_Comment));
                    command.Parameters.Add(new SqlParameter("@Modified_By", SPContext.Current.Web.CurrentUser.Name.ToString()));
                    command.Parameters.Add(new SqlParameter("@Incubation_Scoring_ID", reader.GetInt32(0)));

                    command.ExecuteNonQuery();
                }
                else
                {
                    //insert
                    var sqlInsert = "insert into TB_PRESENTATION_INCUBATION_SCORE"
                                        + "(Application_Number,Programme_ID,Member_Email,Management_Team,Creativity,Business_Viability,Benefit_To_Industry,Proposal_Milestones,"
                                        + "Total_Score,Remarks,Go,Comments,Created_By,Created_Date,Modified_By,Modified_Date) values "
                                        + "(@Application_Number,@Programme_ID,@Member_Email,@Management_Team,@Creativity,@Business_Viability,@Benefit_To_Industry,@Proposal_Milestones,"
                                        + "@Total_Score,@Remarks,@Go,@Comment,@Created_By,GETDATE(),@Created_By,GETDATE() "
                                        + " ) ;";

                    command = new SqlCommand(sqlInsert, connection);
                    command.Parameters.Add(new SqlParameter("@Application_Number", Parameters.ApplicationNo));
                    command.Parameters.Add(new SqlParameter("@Programme_ID", Parameters.ProgrammeID));
                    command.Parameters.Add(new SqlParameter("@Member_Email", Parameters.MemberEmail));
                    command.Parameters.Add(new SqlParameter("@Management_Team", Decimal.Parse(Parameters.CPIP_Management_Team)));
                    command.Parameters.Add(new SqlParameter("@Creativity", Decimal.Parse(Parameters.CPIP_Creativity)));
                    command.Parameters.Add(new SqlParameter("@Business_Viability", Decimal.Parse(Parameters.CPIP_Business_Viability)));
                    command.Parameters.Add(new SqlParameter("@Benefit_To_Industry", Decimal.Parse(Parameters.CPIP_Benefit_To_Industry)));
                    command.Parameters.Add(new SqlParameter("@Proposal_Milestones", Decimal.Parse(Parameters.CPIP_Proposal_Milestones)));

                    SqlParameter Total_Score = new SqlParameter("@Total_Score", SqlDbType.Decimal);
                    Total_Score.Precision = 3;
                    Total_Score.Scale = 2;
                    Total_Score.Value = Parameters.Total_Score;
                    command.Parameters.Add(Total_Score);

                    command.Parameters.Add(new SqlParameter("@Remarks", Parameters.Remarks));
                    command.Parameters.Add(new SqlParameter("@Go", Parameters.GoNotGo));
                    command.Parameters.Add(new SqlParameter("@Comment", Parameters.CPIP_Comment));
                    command.Parameters.Add(new SqlParameter("@Created_By", SPContext.Current.Web.CurrentUser.Name.ToString()));


                    command.ExecuteNonQuery();
                }

                reader.Dispose();
                command.Dispose();



            }
            finally
            {
                ConnectClose();
            }
        }

        public void UpdateTB_TB_PRESENTATION_CCMF_SCORE(SearchResult Parameters)
        {
            ConnectOpen();
            try
            {
                var sqlString = "SELECT tScore.CCMF_Scoring_ID from TB_PRESENTATION_CCMF_SCORE tScore where tScore.Application_Number = @Application_Number and tScore.Programme_ID = @Programme_ID and tScore.Member_Email = @Member_Email ";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Application_Number", Parameters.ApplicationNo));
                command.Parameters.Add(new SqlParameter("@Programme_ID", Parameters.ProgrammeID));
                command.Parameters.Add(new SqlParameter("@Member_Email", SPContext.Current.Web.CurrentUser.Name.ToString()));

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //update
                    var sqlUpdate = "update TB_PRESENTATION_CCMF_SCORE set "
                                        + "Management_Team = @Management_Team, "
                                        + "Business_Model = @Business_Model, "
                                        + "Creativity = @Creativity, "
                                        + "Social_Responsibility = @Social_Responsibility, "
                                        + "Total_Score = @Total_Score, "
                                        + "Remarks = @Remarks, "
                                        + "Go = @Go, "
                                        + "Comments = @Comment, "
                                        + "Modified_By = @Modified_By, "
                                        + "Modified_Date = GETDATE() "
                                        + "where CCMF_Scoring_ID=@CCMF_Scoring_ID; ";

                    command = new SqlCommand(sqlUpdate, connection);

                    command.Parameters.Add(new SqlParameter("@Management_Team", Decimal.Parse(Parameters.CCMF_Management_Team)));
                    command.Parameters.Add(new SqlParameter("@Business_Model", Decimal.Parse(Parameters.CCMF_Business_Model)));
                    command.Parameters.Add(new SqlParameter("@Creativity", Decimal.Parse(Parameters.CCMF_Creativity)));
                    command.Parameters.Add(new SqlParameter("@Social_Responsibility", Decimal.Parse(Parameters.CCMF_Social_Responsibility)));

                    SqlParameter Total_Score = new SqlParameter("@Total_Score", SqlDbType.Decimal);
                    Total_Score.Precision = 3;
                    Total_Score.Scale = 2;
                    Total_Score.Value = Parameters.Total_Score;
                    command.Parameters.Add(Total_Score);

                    command.Parameters.Add(new SqlParameter("@Remarks", Parameters.Remarks));
                    command.Parameters.Add(new SqlParameter("@Go", Parameters.GoNotGo));
                    command.Parameters.Add(new SqlParameter("@Comment", Parameters.CCMF_Comment));
                    command.Parameters.Add(new SqlParameter("@Modified_By", SPContext.Current.Web.CurrentUser.Name.ToString()));
                    command.Parameters.Add(new SqlParameter("@CCMF_Scoring_ID", reader.GetInt32(0)));

                    command.ExecuteNonQuery();
                }
                else
                {
                    //insert
                    var sqlInsert = "insert into TB_PRESENTATION_CCMF_SCORE"
                                        + "(Application_Number,Programme_ID,Member_Email,Management_Team,Business_Model,Creativity,Social_Responsibility,Total_Score,Remarks,"
                                        + "Go,Comments,Created_By,Created_Date,Modified_By,Modified_Date) values "
                                        + "(@Application_Number,@Programme_ID,@Member_Email,@Management_Team,@Business_Model,@Creativity,@Social_Responsibility,@Total_Score,@Remarks,"
                                        + "@Go,@Comment,@Created_By,GETDATE(),@Created_By,GETDATE() "
                                        + " ) ;";


                    command = new SqlCommand(sqlInsert, connection);
                    command.Parameters.Add(new SqlParameter("@Application_Number", Parameters.ApplicationNo));
                    command.Parameters.Add(new SqlParameter("@Programme_ID", Parameters.ProgrammeID));
                    command.Parameters.Add(new SqlParameter("@Member_Email", Parameters.MemberEmail));
                    command.Parameters.Add(new SqlParameter("@Management_Team", Decimal.Parse(Parameters.CCMF_Management_Team)));
                    command.Parameters.Add(new SqlParameter("@Business_Model", Decimal.Parse(Parameters.CCMF_Business_Model)));
                    command.Parameters.Add(new SqlParameter("@Creativity", Decimal.Parse(Parameters.CCMF_Creativity)));
                    command.Parameters.Add(new SqlParameter("@Social_Responsibility", Decimal.Parse(Parameters.CCMF_Social_Responsibility)));

                    SqlParameter Total_Score = new SqlParameter("@Total_Score", SqlDbType.Decimal);
                    Total_Score.Precision = 3;
                    Total_Score.Scale = 2;
                    Total_Score.Value = Parameters.Total_Score;
                    lblDate.Text += Parameters.Total_Score;
                    command.Parameters.Add(Total_Score);


                    command.Parameters.Add(new SqlParameter("@Remarks", Parameters.Remarks));
                    command.Parameters.Add(new SqlParameter("@Go", Parameters.GoNotGo));
                    command.Parameters.Add(new SqlParameter("@Comment", Parameters.CCMF_Comment));
                    command.Parameters.Add(new SqlParameter("@Created_By", SPContext.Current.Web.CurrentUser.Name.ToString()));

                    command.ExecuteNonQuery();
                }

                reader.Dispose();
                command.Dispose();


            }
            finally
            {
                ConnectClose();
            }
        }

        public void UpdateTB_VETTING_DECISION(SearchResult Parameters)
        {
            ConnectOpen();
            try
            {
                var sqlString = "SELECT tvd.Vetting_Delclaration_ID from TB_VETTING_DECISION tvd where tvd.Application_Number = @Application_Number and tvd.Vetting_Meeting_ID = @Vetting_Meeting_ID ";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@Application_Number", Parameters.ApplicationNo));
                command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_VMID));

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //update
                    var sqlUpdate = "update TB_VETTING_DECISION set "
                                        + "Go = @Go "
                                        + "where Vetting_Delclaration_ID=@Vetting_Delclaration_ID; ";

                    command = new SqlCommand(sqlUpdate, connection);

                    command.Parameters.Add(new SqlParameter("@Go", Parameters.GoNotGo));
                    command.Parameters.Add(new SqlParameter("@Vetting_Delclaration_ID", reader.GetGuid(0)));

                    command.ExecuteNonQuery();
                }
                else
                {
                    //insert
                    var sqlInsert = "insert into TB_VETTING_DECISION(Vetting_Delclaration_ID,Vetting_Meeting_ID,Application_Number,Member_Email,Go) values( "
                                        + "@Vetting_Delclaration_ID,@Vetting_Meeting_ID,@Application_Number,@Member_Email,@Go "
                                        + " ) ;";

                    command = new SqlCommand(sqlInsert, connection);
                    command.Parameters.Add(new SqlParameter("@Vetting_Delclaration_ID", Guid.NewGuid()));
                    command.Parameters.Add(new SqlParameter("@Vetting_Meeting_ID", m_VMID));
                    command.Parameters.Add(new SqlParameter("@Application_Number", Parameters.ApplicationNo));
                    command.Parameters.Add(new SqlParameter("@Member_Email", Parameters.MemberEmail));
                    command.Parameters.Add(new SqlParameter("@Go", Parameters.GoNotGo));

                    command.ExecuteNonQuery();
                }

                reader.Dispose();
                command.Dispose();



            }
            finally
            {
                ConnectClose();
            }
        }

        protected static double CCMFscorepresent(string num)
        {
            double _num = 0.0;

            switch (num)
            {
                case "lstMT_Num":
                    _num = 0.3;
                    break;

                case "lstBMTM_Num":
                    _num = 0.3;
                    break;

                case "lstCCMFcipp_Num":
                    _num = 0.3;
                    break;

                case "lstSR_Num":
                    _num = 0.1;
                    break;

                default:
                    break;
            }

            return _num;
        }

        protected static double CPIPscorepresent(string num)
        {
            double _num = 0.0;

            switch (num)
            {
                case "lstqcmt_Num":
                    _num = 0.3;
                    break;

                case "lstcipp_Num":
                    _num = 0.2;
                    break;

                case "lstmbv_Num":
                    _num = 0.2;
                    break;

                case "lstbhkdti_Num":
                    _num = 0.2;
                    break;

                case "lstpsmpb_Num":
                    _num = 0.1;
                    break;

                default:
                    break;
            }

            return _num;
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

    }

    public class SearchResult
    {
        public string Date { get; set; }
        public string ProgrammeName { get; set; }
        public string IntakeNo { get; set; }
        public string ApplicationNo { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string ProgrammeType { get; set; }
        public string ApplicationType { get; set; }
        public string Time { get; set; }
        public string ProgrammeID { get; set; }
        public string MemberEmail { get; set; }

        public string CPIP_Management_Team { get; set; }
        public string CPIP_Creativity { get; set; }
        public string CPIP_Business_Viability { get; set; }
        public string CPIP_Benefit_To_Industry { get; set; }
        public string CPIP_Proposal_Milestones { get; set; }
        public string CPIP_Comment { get; set; }

        public string CCMF_Management_Team { get; set; }
        public string CCMF_Business_Model { get; set; }
        public string CCMF_Creativity { get; set; }
        public string CCMF_Social_Responsibility { get; set; }
        public string CCMF_Comment { get; set; }

        public string Total_Score { get; set; }
        public string Remarks { get; set; }
        public string Creativity { get; set; }

        public string GoNotGo { get; set; }


    }
}
