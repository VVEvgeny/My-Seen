using System.Collections.Generic;
using MySeenLib;
using System;
using System.Linq;

namespace MySeenMobileWebViewLib
{
    public class SerialAddViewModel
    {
        public string Name = string.Empty;
        public int Season = 1;
        public int Series = 1;
        public int Genre = Defaults.Genres.GetMaxId();
        public int Rating = Defaults.Ratings.GetMaxId();

        public string Error = string.Empty;
    }
}