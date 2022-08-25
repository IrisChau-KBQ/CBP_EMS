using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Linq;
using System.Web.Security;
using Microsoft.SharePoint.IdentityModel;
using CBP_EMS_SP.Data.Models;
using CBP_EMS_SP.Common;

namespace CBP_EMS_SP.Page
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblheading.InnerText = Localize("ResetPassword");
            txtsubheading.InnerText = Localize("ResetPass_subheading");
            txtpasswordvalidation.Text = Localize("Error_Registration_Pass_validation");
            txtpasserror.Text = Localize("Error_Registration_Password");
            txtpassconfirm.Text = Localize("Error_Registration_Passwordconfirmation");
            errormismatch.Text = Localize("ResetPass_Mismatch");
            passvalidation.InnerText = Localize("Registration_Passwordmessage");
            txtpassexpired.InnerText = Localize("Reset_Pass_expred");
            btn_HideSubmitPopup.Text= Localize("lblok");
            if (!IsPostBack)
            {
                try
                {


                    if (Request.QueryString["token"].Length > 0)
                    {
                        String SqlMembershipProvider = string.Empty;
                        using (var db = new CyberportEMS_EDM())
                        {
                            SqlMembershipProvider = db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;
                            Guid objGuid = Guid.Parse(Request.QueryString["token"]);
                            TB_RESET_PASSWORD objReset = db.TB_RESET_PASSWORD.FirstOrDefault(x => x.Reset_Password_ID == objGuid);
                            int Reset_Password_Link_Expire_In_Hours = 1;
                            Int32.TryParse(db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "Reset_Password_Link_Expire_In_Hours").Value, out Reset_Password_Link_Expire_In_Hours);

                            if (objReset != null)
                            {
                                if (DateTime.Now <= objReset.Created_Date.AddHours(Reset_Password_Link_Expire_In_Hours))
                                {
                                    MembershipUser Username = Membership.Providers[SqlMembershipProvider].GetUser(objReset.Email, false);
                                    txt_UserEmail.Text = Username.Email;
                                }
                                else
                                {
                                    userPopup.Visible = true;
                                    lnkReset.Enabled = false;
                                }
                            }
                            else
                            {
                                userPopup.Visible = true;  
                                lnkReset.Enabled = false;

                                txtpassexpired.InnerText = "This link has been used, the password cannot be reset using this URL.";
                            }


                        }
                    }
                    else
                    {
                        Response.Redirect("~/SitePages/Home.aspx");
                    }
                }
                catch (Exception)
                {
                    Response.Redirect("~/SitePages/Home.aspx");
                }
            }
        }

        protected void lnkReset_Click(object sender, EventArgs e)
        {
            try
            {
                UserCustomerrorReg.Style.Add("color", "red");

                String SqlMembershipProvider = string.Empty;
                using (var db = new CBP_EMS_SP.Data.Models.CyberportEMS_EDM())
                {

                    SqlMembershipProvider = db.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "SqlMembershipProvider").Value;

                    Guid objGuid = Guid.Parse(Request.QueryString["token"]);
                    TB_RESET_PASSWORD objReset = db.TB_RESET_PASSWORD.FirstOrDefault(x => x.Reset_Password_ID == objGuid);

                    MembershipUser Username = Membership.Providers[SqlMembershipProvider].GetUser(objReset.Email, false);
                    Username.UnlockUser();
                    Username.ChangePassword(Username.ResetPassword("SecurityAnswer"), txtRegPassword.Text.Trim());

                    db.TB_RESET_PASSWORD.Remove(objReset);
                    db.SaveChanges();

                    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, Username.Email, txtRegPassword.Text);
                    if (status)
                    {
                        UserCustomerrorReg.InnerText = Localize("ResetPass_Successfully");
                        UserCustomerrorReg.Style.Add("color", "green");
                        Response.Redirect("~/SitePages/Home.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                UserCustomerrorReg.InnerText = ex.Message;
            }
        }

        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SitePages/Home.aspx");
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }

    }
}
