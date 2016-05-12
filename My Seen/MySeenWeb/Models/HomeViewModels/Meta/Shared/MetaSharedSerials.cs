using System;
using System.Web;
using MySeenLib;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models.Meta.Shared
{
    public class MetaSharedSerials : MetaBase
    {
        public static string Path = "/mymemory/serials/shared/";

        public MetaSharedSerials(HttpRequestBase request)
            : base(request)
        {
            Title = $"{Resource.Serials} {Resource.User2}; ";
            try
            {
                var logic = new SerialsLogic();
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