namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_PROFILE_BASIC_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        public Guid Company_Profile_ID { get; set; }

        [StringLength(255)]
        public string Name_Eng { get; set; }

        [StringLength(255)]
        public string Name_Chi { get; set; }

        [StringLength(255)]
        public string Company_Name { get; set; }

        [StringLength(255)]
        public string Brand_Name { get; set; }

        [StringLength(50)]
        public string CCMF_Custer { get; set; }

        [StringLength(50)]
        public string CPIP_Custer { get; set; }

        [StringLength(255)]
        public string Tag { get; set; }

        public string CCMF_Abstract { get; set; }

        public string CPIP_Abstract { get; set; }

        public string CPIP_Abstract_Chi { get; set; }

        public string CASP_Abstract { get; set; }

        public string Company_Ownership_Structure { get; set; }

        public string Remarks { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
    }
}
