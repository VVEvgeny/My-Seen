using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models.TablesLogic
{
    public class RoadsLogic : Events
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public RoadsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        
        public string GetShare(string id, string userId)
        {
            try
            {
                var key = string.Empty;
                if (id.Contains("-"))
                {
                    id = id.Remove(0, 1);
                    var iid = Convert.ToInt32(id);
                    switch (iid)
                    {
                        case (int)TrackTypes.Bike:
                            key = _ac.Users.First(t => t.Id == userId).ShareTracksBikeKey;
                            break;
                        case (int)TrackTypes.Car:
                            key = _ac.Users.First(t => t.Id == userId).ShareTracksCarKey;
                            break;
                        case (int)TrackTypes.Foot:
                            key = _ac.Users.First(t => t.Id == userId).ShareTracksFootKey;
                            break;
                    }
                }
                else if (id.ToLower().Contains("all"))
                {
                    key = _ac.Users.First(t => t.Id == userId).ShareTracksAllKey;
                }
                else
                {
                    var iid = Convert.ToInt32(id);
                    key = _ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey;
                }
                if (string.IsNullOrEmpty(key)) return "-";
                return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + key;
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
            }
            return "-";
        }
        public string GenerateShare(string id, string userId)
        {
            var genkey = string.Empty;
            genkey += id + userId;
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 20; i++)
            {
                genkey += r.Next().ToString();
            }
            genkey = Md5Tools.Get(genkey);

            if (id.Contains("-"))
            {
                id = id.Remove(0, 1);
                var iid = Convert.ToInt32(id);
                switch (iid)
                {
                    case (int)TrackTypes.Bike:
                        _ac.Users.First(t => t.Id == userId).ShareTracksBikeKey = genkey;
                        break;
                    case (int)TrackTypes.Car:
                        _ac.Users.First(t => t.Id == userId).ShareTracksCarKey = genkey;
                        break;
                    case (int)TrackTypes.Foot:
                        _ac.Users.First(t => t.Id == userId).ShareTracksFootKey = genkey;
                        break;
                }
            }
            else if (id.ToLower().Contains("all"))
            {
                _ac.Users.First(t => t.Id == userId).ShareTracksAllKey = genkey;
            }
            else
            {
                var iid = Convert.ToInt32(id);
                if (_ac.Tracks != null) _ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey = genkey;
            }
            _ac.SaveChanges();
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareTracks + genkey;
        }
        public string DeleteShare(string id, string userId)
        {
            if (id.Contains("-"))
            {
                id = id.Remove(0, 1);
                var iid = Convert.ToInt32(id);
                switch (iid)
                {
                    case (int)TrackTypes.Bike:
                        _ac.Users.First(t => t.Id == userId).ShareTracksBikeKey = string.Empty;
                        break;
                    case (int)TrackTypes.Car:
                        _ac.Users.First(t => t.Id == userId).ShareTracksCarKey = string.Empty;
                        break;
                    case (int)TrackTypes.Foot:
                        _ac.Users.First(t => t.Id == userId).ShareTracksFootKey = string.Empty;
                        break;
                }
            }
            else if (id.ToLower().Contains("all"))
            {
                _ac.Users.First(t => t.Id == userId).ShareTracksAllKey = string.Empty;
            }
            else
            {
                var iid = Convert.ToInt32(id);
                _ac.Tracks.First(t => t.UserId == userId && t.Id == iid).ShareKey = string.Empty;
            }
            _ac.SaveChanges();
            return "-";
        }
    }
}
