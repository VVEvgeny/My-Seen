using System;
using System.Globalization;
using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public enum RoadTypes
    {
        Foot = 1,
        Car = 2,
        Bike = 3
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
    public class RoadsView : Tracks
    {
        public static RoadsView Map(Tracks model)
        {
            if (model == null) return new RoadsView();

            return new RoadsView
            {
                Id = model.Id,
                UserId = model.UserId,
                Date = UmtTime.From(model.Date),
                Distance = model.Distance,
                Name = model.Name,
                Type = model.Type,
                ShareKey = model.ShareKey,
                Coordinates = model.Coordinates
            };
        }

        public string DateText
        {
            get
            {
                if (Date == new DateTime(1980, 3, 3))
                {
                    return UserId;
                }
                return Date.ToShortDateString();
            }
        }
        public string DateFullText
        {
            get
            {
                if (Date == new DateTime(1980, 3, 3))
                {
                    return UserId;
                }
                return Date.ToString(CultureInfo.CurrentCulture);
            }
        }
        public string DistanceText
        {
            get
            {
                //return ((int)(Distance / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1))).ToString() + " " + Resource.Km;
                string distance = (Distance / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1)).ToString(CultureInfo.CurrentCulture);
                char div = CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? '.' : ',';
                if (distance.IndexOf(div) != -1)
                {
                    if (distance.Length > distance.IndexOf(div) + 4) distance = distance.Remove(distance.IndexOf(div) + 4);
                    else if (distance.Length > distance.IndexOf(div) + 3) distance = distance.Remove(distance.IndexOf(div) + 3);
                    else if (distance.Length > distance.IndexOf(div) + 2) distance = distance.Remove(distance.IndexOf(div) + 2);
                }
                return distance += " " + Resource.Km;
            }
        }
    }
}
