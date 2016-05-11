using System;
using System.Linq;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesLogic
{
    public class FilmsLogic : Films
    {
        private readonly ApplicationDbContext _ac;
        private readonly ICacheService _cache;
        public string ErrorMessage;

        public FilmsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public FilmsLogic(ICacheService cache) : this()
        {
            _cache = cache;
        }

        private bool Fill(string name, string year, string datetime, string genre, string rating, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(datetime)) throw new Exception("Нужна дата");
                Name = name;
                Year = string.IsNullOrEmpty(year) ? 0 : Convert.ToInt32(year);
                DateSee = UmtTime.To(Convert.ToDateTime(datetime));
                Genre = Convert.ToInt32(genre);
                Rating = Convert.ToInt32(rating);
                DateChange = UmtTime.To(DateTime.Now);
                UserId = userId;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }

        private bool Fill(string id, string name, string year, string datetime, string genre, string rating,
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
            return Fill(name, year, datetime, genre, rating, userId);
        }

        private bool Contains()
        {
            return _ac.Films.Any(f => f.Name == Name && f.UserId == UserId && f.Id != Id && f.Year == Year);
        }

        private bool Verify()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ErrorMessage = Resource.EnterFilmName;
            }
            else if (Contains())
            {
                ErrorMessage = Resource.FilmNameAlreadyExists;
            }
            else return true;

            return false;
        }

        private bool Add()
        {
            try
            {
                _ac.Films.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.UserFilms.ToString(), UserId);
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
                var film = _ac.Films.First(f => f.UserId == UserId && f.Id == Id);
                film.Name = Name;
                film.Genre = Genre;
                film.Rating = Rating;
                film.DateChange = DateChange;
                film.DateSee = DateSee;
                film.Year = Year;
                _ac.SaveChanges();
                _cache.Remove(CacheNames.UserFilms.ToString(), UserId);
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                return false;
            }
            return true;
        }

        public bool Add(string name, string year, string datetime, string genre, string rating, string userId)
        {
            return Fill(name, year, datetime, genre, rating, userId) && Verify() && Add();
        }

        public bool Update(string id, string name, string year, string datetime, string genre, string rating,
            string userId)
        {
            return Fill(id, name, year, datetime, genre, rating, userId) && Verify() && Update();
        }

        public bool Delete(string id, string userId)
        {
            try
            {
                Id = Convert.ToInt32(id);
                if (_ac.Films.Any(f => f.UserId == userId && f.Id == Id))
                {
                    _ac.Films.RemoveRange(_ac.Films.Where(f => f.UserId == userId && f.Id == Id));
                    _ac.SaveChanges();
                    _cache.Remove(CacheNames.UserFilms.ToString(), userId);
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
                Id = Convert.ToInt32(id);
                if (_ac.Films.First(f => f.UserId == userId && f.Id == Id).Shared)
                {
                    var key = _ac.Users.First(t => t.Id == userId).ShareFilmsKey;
                    return MySeenWebApi.ApiHost + MySeenWebApi.ShareFilms + key;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
            }
            return "-";
        }

        public string GenerateShare(string id, string userId)
        {
            var iid = Convert.ToInt32(id);
            var key = _ac.Users.First(t => t.Id == userId).ShareFilmsKey;
            _ac.Films.First(e => e.Id == iid).Shared = true;
            _ac.SaveChanges();
            _cache.Remove(CacheNames.UserFilms.ToString(), userId);
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareFilms + key;
        }

        public string DeleteShare(string id, string userId)
        {
            var iid = Convert.ToInt32(id);
            _ac.Films.First(e => e.Id == iid && e.UserId == userId).Shared = false;
            _ac.SaveChanges();
            _cache.Remove(CacheNames.UserFilms.ToString(), userId);
            return "-";
        }

        public int GetCountShared(string key)
        {
            return _ac.Films.Count(f => f.Shared && f.User.ShareFilmsKey == key);
        }
    }
}