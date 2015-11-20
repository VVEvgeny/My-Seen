using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelLogs
    {
        public IEnumerable<LogsView> Logs { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelLogs(int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Logs.Count(), countInPage, "Home", "");
            Logs = ac.Logs.Select(LogsView.Map).OrderByDescending(l => l.DateLast).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
