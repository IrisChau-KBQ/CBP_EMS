using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using CBP_EMS_SP.Data.Models;

namespace CBP_EMS_SP.CPIP_FA_Reimbursement_Form.VisualWebPart1
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        private string _LoggedInUser { get { return new Common.SPFunctions().GetCurrentUser().ToLower(); } }
        private Guid FAApplicationId
        {
            get
            {
                Guid guidFAApplicationId;
                Guid.TryParse(hdn_FAApplicationID.Value, out guidFAApplicationId);
                return guidFAApplicationId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);

            if (!Page.IsPostBack)
            {

            }
            CBPCommonConstants Commonconst = new CBPCommonConstants();
            List<ListItem> FACategories = Commonconst.Reimbusement_Categories("cpip").Select(x => new ListItem()
            {
                Text = x.Value,
                Value = x.Key
            }).ToList();


            string strCategorySelected = rdo_Categories.SelectedValue;
            rdo_Categories.DataSource = FACategories;
            rdo_Categories.DataTextField = "Text";
            rdo_Categories.DataValueField = "Value";
            rdo_Categories.DataBind();

            if (!string.IsNullOrEmpty(strCategorySelected))
                rdo_Categories.SelectedValue = strCategorySelected;
            else rdo_Categories.SelectedValue = "I";


        }

        protected void rdo_Categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderCategoryUI(rdo_Categories.SelectedValue);
        }
        private void LoadDefaultUIState()
        {
            pnlBankChequePayble.Visible = false;
            pnlEventsAttend.Visible = false;
            pnlslct_Location.Visible = false;
            pnplEventCatBC.Visible = false;
            pnlReimbursementItems.Visible = false;
            pnlReimbursementSalary.Visible = false;
            pnlReimbursementItemTotalInitialGrant.Visible = true;
            pnlReimbursementItemTotal.Visible = false;
        }
        private void renderCategoryUI(string SelectedValue)
        {
            LoadDefaultUIState();
            if (SelectedValue.ToLower() == "a")
            {
                pnlReimbursementItems.Visible = true;
                pnlBankChequePayble.Visible = true;
                pnlReimbursementItemTotalInitialGrant.Visible = false;
                pnlReimbursementItemTotal.Visible = true;
                FillGridReimbursementItems();
            }
            else if (SelectedValue.ToLower() == "b" || SelectedValue.ToLower() == "c")
            {
                pnlReimbursementItems.Visible = true;
                pnlReimbursementItemTotalInitialGrant.Visible = false;
                pnlEventsAttend.Visible = true;
                pnlslct_Location.Visible = true;
                pnplEventCatBC.Visible = true;
                pnlReimbursementItemTotal.Visible = true;
                FillGridReimbursementItems();

            }
            else if (SelectedValue == "d")
            {
                pnlReimbursementItems.Visible = true;
                pnlReimbursementItemTotalInitialGrant.Visible = false;
                pnlReimbursementItemTotal.Visible = true;
                FillGridReimbursementItems();

            }
            else if (SelectedValue.ToLower() == "e")
            {
                pnlReimbursementItemTotal.Visible = true;
            }
        }

        private void GridReimbursementItemsColumnHide(string SelectedValue)
        {


        }
        private void FillGridReimbursementItems()
        {

            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {

                List<TB_FA_REIMBURSEMENT_ITEM> objReimbursement = dbContext.TB_FA_REIMBURSEMENT_ITEM.Where(x => x.FA_Application_ID == FAApplicationId).ToList();
                if (objReimbursement.Count == 0)
                {
                    objReimbursement = new List<TB_FA_REIMBURSEMENT_ITEM>() { new TB_FA_REIMBURSEMENT_ITEM {
                        Date =null,
                        Amount=null
                    } };
                }
                grdReimbursementItems.DataSource = objReimbursement;
                grdReimbursementItems.DataBind();

            }

        }


    }
}
