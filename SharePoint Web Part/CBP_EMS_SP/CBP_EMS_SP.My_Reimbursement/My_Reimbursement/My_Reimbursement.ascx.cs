using CBP_EMS_SP.Common;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Data.Models;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace CBP_EMS_SP.My_Reimbursement.My_Reimbursement
{
    [ToolboxItemAttribute(false)]
    public partial class My_Reimbursement : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public My_Reimbursement()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        private string _LoggedInUser { get { return new Common.SPFunctions().GetCurrentUser().ToLower(); } }

        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            btn_CPIP_FA_New.Text = Localize("Reimbursement_FA_New_Create_Btn");
            btn_CPIP_SR_New.Text = Localize("Reimbursement_SR_New_Create_Btn");
            btn_CASP_FA_New.Text = Localize("Reimbursement_FA_New_Create_Btn");
            btn_CASP_SR_New.Text = Localize("Reimbursement_SR_New_Create_Btn");
            if (!Page.IsPostBack)
                FillInformation();
        }

        protected void FillInformation()
        {

            //get CPIP FA
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                btn_CASP_FA_New.PostBackUrl = "/SitePages/Financial_Reimbursements_CASP.aspx?new=Y&app=" + Guid.NewGuid().ToString();
                btn_CASP_SR_New.PostBackUrl = "/SitePages/Special_Request_CASP.aspx?new=Y&app=" + Guid.NewGuid().ToString();

                //rptrReimbursementListCPIP.DataSource = dbContext.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.Created_By.ToLower() == _LoggedInUser.ToLower()).ToList();
                //rptrReimbursementListCPIP.DataBind();

                //int objRecount = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.Created_By.ToLower() == _LoggedInUser.ToLower()).Count();
                int objCASPStatus = dbContext.TB_CASP_APPLICATION.Where(x => x.Created_By.ToLower() == _LoggedInUser.ToLower() && x.Status == formsubmitaction.Completed.ToString()).Count();
                if (objCASPStatus > 0)
                {
                    btn_CASP_FA_New.Visible = true;
                    //if (objRecount > 0)
                    btn_CASP_SR_New.Visible = true;
                }

                List<CaspReimburesementList> objCASP = new List<CaspReimburesementList>();

                List<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> objCASPReim = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.Created_By.ToLower() == _LoggedInUser.ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                objCASPReim.ForEach(
                    x => objCASP.Add(
                        new CaspReimburesementList()
                        {
                            CASPType = "re",
                            ApplicationID = x.CASP_FA_ID,
                            ApplicationNo = x.Application_No,
                            Category = x.Category,
                            Company = x.TB_COMPANY_PROFILE_BASIC != null ? x.TB_COMPANY_PROFILE_BASIC.Company_Name : "",
                            Status = x.Status,
                            Submitted = x.Submitted_Date,
                            Created_Date = (DateTime)x.Created_Date
                        }
                        )
                    );
                List<TB_CASP_SPECIAL_REQUEST> objCASPSR = dbContext.TB_CASP_SPECIAL_REQUEST.Where(x => x.Created_By.ToLower() == _LoggedInUser.ToLower() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
                objCASPSR.ForEach(
                   x => objCASP.Add(
                       new CaspReimburesementList()
                       {
                           CASPType = "sr",
                           ApplicationID = x.CASP_Special_Request_ID,
                           ApplicationNo = x.Application_No,
                           Category = "",
                           Company = x.TB_COMPANY_PROFILE_BASIC != null ? x.TB_COMPANY_PROFILE_BASIC.Company_Name : "",
                           Status = x.Status,
                           Submitted = x.Submitted_Date,
                           Created_Date = x.Created_Date
                       }
                       )
                   );
                var CASPList = objCASP.OrderByDescending(x => x.Created_Date);
                rptrReimbursementListCASP.DataSource = CASPList.ToList();
                rptrReimbursementListCASP.DataBind();

            }
        }

        protected void rptrReimbursementListCASP_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton ImgReimbursement = (ImageButton)e.Item.FindControl("ImgReimbursement");
                ImageButton imgbtnDelete = (ImageButton)e.Item.FindControl("imgbtnDelete");


                string item = DataBinder.Eval(e.Item.DataItem, "Status").ToString();

                string type = DataBinder.Eval(e.Item.DataItem, "CASPType").ToString();
                ImgReimbursement.ImageUrl = "/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Edit%20Button.png";

                if (item == "Saved")
                {
                    imgbtnDelete.Visible = true;
                    imgbtnDelete.ImageUrl = "~/_layouts/15/Images/CBP_Images/Internal%20Use-6.5-Delete.png";
                    if (type == "re")
                    {
                        ImgReimbursement.ToolTip = "Edit reimbursement";
                        imgbtnDelete.ToolTip = "Delete riembursement";
                        imgbtnDelete.CommandName = "Delete_re";
                    }
                    else
                    {
                        ImgReimbursement.ToolTip = "Edit Special Request";
                        imgbtnDelete.ToolTip = "Delete Special Request";
                        imgbtnDelete.CommandName = "Delete_Sr";
                    }
                }
                else
                {

                    if (type == "re")
                        ImgReimbursement.ToolTip = "View reimbursement";
                    else ImgReimbursement.ToolTip = "View Special Request";
                }

            }
        }
        protected void rptrReimbursementListCASP_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete_re")
            {
                Guid applicationno = Guid.Parse(e.CommandArgument.ToString());
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objre = new TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT();
                    objre = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.FirstOrDefault(x => x.CASP_FA_ID == applicationno);
                    objre.Status = formsubmitaction.Deleted.ToString();
                    dbContext.SaveChanges();
                    FillInformation();
                }
            }

            if (e.CommandName == "Delete_Sr")
            {
                Guid applicationnosr = Guid.Parse(e.CommandArgument.ToString());
                using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
                {
                    TB_CASP_SPECIAL_REQUEST objsr = new TB_CASP_SPECIAL_REQUEST();
                    objsr = dbContext.TB_CASP_SPECIAL_REQUEST.FirstOrDefault(x => x.CASP_ID == applicationnosr);
                    objsr.Status = formsubmitaction.Deleted.ToString();
                    dbContext.SaveChanges();
                    FillInformation();
                }
            }
        }
    }
}
