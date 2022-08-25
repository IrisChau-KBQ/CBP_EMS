namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_EMAIL_TEMPLATE
    {
        [Key]
        [StringLength(50)]
        public string Email_Template { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Email_Template_Content { get; set; }
    }
}
