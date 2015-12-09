using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelBooksMin
    {
        public RatingGenreViewModel RatinngGenre { get; set; }
        public HomeViewModelBooksMin()
        {
            RatinngGenre = new RatingGenreViewModel();
        }
    }
}
