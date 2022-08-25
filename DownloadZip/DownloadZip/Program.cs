using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using System.Data;
using log4net.Config;

namespace DownloadZip
{
    class Program
    {
        static string _spSite = ConfigurationManager.AppSettings["SpSite"];
        static string _connStr = ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
        static string _attachmentPrimaryFolderName = getSystemParam("AttachmentPrimaryFolder");
        static string _attachmentSecondaryFolderName = getSystemParam("AttachmentSecondaryFolder");
        static double _totalFileSize = 0;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            try
            {
                checkDownloadFileExpired();
                download();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                LogHelper.Exception("Error", ex);
            }
        }

        private static void download()
        {
            DataTable request = getOutstandingDownloadRequest();
            try
            {
                foreach (DataRow row in request.Rows)
                {
                    DateTime startTime = DateTime.Now;

                    string requestType = row["Request_Type"].ToString();

                    string id = row["ID"].ToString();
                    string programName = row["Programme_Name"].ToString();
                    string programID = row["Programme_ID"].ToString();
                    string intake = row["Intake_Number"].ToString();
                    string createdBy = row["Created_By"].ToString();
                    string vmID = row["Vetting_Meeting_ID"] == null ? "" : row["Vetting_Meeting_ID"].ToString();
                    string shortListFilter = row["ShortListFilter"] == null ? "" : row["ShortListFilter"].ToString();
                    string streamFilter = row["StreamFilter"] == null ? "" : row["StreamFilter"].ToString();

                    string source = string.Empty;
                    string userEmail = string.Empty;
                    string path = getSystemParam("zipfiledownloadpath");
                    string zipFileDownloadURL = getSystemParam("zipfiledownloadurl");

                    if (getSystemParam("ApplicationIsInDebug") == "1")
                    {
                        userEmail = getSystemParam("ApplicationDebugEmailSentTo");
                    }
                    else
                    {
                        userEmail = row["User_Email"].ToString();
                    }

                    if (!string.IsNullOrEmpty(_attachmentPrimaryFolderName))
                    {
                        source += _attachmentPrimaryFolderName + "/";
                    }
                    if (!string.IsNullOrEmpty(_attachmentSecondaryFolderName))
                    {
                        source += _attachmentSecondaryFolderName + "/";
                    }

                    source += programName + " " + intake;

                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                    string destination = path + @"\" + fileName;
                    string zipPassword = getRandom(8);
                    string subject = string.Format("Zip File : {0} / {1} is completed.", programName, intake);

                    if (requestType == "Download Attachments")
                    {
                        LogHelper.Debug("Before Download Attachments");
                        processFolder(source, destination, zipPassword);
                    }
                    else if (requestType == "Download Attachments - Invitation Response Summary")
                    {
                        LogHelper.Debug("Before Download Attachments - Invitation Response Sumary");
                        processFolder(source, destination, zipPassword, programID, vmID);
                    }
                    else if (requestType == "Download Attachments - Application for Programme Intake")
                    {
                        LogHelper.Debug("Before Download Attachments - Application for Programme Intake");
                        fileName = "PI_" + getProgrammeType(programID) + "_" + intake + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                        destination = path + @"\" + fileName;
                        subject = string.Format("Zip File for shortlisted applications : {0} / {1} is completed.", programName, intake);

                        processFolder(source, destination, zipPassword, programID, vmID, streamFilter, shortListFilter);
                    }
                    else if (requestType == "Download Shortlisted Application Files")
                    {
                        fileName = "SA_" + getProgrammeType(programID) + "_" + intake + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
                        destination = path + @"\" + fileName;
                        subject = string.Format("Zip File for shortlisted applications : {0} / {1} is completed.", programName, intake);

                        processFolder(source, destination, zipPassword, programID, vmID, createdBy);
                    }

                    DateTime endTime = DateTime.Now;

                    string body = getEmailTemplate("ZipDownloadEndEmailApp")
                                .Replace("@@m_downloadlink", zipFileDownloadURL).Replace("@@m_Programme_Name", programName)
                                .Replace("@@m_Intake_Number", intake).Replace("@@m_FileName", fileName).Replace("@@m_Password", zipPassword)
                                .Replace("@@m_Starttime", startTime.ToString()).Replace("@@m_Endtime", endTime.ToString());

                    updateRequestStatus(id);
                    insertTBDownloadZIP(createdBy, userEmail, "ZIP", destination, fileName, zipPassword, "1");
                    sharepointsendemail(userEmail, subject, body);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Download Error: " + ex);
                LogHelper.Exception("Download Error", ex);
            }
        }

        private static void processFolder(string folderURL, string zipFile, string zipPassword)
        {
            try
            {
                LogHelper.Debug("Before Opening Site");
                using (SPSite site = new SPSite(_spSite))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        LogHelper.Debug("Processing Folder");
                        SPFolder folder = web.GetFolder(folderURL);

                        ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));
                        zipStream.SetLevel(9);
                        zipStream.Password = zipPassword;

                        _totalFileSize = 0;
                        compressFolder(folder, zipStream);
                        Console.WriteLine("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                        LogHelper.Debug("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                        try
                        {
                            zipStream.Finish();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Process Folder Error: " + ex);
                            LogHelper.Exception("Process Folder Error", ex);
                        }

                        zipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("processFolder Error: " + ex);
                LogHelper.Exception("processFolder Error", ex);
            }

        }

