namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_SCREENING_CCMF_SCORE
    {
        [Key]
        public int CCMF_Scoring_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        public int Programme_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Reviewer { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        public decimal Management_Team { get; set; }

        public decimal Business_Model { get; set; }

        public decimal Creativity { get; set; }

        public decimal Social_Responsibility { get; set; }

        public decimal Total_Score { get; set; }

        [StringLength(30)]
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
