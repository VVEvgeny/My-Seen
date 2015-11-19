using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.Database;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.HomeModels.Ext
{
    public class LogsViewModel
    {
        public LogsViewModel(int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Logs.Count(), countInPage, "Home", "");
            Logs =
                ac.Logs.Select(LogsView.Map)
                    .OrderByDescending(l => l.DateLast)
                    .Skip((Pages.CurentPage - 1)*countInPage)
                    .Take(countInPage);
        }

        public IEnumerable<LogsView> Logs { get; set; }
        public PaginationViewModel Pages { get; set; }
    }
}