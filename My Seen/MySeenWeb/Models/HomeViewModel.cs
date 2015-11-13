using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public PaginationViewModel Pages;
        public string Selected { get; set; }

        public bool PageFilms
        {
            get
            {
                return Selected == Defaults.CategoryBase.FilmIndex.ToString();
            }
        }
        public bool PageSerials
        {
            get
            {
                return Selected == Defaults.CategoryBase.SerialIndex.ToString();
            }
        }
        public bool PageBooks
        {
            get
            {
                return Selected == Defaults.CategoryBase.BookIndex.ToString();
            }
        }
        public bool PageTracks
        {
            get
            {
                return Selected == Defaults.CategoryBase.TrackIndex.ToString();
            }
        }

        public IEnumerable<SelectListItem> selectList { get; set; }
        public string Rating { get; set; }
        public IEnumerable<SelectListItem> ratingList { get; set; }
        public string Genre { get; set; }
        public IEnumerable<SelectListItem> genreList { get; set; }
        public string Type { get; set; }
        public IEnumerable<SelectListItem> typesList { get; set; }

        public HomeViewModel()
        {
            Selected = Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex);
            Rating = Defaults.Ratings.GetMaxValue();
            Genre = Defaults.Genres.GetMaxValue();
            Type = ((int)TrackTypes.Foot).ToString();
        }
        public HomeViewModelFilms Films;
        public HomeViewModelSerials Serials;
        public HomeViewModelBooks Books;
        public HomeViewModelTracks Tracks;

        public void Load(string userId, int page, int countInPage)
        {
            LoadSelectList();
            if (Selected == Defaults.CategoryBase.SerialIndex.ToString()) Serials = new HomeViewModelSerials(userId, page, countInPage, ref Pages);
            else if (Selected == Defaults.CategoryBase.BookIndex.ToString()) Books = new HomeViewModelBooks(userId, page, countInPage, ref Pages);
            else if (Selected == Defaults.CategoryBase.TrackIndex.ToString()) Tracks = new HomeViewModelTracks(userId);
            else Films = new HomeViewModelFilms(userId, page, countInPage, ref Pages);//По умолчанию
        }
        private void LoadSelectList()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (string sel in Defaults.Categories.GetAll())
            {
                listItems.Add(new SelectListItem { Text = sel, Value = Defaults.Categories.GetId(sel).ToString(), Selected = (Defaults.Categories.GetId(sel).ToString() == Selected) });
            }
            selectList = listItems;

            List<SelectListItem> listItemsRating = new List<SelectListItem>();
            foreach (string sel in Defaults.Ratings.GetAll())
            {
                listItemsRating.Add(new SelectListItem { Text = sel, Value = Defaults.Ratings.GetId(sel).ToString(), Selected = (sel == Rating) });
            }
            ratingList = listItemsRating;

            List<SelectListItem> listItemsGenre = new List<SelectListItem>();
            foreach (string sel in Defaults.Genres.GetAll())
            {
                listItemsGenre.Add(new SelectListItem { Text = sel, Value = Defaults.Genres.GetId(sel).ToString(), Selected = (sel == Genre) });
            }
            genreList = listItemsGenre;

            List<SelectListItem> listItemsTypes = new List<SelectListItem>();
            listItemsTypes.Add(new SelectListItem { Text = Resource.FootBike, Value = ((int)TrackTypes.Foot).ToString(), Selected = true });
            listItemsTypes.Add(new SelectListItem { Text = Resource.Car, Value = ((int)TrackTypes.Car).ToString(), Selected = false });
            typesList = listItemsTypes;
        }
    }
}
