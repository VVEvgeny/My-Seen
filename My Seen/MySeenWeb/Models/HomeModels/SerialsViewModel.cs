using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.Database;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.HomeModels
{
    public class SerialsViewModel
    {
        public SerialsViewModel(string userId, int page, int countInPage, string search)
        {
            var routeValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(search))
            {
                routeValues.Add("search", search);
            }

            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page,
                ac.Serials.Count(
                    f =>
                        f.UserId == userId && f.isDeleted != true &&
                        (string.IsNullOrEmpty(search) || f.Name.Contains(search))), countInPage, "Home", "", routeValues);
            RatinngGenre = new RatingGenreViewModel();
            Data =
                ac.Serials.Where(
                    f =>
                        f.UserId == userId && f.isDeleted != true &&
                        (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                    .OrderByDescending(f => f.DateLast)
                    .Select(SerialsView.Map)
                    .Skip((Pages.CurentPage - 1)*countInPage)
                    .Take(countInPage);
        }

        public IEnumerable<SerialsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }
        public RatingGenreViewModel RatinngGenre { get; set; }
    }
}