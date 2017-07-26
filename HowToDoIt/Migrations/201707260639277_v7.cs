namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v7 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Images", newName: "Blocks");
            DropIndex("dbo.Texts", new[] { "StepId" });
            DropIndex("dbo.Videos", new[] { "StepId" });
            AddColumn("dbo.Steps", "Name", c => c.String());
            DropTable("dbo.Texts");
            DropTable("dbo.Videos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        StepId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Texts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        StepId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Steps", "Name");
            CreateIndex("dbo.Videos", "StepId");
            CreateIndex("dbo.Texts", "StepId");
            RenameTable(name: "dbo.Blocks", newName: "Images");
        }
    }
}
