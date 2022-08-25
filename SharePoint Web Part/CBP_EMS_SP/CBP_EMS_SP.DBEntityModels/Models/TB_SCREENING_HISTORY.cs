namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_SCREENING_HISTORY
    {
        [Key]
        public int Screening_Comments_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        public int Programme_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Validation_Result { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment_For_Applicants { get; set; }

        [Column(TypeName = "ntext")]
        //[Required]
        public string Comment_For_Internal_Use { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }
    }
}
