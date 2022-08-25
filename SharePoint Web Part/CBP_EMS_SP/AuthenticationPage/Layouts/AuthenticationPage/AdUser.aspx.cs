using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
//using System.DirectoryServices;
//using System.DirectoryServices.ActiveDirectory;
using System.Security.Principal;
namespace AuthenticationPage.Layouts.AuthenticationPage
{
    public partial class AdUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url+ "/_windows/default.aspx?ReturnUrl=/SitePages/Home.aspx");
           
        }
        //private void Adcall() {

        //    try
        //    {
        //        SPSecurity.RunWithElevatedPrivileges(delegate ()
        //        {
        //            DirectoryEntry domain = GetDomainEntry();
        //            if (domain == null)
        //                throw new Exception("Domain not found.");
        //            using (SPSite site = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url))
        //            {
        //                using (SPWeb web = site.RootWeb)
        //                {
        //                    SPListItemCollection userItems = web.SiteUserInfoList.Items;
        //                    for (int i = 0; i < userItems.Count; i++)
        //                    {
        //                        try
        //                        {
        //                            double progress = ((double)(i + 1)) /
        //                                               (double)userItems.Count;
        //                            SPListItem userItem = userItems[i];
        //                            SPUser user = web.SiteUsers.GetByID(userItem.ID);
        //                            if (user == null)
        //                                throw new Exception(string.Format(
        //                                  "User account {0} not found in site {1}.",
        //                                  userItem.Name, site.Url));
        //                            DateTime dtUserItemUpdated =
        //                               (DateTime)userItem["Modified"];
        //                            if (IsPerson(userItem) && !IsSystem(user))
        //                            {
        //                                DirectoryEntry userInfo = GetUserInfo(user, domain);
        //                                //if (userInfo == null || !userInfo.IsActive)
        //                                //{
        //                                //    string jobTitle = (string)userItem["JobTitle"];
        //                                //    if (string.IsNullOrEmpty(jobTitle))
        //                                //        jobTitle = string.Empty;
        //                                //    if (!jobTitle.StartsWith("XX - "))
        //                                //    {
        //                                //        jobTitle = string.Format("XX - {0}", jobTitle);
        //                                //        userItem["JobTitle"] = jobTitle;
        //                                //        userItem.Update();
        //                                //    }
        //                                //}
        //                                //else
        //                                //{
        //                                //    object updateFlag =
        //                                //       userItem.Properties["AdUpdateFlag"];
        //                                //    if (userInfo.LastModified > dtUserItemUpdated
        //                                //        || updateFlag == null)
        //                                //    {
        //                                //        userItem.Properties["AdUpdateFlag"] = 1;
        //                                //        if (userInfo.Email != null)
        //                                //        {
        //                                //            userItem["EMail"] = userInfo.Email;
        //                                //            user.Email = userInfo.Email;
        //                                //        }
        //                                //        if (userInfo.Department != null)
        //                                //            userItem["Department"] = userInfo.Department;
        //                                //        if (userInfo.JobTitle != null)
        //                                //            userItem["JobTitle"] = userInfo.JobTitle;
        //                                //        else
        //                                //        {
        //                                //            string val = (string)userItem["JobTitle"];
        //                                //            if (val != null)
        //                                //            {
        //                                //                if (val.StartsWith("XX - "))
        //                                //                    userItem["JobTitle"] =
        //                                //                       val.Substring(5, val.Length - 5);
        //                                //            }
        //                                //        }
        //                                //        if (userInfo.FirstName != null)
        //                                //            userItem["FirstName"] = userInfo.FirstName;
        //                                //        if (userInfo.LastName != null)
        //                                //            userItem["LastName"] = userInfo.LastName;
        //                                //        if (userInfo.WorkPhone != null)
        //                                //            userItem["WorkPhone"] = userInfo.WorkPhone;
        //                                //        if (userInfo.Office != null)
        //                                //            userItem["Office"] = userInfo.Office;
        //                                //        if (userInfo.WorkZip != null)
        //                                //            userItem.Properties["WorkZip"] =
        //                                //                                userInfo.WorkZip;
        //                                //        if (userInfo.WorkCity != null)
        //                                //            userItem.Properties["WorkCity"] =
        //                                //                                userInfo.WorkCity;
        //                                //        if (userInfo.WorkState != null)
        //                                //            userItem.Properties["WorkState"] =
        //                                //                                userInfo.WorkState;
        //                                //        if (userInfo.WorkCountry != null)
        //                                //            userItem.Properties["WorkCountry"] =
        //                                //                                userInfo.WorkCountry;
        //                                //        userItem.Update();
        //                                //        user.Update();
        //                                //    }
        //                                //}
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {

        //                        }
        //                    }
        //                    web.Dispose();
        //                }
        //                site.Dispose();
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //private bool IsSystem(SPUser user)
        //{
        //    if (user.ID.Equals(1073741823))
        //        return true;
        //    if (user.LoginName == null)
        //        return true;
        //    if (user.LoginName.ToLower().StartsWith("nt authority"))
        //        return true;
        //    if (user.LoginName.ToLower().StartsWith("system"))
        //        return true;
        //    return false;
        //}
        //private DirectoryEntry GetUserInfo(SPUser user, DirectoryEntry domain)
        //{
        //    string id = user.Sid;
        //    bool localMachine = domain.Path.StartsWith("WinNT");
        //    string userFlagProperty = "userAccountControl";
        //    if (localMachine)
        //        userFlagProperty = "UserFlags";
        //    DirectoryEntry deUser = FindUser(id, domain);
        //    if (deUser != null)
        //    {

        //        if (localMachine)
        //        {
        //            //For testing purposes... Production Environment should be using
        //            // Active Directory
        //            //adUserInfo.LastModified = DateTime.Now;
        //            //string value = GetValue("FullName", deUser);
        //            //if (!string.IsNullOrEmpty(value))
        //            //{
        //            //    string[] vals = value.Split(new char[1] { ' ' }
        //            //        , StringSplitOptions.RemoveEmptyEntries);
        //            //    if (vals.Length > 0)
        //            //        adUserInfo.FirstName = vals[0];
        //            //    if (vals.Length > 1)
        //            //        adUserInfo.LastName = vals[vals.Length - 1];
        //            //}
        //            //adUserInfo.WorkCity = "St Louis";
        //            //adUserInfo.WorkState = "MO";
        //            //adUserInfo.WorkZip = "63141";

        //        }
        //        else
        //        {
        //            //DateTime dtModified = DateTime.Now;
        //            //if (DateTime.TryParse(GetValue("whenChanged", deUser),
        //            //                      out dtModified))
        //            //    adUserInfo.LastModified = dtModified;
        //            //else
        //            //    adUserInfo.LastModified = DateTime.Now;
        //            //adUserInfo.LastName = GetValue("sn", deUser);
        //            //adUserInfo.FirstName = GetValue("givenName", deUser);
        //            //adUserInfo.Name = GetValue("sAMAccountName", deUser);
        //            //adUserInfo.Office = GetValue("physicalDeliveryOfficeName", deUser);
        //            //adUserInfo.WorkPhone = GetValue("telephoneNumber", deUser);
        //            //adUserInfo.Department = GetValue("department", deUser);
        //            //adUserInfo.Email = GetValue("mail", deUser);
        //            //adUserInfo.JobTitle = GetValue("title", deUser);
        //            //adUserInfo.WorkCity = GetValue("l", deUser);
        //            //adUserInfo.WorkState = GetValue("st", deUser);
        //            //adUserInfo.WorkCountry = GetValue("c", deUser);
        //            //adUserInfo.WorkZip = GetValue("postalCode", deUser);
        //        }
        //        string userAC = GetValue(userFlagProperty, deUser);
        //        int userValue = 0;
        //        //if (int.TryParse(userAC, out userValue))
        //        //{
        //        //    try
        //        //    {
        //        //        AdUserAccountControl userAccountControl =
        //        //                            (AdUserAccountControl)userValue;
        //        //        adUserInfo.IsActive =
        //        //            //Make sure it's not disabled
        //        //            ((userAccountControl & AdUserAccountControl.ACCOUNTDISABLE)
        //        //                        != AdUserAccountControl.ACCOUNTDISABLE)
        //        //            //Make sure it's a normal account
        //        //            && ((userAccountControl &
        //        //                  AdUserAccountControl.NORMAL_ACCOUNT) ==
        //        //                  AdUserAccountControl.NORMAL_ACCOUNT);
        //        //    }
        //        //    catch (Exception ex)
        //        //    {

        //        //    }
        //        //}
        //        return deUser;
        //    }
        //    else
        //        return null;
        //}
        //private string GetValue(string propertyName, DirectoryEntry deUser)
        //{
        //    if (deUser.Properties.Contains(propertyName))
        //    {
        //        PropertyValueCollection pvc = deUser.Properties[propertyName];
        //        if (pvc.Count > 0)
        //        {
        //            object objValue = pvc[0];
        //            if (objValue != null)
        //                return objValue.ToString();
        //        }
        //    }
        //    return null;
        //}
        //private DirectoryEntry FindUser(string id, DirectoryEntry domain)
        //{
        //    if (!domain.Path.StartsWith("WinNT"))
        //    {
        //        DirectorySearcher search = new DirectorySearcher(domain);
        //        search.Filter =
        //          string.Format("(&(objectClass=person)(objectSid={0}))", id);
        //        SearchResult result = search.FindOne();
        //        if (result != null)
        //            return result.GetDirectoryEntry();
        //    }
        //    else
        //    {
        //        foreach (DirectoryEntry de in domain.Children)
        //        {
        //            SecurityIdentifier si = new SecurityIdentifier(
        //                (byte[])de.Properties["objectSid"][0], 0);
        //            if (string.Compare(si.Value, id, true) == 0)
        //                return de;
        //        }
        //    }
        //    return null;
        //}
        //private DirectoryEntry GetDomainEntry()
        //{
        //    try
        //    {
        //        return Domain.GetComputerDomain().GetDirectoryEntry();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //private bool IsPerson(SPListItem userItem)
        //{
        //    string contentType = userItem.ContentType.Name;
        //    if (!contentType.Equals("Person"))
        //        return false;
        //    return true;
        //}
    }
}
