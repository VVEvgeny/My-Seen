using System.Collections.Generic;
using MySeenLib;
using System;
using System.Linq;

namespace MySeenMobileWebViewLib
{
    public class HomeViewModel
    {
        public bool isFilm = true;
        public IEnumerable<FilmsView> FilmsList;
        public IEnumerable<SerialsView> SerialsList;
    }
}