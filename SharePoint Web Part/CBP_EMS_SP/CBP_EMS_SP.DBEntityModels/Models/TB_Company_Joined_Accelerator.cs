namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_Company_Joined_Accelerator

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Joined_Accelerator_ID { get; set; }
        public Guid Company_Profile_ID { get; set; }
        public DateTime? Participation_Year_Month { get; set; }

        [StringLength(255)]
        public string Accelerator_Programme { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
        public string Remarks { get; set; }

        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }
    }
}



