using CBP_EMS_SP.Data;
using CBP_EMS_SP.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using CBP_EMS_SP.Common;
using System.Web.UI.WebControls;

namespace CBP_EMS_SP.PublicUserControls.InternalUserApplications
{
    [ToolboxItemAttribute(false)]
    public partial class InternalUserApplications : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public InternalUserApplications()
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
                Common.SPFunctions spFun = new Common.SPFunctions();
                if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CPIP BDM", true)
                || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true)
                 || spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true))
                {
                    InitializeApplications();
                }
                else
                {
                    Context.Response.Redirect("~/SitePages/Home.aspx");
                }
            }
        }

        protected void InitializeApplications()
        {
            
            Common.SPFunctions spFun = new Common.SPFunctions();

            if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CPIP BDM", true))
            {
                IncubationApplications();
            }
            else if (spFun.CurrentUserIsInGroup("CCMF Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true))
            {
                CCMFApplications();
            }
            else if (spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true))
            {
                AllApplications();
            }
        }
        protected void IncubationApplications()
        {
            List<ListItem> objIncubation = new List<ListItem>();
            objIncubation.Add(new ListItem() { Value = "Cyberport Incubation Program", Text = "Cyberport Incubation Program" });
            ddlProgramName.DataSource = objIncubation;
            ddlProgramName.DataTextField = "Text";
            ddlProgramName.DataValueField = "Value";
            ddlProgramName.DataBind();

        }

        protected void CCMFApplications()
        {
            List<ListItem> objIncubation = new List<ListItem>();
            objIncubation.Add(new ListItem() { Value = "HongKong", Text = "Cyberport Creative Micro Fund - Hong Kong" });
            objIncubation.Add(new ListItem() { Value = "CrossBorder", Text = "Cyberport Creative Micro Fund - Cross Border" });
            objIncubation.Add(new ListItem() { Value = "CUPP", Text = "Cyberport University Partnership Programme" });
            objIncubation.Add(new ListItem() { Value = "CCMFGBAYEP", Text = "Cyberport Creative Micro Fund - GBAYEP" });    //20220929 mew program
            ddlProgramName.DataSource = objIncubation;
            ddlProgramName.DataTextField = "Text";
            ddlProgramName.DataValueField = "Value";
            ddlProgramName.DataBind();
        }
        protected void AllApplications()
        {

            List<ListItem> objIncubation = new List<ListItem>();
            objIncubation.Add(new ListItem() { Value = "Cyberport Incubation Program", Text = "Cyberport Incubation Program" });
            objIncubation.Add(new ListItem() { Value = "HongKong", Text = "Cyberport Creative Micro Fund - Hong Kong" });
            objIncubation.Add(new ListItem() { Value = "CrossBorder", Text = "Cyberport Creative Micro Fund - Cross Border" });
            objIncubation.Add(new ListItem() { Value = "CUPP", Text = "Cyberport University Partnership Programme" });
            objIncubation.Add(new ListItem() { Value = "CCMFGBAYEP", Text = "Cyberport Creative Micro Fund - GBAYEP" });    //20220929 mew program
            ddlProgramName.DataSource = objIncubation;
            ddlProgramName.DataTextField = "Text";
            ddlProgramName.DataValueField = "Value";
            ddlProgramName.DataBind();
        }

        protected void btn_showselectedlist_Click(object sender, EventArgs e)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
              string ProgrameName =   ddlProgramName.SelectedValue;
                if (ProgrameName.Contains("Incubation"))
                {
                    List<TB_INCUBATION_APPLICATION> objTB_INCUBATION_APPLICATION = IncubationContext.GET_INCUBATION_PROGRAMS();
                    objTB_INCUBATION_APPLICATION.Where(x => x.Status == formsubmitaction.Submitted.ToString());
                    var applist = (from oIncub in objTB_INCUBATION_APPLICATION
                                   where oIncub.Status == formsubmitaction.Submitted.ToString()

                                   select new
                                   {
                                       Application_Number = oIncub.Application_Number,
                                       ComapnyName = oIncub.Company_Name_Eng,
                                       Status = oIncub.Status
                                       //Intake_Number = oIncub.Intake_Number,
                                       //ApplicationNumber = oIncub.Application_Number,
                                       //ProjectName = oIncub.Company_Name_Eng

                                   });
                    rptrshowlist.DataSource = applist;
                    rptrshowlist.DataBind();
                }
                else
                {
                    List<TB_CCMF_APPLICATION> objTB_INCUBATION_APPLICATION = IncubationContext.GET_CCMF_PROGRAMS(ProgrameName);
           
            var applist = (from oIncub in objTB_INCUBATION_APPLICATION
                        where oIncub.Status == formsubmitaction.Submitted.ToString()

                        select new
                        {
                            Application_Number = oIncub.Application_Number,
                            ComapnyName = oIncub.Project_Name_Eng,
                            Status = oIncub.Status
                             //Intake_Number = oIncub.Intake_Number,
                             //ApplicationNumber = oIncub.Application_Number,
                             //ProjectName = oIncub.Company_Name_Eng

                         });
                    rptrshowlist.DataSource = applist;
                    rptrshowlist.DataBind();
                }
            }
            }
    }
}
