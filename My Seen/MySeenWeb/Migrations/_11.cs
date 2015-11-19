using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tracks", "Distance", c => c.Double(false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Tracks", "Distance", c => c.Int(false));
        }
    }
}