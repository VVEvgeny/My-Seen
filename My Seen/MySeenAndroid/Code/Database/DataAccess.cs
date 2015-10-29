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
        public bool isFilmNameExist(string Name)
        {
            return DatabaseHelper.Get.isFilmExist(Name);
        }
        public void AddFilm(string _Name, int _Genre, int _Rating)
        {
            DatabaseHelper.Get.Add(new Films { DateChange = DateTime.Now, DateSee = DateTime.Now, Genre = _Genre, Name = _Name, Rating = _Rating });
        }
        public bool isSerialNameExist(string Name)
        {
            return DatabaseHelper.Get.isSerialExist(Name);
        }
        public void AddSerial(string _Name, int _Season, int _Series, int _Genre, int _Rating)
        {
            DatabaseHelper.Get.Add(new Serials { DateChange = DateTime.Now, Genre = _Genre, Name = _Name, Rating = _Rating, DateBegin = DateTime.Now, DateLast = DateTime.Now, LastSeason = _Season, LastSeries = _Series });
        }

        public void GetFilmById(int id, ref string name, ref int genre, ref int rating)
        {
            Films f = DatabaseHelper.Get.GetFilmById(id);
            if(f!=null)
            {
                name = f.Name;
                genre = f.Genre;
                rating = f.Rating;
            }
        }
    }
}