using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data.Models;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using CBP_EMS_SP.Common;
using System.Web;
using System.Web.Security;
using CBP_EMS_SP.Data;

namespace CPB_EMS_SP.UserDashboard.UserDashboard
{
    [ToolboxItemAttribute(false)]
    public partial class UserDashboard : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public UserDashboard()
        {
        }

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string UserLanguage = string.Empty;
            if (Context.Request.Cookies["CBP_User_Language"] != null)
            {
                UserLanguage = Context.Request.Cookies["CBP_User_Language"].Value;
            }
            SPFunctions.LocalizeUIForPage(UserLanguage);
            CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();

            if (spFun.CurrentUserIsInGroup(SPFunctions.ExternalUserGroup) || spFun.CurrentUserIsInGroup("Collaborator"))
            {
                using (var Intake = new CyberportEMS_EDM())
                {
                    List<TB_PROGRAMME_INTAKE> objProgram = new List<TB_PROGRAMME_INTAKE>();
                    List<CASP_CompanyList> ObjUserCompany = IncubationContext.GetCompanyForUser(GetCurrentUser());
                    if (ObjUserCompany.Count == 0)
                    {
                        objProgram = Intake.TB_PROGRAMME_INTAKE.Where(x => x.Application_Start <= DateTime.Now && DateTime.Now <= x.Application_Deadline && x.Active == true && !x.Programme_Name.ToLower().Contains("cyberport accelerator")).ToList();
                    }
                    else
                    {
                        objProgram = Intake.TB_PROGRAMME_INTAKE.Where(x => x.Application_Start <= DateTime.Now && DateTime.Now <= x.Application_Deadline && x.Active == true).ToList();
                    }
                    Rptr_IntakeProgram.DataSource = objProgram;
                    Rptr_IntakeProgram.DataBind();
                }
            }

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["token"]))
            {
                using (var dbcontext = new CyberportEMS_EDM())
                {
                    string ActLnk = HttpContext.Current.Request.QueryString["token"];
                    CBPTokenGeneration objSQS = new CBPTokenGeneration(ActLnk);
                    //AESEncryption aes = new AESEncryption();
                    int strProgrammeId = Convert.ToInt32(objSQS["ProgrammeId"]);
                    string strAppId = objSQS["AppId"];
                    string Collaboratoremail = objSQS["Collaboratoremail"];
                    TB_PROGRAMME_INTAKE objProgram = dbcontext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == strProgrammeId);
                    if (objProgram.Programme_Name.ToLower().Contains("incubation"))
                    {
                        HttpContext.Current.Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/IncubationProgram.aspx?prog=" + strProgrammeId + "&app=" + strAppId + "&resubmitversion=Y");
                    }
                    else if (objProgram.Programme_Name.ToLower().Contains("accelerator support"))
                    {
                        HttpContext.Current.Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CASPProgram.aspx?prog=" + strProgrammeId + "&app=" + strAppId + "&resubmitversion=Y");
                    }
                    #region Add CCMFGBAYEP
                    else if (objProgram.Programme_Name.ToLower().Contains("gbayep"))
                    {
                        HttpContext.Current.Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMFGBAYEP.aspx?prog=" + strProgrammeId + "&app=" + strAppId + "&resubmitversion=Y");
                    }
                    #endregion
                    else
                    {
                        HttpContext.Current.Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + strProgrammeId + "&app=" + strAppId + "&resubmitversion=Y");
                    }
                }
            }

        }
        protected string GetCurrentUser()
        {
            return new SPFunctions().GetCurrentUser();
        }

        protected void Rptr_IntakeProgram_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btn_ProgramRedirection = (Button)e.Item.FindControl("btn_ProgramRedirection");
                Image img_Program = (Image)e.Item.FindControl("img_Program");
                Label lblprogramname = (Label)e.Item.FindControl("lblprogramname");
                Panel pnl_CASPInfo = (Panel)e.Item.FindControl("pnl_CASPInfo");
                pnl_CASPInfo.Visible = false;
                Panel pnl_IntakeInfo = (Panel)e.Item.FindControl("pnl_IntakeInfo");
                pnl_IntakeInfo.Visible = true;
                int ProgramId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Programme_ID"));
                string ProgramName = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Programme_Name"));
                string UserName = GetCurrentUser();
                if (ProgramName.ToLower().Contains("incubation"))
                {
                    using (var Intake = new CyberportEMS_EDM())
                    {
                        List<TB_INCUBATION_APPLICATION> objProgram = Intake.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == ProgramId && (x.Created_By.ToLower() == UserName.ToLower()) && x.Status != formsubmitaction.Deleted.ToString()).OrderBy(x => x.Modified_Date).ToList();

                        lblprogramname.Text = SPFunctions.LocalizeUI("Incubation", "CyberportEMS_Incubation");
                        if (objProgram.Count == 0)
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/IncubationProgram.aspx?prog=" + ProgramId + "&app=" + Guid.NewGuid().ToString() + "&resubmitversion=Y" + "&new=Y";
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Apply", "CyberportEMS_Common");
                        }
                        else
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/IncubationProgram.aspx?prog=" + ProgramId + "&app=" + objProgram.FirstOrDefault().Incubation_ID + "&resubmitversion=Y";
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Edit_Submission", "CyberportEMS_Common");
                        }

                        img_Program.ImageUrl = "images/_layouts/15/Images/CBP_Images/Programme.png";
                    }
                }
                #region 20220829 Add Case for CCMF-GBAYEP
                //TODO: 20220829 UserDashBoard Add Case for CCMF-GBAYEP
                else if (DataBinder.Eval(e.Item.DataItem, "Programme_Name").ToString().ToLower().Contains("gbayep"))
                {
                    using (var Intake = new CyberportEMS_EDM())
                    {                    
                        lblprogramname.Text = SPFunctions.LocalizeUI("CCMF_GBAYEP_Header", "CyberportEMS_CCMFGBAYEP");

                        //TODO: Pending tto add new object TB_CCMF_GBAYEP_APPLICATION
                        List<TB_CCMF_APPLICATION> objProgram = Intake.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == ProgramId && (x.Created_By.ToLower() == UserName.ToLower()) && x.Status != formsubmitaction.Deleted.ToString()).OrderBy(x => x.Modified_Date).ToList();

                        if (objProgram.Count == 0)
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMFGBAYEP.aspx?prog=" + DataBinder.Eval(e.Item.DataItem, "Programme_ID").ToString() + "&app=" + Guid.NewGuid().ToString() + "&resubmitversion=Y" + "&new=Y"; ;
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Apply", "CyberportEMS_Common");
                        }
                        else
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMFGBAYEP.aspx?prog=" + DataBinder.Eval(e.Item.DataItem, "Programme_ID").ToString() + "&app=" + objProgram.FirstOrDefault().CCMF_ID + "&resubmitversion=Y";
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Edit_Submission", "CyberportEMS_Common");
                        }

                        img_Program.ImageUrl = "images/_layouts/15/Images/CBP_Images/HK.png";
                    }
                }
                #endregion
                else if (DataBinder.Eval(e.Item.DataItem, "Programme_Name").ToString().ToLower().Contains("hong kong") || DataBinder.Eval(e.Item.DataItem, "Programme_Name").ToString().ToLower().Contains("university partnerhip"))
                {
                    using (var Intake = new CyberportEMS_EDM())
                    {
                        if (DataBinder.Eval(e.Item.DataItem, "Programme_Name").ToString().ToLower().Contains("hong kong"))
                        {
                            lblprogramname.Text = SPFunctions.LocalizeUI("CCMF_HK_Header", "CyberportEMS_CCMF");
                        }
                        else
                        {
                            lblprogramname.Text = SPFunctions.LocalizeUI(" CUPP_Header", "CyberportEMS_CCMF");
                        }

                        List<TB_CCMF_APPLICATION> objProgram = Intake.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == ProgramId && (x.Created_By.ToLower() == UserName.ToLower()) && x.Status != formsubmitaction.Deleted.ToString()).OrderBy(x => x.Modified_Date).ToList();

                        if (objProgram.Count == 0)
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + DataBinder.Eval(e.Item.DataItem, "Programme_ID").ToString() + "&app=" + Guid.NewGuid().ToString() + "&resubmitversion=Y" + "&new=Y"; ;
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Apply", "CyberportEMS_Common");
                        }
                        else
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + DataBinder.Eval(e.Item.DataItem, "Programme_ID").ToString() + "&app=" + objProgram.FirstOrDefault().CCMF_ID + "&resubmitversion=Y";
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Edit_Submission", "CyberportEMS_Common");
                        }

                        img_Program.ImageUrl = "images/_layouts/15/Images/CBP_Images/HK.png";
                    }
                }
                else if (DataBinder.Eval(e.Item.DataItem, "Programme_Name").ToString().ToLower().Contains("cross border"))
                {
                    using (var Intake = new CyberportEMS_EDM())
                    {
                        //Label lblprogramname = (Label)e.Item.FindControl("lblprogramname");
                        lblprogramname.Text = SPFunctions.LocalizeUI("CCMF_CrossBorder_Header", "CyberportEMS_CCMF");
                        List<TB_CCMF_APPLICATION> objProgram = Intake.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == ProgramId && (x.Created_By.ToLower() == UserName.ToLower()) && x.Status != formsubmitaction.Deleted.ToString()).OrderByDescending(x => x.Modified_Date).ToList();

                        if (objProgram.Count == 0)
                        {
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Apply", "CyberportEMS_Common");
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + DataBinder.Eval(e.Item.DataItem, "Programme_ID").ToString() + "&app=" + Guid.NewGuid().ToString() + "&resubmitversion=Y" + "&new=Y";
                        }
                        else
                        {
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CCMF.aspx?prog=" + DataBinder.Eval(e.Item.DataItem, "Programme_ID").ToString() + "&app=" + objProgram.FirstOrDefault().CCMF_ID + "&resubmitversion=Y";
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Edit_Submission", "CyberportEMS_Common");
                        }
                        img_Program.ImageUrl = "images/_layouts/15/Images/CBP_Images/HK.png";

                    }
                }
                else if (DataBinder.Eval(e.Item.DataItem, "Programme_Name").ToString().ToLower().Contains("accelerator support"))
                {
                    pnl_CASPInfo.Visible = true;
                  
                    pnl_IntakeInfo.Visible = false;
                    using (var Intake = new CyberportEMS_EDM())
                    {
                        //List<TB_CASP_APPLICATION> objProgram = Intake.TB_CASP_APPLICATION.Where(x => x.Programme_ID == ProgramId && (x.Created_By.ToLower() == UserName.ToLower()) && x.Status != formsubmitaction.Deleted.ToString()).OrderBy(x => x.Modified_Date).ToList();

                        lblprogramname.Text = SPFunctions.LocalizeUI("CASP_ApplicationHeader", "CyberportEMS_CASP");
                        //if (objProgram.Count == 0)
                        //{
                            btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CASPProgram.aspx?prog=" + ProgramId + "&app=" + Guid.NewGuid().ToString() + "&resubmitversion=Y" + "&new=Y";
                            btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Apply", "CyberportEMS_Common");
                        //}
                        //else
                        //{
                        //    btn_ProgramRedirection.PostBackUrl = SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/CASPProgram.aspx?prog=" + ProgramId + "&app=" + objProgram.FirstOrDefault().CASP_ID + "&resubmitversion=Y";
                        //    btn_ProgramRedirection.Text = SPFunctions.LocalizeUI("Edit_Submission", "CyberportEMS_Common");
                        //}

                        img_Program.ImageUrl = "images/_layouts/15/Images/CBP_Images/Cross Border.png";
                    }

                }
            }

        }


    }
}
