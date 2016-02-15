using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelFilms
    {
        public IEnumerable<FilmsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelFilms(string userId, int page, int countInPage, string search, string shareKey)
        {
            var ac = new ApplicationDbContext();

            Pages = new PaginationViewModel(page,
                ac.Films.Count(f =>
                    ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                     ||
                     (!string.IsNullOrEmpty(shareKey) && f.User.ShareFilmsKey == shareKey && f.Shared))
                    && (string.IsNullOrEmpty(search) || f.Name.Contains(search))),
                countInPage);

            Data = ac.Films.AsNoTracking()
                .Where(f =>
                    ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                     ||
                     (!string.IsNullOrEmpty(shareKey) && f.User.ShareFilmsKey == shareKey && f.Shared))
                    && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                .OrderByDescending(f => f.DateSee)
                .Skip(() => Pages.SkipRecords)
                .Take(() => countInPage)
                .Select(FilmsView.Map);
        }
    }
}
