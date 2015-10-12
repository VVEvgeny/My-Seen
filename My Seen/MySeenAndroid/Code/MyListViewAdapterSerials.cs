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
    public class MyListViewAdapterSerials : BaseAdapter
    {
        public List<Serials> list;
        Activity activity;
        TextView txtFilmName;
        TextView txtLastSeasonSeries;
        TextView txtGenre;
        TextView txtDateLast;
        TextView txtDateBegin;
        TextView txtRate;
        public MyListViewAdapterSerials(Activity activity)
        {
            this.activity = activity;
            list = new List<Serials>();
        }

        public override int Count
        {
            get { return list.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            //return ((object)list[position]) as Java.Lang.Object;
            return null;
        }
        public Serials GetById(int position)
        {
            return list[position];
        }

        public override long GetItemId(int position)
        {
            return list[position].Id;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)activity.ApplicationContext.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.column_row_serials, null);

                txtFilmName = (TextView)convertView.FindViewById(Resource.Id.s_name);
                txtLastSeasonSeries = (TextView)convertView.FindViewById(Resource.Id.s_seasonseries);
                txtGenre = (TextView)convertView.FindViewById(Resource.Id.s_genre);
                txtDateLast = (TextView)convertView.FindViewById(Resource.Id.s_datelast);
                txtDateBegin = (TextView)convertView.FindViewById(Resource.Id.s_datebegin);
                txtRate = (TextView)convertView.FindViewById(Resource.Id.s_rate);
            }

            Serials str = list[position];

            txtFilmName.SetText(str.Name, TextView.BufferType.Normal);
            txtLastSeasonSeries.SetText(str.LastSeason.ToString() + "-" + str.LastSeries.ToString(), TextView.BufferType.Normal);
            txtGenre.SetText(LibTools.Genres.GetById(str.Genre), TextView.BufferType.Normal);
            txtDateLast.SetText(str.DateLast.ToShortDateString(), TextView.BufferType.Normal);
            txtDateBegin.SetText(str.DateBegin.ToShortDateString(), TextView.BufferType.Normal);
            txtRate.SetText(LibTools.Ratings.GetById(str.Rate), TextView.BufferType.Normal);

            return convertView;
        }
    }
}

