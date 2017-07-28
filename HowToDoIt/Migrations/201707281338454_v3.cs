namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instructions", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructions", "Image");
        }
    }
}
