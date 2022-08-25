namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_PRESENTATION_INCUBATION_SCORE
    {
        [Key]
        public int Incubation_Scoring_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        public int Programme_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Member_Email { get; set; }

        public decimal Management_Team { get; set; }

        public decimal Creativity { get; set; }

        public decimal Business_Viability { get; set; }

        public decimal Benefit_To_Industry { get; set; }

        public decimal Proposal_Milestones { get; set; }
        public bool? Go { get; set; }

        public decimal Total_Score { get; set; }

        [StringLength(255)]
        public string Comments { get; set; }

        [Column(TypeName = "ntext")]
        public string Remarks { get; set; }

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
