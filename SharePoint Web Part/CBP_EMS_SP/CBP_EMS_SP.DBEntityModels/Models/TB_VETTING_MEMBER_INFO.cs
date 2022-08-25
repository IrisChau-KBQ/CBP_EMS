namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_VETTING_MEMBER_INFO
    {
        [Key]
        public Guid Vetting_Member_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Full_Name { get; set; }

        [StringLength(10)]
        public string Salutation { get; set; }

        [StringLength(500)]
        public string Address1 { get; set; }

        [StringLength(500)]
        public string Address2 { get; set; }
        [StringLength(500)]
        public string Address3 { get; set; }

        [StringLength(250)]
        public string City { get; set; }

        [StringLength(250)]
        public string Country { get; set; }

         [StringLength(255)]
        public string First_Name { get; set; }

         [StringLength(255)]
        public string Title { get; set; }

     

        public bool Registered { get; set; }

        public bool Disabled { get; set; }
    }
}
