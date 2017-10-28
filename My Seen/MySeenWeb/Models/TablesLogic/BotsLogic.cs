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


        public static bool Contains(ICacheService cache, string us)
        {
            var ac = new ApplicationDbContext();
            return ac.Bots.Any(f => us.Contains(f.UserAgent));
        }
        public static bool Contains(ICacheService cache, string us, int language)
        {
            var ac = new ApplicationDbContext();
            return ac.Bots.Any(f => us.Contains(f.UserAgent) && f.Language == language);
        }

        public bool Contains(string us)
        {
            return _ac.Bots.Any(f => us.Contains(f.UserAgent));
        }
        public bool Contains(string us, int language)
        {
            return _ac.Bots.Any(f => us.Contains(f.UserAgent) && f.Language == language);
        }
    }
}