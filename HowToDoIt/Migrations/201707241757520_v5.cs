namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagInstructions",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Instruction_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Instruction_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Instructions", t => t.Instruction_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Instruction_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagInstructions", "Instruction_Id", "dbo.Instructions");
            DropForeignKey("dbo.TagInstructions", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagInstructions", new[] { "Instruction_Id" });
            DropIndex("dbo.TagInstructions", new[] { "Tag_Id" });
            DropTable("dbo.TagInstructions");
            DropTable("dbo.Tags");
        }
    }
}
