namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CASP_APPLICATION_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        public Guid CASP_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Application_No { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid Company_Proejct { get; set; }

        [StringLength(50)]
        public string CCMF_CPIP_App_No { get; set; }

        public string Company_Address { get; set; }

        public string Abstract { get; set; }

        public string Company_Ownership_Structure { get; set; }

        public string Additional_Info { get; set; }

        [StringLength(255)]
        public string Accelerator_Name { get; set; }

        public bool? Endorsed_by_Cyberport { get; set; }

        public DateTime? Commencement_Date { get; set; }

        public int? Duration { get; set; }

        public string Background { get; set; }

        public string Offer { get; set; }

        public string Fund_Raising_Capabilities { get; set; }

        public string Size_of_Alumni { get; set; }

        public string Reputation { get; set; }

        [StringLength(255)]
        public string Website { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Declaration { get; set; }

        [StringLength(255)]
        public string Principle_Full_Name { get; set; }

        [StringLength(255)]
        public string Principle_Title { get; set; }

        public DateTime? Submitted_Date { get; set; }

        [StringLength(50)]
        public string Created_By { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
