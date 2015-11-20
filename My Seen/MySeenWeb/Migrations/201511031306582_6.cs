namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bugs", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bugs", "UserName", c => c.String());
        }
    }
}
