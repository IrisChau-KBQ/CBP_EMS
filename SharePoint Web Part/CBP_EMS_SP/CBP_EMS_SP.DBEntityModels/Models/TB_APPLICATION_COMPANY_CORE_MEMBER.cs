namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_APPLICATION_COMPANY_CORE_MEMBER
    {
        [Key]
        public int Core_Member_ID { get; set; }

        public Guid Application_ID { get; set; }

        public int Programme_ID { get; set; }

     
        [StringLength(255)]
        public string Name { get; set; }

       
        [StringLength(255)]
        public string Position { get; set; }

       
        [StringLength(255)]
        public string HKID { get; set; }

        [StringLength(255)]
        public string Masked_HKID { get; set; }


        [Column(TypeName = "ntext")]
        public string Background_Information { get; set; }

        [StringLength(255)]
        public string Bootcamp_Eligible_Number { get; set; }
        [StringLength(255)]
        public string Professional_Qualifications { get; set; }
        [StringLength(255)]
        public string Working_Experiences { get; set; }
        [StringLength(255)]
        public string Special_Achievements { get; set; }

        [Column(TypeName = "ntext")]
        public string CoreMember_Profile { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
    }
}
