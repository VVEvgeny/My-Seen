namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "isDeleted");
            DropColumn("dbo.Films", "isDeleted");
            DropColumn("dbo.Serials", "isDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Serials", "isDeleted", c => c.Boolean());
            AddColumn("dbo.Films", "isDeleted", c => c.Boolean());
            AddColumn("dbo.Books", "isDeleted", c => c.Boolean());
        }
    }
}
