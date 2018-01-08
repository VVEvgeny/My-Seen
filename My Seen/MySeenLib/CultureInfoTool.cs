using System.Globalization;
using System.Threading;

namespace MySeenLib
{
    public static class CultureInfoTool
    {
        public static bool NeedTranslate => First2Culture.ToLower() != Cultures.English;

        public static string First2Culture => Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2);

        public static string Culture => Thread.CurrentThread.CurrentUICulture.ToString();

        public static bool SetCulture(string cult)
        {
            if (Culture == cult) return false;
            var culture = new CultureInfo(cult);

            var datetimeformat = culture.DateTimeFormat;
            datetimeformat.LongTimePattern = cult == Cultures.English ? "h:mm:ss tt" : "HH:mm:ss";
            culture.DateTimeFormat = datetimeformat;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Defaults.ReloadResources();

            return true;
        }

        public static class Cultures
        {
            public static string English { get; } = "en";

            public static string Russian { get; } = "ru";
        }
    }
}