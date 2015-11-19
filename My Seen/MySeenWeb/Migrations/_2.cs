using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "UserName", c => c.String());
            AddColumn("dbo.Bugs", "Discriminator", c => c.String(false, 128));
        }

        public override void Down()
        {
            DropColumn("dbo.Bugs", "Discriminator");
            DropColumn("dbo.Bugs", "UserName");
        }
    }
}