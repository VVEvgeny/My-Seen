namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _34 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemesStats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        MemesId = c.Int(nullable: false),
                        Plus = c.Boolean(nullable: false),
                        Minus = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Memes", t => t.MemesId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MemesId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemesStats", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MemesStats", "MemesId", "dbo.Memes");
            DropIndex("dbo.MemesStats", new[] { "MemesId" });
            DropIndex("dbo.MemesStats", new[] { "UserId" });
            DropTable("dbo.MemesStats");
        }
    }
}
