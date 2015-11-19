using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySeenWeb.Migrations;
using MySeenWeb.Models.Database.Tables;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
            System.Data.Entity.Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>("DefaultConnection"));
        }

        public DbSet<Films> Films { get; set; }
        public DbSet<Serials> Serials { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Bugs> Bugs { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Tracks> Tracks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}