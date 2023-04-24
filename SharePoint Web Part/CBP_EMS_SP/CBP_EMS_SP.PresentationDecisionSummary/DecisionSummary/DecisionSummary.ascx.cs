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
using System.Web.UI;
using CBP_EMS_SP.Data.CustomModels;
using Microsoft.SharePoint.IdentityModel;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using Novacode;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;


namespace CBP_EMS_SP.PresentationDecisionSummary.DecisionSummary
{
    [ToolboxItemAttribute(false)]
    public partial class DecisionSummary : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DecisionSummary()
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
                if (Context.Request.UrlReferrer != null)
                {
                    btn_Cancel.PostBackUrl = Context.Request.UrlReferrer.ToString();
                    btn_Cancel.OnClientClick = "window.location.href = '" + Context.Request.UrlReferrer.ToString() + "';";

                }
                using (var dbContext = new CyberportEMS_EDM())
                {


                    if (!string.IsNullOrEmpty(Context.Request.QueryString["VMID"]))
                    {
                        CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();
                        string currentemail = spFun.GetCurrentUser();

                        if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                            spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                            spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true))
                        {
                            FillPrograms();
                        }
                        else if (spFun.CurrentUserIsInGroup(WebConfigurationManager.AppSettings["SPVettingMemberGroupName"]))
                        {
                            var vetting_meeting_id = Context.Request.QueryString["VMID"];
                            Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);

