//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBP_EMS_SP.DevelopWP
{
    using System;
    using System.Collections.Generic;
    
    public partial class TB_SCREENING_CCMF_SCORE
    {
        public int CCMF_Scoring_ID { get; set; }
        public string Application_Number { get; set; }
        public Nullable<int> Programme_ID { get; set; }
        public string Reviewer { get; set; }
        public string Role { get; set; }
        public Nullable<decimal> Management_Team { get; set; }
        public Nullable<decimal> Business_Model { get; set; }
        public Nullable<decimal> Creativity { get; set; }
        public Nullable<decimal> Social_Responsibility { get; set; }
        public Nullable<decimal> Total_Score { get; set; }
        public string Comments { get; set; }
        public string Remarks { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}