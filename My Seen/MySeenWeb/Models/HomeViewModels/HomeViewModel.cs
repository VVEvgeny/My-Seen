using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public bool Markers { get; set; }

        public HomeViewModel(int markers)
        {
            Markers = markers == (int) Defaults.EnabledDisabledBase.Indexes.Enabled;
        }
    }
}
