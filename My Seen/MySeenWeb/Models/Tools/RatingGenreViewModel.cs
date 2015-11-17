using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class RatingGenreViewModel
    {
        public string Rating { get; set; }
        public IEnumerable<SelectListItem> RatingList { get; set; }
        public string Genre { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }

        public RatingGenreViewModel()
        {
            Rating = Defaults.Ratings.GetMaxValue();
            Genre = Defaults.Genres.GetMaxValue();

            List<SelectListItem> listItemsRating = new List<SelectListItem>();
            foreach (string sel in Defaults.Ratings.GetAll())
            {
                listItemsRating.Add(new SelectListItem { Text = sel, Value = Defaults.Ratings.GetId(sel).ToString(), Selected = (sel == Rating) });
            }
            RatingList = listItemsRating;

            List<SelectListItem> listItemsGenre = new List<SelectListItem>();
            foreach (string sel in Defaults.Genres.GetAll())
            {
                listItemsGenre.Add(new SelectListItem { Text = sel, Value = Defaults.Genres.GetId(sel).ToString(), Selected = (sel == Genre) });
            }
            GenreList = listItemsGenre;
        }
    }
}
