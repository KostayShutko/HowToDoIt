namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Avatar = c.String(),
                        FirtstName = c.String(),
                        LastName = c.String(),
                        Sex = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        City = c.String(),
                        Contacts = c.String(),
                        Interests = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profiles", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropTable("dbo.Profiles");
        }
    }
}
