using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;
using MySeenWeb.Models.TablesLogic.Base;
using System.Collections.Generic;
using System.Linq;

namespace MySeenWeb.Models.TablesLogic.Portal
{
    public class RealtLogic : Realt, IBaseLogic
    {
        private readonly ICacheService _cache;
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;

        public RealtLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }
        public RealtLogic(ICacheService cache): this()
        {
            _cache = cache;
        }

        public static List<Realt> GetAll(ICacheService cache)
        {
            return new RealtLogic(cache).GetAll();
        }

        public List<Realt> GetAll()
        {
            var realt = _cache.Get<List<Realt>>(CacheNames.Realt.ToString());
            if (realt == null)
            {
                realt = _ac.Realt.AsNoTracking().ToList();
                _cache.Set(CacheNames.Realt.ToString(), realt, 15);
            }
            return realt;
        }

        public string GetError()
        {
            return ErrorMessage;
        }

        public bool Delete(string id, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}