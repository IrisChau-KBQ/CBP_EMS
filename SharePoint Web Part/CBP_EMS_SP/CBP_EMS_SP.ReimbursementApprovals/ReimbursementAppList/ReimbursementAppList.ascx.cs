using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Common;
using System.Collections.Generic;
using CBP_EMS_SP.Data.Models;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace CBP_EMS_SP.ReimbursementApprovals.ReimbursementAppList
{
    [ToolboxItemAttribute(false)]
    public partial class ReimbursementAppList : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ReimbursementAppList()
        {
        }
        private Dictionary<string, string> ReimburesementCategories { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CBPCommonConstants objCBPCommonConstants = new CBPCommonConstants();
            ReimburesementCategories = objCBPCommonConstants.Reimbusement_Categories("both");
            if (!Page.IsPostBack)
            {

                searchddlReimbursementCategory.DataSource = ReimburesementCategories;
                searchddlReimbursementCategory.DataBind();
                searchddlReimbursementCategory.Items.Insert(0, new ListItem() { Selected = true, Text = "All Category", Value = "" });
                BindInitiData();
                Get_Reimbursement_list();
            }

        }

        protected void BindInitiData()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<string> ApplicationNumbers = new List<string>();

                ApplicationNumbers.AddRange(dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.Status != formsubmitaction.Saved.ToString() && x.Status != formsubmitaction.Deleted.ToString()).ToList()
                    .Select(x => x.Application_No));

                //ApplicationNumbers.AddRange(dbContext.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.Status != formsubmitaction.Saved.ToString() && x.Status != formsubmitaction.Deleted.ToString()).ToList()
                //    .Select(x => x.Application_No));
                searchddlApplicationNumber.DataSource = ApplicationNumbers;
                searchddlApplicationNumber.DataBind();
                searchddlApplicationNumber.Items.Insert(0, new ListItem() { Selected = true, Text = "All Application No.", Value = "" });
            }
        }

        protected void Get_Reimbursement_list()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                string srchAppNumber = searchddlApplicationNumber.SelectedValue;
                string srchStatus = searchddlApplicationStatus.SelectedValue;
                string searchTxt = searchTxtCompany.Text;
                string srchProgType = searchddlProgType.SelectedValue;
                string srchCategory = searchddlReimbursementCategory.SelectedValue.Replace("CASP-", "").Replace("CPIP-", "");
                DateTime? DatesrchDateFrom = null, DatesrchDateTo = null;

                if (!string.IsNullOrEmpty(txtSubmissionFromDate.Text))
                {
                    DatesrchDateFrom = Convert.ToDateTime(txtSubmissionFromDate.Text);
                }

                if (!string.IsNullOrEmpty(txtSubmissionToDate.Text))
                {
                    DatesrchDateTo = Convert.ToDateTime(txtSubmissionToDate.Text);
                }

                SqlParameter param1 = new SqlParameter("@srchAppNumber", srchAppNumber);
                SqlParameter param2 = new SqlParameter("@srchStatus", srchStatus);
                SqlParameter param3 = new SqlParameter("@searchTxt", searchTxt);
                SqlParameter param4 = new SqlParameter("@srchProgType", srchProgType);
                SqlParameter param5 = new SqlParameter("@srchCategory", srchCategory);
                SqlParameter param6 = new SqlParameter("@srchDateFrom", DatesrchDateFrom.HasValue ? DatesrchDateFrom.Value : (object)DBNull.Value);
                SqlParameter param7 = new SqlParameter("@srchDateTo", DatesrchDateTo.HasValue ? DatesrchDateTo.Value : (object)DBNull.Value);
                List<SearchResult> objSearch = dbContext.Database.SqlQuery<SearchResult>("GetSearchReimburesement @srchAppNumber, @srchStatus, @searchTxt, @srchProgType, @srchCategory, @srchDateFrom, @srchDateTo", param1, param2, param3, param4, param5, param6, param7).ToList();


                objSearch.ForEach(x => x.Category = ReimburesementCategories.FirstOrDefault(y => y.Key == x.ApplicationType + "-" + x.Category.ToUpper()).Value);
                GridViewApplication.DataSource = objSearch;
                GridViewApplication.DataBind();
            }

        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Get_Reimbursement_list();
            //using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            //{
            //    string srchAppNumber = searchddlApplicationNumber.SelectedValue;
            //    string srchStatus = searchddlApplicationStatus.SelectedValue;
            //    string searchTxt = searchTxtCompany.Text;
            //    string srchProgType = searchddlProgType.SelectedValue;
            //    string srchCategory = searchddlReimbursementCategory.SelectedValue.Replace("CASP-", "").Replace("CPIP-", "");
            //    DateTime? DatesrchDateFrom = null, DatesrchDateTo = null;

            //    if (!string.IsNullOrEmpty(txtSubmissionFromDate.Text))
            //    {
            //        DatesrchDateFrom = Convert.ToDateTime(txtSubmissionFromDate.Text);
            //    }

            //    if (!string.IsNullOrEmpty(txtSubmissionToDate.Text))
            //    {
            //        DatesrchDateTo = Convert.ToDateTime(txtSubmissionToDate.Text);
            //    }

            //    SqlParameter param1 = new SqlParameter("@srchAppNumber", srchAppNumber);
            //    SqlParameter param2 = new SqlParameter("@srchStatus", srchStatus);
            //    SqlParameter param3 = new SqlParameter("@searchTxt", searchTxt);
            //    SqlParameter param4 = new SqlParameter("@srchProgType", srchProgType);
            //    SqlParameter param5 = new SqlParameter("@srchCategory", srchCategory);
            //    SqlParameter param6 = new SqlParameter("@srchDateFrom", DatesrchDateFrom.HasValue ? DatesrchDateFrom.Value : (object)DBNull.Value);
            //    SqlParameter param7 = new SqlParameter("@srchDateTo", DatesrchDateTo.HasValue ? DatesrchDateTo.Value : (object)DBNull.Value);
            //    List<SearchResult> objSearch = dbContext.Database.SqlQuery<SearchResult>("GetSearchReimburesement @srchAppNumber, @srchStatus, @searchTxt, @srchProgType, @srchCategory, @srchDateFrom, @srchDateTo", param1, param2, param3, param4, param5, param6, param7).ToList();



            //    //List<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> objReimburesementList = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.Status != formsubmitaction.Saved.ToString() && x.Status != formsubmitaction.Deleted.ToString()).ToList();
            //    //List<SearchResult> objSearch = new List<SearchResult>();

            //    objSearch.ForEach(x => x.Category = ReimburesementCategories.FirstOrDefault(y => y.Key == x.ApplicationType + "-" + x.Category.ToUpper()).Value);
            //    //objReimburesementList.ForEach(
            //    //    x => objSearch.Add(new SearchResult()
            //    //    {
            //    //        ApplicationNo = x.Application_No,
            //    //        ApplicationType = "CASP",
            //    //        Category = ReimburesementCategories.FirstOrDefault(y => y.Key == "CASP-" + x.Category.ToUpper()).Value,
            //    //        CompanyName = x.TB_COMPANY_PROFILE_BASIC.Company_Name,
            //    //        FA_ID = x.CASP_FA_ID,
            //    //        Status = x.Status,
            //    //        SubmissionDate = x.Submitted_Date
            //    //    })
            //    //    );

            //    GridViewApplication.DataSource = objSearch;
            //    GridViewApplication.DataBind();




            //}
        }

        protected void GridViewApplication_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkApplicationApproval = (LinkButton)e.Row.FindControl("lnkApplicationApproval");
                SearchResult objResult = (SearchResult)e.Row.DataItem;
                lnkApplicationApproval.PostBackUrl = "/SitePages/" + (objResult.ApplicationType == "CASP" ? "Casp_FA_Internal.aspx?app=" + objResult.FA_ID.ToString() : "CASP_SR_Internal.aspx?app=" + objResult.FA_ID.ToString());
            }
        }
    }

    public class SearchResult
    {
        public Guid FA_ID { get; set; }
        public string ApplicationType { get; set; }
        public string ApplicationNo { get; set; }
        public string CompanyName { get; set; }
        public string Category { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string Status { get; set; }
    }
}
