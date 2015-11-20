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
        public int CountCar
        {
            get
            {
                return DataCar.Count();
            }
        }
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }

        public HomeViewModelTracks(string userId)
        {
            Type = ((int)TrackTypes.Foot).ToString();
            ApplicationDbContext ac = new ApplicationDbContext();
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
            TrackInfo ti = new TrackInfo();
            ApplicationDbContext ac = new ApplicationDbContext();
            ti.Path = new List<Location>();
            string coordinates = ac.Tracks.First(t => t.UserId == userId && t.Id == id).Coordinates;
            foreach (string s in coordinates.Split(';'))
            {
                ti.Path.Add(new Location { Latitude = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), Longitude = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
            }
            ti.CallcMinMaxCenter();
            return ti;
        }
        public static string GetTrackNameById(int id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Name;
        }
        public static string GetTrackCoordinatesById(int id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            return ac.Tracks.First(t => t.UserId == userId && t.Id == id).Coordinates;
        }
        public static string GetTrackShare(string id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            string key;
            if (id.ToLower().Contains("all"))
            {
                key = ac.Users.First(t => t.Id == userId).ShareTracksKey;
            }
            else
            {
                int iid = Convert.ToInt32(id);
                key = ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey;
            }
            if (string.IsNullOrEmpty(key)) return key;
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + key;
        }
        public static string GenerateTrackShare(string id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            string genkey = string.Empty;
            genkey += id + userId;
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 20; i++)
            {
                genkey += r.Next().ToString();
            }
            genkey = Md5Tools.GetMd5Hash(genkey);

            if (id.ToLower().Contains("all"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksKey = genkey;
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
            ApplicationDbContext ac = new ApplicationDbContext();

            if (id.ToLower().Contains("all"))
            {
                ac.Users.First(t => t.Id == userId).ShareTracksKey = string.Empty;
            }
            else
            {
                int iid = Convert.ToInt32(id);
                ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey = string.Empty;
            }
            ac.SaveChanges();
        }

    }
}
