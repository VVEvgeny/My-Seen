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
    public class FilmsAddActivity : Activity
    {
        private static string LogTAG = "FilmsAddActivity";
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
            SetContentView(Resource.Layout.FilmsAdd);

            //Log.Warn(LogTAG, "START");
            Log.Warn(LogTAG, "START Mode=" + Intent.GetStringExtra(EXTRA_MODE_KEY));

            Button button_exit = FindViewById<Button>(Resource.Id.ExitButton_FilmAdd);
            button_exit.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };

            Button button_save = FindViewById<Button>(Resource.Id.SaveButton_FilmAd);
            button_save.Click += delegate
            {

                DatabaseHelper db = new DatabaseHelper();
                EditText name_text=FindViewById<EditText>(Resource.Id.edittext_film_name);

                db.Add(new Films { Name = name_text.Text, DateChange = DateTime.Now, DateSee = DateTime.Now, Genre = comboboxgenre.SelectedItemPosition, Rate = comboboxrate.SelectedItemPosition });

                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("greeting", "Hello from the Second Activity!");
                SetResult(Result.Ok, intent);
                //SetResult(Result.Ok);
                Finish();
            };

            if (Intent.GetStringExtra(EXTRA_MODE_KEY) == EXTRA_MODE_VALUE_ADD)//Добавление нового
            {
                Mode = Modes.Add;
            }
            else
            {
                Mode = Modes.Edit;
            }
            comboboxgenre = FindViewById<Spinner>(Resource.Id.spinner_genre);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, LibTools.Genres.GetAll().ToArray());
            comboboxgenre.Adapter = adapter;

            comboboxrate = FindViewById<Spinner>(Resource.Id.spinner_rate);
            ArrayAdapter<String> adapter_rate = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, LibTools.Ratings.GetAll().ToArray());
            comboboxrate.Adapter = adapter_rate;
        }
    }
}

