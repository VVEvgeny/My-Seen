using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.TablesLogic
{
    public class UserRolesLogic : IdentityUserRole
    {
        private readonly ICacheService _cache;
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public UserRolesLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        public UserRolesLogic(ICacheService cache)
        {
            _cache = cache;
        }

        private static IEnumerable<IdentityUserRole> GetAllRoles()
        {
            return new ApplicationDbContext().UserRoles;
        }
        private static IEnumerable<IdentityUserRole> GetAllRoles(ICacheService cache)
        {
            var roles = cache.Get<List<IdentityUserRole>>(CacheNames.UserRoles);
            if (roles == null)
            {
                roles = new ApplicationDbContext().UserRoles.ToList();
                cache.Set(CacheNames.UserRoles, roles, 15);
            }
            return roles;
        }
        public static bool IsAdmin(string userId, ICacheService cache)
        {
            return GetAllRoles(cache).Any(r => r.RoleId == ((int) RolesBase.Indexes.Admin).ToString() && r.UserId == userId);
        }
        public static bool IsAdmin(string userId)
        {
            return GetAllRoles().Any(r => r.RoleId == ((int)RolesBase.Indexes.Admin).ToString() && r.UserId == userId);
        }

        public static bool IsTester(string userId, ICacheService cache)
        {
            return GetAllRoles(cache).Any(r => r.RoleId == ((int)RolesBase.Indexes.Tester).ToString() && r.UserId == userId);
        }

        public IEnumerable<string> GetRoles(string userId)
        {
            try
            {
                if (GetAllRoles(_cache).Any(r => r.UserId == userId))
                {
                    return GetAllRoles(_cache).Where(r => r.UserId == userId).Select(r => r.RoleId);
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
                foreach (var role in RolesTypes.GetAll())
                {
                    var idRole = RolesTypes.GetId(role).ToString();
                    if (roles != null && roles.Any() && roles.Contains(idRole))
                        // если есть роль в выбраных, проверим, если в БД, если есть ничего не делаем, если нет, добавим
                    {
                        if (!GetAllRoles(_cache).Any(r => r.UserId == UserId && r.RoleId == idRole))
                        {
                            _ac.UserRoles.Add(new IdentityUserRole {RoleId = idRole, UserId = UserId});
                            _cache.Remove(CacheNames.UserRoles.ToString());
                        }
                    }
                    else //если нет в выборке, возможно снимаем, проверим может надо по БД снять
                    {
                        if (GetAllRoles(_cache).Any(r => r.UserId == UserId && r.RoleId == idRole))
                        {
                            if (idRole == ((int) RolesBase.Indexes.Admin).ToString())
                            {
                                if (GetAllRoles(_cache).Count(r => r.RoleId == idRole) < 2)
                                {
                                    throw new Exception(Resource.CantDeleteLastAdmin);
                                }
                            }
                            _ac.UserRoles.Remove(_ac.UserRoles.First(r => r.UserId == UserId && r.RoleId == idRole));
                            _cache.Remove(CacheNames.UserRoles.ToString());
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