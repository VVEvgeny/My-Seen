using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RecordPerPage", c => c.Int(false));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RecordPerPage");
        }
    }
}