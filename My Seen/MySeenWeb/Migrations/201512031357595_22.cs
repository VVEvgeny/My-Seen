namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ShareFilmsKey", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareSerialsKey", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareBooksKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ShareBooksKey");
            DropColumn("dbo.AspNetUsers", "ShareSerialsKey");
            DropColumn("dbo.AspNetUsers", "ShareFilmsKey");
        }
    }
}
