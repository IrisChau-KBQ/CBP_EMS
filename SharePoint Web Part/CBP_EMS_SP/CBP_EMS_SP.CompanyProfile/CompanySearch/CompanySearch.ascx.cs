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
using System.Data.SqlClient;

namespace CBP_EMS_SP.CompanyProfile.CompanySearch
{
    [ToolboxItemAttribute(false)]
    public partial class CompanySearch : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CompanySearch()
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
                getgridlist();
            }
        }

        protected List<CompanyProfileList> getcompanyProfiles()
        {
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {
                SqlParameter param1 = new SqlParameter("@ProfileName", txtprofile.Text);
                SqlParameter param2 = new SqlParameter("@coreMemberName", txtcoremember.Text);
                SqlParameter param3 = new SqlParameter("@programmeType", ddlprogramtype.SelectedValue);
                SqlParameter param4 = new SqlParameter("@intake_No", txtintake.Text);
                SqlParameter param5 = new SqlParameter("@cluster", txtcluster.Text);
                SqlParameter param6 = new SqlParameter("@tag", txttags.Text);
                List<CompanyProfileList> objcompanybasic = dbContext.Database.SqlQuery<CompanyProfileList>("get_company_search @ProfileName,@coreMemberName,@programmeType,@intake_No,@cluster,@tag", param1, param2, param3, param4, param5, param6).ToList();
                return objcompanybasic;
            }
        }

        protected void getgridlist()
        {
            List<Guid> objCompanyID = getcompanyProfiles().Select(x => x.Company_Profile_ID).ToList() ;
            using (CyberportEMS_EDM dbContext = new CyberportEMS_EDM())
            {

                List<TB_COMPANY_PROFILE_BASIC> objCompanyDB = dbContext.TB_COMPANY_PROFILE_BASIC.Where(x=>objCompanyID.Contains(x.Company_Profile_ID)).ToList();

                List<CompanyProfileList> ObjCompanyFilter = objCompanyDB.Select(x=> new CompanyProfileList() {
                    Company_Profile_ID = x.Company_Profile_ID,
                    Brand_Name=x.Brand_Name,
                    CCMF_Custer=x.CCMF_Custer,
                    Company_Name=x.Company_Name,
                    CPIP_Custer=x.CPIP_Custer,
                    Name_Chi=x.Name_Chi,
                    Name_Eng=x.Name_Eng,
                    tag=x.Tag,
                    Programme_Type = string.Join("," ,x.TB_COMPANY_APPLICATION_MAP.Select(y=>y.Applicaition_Type).ToList())

                } ).ToList();

                    //List<CompanyProfileList> objCompanyFilterList = objCompany.Where(x => x.Company_Profile_ID == CompanyProfileId).ToList();
                    //CompanyProfileList objCompanyProfile = objCompanyFilterList.FirstOrDefault();
                    //objCompanyProfile.ApplicationTypes = string.Join(",", objCompanyFilterList.Select(x => x.Programme_Type).ToList());
                    //objCompanyFiltered.Add(objCompanyProfile);
                

                gdv_companyProfileList.DataSource = ObjCompanyFilter;// objCompanyFiltered;
                gdv_companyProfileList.DataBind();
                totalappNo.InnerHtml = gdv_companyProfileList.Rows.Count.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            getgridlist();

        }
    }
}
