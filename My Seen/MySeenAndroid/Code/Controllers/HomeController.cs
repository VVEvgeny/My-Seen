using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PortableRazor;
using MySeenAndroid.Code.Views;
using Android.Util;

namespace MySeenAndroid
{
    public class HomeController
    {
    	IHybridWebView webView;
		public HomeController (IHybridWebView webView)
		{
			this.webView = webView;
		}
        public void ShowFilmList()
        {
            HomeViewModel model = new HomeViewModel();
            model.LoadFilms();
            model.LoadSelectList();

            var template = new Home { Model = model };
            var page = template.GenerateString();
            Log.Warn("123", page);
            webView.LoadHtmlString(page);
        }
    }
}