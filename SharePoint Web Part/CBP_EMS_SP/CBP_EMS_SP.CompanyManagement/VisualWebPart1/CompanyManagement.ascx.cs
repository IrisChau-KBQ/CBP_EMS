using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data.Models;
using System.Linq;
using System.Collections.Generic;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Common;
using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace CBP_EMS_SP.CompanyManagement.VisualWebPart1
{
    [ToolboxItemAttribute(false)]
    public partial class CompanyManagement : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CompanyManagement()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Context.Request.QueryString["app"]))
            {
                Context.Response.Redirect("/SitePages/My_Company_Profile.aspx");
            }
            if (!Page.IsPostBack)
            {

                pnl_BasicProfile.Visible = true;
                FillIntake();
                getCoreMember();
                getContacts();
                getMembersList();
                getReimbursement();
                getfund();
                getMergeAcquisition();
                getAward();
                getIP();
                getAdministrator();
                getJoinedAccelerator();
                getSpecialRequest();

            }
            checkuser();



            string BusinessAreaSelected = ddlClusterCCMF.SelectedValue;
            List<ListItem> Cluster = new List<ListItem>();
            Cluster.Add(new ListItem() { Value = "", Text = "--------------" });
            Cluster.Add(new ListItem() { Value = "App Design/ Web Production", Text = "App Design/ Web Production" });
            Cluster.Add(new ListItem() { Value = "E-Commerce", Text = "E-Commerce" });
            Cluster.Add(new ListItem() { Value = "Edutech", Text = "Edutech" });
            Cluster.Add(new ListItem() { Value = "AI / Big Data", Text = "AI / Big Data" });
            Cluster.Add(new ListItem() { Value = "Fintech", Text = "Fintech" });
            Cluster.Add(new ListItem() { Value = "Gaming", Text = "Gaming" });
            Cluster.Add(new ListItem() { Value = "Healthcare", Text = "Healthcare" });
            Cluster.Add(new ListItem() { Value = "Wearable", Text = "Wearable" });
            Cluster.Add(new ListItem() { Value = "Social-Media", Text = "Social-Media" });
            Cluster.Add(new ListItem() { Value = "Others", Text = "Others" });
            ddlClusterCCMF.DataSource = Cluster;
            ddlClusterCCMF.DataTextField = "Text";
            ddlClusterCCMF.DataValueField = "Value";
            ddlClusterCCMF.DataBind();
            if (!string.IsNullOrEmpty(BusinessAreaSelected))
            {
                ddlClusterCCMF.SelectedValue = BusinessAreaSelected;
            }

            string CompanyTypeSelected = ddlclusterCPIP.SelectedValue;

            ddlclusterCPIP.DataSource = Cluster;
            ddlclusterCPIP.DataTextField = "Text";
            ddlclusterCPIP.DataValueField = "Value";
            ddlclusterCPIP.DataBind();
            if (!string.IsNullOrEmpty(CompanyTypeSelected))
            {
                ddlclusterCPIP.SelectedValue = CompanyTypeSelected;
            }
        }
        protected void checkuser()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                TB_COMPANY_PROFILE_BASIC objApp = GetExistingApplication(dbContext);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_ADMIN objAdmins = dbContext.TB_COMPANY_ADMIN.FirstOrDefault(x => x.Email == strCurrentUser && x.Company_Profile_ID == objApp.Company_Profile_ID);

                //Allow for Application and Company Admins
                if (objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup) && (objApp.Created_By.ToLower() == strCurrentUser || objAdmins != null))
                {

                    disableProfileControl();

                    //  btn_Add_CoreMember.Visible = false;
                    //  for (int i = 0; i < gdv_CoreMember.Rows.Count; i++)
                    //  {
                    //      Button btn_Member_Edit = (Button)gdv_CoreMember.Rows[i].Cells[0].FindControl("btn_Member_Edit");
                    //      btn_Member_Edit.Enabled = false;
                    //  }

                    //   btn_Add_Contacts.Visible = false;
                    //   for (int i = 0; i < gdv_Contact.Rows.Count; i++)
                    //   {
                    //       Button btn_Contact_Edit = (Button)gdv_Contact.Rows[i].Cells[0].FindControl("btn_Contact_Edit");
                    //       btn_Contact_Edit.Enabled = false;
                    //   }

                    btn_fund_AddNew.Visible = false;
                    for (int i = 0; i < gdv_Fund.Rows.Count; i++)
                    {
                        Button btnFundEdit = (Button)gdv_Fund.Rows[i].Cells[0].FindControl("btnFundEdit");
                        btnFundEdit.Enabled = false;
                    }

                    btn_MergeAcquNew.Visible = false;
                    for (int i = 0; i < gdv_MergeAcquition.Rows.Count; i++)
                    {
                        Button btn_Merge_Edit = (Button)gdv_MergeAcquition.Rows[i].Cells[0].FindControl("btn_Merge_Edit");
                        btn_Merge_Edit.Enabled = false;
                    }

                    btn_Awards_New.Visible = false;
                    for (int i = 0; i < Gdv_awards.Rows.Count; i++)
                    {
                        Button btn_Award_edit = (Button)Gdv_awards.Rows[i].Cells[0].FindControl("btn_Award_edit");
                        btn_Award_edit.Enabled = false;
                    }

                    btn_Accelerator.Visible = false;
                    for (int i = 0; i < gdv_JoinedAccelerator.Rows.Count; i++)
                    {
                        Button btn_Accelerator_Edit = (Button)gdv_JoinedAccelerator.Rows[i].Cells[0].FindControl("btn_Accelerator_Edit");
                        btn_Accelerator_Edit.Enabled = false;
                    }
                    btn_IPNew.Visible = false;
                    for (int i = 0; i < gdv_IP.Rows.Count; i++)
                    {
                        Button btn_IP_Edit = (Button)gdv_IP.Rows[i].Cells[0].FindControl("btn_IP_Edit");
                        btn_IP_Edit.Enabled = false;
                    }


                }
                else if (!objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                {
                    btn_ManageAdmin.Visible = false;
                    for (int i = 0; i < gdv_ManageAdministrator.Rows.Count; i++)
                    {
                        Button btn_AdminEdit = (Button)gdv_ManageAdministrator.Rows[i].Cells[0].FindControl("btn_AdminEdit");
                        btn_AdminEdit.Enabled = false;
                    }

                }
                else Context.Response.Redirect("/SitePages/My_Company_Profile.aspx");
            }
        }
        protected void disableProfileControl()
        {
            txtNameEng.Enabled = false;
            txtNameChi.Enabled = false;
            txtCompanyName.Enabled = false;
            txtBrandName.Enabled = false;
            ddlClusterCCMF.Enabled = false;
            ddlclusterCPIP.Enabled = false;
            txtcompanyTag.Enabled = false;
            txtCCMFAbsEng.Enabled = false;
            txtCCMFAbsChi.Enabled = false;
            txtCPIPAbsEng.Enabled = false;
            txtCPIPAbsChi.Enabled = false;
            txtCASPAbstract.Enabled = false;
            txtcompanyOwnership.Enabled = false;
            txtRemarks.Enabled = false;
            fu_companyAttachement.Enabled = false;
            btn_Save.Visible = false;
            ImageButton14.Enabled = false;
        }
        protected void FillIntake()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                TB_COMPANY_PROFILE_BASIC objApp = GetExistingApplication(dbContext);
                if (objApp != null)
                {
                    txtNameEng.Text = objApp.Name_Eng;

                    txtNameChi.Text = objApp.Name_Chi;

                    txtCompanyName.Text = objApp.Company_Name;

                    txtBrandName.Text = objApp.Brand_Name;

                    txtCCMFAbsEng.Text = objApp.CCMF_Abstract;

                    ddlClusterCCMF.SelectedValue = objApp.CCMF_Custer;
                    ddlclusterCPIP.SelectedValue = objApp.CPIP_Custer;
                    txtCCMFAbsChi.Text = objApp.CCMF_Abstract_Chi;

                    txtCPIPAbsEng.Text = objApp.CPIP_Abstract;

                    txtCPIPAbsChi.Text = objApp.CPIP_Abstract_Chi;

                    txtCASPAbstract.Text = objApp.CASP_Abstract;

                    txtcompanyOwnership.Text = objApp.Company_Ownership_Structure;

                    txtRemarks.Text = objApp.Remarks;
                    txtcompanyTag.Text = objApp.Tag;

                    InitializeUploadsDocument();

                }
                else
                {
                    Context.Response.Redirect("/SitePages/My_Company_Profile.aspx");
                }
            }
        }
        protected TB_COMPANY_PROFILE_BASIC GetExistingApplication(CyberportEMS_EDM dbContext)
        {
            TB_COMPANY_PROFILE_BASIC objApp = null;
            List<TB_COMPANY_PROFILE_BASIC> objApps = new List<TB_COMPANY_PROFILE_BASIC>();
            Guid objUserProgramId;
            if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                objUserProgramId = Guid.Parse(hdn_CompanyID.Value);
            else
                objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();
            //if (objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
            //{
            objApp = dbContext.TB_COMPANY_PROFILE_BASIC.FirstOrDefault(x => x.Company_Profile_ID == objUserProgramId);
            //}
            if (objApp != null)
            {
                hdn_CompanyID.Value = objApp.Company_Profile_ID.ToString();
            }
            return objApp;
        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_CPM");
        }
        protected void ShowbottomMessage(string Message, bool Success)
        {

            lbl_Exception.InnerHtml = "";
            if (Message.Length > 0)
            {
                if (!Success)
                    lbl_Exception.InnerHtml = Message;
            }
        }
        protected int check_db_validations(bool IsSubmitClick)

        {
            List<String> ErrorLIst = new List<string>();
            using (var dbContext = new CyberportEMS_EDM())
            {
                TB_COMPANY_PROFILE_BASIC objApp = GetExistingApplication(dbContext);
                SPFunctions objfunction = new SPFunctions();
                if (txtNameEng.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNameEng.Text))
                        objApp.Name_Eng = txtNameEng.Text;
                    else ErrorLIst.Add("Error Profile Name Eng Max 255 Characters allowed");
                }
                else
                    ErrorLIst.Add("Error Profile Name Eng Required");

                if (txtNameChi.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtNameChi.Text))
                        objApp.Name_Chi = txtNameChi.Text;
                    else ErrorLIst.Add("Error Profile Name Chi Max 255 Characters allowed");
                }
                //else ErrorLIst.Add("Error Profile Name Chi Required");
                if (txtCompanyName.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCompanyName.Text))
                        objApp.Company_Name = txtCompanyName.Text;
                    else ErrorLIst.Add("Error Company Name Max 255 characters allowed");
                }
                else ErrorLIst.Add("Error Company Name Required");

                if (txtBrandName.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtBrandName.Text))
                        objApp.Brand_Name = txtBrandName.Text;
                    else ErrorLIst.Add("Error Brand Name Max 255 characters allowed");
                }

                //else ErrorLIst.Add("Error Brand Name Required");

                if (ddlClusterCCMF.SelectedValue != "")
                    //   ErrorLIst.Add(Localize("Error Cluster CCMF Required"));
                    //else 
                    objApp.CCMF_Custer = ddlClusterCCMF.SelectedValue;

                if (ddlclusterCPIP.SelectedValue != "")
                    //    ErrorLIst.Add("Error Cluster CPIP Required");
                    //else 
                    objApp.CPIP_Custer = ddlclusterCPIP.SelectedValue;


                if (txtcompanyTag.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtcompanyTag.Text))
                        objApp.Tag = txtcompanyTag.Text;
                    else ErrorLIst.Add("Error Tags Max 255 characters allowed");
                }
                if (txtCCMFAbsEng.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCCMFAbsEng.Text))
                        objApp.CCMF_Abstract = txtCCMFAbsEng.Text;
                    else ErrorLIst.Add("Error CCMF Abstract (Eng) Max 255 characters allowed");
                }

                if (txtCCMFAbsChi.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCCMFAbsChi.Text))
                        objApp.CCMF_Abstract_Chi = txtCCMFAbsChi.Text;
                    else ErrorLIst.Add("Error CCMF Abstract (Chi) Max 255 characters allowed");

                }
                //else ErrorLIst.Add("Error CCMF Abstract (Chi) Required");

                if (txtCPIPAbsEng.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCPIPAbsEng.Text))
                        objApp.CPIP_Abstract = txtCPIPAbsEng.Text;
                    else ErrorLIst.Add("Error CPIP Abstract (Eng) Max 255 characters allowed");

                }

                //else ErrorLIst.Add("Error CPIP Abstract (Eng) Required");


                if (txtCPIPAbsChi.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCPIPAbsChi.Text))
                        objApp.CPIP_Abstract_Chi = txtCPIPAbsChi.Text;
                    else ErrorLIst.Add("Error CPIP Abstract (chi): Max 255 characters allowed");

                }
                //else ErrorLIst.Add("Error CPIP Abstract (Eng) Required");


                if (txtCASPAbstract.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCASPAbstract.Text))
                        objApp.CASP_Abstract = txtCASPAbstract.Text;
                    else ErrorLIst.Add("Error CASP Abstract: Max 255 characters allowed");
                }
                // else ErrorLIst.Add("Error CASP Abstract Required");

                if (txtcompanyOwnership.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtcompanyOwnership.Text))
                        objApp.Company_Ownership_Structure = txtcompanyOwnership.Text;
                    else ErrorLIst.Add("Error Company Ownership Structure: Max 255 characters allowed");

                }
                //else ErrorLIst.Add("Error Company Ownership Structure Required");

                if (txtRemarks.Text != "")
                {
                    if (CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtRemarks.Text))
                        objApp.Remarks = txtRemarks.Text;
                    else ErrorLIst.Add("Error Remarks: Max 255 characters allowed");
                }
                try
                {
                    if (ErrorLIst.Count == 0)
                    {
                        lblgrouperror.Visible = false;
                        dbContext.SaveChanges();
                        ShowbottomMessage("Saved Successfully", true);
                        Context.Response.Redirect("/SitePages/company_internal.aspx");
                    }
                    else
                    {
                        lblgrouperror.Visible = true;
                        lblgrouperror.DataSource = ErrorLIst;
                        lblgrouperror.DataBind();
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                return ErrorLIst.Count;
            }
        }
        protected void SaveAttachment_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SPFunctions objfunction = new SPFunctions();
            string FileName = string.Empty;
            bool IsError = false;
            string ErrorMessage = string.Empty;
            var argument = ((ImageButton)sender).CommandName;
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_PROFILE_BASIC objApp = GetExistingApplication(dbContext);
                    SPFunctions objSPFunctions = new SPFunctions();

                    string _fileUrl = string.Empty;
                    switch (Convert.ToInt32(argument))
                    {
                        case 1:
                            if (fu_companyAttachement.HasFile)
                            {
                                if (fu_companyAttachement.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                {
                                    string Extension = fu_companyAttachement.FileName.Remove(0, fu_companyAttachement.FileName.LastIndexOf(".") + 1);
                                    if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                        Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                        Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                    {
                                        string ProgrameName = "Company Profile Basic";
                                        string Version_Number = "1";

                                        _fileUrl = objSPFunctions.SR_AttachmentSave("", ProgrameName,
                                fu_companyAttachement, enumAttachmentType.Supporting_Document, Version_Number);
                                        SaveAttachmentUrl(_fileUrl, enumAttachmentType.Supporting_Document, objApp.Company_Profile_ID);
                                        lbldocuments.Text = "";
                                        InitializeUploadsDocument();
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lbldocuments.Text = Localize("File_type");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    lbldocuments.Text = Localize("File_size");
                                }

                            }
                            else
                            {
                                IsError = true;
                                lbldocuments.Text = Localize("Error_file_upload");
                            }
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);

            }
        }
        private void InitializeUploadsDocument()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_PROFILE_BASIC objApp = GetExistingApplication(dbContext);
                    if (objApp != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.Company_Profile_ID).ToList();
                        rptrdocuments.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Supporting_Document.ToString().ToLower());
                        rptrdocuments.DataBind();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        public bool SaveAttachmentUrl(string Url, enumAttachmentType objAttachmentType, Guid appId, int progId = 0)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                string FileName = string.Empty;
                SPFunctions objSp = new SPFunctions();
                FileName = Url;
                TB_APPLICATION_ATTACHMENT objAttach = new TB_APPLICATION_ATTACHMENT()
                {
                    Application_ID = appId,
                    Attachment_Path = FileName,
                    Attachment_Type = objAttachmentType.ToString(),
                    Created_By = objSp.GetCurrentUser(),
                    Created_Date = DateTime.Now,
                    Modified_By = objSp.GetCurrentUser(),
                    Modified_Date = DateTime.Now,
                    Programme_ID = progId
                };
                IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach);
                dbContext.SaveChanges();
                return true;
            }
        }
        protected void Attachments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveAttachment")
            {
                int AttachmentId = 0;
                Int32.TryParse(e.CommandArgument.ToString(), out AttachmentId);
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_APPLICATION_ATTACHMENT objAttach = dbContext.TB_APPLICATION_ATTACHMENT.FirstOrDefault(x => x.Attachment_ID == AttachmentId);
                    if (objAttach != null)
                    {
                        //SPFunctions.DeleteAttachmentfromlist(objAttach);
                        dbContext.TB_APPLICATION_ATTACHMENT.Remove(objAttach);
                        dbContext.SaveChanges();
                    }
                }
            }
            InitializeUploadsDocument();
        }
        protected void rptrOtherAttachement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                using (var dbContext = new CyberportEMS_EDM())
                {

                    TB_COMPANY_PROFILE_BASIC objApp = GetExistingApplication(dbContext);

                    SPFunctions objSp = new SPFunctions();
                    string CurrentUser = objSp.GetCurrentUser();
                    if (objApp.Created_By.ToLower() != objSp.GetCurrentUser().ToLower() && objApp.Modified_By.ToLower() != objSp.GetCurrentUser().ToLower())
                    {
                        HyperLink hypNavigation = (HyperLink)e.Item.FindControl("hypNavigation");
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        hypNavigation.Enabled = false;
                        lnkAttachmentDelete.Enabled = false;

                    }
                }
            }
        }
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            check_db_validations(false);
        }
        protected void getCoreMember()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserProgramId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserProgramId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);
                gdv_CoreMember.DataSource = dbContext.TB_COMPANY_MEMBER.Where(x => x.Company_Profile_ID == objUserProgramId).ToList();
                gdv_CoreMember.DataBind();
            }
        }
        protected void getContacts()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserProgramId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserProgramId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);
                gdv_Contact.DataSource = dbContext.TB_COMPANY_CONTACT.Where(x => x.Company_Profile_ID == objUserProgramId).ToList();
                gdv_Contact.DataBind();
            }
        }
        protected void gdv_Contact_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {

                int id = Convert.ToInt32(e.CommandArgument);
                hdn_contactID.Value = id.ToString();
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_CONTACT objContact = new TB_COMPANY_CONTACT();
                    objContact = dbContext.TB_COMPANY_CONTACT.FirstOrDefault(x => x.Contact_ID == id);
                    if (!string.IsNullOrEmpty(objContact.Name_Eng))
                    {
                        txtContactLastNameEng.Text = objContact.Name_Eng.Contains(":") ? objContact.Name_Eng.Split(':')[1] : objContact.Name_Eng;
                        txtContactFirstNameEng.Text = objContact.Name_Eng.Contains(":") ? objContact.Name_Eng.Split(':')[0] : "";
                    }

                    if (!string.IsNullOrEmpty(objContact.Name_Chi))
                    {
                        txtContactLastNameChi.Text = objContact.Name_Chi.Contains(":") ? objContact.Name_Chi.Split(':')[1] : objContact.Name_Chi;
                        txtContactFirstNameChi.Text = objContact.Name_Chi.Contains(":") ? objContact.Name_Chi.Split(':')[0] : "";
                    }


                    ddlContactSalutation.Text = objContact.Salutation;
                    txtContactHKID.Text = MD5Encryption.DecryptData(objContact.HKID);
                    txtContactInstitution.Text = objContact.Education_Institution;
                    txtContactStudentID.Text = objContact.Student_ID;
                    txtContactProgrammeEnrolled.Text = objContact.Programme_Enrolled;
                    txtContactGraduationDate.Text = objContact.Graduation_Date.HasValue ? objContact.Graduation_Date.Value.ToString("MMM-yyyy") : "";
                    txtContactOrganizationName.Text = objContact.Organization_Name;
                    txtContactPosition.Text = objContact.Position;
                    TxtContactNoHome.Text = objContact.Contact_No_Home;
                    TxtContactNoOffice.Text = objContact.Contact_No_Office;
                    TxtContactNoMobile.Text = objContact.Contact_No;
                    txtContactFaxNo.Text = objContact.Fax_No;
                    txtContactEmail.Text = objContact.Email;
                    txtContactMailingAddress.Text = objContact.Mailing_Address;
                    txtContactArea.Text = objContact.Area;
                    h2contact.InnerHtml = "Edit Contact";
                    pnl_ContactPopup.Visible = true;
                    btn_contact_remove.Visible = true;
                }
            }
        }
        protected void gdv_CoreMember_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "EditRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                hdn_MemberId.Value = id.ToString();
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_MEMBER objMember = new TB_COMPANY_MEMBER();
                    objMember = dbContext.TB_COMPANY_MEMBER.FirstOrDefault(x => x.Core_Member_ID == id);
                    txtCoreName.Text = objMember.Name;
                    txtCorePosition.Text = objMember.Position;
                    txtCoreHkid.Text = MD5Encryption.DecryptData(objMember.HKID);
                    txtCoreBackground.Text = objMember.Background_Information;
                    txtCoreBootcamp.Text = objMember.Bootcamp_Eligible_Number;
                    txtCoreQualification.Text = objMember.Professional_Qualifications;
                    txtCoreWorkingExperience.Text = objMember.Working_Experiences;
                    txtCoreAchievement.Text = objMember.Special_Achievements;
                    txtCoreMemberProfile.Text = objMember.CoreMember_Profile;
                    h2coremember.InnerHtml = "Edit Core Member";
                    pnl_CoreMemberPopup.Visible = true;
                    btn_CoreMember_Delete.Visible = true;
                }

            }
        }
        protected void getMembersList()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {

                Guid objUserCompanyId;

                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();


                List<TB_CCMF_APPLICATION> objCCMf = new List<TB_CCMF_APPLICATION>();
                objCCMf = (from a in dbContext.TB_CCMF_APPLICATION join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.CCMF_ID equals b.Application_ID select a).ToList();

                gdv_ccmf_Programme.DataSource = objCCMf;
                gdv_ccmf_Programme.DataBind();

                List<TB_INCUBATION_APPLICATION> ObjCPIP = new List<TB_INCUBATION_APPLICATION>();

                Gdv_Cpip_Programme.DataSource = (from a in dbContext.TB_INCUBATION_APPLICATION join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Incubation_ID equals b.Application_ID select a).ToList();
                Gdv_Cpip_Programme.DataBind();

                List<TB_CASP_APPLICATION> ObjCASP = new List<TB_CASP_APPLICATION>();


                ObjCASP = (from a in dbContext.TB_CASP_APPLICATION join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.CASP_ID equals b.Application_ID select a).ToList();
                Gdv_CASP_Programme.DataSource = ObjCASP;
                Gdv_CASP_Programme.DataBind();
            }
        }
        protected void getReimbursement()
        {

            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                decimal initialCap = 0;
                decimal.TryParse(dbContext.TB_SYSTEM_PARAMETER.FirstOrDefault(x => x.Config_Code == "CASP_Reimbursement_Initial_Capital").Value, out initialCap);
                lblInitialCapital.Text = "HK$" + initialCap.ToString("#,##0.00");

                Guid objUserCompanyId;

                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();

                List<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> objCASPFa = new List<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();

                objCASPFa = (from a in dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_ID equals b.Company_Profile_ID select a).ToList();
                if (objCASPFa.Count() > 0)
                {
                    gdvFaCASP.DataSource = objCASPFa;
                    gdvFaCASP.DataBind();


                    decimal TotalAmount = objCASPFa.Where(x => x.Total_Amount.HasValue).Sum(x => x.Total_Amount).Value;
                    lblCASPFATotalAmount.Text = "HK$" + TotalAmount.ToString("#,##0.00");

                    decimal ClaimedAmount = objCASPFa.Where(x => x.Status == "Completed" && x.Total_Amount.HasValue).Sum(x => x.Total_Amount).Value;
                    lblCASPFAClaimedAmount.Text = "HK$" + ClaimedAmount.ToString("#,##0.00");

                    decimal ProgressAmount = objCASPFa.Where(x => x.Status == "Submitted" && x.Total_Amount.HasValue).Sum(x => x.Total_Amount).Value;
                    lblCASPFAProgressAmount.Text = "HK$" + ProgressAmount.ToString("#,##0.00");

                    lblCASPFABalance.Text = "HK$" + (TotalAmount - (initialCap + ProgressAmount + ClaimedAmount + ProgressAmount)).ToString("#,##0.00");
                }

            }

        }
        protected void getSpecialRequest()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {

                Guid objUserCompanyId;

                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);

                var currentuser = dbContext.TB_COMPANY_PROFILE_BASIC.FirstOrDefault(x => x.Company_Profile_ID == objUserCompanyId);
                string strCurrentUser = currentuser.Created_By.ToString();

                List<TB_CASP_SPECIAL_REQUEST> objsr = new List<TB_CASP_SPECIAL_REQUEST>();
                objsr = dbContext.TB_CASP_SPECIAL_REQUEST.Where(x => x.Created_By.ToLower() == strCurrentUser.ToLower()).ToList();
                if (objsr.Count() > 0)
                {
                    gdvSR.DataSource = objsr;
                    gdvSR.DataBind();
                }
            }
        }
        protected void getfund()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserCompanyId;

                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                List<TB_COMPANY_FUND> ObjFund = dbContext.TB_COMPANY_FUND.Where(x => x.Company_Profile_ID == objUserCompanyId).ToList();
                //ObjFund = (from a in dbContext.TB_COMPANY_FUND join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_Profile_ID equals b.Company_Profile_ID select a).ToList();
                gdv_Fund.DataSource = ObjFund;
                gdv_Fund.DataBind();
            }
        }
        protected void gdv_Fund_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                Pnl_Fund_Popup.Visible = true;
                btn_Fund_Remove.Visible = true;

                int ID = Convert.ToInt32(e.CommandArgument.ToString());


                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    SPFunctions objFUnction = new SPFunctions();
                    string strCurrentUser = objFUnction.GetCurrentUser();
                    TB_COMPANY_FUND ObjFund = new TB_COMPANY_FUND();

                    ObjFund = dbContext.TB_COMPANY_FUND.FirstOrDefault(x => x.Funding_ID == ID);
                    txtFundReportedDate.Text = ObjFund.Reported_Date.HasValue ? ObjFund.Reported_Date.Value.ToString("MMM-yyyy") : "";
                    txtFundReceivedDate.Text = ObjFund.Received_Date.HasValue ? ObjFund.Received_Date.Value.ToString("MMM-yyyy") : "";
                    txtFundProgrammeName.Text = ObjFund.Programme_Name;
                    ddlFundApplicationStatus.SelectedValue = ObjFund.Application_Status;
                    txtfundFundingstatus.Text = ObjFund.Funding_Status;
                    txtFundExpenditureCovered.Text = ObjFund.Expenditure_Nature;
                    ddlfundCurrency.SelectedValue = ObjFund.Currency;
                    txtFundAmountReceived.Text = ObjFund.Amount_Received.ToString();
                    txtFundMaximumAmountReceived.Text = ObjFund.Maximum_Amount.ToString();
                    txtFundFundingOrigin.Text = ObjFund.Funding_Origin;
                    txtFundInvestorInformation.Text = ObjFund.Invertor_Info;
                    txtFundRemarks.Text = ObjFund.Remarks;
                    hdn_FundingID.Value = ID.ToString();
                    h2fund.InnerHtml = "Edit Fund ";
                    Pnl_Fund_Popup.Visible = true;

                }

            }

        }
        protected void getMergeAcquisition()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserCompanyId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                List<TB_COMPANY_MERGE_ACQUISITION> objMerge = dbContext.TB_COMPANY_MERGE_ACQUISITION.Where(x => x.Company_Profile_ID == objUserCompanyId).ToList();
                //objMerge = (from a in dbContext.TB_COMPANY_MERGE_ACQUISITION join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_Profile_ID equals b.Company_Profile_ID select a).ToList();
                gdv_MergeAcquition.DataSource = objMerge;
                gdv_MergeAcquition.DataBind();
            }
        }
        protected void gdv_MergeAcquition_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "EditRow")
            {
                int ID = Convert.ToInt32(e.CommandArgument.ToString());
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_MERGE_ACQUISITION objMerge = new TB_COMPANY_MERGE_ACQUISITION();
                    objMerge = dbContext.TB_COMPANY_MERGE_ACQUISITION.FirstOrDefault(x => x.Merge_Acquistion_ID == ID);
                    hdn_MergeAcquisitionId.Value = objMerge.Merge_Acquistion_ID.ToString();
                    txtmergeCompany.Text = objMerge.Company_Name;
                    txtmergeAmount.Text = objMerge.Amount.ToString();
                    txtmergeDate.Text = objMerge.Date.HasValue ? objMerge.Date.Value.ToString("MMM-yyyy") : "";
                    txtMergeValuation.Text = objMerge.Valuation.ToString();
                    ddlMergeMna.SelectedValue = objMerge.Merge_Acquistion;
                    ddlMergeCurrency.SelectedValue = objMerge.Currency;
                    h2MNA.InnerHtml = "Edit Merge & Acquisition ";
                    Pnl_MergeAcquisition_PopUp.Visible = true;
                    btn_MNA_Remove.Visible = true;
                }
            }

        }
        protected void getAward()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserCompanyId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                List<TB_COMPANY_AWARD> objAward = dbContext.TB_COMPANY_AWARD.Where(x => x.Company_Profile_ID == objUserCompanyId).ToList();
                //objAward = (from a in dbContext.TB_COMPANY_AWARD join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_Profile_ID equals b.Company_Profile_ID select a).ToList();
                Gdv_awards.DataSource = objAward;
                Gdv_awards.DataBind();
            }
        }
        protected void Gdv_awards_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {

                int ID = Convert.ToInt32(e.CommandArgument.ToString());
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_AWARD objAward = new TB_COMPANY_AWARD();
                    objAward = dbContext.TB_COMPANY_AWARD.FirstOrDefault(x => x.Award_ID == ID);
                    hdn_AwardID.Value = ID.ToString();
                    txtawardAwarded.Text = objAward.Awarded_Year_Month.HasValue ? objAward.Awarded_Year_Month.Value.ToString("MMM-yyyy") : "";
                    ddlAwardType.SelectedValue = objAward.Type;
                    ddlAwardNatureAwardee.SelectedValue = objAward.Nature;
                    txtAwardRecognition.Text = objAward.Name;
                    txtAwardProductChi.Text = objAward.Product_Name;
                    txtAwardAwardChi.Text = objAward.Award_Name;
                    txtAwardRemarks.Text = objAward.Remarks;
                    h2award.InnerHtml = "Edit Award";
                    Pnl_Award_Popup.Visible = true;
                    btn_Award_Remove.Visible = true;
                }
            }
        }
        protected void getJoinedAccelerator()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserCompanyId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                List<TB_Company_Joined_Accelerator> objJoined = dbContext.TB_Company_Joined_Accelerator.Where(x => x.Company_Profile_ID == objUserCompanyId).ToList();
                //objJoined = (from a in dbContext.TB_Company_Joined_Accelerator join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_Profile_ID equals b.Company_Profile_ID select a).ToList();
                gdv_JoinedAccelerator.DataSource = objJoined;
                gdv_JoinedAccelerator.DataBind();
            }
        }
        protected void gdv_JoinedAccelerator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                pnl_JoinedAccelerator_Popup.Visible = true;
                btn_JoinedAccelerator_Remove.Visible = true;
                h2joined.InnerHtml = "Edit Joined Accelerator";
                int ID = Convert.ToInt32(e.CommandArgument.ToString());
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_Company_Joined_Accelerator objJoined = new TB_Company_Joined_Accelerator();
                    objJoined = dbContext.TB_Company_Joined_Accelerator.FirstOrDefault(x => x.Joined_Accelerator_ID == ID);
                    txtjoinedYearMonth.Text = objJoined.Participation_Year_Month.HasValue ? objJoined.Participation_Year_Month.Value.ToString("MMM-yyyy") : "";
                    txtJoinedProgramme.Text = objJoined.Accelerator_Programme;
                    txtjoinedRemark.Text = objJoined.Remarks;
                    hdnJoinedID.Value = objJoined.Joined_Accelerator_ID.ToString();
                }
            }
        }
        protected void getIP()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserCompanyId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                List<TB_COMPANY_IP> objIP = dbContext.TB_COMPANY_IP.Where(x => x.Company_Profile_ID == objUserCompanyId).ToList();
                //objIP = (from a in dbContext.TB_COMPANY_IP join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_Profile_ID equals b.Company_Profile_ID select a).ToList();


                gdv_IP.DataSource = objIP;
                gdv_IP.DataBind();
            }
        }
        protected void gdv_IP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                pnl_IP_Popup.Visible = true;
                h2ip.InnerHtml = "Edit IP";
                int ID = Convert.ToInt32(e.CommandArgument.ToString());
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_IP objIP = new TB_COMPANY_IP();
                    objIP = dbContext.TB_COMPANY_IP.FirstOrDefault(x => x.IP_ID == ID);
                    hdn_IPID.Value = ID.ToString();
                    txtIPTitle.Text = objIP.Title;
                    ddlIPCategory.SelectedValue = objIP.Category;
                    txtIPRegistrationDate.Text = objIP.Registration_Date.HasValue ? objIP.Registration_Date.Value.ToString("MMM-yyyy") : "";
                    txtReportedDate.Text = objIP.Reported_Date.HasValue ? objIP.Reported_Date.Value.ToString("MMM-yyyy") : "";
                    txtIPRefrence.Text = objIP.Reference_No;
                    pnl_IP_Popup.Visible = true;
                    btn_IP_Remove.Visible = true;
                }
            }
        }
        protected void getAdministrator()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Guid objUserCompanyId;
                if (!string.IsNullOrEmpty(hdn_CompanyID.Value))
                    objUserCompanyId = Guid.Parse(hdn_CompanyID.Value);
                else
                    objUserCompanyId = Guid.Parse(Context.Request.QueryString["app"]);
                List<TB_COMPANY_ADMIN> objAdmin = dbContext.TB_COMPANY_ADMIN.Where(x => x.Company_Profile_ID == objUserCompanyId).ToList();
                //objAdmin = (from a in dbContext.TB_COMPANY_ADMIN join b in dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => x.Company_Profile_ID == objUserCompanyId) on a.Company_Profile_ID equals b.Company_Profile_ID select a).ToList();
                gdv_ManageAdministrator.DataSource = objAdmin;
                gdv_ManageAdministrator.DataBind();
            }
        }
        protected void gdv_ManageAdministrator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                Pnl_CompanyAdministrator_Popup.Visible = true;

                int ID = Convert.ToInt32(e.CommandArgument.ToString());
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_COMPANY_ADMIN objAdmin = new TB_COMPANY_ADMIN();
                    objAdmin = dbContext.TB_COMPANY_ADMIN.FirstOrDefault(x => x.Administrator_ID == ID && x.Created_By == strCurrentUser);
                    hdn_CompAdminID.Value = objAdmin.Administrator_ID.ToString();
                    txtAdminFullName.Text = objAdmin.Full_Name;
                    txtAdminEmail.Text = objAdmin.Email;
                    Pnl_CompanyAdministrator_Popup.Visible = true;
                    h2administrator.InnerHtml = "Edit Company Profile Administrator ";
                    btn_Admin_Remove.Visible = true;
                }
            }
        }
        protected void quicklnk_1_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            SetPanelVisibilityOfStep(Convert.ToInt32(id));
        }
        protected void SetPanelVisibilityOfStep(int ActiveStep)
        {
            if (ActiveStep > 0)
            {
                quicklnk_1.CssClass = "";
                quicklnk_2.CssClass = "";
                quicklnk_3.CssClass = "";
                quicklnk_4.CssClass = "";
                quicklnk_5.CssClass = "";
                quicklnk_6.CssClass = "";
                quicklnk_7.CssClass = "";
                quicklnk_8.CssClass = "";
                quicklnk_9.CssClass = "";
                quicklnk_10.CssClass = "";

                for (int i = ActiveStep; i > 0; i--)
                {
                    ((LinkButton)(this.FindControl("quicklnk_" + i))).CssClass = "active";
                }
            }
            switch (ActiveStep)
            {
                case 1:
                    {
                        pnl_BasicProfile.Visible = true;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;


                    }
                    break;


                case 2:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = true;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;


                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;


                    }
                    break;

                case 3:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = true;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;

                    }
                    break;

                case 4:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = true;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;


                    }
                    break;

                case 5:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = true;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                    }
                    break;

                case 6:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = true;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                    }
                    break;

                case 7:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = true;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;


                    }
                    break;

                case 8:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        pnl_Reimbursement.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = true;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                    }
                    break;

                case 9:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = false;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = true;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                    }
                    break;

                case 10:
                    {
                        pnl_BasicProfile.Visible = false;
                        pnl_CoreMember.Visible = false;
                        pnl_Programme.Visible = false;
                        Pnl_Fund.Visible = false;
                        pnl_MergeAcquisition.Visible = false;
                        Pnl_Awards.Visible = false;
                        pnl_JoinedAccelerator.Visible = true;
                        pnl_Ip.Visible = false;
                        Pnl_ManageAdministrator.Visible = false;

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                    }
                    break;

            }
        }
        protected void openPopup(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.CommandArgument);
            switch (id)
            {
                case 1:
                    {
                        pnl_CoreMemberPopup.Visible = true;
                        h2coremember.InnerHtml = "New Core Member";
                        btn_CoreMember_Delete.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;
                    }
                    break;


                case 2:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = true;
                        h2contact.InnerHtml = "New Contact";
                        btn_contact_remove.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;
                    }
                    break;

                case 3:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = true;
                        h2fund.InnerHtml = "New Fund";
                        btn_Fund_Remove.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;
                    }
                    break;

                case 4:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = true;
                        h2MNA.InnerHtml = "New Merge & Acquisition";
                        btn_MNA_Remove.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;
                    }
                    break;

                case 5:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = true;
                        h2award.InnerHtml = "New Award";
                        btn_Award_Remove.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;
                    }
                    break;

                case 6:
                    {
                    }
                    break;

                case 7:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = true;
                        h2ip.InnerHtml = "New IP ";
                        btn_IP_Remove.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;
                    }
                    break;
                case 8:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = true;
                        h2administrator.InnerHtml = "Create Company Profile Administrator ";
                        btn_Admin_Remove.Visible = false;
                        btn_Admin_Remove.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = false;

                    }
                    break;
                case 9:
                    {

                        pnl_CoreMemberPopup.Visible = false;
                        pnl_ContactPopup.Visible = false;
                        Pnl_Fund_Popup.Visible = false;
                        Pnl_MergeAcquisition_PopUp.Visible = false;
                        Pnl_Award_Popup.Visible = false;
                        pnl_IP_Popup.Visible = false;
                        Pnl_CompanyAdministrator_Popup.Visible = false;
                        btn_Admin_Remove.Visible = false;
                        pnl_JoinedAccelerator_Popup.Visible = true;
                        h2joined.InnerHtml = "New Joined Accelerator";
                        btn_JoinedAccelerator_Remove.Visible = false;

                    }
                    break;
            }


        }
        protected void cancelClosePopup(object sender, EventArgs e)
        {
            pnl_CoreMemberPopup.Visible = false;
            pnl_ContactPopup.Visible = false;
            Pnl_Fund_Popup.Visible = false;
            Pnl_MergeAcquisition_PopUp.Visible = false;
            Pnl_Award_Popup.Visible = false;
            pnl_IP_Popup.Visible = false;
            pnl_JoinedAccelerator_Popup.Visible = false;
            Pnl_CompanyAdministrator_Popup.Visible = false;
        }
        protected void btnCoreSave_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_MEMBER objMember = new TB_COMPANY_MEMBER();

                if (txtCoreName.Text != "")
                {
                    objMember.Name = txtCoreName.Text;
                    if (objMember.Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Name))
                        ErrorLIst.Add("Error Name Max Length is 255");
                }
                else ErrorLIst.Add("Name Required");
                if (txtCorePosition.Text != "")
                {
                    objMember.Position = txtCorePosition.Text;
                    if (objMember.Position.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Position))
                        ErrorLIst.Add("Error Name Max length is 255");
                }
                else ErrorLIst.Add("Position Required");



                if ((txtCoreHkid.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtCoreHkid.Text)))
                    ErrorLIst.Add("Error HKID Max length is 255");
                else objMember.HKID = MD5Encryption.EncryptData(txtCoreHkid.Text);
                if (txtCoreHkid.Text.Length > 0)
                {
                    if (txtCoreHkid.Text.Length > 4)
                    {
                        string masked = new string('*', txtCoreHkid.Text.Length - 4);
                        objMember.Masked_HKID = txtCoreHkid.Text.Substring(0, 4) + masked;
                    }
                    else
                    {
                        objMember.Masked_HKID = txtCoreHkid.Text;
                    }
                }

                objMember.Background_Information = txtCoreBackground.Text;
                if (objMember.Background_Information.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Background_Information))
                    ErrorLIst.Add("Error Background Max length is 255");

                objMember.Bootcamp_Eligible_Number = txtCoreBootcamp.Text;
                if (objMember.Bootcamp_Eligible_Number.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Bootcamp_Eligible_Number))
                    ErrorLIst.Add("Error Bootcamp Max length is 255");

                objMember.Professional_Qualifications = txtCoreQualification.Text;
                if (objMember.Professional_Qualifications.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Professional_Qualifications))
                    ErrorLIst.Add("Error Professional qualification Max length is 255");

                objMember.Working_Experiences = txtCoreWorkingExperience.Text;
                if (objMember.Working_Experiences.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Working_Experiences))
                    ErrorLIst.Add("Error Working experience Max length is 255");

                objMember.Special_Achievements = txtCoreAchievement.Text;
                if (objMember.Special_Achievements.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.Special_Achievements))
                    ErrorLIst.Add("Error Special achievement Max length is 255");

                objMember.CoreMember_Profile = txtCoreMemberProfile.Text;
                if (objMember.CoreMember_Profile.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMember.CoreMember_Profile))
                    ErrorLIst.Add("Error Core Member Profile Max length is 255");

                objMember.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                objMember.No_Edit = false;
                objMember.Created_By = strCurrentUser;
                objMember.Created_Date = DateTime.Now;
                if (ErrorLIst.Count == 0)
                {
                    if (hdn_MemberId.Value != "")
                    {
                        objMember.Core_Member_ID = Convert.ToInt32(hdn_MemberId.Value);
                        hdn_MemberId.Value = "";
                        dbContext.Entry(objMember).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_MEMBER.Add(objMember);
                        dbContext.Entry(objMember).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    pnl_CoreMemberPopup.Visible = false;
                    getCoreMember();
                    txtCoreName.Text = "";
                    txtCorePosition.Text = "";
                    txtCoreHkid.Text = "";
                    txtCoreBackground.Text = "";
                    txtCoreBootcamp.Text = "";
                    txtCoreQualification.Text = "";
                    txtCoreWorkingExperience.Text = "";
                    txtCoreAchievement.Text = "";
                    txtCoreMemberProfile.Text = "";
                }
                else
                {
                    corememberperror.Visible = true;
                    corememberperror.DataSource = ErrorLIst;
                    corememberperror.DataBind();
                }
            }
        }
        protected void btn_Contact_Save_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_CONTACT objContact = new TB_COMPANY_CONTACT();
                if (txtContactFirstNameEng.Text != "" && txtContactLastNameEng.Text != "")
                {
                    if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactFirstNameEng.Text))
                        ErrorLIst.Add("Error First Name(Eng) Last Name(Eng): Max Length is 255 characters");
                    else objContact.Name_Eng = txtContactFirstNameEng.Text.Trim() + ":" + txtContactLastNameEng.Text.Trim();
                }
                else
                    ErrorLIst.Add("Error Name English: First Name(Eng) and Last Name(Eng) Required");

                if (txtContactFirstNameChi.Text != "" && txtContactLastNameChi.Text != "")
                {
                    if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactFirstNameChi.Text))
                        ErrorLIst.Add("Error First Name(Chi) Last Name(Chi): Max Length is 255 characters");
                    else objContact.Name_Chi = txtContactFirstNameChi.Text + ":" + txtContactLastNameChi.Text.Trim();
                }
                else
                    ErrorLIst.Add("Error Name English: First Name(Chi) and Last Name(Chi) Required");

                objContact.Salutation = ddlContactSalutation.SelectedValue;

                if (txtContactHKID.Text != "")
                {
                    if ((txtCoreHkid.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtContactHKID.Text)))
                        ErrorLIst.Add("Error HKID Max length is 255");
                    else objContact.HKID = MD5Encryption.EncryptData(txtContactHKID.Text);
                    if (txtContactHKID.Text.Length > 0)
                    {
                        if (txtContactHKID.Text.Length > 4)
                        {
                            string masked = new string('*', txtContactHKID.Text.Length - 4);
                            objContact.Masked_HKID = txtContactHKID.Text.Substring(0, 4) + masked;
                        }
                        else
                        {
                            objContact.Masked_HKID = txtContactHKID.Text;
                        }
                    }
                }
                else ErrorLIst.Add("Error HKID Required");

                objContact.Education_Institution = txtContactInstitution.Text;
                objContact.Position = txtContactPosition.Text;

                if (TxtContactNoMobile.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.contactNo, TxtContactNoMobile.Text))
                    ErrorLIst.Add("Error Contact No Mobile is not valid");
                else objContact.Contact_No = TxtContactNoMobile.Text;
                if (TxtContactNoHome.Text != "")
                {
                    if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.contactNo, TxtContactNoHome.Text))
                        ErrorLIst.Add("Error Contact No home is not valid");
                    else objContact.Contact_No_Home = TxtContactNoHome.Text;

                }
                else
                    ErrorLIst.Add("Error Contact no Home Required");
                if (TxtContactNoOffice.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.contactNo, TxtContactNoOffice.Text))
                    ErrorLIst.Add("Error Contact No Office is not valid. ");
                else objContact.Contact_No_Office = TxtContactNoOffice.Text;
                objContact.Fax_No = txtContactFaxNo.Text;
                if (txtContactEmail.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtContactEmail.Text))
                    ErrorLIst.Add(Localize("Error Email is not valid."));
                else
                    objContact.Email = txtContactEmail.Text;

                objContact.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                objContact.Created_By = strCurrentUser;
                objContact.Created_Date = DateTime.Now;
                objContact.No_Edit = false;
                try
                {
                    if (!string.IsNullOrEmpty(txtContactGraduationDate.Text.Trim()))
                    {
                        DateTime dDate;

                        if (DateTime.TryParse(txtContactGraduationDate.Text, out dDate))
                        {
                            String.Format("{0:M-yy}", dDate);
                            objContact.Graduation_Date = Convert.ToDateTime(txtContactGraduationDate.Text.Trim());
                        }
                        else
                        {
                            ErrorLIst.Add(Localize("Error Graduation Date is not valid")); // <-- Control flow goes here
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLIst.Add(ex.Message + ",  value : " + txtContactGraduationDate.Text);
                }

                objContact.Student_ID = txtContactStudentID.Text;
                objContact.Organization_Name = txtContactOrganizationName.Text;
                objContact.Programme_Enrolled = txtContactProgrammeEnrolled.Text;
                objContact.Mailing_Address = txtContactMailingAddress.Text;
                objContact.Area = txtContactArea.Text;
                if (ErrorLIst.Count == 0)
                {
                    if (hdn_contactID.Value != "")
                    {
                        objContact.Contact_ID = Convert.ToInt32(hdn_contactID.Value);
                        hdn_contactID.Value = "";
                        dbContext.Entry(objContact).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_CONTACT.Add(objContact);
                        dbContext.Entry(objContact).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    getContacts();
                    pnl_ContactPopup.Visible = false;
                    txtContactLastNameEng.Text = "";
                    txtContactFirstNameEng.Text = "";
                    txtContactLastNameChi.Text = "";
                    txtContactFirstNameChi.Text = "";
                    ddlContactSalutation.SelectedIndex = 0;
                    txtContactHKID.Text = "";
                    txtContactInstitution.Text = "";
                    txtContactStudentID.Text = "";
                    txtContactProgrammeEnrolled.Text = "";
                    txtContactGraduationDate.Text = "";
                    txtContactOrganizationName.Text = "";
                    txtContactPosition.Text = "";
                    TxtContactNoHome.Text = "";
                    TxtContactNoOffice.Text = "";
                    TxtContactNoMobile.Text = "";
                    txtContactFaxNo.Text = "";
                    txtContactEmail.Text = "";
                    txtContactMailingAddress.Text = "";
                    txtContactArea.Text = "";
                }
                else
                {
                    contactMemberError.Visible = true;
                    contactMemberError.DataSource = ErrorLIst;
                    contactMemberError.DataBind();
                }
            }

        }
        protected void btn_fund_save_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_FUND ObjFund = new TB_COMPANY_FUND();
                ObjFund.Created_By = strCurrentUser;
                if (txtFundReportedDate.Text != "")
                {

                    DateTime dDate;

                    if (DateTime.TryParse(txtFundReportedDate.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        ObjFund.Reported_Date = Convert.ToDateTime(txtFundReportedDate.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error Date is not valid")); // <-- Control flow goes here
                    }

                }
                else ErrorLIst.Add("Required Reported Date");
                if (txtFundReceivedDate.Text != "")
                {

                    DateTime dDate;

                    if (DateTime.TryParse(txtFundReceivedDate.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        ObjFund.Received_Date = Convert.ToDateTime(txtFundReceivedDate.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error Fund Received Date is not valid"));
                    }

                }
                else ObjFund.Received_Date = ObjFund.Reported_Date;


                if (txtFundProgrammeName.Text != "")
                {
                    ObjFund.Programme_Name = txtFundProgrammeName.Text;

                    if (ObjFund.Programme_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), ObjFund.Programme_Name))
                        ErrorLIst.Add("Error Programme Name Max length is 255");
                }
                else ErrorLIst.Add("Programme Name Required");
                ObjFund.Created_Date = DateTime.Now;
                ObjFund.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);

                ObjFund.Application_Status = ddlFundApplicationStatus.SelectedValue;
                if (txtfundFundingstatus.Text != "")
                {
                    ObjFund.Funding_Status = txtfundFundingstatus.Text;
                }
                else ErrorLIst.Add("Funding status Required");
                if (txtFundExpenditureCovered.Text != "")
                {
                    ObjFund.Expenditure_Nature = txtFundExpenditureCovered.Text;

                    if (ObjFund.Expenditure_Nature.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), ObjFund.Expenditure_Nature))
                        ErrorLIst.Add("Error Nature of Expenditure Covered Max length is 255");
                }
                else ErrorLIst.Add("Nature of Expenditure Covered Required");



                if (ddlfundCurrency.SelectedValue != "")
                {
                    ObjFund.Currency = ddlfundCurrency.SelectedValue;
                }
                else ErrorLIst.Add("currency Required");

                if (txtFundAmountReceived.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtFundAmountReceived.Text) || (txtFundAmountReceived.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtFundAmountReceived.Text)))
                    ErrorLIst.Add("Error Amount");
                else if (txtFundAmountReceived.Text.Length > 0)
                    ObjFund.Amount_Received = Convert.ToDecimal(txtFundAmountReceived.Text);

                if (txtFundMaximumAmountReceived.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtFundMaximumAmountReceived.Text) || (txtFundMaximumAmountReceived.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtFundMaximumAmountReceived.Text)))
                    ErrorLIst.Add("Error Maximum Amount");
                else if (txtFundAmountReceived.Text.Length > 0)
                    ObjFund.Maximum_Amount = Convert.ToDecimal(txtFundMaximumAmountReceived.Text);

                ObjFund.Funding_Origin = txtFundFundingOrigin.Text;
                ObjFund.Invertor_Info = txtFundInvestorInformation.Text;
                ObjFund.Remarks = txtFundRemarks.Text;
                ObjFund.No_Edit = false;
                if (ErrorLIst.Count == 0)
                {
                    if (hdn_FundingID.Value != "")
                    {
                        ObjFund.Funding_ID = Convert.ToInt32(hdn_FundingID.Value);
                        hdn_FundingID.Value = "";
                        dbContext.Entry(ObjFund).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_FUND.Add(ObjFund);
                        dbContext.Entry(ObjFund).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    getfund();
                    Pnl_Fund_Popup.Visible = false;
                }
                else
                {
                    FundError.Visible = true;
                    FundError.DataSource = ErrorLIst;
                    FundError.DataBind();
                }
            }
            txtFundReportedDate.Text = "";
            txtFundReceivedDate.Text = "";
            txtFundProgrammeName.Text = "";
            
            txtfundFundingstatus.Text = "";
            txtFundExpenditureCovered.Text = "";
            txtFundAmountReceived.Text = "";
            txtFundMaximumAmountReceived.Text = "";
            txtFundFundingOrigin.Text = "";
            txtFundInvestorInformation.Text = "";
            txtFundRemarks.Text = "";
        }
        protected void btn_Merge_Edit_Save_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_MERGE_ACQUISITION objMerge = new TB_COMPANY_MERGE_ACQUISITION();
                objMerge.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                objMerge.Created_By = strCurrentUser;
                objMerge.Created_Date = DateTime.Now;
                if (txtmergeCompany.Text != "")
                {
                    objMerge.Company_Name = txtmergeCompany.Text;

                    if (objMerge.Company_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objMerge.Company_Name))
                        ErrorLIst.Add("Error Nature of Expenditure Covered Max length is 255");
                }
                else ErrorLIst.Add("Company Name Required");

                if (txtmergeAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtmergeAmount.Text) || (txtmergeAmount.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtmergeAmount.Text)))
                    ErrorLIst.Add("Error Amount");
                else if (txtmergeAmount.Text.Length > 0)
                    objMerge.Amount = Convert.ToDecimal(txtmergeAmount.Text);


                if (txtmergeAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtMergeValuation.Text) || (txtMergeValuation.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtMergeValuation.Text)))
                    ErrorLIst.Add("Error Amount");
                else if (txtmergeAmount.Text.Length > 0)
                    objMerge.Valuation = Convert.ToDecimal(txtMergeValuation.Text);

                objMerge.Merge_Acquistion = ddlMergeMna.SelectedValue;
                objMerge.Currency = ddlMergeCurrency.SelectedValue;
                if (txtmergeDate.Text != "")

                {
                    DateTime dDate;

                    if (DateTime.TryParse(txtmergeDate.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        objMerge.Date = Convert.ToDateTime(txtmergeDate.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error Date is not valid")); // <-- Control flow goes here
                    }
                }
                else ErrorLIst.Add("Error Date Required");

                if (ErrorLIst.Count == 0)
                {
                    if (hdn_MergeAcquisitionId.Value != "")
                    {
                        objMerge.Merge_Acquistion_ID = int.Parse(hdn_MergeAcquisitionId.Value);
                        hdn_MergeAcquisitionId.Value = "";
                        dbContext.Entry(objMerge).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_MERGE_ACQUISITION.Add(objMerge);
                        dbContext.Entry(objMerge).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    getMergeAcquisition();
                    Pnl_MergeAcquisition_PopUp.Visible = false;
                    txtmergeCompany.Text = "";
                    txtmergeAmount.Text = "";
                    txtmergeDate.Text = "";
                    txtMergeValuation.Text = "";
                }
                else
                {
                    MnAError.Visible = true;
                    MnAError.DataSource = ErrorLIst;
                    MnAError.DataBind();
                }
            }

        }
        protected void btn_Award_Save_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_AWARD objAward = new TB_COMPANY_AWARD();
                objAward.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                objAward.Created_By = strCurrentUser;
                objAward.Created_Date = DateTime.Now;
                if (txtawardAwarded.Text != "")
                {
                    DateTime dDate;
                    if (DateTime.TryParse(txtawardAwarded.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        objAward.Awarded_Year_Month = Convert.ToDateTime(txtawardAwarded.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error Awarded Date is not valid")); // <-- Control flow goes here
                    }
                }
                objAward.Type = ddlAwardType.SelectedValue;
                objAward.Nature = ddlAwardNatureAwardee.SelectedValue;

                objAward.Name = txtAwardRecognition.Text;
                if (objAward.Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objAward.Name))
                    ErrorLIst.Add("Error Award / Recognition Max length is 255");

                objAward.Product_Name = txtAwardProductChi.Text;
                if (objAward.Product_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objAward.Product_Name))
                    ErrorLIst.Add("Error Product(Chi) Max length is 255");

                objAward.Award_Name = txtAwardAwardChi.Text;
                if (objAward.Award_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objAward.Award_Name))
                    ErrorLIst.Add("Error Award(Chi) Max length is 255");


                objAward.Remarks = txtAwardRemarks.Text;
                if (ErrorLIst.Count == 0)
                {

                    if (hdn_AwardID.Value != "")
                    {
                        objAward.Award_ID = Convert.ToInt32(hdn_AwardID.Value);
                        hdn_AwardID.Value = "";
                        dbContext.Entry(objAward).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_AWARD.Add(objAward);
                        dbContext.Entry(objAward).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    getAward();
                    Pnl_Award_Popup.Visible = false;
                    txtawardAwarded.Text = "";
                    ddlAwardType.SelectedValue = "";
                    ddlAwardNatureAwardee.SelectedValue = "";
                    txtAwardRecognition.Text = "";
                    txtAwardProductChi.Text = "";
                    txtAwardAwardChi.Text = "";
                    txtAwardRemarks.Text = "";
                }
                else
                {

                    AwardError.Visible = true;
                    AwardError.DataSource = ErrorLIst;
                    AwardError.DataBind();
                }
            }

        }
        protected void btn_IP_Save_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_IP objIP = new TB_COMPANY_IP();
                objIP.Created_By = strCurrentUser;
                objIP.Created_Date = DateTime.Now;
                objIP.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                if (txtIPTitle.Text != "")
                {
                    objIP.Title = txtIPTitle.Text;
                    if (objIP.Title.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), objIP.Title))
                        ErrorLIst.Add("Error Title Max length is 50");
                }
                else ErrorLIst.Add("Error Title Required");
                if (ddlIPCategory.SelectedValue != "")
                    objIP.Category = ddlIPCategory.SelectedValue;
                else ErrorLIst.Add("Error Category Required");


                if (txtIPRegistrationDate.Text != "")

                {
                    DateTime dDate;

                    if (DateTime.TryParse(txtIPRegistrationDate.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        objIP.Registration_Date = Convert.ToDateTime(txtIPRegistrationDate.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error Registration Date is not valid")); // <-- Control flow goes here
                    }


                }
                else ErrorLIst.Add("Error Registration Date Required");

                if (txtReportedDate.Text != "")


                {
                    DateTime dDate;

                    if (DateTime.TryParse(txtReportedDate.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        objIP.Reported_Date = Convert.ToDateTime(txtReportedDate.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add(Localize("Error Registration Date is not valid")); // <-- Control flow goes here
                    }

                }
                else ErrorLIst.Add("Error Reported Date Required");
                objIP.Reference_No = txtIPRefrence.Text;
                if (objIP.Reference_No.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), objIP.Reference_No))
                    ErrorLIst.Add("Error Refrence No Max length is 50");

                if (ErrorLIst.Count == 0)
                {
                    if (hdn_IPID.Value != "")
                    {
                        objIP.IP_ID = Convert.ToInt32(hdn_IPID.Value);
                        hdn_IPID.Value = "";
                        dbContext.Entry(objIP).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_IP.Add(objIP);
                        dbContext.Entry(objIP).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    pnl_IP_Popup.Visible = false;
                    getIP();
                    txtIPTitle.Text = "";
                    txtIPRegistrationDate.Text = "";
                    txtReportedDate.Text = "";
                    txtIPRefrence.Text = "";
                }
                else
                {
                    IPError.Visible = true;
                    IPError.DataSource = ErrorLIst;
                    IPError.DataBind();
                }
            }

        }
        protected void btn_Admin_Save_Click(object sender, EventArgs e)
        {

            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_COMPANY_ADMIN objAdmin = new TB_COMPANY_ADMIN();
                objAdmin.Created_By = strCurrentUser;
                objAdmin.Created_Date = DateTime.Now;
                objAdmin.Full_Name = txtAdminFullName.Text;
                objAdmin.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                if (objAdmin.Full_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objAdmin.Full_Name))
                    ErrorLIst.Add("Error Full Name Max length is 255");

                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtAdminEmail.Text) && !string.IsNullOrEmpty(txtAdminEmail.Text))
                    objAdmin.Email = txtAdminEmail.Text;
                else ErrorLIst.Add("Error  Email");

                if (ErrorLIst.Count == 0)
                {
                    if (hdn_CompAdminID.Value != "")
                    {
                        objAdmin.Administrator_ID = Convert.ToInt32(hdn_CompAdminID.Value);
                        hdn_CompAdminID.Value = "";
                        dbContext.Entry(objAdmin).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_COMPANY_ADMIN.Add(objAdmin);
                        dbContext.Entry(objAdmin).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    getAdministrator();
                    Pnl_CompanyAdministrator_Popup.Visible = false;
                    txtAdminFullName.Text = "";
                    txtAdminEmail.Text = "";
                }
                else
                {
                    adminError.Visible = true;
                    adminError.DataSource = ErrorLIst;
                    adminError.DataBind();
                }
            }

        }
        protected void btn_joinedAccelerator_save_Click(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<String> ErrorLIst = new List<string>();
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                TB_Company_Joined_Accelerator objJoined = new TB_Company_Joined_Accelerator();
                objJoined.Created_By = strCurrentUser;
                objJoined.Created_Date = DateTime.Now;
                objJoined.Company_Profile_ID = Guid.Parse(hdn_CompanyID.Value);
                if (txtjoinedYearMonth.Text != "")
                {
                    DateTime dDate;
                    if (DateTime.TryParse(txtjoinedYearMonth.Text, out dDate))
                    {
                        String.Format("{0:M-yy}", dDate);
                        objJoined.Participation_Year_Month = Convert.ToDateTime(txtjoinedYearMonth.Text.Trim());
                    }
                    else
                    {
                        ErrorLIst.Add("Error Year-Month is not valid"); // <-- Control flow goes here
                    }
                }
                if (txtJoinedProgramme.Text != "")
                {
                    objJoined.Accelerator_Programme = txtJoinedProgramme.Text;
                    if (objJoined.Accelerator_Programme.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objJoined.Accelerator_Programme))
                        ErrorLIst.Add("Error Accelerator Programme Max length is 255");
                }
                else
                    ErrorLIst.Add("Error Accelerator Programme required");

                objJoined.Remarks = txtjoinedRemark.Text;
                if (objJoined.Remarks.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objJoined.Remarks))
                    ErrorLIst.Add("Error Remarks Max length is 255");
                if (ErrorLIst.Count == 0)
                {
                    if (hdnJoinedID.Value != "")
                    {
                        objJoined.Joined_Accelerator_ID = Convert.ToInt32(hdnJoinedID.Value);
                        hdn_CompAdminID.Value = "";
                        dbContext.Entry(objJoined).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        dbContext.TB_Company_Joined_Accelerator.Add(objJoined);
                        dbContext.Entry(objJoined).State = System.Data.Entity.EntityState.Added;
                    }
                    dbContext.SaveChanges();
                    getJoinedAccelerator();
                    pnl_JoinedAccelerator_Popup.Visible = false;
                    txtjoinedYearMonth.Text = "";
                    txtJoinedProgramme.Text = "";
                    txtjoinedRemark.Text = "";
                }
                else
                {
                    joinedError.Visible = true;
                    joinedError.DataSource = ErrorLIst;
                    joinedError.DataBind();
                }
            }
        }
        protected void open_Delete_confirmation(object sender, EventArgs e)
        {
          
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.CommandArgument);
            btn_Confirmation_Yes.CommandArgument = id.ToString();
            pnl_CoreMemberPopup.Visible = false;
            pnl_ContactPopup.Visible = false;
            Pnl_Fund_Popup.Visible = false;
            pnl_IP_Popup.Visible = false;
            pnl_JoinedAccelerator_Popup.Visible = false;
            Pnl_MergeAcquisition_PopUp.Visible = false;
            Pnl_Award_Popup.Visible = false;
            Pnl_CompanyAdministrator_Popup.Visible = false;
            pnlconfirmation.Visible = true;
        }
        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlconfirmation.Visible = false;
            btn_Confirmation_Yes.CommandArgument = "";
        }

        protected void Delete_confirmation(object sender, EventArgs e)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                Button btn = (Button)sender;
                int id = Convert.ToInt32(btn.CommandArgument);
                switch (id)
                {
                    case 1:
                        {
                            TB_COMPANY_MEMBER objMember = new TB_COMPANY_MEMBER();
                            objMember.Core_Member_ID = Convert.ToInt32(hdn_MemberId.Value);
                            dbContext.Entry(objMember).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getCoreMember();
                            pnlconfirmation.Visible = false;
                            btn_CoreMember_Delete.Visible = false;
                            hdn_MemberId.Value = "";
                            txtCoreName.Text = "";
                            txtCorePosition.Text = "";
                            txtCoreHkid.Text = "";
                            txtCoreBackground.Text = "";
                            txtCoreBootcamp.Text = "";
                            txtCoreQualification.Text = "";
                            txtCoreWorkingExperience.Text = "";
                            txtCoreAchievement.Text = "";
                            txtCoreMemberProfile.Text = "";

                        }
                        break;
                    case 2:
                        {
                            TB_COMPANY_CONTACT objContact = new TB_COMPANY_CONTACT();
                            objContact.Contact_ID = Convert.ToInt32(hdn_contactID.Value);
                            dbContext.Entry(objContact).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getContacts();
                            btn_contact_remove.Visible = false;
                            pnlconfirmation.Visible = false;
                            hdn_contactID.Value = "";
                            txtContactLastNameEng.Text = "";
                            txtContactFirstNameEng.Text = "";
                            txtContactLastNameChi.Text = "";
                            txtContactFirstNameChi.Text = "";
                            ddlContactSalutation.SelectedIndex = 0;
                            txtContactHKID.Text = "";
                            txtContactInstitution.Text = "";
                            txtContactStudentID.Text = "";
                            txtContactProgrammeEnrolled.Text = "";
                            txtContactGraduationDate.Text = "";
                            txtContactOrganizationName.Text = "";
                            txtContactPosition.Text = "";
                            TxtContactNoHome.Text = "";
                            TxtContactNoOffice.Text = "";
                            TxtContactNoMobile.Text = "";
                            txtContactFaxNo.Text = "";
                            txtContactEmail.Text = "";
                            txtContactMailingAddress.Text = "";
                            txtContactArea.Text = "";
                        }
                        break;
                    case 3:
                        {

                            TB_COMPANY_FUND ObjFund = new TB_COMPANY_FUND();
                            ObjFund.Funding_ID = Convert.ToInt32(hdn_FundingID.Value);
                            dbContext.Entry(ObjFund).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getfund();
                            pnlconfirmation.Visible = false;
                            btn_Fund_Remove.Visible = false;
                            txtFundReportedDate.Text = "";
                            txtFundReceivedDate.Text = "";
                            txtFundProgrammeName.Text = "";
                            hdn_FundingID.Value = "";
                            txtfundFundingstatus.Text = "";
                            txtFundExpenditureCovered.Text = "";
                            txtFundAmountReceived.Text = "";
                            txtFundMaximumAmountReceived.Text = "";
                            txtFundFundingOrigin.Text = "";
                            txtFundInvestorInformation.Text = "";
                            txtFundRemarks.Text = "";
                        }
                        break;
                    case 4:
                        {
                            TB_COMPANY_MERGE_ACQUISITION objMerge = new TB_COMPANY_MERGE_ACQUISITION();
                            objMerge.Merge_Acquistion_ID = Convert.ToInt32(hdn_MergeAcquisitionId.Value);
                            dbContext.Entry(objMerge).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getMergeAcquisition();
                            hdn_MergeAcquisitionId.Value = "";
                            pnlconfirmation.Visible = false;
                            btn_MNA_Remove.Visible = false;
                            txtmergeCompany.Text = "";
                            txtmergeAmount.Text = "";
                            txtmergeDate.Text = "";
                            txtMergeValuation.Text = "";
                        }
                        break;
                    case 5:
                        {
                            TB_COMPANY_AWARD objAward = new TB_COMPANY_AWARD();
                            objAward.Award_ID = Convert.ToInt32(hdn_AwardID.Value);
                            dbContext.Entry(objAward).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getAward();
                            pnlconfirmation.Visible = false;
                            btn_Award_Remove.Visible = false;
                            txtawardAwarded.Text = "";
                            ddlAwardType.SelectedIndex = 0;
                            hdn_AwardID.Value = "";
                            txtAwardRecognition.Text = "";
                            txtAwardProductChi.Text = "";
                            txtAwardAwardChi.Text = "";
                            txtAwardRemarks.Text = "";
                        }
                        break;
                    case 6:
                        {
                            TB_COMPANY_IP objIP = new TB_COMPANY_IP();
                            objIP.IP_ID = Convert.ToInt32(hdn_IPID.Value);
                            dbContext.Entry(objIP).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            pnlconfirmation.Visible = false;
                            btn_IP_Remove.Visible = false;
                            hdn_IPID.Value = "";
                            getIP();
                            txtIPTitle.Text = "";
                            txtIPRegistrationDate.Text = "";
                            txtReportedDate.Text = "";
                            txtIPRefrence.Text = "";
                        }
                        break;
                    case 7:
                        {
                            TB_Company_Joined_Accelerator objJoined = new TB_Company_Joined_Accelerator();
                            objJoined.Joined_Accelerator_ID = Convert.ToInt32(hdnJoinedID.Value);
                            dbContext.Entry(objJoined).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getJoinedAccelerator();
                            pnlconfirmation.Visible = false;
                            btn_JoinedAccelerator_Remove.Visible = false;
                            hdnJoinedID.Value = "";
                            txtjoinedYearMonth.Text = "";
                            txtJoinedProgramme.Text = "";
                            txtjoinedRemark.Text = "";
                        }
                        break;
                    case 8:
                        {

                            TB_COMPANY_ADMIN objAdmin = new TB_COMPANY_ADMIN();
                            objAdmin.Administrator_ID = Convert.ToInt32(hdn_CompAdminID.Value);
                            dbContext.Entry(objAdmin).State = System.Data.Entity.EntityState.Deleted;
                            dbContext.SaveChanges();
                            getAdministrator();
                            pnlconfirmation.Visible = false;
                            btn_Admin_Remove.Visible = false;
                            txtAdminFullName.Text = "";
                            txtAdminEmail.Text = "";
                            hdn_CompAdminID.Value = "";
                        }
                        break;
                    case 9:
                        {
                            btn_Confirmation_Yes.CommandArgument = "";
                            pnlconfirmation.Visible = false;
                        }
                        break;

                }
            }
        }
    }
}
