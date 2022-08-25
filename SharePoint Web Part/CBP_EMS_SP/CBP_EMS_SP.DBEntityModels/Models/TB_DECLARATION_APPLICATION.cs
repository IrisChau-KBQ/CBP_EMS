namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_DECLARATION_APPLICATION
    {
        [Key]
        public Guid Vetting_Delclaration_ID { get; set; }

        //public Guid Vetting_Meeting_ID { get; set; }
        public int Conflict_Of_Interest { get; set; }
        public string Reason { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }
    }
}
