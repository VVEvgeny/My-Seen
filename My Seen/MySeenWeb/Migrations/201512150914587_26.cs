namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _26 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "LastAction", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastAction", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime(nullable: false));
        }
    }
}
