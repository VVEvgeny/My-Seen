using System;
using System.Collections.Generic;
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
        MemesPages
    }

    public interface ICacheService
    {
        T Get<T>(string cacheId) where T : class;
        T Set<T>(string cacheId, T item, int expMinutes) where T : class;
        //void Remove(string cacheId);
        void Remove(Func<KeyValuePair<string, object>, bool> predicate);

        void Remove(string cacheTemplate, string cacheTemplate2);
        string GetFormatedName(string name, params object[] values);
    }

    public abstract class InMemoryCache : ICacheService
    {
        public string GetFormatedName(string name, params object[] values)
        {
            return values.Where(s => s != null).Aggregate(name, (current, s) => current + ("-" + s));
        }

        public T Get<T>(string cacheId) where T : class
        {
            var item = HttpRuntime.Cache.Get(cacheId) as T;
            return item;
        }

        public T Set<T>(string cacheId, T item, int expMinutes) where T : class
        {
            HttpRuntime.Cache.Insert(cacheId, item, null, DateTime.UtcNow.AddMinutes(expMinutes),
                Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            return item;
        }

        public void Remove(string cacheTemplate, string cacheTemplate2)
        {
            if (string.IsNullOrEmpty(cacheTemplate) || string.IsNullOrEmpty(cacheTemplate2)) return;

            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key.ToString().Contains(cacheTemplate) &&
                    cacheEnum.Key.ToString().Contains(cacheTemplate2))
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public void Remove(Func<KeyValuePair<string, object>, bool> predicate)
        {
            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            var collection = new Dictionary<string, object>();

            while (cacheEnum.MoveNext())
            {
                collection.Add(cacheEnum.Key.ToString(), HttpRuntime.Cache[cacheEnum.Key.ToString()]);
            }

            var filtered = collection.Where(predicate).ToList();

            if (!filtered.Any())
                return;

            foreach (var item in filtered)
            {
                HttpRuntime.Cache.Remove(item.Key);
            }
        }

        public void Remove(string cacheId)
        {
            if (string.IsNullOrEmpty(cacheId))
            {
                var cacheEnum = HttpRuntime.Cache.GetEnumerator();

                while (cacheEnum.MoveNext())
                {
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
                }
            }
            else
            {
                if (HttpRuntime.Cache[cacheId] != null) HttpRuntime.Cache.Remove(cacheId);
            }
        }
    }
}