using System;
using System.Web;
using MySeenResources;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models.Meta.Shared
{
    public class MetaSharedEvents : MetaBase
    {
        public static string Path = "/mymemory/events/shared/";

        public MetaSharedEvents(HttpRequestBase request, ICacheService cache)
            : base(request, cache)
        {
            Title = Resource.Events + " " + Resource.User2;
            try
            {
                var logic = new EventsLogic();
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