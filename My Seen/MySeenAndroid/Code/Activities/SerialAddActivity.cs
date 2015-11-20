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
    [Activity(Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen", ScreenOrientation = ScreenOrientation.Landscape)]
    public class SerialAddActivity : Activity
    {
        private static string LogTAG = "SerialAddActivity";
        public const string EXTRA_MODE_KEY = "Mode";
        public const string EXTRA_MODE_VALUE_ADD = "Add";
        public const string EXTRA_MODE_VALUE_EDIT = "Edit";
        public const string EXTRA_EDIT_ID_KEY = "EditId";
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

            Log.Warn(LogTAG, "START");

            int edited_id = 0;
            Serials film = new Serials();
            if (Intent.GetStringExtra(EXTRA_MODE_KEY) == EXTRA_MODE_VALUE_ADD)//Добавление нового
            {
                Mode = Modes.Add;
            }
            else
            {
                Mode = Modes.Edit;
                edited_id = Convert.ToInt32(Intent.GetStringExtra(EXTRA_EDIT_ID_KEY));
                film = DatabaseHelper.Get.GetSerialById(edited_id);
            }

            Log.Warn(LogTAG, "START Mode=" + Intent.GetStringExtra(EXTRA_MODE_KEY));

            Button button_exit = FindViewById<Button>(Resource.Id.ExitButton_SerialAdd);
            button_exit.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };

            Button button_save = FindViewById<Button>(Resource.Id.SaveButton_SerialAdd);
            TextView tv_error = FindViewById<TextView>(Resource.Id.serial_add_error);
            tv_error.Visibility = ViewStates.Gone;

            EditText name_text = FindViewById<EditText>(Resource.Id.edittext_serial_name);
            EditText season = FindViewById<EditText>(Resource.Id.edittext_season);
            EditText series = FindViewById<EditText>(Resource.Id.edittext_series);

            button_save.Click += delegate
            {
                if (string.IsNullOrEmpty(name_text.Text))
                {
                    tv_error.Visibility = ViewStates.Visible;
                    tv_error.Text = MySeenLib.Resource.EnterSerialName;
                    return;
                }
                int iseason = 0;
                try
                {
                    iseason = Convert.ToInt32(season.Text);
                }
                catch
                {
                    iseason = 1;
                }
                int iseries = 0;
                try
                {
                    iseries = Convert.ToInt32(series.Text);
                }
                catch
                {
                    iseries = 1;
                }
                if (Mode == Modes.Add)
                {
                    if (DatabaseHelper.Get.isSerialExist(name_text.Text))
                    {
                        tv_error.Visibility = ViewStates.Visible;
                        tv_error.Text = MySeenLib.Resource.SerialNameAlreadyExists;
                        return;
                    }

                    DatabaseHelper.Get.Add(new Serials
                    {
                        Name = name_text.Text,
                        DateChange = UMTTime.To(DateTime.Now),
                        DateLast = UMTTime.To(DateTime.Now),
                        DateBegin = UMTTime.To(DateTime.Now),
                        LastSeason = iseason,
                        LastSeries = iseries,
                        Genre = comboboxgenre.SelectedItemPosition,
                        Rating = comboboxrate.SelectedItemPosition
                    });
                }
                else
                {
                    if (DatabaseHelper.Get.isSerialExistAndNotSame(name_text.Text, edited_id))
                    {
                        tv_error.Visibility = ViewStates.Visible;
                        tv_error.Text = MySeenLib.Resource.SerialNameAlreadyExists;
                        return;
                    }
                    DatabaseHelper.Get.Update(new Serials
                    {
                        Id = film.Id,
                        Name = name_text.Text,
                        DateChange = UMTTime.To(DateTime.Now),
                        DateLast = ((iseason == film.LastSeason && iseries == film.LastSeries) ? film.DateLast : UMTTime.To(DateTime.Now)),
                        DateBegin = film.DateBegin,
                        LastSeason = iseason,
                        LastSeries = iseries,
                        Genre = comboboxgenre.SelectedItemPosition,
                        Rating = comboboxrate.SelectedItemPosition,
                        Id_R = film.Id_R,
                        isDeleted = film.isDeleted
                    });
                }
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };
            comboboxgenre = FindViewById<Spinner>(Resource.Id.spinner_genre_s);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, Defaults.Genres.GetAll().ToArray());
            comboboxgenre.Adapter = adapter;
            comboboxrate = FindViewById<Spinner>(Resource.Id.spinner_rate_s);
            ArrayAdapter<String> adapter_rate = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, Defaults.Ratings.GetAll().ToArray());
            comboboxrate.Adapter = adapter_rate;

            if (Mode == Modes.Edit)
            {
                name_text.Text = film.Name;
                comboboxgenre.SetSelection(adapter.GetPosition(Defaults.Genres.GetById(film.Genre)));
                comboboxrate.SetSelection(adapter_rate.GetPosition(Defaults.Ratings.GetById(film.Rating)));
                season.Text = film.LastSeason.ToString();
                series.Text = film.LastSeries.ToString();
            }
        }
    }
}

