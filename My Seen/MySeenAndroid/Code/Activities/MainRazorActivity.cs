using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.Collections.Generic;
using System.Collections;
using MySeenLib;
using System.Net;
using Android.Webkit;
using MySeenAndroid.Code.Views;

namespace MySeenAndroid
{
    [Activity(Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class MainRazorActivity : Activity
    {
        private static string LogTAG = "MainRazorActivity";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainRazor);

            Log.Warn(LogTAG, "START");

            var webView = FindViewById<WebView>(Resource.Id.webView);
            var homeController = new HomeController(new HybridWebView(webView));
            PortableRazor.RouteHandler.RegisterController("Home", homeController);
            homeController.ShowFilmList();

            Log.Warn(LogTAG, "end OnCreate");
        }
    }
}
