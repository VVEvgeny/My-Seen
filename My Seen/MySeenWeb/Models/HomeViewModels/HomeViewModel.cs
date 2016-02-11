using MySeenLib;

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
