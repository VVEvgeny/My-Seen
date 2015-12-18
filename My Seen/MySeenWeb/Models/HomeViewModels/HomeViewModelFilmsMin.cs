using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelFilmsMin
    {
        public RatingGenreViewModel RatinngGenre { get; set; }

        public HomeViewModelFilmsMin()
        {
            RatinngGenre = new RatingGenreViewModel();
        }
    }
}
