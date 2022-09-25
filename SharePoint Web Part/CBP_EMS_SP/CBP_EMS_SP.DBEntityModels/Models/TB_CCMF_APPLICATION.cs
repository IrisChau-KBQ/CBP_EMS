namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_CCMF_APPLICATION
    {
        [Key]
        public Guid CCMF_ID { get; set; }

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

        [Required]
        [StringLength(251)]
        public string Programme_Type { get; set; }

        [StringLength(251)]
        public string Hong_Kong_Programme_Stream { get; set; }

        [StringLength(251)]
        public string CrossBorder_Programme_Type { get; set; }

        [StringLength(251)]
        public string CCMF_Application_Type { get; set; }

        public bool? Question2_1_1a { get; set; }

        public bool? Question2_1_1b { get; set; }

        public bool? Question2_1_1c { get; set; }

        public bool? Question2_1_1d { get; set; }

        public bool? Question2_1_1e { get; set; }

        public bool? Question2_1_1f { get; set; }

        public bool? Question2_1_1g { get; set; }

        public bool? Question2_1_1h { get; set; }

        public bool? Question2_1_1i { get; set; }

        public bool? Question2_1_1j { get; set; }

        public bool? Question2_1_2a { get; set; }

        public bool? Question2_1_2b { get; set; }

        public bool? Question2_1_2c { get; set; }

        public bool? Question2_1_2d { get; set; }

        public bool? Question2_1_2e { get; set; }

        public bool? Question2_1_2f { get; set; }

        public bool? Question2_1_2f_1 { get; set; }

        public bool? Question2_1_2g { get; set; }

        public bool? Question2_1_2h { get; set; }

        public bool? Question2_1_2i { get; set; }

        public bool? Question2_1_2j { get; set; }

        public bool? Question2_1_2k { get; set; }
        
        public bool? Question2_1_2l { get; set; }
        
        public bool? Question2_1_2m { get; set; }
        
        public bool? Question2_1_2n { get; set; }

        public bool? Question1_3 { get; set; }

        public bool? Question2_2a { get; set; }

        public bool? Question2_2b { get; set; }

        public bool? Question2_2c { get; set; }

        public bool? Question2_2d { get; set; }

        public bool? Question2_2e { get; set; }

        public bool? Question2_2f { get; set; }

        public bool? Question2_2g { get; set; }

        public bool? Question2_2h { get; set; }

        public bool? Question2_2i { get; set; }

        public bool? Question2_2j { get; set; }

        public bool? Question2_2k { get; set; }

        public bool? Question2_2l { get; set; }

        public bool? Question2_2m { get; set; }

        public bool? Question2_3_1a { get; set; }

        public bool? Question2_3_1b { get; set; }

        public bool? Question2_3_1c { get; set; }

        public bool? Question2_3_1d { get; set; }

        public bool? Question2_3_1e { get; set; }

        public bool? Question2_3_1f { get; set; }

        public bool? Question2_3_1g { get; set; }

        public bool? Question2_3_1h { get; set; }

        public bool? Question2_3_1i { get; set; }

        public bool? Question2_3_1j { get; set; }

        public bool? Question2_3_1k { get; set; }

    
        [StringLength(255)]
        public string Project_Name_Eng { get; set; }

        [StringLength(255)]
        public string Project_Name_Chi { get; set; }

        [Column(TypeName = "ntext")]
        public string Abstract_Eng { get; set; }

        [Column(TypeName = "ntext")]
        public string Abstract_Chi { get; set; }

        [StringLength(30)]
        public string Business_Area { get; set; }

        [StringLength(255)]
        public string Other_Business_Area { get; set; }

        public DateTime? Commencement_Date { get; set; }

        public DateTime? Completion_Date { get; set; }

        public bool? Declaration { get; set; }

        public bool? Have_Read_Statement { get; set; }

        public bool? Marketing_Information { get; set; }

        [StringLength(255)]
        public string Principal_Full_Name { get; set; }

        [StringLength(255)]
        public string Principal_Position_Title { get; set; }

        [StringLength(255)]
        public string Principal_2nd_Full_Name { get; set; }

        [StringLength(255)]
        public string Principal_2nd_Position_Title { get; set; }
        
        [StringLength(50)]
        public string Principal_2nd_Email { get; set; }

        public DateTime? Submission_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Modified_By { get; set; }

        public DateTime Modified_Date { get; set; }
        public string Project_Management_Team { get; set; }

        public string Business_Model { get; set; }
        public string Advisor_Info { get; set; }

        public string Innovation { get; set; }

        public string Social_Responsibility { get; set; }

        public string Competition_Analysis { get; set; }

       
        public string Project_Milestone_M1 { get; set; }

        
        public string Project_Milestone_M2 { get; set; }

      
        public string Project_Milestone_M3 { get; set; }

       
        public string Project_Milestone_M4 { get; set; }

       
        public string Project_Milestone_M5 { get; set; }

       
        public string Project_Milestone_M6 { get; set; }

        public string Cost_Projection { get; set; }

        public string Exit_Stategy { get; set; }

        public string Additional_Information { get; set; }
        [StringLength(36)]
        public string Application_Parent_ID { get; set; }
        public bool Awarded { get; set; }
        public string SmartSpace { get; set; }
        //public string Project_Name { get; set; }
        [StringLength(255)]
        public string Company_Name { get; set; }
        public DateTime? Establishment_Year { get; set; }

        [StringLength(255)]
        public string Country_Of_Origin { get; set; }
        public bool? NEW_to_HK { get; set; }
    }
}
