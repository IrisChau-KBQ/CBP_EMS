namespace CBP_EMS_SP.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CyberportEMS_EDM : DbContext
    {
        public CyberportEMS_EDM()
            : base("name=CyberportEMSConnectionString")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public virtual DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public virtual DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        public virtual DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        public virtual DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public virtual DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        public virtual DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public virtual DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public virtual DbSet<aspnet_Users> aspnet_Users { get; set; }
        public virtual DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        public virtual DbSet<ContactForm> ContactForms { get; set; }
        public virtual DbSet<ContactFormProblemTag> ContactFormProblemTags { get; set; }
        public virtual DbSet<ContactFormRefProblemTag> ContactFormRefProblemTags { get; set; }
        public virtual DbSet<ContactFormResponsibleSale> ContactFormResponsibleSales { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TB_APPLICATION_ATTACHMENT> TB_APPLICATION_ATTACHMENT { get; set; }
        public virtual DbSet<TB_APPLICATION_COLLABORATOR> TB_APPLICATION_COLLABORATOR { get; set; }
        public virtual DbSet<TB_APPLICATION_COMPANY_CORE_MEMBER> TB_APPLICATION_COMPANY_CORE_MEMBER { get; set; }
        public virtual DbSet<TB_APPLICATION_CONTACT_DETAIL> TB_APPLICATION_CONTACT_DETAIL { get; set; }
        public virtual DbSet<TB_APPLICATION_FUNDING_STATUS> TB_APPLICATION_FUNDING_STATUS { get; set; }
        public virtual DbSet<TB_APPLICATION_SHORTLISTING> TB_APPLICATION_SHORTLISTING { get; set; }
        //public virtual DbSet<TB_BANK_INFO_FOR_DIRECT_TRANSFER> TB_BANK_INFO_FOR_DIRECT_TRANSFER { get; set; }
        public virtual DbSet<TB_CASP_APPLICATION> TB_CASP_APPLICATION { get; set; }
        public virtual DbSet<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }
        public virtual DbSet<TB_CASP_SPECIAL_REQUEST> TB_CASP_SPECIAL_REQUEST { get; set; }
        public virtual DbSet<TB_CCMF_APPLICATION> TB_CCMF_APPLICATION { get; set; }
        public virtual DbSet<TB_COMPANY_ADMIN> TB_COMPANY_ADMIN { get; set; }
        public virtual DbSet<TB_COMPANY_APPLICATION_MAP> TB_COMPANY_APPLICATION_MAP { get; set; }
        public virtual DbSet<TB_COMPANY_AWARD> TB_COMPANY_AWARD { get; set; }
        public virtual DbSet<TB_COMPANY_CONTACT> TB_COMPANY_CONTACT { get; set; }
        public virtual DbSet<TB_COMPANY_FUND> TB_COMPANY_FUND { get; set; }
        public virtual DbSet<TB_COMPANY_IP> TB_COMPANY_IP { get; set; }
        public virtual DbSet<TB_COMPANY_MEMBER> TB_COMPANY_MEMBER { get; set; }
        public virtual DbSet<TB_COMPANY_MEMBER_HISTORY> TB_COMPANY_MEMBER_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_MERGE_ACQUISITION> TB_COMPANY_MERGE_ACQUISITION { get; set; }
        public virtual DbSet<TB_COMPANY_PROFILE_BASIC> TB_COMPANY_PROFILE_BASIC { get; set; }
        //public virtual DbSet<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT> TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT { get; set; }
        //public virtual DbSet<TB_CPIP_SPECIAL_REQUEST> TB_CPIP_SPECIAL_REQUEST { get; set; }
        public virtual DbSet<TB_Download_ZIP> TB_Download_ZIP { get; set; }
        public virtual DbSet<TB_EC_RESULT> TB_EC_RESULT { get; set; }
        public virtual DbSet<TB_EMAIL_TEMPLATE> TB_EMAIL_TEMPLATE { get; set; }
        public virtual DbSet<TB_FA_REIMBURSEMENT_ITEM> TB_FA_REIMBURSEMENT_ITEM { get; set; }
        public virtual DbSet<TB_FA_REIMBURSEMENT_SALARY> TB_FA_REIMBURSEMENT_SALARY { get; set; }
        public virtual DbSet<TB_INCUBATION_APPLICATION> TB_INCUBATION_APPLICATION { get; set; }
        public virtual DbSet<TB_MASTER_DATA> TB_MASTER_DATA { get; set; }
        public virtual DbSet<TB_PRESENTATION_APPLICATION_REMARKS> TB_PRESENTATION_APPLICATION_REMARKS { get; set; }
        public virtual DbSet<TB_PRESENTATION_CCMF_SCORE> TB_PRESENTATION_CCMF_SCORE { get; set; }
        public virtual DbSet<TB_PRESENTATION_INCUBATION_SCORE> TB_PRESENTATION_INCUBATION_SCORE { get; set; }
        public virtual DbSet<TB_PROGRAMME_INTAKE> TB_PROGRAMME_INTAKE { get; set; }
        public virtual DbSet<TB_SCREENING_CCMF_SCORE> TB_SCREENING_CCMF_SCORE { get; set; }
        public virtual DbSet<TB_SCREENING_HISTORY> TB_SCREENING_HISTORY { get; set; }
        public virtual DbSet<TB_SCREENING_INCUBATION_SCORE> TB_SCREENING_INCUBATION_SCORE { get; set; }
        public virtual DbSet<TB_SYSTEM_PARAMETER> TB_SYSTEM_PARAMETER { get; set; }
        public virtual DbSet<TB_VETTING_APPLICATION> TB_VETTING_APPLICATION { get; set; }
        public virtual DbSet<TB_VETTING_DECISION> TB_VETTING_DECISION { get; set; }
        public virtual DbSet<TB_VETTING_DECLARATION> TB_VETTING_DECLARATION { get; set; }
        public virtual DbSet<TB_VETTING_MEETING> TB_VETTING_MEETING { get; set; }
        public virtual DbSet<TB_VETTING_MEMBER_INFO> TB_VETTING_MEMBER_INFO { get; set; }
        //public virtual DbSet<TB_BANK_INFO_HISTORY> TB_BANK_INFO_HISTORY { get; set; }
        public virtual DbSet<TB_CASP_APPLICATION_HISTORY> TB_CASP_APPLICATION_HISTORY { get; set; }
        public virtual DbSet<TB_CASP_FA_REIMBURSEMENT_HISTORY> TB_CASP_FA_REIMBURSEMENT_HISTORY { get; set; }
        public virtual DbSet<TB_CASP_SPECIAL_REQUEST_HISTORY> TB_CASP_SPECIAL_REQUEST_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_AWARD_HISTORY> TB_COMPANY_AWARD_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_CONTACT_HISTORY> TB_COMPANY_CONTACT_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_FUND_HISTORY> TB_COMPANY_FUND_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_IP_HISTORY> TB_COMPANY_IP_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_MERGE_ACQUISITION_HISTORY> TB_COMPANY_MERGE_ACQUISITION_HISTORY { get; set; }
        public virtual DbSet<TB_COMPANY_PROFILE_BASIC_HISTORY> TB_COMPANY_PROFILE_BASIC_HISTORY { get; set; }
        //public virtual DbSet<TB_CPIP_FA_REIMBURSEMENT_HISTORY> TB_CPIP_FA_REIMBURSEMENT_HISTORY { get; set; }
        //public virtual DbSet<TB_CPIP_SPECIAL_REQUEST_HISTORY> TB_CPIP_SPECIAL_REQUEST_HISTORY { get; set; }
        public virtual DbSet<TB_DECLARATION_APPLICATION> TB_DECLARATION_APPLICATION { get; set; }
        public virtual DbSet<TB_FA_REIMBURSEMENT_ITEM_HISTORY> TB_FA_REIMBURSEMENT_ITEM_HISTORY { get; set; }
        public virtual DbSet<TB_FA_REIMBURSEMENT_SALARY_HISTORY> TB_FA_REIMBURSEMENT_SALARY_HISTORY { get; set; }
        public virtual DbSet<TB_PAST_APPLICATION> TB_PAST_APPLICATION { get; set; }
        public virtual DbSet<TB_RESET_PASSWORD> TB_RESET_PASSWORD { get; set; }
        public virtual DbSet<TB_VETTING_MEMBER> TB_VETTING_MEMBER { get; set; }
        public virtual DbSet<TB_Company_Joined_Accelerator> TB_Company_Joined_Accelerator { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Membership)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Paths)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Roles)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Users)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Paths>()
                .HasOptional(e => e.aspnet_PersonalizationAllUsers)
                .WithRequired(e => e.aspnet_Paths);

            modelBuilder.Entity<aspnet_Roles>()
                .HasMany(e => e.aspnet_Users)
                .WithMany(e => e.aspnet_Roles)
                .Map(m => m.ToTable("aspnet_UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<aspnet_Users>()
                .HasOptional(e => e.aspnet_Membership)
                .WithRequired(e => e.aspnet_Users);

            modelBuilder.Entity<aspnet_Users>()
                .HasOptional(e => e.aspnet_Profile)
                .WithRequired(e => e.aspnet_Users);

            modelBuilder.Entity<aspnet_WebEvent_Events>()
                .Property(e => e.EventId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<aspnet_WebEvent_Events>()
                .Property(e => e.EventSequence)
                .HasPrecision(19, 0);

            modelBuilder.Entity<aspnet_WebEvent_Events>()
                .Property(e => e.EventOccurrence)
                .HasPrecision(19, 0);

            modelBuilder.Entity<ContactForm>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ContactForm>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ContactForm>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<ContactForm>()
                .Property(e => e.Inquiry)
                .IsUnicode(false);

            modelBuilder.Entity<ContactForm>()
                .HasMany(e => e.ContactFormRefProblemTags)
                .WithRequired(e => e.ContactForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContactFormProblemTag>()
                .HasMany(e => e.ContactFormRefProblemTags)
                .WithRequired(e => e.ContactFormProblemTag)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_APPLICATION_FUNDING_STATUS>()
                .Property(e => e.Amount_Received)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_APPLICATION_FUNDING_STATUS>()
                .Property(e => e.Maximum_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_CASP_APPLICATION>()
                .HasMany(e => e.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
                .WithOptional(e => e.TB_CASP_APPLICATION)
                .HasForeignKey(e => e.CASP_Attended);

            modelBuilder.Entity<TB_CASP_APPLICATION>()
                .HasMany(e => e.TB_CASP_SPECIAL_REQUEST)
                .WithRequired(e => e.TB_CASP_APPLICATION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>()
                .Property(e => e.Total_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_CASP_SPECIAL_REQUEST>()
                .Property(e => e.Estimate_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_CASP_SPECIAL_REQUEST>()
                .HasMany(e => e.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
                .WithOptional(e => e.TB_CASP_SPECIAL_REQUEST)
                .HasForeignKey(e => e.Preapproved_SpecialRequest);

            modelBuilder.Entity<TB_COMPANY_FUND>()
                .Property(e => e.Amount_Received)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_FUND>()
                .Property(e => e.Maximum_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_MERGE_ACQUISITION>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_MERGE_ACQUISITION>()
                .Property(e => e.Valuation)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
                .WithOptional(e => e.TB_COMPANY_PROFILE_BASIC)
                .HasForeignKey(e => e.Company_ID);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_CASP_SPECIAL_REQUEST)
                .WithOptional(e => e.TB_COMPANY_PROFILE_BASIC)
                .HasForeignKey(e => e.Company_ID);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_ADMIN)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_APPLICATION_MAP)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_AWARD)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_CONTACT)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_FUND)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_IP)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_MEMBER)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
                .HasMany(e => e.TB_COMPANY_MERGE_ACQUISITION)
                .WithRequired(e => e.TB_COMPANY_PROFILE_BASIC)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TB_COMPANY_PROFILE_BASIC>()
            //    .HasMany(e => e.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
            //    .WithOptional(e => e.TB_COMPANY_PROFILE_BASIC)
            //    .HasForeignKey(e => e.Company_ID);

            //modelBuilder.Entity<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>()
            //    .Property(e => e.Total_Fee)
            //    .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>()
            //    .Property(e => e.Total_Amount)
            //    .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>()
            //    .HasMany(e => e.TB_BANK_INFO_FOR_DIRECT_TRANSFER)
            //    .WithRequired(e => e.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>()
            //    .HasMany(e => e.TB_FA_REIMBURSEMENT_ITEM)
            //    .WithRequired(e => e.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
            //    .HasForeignKey(e => e.FA_Application_ID)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT>()
            //    .HasMany(e => e.TB_FA_REIMBURSEMENT_SALARY)
            //    .WithRequired(e => e.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
            //    .HasForeignKey(e => e.FA_Application_ID)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TB_CPIP_SPECIAL_REQUEST>()
            //    .Property(e => e.Fee)
            //    .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_SPECIAL_REQUEST>()
            //    .Property(e => e.Estimate_Amount)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<TB_CPIP_SPECIAL_REQUEST>()
            //    .HasMany(e => e.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
            //    .WithOptional(e => e.TB_CPIP_SPECIAL_REQUEST)
            //    .HasForeignKey(e => e.Special_Request_ID);

            modelBuilder.Entity<TB_EC_RESULT>()
                .Property(e => e.Cluster)
                .IsFixedLength();

            modelBuilder.Entity<TB_EC_RESULT>()
                .Property(e => e.Company_Program)
                .IsFixedLength();

            modelBuilder.Entity<TB_EC_RESULT>()
                .Property(e => e.Programme_Type)
                .IsFixedLength();

            modelBuilder.Entity<TB_EC_RESULT>()
                .Property(e => e.Confirm_By)
                .IsUnicode(false);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_ITEM>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY>()
                .Property(e => e.Monthly_Salary)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY>()
                .Property(e => e.MPF)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY>()
                .Property(e => e.Tax)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_INCUBATION_APPLICATION>()
                .Property(e => e.Company_Type)
                .IsFixedLength();

            //modelBuilder.Entity<TB_INCUBATION_APPLICATION>()
            //    .HasMany(e => e.TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT)
            //    .WithRequired(e => e.TB_INCUBATION_APPLICATION)
            //    .HasForeignKey(e => e.CPIP_ID)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_PRESENTATION_CCMF_SCORE>()
                .Property(e => e.Management_Team)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_CCMF_SCORE>()
                .Property(e => e.Business_Model)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_CCMF_SCORE>()
                .Property(e => e.Creativity)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_CCMF_SCORE>()
                .Property(e => e.Social_Responsibility)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_CCMF_SCORE>()
                .Property(e => e.Total_Score)
                .HasPrecision(3, 2);

            modelBuilder.Entity<TB_PRESENTATION_INCUBATION_SCORE>()
                .Property(e => e.Management_Team)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_INCUBATION_SCORE>()
                .Property(e => e.Creativity)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_INCUBATION_SCORE>()
                .Property(e => e.Business_Viability)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_INCUBATION_SCORE>()
                .Property(e => e.Benefit_To_Industry)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_INCUBATION_SCORE>()
                .Property(e => e.Proposal_Milestones)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_PRESENTATION_INCUBATION_SCORE>()
                .Property(e => e.Total_Score)
                .HasPrecision(3, 2);

            modelBuilder.Entity<TB_PROGRAMME_INTAKE>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<TB_SCREENING_CCMF_SCORE>()
                .Property(e => e.Management_Team)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_CCMF_SCORE>()
                .Property(e => e.Business_Model)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_CCMF_SCORE>()
                .Property(e => e.Creativity)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_CCMF_SCORE>()
                .Property(e => e.Social_Responsibility)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_CCMF_SCORE>()
                .Property(e => e.Total_Score)
                .HasPrecision(3, 2);

            modelBuilder.Entity<TB_SCREENING_INCUBATION_SCORE>()
                .Property(e => e.Management_Team)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_INCUBATION_SCORE>()
                .Property(e => e.Creativity)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_INCUBATION_SCORE>()
                .Property(e => e.Business_Viability)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_INCUBATION_SCORE>()
                .Property(e => e.Benefit_To_Industry)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_INCUBATION_SCORE>()
                .Property(e => e.Proposal_Milestones)
                .HasPrecision(2, 2);

            modelBuilder.Entity<TB_SCREENING_INCUBATION_SCORE>()
                .Property(e => e.Total_Score)
                .HasPrecision(3, 2);

            //modelBuilder.Entity<TB_VETTING_DECLARATION>()
            //    .HasMany(e => e.TB_DECLARATION_APPLICATION)
            //    .WithRequired(e => e.TB_VETTING_DECLARATION)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TB_VETTING_MEETING>()
            //    .HasMany(e => e.TB_VETTING_MEMBER)
            //    .WithRequired(e => e.TB_VETTING_MEETING)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_CASP_FA_REIMBURSEMENT_HISTORY>()
                .Property(e => e.Total_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_CASP_SPECIAL_REQUEST_HISTORY>()
                .Property(e => e.Estimate_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_FUND_HISTORY>()
                .Property(e => e.Amount_Received)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_FUND_HISTORY>()
                .Property(e => e.Maximum_Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_MERGE_ACQUISITION_HISTORY>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_COMPANY_MERGE_ACQUISITION_HISTORY>()
                .Property(e => e.Valuation)
                .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_FA_REIMBURSEMENT_HISTORY>()
            //    .Property(e => e.Total_Fee)
            //    .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_FA_REIMBURSEMENT_HISTORY>()
            //    .Property(e => e.Total_Amount)
            //    .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_SPECIAL_REQUEST_HISTORY>()
            //    .Property(e => e.Fee)
            //    .HasPrecision(15, 2);

            //modelBuilder.Entity<TB_CPIP_SPECIAL_REQUEST_HISTORY>()
            //    .Property(e => e.Estimate_Amount)
            //    .HasPrecision(9, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_ITEM_HISTORY>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY_HISTORY>()
                .Property(e => e.Monthly_Salary)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY_HISTORY>()
                .Property(e => e.MPF)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY_HISTORY>()
                .Property(e => e.Tax)
                .HasPrecision(15, 2);

            modelBuilder.Entity<TB_FA_REIMBURSEMENT_SALARY_HISTORY>()
                .Property(e => e.Amount)
                .HasPrecision(15, 2);
        }
    }
}
