using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ShareTracksKey", c => c.String());
            AddColumn("dbo.Tracks", "ShareKey", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Tracks", "ShareKey");
            DropColumn("dbo.AspNetUsers", "ShareTracksKey");
        }
    }
}