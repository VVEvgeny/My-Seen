using System;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MySeenWeb.Add_Code
{
    public enum CacheNames
    {
        UserFilms,
        UserFilmsPages,
        UserBooks,
        UserBooksPages,
        UserSerials,
        UserSerialsPages,
        UserRoads, //Для общего названия, по нему будем чистить КЭШ
        UserRoadsYearsList,
        UserRoadsFoot,
        UserRoadsBike,
        UserRoadsCar,
        Memes,
        MemesPages,
        Salaries,
        Deals,
        Realt,
        UserRoles,
        Bots
    }

    public interface ICacheService
    {
        T Get<T>(CacheNames cacheId) where T : class;
        T Get<T>(string cacheId) where T : class;
        T Set<T>(CacheNames cacheId, T item, int expMinutes) where T : class;
        T Set<T>(string cacheId, T item, int expMinutes) where T : class;

        void Remove(string cacheId);//for all users
        void Remove(string cacheId, string userId);

        int RemoveAndCount(string cacheId, string userId); 

        string GetFormatedName(CacheNames name, params object[] values);
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class InMemoryCache : ICacheService
    {
        public string GetFormatedName(CacheNames name, params object[] values)
        {
            return values.Where(s => s != null).Aggregate(name.ToString(), (current, s) => current + ("-" + s));
        }

        public T Get<T>(CacheNames cacheId) where T : class
        {
            return Get<T>(cacheId.ToString());
        }

        public T Get<T>(string cacheId) where T : class
        {
            var item = HttpRuntime.Cache.Get(cacheId) as T;
            return item;
        }

        public T Set<T>(CacheNames cacheId, T item, int expMinutes) where T : class
        {
            return Set(cacheId.ToString(), item, expMinutes);
        }

        public T Set<T>(string cacheId, T item, int expMinutes) where T : class
        {
            HttpRuntime.Cache.Insert(cacheId, item, null, DateTime.UtcNow.AddMinutes(expMinutes),
                Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            return item;
        }
        public void Remove(string cacheId)
        {
            if (string.IsNullOrEmpty(cacheId)) return;

            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key != null && cacheEnum.Key.ToString().Contains(cacheId))
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
            }
        }
        public void Remove(string cacheId, string userId)
        {
            if (string.IsNullOrEmpty(cacheId) || string.IsNullOrEmpty(userId)) return;

            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key != null && cacheEnum.Key.ToString().Contains(cacheId) && cacheEnum.Key.ToString().Contains(userId))
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
            }
        }
        public int RemoveAndCount(string cacheId, string userId)
        {
            var i = 0;
            if (string.IsNullOrEmpty(cacheId) || string.IsNullOrEmpty(userId)) return i;

            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key != null && cacheEnum.Key.ToString().Contains(cacheId) && cacheEnum.Key.ToString().Contains(userId))
                {
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
                    i++;
                }
            }
            return i;
        }
    }
}