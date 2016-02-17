using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelSerials
    {
        public IEnumerable<SerialsView> Data { get; set; }
        public Pagination Pages { get; set; }
        public bool IsMyData { get; set; }

        public HomeViewModelSerials(string userId, int page, int countInPage, string search, string shareKey)
        {
            var ac = new ApplicationDbContext();
            Pages = new Pagination(page, ac.Serials.Count(f =>
                ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                 ||
                 (!string.IsNullOrEmpty(shareKey) && f.User.ShareSerialsKey == shareKey && f.Shared))
                && (string.IsNullOrEmpty(search) || f.Name.Contains(search))), countInPage);

            Data = ac.Serials.AsNoTracking()
                .Where(f =>
                    ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                     ||
                     (!string.IsNullOrEmpty(shareKey) && f.User.ShareSerialsKey == shareKey && f.Shared))
                    && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                .OrderByDescending(f => f.DateLast)
                .Skip(() => Pages.SkipRecords).Take(() => countInPage).Select(SerialsView.Map);

            IsMyData = !string.IsNullOrEmpty(shareKey) && Data.Any() && Data.First().UserId == userId;
        }
    }
}
