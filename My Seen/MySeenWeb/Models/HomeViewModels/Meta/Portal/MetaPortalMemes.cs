using System;
using System.Web;
using MySeenLib;
using MySeenWeb.Models.TablesLogic.Portal;

namespace MySeenWeb.Models.Meta.Portal
{
    public class MetaPortalMemes : MetaBase
    {
        public static string Path = "/portal/memes/";
        public MetaPortalMemes(HttpRequestBase request)
            : base(request)
        {
            Title = Resource.Memes;

            if (request.Path.ToLower() != Path)
            {
                try
                {
                    var logic = new MemesLogic();
                    var mem = logic.Get(Convert.ToInt32(request.Path.Split('/')[request.Path.Split('/').Length - 1]));
                    Title = Resource.Mem + ": " + mem.Name + "";
                    Image = mem.Image;
                    Description = Title + "; " + Resource.Added + ": " + mem.Date + "";
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
