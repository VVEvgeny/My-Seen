using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public string Selected { get; set; }

        public bool ExtPage
        {
            get
            {
                return PageUsers || PageLogs || PageImprovements || PageErrors;
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
        public bool PageRoads
        {
            get
            {
                return Selected == Defaults.CategoryBase.Indexes.Roads.ToString();
            }
        }
        public bool PageUsers
        {
            get
            {
                return Selected == Defaults.CategoryBase.IndexesExt.Users.ToString();
            }
        }
        public bool PageLogs
        {
            get
            {
                return Selected == Defaults.CategoryBase.IndexesExt.Logs.ToString();
            }
        }
        public bool PageImprovements
        {
            get
            {
                return Selected == Defaults.CategoryBase.IndexesExt.Improvements.ToString();
            }
        }
        public bool PageEvents
        {
            get
            {
                return Selected == Defaults.CategoryBase.Indexes.Events.ToString();
            }
        }
        public bool PageErrors
        {
            get
            {
                return Selected == Defaults.CategoryBase.IndexesExt.Errors.ToString();
            }
        }

        public IEnumerable<SelectListItem> SelectList { get; set; }

        public HomeViewModelFilmsMin Films;
        public HomeViewModelSerialsMin Serials;
        public HomeViewModelBooksMin Books;
        public HomeViewModelRoadsMin Roads;
        public HomeViewModelLogsMin Logs;
        public HomeViewModelImprovementsMin Improvements;
        public HomeViewModelAbout About;
        public HomeViewModelEventsMin Events;
        public HomeViewModelErrorsMin Errors;

        public HomeViewModel(string selected, string userId, int complex, bool onlyEnded)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Selected = selected;
                if (PageSerials) Serials = new HomeViewModelSerialsMin();
                else if (PageBooks) Books = new HomeViewModelBooksMin();
                else if (PageRoads) Roads = new HomeViewModelRoadsMin();
                else if (PageUsers) { }
                else if (PageLogs) Logs = new HomeViewModelLogsMin();
                else if (PageErrors) Errors = new HomeViewModelErrorsMin();
                else if (PageImprovements) Improvements = new HomeViewModelImprovementsMin(complex);
                else if (PageEvents) Events = new HomeViewModelEventsMin(onlyEnded);
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
