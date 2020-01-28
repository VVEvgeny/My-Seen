namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class index : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Bots", "Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Bots", new[] { "Name" });
        }
    }
}
