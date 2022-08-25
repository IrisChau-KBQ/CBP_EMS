using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Net;
using System.Net.Mail;

namespace CBP_EMS_SP.Downloadzip.DownloadZipFile
{
    [ToolboxItemAttribute(false)]
    public partial class DownloadZipFile : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        private string setpasswordforzip;
        private string path;
        private string outpath;
        private string zipfilename;
        private string startzip;
        private string endzip;
        private string txtBody;

        public DownloadZipFile()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            setpasswordforzip = genRandom(8);
            path = @"D:\tmp\test";
            outpath = @"D:\tmp";
            zipfilename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            lblnumber.Text = zipfilename;
            lblnumber.Text += "<br/>Password : " + setpasswordforzip; 
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

        protected void btnZipFolder_Click(object sender, EventArgs e)
        {
            zipfolder();
        }

        protected void zipfolder()
        {
            string zipFileName = outpath + @"\" + zipfilename;            
            CreateSample(zipFileName, setpasswordforzip, path);
        }

        public void CreateSample(string outPathname, string password, string folderName)
        {
            txtBody = "When zip is finished, we will send email to confirm you.";
            sendemailbysmtp("m62737166@gmail.com", "andysgi@gmail.com", "zip file is starting, please waiting.", txtBody);

//            startzip = DateTime.Now.ToString();
//            lblnumber.Text += "<br/> Start: " + startzip;
//
//            //ZipOutputStream zipStream = null;
//            //string zipPath = path + @"\" + Path.GetFileName(path);
//            //ArrayList files = GetFiles(path);
//            //zipStream = new ZipOutputStream(File.Create(zipPath));
//
//            FileStream fsOut = File.Create(outPathname);
//            ZipOutputStream zipStream = new ZipOutputStream(fsOut);
//
//            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression
//
//            zipStream.Password = password;	// optional. Null is the same as not setting. Required if using AES.
//
//            // This setting will strip the leading part of the folder path in the entries, to
//            // make the entries relative to the starting folder.
//            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
//            int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);
//
//            CompressFolder(folderName, zipStream, folderOffset);
//
//            zipStream.IsStreamOwner = true;	// Makes the Close also Close the underlying stream
//            zipStream.Close();
//
//            endzip = DateTime.Now.ToString();
//            lblnumber.Text += "<br/> End: " + endzip;
            txtBody = "zip file is ready, please download from : http://www.zipfile.com. <br/>password is : " + setpasswordforzip + "<br/>Start Zip time:" + startzip + "<br/>End Zip time:" + endzip;
            sendemailbysmtp("m62737166@gmail.com", "andysgi@gmail.com", "zip file is ready.", txtBody);
        }

        private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {

                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];

                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }

        protected void sendemailbysmtp(string txtFrom, string txtTo, string txtSubject, string txtbody)
        {
            //MailMessage mail = new MailMessage("you@yourcompany.com", "user@hotmail.com");
            MailMessage mail = new MailMessage(txtFrom, txtTo);
            SmtpClient client = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential("m62737166@gmail.com", "Oopser1234");
            //client.Port = 465;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";
            //client.Host = "smtp.bbmail.com.hk";

            mail.IsBodyHtml = true;
            mail.Subject = txtSubject;
            mail.Body = txtbody;
            client.Send(mail);

            //SPSite oSiteCollection = SPContext.Current.Site;
            //using (SPWeb oWebsite = oSiteCollection.AllWebs["Website_Name"])
            //{
            //    SPUser oUser = oWebsite.AllUsers["User_Name"];
            //
            //    oUser.Email = " E-mail_Address";
            //
            //    oUser.Update();
            //}
        }


    }
}
