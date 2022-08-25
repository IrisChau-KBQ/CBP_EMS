namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_VETTING_MEMBER
    {
        [Key]
        [Column(Order = 0)]
        public Guid Vetting_Meeting_ID { get; set; }

        public Guid Vetting_Member_ID { get; set; }
        public bool isLeader { get; set; }

        public bool? PList_Confirmed { get; set; }
        //[Key]
        //[Column(Order = 1)]
        //[StringLength(255)]
        //public string Member_Email { get; set; }
    }
}
