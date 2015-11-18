using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelFilms
    {
        public IEnumerable<FilmsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }
        public RatingGenreViewModel RatinngGenre { get; set; }

        public HomeViewModelFilms(string userId, int page, int countInPage, string search)
        {
            ApplicationDbContext ac = new ApplicationDbContext();

            var routeValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(search))
            {
                routeValues.Add("search", search);
            }

            Pages = new PaginationViewModel(page,
                ac.Films.Count(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                , countInPage, "Home", "", routeValues);

            RatinngGenre = new RatingGenreViewModel();
            Data = ac.Films.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                .OrderByDescending(f => f.DateSee)
                .Select(FilmsView.Map)
                .Skip((Pages.CurentPage - 1) * countInPage)
                .Take(countInPage);
        }
    }
}
