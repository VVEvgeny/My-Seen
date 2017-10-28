using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Add_Code;
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

        public HomeViewModelSerials(string userId, int page, int countInPage, string search, string shareKey,
            ICacheService cache)
        {
            Pages =
                cache.Get<Pagination>(cache.GetFormatedName(CacheNames.UserSerialsPages.ToString(), userId, page,
                    countInPage, search, shareKey));
            Data =
                cache.Get<IEnumerable<SerialsView>>(cache.GetFormatedName(CacheNames.UserSerials.ToString(), userId,
                    page, countInPage, search, shareKey));

            if (Pages == null || Data == null)
            {
                var ac = new ApplicationDbContext();

                if (Pages == null)
                {
                    Pages = new Pagination(page, ac.Serials.Count(f =>
                        ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                         ||
                         (!string.IsNullOrEmpty(shareKey) && f.User.ShareSerialsKey == shareKey && f.Shared))
                        && (string.IsNullOrEmpty(search) || f.Name.Contains(search))), countInPage);
                    cache.Set(
                        cache.GetFormatedName(CacheNames.UserSerialsPages.ToString(), userId, page, countInPage, search,
                            shareKey), Pages, 15);
                }
                if (Data == null)
                {
                    Data = ac.Serials.AsNoTracking()
                        .Where(f =>
                            ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                             ||
                             (!string.IsNullOrEmpty(shareKey) && f.User.ShareSerialsKey == shareKey && f.Shared))
                            && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                        .OrderByDescending(f => f.DateLast)
                        .ThenBy(f => f.Name)
                        .Skip(() => Pages.SkipRecords).Take(() => countInPage).Select(SerialsView.Map).ToList();
                    cache.Set(
                        cache.GetFormatedName(CacheNames.UserSerials.ToString(), userId, page, countInPage, search,
                            shareKey), Data, 15);
                }
            }
            IsMyData = !string.IsNullOrEmpty(shareKey) && Data.Any() && Data.First().UserId == userId;
        }
    }
}