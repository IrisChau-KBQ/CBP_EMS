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
using Spire;
using Spire.Doc;
using Spire.Utils;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using Spire.Xls;

namespace VPResultSummary.VisualWebPart1
{
  [ToolboxItemAttribute(false)]
  public partial class VPResultSummary : WebPart
  {
    // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
    // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
    // for production. Because the SecurityPermission attribute bypasses the security check for callers of
    // your constructor, it's not recommended for production purposes.
    // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

    List<CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary> objPrsentationResultSummary = new List<CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary>();
    List<PresentationVettingUser> objUserData = new List<PresentationVettingUser>();
    int totalCCMF_Application = 0;
    int totalCCMFProfessionalStream = 0;
    int totalCCMF_HK_YoungStream = 0;
    int totalshortlisted = 0;
    int totalRecomendedProff = 0;
    int totalRecomendedHK_young = 0;
    int totalNotRecomended = 0;
    int totalWithdraw = 0;
    MemoryStream msTable = new MemoryStream();

    public VPResultSummary()
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
          // set the client click handler
          btn_Cancel.OnClientClick = "window.location.href = '" + Context.Request.UrlReferrer.ToString() + "';";
        }

        BindDataSortColumn();

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


    }


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
          //objVInfo.Full_Name = Convert.ToString(dr["Full_Name"]);
          objVInfo.Full_Name = Convert.ToString(dr["Salutation"]) + " " + Convert.ToString(dr["Full_Name"]) + " " + Convert.ToString(dr["First_Name"]);
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
    private bool IsMeetingCompleted = false;
    private TB_VETTING_MEETING objTB_VETTING_MEETING = null;
    int programId = 0;
    private List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = new List<TB_PROGRAMME_INTAKE>();
    private List<PrsentationResultSummary> objPrsentationResultSummary1 = new List<PrsentationResultSummary>();
    int totalapp = 0;
    int shortlist = 0;
    int recom = 0;
    int recoffsite = 0;
    int reconsite = 0;
    int notrecom = 0;
    int WithdrawCount = 0;
    int tbc = 0;
    string meetingdate = "";
    int recommended = 0;
    int nrecommended = 0;
    TB_VETTING_MEETING objApp = null;
    protected void PrepareProgramData()
    {
      if (!string.IsNullOrEmpty(Context.Request.QueryString["VMID"]))
      {
        using (var dbContext = new CyberportEMS_EDM())
        {
          var vetting_meeting_id = Context.Request.QueryString["VMID"];
          Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);

          objUserData = GetUserListFiltered(dbContext, vetting_metting_id);

          objTB_VETTING_MEETING = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id);
          objPrsentationResultSummary = IncubationContext.Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);

          programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;
          objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList();
          //var connection = new SqlConnection(connStr);
          objPrsentationResultSummary1 = new List<PrsentationResultSummary>();
          // connection.Open();
          totalapp = 0;
          shortlist = 0;
          recom = 0;
          recoffsite = 0;
          reconsite = 0;
          notrecom = 0;
          WithdrawCount = 0;
          tbc = 0;
          try
          {
            if (objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name.ToLower().Contains("incubation"))
            {
              totalapp = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Programme_ID == programId && (x.Status == "Complete Screening" || x.Status == "Awarded" || x.Status == "Presentation Withdraw")).Count();
              shortlist = objPrsentationResultSummary.Count();
              meetingdate = objTB_VETTING_MEETING.Presentation_From.ToString("dd MMM yyyy. hh:mm tt") + " - " + objTB_VETTING_MEETING.Presentation_To.ToString("hh:mm tt");
              recommended = 0;
              nrecommended = 0;
              foreach (PrsentationResultSummary item in objPrsentationResultSummary)
              {
                if (item.Withdraw.HasValue && item.Withdraw == true)
                {
                  WithdrawCount += 1;
                }
                else if (item.isRecommended > item.isNotRecommended)
                {
                  recommended++;
                  if (item.Preferred_track.ToLower().Contains("on-site"))
                  {
                    reconsite++;
                  }
                  if (item.Preferred_track.ToLower().Contains("off-site"))
                  {
                    recoffsite++;
                  }
                }
                else if (item.isRecommended < item.isNotRecommended)
                {
                  nrecommended++;
                }
                else if (item.isRecommended == item.isNotRecommended)
                {
                  tbc++;
                }
                else if (item.totalvotes == 0)
                {
                  tbc++;
                }

              }
              notrecom = nrecommended;
              recom = recommended;
              shortlist = objPrsentationResultSummary.Count();

            }
            else
            {
              totalapp = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId && (x.Status == "Complete Screening" || x.Status == "Awarded" || x.Status == "Presentation Withdraw")).Count();
              shortlist = objPrsentationResultSummary.Count();
              meetingdate = objTB_VETTING_MEETING.Presentation_From.ToString("dd MMM yyyy. hh:mm tt") + " - " + objTB_VETTING_MEETING.Presentation_To.ToString("hh:mm tt");
              recommended = 0;
              nrecommended = 0;
              foreach (PrsentationResultSummary item in objPrsentationResultSummary)
              {

                if (item.Withdraw.HasValue && item.Withdraw == true)
                {
                  WithdrawCount += 1;
                }
                else if (item.isRecommended > item.isNotRecommended)
                {
                  recommended++;
                }
                else if (item.isRecommended < item.isNotRecommended)
                {
                  nrecommended++;
                }
                else if (item.isRecommended == item.isNotRecommended)
                {
                  tbc++;
                }
                else if (item.totalvotes == 0)
                {
                  tbc++;
                }
              }
              recom = recommended;
              notrecom = nrecommended;
              shortlist = objPrsentationResultSummary.Count();

            }

            CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();
            objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id && x.Meeting_Completed.HasValue);


            IsMeetingCompleted = true;

            if (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                 spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                 spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true))
            {

              IsMeetingCompleted = objApp == null ? false : objApp.Meeting_Completed.Value;
            }



          }
          catch (Exception e)
          {
            throw e;
          }
        }
      }
    }
    public void FillPrograms()
    {
      if (!string.IsNullOrEmpty(Context.Request.QueryString["VMID"]))
      {
        PrepareProgramData();

        using (var dbContext = new CyberportEMS_EDM())
        {
          var vetting_meeting_id = Context.Request.QueryString["VMID"];
          Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
          //objUserData = GetUserListFiltered(dbContext, vetting_metting_id);
          rptrUserList.DataSource = objUserData;
          rptrUserList.DataBind();
          //TB_VETTING_MEETING objTB_VETTING_MEETING = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id);
          //objPrsentationResultSummary = IncubationContext.Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);
          ////objPrsentationResultSummary.RemoveAll(x => x.isRecommended < x.isNotRecommended);
          ////objPrsentationResultSummary.RemoveAll(x => x.isRecommended == x.isNotRecommended);

          //int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;
          //List<TB_PROGRAMME_INTAKE> objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList();
          ////var connection = new SqlConnection(connStr);
          //List<PrsentationResultSummary> objPrsentationResultSummary1 = new List<PrsentationResultSummary>();
          //// connection.Open();
          //int totalapp = 0;
          //int shortlist = 0;
          //int recom = 0;
          //int recoffsite = 0;
          //int reconsite = 0;
          //int notrecom = 0;
          //int WithdrawCount = 0;
          //int tbc = 0;
          try
          {
            if (objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name.ToLower().Contains("incubation"))
            {
              lbloffsite.Visible = true;
              lblonsite.Visible = true;
              lblrecommended.Visible = false;
              tdrecommended.Visible = false;

              lblprogramme.Text = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name;
              lblintake.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Intake_Number);
              lblmeetingdate.Text = meetingdate;
              lblvenue.Text = objTB_VETTING_MEETING.Venue;
              //foreach (PrsentationResultSummary item in objPrsentationResultSummary)
              //{
              //  if (item.Withdraw.HasValue && item.Withdraw == true)
              //  {
              //    WithdrawCount += 1;
              //  }
              //  else if (item.isRecommended > item.isNotRecommended)
              //  {
              //    recommended++;
              //    if (item.Preferred_track.ToLower().Contains("on-site"))
              //    {
              //      reconsite++;
              //    }
              //    if (item.Preferred_track.ToLower().Contains("off-site"))
              //    {
              //      recoffsite++;
              //    }
              //  }
              //  else if (item.isRecommended < item.isNotRecommended)
              //  {
              //    nrecommended++;
              //  }
              //  else if (item.isRecommended == item.isNotRecommended)
              //  {
              //    tbc++;
              //  }
              //  else if (item.totalvotes == 0)
              //  {
              //    tbc++;
              //  }

              //}
              //notrecom = nrecommended;
              //recom = recommended;
              //shortlist = objPrsentationResultSummary.Count();

            }
            else
            {
              lbloffsite.Visible = false;
              lblonsite.Visible = false;
              lblrecommended.Visible = true;
              tdrecommended.Visible = true;
              //totalapp = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId && (x.Status == "Complete Screening" || x.Status == "Awarded" || x.Status == "Presentation Withdraw")).Count();
              //shortlist = objPrsentationResultSummary.Count();
              lblprogramme.Text = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name;
              lblintake.Text = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Intake_Number);
              lblmeetingdate.Text = meetingdate;
              lblvenue.Text = objTB_VETTING_MEETING.Venue;
              //int recommended = 0;
              //int nrecommended = 0;
              //foreach (PrsentationResultSummary item in objPrsentationResultSummary)
              //{

              //  if (item.Withdraw.HasValue && item.Withdraw == true)
              //  {
              //    WithdrawCount += 1;
              //  }
              //  else if (item.isRecommended > item.isNotRecommended)
              //  {
              //    recommended++;
              //  }
              //  else if (item.isRecommended < item.isNotRecommended)
              //  {
              //    nrecommended++;
              //  }
              //  else if (item.isRecommended == item.isNotRecommended)
              //  {
              //    tbc++;
              //  }
              //  else if (item.totalvotes == 0)
              //  {
              //    tbc++;
              //  }
              //}
              //recom = recommended;
              //notrecom = nrecommended;
              //shortlist = objPrsentationResultSummary.Count();

            }

            lbltotalprojects.Text = Convert.ToString(totalapp);
            lblShort_Listed.Text = Convert.ToString(shortlist);
            if (lblprogramme.Text.Contains("Incubation"))
            {

              lblrecommended.Text = Convert.ToString(reconsite + "(on-site)" + " " + recoffsite + "(off-site)");

            }
            else
            {
              lblrecommended.Text = Convert.ToString(recom);
            }
            lblnotrecommended.Text = Convert.ToString(notrecom);
            lblWithdrawCount.Text = Convert.ToString(WithdrawCount);
            lblTBC.Text = Convert.ToString(tbc);
            if (objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name.ToLower().Contains("incubation"))
            {
              lbloffsite.Text = Convert.ToString(recoffsite + "(off-site)");

              lblonsite.Text = Convert.ToString(reconsite + "(on-site)");
            }
            CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();
            //TB_VETTING_MEETING objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id && x.Meeting_Completed.HasValue);


            //IsMeetingCompleted = true;
            if (objPrsentationResultSummary.Count > 0 && (spFun.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"]) || spFun.CurrentUserIsInGroup((String)WebConfigurationManager.AppSettings["SPVettingMemberGroupName"])))
            {

              btnrefresh.Visible = false;
              btn_Confirm.Enabled = true;
              if (objApp != null)
              {
                if ((bool)objApp.Meeting_Completed)
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
              btn_Confirm.Visible = false; // no change 
              if (objApp != null)
              {
                if ((bool)objApp.Meeting_Completed)
                {
                  if (objPrsentationResultSummary.Count > 0)
                  {
                    btnexport.Visible = true;
                  }
                }

              }
              //IsMeetingCompleted = objApp == null ? false : objApp.Meeting_Completed.Value;
            }

            if (radioSort.SelectedValue == "desc")
            {
              if (lstSort.SelectedValue == "Result")
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary.OrderByDescending(o => o.Result);
              }
              else if (lstSort.SelectedValue == "Recommended")
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary.OrderByDescending(o => o.isRecommended);
              }
              else if (lstSort.SelectedValue == "Score")
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary.OrderByDescending(o => o.Averagescore);
              }
              else
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary;
              }
            }
            else
            {
              if (lstSort.SelectedValue == "Result")
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary.OrderBy(o => o.Result);
              }
              else if (lstSort.SelectedValue == "Recommended")
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary.OrderBy(o => o.isRecommended);
              }
              else if (lstSort.SelectedValue == "Score")
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary.OrderBy(o => o.Averagescore);
              }
              else
              {
                rptrprogrammesummary.DataSource = objPrsentationResultSummary;
              }

            }
            rptrprogrammesummary.DataBind();

          }
          catch (Exception e)
          {
            throw e;
          }
        }
      }
    }

    private void BindDataSortColumn()
    {
      List<SortColumnClass> sortList = new List<SortColumnClass>();


      //Sequence
      sortList.Add(new SortColumnClass
      {
        ColumnValue = "Default",
        ColumnText = "Default"
      });
      sortList.Add(new SortColumnClass
      {
        ColumnValue = "Score",
        ColumnText = "Average Score"
      });
      sortList.Add(new SortColumnClass
      {
        ColumnValue = "Recommended",
        ColumnText = "Recommended"
      });
      sortList.Add(new SortColumnClass
      {
        ColumnValue = "Result",
        ColumnText = "Result"
      });

      lstSort.DataSource = sortList;
      lstSort.DataBind();
      lstSort.SelectedValue = "Default";

    }

    protected void rptrprogrammesummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
      CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();

      if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Header)
      {
        if (objPrsentationResultSummary.Count > 0)
        {
          Repeater Rptr_Header = (Repeater)e.Item.FindControl("Rptr_Header");
          Rptr_Header.DataSource = objUserData;
          Rptr_Header.DataBind();
          Repeater Rptr_HeaderGo = (Repeater)e.Item.FindControl("Rptr_HeaderGo");
          Rptr_HeaderGo.DataSource = objUserData;
          Rptr_HeaderGo.DataBind();

        }
        //if (IsMeetingCompleted || !spFun.CurrentUserIsInGroup("CBP EMS DEV Vetting Team"))
        //{
        //    var label = (Label)e.Item.FindControl("lbl_Edit");
        //    label.Visible = false;
        //}
        var label = (Label)e.Item.FindControl("lbl_Edit");

        if (!IsMeetingCompleted && (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                    spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                    spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true)))
        {
          label.Visible = true;
          label.Enabled = true;
        }
        else if (IsMeetingCompleted && (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                    spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                    spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true)))
        {
          label.Visible = true;
          label.Enabled = false;

        }
        else
        {
          label.Visible = false;
          label.Enabled = false;
        }
      }
      else if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
      {
        var imagebutton = (ImageButton)e.Item.FindControl("EditButton");
        if (!IsMeetingCompleted && (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                    spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                    spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true)))
        {
          imagebutton.Visible = true;
          imagebutton.Enabled = true;
        }
        else if (IsMeetingCompleted && (spFun.CurrentUserIsInGroup("CPIP Coordinator", true) || spFun.CurrentUserIsInGroup("CCMF Coordinator", true) ||
                    spFun.CurrentUserIsInGroup("CPIP BDM", true) || spFun.CurrentUserIsInGroup("CCMF BDM", true) ||
                    spFun.CurrentUserIsInGroup("Senior Manager", true) || spFun.CurrentUserIsInGroup("CPMO", true)))
        {
          imagebutton.Visible = true;
          imagebutton.Enabled = false;
          imagebutton.ImageUrl = "/_layouts/15/Images/CBP_Images/edit_gray.png";
        }
        else
        {
          imagebutton.Visible = false;
          imagebutton.Enabled = false;
        }

      }
    }

    protected void lstSort_SelectedIndexChanged(object sender, EventArgs e)
    {
      FillPrograms();
    }

    protected void radioSort_SelectedIndexChanged(object sender, EventArgs e)
    {
      FillPrograms();
    }

    protected void btn_Confirm_Click(object sender, EventArgs e)
    {


      using (var dbContext = new CyberportEMS_EDM())
      {

        var vetting_meeting_id = Context.Request.QueryString["VMID"];
        Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
        List<TB_VETTING_DECLARATION> objTB_VETTING_DECLARATION = dbContext.TB_VETTING_DECLARATION.Where(x => x.Vetting_Meeting_ID == vetting_metting_id).ToList();
        List<TB_VETTING_MEMBER> objTB_VETTING_MEMBER = dbContext.TB_VETTING_MEMBER.Where(x => x.Vetting_Meeting_ID == vetting_metting_id).ToList();

        TB_VETTING_MEETING objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id && x.Decision_Completed.HasValue);

        if (objApp == null)
        {
          errormsg.InnerText = LocalizeCommon("Error_Vetting_meeting_Decision_confirm");

        }
        else if (!(bool)objApp.Decision_Completed)
        {
          errormsg.InnerText = LocalizeCommon("Error_Vetting_meeting_Decision_confirm");
        }
        //else if (objTB_VETTING_DECLARATION.Count() != objTB_VETTING_MEMBER.Count())
        //{
        //    errormsg.InnerText = LocalizeCommon("Error_Vetting_meeting_confirm");
        //}
        else
        {
          bool isconfirmed = true;
          foreach (TB_VETTING_MEMBER item in objTB_VETTING_MEMBER)
          {
            if (item.PList_Confirmed != true)
            {
              isconfirmed = false;
            }
          }
          if (!isconfirmed)
          {
            errormsg.InnerText = "All the vetting team members need to confirm the presentation list before confirm this page.";

          }


          if (isconfirmed)
          {
            objUserData = GetUserList(dbContext, vetting_metting_id);
            List<CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary> objPrsentationResultSummary1 = IncubationContext.Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);


            //if (objPrsentationResultSummary1.Where(x => x.totalvotes > 0 && x.isRecommended == x.isNotRecommended).Count() > 0)
            //{

            //    isconfirmed = false;
            //    errormsg.InnerText = LocalizeCommon("Error_Vetting_Decision_TBC_application");
            //}


            bool isTBC = false;
            foreach (var item in objPrsentationResultSummary1)
            {
              if (!item.Withdraw.HasValue && item.Withdraw != true)
              {

                if (item.totalvotes == 0)
                {
                  isTBC = true;
                  break;
                }

                if (item.isRecommended == item.isNotRecommended)
                {
                  isTBC = true;
                  break;
                }
              }
            }

            try
            {
              if (Convert.ToInt32(lblTBC.Text) > 0)
              {
                isTBC = true;
              }
            }
            catch
            {
            }

            if (isTBC)
            {
              isconfirmed = false;
              errormsg.InnerText = LocalizeCommon("Error_Vetting_Decision_TBC_application");
            }

            List<CBP_EMS_SP.Data.CustomModels.PrsentationResultSummary> objPrsentationResultSummaryDiffer = objPrsentationResultSummary1.Where(x => x.totalvotes < objUserData.Count()).ToList();

            //if (objPrsentationResultSummaryDiffer.Count() > 0)
            //{
            //    if (objPrsentationResultSummaryDiffer.Where(x => x.Withdraw == null).Count() > 0 || objPrsentationResultSummaryDiffer.Where(x => x.Withdraw.Value == false).Count() > 0)
            //    {
            //        isconfirmed = false;
            //        errormsg.InnerText = LocalizeCommon("Error_Vetting_Confimation_Error_NOWithdraw");
            //    }

            //}

          }


          if (isconfirmed)
          {
            SPFunctions objFUnction = new SPFunctions();
            SubmitPopup.Visible = true;
            // commented by ANil
            //txtUserName.Text = objFUnction.GetCurrentUser();

          }

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
        List<TB_EC_RESULT> listobjTB_EC_RESULT = new List<TB_EC_RESULT>();
        using (var dbContext = new CyberportEMS_EDM())
        {
          var vetting_meeting_id = Context.Request.QueryString["VMID"];
          Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
          objUserData = GetUserList(dbContext, vetting_metting_id);
          objPrsentationResultSummary = IncubationContext.Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);
          //objPrsentationResultSummary.RemoveAll(x => x.isRecommended < x.isNotRecommended);
          //objPrsentationResultSummary.RemoveAll(x => x.isRecommended == x.isNotRecommended);
          foreach (PrsentationResultSummary item in objPrsentationResultSummary)
          {


            TB_EC_RESULT objTB_EC_RESULT = new TB_EC_RESULT();
            objTB_EC_RESULT.Application_Number = item.Application_Number;
            objTB_EC_RESULT.Created_Date = DateTime.Now;
            objTB_EC_RESULT.EC_Result_ID = Guid.NewGuid();
            objTB_EC_RESULT.Created_By = new SPFunctions().GetCurrentUser();

            objTB_EC_RESULT.Application_Type = item.Application_Type;
            objTB_EC_RESULT.Programme_Type = item.Programme_Type;
            objTB_EC_RESULT.Cluster = item.Cluster;
            objTB_EC_RESULT.Company_Program = item.company_name;
            objTB_EC_RESULT.Programme_ID = item.ProgramId;
            if (item.isRecommended > item.isNotRecommended)
              if (item.isRecommended > item.isNotRecommended)
                objTB_EC_RESULT.Recommended = true;
              else
                objTB_EC_RESULT.Recommended = false;
            objTB_EC_RESULT.Recommendedcount = item.isRecommended;
            objTB_EC_RESULT.NotRecommendedcount = item.isNotRecommended;

            objTB_EC_RESULT.Total_votes = Convert.ToInt32(item.Totalscore);

            objTB_EC_RESULT.Remarks = String.Join(",", item.Score_of_vettingmember.Select(x => x.Remarks != "").ToList());

            listobjTB_EC_RESULT.Add(objTB_EC_RESULT);
          }



          dbContext.TB_EC_RESULT.AddRange(listobjTB_EC_RESULT);

          TB_VETTING_MEETING objApp = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id);
          if (objApp != null)
          {
            objApp.Meeting_Completed = true;
          }

          dbContext.SaveChanges();
          pnlsubmissionpopup.Visible = true;
          //btnexport.Visible = true;
          btn_Confirm.Enabled = false;
          //    }
          //}
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
        //}
        //commented in 2018
        //UserCustomerrorLogin.InnerText = rs;
      }
      catch (Exception ex)
      {
        //commented in 2018
        //  UserCustomerrorLogin.InnerText = ex.Message;
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

      Context.Response.Redirect("~/SitePages/Application%20List%20for%20Vetting%20Team.aspx", false);


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

        var vetting_meeting_id = Context.Request.QueryString["VMID"];// "9A4E9644-7808-45EF-AFA9-C05AE0D98231";//"33F3A3D3-C4C2-434C-96EB-69EEB4C979AD"; Context.Request.QueryString["VMID"];
        Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);
        using (var dbContext = new CyberportEMS_EDM())
        {
          msTable = exportData(vetting_metting_id);

          if (objPrsentationResultSummary.Count > 0)
          {
            if (objPrsentationResultSummary.FirstOrDefault().Application_Number.ToLower().Contains("cpip"))
            {
              GetCPIPSPFiles(vetting_metting_id, msTable);
            }
            else if (objPrsentationResultSummary.FirstOrDefault().Application_Number.ToLower().Contains("ccmf"))
            {
              GetCCMFSPFiles(vetting_metting_id, msTable);
            }
          }
        }
        //using (var dbContext = new CyberportEMS_EDM())
        //{
        //    using (DocX doc = DocX.Create(string.Format("Report-{0}.doc", DateTime.Now.Ticks)))
        //    {
        //        var vetting_meeting_id = Context.Request.QueryString["VMID"];
        //        Guid vetting_metting_id = Guid.Parse(vetting_meeting_id);

        //        int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;

        //        TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);
        //        string fileNameTemplate = @"F:\New folder\DocXExample.docx";


        //        //string outputFileName =
        //        //string.Format(fileNameTemplate, DateTime.Now.ToString("MM-dd-yy"));


        //        DocX letter = this.GetRejectionLetterTemplate(doc, dbContext);

        //        letter.PageLayout.Orientation = Novacode.Orientation.Landscape;
        //        letter.ReplaceText("%ProgrammeName%", objTB_PROGRAMME_INTAKE.Programme_Name);
        //        letter.ReplaceText("%Intakenumber%", objTB_PROGRAMME_INTAKE.Intake_Number.ToString());
        //        letter.ReplaceText("%TotalApplicatio%", lbltotalprojects.Text);
        //        letter.ReplaceText("%ShortlistedApplications%", lblShort_Listed.Text);
        //        letter.ReplaceText("%Recommended%", lblrecommended.Text);
        //        letter.ReplaceText("%NotRecommended%", lblnotrecommended.Text);
        //        // added
        //        letter.ReplaceText("%Withdraw%", lblWithdrawCount.Text);
        //        if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
        //        {
        //            letter.ReplaceText("%shortprogramme%", "Incubation");
        //        }
        //        else if (!objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("university") && !objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("hong kong"))
        //        {
        //            letter.ReplaceText("%shortprogramme%", "CCMF – Cross Border");
        //        }
        //        else if (!objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("crossborder") && !objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("university"))
        //        {
        //            letter.ReplaceText("%shortprogramme%", "CCMF – Hong Kong");

        //        }
        //        else
        //        {
        //            letter.ReplaceText("%shortprogramme%", "CCMF – CUPP");

        //        }
        //        SPFunctions objfunction = new SPFunctions();
        //        letter.ReplaceText("%Recievername%", objfunction.GetCurrentUser());
        //        letter.ReplaceText("%Date%", objTB_PROGRAMME_INTAKE.Application_Start.ToShortDateString());
        //        IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
        //        letter.ReplaceText("%contactno%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Contact").Value);
        //        letter.ReplaceText("%email%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Email").Value);

        //        //letter.SaveAs(outputFileName);

        //        MemoryStream ms = new MemoryStream();
        //        letter.SaveAs(ms);


        //        System.Web.HttpContext.Current.Response.Clear();
        //        System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Presentation Result Summary " + objTB_PROGRAMME_INTAKE.Programme_Name + " " + objTB_PROGRAMME_INTAKE.Intake_Number.ToString() + " .docx");
        //        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //        ms.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
        //        System.Web.HttpContext.Current.Response.End();
        //    }


        //}
      }
    }

    #region EndorsemenLetter

    private MemoryStream exportData(Guid vetting_metting_id)
    {

      MemoryStream ms = new MemoryStream();


      using (var dbContext = new CyberportEMS_EDM())
      {
        using (DocX doc = DocX.Create(string.Format("Report-{0}.doc", DateTime.Now.Ticks)))
        {


          int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;

          TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.FirstOrDefault(x => x.Programme_ID == programId);


          DocX letter = this.GetRejectionLetterTemplate(doc, dbContext, vetting_metting_id);

          letter.PageLayout.Orientation = Novacode.Orientation.Portrait;
          letter.ReplaceText("%ProgrammeName%", objTB_PROGRAMME_INTAKE.Programme_Name);
          letter.ReplaceText("%Intakenumber%", objTB_PROGRAMME_INTAKE.Intake_Number.ToString());
          //letter.ReplaceText("%TotalApplicatio%", lbltotalprojects.Text);
          //letter.ReplaceText("%ShortlistedApplications%", lblShort_Listed.Text);
          //letter.ReplaceText("%Recommended%", lblrecommended.Text);
          //letter.ReplaceText("%NotRecommended%", lblnotrecommended.Text);
          // added
          letter.ReplaceText("%Withdraw%", lblWithdrawCount.Text);
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

          //SPFunctions objfunction = new SPFunctions();
          //letter.ReplaceText("%Recievername%", objfunction.GetCurrentUser());


          letter.ReplaceText("%Date%", objTB_PROGRAMME_INTAKE.Application_Start.ToShortDateString());
          IEnumerable<TB_SYSTEM_PARAMETER> objTbParams = new CyberportEMS_EDM().TB_SYSTEM_PARAMETER;
          letter.ReplaceText("%contactno%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Contact").Value);
          letter.ReplaceText("%email%", objTbParams.FirstOrDefault(x => x.Config_Code == "Endorsement_Email").Value);



          letter.SaveAs(ms);

        }


      }

      return ms;
    }

    private void GetCCMFSPFiles(Guid vetting_metting_id, MemoryStream msTable)
    {
      try
      {
        MemoryStream fs = new MemoryStream();

        SPSecurity.RunWithElevatedPrivileges(delegate ()
        {
          SPSite site = SPContext.Current.Site;
          // oGroup = site.SiteGroups[spUserRoleGroup];
          using (SPWeb web = site.OpenWeb())
          {
            // Library name - Shared Documents  
            SPList list = web.Lists["EMSDocumentTemplate"];
            fs = processFolder(list.RootFolder.Url, "CCMF_EndorsementLetter.docx");

          }


        });


        using (var dbContext = new CyberportEMS_EDM())
        {


          int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;
          Guid teamLeaderID = dbContext.TB_VETTING_MEMBER.FirstOrDefault(x => x.isLeader == true && x.Vetting_Meeting_ID == vetting_metting_id).Vetting_Member_ID;
          var teamLeaderInfo = dbContext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == teamLeaderID);
          TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList().FirstOrDefault();



          //var ccmfList = (from e in dbContext.TB_CCMF_APPLICATION
          //                where e.Intake_Number == objTB_PROGRAMME_INTAKE.Intake_Number && (e.Status.Contains("withdraw") || e.Status.Contains("Complete") || e.Status.Contains("Disqualified"))
          //                select e).ToList();

          var ccmfList = dbContext.TB_CCMF_APPLICATION.Where(x => x.Programme_ID == programId && (x.Status == "Complete Screening" || x.Status == "Awarded" || x.Status == "Presentation Withdraw")).ToList();

          totalCCMF_Application = ccmfList.Count();

          var youngList = (from emp in ccmfList
                           where emp.Hong_Kong_Programme_Stream.Contains("Young") && emp.Intake_Number == objTB_PROGRAMME_INTAKE.Intake_Number
                           select emp.Intake_Number).Count();

          totalCCMF_HK_YoungStream = youngList;

          var proffList = (from emp in ccmfList
                           where emp.Hong_Kong_Programme_Stream.Contains("Professional") && emp.Intake_Number == objTB_PROGRAMME_INTAKE.Intake_Number
                           select emp.Intake_Number).Count();

          totalCCMFProfessionalStream = proffList;

          var shortListedVetting = (from e in ccmfList
                                    join b in objPrsentationResultSummary on e.Application_Number equals b.Application_Number
                                    where b.totalrecommended > 0 && !e.Status.Contains("withdraw")
                                    //e.Intake_Number == objTB_PROGRAMME_INTAKE.Intake_Number && b.Vetting_Meeting_ID == vetting_metting_id
                                    select e).ToList();


          totalshortlisted = objPrsentationResultSummary.Count;
          totalRecomendedHK_young = (from emp in shortListedVetting
                                     where emp.Hong_Kong_Programme_Stream.Contains("Young")
                                     select emp).Count();

          totalRecomendedProff = (from emp in shortListedVetting
                                  where emp.Hong_Kong_Programme_Stream.Contains("Professional")
                                  select emp).Count();

          totalNotRecomended = objPrsentationResultSummary.Where(o => o.totalrecommended == 0).Count();
          totalWithdraw = (from e in ccmfList
                           join b in objPrsentationResultSummary on e.Application_Number equals b.Application_Number
                           where b.totalrecommended > 0 && e.Status.Contains("withdraw")
                           //e.Intake_Number == objTB_PROGRAMME_INTAKE.Intake_Number && b.Vetting_Meeting_ID == vetting_metting_id
                           select e).Count();


          Document docpdf = new Document();
          docpdf.LoadFromStream(fs, Spire.Doc.FileFormat.Docx2013);



          docpdf.Replace("#DateTableHeader", DateTime.Now.ToString("dd MMM yyyy"), true, true);
          docpdf.Replace("#currentDate", DateTime.Now.ToString("dd MMM yyyy"), true, true);

          string tlInfo = "";
          string tlFullName = "";
          if (!string.IsNullOrEmpty(teamLeaderInfo.Salutation))
          {
            tlInfo += teamLeaderInfo.Salutation + " ";
            tlFullName += teamLeaderInfo.Salutation + " ";
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.First_Name))
          {
            tlInfo += teamLeaderInfo.First_Name + " ";
            tlFullName += teamLeaderInfo.First_Name + " ";
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.Full_Name))
          {
            tlInfo += teamLeaderInfo.Full_Name;
            tlFullName += teamLeaderInfo.Full_Name;
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.Title))
          {
            tlInfo += "\n" + teamLeaderInfo.Title;
          }
          if (!string.IsNullOrEmpty(teamLeaderInfo.Address1))
          {
            tlInfo += "\n" + teamLeaderInfo.Address1;
          }
          if (!string.IsNullOrEmpty(teamLeaderInfo.Address2))
          {
            tlInfo += "\n" + teamLeaderInfo.Address2;
          }
          if (!string.IsNullOrEmpty(teamLeaderInfo.Address3))
          {
            tlInfo += "\n" + teamLeaderInfo.Address3;
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.City))
          {
            tlInfo += "\n" + teamLeaderInfo.City;
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.Country))
          {
            tlInfo += "\n" + teamLeaderInfo.Country;
          }

          docpdf.Replace("#teamLeadInfo", tlInfo, true, true);

          docpdf.Replace("#teamLeadFirstName", string.IsNullOrEmpty(teamLeaderInfo.First_Name) ? "" : teamLeaderInfo.First_Name, true, true);


          docpdf.Replace("#teamLeadFullName", tlFullName, true, true);

          docpdf.Replace("#totalCCMF_Application", lbltotalprojects.Text, true, true);
          docpdf.Replace("#totalCCMFProfessionalStream", totalCCMFProfessionalStream.ToString(), true, true);
          docpdf.Replace("#totalCCMF_HK_YoungStream", totalCCMF_HK_YoungStream.ToString(), true, true);
          docpdf.Replace("#totalshortlisted", lblShort_Listed.Text, true, true);
          docpdf.Replace("#totalRecomendedProff", totalRecomendedProff.ToString(), true, true);
          docpdf.Replace("#totalRecomendedHK_young", totalRecomendedHK_young.ToString(), true, true);
          docpdf.Replace("#totalNotRecomended", lblnotrecommended.Text, true, true);
          docpdf.Replace("#totalWithdraw", lblWithdrawCount.Text, true, true);


          string yr = objTB_PROGRAMME_INTAKE.Intake_Number.ToString().Substring(0, 4);
          string month = objTB_PROGRAMME_INTAKE.Intake_Number.ToString().Substring(objTB_PROGRAMME_INTAKE.Intake_Number.ToString().Length - 2, 2);
          string inTakeValue = DateTime.Parse(yr + "-" + month + "-01").ToString("MMM") + "  " + yr;
          docpdf.Replace("#intakeNO", inTakeValue, true, true);


          MemoryStream msnew = new MemoryStream();
          docpdf.SaveToStream(msnew, Spire.Doc.FileFormat.PDF);

          Document docpdf1 = new Document();
          docpdf1.LoadFromStream(msTable, Spire.Doc.FileFormat.Docx2013);

          MemoryStream msnew1 = new MemoryStream();
          docpdf1.SaveToStream(msnew1, Spire.Doc.FileFormat.PDF);

          PdfDocument[] documents = new PdfDocument[2];
          documents[0] = new PdfDocument(msnew);
          documents[1] = new PdfDocument(msnew1);

          for (int i = 1; i > -1; i--)
          {
            documents[0].AppendPage(documents[i]);
          }


          PdfDocument newPdf = new PdfDocument();

          foreach (PdfPageBase page in documents[0].Pages)
          {
            PdfPageBase newPage = newPdf.Pages.Add();
            PdfTextLayout loLayout = new PdfTextLayout();
            loLayout.Layout = PdfLayoutType.OnePage;
            page.CreateTemplate().Draw(newPage, new PointF(0, 0), loLayout);
          }

          MemoryStream msnew12 = new MemoryStream();
          newPdf.SaveToStream(msnew12);

          System.Web.HttpContext.Current.Response.Clear();
          System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Decision Summary " + objTB_PROGRAMME_INTAKE.Programme_Name + " " + objTB_PROGRAMME_INTAKE.Intake_Number.ToString() + ".pdf");
          System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
          msnew12.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
          System.Web.HttpContext.Current.Response.End();
        }
      }
      catch (Exception ex)
      {

        // Response.Write(ex.ToString());
      }
    }

    private void GetCPIPSPFiles(Guid vetting_metting_id, MemoryStream msTable)
    {
      try
      {
        MemoryStream fs = new MemoryStream();

        SPSecurity.RunWithElevatedPrivileges(delegate ()
        {
          SPSite site = SPContext.Current.Site;
          // oGroup = site.SiteGroups[spUserRoleGroup];
          using (SPWeb web = site.OpenWeb())
          {
            // Library name - Shared Documents  
            SPList list = web.Lists["EMSDocumentTemplate"];
            fs = processFolder(list.RootFolder.Url, "CPIP_EndorsementLetter.docx");

          }


        });


        using (var dbContext = new CyberportEMS_EDM())
        {


          int programId = dbContext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vetting_metting_id).Programme_ID;
          Guid teamLeaderID = dbContext.TB_VETTING_MEMBER.FirstOrDefault(x => x.isLeader == true && x.Vetting_Meeting_ID == vetting_metting_id).Vetting_Member_ID;
          var teamLeaderInfo = dbContext.TB_VETTING_MEMBER_INFO.FirstOrDefault(x => x.Vetting_Member_ID == teamLeaderID);
          TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbContext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList().FirstOrDefault();



          var cpipList = (from e in dbContext.TB_INCUBATION_APPLICATION
                          where e.Intake_Number == objTB_PROGRAMME_INTAKE.Intake_Number && (e.Status.Contains("withdraw") || e.Status.Contains("Complete") || e.Status.Contains("Disqualified"))
                          select e).ToList();






          Document docpdf = new Document();
          docpdf.LoadFromStream(fs, Spire.Doc.FileFormat.Docx2013);



          docpdf.Replace("#DateTableHeader", DateTime.Now.ToString("dd MMM yyyy"), true, true);
          docpdf.Replace("#currentDate", DateTime.Now.ToString("dd MMM yyyy"), true, true);

          string tlInfo = "";
          string tlFullName = "";
          if (!string.IsNullOrEmpty(teamLeaderInfo.Salutation))
          {
            tlInfo += teamLeaderInfo.Salutation + " ";
            tlFullName += teamLeaderInfo.Salutation + " ";
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.First_Name))
          {
            tlInfo += teamLeaderInfo.First_Name + " ";
            tlFullName += teamLeaderInfo.First_Name + " ";
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.Full_Name))
          {
            tlInfo += teamLeaderInfo.Full_Name;
            tlFullName += teamLeaderInfo.Full_Name;
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.Title))
          {
            tlInfo += "\n" + teamLeaderInfo.Title;
          }
          if (!string.IsNullOrEmpty(teamLeaderInfo.Address1))
          {
            tlInfo += "\n" + teamLeaderInfo.Address1;
          }
          if (!string.IsNullOrEmpty(teamLeaderInfo.Address2))
          {
            tlInfo += "\n" + teamLeaderInfo.Address2;
          }
          if (!string.IsNullOrEmpty(teamLeaderInfo.Address3))
          {
            tlInfo += "\n" + teamLeaderInfo.Address3;
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.City))
          {
            tlInfo += "\n" + teamLeaderInfo.City;
          }

          if (!string.IsNullOrEmpty(teamLeaderInfo.Country))
          {
            tlInfo += "\n" + teamLeaderInfo.Country;
          }

          docpdf.Replace("#teamLeadInfo", tlInfo, true, true);

          docpdf.Replace("#teamLeadFirstName", string.IsNullOrEmpty(teamLeaderInfo.First_Name) ? "" : teamLeaderInfo.First_Name, true, true);
          docpdf.Replace("#teamLeadFullName", tlFullName, true, true);
          docpdf.Replace("#totalCPIP_Application", lbltotalprojects.Text, true, true);
          docpdf.Replace("#totalshortlisted", lblShort_Listed.Text, true, true);
          docpdf.Replace("#totalRecomended", lblrecommended.Text, true, true);
          docpdf.Replace("#totalNotRecomended", lblnotrecommended.Text, true, true);
          docpdf.Replace("#totalWithdraw", lblWithdrawCount.Text, true, true);


          string yr = objTB_PROGRAMME_INTAKE.Intake_Number.ToString().Substring(0, 4);
          string month = objTB_PROGRAMME_INTAKE.Intake_Number.ToString().Substring(objTB_PROGRAMME_INTAKE.Intake_Number.ToString().Length - 2, 2);
          string inTakeValue = DateTime.Parse(yr + "-" + month + "-01").ToString("MMM") + "  " + yr;
          docpdf.Replace("#intakeNO", inTakeValue, true, true);


          MemoryStream msnew = new MemoryStream();
          docpdf.SaveToStream(msnew, Spire.Doc.FileFormat.PDF);

          Document docpdf1 = new Document();
          docpdf1.LoadFromStream(msTable, Spire.Doc.FileFormat.Docx2013);

          MemoryStream msnew1 = new MemoryStream();
          docpdf1.SaveToStream(msnew1, Spire.Doc.FileFormat.PDF);

          PdfDocument[] documents = new PdfDocument[2];
          documents[0] = new PdfDocument(msnew);
          documents[1] = new PdfDocument(msnew1);

          for (int i = 1; i > -1; i--)
          {
            documents[0].AppendPage(documents[i]);
          }


          PdfDocument newPdf = new PdfDocument();

          foreach (PdfPageBase page in documents[0].Pages)
          {
            PdfPageBase newPage = newPdf.Pages.Add();
            PdfTextLayout loLayout = new PdfTextLayout();
            loLayout.Layout = PdfLayoutType.OnePage;
            page.CreateTemplate().Draw(newPage, new PointF(0, 0), loLayout);
          }

          MemoryStream msnew12 = new MemoryStream();
          newPdf.SaveToStream(msnew12);

          System.Web.HttpContext.Current.Response.Clear();
          System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Decision Summary " + objTB_PROGRAMME_INTAKE.Programme_Name + " " + objTB_PROGRAMME_INTAKE.Intake_Number.ToString() + ".pdf");
          System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
          msnew12.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
          System.Web.HttpContext.Current.Response.End();
        }
      }
      catch (Exception ex)
      {

        // Response.Write(ex.ToString());
      }
    }

    public MemoryStream processFolder(string folderURL, string fileName)
    {
      MemoryStream stream = null;
      SPSecurity.RunWithElevatedPrivileges(delegate ()
      {
        // oGroup = site.SiteGroups[spUserRoleGroup];
        SPSite site = SPContext.Current.Site;
        using (SPWeb web = site.OpenWeb())
        {
          SPFolder folder = web.GetFolder(folderURL);
          SPFile file = folder.Files.Web.GetFile(folderURL + "/" + fileName);//.Where(o => o.Name.Contains(fileName)).FirstOrDefault(); //folder.Files[0];

          //  string destinationfolder = Destination + "/" + folder.Url;
          if (file.Exists)
          {
            byte[] binary = file.OpenBinary();
            stream = new MemoryStream(binary);
          }

        }


      });

      //writer.Close();
      return stream;
    }
    public static byte[] ReadFully(Stream input)
    {
      byte[] buffer = new byte[16 * 1024];
      using (MemoryStream ms = new MemoryStream())
      {
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
          ms.Write(buffer, 0, read);
        }
        return ms.ToArray();
      }
    }

    private DocX GetRejectionLetterTemplate(DocX doc, CyberportEMS_EDM dbContext, Guid vetting_metting_id)
    {

      // Adjust the path so suit your machine:
      string fileName = @"D:\Users\John\Documents\DocXExample.docx";

      // Set up our paragraph contents:
      string headerText = "Presentation Result Summary for the %ProgrammeName% %Intakenumber%";
      string paraThree = "" + "Summary of %shortprogramme%  %Intakenumber% Result ";


      // Title Formatting:
      var titleFormat = new Formatting();
      titleFormat.FontFamily = new System.Drawing.FontFamily("Arial");
      titleFormat.Size = 9D;
      titleFormat.Position = 9;
      titleFormat.Bold = true;

      // Body Formatting
      var paraFormat = new Formatting();
      paraFormat.FontFamily = new System.Drawing.FontFamily("Arial");
      paraFormat.Size = 9D;
      titleFormat.Position = 9;

      var listformatting = new Formatting();
      listformatting.FontFamily = new FontFamily("Arial");
      listformatting.Size = 7D;
      listformatting.Position = 7;

      // Create the doculment in memory:
      // var doc = DocX.Create(fileName);


      doc.MarginLeft = 10f;
      doc.MarginRight = 10f;



      // Insert each prargraph, with appropriate spacing and alignment:
      doc.InsertParagraph(Environment.NewLine);
      Paragraph title = doc.InsertParagraph(headerText, false, titleFormat);
      title.Alignment = Alignment.center;

      //doc.InsertParagraph(paraThree, false, paraFormat);


      //var list = doc.AddList(listType: Novacode.ListItemType.Bulleted, startNumber: 1);
      //doc.AddListItem(list, "Total no. of Application: %TotalApplicatio%", 0, listType: Novacode.ListItemType.Bulleted);
      //doc.AddListItem(list, "Shortlisted for Presentation:  %ShortlistedApplications%", 0, listType: Novacode.ListItemType.Bulleted);
      //doc.AddListItem(list, "Recommended:  %Recommended%", 0, listType: Novacode.ListItemType.Bulleted);
      //doc.AddListItem(list, "Not Recommended: %NotRecommended%", 0, listType: Novacode.ListItemType.Bulleted);
      ////added
      //doc.AddListItem(list, "Withdraw:%Withdraw%", 0, listType: Novacode.ListItemType.Bulleted);
      //doc.InsertList(list);

      doc.InsertParagraph(Environment.NewLine);



      objUserData = GetUserList(dbContext, vetting_metting_id);

      objPrsentationResultSummary = IncubationContext.Get_programme_summary_ccmf(vetting_metting_id, dbContext, objUserData);
      //objPrsentationResultSummary.RemoveAll(x => x.isRecommended < x.isNotRecommended);
      //objPrsentationResultSummary.RemoveAll(x => x.isRecommended == x.isNotRecommended);

      Novacode.Table tdata = doc.AddTable(objPrsentationResultSummary.Count + 1, 8);
      tdata.Alignment = Alignment.center;
      tdata.SetColumnWidth(0, 500);
      tdata.SetColumnWidth(1, 2000);
      tdata.SetColumnWidth(2, 1500);
      tdata.SetColumnWidth(3, 1500);
      tdata.SetColumnWidth(4, 1500);
      tdata.SetColumnWidth(5, 1000);
      tdata.SetColumnWidth(6, 1500);
      tdata.SetColumnWidth(7, 2000);
      tdata.SetTableCellMargin(TableCellMarginType.left, 50);
      tdata.SetTableCellMargin(TableCellMarginType.right, 100);


      tdata.SetBorder(TableBorderType.InsideH, new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.one, 1, System.Drawing.Color.Black));


      tdata.Rows[0].Cells[0].Paragraphs.First().Append("No.");
      tdata.Rows[0].Cells[1].Paragraphs.First().Append("Application No.");
      tdata.Rows[0].Cells[2].Paragraphs.First().Append("Project Name");

      tdata.Rows[0].Cells[3].Paragraphs.First().Append("Recommended");

      tdata.Rows[0].Cells[4].Paragraphs.First().Append("Not Recommended");

      tdata.Rows[0].Cells[5].Paragraphs.First().Append("Total No. of Votes");
      tdata.Rows[0].Cells[6].Paragraphs.First().Append("Result");
      tdata.Rows[0].Cells[7].Paragraphs.First().Append("Remarks");
      for (int i = 0; i < objPrsentationResultSummary.Count; i++)
      {

        tdata.Rows[i + 1].Cells[0].Paragraphs.First().Append((i + 1).ToString());
        tdata.Rows[i + 1].Cells[1].Paragraphs.First().Append(objPrsentationResultSummary[i].Application_Number.ToString());
        tdata.Rows[i + 1].Cells[2].Paragraphs.First().Append(objPrsentationResultSummary[i].company_name.ToString());
        tdata.Rows[i + 1].Cells[3].Paragraphs.First().Append(objPrsentationResultSummary[i].isRecommended.ToString());

        tdata.Rows[i + 1].Cells[4].Paragraphs.First().Append(objPrsentationResultSummary[i].isNotRecommended.ToString());
        tdata.Rows[i + 1].Cells[5].Paragraphs.First().Append(objPrsentationResultSummary[i].totalvotes.ToString());

        string Results = "";
        if (objPrsentationResultSummary[i].Withdraw != true)
        {
          if (objPrsentationResultSummary[i].isRecommended > objPrsentationResultSummary[i].isNotRecommended)
          {
            Results = "Recommended";
          }
          else if (objPrsentationResultSummary[i].isRecommended < objPrsentationResultSummary[i].isNotRecommended)
          {
            Results = "Not Recommended";
          }
        }
        else
        {

          Results = "Withdraw";
          tdata.Rows[i + 1].Cells[7].Paragraphs.First().Append(objPrsentationResultSummary[i].Remark.ToString());
        }
        tdata.Rows[i + 1].Cells[6].Paragraphs.First().Append(Results);


      }
      doc.InsertTable(tdata);
      return doc;
    }

    #endregion






    protected void rptrprogrammesummary_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
      if (e.CommandName == "Edit")
      {
        EditPopupRemarks.Visible = true;
        Guid vettingApp_Id = Guid.Parse(e.CommandArgument.ToString());
        if (vettingApp_Id != null)
        {
          hdnRemarkid.Value = Convert.ToString(vettingApp_Id);
          using (CyberportEMS_EDM dbcontext = new CyberportEMS_EDM())
          {
            TB_PRESENTATION_APPLICATION_REMARKS objremark = dbcontext.TB_PRESENTATION_APPLICATION_REMARKS.SingleOrDefault(k => k.Vetting_Appilcation_ID == vettingApp_Id);
            if (objremark != null)
            {
              chkwithdraw.Checked = objremark.Withdraw;
              txtRemarks.Text = objremark.Remark;
            }
            else
            {
              chkwithdraw.Checked = false;
              txtRemarks.Text = "";
            }

          }

        }

      }
    }

    protected void btnClosePopup_Click(object sender, EventArgs e)
    {
      EditPopupRemarks.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {

      Guid vmid = Guid.Parse(Context.Request.QueryString["VMID"]);
      CBP_EMS_SP.Common.SPFunctions spFun = new CBP_EMS_SP.Common.SPFunctions();
      string currentemail = spFun.GetCurrentUser();
      Guid vetApp_Id = Guid.Parse(hdnRemarkid.Value);
      using (CyberportEMS_EDM dbcontext = new CyberportEMS_EDM())
      {
        TB_PRESENTATION_APPLICATION_REMARKS ObjPRemark = dbcontext.TB_PRESENTATION_APPLICATION_REMARKS.FirstOrDefault(k => k.Vetting_Appilcation_ID == vetApp_Id);
        if (ObjPRemark != null)
        {
          ObjPRemark.Withdraw = chkwithdraw.Checked;
          ObjPRemark.Modified_By = currentemail;
          ObjPRemark.Modified_Date = DateTime.Now;
        }
        else
        {
          ObjPRemark = new TB_PRESENTATION_APPLICATION_REMARKS();
          ObjPRemark.Withdraw = chkwithdraw.Checked;
          ObjPRemark.Vetting_Meeting_Remark_ID = Guid.NewGuid();
          ObjPRemark.Vetting_Appilcation_ID = vetApp_Id;
          ObjPRemark.Created_By = currentemail;
          ObjPRemark.Created_Date = DateTime.Now;
          ObjPRemark.Modified_By = currentemail;
          dbcontext.TB_PRESENTATION_APPLICATION_REMARKS.Add(ObjPRemark);
        }
        ObjPRemark.Remark = txtRemarks.Text;

        int programId = dbcontext.TB_VETTING_MEETING.FirstOrDefault(x => x.Vetting_Meeting_ID == vmid).Programme_ID;
        TB_PROGRAMME_INTAKE objTB_PROGRAMME_INTAKE = dbcontext.TB_PROGRAMME_INTAKE.Where(x => x.Programme_ID == programId).ToList().FirstOrDefault();
        if (objTB_PROGRAMME_INTAKE != null)
        {
          TB_VETTING_APPLICATION ObjVettingApplication = dbcontext.TB_VETTING_APPLICATION.FirstOrDefault(k => k.Vetting_Application_ID == vetApp_Id);
          if (ObjVettingApplication != null)
          {
            if (objTB_PROGRAMME_INTAKE.Programme_Name.ToLower().Contains("incubation"))
            {
              TB_INCUBATION_APPLICATION objTB_INCUBATION_APPLICATION = dbcontext.TB_INCUBATION_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId
              && x.Application_Number == ObjVettingApplication.Application_Number && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
              if (objTB_INCUBATION_APPLICATION != null)
                objTB_INCUBATION_APPLICATION.Status = ObjPRemark.Withdraw == true ? "Presentation Withdraw" : "Complete Screening";
            }
            else
            {
              TB_CCMF_APPLICATION objTB_CCMF_APPLICATION = dbcontext.TB_CCMF_APPLICATION.FirstOrDefault(x => x.Programme_ID == programId
              && x.Application_Number == ObjVettingApplication.Application_Number && (x.Status != "Saved" && x.Status != "Deleted") && string.IsNullOrEmpty(x.Application_Parent_ID));
              if (objTB_CCMF_APPLICATION != null)
                objTB_CCMF_APPLICATION.Status = ObjPRemark.Withdraw == true ? "Presentation Withdraw" : "Complete Screening";

            }
          }
        }
        dbcontext.SaveChanges();
        EditPopupRemarks.Visible = false;
        FillPrograms();
      }
    }

    protected void ImageButtonSubmitClose_Click(object sender, ImageClickEventArgs e)
    {
      SubmitPopup.Visible = false;
    }

    protected void btn_ExportXls_Click(object sender, EventArgs e)
    {
      try
      {


        if (!string.IsNullOrEmpty(Context.Request.QueryString["VMID"]))
        {
          PrepareProgramData();
          Workbook book = new Workbook();
          Worksheet sheet = book.Worksheets[0];


          bool IsIncubation = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name.ToLower().Contains("incubation");
          string FileName = "Presentation Result Summary.xls";
          sheet.Range["A1"].Text = "Presentation Result Summary";


          sheet.Range["A2"].Text = "Programme Name";
          sheet.Range["B2"].Text = objTB_PROGRAMME_INTAKE.FirstOrDefault().Programme_Name;
          sheet.Range["A3"].Text = "Intake No.";
          sheet.Range["B3"].Text = Convert.ToString(objTB_PROGRAMME_INTAKE.FirstOrDefault().Intake_Number);
          sheet.Range["A4"].Text = "Meeting Date";
          sheet.Range["B4"].Text = meetingdate + "(Presentation Session)";
          sheet.Range["A5"].Text = "Venue";
          sheet.Range["B5"].Text = objTB_VETTING_MEETING.Venue;


          sheet.Range["C2"].Text = "Total No. of Applications";
          sheet.Range["D2"].NumberValue = totalapp;
          sheet.Range["C3"].Text = "Shortlisted For Presentation";
          sheet.Range["D3"].NumberValue = shortlist;
          sheet.Range["C4"].Text = "Recommended";
          sheet.Range["C5"].Text = "Not Recommended";
          sheet.Range["D5"].NumberValue = notrecom;

          sheet.Range["C6"].Text = "Withdraw";
          sheet.Range["D6"].NumberValue = WithdrawCount;
          sheet.Range["C7"].Text = "TBC";
          sheet.Range["D7"].NumberValue = tbc;

          if (IsIncubation)
          {
            sheet.Range["D4"].Text = Convert.ToString(reconsite + "(on-site)" + " " + recoffsite + "(off-site)");
          }
          else
          {
            sheet.Range["D4"].NumberValue = recom;
          }
          List<PrsentationResultSummary> oData = objPrsentationResultSummary;
          if (radioSort.SelectedValue == "desc")
          {
            if (lstSort.SelectedValue == "Result")
            {
              oData = objPrsentationResultSummary.OrderByDescending(o => o.Result).ToList();
            }
            else if (lstSort.SelectedValue == "Recommended")
            {
              oData = objPrsentationResultSummary.OrderByDescending(o => o.isRecommended).ToList();
            }
            else if (lstSort.SelectedValue == "Score")
            {
              oData = objPrsentationResultSummary.OrderByDescending(o => o.Averagescore).ToList();
            }
          }
          else
          {
            if (lstSort.SelectedValue == "Result")
            {
              oData = objPrsentationResultSummary.OrderBy(o => o.Result).ToList();
            }
            else if (lstSort.SelectedValue == "Recommended")
            {
              oData = objPrsentationResultSummary.OrderBy(o => o.isRecommended).ToList();
            }
            else if (lstSort.SelectedValue == "Score")
            {
              oData = objPrsentationResultSummary.OrderBy(o => o.Averagescore).ToList();
            }
          }

          int noOfUser = objUserData.Count;

          sheet.Range[9, 1].Text = "Sequence";
          sheet.Range[9, 1, 10, 1].Merge();

          sheet.Range[9, 2].Text = "Application No.";
          sheet.Range[9, 2, 10, 2].Merge();

          sheet.Range[9, 3].Text = "Company/Project Name";
          sheet.Range[9, 3, 10, 3].Merge();
          sheet.Range[9, 4].Text = "Score of each vetting member";
          int firstColMerge = (4 + (noOfUser - 1)); //to include self
          sheet.Range[9, 4, 9, firstColMerge].Merge();

          for (int i = 0; i < noOfUser; i++)
          {
            sheet.Range[10, (4 + i)].Text = "0" + (i + 1);
          }


          sheet.Range[9, (firstColMerge + 1)].Text = "Total Score";
          sheet.Range[9, (firstColMerge + 1), 10, (firstColMerge + 1)].Merge();
          sheet.Range[9, (firstColMerge + 2)].Text = "Average Score";
          sheet.Range[9, (firstColMerge + 2), 10, (firstColMerge + 2)].Merge();
          sheet.Range[9, (firstColMerge + 3)].Text = "Recommended choice of each vetting member";
          for (int i = 0; i < noOfUser; i++)
          {
            sheet.Range[10, (firstColMerge + 3 + i)].Text = "0" + (i + 1);
          }

          int secondColMerge = firstColMerge + 3 + noOfUser - 1; //to include self
          sheet.Range[9, (firstColMerge + 3), 9, secondColMerge].Merge();

          sheet.Range[9, (secondColMerge + 1)].Text = "Recommended";
          sheet.Range[9, (secondColMerge + 1), 10, (secondColMerge + 1)].Merge();
          sheet.Range[9, (secondColMerge + 2)].Text = "Not Recommended";
          sheet.Range[9, (secondColMerge + 2), 10, (secondColMerge + 2)].Merge();
          sheet.Range[9, (secondColMerge + 3)].Text = "No. of Votes";
          sheet.Range[9, (secondColMerge + 3), 10, (secondColMerge + 3)].Merge();
          sheet.Range[9, (secondColMerge + 4)].Text = "Result";
          sheet.Range[9, (secondColMerge + 4), 10, (secondColMerge + 4)].Merge();
          sheet.Range[9, (secondColMerge + 5)].Text = "Withdraw";
          sheet.Range[9, (secondColMerge + 5), 10, (secondColMerge + 5)].Merge();
          sheet.Range[9, (secondColMerge + 6)].Text = "Remarks";
          sheet.Range[9, (secondColMerge + 6), 10, (secondColMerge + 6)].Merge();

          sheet.Range[9, 1, 9, secondColMerge + 6].HorizontalAlignment = HorizontalAlignType.Center;
          sheet.Range[9, 1, 9, secondColMerge + 6].VerticalAlignment = VerticalAlignType.Center;
          sheet.Range[9, 1, 9, secondColMerge + 6].AutoFitColumns();
          sheet.Range[9, 1, 9, secondColMerge + 6].AutoFitRows();



          //up to 10th row grid head exists; from 11th data should be insert
          for (int i = 0; i < oData.Count; i++)
          {
            int seqNo = i + 1;
            var item = oData[i];
            sheet.Range[10 + seqNo, 1].NumberValue = seqNo;             //"Sequence";
            sheet.Range[10 + seqNo, 2].Text = item.Application_Number; //"Application No.";
            sheet.Range[10 + seqNo, 3].Text = item.company_name; //"Company/Project Name";

            bool IsWithdraw = Convert.ToString(item.Withdraw) == "True" ? true : false;
            //"Score of each vetting member"
            for (int j = 0; j < item.Score_of_vettingmember.Count; j++)
            {
              var score = item.Score_of_vettingmember[j];
              string scoreVal = IsWithdraw ?
                  (
                  string.IsNullOrEmpty(Convert.ToString(score.Total_Score)) ? "NA" : "NA")
                  :
                  (
                  string.IsNullOrEmpty(Convert.ToString(score.Total_Score)) ? "--" : Convert.ToString(score.Total_Score));

              sheet.Range[10 + seqNo, 4 + j].Text = scoreVal;

              string Choice = IsWithdraw ? "NA" : ((score.Go == null) ? "NA" : (score.Go.ToString().ToLower() == "true") ? "Yes" : "No");


              //"Recommended choice of each vetting member";
              sheet.Range[10 + seqNo, (firstColMerge + 3 + j)].Text = Choice;
            }

            if (IsWithdraw)
            {
              //"Total Score";
              sheet.Range[10 + seqNo, (firstColMerge + 1)].Text =
                sheet.Range[10 + seqNo, (firstColMerge + 2)].Text =
              sheet.Range[10 + seqNo, (secondColMerge + 1)].Text =
              sheet.Range[10 + seqNo, (secondColMerge + 2)].Text =
              sheet.Range[10 + seqNo, (secondColMerge + 3)].Text = "NA";
            }
            else
            {
              //"Total Score";
              sheet.Range[10 + seqNo, (firstColMerge + 1)].Text =  Convert.ToString(item.Totalscore);
              //"Average Score";
              sheet.Range[10 + seqNo, (firstColMerge + 2)].Text =  Convert.ToString(item.Averagescore);
              //"Recommended";
              sheet.Range[10 + seqNo, (secondColMerge + 1)].NumberValue =item.isRecommended;
              sheet.Range[10 + seqNo, (secondColMerge + 2)].NumberValue= item.isNotRecommended;
              sheet.Range[10 + seqNo, (secondColMerge + 3)].NumberValue= item.totalvotes;
            }


            sheet.Range[10 + seqNo, (secondColMerge + 4)].Text = IsWithdraw ? "NA" : (item.isRecommended > item.isNotRecommended ? "Recommended" : item.isRecommended < item.isNotRecommended ? "Not Recommended" : item.totalvotes == 0 ? "NA" : item.isRecommended == item.isNotRecommended ? "TBC" : "");
            sheet.Range[10 + seqNo, (secondColMerge + 5)].Text = IsWithdraw ? "Yes" : "No";
            sheet.Range[10 + seqNo, (secondColMerge + 6)].Text = item.Remark;

          }

          sheet.Range[9, 1, 10, secondColMerge + 6].BorderAround(LineStyleType.Thin, Color.Gray);
          if (oData.Count > 0)
          {
            sheet.Range[11, 1, (11 + oData.Count-1), secondColMerge + 6].BorderAround(LineStyleType.Thin, Color.Gray);

          }

          for (int i = 0; i < objUserData.Count; i++)
          {
            int seqNo = i + 1;
            var oUser = objUserData[i];
            sheet.Range[10 + oData.Count + 2, seqNo].Text = "0" + seqNo + " " + oUser.UserData.Full_Name;
          }

          //sheet.AllocatedRange.AutoFitColumns();
          //sheet.AllocatedRange.AutoFitRows();
          using (MemoryStream stream = new MemoryStream())
          {
            book.SaveToStream(stream);
            byte[] buffer = stream.ToArray();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            HttpContext.Current.Response.OutputStream.Write(buffer, 0, buffer.Length);
            System.Web.HttpContext.Current.Response.End();
          }
        }
      }
      catch (Exception ex)
      {

        errormsg.InnerHtml = ex.Message;
      }
    }
  }

  public class SortColumnClass
  {
    public string ColumnValue { get; set; }
    public string ColumnText { get; set; }

  }


}
