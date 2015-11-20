using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class FilmsView : Films
    {
        public static FilmsView Map(Films model)
        {
            if (model == null) return new FilmsView();

            return new FilmsView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                DateChange = UmtTime.From(model.DateChange),
                DateSee = UmtTime.From(model.DateSee),
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.isDeleted
            };
        }
        public string GenreText
        {
            get
            {
                return Defaults.Genres.GetById(Genre);
            }
        }
        public string RatingText
        {
            get
            {
                return Defaults.Ratings.GetById(Rating);
            }
        }
    }
}
