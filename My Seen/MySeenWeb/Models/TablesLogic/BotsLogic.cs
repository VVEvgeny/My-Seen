using System.Collections.Generic;
using System.Linq;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using MySeenWeb.Models.TablesLogic.Base;

namespace MySeenWeb.Models.TablesLogic
{
    public class BotsLogic : Films, IBaseLogic
    {
        //private readonly ApplicationDbContext _ac;
        private readonly ICacheService _cache;
        public string ErrorMessage;

        public BotsLogic()
        {
            ErrorMessage = string.Empty;
            //_ac = new ApplicationDbContext();
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

        private static IEnumerable<Bots> GetAll(ICacheService cache)
        {
            var bots = cache.Get<List<Bots>>(cache.GetFormatedName(CacheNames.Bots.ToString()));
            if (bots == null)
            {
                var ac = new ApplicationDbContext();
                bots = ac.Bots.ToList();
                cache.Set(cache.GetFormatedName(CacheNames.Bots.ToString()), bots, 15);
            }
            return bots;
        }

        public static bool Contains(ICacheService cache, string us)
        {
            return GetAll(cache).Any(f => us.Contains(f.UserAgent));
        }
        public static bool Contains(ICacheService cache, string us, int language)
        {
            return GetAll(cache).Any(f => us.Contains(f.UserAgent) && f.Language == language);
        }

        public bool Contains(string us)
        {
            return GetAll(_cache).Any(f => us.Contains(f.UserAgent));
        }
        public bool Contains(string us, int language)
        {
            return GetAll(_cache).Any(f => us.Contains(f.UserAgent) && f.Language == language);
        }
    }
}