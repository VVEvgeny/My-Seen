using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public class HomeViewModelTracks
    {
        public IEnumerable<TracksView> DataFoot { get; set; }
        public IEnumerable<TracksView> DataCar { get; set; }
        public bool HaveFoot
        {
            get
            {
                return DataFoot.Any();
            }
        }
        public bool HaveCar
        {
            get
            {
                return DataCar.Any();
            }
        }
        public int CountFoot
        {
            get
            {
                return DataFoot.Count();
            }
        }

        public double DistanceFoot
        {
            get
            {
                return DataFoot.Sum(item => item.Distance);
            }
        }
        public string DistanceFootText
        {
            get
            {
                return ((int)(DistanceFoot / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1))).ToString() + " " + Resource.Km;
            }
        }
        public int CountCar
        {
            get
            {
                return DataCar.Count();
            }
        }
        public double DistanceCar
        {
            get
            {
                return DataCar.Sum(item => item.Distance);
            }
        }
        public string DistanceCarText
        {
            get
            {
                return ((int)(DistanceCar / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1))).ToString() + " " + Resource.Km;
            }
        }
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }

        public HomeViewModelTracks(string userId)
        {
            Type = ((int)TrackTypes.Foot).ToString();
            var ac = new ApplicationDbContext();
            DataFoot = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Foot).OrderByDescending(t => t.Date).Select(TracksView.Map);
            DataCar = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Car).OrderByDescending(t => t.Date).Select(TracksView.Map);

            var listItemsTypes = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Resource.FootBike,
                    Value = ((int) TrackTypes.Foot).ToString(),
                    Selected = true
                },
                new SelectListItem {Text = Resource.Car, Value = ((int) TrackTypes.Car).ToString(), Selected = false}
            };
            TypeList = listItemsTypes;
        }

        public static TrackInfo GetTrack(int id, string userId)
        {
            var ti = new TrackInfo {Path = new List<Location>()};
            var ac = new ApplicationDbContext();
            var coordinates = ac.Tracks.First(t => t.UserId == userId && t.Id == id).Coordinates;
            foreach (var s in coordinates.Split(';'))
            {
                ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
            }
            ti.CallcMinMaxCenter();
            return ti;
        }
        public static IEnumerable<TrackInfo> GetTrackByKey(string key)
        {
            var list = new List<TrackInfo>();
            var ac = new ApplicationDbContext();

            if (ac.Users.Any(u => u.ShareTracksFootKey != null && u.ShareTracksFootKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksFootKey == key).Id;
                const int type = (int)TrackTypes.Foot;

                foreach (var item in ac.Tracks.Where(t => t.UserId == userId && t.Type == type))
                {
                    var ti = new TrackInfo { Path = new List<Location>() };
                    foreach (var s in item.Coordinates.Split(';'))
                    {
                        ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
                    }
                    ti.CallcMinMaxCenter();
                    list.Add(ti);
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksCarKey != null && u.ShareTracksCarKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksCarKey == key).Id;
                const int type = (int)TrackTypes.Car;
                foreach (var item in ac.Tracks.Where(t => t.UserId == userId && t.Type == type))
                {
                    var ti = new TrackInfo { Path = new List<Location>() };
                    foreach (var s in item.Coordinates.Split(';'))
                    {
                        ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
                    }
                    ti.CallcMinMaxCenter();
                    list.Add(ti);
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksAllKey != null && u.ShareTracksAllKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksAllKey == key).Id;
                foreach (var item in ac.Tracks.Where(t => t.UserId == userId))
                {
                    var ti = new TrackInfo { Path = new List<Location>() };
                    foreach (var s in item.Coordinates.Split(';'))
                    {
                        ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
                    }
                    ti.CallcMinMaxCenter();
                    list.Add(ti);
                }
            }
            else
            {
                foreach (var item in ac.Tracks.Where(t => t.ShareKey == key))
                {
                    var ti = new TrackInfo { Path = new List<Location>() };
                    foreach (var s in item.Coordinates.Split(';'))
                    {
                        ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
                    }
                    ti.CallcMinMaxCenter();
                    list.Add(ti);
                }
            }
            return list;
        }
        public static string GetTrackNameById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Name;
        }
        public static string GetTrackDateById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Date.ToString();
        }
        public static string GetTrackCoordinatesById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Coordinates;
        }
        public static string GetTrackShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            string key;
            if (id.ToLower().Contains("all foot"))
            {
                key = ac.Users.First(t => t.Id == userId).ShareTracksFootKey;
            }
            else if (id.ToLower().Contains("all car"))
            {
                key = ac.Users.First(t => t.Id == userId).ShareTracksCarKey;
            }
            else if (id.ToLower().Contains("all all"))
            {
                key = ac.Users.First(t => t.Id == userId).ShareTracksAllKey;
            }
            else
            {
                var iid = Convert.ToInt32(id);
                key = ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey;
            }
            if (string.IsNullOrEmpty(key)) return key;
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + key;
        }
        public static string GenerateTrackShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var genkey = string.Empty;
            genkey += id + userId;
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 20; i++)
            {
                genkey += r.Next().ToString();
            }
            genkey = Md5Tools.GetMd5Hash(genkey);

            if (id.ToLower().Contains("all foot"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksFootKey = genkey;
            }
            else if (id.ToLower().Contains("all car"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksCarKey = genkey;
            }
            else if (id.ToLower().Contains("all all"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksAllKey = genkey;
            }
            else
            {
                var iid = Convert.ToInt32(id);
                if (ac.Tracks != null) ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey = genkey;
            }
            ac.SaveChanges();
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + genkey;
        }
        public static void DeleteTrackShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();

            if (id.ToLower().Contains("all foot"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksFootKey = string.Empty;
            }
            else if (id.ToLower().Contains("all car"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksCarKey = string.Empty;
            }
            else
            {
                var iid = Convert.ToInt32(id);
                ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey = string.Empty;
            }
            ac.SaveChanges();
        }

    }
}
