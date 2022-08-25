using CBP_EMS_SP.Data.Models;
using Microsoft.ApplicationServer.Caching;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace CBP_EMS_SP.Common
{
    public partial class SPFunctions
    {
        public SPFunctions() { }
        public static string ExternalUserGroup
        {
            get
            {
                return (String)WebConfigurationManager.AppSettings["SPExternalUserGroupName"];
            }
        }

        public static SPWeb GetCurrentWeb
        {
            get
            {
                SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPWeb site = siteCollection.OpenWeb();
                return site;
            }
        }

        public string GetCurrentUser()
        {
            try
            {
                string UserName = string.Empty;
                using (SPSite oSPsite = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url))
                {
                    using (SPWeb oSPweb = oSPsite.OpenWeb())
                    {
                        SPUser CurrentUser = oSPweb.CurrentUser;
                        UserName = CurrentUser.Name;
                        // SPContext.Current.Web.CurrentUser.Email
                    }
                }
                return UserName;
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        public bool CurrentUserIsInGroup(string GroupName, bool IsAdUser = false)
        {
            string UserEmail = (new SPFunctions().GetCurrentUser());
            bool UserExists = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPWeb site = GetCurrentWeb)
                    {
                        SPGroup oGroup = site.SiteGroups[GroupName];
                        foreach (SPUser objuser in oGroup.Users)
                        {
                            if (objuser.Email.ToLower() == UserEmail.ToLower() || (objuser.Name.Contains("@") && objuser.Name.ToLower() == UserEmail.ToLower()))
                                UserExists = true;
                            if (!UserExists && IsAdUser && objuser.Name.ToLower() == UserEmail.ToLower())
                            {
                                UserExists = true;
                            }
                        }
                    }


                });

            }
            catch (Exception)
            {

            }

            return UserExists;
        }

        public bool IsFolderExists(SPWeb oWeb, string FolderUrl)
        {
            return oWeb.GetFolder(FolderUrl).Exists;
        }

        public bool CreateFolder(SPWeb oWeb, string FolderUrlWithRoot)
        {
            try
            {
                SPFolder oFolder = null;
                foreach (string FolderName in FolderUrlWithRoot.Split('/').Where(x => !string.IsNullOrEmpty(x)))
                {
                    string strFolder = oFolder == null ? FolderName : (oFolder.ToString() + "/" + FolderName);
                    if (!IsFolderExists(oWeb, strFolder))
                    {
                        oWeb.Folders.Add(strFolder);
                    }
                    oFolder = oWeb.GetFolder(strFolder);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string AttachmentSave(string ApplicationNumber, TB_PROGRAMME_INTAKE objProgram, FileUpload FileUpload1,
            enumAttachmentType oAttachmentType, string ApplicationVersion)
        {
            string _fileUrl = string.Empty;

            if (FileUpload1.HasFile)
            {
                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
                string RootFolder = objTbParams.FirstOrDefault(x => x.Config_Code == "AttachmentPrimaryFolder").Value;
                string FolderAttachement = objTbParams.FirstOrDefault(x => x.Config_Code == "AttachmentSecondaryFolder").Value;
                SPSite site1 = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPWeb web1 = site1.OpenWeb();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPSite site = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    SPWeb web = site.OpenWeb();
                    web.AllowUnsafeUpdates = true;
                    string ProgramFolderName = objProgram.Programme_Name + " " + objProgram.Intake_Number;
                    string ProgramFolderUrl = RootFolder + "/" + ProgramFolderName + "/" + ApplicationNumber; ;
                    if (!string.IsNullOrEmpty(FolderAttachement))
                        ProgramFolderUrl = RootFolder + "/" + FolderAttachement + "/" + ProgramFolderName + "/" + ApplicationNumber;

                    //string ProgramFolderUrl = ProgramFolderName + "/" + ApplicationNumber; ;
                    //if (!string.IsNullOrEmpty(FolderAttachement))
                    //    ProgramFolderUrl = FolderAttachement + "/" + ProgramFolderName + "/" + ApplicationNumber;


                    //SPListItem item = list.AddItem(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder,"test folder");
                    //item.Update();
                    if (!IsFolderExists(web, ProgramFolderUrl))
                    {
                        CreateFolder(web, ProgramFolderUrl);
                    }
                    SPFolder SharedFolder = web.GetFolder(ProgramFolderUrl);

                    Stream fStream = FileUpload1.PostedFile.InputStream;
                    byte[] _byteArray = new byte[fStream.Length];
                    fStream.Read(_byteArray, 0, (int)fStream.Length);
                    fStream.Close();
                    if (oAttachmentType == enumAttachmentType.Other_Attachment || oAttachmentType == enumAttachmentType.HK_ID || oAttachmentType == enumAttachmentType.Student_ID || oAttachmentType == enumAttachmentType.Presentation_Slide_Response)
                    {
                        int i = 0;
                        Int32.TryParse(ApplicationVersion, out i);
                        ApplicationVersion = (i + 1).ToString();
                    }
                    else ApplicationVersion = CreateVersion(ApplicationVersion) != string.Empty ? "_" + CreateVersion(ApplicationVersion) : "";


                    string Extension = FileUpload1.FileName.Remove(0, FileUpload1.FileName.LastIndexOf("."));

                    string FileName = ApplicationNumber + "_" + oAttachmentType.ToString() + ApplicationVersion + Extension;
                    //string FileName = ApplicationNumber + "_" + oAttachmentType.ToString()+ Extension;
                    // _fileUrl = ProgramFolderUrl +"/"+ApplicationNumber+"_"+ FileName
                    _fileUrl = ProgramFolderUrl + "/" + FileName;


                    SharedFolder.Files.Add(FileName, _byteArray, true);
                    web.AllowUnsafeUpdates = false;

                    SPFolder list = web.GetFolder(ProgramFolderUrl);
                    web.AllowUnsafeUpdates = true;
                    if (!list.Item.HasUniqueRoleAssignments)
                    {
                        list.Item.BreakRoleInheritance(false, true);
                    }

                    SPRoleAssignment roleAssignment = new SPRoleAssignment(web1.CurrentUser);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    list.Item.RoleAssignments.Add(roleAssignment);
                    web.AllowUnsafeUpdates = false;


                });





            }
            return _fileUrl;
        }



        public string SR_AttachmentSave(string ApplicationNumber, string programName, FileUpload FileUpload1,
         enumAttachmentType oAttachmentType, string ApplicationVersion)
        {
            string _fileUrl = string.Empty;

            if (FileUpload1.HasFile)
            {
                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
                string RootFolder = objTbParams.FirstOrDefault(x => x.Config_Code == "AttachmentPrimaryFolder").Value;
                string FolderAttachement = objTbParams.FirstOrDefault(x => x.Config_Code == "AttachmentSecondaryFolder").Value;
                SPSite site1 = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPWeb web1 = site1.OpenWeb();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPSite site = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    SPWeb web = site.OpenWeb();
                    web.AllowUnsafeUpdates = true;
                    string ProgramFolderName = programName;
                    string ProgramFolderUrl = RootFolder + "/" + ProgramFolderName + "/" + ApplicationNumber; ;
                    if (!string.IsNullOrEmpty(FolderAttachement))
                        ProgramFolderUrl = RootFolder + "/" + FolderAttachement + "/" + ProgramFolderName + "/" + ApplicationNumber;

                    //string ProgramFolderUrl = ProgramFolderName + "/" + ApplicationNumber; ;
                    //if (!string.IsNullOrEmpty(FolderAttachement))
                    //    ProgramFolderUrl = FolderAttachement + "/" + ProgramFolderName + "/" + ApplicationNumber;


                    //SPListItem item = list.AddItem(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder,"test folder");
                    //item.Update();
                    if (!IsFolderExists(web, ProgramFolderUrl))
                    {
                        CreateFolder(web, ProgramFolderUrl);
                    }
                    SPFolder SharedFolder = web.GetFolder(ProgramFolderUrl);

                    Stream fStream = FileUpload1.PostedFile.InputStream;
                    byte[] _byteArray = new byte[fStream.Length];
                    fStream.Read(_byteArray, 0, (int)fStream.Length);
                    fStream.Close();
                    if (oAttachmentType == enumAttachmentType.Other_Attachment || oAttachmentType == enumAttachmentType.HK_ID || oAttachmentType == enumAttachmentType.Student_ID || oAttachmentType == enumAttachmentType.Presentation_Slide_Response)
                    {
                        int i = 0;
                        Int32.TryParse(ApplicationVersion, out i);
                        ApplicationVersion = (i + 1).ToString();
                    }
                    else ApplicationVersion = CreateVersion(ApplicationVersion) != string.Empty ? "_" + CreateVersion(ApplicationVersion) : "";


                    string Extension = FileUpload1.FileName.Remove(0, FileUpload1.FileName.LastIndexOf("."));

                    string FileName = ApplicationNumber + "_" + oAttachmentType.ToString() + ApplicationVersion + Extension;
                    //string FileName = ApplicationNumber + "_" + oAttachmentType.ToString()+ Extension;
                    // _fileUrl = ProgramFolderUrl +"/"+ApplicationNumber+"_"+ FileName
                    _fileUrl = ProgramFolderUrl + "/" + FileName;


                    SharedFolder.Files.Add(FileName, _byteArray, true);
                    web.AllowUnsafeUpdates = false;

                    SPFolder list = web.GetFolder(ProgramFolderUrl);
                    web.AllowUnsafeUpdates = true;
                    if (!list.Item.HasUniqueRoleAssignments)
                    {
                        list.Item.BreakRoleInheritance(false, true);
                    }

                    SPRoleAssignment roleAssignment = new SPRoleAssignment(web1.CurrentUser);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    list.Item.RoleAssignments.Add(roleAssignment);
                    web.AllowUnsafeUpdates = false;


                });





            }
            return _fileUrl;
        }



        public TB_APPLICATION_COLLABORATOR CurrentUserIsCollaborator(CyberportEMS_EDM dbContext, int programId, string ApplicationNumber)
        {
            string CurrentUser = GetCurrentUser().ToLower();
            TB_APPLICATION_COLLABORATOR objCollb = dbContext.TB_APPLICATION_COLLABORATOR.FirstOrDefault(x => x.Email == CurrentUser && x.Programme_ID == programId && x.Application_Number == ApplicationNumber);
            return objCollb;
        }
        public static string GetMacAddress()
        {
            string macaddress = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                                 where nic.OperationalStatus == OperationalStatus.Up
                                 select nic.GetPhysicalAddress().ToString()).FirstOrDefault();

            return macaddress;
        }

        //public void CurrentUserIsInGroup(string GroupName)
        //{
        //    string message = "test " + GroupName;
        //    string UserEmail = new SPFunctions().GetCurrentUser();
        //    message += UserEmail;
        //    //   string UserEmail = (new SPFunctions().GetCurrentUser());
        //    bool UserExists = false;
        //    try
        //    {
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            message += "site.Name";
        //            SPWeb site = SPFunctions.GetCurrentWeb;
        //            message += site.Name;


        //            SPGroup oGroup = site.SiteGroups[GroupName];
        //            message += oGroup.Name;
        //            message += oGroup.Users.Count.ToString();
        //            foreach (SPUser objuser in oGroup.Users)
        //            {
        //                // if (objuser.Email.ToLower() == UserEmail.ToLower())
        //                message += "<br/>" + objuser.Name;
        //            }

        //            //if (objUser != null)
        //            //    UserExists = true;

        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        message += ex.InnerException.ToString();
        //    }
        //    //lbl_success.InnerText = message;
        //    //return UserExists;
        //    lbl_success.InnerText = message;

        //}

        private static string CreateVersion(string strVersion)
        {

            try
            {
                if (strVersion != "0.1")
                {
                    Int32 i = 0;
                    strVersion = strVersion.Remove(0, strVersion.LastIndexOf(".") + 1);
                    strVersion = strVersion.EndsWith("0") ? strVersion.Remove(strVersion.LastIndexOf("0"), 1) : strVersion;
                    Int32.TryParse(strVersion, out i);
                    strVersion = (i - 1).ToString();
                }
                else
                    strVersion = string.Empty;

            }
            catch (Exception)
            {
                strVersion = string.Empty;
            }
            return strVersion;
        }

        public static void LocalizeUIForPage(string LanguageKey)
        {
            LanguageKey = string.IsNullOrEmpty(LanguageKey) ? "en-US" : LanguageKey;
            CultureInfo culture = new CultureInfo(LanguageKey);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            //CacheManager objCommonFunctions = new CacheManager();
            //DataCache emsuserlanguage = objCommonFunctions.DefaultCache;
            //string macaddress = SPFunctions.GetMacAddress();

            //if (!string.IsNullOrEmpty(new SPFunctions().GetCurrentUser()))
            //{
            //    if (emsuserlanguage[new SPFunctions().GetCurrentUser()] != null)
            //    {
            //        var CultureName = emsuserlanguage[new SPFunctions().GetCurrentUser()];
            //        CultureInfo culture = new CultureInfo(CultureName.ToString());
            //        Thread.CurrentThread.CurrentCulture = culture;
            //        Thread.CurrentThread.CurrentUICulture = culture;
            //    }
            //}
            //else if (emsuserlanguage[macaddress] != null)
            //{
            //    var CultureName = emsuserlanguage[macaddress];

            //    CultureInfo culture = new CultureInfo(CultureName.ToString());
            //    Thread.CurrentThread.CurrentCulture = culture;
            //    Thread.CurrentThread.CurrentUICulture = culture;

            //}

        }
        public static string LocalizeUI(string Key, string ResourceFileName)
        {

            // LocalizeUIForPage();
            //CacheManager objCommonFunctions = new CacheManager();
            //string macaddress = SPFunctions.GetMacAddress();
            //DataCache emsuserlanguage = objCommonFunctions.DefaultCache;
            //if (emsuserlanguage[macaddress] != null)
            //{
            //    var CultureName = emsuserlanguage[macaddress];

            //    CultureInfo culture = new CultureInfo(CultureName.ToString());
            //    Thread.CurrentThread.CurrentCulture = culture;
            //    Thread.CurrentThread.CurrentUICulture = culture;

            //}

            int Culture = Thread.CurrentThread.CurrentCulture.LCID;
            return SPUtility.GetLocalizedString("$Resources:" + Key, ResourceFileName, (uint)Culture);

        }

        public static void Rollback(CyberportEMS_EDM dbContext)
        {

            //dbContext.Dispose();
            //CyberportEMS_EDM dataContext = new CyberportEMS_EDM();

            var changedEntries = dbContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public static string dbTextbyLanguage(string English, string HKSimplify, string HKTrad)
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "zh-HK")
            {
                return HKTrad;
            }
            else if (Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
            {
                return HKSimplify;
            }
            else

            {
                return English;
            }

        }

        public static void DeleteAttachmentfromlist( TB_APPLICATION_ATTACHMENT objAttach)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                SPSite site = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPWeb web = site.OpenWeb();
                web.AllowUnsafeUpdates = true;
                SPFolder SharedFolder = web.GetFolder(objAttach.Attachment_Path);
                string fileUrl = SharedFolder.ToString().Remove(0, (objAttach.Attachment_Path).LastIndexOf("/") + 1);


                string folderUrl = objAttach.Attachment_Path.Substring(0, objAttach.Attachment_Path.LastIndexOf("/"));

                SPFolder folder = web.GetFolder(folderUrl);

                SPFile file = folder.Files[fileUrl];

                SPListItem item = file.Item;

                item.Delete();
                web.AllowUnsafeUpdates = false;

            });
        }
        //public static string ResourceTextbyKey(string Key)
        //{
        //    if (Thread.CurrentThread.CurrentCulture.Name == "zh-HK")
        //    {
        //        return LocalizeUI("Key ", "CyberportEMS_Common.zh-HK");
        //    }
        //    else if (Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
        //    {
        //        return LocalizeUI("Key ", "CyberportEMS_Common.zh-CN");
        //    }
        //    else

        //    {
        //        return LocalizeUI("Key ", "CyberportEMS_Common");
        //    }

        //}


        public string FA_AttachmentSave(string ApplicationNumber, string programName, FileUpload FileUpload1,
        enumAttachmentType oAttachmentType, string PreviousCount, string ShortName)
        {
            string _fileUrl = string.Empty;
            string ApplicationVersion = PreviousCount;

            if (FileUpload1.HasFile)
            {
                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
                string RootFolder = objTbParams.FirstOrDefault(x => x.Config_Code == "AttachmentPrimaryFolder").Value;
                string FolderAttachement = objTbParams.FirstOrDefault(x => x.Config_Code == "AttachmentSecondaryFolder").Value;
                SPSite site1 = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPWeb web1 = site1.OpenWeb();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    SPSite site = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    SPWeb web = site.OpenWeb();
                    web.AllowUnsafeUpdates = true;
                    string ProgramFolderName = programName;
                    string ProgramFolderUrl = RootFolder + "/" + ProgramFolderName + "/" + ApplicationNumber; ;
                    if (!string.IsNullOrEmpty(FolderAttachement))
                        ProgramFolderUrl = RootFolder + "/" + FolderAttachement + "/" + ProgramFolderName + "/" + ApplicationNumber;

                    //string ProgramFolderUrl = ProgramFolderName + "/" + ApplicationNumber; ;
                    //if (!string.IsNullOrEmpty(FolderAttachement))
                    //    ProgramFolderUrl = FolderAttachement + "/" + ProgramFolderName + "/" + ApplicationNumber;


                    //SPListItem item = list.AddItem(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder,"test folder");
                    //item.Update();
                    if (!IsFolderExists(web, ProgramFolderUrl))
                    {
                        CreateFolder(web, ProgramFolderUrl);
                    }
                    SPFolder SharedFolder = web.GetFolder(ProgramFolderUrl);

                    Stream fStream = FileUpload1.PostedFile.InputStream;
                    byte[] _byteArray = new byte[fStream.Length];
                    fStream.Read(_byteArray, 0, (int)fStream.Length);
                    fStream.Close();

                    string Extension = FileUpload1.FileName.Remove(0, FileUpload1.FileName.LastIndexOf("."));

                    string FileName = ApplicationNumber + "_" + (string.IsNullOrEmpty(ShortName) ? oAttachmentType.ToString() : ShortName) + ApplicationVersion + Extension;
                    //string FileName = ApplicationNumber + "_" + oAttachmentType.ToString()+ Extension;
                    // _fileUrl = ProgramFolderUrl +"/"+ApplicationNumber+"_"+ FileName
                    _fileUrl = ProgramFolderUrl + "/" + FileName;


                    SharedFolder.Files.Add(FileName, _byteArray, true);
                    web.AllowUnsafeUpdates = false;

                    SPFolder list = web.GetFolder(ProgramFolderUrl);
                    web.AllowUnsafeUpdates = true;
                    if (!list.Item.HasUniqueRoleAssignments)
                    {
                        list.Item.BreakRoleInheritance(false, true);
                    }

                    SPRoleAssignment roleAssignment = new SPRoleAssignment(web1.CurrentUser);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    list.Item.RoleAssignments.Add(roleAssignment);
                    web.AllowUnsafeUpdates = false;


                });





            }
            return _fileUrl;
        }
    }
}
