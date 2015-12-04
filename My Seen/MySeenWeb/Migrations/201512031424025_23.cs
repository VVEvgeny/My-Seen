namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Shared", c => c.Boolean(nullable: false));
            AddColumn("dbo.Films", "Shared", c => c.Boolean(nullable: false));
            AddColumn("dbo.Serials", "Shared", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Serials", "Shared");
            DropColumn("dbo.Films", "Shared");
            DropColumn("dbo.Books", "Shared");
        }
    }
}
