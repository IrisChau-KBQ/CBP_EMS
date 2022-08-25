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

namespace CBP_EMS_SP.CASP_FA_Reimbursement_Form.VisualWebPart1
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
            if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
            {
                hdn_ApplicationID.Value = hdn_ApplicationID.Value;
            }
            if (string.IsNullOrEmpty(Context.Request.QueryString["app"]))
            {
                Context.Response.Redirect("/SitePages/MyReimbursements.aspx");
            }
            
                if (!Page.IsPostBack)
            {
                //    string strCategorySel = rdo_Categories.SelectedValue;
                CBPCommonConstants objConst = new CBPCommonConstants();
                rdo_Categories.DataSource = objConst.Reimbusement_Categories("casp");
                rdo_Categories.DataTextField = "Value";
                rdo_Categories.DataValueField = "Key";
                rdo_Categories.DataBind();
                FillReimbursementDetails();

            }

            List<ListItem> RdoDeclarationConflict = new List<ListItem>();
            RdoDeclarationConflict.Add(new ListItem() { Value = "false", Text = SPFunctions.LocalizeUI("Rdo_PanelF_Declaration1", "CyberportEMS_CASP_Reimbursement") });
            RdoDeclarationConflict.Add(new ListItem() { Value = "true", Text = SPFunctions.LocalizeUI("Rdo_PanelF_Declaration2", "CyberportEMS_CASP_Reimbursement") });

            string selRdoConflict = rdoFDeclarationofConflicts.SelectedValue;

            rdoFDeclarationofConflicts.DataSource = RdoDeclarationConflict;
            rdoFDeclarationofConflicts.DataTextField = "Text";
            rdoFDeclarationofConflicts.DataValueField = "Value";
            rdoFDeclarationofConflicts.DataBind();
           
           

            if (!string.IsNullOrEmpty(selRdoConflict))
            {
                rdoFDeclarationofConflicts.SelectedValue = selRdoConflict;
            }


            chkDeclareA.Text = SPFunctions.LocalizeUI("chk_DeclareA", "CyberportEMS_CASP_Reimbursement");
            chkDeclareB.Text = SPFunctions.LocalizeUI("chk_DeclareB", "CyberportEMS_CASP_Reimbursement");

            //CBPCommonConstants objConst = new CBPCommonConstants();
            //string strCategorySel = rdo_Categories.SelectedValue;
            //rdo_Categories.DataSource = objConst.Reimbusement_Categories("casp");
            //rdo_Categories.DataTextField = "Value";
            //rdo_Categories.DataValueField = "Key";
            //rdo_Categories.DataBind();

            //if (!string.IsNullOrEmpty(strCategorySel))
            //{
            //    rdo_Categories.SelectedValue = strCategorySel;
            //}
            //else
            //{
            //    rdo_Categories.SelectedValue = "A";
            //    renderCategoryUI("A");
            //}

        }


        protected void FillReimbursementDetails()
        {
            SPFunctions objSp = new SPFunctions();
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);
                bool IsDisabled = false;
                FillUserAttendent(objApp);
                FillUserCompany(objApp);
                FillPreApprovedSR(objApp);
                if (objApp != null)
                {
                    rdo_Categories.SelectedValue = objApp.Category;
                    renderCategoryUI(objApp.Category);
                    if ((objApp.Status.ToLower().Replace(" ", "_") != formsubmitaction.Saved.ToString().ToLower() && objApp.Status.ToLower().Replace(" ", "_") != formsubmitaction.Waiting_for_response_from_applicant.ToString().ToLower())
                        || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                    {
                        DisableControls();
                        IsDisabled = true;
                    }
                    if (objApp.Status.ToLower().Replace(" ", "_") == formsubmitaction.Waiting_for_response_from_applicant.ToString().ToLower())
                    {
                        rdo_Categories.Enabled = false;
                        btn_Save.Enabled = false;
                    }
                    else
                    {

                        rdo_Categories.Enabled = false;
                    }
                    lblApplicant.Text = objApp.Created_By;
                    lblApplicationNo.Text = objApp.Application_No;
                    txtcheque.Text = lblcheque.Text = objApp.Payable_To;
                    if (objApp.Submitted_Date.HasValue)
                    {
                        lblLastSubmitted.Text = objApp.Submitted_Date.Value.ToString("dd MMM yyyy");
                    }

                    chkbx_prepaidservice.Checked = Convert.ToBoolean(objApp.Prepaid_Service);

                    if (objApp.Estimated_Service_From.HasValue)
                        txtServiceperiodfrom.Text = lblServiceperiodfrom.Text = objApp.Estimated_Service_From.Value.ToString("dd/MMM/yyyy");

                    if (objApp.Estimated_Service_To.HasValue)
                        txtServiceperiodTo.Text = lblServiceperiodTo.Text = objApp.Estimated_Service_To.Value.ToString("dd/MMM/yyyy");

                    chkbx_freelance.Checked = Convert.ToBoolean(objApp.Freelancer);
                    txtServiceProviderName.Text = lblServiceProviderName.Text = objApp.Service_Provider_Name;
                    txtServiceContractName.Text = lblServiceContractName.Text = objApp.Service_Contract;
                    if (objApp.Total_Fee.HasValue)
                        txtServiceTotalFee.Text = lblServiceTotalFee.Text = objApp.Total_Fee.Value.ToString("F2");


                    if (objApp.Conflict_of_Interest.HasValue)
                    {
                        rdoFDeclarationofConflicts.SelectedValue = objApp.Conflict_of_Interest.Value.ToString().ToLower();
                    }
                    txtconflicts.Text = lblconflicts.Text = objApp.Conflict_detail;

                    //not for C
                    chkDeclareA.Checked = Convert.ToBoolean(objApp.Declared_A);
                    //only for C
                    chkDeclareB.Checked = Convert.ToBoolean(objApp.Declared_D);

                }
                else
                {
                    rdo_Categories.SelectedIndex = 0;
                    renderCategoryUI("a");
                    lblApplicant.Text = objSp.GetCurrentUser();

                }
                InitializeUploadsDocument();
                FillGridReimbursementItems();
                FillReimburesementSalary();
                if (IsDisabled)
                {
                    DisableGrids();
                }

            }

        }

        protected void FillCalculationDetail(decimal totalAmt)
        {
            if (rdo_Categories.SelectedValue.ToLower() != "c")
            {
                lbl_CalcTotalVal.Text = "$" + totalAmt.ToString("#,#.00");
                lbl_CalcDeductionVal.Text = "$" + ((totalAmt * 25) / 100).ToString("#,#.00");
                lbl_CalcTotalReimbursementVal.Text = "$" + ((totalAmt * 75) / 100).ToString("#,#.00");

            }
            else
            {
                lbl_CalcTotalVal.Text = "$" + totalAmt.ToString("#,#.00");
                lbl_CalcTotalReimbursementVal.Text = lbl_CalcDeductionVal.Text = "$" + ((totalAmt * 50) / 100).ToString("#,#.00");

            }
        }
        protected TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT GetExistingApplication(CyberportEMS_EDM dbContext)
        {
            TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = null;
            List<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> objApps = new List<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();
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
                objApp = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.FirstOrDefault(x => x.CASP_FA_ID == objUserProgramId && (x.Created_By.ToLower() == strCurrentUser.ToLower()) && x.Status != formsubmitaction.Deleted.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                    objUserProgramId = Guid.Parse(hdn_ApplicationID.Value);
                else
                    objUserProgramId = Guid.Parse(Context.Request.QueryString["app"]);

                objApp = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.FirstOrDefault(x => x.CASP_FA_ID == objUserProgramId && x.Status != formsubmitaction.Deleted.ToString());

            }
            if (objApp != null)
            {
                hdn_ApplicationID.Value = objApp.CASP_FA_ID.ToString();
            }
            return objApp;
        }

        protected void rdo_Categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderCategoryUI(rdo_Categories.SelectedValue);

        }

        private void LoadDefaultUIState()
        {
            pnl_programDetail.Visible = true;

        }

        private void renderCategoryUI(string SelectedValue)
        {
            lbl_CalcTotal.Text = Localize("Calc_Total");
            lbl_CalcDeduction.Text = Localize("Calc_Deduction_ABDEF");
            lbl_CalcTotalReimbursement.Text = Localize("Calc_TotalReimbursement_ABDEF");
            ShowHideReimbursementItemColumn(SelectedValue);
            pnl_DocumentsSubsequent.Visible = false;
            if (SelectedValue.ToLower() == "a")
            {

                pnlPreApproveSR.Visible = false;
                pnl_Section2.Visible = false;
                panel_F_Declaration.Visible = false;
                pnl_ReimbursementItems.Visible = true;
                pnl_ReimbursementSalary.Visible = false;
                pnl_DeclareA.Visible = true;
                pnl_DeclareB.Visible = false;

                pnlOriginalReceipt.Visible = true;
                pnlOriginal_Invoice.Visible = true;
                pnlQuotation.Visible = true;
                pnlEventPhoto.Visible = false;
                pnlEventPass.Visible = false;
                pnlPrintSample.Visible = false;
                pnlBoardingPass.Visible = false;
                pnlflight_Inventory.Visible = false;
                pnlBusinessCard.Visible = false;
                pnlInternPayroll.Visible = false;
                pnlInternCertification.Visible = false;
                pnlPaymentProof.Visible = false;
                pnlEmployementContract.Visible = false;
                pnlResume.Visible = false;
                pnlHKIDCard.Visible = false;


            }
            else if (SelectedValue.ToLower() == "b")
            {
                pnlPreApproveSR.Visible = false;
                pnl_Section2.Visible = false;
                panel_F_Declaration.Visible = false;
                pnl_ReimbursementItems.Visible = true;
                pnl_ReimbursementSalary.Visible = false;
                pnl_DeclareA.Visible = true;
                pnl_DeclareB.Visible = false;

                pnlOriginalReceipt.Visible = true;
                pnlOriginal_Invoice.Visible = true;
                pnlQuotation.Visible = true;
                pnlEventPhoto.Visible = false;
                pnlEventPass.Visible = false;
                pnlPrintSample.Visible = false;
                pnlBoardingPass.Visible = false;
                pnlflight_Inventory.Visible = false;
                pnlBusinessCard.Visible = false;
                pnlInternPayroll.Visible = false;
                pnlInternCertification.Visible = false;
                pnlPaymentProof.Visible = false;
                pnlEmployementContract.Visible = false;
                pnlResume.Visible = false;
                pnlHKIDCard.Visible = false;
            }
            else if (SelectedValue.ToLower() == "c")
            {
                lbl_CalcDeduction.Text = Localize("Calc_Deduction_C");
                lbl_CalcTotalReimbursement.Text = Localize("Calc_TotalReimbursement_C");
                pnlPreApproveSR.Visible = false;
                pnl_Section2.Visible = false;
                panel_F_Declaration.Visible = false;
                pnl_ReimbursementItems.Visible = false;
                pnl_ReimbursementSalary.Visible = true;
                pnl_DeclareA.Visible = false;
                pnl_DeclareB.Visible = true;

                pnlOriginalReceipt.Visible = false;
                pnlOriginal_Invoice.Visible = false;
                pnlQuotation.Visible = false;
                pnlEventPhoto.Visible = false;
                pnlEventPass.Visible = false;
                pnlPrintSample.Visible = false;
                pnlBoardingPass.Visible = false;
                pnlflight_Inventory.Visible = false;
                pnlBusinessCard.Visible = false;
                pnlInternPayroll.Visible = true;
                pnlInternCertification.Visible = true;
                pnlPaymentProof.Visible = true;
                pnlEmployementContract.Visible = true;
                pnlResume.Visible = true;
                pnlHKIDCard.Visible = true;
                pnl_DocumentsSubsequent.Visible = true;
            }
            else if (SelectedValue.ToLower() == "d")
            {
                pnlPreApproveSR.Visible = false;
                pnl_Section2.Visible = false;
                panel_F_Declaration.Visible = false;
                pnl_ReimbursementItems.Visible = true;
                pnl_ReimbursementSalary.Visible = false;
                pnl_DeclareA.Visible = true;
                pnl_DeclareB.Visible = false;

                pnlOriginalReceipt.Visible =
                pnlOriginal_Invoice.Visible =
                pnlQuotation.Visible =
                pnlEventPhoto.Visible =
                pnlEventPass.Visible = true;
                pnlPrintSample.Visible = false;
                pnlBoardingPass.Visible = true;
                pnlflight_Inventory.Visible = true;
                pnlBusinessCard.Visible = true;
                pnlInternPayroll.Visible = false;
                pnlInternCertification.Visible = false;
                pnlPaymentProof.Visible = false;
                pnlEmployementContract.Visible = false;
                pnlResume.Visible = false;
                pnlHKIDCard.Visible = false;
            }
            else if (SelectedValue.ToLower() == "e")
            {
                pnlPreApproveSR.Visible = false;
                pnl_Section2.Visible = true;
                panel_F_Declaration.Visible = false;
                pnl_ReimbursementItems.Visible = true;
                pnl_ReimbursementSalary.Visible = false;
                pnl_DeclareA.Visible = true;
                pnl_DeclareB.Visible = false;

                pnlOriginalReceipt.Visible = pnlOriginal_Invoice.Visible =
                pnlQuotation.Visible =
                pnlEventPhoto.Visible =
                pnlEventPass.Visible =
                pnlPrintSample.Visible =
                pnlBoardingPass.Visible =
                pnlflight_Inventory.Visible =
                pnlBusinessCard.Visible = true;
                pnlInternPayroll.Visible = false;
                pnlInternCertification.Visible = false;
                pnlPaymentProof.Visible = false;
                pnlEmployementContract.Visible = false;
                pnlResume.Visible = false;
                pnlHKIDCard.Visible = false;
            }
            else if (SelectedValue.ToLower() == "f")
            {
                pnlPreApproveSR.Visible = true;
                pnl_Section2.Visible =
                panel_F_Declaration.Visible = true;
                pnl_ReimbursementItems.Visible = true;
                pnl_ReimbursementSalary.Visible = false;
                pnl_DeclareA.Visible = true;
                pnl_DeclareB.Visible = false;

                pnlOriginalReceipt.Visible =
                pnlOriginal_Invoice.Visible =
                pnlQuotation.Visible = true;
                pnlEventPhoto.Visible = false;
                pnlEventPass.Visible = false;
                pnlPrintSample.Visible = false;
                pnlBoardingPass.Visible = false;
                pnlflight_Inventory.Visible = false;
                pnlBusinessCard.Visible = false;
                pnlInternPayroll.Visible = false;
                pnlInternCertification.Visible = false;
                pnlPaymentProof.Visible = false;
                pnlEmployementContract.Visible = false;
                pnlResume.Visible = false;
                pnlHKIDCard.Visible = false;
            }
            //if (SelectedValue.ToLower() == "a" || SelectedValue.ToLower() == "b" || SelectedValue.ToLower() == "d" || SelectedValue.ToLower() == "e")
            //{

            //    pnl_ABDE.Visible = true;
            //    //pnl_Reimbursement.Visible = true;
            //    panel_ABDE_Declaration.Visible = true;
            //    panel_F_Declaration.Visible = false;
            //    pnl_FDocument.Visible = false;

            //   // pnl_ReimbursementforC.Visible = false;
            //    panel_CDocuments.Visible = false;
            //    Panel_cfDeclaration.Visible = false;
            //    forf.Visible = false;

            //}

            //if (SelectedValue.ToLower() == "f")
            //{
            //    forf.Visible = true;
            //    pnl_ABDE.Visible = false;
            //    //pnl_Reimbursement.Visible = true;
            //    panel_ABDE_Declaration.Visible = false;
            //    panel_F_Declaration.Visible = true;

            //  //  pnl_ReimbursementforC.Visible = false;
            //    panel_CDocuments.Visible = false;
            //    pnl_FDocument.Visible = true;
            //    Panel_cfDeclaration.Visible = true;
            //}

            //if (SelectedValue.ToLower() == "c")
            //{

            //    pnl_ABDE.Visible = false;
            //  //  pnl_Reimbursement.Visible = false;
            //    panel_ABDE_Declaration.Visible = false;
            //    //pnl_ReimbursementforC.Visible = true;
            //    panel_CDocuments.Visible = true;
            //    Panel_cfDeclaration.Visible = true;
            //    panel_F_Declaration.Visible = false;
            //    pnl_FDocument.Visible = false;
            //    forf.Visible = false;

            //}


        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            check_db_validations(false);
        }


        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_CASP_Reimbursement");
        }




        protected int check_db_validations(bool IsSubmitClick)

        {
            List<String> ErrorLIst = new List<string>();
            using (var dbContext = new CyberportEMS_EDM())
            {


                SPFunctions objfunction = new SPFunctions();
                TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);
                bool isnewobj = false;
                if (objApp == null)
                {
                    isnewobj = true;
                    objApp = new TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT();
                }

                if (string.IsNullOrEmpty(rdo_Categories.SelectedValue))
                    ErrorLIst.Add(Localize("err_CategorySelect"));
                else
                    objApp.Category = rdo_Categories.SelectedValue;

                if (ddlcompanyname.SelectedValue != "")
                {
                    objApp.Company_ID = Guid.Parse(ddlcompanyname.SelectedValue);
                }

                objApp.Payable_To = txtcheque.Text;
                if (ddlProgrammeattended.SelectedValue != "")
                {
                    objApp.CASP_Attended = Guid.Parse(ddlProgrammeattended.SelectedValue);
                }

                if (ddl_preapproved.SelectedValue != "")
                    objApp.Preapproved_SpecialRequest = Guid.Parse(ddl_preapproved.SelectedValue);



                if (rdo_Categories.SelectedValue.ToLower() == "e" || rdo_Categories.SelectedValue.ToLower() == "f")
                {
                    objApp.Prepaid_Service = chkbx_prepaidservice.Checked;
                    if (!string.IsNullOrEmpty(txtServiceperiodfrom.Text))
                        objApp.Estimated_Service_From = Convert.ToDateTime(txtServiceperiodfrom.Text);
                    if (!string.IsNullOrEmpty(txtServiceperiodTo.Text))
                        objApp.Estimated_Service_To = Convert.ToDateTime(txtServiceperiodTo.Text);
                    objApp.Freelancer = chkbx_freelance.Checked;
                    objApp.Service_Provider_Name = txtServiceProviderName.Text;
                    objApp.Service_Contract = txtServiceContractName.Text;

                    if (!string.IsNullOrEmpty(txtServiceTotalFee.Text) && CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtServiceTotalFee.Text))
                        objApp.Total_Fee = Convert.ToDecimal(txtServiceTotalFee.Text);
                    else if (!string.IsNullOrEmpty(txtServiceTotalFee.Text) && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtServiceTotalFee.Text))
                        ErrorLIst.Add(Localize("err_TotalFee"));

                    if (rdo_Categories.SelectedValue.ToLower() == "f")
                    {
                        if (!string.IsNullOrEmpty(rdoFDeclarationofConflicts.SelectedValue))
                        {
                            objApp.Conflict_of_Interest = Convert.ToBoolean(rdoFDeclarationofConflicts.SelectedValue);
                        }
                        objApp.Conflict_detail = txtconflicts.Text;
                    }
                }

                //not for C
                objApp.Declared_A = chkDeclareA.Checked;
                //only for C
                objApp.Declared_D = chkDeclareB.Checked;
                List<TB_FA_REIMBURSEMENT_ITEM> objRemItems = new List<TB_FA_REIMBURSEMENT_ITEM>();
                List<TB_FA_REIMBURSEMENT_SALARY> objRemSalary = new List<TB_FA_REIMBURSEMENT_SALARY>();
                if (rdo_Categories.SelectedValue.ToLower() != "c")
                {
                    objRemItems = GetReimbursementForSave(false, ref ErrorLIst);
                    objApp.Total_Amount = objRemItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount);
                    objApp.Total_Amount_After_Deduction = ((objApp.Total_Amount.Value * 75) / 100);
                }
                else
                {
                    objRemSalary = GetReimbursementSalaryForSave(false, ref ErrorLIst);
                    objApp.Total_Amount = objRemSalary.Where(x => x.Amount.HasValue).Sum(x => x.Amount);
                    objApp.Total_Amount_After_Deduction = ((objApp.Total_Amount.Value * 50) / 100);

                }

                try
                {
                    objApp.Modified_By = objfunction.GetCurrentUser();
                    objApp.Modified_Date = DateTime.Now;
                    if (ErrorLIst.Count == 0)
                    {
                        lblgrouperror.Visible = false;
                        if (isnewobj)
                        {
                            objApp.CASP_FA_ID = NewProgramId();
                            hdn_ApplicationID.Value = objApp.CASP_FA_ID.ToString();
                            var result = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.OrderByDescending(x => x.Application_No).FirstOrDefault();
                            int count = 0;
                            if (result != null)
                            {
                                count = Convert.ToInt32(result.Application_No.Substring(result.Application_No.Length - 4, 4)) + 1;
                            }
                            else
                            {
                                count = 1;
                            }
                            lblApplicationNo.Text = HttpUtility.HtmlEncode("CASP-PR-" + DateTime.Now.ToString("yy") + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                            objApp.Application_No = lblApplicationNo.Text;
                            objApp.Created_By = objfunction.GetCurrentUser();
                            objApp.Status = formsubmitaction.Saved.ToString();
                            objApp.Created_By = objfunction.GetCurrentUser();
                            objApp.Created_Date = DateTime.Now;
                            dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Add(objApp);
                        }
                        else
                        {
                            //objApp.Status = formsubmitaction.Saved.ToString();

                            if (objApp.Status.ToLower().Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                            {
                                objApp.Status = formsubmitaction.Saved.ToString();
                            }

                        }

                        dbContext.SaveChanges();
                        objApp = GetExistingApplication(dbContext);
                        if (rdo_Categories.SelectedValue.ToLower() != "c")
                        {
                            objRemItems.ForEach(x => x.FA_Application_ID = objApp.CASP_FA_ID);
                            IncubationContext.REIMBURSEMENT_ITEM_ADDUPDATE(dbContext, objRemItems, objApp.CASP_FA_ID);
                        }
                        else
                        {
                            objRemSalary.ForEach(x => x.FA_Application_ID = objApp.CASP_FA_ID);
                            IncubationContext.REIMBURSEMENT_SALARY_ADDUPDATE(dbContext, objRemSalary, objApp.CASP_FA_ID);
                        }
                        dbContext.SaveChanges();




                        FillGridReimbursementItems();
                        FillReimburesementSalary();
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

            }
            return ErrorLIst.Count;
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
        private Guid NewProgramId()
        {
            Guid objNewId = Guid.NewGuid();
            while (new CyberportEMS_EDM().TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Where(x => x.CASP_FA_ID == objNewId).Count() == 0)
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
                    TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);//
                    if (objApp == null)
                    {
                        objApp = new TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT();
                        objApp.CASP_FA_ID = NewProgramId();
                        hdn_ApplicationID.Value = objApp.CASP_FA_ID.ToString();

                        int count = 0;
                        var result = dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.OrderByDescending(x => x.Application_No).FirstOrDefault();
                        if (result != null)
                        {
                            count = Convert.ToInt32(result.Application_No.Substring(result.Application_No.Length - 4, 4)) + 1;
                        }
                        else
                        {
                            count = 1;
                        }
                        lblApplicationNo.Text = HttpUtility.HtmlEncode("CASP-PR-" + DateTime.Now.ToString("yy") + "-" + (count <= 9 ? "000" + count.ToString() : (count <= 99 ? "00" + count.ToString() : (count <= 999 ? "0" + count.ToString() : count.ToString()))));
                        objApp.Application_No = lblApplicationNo.Text;
                        objApp.Status = formsubmitaction.Saved.ToString();
                        objApp.Created_By = objfunction.GetCurrentUser();
                        objApp.Created_Date = DateTime.Now;
                        objApp.Modified_By = objfunction.GetCurrentUser();
                        objApp.Modified_Date = DateTime.Now;
                        objApp.Category = rdo_Categories.SelectedValue;
                        objApp.Freelancer = false;
                        objApp.Prepaid_Service = false;
                        dbContext.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT.Add(objApp);
                        dbContext.SaveChanges();
                        objApp = GetExistingApplication(dbContext);
                    }
                    if (objApp != null)
                    {


                        SPFunctions objSPFunctions = new SPFunctions();
                        string _fileUrl = string.Empty;
                        string ProgramName = "CASP Financial Assistance Reimbursement";
                        int CountOfAttachment = 0;
                        switch (Convert.ToInt32(argument))
                        {
                            case 1:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_Original_Receipt.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_OriginalReceipterr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_OriginalReceipt.HasFile)
                                {
                                    if (Fu_OriginalReceipt.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_OriginalReceipt.FileName.Remove(0, Fu_OriginalReceipt.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_OriginalReceipt, enumAttachmentType.FA_Original_Receipt, Convert.ToString(CountOfAttachment + 1), "Receipt");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_Original_Receipt, objApp.CASP_FA_ID, 0, true);
                                            Fu_OriginalReceipterr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_OriginalReceipterr.Text = Localize("File_Type_image");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_OriginalReceipterr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_OriginalReceipterr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 2:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_Original_Invoice.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_Original_Invoiceerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_Original_Invoice.HasFile)
                                {
                                    if (Fu_Original_Invoice.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_Original_Invoice.FileName.Remove(0, Fu_Original_Invoice.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_Original_Invoice, enumAttachmentType.FA_Original_Invoice, Convert.ToString(CountOfAttachment + 1), "Invoice");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_Original_Invoice, objApp.CASP_FA_ID, 0, true);
                                            Fu_Original_Invoiceerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_Original_Invoiceerr.Text = Localize("File_Type_image");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_Original_Invoiceerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_Original_Invoiceerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 3:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_Quotation.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_Quotationerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_Quotation.HasFile)
                                {
                                    if (Fu_Quotation.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_Quotation.FileName.Remove(0, Fu_Quotation.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_Quotation, enumAttachmentType.FA_Quotation, Convert.ToString(CountOfAttachment + 1), "Quote");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_Quotation, objApp.CASP_FA_ID, 0, true);
                                            Fu_Quotationerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_Quotationerr.Text = Localize("File_Type_image");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_Quotationerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_Quotationerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 4:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_EventPhoto.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_EventPhotoerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_EventPhoto.HasFile)
                                {
                                    if (Fu_EventPhoto.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_EventPhoto.FileName.Remove(0, Fu_EventPhoto.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_EventPhoto, enumAttachmentType.FA_EventPhoto, Convert.ToString(CountOfAttachment + 1), "Photo");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_EventPhoto, objApp.CASP_FA_ID, 0, true);
                                            Fu_EventPhotoerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_EventPhotoerr.Text = Localize("File_Type_image");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_EventPhotoerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_EventPhotoerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 5:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_EventPass.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_EventPasserr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_EventPass.HasFile)
                                {
                                    if (Fu_EventPass.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_EventPass.FileName.Remove(0, Fu_EventPass.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                             Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                             Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_EventPass, enumAttachmentType.FA_EventPass, Convert.ToString(CountOfAttachment + 1), "Epass");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_EventPass, objApp.CASP_FA_ID, 0, true);
                                            Fu_EventPasserr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_EventPasserr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_EventPasserr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_EventPasserr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 6:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_PrintSample.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_PrintSampleerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_PrintSample.HasFile)
                                {
                                    if (Fu_PrintSample.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_PrintSample.FileName.Remove(0, Fu_PrintSample.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                             Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                             Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_PrintSample, enumAttachmentType.FA_PrintSample, Convert.ToString(CountOfAttachment + 1), "MTLSample");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_PrintSample, objApp.CASP_FA_ID, 0, true);
                                            Fu_PrintSampleerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_PrintSampleerr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_PrintSampleerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_PrintSampleerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 7:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_BoardingPass.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_BoardingPasserr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_BoardingPass.HasFile)
                                {
                                    if (Fu_BoardingPass.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_BoardingPass.FileName.Remove(0, Fu_BoardingPass.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                             Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                             Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_BoardingPass, enumAttachmentType.FA_BoardingPass, Convert.ToString(CountOfAttachment + 1), "BPass");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_BoardingPass, objApp.CASP_FA_ID, 0, true);
                                            Fu_BoardingPasserr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_BoardingPasserr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_BoardingPasserr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_BoardingPasserr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 8:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_flight_Inventory.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    fu_flight_Inventoryerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (fu_flight_Inventory.HasFile)
                                {
                                    if (fu_flight_Inventory.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = fu_flight_Inventory.FileName.Remove(0, fu_flight_Inventory.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                              Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                              Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, fu_flight_Inventory, enumAttachmentType.FA_flight_Inventory, Convert.ToString(CountOfAttachment + 1), "Itinerary");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_flight_Inventory, objApp.CASP_FA_ID, 0, true);
                                            fu_flight_Inventoryerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            fu_flight_Inventoryerr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        fu_flight_Inventoryerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    fu_flight_Inventoryerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 9:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_BusinessCard.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_BusinessCarderr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_BusinessCard.HasFile)
                                {
                                    if (Fu_BusinessCard.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_BusinessCard.FileName.Remove(0, Fu_BusinessCard.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                             Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                             Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_BusinessCard, enumAttachmentType.FA_BusinessCard, Convert.ToString(CountOfAttachment + 1), "SupportDoc");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_BusinessCard, objApp.CASP_FA_ID, 0, true);
                                            Fu_BusinessCarderr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_BusinessCarderr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_BusinessCarderr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_BusinessCarderr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 10:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_Intern_Payroll.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_InternPayrollerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_InternPayroll.HasFile)
                                {
                                    if (Fu_InternPayroll.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_InternPayroll.FileName.Remove(0, Fu_InternPayroll.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                             Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                             Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_InternPayroll, enumAttachmentType.FA_Intern_Payroll, Convert.ToString(CountOfAttachment + 1), "Payroll");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_Intern_Payroll, objApp.CASP_FA_ID, 0, true);
                                            Fu_InternPayrollerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_InternPayrollerr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_InternPayrollerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_InternPayrollerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 11:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_Intern_Academic_Certification.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_InternCertificationerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_InternCertification.HasFile)
                                {
                                    if (Fu_InternCertification.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_InternCertification.FileName.Remove(0, Fu_InternCertification.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                               Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                               Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_InternCertification, enumAttachmentType.FA_Intern_Academic_Certification, Convert.ToString(CountOfAttachment + 1), "Cert");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_Intern_Academic_Certification, objApp.CASP_FA_ID, 0, true);
                                            Fu_InternCertificationerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_InternCertificationerr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_InternCertificationerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_InternCertificationerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 12:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_MPF_PaymentProof.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_PaymentProoferr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_PaymentProof.HasFile)
                                {
                                    if (Fu_PaymentProof.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_PaymentProof.FileName.Remove(0, Fu_PaymentProof.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                              Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                              Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_PaymentProof, enumAttachmentType.FA_MPF_PaymentProof, Convert.ToString(CountOfAttachment + 1), "MPF");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_MPF_PaymentProof, objApp.CASP_FA_ID, 0, true);
                                            Fu_PaymentProoferr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_PaymentProoferr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_PaymentProoferr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_PaymentProoferr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 13:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_Employement_Contract.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_EmployementContracterr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_EmployementContract.HasFile)
                                {
                                    if (Fu_EmployementContract.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_EmployementContract.FileName.Remove(0, Fu_EmployementContract.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                             Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                             Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_EmployementContract, enumAttachmentType.FA_Employement_Contract, Convert.ToString(CountOfAttachment + 1), "Contract");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_Employement_Contract, objApp.CASP_FA_ID, 0, true);
                                            Fu_EmployementContracterr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_EmployementContracterr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_EmployementContracterr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_EmployementContracterr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 14:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_ResumeCV.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_Resumeerr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_Resume.HasFile)
                                {
                                    if (Fu_Resume.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_Resume.FileName.Remove(0, Fu_Resume.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                              Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                              Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_Resume, enumAttachmentType.FA_ResumeCV, Convert.ToString(CountOfAttachment + 1), "CV");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_ResumeCV, objApp.CASP_FA_ID, 0, true);
                                            Fu_Resumeerr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_Resumeerr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_Resumeerr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_Resumeerr.Text = Localize("Error_file_upload");
                                }
                                break;
                            case 15:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_HKID_Card.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    Fu_HKIDCarderr.Text = Localize("err_FileUploadCount");
                                    break;
                                }
                                if (Fu_HKIDCard.HasFile)
                                {
                                    if (Fu_HKIDCard.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_HKIDCard.FileName.Remove(0, Fu_HKIDCard.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                              Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                              Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_HKIDCard, enumAttachmentType.FA_HKID_Card, Convert.ToString(CountOfAttachment + 1), "HKID");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_HKID_Card, objApp.CASP_FA_ID, 0, true);
                                            Fu_HKIDCarderr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            Fu_HKIDCarderr.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        Fu_HKIDCarderr.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    Fu_HKIDCarderr.Text = Localize("Error_file_upload");
                                }
                                break;

                            case 16:
                                if (Fu_FreelanceResume.HasFile)
                                {

                                    if (Fu_FreelanceResume.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_FreelanceResume.FileName.Remove(0, Fu_FreelanceResume.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                            Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                            Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            string ProgrameName = "CASP-FA-Resume";
                                            string Version_Number = "1";

                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgrameName,
                                    Fu_FreelanceResume, enumAttachmentType.FA_FreelancerResumeCV, Version_Number, "FCV");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_FreelancerResumeCV, objApp.CASP_FA_ID);
                                            lblResumeError.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lblResumeError.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lblResumeError.Text = Localize("File_size");
                                    }

                                }
                                else
                                {
                                    IsError = true;
                                    lblResumeError.Text = Localize("Error_file_upload");
                                }
                                break;

                            case 17:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_EmpPayslip.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    lblEmpPayslip.Text = Localize("err_FileUploadCount");
                                    break;
                                }


                                if (Fu_EmpPayslip.HasFile)
                                {

                                    if (Fu_EmpPayslip.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_EmpPayslip.FileName.Remove(0, Fu_EmpPayslip.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                              Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                              Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_EmpPayslip, enumAttachmentType.FA_EmpPayslip, Convert.ToString(CountOfAttachment + 1), "Payslip");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_EmpPayslip, objApp.CASP_FA_ID, 0, true);
                                            lblEmpPayslip.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lblEmpPayslip.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lblEmpPayslip.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    lblEmpPayslip.Text = Localize("Error_file_upload");
                                }
                                break;

                            case 18:
                                CountOfAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID && x.Attachment_Type == enumAttachmentType.FA_SalaryProof.ToString()).Count();
                                if (CountOfAttachment > 3)
                                {
                                    IsError = true;
                                    lblSalaryProofError.Text = Localize("err_FileUploadCount");
                                    break;
                                }

                                if (Fu_SalaryProof.HasFile)
                                {

                                    if (Fu_SalaryProof.PostedFile.ContentLength <= (5 * 1024 * 1024))
                                    {
                                        string Extension = Fu_SalaryProof.FileName.Remove(0, Fu_SalaryProof.FileName.LastIndexOf(".") + 1);
                                        if (Extension.ToLower() == "pdf" || Extension.ToLower() == "doc" || Extension.ToLower() == "docx" || Extension.ToLower() == "xls" ||
                                              Extension.ToLower() == "xlsx" || Extension.ToLower() == "ppt" || Extension.ToLower() == "pptx" || Extension.ToLower() == "png" ||
                                              Extension.ToLower() == "jpg" || Extension.ToLower() == "gif")
                                        {
                                            _fileUrl = objSPFunctions.FA_AttachmentSave(objApp.Application_No, ProgramName, Fu_SalaryProof, enumAttachmentType.FA_SalaryProof, Convert.ToString(CountOfAttachment + 1), "SalAdjust");
                                            SaveAttachmentUrl(_fileUrl, enumAttachmentType.FA_SalaryProof, objApp.CASP_FA_ID, 0, true);
                                            Fu_HKIDCarderr.Text = "";
                                            InitializeUploadsDocument();
                                        }
                                        else
                                        {
                                            IsError = true;
                                            lblSalaryProofError.Text = Localize("File_type");
                                        }
                                    }
                                    else
                                    {
                                        IsError = true;
                                        lblSalaryProofError.Text = Localize("File_size");
                                    }
                                }
                                else
                                {
                                    IsError = true;
                                    lblSalaryProofError.Text = Localize("Error_file_upload");
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
        private void InitializeUploadsDocument()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {

                    TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);
                    if (objApp != null)
                    {
                        List<TB_APPLICATION_ATTACHMENT> attachments = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID).ToList();
                        rptrOriginalReceipt.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_Original_Receipt.ToString().ToLower());
                        rptrOriginalReceipt.DataBind();
                        rptrOriginal_Invoice.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_Original_Invoice
                        .ToString().ToLower());
                        rptrOriginal_Invoice.DataBind();
                        rptrQuotation.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_Quotation
                        .ToString().ToLower());
                        rptrQuotation.DataBind();
                        rptrFu_EventPhoto.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_EventPhoto
                        .ToString().ToLower());
                        rptrFu_EventPhoto.DataBind();
                        rptrFu_EventPass.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_EventPass
                                               .ToString().ToLower());
                        rptrFu_EventPass.DataBind();
                        rptrFu_PrintSample.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_PrintSample
                                               .ToString().ToLower());
                        rptrFu_PrintSample.DataBind();

                        rptrFu_BoardingPass.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_BoardingPass
                                             .ToString().ToLower());
                        rptrFu_BoardingPass.DataBind();
                        rptrfu_flight_Inventory.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_flight_Inventory
                                           .ToString().ToLower());
                        rptrfu_flight_Inventory.DataBind();

                        rptrFu_BusinessCard.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_BusinessCard
                                           .ToString().ToLower());
                        rptrFu_BusinessCard.DataBind();
                        rptrFu_InternPayroll.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_Intern_Payroll
                                          .ToString().ToLower());
                        rptrFu_InternPayroll.DataBind();
                        rptrFu_InternCertification.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_Intern_Academic_Certification
                                        .ToString().ToLower());
                        rptrFu_InternCertification.DataBind();

                        rptrFu_PaymentProof.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_MPF_PaymentProof
                                       .ToString().ToLower());
                        rptrFu_PaymentProof.DataBind();
                        rptrFu_EmployementContract.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_Employement_Contract
                                      .ToString().ToLower());
                        rptrFu_EmployementContract.DataBind();
                        rptrFu_Resume.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_ResumeCV
                                      .ToString().ToLower());
                        rptrFu_Resume.DataBind();

                        rptrFu_HKIDCard.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_HKID_Card
                                     .ToString().ToLower());
                        rptrFu_HKIDCard.DataBind();


                        rptr_Resume.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_FreelancerResumeCV
                                       .ToString().ToLower());
                        rptr_Resume.DataBind();

                        rptrFu_EmpPayslip.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_EmpPayslip
                                       .ToString().ToLower());
                        rptrFu_EmpPayslip.DataBind();
                        rptrFu_SalaryProof.DataSource = attachments.Where(x => x.Attachment_Type.ToLower() == enumAttachmentType.FA_SalaryProof
                                       .ToString().ToLower());
                        rptrFu_SalaryProof.DataBind();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        public bool SaveAttachmentUrl(string Url, enumAttachmentType objAttachmentType, Guid appId, int progId = 0, bool AllowMultiple = false)
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
                IncubationContext.TB_APPLICATION_ATTACHMENTADDUPDATE(dbContext, objAttach, AllowMultiple);
                dbContext.SaveChanges();
                return true;
            }

        }

        protected void rptrOtherAttachement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);
                    SPFunctions objSp = new SPFunctions();
                    string AccessUser = lblApplicant.Text.Trim();
                    string CurrentUser = objSp.GetCurrentUser();

                    if (objApp.Status.ToLower().Replace(" ", "_") != formsubmitaction.Saved.ToString().ToLower() && objApp.Status.ToLower().Replace(" ", "_") != formsubmitaction.Waiting_for_response_from_applicant.ToString().ToLower())
                    {
                        LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                        lnkAttachmentDelete.Visible = false;
                    }

                    //if (objApp.Created_By.ToLower() != objSp.GetCurrentUser().ToLower() && objApp.Modified_By.ToLower() != objSp.GetCurrentUser().ToLower())
                    //{
                    //    HyperLink hypNavigation = (HyperLink)e.Item.FindControl("hypNavigation");
                    //    LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                    //    hypNavigation.Enabled = false;
                    //    lnkAttachmentDelete.Enabled = false;
                    //}
                    //if (((objApp.Status.Replace("_", " ") != formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ")) && (objApp.Status.Replace("_", " ") != formsubmitaction.Saved.ToString().Replace("_", " "))) || !objSp.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup))
                    //{
                    //    LinkButton lnkAttachmentDelete = (LinkButton)e.Item.FindControl("lnkAttachmentDelete");
                    //    lnkAttachmentDelete.Visible = false;
                    //}


                }
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


        private List<TB_FA_REIMBURSEMENT_SALARY> GetReimbursementSalaryForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            List<TB_FA_REIMBURSEMENT_SALARY> objsalary = new List<TB_FA_REIMBURSEMENT_SALARY>();
            for (int i = 0; i < grdReimbursementSalary.Rows.Count; i++)
            {
                string titleerror = "Reimbursement salary" + (i + 1) + " : ";

                try
                {

                    HiddenField hdn_Item_ID = (HiddenField)grdReimbursementSalary.Rows[i].Cells[0].FindControl("hdn_Item_ID");
                    TextBox txtname = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtname");
                    TextBox txtmonthlysalary = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtmonthlysalary");
                    TextBox txtsalaryfrom = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtsalaryfrom");
                    TextBox txtsalaryto = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtsalaryto");
                    TextBox txtTax = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtTax");
                    TextBox txtAmount = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtAmount");
                    if (txtname.Text != "" || txtmonthlysalary.Text != "" || txtsalaryfrom.Text != "" || txtsalaryto.Text != "" || txtTax.Text != "" || txtTax.Text != "")
                    {
                        TB_FA_REIMBURSEMENT_SALARY objInternSalary = new TB_FA_REIMBURSEMENT_SALARY();
                        objInternSalary.Salary_ID = Guid.Parse(hdn_Item_ID.Value);

                        if ((txtname.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtname.Text))
                            || (IsSubmitClick && txtname.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtname.Text)))
                            objerror.Add(titleerror + "Intern's Name Required");
                        else objInternSalary.Intern_Name = txtname.Text;

                        if (txtsalaryfrom.Text != "")
                        {
                            DateTime dDate;

                            if (DateTime.TryParse(txtsalaryfrom.Text, out dDate))
                            {
                                String.Format("{0:M-yy}", dDate);
                                objInternSalary.Period_From = Convert.ToDateTime(txtsalaryfrom.Text.Trim());

                            }
                            else
                            {
                                objerror.Add(titleerror + " Invalid Monthly Salary From"); // <-- Control flow goes here
                            }
                        }
                        if (txtsalaryto.Text != "")
                        {
                            DateTime dDate;

                            if (DateTime.TryParse(txtsalaryto.Text, out dDate))
                            {
                                String.Format("{0:M-yy}", dDate);
                                objInternSalary.Period_To = Convert.ToDateTime(txtsalaryto.Text.Trim());

                            }
                            else
                            {
                                objerror.Add(titleerror + " Invalid Monthly Salary To"); // <-- Control flow goes here

                            }
                        }


                        if ((txtmonthlysalary.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtmonthlysalary.Text))
                            || (IsSubmitClick && txtmonthlysalary.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtmonthlysalary.Text)))
                            objerror.Add(titleerror + " Invalid Monthly Salary"); // <-- Control flow goes here
                        else if (Convert.ToDouble(txtmonthlysalary.Text.Trim()) > 9000)
                            objerror.Add(titleerror + " Maximum allow salary is 9,000.00");
                        else if (txtmonthlysalary.Text.Length > 0)
                            objInternSalary.Monthly_Salary = Convert.ToDecimal(txtmonthlysalary.Text);


                        if ((txtTax.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtTax.Text))
                            || (IsSubmitClick && txtTax.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtTax.Text)))
                            objerror.Add(titleerror + " Invalid Tax"); // <-- Control flow goes here
                        else if (Convert.ToDouble(txtTax.Text.Trim()) > 450)
                            objerror.Add(titleerror + " Maximum allow MPF is 450.00");
                        else if (txtTax.Text.Length > 0)
                            objInternSalary.MPF = Convert.ToDecimal(txtTax.Text);


                        if ((txtAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmount.Text))
                            || (IsSubmitClick && txtAmount.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmount.Text)))
                            objerror.Add(titleerror + " Invalid Total Amount"); // <-- Control flow goes here

                        else if (txtAmount.Text.Length > 0)
                            objInternSalary.Amount = Convert.ToDecimal(txtAmount.Text);


                        objInternSalary.Created_Date = DateTime.Now;
                        objInternSalary.Modified_Date = DateTime.Now;
                        SPFunctions objFUnction = new SPFunctions();
                        objInternSalary.Created_By = objFUnction.GetCurrentUser();
                        objInternSalary.Modified_By = objFUnction.GetCurrentUser();


                        objsalary.Add(objInternSalary);
                    }
                }
                catch (Exception ex)
                {
                    objerror.Add(titleerror + ex.Message);
                }
            }

            return objsalary;


        }

        private List<TB_FA_REIMBURSEMENT_ITEM> GetReimbursementForSave(bool IsSubmitClick, ref List<string> objerror)
        {
            List<TB_FA_REIMBURSEMENT_ITEM> objitem = new List<TB_FA_REIMBURSEMENT_ITEM>();
            for (int i = 0; i < grdReimbursementItems.Rows.Count; i++)
            {
                string titleerror = "Reimbursement Item" + (i + 1) + " : ";

                try
                {

                    HiddenField hdn_Item_ID = (HiddenField)grdReimbursementItems.Rows[i].Cells[0].FindControl("hdn_Item_ID");
                    TextBox txtAmount = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txtAmount");
                    TextBox txtItemDescription = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txtItemDescription");
                    TextBox txt_Date = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txt_Date");
                    CheckBox chkAdvertisement = (CheckBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("chkAdvertisement");
                    if (txt_Date.Text != "" || txtItemDescription.Text != "" || txtAmount.Text != "")
                    {

                        TB_FA_REIMBURSEMENT_ITEM objreimburseitem = new TB_FA_REIMBURSEMENT_ITEM();
                        objreimburseitem.Item_ID = Guid.Parse(hdn_Item_ID.Value);
                        //objreimburseitem.FA_Application_ID = Guid.Parse(hdn_ApplicationID.Value);

                        if ((txtItemDescription.Text.Length > 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtItemDescription.Text))
                            || (IsSubmitClick && txtItemDescription.Text.Length == 0 && !CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 255, true, AllowAllSymbol: true), txtItemDescription.Text)))
                            objerror.Add(titleerror + "Item/Description should not be greater then 255 char");
                        else objreimburseitem.Description = txtItemDescription.Text;


                        if (txt_Date.Text != "")
                        {
                            DateTime dDate;

                            if (DateTime.TryParse(txt_Date.Text, out dDate))
                            {
                                String.Format("{0:M-yy}", dDate);
                                objreimburseitem.Date = Convert.ToDateTime(txt_Date.Text.Trim());

                            }
                            else
                            {
                                objerror.Add(titleerror + "Invalid date"); // <-- Control flow goes here
                            }
                        }
                        if ((txtAmount.Text.Length > 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmount.Text))
                            || (IsSubmitClick && txtAmount.Text.Length == 0 && !CBPRegularExpression.RegExValidate(@"^(?=.*\d)\d*(?:\.\d\d)?$", txtAmount.Text)))
                            objerror.Add(titleerror + "Invalid Amount");
                        else if (txtAmount.Text.Length > 0)
                            objreimburseitem.Amount = Convert.ToDecimal(txtAmount.Text);



                        objreimburseitem.Created_Date = DateTime.Now;
                        objreimburseitem.Modified_Date = DateTime.Now;
                        SPFunctions objFUnction = new SPFunctions();
                        objreimburseitem.Created_By = objFUnction.GetCurrentUser();
                        objreimburseitem.Modified_By = objFUnction.GetCurrentUser();

                        if (rdo_Categories.SelectedValue != "f")
                            objreimburseitem.Advertisement = chkAdvertisement.Checked;


                        objitem.Add(objreimburseitem);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return objitem;


        }

        protected void FillUserCompany(TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp)
        {

            IncubationContext objcomp = new IncubationContext();
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();
            if (!IsApplicantUser && objApp != null)
                strCurrentUser = objApp.Created_By;
            List<CASP_CompanyList> objCompanyList = IncubationContext.GetCompanyForUserCASP(strCurrentUser);
            ddlcompanyname.DataSource = objCompanyList;
            ddlcompanyname.DataTextField = "CompanyName";
            ddlcompanyname.DataValueField = "CompanyIdNumber";
            ddlcompanyname.DataBind();
            ddlcompanyname.Items.Insert(0, new ListItem() { Text = "Select Company", Value = "" });
            if (objApp != null)
            {
                if (objApp.Company_ID.HasValue)
                    ddlcompanyname.SelectedValue = Convert.ToString(objApp.Company_ID.Value);
            }
        }

        protected void FillUserAttendent(TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp)
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                if (!IsApplicantUser && objApp != null)
                    strCurrentUser = objApp.Created_By;

                List<CASP_Programme_Attended> applicantprogram = (from a in dbContext.TB_CASP_APPLICATION.Where(x => x.Applicant == strCurrentUser && (x.Status != "Saved" || x.Status != "Deleted")) from b in dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == a.Programme_ID) select new CASP_Programme_Attended() { CASP_ID = a.CASP_ID, Programme_Name = a.Accelerator_Name }).ToList();

                ddlProgrammeattended.DataSource = applicantprogram;
                ddlProgrammeattended.DataTextField = "Programme_Name";
                ddlProgrammeattended.DataValueField = "CASP_ID";
                ddlProgrammeattended.DataBind();
                ddlProgrammeattended.Items.Insert(0, new ListItem() { Text = "Select Programme", Value = "" });
                if (objApp != null)
                {
                    if (objApp.CASP_Attended.HasValue)
                        ddlProgrammeattended.SelectedValue = Convert.ToString(objApp.CASP_Attended);
                }
            }

        }

        protected void FillPreApprovedSR(TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp)
        {

            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                SPFunctions objFUnction = new SPFunctions();
                string strCurrentUser = objFUnction.GetCurrentUser();
                if (!IsApplicantUser && objApp != null)
                    strCurrentUser = objApp.Created_By;

                List<TB_CASP_SPECIAL_REQUEST> applicantprogram = dbContext.TB_CASP_SPECIAL_REQUEST.Where(x => x.Created_By.ToLower() == strCurrentUser.ToLower() && (x.Status == "Submitted" || x.Status == "Completed")).ToList();

                ddl_preapproved.DataSource = applicantprogram;
                ddl_preapproved.DataTextField = "Application_No";
                ddl_preapproved.DataValueField = "CASP_Special_Request_ID";
                ddl_preapproved.DataBind();
                ddl_preapproved.Items.Insert(0, new ListItem() { Text = "Select Special Request", Value = "" });
                if (objApp != null)
                {
                    if (objApp.Preapproved_SpecialRequest.HasValue)
                        ddl_preapproved.SelectedValue = Convert.ToString(objApp.Preapproved_SpecialRequest);
                }
            }

        }
        private void ShowHideReimbursementItemColumn(string SelectedValue)
        {

            if (SelectedValue.ToLower() == "e")
            {
                grdReimbursementItems.Columns[2].Visible = true;
            }
            else
                grdReimbursementItems.Columns[2].Visible = false;


        }
        private void FillGridReimbursementItems()
        {


            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<TB_FA_REIMBURSEMENT_ITEM> objReimbursement = new List<TB_FA_REIMBURSEMENT_ITEM>();
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                {
                    Guid AppId = Guid.Parse(hdn_ApplicationID.Value);
                    objReimbursement = dbContext.TB_FA_REIMBURSEMENT_ITEM.Where(x => x.FA_Application_ID == AppId).OrderBy(x => x.Created_Date).ToList();

                }
                if (objReimbursement.Count == 0)
                {
                    objReimbursement = new List<TB_FA_REIMBURSEMENT_ITEM>() { new TB_FA_REIMBURSEMENT_ITEM {
                        Item_ID =default(Guid),
                        Date =null,
                        Amount=null
                    } };
                }
                if (rdo_Categories.SelectedValue.ToLower() != "c")
                {
                    decimal? total = objReimbursement.Where(x => x.Amount.HasValue).Select(x => x.Amount).Sum();
                    FillCalculationDetail(total.HasValue ? total.Value : 0);
                }
                grdReimbursementItems.DataSource = objReimbursement;
                grdReimbursementItems.DataBind();

            }

        }

        protected void grdReimbursementItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


            }

        }


        protected void btn_AddReimbursementItems_Click(object sender, ImageClickEventArgs e)
        {

            bool IsError = false;
            lblgriderror.Text = string.Empty;
            List<TB_FA_REIMBURSEMENT_ITEM> objFaItems = new List<TB_FA_REIMBURSEMENT_ITEM>();
            for (int i = 0; i < grdReimbursementItems.Rows.Count; i++)
            {
                try
                {
                    HiddenField hdn_Item_ID = (HiddenField)grdReimbursementItems.Rows[i].Cells[0].FindControl("hdn_Item_ID");
                    TextBox txtAmount = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txtAmount");
                    TextBox txt_Date = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txt_Date");
                    TextBox txtItemDescription = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txtItemDescription");
                    CheckBox chkAdvertisement = (CheckBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("chkAdvertisement");
                    TB_FA_REIMBURSEMENT_ITEM objItem = new TB_FA_REIMBURSEMENT_ITEM();
                    objItem.Item_ID = Guid.Parse(hdn_Item_ID.Value);
                    objItem.Amount = Convert.ToDecimal(txtAmount.Text);
                    objItem.Description = txtItemDescription.Text;
                    if (txt_Date.Text != "")
                    {
                        objItem.Date = Convert.ToDateTime(txt_Date.Text.Trim());
                    }

                    objItem.Advertisement = chkAdvertisement.Checked;
                    objFaItems.Add(objItem);
                }
                catch (Exception ex)
                {
                    IsError = true;
                    lblgriderror.Text = ex.Message;
                }
            }
            if (IsError == false)
            {
                objFaItems.Add(new TB_FA_REIMBURSEMENT_ITEM
                {
                    Item_ID = default(Guid),
                    Date = null,
                    Amount = null
                });
                if (rdo_Categories.SelectedValue.ToLower() != "c")
                {
                    decimal? total = objFaItems.Where(x => x.Amount.HasValue).Select(x => x.Amount).Sum();
                    FillCalculationDetail(total.HasValue ? total.Value : 0);
                }
                grdReimbursementItems.DataSource = objFaItems;
                grdReimbursementItems.DataBind();

            }
        }

        protected void grdReimbursementItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                bool IsError = false;
                lbl_gridcError.Text = string.Empty;

                List<TB_FA_REIMBURSEMENT_ITEM> objFaItems = new List<TB_FA_REIMBURSEMENT_ITEM>();

                for (int i = 0; i < grdReimbursementItems.Rows.Count; i++)
                {
                    try
                    {
                        HiddenField hdn_Item_ID = (HiddenField)grdReimbursementItems.Rows[i].Cells[0].FindControl("hdn_Item_ID");
                        if (i != id)
                        {

                            TextBox txtAmount = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txtAmount");
                            TextBox txt_Date = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txt_Date");
                            TextBox txtItemDescription = (TextBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("txtItemDescription");
                            CheckBox chkAdvertisement = (CheckBox)grdReimbursementItems.Rows[i].Cells[0].FindControl("chkAdvertisement");
                            TB_FA_REIMBURSEMENT_ITEM objItem = new TB_FA_REIMBURSEMENT_ITEM();
                            objItem.FA_Application_ID = Guid.Parse(hdn_Item_ID.Value);
                            objItem.Amount = Convert.ToDecimal(txtAmount.Text);
                            objItem.Description = txtItemDescription.Text;
                            objItem.Date = DateTime.Parse(txt_Date.Text);
                            objItem.Advertisement = chkAdvertisement.Checked;
                            objFaItems.Add(objItem);
                        }

                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        lbl_gridcError.Text = ex.Message;
                    }
                }
                if (IsError == false)
                {
                    if (objFaItems.Count == 0)
                    {
                        objFaItems.Add(new TB_FA_REIMBURSEMENT_ITEM
                        {
                            Item_ID = default(Guid),
                            Date = null,
                            Amount = null
                        });
                    }
                    grdReimbursementItems.DataSource = objFaItems;
                    grdReimbursementItems.DataBind();
                }


            }


        }

        protected void FillReimburesementSalary()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                List<TB_FA_REIMBURSEMENT_SALARY> objReimbursement = new List<TB_FA_REIMBURSEMENT_SALARY>();
                if (!string.IsNullOrEmpty(hdn_ApplicationID.Value))
                {
                    Guid AppId = Guid.Parse(hdn_ApplicationID.Value);
                    objReimbursement = dbContext.TB_FA_REIMBURSEMENT_SALARY.Where(x => x.FA_Application_ID == AppId).OrderBy(x => x.Created_Date).ToList();
                }
                if (objReimbursement.Count == 0)
                {
                    objReimbursement = new List<TB_FA_REIMBURSEMENT_SALARY>() { new TB_FA_REIMBURSEMENT_SALARY{
                        Salary_ID=default(Guid)
                    } };
                }
                if (rdo_Categories.SelectedValue.ToLower() == "c")
                {
                    decimal? total = objReimbursement.Where(x => x.Amount.HasValue).Select(x => x.Amount).Sum();

                    FillCalculationDetail(total.HasValue ? total.Value : 0);
                }
                grdReimbursementSalary.DataSource = objReimbursement;
                grdReimbursementSalary.DataBind();

            }
        }
        protected void btn_AddReimbursementSalary_Click(object sender, ImageClickEventArgs e)
        {

            bool IsError = false;
            List<TB_FA_REIMBURSEMENT_SALARY> objFaItems = new List<TB_FA_REIMBURSEMENT_SALARY>();
            for (int i = 0; i < grdReimbursementSalary.Rows.Count; i++)
            {
                try
                {
                    HiddenField hdn_Item_ID = (HiddenField)grdReimbursementSalary.Rows[i].Cells[0].FindControl("hdn_Item_ID");
                    TextBox txtname = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtname");
                    TextBox txtmonthlysalary = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtmonthlysalary");
                    TextBox txtsalaryfrom = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtsalaryfrom");
                    TextBox txtsalaryto = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtsalaryto");
                    TextBox txtTax = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtTax");
                    TextBox txtAmount = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtAmount");
                    TB_FA_REIMBURSEMENT_SALARY objItem = new TB_FA_REIMBURSEMENT_SALARY();
                    objItem.Salary_ID = Guid.Parse(hdn_Item_ID.Value);
                    objItem.Intern_Name = txtname.Text;
                    objItem.Monthly_Salary = Convert.ToDecimal(txtmonthlysalary.Text);
                    objItem.Period_From = Convert.ToDateTime(txtsalaryfrom.Text);
                    objItem.Period_To = Convert.ToDateTime(txtsalaryto.Text);
                    objItem.MPF = Convert.ToDecimal(txtTax.Text);
                    objItem.Amount = Convert.ToDecimal(txtAmount.Text);

                    objFaItems.Add(objItem);
                }
                catch (Exception ex)
                {
                    IsError = true;
                    lbl_gridcError.Text = ex.Message;
                }
            }
            if (IsError == false)
            {
                lbl_gridcError.Text = "";
                objFaItems.Add(new TB_FA_REIMBURSEMENT_SALARY
                {
                    Salary_ID = default(Guid)
                });
                decimal? total = objFaItems.Where(x => x.Amount.HasValue).Select(x => x.Amount).Sum();
                FillCalculationDetail(total.HasValue ? total.Value : 0);

                grdReimbursementSalary.DataSource = objFaItems;
                grdReimbursementSalary.DataBind();

            }
        }
        protected void grdReimbursementSalary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void grdReimbursementSalary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                bool IsError = false;
                lbl_gridcError.Text = string.Empty;

                List<TB_FA_REIMBURSEMENT_SALARY> objFaItems = new List<TB_FA_REIMBURSEMENT_SALARY>();

                for (int i = 0; i < grdReimbursementSalary.Rows.Count; i++)
                {
                    try
                    {
                        HiddenField hdn_Item_ID = (HiddenField)grdReimbursementSalary.Rows[i].Cells[0].FindControl("hdn_Item_ID");
                        if (i != id)
                        {
                            TextBox txtname = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtname");

                            TextBox txtmonthlysalary = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtmonthlysalary");
                            TextBox txtsalaryfrom = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtsalaryfrom");
                            TextBox txtsalaryto = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtsalaryto");
                            TextBox txtTax = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtTax");
                            TextBox txtAmount = (TextBox)grdReimbursementSalary.Rows[i].Cells[0].FindControl("txtAmount");

                            TB_FA_REIMBURSEMENT_SALARY objItem = new TB_FA_REIMBURSEMENT_SALARY();
                            objItem.FA_Application_ID = Guid.Parse(hdn_Item_ID.Value);
                            objItem.Intern_Name = txtname.Text;
                            if (txtmonthlysalary.Text != "")
                                objItem.Monthly_Salary = Convert.ToDecimal(txtmonthlysalary.Text);
                            else
                                objItem.Monthly_Salary = null;
                            objItem.Period_From = Convert.ToDateTime(txtsalaryfrom.Text);
                            objItem.Period_To = Convert.ToDateTime(txtsalaryto.Text);
                            if (txtTax.Text != "")
                                objItem.Tax = Convert.ToDecimal(txtTax.Text);
                            else objItem.Tax = null;
                            if (txtAmount.Text != "")
                                objItem.Amount = Convert.ToDecimal(txtAmount.Text);
                            else objItem.Amount = null;
                            objFaItems.Add(objItem);
                        }

                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        lbl_gridcError.Text = ex.Message;
                    }
                }
                if (IsError == false)
                {
                    if (objFaItems.Count == 0)
                    {
                        new TB_FA_REIMBURSEMENT_SALARY
                        {
                            Salary_ID = default(Guid)
                        };
                    }
                    grdReimbursementSalary.DataSource = objFaItems;
                    grdReimbursementSalary.DataBind();
                }


            }


        }

        protected void DisableControls()
        {
            btn_Save.Enabled = false;
            btn_Submit.Enabled = false;
            //pnl_FormOutside.Enabled = false;

            rdo_Categories.Enabled = ddlcompanyname.Enabled = ddlProgrammeattended.Enabled
                = ddl_preapproved.Enabled = chkbx_prepaidservice.Enabled = chkbx_freelance.Enabled
                = rdoFDeclarationofConflicts.Enabled = chkDeclareA.Enabled = chkDeclareB.Enabled = false;



            Fu_OriginalReceipt.Visible = ImageButton1.Visible = Fu_Original_Invoice.
                Visible = ImageButton2.Visible = Fu_Quotation.Visible = ImageButton3.Visible = Fu_EventPhoto.Visible = ImageButton4.Visible =
                Fu_EventPass.Visible = ImageButton5.Visible = Fu_PrintSample.Visible = ImageButton6.Visible =
                Fu_BoardingPass.Visible = ImageButton7.Visible =
                fu_flight_Inventory.Visible = ImageButton8.Visible =
                Fu_BusinessCard.Visible = ImageButton9.Visible =
                Fu_InternPayroll.Visible = ImageButton10.Visible =
                Fu_InternCertification.Visible = ImageButton11.Visible =
                Fu_PaymentProof.Visible = ImageButton12.Visible =
                Fu_EmployementContract.Visible = ImageButton13.Visible =
                Fu_Resume.Visible = ImageButton14.Visible =
                Fu_HKIDCard.Visible = ImageButton15.Visible =
                Fu_EmpPayslip.Visible = ImageButton18.Visible =
                Fu_SalaryProof.Visible = ImageButton19.Visible =
                Fu_FreelanceResume.Visible = ImageButton17.Visible =
                btn_AddReimbursementItems.Visible = btn_AddReimbursementSalary.Visible
                = false;

            txtcheque.Visible = false;
            lblcheque.Visible = true;
            txtServiceperiodfrom.Visible = false;
            lblServiceperiodfrom.Visible = true;
            txtServiceperiodTo.Visible = false;
            lblServiceperiodTo.Visible = true;

            txtServiceProviderName.Visible = false;
            lblServiceProviderName.Visible = true;
            txtServiceContractName.Visible = false;
            lblServiceContractName.Visible = true;

            txtServiceTotalFee.Visible = false;
            lblServiceTotalFee.Visible = true;

            txtconflicts.Visible = false;
            lblconflicts.Visible = true;

            Fu_BoardingPass.Enabled = false;
            Fu_BusinessCard.Enabled = false;
            Fu_EmployementContract.Enabled = false;
            Fu_EventPass.Enabled = false;
            Fu_EventPhoto.Enabled = false;
            fu_flight_Inventory.Enabled = false;
            Fu_FreelanceResume.Enabled = false;
            Fu_HKIDCard.Enabled = false;
            Fu_InternCertification.Enabled = false;
            Fu_InternPayroll.Enabled = false;
            Fu_OriginalReceipt.Enabled = false;
            Fu_Original_Invoice.Enabled = false;
            Fu_PaymentProof.Enabled = false;

        }
        protected void DisableGrids()
        {
            for (int i = 0; i < grdReimbursementItems.Rows.Count; i++)
            {
                ((TextBox)grdReimbursementItems.Rows[i].FindControl("txt_Date")).Visible =
                ((TextBox)grdReimbursementItems.Rows[i].FindControl("txtItemDescription")).Visible =
                 ((TextBox)grdReimbursementItems.Rows[i].FindControl("txtAmount")).Visible =
                   ((ImageButton)grdReimbursementItems.Rows[i].FindControl("btn_GridRemove")).Visible =
                    false;

                ((CheckBox)grdReimbursementItems.Rows[i].FindControl("chkAdvertisement")).Enabled = false;

                ((Label)grdReimbursementItems.Rows[i].FindControl("lbl_Date")).Visible =
                ((Label)grdReimbursementItems.Rows[i].FindControl("lblItemDescription")).Visible =
                     ((Label)grdReimbursementItems.Rows[i].FindControl("lblAmount")).Visible =

                    true;
            }


            for (int i = 0; i < grdReimbursementSalary.Rows.Count; i++)
            {
                ((TextBox)grdReimbursementSalary.Rows[i].FindControl("txtname")).Visible =
                    ((TextBox)grdReimbursementSalary.Rows[i].FindControl("txtmonthlysalary")).Visible =
 ((TextBox)grdReimbursementSalary.Rows[i].FindControl("txtsalaryfrom")).Visible =
 ((TextBox)grdReimbursementSalary.Rows[i].FindControl("txtsalaryto")).Visible =
     ((TextBox)grdReimbursementSalary.Rows[i].FindControl("txtTax")).Visible =
     ((TextBox)grdReimbursementSalary.Rows[i].FindControl("txtAmount")).Visible =
     ((ImageButton)grdReimbursementSalary.Rows[i].FindControl("btn_GridCRemove")).Visible =
                    false;

                ((Label)grdReimbursementSalary.Rows[i].FindControl("lblname")).Visible =
                ((Label)grdReimbursementSalary.Rows[i].FindControl("lblmonthlysalary")).Visible =
                ((Label)grdReimbursementSalary.Rows[i].FindControl("lblsalaryfrom")).Visible =
                ((Label)grdReimbursementSalary.Rows[i].FindControl("lblsalaryto")).Visible =
                ((Label)grdReimbursementSalary.Rows[i].FindControl("lblTax")).Visible =
                ((Label)grdReimbursementSalary.Rows[i].FindControl("lblAmount")).Visible =
                    true;
            }
        }
        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            UserSubmitPasswordPopup.Visible = false;
        }
        protected void btn_Submit_Click1(object sender, EventArgs e)
        {
            int errors = check_db_validations(true);
            if (errors == 0)
            {
                if (!SubmitValidationError())
                {
                    UserSubmitPasswordPopup.Visible = true;
                    SPFunctions objFUnction = new SPFunctions();
                    txtLoginUserName.Text = objFUnction.GetCurrentUser();
                    lblSubmissionApplication.Text = lblApplicationNo.Text;
                }
                else
                {
                    lblgrouperror.Visible = true;
                }
            }
            //enable later ShowHideControlsBasedUponUserData();
        }
        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;
            Context.Response.Redirect("/SitePages/MyReimbursements.aspx");


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
                            TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);

                            if (objApp != null)
                            {
                                bool isrequestor = false;
                                SPFunctions objFn = new SPFunctions();
                                string strCurrentUser = objFn.GetCurrentUser();
                                if (objApp.Status.ToLower().Replace("_", " ") == formsubmitaction.Waiting_for_response_from_applicant.ToString().Replace("_", " ").ToLower())
                                {
                                    objApp.Status = formsubmitaction.Resubmitted_information.ToString().Replace("_", " ");
                                    isrequestor = true;
                                }
                                else
                                {
                                    objApp.Status = formsubmitaction.Submitted.ToString();
                                    isrequestor = false;
                                }
                                objApp.Modified_Date = DateTime.Now;
                                objApp.Modified_By = strCurrentUser;
                                objApp.Submitted_Date = DateTime.Now;

                                dbContext.SaveChanges();
                                string requestor = "";
                                string strEmailContent = "";
                                string strEmailsubject = "";

                                IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new List<TB_SYSTEM_PARAMETER>();
                                objTbParams = dbContext.TB_SYSTEM_PARAMETER;


                                string WebsiteUrl = objTbParams.FirstOrDefault(x => x.Config_Code == "WebsiteUrl").Value;
                                WebsiteUrl = WebsiteUrl.EndsWith("/") ? (WebsiteUrl.Remove(WebsiteUrl.LastIndexOf("/"))) : WebsiteUrl;

                                string applicationType = "Financial_Reimbursements_CASP.aspx";
                                string token = "/SitePages/" + applicationType + "?resubmit=Y&app=" + objApp.CASP_FA_ID;

                                if (isrequestor == true)
                                {

                                    List<TB_SCREENING_HISTORY> objTB_SCREENING_HISTORY1 = new List<TB_SCREENING_HISTORY>();
                                    TB_SCREENING_HISTORY objTB_SCREENING_HISTORY = new TB_SCREENING_HISTORY();
                                    objTB_SCREENING_HISTORY1 = dbContext.TB_SCREENING_HISTORY.OrderByDescending(x => x.Created_Date).ToList();
                                    objTB_SCREENING_HISTORY = objTB_SCREENING_HISTORY1.FirstOrDefault(x => x.Application_Number == objApp.Application_No);// && x.Programme_ID == objApp.);

                                    requestor = objTB_SCREENING_HISTORY.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("CASP_Reimbursement_ReSubmitted_Applicant");

                                    strEmailContent = strEmailContent.Replace("@@AppNumber", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ProgramName", "CASP Financial Assistance Reimbursement");

                                    strEmailsubject = LocalizeCommon("Mail_App_submitted_Requestor").Replace("@@Applicationnumber", objApp.Application_No);

                                    strEmailsubject = strEmailsubject.Replace("@@ProgramName", "CASP Financial Assistance Reimbursement");

                                    int IsEmailSent = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);

                                    TB_SCREENING_HISTORY objScreening = new TB_SCREENING_HISTORY();
                                    objScreening.Application_Number = objApp.Application_No;
                                    objScreening.Programme_ID = Convert.ToInt32(0);
                                    objScreening.Validation_Result = formsubmitaction.Resubmitted_information.ToString().Replace("_", " ");
                                    objScreening.Comment_For_Applicants = " ";
                                    objScreening.Comment_For_Internal_Use = objScreening.Validation_Result +" By "+ objApp.Created_By;
                                    objScreening.Created_By = objApp.Created_By;
                                    objScreening.Created_Date = DateTime.Now;
                                    dbContext.TB_SCREENING_HISTORY.Add(objScreening);
                                    dbContext.SaveChanges();

                                    requestor = objApp.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("CASP_Reimbursement_Submitted");

                                    strEmailContent = strEmailContent.Replace("@@Application_No", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objApp.Application_No).Replace("submitted ", "resubmitted");
                                    int IsEmailed = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);


                                }
                                else
                                {
                                    requestor = objApp.Created_By;
                                    strEmailContent = CBPEmail.GetEmailTemplate("CASP_Reimbursement_Submitted");

                                    strEmailContent = strEmailContent.Replace("@@Application_No", objApp.Application_No);
                                    strEmailContent = strEmailContent.Replace("@@ApplicationUrl", WebsiteUrl + token);
                                    strEmailsubject = LocalizeCommon("Mail_App_submitted").Replace("@@Applicationnumber", objApp.Application_No);
                                    int IsEmailed = CBPEmail.SendMail(requestor, strEmailsubject, strEmailContent);
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
                                TB_COMPANY_PROFILE_BASIC objCompany = dbContext.TB_COMPANY_PROFILE_BASIC.FirstOrDefault(x => x.Company_Profile_ID == objApp.Company_ID);

                                Fill_Programelist(objApp.Application_No, objApp.Category, objCompany.Company_Name, objApp.Status, objApp.Created_By, Convert.ToDecimal(objApp.Total_Amount_After_Deduction), objApp.CASP_FA_ID.ToString());

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
                    UserSubmitPasswordPopup.Visible = true;
                    UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                }
            }
            catch (Exception ex)
            {
                UserSubmitPasswordPopup.Visible = true;
                UserCustomerrorLogin.InnerText = ex.Message;
            }
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
        private void Fill_Programelist(string Application_number, string Category, string CompanyName, string status, string Applicant, decimal Amount, string Application_ID)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPWeb site = SPFunctions.GetCurrentWeb)
                    {
                        site.AllowUnsafeUpdates = true;
                        SPList list = site.Lists["Application List CASP Reimbursement"];

                        SPListItem itemAttachment = GetItemByBdcId(list, Application_number.ToString(), "Application_Number", Applicant, "Applicant");

                        if (itemAttachment == null)
                        {

                            itemAttachment = list.Items.Add();
                        }


                        itemAttachment["Application_Number"] = Application_number;
                        itemAttachment["Programme_Name"] = "CASP Financial Assistance Reimbursement Form";
                        itemAttachment["CompanyName"] = CompanyName;
                        itemAttachment["Category"] = Category;
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

            }
            //finally
            //{
            //    Context.Response.Redirect("~/SitePages/Home.aspx", false);
            //}
        }
        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
        protected bool SubmitValidationError()
        {
            bool IsError = false;
            List<string> errlist = new List<string>();
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    SPFunctions objFn = new SPFunctions();
                    string strCurrentUser = objFn.GetCurrentUser();
                    TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT objApp = GetExistingApplication(dbContext);

                    if (objApp != null)
                    {
                        lblSubmissionApplication.Text = objApp.Application_No;
                        List<TB_FA_REIMBURSEMENT_ITEM> ObjItems = dbContext.TB_FA_REIMBURSEMENT_ITEM.Where(x => x.FA_Application_ID == objApp.CASP_FA_ID).ToList();
                        List<TB_FA_REIMBURSEMENT_SALARY> objSalary = dbContext.TB_FA_REIMBURSEMENT_SALARY.Where(x => x.FA_Application_ID == objApp.CASP_FA_ID).ToList();
                        List<TB_APPLICATION_ATTACHMENT> objAttachment = dbContext.TB_APPLICATION_ATTACHMENT.Where(x => x.Application_ID == objApp.CASP_FA_ID).ToList();

                        if (ddlcompanyname.SelectedValue == "")
                        {
                            IsError = true;
                            errlist.Add(Localize("err_Company"));
                        }
                        if (string.IsNullOrEmpty(objApp.Payable_To))
                        {
                            IsError = true;
                            errlist.Add(Localize("err_ChequePayble"));
                        }
                        if (!objApp.CASP_Attended.HasValue)
                        {
                            IsError = true;
                            errlist.Add(Localize("err_AcceleratorProg"));
                        }


                        if (objApp.Category.ToLower() != "c")
                        {
                            if (ObjItems.Count == 0)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_ReimbursementItem"));
                            }

                            if (objApp.Category.ToLower() == "f")
                            {
                                if (ObjItems.Count > 0 && ObjItems.Sum(x => x.Amount) > 10000 && !objApp.Preapproved_SpecialRequest.HasValue)
                                {
                                    IsError = true;
                                    errlist.Add(Localize("err_SpecialRequestForm"));
                                }
                            }

                            if (objAttachment.Where(x => x.Attachment_Type == enumAttachmentType.FA_Original_Receipt.ToString()).Count() == 0)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_FileOriginalReceiptReq"));
                            }
                            if (chkDeclareA.Checked == false)

                            {
                                IsError = true;
                                errlist.Add(Localize("err_checkbox_confirm"));

                            }

                        }

                        if (objApp.Category.ToLower() == "c")
                        {
                            if (objSalary.Count == 0)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_ReimbursementSalaryAtleast"));
                            }
                            if (objAttachment.Where(x => x.Attachment_Type == enumAttachmentType.FA_Intern_Payroll.ToString()).Count() == 0)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_FileInternPayroll"));
                            }

                            //if (objAttachment.Where(x => x.Attachment_Type == enumAttachmentType.FA_MPF_PaymentProof.ToString()).Count() == 0)
                            //{
                            //    IsError = true;
                            //    errlist.Add("Please upload required document : MPF Payment Proof");
                            //}

                            if (objAttachment.Where(x => x.Attachment_Type == enumAttachmentType.FA_Employement_Contract.ToString()).Count() == 0)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_FileEmployementContract"));
                            }

                            if (objAttachment.Where(x => x.Attachment_Type == enumAttachmentType.FA_ResumeCV.ToString()).Count() == 0)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_FileResumeCV"));
                            }
                            if (chkDeclareB.Checked == false)
                            {
                                IsError = true;
                                errlist.Add(Localize("err_Checkbox_certify"));

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                errlist.Add(ex.Message);

            }
            //if (IsError == true)
            //{
            lblgrouperror.DataSource = errlist;
            lblgrouperror.DataBind();
            ShowbottomMessage("", false);
            //}
            //else
            //{
            //    ShowbottomMessage("", true);
            //}
            return IsError;


        }


    }
}
