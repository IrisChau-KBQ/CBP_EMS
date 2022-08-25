namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_APPLICATION_ATTACHMENT
    {
        [Key]
        public int Attachment_ID { get; set; }

        public Guid Application_ID { get; set; }

        public int Programme_ID { get; set; }

        [StringLength(255)]
        public string Attachment_Type { get; set; }

        [StringLength(255)]
        public string Attachment_Path { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
    }
}
