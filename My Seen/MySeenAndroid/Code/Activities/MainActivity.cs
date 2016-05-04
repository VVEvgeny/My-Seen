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
using System.Globalization;

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
    public class MainActivity : Activity
    {
        private static string LogTAG = "MySeenAndroid";
        private States State;
        private MyListViewAdapterFilms FilmsAdapter;
        private MyListViewAdapterSerials SerialsAdapter;
        private ListView listview;
        private Spinner comboboxSelector;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            State = States.Films;
            
             /*
             //ПРОВАЛЬНЫЕ ПОПЫТКИ ДОСТУЧАТЬСЯ до русского языка в библиотеке
                         
            {
                StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().PermitAll().Build();
                StrictMode.SetThreadPolicy(policy);
            }

            MySeenLib.Resource.Culture = new System.Globalization.CultureInfo("ru-RU");
            CultureInfoTool.SetCulture("ru-RU");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru");

            Android.Content.Res.Configuration conf = this.Resources.Configuration;
            conf.Locale = new Java.Util.Locale("ru");
            DisplayMetrics dm = this.Resources.DisplayMetrics;
            this.Resources.UpdateConfiguration(conf, dm);

            var languageIso = "ru";
            var locale = new Java.Util.Locale(languageIso);
            Java.Util.Locale.Default = locale;
            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
            SetContentView(Resource.Layout.Main);

            Resources.Configuration.Locale = locale;

            //Log.Warn(LogTAG, "test000=" + MySeenLib.Resource.Culture.ToString());
            Log.Warn(LogTAG, "test1=" + MySeenLib.Resource.CreatePasswordText1);
            Log.Warn(LogTAG, "test2=" + MySeenLib.Resource.ResourceManager.GetString(MySeenLib.Resource.CreatePasswordText1, new System.Globalization.CultureInfo("ru-RU")));
            MySeenLib.Resource.Culture = new System.Globalization.CultureInfo("ru-RU");
            Log.Warn(LogTAG, "test000=" + MySeenLib.Resource.Culture.ToString());
            Log.Warn(LogTAG, "test3=" + MySeenLib.Resource.CreatePasswordText1);
            MySeenLib.Resource.Culture = new System.Globalization.CultureInfo(MySeenLib.CultureInfoTool.Cultures.Russian);
            Log.Warn(LogTAG, "test000=" + MySeenLib.Resource.Culture.ToString());
            Log.Warn(LogTAG, "test4=" + MySeenLib.Resource.CreatePasswordText1);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MySeenLib.CultureInfoTool.Cultures.Russian);
            Log.Warn(LogTAG, "test5=" + MySeenLib.Resource.CreatePasswordText1);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(MySeenLib.CultureInfoTool.Cultures.Russian);
            Log.Warn(LogTAG, "test000=" + MySeenLib.Resource.Culture.ToString());
            Log.Warn(LogTAG, "test6=" + MySeenLib.Resource.CreatePasswordText1);
            Log.Warn(LogTAG, "test7=" + MySeenLib.Resource.ResourceManager.GetString(MySeenLib.Resource.CreatePasswordText1, new System.Globalization.CultureInfo(MySeenLib.CultureInfoTool.Cultures.Russian)));
            Log.Warn(LogTAG, "test000=" + MySeenLib.Resource.Culture.ToString());

            System.Resources.ResourceManager resmgr = new System.Resources.ResourceManager("MySeenLib.Resource", typeof(MySeenLib.Resource).Assembly);
            CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            Log.Warn(LogTAG, "test10=" + resmgr.GetString(MySeenLib.Resource.CreatePasswordText1, ci));
            ci = new System.Globalization.CultureInfo("ru");
            Log.Warn(LogTAG, "test11=" + resmgr.GetString(MySeenLib.Resource.CreatePasswordText1, ci));
            */

            Log.Warn(LogTAG,"START");

            Button button_add = FindViewById<Button>(Resource.Id.AddButton);
            Button exitbutton = FindViewById<Button>(Resource.Id.ExitButton);
            Button button_config = FindViewById<Button>(Resource.Id.ConfigButton);

            listview = FindViewById<ListView>(Resource.Id.listView1);

            FilmsAdapter = new MyListViewAdapterFilms(this);
            SerialsAdapter = new MyListViewAdapterSerials(this);

            listview.Adapter = FilmsAdapter;

            //LoadFromDatabase();

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
            };

            button_config.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ConfigActivity));
                StartActivityForResult(intent,0);
            };

            exitbutton.Click += delegate
            {
                Finish();
            };

            listview.ItemLongClick += listView_ItemLongClick;

            comboboxSelector = FindViewById<Spinner>(Resource.Id.selectorSpinner);
            ArrayAdapter<String> adapter_selector = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, Defaults.Categories.GetAll().ToArray());
            comboboxSelector.Adapter = adapter_selector;
            comboboxSelector.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Log.Warn(LogTAG, "spinner item selected item=" + e.Position.ToString());
            Spinner spinner = (Spinner)sender;
            if ((int)Defaults.CategoryBase.Indexes.Films == e.Position)
            {
                State = States.Films;
                listview.Adapter = FilmsAdapter;
            }
            else
            {
                State = States.Serials;
                listview.Adapter = SerialsAdapter;
            }
            LoadFromDatabase();
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

                Intent intent = new Intent(this, typeof(SerialAddActivity));
                intent.PutExtra(SerialAddActivity.EXTRA_EDIT_ID_KEY, item.Id.ToString());
                StartActivityForResult(intent, 0);
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
                Log.Warn(LogTAG, "LoadFromDatabase films count in db=" + DatabaseHelper.Get.GetFilmsCount().ToString());
                FilmsAdapter.list.Clear();

                foreach (Films f in DatabaseHelper.Get.GetFilms())
                {
                    Log.Warn(LogTAG, "film=" + f.Name + " isdeleted=" + (f.isDeleted == null ? "null" : (f.isDeleted.Value ? "+" : "-")));
                }

                FilmsAdapter.list.AddRange(DatabaseHelper.Get.GetFilms());
                FilmsAdapter.NotifyDataSetChanged();
            }
            else
            {
                Log.Warn(LogTAG, "LoadFromDatabase serials count in db=" + DatabaseHelper.Get.GetSerialsCount().ToString());
                SerialsAdapter.list.Clear();
                SerialsAdapter.list.AddRange(DatabaseHelper.Get.GetSerials());
                SerialsAdapter.NotifyDataSetChanged();
            }
            ReloadListHeaders();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)//Не пашет 
        {
            Log.Warn(LogTAG, "OnActivityResult");
            LoadFromDatabase();
        }
    }
}

