namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
    {
        [Key]
        public Guid CASP_FA_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_No { get; set; }

        public DateTime? Submitted_Date { get; set; }

        [Required]
        [StringLength(1)]
        public string Category { get; set; }

        public Guid? Company_ID { get; set; }

        public Guid? CASP_Attended { get; set; }

        [StringLength(50)]
        public string Payable_To { get; set; }

        public Guid? Preapproved_SpecialRequest { get; set; }

        public bool? Conflict_of_Interest { get; set; }

        public string Conflict_detail { get; set; }

        public decimal? Total_Amount { get; set; }

        public bool? Declared_A { get; set; }

        public bool? Declared_D { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime? Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        public DateTime? Estimated_Service_From { get; set; }
        public DateTime? Estimated_Service_To { get; set; }
        public bool Prepaid_Service { get; set; }
        public bool Freelancer { get; set; }
        public string Service_Provider_Name { get; set; }
        public string Service_Contract { get; set; }
        public decimal? Total_Fee { get; set; }

        public DateTime? Completed_Date { get; set; }

        public decimal? Actual_Claim_Amount { get; set; }

        [StringLength(255)]
        public string Deliver_Cheque_By { get; set; }

        public DateTime? Deliver_Cheque_Date_Finance { get; set; }
        public DateTime? Deliver_Cheque_Date_Coordinator { get; set; }

        public decimal? Total_Amount_After_Deduction { get; set; }

        public virtual TB_CASP_APPLICATION TB_CASP_APPLICATION { get; set; }

        public virtual TB_CASP_SPECIAL_REQUEST TB_CASP_SPECIAL_REQUEST { get; set; }

        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }
    }
}
