namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_PROFILE_BASIC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_COMPANY_PROFILE_BASIC()
        {
            TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT = new HashSet<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();
            TB_CASP_SPECIAL_REQUEST = new HashSet<TB_CASP_SPECIAL_REQUEST>();
            TB_COMPANY_ADMIN = new HashSet<TB_COMPANY_ADMIN>();
            TB_COMPANY_APPLICATION_MAP = new HashSet<TB_COMPANY_APPLICATION_MAP>();
            TB_COMPANY_AWARD = new HashSet<TB_COMPANY_AWARD>();
            TB_COMPANY_CONTACT = new HashSet<TB_COMPANY_CONTACT>();
            TB_COMPANY_FUND = new HashSet<TB_COMPANY_FUND>();
            TB_COMPANY_IP = new HashSet<TB_COMPANY_IP>();
            TB_COMPANY_MEMBER = new HashSet<TB_COMPANY_MEMBER>();
            TB_COMPANY_MERGE_ACQUISITION = new HashSet<TB_COMPANY_MERGE_ACQUISITION>();
            //TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT = new HashSet<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();
        }

        [Key]
        public Guid Company_Profile_ID { get; set; }

        [StringLength(255)]
        public string Name_Eng { get; set; }

        [StringLength(255)]
        public string Name_Chi { get; set; }

        [StringLength(255)]
        public string Company_Name { get; set; }

        [StringLength(255)]
        public string Brand_Name { get; set; }

        [StringLength(50)]
        public string CCMF_Custer { get; set; }

        [StringLength(50)]
        public string CPIP_Custer { get; set; }

        [StringLength(255)]
        public string Tag { get; set; }

        public string CCMF_Abstract { get; set; }
        public string CCMF_Abstract_Chi { get; set; }
        public string CPIP_Abstract { get; set; }

        public string CPIP_Abstract_Chi { get; set; }

        public string CASP_Abstract { get; set; }

        public string Company_Ownership_Structure { get; set; }

        public string Remarks { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_CASP_SPECIAL_REQUEST> TB_CASP_SPECIAL_REQUEST { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_ADMIN> TB_COMPANY_ADMIN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_APPLICATION_MAP> TB_COMPANY_APPLICATION_MAP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_AWARD> TB_COMPANY_AWARD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_CONTACT> TB_COMPANY_CONTACT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_FUND> TB_COMPANY_FUND { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_IP> TB_COMPANY_IP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_MEMBER> TB_COMPANY_MEMBER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_COMPANY_MERGE_ACQUISITION> TB_COMPANY_MERGE_ACQUISITION { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }
    }
}
