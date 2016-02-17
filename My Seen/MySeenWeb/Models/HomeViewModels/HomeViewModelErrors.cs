using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelErrors
    {
        public IEnumerable<NLogErrorsView> Data { get; set; }
        public Pagination Pages { get; set; }

        public HomeViewModelErrors(int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();
            Pages = new Pagination(page, ac.NLogErrors.Count(f => (string.IsNullOrEmpty(search) || f.Message.Contains(search))), countInPage);
            Data =
                ac.NLogErrors.AsNoTracking()
                    .Where(f => (string.IsNullOrEmpty(search) || f.Message.Contains(search)))
                    .OrderByDescending(l => l.DateTimeStamp)
                    .Skip(() => Pages.SkipRecords)
                    .Take(() => countInPage)
                    .Select(NLogErrorsView.Map);
        }
    }
}
