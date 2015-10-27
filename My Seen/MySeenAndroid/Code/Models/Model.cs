using System.Collections.Generic;
using MySeenLib;
using System;
using System.Linq;

namespace MySeenAndroid
{
    public class HomeViewModel
    {
        public IEnumerable<FilmsView> FilmsList;
        public IEnumerable<Serials> SerialsList;

        public void LoadFilms()
        {
            List<FilmsView> list = new List<FilmsView>();
            foreach(Films f in DatabaseHelper.Get.GetFilms())
            {
                list.Add(FilmsView.Map(f));
            }
            FilmsList = list;
        }
        public void LoadSerials()
        {
            List<SerialsView> list = new List<SerialsView>();
            foreach (Serials f in DatabaseHelper.Get.GetSerials())
            {
                list.Add(SerialsView.Map(f));
            }
            SerialsList = list;
        }
        public string Selected;
        public IEnumerable<Object> selectList { get; set; }
        public void LoadSelectList()
        {
            List<Object> listItems = new List<Object>();
            //foreach (eSelected sel in Enum.GetValues(typeof(eSelected)).Cast<eSelected>())
            foreach (string sel in Defaults.Categories.GetAll())
            {
                listItems.Add(sel);
            }
            selectList = listItems;
        }
    }
}