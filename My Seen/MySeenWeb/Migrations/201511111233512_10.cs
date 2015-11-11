namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Coordinates = c.String(),
                        Distance = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tracks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tracks", new[] { "UserId" });
            DropTable("dbo.Tracks");
        }
    }
}
