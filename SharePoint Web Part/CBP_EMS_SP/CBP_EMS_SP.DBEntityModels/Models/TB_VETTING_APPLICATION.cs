namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_VETTING_APPLICATION
    {
        [Key]
        public Guid Vetting_Application_ID { get; set; }

        public Guid Vetting_Meeting_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        public DateTime? Presentation_From { get; set; }

        public DateTime? Presentation_To { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Mobile_Number { get; set; }

        public int? Attend { get; set; }

        [Column(TypeName = "ntext")]
        public string Name_of_Attendees { get; set; }

        [Column(TypeName = "ntext")]
        public string Presentation_Tools { get; set; }

        [Column(TypeName = "ntext")]
        public string Special_Request { get; set; }

        public bool? Go { get; set; }
    }
}
