using System;
using System.Web;
using MySeenLib;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models.Meta.Shared
{
    public class MetaSharedFilms : MetaBase
    {
        public static string Path = "/mymemory/films/shared/";

        public MetaSharedFilms(HttpRequestBase request, ICacheService cache)
            : base(request, cache)
        {
            Title = Resource.Films + " " + Resource.User2;
            try
            {
                var logic = new FilmsLogic();
                Description = Resource.Total + ": " +
                              logic.GetCountShared(request.Path.Split('/')[request.Path.Split('/').Length - 1]);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}