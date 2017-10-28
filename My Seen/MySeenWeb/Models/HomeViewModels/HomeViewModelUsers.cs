using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models
{
    public class HomeViewModelUsers
    {
        public IEnumerable<UsersView> Data { get; set; }

        public IEnumerable<SelectListItem> UserRoles { get; } =
            RolesTypes.GetAll()
                .Select(
                    sel =>
                        new SelectListItem
                        {
                            Text = sel,
                            Value = RolesTypes.GetId(sel).ToString()
                        })
                .ToList();

        public Pagination Pages { get; set; }
        public bool CanControl { get; set; }

        public HomeViewModelUsers(string userId, int page, int countInPage, string search, ICacheService cache)
        {
            var ac = new ApplicationDbContext();
            Pages = new Pagination(page,
                ac.Users.Count(f => string.IsNullOrEmpty(search) || f.UserName.Contains(search)), countInPage);
            Data = ac.Users.AsNoTracking()
                .Where(f => string.IsNullOrEmpty(search) || f.UserName.Contains(search))
                .OrderByDescending(l => l.RegisterDate)
                .Skip(() => Pages.SkipRecords)
                .Take(() => countInPage)
                .Select(UsersView.Map)
                .OrderByDescending(l => l.LastAction);

            CanControl = UserRolesLogic.IsAdmin(userId, cache);
        }
    }
}