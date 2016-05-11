using System;
using System.Globalization;
using MySeenLib;
using MySeenWeb.Models.Tables;
using static MySeenLib.CultureInfoTool;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews
{
    public enum RoadTypes
    {
        Foot = 1,
        Car = 2,
        Bike = 3
    }

    public class RoadsView : Tracks
    {
        public string DateText => Date == new DateTime(1980, 3, 3) ? UserId : Date.ToShortDateString();

        public string DateFullText
            => Date == new DateTime(1980, 3, 3) ? UserId : Date.ToString(CultureInfo.CurrentCulture);

        public string DistanceText
        {
            get
            {
                var distance =
                    (Distance/(Culture == Cultures.English ? 1.66 : 1)).ToString(
                       CultureInfo.CurrentCulture);
                var div = Culture == Cultures.English ? '.' : ',';
                if (distance.IndexOf(div) == -1) return distance + " " + Resource.Km;

                if (distance.Length > distance.IndexOf(div) + 4)
                    distance = distance.Remove(distance.IndexOf(div) + 4);
                else if (distance.Length > distance.IndexOf(div) + 3)
                    distance = distance.Remove(distance.IndexOf(div) + 3);
                else if (distance.Length > distance.IndexOf(div) + 2)
                    distance = distance.Remove(distance.IndexOf(div) + 2);
                return distance + " " + Resource.Km;
            }
        }

        public static RoadsView Map(Tracks model)
        {
            if (model == null) return new RoadsView();

            return new RoadsView
            {
                Id = model.Id,
                UserId = model.UserId,
                Date = From(model.Date),
                Distance = model.Distance,
                Name = model.Name,
                Type = model.Type,
                ShareKey = model.ShareKey,
                Coordinates = model.Coordinates
            };
        }
    }
}