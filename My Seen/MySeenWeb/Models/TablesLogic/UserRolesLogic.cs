using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.TablesLogic
{
    public class UserRolesLogic : IdentityUserRole
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public UserRolesLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public static bool IsAdmin(string userId)
        {
            var ac = new ApplicationDbContext();
            return
                ac.UserRoles.Any(
                    r => r.RoleId == ((int) Defaults.RolesBase.Indexes.Admin).ToString() && r.UserId == userId);
        }

        public static bool IsTester(string userId)
        {
            var ac = new ApplicationDbContext();
            return
                ac.UserRoles.Any(
                    r => r.RoleId == ((int) Defaults.RolesBase.Indexes.Tester).ToString() && r.UserId == userId);
        }

        public IEnumerable<string> GetRoles(string userId)
        {
            try
            {
                if (_ac.UserRoles.Any(r => r.UserId == userId))
                {
                    return _ac.UserRoles.Where(r => r.UserId == userId).Select(r => r.RoleId);
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return new List<string>();
            }
            return new List<string>();
        }

        public bool Update(string userName, IEnumerable<string> roles)
        {
            try
            {
                UserId = _ac.Users.First(u => u.UserName == userName).Id;
                foreach (var role in Defaults.RolesTypes.GetAll())
                {
                    var idRole = Defaults.RolesTypes.GetId(role).ToString();
                    if (roles != null && roles.Any() && roles.Contains(idRole))
                        // если есть роль в выбраных, проверим, если в БД, если есть ничего не делаем, если нет, добавим
                    {
                        if (!_ac.UserRoles.Any(r => r.UserId == UserId && r.RoleId == idRole))
                        {
                            _ac.UserRoles.Add(new IdentityUserRole {RoleId = idRole, UserId = UserId});
                        }
                    }
                    else //если нет в выборке, возможно снимаем, проверим может надо по БД снять
                    {
                        if (_ac.UserRoles.Any(r => r.UserId == UserId && r.RoleId == idRole))
                        {
                            if (idRole == ((int) Defaults.RolesBase.Indexes.Admin).ToString())
                            {
                                if (_ac.UserRoles.Count(r => r.RoleId == idRole) < 2)
                                {
                                    throw new Exception(Resource.CantDeleteLastAdmin);
                                }
                            }
                            _ac.UserRoles.Remove(_ac.UserRoles.First(r => r.UserId == UserId && r.RoleId == idRole));
                        }
                    }
                }
                _ac.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
    }
}