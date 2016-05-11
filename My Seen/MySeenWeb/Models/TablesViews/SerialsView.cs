using System.Globalization;
using MySeenWeb.Models.Tables;
using static MySeenLib.Defaults;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews
{
    public class SerialsView : Serials
    {
        public string GenreText => Genres.GetById(Genre);

        public string RatingText => Ratings.GetById(Rating);

        public string GenreVal => Genre.ToString();

        public string RatingVal => Rating.ToString();

        public string YearText => Year == 0 ? "" : Year.ToString();

        public string SeasonSeries => LastSeason + "-" + LastSeries;

        public string DateLastText => DateLast.ToString(CultureInfo.CurrentCulture);

        public string DateBeginText => DateBegin.ToString(CultureInfo.CurrentCulture);

        public static SerialsView Map(Serials model)
        {
            if (model == null) return new SerialsView();

            return new SerialsView
            {
                Id = model.Id,
                Name = model.Name,
                Year = model.Year,
                UserId = model.UserId,
                DateChange = From(model.DateChange),
                Genre = model.Genre,
                Rating = model.Rating,
                DateBegin = From(model.DateBegin),
                DateLast = From(model.DateLast),
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                Shared = model.Shared
            };
        }
    }
}