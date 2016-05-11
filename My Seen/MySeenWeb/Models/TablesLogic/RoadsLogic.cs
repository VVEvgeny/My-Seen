using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models.TablesLogic
{
    public class RoadsLogic : Tracks
    {
        private readonly ApplicationDbContext _ac;
        private readonly ICacheService _cache;
        public string ErrorMessage;

        public RoadsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public RoadsLogic(ICacheService cache) : this()
        {
            _cache = cache;
        }

        private bool Fill(string name, string datetime, string type, string coordinates, string distance, string userId)
        {
            try
            {
                Name = name;
                Date = UmtTime.To(Convert.ToDateTime(datetime));
                Type = Convert.ToInt32(type);
                Coordinates = coordinates;
                if (distance.Contains('.'))
                    distance = distance.Remove(distance.IndexOf('.')); //Только кол-во КМ запишем
                Distance = Convert.ToDouble(distance);
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }

        private bool Fill(string id, string name, string datetime, string type, string coordinates, string distance,
            string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return Fill(name, datetime, type, coordinates, distance, userId);
        }

        private bool Verify()
        {
            if (string.IsNullOrEmpty(Name)) ErrorMessage = Resource.ShortName;
            else if (Coordinates.Length == 0) ErrorMessage = Resource.NoCoordinates;
            else if (Distance == 0) ErrorMessage = Resource.ErrorCalculating;
            else return true;
            return false;
        }

        private bool Add()
        {
            try
            {
                _ac.Tracks.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.UserRoads.ToString(), UserId);
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }

        private bool Update()
        {
            try
            {
                var elem = _ac.Tracks.First(t => t.Id == Id && t.UserId == UserId);
                elem.Name = Name;
                elem.Type = Type;
                elem.Coordinates = Coordinates;
                elem.Date = Date;
                elem.Distance = Distance;
                _ac.SaveChanges();
                _cache.Remove(CacheNames.UserRoads.ToString(), UserId);
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }

        public bool Add(string name, string datetime, string type, string coordinates, string distance, string userId)
        {
            return Fill(name, datetime, type, coordinates, distance, userId) && Verify() && Add();
        }

        public bool Update(string id, string name, string datetime, string type, string coordinates, string distance,
            string userId)
        {
            return Fill(id, name, datetime, type, coordinates, distance, userId) && Verify() && Update();
        }

        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                if (_ac.Tracks.Any(f => f.UserId == userId && f.Id == Id))
                {
                    _ac.Tracks.RemoveRange(_ac.Tracks.Where(f => f.UserId == userId && f.Id == Id));
                    _ac.SaveChanges();
                    _cache.Remove(CacheNames.UserRoads.ToString(), userId);
                }
                else
                {
                    ErrorMessage = Resource.NoData;
                    return false;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
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
                        case (int) RoadTypes.Bike:
                            key = _ac.Users.First(t => t.Id == userId).ShareTracksBikeKey;
                            break;
                        case (int) RoadTypes.Car:
                            key = _ac.Users.First(t => t.Id == userId).ShareTracksCarKey;
                            break;
                        case (int) RoadTypes.Foot:
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
                    case (int) RoadTypes.Bike:
                        _ac.Users.First(t => t.Id == userId).ShareTracksBikeKey = genkey;
                        break;
                    case (int) RoadTypes.Car:
                        _ac.Users.First(t => t.Id == userId).ShareTracksCarKey = genkey;
                        break;
                    case (int) RoadTypes.Foot:
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
            _cache.Remove(CacheNames.UserRoads.ToString(), userId);
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
                    case (int) RoadTypes.Bike:
                        _ac.Users.First(t => t.Id == userId).ShareTracksBikeKey = string.Empty;
                        break;
                    case (int) RoadTypes.Car:
                        _ac.Users.First(t => t.Id == userId).ShareTracksCarKey = string.Empty;
                        break;
                    case (int) RoadTypes.Foot:
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
            _cache.Remove(CacheNames.UserRoads.ToString(), userId);
            return "-";
        }

        public int GetCountShared(string key)
        {
            return
                _ac.Tracks.Count(
                    f =>
                        //!string.IsNullOrEmpty(f.ShareKey) &&
                        f.User.ShareTracksAllKey == key || f.User.ShareTracksFootKey == key ||
                        f.User.ShareTracksCarKey == key || f.User.ShareTracksBikeKey == key);
        }

        public bool IsSingle(string key)
        {
            return _ac.Tracks.Any(f => f.ShareKey == key);
        }

        public Tracks GetOne(string key)
        {
            return _ac.Tracks.FirstOrDefault(f => f.ShareKey == key);
        }
    }
}