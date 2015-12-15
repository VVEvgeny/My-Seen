namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _29 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCredits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DateTo = c.DateTime(nullable: false),
                        PrivateKey = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCredits", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserCredits", new[] { "UserId" });
            DropTable("dbo.UserCredits");
        }
    }
}
