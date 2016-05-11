using System.Globalization;
using MySeenWeb.Models.Tables;
using static MySeenLib.Defaults;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews
{
    public class FilmsView : Films
    {
        public string GenreText => Genres.GetById(Genre);

        public string RatingText => Ratings.GetById(Rating);

        public string GenreVal => Genre.ToString();

        public string RatingVal => Rating.ToString();

        public string YearText => Year == 0 ? "" : Year.ToString();

        public string DateSeeText => DateSee.ToString(CultureInfo.CurrentCulture);

        public static FilmsView Map(Films model)
        {
            if (model == null) return new FilmsView();

            return new FilmsView
            {
                Id = model.Id,
                Name = model.Name,
                Year = model.Year,
                UserId = model.UserId,
                DateChange = From(model.DateChange),
                DateSee = From(model.DateSee),
                Genre = model.Genre,
                Rating = model.Rating,
                Shared = model.Shared
            };
        }
    }
}