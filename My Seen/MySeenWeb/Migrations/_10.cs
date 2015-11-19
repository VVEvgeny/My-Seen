using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tracks",
                c => new
                {
                    Id = c.Int(false, true),
                    UserId = c.String(maxLength: 128),
                    Name = c.String(),
                    Type = c.Int(false),
                    Date = c.DateTime(false),
                    Coordinates = c.String(),
                    Distance = c.Int(false),
                    Discriminator = c.String(false, 128)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Tracks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tracks", new[] {"UserId"});
            DropTable("dbo.Tracks");
        }
    }
}