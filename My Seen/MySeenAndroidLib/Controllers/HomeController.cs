using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortableRazor;
using MySeenMobileWebViewLib.Views;
using MySeenLib;

namespace MySeenMobileWebViewLib
{
    public class HomeController
    {
    	private IHybridWebView webView;
        private IDataAccess dataAccess;
        private int CurrentView = Defaults.CategoryBase.FilmIndex;
        public string lastPage;
        public HomeController(IHybridWebView webView, IDataAccess dataAccess)
		{
			this.webView = webView;
            this.dataAccess = dataAccess;
            lastPage = string.Empty;
		}
        public void ChangeSelected()
        {
            if (CurrentView == Defaults.CategoryBase.FilmIndex) CurrentView = Defaults.CategoryBase.SerialIndex;
            else CurrentView = Defaults.CategoryBase.FilmIndex;
            ShowFilmList();
        }
        public void ShowFilmList()
        {
            HomeViewModel model = new HomeViewModel();
            model.isFilm = CurrentView == Defaults.CategoryBase.FilmIndex;
            if (CurrentView == Defaults.CategoryBase.FilmIndex)
            {
                model.FilmsList = dataAccess.LoadFilms();
            }
            else
            {
                model.SerialsList = dataAccess.LoadSerials();
            }
            var template = new Home { Model = model };
            var page = template.GenerateString();
            lastPage = page;
            webView.LoadHtmlString(page);
         }
        public void AddFilm()
        {
            FilmAddViewModel model = new FilmAddViewModel();
            var template = new AddFilm { Model = model };
            var page = template.GenerateString();
            lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void EditFilm(string _id)
        {
            FilmAddViewModel model = new FilmAddViewModel();
            model.Error = " id=" + _id.ToString();
            //model.id = _id;
            dataAccess.GetFilmById(model.id, ref model.Name, ref model.Genre, ref model.Rating);
            var template = new AddFilm { Model = model };
            var page = template.GenerateString();
            lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void AddSerial()
        {
            SerialAddViewModel model = new SerialAddViewModel();
            var template = new AddSerial { Model = model };
            var page = template.GenerateString();
            lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void SaveFilm(string Name, string Genre, string Rating)
        {
            FilmAddViewModel model = new FilmAddViewModel();
            if (string.IsNullOrEmpty(model.Error))
            {
                if (Name.Length == 0)
                {
                    model.Error = "Too Short Film Name";
                }
                else if (dataAccess.isFilmNameExist(Name))
                {
                    model.Error = "Film already exist";
                }
                else
                {
                    model.Name = Name;
                }
            }
            int iGenre = 0;
            int iRating = 0;
            if (string.IsNullOrEmpty(model.Error))
            {
                try
                {
                    iGenre = Convert.ToInt32(Genre);
                    model.Genre = iGenre;
                }
                catch
                {
                    iGenre = Defaults.Genres.GetMaxId();
                }

                try
                {
                    iRating = Convert.ToInt32(Rating);
                    model.Rating = iRating;
                }
                catch
                {
                    iRating = Defaults.Ratings.GetMaxId();
                }
            }

            if (string.IsNullOrEmpty(model.Error))
            {
                dataAccess.AddFilm(model.Name, model.Genre, model.Rating);
            }
            else
            {
                var template = new AddFilm { Model = model };
                var page = template.GenerateString();
                lastPage = page;
                webView.LoadHtmlString(page);
                return;
            }
            ShowFilmList();
        }
        public void SaveSerial(string Name, string Season, string Series, string Genre, string Rating)
        {
            SerialAddViewModel model = new SerialAddViewModel();

            //model.Error = "Name=" + Name + " Season=" + Season + " Series=" + Series + " Genre=" + Genre + " Rating=" + Rating;
            if (string.IsNullOrEmpty(model.Error))
            {
                if (Name.Length == 0)
                {
                    model.Error = "Too Short Film Name";
                }
                else if (dataAccess.isFilmNameExist(Name))
                {
                    model.Error = "Film already exist";
                }
                else
                {
                    model.Name = Name;
                }
            }
            int iSeason = 0;
            int iSeries = 0;
            int iGenre = 0;
            int iRating = 0;
            if (string.IsNullOrEmpty(model.Error))
            {
                try
                {
                    iSeason = Convert.ToInt32(Season);
                    model.Season = iSeason;
                }
                catch
                {
                    iSeason = 1;
                }
                try
                {
                    iSeries = Convert.ToInt32(Series);
                    model.Series = iSeries;
                }
                catch
                {
                    iSeries = 1;
                }
                try
                {
                    iGenre = Convert.ToInt32(Genre);
                    model.Genre = iGenre;
                }
                catch
                {
                    iGenre = Defaults.Genres.GetMaxId();
                }
                try
                {
                    iRating = Convert.ToInt32(Rating);
                    model.Rating = iRating;
                }
                catch
                {
                    iRating = Defaults.Ratings.GetMaxId();
                }
            }
            if (string.IsNullOrEmpty(model.Error))
            {
                dataAccess.AddSerial(model.Name, model.Season, model.Series, model.Genre, model.Rating);
            }
            else
            {
                var template = new AddSerial { Model = model };
                var page = template.GenerateString();
                lastPage = page;
                webView.LoadHtmlString(page);
                return;
            }
            ShowFilmList();
        }
    }
}