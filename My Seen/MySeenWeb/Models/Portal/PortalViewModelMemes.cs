using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews.Portal;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.Portal
{
    public class PortalViewModelMemes
    {
        public IEnumerable<MemesView> Data { get; set; }
        public Pagination Pages { get; set; }

        public PortalViewModelMemes(string userId, int page, int countInPage, string search, int id, ICacheService cache)
        {
            Pages =
                cache.Get<Pagination>(cache.GetFormatedName(CacheNames.MemesPages.ToString(), page, countInPage, search,
                    id));
            Data =
                cache.Get<IEnumerable<MemesView>>(cache.GetFormatedName(CacheNames.Memes.ToString(), page, countInPage,
                    search, id));

            if (Pages == null || Data == null)
            {
                var ac = new ApplicationDbContext();

                if (Pages == null)
                {
                    Pages = new Pagination(page,
                        ac.Memes.Count(f => string.IsNullOrEmpty(search) || f.Name.Contains(search)),
                        countInPage);
                    if (id != 0) Pages.SkipRecords = 0;

                    cache.Set(
                        cache.GetFormatedName(CacheNames.MemesPages.ToString(), page, countInPage, search, id),
                        Pages, 15);
                }

                if (Data == null)
                {
                    Data = MemesView.Map(ac.Memes.Where(f =>
                        (string.IsNullOrEmpty(search) || f.Name.Contains(search)) &&
                        (id == 0 || f.Id == id) //если конкретная запись то показать ее
                        ).OrderByDescending(f => f.Date)
                        .Skip(() => Pages.SkipRecords).Take(() => countInPage), userId);

                    cache.Set(
                        cache.GetFormatedName(CacheNames.Memes.ToString(), page, countInPage, search, id), Data,
                        15);
                }
            }
        }
    }
}