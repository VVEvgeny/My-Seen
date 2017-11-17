namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _48_bots_add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bots", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bots", "Discriminator");
        }
    }
}
