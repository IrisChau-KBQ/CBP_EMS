namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_APPLICATION_MAP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Map_ID { get; set; }

        public Guid Company_Profile_ID { get; set; }

        public Guid Application_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_No { get; set; }

        [Required]
        [StringLength(50)]
        public string Applicaition_Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }
    }
}
