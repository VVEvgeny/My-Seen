using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _38 : DbMigration
    {
        public override void Up()
        {
            var _ac = new ApplicationDbContext();

            var userIdAdmn = _ac.Users.First(u => u.Email == "vvevgeny@gmail.com").Id;
            foreach (var role in Defaults.RolesTypes.GetAll())
            {
                var idRole = Defaults.RolesTypes.GetId(role).ToString();
                _ac.Roles.Add(new IdentityRole {Id = idRole, Name = role});
                _ac.UserRoles.Add(new IdentityUserRole {UserId = userIdAdmn, RoleId = idRole});
            }


            _ac.SaveChanges();
        }
        
        public override void Down()
        {
        }
    }
}
