using System.Web;
using MySeenLib;
using MySeenWeb.Models.Meta.Portal;
using MySeenWeb.Models.Meta.Shared;
using static MySeenLib.MySeenWebApi;

namespace MySeenWeb.Models.Meta
{
    public class MetaBase
    {
        public string Twitter { get; } = "@vvevgeny";

        public string FacebookAppId { get; } = "1485611081742857";

        public string Title { get; } = Resource.AppName;
        public string Url { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool UaBot { get; set; }

        public MetaBase(HttpRequestBase request)
        {
            Url = ApiHost + request.Path;
            UaBot = IsBot(request.UserAgent);
            Image = ApiHost + "/content/images/icon-512.png";
        }

        public static MetaBase Create(HttpRequestBase request)
        {
            if (IsBot(request.UserAgent))
            {
                if (request.Path.ToLower().Contains(MetaPortalMemes.Path)) return new MetaPortalMemes(request);
                if (request.Path.ToLower().Contains(MetaSharedFilms.Path)) return new MetaSharedFilms(request);
                if (request.Path.ToLower().Contains(MetaSharedSerials.Path)) return new MetaSharedSerials(request);
                if (request.Path.ToLower().Contains(MetaSharedBooks.Path)) return new MetaSharedBooks(request);
                if (request.Path.ToLower().Contains(MetaSharedEvents.Path)) return new MetaSharedEvents(request);
                if (request.Path.ToLower().Contains(MetaSharedRoads.Path)) return new MetaSharedRoads(request);
            }
            return new MetaBase(request);
        }

        public static bool IsBot(string ua)
        {
            if (
                ua.Contains("SkypeUriPreview")
                || ua.Contains("vkShare")
                || ua.Contains("YandexMetrika")
                || ua.Contains("YandexBot")
                || ua.Contains("developers.google.com")
                || ua.ToLower().Contains("Google Favicon".ToLower()) //они с разными регистрами ходят...
                || ua.Contains("Googlebot")
                || ua.Contains("Google Page Speed")
                || ua.Contains("Structured-Data-Testing-Tool")
                || ua.Contains("Google PP Default")
                || ua.Contains("Relap fetcher")
                || ua.Contains("AddThis.com")
                || ua.Contains("facebookexternalhit")
                || ua.Contains("bingbot")
                || ua.Contains("UnitPay Robot")
                || ua.Contains("http://www.site-shot.com/")
                || ua.Contains("http://validator.w3.org/services")
                || ua.Contains("SurveyBot")
                || ua.Contains("DuckDuckGo-Favicons-Bot")
                || ua.Contains("openstat.ru/Bot")
                || ua.Contains("top100.rambler.ru")
                || ua.Contains("CheckHost")
                || ua.Contains("Yahoo! Slurp")
                ) return true;
            return false;
        }

        public static bool IsBotRus(string ua)
        {
            if (
                ua.Contains("SkypeUriPreview")
                || ua.Contains("vkShare")
                || ua.Contains("YandexMetrika")
                || ua.Contains("YandexBot")
                || ua.Contains("openstat.ru/Bot")
                || ua.Contains("top100.rambler.ru")
                ) return true;
            return false;
        }
    }
}