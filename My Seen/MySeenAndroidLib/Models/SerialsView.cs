using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySeenLib;

namespace MySeenAndroidLib
{
    public class SerialsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LastSeason { get; set; }
        public int LastSeries { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateLast { get; set; }
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
    }
}
