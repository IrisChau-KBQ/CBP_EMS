using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBP_EMS_SP.Data.Models
{

    public class CASP_CompanyListPrograms
    {
        public CASP_CompanyListPrograms()
        {
        }
        public Guid ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string ApplicationType { get; set; }


    }

    public class CASP_CompanyList
    {
        public CASP_CompanyList() { }
        public string CompanyIdNumber { get; set; }
        public string CompanyName { get; set; }
    }

    public partial class CASP_Programme_Attended
    {

        public Guid CASP_ID { get; set; }

        public string Programme_Name { get; set; }
    }
    public class CaspReimburesementList
    {
        public string CASPType { get; set; }
        public Guid ApplicationID { get; set; }
        public string ApplicationNo { get; set; }
        public string Company { get; set; }
        public string Category { get; set; }
        public DateTime? Submitted { get; set; }
        public string Status { get; set; }
        public DateTime Created_Date { get; set; }
    }

    public class CompanyProfileList
    {
        public Guid Company_Profile_ID { get; set; }
        [NotMapped]
        public string Name_Eng { get; set; }
        [NotMapped]
        public string Name_Chi { get; set; }
        [NotMapped]
        public string Company_Name { get; set; }
        [NotMapped]
        public string Brand_Name { get; set; }

        [NotMapped]
        public string Programme_Type { get; set; }
        //public List<string> Intake_Number { get; set; }
        //public List<string> Core_Member { get; set; }
        [NotMapped]
        public string CCMF_Custer { get; set; }
        [NotMapped]
        public string tag { get; set; }
        [NotMapped]
        public string CPIP_Custer { get; set; }
    }
}
