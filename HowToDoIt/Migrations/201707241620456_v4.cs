namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Instructions", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Instructions", new[] { "CategoryId" });
            DropPrimaryKey("dbo.Categories");
            DropPrimaryKey("dbo.Instructions");
            AlterColumn("dbo.Categories", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Instructions", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Instructions", "CategoryId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Categories", "Id");
            AddPrimaryKey("dbo.Instructions", "Id");
            CreateIndex("dbo.Instructions", "CategoryId");
            AddForeignKey("dbo.Instructions", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Instructions", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Instructions", new[] { "CategoryId" });
            DropPrimaryKey("dbo.Instructions");
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Instructions", "CategoryId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Instructions", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Categories", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Instructions", "Id");
            AddPrimaryKey("dbo.Categories", "Id");
            CreateIndex("dbo.Instructions", "CategoryId");
            AddForeignKey("dbo.Instructions", "CategoryId", "dbo.Categories", "Id");
        }
    }
}
