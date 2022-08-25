namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_VETTING_DECLARATION
    {
        [Key]
        public Guid Vetting_Delclaration_ID { get; set; }

        public Guid Vetting_Meeting_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Member_Email { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool No_Conflict_Application { get; set; }

        public bool Abstained_Voting_Application { get; set; }
    }
}