        private static void processFolder(string folderURL, string zipFile, string zipPassword, string programmeID, string vmID)
        {
            try
            {
                LogHelper.Debug("Before Opening Site");

                using (SPSite site = new SPSite(_spSite))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPFolder folder = web.GetFolder(folderURL);

                        ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));
                        zipStream.SetLevel(9);
                        zipStream.Password = zipPassword;

                        _totalFileSize = 0;
                        compressFolder(folder, zipStream, programmeID, vmID);

                        Console.WriteLine("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                        LogHelper.Debug("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");

                        try
                        {
                            zipStream.Finish();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Process Folder Error: " + ex);
                            LogHelper.Exception("Process Folder Error", ex);
                        }

                        zipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("processFolder Error: " + ex);
                LogHelper.Exception("processFolder Error", ex);
            }

        }

        private static void processFolder(string folderURL, string zipFile, string zipPassword, string programmeID, string vmID, string streamFilter, string shortListFilter)
        {
            try
            {
                List<TimeSlotList> result = getApplication(programmeID, vmID, streamFilter, shortListFilter);
                List<string> applicationIDs = new List<String>();

                foreach (TimeSlotList list in result)
                {
                    applicationIDs.Add(list.Application_ID);
                }

                var filePaths = GetPresentationSlideAttachment(applicationIDs);

                if (filePaths.Count > 0)
                {
                    using (SPSite site = new SPSite(_spSite))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));
                            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression
                            zipStream.Password = zipPassword;

                            _totalFileSize = 0;
                            compressFolder(web, zipStream, filePaths);
                            Console.WriteLine("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                            LogHelper.Debug("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                            try
                            {
                                zipStream.Finish();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Process Folder Error: " + ex);
                                LogHelper.Exception("Process Folder Error", ex);
                            }
                            zipStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("processFolder Error: " + ex);
                LogHelper.Exception("processFolder Error", ex);
            }
        }

        private static void processFolder(string folderURL, string zipFile, string zipPassword, string programmeID, string vmID, string email)
        {
            try
            {
                List<string> result = getApplication(programmeID, vmID, email);
                var filePaths = GetPresentationSlideAttachment(result);

                if (filePaths.Count > 0)
                {
                    using (SPSite site = new SPSite(_spSite))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFile));
                            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression
                            zipStream.Password = zipPassword;

                            _totalFileSize = 0;
                            compressFolder(web, zipStream, filePaths);
                            Console.WriteLine("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                            LogHelper.Debug("Compress folder finished, Total file size: " + _totalFileSize.ToString("N2") + " KB");
                            try
                            {
                                zipStream.Finish();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Process Folder Error: " + ex);
                                LogHelper.Exception("Process Folder Error", ex);
                            }
                            zipStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("processFolder Error: " + ex);
                LogHelper.Exception("processFolder Error", ex);
            }

        }

        private static void compressFolder(SPFolder folder, ZipOutputStream zipStream)
        {
            try
            {
                foreach (SPFile file in folder.Files)
                {
                    string entryName = "";

                    try
                    {
                        string DeletepathName = "";
                        if (_attachmentPrimaryFolderName != "")
                        {
                            DeletepathName += _attachmentPrimaryFolderName + @"\";
                        }
                        if (_attachmentSecondaryFolderName != "")
                        {
                            DeletepathName += _attachmentSecondaryFolderName + @"\";
                        }
                        entryName = file.Url.Substring(DeletepathName.Length);

                        ZipEntry entry = new ZipEntry(entryName);
                        entry.DateTime = DateTime.Now;
                        zipStream.PutNextEntry(entry);

                        var size = file.Length / 1024;
                        _totalFileSize += size;

                        Console.WriteLine("File name: " + entryName + " (" + size.ToString("N2") + " KB)");
                        Console.WriteLine("Total file size: " + _totalFileSize.ToString("N2") + " KB");
                        LogHelper.Debug("File name: " + entryName + " (" + size.ToString("N2") + " KB)");
                        LogHelper.Debug("Total file size: " + _totalFileSize.ToString("N2") + " KB");

                        byte[] binary = file.OpenBinary();

                        zipStream.Write(binary, 0, binary.Length);

                        Array.Clear(binary, 0, binary.Length);
                        binary = null;
                    }
                    catch (Exception ex)
                    {
                        var str = entryName;
                        Console.WriteLine(str + " " + ex);
                        LogHelper.Exception(str, ex);
                    }
                }

                foreach (SPFolder subfoldar in folder.SubFolders)
                {
                    compressFolder(subfoldar, zipStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("compressFolder Error: " + ex);
                LogHelper.Exception("compressFolder Error", ex);
            }

        }

        private static void compressFolder(SPFolder folder, ZipOutputStream zipStream, string programmeID, string vmID)
        {
            try
            {
                foreach (SPFile file in folder.Files)
                {
                    string entryName = "";

                    try
                    {
                        string deletepathName = "";
                        if (_attachmentPrimaryFolderName != "")
                        {
                            deletepathName += _attachmentPrimaryFolderName + @"\";
                        }
                        if (_attachmentSecondaryFolderName != "")
                        {
                            deletepathName += _attachmentSecondaryFolderName + @"\";
                        }
                        entryName = file.Url.Substring(deletepathName.Length);


                        if (checkAttFileZip(file.Url, programmeID, vmID))
                        {
                            //lbltest.Text += "True: " + entryName + "<br/>";

                            var size = file.Length / 1024;
                            _totalFileSize += size;

                            Console.WriteLine("File name: " + entryName + " (" + size.ToString("N2") + " KB)");
                            Console.WriteLine("Total file size: " + _totalFileSize.ToString("N2") + " KB");
                            LogHelper.Debug("File name: " + entryName + " (" + size.ToString("N2") + " KB)");
                            LogHelper.Debug("Total file size: " + _totalFileSize.ToString("N2") + " KB");

                            ZipEntry entry = new ZipEntry(entryName);
                            entry.DateTime = DateTime.Now;
                            zipStream.PutNextEntry(entry);
                            byte[] binary = file.OpenBinary();
                            zipStream.Write(binary, 0, binary.Length);

                        }
                        else {

                            Console.WriteLine("File Not Found at Sharepoint Folder: " + entryName);
                            LogHelper.Debug("File Not Found at Sharepoint Folder: " + entryName);
                        }
                    }
                    catch (Exception ex)
                    {
                        var str = entryName;
                        Console.WriteLine(str + " " + ex);
                        LogHelper.Exception(str, ex);
                    }
                }

                foreach (SPFolder subfoldar in folder.SubFolders)
                {
                    compressFolder(subfoldar, zipStream, programmeID, vmID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("compressFolder Error: " + ex);
                LogHelper.Exception("compressFolder Error", ex);
            }
        }

        private static void compressFolder(SPWeb web, ZipOutputStream zipStream, List<String> filePaths)
        {
            try
            {
                LogHelper.Debug("Folder File Counts: " + filePaths.Count);
                foreach (var file in filePaths)
                {
                    string entryName = "";
                    try
                    {
                        string DeletepathName = "";
                        if (_attachmentPrimaryFolderName != "")
                        {
                            DeletepathName += _attachmentPrimaryFolderName + @"\";
                        }
                        if (_attachmentSecondaryFolderName != "")
                        {
                            DeletepathName += _attachmentSecondaryFolderName + @"\";
                        }

                        entryName = file.Substring(DeletepathName.Length);
                        ZipEntry entry = new ZipEntry(entryName);
                        entry.DateTime = DateTime.Now;
                        zipStream.PutNextEntry(entry);

                        var size = file.Length / 1024;
                        _totalFileSize += size;

                        var filObject = web.GetFile(file);
                        if (filObject.Exists)
                        {
                            Console.WriteLine("File name: " + entryName + " (" + size.ToString("N2") + " KB)");
                            Console.WriteLine("Total file size: " + _totalFileSize.ToString("N2") + " KB");
                            LogHelper.Debug("File name: " + entryName + " (" + size.ToString("N2") + " KB)");
                            LogHelper.Debug("Total file size: " + _totalFileSize.ToString("N2") + " KB");

                            byte[] binary = filObject.OpenBinary();
                            zipStream.Write(binary, 0, binary.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        var str = entryName;
                        Console.WriteLine(str + " " + ex);
                        LogHelper.Exception(str, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("compressFolder Error: " + ex);
                LogHelper.Exception("compressFolder Error", ex);
            }
        }

        private static bool checkAttFileZip(string filePath, string programmeID, string vmID)
        {
            string sql = string.Empty;
            bool match = false;
            DataTable dataTable = new DataTable();
            if (getProgrammeType(programmeID) == "CCMF")
            {
                sql = @"SELECT ccmf.Programme_ID, ccmf.Application_Number, ccmf.CCMF_ID as Application_ID, att.Attachment_Path, Attachment_Type  FROM TB_VETTING_APPLICATION vetting
                        INNER JOIN TB_CCMF_APPLICATION ccmf ON vetting.Application_Number = ccmf.Application_Number
                        LEFT JOIN TB_APPLICATION_ATTACHMENT att ON ccmf.CCMF_ID = att.Application_ID
                        WHERE Vetting_Meeting_ID = @m_VMID AND ccmf.Status IN ('Complete Screening', 'Presentation Withdraw', 'Awarded')
                        AND Attachment_Type <> 'Video_Clip'
                        ORDER BY Presentation_From";
            }
            else
            {
                sql = @"SELECT cpip.Programme_ID, cpip.Application_Number, cpip.Incubation_ID as Application_ID, att.Attachment_Path, Attachment_Type FROM TB_VETTING_APPLICATION vetting
                        INNER JOIN TB_INCUBATION_APPLICATION cpip ON vetting.Application_Number = cpip.Application_Number
                        LEFT JOIN TB_APPLICATION_ATTACHMENT att ON cpip.Incubation_ID = att.Application_ID
                        WHERE Vetting_Meeting_ID =  @m_VMID AND cpip.Status IN ('Complete Screening', 'Presentation Withdraw', 'Awarded')
                        AND Attachment_Type <> 'Video_Clip'
                        ORDER BY Presentation_From";
            }

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@m_VMID", vmID);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    conn.Close();
                }
            }


            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Attachment_Path"].ToString().IndexOf(filePath) >= 0)
                {
                    match = true;
                    break;
                }
            }

            return match;
        }

        private static string getRandom(int digit)
        {
            string stringlist = "01234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*";
            string num = "";
            int rnd_titleid = 0;

            Random rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 1; i <= digit; i++)
            {
                rnd_titleid = rnd.Next(0, stringlist.Length);
                num += stringlist.Substring(rnd_titleid, 1);
            }

            return num;
        }

        private static DataTable getOutstandingDownloadRequest()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT * FROM [TB_DOWNLOAD_REQUEST] WHERE Status = 0";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    conn.Close();
                    da.Dispose();
                }

            }

            return dataTable;
        }

        private static string getEmailTemplate(string template)
        {
            string content = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT Email_Template_Content FROM TB_EMAIL_TEMPLATE WHERE Email_Template = @emailTemplate;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@emailTemplate", template);
                    content = cmd.ExecuteScalar().ToString();
                    conn.Close();
                }
            }

            return content;
        }

        private static List<string> GetPresentationSlideAttachment(List<String> ApplicationIDS)
        {
            List<string> attachmentPaths = new List<String>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = String.Format("select Attachment_Type,Attachment_Path from TB_APPLICATION_ATTACHMENT where Application_ID in ({0}) and Attachment_Type <> 'HK_ID' and Attachment_Type <> 'BR_COPY' and Attachment_Type <> 'Video_Clip' and Attachment_Type <> 'Student_ID' and Attachment_Type <> 'Company_Annual_Return' and Attachment_Type <> 'Presentation_Slide_Response';", String.Join(",", ApplicationIDS.Select((x, i) => "@ApplicationID" + i).ToArray()));
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();
                        for (int i = 0; i < ApplicationIDS.Count; i++)
                        {
                            cmd.Parameters.AddWithValue("@ApplicationID" + i, ApplicationIDS[i]);
                        }
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            attachmentPaths.Add(reader.GetString(1));
                        }
                        conn.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPresentationSlideAttachment Error: " + ex);
                LogHelper.Exception("GetPresentationSlideAttachment Error", ex);
            }

            return attachmentPaths;

        }

        private static void updateRequestStatus(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = "UPDATE TB_DOWNLOAD_REQUEST SET Status = 1 WHERE ID  = @id";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        cmd.Dispose();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("updateRequestStatus Error: " + ex);
                LogHelper.Exception("updateRequestStatus Error", ex);
            }
        }

        private static void sharepointsendemail(string toAddress, string subject, string body)
        {
            LogHelper.Debug("Sending email via sharepoint");
            try
            {
                using (SPSite site = new SPSite(_spSite))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPUtility.SendEmail(web, false, false,
                                                  toAddress,
                                                  subject,
                                                  body);

                        LogHelper.Debug("Email Sent");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("sharepointsendemail Error: " + ex);
                LogHelper.Exception("sharepointsendemail Error", ex);

            }

        }

        private static void insertTBDownloadZIP(string userName, string email, string type, string path, string fileName, string password, string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = @"INSERT INTO TB_Download_ZIP(User_Name,Email,type,Path,File_Name,Password,Status,Created_By,Created_Date,Modified_By,Modified_Date) 
                               values( @userName, @email, @type, @path, @fileName, @password, @status, @reqUser, GETDATE(), @reqUser, GETDATE()  )";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@path", path);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@reqUser", userName);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertTBDownloadZIP Error: " + ex);
                LogHelper.Exception("insertTBDownloadZIP Error", ex);
            }
        }

        private static string getSystemParam(string configCode)
        {
            string value = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = "SELECT VALUE FROM TB_SYSTEM_PARAMETER WHERE Config_Code = @config";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@config", configCode);

                        conn.Open();
                        value = Convert.ToString(cmd.ExecuteScalar());
                        conn.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getSystemParam Error: " + ex);
                LogHelper.Exception("getSystemParam Error", ex);
            }
            return value;
        }

        private static string getProgrammeType(string progID)
        {
            string progName = string.Empty;
            string progType = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = "SELECT Programme_Name FROM TB_PROGRAMME_INTAKE WHERE Programme_ID = @m_Programme_ID";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@m_Programme_ID", progID);
                        conn.Open();
                        progName = cmd.ExecuteScalar().ToString();
                        conn.Close();
                    }
                }

                if (progName.Contains("Cyberport Creative Micro Fund"))
                {
                    progType = "CCMF";
                }
                else if (progName.Contains("Cyberport Incubation"))
                {
                    progType = "CPIP";
                }
                else if (progName.Contains("Cyberport University Partnerhip"))
                {
                    progType = "CUPP";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getProgrammeType Error: " + ex);
                LogHelper.Exception("getProgrammeType Error", ex);

            }

            return progType;

        }

        private static void checkDownloadFileExpired()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    foreach (string fileName in Directory.GetFiles(getSystemParam("zipfiledownloadpath")))
                    {
                        FileInfo fileinfo = new FileInfo(fileName);
                        double Diffhour = (DateTime.Now - fileinfo.CreationTime).TotalHours;
                        if (Diffhour > Convert.ToInt32(getSystemParam("ZipFileDeletionPeriod")))
                        {
                            try
                            {
                                if (fileinfo.Exists)
                                {

                                    if (!fileName.Contains("logDownload"))
                                    {
                                        fileinfo.Delete();
                                        Console.WriteLine(fileinfo.Name + " deleted.");
                                        LogHelper.Debug(fileinfo.Name + " deleted.");
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Delete File Error", ex);
                                LogHelper.Exception("Delete File Error", ex);
                            }
                            var sql = "update TB_Download_ZIP set Status = 0 where path = @fileName ";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                conn.Open();
                                cmd.Parameters.AddWithValue("@fileName", fileName);
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                cmd.Dispose();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("checkDownloadFileExpired Error: " + ex);
                LogHelper.Exception("checkDownloadFileExpired Error", ex);
            }
        }

        private static List<TimeSlotList> getApplication(string programmeID, string vmID, string streamFilter, string shortListFilter)
        {
            List<TimeSlotList> lstTimeSlotList = new List<TimeSlotList>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {

                    string sql = "";

                    if (getProgrammeType(programmeID) == "CCMF")
                    {

                        sql = @"SELECT * FROM (
                            SELECT vetting.Vetting_Application_ID, 
                            CCMF.Application_Number,
                            CCMF.CCMF_ID,
                            CCMF.Programme_Type,
                            CCMF.Hong_Kong_Programme_Stream,
                            CCMF.CCMF_Application_Type,
                            CCMF.Programme_ID,
                            CCMF.Status,
                            isNull(meeting.Date,'') as Date,
                            meeting.Vetting_Meeting_ID as Vetting_Meeting_ID,
                            Shortlisted
                            FROM TB_CCMF_APPLICATION CCMF
                            LEFT JOIN TB_VETTING_APPLICATION vetting ON CCMF.Application_Number = vetting.Application_Number
                            LEFT JOIN TB_VETTING_MEETING meeting ON meeting.Vetting_Meeting_ID = vetting.Vetting_Meeting_ID
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CCMF.Application_Number
                            WHERE isnull(CONVERT(varchar(50),meeting.Vetting_Meeting_ID),'') = @MeetingID AND CCMF.Status = 'Complete Screening'

                            UNION ALL

                            SELECT 
                            null,
                            CCMF.Application_Number,
                            CCMF.CCMF_ID,
                            CCMF.Programme_Type,
                            CCMF.Hong_Kong_Programme_Stream,
                            CCMF.CCMF_Application_Type,
                            CCMF.Programme_ID,
                            CCMF.Status,
                            null as Date,
                            null as Vetting_Meeting_ID,
                            Shortlisted
                            FROM TB_CCMF_APPLICATION CCMF
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CCMF.Application_Number
                            WHERE CCMF.Application_Number NOT IN (SELECT Application_Number FROM TB_VETTING_APPLICATION)
                            AND CCMF.Status = 'Complete Screening'
                            AND CCMF.Programme_ID = @ProgrammeID
                            AND Shortlisted = 0 
                            ) as CCMF
                            ORDER BY Application_Number";

                    }
                    else
                    {
                        sql = @"SELECT * FROM (
                            SELECT 
                            vetting.Vetting_Application_ID,
                            isnull(shortlist.Application_Number,'') as Application_Number, 
                            CPIP.Incubation_ID, 
                            shortlist.[Programme_ID], isNull(meeting.Date,'') as Date,
                            meeting.Vetting_Meeting_ID, CPIP.Status,
                            isnull(Shortlisted,0) as Shortlisted
                            FROM TB_VETTING_MEETING meeting
                            LEFT JOIN TB_VETTING_APPLICATION vetting ON meeting.Vetting_Meeting_ID = vetting.Vetting_Meeting_ID
                            LEFT JOIN TB_INCUBATION_APPLICATION CPIP ON vetting.Application_Number = CPIP.Application_Number
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CPIP.Application_Number
                            WHERE meeting.Vetting_Meeting_ID = @MeetingID AND CPIP.status = 'Complete Screening' 

                            UNION ALL

                            SELECT 
                            null as Vetting_Application_ID,
                            shortlist.Application_Number, CPIP.Incubation_ID,
                            shortlist.[Programme_ID], null as Date,
                            null as Vetting_Meeting_ID, CPIP.Status,
                            Shortlisted
                            FROM TB_INCUBATION_APPLICATION CPIP
                            LEFT JOIN TB_APPLICATION_SHORTLISTING shortlist on shortlist.Application_Number = CPIP.Application_Number
                            WHERE CPIP.Application_Number NOT IN (SELECT Application_Number FROM TB_VETTING_APPLICATION)
                            AND CPIP.Status = 'Complete Screening'
                            AND CPIP.Programme_ID = @ProgrammeID
                            AND Shortlisted = 0 
                            ) as CPIP
                            ORDER BY Application_Number";

                    }

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@MeetingID", vmID);
                        cmd.Parameters.AddWithValue("@ProgrammeID", programmeID);

                        //m_ddlDateFilter.SelectedItem.Text = getVettingDate();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            int iVettingDate = reader.GetOrdinal("Date");
                            string vettingDate = string.Empty;
                            int iVettingID = reader.GetOrdinal("Vetting_Meeting_ID");
                            string vettingID = string.Empty;
                            int iVettingAppID = reader.GetOrdinal("Vetting_Application_ID");
                            string vettingAppID = string.Empty;

                            if (shortListFilter == "All" ||
                               (shortListFilter == "Shortlisted" && (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted")) == true) ||
                               (shortListFilter == "Non Shortlisted" && (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted")) == false))
                            {
                                if (!reader.IsDBNull(iVettingDate))
                                {
                                    vettingDate = reader.GetDateTime(iVettingDate).ToString("dd MMM, yyyy");
                                }
                                if (!reader.IsDBNull(iVettingID))
                                {
                                    vettingID = reader.GetValue(iVettingID).ToString();
                                }
                                if (!reader.IsDBNull(iVettingAppID))
                                {
                                    vettingAppID = reader.GetValue(iVettingAppID).ToString();
                                }

                                if (getProgrammeType(programmeID) == "CCMF")
                                {
                                    if (streamFilter == "All" ||
                                    (streamFilter == reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")).ToString().Replace("Professional", "PRO").Replace("Young Entrepreneur", "YEP")))
                                    {

                                        lstTimeSlotList.Add(new TimeSlotList
                                        {
                                            Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                            Application_ID = reader.GetValue(reader.GetOrdinal("CCMF_ID")).ToString(),
                                            Programme_Type = reader.GetValue(reader.GetOrdinal("Programme_Type")).ToString(),
                                            CCMF_Application_Type = reader.GetValue(reader.GetOrdinal("CCMF_Application_Type")).ToString(),
                                            Vetting_Application_ID = vettingAppID,
                                            Vetting_Meeting_ID = vettingID,
                                            Vetting_Meeting_Date = vettingDate,
                                            Hong_Kong_Programme_Stream = reader.GetValue(reader.GetOrdinal("Hong_Kong_Programme_Stream")).ToString().Replace("Professional", "PRO").Replace("Young Entrepreneur", "YEP"),
                                            ShortlistedChecked = (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted"))
                                        });
                                    }
                                }
                                else
                                {

                                    lstTimeSlotList.Add(new TimeSlotList
                                    {
                                        Application_Number = reader.GetValue(reader.GetOrdinal("Application_Number")).ToString(),
                                        Application_ID = reader.GetValue(reader.GetOrdinal("Incubation_ID")).ToString(),
                                        Company = reader.GetValue(reader.GetOrdinal("Company_Name_Eng")).ToString(),
                                        Vetting_Application_ID = vettingAppID,
                                        Vetting_Meeting_ID = vettingID,
                                        Vetting_Meeting_Date = vettingDate,
                                        ShortlistedChecked = (Boolean)reader.GetValue(reader.GetOrdinal("Shortlisted"))
                                    });
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("List<TimeSlotList> lstTimeSlotList " + ex);
                LogHelper.Exception("List<TimeSlotList> lstTimeSlotList ", ex);
            }

            return lstTimeSlotList;
        }

        private static List<string> getApplication(string programmeID, string vmID, string email)
        {

            List<string> applicationIDList = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
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

                    if (getProgrammeType(programmeID) == "CPIP")
                    {
                        //CPIP
                        sqlColumn += " ,isNull(tApp.Company_Name_Eng,'') as Company_Name_Eng,"
                                    + "tApp.Incubation_ID as Application_ID,tApp.Programme_ID";
                        sqlFrom += " inner join TB_INCUBATION_APPLICATION tApp on tApp.Application_Number = tba.Application_Number "
                                    + "left join TB_PRESENTATION_INCUBATION_SCORE tsScore on tsScore.Application_Number = tba.Application_Number and tsScore.Member_Email = @Email";

                        sqlColumnProjectDesc = " ,tApp.Abstract as ProjectDesc ";
                        sqlScreeningScoreTable = "TB_SCREENING_INCUBATION_SCORE";
                        BDMRole = "CPIP BDM";
                    }
                    else
                    {
                        //CCMF
                        sqlColumn += " ,isNull(tApp.Project_Name_Eng,'') as Project_Name_Eng,"
                                    + "isNull(tApp.Programme_Type,'') as Programme_Type,"
                                    + "isNull(tApp.CCMF_Application_Type,'') as CCMF_Application_Type,"
                                    + "tApp.CCMF_ID as Application_ID,tApp.Programme_ID";
                        sqlFrom += " inner join TB_CCMF_APPLICATION tApp on tApp.Application_Number = tba.Application_Number "
                                    + "left join TB_PRESENTATION_CCMF_SCORE tsScore on tsScore.Application_Number = tba.Application_Number and tsScore.Member_Email = @Email ";

                        sqlColumnProjectDesc = " ,tApp.Abstract_Eng as ProjectDesc ";
                        sqlScreeningScoreTable = "TB_SCREENING_CCMF_SCORE";
                        BDMRole = "CCMF BDM";
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

                    using (SqlCommand cmd = new SqlCommand(sqlString, conn))
                    {
                        cmd.Parameters.AddWithValue("@m_VMID", vmID);
                        cmd.Parameters.AddWithValue("@Email", email);

                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string applicationID = reader.GetValue(reader.GetOrdinal("Application_ID")).ToString();

                            applicationIDList.Add(applicationID);
                        }
                        conn.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("List<string> getApplication " + ex);
                LogHelper.Exception("List<string> getApplication ", ex);
            }
            return applicationIDList;
        }
    }
}
