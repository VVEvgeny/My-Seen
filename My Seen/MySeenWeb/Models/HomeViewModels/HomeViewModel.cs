using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public bool Markers { get; set; }

        public HomeViewModel(int markers, string selected, string userId, int complex, bool onlyEnded)
        {
            Markers = markers == (int) Defaults.EnabledDisabledBase.Indexes.Enabled;
        }
    }
}
