namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Memes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Image = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Memes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Memes", new[] { "UserId" });
            DropTable("dbo.Memes");
        }
    }
}
