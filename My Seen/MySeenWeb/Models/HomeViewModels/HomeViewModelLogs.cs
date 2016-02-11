using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelLogs
    {
        public IEnumerable<LogsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelLogs(int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Logs.Count(f => (string.IsNullOrEmpty(search) || f.UserAgent.Contains(search))), countInPage);
            Data =
                ac.Logs.AsNoTracking()
                    .Where(f => (string.IsNullOrEmpty(search) || f.UserAgent.Contains(search)))
                    .OrderByDescending(l => l.DateLast)
                    .Skip(() => Pages.SkipRecords)
                    .Take(() => countInPage)
                    .Select(LogsView.Map);
        }
    }
}
