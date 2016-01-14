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

        public HomeViewModelUsers(int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Users.Count(), countInPage);
            Data = ac.Users.AsNoTracking().Select(UsersView.Map).OrderByDescending(l => l.LastAction).Skip(Pages.SkipRecords).Take(countInPage);
        }
    }
}
