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
    public class HomeViewModelSerials
    {
        public IEnumerable<SerialsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }
        public RatingGenreViewModel RatinngGenre { get; set; }

        public HomeViewModelSerials(string userId, int page, int countInPage, string search)
        {
            var RouteValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(search))
            {
                RouteValues.Add("search", search);
            }

            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Serials.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) ? true : f.Name.Contains(search))).Count(), countInPage, "Home", "", RouteValues);
            RatinngGenre = new RatingGenreViewModel();
            Data = ac.Serials.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) ? true : f.Name.Contains(search))).OrderByDescending(f => f.DateLast).Select(SerialsView.Map).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
