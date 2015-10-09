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
using Android.Content.PM;
using MySeenLib;

namespace MySeenAndroid
{
    [Activity(Label = "MySeenAndroid"
    , MainLauncher = false
    , Icon = "@drawable/icon"
    , NoHistory = true
    , LaunchMode = LaunchMode.SingleTask
    , ScreenOrientation = ScreenOrientation.Landscape
    , Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen"
    )]
    public class SerialAddActivity : Activity
    {
        private static string LogTAG = "SerialAddActivity";
        public const string EXTRA_MODE_KEY = "Mode";
        public const string EXTRA_MODE_VALUE_ADD = "Add";
        public const string EXTRA_MODE_VALUE_EDIT = "Edit";
        private enum Modes
        {
            Add,
            Edit
        };
        private Modes Mode;
        private Spinner comboboxgenre;
        private Spinner comboboxrate;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SerialsAdd);

            //Log.Warn(LogTAG, "START");
            Log.Warn(LogTAG, "START Mode=" + Intent.GetStringExtra(EXTRA_MODE_KEY));

            Button button_exit = FindViewById<Button>(Resource.Id.ExitButton_SerialAdd);
            Log.Warn(LogTAG, "000000000");
            button_exit.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };
            Log.Warn(LogTAG, "111111111111111");
            Button button_save = FindViewById<Button>(Resource.Id.SaveButton_SerialAdd);
            button_save.Click += delegate
            {

                DatabaseHelper db = new DatabaseHelper();
                EditText name_text = FindViewById<EditText>(Resource.Id.edittext_serial_name);
                EditText season = FindViewById<EditText>(Resource.Id.edittext_season);
                EditText series = FindViewById<EditText>(Resource.Id.edittext_series);

                int iseason=0;
                try
                {
                    iseason=Convert.ToInt32(season.Text);
                }
                catch
                {
                    iseason=1;
                }
                int iseries=0;
                try
                {
                    iseries=Convert.ToInt32(series.Text);
                }
                catch
                {
                    iseries=1;
                }

                db.Add(new Serials { Name = name_text.Text, DateChange = DateTime.Now, DateLast = DateTime.Now, DateBegin = DateTime.Now, LastSeason = iseason, LastSeries = iseries, Genre = comboboxgenre.SelectedItemPosition, Rate = comboboxrate.SelectedItemPosition });

                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("greeting", "Hello from the Second Activity!");
                SetResult(Result.Ok, intent);
                //SetResult(Result.Ok);
                Finish();
            };
            Log.Warn(LogTAG, "2222222222222222");
            if (Intent.GetStringExtra(EXTRA_MODE_KEY) == EXTRA_MODE_VALUE_ADD)//Добавление нового
            {
                Mode = Modes.Add;
                EditText season = FindViewById<EditText>(Resource.Id.edittext_season);
                season.Text = "1";
                EditText series = FindViewById<EditText>(Resource.Id.edittext_series);
                series.Text = "1";
            }
            else
            {
                Mode = Modes.Edit;
            }
            Log.Warn(LogTAG, "33333333333");
            comboboxgenre = FindViewById<Spinner>(Resource.Id.spinner_genre_s);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, LibTools.Genres.GetAll().ToArray());
            comboboxgenre.Adapter = adapter;
            Log.Warn(LogTAG, "44444444444444");
            comboboxrate = FindViewById<Spinner>(Resource.Id.spinner_rate_s);
            ArrayAdapter<String> adapter_rate = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, LibTools.Ratings.GetAll().ToArray());
            comboboxrate.Adapter = adapter_rate;
            Log.Warn(LogTAG, "55555555555555");
        }
    }
}

