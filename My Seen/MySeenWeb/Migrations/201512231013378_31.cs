using MySeenLib;

namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NLogErrors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTimeStamp = c.DateTime(nullable: false,defaultValue:UmtTime.To(DateTime.Now)),
                        Host = c.String(),
                        Type = c.String(),
                        Message = c.String(),
                        Level = c.String(),
                        StackTrace = c.String(),
                        Variables = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NLogErrors");
        }
    }
}
