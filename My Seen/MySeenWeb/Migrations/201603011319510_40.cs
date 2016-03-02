using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _40 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Realts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Realts");
        }
    }
}
