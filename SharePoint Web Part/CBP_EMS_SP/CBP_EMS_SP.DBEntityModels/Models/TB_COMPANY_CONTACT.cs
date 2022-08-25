namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_CONTACT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Contact_ID { get; set; }

        public Guid Company_Profile_ID { get; set; }


        [StringLength(255)]
        public string Name_Eng { get; set; }

        [StringLength(255)]
        public string Name_Chi { get; set; }

        [StringLength(255)]
        public string Position { get; set; }

        [StringLength(20)]
        public string Contact_No { get; set; }

        [StringLength(20)]
        public string Fax_No { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public string Mailing_Address { get; set; }

        public string Additional_Info { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        public string Salutation { get; set; }
        public string HKID { get; set; }
        public DateTime? Graduation_Date { get; set; }
        public string Education_Institution { get; set; }
        public string Student_ID { get; set; }
        public string Programme_Enrolled { get; set; }
        public string Organization_Name { get; set; }
        public string Contact_No_Home { get; set; }
        public string Area { get; set; }
        public string Contact_No_Office { get; set; }
        public string Masked_HKID { get; set; }
        public bool? No_Edit { get; set; }
        public virtual TB_COMPANY_PROFILE_BASIC TB_COMPANY_PROFILE_BASIC { get; set; }
    }
}
