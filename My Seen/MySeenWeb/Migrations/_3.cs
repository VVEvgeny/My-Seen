using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "Complex", c => c.Int(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Bugs", "Complex");
        }
    }
}