using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.DownloadZipWP.DownloadZipWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class DownloadZipWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public String m_path = @"D:\\tmp";
        public String m_programName;
        public String m_intake;
        public String m_folderStruct = "";
        public String m_AttachmentPrimaryFolderName;
        public String m_AttachmentSecondaryFolderName;
        public String m_zipfiledownloadurl;
        public String m_downloadLink;
        
        //private String connStr = "Data Source=SPDEVSQL\\SPDEVSQLDB; Initial Catalog=CyberportWMS; persist security info=True; User Id=sa; Password=Password1234*;";
        //private string connStr = "Data Source=192.168.99.110; initial catalog=CyberportWMS; persist security info=True; user id=spservice; password=passw0rd!;";

        private string connStr
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["CyberportEMSConnectionString"].ConnectionString;
            }
        }
        private SqlConnection connection;

        public DownloadZipWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SPSite site = SPContext.Current.Site;
            SPWeb web = site.OpenWeb();
            SPFolder folder = web.GetFolder(@"Shared Documents\ApplicationAttachments\Cyberport Incubation Programme 201705\CPIP-201705-0001");
            var str = @"Shared Documents\ApplicationAttachments\";
            str = m_AttachmentPrimaryFolderName + @"\" + m_AttachmentSecondaryFolderName + @"\";
            Label1.Text = folder.Url;
            foreach (SPFile file in folder.Files)
            { 
                var fileurl = file.Url.Substring(str.Length);
                Label1.Text += "<br>" + fileurl;
            }
            
        }

        protected void btnDownload_Click(object sender, EventArgs e)
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
            }

            reader.Dispose();
            command.Dispose();


            ConnectClose();


            m_programName = "Cyberport Incubation Programme";
            m_intake = "201705";
            String Source = m_AttachmentPrimaryFolderName + @"\" + m_AttachmentSecondaryFolderName + @"\" + m_programName + " " + m_intake;
            var FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            String Destination = m_path + @"\" + FileName;

            m_downloadLink = m_zipfiledownloadurl + FileName;

            processFolder(Source, Destination);

        }

        public void processFolder(string folderURL, string zipFile)
        {
            string m_username = SPContext.Current.Web.CurrentUser.Name.ToString();
            string m_mail = SPContext.Current.Web.CurrentUser.Email;
            //string m_mail = "Blue.Qiu@mouxidea.com.hk";
            string m_subject = "";
            string m_body = "";
            string m_Programme_Name = m_programName;
            string m_Intake_Number = m_intake;
            string m_Password = genRandom(6);
            string m_downloadlink = m_downloadLink;
            string m_Starttime;
            string m_Endtime;
            //string m_zipstatus = "done";

            

            //starting email
            m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is processing.";
            m_body = "Hi, Zip File is processing, please wait next email to confirm Zip File is ready.";
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
            Label1.Text = zipStream.Password;

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

            //Completed email                
            m_subject = "Zip File : " + m_Programme_Name + " / " + m_Intake_Number + " is completed.";
            m_body = "Hi, Zip File is ready, please download : " + m_downloadlink + " . <br/>Password is : " + m_Password + ". <br/>";
            m_body += "Zip Start time : " + m_Starttime + " to End Time :" + m_Endtime + "";
            sharepointsendemail(m_mail, m_subject, m_body);
            //sharepointsendemail("andysgi@gmail.com", "hi", "ko");
            //lbltest.Text = m_subject + m_body;
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

        //private void CompressFolder(SPFolder folder, ZipOutputStream zipStream, int folderOffset)
        //{
        //    foreach (SPFile file in folder.Files)
        //    {
        //        var filename = file.Name;
        //        FileInfo fi = new FileInfo(filename);

        //        //string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
        //        string entryName = filename;
        //        entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
        //        ZipEntry newEntry = new ZipEntry(entryName);
        //        newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

        //        // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
        //        // A password on the ZipOutputStream is required if using AES.
        //        //   newEntry.AESKeySize = 256;

        //        // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
        //        // you need to do one of the following: Specify UseZip64.Off, or set the Size.
        //        // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
        //        // but the zip will be in Zip64 format which not all utilities can understand.
        //        //   zipStream.UseZip64 = UseZip64.Off;
        //        newEntry.Size = fi.Length;

        //        zipStream.PutNextEntry(newEntry);

        //        // Zip the file in buffered chunks
        //        // the "using" will close the stream even if an exception occurs
        //        byte[] buffer = new byte[4096];

        //        using (FileStream streamReader = File.OpenRead(filename))
        //        {
        //            StreamUtils.Copy(streamReader, zipStream, buffer);
        //        }
        //        zipStream.CloseEntry();
        //    }

        //    foreach (SPFolder Item in folder.SubFolders)
        //    {
        //        CompressFolder(Item, zipStream, folderOffset);
        //    }
        //}


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
    }
}
