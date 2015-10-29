using System;
using Android.App;
using Android.Content;
using Android.Webkit;
using PortableRazor;
using Android.Util;

namespace MySeenAndroid
{
    public class HybridWebView : IHybridWebView
    {
        private string basePath = string.Empty;
        public string BasePath 
        { 
            get
            {
                return basePath;
            }
        }

        WebView webView;

        public HybridWebView(WebView uiWebView)
        {
            webView = uiWebView;

            // Use subclassed WebViewClient to intercept hybrid native calls
            var webViewClient = new HybridWebViewClient();

            webView.SetWebViewClient(webViewClient);
            webView.Settings.CacheMode = CacheModes.CacheElseNetwork;
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DomStorageEnabled = true;
            webView.Settings.JavaScriptCanOpenWindowsAutomatically = false;

            webView.SetWebChromeClient(new HybridWebChromeClient(webView.Context));
        }

        #region IHybridWebView implementation

        public void LoadHtmlString(string html)
        {
            //var datapath = String.Format("/data/data/{0}/files/", Application.Context.PackageName);
            //var url = "file://" + datapath;
            var url = "file:///android_asset/home.cshtml";
            webView.LoadDataWithBaseURL(url, html, "text/html", "UTF-8", null);
        }

        public string EvaluateJavascript(string script)
        {
            webView.LoadUrl("javascript:" + script);
            return "";
        }

        #endregion

        class HybridWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView webView, string url)
            {
                var handled = RouteHandler.HandleRequest(url);
                return handled;
            }
        }

        class HybridWebChromeClient : WebChromeClient
        {
            Context context;

            public HybridWebChromeClient(Context context)
                : base()
            {
                this.context = context;
            }

            public override bool OnJsAlert(WebView view, string url, string message, JsResult result)
            {
                var alertDialogBuilder = new AlertDialog.Builder(context)
                    .SetMessage(message)
                    .SetCancelable(false)
                    .SetPositiveButton("Ok", (sender, args) =>
                    {
                        result.Confirm();
                    });

                alertDialogBuilder.Create().Show();

                return true;
            }

            public override bool OnJsConfirm(WebView view, string url, string message, JsResult result)
            {
                var alertDialogBuilder = new AlertDialog.Builder(context)
                    .SetMessage(message)
                    .SetCancelable(false)
                    .SetPositiveButton("Ok", (sender, args) =>
                    {
                        result.Confirm();
                    })
                    .SetNegativeButton("Cancel", (sender, args) =>
                    {
                        result.Cancel();
                    });

                alertDialogBuilder.Create().Show();

                return true;
            }
        }
    }
}