namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CASP_SPECIAL_REQUEST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_CASP_SPECIAL_REQUEST()
        {
            TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT = new HashSet<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();
        }

        [Key]
        public Guid CASP_Special_Request_ID { get; set; }

        [StringLength(50)]
        public string Application_No { get; set; }

        public DateTime? Submitted_Date { get; set; }

        public Guid CASP_ID { get; set; }

        public Guid? Company_ID { get; set; }

        [StringLength(255)]
        public string Contact_Name { get; set; }

        [StringLength(20)]
        public string Phone_No { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public Guid? CASP_Attended { get; set; }

        [StringLength(255)]
        public string Service_Provider_Name { get; set; }

        public decimal? Estimate_Amount { get; set; }

        public string Purpose { get; set; }

        public string Description { get; set; }

        public string Justification { get; set; }
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        public DateTime? Completed_Date { get; set; }

        public decimal? Actual_Claim_Amount { get; set; }

        public string Deliver_Cheque_By { get; set; }

        public DateTime? Deliver_Cheque_Date_Finance { get; set; }

        public virtual TB_CASP_APPLICATION TB_CASP_APPLICATION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }

        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }
    }
}
