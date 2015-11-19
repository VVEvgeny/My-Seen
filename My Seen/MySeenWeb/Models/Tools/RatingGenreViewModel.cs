using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models.Tools
{
    public class RatingGenreViewModel
    {
        public RatingGenreViewModel()
        {
            Rating = Defaults.Ratings.GetMaxValue();
            Genre = Defaults.Genres.GetMaxValue();

            var listItemsRating =
                Defaults.Ratings.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Ratings.GetId(sel).ToString(),
                                Selected = sel == Rating
                            })
                    .ToList();
            RatingList = listItemsRating;

            var listItemsGenre =
                Defaults.Genres.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Genres.GetId(sel).ToString(),
                                Selected = sel == Genre
                            })
                    .ToList();
            GenreList = listItemsGenre;
        }

        public string Rating { get; set; }
        public IEnumerable<SelectListItem> RatingList { get; set; }
        public string Genre { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }
    }
}