namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_BANK_INFO_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        public Guid Bank_Info_ID { get; set; }

        [Key]
        [Column(Order = 1)]
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
