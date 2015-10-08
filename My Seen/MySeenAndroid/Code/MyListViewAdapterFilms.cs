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
    public class MyListViewAdapterFilms : BaseAdapter
    {
        public List<Films> list;
        Activity activity;
        TextView txtFilmName;
        TextView txtGenre;
        TextView txtDateSee;
        TextView txtRate;
        public MyListViewAdapterFilms(Activity activity, List<Films> _list)
        {
            this.activity = activity;
            list = _list;
        }

        public override int Count
        {
            get { return list.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)activity.ApplicationContext.GetSystemService(Context.LayoutInflaterService);

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.colmn_row, null);

                txtFilmName = (TextView)convertView.FindViewById(Resource.Id.name);
                txtGenre = (TextView)convertView.FindViewById(Resource.Id.gender);
                txtDateSee = (TextView)convertView.FindViewById(Resource.Id.age);
                txtRate = (TextView)convertView.FindViewById(Resource.Id.status);
            }

            Films str = list[position];

            txtFilmName.SetText(str.Name, TextView.BufferType.Normal);
            txtGenre.SetText(LibTools.Genres.GetById(str.Genre), TextView.BufferType.Normal);
            txtDateSee.SetText(str.DateSee.ToShortDateString(), TextView.BufferType.Normal);
            txtRate.SetText(LibTools.Ratings.GetById(str.Rate), TextView.BufferType.Normal);

            return convertView;
        }
    }
}

