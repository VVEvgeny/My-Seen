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

namespace MySeenAndroid
{
    [Activity(Label = "MySeenAndroid"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , NoHistory = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen"
        )]
    public class MainActivity : Activity
    {
        private static string LogTAG = "MySeenAndroid";
        private States State;
        private List<Films> FilmsList;
        private List<Serials> SerialsList;
        private MyListViewAdapterFilms FilmsAdapter;
        private MyListViewAdapterSerials SerialsAdapter;
        private DatabaseHelper db;
        private ListView listview;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            State = States.Films;

            Log.Warn(LogTAG,"START");

            Button button_test = FindViewById<Button>(Resource.Id.MyButton);
            listview = FindViewById<ListView>(Resource.Id.listView1);

            FilmsList = new List<Films>();
            SerialsList = new List<Serials>();

            FilmsAdapter = new MyListViewAdapterFilms(this, FilmsList);
            SerialsAdapter = new MyListViewAdapterSerials(this, SerialsList);

            listview.Adapter = FilmsAdapter;
            //listview.Adapter = SerialsAdapter;

            db = new DatabaseHelper();
            button_test.Click += delegate 
            {
                if (State == States.Films)
                {
                    db.Add(new Films { Name = "Film test", DateSee = DateTime.Now, DateChange = DateTime.Now, Genre = 0, Rate = 0 });
                }
                else
                {
                    db.Add(new Serials { Name = "Serial Test", DateLast = DateTime.Now, DateBegin = DateTime.Now, Genre = 0, DateChange = DateTime.Now, LastSeason = 1, LastSeries = 2, Rate = 2 });
                }
                LoadFromDatabase();
            };
            LoadFromDatabase();

            Button selectorbutton = FindViewById<Button>(Resource.Id.SelectorButton);
            selectorbutton.Click += delegate
            {
                if(State == States.Films)
                {
                    selectorbutton.Text = "To Films";
                    State = States.Serials;
                    listview.Adapter = SerialsAdapter;
                }
                else
                {
                    selectorbutton.Text = "To Serials";
                    State = States.Films;
                    listview.Adapter = FilmsAdapter;
                }
                LoadFromDatabase();
            };

            Button exitbutton = FindViewById<Button>(Resource.Id.ExitButton);
            exitbutton.Click += delegate
            {
                Finish();
            };
        }

        private void LoadFromDatabase()
        {
            //Log.Warn(LogTAG, "LoadFromDatabase Begin");
            if (State == States.Films)
            {
                Log.Warn(LogTAG, "LoadFromDatabase films count in db=" + db.GetFilmsCount().ToString());

                FilmsList.Clear();

                foreach (Films film in db.GetFilms())
                {
                    //Log.Warn(LogTAG, "Loaded film ID="+film.Id.ToString());
                    //FilmsList.Add(new MyListViewItemFilms(film.Id, film.Name, LibTools.Genres.GetById(film.Genre), film.DateSee.ToShortDateString(), LibTools.Ratings.GetById(film.Rate)));
                    FilmsList.Add(film);
                }
                FilmsAdapter.NotifyDataSetChanged();//В случае если используется базовый то надо пересоздавать...
            }
            else
            {
                Log.Warn(LogTAG, "LoadFromDatabase serials count in db=" + db.GetSerialsCount().ToString());

                SerialsList.Clear();

                foreach (Serials film in db.GetSerials())
                {
                    //Log.Warn(LogTAG, "Loaded film ID="+film.Id.ToString());
                    //FilmsList.Add(new MyListViewItemFilms(film.Id, film.Name, LibTools.Genres.GetById(film.Genre), film.DateSee.ToShortDateString(), LibTools.Ratings.GetById(film.Rate)));
                    SerialsList.Add(film);
                }
                SerialsAdapter.NotifyDataSetChanged();//В случае если используется базовый то надо пересоздавать...
            }
            //Log.Warn(LogTAG, "LoadFromDatabase End");
        }
    }
}

