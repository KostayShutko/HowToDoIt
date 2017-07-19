namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Profiles", "DateOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
