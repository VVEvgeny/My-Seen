namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _44 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Theme", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "VkServiceEnabled");
            DropColumn("dbo.AspNetUsers", "GoogleServiceEnabled");
            DropColumn("dbo.AspNetUsers", "FacebookServiceEnabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FacebookServiceEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "GoogleServiceEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "VkServiceEnabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "Theme");
        }
    }
}
