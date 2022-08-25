namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_INCUBATION_APPLICATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_INCUBATION_APPLICATION()
        {
            //TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT = new HashSet<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>();
        }
        [Key]
        public Guid Incubation_ID { get; set; }

        public int Programme_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Application_Number { get; set; }

        public int Intake_Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Applicant { get; set; }

        public DateTime Last_Submitted { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(20)]
        public string Version_Number { get; set; }

        public bool? Question1_1 { get; set; }

        public bool? Question1_2 { get; set; }

        public bool? Question1_3 { get; set; }

        public bool? Question1_4 { get; set; }

        public bool? Question1_5 { get; set; }

        public bool? Question1_6 { get; set; }

        public bool? Question1_7 { get; set; }

        public bool? Question1_8 { get; set; }

        public bool? Question1_8_1 { get; set; }

        public bool? Question1_9 { get; set; }

        public bool? Question1_10 { get; set; }


        [StringLength(255)]
        public string Company_Name_Eng { get; set; }

        [StringLength(255)]
        public string Company_Name_Chi { get; set; }

        [Column(TypeName = "ntext")]
        public string Abstract { get; set; }
        [Column(TypeName = "ntext")]
        public string Abstract_Chi { get; set; }

        [Column(TypeName = "ntext")]
        public string Objective { get; set; }

        [Column(TypeName = "ntext")]
        public string Background { get; set; }

        [Column(TypeName = "ntext")]
        public string Pilot_Work_Done { get; set; }

        [Column(TypeName = "ntext")]
        public string Additional_Information { get; set; }

        [Column(TypeName = "ntext")]
        public string Proposed_Products { get; set; }

        [Column(TypeName = "ntext")]
        public string Target_Market { get; set; }

        [Column(TypeName = "ntext")]
        public string Competition_Analysis { get; set; }

        [Column(TypeName = "ntext")]
        public string Revenus_Model { get; set; }

        [Column(TypeName = "ntext")]
        public string Exit_Strategy { get; set; }


        public string First_6_Months_Milestone { get; set; }


        public string Second_6_Months_Milestone { get; set; }


        public string Third_6_Months_Milestone { get; set; }


        public string Forth_6_Months_Milestone { get; set; }

        public bool? Resubmission { get; set; }

        [Column(TypeName = "ntext")]
        public string Resubmission_Project_Reference { get; set; }

        [Column(TypeName = "ntext")]
        public string Resubmission_Main_Differences { get; set; }

        [StringLength(10)]
        public string Company_Type { get; set; }

        [StringLength(255)]
        public string Other_Company_Type { get; set; }

        [StringLength(40)]
        public string Business_Area { get; set; }

        [StringLength(30)]
        public string Other_Bussiness_Area { get; set; }

        [Column(TypeName = "ntext")]
        public string Positioning { get; set; }

        [StringLength(255)]
        public string Management_Positioning { get; set; }

        [StringLength(255)]
        public string Other_Positioning { get; set; }

        [Column(TypeName = "ntext")]
        public string Other_Attributes { get; set; }

        [StringLength(30)]
        public string Preferred_Track { get; set; }

        [Column(TypeName = "ntext")]
        public string Company_Ownership_Structure { get; set; }

        [Column(TypeName = "ntext")]
        public string Core_Members_Profiles { get; set; }

        [Column(TypeName = "ntext")]
        public string Major_Partners_Profiles { get; set; }

        [Column(TypeName = "ntext")]
        public string Manpower_Distribution { get; set; }

        [Column(TypeName = "ntext")]
        public string Equipment_Distribution { get; set; }

        [Column(TypeName = "ntext")]
        public string Other_Direct_Costs { get; set; }

        [Column(TypeName = "ntext")]
        public string Forecast_Income { get; set; }

        public bool? Declaration { get; set; }

        public bool? Have_Read_Statement { get; set; }

        public bool? Marketing_Information { get; set; }

        [StringLength(255)]
        public string Principal_Full_Name { get; set; }

        [StringLength(255)]
        public string Principal_Position_Title { get; set; }

        public DateTime? Submission_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
        [StringLength(36)]
        public string Application_Parent_ID { get; set; }
        public bool Awarded { get; set; }
        public string Project_Name { get; set; }
        public DateTime? Establishment_Year { get; set; }
        public string Country_Of_Origin { get; set; }
        public bool? NEW_to_HK { get; set; }

        [StringLength(255)]
        public string Website { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }

    }
}
