using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;

namespace MySeenWeb.Models
{
    public enum TrackTypes
    {
        Foot =1,
        Car=2
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

    public class TrackInfo: ITrackInfo
    {

    }
    public class TracksViewModel
    {
        public IEnumerable<SelectListItem> typesList { get; set; }
        public string Type;

        public IEnumerable<TracksView> TracksFoot;
        public IEnumerable<TracksView> TracksCar;
        public bool HaveTracksFoot { get; set; }
        public bool HaveTracksCar { get; set; }
        public DateTime Date { get; set; }

        public void Load(string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            TracksFoot = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Foot).OrderByDescending(t => t.Date).Select(TracksView.Map);
            HaveTracksFoot = TracksFoot.Count() > 0;
            TracksCar = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Car).OrderByDescending(t => t.Date).Select(TracksView.Map);
            HaveTracksCar = TracksCar.Count() > 0;

            Type = ((int)TrackTypes.Foot).ToString();
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = Resource.FootBike, Value = ((int)TrackTypes.Foot).ToString(), Selected = true });
            listItems.Add(new SelectListItem { Text = Resource.Car, Value = ((int)TrackTypes.Car).ToString(), Selected = false });
            typesList = listItems;

            Date = DateTime.Now;
        }

        public TrackInfo GetTrack(int id, string userId)
        {
            TrackInfo ti = new TrackInfo();
            ApplicationDbContext ac = new ApplicationDbContext();
            ti.Path = new List<Location>();
            string coordinates = ac.Tracks.Where(t => t.UserId == userId && t.Id == id).OrderByDescending(t => t.Date).Select(t => t.Coordinates).First();
            foreach(string s in coordinates.Split(';'))
            {
                //double.Parse(text, CultureInfo.InvariantCulture);
                ti.Path.Add(new Location { lat = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), lng = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
            }
            ti.CallcMinMaxCenter();
            return ti;
        }
    }
}
