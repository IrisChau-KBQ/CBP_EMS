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
    
    public partial class ContactFormResponsibleSale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactFormResponsibleSale()
        {
            this.ContactForms = new HashSet<ContactForm>();
        }
    
        public int ContactFormResponsibleSalesId { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactForm> ContactForms { get; set; }
    }
}
