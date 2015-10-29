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
        public string lastPage;
        public HomeController(IHybridWebView webView, IDataAccess dataAccess)
		{
			this.webView = webView;
            this.dataAccess = dataAccess;
            lastPage = string.Empty;
		}
        public void ShowFilmList()
        {
            HomeViewModel model = new HomeViewModel();
            model.FilmsList = dataAccess.LoadFilms();
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
        public void SaveFilm(string name)
        {
            FilmAddViewModel model = new FilmAddViewModel();
            bool error=false;
            /*
            model.Name = Name;
            
            if(Name.Length==0)
            {
                error = true;
                model.Name_Error = "Too Short Film Name";
            }
            if(!error)
            {
                if(dataAccess.isFilmNameExist(Name))
                {
                    error = true;
                    model.Name_Error = "Film already exist";
                }
            }
            if (!error)
            {
                try
                {
                    model.Genre = Convert.ToInt32(Genre);
                }
                catch
                {
                    error = true;
                    model.Unknown_Error = true;
                    model.Genre = Defaults.Genres.GetMaxId();
                }
            }
            if (!error)
            {
                try
                {
                    model.Rating = Convert.ToInt32(Rating);
                }
                catch
                {
                    error = true;
                    model.Unknown_Error = true;
                    model.Rating = Defaults.Ratings.GetMaxId();
                }
            }
            */

            //test
            /*error = false;
            model.Name = "test film";
            model.Rating = Defaults.Ratings.GetMaxId();
            model.Genre = Defaults.Genres.GetMaxId();
            */
            
            error = true;
            model.Name_Error = "Name=" + name ;


              
            if(error)
            {
                var template = new AddFilm { Model = model };
                var page = template.GenerateString();
                lastPage = page;
                webView.LoadHtmlString(page);
                return;
            }
            else
            {
                dataAccess.AddFilm(model.Name, model.Genre, model.Rating);
            }
            ShowFilmList();
        }
        public void AddSerial()
        {
            SerialAddViewModel model = new SerialAddViewModel();
            var template = new AddSerial { Model = model };
            var page = template.GenerateString();
            lastPage = page;
            webView.LoadHtmlString(page);
        }
        public void SaveSerial(string Name, string Season, string Series, string Genre, string Rating)
        {

            
            SerialAddViewModel model = new SerialAddViewModel();

            model.Name_Error = "Name=" + Name + " Season=" + Season + " Series=" + Series + " Genre=" + Genre + " Rating=" + Rating;
            model.Name = Name;
            model.Season =Season;
            model.Series = Series;
            model.Genre = Convert.ToInt32(Genre);
            model.Rating = Convert.ToInt32(Rating);



            var template = new AddSerial { Model = model };
            var page = template.GenerateString();
            lastPage = page;
            webView.LoadHtmlString(page);
        }
    }
}