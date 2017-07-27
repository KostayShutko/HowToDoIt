namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class n1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserLogin = c.String(),
                        Mark = c.Int(nullable: false),
                        InstructionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instructions", t => t.InstructionId, cascadeDelete: true)
                .Index(t => t.InstructionId);
            
            AddColumn("dbo.Instructions", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Instructions", "UserId");
            AddForeignKey("dbo.Instructions", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Instructions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "InstructionId", "dbo.Instructions");
            DropIndex("dbo.Ratings", new[] { "InstructionId" });
            DropIndex("dbo.Instructions", new[] { "UserId" });
            DropColumn("dbo.Instructions", "UserId");
            DropTable("dbo.Ratings");
        }
    }
}
