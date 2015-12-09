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
                return Selected == Defaults.CategoryBase.Indexes.Films.ToString();
            }
        }
        public bool PageSerials
        {
            get
            {
                return Selected == Defaults.CategoryBase.Indexes.Serials.ToString();
            }
        }
        public bool PageBooks
        {
            get
            {
                return Selected == Defaults.CategoryBase.Indexes.Books.ToString();
            }
        }
        public bool PageTracks
        {
            get
            {
                return Selected == Defaults.CategoryBase.Indexes.Tracks.ToString();
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
        public bool PageEvents
        {
            get
            {
                return Selected == Defaults.CategoryBase.Indexes.Events.ToString();
            }
        }
        public IEnumerable<SelectListItem> SelectList { get; set; }

        public HomeViewModelFilmsMin Films;
        public HomeViewModelSerialsMin Serials;
        public HomeViewModelBooksMin Books;
        public HomeViewModelTracks Tracks;
        public HomeViewModelUsers Users;
        public HomeViewModelLogs Logs;
        public HomeViewModelImprovements Improvements;
        public HomeViewModelAbout About;
        public HomeViewModelEventsMin Events;

        public string Search { get; set; }

        public HomeViewModel(string selected, string userId, int page, int countInPage, int complex, string search, int markersOnRoads, int roadsYear)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Search = search;
                Selected = selected;
                if (PageSerials) Serials = new HomeViewModelSerialsMin();
                else if (PageBooks) Books = new HomeViewModelBooksMin();
                else if (PageTracks) Tracks = new HomeViewModelTracks(userId, markersOnRoads, roadsYear);
                else if (PageUsers) Users = new HomeViewModelUsers(page, countInPage);
                else if (PageLogs) Logs = new HomeViewModelLogs(page, countInPage);
                else if (PageImprovements) Improvements = new HomeViewModelImprovements(complex, page, countInPage);
                else if (PageEvents) Events = new HomeViewModelEventsMin();
                else Films = new HomeViewModelFilmsMin();

                var listItems =
                    Defaults.Categories.GetAll()
                        .Select(
                            sel =>
                                new SelectListItem
                                {
                                    Text = sel,
                                    Value = Defaults.Categories.GetId(sel).ToString(),
                                    Selected = (Defaults.Categories.GetId(sel).ToString() == Selected)
                                })
                        .ToList();
                SelectList = listItems;
            }
            else
            {
                About = new HomeViewModelAbout();
            }
        }
    }
}
