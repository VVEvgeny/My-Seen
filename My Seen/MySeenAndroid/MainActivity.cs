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

namespace MySeenAndroid
{
    [Activity(Label = "MySeenAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static string LogTAG = "MySeenAndroid";

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

            Log.Warn(LogTAG,"START");

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            listview = FindViewById<ListView>(Resource.Id.listView1);

            FilmsList = new List<Films>();
            SerialsList = new List<Serials>();

            FilmsAdapter = new MyListViewAdapterFilms(this, FilmsList);
            SerialsAdapter = new MyListViewAdapterSerials(this, SerialsList);

            listview.Adapter = FilmsAdapter;
            //listview.Adapter = SerialsAdapter;

            db = new DatabaseHelper();
            button.Click += delegate 
            {
                db.Add(new Films { Name = "test", DateSee = DateTime.Now, DateChange = DateTime.Now, Genre = 0, Rate = 0 });
                LoadFromDatabase();
            };
            LoadFromDatabase();
        }

        private void LoadFromDatabase()
        {
            Log.Warn(LogTAG, "LoadFromDatabase Begin");

            Log.Warn(LogTAG, "LoadFromDatabase films count in db=" + db.GetFilmsCount().ToString());
            
            FilmsList.Clear();

            foreach(Films film in db.GetFilms())
            {
                Log.Warn(LogTAG, "Loaded film ID="+film.Id.ToString());
                //FilmsList.Add(new MyListViewItemFilms(film.Id, film.Name, LibTools.Genres.GetById(film.Genre), film.DateSee.ToShortDateString(), LibTools.Ratings.GetById(film.Rate)));
                FilmsList.Add(film);
            }
            FilmsAdapter.NotifyDataSetChanged();//В случае если используется базовый то надо пересоздавать...
            Log.Warn(LogTAG, "LoadFromDatabase End");
        }
    }
}

