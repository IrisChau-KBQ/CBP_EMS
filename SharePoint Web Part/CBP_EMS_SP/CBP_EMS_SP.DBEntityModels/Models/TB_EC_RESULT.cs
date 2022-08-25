namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_EC_RESULT
    {
        [Key]
        public Guid EC_Result_ID { get; set; }

        public int Programme_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }
        [StringLength(30)]
        public string Cluster { get; set; }
        [StringLength(255)]
        public string Company_Program { get; set; }
        [StringLength(30)]
        public string Programme_Type { get; set; }
        [StringLength(30)]
        public string Application_Type { get; set; }
        public bool Recommended { get; set; }
      
        public int Total_votes { get; set; }
        public string Remarks { get; set; }
        [StringLength(30)]
        public string Status { get; set; }

        public int Recommendedcount { get; set; }
        public int NotRecommendedcount { get; set; }
        public string Confirm_By { get; set; }
   
    }
}
