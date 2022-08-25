namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CASP_SPECIAL_REQUEST_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        public Guid CASP_Special_Request_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Application_No { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Submitted_Date { get; set; }

        [Key]
        [Column(Order = 3)]
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

        public decimal? Actual_Claim_Amount { get; set; }

        public string Deliver_Cheque_By { get; set; }

        public DateTime? Deliver_Cheque_Date_Finance { get; set; }

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
