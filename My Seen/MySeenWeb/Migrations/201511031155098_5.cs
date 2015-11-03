namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Logs", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "UserName", c => c.String());
        }
    }
}
