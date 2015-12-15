namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastAction", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastAction");
        }
    }
}
