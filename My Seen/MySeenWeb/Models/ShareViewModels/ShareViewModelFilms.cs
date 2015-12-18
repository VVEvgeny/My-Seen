using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelFilms
    {
        public IEnumerable<FilmsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public bool HaveData
        {
            get { return Data.Any(); }
        }
        public ShareViewModelFilms(string key, int page, int countInPage)
        {
            var ac = new ApplicationDbContext();

            Pages = new PaginationViewModel(page,ac.Films.Count(f => f.User.ShareFilmsKey == key && f.Shared), countInPage);
            Data = ac.Films.Where(f => f.User.ShareFilmsKey == key && f.Shared)
                .OrderByDescending(f => f.DateSee)
                .Select(FilmsView.Map)
                .Skip((Pages.CurentPage - 1)*countInPage)
                .Take(countInPage);
        }
    }
}
