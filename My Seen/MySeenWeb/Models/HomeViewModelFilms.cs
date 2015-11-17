using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;

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

            var RouteValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(search))
            {
                RouteValues.Add("search", search);
            }

            Pages = new PaginationViewModel(page,
                ac.Films.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) ? true : f.Name.Contains(search))).Count()
                , countInPage, "Home", "", RouteValues);

            RatinngGenre = new RatingGenreViewModel();
            Data = ac.Films.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) ? true : f.Name.Contains(search)))
                .OrderByDescending(f => f.DateSee)
                .Select(FilmsView.Map)
                .Skip((Pages.CurentPage - 1) * countInPage)
                .Take(countInPage);
        }
    }
}
