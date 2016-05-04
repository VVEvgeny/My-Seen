using System;
using PortableRazor;
using MySeenMobileWebViewLib.Views;
using MySeenLib;

namespace MySeenMobileWebViewLib
{
    public class HomeController
    {
    	private readonly IHybridWebView webView;
        private readonly IDataAccess dataAccess;
        private int _currentView = (int)Defaults.CategoryBase.Indexes.Films;
        private string _lastPage;

        public HomeController(IHybridWebView webView, IDataAccess dataAccess)
		{
			this.webView = webView;
            this.dataAccess = dataAccess;
            _lastPage = string.Empty;
		}

        private void ShowFilmList()
        {
            var model = new HomeViewModel {isFilm = _currentView == (int) Defaults.CategoryBase.Indexes.Films};
            if (_currentView == (int)Defaults.CategoryBase.Indexes.Films)
            {
                model.FilmsList = dataAccess.LoadFilms();
            }
            else
            {
                model.SerialsList = dataAccess.LoadSerials();
            }
            var template = new Home { Model = model };
            var page = template.GenerateString();
            _lastPage = page;
            webView.LoadHtmlString(page);
         }
        public void AddFilm()
        {
            var model = new FilmAddViewModel();
            var template = new AddFilm { Model = model };
            var page = template.GenerateString();
            _lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void EditFilm(string id)
        {
            var model = new FilmAddViewModel();
            model.Error = " id=" + id.ToString();
            //model.id = _id;
            dataAccess.GetFilmById(model.id, ref model.Name, ref model.Genre, ref model.Rating);
            var template = new AddFilm { Model = model };
            var page = template.GenerateString();
            _lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void AddSerial()
        {
            var model = new SerialAddViewModel();
            var template = new AddSerial { Model = model };
            var page = template.GenerateString();
            _lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void SaveFilm(string name, string genre, string rating)
        {
            var model = new FilmAddViewModel();
            if (string.IsNullOrEmpty(model.Error))
            {
                if (name.Length == 0)
                {
                    model.Error = "Too Short Film Name";
                }
                else if (dataAccess.IsFilmNameExist(name))
                {
                    model.Error = "Film already exist";
                }
                else
                {
                    model.Name = name;
                }
            }
            if (string.IsNullOrEmpty(model.Error))
            {
                int iGenre;
                try
                {
                    iGenre = Convert.ToInt32(genre);
                    model.Genre = iGenre;
                }
                catch
                {
                    iGenre = Defaults.Genres.GetMaxId();
                }

                int iRating;
                try
                {
                    iRating = Convert.ToInt32(rating);
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
                _lastPage = page;
                webView.LoadHtmlString(page);
                return;
            }
            ShowFilmList();
        }
        public void SaveSerial(string name, string season, string series, string genre, string rating)
        {
            var model = new SerialAddViewModel();

            //model.Error = "Name=" + Name + " Season=" + Season + " Series=" + Series + " Genre=" + Genre + " Rating=" + Rating;
            if (string.IsNullOrEmpty(model.Error))
            {
                if (name.Length == 0)
                {
                    model.Error = "Too Short Film Name";
                }
                else if (dataAccess.IsFilmNameExist(name))
                {
                    model.Error = "Film already exist";
                }
                else
                {
                    model.Name = name;
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
                    iSeason = Convert.ToInt32(season);
                    model.Season = iSeason;
                }
                catch
                {
                    iSeason = 1;
                }
                try
                {
                    iSeries = Convert.ToInt32(series);
                    model.Series = iSeries;
                }
                catch
                {
                    iSeries = 1;
                }
                try
                {
                    iGenre = Convert.ToInt32(genre);
                    model.Genre = iGenre;
                }
                catch
                {
                    iGenre = Defaults.Genres.GetMaxId();
                }
                try
                {
                    iRating = Convert.ToInt32(rating);
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
                _lastPage = page;
                webView.LoadHtmlString(page);
                return;
            }
            ShowFilmList();
        }
    }
}