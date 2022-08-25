namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_FA_REIMBURSEMENT_SALARY
    {
        [Key]
        public Guid Salary_ID { get; set; }

        public Guid FA_Application_ID { get; set; }

        [StringLength(255)]
        public string Intern_Name { get; set; }

        public decimal? Monthly_Salary { get; set; }

        public decimal? MPF { get; set; }

        public decimal? Tax { get; set; }

        public DateTime? Period_From { get; set; }

        public DateTime? Period_To { get; set; }

        public decimal? Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

    }
}
