namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastLogin");
        }
    }
}
