using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bugs",
                c => new
                {
                    Id = c.Int(false, true),
                    UserId = c.String(maxLength: 128),
                    DateFound = c.DateTime(false),
                    Text = c.String(),
                    DateEnd = c.DateTime(),
                    TextEnd = c.String()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Bugs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bugs", new[] {"UserId"});
            DropTable("dbo.Bugs");
        }
    }
}