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
        public PaginationViewModel Pages { get; set; }

        public bool HaveData
        {
            get { return Data.Any(); }
        }

        public HomeViewModelUsers(int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Users.Count(f=> (string.IsNullOrEmpty(search) || f.UserName.Contains(search))), countInPage);
            Data = ac.Users.AsNoTracking()
                .Where(f => (string.IsNullOrEmpty(search) || f.UserName.Contains(search)))
                .Select(UsersView.Map)
                .OrderByDescending(l => l.LastAction)
                .Skip(Pages.SkipRecords)
                .Take(countInPage);
        }
    }
}
