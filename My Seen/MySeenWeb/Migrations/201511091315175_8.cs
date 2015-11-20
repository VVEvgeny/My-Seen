namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using MySeenWeb.Models;

    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "Version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bugs", "Version");
        }
    }
}
