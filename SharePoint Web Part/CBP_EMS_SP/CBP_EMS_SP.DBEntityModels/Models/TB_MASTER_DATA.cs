namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_MASTER_DATA
    {
        [Key]
        [StringLength(30)]
        public string Data_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Data_Value { get; set; }

        public bool Active { get; set; }
    }
}
