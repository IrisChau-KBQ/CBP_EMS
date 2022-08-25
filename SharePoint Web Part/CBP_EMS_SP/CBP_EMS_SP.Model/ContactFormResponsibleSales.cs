using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CBP_EMS_SP.Model
{
    [Table("ContactFormResponsibleSales")]
    public partial class ContactFormResponsibleSales
    {
        [Key]
        public int ContactFormResponsibleSalesId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
