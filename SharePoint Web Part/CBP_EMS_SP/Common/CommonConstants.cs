using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CBP_EMS_SP.Common
{
    public enum enumAttachmentType
    {
        Company_Ownership_Structure,
        BR_COPY,// BR map to CI
        CI_COPY,
        Company_Annual_Return,
        Video_Clip,
        Presentation_Slide,
        Presentation_Slide_Response,
        Other_Attachment,
        Student_ID,
        HK_ID,
        Accelerator_Admission_Record,
        FA_Original_Receipt,
        FA_Original_Invoice,
        FA_Quotation,
        FA_EventPhoto,
        FA_EventPass,
        FA_PrintSample,
        FA_BoardingPass,
        FA_flight_Inventory,
        FA_BusinessCard,
        FA_Intern_Payroll,
        FA_Intern_Academic_Certification,
        FA_MPF_PaymentProof,
        FA_Employement_Contract,
        FA_ResumeCV,
        FA_FreelancerResumeCV,
        FA_HKID_Card,
        Supporting_Document,
        Fact_Sheet,
        FA_EmpPayslip,
        FA_SalaryProof

    }

    public enum formsubmitaction
    {
        Submitted,
        Waiting_for_response_from_applicant,
        Resubmitted_information,
        Eligibility_checked,
        To_be_disqualified,
        Disqualified,
        BDM_Reviewed,
        Sr_Mgr_Reviewed,
        CPMO_Reviewed,
        Complete_Screening,
        Saved,
        Deleted,
        Completed
    }
    public class CBPCommonConstants
    {
        public CBPCommonConstants()
        {

        }

        public static string GetAttachementFolderPhysical(enumAttachmentType objAttachmentType)
        {
            if (objAttachmentType.CompareTo(enumAttachmentType.Company_Ownership_Structure) > 0)
                return SPUtility.GetVersionedGenericSetupPath(@"TEMPLATE\DocumentTemplates\ApplicationAttachement\", 15);
            else
                return SPUtility.GetVersionedGenericSetupPath(@"TEMPLATE\DocumentTemplates\ApplicationAttachement\", 15);

        }

        public Dictionary<string, string> Reimbusement_Categories(string FormType)
        {
            //if (FormType.ToLower() == "cpip")
            //{
            ////    return new Dictionary<string, string>() {
            ////    {"I"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_Init", "CyberportEMS_Common")},
            ////    {"A"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_A", "CyberportEMS_Common")},
            ////    {"B"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_B", "CyberportEMS_Common")},
            ////    {"C"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_C", "CyberportEMS_Common")},
            ////    {"D"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_D", "CyberportEMS_Common")},
            ////    {"E"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_E", "CyberportEMS_Common")}
            ////};
            //}
            //else 
            if (FormType.ToLower() == "casp")
            {
                return new Dictionary<string, string>() {
                {"A"  ,"Category A – Programme fees"},
                {"B"  ,"Category B – Office Rental"},
                {"C"  ,"Category C – Internship"},
                {"D"  ,"Category D – Travel & Accommodation"},
                {"E"  ,"Category E – Promotion & Marketing"},
                {"F"  ,"Category F – Professional Service"}

            };
            }
            else if (FormType.ToLower() == "both")
            {
                return new Dictionary<string, string>() {
                //{"CPIP-I"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_Init", "CyberportEMS_Common")},
                //{"CPIP-A"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_A", "CyberportEMS_Common")},
                //{"CPIP-B"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_B", "CyberportEMS_Common")},
                //{"CPIP-C"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_C", "CyberportEMS_Common")},
                //{"CPIP-D"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_D", "CyberportEMS_Common")},
                //{"CPIP-E"  ,SPFunctions.LocalizeUI("CPIP_FA_Options_E", "CyberportEMS_Common")},
                {"CASP-A"  ,"Category A – Programme fees"},
                {"CASP-B"  ,"Category B – Office Rental"},
                {"CASP-C"  ,"Category C – Internship"},
                {"CASP-D"  ,"Category D – Travel & Accommodation"},
                {"CASP-E"  ,"Category E – Promotion & Marketing"},
                {"CASP-F"  ,"Category F – Professional Service"}
                };
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
        public List<string> CPIP_Reimbusement_ProfessionalServices(string FormType)
        {
            return new List<string>() {
                {SPFunctions.LocalizeUI("lbl_ProfessionServiceLegal", "CyberportEMS_CPIP_Reimbursement")},
                {SPFunctions.LocalizeUI("lbl_ProfessionServiceConsultancy", "CyberportEMS_CPIP_Reimbursement")},
                {SPFunctions.LocalizeUI("lbl_ProfessionServiceFinance", "CyberportEMS_CPIP_Reimbursement")},
                {SPFunctions.LocalizeUI("lbl_ProfessionServiceOther", "CyberportEMS_CPIP_Reimbursement")}
             };
        }

        //public static class ExtTEntityMapper
        //{

        //    public static T1 ExtMapTType<T, T1>(this T dt, T1 OldEntity, params string[] SkipProperties)
        //    {
        //        //  var objT = Activator.CreateInstance<T1>();

        //        if (dt != null)
        //        {
        //            var propertiesFrom = typeof(T).GetProperties();
        //            var propertiesTo = typeof(T1).GetProperties();

        //            foreach (var pro in propertiesTo)
        //            {
        //                if (SkipProperties.FirstOrDefault(x => x.ToLower() == pro.Name.ToLower()) == null)
        //                {
        //                    if (propertiesFrom.Where(x => x.Name.ToLower() == pro.Name.ToLower()).Count() > 0)
        //                    {
        //                        PropertyInfo pI = OldEntity.GetType().GetProperty(pro.Name);
        //                        PropertyInfo pJ = propertiesFrom.FirstOrDefault(x => x.Name.ToLower() == pro.Name.ToLower());

        //                            pro.SetValue(OldEntity, Convert.ChangeType(pJ.GetValue(dt), pI.PropertyType), null);

        //                    }
        //                }
        //            }
        //        }
        //        return OldEntity;

        //    }


        //}

        public string[] GetNationalityList = new string[]{"Afghanistan",
"Albania",
"Algeria",
"Andorra",
"Angola",
"Antigua and Barbuda",
"Argentina",
"Armenia",
"Australia",
"Austria",
"Azerbaijan",
"Bahamas",
"Bahrain",
"Bangladesh",
"Barbados",
"Belarus",
"Belgium",
"Benin",
"Bolivia",
"Bosnia and Herzegovina",
"Botswana",
"Brazil",
"Brunei",
"Bulgaria",
"Burundi",
"Cambodia",
"Cameroon",
"Canada",
"Cape Verde",
"Chad",
"Chile",
"China",
"Colombia",
"Comoros",
"Congo",
"Congo(Kinshasa)",
"Cook Islands",
"Costa Rica",
"Cote d'lvoire",
"Croatia",
"Cuba",
"Cyprus",
"Czech",
"Denmark",
"Djibouti",
"Dominica",
"Dominican Republic",
"DPRK(Democratic People's Republic of Korea)",
"Ecuador",
"Egypt",
"Equatorial Guinea",
"Eritrea",
"Estonia",
"Ethiopia",
"European Union",
"Fiji",
"Finland",
"France",
"Gabon",
"Gambia",
"Georgia",
"Germany",
"Ghana",
"Greece",
"Grenada",
"Guinea",
"Guinea Bissau",
"Guyana",
"Hong Kong",
"Hungary",
"Iceland",
"India",
"Indonesia",
"Iran",
"Iraq",
"Ireland",
"Israel",
"Italy",
"Jamaica",
"Japan",
"Jordan",
"Kazakstan",
"Kenya",
"Kuwait",
"Kyrgyzstan",
"Laos",
"Latvia",
"Lebanon",
"Lesotho",
"Liberia",
"Libya",
"Liechtenstein",
"Lithuania",
"Luxembourg",
"Macau",
"Macedonia",
"Madagascar",
"Malawi",
"Malaysia",
"Maldives",
"Mali",
"Malta",
"Mauritania",
"Mauritius",
"Mexico",
"Micronesia",
"Moldova",
"Monaco",
"Mongolia",
"Montenegro",
"Morocco",
"Mozambique",
"Myanmar",
"Namibia",
"Nepal",
"Netherlands",
"New Zealand",
"Niger",
"Nigeria",
"Niue",
"Norway",
"Oman",
"Pakistan",
"Palestine",
"Panama",
"Papua New Guinea",
"Peru",
"Philippines",
"Poland",
"Portugal",
"Qatar",
"ROK(Republic of Korea)",
"Romania",
"Russia",
"Rwanda",
"Samoa",
"San Marino",
"Sao Tome and Principe",
"Saudi Arabia",
"Senegal",
"Serbia",
"Seychelles",
"Sierra Leone",
"Singapore",
"Slovakia",
"Slovenia",
"Somalia",
"South Africa",
"South Sudan",
"Spain",
"Sri Lanka",
"Sudan",
"Suriname",
"Sweden",
"Switzerland",
"Syria",
"Taiwan",
"Tajikistan",
"Tanzania",
"Thailand",
"The Central African Republic",
"Timor-Leste",
"Togo",
"Tonga",
"Trinidad and Tobago",
"Tunisia",
"Turkey",
"Turkmenistan",
"UAE(United Arab Emirates)",
"Uganda",
"Ukraine",
"United Kingdom",
"United States of America",
"Uruguay",
"Uzbekistan",
"Vanuatu",
"Venezuela",
"Vietnam",
"Yemen",
"Zambia",
"Zimbabwe",
        "Other"};
    }



}
