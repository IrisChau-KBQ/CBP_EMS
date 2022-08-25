using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Microsoft.SharePoint.Utilities;

namespace CBP_EMS_SP.ECheckWebPart.ECheckWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ECheckWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        private String m_progid = "";
        private String m_ApplicationID= "";
        private String m_systemuser = "";
        private String m_ApplicationNumber= "";
        private int m_CCMF = 0 ;
        private int m_INCUBATION = 0 ;
        private String m_Status= "";
        private String m_insert_Update = "";
        private String m_Role ="";

        private SqlConnection connection;
        public String m_path = @"D:\\tmp";
        public String m_programName;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_ApplicationIsInDebug;
        public String m_ApplicationDebugEmailSentTo;
        public String m_zipfiledownloadurl;
        public String m_downloadLink;

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }

        public ECheckWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //m_progid = Context.Request.QueryString["ProgNo"]; //Get Programme ID from URL
            //m_ApplicationID = Context.Request.QueryString["AppNo"]; //Get Application Number from URL
            //m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User
            //lblCoordinator.Text = m_systemuser;

            //m_progid = Context.Request.QueryString["Prog"]; //Get Programme ID from URL
            //m_ApplicationID = Context.Request.QueryString["App"]; //Get Application Number from URL
            m_systemuser = SPContext.Current.Web.CurrentUser.Name.ToString(); //Get Name of SharePoint User
            lblCoordinator.Text = m_systemuser;

            m_progid = HttpContext.Current.Request.QueryString["Prog"];
            m_ApplicationID = HttpContext.Current.Request.QueryString["App"];

            if (m_progid != null && m_ApplicationID != null)
            {
                // Convert application ID to application name
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT('Incubation_ID') FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = '" + m_ApplicationID + "'", conn))
                    {
                        conn.Open();
                        m_INCUBATION = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        conn.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT('CCMF_ID') FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                    {
                        conn.Open();
                        m_CCMF = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        conn.Close();
                    }

                    if (Convert.ToInt32(m_INCUBATION) != 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = '" + m_ApplicationID + "'", conn))
                        {
                            conn.Open();
                            //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                            conn.Close();
                        }
                        //txtcommentforapplicants.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;

                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_INCUBATION_APPLICATION] WHERE Incubation_ID = '" + m_ApplicationID + "'", conn))
                        {
                            conn.Open();
                            //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_Status = cmd.ExecuteScalar().ToString();
                            conn.Close();
                        }

                    }
                    else if (Convert.ToInt32(m_CCMF) != 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Application_Number FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                        {
                            conn.Open();
                            ////cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_ApplicationNumber = cmd.ExecuteScalar().ToString();
                            conn.Close();
                        }


                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Status FROM [TB_CCMF_APPLICATION] WHERE CCMF_ID = '" + m_ApplicationID + "'", conn))
                        {
                            conn.Open();
                            //cmd.Parameters.Add(new SqlParameter("@ID", m_ApplicationID));
                            m_Status = cmd.ExecuteScalar().ToString();
                            conn.Close();
                        }
                        //txtcommentforinternaluse.Text = m_CCMF + "/" + m_INCUBATION + "/" + m_ApplicationID + "/" + m_ApplicationNumber;
                    }


                }


                getReview();

                if (!Page.IsPostBack)
                {
                    getdbdata();
                }

                AccessControl();

                getSYSTEMPARAMETER();
                if (m_ApplicationIsInDebug == "1")
                {
                    btnDownload.OnClientClick = "return confirm('Start processing now, an email will be sent to (" + m_ApplicationDebugEmailSentTo + "). Please wait.')";
                }
                else
                {
                    btnDownload.OnClientClick = "return confirm('Start processing now, an email will be sent to (" + SPContext.Current.Web.CurrentUser.Email + "). Please wait.')";
                }




            }
            else 
            {
                lblCoordinator.Text = "Application id & ProgID is null";
            }

            

        }

        protected void AccessControl()
        {
            // Check Role can display this web part
            //Applicant  //Collaborator  //CCMF Coordinator //CCMF BDM  //CPIP Coordinator  //CPIP BDM  //Senior Manager  //CPMO

            if (m_Role == "Applicant")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "Collaborator")
            {
                MainPanel.Visible = false;
            }
            else if (m_Role == "CCMF Coordinator")
            {
                MainPanel.Visible = true;
            }
            else if (m_Role == "CPIP Coordinator")
            {
                MainPanel.Visible = true;
            }
            else if (m_Role == "CCMF BDM")
            {
                MainPanel.Visible = true;
            }
            else if (m_Role == "CPIP BDM")
            {
                MainPanel.Visible = true;
            }
            else if (m_Role == "Senior Manager")
            {
                MainPanel.Visible = true;
            }
            else if (m_Role == "CPMO")
            {
                MainPanel.Visible = true;
            }


            // Check Status can display submit button
            if (m_Status == "Submitted")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Waiting for response from applicant")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Resubmitted information")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Coordinator Reviewed")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Eligibility checked")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "To be disqualified")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "Disqualified")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
            }
            else if (m_Status == "BDM Reviewed")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Rejected")
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }
            else if (m_Status == "Sr. Mgr. Reviewed")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "CPMO Reviewed")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else if (m_Status == "BDM Final Review")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "Cannot submit Score on " + m_Status + " status.";
            }
            else if (m_Status == "Complete Screening")
            {
                BtnSubmit.Enabled = false;
                lblmessage.Text = "";
            }
            else
            {
                BtnSubmit.Enabled = true;
                lblmessage.Text = "";
            }

        }

        protected void getdbdata()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                //string tempsql = "SELECT CCMF_Scoring_ID , Application_Number , Programme_ID , Reviewer , Role , Management_Team , Business_Model , Creativity , Social_Responsibility , Total_Score , Comments , Remarks , Created_By , Created_Date , Modified_By , Modified_Date FROM [TB_SCREENING_CCMF_SCORE] WHERE Application_Number = '" + m_ApplicationNumber + "' AND Role='" + m_Role + "'";
                string tempsql = "SELECT [Screening_Comments_ID],[Application_Number],[Programme_ID],[Validation_Result],[Comment_For_Applicants],[Comment_For_Internal_Use],[Created_By],[Created_Date] FROM [CyberportWMS].[dbo].[TB_SCREENING_HISTORY] "
                + "where Programme_ID='" + m_progid + "' and Application_Number='" + m_ApplicationNumber + "'";

                //txtcommentforapplicants.Text = tempsql;

                using (SqlCommand cmd = new SqlCommand(tempsql, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        txtcommentforapplicants.Text = reader.GetValue(reader.GetOrdinal("Comment_For_Applicants")).ToString();
                        txtcommentforinternaluse.Text = reader.GetValue(reader.GetOrdinal("Comment_For_Internal_Use")).ToString();

                        if (reader.GetValue(reader.GetOrdinal("Validation_Result")) == "Coordinator Reviewed" || reader.GetValue(reader.GetOrdinal("Validation_Result")) == "Require additional information" || reader.GetValue(reader.GetOrdinal("Validation_Result")) == "To be disqualified")
                        {
                            rbtnresult.Items.FindByValue(reader.GetValue(reader.GetOrdinal("Validation_Result")).ToString()).Selected = true;
                        }
                    }
                    conn.Close();
                }
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
                        //lblrole.Text = ogroup.Name;
                        m_Role = ogroup.Name;
                    }
                }
            }
            //m_Role = "CCMF BDM";

            if (HttpContext.Current.Request.QueryString["testrole"] != null)
            {
                m_Role = HttpContext.Current.Request.QueryString["testrole"];
                //lblrole.Text = m_Role;
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            //string connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportEMS; persist security info=True; User Id=sa; Password=Password1234*;";
            //string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

            //txtcommentforapplicants.Text = m_ApplicationNumber;

            SqlConnection conn = new SqlConnection(connStr);

            if (Page.IsValid)
            {
                try
                {
                    //string Application_Number = m_ApplicationID;
                    string Application_Number = m_ApplicationNumber;
                    string Programme_ID = m_progid;
                    string Validation_Results = "";
                    if (rbtnresult.SelectedValue == "Coordinator Reviewed")
                    {
                        Validation_Results = "Coordinator Reviewed";
                    }
                    else if (rbtnresult.SelectedValue == "Require additional information")
                    {
                        Validation_Results = "Require additional information"; //Waiting for response from applicant
                    }
                    else if (rbtnresult.SelectedValue == "To be disqualified")
                    {
                        Validation_Results = "To be disqualified";
                    }
                    string Comment_For_Applicants = txtcommentforapplicants.Text.ToString();
                    string Comment_For_Internal_Use = txtcommentforinternaluse.Text.ToString();
                    string Created_By = m_systemuser;
                    DateTime Created_Date = DateTime.Now;
                    string sql = "";

                    string tempsql = "SELECT Screening_Comments_ID ,Application_Number ,Programme_ID ,Validation_Result ,Comment_For_Applicants ,Comment_For_Internal_Use ,Created_By ,Created_Date FROM [TB_SCREENING_HISTORY] WHERE Application_Number = '" + m_ApplicationNumber + "' AND Programme_ID='" + Programme_ID + "'";
                    using (SqlCommand cmdscore = new SqlCommand(tempsql, conn))
                    {
                        conn.Open();
                        var reader = cmdscore.ExecuteReader();
                        while (reader.Read())
                        {
                            m_insert_Update = reader.GetValue(1).ToString();
                        }
                        conn.Close();
                    }


                    if (m_insert_Update != "")
                    {
                        sql = "UPDATE TB_SCREENING_HISTORY SET "
                        + "Validation_Result='" + Validation_Results + "', "
                        + "Comment_For_Applicants='" + Comment_For_Applicants + "', "
                        + "Comment_For_Internal_Use='" + Comment_For_Internal_Use + "' "
                        + "WHERE Application_Number = '" + Application_Number + "' "
                        + "AND Programme_ID='" + Programme_ID + "'";
                    }
                    else
                    {
                        sql = "insert into TB_SCREENING_HISTORY(Application_Number,Programme_ID,Validation_Result,Comment_For_Applicants,Comment_For_Internal_Use,Created_By,Created_Date) VALUES ("
                        + "'" + Application_Number + "' , "
                        + "'" + Programme_ID + "' , "
                        + "'" + Validation_Results + "' , "
                        + "'" + Comment_For_Applicants + "' , "
                        + "'" + Comment_For_Internal_Use + "' , "
                        + "'" + Created_By + "' , "
                        + "GETDATE()"
                        + ")";
                    }




                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    updateStatus();

                    string newdirection = "";
                    newdirection = "Application%20List.aspx?";
                    newdirection += "ProgName=" + HttpContext.Current.Request.QueryString["ProgName"];
                    newdirection += "&IntakeNo=" + HttpContext.Current.Request.QueryString["IntakeNo"];
                    newdirection += "&Cluster=" + HttpContext.Current.Request.QueryString["Cluster"];
                    newdirection += "&Status=" + HttpContext.Current.Request.QueryString["Status"];
                    newdirection += "&SortColumn1=" + HttpContext.Current.Request.QueryString["SortColumn1"];
                    newdirection += "&SortOrder1=" + HttpContext.Current.Request.QueryString["SortOrder1"];
                    newdirection += "&SortColumn2=" + HttpContext.Current.Request.QueryString["SortColumn2"];
                    newdirection += "&SortOrder2=" + HttpContext.Current.Request.QueryString["SortOrder2"];


                    //newdirection = "Application%20List.aspx";
                    Context.Response.Redirect(newdirection);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
        }

        protected void updateStatus()
        {
            string m_ApplicationID_temp = m_ApplicationID;
            m_ApplicationID = m_ApplicationNumber;
            //txtcommentforapplicants.Text = "updateStatus" + m_ApplicationID + "/";

            SPWeb oWebsiteRoot = SPContext.Current.Site.RootWeb;
            SPList oList = oWebsiteRoot.Lists["Application List"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='Title'  /><Value Type='Text'>" + m_ApplicationID + "</Value></Eq></Where>";
            SPListItemCollection collListItems = oList.GetItems(oQuery);

            foreach (SPListItem oListItem in collListItems)
            {

                string Validation_Results_SP = "";
                if (rbtnresult.SelectedValue == "Coordinator Reviewed")
                {
                    Validation_Results_SP = "Coordinator Reviewed";
                }
                else if (rbtnresult.SelectedValue == "Waiting for response from applicant")
                {
                    Validation_Results_SP = "Waiting for response from applicant"; //Waiting for response from applicant
                }
                else if (rbtnresult.SelectedValue == "To be disqualified")
                {
                    Validation_Results_SP = "To be disqualified";
                }


                //                txtcommentforapplicants.Text += "/" + Validation_Results_SP + "/" + Validation_Results_SP;

                oListItem["Status"] = Validation_Results_SP;
                oListItem["Coordinator_Comment_for_applicants"] = txtcommentforapplicants.Text.ToString();
                oListItem["Coordinator_Comment_for_internal_use"] = txtcommentforinternaluse.Text.ToString();

                oListItem.Web.AllowUnsafeUpdates = true;
                oListItem.Update();
                oListItem.Web.AllowUnsafeUpdates = false;

                SqlConnection conn = new SqlConnection(connStr);
                string sql = "";

                sql = "UPDATE TB_CCMF_APPLICATION SET Status = '" + Validation_Results_SP + "' WHERE CCMF_ID = '" + m_ApplicationID_temp + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                sql = "UPDATE TB_INCUBATION_APPLICATION SET Status = '" + Validation_Results_SP + "' WHERE Incubation_ID = '" + m_ApplicationID_temp + "'";
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.ExecuteNonQuery();
                conn.Close();



                //;

            }
        }

        protected void rbtnresult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnresult.SelectedValue.ToString() == "Waiting for response from applicant")
            {
                RequiredFieldValidator.Enabled = true;
            }
            else
            {
                RequiredFieldValidator.Enabled = false;
            }
        }

        protected void getProgrmNameIntakeNumByProgID()
        {
            ConnectOpen();

            var sqlString = "select Programme_Name,Intake_Number from TB_PROGRAMME_INTAKE where Programme_ID='" + m_progid + "';";

            var command = new SqlCommand(sqlString, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                m_programName = reader.GetString(0);
                m_intake = reader.GetInt32(1).ToString();
            }

            reader.Dispose();
            command.Dispose();


            ConnectClose();
        }
        protected void getSYSTEMPARAMETER()
        {
            ConnectOpen();

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
            }

            reader.Dispose();
            command.Dispose();


            ConnectClose();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //m_programName = "Cyberport Incubation Programme";
            //m_intake = "201705";
            getProgrmNameIntakeNumByProgID();
            String Source = m_AttachmentPrimaryFolderName + @"\" + m_AttachmentSecondaryFolderName + @"\" + m_programName + " " + m_intake + @"\" + m_ApplicationNumber;
            var FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            String Destination = m_path + @"\" + FileName;

            m_downloadLink = m_zipfiledownloadurl + FileName;

            processFolder(Source, Destination, FileName);
        }
        public void processFolder(string folderURL, string zipFile, String FileName)
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

            //string m_mail = "Blue.Qiu@mouxidea.com.hk";

            if (Page.IsValid)
            {
                string m_subject = "";
                string m_body = "";
                string m_Programme_Name = m_programName;
                string m_Intake_Number = m_intake;
                string m_Password;
                string m_downloadlink = m_downloadLink;
                string m_Starttime;
                string m_Endtime;
                //string m_zipstatus = "done";



                //starting email
                m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is processing.";
                //m_body = "Hi, Zip File is processing, please wait next email to confirm Zip File is ready.";
                m_body = getEmailTemplate("ZipDownloadStartEmail");
                sharepointsendemail(m_mail, m_subject, m_body);


                /*************************/
                //zip programm in here:

                m_Starttime = DateTime.Now.ToString();
                SPSite site = SPContext.Current.Site;
                SPWeb web = site.OpenWeb();
                SPFolder folder = web.GetFolder(folderURL);

                ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));
                //MemoryStream ms = new MemoryStream();
                //ZipOutputStream zipStream = new ZipOutputStream(ms);

                zipStream.SetLevel(9); //0-9, 9 being the highest level of compression

                zipStream.Password = genRandom(8);	// optional. Null is the same as not setting. Required if using AES.
                m_Password = zipStream.Password;
                //lbldatetime.Text = lbldatetime.Text + "   "+zipStream.Password;

                CompressFolder(folder, zipStream);

                zipStream.Finish();

                //zipStream.IsStreamOwner = false;	

                zipStream.Close();

                ////var libName = System.Configuration.ConfigurationManager.AppSettings["AssetLibraryName"];
                ////Label3.Text = "libName: "+libName;
                //var path = @"Shared Documents\temp";
                //SPFolder myLibrary = web.GetFolder(path); 
                //// Prepare to upload  
                //Boolean replaceExistingFiles = true;
                //// Upload document  
                //var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                //SPFile spfile = myLibrary.Files.Add(filename, ms, replaceExistingFiles);
                //// Commit   
                //myLibrary.Update();

                m_Endtime = DateTime.Now.ToString();
                /*************************/


                //insert into TB_Download_ZIP
                InsertTBDownloadZIP(m_username, m_mail, "ZIP", zipFile, FileName, m_Password, "1");

                //Completed email  

                m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
                //m_body = "Hi, Zip File is ready, please download : " + m_downloadlink + " . <br/>Password is : " + m_Password + " <br/>";
                //m_body += "Zip Start time : " + m_Starttime + " to End Time :" + m_Endtime + "";
                m_body = getEmailTemplate("ZipDownloadEndEmail");
                m_body = m_body.Replace("@@m_downloadlink", m_downloadlink).Replace("@@m_Password", m_Password).Replace("@@m_Starttime", m_Starttime).Replace("@@m_Endtime", m_Endtime);
                sharepointsendemail(m_mail, m_subject, m_body);
                //sharepointsendemail("andysgi@gmail.com", "hi", "ko");
                lbldownloadmessage.Text = "Download Complete.";

            }
        }

        //compress file and folder
        private void CompressFolder(SPFolder folder, ZipOutputStream zipStream)
        {
            foreach (SPFile file in folder.Files)
            {
                var DeletepathName = m_AttachmentPrimaryFolderName + @"\" + m_AttachmentSecondaryFolderName + @"\";
                String entryName = file.Url.Substring(DeletepathName.Length);
                ZipEntry entry = new ZipEntry(entryName);
                entry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(entry);

                byte[] binary = file.OpenBinary();
                zipStream.Write(binary, 0, binary.Length);
            }

            foreach (SPFolder subfoldar in folder.SubFolders)
            {
                CompressFolder(subfoldar, zipStream);
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
        protected String getEmailTemplate(string emailTemplate)
        {
            ConnectOpen();
            String emailTemplateContent = "";
            var sqlString = "select Email_Template,Email_Template_Content from TB_EMAIL_TEMPLATE where Email_Template='" + emailTemplate + "';";

            var command = new SqlCommand(sqlString, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                emailTemplateContent = reader.GetString(1);
            }

            reader.Dispose();
            command.Dispose();


            ConnectClose();

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

        protected void InsertTBDownloadZIP(String User_Name, String Email, String type, String Path, String File_Name, String Password, String Status)
        {
            ConnectOpen();
            var sqlUpdate = "insert into TB_Download_ZIP(User_Name,Email,type,Path,File_Name,Password,Status,Created_By,Created_Date,Modified_By,Modified_Date) values("
                                + "'" + User_Name + "', "
                                + "'" + Email + "', "
                                + "'" + type + "', "
                                + "'" + Path + "', "
                                + "'" + File_Name + "', "
                                + "'" + Password + "', "
                                + "'" + Status + "', "
                                + "'" + SPContext.Current.Web.CurrentUser.Name.ToString() + "', "
                                + "GETDATE(), "
                                + "'" + SPContext.Current.Web.CurrentUser.Name.ToString() + "', "
                                + "GETDATE() "
                                + " ) ;";

            var command = new SqlCommand(sqlUpdate, connection);
            command.ExecuteNonQuery();

            command.Dispose();
            ConnectClose();
        }


    }
}
