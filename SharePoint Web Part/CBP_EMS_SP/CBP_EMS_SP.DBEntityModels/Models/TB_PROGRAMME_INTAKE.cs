namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_PROGRAMME_INTAKE
    {
        [Key]
        public int Programme_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Programme_Name { get; set; }

        public int Intake_Number { get; set; }

        [Required]
        [StringLength(30)]
        public string Application_No_Prefix { get; set; }

        public DateTime Application_Start { get; set; }

        public DateTime Application_Deadline { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Deadline_Eng { get; set; }

        [StringLength(50)]
        public string Application_Deadline_TradChin { get; set; }

        [StringLength(50)]
        public string Application_Deadline_SimpChin { get; set; }

        [Required]
        [StringLength(50)]
        public string Vetting_Session_Eng { get; set; }

        [StringLength(50)]
        public string Vetting_Session_TradChin { get; set; }

        [StringLength(50)]
        public string Vetting_Session_SimpChin { get; set; }

        [Required]
        [StringLength(50)]
        public string Result_Announce_Eng { get; set; }

        [StringLength(50)]
        public string Result_Announce_TradChin { get; set; }

        [StringLength(50)]
        public string Result_Announce_Simp_Chin { get; set; }

        public bool Active { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        // CCMF & YEP Check Box
        public bool ProfShow { get; set; }

        public bool YEPShow { get; set; }
    }
}
