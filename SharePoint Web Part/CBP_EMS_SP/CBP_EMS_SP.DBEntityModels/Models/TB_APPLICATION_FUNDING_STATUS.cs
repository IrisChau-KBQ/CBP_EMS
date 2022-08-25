namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_APPLICATION_FUNDING_STATUS
    {
        [Key]
        public int Funding_ID { get; set; }

        public Guid Application_ID { get; set; }

        public int Programme_ID { get; set; }

        public DateTime? Date { get; set; }

       
        [StringLength(255)]
        public string Programme_Name { get; set; }

      
        [StringLength(255)]
        public string Application_Status { get; set; }

        [StringLength(255)]
        public string Funding_Status { get; set; }

        [StringLength(255)]
        public string Expenditure_Nature { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        public decimal? Amount_Received { get; set; }

        public decimal? Maximum_Amount { get; set; }
    }
}
