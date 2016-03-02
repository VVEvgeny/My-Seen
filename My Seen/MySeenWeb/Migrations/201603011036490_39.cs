namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _39 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Logs", "PageName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "PageName", c => c.String());
        }
    }
}
