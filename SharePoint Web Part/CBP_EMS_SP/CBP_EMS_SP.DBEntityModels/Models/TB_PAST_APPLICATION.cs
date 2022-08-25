namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_PAST_APPLICATION
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Application_Number { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string ID_Number { get; set; }
    }
}
