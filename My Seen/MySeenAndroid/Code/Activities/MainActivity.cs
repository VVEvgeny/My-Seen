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
        , NoHistory = false //для второго интента чтобы можно было вернуться назад
        , LaunchMode = LaunchMode.SingleTask
        , ScreenOrientation = ScreenOrientation.Landscape
        , Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen"
        )]
    public class MainActivity : Activity
    {
        private static string LogTAG = "MySeenAndroid";
        private States State;
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

            Button button_add = FindViewById<Button>(Resource.Id.AddButton);
            Button selectorbutton = FindViewById<Button>(Resource.Id.SelectorButton);
            Button exitbutton = FindViewById<Button>(Resource.Id.ExitButton);

            listview = FindViewById<ListView>(Resource.Id.listView1);

            FilmsAdapter = new MyListViewAdapterFilms(this);
            SerialsAdapter = new MyListViewAdapterSerials(this);

            listview.Adapter = FilmsAdapter;
            //listview.Adapter = SerialsAdapter;

            db = new DatabaseHelper();
            LoadFromDatabase();

            button_add.Click += delegate 
            {
                if (State == States.Films)
                {
                    Intent intent = new Intent(this, typeof(FilmsAddActivity));
                    intent.PutExtra(FilmsAddActivity.EXTRA_MODE_KEY, FilmsAddActivity.EXTRA_MODE_VALUE_ADD);
                    StartActivityForResult(intent, 0);
                }
                else
                {
                    Intent intent = new Intent(this, typeof(SerialAddActivity));
                    intent.PutExtra(SerialAddActivity.EXTRA_MODE_KEY, SerialAddActivity.EXTRA_MODE_VALUE_ADD);
                    StartActivityForResult(intent, 0);
                }
                LoadFromDatabase();
            };
            
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

            exitbutton.Click += delegate
            {
                Finish();
            };

            listview.ItemLongClick += listView_ItemLongClick;
        }
        void listView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Log.Warn(LogTAG, "listView_ItemLongClick");

            if(State == States.Films)
            {
                Films item = FilmsAdapter.GetById(e.Position);
                Log.Warn(LogTAG, "fims name="+item.Name+" id="+item.Id.ToString());

                Intent intent = new Intent(this, typeof(FilmsAddActivity));
                intent.PutExtra(FilmsAddActivity.EXTRA_EDIT_ID_KEY, item.Id.ToString());
                StartActivityForResult(intent, 0);
            }
            else
            {
                Serials item = SerialsAdapter.GetById(e.Position);
                Log.Warn(LogTAG, "Serials name=" + item.Name + " id=" + item.Id.ToString());
            }
        }

        private void ReloadListHeaders()
        {
            TableRow tr_films = FindViewById<TableRow>(Resource.Id.tableRow_films);
            TableRow tr_serials = FindViewById<TableRow>(Resource.Id.tableRow_serials);
            if (State == States.Films)
            {
                tr_films.Visibility = ViewStates.Visible;
                tr_serials.Visibility = ViewStates.Gone;
            }
            else
            {
                tr_films.Visibility = ViewStates.Gone;
                tr_serials.Visibility = ViewStates.Visible;
            }
        }
        private void LoadFromDatabase()
        {
            if (State == States.Films)
            {
                Log.Warn(LogTAG, "LoadFromDatabase films count in db=" + db.GetFilmsCount().ToString());
                //FilmsList.Clear();
                FilmsAdapter.list.Clear();
                //foreach (Films film in db.GetFilms())
                {
                    //FilmsList.AddRange(db.GetFilms());
                    FilmsAdapter.list.AddRange(db.GetFilms());
                }
                FilmsAdapter.NotifyDataSetChanged();//В случае если используется базовый то надо пересоздавать...
            }
            else
            {
                Log.Warn(LogTAG, "LoadFromDatabase serials count in db=" + db.GetSerialsCount().ToString());
                //SerialsList.Clear();
                //foreach (Serials film in db.GetSerials())
                {
                    //Log.Warn(LogTAG, "reload list id=" + film.Id.ToString() + " name=" + film.Name);
                    //SerialsList.AddRange(db.GetSerials());
                }
                RunOnUiThread(() =>
                {
                    MyListViewAdapterSerials adapter = (MyListViewAdapterSerials)listview.Adapter;
                    adapter.list.Clear();
                    adapter.list.AddRange(db.GetSerials());
                    adapter.NotifyDataSetChanged();//В случае если используется базовый то надо пересоздавать...
                    
                });
            }
            ReloadListHeaders();
            RunOnUiThread(() =>
                {
                    listview.InvalidateViews();
                    listview.RefreshDrawableState();
                    listview.RequestLayout();
                });

            LinearLayout main_l = FindViewById<LinearLayout>(Resource.Layout.Main);
            //main_l.RefreshDrawableState();
            //main_l.RequestLayout();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)//Не пашет 
        {
            LoadFromDatabase();
        }
    }
}

