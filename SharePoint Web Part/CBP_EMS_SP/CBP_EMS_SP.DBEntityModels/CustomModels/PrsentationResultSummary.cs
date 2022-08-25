using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CBP_EMS_SP.Data.Models;

namespace CBP_EMS_SP.Data.CustomModels
{

    public class PresentationVettingUser
    {
        public TB_VETTING_MEMBER VettingMember { get; set; }
        public TB_VETTING_MEMBER_INFO UserData { get; set; }
        public bool IsDeclared { get; set; }
        public bool Isconfirmed { get; set; }
    }
    public class Finallistsummary
    {
        public TB_INCUBATION_APPLICATION Incubation_score { get; set; }
        public TB_VETTING_APPLICATION Vetting_Application { get; set; }
        public TB_VETTING_MEETING Vetting_Metting { get; set; }
        public PrsentationResultSummary ResultSummary { get; set; }
    }
    public class PrsentationResultSummary
    {
        public int offsite { get; set; }
        public int onsite { get; set; }
        public string Preferred_track { get; set; }
        public string Meeting_Status { get; set; }
        public Guid Vetting_Meeting_ID { get; set; }
        public string Application_Number { get; set; }
        public string Cluster { get; set; }
        public string Programme_Type { get; set; }
        public string Application_Type { get; set; }
        public string company_name { get; set; }
        public decimal? Totalscore { get; set; }
        public decimal? Averagescore { get; set; }
        public int totalrecommended { get; set; }
        public int totalvotes { get; set; }
        public int isRecommended { get; set; }
        public int isNotRecommended { get; set; }
        public int ProgramId { get; set; }
        public bool recommended { get; set; }
        public List<PresentationVettingUser> userdata { get; set; }
        public List<Presentation_Score> Score_of_vettingmember { get; set; }
        public int? Recommendedcount { get; set; }
        public int? NotRecommendedcount { get; set; }
        public Guid Vetting_Application_ID { get; set; }
        public bool? Withdraw { get; set; }
        public string Remark { get; set; }
        public string Result { get; set; }
        public Nullable<DateTime>  PresentationTime { get; set; }

    }

    public class Presentation_Score
    {
        public string Member_Email { get; set; }
        public bool? Go { get; set; }
        public decimal? Total_Score { get; set; }
        public string Remarks { get; set; }
    }
}
