using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataSerials
    {
        public IEnumerable<SelectListItem> GenreList { get; set; }
        public IEnumerable<SelectListItem> RatingList { get; set; }
        public int Year { get; set; }
        public string DateTimeNow { get; set; }

        public PreparedDataSerials()
        {
            RatingList =
                Defaults.Ratings.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Ratings.GetId(sel).ToString(),
                                Selected = Defaults.Ratings.GetId(sel) == 0
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
                                Selected = Defaults.Genres.GetId(sel) == 0
                            })
                    .ToList();
            Year = DateTime.Now.Year;
            DateTimeNow = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }
    }
}