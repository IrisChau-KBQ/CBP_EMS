namespace CBP_EMS_SP.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactForm")]
    public partial class ContactForm
    {
        public ContactForm()
        {
            this.ContactFormProblemTags = new HashSet<ContactFormProblemTag>();
        }

        [Key]
        public int ContactFormId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [Column(TypeName = "text")]
        public string Inquiry { get; set; }

        public int ContactFormResponsibleSalesId { get; set; }

        [ForeignKey("ContactFormResponsibleSalesId")]
        public virtual ContactFormResponsibleSales ContactFormResponsibleSales { get; set; }

        public virtual ICollection<ContactFormProblemTag> ContactFormProblemTags { get; set; }
    }
}
