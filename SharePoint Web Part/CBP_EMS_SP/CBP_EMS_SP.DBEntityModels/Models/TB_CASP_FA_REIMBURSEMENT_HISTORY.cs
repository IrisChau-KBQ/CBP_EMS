namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CASP_FA_REIMBURSEMENT_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        public Guid CASP_FA_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Application_No { get; set; }

        public DateTime? Submitted_Date { get; set; }

        [Key]
        [Column(Order = 2)]
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

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
    }
}
