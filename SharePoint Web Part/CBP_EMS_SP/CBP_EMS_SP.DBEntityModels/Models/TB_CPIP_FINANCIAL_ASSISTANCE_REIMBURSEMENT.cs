namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT()
        {
            TB_BANK_INFO_FOR_DIRECT_TRANSFER = new HashSet<TB_BANK_INFO_FOR_DIRECT_TRANSFER>();
            //TB_FA_REIMBURSEMENT_ITEM = new HashSet<TB_FA_REIMBURSEMENT_ITEM>();
            //TB_FA_REIMBURSEMENT_SALARY = new HashSet<TB_FA_REIMBURSEMENT_SALARY>();
        }

        [Key]
        public Guid CPIP_FA_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_No { get; set; }

        public DateTime? Submitted_Date { get; set; }

        [Required]
        [StringLength(1)]
        public string Category { get; set; }

        public Guid CPIP_ID { get; set; }

        public Guid? Company_ID { get; set; }

        [StringLength(255)]
        public string Payable_To { get; set; }

        [StringLength(255)]
        public string Event_Attended { get; set; }

        [StringLength(50)]
        public string Location_ID { get; set; }

        public bool? Service_Not_Completed { get; set; }

        public DateTime? Service_From { get; set; }

        public DateTime? Service_To { get; set; }

        public bool? Freelancer { get; set; }

        [StringLength(255)]
        public string Service_Provider_Name { get; set; }

        [StringLength(255)]
        public string Service_Contract { get; set; }

        public decimal? Total_Fee { get; set; }

        public bool? Conflict_of_Interest { get; set; }

        public string Conflict_detail { get; set; }

        [StringLength(50)]
        public string Professional_Service_Category { get; set; }

        [StringLength(255)]
        public string Other_Service_Category { get; set; }

        public decimal? Total_Amount { get; set; }

        public Guid? Special_Request_ID { get; set; }

        public bool? Declared { get; set; }

        public bool? Declared_B { get; set; }

        public bool? Declared_E { get; set; }

        [StringLength(255)]
        public string WebSite { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_BANK_INFO_FOR_DIRECT_TRANSFER> TB_BANK_INFO_FOR_DIRECT_TRANSFER { get; set; }

        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }

        public virtual TB_CPIP_SPECIAL_REQUEST TB_CPIP_SPECIAL_REQUEST { get; set; }

        public virtual TB_INCUBATION_APPLICATION TB_INCUBATION_APPLICATION { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TB_FA_REIMBURSEMENT_ITEM> TB_FA_REIMBURSEMENT_ITEM { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TB_FA_REIMBURSEMENT_SALARY> TB_FA_REIMBURSEMENT_SALARY { get; set; }
    }
}
