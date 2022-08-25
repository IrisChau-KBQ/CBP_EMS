namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_BANK_INFO_FOR_DIRECT_TRANSFER
    {
        [Key]
        public Guid Bank_Info_ID { get; set; }

        public Guid CPIP_FA_ID { get; set; }

        [StringLength(255)]
        public string Bank_Name { get; set; }

        [StringLength(50)]
        public string Bank_Code { get; set; }

        [StringLength(50)]
        public string Bank_Account_No { get; set; }

        [StringLength(50)]
        public string Bank_SWIFT_Code { get; set; }

        [StringLength(255)]
        public string Account_Holder { get; set; }

        public bool? Agree { get; set; }

        public string Remark { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        public virtual TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }
    }
}
