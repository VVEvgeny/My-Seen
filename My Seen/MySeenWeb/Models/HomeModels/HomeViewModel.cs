using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.HomeModels.Ext;

namespace MySeenWeb.Models.HomeModels
{
    public class HomeViewModel
    {
        public enum CategoryExt
        {
            Users = 101,
            Logs = 102,
            Improvements = 103
        }

        public BooksViewModel Books;

        public FilmsViewModel Films;
        public ImprovementsViewModel Improvements;
        public LogsViewModel Logs;
        public SerialsViewModel Serials;
        public TracksViewModel Tracks;
        public UsersViewModel Users;


        public HomeViewModel(string selected, string userId, int page, int countInPage, int complex, string search)
        {
            Search = search;
            Selected = selected;
            if (PageSerials) Serials = new SerialsViewModel(userId, page, countInPage, search);
            else if (PageBooks) Books = new BooksViewModel(userId, page, countInPage, search);
            else if (PageTracks) Tracks = new TracksViewModel(userId);
            else if (PageUsers) Users = new UsersViewModel(page, countInPage);
            else if (PageLogs) Logs = new LogsViewModel(page, countInPage);
            else if (PageImprovements) Improvements = new ImprovementsViewModel(complex, page, countInPage);
            else Films = new FilmsViewModel(userId, page, countInPage, search);

            var listItems =
                Defaults.Categories.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Categories.GetId(sel).ToString(),
                                Selected = Defaults.Categories.GetId(sel).ToString() == Selected
                            })
                    .ToList();
            SelectList = listItems;
        }

        public string Selected { get; set; }

        public bool ExtPage
        {
            get { return PageUsers || PageLogs || PageImprovements; }
        }

        public bool PageFilms
        {
            get { return Selected == Defaults.CategoryBase.FilmIndex.ToString(); }
        }

        public bool PageSerials
        {
            get { return Selected == Defaults.CategoryBase.SerialIndex.ToString(); }
        }

        public bool PageBooks
        {
            get { return Selected == Defaults.CategoryBase.BookIndex.ToString(); }
        }

        public bool PageTracks
        {
            get { return Selected == Defaults.CategoryBase.TrackIndex.ToString(); }
        }

        public bool PageUsers
        {
            get { return Selected == ((int) CategoryExt.Users).ToString(); }
        }

        public bool PageLogs
        {
            get { return Selected == ((int) CategoryExt.Logs).ToString(); }
        }

        public bool PageImprovements
        {
            get { return Selected == ((int) CategoryExt.Improvements).ToString(); }
        }

        public IEnumerable<SelectListItem> SelectList { get; set; }

        public string Search { get; set; }

        public static bool IsCategoryExt(int category)
        {
            return category == (int) CategoryExt.Users || category == (int) CategoryExt.Logs ||
                   category == (int) CategoryExt.Improvements;
        }
    }
}