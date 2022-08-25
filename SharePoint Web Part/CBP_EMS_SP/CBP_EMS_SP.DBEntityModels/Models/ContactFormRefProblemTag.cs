namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactFormRefProblemTag")]
    public partial class ContactFormRefProblemTag
    {
        [Key]
        public int RefId { get; set; }

        public int ContactFormId { get; set; }

        public int ContactFormProblemTagId { get; set; }

        public virtual ContactForm ContactForm { get; set; }

        public virtual ContactFormProblemTag ContactFormProblemTag { get; set; }
    }
}
