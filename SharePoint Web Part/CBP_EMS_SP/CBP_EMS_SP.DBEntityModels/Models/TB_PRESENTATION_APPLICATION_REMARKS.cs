namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_PRESENTATION_APPLICATION_REMARKS
    {
        [Key]
        public Guid Vetting_Meeting_Remark_ID { get; set; }

        public Guid Vetting_Appilcation_ID { get; set; }

        public bool Withdraw { get; set; }

        [StringLength(255)]
        public string  Remark { get; set; }

        public string Created_By { get; set; }

       
        public DateTime Created_Date { get; set; }

      
        public string  Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

       
      
    }
}
