namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "VkServiceEnabled", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.AspNetUsers", "GoogleServiceEnabled", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.AspNetUsers", "FacebookServiceEnabled", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FacebookServiceEnabled");
            DropColumn("dbo.AspNetUsers", "GoogleServiceEnabled");
            DropColumn("dbo.AspNetUsers", "VkServiceEnabled");
        }
    }
}
