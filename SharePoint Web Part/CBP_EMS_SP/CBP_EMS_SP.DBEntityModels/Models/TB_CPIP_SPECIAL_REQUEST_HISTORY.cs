namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CPIP_SPECIAL_REQUEST_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        public Guid CPIP_Special_Request_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Application_No { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Submitted_Date { get; set; }

        public Guid? CPIP_id { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid Company_ID { get; set; }

        [StringLength(255)]
        public string Contact_Name { get; set; }

        [StringLength(20)]
        public string Phone_No { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Event_Name { get; set; }

        public DateTime? Event_Date_Start { get; set; }

        public DateTime? Event_Date_End { get; set; }

        [StringLength(50)]
        public string Country_City { get; set; }

        public decimal? Fee { get; set; }

        public string Purpose_1 { get; set; }

        [StringLength(50)]
        public string Service_Provider_Name { get; set; }

        public decimal? Estimate_Amount { get; set; }

        public string Purpose { get; set; }

        public string Description { get; set; }

        public string Justification { get; set; }

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
