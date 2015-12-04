using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string UniqueKey { get; set; }
        public string Culture { get; set; }
        public DateTime RegisterDate { get; set; }
        public int RecordPerPage { get; set; }
        public int MarkersOnRoads { get; set; }
        public string ShareTracksAllKey { get; set; }
        public string ShareTracksFootKey { get; set; }
        public string ShareTracksCarKey { get; set; }
        public string ShareTracksBikeKey { get; set; }
        public string ShareFilmsKey { get; set; }
        public string ShareSerialsKey { get; set; }
        public string ShareBooksKey { get; set; }
        public string ShareEventsKey { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>("DefaultConnection"));
        }

        public DbSet<Films> Films { get; set; }
        public DbSet<Serials> Serials { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Bugs> Bugs { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Tracks> Tracks { get; set; }
        public DbSet<Events> Events { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}