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

        public bool HaveData
        {
            get { return Data.Any(); }
        }

        public HomeViewModelFilms(string userId, int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();

            Pages = new PaginationViewModel(page,
                ac.Films.Count(f => f.UserId == userId && (string.IsNullOrEmpty(search) || f.Name.Contains(search))),
                countInPage);

            Data = ac.Films.AsNoTracking()
                .Where(f => f.UserId == userId && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                .OrderByDescending(f => f.DateSee)
                .Skip(() => (Pages.CurentPage - 1)*countInPage)
                .Take(() => countInPage)
                .Select(FilmsView.Map);
        }
    }
}
