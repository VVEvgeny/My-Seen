namespace MySeenWeb.Models.Meta
{
    public static class UserAgents
    {
        private static bool IsBot(string ua)
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

        private static bool IsBotRus(string ua)
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