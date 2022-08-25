using CBP_EMS_SP.Data;
using CBP_EMS_SP.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using Microsoft.SharePoint;
using System.Web;
using System.Web.UI.WebControls;
using CBP_EMS_SP.Data.CustomModels;

namespace ECProgramList.ECProgramList
{
    [ToolboxItemAttribute(false)]
    public partial class ECProgramList : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ECProgramList()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillPrograms();
        }

        public void FillPrograms()
        {

            using (var dbContext = new CyberportEMS_EDM())
            {


                //var objInc = (from oprgintake in dbContext.TB_PROGRAMME_INTAKE
                //              join oecresult in dbContext.TB_EC_RESULT on oprgintake.Programme_ID equals oecresult.Programme_ID

                //              select new
                //              {
                //                  Programe_name = oprgintake.Programme_Name,
                //                  intake_number = oprgintake.Intake_Number,
                //                  status = oecresult.Status,
                //                  Program_id= oprgintake.Programme_ID,

                //              }).ToList();
                //List<TB_EC_RESULT> objTB_EC_RESULT =   dbContext.TB_EC_RESULT.ToList();

                // objTB_EC_RESULT = objTB_EC_RESULT.GroupBy(p => new { p.Programme_ID }).Select(g => g.First()).ToList();

                var objInc = (from oprgintake in dbContext.TB_PROGRAMME_INTAKE
                              join oecresult in dbContext.TB_EC_RESULT on oprgintake.Programme_ID equals oecresult.Programme_ID

                              select new
                              {
                                  Programe_name = oprgintake.Programme_Name,
                                  intake_number = oprgintake.Intake_Number,
                                  status = oecresult.Status,
                                  Program_id = oprgintake.Programme_ID,

                              }).ToList();
                objInc = objInc.GroupBy(p => new { p.Program_id }).Select(g => g.First()).ToList();
                List<TB_VETTING_MEETING> objTB_VETTING_MEETING = new List<TB_VETTING_MEETING>();
                foreach (var item in objInc)
                {
                    List<TB_VETTING_MEETING> obj = new List<TB_VETTING_MEETING>();
                    obj = dbContext.TB_VETTING_MEETING.Where(x => x.Programme_ID == item.Program_id).ToList();
                    if (obj.Count() > 1)
                    {
                        bool issaved = false;
                        foreach (TB_VETTING_MEETING item1 in obj)
                        {
                            if (!item1.Meeting_Completed.HasValue || item1.Meeting_Completed.Value == false)
                            {
                                issaved = true;
                            }
                        }
                        if (issaved)
                        {
                            objTB_VETTING_MEETING.AddRange(obj);

                        }

                    }
                }
                objInc = objInc.Where(x => !(objTB_VETTING_MEETING.Exists(y => y.Programme_ID == x.Program_id))).ToList();
                foreach (var item in objInc)
                {
                    if (dbContext.TB_VETTING_MEETING.Any(k => k.Programme_ID == item.Program_id && (!k.Meeting_Completed.HasValue ||k.Meeting_Completed.Value==false)))
                    {
                        objInc.Skip(item.Program_id);
                    }
                }
                rptrprogrammesummary.DataSource = objInc;
                rptrprogrammesummary.DataBind();

            }
        }

        //protected void Final_vetting_result_click(object sender, EventArgs e)
        //{
        //    string Programid = ((LinkButton)sender).CommandArgument;
        //    HttpContext.Current.Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/FinalVettingResult.aspx?program_id=" + Programid);
        //}

        protected void ECResult_Click(object sender, EventArgs e)
        {
            string Programid = ((LinkButton)sender).CommandArgument;
            HttpContext.Current.Response.Redirect(SPContext.GetContext(System.Web.HttpContext.Current).Site.Url + "/SitePages/ECResult.aspx?program_id=" + Programid);
        }
    }
}
