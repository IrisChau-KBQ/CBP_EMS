namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_AWARD_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Award_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Company_Profile_ID { get; set; }

        public DateTime? Awarded_Year_Month { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Nature { get; set; }

        [StringLength(255)]
        public string Product_Name { get; set; }

        [StringLength(255)]
        public string Award_Name { get; set; }

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
