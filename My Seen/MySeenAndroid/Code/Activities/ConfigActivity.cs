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
using System.IO;
using Android.Content.PM;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

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

            EditText email_text = FindViewById<EditText>(Resource.Id.edit_config_email);
            if (DatabaseHelper.Get.GetAddDataCount() != 0) email_text.Text = DatabaseHelper.Get.GetAddData().Email;

            Button button = FindViewById<Button>(Resource.Id.Config_Sync);
            button.Click += delegate
            {
                Sync(email_text.Text);
            };

            Button button_exit = FindViewById<Button>(Resource.Id.ExitButton_Config);
            button_exit.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };

            Log.Warn(LogTAG, "end OnCreate");
        }

        public static MySeenWebApi.SyncJsonData Map(Films model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                IsFilm = true,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.isDeleted
            };
        }
        public static Films MapToFilm(MySeenWebApi.SyncJsonData model)
        {
            if (model == null) return new Films();

            return new Films
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.isDeleted
            };
        }
        public static Serials MapToSerial(MySeenWebApi.SyncJsonData model)
        {
            if (model == null) return new Serials();

            return new Serials
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                isDeleted = model.isDeleted
            };
        }
        public static MySeenWebApi.SyncJsonData Map(Serials model)
        {
            if (model == null) return new MySeenWebApi.SyncJsonData();

            return new MySeenWebApi.SyncJsonData
            {
                IsFilm = false,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                isDeleted = model.isDeleted
            };
        }
        private void Sync(string email)
        {
            Log.Warn(LogTAG, "BEGIN SYNC email=" + email);

            TextView tv_error = FindViewById<TextView>(Resource.Id.config_error);
            tv_error.Visibility = ViewStates.Gone;

            TextView tv_info = FindViewById<TextView>(Resource.Id.config_info);
            tv_info.Visibility = ViewStates.Visible;

            string mess = string.Empty;
            if (!Validations.ValidateEmail(ref mess, email))
            {
                tv_error.Visibility = ViewStates.Visible;
                tv_error.Text = MySeenLib.Resource.EmailIncorrect;
                Log.Warn(LogTAG, MySeenLib.Resource.EmailIncorrect);
                tv_info.Visibility = ViewStates.Gone;
                return;
            }

            ServicePointManager.ServerCertificateValidationCallback += delegate
            {
                return true;
            };

            List<MySeenWebApi.SyncJsonData> films = new List<MySeenWebApi.SyncJsonData>();
            foreach (Films film in DatabaseHelper.Get.GetFilmsWithDeleted())
            {
                films.Add(Map(film));
            }
            foreach (Serials film in DatabaseHelper.Get.GetSerialsWithDeleted())
            {
                films.Add(Map(film));
            }
            Log.Warn(LogTAG, "data for sync=" + films.Count.ToString());

            WebRequest req;
            MySeenWebApi.SyncJsonAnswer answer;
            if (films.Count != 0)
            {
                req = WebRequest.Create(MySeenWebApi.ApiHostAndroid + MySeenWebApi.ApiSync + MD5Tools.GetMd5Hash(email.ToLower()) + "/" + ((int)MySeenWebApi.SyncModesApiData.PostNewUpdatedDeleted).ToString());
                req.Method = "POST";
                req.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest)req).UserAgent = "MySeen";
                req.ContentType = "application/json";
                string postData = MySeenWebApi.SetResponse(films);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                answer = MySeenWebApi.GetResponseAnswer((new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd());
                req.GetResponse().Close();
                if (answer == null)
                {
                    tv_error.Visibility = ViewStates.Visible;
                    tv_error.Text = MySeenLib.Resource.ApiError;
                    Log.Warn(LogTAG, MySeenLib.Resource.ApiError);
                    tv_info.Visibility = ViewStates.Gone;
                    return;
                }
            }
            req = WebRequest.Create(MySeenWebApi.ApiHostAndroid + MySeenWebApi.ApiSync + MD5Tools.GetMd5Hash(email.ToLower()) + "/" + ((int)MySeenWebApi.SyncModesApiData.GetAll).ToString());
            string data = (new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd();
            req.GetResponse().Close();
            answer = MySeenWebApi.GetResponseAnswer(data);
            if (answer != null)
            {
                Log.Warn(LogTAG, "GetResponseAnswer=" + answer.Value);
                if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.UserNotExist)
                {
                    tv_error.Visibility = ViewStates.Visible;
                    tv_error.Text = MySeenLib.Resource.UserNotExist;
                    Log.Warn(LogTAG, MySeenLib.Resource.UserNotExist);
                    tv_info.Visibility = ViewStates.Gone;
                    return;
                }
                else if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.BadRequestMode)
                {
                    tv_error.Visibility = ViewStates.Visible;
                    tv_error.Text = MySeenLib.Resource.BadRequestMode;
                    Log.Warn(LogTAG, MySeenLib.Resource.BadRequestMode);
                    tv_info.Visibility = ViewStates.Gone;
                    return;
                }
            }
            else
            {
                Log.Warn(LogTAG, "save data");
                DatabaseHelper.Get.ClearFilms();
                DatabaseHelper.Get.ClearSerials();
                foreach (MySeenWebApi.SyncJsonData film in MySeenWebApi.GetResponse(data))
                {
                    if (film.IsFilm)
                    {
                        //Log.Warn(LogTAG, "film name=" + film.Name + " id=" + film.Id);
                        DatabaseHelper.Get.Add(MapToFilm(film));
                    }
                    else
                    {
                        //Log.Warn(LogTAG, "serial name=" + film.Name);
                        DatabaseHelper.Get.Add(MapToSerial(film));
                    }
                }
            }
            foreach (Films f in DatabaseHelper.Get.GetFilms())
            {
                if (f.isDeleted.GetValueOrDefault(false)) DatabaseHelper.Get.Delete(f);
            }
            foreach (Serials f in DatabaseHelper.Get.GetSerials())
            {
                if (f.isDeleted.GetValueOrDefault(false)) DatabaseHelper.Get.Delete(f);
            }

            tv_info.Text = "Sync OK";
            AddData ad = new AddData() { Email = email };
            if (DatabaseHelper.Get.GetAddDataCount() != 0)
            {
                DatabaseHelper.Get.ClearAddData();
            }
            DatabaseHelper.Get.Add(ad);






            /*
            Log.Warn(LogTAG, "BEGIN SYNC");
            try
            {
                WebRequest req = WebRequest.Create("https://10.0.2.2:443" + MySeenWebApi.ApiUsers + MD5Tools.GetMd5Hash(email.ToLower()) + "/" + ((int)MySeenWebApi.SyncModesApiUsers.isUserExists).ToString());
                MySeenWebApi.SyncJsonAnswer answer = MySeenWebApi.GetResponseAnswer((new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd());
                if (answer != null)
                {
                    if (answer.Value == MySeenWebApi.SyncJsonAnswer.Values.UserNotExist)
                    {
                        tv_error.Visibility = ViewStates.Visible;
                        tv_error.Text = MySeenLib.Resource.UserNotExist;
                        Log.Warn(LogTAG, MySeenLib.Resource.UserNotExist);
                        tv_info.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        Log.Warn(LogTAG, MySeenLib.Resource.UserOK);

                        AddData ad = new AddData() { Email = email };
                        if (DatabaseHelper.Get.GetAddDataCount() != 0)
                        {
                            DatabaseHelper.Get.ClearAddData();
                        }
                        DatabaseHelper.Get.Add(ad);
                    }
                }
                req.GetResponse().Close();
                tv_info.Text = "Sync OK";
            }
            catch (Exception e)
            {
                tv_error.Visibility = ViewStates.Visible;
                tv_error.Text = e.Message;
                Log.Warn(LogTAG, e.Message);
                tv_info.Visibility = ViewStates.Gone;
            }
            //Log.Warn(LogTAG, MySeenLib.Resource.ApiError);
            */

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
