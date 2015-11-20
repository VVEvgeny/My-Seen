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
    public class FilmsAddActivity : Activity
    {
        private static string LogTAG = "FilmsAddActivity";
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
            SetContentView(Resource.Layout.FilmsAdd);

            Log.Warn(LogTAG, "START");

            int edited_id=0;
            Films film = new Films();
            if (Intent.GetStringExtra(EXTRA_MODE_KEY) == EXTRA_MODE_VALUE_ADD)//Добавление нового
            {
                Mode = Modes.Add;
            }
            else
            {
                Mode = Modes.Edit;
                edited_id = Convert.ToInt32(Intent.GetStringExtra(EXTRA_EDIT_ID_KEY));
                film = DatabaseHelper.Get.GetFilmById(edited_id);
            }
            Log.Warn(LogTAG, "START Mode=" + Mode.ToString());

            Button button_exit = FindViewById<Button>(Resource.Id.ExitButton_FilmAdd);
            button_exit.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };

            Button button_save = FindViewById<Button>(Resource.Id.SaveButton_FilmAd);
            TextView tv_error = FindViewById<TextView>(Resource.Id.film_add_error);
            tv_error.Visibility = ViewStates.Gone;

            EditText name_text = FindViewById<EditText>(Resource.Id.edittext_film_name);

            button_save.Click += delegate
            {
                if (string.IsNullOrEmpty(name_text.Text))
                {
                    tv_error.Visibility = ViewStates.Visible;
                    tv_error.Text = MySeenLib.Resource.EnterFilmName;
                    return;
                }
                if (Mode == Modes.Add)
                {
                    if (DatabaseHelper.Get.isFilmExist(name_text.Text))
                    {
                        tv_error.Visibility = ViewStates.Visible;
                        tv_error.Text = MySeenLib.Resource.FilmNameAlreadyExists;
                        return;
                    }

                    DatabaseHelper.Get.Add(new Films { Name = name_text.Text, DateChange = UMTTime.To(DateTime.Now), DateSee = UMTTime.To(DateTime.Now), Genre = comboboxgenre.SelectedItemPosition, Rating = comboboxrate.SelectedItemPosition });
                }
                else
                {
                    if (DatabaseHelper.Get.isFilmExistAndNotSame(name_text.Text, edited_id))
                    {
                        tv_error.Visibility = ViewStates.Visible;
                        tv_error.Text = MySeenLib.Resource.FilmNameAlreadyExists;
                        return;
                    }
                    DatabaseHelper.Get.Update(new Films { Id = film.Id, Name = name_text.Text, DateChange = UMTTime.To(DateTime.Now), DateSee = film.DateSee, Genre = comboboxgenre.SelectedItemPosition, Rating = comboboxrate.SelectedItemPosition, Id_R = film.Id_R, isDeleted = film.isDeleted });
                }
                var intent = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, intent);
                Finish();
            };

            comboboxgenre = FindViewById<Spinner>(Resource.Id.spinner_genre);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, Defaults.Genres.GetAll().ToArray());
            comboboxgenre.Adapter = adapter;

            comboboxrate = FindViewById<Spinner>(Resource.Id.spinner_rate);
            ArrayAdapter<String> adapter_rate = new ArrayAdapter<String>(this, Resource.Layout.comboboxitem, Resource.Id.spinnerItem, Defaults.Ratings.GetAll().ToArray());
            comboboxrate.Adapter = adapter_rate;

            if (Mode == Modes.Edit)
            {
                name_text.Text = film.Name;
                comboboxgenre.SetSelection(adapter.GetPosition(Defaults.Genres.GetById(film.Genre)));
                comboboxrate.SetSelection(adapter_rate.GetPosition(Defaults.Ratings.GetById(film.Rating)));
            }
        }
    }
}

