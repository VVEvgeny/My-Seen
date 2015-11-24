namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ShareTracksFoot", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShareTracksCar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ShareTracksCar");
            DropColumn("dbo.AspNetUsers", "ShareTracksFoot");
        }
    }
}
