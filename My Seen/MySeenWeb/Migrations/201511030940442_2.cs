namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "UserName", c => c.String());
            AddColumn("dbo.Bugs", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bugs", "Discriminator");
            DropColumn("dbo.Bugs", "UserName");
        }
    }
}
