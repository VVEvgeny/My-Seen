using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public static class AFCookies
        {
            public static string CoockieSelectedKey = "eSelected";
            public static string CoockieSelectedValueFilms = Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex);
            public static string CoockieSelectedValueSerials = Defaults.Categories.GetById(Defaults.CategoryBase.SerialIndex);
            public static string CoockieSelectedValueBooks = Defaults.Categories.GetById(Defaults.CategoryBase.BookIndex);
        }
        public PaginationViewModel Pages { get; set; }
        public string Selected;
        public bool IsSelectedFilm
        {
            get { return Selected == Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex); }            
        }
        public bool IsSelectedSerial
        {
            get { return Selected == Defaults.Categories.GetById(Defaults.CategoryBase.SerialIndex); }
        }

        public IEnumerable<SelectListItem> selectList { get; set; }
        public string Rating;
        public IEnumerable<SelectListItem> ratingList { get; set; }
        public string Genre;
        public IEnumerable<SelectListItem> genreList { get; set; }

        public HomeViewModel()
        {
            Selected = Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex);
            Rating = Defaults.Ratings.GetMaxValue();
            Genre = Defaults.Genres.GetMaxValue();
        }
        public IEnumerable<FilmsView> Films;
        public IEnumerable<SerialsView> Serials;
        public IEnumerable<BooksView> Books;

        public void LoadSelectList()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            //foreach (eSelected sel in Enum.GetValues(typeof(eSelected)).Cast<eSelected>())
            foreach (string sel in Defaults.Categories.GetAll())
            {
                listItems.Add(new SelectListItem { Text = sel, Value = sel, Selected = (sel == Selected) });
            }
            selectList = listItems;

            List<SelectListItem> listItemsRating = new List<SelectListItem>();
            foreach (string sel in Defaults.Ratings.GetAll())
            {
                listItemsRating.Add(new SelectListItem { Text = sel, Value = Defaults.Ratings.GetId(sel).ToString(), Selected = (sel == Rating) });
            }
            ratingList = listItemsRating;

            List<SelectListItem> listItemsGenre = new List<SelectListItem>();
            foreach (string sel in Defaults.Genres.GetAll())
            {
                listItemsGenre.Add(new SelectListItem { Text = sel, Value = Defaults.Genres.GetId(sel).ToString(), Selected = (sel == Genre) });
            }
            genreList = listItemsGenre;
        }
        public void LoadFilms(string userId, int page, int countInPage)
        {
            ApplicationDbContext ac= new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Films.Where(f => f.UserId == userId && f.isDeleted != true).Count(), countInPage, "Home", "");
            Films = ac.Films.Where(f => f.UserId == userId && f.isDeleted != true).OrderByDescending(f => f.DateSee).Select(FilmsView.Map).Skip((page - 1) * countInPage).Take(countInPage);
        }
        public void LoadSerials(string userId, int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Serials.Where(f => f.UserId == userId && f.isDeleted != true).Count(), countInPage, "Home", "");
            Serials = ac.Serials.Where(f => f.UserId == userId && f.isDeleted != true).OrderByDescending(f => f.DateLast).Select(SerialsView.Map).Skip((page - 1) * countInPage).Take(countInPage);
        }
        public void LoadBooks(string userId, int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Books.Where(f => f.UserId == userId && f.isDeleted != true).Count(), countInPage, "Home", "");
            Books = ac.Books.Where(f => f.UserId == userId && f.isDeleted != true).OrderByDescending(f => f.DateRead).Select(BooksView.Map).Skip((page - 1) * countInPage).Take(countInPage);
        }
    }
}
