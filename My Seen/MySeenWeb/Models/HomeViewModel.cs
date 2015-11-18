using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public string Selected { get; set; }

        public enum CategoryExt
        {
            Users = 101,
            Logs = 102,
            Improvements = 103
        }
        public static bool IsCategoryExt(int category)
        {
            return category == (int)CategoryExt.Users || category == (int)CategoryExt.Logs || category == (int)CategoryExt.Improvements;
        }

        public bool ExtPage
        {
            get
            {
                return PageUsers || PageLogs || PageImprovements;
            }
        }

        public bool PageFilms
        {
            get
            {
                return Selected == Defaults.CategoryBase.FilmIndex.ToString();
            }
        }
        public bool PageSerials
        {
            get
            {
                return Selected == Defaults.CategoryBase.SerialIndex.ToString();
            }
        }
        public bool PageBooks
        {
            get
            {
                return Selected == Defaults.CategoryBase.BookIndex.ToString();
            }
        }
        public bool PageTracks
        {
            get
            {
                return Selected == Defaults.CategoryBase.TrackIndex.ToString();
            }
        }
        public bool PageUsers
        {
            get
            {
                return Selected == ((int)CategoryExt.Users).ToString();
            }
        }
        public bool PageLogs
        {
            get
            {
                return Selected == ((int)CategoryExt.Logs).ToString();
            }
        }
        public bool PageImprovements
        {
            get
            {
                return Selected == ((int)CategoryExt.Improvements).ToString();
            }
        }

        public IEnumerable<SelectListItem> SelectList { get; set; }

        public HomeViewModelFilms Films;
        public HomeViewModelSerials Serials;
        public HomeViewModelBooks Books;
        public HomeViewModelTracks Tracks;
        public HomeViewModelUsers Users;
        public HomeViewModelLogs Logs;
        public HomeViewModelImprovements Improvements;

        public string Search { get; set; }


        public HomeViewModel(string selected, string userId, int page, int countInPage, int complex, string search)
        {
            Search = search;
            Selected = selected;
            if (PageSerials) Serials = new HomeViewModelSerials(userId, page, countInPage, search);
            else if (PageBooks) Books = new HomeViewModelBooks(userId, page, countInPage, search);
            else if (PageTracks) Tracks = new HomeViewModelTracks(userId);
            else if (PageUsers) Users = new HomeViewModelUsers(page, countInPage);
            else if (PageLogs) Logs = new HomeViewModelLogs(page, countInPage);
            else if (PageImprovements) Improvements = new HomeViewModelImprovements(complex, page, countInPage);
            else Films = new HomeViewModelFilms(userId, page, countInPage, search);

            List<SelectListItem> listItems = Defaults.Categories.GetAll().Select(sel => new SelectListItem {Text = sel, Value = Defaults.Categories.GetId(sel).ToString(), Selected = (Defaults.Categories.GetId(sel).ToString() == Selected)}).ToList();
            SelectList = listItems;
        }
    }
}
