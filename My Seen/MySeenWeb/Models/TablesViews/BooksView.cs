using System.Globalization;
using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class BooksView : Books
    {
        public static BooksView Map(Books model)
        {
            if (model == null) return new BooksView();

            return new BooksView
            {
                Id = model.Id,
                Name = model.Name,
                Year = model.Year,
                UserId = model.UserId,
                DateChange = UmtTime.From(model.DateChange),
                Genre = model.Genre,
                Rating = model.Rating,
                DateRead = UmtTime.From(model.DateRead),
                isDeleted = model.isDeleted,
                Authors = model.Authors,
                Shared = model.Shared
            };
        }
        public string GenreText
        {
            get { return Defaults.Genres.GetById(Genre); }
        }
        public string RatingText
        {
            get { return Defaults.Ratings.GetById(Rating); }
        }
        public string YearText
        {
            get { return Year == 0 ? "" : Year.ToString(); }
        }

        public string DateReadText
        {
            get { return DateRead.ToString(CultureInfo.CurrentCulture); }
        }
    }
}
