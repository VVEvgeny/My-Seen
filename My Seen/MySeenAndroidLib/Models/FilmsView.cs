using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySeenLib;

namespace MySeenAndroidLib
{
    public class FilmsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateSee { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
        public DateTime DateChange { get; set; }

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