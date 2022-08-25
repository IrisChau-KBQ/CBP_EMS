namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_CONTACT_HISTORY
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Contact_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Company_Profile_ID { get; set; }

        [Key]
        [Column(Order = 2)]
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

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Created_By { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
    }
}
