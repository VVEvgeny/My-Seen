using System.Web;
using MySeenLib;

namespace MySeenWeb.Models.Meta
{
    public class MetaBase
    {
        public MetaBase(HttpRequestBase request)
        {
            Title = Resource.AppName;
            Url = MySeenWebApi.ApiHost + request.Path;
            UaBot = IsBot(request.UserAgent);
            Image = MySeenWebApi.ApiHost + "/content/images/icon-512.png";
        }
        public string Twitter
        {
            get { return "@vvevgeny"; }
        }
        public string FacebookAppId
        {
            get { return "1485611081742857"; }
        }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool UaBot { get; set; }

        public static bool IsBot(string ua)
        {
            if (
                ua.Contains("SkypeUriPreview")

                || ua.Contains("vkShare")

                || ua.Contains("YandexMetrika")
                || ua.Contains("YandexBot")

                || ua.Contains("developers.google.com")
                || ua.ToLower().Contains(("Google Favicon").ToLower())
                || ua.Contains("Googlebot")

                || ua.Contains("bingbot")

                || ua.Contains("SurveyBot")

                || ua.Contains("DuckDuckGo-Favicons-Bot")

                || ua.Contains("openstat.ru/Bot")

                || ua.Contains("top100.rambler.ru")
                
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
