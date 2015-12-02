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
        public IEnumerable<TracksView> DataBike { get; set; }
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
        public bool HaveBike
        {
            get
            {
                return DataBike.Any();
            }
        }
        public int CountFoot
        {
            get
            {
                return DataFoot.Count();
            }
        }
        public int CountCar
        {
            get
            {
                return DataCar.Count();
            }
        }
        public int CountBike
        {
            get
            {
                return DataBike.Count();
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
        public double DistanceBike
        {
            get
            {
                return DataBike.Sum(item => item.Distance);
            }
        }
        public string DistanceBikeText
        {
            get
            {
                return ((int)(DistanceBike / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1))).ToString() + " " + Resource.Km;
            }
        }

        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public bool Markers { get; set; }
        public IEnumerable<SelectListItem> YearsList { get; set; }

        public HomeViewModelTracks(string userId, int markersOnRoads, int roadsYear)
        {
            Markers = markersOnRoads == Defaults.MarkersOnRoadsBase.IndexEnabled;
            Type = ((int)TrackTypes.Foot).ToString();
            var ac = new ApplicationDbContext();
            DataFoot = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Foot && (roadsYear == 0 || t.Date.Year==roadsYear)).OrderByDescending(t => t.Date).Select(TracksView.Map);
            DataCar = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Car && (roadsYear == 0 || t.Date.Year == roadsYear)).OrderByDescending(t => t.Date).Select(TracksView.Map);
            DataBike = ac.Tracks.Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Bike && (roadsYear == 0 || t.Date.Year == roadsYear)).OrderByDescending(t => t.Date).Select(TracksView.Map);

            var years = new List<SelectListItem> { 
                new SelectListItem
                {
                    Text = Resource.All,
                    Value = "0",
                    Selected = roadsYear == 0
                } };
            foreach (var elem in ac.Tracks.Where(t => t.UserId == userId).Select(t=>t.Date.Year).Distinct())
            {
                years.Add(new SelectListItem
                {
                    Text = elem.ToString(),
                    Value = elem.ToString(),
                    Selected = roadsYear==elem
                });
            }
            YearsList = years.OrderByDescending(y => y.Text);

            TypeList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Resource.Foot,
                    Value = ((int) TrackTypes.Foot).ToString(),
                    Selected = true
                },
                new SelectListItem {Text = Resource.Car, Value = ((int) TrackTypes.Car).ToString(), Selected = false},
                new SelectListItem {Text = Resource.Bike, Value = ((int) TrackTypes.Bike).ToString(), Selected = false}
            }; 
        }

        public static TrackInfo GetTrack(int id, string userId)
        {
            var ti = new TrackInfo {Path = new List<Location>()};
            var ac = new ApplicationDbContext();
            var track = ac.Tracks.First(t => t.UserId == userId && t.Id == id);
            ti.Name = track.Name;
            ti.Date = track.Date;
            ti.Id = track.Id;
            if (track.Coordinates[track.Coordinates.Length - 1] == ';')
                track.Coordinates = track.Coordinates.Remove(track.Coordinates.Length - 1);
            foreach (var s in track.Coordinates.Split(';'))
            {
                ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
            }
            ti.CallcMinMaxCenter();
            return ti;
        }
        public static ShareTrackInfo GetTrackByKey(string key, int year)
        {
            var list =new List<TrackInfo>();
            var ac = new ApplicationDbContext();

            if (ac.Users.Any(u => u.ShareTracksFootKey != null && u.ShareTracksFootKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksFootKey == key).Id;
                const int type = (int)TrackTypes.Foot;

                foreach (var item in ac.Tracks.Where(t => t.UserId == userId && t.Type == type && (year == 0 || t.Date.Year == year)))
                {
                    if (item.Coordinates[item.Coordinates.Length - 1] == ';')
                        item.Coordinates = item.Coordinates.Remove(item.Coordinates.Length - 1);
                    list.Add(new TrackInfo
                    {
                        Path =
                            new List<Location>(
                                item.Coordinates.Split(';')
                                    .Select(
                                        s =>
                                            new Location
                                            {
                                                Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture),
                                                Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture)
                                            })
                                    .ToList()),
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id
                    });
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksCarKey != null && u.ShareTracksCarKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksCarKey == key).Id;
                const int type = (int)TrackTypes.Car;

                foreach (var item in ac.Tracks.Where(t => t.UserId == userId && t.Type == type && (year == 0 || t.Date.Year == year)))
                {
                    if (item.Coordinates[item.Coordinates.Length - 1] == ';')
                        item.Coordinates = item.Coordinates.Remove(item.Coordinates.Length - 1);
                    list.Add(new TrackInfo
                    {
                        Path =
                            new List<Location>(
                                item.Coordinates.Split(';')
                                    .Select(
                                        s =>
                                            new Location
                                            {
                                                Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture),
                                                Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture)
                                            })
                                    .ToList()),
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id
                    });
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksCarKey != null && u.ShareTracksBikeKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksBikeKey == key).Id;
                const int type = (int)TrackTypes.Bike;

                foreach (var item in ac.Tracks.Where(t => t.UserId == userId && t.Type == type && (year == 0 || t.Date.Year == year)))
                {
                    if (item.Coordinates[item.Coordinates.Length - 1] == ';')
                        item.Coordinates = item.Coordinates.Remove(item.Coordinates.Length - 1);
                    list.Add(new TrackInfo
                    {
                        Path =
                            new List<Location>(
                                item.Coordinates.Split(';')
                                    .Select(
                                        s =>
                                            new Location
                                            {
                                                Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture),
                                                Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture)
                                            })
                                    .ToList()),
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id
                    });
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksAllKey != null && u.ShareTracksAllKey == key))
            {
                var userId = ac.Users.First(u => u.ShareTracksAllKey == key).Id;

                foreach (var item in ac.Tracks.Where(t => t.UserId == userId && (year == 0 || t.Date.Year == year)))
                {
                    if (item.Coordinates[item.Coordinates.Length - 1] == ';')
                        item.Coordinates = item.Coordinates.Remove(item.Coordinates.Length - 1);
                    list.Add(new TrackInfo
                    {
                        Path =
                            new List<Location>(
                                item.Coordinates.Split(';')
                                    .Select(
                                        s =>
                                            new Location
                                            {
                                                Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture),
                                                Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture)
                                            })
                                    .ToList()),
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id
                    });
                }
            }
            else
            {
                foreach (var item in ac.Tracks.Where(t => t.ShareKey == key && (year == 0 || t.Date.Year == year)))
                {
                    if (item.Coordinates[item.Coordinates.Length - 1] == ';')
                        item.Coordinates = item.Coordinates.Remove(item.Coordinates.Length - 1);
                    list.Add(new TrackInfo
                    {
                        Path =new List<Location>(
                                item.Coordinates.Split(';')
                                    .Select(
                                        s =>
                                            new Location
                                            {
                                                Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture),
                                                Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture)
                                            })
                                    .ToList()),
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id
                    });
                }
            }
            var obj = new ShareTrackInfo {Data = list};
            obj.CallcMinMaxCenter();
            return obj;
        }

        public static ShareTrackInfo GetTrackByType(string type)
        {
            var list = new List<TrackInfo>();
            var ac = new ApplicationDbContext();
            var tType = type == "Foot"
                ? (int) TrackTypes.Foot
                : type == "Car" ? (int) TrackTypes.Car : (int) TrackTypes.Bike;

            foreach (var item in ac.Tracks.Where(t => t.Type == tType))
            {
                if (item.Coordinates[item.Coordinates.Length - 1] == ';')
                    item.Coordinates = item.Coordinates.Remove(item.Coordinates.Length - 1);
                list.Add(new TrackInfo
                {
                    Path = new List<Location>(
                        item.Coordinates.Split(';')
                            .Select(
                                s =>
                                    new Location
                                    {
                                        Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture),
                                        Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture)
                                    })
                            .ToList()),
                    Name = item.Name,
                    Date = item.Date,
                    Id = item.Id
                });
            }
            var obj = new ShareTrackInfo { Data = list };
            obj.CallcMinMaxCenter();
            return obj;
        }
        public static string GetTrackNameById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Name;
        }
        public static string GetTrackDateById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Date.ToString(CultureInfo.CurrentCulture);
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
            else if (id.ToLower().Contains("all bike"))
            {
                key = ac.Users.First(t => t.Id == userId).ShareTracksBikeKey;
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
            if (string.IsNullOrEmpty(key)) return "-";
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + key;
        }
        public static string GenerateTrackShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var genkey = string.Empty;
            genkey += id + userId;
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 20; i++)
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
            else if (id.ToLower().Contains("all bike"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksBikeKey = genkey;
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
            else if (id.ToLower().Contains("all bike"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksBikeKey = string.Empty;
            }
            else
            {
                var iid = Convert.ToInt32(id);
                ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey = string.Empty;
            }
            ac.SaveChanges();
        }
        public static void DeleteTrack(string id, string userId)
        {
            var ac = new ApplicationDbContext();

            var iid = Convert.ToInt32(id);
            ac.Tracks.RemoveRange(ac.Tracks.Where(t => t.UserId == userId && t.Id == iid));

            ac.SaveChanges();
        }
    }
}
