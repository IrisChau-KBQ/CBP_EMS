using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CBP_EMS_SP.Data.Models;
using System.Linq;
using System.Collections.Generic;
using CBP_EMS_SP.Data;
using CBP_EMS_SP.Common;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.ApplicationServer.Caching;
using System.Globalization;
using System.Threading;

namespace CBP_EMS_SP.PublicUserControls.ProgrammeManagement
{
    [ToolboxItemAttribute(false)]
    public partial class ProgrammeManagement : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ProgrammeManagement()
        {
        }

        public void FillPrograms()
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                grvaddprogram.Visible = false;
                addprogramme.Visible = true;
                bltErrorList.DataSource = new List<String>();
                bltErrorList.DataBind();
                List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = IncubationContext.Get_programme_name();
                rptrprogrammemanag.DataSource = objTB_PROGRAMME_INTAKE;
                rptrprogrammemanag.DataBind();
            }
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

            if (!Page.IsPostBack)
            {
                Common.SPFunctions spFun = new Common.SPFunctions();
                if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CPIP BDM", true)
                    || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true)
                    || spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true)
                    || spFun.CurrentUserIsInGroup("CASP Coordinator", true) || spFun.CurrentUserIsInGroup("CASP BDM", true)
                    || spFun.CurrentUserIsInGroup("CASP Senior Manager", true))
                {
                    FillPrograms();
                }
                else
                {
                    Context.Response.Redirect("~/SitePages/Home.aspx");
                }
            }
               

        }

        protected void AddProgramme_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            hdnProgramme_ID.Value = "0";
            InitializeEditGridView();

        }
        protected void InitializeEditGridView()
        {
            try
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = new List<TB_PROGRAMME_INTAKE>();
                    grvaddprogram.Visible = true;
                    addprogramme.Visible = false;

                    int Programme_IDint = Convert.ToInt32(hdnProgramme_ID.Value);
                    if (Programme_IDint == 0)
                        objTB_PROGRAMME_INTAKE.Add(new TB_PROGRAMME_INTAKE() { Programme_ID = 0 });
                    else
                    {
                        objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == Programme_IDint).ToList();
                    }
                    grvaddprogram.DataSource = objTB_PROGRAMME_INTAKE;
                    grvaddprogram.DataBind();

                    for (int i = 0; i < grvaddprogram.Rows.Count; i++)
                    {

                        DropDownList ddlProgramName = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("ddlProgramName");
                        if (objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_ID > 0)
                            ddlProgramName.SelectedValue = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name;
                        DropDownList dldstartinghours = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("dldstartinghours");

                        CheckBox cbShowCCMFYEP = (CheckBox)grvaddprogram.Rows[i].Cells[0].FindControl("showCCMFYEP");
                        CheckBox cbShowCCMFProf = (CheckBox)grvaddprogram.Rows[i].Cells[0].FindControl("showCCMFProf");
                        
                        cbShowCCMFProf.Text = SPFunctions.LocalizeUI("Professional_Stream", "CyberportEMS_CCMF");
                        cbShowCCMFYEP.Text = SPFunctions.LocalizeUI("Hong_Kong_Young_Entrepreneur_Programme", "CyberportEMS_CCMF");
 
                        dldstartinghours.SelectedValue = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Application_Start.Hour);
                        DropDownList ddlstartingmins = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("ddlstartingmins");
                        List<ListItem> startmins = new List<ListItem>();
                        List<ListItem> endmins = new List<ListItem>();
                        if (ddlProgramName.SelectedValue == "Cyberport Creative Micro Fund - Hong Kong")
                        {
                            cbShowCCMFProf.Checked = objTB_PROGRAMME_INTAKE.FirstOrDefault().ProfShow;
                            cbShowCCMFYEP.Checked = objTB_PROGRAMME_INTAKE.FirstOrDefault().YEPShow;
                            cbShowCCMFProf.Visible = true;
                            cbShowCCMFYEP.Visible = true;
                        }
                        else
                        {
                            cbShowCCMFProf.Visible = false;
                            cbShowCCMFYEP.Visible = false;
                        }

                        for (int j = 0; j < 60; j++)
                        {
                            int length = Convert.ToInt32(j.ToString().Length);
                            string k = Convert.ToString(j);
                             if (length <= 1)
                            {
                                k = "0" + Convert.ToString(k);
                            }
                            startmins.Add(new ListItem() { Value = k, Text = k });
                            endmins.Add(new ListItem() { Value = k, Text = k });
                        }
                        ddlstartingmins.DataSource = startmins;
                        ddlstartingmins.DataTextField = "Text";
                        ddlstartingmins.DataValueField = "Value";
                        ddlstartingmins.DataBind();

                        ddlstartingmins.SelectedValue = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Application_Start.Minute);
                        DropDownList dldendinghours = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("dldendinghours");


                        dldendinghours.SelectedValue = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Application_Deadline.Hour);
                        DropDownList dldendingmins = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("dldendingmins");
                        dldendingmins.DataSource = endmins;
                        dldendingmins.DataTextField = "Text";
                        dldendingmins.DataValueField = "Value";
                        dldendingmins.DataBind();
                        dldendingmins.SelectedValue = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Application_Deadline.Minute);

                        
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btn_SaveIntake_Click(object sender, EventArgs e)
        {
            List<String> ErrorLIst = new List<string>();
            for (int i = 0; i < grvaddprogram.Rows.Count; i++)
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    //using (var dbCtxTxn = dbContext.Database.BeginTransaction())
                    //{
                    try
                    {

                        int Programme_IDint = Convert.ToInt32(hdnProgramme_ID.Value);
                        DropDownList ddlProgramName = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("ddlProgramName");
                        DropDownList dldstartinghours = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("dldstartinghours");
                        DropDownList ddlstartingmins = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("ddlstartingmins");
                        DropDownList dldendinghours = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("dldendinghours");
                        DropDownList dldendingmins = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("dldendingmins");
                        TextBox txtintakenumber = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtintakenumber");
                        CheckBox cbShowCCMFProf = (CheckBox)grvaddprogram.Rows[i].Cells[0].FindControl("showCCMFProf");
                        CheckBox cbShowCCMFYEP = (CheckBox)grvaddprogram.Rows[i].Cells[0].FindControl("showCCMFYEP");

                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.IntergerValue, txtintakenumber.Text.Trim()))
                            ErrorLIst.Add("Intake Number should be integer");

                       TextBox txtapplicationno = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtapplicationno");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 30, true, AllowAllSymbol: true), txtapplicationno.Text.Trim()))
                            ErrorLIst.Add("Application No. Prefix length should be 1-30 characters.");

                        TextBox txtappstartdate = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtappstartdate");
                        if (Programme_IDint == 0)
                        {
                            if (DateTime.Parse(txtappstartdate.Text.Trim()) <= DateTime.Parse(DateTime.Now.ToString("dd MMM yyyy")) || (Convert.ToDateTime(txtappstartdate.Text).Year) < DateTime.Now.Year)
                            {
                                ErrorLIst.Add("Application StartDate should be greater than Today's Date");
                            }
                        }
                        TextBox txtappdeadline = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtappdeadline");
                        if (DateTime.Parse(txtappdeadline.Text.Trim()) <= DateTime.Parse(DateTime.Now.ToString("dd MMMM yyyy")))
                        {
                            ErrorLIst.Add("Application Deadline should be greater than Today's Date");
                        }
                        if (DateTime.Parse(txtappstartdate.Text.Trim()) > DateTime.Parse(txtappdeadline.Text.Trim()))
                        {
                            ErrorLIst.Add("Application Deadline should be greater than Start Date");
                        }

                        TextBox txtappdeadlineengtxt = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtappdeadlineengtxt");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtappdeadlineengtxt.Text.Trim()))
                            ErrorLIst.Add("Application Deadline English length should be 1-50 characters.");
                        TextBox txtappdeadlinetradchitxt = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtappdeadlinetradchitxt");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtappdeadlinetradchitxt.Text.Trim()))
                            ErrorLIst.Add("Application Deadline Trad Chin length should be 1-50 characters.");
                        TextBox txtappdeadlinesimpchitxt = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtappdeadlinesimpchitxt");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtappdeadlinesimpchitxt.Text.Trim()))
                            ErrorLIst.Add("Application Deadline Simp. Chin length should be 1-50 characters.");
                        TextBox txtvnpeng = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtvnpeng");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtvnpeng.Text.Trim()))
                            ErrorLIst.Add("Vetting Session Eng length should be 1-50 characters.");
                        TextBox txtvnptradchi = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtvnptradchi");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtvnptradchi.Text.Trim()))
                            ErrorLIst.Add("Vetting Session TradChin length should be 1-50 characters.");
                        TextBox txtvnpsimchi = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtvnpsimchi");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtvnpsimchi.Text.Trim()))
                            ErrorLIst.Add("Vetting Session Simp Chi length should be 1-50 characters.");
                        TextBox txtresulteng = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtresulteng");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtresulteng.Text.Trim()))
                            ErrorLIst.Add("Result Announce Eng length should be 1-50 characters.");
                        TextBox txtresulttradchi = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtresulttradchi");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtresulttradchi.Text.Trim()))
                            ErrorLIst.Add("Result Announce TradChin length should be 1-50 characters.");
                        TextBox txtresultsimchi = (TextBox)grvaddprogram.Rows[i].Cells[0].FindControl("txtresultsimchi");
                        if (!CBPRegularExpression.RegExValidate(CBPRegularExpression.StringExpression(1, 50, true, AllowAllSymbol: true), txtresultsimchi.Text.Trim()))
                            ErrorLIst.Add("Result Announce Simp Chin length should be 1-50 characters.");
                        bool startisbetween = Getdatabyprogramname(ddlProgramName.SelectedValue, Convert.ToDateTime(txtappstartdate.Text), Convert.ToDateTime(txtappdeadline.Text), Programme_IDint);
                        if (startisbetween == false)
                        {
                            ErrorLIst.Add("Application Period should not overlap that of other programs of the same type.");
                        }
                        //bool endisbetween = Getdatabyprogramname(ddlProgramName.SelectedValue, Convert.ToDateTime(txtappdeadline.Text));
                        //if (endisbetween == false)
                        //{
                        //    ErrorLIst.Add("EndDate cannot be set this as another programme is already running in this period.");
                        //}

                        if (ErrorLIst.Count == 0)
                        {

                            SPFunctions objfunction = new SPFunctions();
                            TB_PROGRAMME_INTAKE objIntake = new TB_PROGRAMME_INTAKE();
                            if (Programme_IDint > 0)
                            {
                                objIntake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == Programme_IDint);
                            }
                            objIntake.Programme_Name = ddlProgramName.SelectedValue;
                            objIntake.Intake_Number = Convert.ToInt32(txtintakenumber.Text);
                            objIntake.Application_No_Prefix = txtapplicationno.Text;
                            string sdate = txtappstartdate.Text;

                            string shours = dldstartinghours.SelectedValue;
                            string smins = ddlstartingmins.SelectedValue;
                            string ssec = "59";
                            DateTime startdate = Convert.ToDateTime(sdate + " " + shours + ":" + smins);
                            objIntake.Application_Start = startdate;
                            string ddate = txtappdeadline.Text;
                            string dhours = dldendinghours.SelectedValue;
                            string dmins = dldendingmins.SelectedValue;
                            DateTime enddate = Convert.ToDateTime(ddate + " " + dhours + ":" + dmins + ":" + ssec);
                            objIntake.Application_Deadline = enddate;
                            objIntake.Application_Deadline_Eng = txtappdeadlineengtxt.Text;
                            objIntake.Active = true;
                            objIntake.Application_Deadline_SimpChin = txtappdeadlinesimpchitxt.Text;
                            objIntake.Application_Deadline_TradChin = txtappdeadlinetradchitxt.Text;
                            objIntake.Vetting_Session_Eng = txtvnpeng.Text;
                            objIntake.Vetting_Session_TradChin = txtvnptradchi.Text;
                            objIntake.Vetting_Session_SimpChin = txtvnpsimchi.Text;
                            objIntake.Result_Announce_Eng = txtresulteng.Text;
                            objIntake.Result_Announce_TradChin = txtresulttradchi.Text;
                            objIntake.Result_Announce_Simp_Chin = txtresultsimchi.Text;
                            objIntake.Created_By = objfunction.GetCurrentUser();
                            objIntake.Created_Date = DateTime.Now;
                            objIntake.Modified_By = objfunction.GetCurrentUser();
                            objIntake.Modified_Date = DateTime.Now;

                            // Save showCCMFYEP & showCCMFPRO Checkbox
                                           //showCCMFProf.Checked = Convert.ToBoolean(objIntake.ProfShow);
                            objIntake.ProfShow = cbShowCCMFProf.Checked ? true : false;
                            objIntake.YEPShow = cbShowCCMFYEP.Checked ? true : false;  


                            if (Programme_IDint == 0)
                            {
                                dbContext.TB_PROGRAMME_INTAKE.Add(objIntake);
                                dbContext.Entry(objIntake).State = System.Data.Entity.EntityState.Added;
                            }
                            else
                            {
                                dbContext.Entry(objIntake).State = System.Data.Entity.EntityState.Modified;
                            }


                            dbContext.SaveChanges();
                            // dbCtxTxn.Commit();

                            FillPrograms();
                            Fill_ProgramIntake(objIntake.Programme_ID);

                        }
                        else
                        {
                            bltErrorList.DataSource = ErrorLIst;
                            bltErrorList.DataBind();
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                                //raise a new exception inserting the current one as the InnerException
                                ErrorLIst.Add(message);
                            }
                        }
                        bltErrorList.DataSource = ErrorLIst;
                        bltErrorList.DataBind();
                    }
                    catch (Exception ex)
                    {
                        ErrorLIst.Add(ex.Message);
                        bltErrorList.DataSource = ErrorLIst;
                        bltErrorList.DataBind();

                        //dbCtxTxn.Rollback();
                    }

                    //}
                }
            }




        }




        //catch (Exception ex)
        //{
        //    dbCtxTxn.Rollback();
        //    //SPFunctions.Rollback(dbContext);
        //    //IsError = true;
        //    //ShowbottomMessage("Fill Correct values in " + (i + 1).ToString() + " Core members ", false);
        //    //break;

        //}

        protected bool Getdatabyprogramname(string ProgrammeName, DateTime txtappstartdate, DateTime txtappenddate, int ExistingProgId)
        {
            bool Isbetween = true;
            List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = null;
            using (var dbContext = new CyberportEMS_EDM())
            {
                if (ExistingProgId > 0)
                    objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_Name == ProgrammeName && x.Programme_ID != ExistingProgId && x.Active==true).ToList();
                else
                    objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_Name == ProgrammeName && x.Active == true).ToList();

            }

            foreach (TB_PROGRAMME_INTAKE item in objTB_PROGRAMME_INTAKE)
            {
                if ((txtappstartdate.Ticks > item.Application_Start.Ticks && txtappstartdate.Ticks < item.Application_Deadline.Ticks) || (txtappenddate.Ticks > item.Application_Start.Ticks && txtappenddate.Ticks < item.Application_Deadline.Ticks))
                {
                    Isbetween = false;
                }

            }
            return Isbetween;
        }



        protected void rptrprogrammemanag_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                hdnProgramme_ID.Value = e.CommandArgument.ToString();
                InitializeEditGridView();
            }
            else if (e.CommandName == "Delete")
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    int programme_idint = Convert.ToInt32(e.CommandArgument);
                    TB_PROGRAMME_INTAKE objintake = new TB_PROGRAMME_INTAKE();
                    if (programme_idint > 0)
                    {
                        objintake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programme_idint);
                        objintake.Active = false;
                        dbContext.SaveChanges();
                        Fill_ProgramIntake(programme_idint);
                        FillPrograms();
                    }
                }

            }
        }

        protected void btn_CancelIntake_Click(object sender, EventArgs e)
        {
            hdnProgramme_ID.Value = "0";
            FillPrograms();
        }
        public static SPWeb GetCurrentWeb
        {
            get
            {
                SPSite siteCollection = new SPSite(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url);
                SPWeb site = siteCollection.OpenWeb();
                return site;
            }
        }
        private void Fill_ProgramIntake(int Programme_IDint)
        {
            using (SPWeb site = GetCurrentWeb)
            {


                site.AllowUnsafeUpdates = true;
                SPList list = site.Lists["Programme Intake"];

                SPListItem itemAttachment = GetItemByBdcId(list, Programme_IDint.ToString(), "Programme_ID");
                if (itemAttachment == null)
                {

                    itemAttachment = list.Items.Add();
                    itemAttachment["Programme_ID"] = Programme_IDint;
                }


                using (var dbContext = new CyberportEMS_EDM())
                {
                    TB_PROGRAMME_INTAKE objIntake = new TB_PROGRAMME_INTAKE();
                    objIntake = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == Programme_IDint);

                    
                    if (objIntake.Programme_Name.ToLower().Contains("accelerator support"))
                    {
                        itemAttachment["Programme_Name"] = "CASP";
                    }
                    else if (!objIntake.Programme_Name.ToLower().Contains("university") && !objIntake.Programme_Name.ToLower().Contains("hong kong"))
                    {
                        itemAttachment["Programme_Name"] = "CCMF – Cross Border";
                    }
                    else if (!objIntake.Programme_Name.ToLower().Contains("crossborder") && !objIntake.Programme_Name.ToLower().Contains("university"))
                    {
                        itemAttachment["Programme_Name"] = "CCMF – Hong Kong";
                    }
                    else 
                    {
                        itemAttachment["Programme_Name"] = "CCMF – CUPP";
                    }
                    itemAttachment["Intake_Number"] = objIntake.Intake_Number;
                    itemAttachment["Application_No_Prefix"] = objIntake.Application_No_Prefix;
                    itemAttachment["Application_Start"] = objIntake.Application_Start;
                    itemAttachment["Application_Deadline"] = objIntake.Application_Deadline;
                    itemAttachment["Active"] = objIntake.Active;
                    if (objIntake.Programme_Name.ToLower().Contains("university"))
                    {
                        itemAttachment["Programme_Name_Full"] = "Cyberport University Partnership Programme";
                    }
                    else if(objIntake.Programme_Name.ToLower().Contains("incubation"))
                    {
                        itemAttachment["Programme_Name"] = "CPIP";
                        itemAttachment["Programme_Name_Full"] = "Cyberport Incubation Programme";
                    }
                    else
                    {
                        itemAttachment["Programme_Name_Full"] =  objIntake.Programme_Name;
                        
                    }
                  
                    itemAttachment.Update();

                }

                site.AllowUnsafeUpdates = false;

            }

        }
        public static SPListItem GetItemByBdcId(SPList list, string bdcIdentity, string IdentityName)
        {
            SPListItem myitem = null;
            foreach (SPListItem item in list.Items)
            {
                if (item[IdentityName].ToString() == bdcIdentity)
                {
                    myitem = item;
                }
            }

            return myitem;
        }
        protected void HideShow_CCMFStream(object sender, EventArgs e)
        {
            for (int i = 0; i < grvaddprogram.Rows.Count; i++)
            {
                DropDownList ddlProgramName = (DropDownList)grvaddprogram.Rows[i].Cells[0].FindControl("ddlProgramName");
                CheckBox cbShowCCMFYEP = (CheckBox)grvaddprogram.Rows[i].Cells[0].FindControl("showCCMFYEP");
                CheckBox cbShowCCMFProf = (CheckBox)grvaddprogram.Rows[i].Cells[0].FindControl("showCCMFProf");

                if (ddlProgramName.SelectedValue == "Cyberport Creative Micro Fund - Hong Kong")
                {
                    cbShowCCMFProf.Checked = true;
                    cbShowCCMFYEP.Checked = true;
                    cbShowCCMFProf.Visible = true;
                    cbShowCCMFYEP.Visible = true;
                }
                else
                {
                    cbShowCCMFYEP.Visible = false;
                    cbShowCCMFProf.Visible = false;
                }
            }
 
        }
    }
}

