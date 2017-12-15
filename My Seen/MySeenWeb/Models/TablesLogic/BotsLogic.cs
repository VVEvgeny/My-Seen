using System;
using System.Collections.Generic;
using System.Linq;
using MySeenResources;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.TablesLogic.Base;

namespace MySeenWeb.Models.TablesLogic
{
    public class BotsLogic : Bots, IBaseLogic
    {
        private readonly ApplicationDbContext _ac;
        private readonly ICacheService _cache;
        public string ErrorMessage;

        public BotsLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public BotsLogic(ICacheService cache) : this()
        {
            _cache = cache;
        }

        public string GetError()
        {
            return ErrorMessage;
        }

        public bool Delete(string id, string userId)
        {
            ErrorMessage = $"{Resource.NotAuthorized}";
            return true;
        }

        public static IEnumerable<Bots> GetAll(ICacheService cache)
        {
            var bots = cache.Get<List<Bots>>(CacheNames.Bots);
            if (bots == null)
            {
                var ac = new ApplicationDbContext();
                bots = ac.Bots.AsNoTracking().ToList();
                cache.Set(CacheNames.Bots, bots, 15);
            }
            return bots;
        }

        public static bool Contains(ICacheService cache, string us)
        {
            if (us == null) return false;
            return GetAll(cache).Any(f => us.Contains(f.UserAgent));
        }
        public static bool Contains(ICacheService cache, string us, int language)
        {
            if (us == null) return false;
            return GetAll(cache).Any(f => us.Contains(f.UserAgent) && f.Language == language);
        }

        public bool Contains(string us)
        {
            if (us == null) return false;
            return GetAll(_cache).Any(f => us.Contains(f.UserAgent));
        }
        public bool Contains(string us, int language)
        {
            if (us == null) return false;
            return GetAll(_cache).Any(f => us.Contains(f.UserAgent) && f.Language == language);
        }
        private bool Fill(string name, string userAgent, int languageType)
        {
            try
            {
                Name = name;
                UserAgent = userAgent;
                Language = languageType;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            return true;
        }
        private bool Contains()
        {
            return Contains(UserAgent);
        }
        private bool Verify()
        {
            if (Contains()) ErrorMessage = Resource.AlreadyExists;
            else return true;

            return false;
        }
        public bool Add(string name, string userAgent, int language)
        {
            return Fill(name, userAgent, language) && Verify() && Add();
        }
        private bool Add()
        {
            try
            {
                _ac.Bots.Add(this);
                _ac.SaveChanges();
                _cache.Remove(CacheNames.Bots.ToString());
            }
            catch (Exception e)
            {
                ErrorMessage = $"{Resource.ErrorWorkWithDB} = {e.Message}";
                return false;
            }
            return true;
        }
    }
}