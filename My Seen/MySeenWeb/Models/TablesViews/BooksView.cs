using MySeenLib;
using MySeenWeb.Models.Database.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class BooksView : Books
    {
        public string GenreText
        {
            get { return Defaults.Genres.GetById(Genre); }
        }

        public string RatingText
        {
            get { return Defaults.Ratings.GetById(Rating); }
        }

        public static BooksView Map(Books model)
        {
            if (model == null) return new BooksView();

            return new BooksView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                DateChange = UmtTime.From(model.DateChange),
                Genre = model.Genre,
                Rating = model.Rating,
                DateRead = UmtTime.From(model.DateRead),
                isDeleted = model.isDeleted,
                Authors = model.Authors
            };
        }
    }
}