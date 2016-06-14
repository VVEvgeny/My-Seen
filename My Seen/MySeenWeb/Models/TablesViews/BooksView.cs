using System.Globalization;
using MySeenWeb.Models.Tables;
using static MySeenLib.Defaults;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews
{
    public class BooksView : Books
    {
        public string GenreText => Genres.GetById(Genre);

        public string RatingText => Ratings.GetById(Rating);

        public string GenreVal => Genre.ToString();

        public string RatingVal => Rating.ToString();

        public string YearText => Year == 0 ? "" : Year.ToString();

        public string DateReadText => DateRead.ToString(CultureInfo.CurrentCulture).Remove(DateRead.ToString(CultureInfo.CurrentCulture).Length - 3);

        public static BooksView Map(Books model)
        {
            if (model == null) return new BooksView();

            return new BooksView
            {
                Id = model.Id,
                Name = model.Name,
                Year = model.Year,
                UserId = model.UserId,
                DateChange = From(model.DateChange),
                Genre = model.Genre,
                Rating = model.Rating,
                DateRead = From(model.DateRead),
                Authors = model.Authors,
                Shared = model.Shared
            };
        }
    }
}