using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CBP_EMS_SP.Model
{
    [Table("ContactFormProblemTag")]
    public partial class ContactFormProblemTag
    {
        public ContactFormProblemTag()
        {
            this.ContactForms = new HashSet<ContactForm>();
        }

        [Key]
        public int ContactFormProblemTagId { get; set; }

        [StringLength(255)]
        public string ProblemTagName { get; set; }

        public virtual ICollection<ContactForm> ContactForms { get; set; }
    }
}
