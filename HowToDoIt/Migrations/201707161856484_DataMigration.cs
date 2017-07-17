namespace HowToDoIt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.AspNetUsers", "FirtstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "Sex");
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "Contacts");
            DropColumn("dbo.AspNetUsers", "Interests");
            DropColumn("dbo.AspNetUsers", "Language");
            DropColumn("dbo.AspNetUsers", "Color");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Color", c => c.String());
            AddColumn("dbo.AspNetUsers", "Language", c => c.String());
            AddColumn("dbo.AspNetUsers", "Interests", c => c.String());
            AddColumn("dbo.AspNetUsers", "Contacts", c => c.String());
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Sex", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirtstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
        }
    }
}
