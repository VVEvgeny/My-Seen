namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "Year", c => c.Int(nullable: false, defaultValue:0));
            AddColumn("dbo.Serials", "Year", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Serials", "Year");
            DropColumn("dbo.Films", "Year");
        }
    }
}
