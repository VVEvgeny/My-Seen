using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public enum TrackTypes
    {
        Foot = 1,
        Car = 2
    }
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location()
        {

        }
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public class TracksView : Tracks
    {
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
        public string DateText
        {
            get
            {
                return Date.ToShortDateString();
            }
        }
        public string DistanceText
        {
            get
            {
                return ((int)(Distance / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1))).ToString() + " " + Resource.Km;
            }
        }
    }
}
