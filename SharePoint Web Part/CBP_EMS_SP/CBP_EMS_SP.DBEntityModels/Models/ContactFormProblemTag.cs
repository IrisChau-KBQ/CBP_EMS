namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactFormProblemTag")]
    public partial class ContactFormProblemTag
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactFormProblemTag()
        {
            ContactFormRefProblemTags = new HashSet<ContactFormRefProblemTag>();
        }

        public int ContactFormProblemTagId { get; set; }

        [Required]
        [StringLength(255)]
        public string ProblemTagName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactFormRefProblemTag> ContactFormRefProblemTags { get; set; }
    }
}
