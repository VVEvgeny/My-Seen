using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "Version", c => c.Int(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bugs", "Version");
        }
    }
}