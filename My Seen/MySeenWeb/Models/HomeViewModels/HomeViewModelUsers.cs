using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelUsers
    {
        public IEnumerable<UsersView> Data;
        public Pagination Pages { get; set; }

        public HomeViewModelUsers(int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();
            Pages = new Pagination(page, ac.Users.Count(f=> (string.IsNullOrEmpty(search) || f.UserName.Contains(search))), countInPage);
            Data = ac.Users.AsNoTracking()
                .Where(f => (string.IsNullOrEmpty(search) || f.UserName.Contains(search)))
                .Skip(() => Pages.SkipRecords)
                .Take(() => countInPage)
                .Select(UsersView.Map)
                .OrderByDescending(l => l.LastAction);
        }
    }
}
