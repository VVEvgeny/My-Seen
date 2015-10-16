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
        private class FilmsViewHolder : Java.Lang.Object
        {
            public TextView txtFilmName { get; set; }
            public TextView txtLastSeasonSeries { get; set; }
            public TextView txtGenre { get; set; }
            public TextView txtDateSee { get; set; }
            public TextView txtRate { get; set; }
        }

        public List<Films> list;
        private Activity activity;
        private LayoutInflater inflater;

        public MyListViewAdapterFilms(Activity _activity)
        {
            list = new List<Films>();
            this.activity = _activity;
            inflater = (LayoutInflater)_activity.ApplicationContext.GetSystemService(Context.LayoutInflaterService);
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
        public Films GetById(int position)
        {
            return list[position];
        }

        public override long GetItemId(int position)
        {
            return list[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            FilmsViewHolder holder = null;
            var view = convertView;
            if (view != null) holder = view.Tag as FilmsViewHolder;
            if (holder == null)
            {
                holder = new FilmsViewHolder();
                view = activity.LayoutInflater.Inflate(Resource.Layout.column_row_films, null);
                holder.txtFilmName = (TextView)view.FindViewById(Resource.Id.f_name);
                holder.txtGenre = (TextView)view.FindViewById(Resource.Id.f_genre);
                holder.txtDateSee = (TextView)view.FindViewById(Resource.Id.f_datesee);
                holder.txtRate = (TextView)view.FindViewById(Resource.Id.f_rate);
                view.Tag = holder;
            }

            Films str = list[position];

            holder.txtFilmName.SetText(str.Name, TextView.BufferType.Normal);
            holder.txtGenre.SetText(Defaults.Genres.GetById(str.Genre), TextView.BufferType.Normal);
            holder.txtDateSee.SetText(str.DateSee.ToShortDateString(), TextView.BufferType.Normal);
            holder.txtRate.SetText(Defaults.Ratings.GetById(str.Rate), TextView.BufferType.Normal);

            return view;
        }
    }
}

