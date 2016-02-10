using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models.Tools
{
    public class RatingGenreViewModel
    {
        public IEnumerable<SelectListItem> RatingList { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }

        public RatingGenreViewModel()
        {
            RatingList =
                Defaults.Ratings.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Ratings.GetId(sel).ToString(),
                                Selected = sel == Defaults.Ratings.GetMaxValue()
                            })
                    .ToList();

            GenreList =
                Defaults.Genres.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Genres.GetId(sel).ToString(),
                                Selected = sel == Defaults.Genres.GetMaxValue()
                            })
                    .ToList();
        }
    }
}
