using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public class HomeViewModelRoads
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
        
        public bool Markers { get; set; }
        public IEnumerable<SelectListItem> YearsList { get; set; }

        public HomeViewModelRoads(string userId, int markersOnRoads, int roadsYear)
        {
            Markers = markersOnRoads == (int)Defaults.EnabledDisabledBase.Indexes.Enabled;
            var ac = new ApplicationDbContext();

            DataFoot = ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Foot && (roadsYear == 0 || t.Date.Year == roadsYear)).OrderByDescending(t => t.Date).Select(TracksView.Map).ToList();
            if (DataFoot.Any())
            {
                var dataFootAll = new List<TracksView>
                {
                    new TracksView
                    {
                        Id = -(int) TrackTypes.Foot,
                        Name = Resource.All,
                        UserId = DataFoot.Count().ToString(),
                        Date = new DateTime(1980, 3, 3),
                        Distance = DataFoot.Sum(item => item.Distance),
                        ShareKey = "+"
                    }
                };
                DataFoot = dataFootAll.Concat(DataFoot);
            }

            DataCar = ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Car && (roadsYear == 0 || t.Date.Year == roadsYear)).OrderByDescending(t => t.Date).Select(TracksView.Map).ToList();
            if (DataCar.Any())
            {
                var dataCarAll = new List<TracksView>
                {
                    new TracksView
                    {
                        Id = -(int) TrackTypes.Car,
                        Name = Resource.All,
                        UserId = DataCar.Count().ToString(),
                        Date = new DateTime(1980, 3, 3),
                        Distance = DataCar.Sum(item => item.Distance),
                        ShareKey = "+"
                    }
                };
                DataCar = dataCarAll.Concat(DataCar);
            }

            DataBike = ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && t.Type == (int)TrackTypes.Bike && (roadsYear == 0 || t.Date.Year == roadsYear)).OrderByDescending(t => t.Date).Select(TracksView.Map).ToList();
            if (DataBike.Any())
            {
                var dataBikeAll = new List<TracksView>
                {
                    new TracksView
                    {
                        Id = -(int) TrackTypes.Bike,
                        Name = Resource.All,
                        UserId = DataBike.Count().ToString(),
                        Date = new DateTime(1980, 3, 3),
                        Distance = DataBike.Sum(item => item.Distance),
                        ShareKey = "+"
                    }
                };
                DataBike = dataBikeAll.Concat(DataBike);
            }

            var years = new List<SelectListItem> { 
                new SelectListItem
                {
                    Text = Resource.All,
                    Value = "0",
                    Selected = roadsYear == 0
                } };
            foreach (var elem in ac.Tracks.AsNoTracking().Where(t => t.UserId == userId).Select(t => t.Date.Year).Distinct())
            {
                years.Add(new SelectListItem
                {
                    Text = elem.ToString(),
                    Value = elem.ToString(),
                    Selected = roadsYear==elem
                });
            }
            YearsList = years.OrderByDescending(y => y.Text);
        }

        public static TrackInfo GetTrack(int id, string userId)
        {
            var ti = new TrackInfo {Path = new List<Location>()};
            var ac = new ApplicationDbContext();
            var track = ac.Tracks.AsNoTracking().First(t => t.UserId == userId && t.Id == id);
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
            var list = new List<TrackInfo>();
            var ac = new ApplicationDbContext();

            if (ac.Users.Any(u => u.ShareTracksFootKey != null && u.ShareTracksFootKey == key))
            {
                var userId = ac.Users.AsNoTracking().First(u => u.ShareTracksFootKey == key).Id;
                const int type = (int) TrackTypes.Foot;

                foreach (
                    var item in
                        ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && t.Type == type && (year == 0 || t.Date.Year == year))
                    )
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
                var userId = ac.Users.AsNoTracking().First(u => u.ShareTracksCarKey == key).Id;
                const int type = (int) TrackTypes.Car;

                foreach (
                    var item in
                        ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && t.Type == type && (year == 0 || t.Date.Year == year))
                    )
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
                var userId = ac.Users.AsNoTracking().First(u => u.ShareTracksBikeKey == key).Id;
                const int type = (int) TrackTypes.Bike;

                foreach (
                    var item in
                        ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && t.Type == type && (year == 0 || t.Date.Year == year))
                    )
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
                var userId = ac.Users.AsNoTracking().First(u => u.ShareTracksAllKey == key).Id;

                foreach (var item in ac.Tracks.AsNoTracking().Where(t => t.UserId == userId && (year == 0 || t.Date.Year == year)))
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
                foreach (var item in ac.Tracks.AsNoTracking().Where(t => t.ShareKey == key))
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
            }
            var obj = new ShareTrackInfo {Data = list};
            obj.CallcMinMaxCenter();
            return obj;
        }

        public static ShareTrackInfo GetTrackByType(string type, int year)
        {
            var list = new List<TrackInfo>();
            var ac = new ApplicationDbContext();

            type = type.Remove(0, 1);//Удалим минус
            var tType = Convert.ToInt32(type);

            foreach (var item in ac.Tracks.AsNoTracking().Where(t => t.Type == tType && (year == 0 || t.Date.Year == year)))
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
            return ac.Tracks.AsNoTracking().First(t => t.UserId == userId && t.Id == id).Name;
        }
        public static string GetTrackDateById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.AsNoTracking().First(t => t.UserId == userId && t.Id == id).Date.ToString(CultureInfo.CurrentCulture);
        }
        public static string GetTrackCoordinatesById(int id, string userId)
        {
            var ac = new ApplicationDbContext();
            return ac.Tracks.AsNoTracking().First(t => t.UserId == userId && t.Id == id).Coordinates;
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
