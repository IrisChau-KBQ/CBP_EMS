//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBP_EMS_SP.DevelopWP
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContactForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactForm()
        {
            this.ContactFormRefProblemTags = new HashSet<ContactFormRefProblemTag>();
        }
    
        public int ContactFormId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Inquiry { get; set; }
        public Nullable<int> ContactFormResponsibleSalesId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactFormRefProblemTag> ContactFormRefProblemTags { get; set; }
        public virtual ContactFormResponsibleSale ContactFormResponsibleSale { get; set; }
    }
}
