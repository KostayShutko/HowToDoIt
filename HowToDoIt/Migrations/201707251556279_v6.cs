namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Steps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        InstructionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instructions", t => t.InstructionId, cascadeDelete: true)
                .Index(t => t.InstructionId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        StepId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Steps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId);
            
            CreateTable(
                "dbo.Texts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        StepId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Steps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Name = c.String(),
                        StepId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Steps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "StepId", "dbo.Steps");
            DropForeignKey("dbo.Texts", "StepId", "dbo.Steps");
            DropForeignKey("dbo.Images", "StepId", "dbo.Steps");
            DropForeignKey("dbo.Steps", "InstructionId", "dbo.Instructions");
            DropIndex("dbo.Videos", new[] { "StepId" });
            DropIndex("dbo.Texts", new[] { "StepId" });
            DropIndex("dbo.Images", new[] { "StepId" });
            DropIndex("dbo.Steps", new[] { "InstructionId" });
            DropTable("dbo.Videos");
            DropTable("dbo.Texts");
            DropTable("dbo.Images");
            DropTable("dbo.Steps");
        }
    }
}
