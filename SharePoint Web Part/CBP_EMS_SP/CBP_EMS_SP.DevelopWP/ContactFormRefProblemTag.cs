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
    
    public partial class ContactFormRefProblemTag
    {
        public int RefId { get; set; }
        public int ContactFormId { get; set; }
        public int ContactFormProblemTagId { get; set; }
    
        public virtual ContactForm ContactForm { get; set; }
        public virtual ContactFormProblemTag ContactFormProblemTag { get; set; }
    }
}