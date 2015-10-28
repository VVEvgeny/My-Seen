using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortableRazor;
using MySeenMobileWebViewLib.Views;

namespace MySeenMobileWebViewLib
{
    public class HomeController
    {
    	IHybridWebView webView;
        IDataAccess dataAccess;
        public HomeController(IHybridWebView webView, IDataAccess dataAccess)
		{
			this.webView = webView;
            this.dataAccess = dataAccess;
		}
        public void ShowFilmList()
        {
            HomeViewModel model = new HomeViewModel();
            model.FilmsList = dataAccess.LoadFilms();

            var template = new Home { Model = model };
            var page = template.GenerateString();
            webView.LoadHtmlString(page);
        }
        public void AddFilm()
        {
            webView.LoadHtmlString("PAGE AddFilm NOT EXIST");
        }
    }
}