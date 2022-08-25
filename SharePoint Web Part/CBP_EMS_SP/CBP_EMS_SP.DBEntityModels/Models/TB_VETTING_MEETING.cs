namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_VETTING_MEETING
    {
        [Key]
        public Guid Vetting_Meeting_ID { get; set; }
        [StringLength(50)]
        public string Meeting_status { get; set; }
        [StringLength(50)]
        public string Time_Interval { get; set; }
        public int Programme_ID { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public DateTime Vetting_Meeting_From { get; set; }

        public DateTime Vetting_Meeting_To { get; set; }

        public DateTime Presentation_From { get; set; }

        public DateTime Presentation_To { get; set; }

        [Required]
        [StringLength(255)]
        public string Vetting_Team_Leader { get; set; }

        public int No_of_Attendance { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; }

        public bool? Meeting_Completed { get; set; }
        public DateTime Modified_Date { get; set; }
        public bool? Decision_Completed { get; set; }
    }
}
