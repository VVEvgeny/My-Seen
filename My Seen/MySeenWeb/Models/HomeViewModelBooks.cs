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
        public HomeViewModelBooks()
        {

        }
        public HomeViewModelBooks(string userId, int page, int countInPage, ref PaginationViewModel Pages)
        {
            Load(userId, page, countInPage, ref Pages);
        }
        public IEnumerable<BooksView> Data { get; set; }

        private void Load(string userId, int page, int countInPage, ref PaginationViewModel Pages)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Books.Where(f => f.UserId == userId && f.isDeleted != true).Count(), countInPage, "Home", "");
            Data = ac.Books.Where(f => f.UserId == userId && f.isDeleted != true).OrderByDescending(f => f.DateRead).Select(BooksView.Map).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
