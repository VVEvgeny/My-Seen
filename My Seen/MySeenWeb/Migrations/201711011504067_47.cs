namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _47 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Deals");
        }
    }
}
