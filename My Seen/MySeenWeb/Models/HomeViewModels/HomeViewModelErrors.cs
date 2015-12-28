using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelErrors
    {
        public IEnumerable<NLogErrorsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelErrors(int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.NLogErrors.Count(), countInPage);
            Data = ac.NLogErrors.Select(NLogErrorsView.Map).OrderByDescending(l => l.DateTimeStamp).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
