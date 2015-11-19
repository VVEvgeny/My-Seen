using System.Data.Entity.Migrations;

namespace MySeenWeb.Migrations
{
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                {
                    Id = c.Int(false, true),
                    UserId = c.String(maxLength: 128),
                    Name = c.String(),
                    Authors = c.String(),
                    DateRead = c.DateTime(false),
                    Genre = c.Int(false),
                    Rating = c.Int(false),
                    DateChange = c.DateTime(false),
                    isDeleted = c.Boolean(),
                    Discriminator = c.String(false, 128)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Books", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Books", new[] {"UserId"});
            DropTable("dbo.Books");
        }
    }
}