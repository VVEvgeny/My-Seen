using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Discriminator", c => c.String(false, 128));
            AddColumn("dbo.Logs", "UserName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Logs", "Discriminator");
            DropColumn("dbo.Logs", "UserName");
        }
    }
}