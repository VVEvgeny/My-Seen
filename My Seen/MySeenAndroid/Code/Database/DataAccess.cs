using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySeenMobileWebViewLib;

namespace MySeenAndroid.Code.Database
{
    class DataAccess : IDataAccess
    {
        public IEnumerable<FilmsView> LoadFilms()
        {
            List<FilmsView> li= new List<FilmsView>();
            foreach(Films f in DatabaseHelper.Get.GetFilms())
            {
                li.Add(new FilmsView { Id = f.Id, DateChange = f.DateChange, DateSee = f.DateSee, Genre = f.Genre, Name = f.Name, Rating = f.Rating });
            }
            return li;
        }
        public IEnumerable<SerialsView> LoadSerials()
        {
            List<SerialsView> li = new List<SerialsView>();
            foreach (Serials f in DatabaseHelper.Get.GetSerials())
            {
                li.Add(new SerialsView { Id = f.Id, DateChange = f.DateChange, Genre = f.Genre, Name = f.Name, Rating = f.Rating, DateBegin = f.DateBegin, DateLast = f.DateLast, LastSeason = f.LastSeason, LastSeries = f.LastSeries });
            }
            return li;
        }
    }
}