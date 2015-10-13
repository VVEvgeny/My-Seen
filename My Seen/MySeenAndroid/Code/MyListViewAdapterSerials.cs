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
        private class SerialsViewHolder : Java.Lang.Object
        {
            public TextView txtFilmName { get; set; }
            public TextView txtLastSeasonSeries { get; set; }
            public TextView txtGenre { get; set; }
            public TextView txtDateLast { get; set; }
            public TextView txtDateBegin { get; set; }
            public TextView txtRate { get; set; }
        }
        public List<Serials> list {get;set;}
        private LayoutInflater inflater;
        private Activity activity;
        public MyListViewAdapterSerials(Activity _activity)
        {
            list = new List<Serials>();
            inflater = (LayoutInflater)_activity.ApplicationContext.GetSystemService(Context.LayoutInflaterService);
            activity = _activity;
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
            SerialsViewHolder holder = null;
            var view = convertView;
            if (view != null)holder = view.Tag as SerialsViewHolder;
            if (holder == null)
            {
                holder = new SerialsViewHolder();
                view = activity.LayoutInflater.Inflate(Resource.Layout.column_row_serials, null);
                holder.txtFilmName = (TextView)view.FindViewById(Resource.Id.s_name);
                holder.txtLastSeasonSeries = (TextView)view.FindViewById(Resource.Id.s_seasonseries);
                holder.txtGenre = (TextView)view.FindViewById(Resource.Id.s_genre);
                holder.txtDateLast = (TextView)view.FindViewById(Resource.Id.s_datelast);
                holder.txtDateBegin = (TextView)view.FindViewById(Resource.Id.s_datebegin);
                holder.txtRate = (TextView)view.FindViewById(Resource.Id.s_rate);
                view.Tag = holder;
            }

            Serials str = list[position];

            holder.txtFilmName.SetText(str.Name, TextView.BufferType.Normal);
            holder.txtLastSeasonSeries.SetText(str.LastSeason.ToString() + "-" + str.LastSeries.ToString(), TextView.BufferType.Normal);
            holder.txtGenre.SetText(LibTools.Genres.GetById(str.Genre), TextView.BufferType.Normal);
            holder.txtDateLast.SetText(str.DateLast.ToShortDateString(), TextView.BufferType.Normal);
            holder.txtDateBegin.SetText(str.DateBegin.ToShortDateString(), TextView.BufferType.Normal);
            holder.txtRate.SetText(LibTools.Ratings.GetById(str.Rate), TextView.BufferType.Normal);

            return view;
        }
    }
}

