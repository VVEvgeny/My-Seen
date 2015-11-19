using MySeenLib;
using MySeenWeb.Models.Database.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class TracksView : Tracks
    {
        public string DateText
        {
            get { return Date.ToShortDateString(); }
        }

        public string DistanceText
        {
            get
            {
                return (int) (Distance/(CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1)) +
                       " " + Resource.Km;
            }
        }

        public static TracksView Map(Tracks model)
        {
            if (model == null) return new TracksView();

            return new TracksView
            {
                Id = model.Id,
                Date = model.Date,
                Distance = model.Distance,
                Name = model.Name,
                Type = model.Type
            };
        }
    }
}