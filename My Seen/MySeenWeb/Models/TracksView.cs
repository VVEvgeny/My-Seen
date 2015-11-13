using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public enum TrackTypes
    {
        Foot = 1,
        Car = 2
    }
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }

        public Location()
        {

        }
        public Location(double _lat, double _lng)
        {
            lat = _lat;
            lng = _lng;
        }
    }
    public class ITrackInfo
    {
        public List<Location> Path { get; set; }
        public Location Min { get; set; }
        public Location Center { get; set; }
        public Location Max { get; set; }

        public void CallcMinMaxCenter()
        {
            double minX = Path[0].lat;
            double maxX = Path[0].lat;

            double minY = Path[0].lng;
            double maxY = Path[0].lng;

            foreach (Location l in Path)
            {
                if (minX > l.lat) minX = l.lat;
                if (maxX < l.lat) maxX = l.lat;
                if (maxY < l.lng) maxY = l.lng;
                if (minY > l.lng) minY = l.lng;
            }
            Max = new Location(maxX, maxY);
            Min = new Location(minX, minY);
            Center = new Location((maxX + minX) / 2, (maxY + minY) / 2);
        }
    }

    public class TrackInfo : ITrackInfo
    {

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
                return ((int)(Distance / (CultureInfoTool.GetCulture()==CultureInfoTool.Cultures.English ? 1.66: 1))).ToString() +" "+ Resource.Km;
            }
        }
    }
}
