namespace CBP_EMS_SP.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CyberportEMS_EDM : DbContext
    {
        public CyberportEMS_EDM()
            : base("CyberportEMSConnectionString")
        {

        }

        public virtual DbSet<ContactForm> ContactForms { get; set; }
        public virtual DbSet<ContactFormResponsibleSales> ContactFormResponsibleSales { get; set; }
        public virtual DbSet<ContactFormProblemTag> ContactFormProblemTags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CyberportEMS_EDM>(null);
            base.OnModelCreating(modelBuilder);

            // ContactForm

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

            // ContactFormResponsibleSales

            modelBuilder.Entity<ContactFormResponsibleSales>()
                .Property(e => e.Name)
                .IsUnicode(false);

            // ContactFormProblemTag

            modelBuilder.Entity<ContactFormProblemTag>()
                .Property(e => e.ProblemTagName)
                .IsUnicode(false);

            // ContactFormRefProblemTag

            modelBuilder.Entity<ContactForm>()
                .HasMany<ContactFormProblemTag>(s => s.ContactFormProblemTags)
                .WithMany(c => c.ContactForms)
                .Map(cs =>
                {
                    cs.MapLeftKey("ContactFormId");
                    cs.MapRightKey("ContactFormProblemTagId");
                    cs.ToTable("ContactFormRefProblemTag");
                });

        }
    }
}
