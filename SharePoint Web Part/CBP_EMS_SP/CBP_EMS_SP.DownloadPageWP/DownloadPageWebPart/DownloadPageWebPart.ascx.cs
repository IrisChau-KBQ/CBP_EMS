using Microsoft.SharePoint;
using Microsoft.SharePoint.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace CBP_EMS_SP.DownloadPageWP.DownloadPageWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class DownloadPageWebPart : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

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
        private String m_UserName;
        private String m_Mail;
        private String m_Path = System.Configuration.ConfigurationManager.AppSettings["DownloadPath"] ?? @"D:\\tmp";

        public DownloadPageWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_UserName = SPContext.Current.Web.CurrentUser.Name.ToString();
            //m_UserName = "System Account";
            m_Mail = SPContext.Current.Web.CurrentUser.Email;

            if (!Page.IsPostBack)
            {
                //CheckDownloadFileExpired();
                BindGridViewDownload();
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

        protected void BindGridViewDownload()
        {
            List<DownloadClass> downloadList = new List<DownloadClass>();
            ConnectOpen();
            try
            {
                var sqlString = "select User_Name,Email,type,Path,File_Name,Created_By,Created_Date from TB_Download_ZIP where type in( 'ZIP','PDF ZIP') and Status = '1' and User_Name= @m_UserName order by Created_Date desc;";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@m_UserName", m_UserName));

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    downloadList.Add(new DownloadClass
                    {
                        Path = reader.GetString(3),
                        FileName = reader.GetString(4),
                        CreatedDate = reader.GetDateTime(6).ToString("yyyy/MM/dd HH:mm:ss")

                    });
                }
                reader.Dispose();
                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
            GridViewDownload.DataSource = downloadList;
            GridViewDownload.DataBind();
        }

        protected void CheckDownloadFileExpired()
        {

            ConnectOpen();
            try
            {
                foreach (string fileName in Directory.GetFiles(m_Path))
                {
                    var fileinfo = new FileInfo(fileName);
                    var Diffhour = (DateTime.Now - fileinfo.CreationTime).TotalHours;
                    if (Diffhour > 24)
                    {
                        try
                        {
                            //File.Delete(fileName);
                            fileinfo.Delete();
                        }
                        catch
                        {

                        }
                        var sql = "update TB_Download_ZIP set Status = 0 where path=@fileName ";
                        var command = new SqlCommand(sql, connection);
                        command.Parameters.Add(new SqlParameter("@fileName", fileName));

                        command.ExecuteNonQuery();
                        command.Dispose();


                    }


                }

            }
            finally
            {
                ConnectClose();
            }

        }

        protected void inserttoDownloadZIP(String Path, String File_Name, String Password)
        {
            ConnectOpen();
            try
            {
                string DownloadType = File_Name.ToLower().Contains("pdf") ? "Download PDF" : "Downloaded";
                var sql = "insert into TB_Download_ZIP(User_Name,Email,type,Path,File_Name,Password,Status,Created_By,Created_Date,Modified_By,Modified_Date) values("
                                        + "@m_UserName, "
                                        + "@m_Mail, "
                                        + "@DownloadType, "
                                        + "@Path, "
                                        + "@File_Name, "
                                        + "@Password, "
                                        + "'1', "
                                        + "@m_UserName, "
                                        + "GETDATE(), "
                                        + "@m_UserName, "
                                        + "GETDATE() "
                                        + " ) ;";
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@m_UserName", m_UserName));
                command.Parameters.Add(new SqlParameter("@DownloadType", DownloadType));
                
                command.Parameters.Add(new SqlParameter("@m_Mail", m_Mail));
                command.Parameters.Add(new SqlParameter("@Path", Path));
                command.Parameters.Add(new SqlParameter("@File_Name", File_Name));
                command.Parameters.Add(new SqlParameter("@Password", Password));

                command.ExecuteNonQuery();


                command.Dispose();
            }
            finally
            {
                ConnectClose();
            }
        }

        protected void LinkButtonDownload_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            ConnectOpen();
            try
            {
                var path = e.CommandName;
                var sqlString = "select top 1 User_Name,Email,type,Path,File_Name,Password,Created_By,Created_Date from TB_Download_ZIP where path=@path and Status = '1' and type in ('ZIP','PDF ZIP');";
                var command = new SqlCommand(sqlString, connection);
                command.Parameters.Add(new SqlParameter("@path", path));

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var FileName = reader.GetString(4);
                    var Password = reader.GetString(5);

                    //Log(path);
                    string dir = GetSysParam("zipfiledownloadpath");
                    using (new NetworkConnection(dir, new NetworkCredential(GetSysParam("NetworkDriveUser"), GetSysParam("NetworkDrivePassword"), GetSysParam("NetworkDriveDomain"))))
                    {
                        //Log("Path: " + System.IO.File.Exists(path).ToString());
                        if (System.IO.File.Exists(path))
                        {
                            //Log("Download Page: File Exists, Downloading");
                            inserttoDownloadZIP(path, FileName, Password);


                            //var MyFileInfo = new FileInfo(path);
                            //var FileSize = MyFileInfo.Length;

                            Context.Response.Clear();
                            Context.Response.ClearHeaders();
                            Context.Response.ClearContent();
                            Context.Response.AddHeader("content-disposition", "attachment; filename = " + FileName);
                            Context.Response.ContentType = "application/zip";
                            Context.Response.AddHeader("Pragma", "public");

                            Context.Response.WriteFile(path);
                            Context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                            Context.Response.End(); //send and close
                            //Log("Download Page: Download Finished");
                            Context.Response.Redirect(Context.Request.RawUrl);

                            //var buffer = System.IO.File.ReadAllBytes(path);
                            //Context.Response.BinaryWrite(buffer);

                            //HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                            //HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                            //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.

                            //Context.Response.Redirect(Context.Request.RawUrl);
                        }
                    }
                }
                reader.Dispose();
                command.Dispose();

            }
            catch (Exception ex)
            {
                //Log("Error: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                ConnectClose();
            }

            //Context.Response.Redirect(Context.Request.RawUrl);
        }

        //public void Log(String message)
        //{

        //    //TB_Debug_Log

        //    string islog = GetSysParam("IsLogEvent");
        //    string path = GetSysParam("zipfiledownloadpath");
        //    if (islog == "1")
        //    {
        //        using (SqlConnection conn = new SqlConnection(connStr))
        //        {
        //            string sql = @" INSERT INTO [TB_Debug_Log] (log_message, log_time) VALUES (@message, @time)";

        //            using (SqlCommand cmd = new SqlCommand(sql, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@message", message);
        //                cmd.Parameters.AddWithValue("@time", DateTime.Now);

        //                conn.Open();
        //                cmd.ExecuteNonQuery();
        //                conn.Close();
        //            }
        //        }
        //    }
        //}

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
    }

    public class DownloadClass
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string CreatedDate { get; set; }

    }
}
