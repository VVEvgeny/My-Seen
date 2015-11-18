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
    public class HomeViewModelTracks
    {
        public IEnumerable<TracksView> DataFoot { get; set; }
        public IEnumerable<TracksView> DataCar { get; set; }
        public bool HaveFoot
        {
            get
            {
                return DataFoot.Count() > 0;
            }
        }
        public bool HaveCar
        {
            get
            {
                return DataCar.Count() > 0;
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

            List<SelectListItem> listItemsTypes = new List<SelectListItem>();
            listItemsTypes.Add(new SelectListItem { Text = Resource.FootBike, Value = ((int)TrackTypes.Foot).ToString(), Selected = true });
            listItemsTypes.Add(new SelectListItem { Text = Resource.Car, Value = ((int)TrackTypes.Car).ToString(), Selected = false });
            TypeList = listItemsTypes;
        }

        public static TrackInfo GetTrack(int id, string userId)
        {
            TrackInfo ti = new TrackInfo();
            ApplicationDbContext ac = new ApplicationDbContext();
            ti.Path = new List<Location>();
            string coordinates = ac.Tracks.Where(t => t.UserId == userId && t.Id == id).OrderByDescending(t => t.Date).Select(t => t.Coordinates).First();
            foreach(string s in coordinates.Split(';'))
            {
                ti.Path.Add(new Location { lat = double.Parse(s.Split(',')[0], CultureInfo.InvariantCulture), lng = double.Parse(s.Split(',')[1], CultureInfo.InvariantCulture) });
            }
            ti.CallcMinMaxCenter();
            return ti;
        }
        public static string GetTrackNameById(int id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            return ac.Tracks.Where(t => t.UserId == userId && t.Id == id).OrderByDescending(t => t.Date).Select(t => t.Name).First();
        }
        public static string GetTrackCoordinatesById(int id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            return ac.Tracks.Where(t => t.UserId == userId && t.Id == id).OrderByDescending(t => t.Date).Select(t => t.Coordinates).First();
        }
        public static string GetTrackShare(string id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            string key = string.Empty;
            if (id.ToLower().Contains("all"))
            {
                key=ac.Users.Where(t => t.Id == userId).Select(t => t.ShareTracksKey).First();
            }
            else
            {
                int _id = Convert.ToInt32(id);
                key=ac.Tracks.Where(t => t.UserId == userId && t.Id == _id).OrderByDescending(t => t.Date).Select(t => t.ShareKey).First();
            }
            if(string.IsNullOrEmpty(key))return key;
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + key;
        }
        public static string GenerateTrackShare(string id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            string genkey = string.Empty;
            genkey += id + userId;
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 20;i++ )
            {
                genkey += r.Next().ToString();
            }
            genkey = MD5Tools.GetMd5Hash(genkey);

            if (id.ToLower().Contains("all"))
            {
                var trackkey = ac.Users.Where(t => t.Id == userId).First();
                trackkey.ShareTracksKey = genkey;
            }
            else
            {
                int _id = Convert.ToInt32(id);
                var trackkey=ac.Tracks.Where(t => t.UserId == userId && t.Id == _id).First();
                trackkey.ShareKey = genkey;
            }
            ac.SaveChanges();
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + genkey;
        }
        public static void DeleteTrackShare(string id, string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();

            if (id.ToLower().Contains("all"))
            {
                var trackkey = ac.Users.Where(t => t.Id == userId).First();
                trackkey.ShareTracksKey = string.Empty;
            }
            else
            {
                int _id = Convert.ToInt32(id);
                var trackkey=ac.Tracks.Where(t => t.UserId == userId && t.Id == _id).First();
                trackkey.ShareKey = string.Empty;
            }
            ac.SaveChanges();
        }
        
    }
}
