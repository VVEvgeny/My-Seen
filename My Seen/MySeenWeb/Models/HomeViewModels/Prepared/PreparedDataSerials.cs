using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using static MySeenLib.Defaults;

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
                Ratings.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Ratings.GetId(sel).ToString(),
                                Selected = Ratings.GetId(sel) == 0
                            })
                    .ToList();

            GenreList =
                Genres.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Genres.GetId(sel).ToString(),
                                Selected = Genres.GetId(sel) == 0
                            })
                    .ToList();
            Year = DateTime.Now.Year;
            DateTimeNow = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }
    }
}