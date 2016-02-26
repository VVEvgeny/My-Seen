using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelUsers
    {
        public IEnumerable<UsersView> Data;
        public Pagination Pages { get; set; }
        public bool CanControl { get; set; }
        public IEnumerable<SelectListItem> UserRoles;

        public HomeViewModelUsers(string userId, int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();
            Pages = new Pagination(page, ac.Users.Count(f=> (string.IsNullOrEmpty(search) || f.UserName.Contains(search))), countInPage);
            Data = ac.Users.AsNoTracking()
                .Where(f => (string.IsNullOrEmpty(search) || f.UserName.Contains(search)))
                .OrderByDescending(l => l.RegisterDate)
                .Skip(() => Pages.SkipRecords)
                .Take(() => countInPage)
                .Select(UsersView.Map)
                .OrderByDescending(l => l.LastAction);

            CanControl = UserRolesLogic.IsAdmin(userId);
            
            UserRoles =
                Defaults.RolesTypes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.RolesTypes.GetId(sel).ToString()
                            })
                    .ToList();
        }
    }
}
