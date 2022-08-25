namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_IP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IP_ID { get; set; }

        public Guid Company_Profile_ID { get; set; }

        [StringLength(255)]
        public string Category { get; set; }

        public DateTime? Registration_Date { get; set; }

        public DateTime? Reported_Date { get; set; }

        [StringLength(50)]
        public string Reference_No { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
        public string Title { get; set; }

        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }
    }
}
