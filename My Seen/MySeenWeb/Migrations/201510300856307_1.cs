namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        UserAgent = c.String(),
                        IPAdress = c.String(),
                        OnlyDate = c.String(),
                        DateFirst = c.DateTime(nullable: false),
                        DateLast = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        PageName = c.String(),
                        AddData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
