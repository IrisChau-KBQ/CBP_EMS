namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_APPLICATION_SHORTLISTING
    {
        [Key]
        public int Application_Shortlisting_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        public int Programme_ID { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Remarks_To_Vetting { get; set; }

        public bool Shortlisted { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
    }
}
