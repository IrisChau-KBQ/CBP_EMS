namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactForm")]
    public partial class ContactForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactForm()
        {
            ContactFormRefProblemTags = new HashSet<ContactFormRefProblemTag>();
        }

        public int ContactFormId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [Column(TypeName = "text")]
        public string Inquiry { get; set; }

        public int? ContactFormResponsibleSalesId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactFormRefProblemTag> ContactFormRefProblemTags { get; set; }

        public virtual ContactFormResponsibleSale ContactFormResponsibleSale { get; set; }
    }
}
