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
    public class HomeViewModelBooks
    {
        public IEnumerable<BooksView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }
        public RatingGenreViewModel RatinngGenre { get; set; }

        public HomeViewModelBooks(string userId, int page, int countInPage,string search)
        {
            var RouteValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(search))
            {
                RouteValues.Add("search", search);
            }

            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Books.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) ? true : f.Name.Contains(search))).Count(), countInPage, "Home", "", RouteValues);
            RatinngGenre = new RatingGenreViewModel();
            Data = ac.Books.Where(f => f.UserId == userId && f.isDeleted != true && (string.IsNullOrEmpty(search) ? true : f.Name.Contains(search))).OrderByDescending(f => f.DateRead).Select(BooksView.Map).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
