namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_FUND_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Funding_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Company_Profile_ID { get; set; }

        [StringLength(255)]
        public string Programme_Name { get; set; }

        [StringLength(20)]
        public string Programme_Type { get; set; }

        [StringLength(50)]
        public string Application_No { get; set; }

        [StringLength(255)]
        public string Funding_Status { get; set; }

        [StringLength(255)]
        public string Expenditure_Nature { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        public decimal? Amount_Received { get; set; }

        public decimal? Maximum_Amount { get; set; }

        [StringLength(50)]
        public string Application_Status { get; set; }

        [StringLength(255)]
        public string Funding_Origin { get; set; }

        public string Invertor_Info { get; set; }

        public string Remarks { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
    }
}
