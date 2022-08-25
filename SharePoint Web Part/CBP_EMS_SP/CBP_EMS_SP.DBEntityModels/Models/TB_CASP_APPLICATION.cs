namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CASP_APPLICATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_CASP_APPLICATION()
        {
            TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT = new HashSet<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();
            TB_CASP_SPECIAL_REQUEST = new HashSet<TB_CASP_SPECIAL_REQUEST>();
        }

        [Key]
        public Guid CASP_ID { get; set; }

        [Required]
        public int Programme_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_No { get; set; }

        [Required]
        [StringLength(255)]
        public string Applicant { get; set; }
        [StringLength(255)]
        public string Company_Project { get; set; }

        [Required]
        [StringLength(50)]
        public string CCMF_CPIP_App_No { get; set; }

        public string Company_Address { get; set; }

        public string Abstract { get; set; }

        public string Company_Ownership_Structure { get; set; }

        public string Additional_Info { get; set; }

        [StringLength(255)]
        public string Accelerator_Name { get; set; }

        public bool? Endorsed_by_Cyberport { get; set; }

        public DateTime? Commencement_Date { get; set; }

        public int? Duration { get; set; }

        public string Background { get; set; }

        public string Offer { get; set; }

        public string Fund_Raising_Capabilities { get; set; }

        public string Size_of_Alumni { get; set; }

        public string Reputation { get; set; }

        [StringLength(255)]
        public string Website { get; set; }

        public bool? Declaration { get; set; }

        [StringLength(255)]
        public string Principle_Full_Name { get; set; }

        [StringLength(255)]
        public string Principle_Title { get; set; }

        public DateTime? Submitted_Date { get; set; }

        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime? Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(36)]
        public string Application_Parent_ID { get; set; }


        [Required]
        [StringLength(20)]
        public string Version_Number { get; set; }
        public DateTime? Last_Submitted { get; set; }

        public DateTime? Completed_Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_CASP_SPECIAL_REQUEST> TB_CASP_SPECIAL_REQUEST { get; set; }
    }
}