                            Guid UserId = dbContext.aspnet_Users.FirstOrDefault(x => x.UserName.ToLower() == currentemail.ToLower()).UserId;
                            TB_VETTING_MEMBER member = dbContext.TB_VETTING_MEMBER.FirstOrDefault(x => x.Vetting_Member_ID == UserId && x.Vetting_Meeting_ID == vetting_metting_id && x.isLeader == true);
                            if (member == null)
                            {
                                Context.Response.Redirect("~/SitePages/Home.aspx");
                            }
                            else
                            {
                                FillPrograms();
                            }


                        }
                        else
                        {
                            Context.Response.Redirect("~/SitePages/Home.aspx");
                        }
                    }
                }
            }
            //New Added in 18012018
            if (CheckDecisionIsCompleted() == true)
            {
                btn_Confirm.Enabled = false;
            }

        }
        List<CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary> objPrsentationResultSummary = new List<CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary>();
        List<PresentationVettingUser> objUserData = new List<PresentationVettingUser>();
        private string connStr
        {
            get
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    return dbContext.Database.Connection.ConnectionString;
                }
            }
        }
        protected List<PresentationVettingUser> GetUserList(CyberportEMS_EDM DbContext, Guid vetting_metting_id)
        {

            return (from Vm in DbContext.TB_VETTING_MEMBER
                    join usr in DbContext.TB_VETTING_MEMBER_INFO on
                    Vm.Vetting_Member_ID equals usr.Vetting_Member_ID
                    where Vm.Vetting_Meeting_ID == vetting_metting_id
                    orderby usr.Email ascending
                    select new PresentationVettingUser()
                    {
                        VettingMember = Vm,
                        UserData = usr
                    }).ToList();
        }

        protected List<PresentationVettingUser> GetUserListFiltered(CyberportEMS_EDM DbContext, Guid vetting_metting_id)
        {

            //List<PresentationVettingUser> objUser = (from usr in DbContext.TB_VETTING_MEMBER_INFO
            //                                         join  Vm in DbContext.TB_VETTING_MEMBER on
            //                                         usr.Vetting_Member_ID equals Vm.Vetting_Member_ID 
            //                                         where Vm.Vetting_Meeting_ID == vetting_metting_id
            //                                         orderby usr.Email ascending
            //                                         select new PresentationVettingUser()
            //                                         {
            //                                             VettingMember = Vm,
            //                                             UserData = usr
            //                                         }).ToList();
            string sql = "";

            var connection = new SqlConnection(connStr);
            connection.Open();
            List<PresentationVettingUser> objPresentationVettingUser = new List<PresentationVettingUser>();
            try
            {
                sql = "select mem.Vetting_Meeting_ID,mem.Vetting_Member_ID,IsLeader,isnull(PList_Confirmed, 0) PList_Confirmed,info.Email," +
                    " vd.Member_Email, info.Salutation, info.Full_Name, info.First_Name, case when len(vd.Vetting_Delclaration_ID) = 36 then 1 else 0 end as IsDeclared from " +
                    " TB_VETTING_MEMBER  mem left join TB_VETTING_MEMBER_INFO info on info.Vetting_Member_ID = mem.Vetting_Member_ID" +
                    " left join TB_VETTING_DECLARATION vd on vd.Vetting_Meeting_ID = mem.Vetting_Meeting_ID and info.Email = vd.Member_Email" +
" where mem.Vetting_Meeting_ID  = '" + vetting_metting_id + "' order by info.Email";
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtResult);

                //List<TB_VETTING_MEMBER> objTB_VETTING_MEMBER = new List<TB_VETTING_MEMBER>();

                foreach (DataRow dr in dtResult.Rows)
                {
                    PresentationVettingUser objUser = new PresentationVettingUser();
                    TB_VETTING_MEMBER objVMem = new TB_VETTING_MEMBER();
                    TB_VETTING_MEMBER_INFO objVInfo = new TB_VETTING_MEMBER_INFO();


                    objVMem.Vetting_Meeting_ID = !string.IsNullOrEmpty(Convert.ToString(dr["Vetting_Meeting_ID"])) ? Guid.Parse(Convert.ToString(dr["Vetting_Meeting_ID"])) : Guid.Empty;
                    objVMem.Vetting_Member_ID = !string.IsNullOrEmpty(Convert.ToString(dr["Vetting_Member_ID"])) ? Guid.Parse(Convert.ToString(dr["Vetting_Member_ID"])) : Guid.Empty;
                    objVInfo.Vetting_Member_ID = objVMem.Vetting_Member_ID;
                    objVMem.isLeader = Convert.ToBoolean(dr["IsLeader"]);
                    objVMem.PList_Confirmed = Convert.ToBoolean(dr["PList_Confirmed"]);
                    objVInfo.Full_Name =  Convert.ToString(dr["Salutation"]) + " " + Convert.ToString(dr["Full_Name"]) + " " + Convert.ToString(dr["First_Name"]);
                    objVInfo.Email = Convert.ToString(dr["Email"]);
                    objUser.Isconfirmed = (bool)objVMem.PList_Confirmed;
                    objUser.IsDeclared = Convert.ToBoolean(dr["IsDeclared"]);
                    objUser.UserData = objVInfo;
                    objUser.VettingMember = objVMem;


                    objPresentationVettingUser.Add(objUser);

                }
                //foreach (PresentationVettingUser Usr in objUser)
                //{
                //    List<TB_VETTING_DECLARATION> objTB_VETTING_DECLARATION = DbContext.TB_VETTING_DECLARATION.Where(x => x.Member_Email == Usr.UserData.Email && x.Vetting_Meeting_ID == Usr.VettingMember.Vetting_Meeting_ID).ToList();
                //    List<TB_DECLARATION_APPLICATION> objTB_DECLARATION_APPLICATION = DbContext.TB_DECLARATION_APPLICATION.ToList();
                //    // List <TB_DECLARATION_APPLICATION> objTB_DECLARATION_APPLICATION = DbContext.TB_DECLARATION_APPLICATION.Where(x => objTB_VETTING_DECLARATION.Exists(y => y.Vetting_Delclaration_ID == x.Vetting_Delclaration_ID)).ToList();
                //    objTB_DECLARATION_APPLICATION = objTB_DECLARATION_APPLICATION.Where(x => (objTB_VETTING_DECLARATION.Exists(y => y.Vetting_Delclaration_ID == x.Vetting_Delclaration_ID))).ToList();
                //    if (objTB_DECLARATION_APPLICATION.Count == objTB_VETTING_DECLARATION.Count)
                //    {
                //        Usr.IsDeclared = true;
                //    }
                //    else
                //    {
                //        Usr.IsDeclared = false;
                //    }

                //    if (Usr.VettingMember.PList_Confirmed.Value == true)
                //    {
                //        Usr.Isconfirmed = true;
                //    }
                //    else
                //    {
                //        Usr.Isconfirmed = false;
                //    }

                //}
            }
            catch (Exception e)
            {

            }



            return objPresentationVettingUser;


        }
        public void FillPrograms()
        {
            if (!string.IsNullOrEmpty(Context.Request.QueryString["VMID"]))
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    var vetting_meeting_id = Context.Request.QueryString["VMID"];
                    Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);



                    objUserData = GetUserListFiltered(dbContext, vetting_metting_id);
                    rptrUserList.DataSource = objUserData;
                    rptrUserList.DataBind();
                    TB_VETTING_MEETING objTB_VETTING_MEETING = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id);
                    objPrsentationResultSummary = Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);
                    int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;
                    List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList();
                    //var connection = new SqlConnection(connStr);
                    List<PrsentationResultSummary> objPrsentationResultSummary1 = new List<PrsentationResultSummary>();
                    // connection.Open();
                    int totalapp = 0;
                    int shortlist = 0;
                    int recom = 0;
                    int recoffsite = 0;
                    int reconsite = 0;
                    int notrecom = 0;
                    try
                    {
                        List<string> strApplicationGoing = new List<string>();
                        if (objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name.ToLower().Contains("incubation"))
                        {
                            totalapp = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == programId && x.Application_Parent_ID == null).Count();
                            shortlist = objPrsentationResultSummary.Count();
                            lblprogramme.Text = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name;
                            lblintake.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Intake_Number);
                            lblmeetingdate.Text = objTB_VETTING_MEETING.Vetting_Meeting_From.ToString("dd MMM yyyy hh:mm tt") + " - " + objTB_VETTING_MEETING.Vetting_Meeting_To.ToString("hh:mm tt");
                            lblvenue.Text = objTB_VETTING_MEETING.Venue;
                            int recommended = 0;
                            int nrecommended = 0;
                            foreach (PrsentationResultSummary item in objPrsentationResultSummary)
                            {
                                if (item.isRecommended > item.isNotRecommended)
                                {
                                    strApplicationGoing.Add(item.Application_Number);
                                    recommended++;
                                }
                                else if (item.isRecommended < item.isNotRecommended)
                                {
                                    nrecommended++;
                                }
                                if (item.isNotRecommended == item.isRecommended)
                                {
                                    hdn_isTbc.Value = "1";
                                }
                                
                                if (item.Score_of_vettingmember.Count() == (item.totalvotes))
                                {
                                    hdn_isConfirm.Value = "1";
                                }
                            }


                            reconsite = objPrsentationResultSummary.Where(x => x.Preferred_track.ToLower().Contains("on")).Count();
                            recoffsite = objPrsentationResultSummary.Where(x => x.Preferred_track.ToLower().Contains("off")).Count();
                            recom = recommended;
                            notrecom = nrecommended;
                        }
                        else
                        {
                            totalapp = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId && x.Application_Parent_ID == null).Count();
                            shortlist = objPrsentationResultSummary.Count();
                            lblprogramme.Text = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name;
                            lblintake.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Intake_Number);
                            lblmeetingdate.Text = objTB_VETTING_MEETING.Vetting_Meeting_From.ToString("dd MMM yyyy hh:mm tt") + " - " + objTB_VETTING_MEETING.Vetting_Meeting_To.ToString("hh:mm tt");
                            lblvenue.Text = objTB_VETTING_MEETING.Venue;
                            int recommended = 0;
                            int nrecommended = 0;
                            foreach (PrsentationResultSummary item in objPrsentationResultSummary)
                            {
                                if (item.isRecommended < item.isNotRecommended)
                                {
                                    nrecommended++;
                                }
                                else if (item.isRecommended > item.isNotRecommended)
                                {
                                    strApplicationGoing.Add(item.Application_Number);

                                    recommended++;
                                }
                                if (item.isNotRecommended == item.isRecommended)
                                {
                                    hdn_isTbc.Value = "1";
                                }

                                if (item.Score_of_vettingmember.Count() == (item.totalvotes))
                                {
                                    hdn_isConfirm.Value = "1";
                            }

                            }
                            recom = recommended;
                            notrecom = nrecommended;

                        }
                        hdn_ApplicationGo.Value = string.Join(";", strApplicationGoing);
                        lblTotalGo.Text = Convert.ToString(recom);
                        lblTotalNotGo.Text = Convert.ToString(notrecom);
                        CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();

                        rptrprogrammesummary.DataSource = objPrsentationResultSummary;
                        rptrprogrammesummary.DataBind();
                        TB_VETTING_MEETING objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id && x.Decision_Completed.HasValue);

                        if (objPrsentationResultSummary.Count > 0 && (spFun.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"]) || spFun.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"])))
                        {
                            btnrefresh.Visible = false;
                            btn_Confirm.Enabled = true;
                            if (objApp != null)
                            {
                                if ((bool)objApp.Decision_Completed)
                                {
                                    //btn_Confirm.Visible = false;
                                    btn_Confirm.Enabled = false;
                                }
                            }

                        }
                        else if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                            spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                            spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true))
                        {
                            btnrefresh.Visible = true;
                            btn_Confirm.Visible = false;
                            if (objApp != null)
                            {
                                if ((bool)objApp.Decision_Completed)
                                {
                                    
                                    btnexport.Visible = true;
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }


        protected void rptrprogrammesummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Header)
            {
                if (objPrsentationResultSummary.Count > 0)
                {
                    Repeater Rptr_HeaderGo = (Repeater)e.Item.FindControl("Rptr_HeaderGo");
                    Rptr_HeaderGo.DataSource = objUserData;
                    Rptr_HeaderGo.DataBind();

                }
            }
        }

        protected void btn_Confirm_Click(object sender, EventArgs e)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {

                Guid vetting_meeting_id = Guid.Parse(Context.Request.QueryString["VMID"]);

                objUserData = GetUserListFiltered(dbContext, vetting_meeting_id);

                foreach (var item in objUserData)
                {
                    if (!item.Isconfirmed)
                    {
                        errormsg.InnerText = "All the vetting team members need to confirm the presentation list before confirm this page.";
                        return;
                    }
                }

                if (hdn_isTbc.Value == "1")
                {
                    errormsg.InnerText = LocalizeCommon("Error_Vetting_Decision_TBC_application");
                }
                else
                {
                    if (hdn_isConfirm.Value != "1")
                    {
                        //lbWarning.Text = "Some of the vetting members have not confirmed yet, please check again before submit.";
                        //lbWarning.Visible = true;
                        errormsg.InnerText = "Some of the vetting members have not confirmed yet, please check again before submit.";
                    }
                    else 
                    { 
                    SPFunctions objFUnction = new SPFunctions();

                    // New Added in 18012018
                  
                        //var vetting_meeting_id = Context.Request.QueryString["VMID"];
                        //Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
                        TB_VETTING_MEETING objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_meeting_id);
                        if (objApp != null)
                        {
                            objApp.Decision_Completed = true;
                        }
                        List<string> ApplicationGoing = new List<string>();

                        if (!string.IsNullOrEmpty(hdn_ApplicationGo.Value))
                        {
                            ApplicationGoing = hdn_ApplicationGo.Value.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToList();
                        }
                        List<TB_VETTING_APPLICATION> objApplications = dbContext.TB_VETTING_APPLICATION.Where(x => x.Vetting_Meeting_ID == vetting_meeting_id).ToList();
                        objApplications.ForEach(x => x.Go = ApplicationGoing.Exists(y => y == x.Application_Number) ? true : false);
                            
                        dbContext.SaveChanges();
                        //pnlsubmissionpopup.Visible = true;

                        SubmitPopup.Visible = true;
                        //btn_Confirm.Enabled = false;

                    }
                      
                    //added in 2018
                    //btn_Confirm.Enabled = false;
                    //commented in 2018
                   // txtUserName.Text = objFUnction.GetCurrentUser();
                }
            }



        }
        protected void btn_HideSubmitPopup_Click(object sender, EventArgs e)
        {
            SubmitPopup.Visible = false;
        }

        protected void btn_submitFinal_Click(object sender, EventArgs e)
        {
            try
            {
                //commented in 2018
                //if (CBPRegularExpression.RegExValidate(CBPRegularExpression.Email, txtUserName.Text) && !string.IsNullOrEmpty(txtLoginPassword.Text))
                //{
                //    bool status = SPClaimsUtility.AuthenticateFormsUser(Context.Request.UrlReferrer, txtUserName.Text, txtLoginPassword.Text);

                //    if (!status)
                //    {
                //        SubmitPopup.Visible = true;
                //        UserCustomerrorLogin.InnerText = Localize("Finalsubmit_emalandpass");
                //    }
                //    else
                //    {
                        SubmitPopup.Visible = false;
                        using (var dbContext = new CyberportEMS_EDM())
                        {
                            var vetting_meeting_id = Context.Request.QueryString["VMID"];
                            Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
                            TB_VETTING_MEETING objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id);
                            if (objApp != null)
                            {
                                objApp.Decision_Completed = true;
                            }
                            List<string> ApplicationGoing = new List<string>();

                            if (!string.IsNullOrEmpty(hdn_ApplicationGo.Value))
                            {
                                ApplicationGoing = hdn_ApplicationGo.Value.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToList();
                            }
                            List<TB_VETTING_APPLICATION> objApplications = dbContext.TB_VETTING_APPLICATION.Where(x => x.Vetting_Meeting_ID == vetting_metting_id).ToList();
                            objApplications.ForEach(x => x.Go = ApplicationGoing.Exists(y => y == x.Application_Number) ? true : false);
                            dbContext.SaveChanges();
                            pnlsubmissionpopup.Visible = true;
                         
                            btn_Confirm.Enabled = false;

                    //commented in 2018
                    //}
                    // }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ext)
            {
                string rs = "";
                //foreach (var eve in ext.EntityValidationErrors)
                //{
                //    rs = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    Console.WriteLine(rs);

                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        rs += "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //    }
               // }
                //commented in 2018
                // UserCustomerrorLogin.InnerText = rs;
            }
            catch (Exception ex)
            {
                //commented in 2018
                //UserCustomerrorLogin.InnerText = ex.Message;
            }

        }
        public static string Localize(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Incubation");
        }

        public static string LocalizeCommon(string Key)
        {
            return SPFunctions.LocalizeUI(Key, "CyberportEMS_Common");
        }
        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            pnlsubmissionpopup.Visible = false;

            // Context.Response.Redirect("~/SitePages/Application%20List%20for%20Vetting%20Team.aspx", false);


        }

        //protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        //{
        //            popupformnotsubmitted.Visible = false;

        //}


        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            FillPrograms();
        }

        protected void rptrUserList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            using (var dbContext = new CyberportEMS_EDM())
            {
                var vetting_meeting_id = Context.Request.QueryString["VMID"];
                Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);

                List<TB_VETTING_DECLARATION> objTB_VETTING_DECLARATION1 = new List<TB_VETTING_DECLARATION>();
                if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
                {
                    CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();
                    bool isconfirmed = (bool)DataBinder.Eval(e.Item.DataItem, "Isconfirmed");
                    //HiddenField isconfirmed = (HiddenField)e.Item.FindControl("isconfirmed");
                    if (isconfirmed == true)
                    {
                        System.Web.UI.WebControls.Image right = (System.Web.UI.WebControls.Image)e.Item.FindControl("right");
                        right.ImageUrl = "/_layouts/15/images/CBP_Images/right.png";
                        //right.Visible = true;
                        //Image imgwrong = (Image)e.Item.FindControl("imgwrong");
                        //imgwrong.Visible = false;
                    }
                    else
                    {
                        //Image imgwrong = (Image)e.Item.FindControl("imgwrong");
                        //imgwrong.Visible = true;
                        System.Web.UI.WebControls.Image right = (System.Web.UI.WebControls.Image)e.Item.FindControl("right");
                        right.ImageUrl = "/_layouts/15/images/CBP_Images/icon_x.gif";
                    }
                    if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                            spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) || 
                            spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true))
                    {


                        HiddenField hdndeclaration = (HiddenField)e.Item.FindControl("hdndeclaration");
                        if (hdndeclaration.Value.ToLower() == "true")
                        {
                            Label lbldeclaration = (Label)e.Item.FindControl("lbldeclaration");

                            lbldeclaration.Visible = true;
                            Label lbldeclarationlabel = (Label)e.Item.FindControl("lbldeclarationlabel");

                            lbldeclarationlabel.Visible = false;
                        }
                        else
                        {
                            Label lbldeclarationlabel = (Label)e.Item.FindControl("lbldeclarationlabel");

                            lbldeclarationlabel.Visible = true;
                            Label lbldeclaration = (Label)e.Item.FindControl("lbldeclaration");

                            lbldeclaration.Visible = false;

                        }


                    }
                }
            }
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Context.Request.QueryString["VMID"]))
            {
                using (var dbContext = new CyberportEMS_EDM())
                {
                    var vetting_meeting_id = Context.Request.QueryString["VMID"];
                    Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);

                    int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;
                    TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList().FirstOrDefault();
                    
                    using (DocX doc = DocX.Create(string.Format("Report-{0}.doc", DateTime.Now.Ticks)))
                    {
                       
                        DocX letter = this.GetExportTemplate(doc, dbContext);
                        letter.PageLayout.Orientation = Novacode.Orientation.Landscape;
                        letter.ReplaceText("%ProgrammeName%", objTB_PROGRAMME_INTAKE.Programme_Name);
                        letter.ReplaceText("%Intakenumber%", objTB_PROGRAMME_INTAKE.Intake_Number.ToString());
                        letter.ReplaceText("%Go%", lblTotalGo.Text);
                        letter.ReplaceText("%NotGo%", lblTotalNotGo.Text);
                        if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
                        {
                            letter.ReplaceText("%shortprogramme%", "Incubation");
                        }
                        else if (!objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("university") && !objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("hong kong"))
                        {
                            letter.ReplaceText("%shortprogramme%", "CCMF – Cross Border");
                        }
                        else if (!objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("crossborder") && !objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("university"))
                        {
                            letter.ReplaceText("%shortprogramme%", "CCMF – Hong Kong");

                        }
                        else
                        {
                            letter.ReplaceText("%shortprogramme%", "CCMF – CUPP");

                        }
                        SPFunctions objfunction = new SPFunctions();
                        letter.ReplaceText("%Recievername%", objfunction.GetCurrentUser());
                        letter.ReplaceText("%Date%", objTB_PROGRAMME_INTAKE.Application_Start.ToShortDateString());
                        IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
                        letter.ReplaceText("%contactno%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Contact").Value);
                        letter.ReplaceText("%email%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Email").Value);
                        
                        //letter.SaveAs(outputFileName);

                        MemoryStream ms = new MemoryStream();
                        letter.SaveAs(ms);



                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Decision Summary " + objTB_PROGRAMME_INTAKE.Programme_Name + " " + objTB_PROGRAMME_INTAKE.Intake_Number.ToString() + ".docx");
                        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        ms.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
                        System.Web.HttpContext.Current.Response.End();


                    }

                }
            }

        }

        private DocX GetExportTemplate(DocX doc, CyberportEMS_EDM dbContext)
        {
           // doc.PageLayout.Orientation = Novacode.Orientation.Landscape;
            // Set up our paragraph contents:
            string headerText = "Vetting Member Decision summary for %ProgrammeName% %Intakenumber%";
            string paraThree = "Summary of %shortprogramme%  %Intakenumber%";
            // Title Formatting:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Arial");
            titleFormat.Size = 15D;
            titleFormat.Position = 12;
            titleFormat.Bold = true;

            // Body Formatting
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Arial");
            paraFormat.Size = 11D;
            titleFormat.Position = 12;
            var listformatting = new Formatting();
            listformatting.FontFamily = new System.Drawing.FontFamily("Arial");
            listformatting.Size = 11D;
            titleFormat.Position = 12;

            // Create the doculment in memory:
            // var doc = DocX.Create(fileName);


            doc.MarginLeft = 10f;
            doc.MarginRight = 10f;
            Paragraph title = doc.InsertParagraph(headerText, false, titleFormat);
            title.Alignment = Alignment.center;

            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph(paraThree, false, paraFormat);
            var list = doc.AddList(listType: Novacode.ListItemType.Bulleted, startNumber: 1);

            doc.AddListItem(list, "Programme Name: %ProgrammeName%", 0, listType: Novacode.ListItemType.Bulleted);
            doc.AddListItem(list, "Intake: %Intakenumber%", 0, listType: Novacode.ListItemType.Bulleted);
            doc.AddListItem(list, "Go:  %Go%", 0, listType: Novacode.ListItemType.Bulleted);
            doc.AddListItem(list, "Not Go: %NotGo%", 0, listType: Novacode.ListItemType.Bulleted);
            doc.InsertList(list);
            doc.InsertParagraph(Environment.NewLine);


            var vetting_meeting_id = Context.Request.QueryString["VMID"];
            Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
            objUserData = GetUserListFiltered(dbContext, vetting_metting_id);
            TB_VETTING_MEETING objTB_VETTING_MEETING = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id);
            objPrsentationResultSummary = Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);

            Novacode.Table tdata = doc.AddTable(objPrsentationResultSummary.Count + 1, 7);
            tdata.Alignment = Alignment.left;
            tdata.SetColumnWidth(0, 500);
            tdata.SetColumnWidth(1, 2200);
            tdata.SetColumnWidth(2, 2200);
            tdata.SetColumnWidth(3, 800);
            tdata.SetColumnWidth(4, 1000);
            tdata.SetColumnWidth(5, 2000);
            tdata.SetColumnWidth(6, 3000);
            tdata.SetTableCellMargin(TableCellMarginType.left, 100);
            tdata.SetTableCellMargin(TableCellMarginType.right, 100);


            tdata.SetBorder(TableBorderType.InsideH, new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.one, 1, Color.Black));

            tdata.Rows[0].Cells[0].Paragraphs.First().Append("No.");
            tdata.Rows[0].Cells[1].Paragraphs.First().Append("Application No.");
            tdata.Rows[0].Cells[2].Paragraphs.First().Append("Project Name");
            tdata.Rows[0].Cells[3].Paragraphs.First().Append("Go");
            tdata.Rows[0].Cells[4].Paragraphs.First().Append("Not Go");
            tdata.Rows[0].Cells[5].Paragraphs.First().Append("Total No. of Votes");
            tdata.Rows[0].Cells[6].Paragraphs.First().Append("Remarks");

            for (int i = 0; i < objPrsentationResultSummary.Count; i++)
            {
                tdata.Rows[i + 1].Cells[0].Paragraphs.First().Append((i + 1).ToString());
                tdata.Rows[i + 1].Cells[1].Paragraphs.First().Append(objPrsentationResultSummary[i].Application_Number.ToString());
                tdata.Rows[i + 1].Cells[2].Paragraphs.First().Append(objPrsentationResultSummary[i].company_name.ToString());
                tdata.Rows[i + 1].Cells[3].Paragraphs.First().Append(objPrsentationResultSummary[i].isRecommended.ToString());
                tdata.Rows[i + 1].Cells[4].Paragraphs.First().Append(objPrsentationResultSummary[i].isNotRecommended.ToString());
                tdata.Rows[i + 1].Cells[5].Paragraphs.First().Append(objPrsentationResultSummary[i].totalvotes.ToString());

                string Remarks = "";
                if (objPrsentationResultSummary[i].isRecommended > objPrsentationResultSummary[i].isNotRecommended)
                {
                    Remarks = "Go";
                }
                else if (objPrsentationResultSummary[i].isRecommended < objPrsentationResultSummary[i].isNotRecommended)
                {
                    Remarks = "Not Go";
                }
                tdata.Rows[i + 1].Cells[6].Paragraphs.First().Append(Remarks);


            }
            doc.InsertTable(tdata);
            return doc;
        }


        public static List<PrsentationResultSummary> Get_programme_summary_ccmf(Guid vettingid, CyberportEMS_EDM dbcontext, List<PresentationVettingUser> objUserData)
        {
            List<PrsentationResultSummary> listPrsentationResultSummary = new List<PrsentationResultSummary>();
            List<TB_VETTING_APPLICATION> objTB_VETTING_APPLICATION = dbcontext.TB_VETTING_APPLICATION.Where(x => x.Vetting_Meeting_ID == vettingid).ToList();
            List<TB_VETTING_APPLICATION> ObjApp = objTB_VETTING_APPLICATION.Where(x => x.Application_Number.ToLower() == "time break").ToList();
            objTB_VETTING_APPLICATION = objTB_VETTING_APPLICATION.Except(ObjApp).ToList();
            //List<TB_PRESENTATION_CCMF_SCORE> objTB_PRESENTATION_CCMF_SCORE = new List<TB_PRESENTATION_CCMF_SCORE>();
            if (objTB_VETTING_APPLICATION.Count > 0)
            {
                if (objTB_VETTING_APPLICATION.FirstOrDefault().Application_Number.ToLower().Contains("cpip"))
                {


                    foreach (TB_VETTING_APPLICATION item in objTB_VETTING_APPLICATION)
                    {
                        PrsentationResultSummary objPrsentationResultSummary = new PrsentationResultSummary();
                        objPrsentationResultSummary.Application_Number = item.Application_Number;
                        objPrsentationResultSummary.PresentationTime = item.Presentation_From;

                        TB_INCUBATION_APPLICATION objProgram = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Status != "Saved" && x.Status != "Deleted");
                        if (objProgram != null)
                        {
                            objPrsentationResultSummary.company_name = objProgram.Company_Name_Eng;
                            objPrsentationResultSummary.ProgramId = objProgram.Programme_ID;
                            objPrsentationResultSummary.Cluster = objProgram.Business_Area;
                            objPrsentationResultSummary.Programme_Type = "";
                            objPrsentationResultSummary.Application_Type = "";
                            objPrsentationResultSummary.Preferred_track = objProgram.Preferred_Track;

                        }

                        if (dbcontext.TB_VETTING_DECISION.Where(x => x.Application_Number == item.Application_Number && x.Vetting_Meeting_ID == vettingid).Count() > 0)
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {
                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                TB_VETTING_DECISION objTbScore = dbcontext.TB_VETTING_DECISION.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Member_Email.ToLower() == objPresUser.UserData.Email.ToLower() && x.Vetting_Meeting_ID == vettingid);
                                if (objTbScore != null)
                                {
                                    objScoreNew.Go = objTbScore.Go;
                                    objScoreNew.Member_Email = objTbScore.Member_Email;
                                    objScoreNew.Remarks = "";
                                    objScoreNew.Total_Score = 0;


                                }

                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }

                        }
                        else
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {

                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }
                        }
                        objPrsentationResultSummary.Totalscore = objPrsentationResultSummary.Score_of_vettingmember.Sum(x => x.Total_Score);
                        objPrsentationResultSummary.Averagescore = objPrsentationResultSummary.Score_of_vettingmember.Average(x => x.Total_Score);
                        objPrsentationResultSummary.isRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == true).Select(x => x.Go).Count();
                        objPrsentationResultSummary.isNotRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == false).Select(x => x.Go).Count();
                        objPrsentationResultSummary.totalvotes = objPrsentationResultSummary.isRecommended + objPrsentationResultSummary.isNotRecommended;

                        listPrsentationResultSummary.Add(objPrsentationResultSummary);
                    }
                }
                else if (objTB_VETTING_APPLICATION.FirstOrDefault().Application_Number.ToLower().Contains("ccmf") || objTB_VETTING_APPLICATION.FirstOrDefault().Application_Number.ToLower().Contains("gbayep") || objTB_VETTING_APPLICATION.FirstOrDefault().Application_Number.ToLower().Contains("cupp"))
                {
                    foreach (TB_VETTING_APPLICATION item in objTB_VETTING_APPLICATION)
                    {
                        PrsentationResultSummary objPrsentationResultSummary = new PrsentationResultSummary();
                        objPrsentationResultSummary.Application_Number = item.Application_Number;
                        objPrsentationResultSummary.PresentationTime = item.Presentation_From;
                        //Guid Vetting_declaration_id = dbcontext.TB_VETTING_DECISION.FirstOrDefault(x => x.Application_Number == item.Application_Number).Vetting_Delclaration_ID;
                        //TB_VETTING_DECLARATION objTB_VETTING_DECLARATION = dbcontext.TB_VETTING_DECLARATION.FirstOrDefault(x => x.Vetting_Delclaration_ID == Vetting_declaration_id);
                        TB_CCMF_APPLICATION objProgram = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Status != "Saved" && x.Status != "Deleted");
                        if (objProgram != null)
                        {
                            objPrsentationResultSummary.company_name = objProgram.Project_Name_Eng;
                            objPrsentationResultSummary.ProgramId = objProgram.Programme_ID;
                            objPrsentationResultSummary.Cluster = objProgram.Business_Area;
                            objPrsentationResultSummary.Programme_Type = objProgram.Programme_Type;
                            objPrsentationResultSummary.Application_Type = objProgram.CCMF_Application_Type;

                        }

                        if (dbcontext.TB_VETTING_DECISION.Where(x => x.Application_Number == item.Application_Number && x.Vetting_Meeting_ID == vettingid).Count() > 0)
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {
                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                TB_VETTING_DECISION objTbScore = dbcontext.TB_VETTING_DECISION.FirstOrDefault(x => x.Application_Number == item.Application_Number && x.Member_Email.ToLower() == objPresUser.UserData.Email.ToLower() && x.Vetting_Meeting_ID == vettingid);
                                if (objTbScore != null)
                                {
                                    objScoreNew.Go = objTbScore.Go;
                                    objScoreNew.Member_Email = objTbScore.Member_Email;
                                    objScoreNew.Remarks = "";
                                    objScoreNew.Total_Score = 0;


                                }

                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }

                        }
                        else
                        {
                            objPrsentationResultSummary.Score_of_vettingmember = new List<Presentation_Score>();
                            foreach (PresentationVettingUser objPresUser in objUserData)
                            {
                                Presentation_Score objScoreNew = new Presentation_Score()
                                {

                                    Go = null,
                                    Member_Email = string.Empty,
                                    Remarks = string.Empty,
                                    Total_Score = null
                                };
                                objPrsentationResultSummary.Score_of_vettingmember.Add(objScoreNew);
                            }
                        }

                        objPrsentationResultSummary.Totalscore = Math.Round(Convert.ToDecimal(objPrsentationResultSummary.Score_of_vettingmember.Sum(x => x.Total_Score)), 3);
                        objPrsentationResultSummary.Averagescore = Math.Round(Convert.ToDecimal(objPrsentationResultSummary.Score_of_vettingmember.Average(x => x.Total_Score)), 3);
                        objPrsentationResultSummary.isRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == true).Select(x => x.Go).Count();

                        objPrsentationResultSummary.isNotRecommended = objPrsentationResultSummary.Score_of_vettingmember.Where(x => x.Go == false).Select(x => x.Go).Count();
                        objPrsentationResultSummary.totalrecommended = (objPrsentationResultSummary.isRecommended > objPrsentationResultSummary.isNotRecommended) ? 1 : 0;
                        objPrsentationResultSummary.totalvotes = objPrsentationResultSummary.isRecommended + objPrsentationResultSummary.isNotRecommended;
                        listPrsentationResultSummary.Add(objPrsentationResultSummary);
                    }




                }
            }





            return listPrsentationResultSummary.OrderBy(k=>k.PresentationTime).ToList();

        }

        protected void ImageButtonClose_Click(object sender, ImageClickEventArgs e)
        {
            SubmitPopup.Visible = false;

           // Context.Response.Redirect("~/SitePages/Application%20List%20for%20Vetting%20Team.aspx", false);
        }
        //new added
        private bool CheckDecisionIsCompleted()
        {
            bool result = false;
            var vetting_meeting_id = Context.Request.QueryString["VMID"];
            Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
            string sql = "SELECT Decision_Completed FROM TB_VETTING_MEETING WHERE Vetting_Meeting_ID = @vmID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@vmID", vetting_metting_id);

                    if (command.ExecuteScalar() != null && DBNull.Value != command.ExecuteScalar())
                    {
                        result = (bool)command.ExecuteScalar();
                    }

                    conn.Close();
                }
            }
            return result;
        }

    }
}
