namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_APPLICATION_CONTACT_DETAIL
    {
        [Key]
        public int CONTACT_DETAILS_ID { get; set; }

        public Guid Application_ID { get; set; }

        public int Programme_ID { get; set; }

      
        [StringLength(255)]
        public string Last_Name_Eng { get; set; }

      
        [StringLength(255)]
        public string First_Name_Eng { get; set; }

        [StringLength(255)]
        public string Last_Name_Chi { get; set; }

        [StringLength(255)]
        public string First_Name_Chi { get; set; }

     
        [StringLength(255)]
        public string Salutation { get; set; }

        [StringLength(255)]
        public string ID_Number { get; set; }

        [StringLength(255)]
        public string Student_ID_Number { get; set; }

        [StringLength(255)]
        public string Education_Institution_Eng { get; set; }

        [StringLength(255)]
        public string Programme_Enrolled_Eng { get; set; }
        [StringLength(50)]
        public string Area { get; set; }

        public int? Graduation_Month { get; set; }

        public int? Graduation_Year { get; set; }

        [StringLength(255)]
        public string Organisation_Name { get; set; }

        [StringLength(255)]
        public string Position { get; set; }
        [StringLength(255)]
        public string Contact_No { get; set; }

        [StringLength(20)]
        public string Contact_No_Type { get; set; }
        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [Column(TypeName = "ntext")]
        public string Mailing_Address { get; set; }

        [StringLength(255)]
        public string Contact_No_Home { get; set; }
        [StringLength(255)]
        public string Contact_No_Office { get; set; }
        [StringLength(255)]
        public string Contact_No_Mobile { get; set; }

        public string  Nationality { get; set; }
    }
}
