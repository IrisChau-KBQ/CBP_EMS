namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_COMPANY_MEMBER_HISTORY
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Core_Member_ID { get; set; }

        public Guid Company_Profile_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Position { get; set; }

        public bool? CCMF { get; set; }

        public bool? CPIP { get; set; }

        public bool? CASP { get; set; }

        [StringLength(255)]
        public string HKID { get; set; }

        [StringLength(255)]
        public string Masked_HKID { get; set; }

        public string Background_Information { get; set; }

        [StringLength(255)]
        public string Bootcamp_Eligible_Number { get; set; }

        [StringLength(255)]
        public string Professional_Qualifications { get; set; }

        [StringLength(255)]
        public string Working_Experiences { get; set; }

        [StringLength(255)]
        public string Special_Achievements { get; set; }

        [StringLength(255)]
        public string CoreMember_Profile { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime? Modified_Date { get; set; }
    }
}
