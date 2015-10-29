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
using MySeenMobileWebViewLib;
using MySeenAndroid.Code.Database;
using Android.Content.PM;

namespace MySeenAndroid
{
        [Activity(Label = "@string/ApplicationName"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.ScreenSize
        , NoHistory = false //для второго интента чтобы можно было вернуться назад
        , LaunchMode = LaunchMode.SingleTask
        //, ScreenOrientation = ScreenOrientation.Landscape 
        , Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen"
        )]
    //[Activity(Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class MainRazorActivity : Activity
    {
        private static string LogTAG = "MainRazorActivity";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MainRazor);

            Log.Warn(LogTAG, "START");

            var webView = FindViewById<WebView>(Resource.Id.webView);
            //test
            /*
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.LoadUrl("file:///android_asset/test.html");
            */
            
            var homeController = new HomeController(new HybridWebView(webView), new DataAccess());
            PortableRazor.RouteHandler.RegisterController("Home", homeController);
            //homeController.ShowFilmList();
            homeController.AddFilm();
            Log.Warn(LogTAG, homeController.lastPage);
            
            Log.Warn(LogTAG, "end OnCreate");
        }
    }
}
