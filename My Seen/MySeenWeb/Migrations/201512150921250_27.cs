namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "LastLogin");
            DropColumn("dbo.AspNetUsers", "LastAction");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LastAction", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime());
        }
    }
}
