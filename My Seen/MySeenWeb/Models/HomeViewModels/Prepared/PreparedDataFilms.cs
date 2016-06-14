using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataFilms
    {
        public IEnumerable<SelectListItem> GenreList { get; } =
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

        public IEnumerable<SelectListItem> RatingList { get; } =
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

        public int Year { get; } = DateTime.Now.Year;

        public string DateTimeNow { get; } =
            DateTime.Now.ToString(CultureInfo.CurrentCulture)
                .Remove(DateTime.Now.ToString(CultureInfo.CurrentCulture).Length - 3);
    }
}