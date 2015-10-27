using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySeenLib;

namespace MySeenAndroid
{
    public class FilmsView : Films
    {
        public static FilmsView Map(Films model)
        {
            if (model == null) return new FilmsView();

            return new FilmsView
            {
                Id = model.Id,
                Name = model.Name,
                DateChange = UMTTime.From(model.DateChange),
                DateSee = UMTTime.From(model.DateSee),
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.isDeleted
            };
        }
        public string GenreText
        {
            get
            {
                return Defaults.Genres.GetById(Genre);
            }
        }
        public string RatingText
        {
            get
            {
                return Defaults.Ratings.GetById(Rating);
            }
        }
        public string DateSee10
        {
            get
            {
                return ((DateSee.Day < 10 ? "0" + DateSee.Day.ToString() : DateSee.Day.ToString()) +"."
                    + (DateSee.Month < 10 ? "0" + DateSee.Month.ToString() : DateSee.Month.ToString()) + "."
                    + DateSee.Year.ToString());
            }
        }
    }
}