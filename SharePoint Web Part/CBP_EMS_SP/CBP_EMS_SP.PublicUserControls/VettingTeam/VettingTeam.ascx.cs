using CBP_EMS_SP.Common;
using CBP_EMS_SP.Data.Models;

using System;
using System.Web.Configuration;
using System.ComponentModel;
using System.Linq;

using System.Web.Security;

using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Administration.Claims;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;

namespace CBP_EMS_SP.PublicUserControls.VettingTeam
{
    [ToolboxItemAttribute(false)]
    public partial class VettingTeam : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public VettingTeam()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btn_addVettingMember.Visible = true;
                MemberPanel.Visible = false;
                txtvettingteamEmail.ReadOnly = false;
                txtvettingteamfullname.Text = "";
                txtvettingteamEmail.Text = "";
                lblInvitationvettingteam.Text = "";
                ddlSalutation.SelectedIndex = 0;
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtCity.Text = "";
                txtCountry.Text = "";
                txtFirstName.Text = "";
                txtTitle.Text = "";
                FillVettingTeam();
            }
        }

        protected void AddVettingMember_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            hdnVettingMemberID.Value = "0";
            btn_addVettingMember.Visible = false;
            MemberPanel.Visible = true;
            txtvettingteamEmail.ReadOnly = false;
            txtvettingteamfullname.Text = "";
            txtvettingteamEmail.Text = "";
            ddlSalutation.SelectedIndex = 0;
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtFirstName.Text = "";
            txtTitle.Text = "";
            FillVettingTeam();
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnVettingMemberID.Value == "0")
                {
                    CreateVettingMember();
                }
                else
                {
                    EditVettingMember(hdnVettingMemberID.Value, txtvettingteamfullname.Text, ddlSalutation.SelectedItem.Text, txtAddress1.Text, txtAddress2.Text, txtAddress3.Text, txtCity.Text, txtCountry.Text, txtFirstName.Text, txtTitle.Text);
                }

                if (string.IsNullOrEmpty(lblInvitationvettingteam.Text))
                {
                    btn_addVettingMember.Visible = true;
                    txtvettingteamEmail.ReadOnly = false;
                    MemberPanel.Visible = false;
                    txtvettingteamfullname.Text = "";
                    txtvettingteamEmail.Text = "";
                    ddlSalutation.SelectedIndex = 0;
                    txtAddress1.Text = "";
                    txtAddress2.Text = "";
                    txtAddress3.Text = "";
                    txtCity.Text = "";
                    txtCountry.Text = "";
                    txtFirstName.Text = "";
                    txtTitle.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                //lblInvitationvettingteam.CssClass = "text-danger";
            }


            FillVettingTeam();
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            btn_addVettingMember.Visible = true;
            MemberPanel.Visible = false;
            txtvettingteamEmail.ReadOnly = false;
            txtvettingteamfullname.Text = "";
            txtvettingteamEmail.Text = "";
            lblInvitationvettingteam.Text = "";

            FillVettingTeam();
        }

        protected void ImageButton1_Click(object send, EventArgs e)
        {
            panelResetPassword.Visible = false;
            Context.Response.Redirect("~/SitePages/VettingTeam.aspx", false);
        }

        protected void rptrvettingteam_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    SPWeb site = null;
                    SPGroup oGroup = null;
                    try
                    {
                        string spVettingGroup = (String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"];
                        using (var dbContext = new CyberportEMS_EDM())
                        {
                            //lblError.Text = "THe update started - CommandArgument:" + e.CommandArgument.ToString();
                            // Disable the vetting member
                            Guid user_id = new Guid((e.CommandArgument.ToString()));
                            TB_VETTING_MEMBER_INFO memberInfo = dbContext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == user_id);
                            memberInfo.Disabled = true;
                            // remove the user from sharepoint


                            SPClaimProviderManager mgr = SPClaimProviderManager.Local;

                            SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                            site = siteCollection.OpenWeb();
                            site.AllowUnsafeUpdates = true;
                            oGroup = site.SiteGroups[spVettingGroup];
                            if (mgr != null)
                            {
                                SPUser oUser = site.EnsureUser(memberInfo.Email);
                                if (oUser != null)
                                {
                                    oGroup.RemoveUser(oUser);
                                }

                            }
                            dbContext.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblInvitationvettingteam.Text = ex.Message;
                        lblInvitationvettingteam.CssClass = "text-danger";
                    }
                    finally
                    {
                        if (site != null)
                        {
                            site.Dispose();
                        }
                    }
                });

                btn_addVettingMember.Visible = true;
                MemberPanel.Visible = false;
                txtvettingteamEmail.ReadOnly = false;
                txtvettingteamfullname.Text = "";
                txtvettingteamEmail.Text = "";
                lblInvitationvettingteam.Text = "";

                FillVettingTeam();

            }
            else if (e.CommandName == "edit")
            {
                hdnVettingMemberID.Value = e.CommandArgument.ToString();

                // Edit the existing record
                using (var dbContext = new CyberportEMS_EDM())
                {
                    Guid user_id = new Guid(e.CommandArgument.ToString());
                    TB_VETTING_MEMBER_INFO memberInfo = dbContext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == user_id);
                    if (memberInfo != null)
                    {
                        txtvettingteamfullname.Text = memberInfo.Full_Name;
                        txtvettingteamEmail.Text = memberInfo.Email;
                        txtvettingteamEmail.ReadOnly = true;
                        ddlSalutation.SelectedItem.Text = memberInfo.Salutation;
                        txtAddress1.Text = memberInfo.Address1;
                        txtAddress2.Text = memberInfo.Address2;
                        txtAddress3.Text = memberInfo.Address3;
                        txtCity.Text = memberInfo.City;
                        txtCountry.Text = memberInfo.Country;
                        txtFirstName.Text = memberInfo.First_Name;
                        txtTitle.Text = memberInfo.Title;
                    }
                }
                MemberPanel.Visible = true;
                btn_addVettingMember.Visible = false;
                lblInvitationvettingteam.Text = "";

                FillVettingTeam();
            }
            else if (e.CommandName == "reset")
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    Guid user_id = new Guid(e.CommandArgument.ToString());
                    TB_VETTING_MEMBER_INFO memberInfo = dbContext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == user_id);
                    if (memberInfo != null)
                    {
                        ResetPassword(memberInfo.Email);
                        lblappsucess.Text = Localize("Forgot_pass_successfull_msg") + ": " + memberInfo.Email;
                        panelResetPassword.Visible = true;
                    }
                }
            }
        }

        protected void FillVettingTeam()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    //rptrvettingteam.DataSource = dbContext.TB_VETTING_MEMBER.ToList();
                    //rptrvettingteam.DataBind();
                    //lblInvitationvettingteam.Text = "";
                    //txtvettingteamEmail.Text = "";
                    string SqlMembershipProvider = dbContext.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;
                    string spVettingGroup = (String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"];
                    var usernames = Roles.GetUsersInRole(spVettingGroup);
                    List<VettingMemberBO> BOs = new List<VettingMemberBO>();
                    foreach (var username in usernames)
                    {
                        var user = Membership.Providers[SqlMembershipProvider].GetUser(username, false);
                        // Get the fullname in Vetting Member Info table
                        TB_VETTING_MEMBER_INFO memberInfo = dbContext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == (Guid)user.ProviderUserKey);
                        if (memberInfo != null && memberInfo.Disabled == false)
                        {
                            BOs.Add(new VettingMemberBO(memberInfo.Full_Name, user.UserName, memberInfo.Vetting_Member_ID, memberInfo.Salutation, memberInfo.Address1, memberInfo.Address2, memberInfo.Address3, memberInfo.City, memberInfo.Country, memberInfo.First_Name, memberInfo.Title));
                        }
                    }

                    rptrvettingteam.DataSource = BOs;
                    rptrvettingteam.DataBind();
                    //lblInvitationvettingteam.Text = "";
                    //txtvettingteamEmail.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private class VettingMemberBO
        {
            public string fullname { get; set; }
            public string email { get; set; }
            public string Vetting_Member_ID { get; set; }

            public string Salutation { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Address3 { get; set; }
            public string City { get; set; }

            public string Country { get; set; }
            public string FirstName { get; set; }
            public string Title { get; set; }

            public VettingMemberBO(string fullname, string email, Guid Vetting_Member_ID, string salutation, string address1, string address2, string address3, string city, string country, string firstName, string title)
            {
                this.fullname = fullname;
                this.email = email;
                this.Vetting_Member_ID = Vetting_Member_ID.ToString();
                this.Salutation = salutation;
                this.Address1 = address1;
                this.Address2 = address2;
                this.Address3 = address3;
                this.City = city;
                this.Country = country;
                this.FirstName = firstName;
                this.Title = title;

            }
        }

        protected void CreateVettingMember()
        {
            bool isSuccess = true;
            SPWeb site = null;
            SPGroup oGroup = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                try
                {
                    string spVettingGroup = (String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"]; // Vetting Group Name
                    string spUserRoleGroup = (String)WebConfigurationManager.AppSettings["SPExternalUserGroupName"]; // Applicant Group Name
                    SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                    SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    site = siteCollection.OpenWeb();
                    site.AllowUnsafeUpdates = true;
                    string errMsg = "";



                    if (string.IsNullOrEmpty(ddlSalutation.SelectedValue.ToString().Trim()))
                    {
                        errMsg += "Please select a salutation. ";

                    }
                    if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                    {
                        errMsg += "<br /> First Name must be entered. ";

                    }
                    if (string.IsNullOrEmpty(txtvettingteamfullname.Text.Trim()))
                    {
                        errMsg += "<br /> Last Name must be entered. ";

                    }
                    if (!Common.CBPRegularExpression.RegExValidate(Common.CBPRegularExpression.Email, txtvettingteamEmail.Text.Trim()))
                    {
                        errMsg += "<br /> Invalid email format. ";

                    }

                    if (txtvettingteamEmail.Text.ToLower().Trim().EndsWith("@cyberport.hk"))
                    {
                        errMsg += "<br /> Email address cannot end with @cyberport.hk ";

                    }
                    else
                    {


                        String SqlMembershipProvider = string.Empty;
                        SPFunctions objfunction = new SPFunctions();
                        using (var db = new CyberportEMS_EDM())
                        {
                            SqlMembershipProvider = db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;

                            MembershipUser user = Membership.Providers[SqlMembershipProvider].GetUser(txtvettingteamEmail.Text.Trim(), false);


                            if (user == null)
                            {
                                // the email has not been registered
                                // Create a new user and Put it into Vetting Group
                                bool isCreated = CreateUser(txtvettingteamEmail.Text.Trim(), txtvettingteamfullname.Text.Trim(), ddlSalutation.SelectedItem.Text.Trim(), txtAddress1.Text.Trim(), txtAddress2.Text.Trim(), txtAddress3.Text.Trim(), txtCity.Text.Trim(), txtCountry.Text.Trim(), txtFirstName.Text, txtTitle.Text);
                                // Send reset password email
                                if (isCreated)
                                {
                                    ResetPassword(txtvettingteamEmail.Text.Trim());
                                }

                            }
                            else if (txtvettingteamEmail.Text.ToLower().Trim() == objfunction.GetCurrentUser().ToLower().Trim())
                            {
                                // the email is the current user's email
                                errMsg += "<br /> You cannot invite the same email account as the registered email account.";

                                isSuccess = false;
                            }
                            // the email has been registered
                            else if (Roles.IsUserInRole(user.UserName, spUserRoleGroup))
                            {
                                // the user is applicant, show error message
                                errMsg += "<br /> This email has been registered as an applicant.";

                                isSuccess = false;
                            }
                            else if (Roles.IsUserInRole(user.UserName, spVettingGroup))
                            {
                                // the user is already in Vetting group
                                TB_VETTING_MEMBER_INFO memberInfo = db.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == (Guid)user.ProviderUserKey);
                                if (memberInfo != null && memberInfo.Disabled == true) // ????
                                {
                                    // The user is disabled
                                    // re-activate the member (Setting Dsiabeld = 0 in TB_Vetting_Member_Info)
                                    memberInfo.Disabled = false;
                                    memberInfo.Full_Name = txtvettingteamfullname.Text.Trim();
                                    // Add the user into Vetting group in sharepoint
                                    if (mgr != null)
                                    {
                                        oGroup = site.SiteGroups[spVettingGroup];

                                        SPUser oUser = site.EnsureUser(user.UserName);
                                        oUser.Email = user.UserName;
                                        oUser.Name = user.UserName;
                                        oGroup.AddUser(oUser);
                                    }
                                    db.SaveChanges();
                                    // send invitation email without asking reset password
                                    ResetPassword(user.UserName);
                                }
                                else
                                {
                                    // the user is already active
                                    errMsg += "\n Vetting Member is already in list";

                                    isSuccess = false;
                                }
                            }
                            else
                            {
                                // the user is not applicant but not yet in vetting group
                                // Add the user into Vetting gorup in db
                                Roles.AddUserToRole(user.UserName, spVettingGroup);
                                // Add the user into Vetting group in sharepoint
                                if (mgr != null)
                                {
                                    oGroup = site.SiteGroups[spVettingGroup];

                                    SPUser oUser = site.EnsureUser(user.UserName);
                                    oUser.Email = user.UserName;
                                    oUser.Name = user.UserName;
                                    oGroup.AddUser(oUser);
                                }
                                // Send invitation email without asking reset password
                                ResetPassword(user.UserName);
                            }
                        }


                    }
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        lblInvitationvettingteam.Text = errMsg;
                        lblInvitationvettingteam.CssClass = "text-danger";
                    }
                    else
                    {
                        lblInvitationvettingteam.Text = "";
                    }



                }
                catch (Exception ex)
                {
                    lblInvitationvettingteam.Text = ex.Message;
                    lblInvitationvettingteam.CssClass = "text-danger";
                }
                finally
                {
                    if (site != null)
                    {
                        site.Dispose();
                    }
                }
            });

        }
        private bool CreateUser(string UserEmail, string FullName, string Salutation, string Address1, string Address2, string Address3, string City, string Country, string firstName, string title)
        {
            bool IsValid = false;
            SPWeb site = null;
            SPGroup oGroup = null;
            try
            {
                MembershipCreateStatus UserState;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    string spVettingGroup = (String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"]; // Vetting Group Name
                    SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                    site = siteCollection.OpenWeb();
                    site.AllowUnsafeUpdates = true;
                    oGroup = site.SiteGroups[spVettingGroup];


                    // Attempt to create the user
                    var newUser = Membership.CreateUser(UserEmail, "dummypassowrd@123", UserEmail
                                            , "SecurityQuestion"
                        , "SecurityAnswer"
                        , true
                        , out UserState);

                    // Validate user creation
                    string Message = "";
                    #region User account creation status handling
                    switch (UserState)
                    {
                        case MembershipCreateStatus.Success:
                            IsValid = true;
                            break;

                        case MembershipCreateStatus.DuplicateEmail:
                            Message = "Email already exists.";
                            break;

                        case MembershipCreateStatus.DuplicateUserName:
                            Message = "Email already exists.";
                            break;

                        case MembershipCreateStatus.InvalidAnswer:
                            Message = "Invalid Security answer.";
                            break;

                        case MembershipCreateStatus.InvalidEmail:
                            Message = "Invalid Email address.";
                            break;

                        case MembershipCreateStatus.InvalidPassword:
                            Message = "Password is not secure.";
                            break;

                        case MembershipCreateStatus.InvalidQuestion:
                            Message = "Invalid security question";
                            break;

                        default:
                            Message = "internal error";
                            break;
                    }
                    #endregion



                    if (!IsValid)
                    {
                        lblInvitationvettingteam.Text = Message;
                        lblInvitationvettingteam.CssClass = "text-danger";
                    }

                    if (IsValid)
                    {
                        Roles.AddUserToRole(UserEmail, spVettingGroup);

                        var userinfo = SPUtility.ResolvePrincipal(site, UserEmail, SPPrincipalType.User, SPPrincipalSource.All, null, true);



                        SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                        if (mgr != null)
                        {
                            //SPUser oUser = site.EnsureUser(UserEmail);
                            SPUser oUser = site.EnsureUser(userinfo.LoginName);
                            oUser.Email = UserEmail;
                            oUser.Name = UserEmail;
                            oGroup.AddUser(oUser);

                            using (var dbcotext = new CyberportEMS_EDM())
                            {
                                // Save user's fullname 
                                TB_VETTING_MEMBER_INFO memberInfo = new TB_VETTING_MEMBER_INFO();
                                memberInfo.Vetting_Member_ID = (Guid)newUser.ProviderUserKey;
                                memberInfo.Email = UserEmail;
                                memberInfo.Full_Name = FullName;
                                memberInfo.Salutation = Salutation;
                                memberInfo.Address1 = Address1;
                                memberInfo.Address2 = Address2;
                                memberInfo.Address3 = Address3;
                                memberInfo.City = City;
                                memberInfo.Country = Country;
                                memberInfo.First_Name = firstName;
                                memberInfo.Title = title;
                                memberInfo.Registered = true;
                                memberInfo.Disabled = false;
                                dbcotext.TB_VETTING_MEMBER_INFO.Add(memberInfo);

                                dbcotext.SaveChanges();
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                IsValid = false;
                Membership.DeleteUser(UserEmail);
                SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                if (mgr != null)
                {
                    SPUser oUser = site.EnsureUser(UserEmail);
                    oGroup.RemoveUser(oUser);
                }
                lblInvitationvettingteam.Text = ex.Message;
                lblInvitationvettingteam.CssClass = "text-danger";
            }
            return IsValid;
        }

        protected void EditVettingMember(string user_id_txt, string newName, string Salutation, string Address1, string Address2, string Address3, string City, string Country, string firstName, string title)
        {
            string errMsg = "";
            if (string.IsNullOrEmpty(ddlSalutation.SelectedValue.ToString().Trim()))
            {
                errMsg += "Please select a salutation. ";

            }
            if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                errMsg += "<br /> First Name must be entered. ";

            }
            if (string.IsNullOrEmpty(txtvettingteamfullname.Text.Trim()))
            {
                errMsg += "<br /> Last Name must be entered. ";

            }
            else
            {

                Guid user_id = new Guid(user_id_txt);
                using (var dbcotext = new CyberportEMS_EDM())
                {
                    // Save user's fullname 
                    TB_VETTING_MEMBER_INFO memberInfo = dbcotext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == user_id);
                    if (memberInfo != null)
                    {
                        memberInfo.Full_Name = newName;
                        memberInfo.Salutation = Salutation;
                        memberInfo.Address1 = Address1;
                        memberInfo.Address2 = Address2;
                        memberInfo.Address3 = Address3;
                        memberInfo.City = City;
                        memberInfo.Country = Country;
                        memberInfo.First_Name = firstName;
                        memberInfo.Title = title;
                    }

                    dbcotext.SaveChanges();
                }


            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                lblInvitationvettingteam.Text = errMsg;
                lblInvitationvettingteam.CssClass = "text-danger";
            }
            else
            {
                lblInvitationvettingteam.Text = "";
            }
        }

        protected int ResetPassword(string userEmail)
        {
            using (var db = new CyberportEMS_EDM())
            {
                TB_RESET_PASSWORD objRequestPassword = db.TB_RESET_PASSWORD.FirstOrDefault(x => x.Email.ToLower() == userEmail.ToLower());
                if (objRequestPassword == null)
                {
                    objRequestPassword = new TB_RESET_PASSWORD()
                    {
                        Created_Date = DateTime.Now,
                        Email = userEmail.Trim(),
                        Email_Type = "user",
                    };
                    db.TB_RESET_PASSWORD.Add(objRequestPassword);
                }
                else
                {
                    objRequestPassword.Created_Date = DateTime.Now;
                }
                db.SaveChanges();
                objRequestPassword = db.TB_RESET_PASSWORD.FirstOrDefault(x => x.Email.ToLower() == userEmail.Trim().ToLower());
                string strEmailContent = CBPEmail.GetEmailTemplate("Vetting_Team_Invitaion");
                strEmailContent = strEmailContent.Replace("@@ForgotPasswordLink", objRequestPassword.Reset_Password_ID.ToString());
                strEmailContent = strEmailContent.Replace("@@FullName", txtvettingteamfullname.Text);
                return CBPEmail.SendMail(userEmail.Trim(), Localize("Vetting_Member_Invitation"), strEmailContent);
            }
        }

        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }

    }
}
