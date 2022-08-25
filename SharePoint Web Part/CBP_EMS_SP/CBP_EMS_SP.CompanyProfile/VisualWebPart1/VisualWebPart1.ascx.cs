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

namespace CBP_EMS_SP.CompanyProfile.VisualWebPart1
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

        protected void Page_Load(object sender, EventArgs e)
        {
            getCompany();

        }



        protected void getCompany()
        {
            SPFunctions objFUnction = new SPFunctions();
            string strCurrentUser = objFUnction.GetCurrentUser();

            rptrReimbursementListCASP.DataSource = GetCompanyProfileForUser(strCurrentUser);
            rptrReimbursementListCASP.DataBind();

        }




        public static List<CompanyProfiles> GetCompanyProfileForUser(string strCurrentUser)
        {

            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {

                //List<CASP_CompanyListPrograms> objUserProg = new List<CASP_CompanyListPrograms>();
                //List<TB_INCUBATION_APPLICATION> objIncubation = dbContext.TB_INCUBATION_APPLICATION.Where(x => x.Applicant == strCurrentUser && x.Awarded == true).ToList();
                //objIncubation.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                //{
                //    ApplicationId = x.Incubation_ID,

                //}));

                //List<TB_CCMF_APPLICATION> objCCMF = dbContext.TB_CCMF_APPLICATION.Where(x => x.Applicant == strCurrentUser && x.Awarded == true).ToList();
                //objCCMF.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                //{
                //    ApplicationId = x.CCMF_ID,

                //}));

                //List<TB_CASP_APPLICATION> objCASP = dbContext.TB_CASP_APPLICATION.Where(x => x.Applicant == strCurrentUser).ToList();
                //objCCMF.ForEach(x => objUserProg.Add(new CASP_CompanyListPrograms()
                //{
                //    ApplicationId = x.CCMF_ID,

                //}));

                //List<Guid> Applications = objUserProg.Select(x => x.ApplicationId).ToList();

                List<Guid> objCompanyadmin = dbContext.TB_COMPANY_ADMIN.Where(x => x.Email == strCurrentUser).Select(x=>x.Company_Profile_ID).ToList();

                //List<TB_COMPANY_APPLICATION_MAP> objCompanies = dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => Applications.AsEnumerable().Contains(x.Application_ID) ).ToList();


                // objCompanies.AddRange( dbContext.TB_COMPANY_APPLICATION_MAP.Where(x => objCompanyadmin.Contains(x.Company_Profile_ID) ));

                List<CompanyProfiles> objCompanyList = new List<CompanyProfiles>();
                dbContext.TB_COMPANY_PROFILE_BASIC.Where(x => x.Created_By == strCurrentUser || objCompanyadmin.Contains(x.Company_Profile_ID)).ToList().ForEach(x =>
                objCompanyList.Add(
                                 new CompanyProfiles()
                                 {
                                     Company_Profile_ID = x.Company_Profile_ID,

                                     Name_Eng = x.Name_Eng,
                                     Name_Chi = x.Name_Chi,
                                     Applicaition_Type = string.Join(",", x.TB_COMPANY_APPLICATION_MAP.Select(y => y.Applicaition_Type).ToList())

                                 }
                                 )
                                  );

                //objCompanies.ForEach(x => objCompanyList.Add(

                //    new CompanyProfiles()
                //    {
                //        Company_Profile_ID = Guid.Parse(x.Company_Profile_ID.ToString()),

                //        Name_Eng = x.TB_COMPANY_PROFILE_BASIC.Name_Eng,
                //        Name_Chi = x.TB_COMPANY_PROFILE_BASIC.Name_Chi,
                //        Applicaition_Type = x.Applicaition_Type

                //    }

                //    ));

                return objCompanyList;
            }



        }
    }
}
