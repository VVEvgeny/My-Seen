using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews.Portal;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.Portal
{
    public class PortalViewModelMemes
    {
        public IEnumerable<MemesView> Data { get; set; }
        public Pagination Pages { get; set; }

        public PortalViewModelMemes(string userId, int page, int countInPage, string search, int id)
        {
            var ac = new ApplicationDbContext();

            Pages = new Pagination(page, ac.Memes.Count(f => (string.IsNullOrEmpty(search) || f.Name.Contains(search))),
                countInPage);
            Data = MemesView.Map(ac.Memes.Where(f =>
                (string.IsNullOrEmpty(search) || f.Name.Contains(search)) &&
                (id == 0 || f.Id == id) //если конкретная запись то показать ее
                ).OrderByDescending(f => f.Date)
                .Skip(() => Pages.SkipRecords).Take(() => countInPage), userId);
        }
    }
}
