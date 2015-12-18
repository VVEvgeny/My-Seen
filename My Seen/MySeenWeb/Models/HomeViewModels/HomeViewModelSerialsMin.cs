using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelSerialsMin
    {
        public RatingGenreViewModel RatinngGenre { get; set; }
        public HomeViewModelSerialsMin()
        {
            RatinngGenre = new RatingGenreViewModel();
        }
    }
}
