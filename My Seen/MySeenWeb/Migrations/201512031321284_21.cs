namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ShareEventsKey", c => c.String());
            AddColumn("dbo.Events", "Shared", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Shared");
            DropColumn("dbo.AspNetUsers", "ShareEventsKey");
        }
    }
}
