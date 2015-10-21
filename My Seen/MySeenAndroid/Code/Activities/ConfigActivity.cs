using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.Collections.Generic;
using MySeenLib;
using System.Net;
using System.IO;
using Android.Content.PM;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MySeenAndroid
{
    [Activity(Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen", ScreenOrientation = ScreenOrientation.Landscape)]
    public class ConfigActivity : Activity
    {
        private static string LogTAG = "ConfigActivity";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Config);

            Log.Warn(LogTAG, "START");

            //DatabaseHelper db = new DatabaseHelper();

            Button button = FindViewById<Button>(Resource.Id.ButtonInConfig);
            button.Click += delegate
            {
                Log.Warn(LogTAG, "CLICK");
                Sync();
            };

            TextView tv = FindViewById<TextView>(Resource.Id.textViewConfig);


            Log.Warn(LogTAG, "end OnCreate");
        }
        private void Sync()
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate
            {
                return true;
            };

            Log.Warn(LogTAG, "BEGIN SYNC");

            //string email = "vvevgeny@gmail.com";

            try
            {
                //string ApiHost = @"";

                //WebRequest req = WebRequest.Create(ApiHost + MySeenWebApi.ApiUsers + MD5Tools.GetMd5Hash(email.ToLower()) + "/" + ((int)MySeenWebApi.SyncModesApiUsers.isUserExists).ToString());
                WebRequest req = WebRequest.Create("https://10.0.2.2:44301/api/ApiUsers/e290b118702482ec7fd7c1343e02553f/1");
                //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(new Uri(ApiHost + MySeenWebApi.ApiUsers + MD5Tools.GetMd5Hash(email.ToLower()) + "/" + ((int)MySeenWebApi.SyncModesApiUsers.isUserExists).ToString()));
                //req.ContentType = "application/json";
                //req.Method = "GET";

                Log.Warn(LogTAG, "HttpWebRequest");
                //WebResponse response = req.GetResponse();
                //StreamReader stream = new System.IO.StreamReader(response.GetResponseStream());
                //string data = stream.ReadToEnd();
                //Log.Warn(LogTAG, "data="+data);
                //MySeenWebApi.SyncJsonAnswer answer = MySeenWebApi.GetResponseAnswer(data);

                MySeenWebApi.SyncJsonAnswer answer = MySeenWebApi.GetResponseAnswer((new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd());

                if (answer != null)
                {
                    if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.UserNotExist)
                    {
                        Log.Warn(LogTAG, MySeenLib.Resource.UserNotExist);
                    }
                    else
                    {
                        Log.Warn(LogTAG, MySeenLib.Resource.UserOK);
                    }
                }
                req.GetResponse().Close();
            }
            catch (Exception e)
            {
                Log.Warn(LogTAG, e.Message);
            }
            Log.Warn(LogTAG, MySeenLib.Resource.ApiError);

            Log.Warn(LogTAG, "END SYNC");
        }
    }
    public static class MD5Tools
    {
        public static string GetMd5Hash(string input)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
            catch
            {

            }
            return string.Empty;
        }
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
