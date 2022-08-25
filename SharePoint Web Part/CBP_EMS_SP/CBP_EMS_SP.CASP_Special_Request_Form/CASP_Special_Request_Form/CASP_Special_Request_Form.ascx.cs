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

namespace CBP_EMS_SP.CASP_Special_Request_Form.VisualWebPart1
{
    [ToolboxItemAttribute(false)]
    public partial class VisualWebPart1 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public VisualWebPart1()
        {
        }
        private bool IsApplicantUser = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Context.Request.QueryString["app"]))
            {
                Context.Response.Redirect("/SitePages/MyReimbursements.aspx");
            }

            if (!Page.IsPostBack)
            {
                FillSpecialRequestDetails();
            }

        }


        protected void FillSpecialRequestDetails()
        {
            SPFunctions objSp = new SPFunctions();
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);
                FillUserAttendent(objApp);
                FillUserCompany(objApp);

                if (objApp != null)
                {
                    bool isDisabled = false;

                    if (((objApp.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) && (objApp.Status.Replace("_", " ") != formsubmitaction.Saved.ToString().Replace("_", " "))) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                    {
                        isDisabled = true;
                        DisableControls();
                    }
                    if (objApp.Status.Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " "))
                    {
                        btn_Save.Visible = false;


                    }

                    ddlcompanyname.SelectedValue = objApp.Company_ID.ToString();
                    ddlProgrammeattended.SelectedValue = objApp.CASP_ID.ToString();

                    lblApplicant.Text = objApp.Created_By;
                    lblApplicationNo.Text = objApp.Application_No;

                    hdn_ApplicationID.Value = objApp.CASP_Special_Request_ID.ToString();

                    if (objApp.Modified_Date.HasValue)
                        lblLastSubmitted.Text = objApp.Modified_Date.Value.ToString("dd MMM yyyy");

                    txtContactName.Text = objApp.Contact_Name;
                    lblContactName.Text = objApp.Contact_Name;
                    txtDescription.Text = objApp.Description;
                    lblDescription.Text = objApp.Description;
                    txtEmail.Text = objApp.Email;
                    lblEmail.Text = objApp.Email;
                    txtEstimatedAmount.Text = objApp.Estimate_Amount.ToString();
                    if (objApp.Estimate_Amount != null)
                    {
                        decimal amount = objApp.Estimate_Amount.GetValueOrDefault(0m);
                        lblEstimatedAmount.Text = amount.ToString("#,0.00");
                    }
                    else
                        lblEstimatedAmount.Text = objApp.Estimate_Amount.ToString();
                    lbljustification.Text = objApp.Justification;
                    txtPhoneNo.Text = objApp.Phone_No;
                    lblPhoneNo.Text = objApp.Phone_No;
                    txtPurpose.Text = objApp.Purpose;
                    lblPurpose.Text = objApp.Purpose;
                    txtServiceProviderName.Text = objApp.Service_Provider_Name;
                    lblServiceProviderName.Text = objApp.Service_Provider_Name;
                    InitializeUploadsDocument();
                }

                else
                {
                    int count = 0;
                    DateTime year = new DateTime();
                    year = DateTime.Now;
                    //        lblApplicationNo.Text = HttpUtility.HtmlEncode("CASP-SR-" +year.ToString("yy")+"-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                    lblApplicant.Text = objSp.GetCurrentUser();
                    hdn_ApplicationID.Value = "";
                }

            }
        }


        protected void DisableControls()
        {
            ddlcompanyname.Enabled = false;
            ddlProgrammeattended.Enabled = false;

            btn_Save.Visible = false;
            btn_Submit.Visible = false;

            Fu_SupportingDocuments.Enabled = false;
            ImageButton14.Enabled = false;
            // pnlSupportingDocument.Enabled = false;

            ShowLabels();
        }


        private void ShowLabels()
        {
            txtContactName.Visible = false;
            lblContactName.Visible = true;

            txtDescription.Visible = false;
            lblDescription.Visible = true;

            txtEmail.Visible = false;
            lblEmail.Visible = true;

            txtEstimatedAmount.Visible = false;
            lblEstimatedAmount.Visible = true;

            txtjustification.Visible = false;
            lbljustification.Visible = true;

            txtPhoneNo.Visible = false;
            lblPhoneNo.Visible = true;



            txtPurpose.Visible = false;
            lblPurpose.Visible = true;

            txtServiceProviderName.Visible = false;
            lblServiceProviderName.Visible = true;


        }
        protected TB_CASP_SPECIAL_REQUEST GetExistingApplication(CyberportEMS_EDM dbContext)
        {
            TB_CASP_SPECIAL_REQUEST objApp = null;
            List<TB_CASP_SPECIAL_REQUEST> objApps = new List<TB_CASP_SPECIAL_REQUEST>();
            Guid objUserProgramId;
            if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
            else
                objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();

            if (objFUnction.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
            {
                IsApplicantUser = true;
                objApp = dbContext.TB_CASP_SPECIAL_REQUEST.FirstOrDefault(x => x.CASP_Special_Request_ID == objUserProgramId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                    objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);

                objApp = dbContext.TB_CASP_SPECIAL_REQUEST.FirstOrDefault(x => x.CASP_Special_Request_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());

            }
            if (objApp != null)
            {
                lblSubmissionApplication.Text = objApp.Application_No;
                hdn_ApplicationID.Value = objApp.CASP_Special_Request_ID.ToString();
            }
            return objApp;
        }


        protected int check_db_validations(bool IsSubmitClick)

        {
            List<String> ErrorLIst = new List<string>();

            using (var dbContext = new CyberportEMS_EDM())
            {



                SPFunctions objfunction = new SPFunctions();

                TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);

                bool isnewobj = false;
                if (objApp == null)
                {
                    isnewobj = true;
                    objApp = new TB_CASP_SPECIAL_REQUEST();

                }


                if (ddlProgrammeattended.SelectedValue == "")
                { ErrorLIst.Add(Localize("Error_AcceleratorProgram")); }
                else objApp.CASP_ID = Guid.Parse(ddlProgrammeattended.SelectedValue);



                if (ddlcompanyname.SelectedValue == "")
                {
                    ErrorLIst.Add(Localize("Error_CompanySelect"));
                }
                else objApp.Company_ID = Guid.Parse(ddlcompanyname.SelectedValue);

                objApp.Contact_Name = txtContactName.Text;
                if ((IsSubmitClick && objApp.Contact_Name.Length <= 0) || objApp.Contact_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Contact_Name))
                    ErrorLIst.Add(Localize("Error_ContactName"));


                objApp.Phone_No = txtPhoneNo.Text;

                if ((IsSubmitClick && txtPhoneNo.Text.Length <= 0) || txtPhoneNo.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.contactNo, txtPhoneNo.Text))
                    ErrorLIst.Add(Localize("Error_PhoneNo"));


                objApp.Email = txtEmail.Text;
                if ((IsSubmitClick && txtEmail.Text.Length <= 0) || txtEmail.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtEmail.Text))
                    ErrorLIst.Add(Localize("Error_Email"));


                objApp.Service_Provider_Name = txtServiceProviderName.Text;
                if ((IsSubmitClick && objApp.Service_Provider_Name.Length <= 0) || objApp.Service_Provider_Name.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Service_Provider_Name))
                    ErrorLIst.Add(Localize("Error_ServiceProvider"));

                if ((txtEstimatedAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtEstimatedAmount.Text)) || (IsSubmitClick && txtEstimatedAmount.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtEstimatedAmount.Text)))

                    ErrorLIst.Add(Localize("Error_EstimatedAmount"));

                else if (txtEstimatedAmount.Text.Length > 0)
                {
                    objApp.Estimate_Amount = Convert.ToDecimal(txtEstimatedAmount.Text);
                }
                objApp.Purpose = txtPurpose.Text;
                if ((IsSubmitClick && objApp.Purpose.Length <= 0) || objApp.Purpose.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Purpose))
                    ErrorLIst.Add(Localize("Error_Purpose"));


                objApp.Description = txtDescription.Text;
                if ((IsSubmitClick && objApp.Description.Length <= 0) || objApp.Description.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Description))
                    ErrorLIst.Add(Localize("Error_DescriptionOfService"));


                objApp.Justification = txtjustification.Text;
                if ((IsSubmitClick && objApp.Justification.Length <= 0) || objApp.Justification.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), objApp.Justification))
                    ErrorLIst.Add(Localize("Error_Justification"));

                try
                {
                    if (ErrorLIst.Count == 0)
                    {
                        lblgrouperror.Visible = false;


                        if (isnewobj)
                        {

                            objApp.CASP_Special_Request_ID = Guid.NewGuid();
                            objApp.Status = "Saved";

                            int count = 0;

                            TB_CASP_SPECIAL_REQUEST result = dbContext.TB_CASP_SPECIAL_REQUEST.OrderByDescending(x => x.Application_No).FirstOrDefault();
                            if (result != null)
                            {
                                count = Convert.ToInt32(result.Application_No.Substring(result.Application_No.Length - 4, 4)) + 1;
                            }
                            else
                            {
                                count = 1;
                            }
                            DateTime year = new DateTime();
                            year = DateTime.Now;
                            lblApplicationNo.Text = HttpUtility.HtmlEncode("CASP-SR-" + year.ToString("yy") + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                            objApp.Application_No = lblApplicationNo.Text;
                            objApp.Created_By = Convert.ToString(objfunction.GetCurrentUser());
                            objApp.Created_Date = DateTime.Now;
                            objApp.Application_No = lblApplicationNo.Text;
                            dbContext.TB_CASP_SPECIAL_REQUEST.Add(objApp);

                        }

                        dbContext.SaveChanges();
                        hdn_ApplicationID.Value = objApp.CASP_Special_Request_ID.ToString();

                        objApp = GetExistingApplication(dbContext);

                        ShowbottomMessage("Saved Successfully", true);

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

        protected void FillUserCompany(TB_CASP_SPECIAL_REQUEST objApp)
        {

            IncubationContext objcomp = new IncubationContext();
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();
            if (!IsApplicantUser)
                strCurrentUser = objApp.Created_By;
            List<CASP_CompanyList> objCompanyList = IncubationContext.GetCompanyForUserCASP(strCurrentUser);

            ddlcompanyname.DataSource = objCompanyList;
            ddlcompanyname.DataTextField = "CompanyName";
            ddlcompanyname.DataValueField = "CompanyIdNumber";
            ddlcompanyname.DataBind();
            ddlcompanyname.Items.Insert(0, new ListItem() { Text = "Select Company", Value = "" });
            if (objApp != null)
            {
                ddlcompanyname.SelectedValue = Convert.ToString(objApp.Company_ID.Value);
            }

        }


        protected void FillUserAttendent(TB_CASP_SPECIAL_REQUEST objApp)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                if (!IsApplicantUser)
                    strCurrentUser = objApp.Created_By;
                List<CASP_Programme_Attended> applicantprogram = (from a in dbContext.TB_CASP_APPLICATION.Where(x => x.Applicant == strCurrentUser) from b in dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == a.Programme_ID) select new CASP_Programme_Attended() { CASP_ID = a.CASP_ID, Programme_Name = a.Accelerator_Name }).ToList();

                ddlProgrammeattended.DataSource = applicantprogram;
                ddlProgrammeattended.DataTextField = "Programme_Name";
                ddlProgrammeattended.DataValueField = "CASP_ID";
                ddlProgrammeattended.DataBind();
                ddlProgrammeattended.Items.Insert(0, new ListItem() { Text = "Select Programme", Value = "" });
                if (objApp != null)
                {
                    ddlProgrammeattended.SelectedValue = Convert.ToString(objApp.CASP_Attended);
                }
            }

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            check_db_validations(false);
        }


        private Guid NewProgramId()
        {
            Guid objNewId = Guid.NewGuid();
            while (new CyberportEMS_EDM().TB_CASP_SPECIAL_REQUEST.Where(x => x.CASP_Special_Request_ID == objNewId).Count() == 0)
            {
                objNewId = Guid.NewGuid();
                break;
            }
            return objNewId;
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

                    TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);
                    if (objApp == null)
                    {
                        if (check_db_validations(false) == 0)
                        {

                            objApp = GetExistingApplication(dbContext);
                            hdn_ApplicationID.Value = objApp.CASP_Special_Request_ID.ToString();

                        }
                    }
                    if (objApp != null)
                    {
                        if (objApp.Submitted_Date.HasValue && objApp.Status.ToLower() == formsubmitaction.Submitted.ToString().ToLower())
                        {

                            //  objApp = GetExistingApplication(dbContext, progId);
                            //  objApp.Version_Number = (Convert.ToDecimal(objApp.Version_Number) + Convert.ToDecimal(".01")).ToString();
                            //  dbContext.SaveChanges();
                        }

                        SPFunctions objSPFunctions = new SPFunctions();

                        string _fileUrl = string.Empty;
                        switch (Convert.ToInt32(argument))
                        {
                            case 1:
                                if (Fu_SupportingDocuments.HasFile)
                                {
                                    if (Fu_SupportingDocuments.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_SupportingDocuments.FileName.Remove(0, Fu_SupportingDocuments.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            string ProgrameName = "CASP-SpecialRequest";
                                            string Version_Number = "1";

                                            _fileUrl = objSPFunctions.SR_AttachmentSave(objApp.Application_No, ProgrameName,
                                    Fu_SupportingDocuments, enumAttachmentType.Supporting_Document, Version_Number);
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.Supporting_Document, objApp.CASP_Special_Request_ID);
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
            }
            catch (Exception ex)
            {
                ShowbottomMessage(ex.Message, false);

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

        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_CASP_SR");
        }
        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
        protected void ShowbottomMessage(string Message, bool Success)
        {
            lbl_success.InnerHtml = "";
            lbl_Exception.InnerHtml = "";
            if (Message.Length > 0)
            {
                if (Success)
                    lbl_success.InnerHtml = Message;
                else
                    lbl_Exception.InnerHtml = Message;
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                using (var dbContext = new CyberportEMS_EDM())
                {

                    TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);

                    SPFunctions objSp = new SPFunctions();
                    string AccessUser = lblApplicant.Text.Trim();
                    string CurrentUser = objSp.GetCurrentUser();
                    if (objApp.Created_By.ToLower() != objSp.GetCurrentUser().ToLower() && objApp.Modified_By.ToLower() != objSp.GetCurrentUser().ToLower())
                    {
                        HyperLink hypNavigation = (HyperLink)e.Item.FindControl("hypNavigation");
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        hypNavigation.Enabled = false;
                        lnkAttachmentDelete.Enabled = false;

                    }
                    if (((objApp.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) && (objApp.Status.Replace("_", " ") != formsubmitaction.Saved.ToString().Replace("_", " "))) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                    {
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        lnkAttachmentDelete.Visible = false;
                    }
                }
            }
        }

        private void InitializeUploadsDocument()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);
                    if (objApp != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_Special_Request_ID).ToList();
                        rptrdocuments.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.Supporting_Document.ToString().ToLower());
                        rptrdocuments.DataBind();


                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;

            //Context.Response.Redirect("~/SitePages/Home.aspx", false);
            Context.Response.Redirect("/SitePages/MyReimbursements.aspx");

        }

        protected bool SubmitValidationError()
        {
            bool IsError = false;
            List<string> ErrorLIst = new List<string>();
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int progId = Convert.ToInt32(hdn_ProgramID.Value);
                    SPFunctions objFn = new SPFunctions();
                    string strCurrentUser = objFn.GetCurrentUser();
                    TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);

                    if (objApp != null)
                    {

                        if (string.IsNullOrEmpty(objApp.CASP_ID.ToString()))
                        {
                            IsError = true;
                            ErrorLIst.Add(Localize("Error_AcceleratorProgram"));
                        }
                        if (string.IsNullOrEmpty(objApp.Company_ID.ToString()))
                        {
                            IsError = true;
                            ErrorLIst.Add(Localize("Error_CompanyName"));
                        }

                        //if (string.IsNullOrEmpty(objApp.Contact_Name))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_ContactName"));
                        //}

                        //if (string.IsNullOrEmpty(objApp.Phone_No))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_PhoneNo"));
                        //}
                        //if (string.IsNullOrEmpty(objApp.Email))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_Email"));
                        //}

                        //if (string.IsNullOrEmpty(objApp.Service_Provider_Name))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_ServiceProvider"));
                        //}



                        //if (string.IsNullOrEmpty(objApp.Estimate_Amount.ToString()))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_EstimatedAmount"));
                        //}

                        //if (string.IsNullOrEmpty(objApp.Purpose))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_Purpose"));
                        //}

                        //if (string.IsNullOrEmpty(objApp.Description))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_DescriptionOfService"));
                        //}

                        //if (string.IsNullOrEmpty(objApp.Justification))
                        //{
                        //    IsError = true;
                        //    ErrorLIst.Add(Localize("Error_Justification"));
                        //}

                    }
                }

            }
            catch (Exception ex)
            {

                ErrorLIst.Add(ex.Message);

            }
            //if (IsError == true)
            //{
            lblgrouperror.DataSource = ErrorLIst;
            lblgrouperror.DataBind();
            ShowbottomMessage("", false);
            //}
            //else
            //{
            //    ShowbottomMessage("", true);
            //}
            return IsError;
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

            int errors = check_db_validations(true);
            if (errors == 0)
            {
                if (!SubmitValidationError())
                {
                    UserSubmitPasswordPopup.Visible = true;
                    SPFunctions objFUnction = new SPFunctions();
                    txtLoginUserName.Text = objFUnction.GetCurrentUser();

                }
                else
                {
                    lblgrouperror.Visible = true;
                }
            }
            //enable later ShowHideControlsBasedUponUserData();
        }

        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            UserSubmitPasswordPopup.Visible = false;
        }

        protected void btn_submitFinal_Click(object sender, EventArgs e)
        {


            try
            {
                if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtLoginUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                {
                    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtLoginUserName.Text, txtLoginPassword.Text);

                    if (!status)
                    {
                        UserSubmitPasswordPopup.Visible = true;
                        UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                    }
                    else
                    {

                        using (var dbContext = new CyberportEMS_EDM())
                        {
                           
                            TB_CASP_SPECIAL_REQUEST objApp = GetExistingApplication(dbContext);
                          
                            if (objApp != null)
                            {

                                bool isrequestor = false;
                                SPFunctions objFn = new SPFunctions();
                                string strCurrentUser = objFn.GetCurrentUser();

                                if (objApp.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                {
                                    objApp.Status = formsubmitaction.Resubmitted_information.ToString().Replace("_", " ").Trim();
                                    isrequestor = true;
                                }
                                else
                                {
                                    objApp.Status = formsubmitaction.Submitted.ToString().Trim();
                                    isrequestor = false;
                                }

                                objApp.Submitted_Date = DateTime.Now;
                                objApp.Modified_Date = DateTime.Now;
                                objApp.Modified_By = objFn.GetCurrentUser();
                                dbContext.SaveChanges();

                                string requestor = "";
                                string strEmailContent = "";
                                string strEmailsubject = "";

                              
                                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                                objTbParams = dbContext.TB_SYSTEM_PARAMETER;

                                string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                                WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;

                                string applicationType = "Special_Request_CASP.aspx";
                                string token = "/SitePages/" + applicationType + "?resubmit=Y&app=" + objApp.CASP_Special_Request_ID;

                                if (isrequestor == true)
                                {

                                    List<TB_SCREENING_HISTORY> objTB_SCREENING_HISTORY1 = new List<TB_SCREENING_HISTORY>();
                                    TB_SCREENING_HISTORY objTB_SCREENING_HISTORY = new TB_SCREENING_HISTORY();
                                    objTB_SCREENING_HISTORY1 = dbContext.TB_SCREENING_HISTORY.OrderByDescending(x => x.Created_Date).ToList();
                                    objTB_SCREENING_HISTORY = objTB_SCREENING_HISTORY1.FirstOrDefault(x => x.Application_Number == objApp.Application_No);// && x.Programme_ID == objApp.);

                                    requestor = objTB_SCREENING_HISTORY.Created_By;

                                    strEmailContent = CBPEmail.GetEmailTemplate("CASP_Reimbursement_ReSubmitted_Applicant");

                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", "CASP Special Request");

                                    strEmailsubject = LocalizeCommon("Mail_App_submitted_Requestor").Replace("@@Applicationnumber", objApp.Application_No);

                                    strEmailsubject = strEmailsubject.Replace("@@ProgramName", "CASP Special Request");

                                    int IsEmailSent = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);

                                    TB_SCREENING_HISTORY objScreening = new TB_SCREENING_HISTORY();
                                    objScreening.Application_Number = objApp.Application_No;
                                    objScreening.Programme_ID = Convert.ToInt32(0);
                                    objScreening.Validation_Result = formsubmitaction.Resubmitted_information.ToString().Replace("_", " ");
                                    objScreening.Comment_For_Applicants = " ";
                                    //objScreening.Comment_For_Internal_Use = objScreening.Validation_Result + " By " + objApp.Created_By;
                                    objScreening.Created_By = objApp.Created_By;
                                    objScreening.Created_Date = DateTime.Now;
                                    dbContext.TB_SCREENING_HISTORY.Add(objScreening);
                                    dbContext.SaveChanges();

                                    requestor = objApp.Created_By.ToString();
                                    strEmailContent = CBPEmail.GetEmailTemplate("CASP_Special_Request");
                                    strEmailContent = strEmailContent.Replace("@@Application_No", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objApp.Application_No).Replace("submitted ", "resubmitted");
                                }
                                else
                                {
                                    requestor = objApp.Created_By.ToString();
                                    strEmailContent = CBPEmail.GetEmailTemplate("CASP_Special_Request");
                                    strEmailContent = strEmailContent.Replace("@@Application_No", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objApp.Application_No);
                                    int IsEmailSent = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);


                                }



                                UserSubmitPasswordPopup.Visible = false;
                                pnlsubmissionpopup.Visible = true;
                                if (isrequestor == true)
                                {
                                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Re-submitted");
                                }
                                else
                                {
                                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Submitted");
                                }
                                Fill_Programelist(objApp.Application_No, objApp.TB_COMPANY_PROFILE_BASIC.Company_Name, objApp.Status, objApp.Created_By, objApp.Estimate_Amount.Value, objApp.CASP_Special_Request_ID.ToString());



                              

                            }
                            else
                            {
                                UserSubmitPasswordPopup.Visible = true;
                                UserCustomerrorLogin.InnerHtml = Localize("Releventdata_error");
                            }




                        }
                    }
                }
                else
                {
                    UserSubmitPasswordPopup.Visible = false;
                    pnlsubmissionpopup.Visible = true;
                    lblappsucess.Text = LocalizeCommon("Appication_submission").Replace("@@submit", "Submitted");
                }
            }
            catch (Exception ex)
            {

                UserSubmitPasswordPopup.Visible = true;
                UserCustomerrorLogin.InnerText = ex.Message;
            }
        }

        private void Fill_Programelist(string Application_number, string CompanyName, string status, string Applicant, decimal Amount, string Application_ID)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPWeb site = SPFunctions.GetCurrentWeb)
                    {
                        site.AllowUnsafeUpdates = true;
                        SPList list = site.Lists["Application List CASP Special Request"];

                        SPListItem itemAttachment = GetItemByBdcId(list, Application_number.ToString(), "Application_Number", Applicant, "Applicant");

                        if (itemAttachment == null)
                        {

                            itemAttachment = list.Items.Add();
                        }

                        itemAttachment["Application_Number"] = Application_number;
                        itemAttachment["Programme_Name"] = "CASP Special Request";
                        itemAttachment["CompanyName"] = CompanyName;
                        itemAttachment["Applicant"] = Applicant;

                        itemAttachment["Amount_of_Payment"] = Amount;
                        itemAttachment["Application_ID"] = Application_ID;

                        itemAttachment["Status"] = status;

                        itemAttachment.Update();
                        site.AllowUnsafeUpdates = false;
                    }
                });

            }
            catch (Exception e)
            {
                //UserCustomerrorLogin.InnerText = e.Message;
                throw;
            }
            //finally
            //{
            //    Context.Response.Redirect("~/SitePages/Home.aspx", false);
            //}
        }
        public static SPListItem GetItemByBdcId(SPList list, string bdcIdentity, string IdentityName, string bdcIdentity1, string IdentityName1)
        {
            SPListItem myitem = null;
            foreach (SPListItem item in list.Items)
            {
                if (item[IdentityName].ToString() == bdcIdentity && item[IdentityName1].ToString() == bdcIdentity1)
                {
                    myitem = item;
                }
            }

            return myitem;
        }
    }
}
