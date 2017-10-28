using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelFilms
    {
        public IEnumerable<FilmsView> Data { get; set; }
        public Pagination Pages { get; set; }
        public bool IsMyData { get; set; }

        public HomeViewModelFilms(string userId, int page, int countInPage, string search, string shareKey,
            ICacheService cache)
        {
            Pages =
                cache.Get<Pagination>(cache.GetFormatedName(CacheNames.UserFilmsPages.ToString(), userId, page,
                    countInPage, search, shareKey));
            Data =
                cache.Get<IEnumerable<FilmsView>>(cache.GetFormatedName(CacheNames.UserFilms.ToString(), userId, page,
                    countInPage, search, shareKey));
            
            if (Pages == null || Data == null)
            {
                var ac = new ApplicationDbContext();

                if (Pages == null)
                {
                    Pages = new Pagination(page,
                        ac.Films.Count(f =>
                            ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                             ||
                             (!string.IsNullOrEmpty(shareKey) && f.User.ShareFilmsKey == shareKey && f.Shared))
                            && (string.IsNullOrEmpty(search) || f.Name.Contains(search))),
                        countInPage);
                    cache.Set(
                        cache.GetFormatedName(CacheNames.UserFilmsPages.ToString(), userId, page, countInPage, search,
                            shareKey), Pages, 15);
                }
                if (Data == null)
                {
                    Data = ac.Films.AsNoTracking()
                        .Where(f =>
                            ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                             ||
                             (!string.IsNullOrEmpty(shareKey) && f.User.ShareFilmsKey == shareKey && f.Shared))
                            && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                        .OrderByDescending(f => f.DateSee)
                        .ThenBy(f => f.Name)
                        .Skip(() => Pages.SkipRecords)
                        .Take(() => countInPage)
                        .Select(FilmsView.Map).ToList();
                    cache.Set(
                        cache.GetFormatedName(CacheNames.UserFilms.ToString(), userId, page, countInPage, search,
                            shareKey), Data, 15);
                }
            }
            IsMyData = !string.IsNullOrEmpty(shareKey) && Data.Any() && Data.First().UserId == userId;
        }
    }
}