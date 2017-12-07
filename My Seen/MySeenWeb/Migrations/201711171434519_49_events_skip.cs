namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _49_events_skip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventsSkips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventsSkips", "EventId", "dbo.Events");
            DropIndex("dbo.EventsSkips", new[] { "EventId" });
            DropTable("dbo.EventsSkips");
        }
    }
}
