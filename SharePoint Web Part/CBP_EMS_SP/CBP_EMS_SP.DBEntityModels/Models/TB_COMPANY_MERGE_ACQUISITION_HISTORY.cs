namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_MERGE_ACQUISITION_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Merge_Acquistion_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Company_Profile_ID { get; set; }

        [StringLength(255)]
        public string Company_Name { get; set; }

        [StringLength(50)]
        public string Merge_Acquistion { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Valuation { get; set; }

        public DateTime? Date { get; set; }

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
