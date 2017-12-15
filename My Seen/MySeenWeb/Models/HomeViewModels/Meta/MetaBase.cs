using System.Web;
using MySeenResources;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.Meta.Portal;
using MySeenWeb.Models.Meta.Shared;
using MySeenWeb.Models.TablesLogic;
using static MySeenLib.MySeenWebApi;

namespace MySeenWeb.Models.Meta
{
    public partial class MetaBase
    {
        public string Twitter { get; } = "@vvevgeny";

        public string FacebookAppId { get; } = "1485611081742857";

        public string Title { get; set; } = Resource.AppName;
        public string Url { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool UaBot { get; set; }

        public MetaBase(HttpRequestBase request, ICacheService cache)
        {
            Url = ApiHost + request.Path;
            UaBot = BotsLogic.Contains(cache,request.UserAgent);
            Image = ApiHost + "/content/images/icon-512.png";
        }

        public static MetaBase Create(HttpRequestBase request, ICacheService cache)
        {
            if (BotsLogic.Contains(cache, request.UserAgent))
            {
                if (request.Path.ToLower().Contains(MetaPortalMemes.Path)) return new MetaPortalMemes(request, cache);
                if (request.Path.ToLower().Contains(MetaSharedFilms.Path)) return new MetaSharedFilms(request, cache);
                if (request.Path.ToLower().Contains(MetaSharedSerials.Path)) return new MetaSharedSerials(request, cache);
                if (request.Path.ToLower().Contains(MetaSharedBooks.Path)) return new MetaSharedBooks(request, cache);
                if (request.Path.ToLower().Contains(MetaSharedEvents.Path)) return new MetaSharedEvents(request, cache);
                if (request.Path.ToLower().Contains(MetaSharedRoads.Path)) return new MetaSharedRoads(request, cache);
            }
            return new MetaBase(request, cache);
        }
    }
}