namespace CBP_EMS_SP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddContactForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactForm",
                c => new
                {
                    ContactFormId = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 255, unicode: false),
                    Email = c.String(maxLength: 255, unicode: false),
                    Phone = c.String(maxLength: 255, unicode: false),
                    Inquiry = c.String(unicode: false, storeType: "text"),
                })
                .PrimaryKey(t => t.ContactFormId);

        }

        public override void Down()
        {
            DropTable("dbo.ContactForm");
        }
    }
}
