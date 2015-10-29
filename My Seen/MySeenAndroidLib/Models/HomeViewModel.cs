using System.Collections.Generic;
using MySeenLib;
using System;
using System.Linq;

namespace MySeenMobileWebViewLib
{
    public class HomeViewModel
    {
        public string Selected;
        public HomeViewModel()
        {
            Selected = Defaults.Categories.GetById(Defaults.CategoryBase.SerialIndex);
        }
        public IEnumerable<FilmsView> FilmsList;
        public IEnumerable<SerialsView> SerialsList;
    }
}