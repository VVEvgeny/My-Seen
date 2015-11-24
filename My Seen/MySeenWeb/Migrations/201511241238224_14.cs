namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ShareTracksAllKey", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareTracksFootKey", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareTracksCarKey", c => c.String());
            DropColumn("dbo.AspNetUsers", "ShareTracksKey");
            DropColumn("dbo.AspNetUsers", "ShareTracksFoot");
            DropColumn("dbo.AspNetUsers", "ShareTracksCar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ShareTracksCar", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareTracksFoot", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareTracksKey", c => c.String());
            DropColumn("dbo.AspNetUsers", "ShareTracksCarKey");
            DropColumn("dbo.AspNetUsers", "ShareTracksFootKey");
            DropColumn("dbo.AspNetUsers", "ShareTracksAllKey");
        }
    }
}
