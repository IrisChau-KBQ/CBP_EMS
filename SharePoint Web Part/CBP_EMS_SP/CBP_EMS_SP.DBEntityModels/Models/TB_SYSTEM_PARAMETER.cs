namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_SYSTEM_PARAMETER
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Config_Code { get; set; }

        [StringLength(255)]
        public string Value { get; set; }

        [StringLength(20)]
        public string Created_by { get; set; }

        public DateTime? Created_date { get; set; }

        [StringLength(20)]
        public string Modified_by { get; set; }

        public DateTime? Modified_date { get; set; }
    }
}
