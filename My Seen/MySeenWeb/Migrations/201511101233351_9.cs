namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Authors = c.String(),
                        DateRead = c.DateTime(nullable: false),
                        Genre = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        DateChange = c.DateTime(nullable: false),
                        isDeleted = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Books", new[] { "UserId" });
            DropTable("dbo.Books");
        }
    }
}
