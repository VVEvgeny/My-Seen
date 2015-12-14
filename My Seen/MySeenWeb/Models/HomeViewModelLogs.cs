using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelLogs
    {
        public IEnumerable<LogsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelLogs(int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Logs.Count(), countInPage, "Home", "");
            Data = ac.Logs.Select(LogsView.Map).OrderByDescending(l => l.DateLast).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
