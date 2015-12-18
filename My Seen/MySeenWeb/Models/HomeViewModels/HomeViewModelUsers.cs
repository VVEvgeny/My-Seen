using System.Collections.Generic;
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
            Data = ac.Users.Select(UsersView.Map).OrderBy(l => l.RegisterDate).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
