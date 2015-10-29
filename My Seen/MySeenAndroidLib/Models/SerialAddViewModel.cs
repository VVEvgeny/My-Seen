using System.Collections.Generic;
using MySeenLib;
using System;
using System.Linq;

namespace MySeenMobileWebViewLib
{
    public class SerialAddViewModel
    {
        public string Name = string.Empty;
        public string Season;
        public string Series;
        public int Genre = Defaults.Genres.GetMaxId();
        public int Rating = Defaults.Ratings.GetMaxId();

        public string Name_Error = string.Empty;
        public bool Unknown_Error = false;
    }
}