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
        public PaginationViewModel Pages { get; set; }
        public bool HaveData
        {
            get { return Data.Any(); }
        }
        public HomeViewModelSerials(string userId, int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Serials.Count(f => f.UserId == userId && (string.IsNullOrEmpty(search) || f.Name.Contains(search))), countInPage);
            Data = ac.Serials.AsNoTracking()
                .Where(f => f.UserId == userId && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                .OrderByDescending(f => f.DateLast)
                .Skip(() => Pages.SkipRecords).Take(() => countInPage).Select(SerialsView.Map);
        }
    }
}
