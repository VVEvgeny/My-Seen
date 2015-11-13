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
        public HomeViewModelFilms()
        {

        }
        public HomeViewModelFilms(string userId, int page, int countInPage, ref PaginationViewModel Pages)
        {
            Load(userId, page, countInPage,ref Pages);
        }
        public IEnumerable<FilmsView> Data { get; set; }
        public void Load(string userId, int page, int countInPage,ref PaginationViewModel Pages)
        {
            ApplicationDbContext ac= new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Films.Where(f => f.UserId == userId && f.isDeleted != true).Count(), countInPage, "Home", "");
            Data = ac.Films.Where(f => f.UserId == userId && f.isDeleted != true).OrderByDescending(f => f.DateSee).Select(FilmsView.Map).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
