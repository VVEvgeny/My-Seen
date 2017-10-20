using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using static System.Threading.Thread;
using static MySeenLib.Admin;
using static MySeenLib.CultureInfoTool.Cultures;
using static MySeenLib.Defaults;

namespace MySeenLib
{
    public static class SyncJsonAnswerExtension
    {
        public static string SplitByWords(this MySeenWebApi.SyncJsonAnswer.Values value)
        {
            var s = value.ToString();
            var ss = string.Empty;

            for (var index = 0; index < s.Length; index++)
            {
                var c = s[index];
                if (index != 0 && char.IsUpper(c)) ss += " ";
                ss += c.ToString();
            }
            return ss;
        }
    }
    public static class Versions
    {
        //Строка с версией библиотеки в ресурсах LibVersionNum
        public static int Version { get; } = 25;
        public static int Android { get; } = 1;
        public static int AndroidLib { get; } = 1;
        public static int Pc { get; } = 1;
    }

    public static class Admin
    {
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                    return false;
                #endif
            }
        }
    }

    public static class UmtTime
    {
        public static DateTime To(DateTime datetime)
        {
            return datetime.ToUniversalTime();
        }

        public static DateTime From(DateTime datetime)
        {
            return datetime.ToLocalTime();
        }
    }

    public static class CultureInfoTool
    {
        public static string First2Culture => CurrentThread.CurrentUICulture.ToString().Substring(0, 2);

        public static string Culture => CurrentThread.CurrentUICulture.ToString();

        public static bool SetCulture(string cult)
        {
            if (Culture == cult) return false;
            var culture = new CultureInfo(cult);

            var datetimeformat = culture.DateTimeFormat;
            datetimeformat.LongTimePattern = cult == English ? "h:mm:ss tt" : "HH:mm:ss";
            culture.DateTimeFormat = datetimeformat;

            CurrentThread.CurrentCulture = culture;
            CurrentThread.CurrentUICulture = culture;

            ReloadResources();

            return true;
        }

        public static class Cultures
        {
            public static string English { get; } = "en";

            public static string Russian { get; } = "ru";
        }
    }

    public static class MySeenWebApi
    {
        public static string ApiHost => IsDebug ? "http://localhost:44301" : "http://myseen.by";
        public static string ApiHostAndroid => IsDebug ? "https://10.0.2.2:443" : ApiHost;

        public enum DataModes
        {
            Road = 1
        }

        public enum SyncModesApiData
        {
            GetRoads = 1
        }

        public enum SyncModesApiUsers
        {
            IsUserExists = 1
        }
        public enum SyncModesApiLogin
        {
            GetKey = 1
        }

        public static int ApiVersion = 1;

        public static bool CheckApiVersion(int apiVersion)
        {
            if (apiVersion > ApiVersion) return false;

            return true;
        }

        public static string ApiUsers { get; } = @"/api/Users/";
        public static string ApiLogin { get; } = @"/api/Login/";
        public static string ApiSync { get; } = @"/api/Sync/";

        public static string ShareTracks { get; } = @"/roads/shared/";
        public static string ShareEvents { get; } = @"/events/shared/";
        public static string ShareFilms { get; } = @"/films/shared/";
        public static string ShareSerials { get; } = @"/serials/shared/";
        public static string ShareBooks { get; } = @"/books/shared/";

        public static IEnumerable<SyncJsonData> GetResponse(string data)
        {
            return JsonConvert.DeserializeObject<IEnumerable<SyncJsonData>>(data);
        }

        public static string SetResponse(IEnumerable<SyncJsonData> data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static SyncJsonAnswer GetResponseAnswer(string data)
        {
            SyncJsonAnswer answer = null;
            try
            {
                answer = JsonConvert.DeserializeObject<SyncJsonAnswer>(data);
            }
            catch
            {
                // ignored
            }
            return answer;
        }
        public class SyncJsonAnswer
        {
            [JsonProperty("Value")]
            public Values Value { get; set; }

            [JsonProperty("Data")]
            public string Data { get; set; }

            public enum Values
            {
                Ok = 1,
                NoData = 2,
                BadRequestMode = 3,
                UserNotExist = 4,
                NewDataRecieved = 5,
                NoLongerSupportedVersion = 6,
                SomeErrorObtained = 7
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class SyncJsonData
        {
            [JsonProperty("DataMode")]
            public int DataMode { get; set; } //in DataModes

            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Type")]
            public int Type { get; set; }

            //[JsonProperty("Date")]
            //public DateTime Date { get; set; }

            [JsonProperty("Coordinates")]
            public string Coordinates { get; set; }

            [JsonProperty("Distance")]
            public double Distance { get; set; }
        }
    }

    public static class Defaults
    {
        public static readonly GenresBase Genres = new GenresBase();
        public static readonly RatingsBase Ratings = new RatingsBase();
        public static readonly CategoryBase Categories = new CategoryBase();
        public static readonly LanguagesBase Languages = new LanguagesBase();
        public static readonly ComplexBase Complexes = new ComplexBase();
        public static readonly RecordPerPageBase RecordPerPage = new RecordPerPageBase();
        public static readonly EnabledDisabledBase EnabledDisabled = new EnabledDisabledBase();
        public static readonly EventsTypesBase EventTypes = new EventsTypesBase();
        public static readonly RolesBase RolesTypes = new RolesBase();
        public static readonly ThemesBase Themes = new ThemesBase();

        private static readonly List<ListStringBase> AllResourcesLink = new List<ListStringBase>
        {
            Genres,
            Ratings,
            Categories,
            Languages,
            Complexes,
            RecordPerPage,
            EnabledDisabled,
            EventTypes,
            RolesTypes,
            Themes
        };

        public static void ReloadResources()
        {
            foreach (var val in AllResourcesLink)
            {
                val.Reload();
            }
        }

        public abstract class ListStringBase
        {
            protected List<string> All;
            protected abstract void Load();

            public void Reload()
            {
                All = null;
                Load();
            }

            public List<string> GetAll()
            {
                if (All == null) Load();
                return All;
            }

            public int GetId(string str)
            {
                if (All == null) Load();
                return All?.IndexOf(str) ?? -1;
            }

            public string GetById(int id)
            {
                if (All == null) Load();
                if (All != null && (id >= All.Count || id < 0)) return "";
                return All != null ? All[id] : "";
            }

            public int GetMaxId()
            {
                if (All == null) Load();
                return All?.Count - 1 ?? 0;
            }

            public string GetMaxValue()
            {
                if (All == null) Load();
                if (All != null && All.Count == 0) return "";
                return All != null ? All[GetMaxId()] : "";
            }
        }

        public abstract class ListStringBoolBase : ListStringBase
        {
            protected List<bool> Type;

            public bool GetTypeById(int id)
            {
                if (All == null) Load();
                if (All != null && id >= All.Count) return false;
                return All != null && Type[id];
            }

            public new void Reload()
            {
                Type = null;
                base.Reload();
            }
        }

        public class GenresBase : ListStringBase
        {
            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string>
                    {
                        Resource.GenreThriller,
                        Resource.GenreDocumentary,
                        Resource.GenreDrama,
                        Resource.GenreComedy,
                        Resource.GenreConcert,
                        Resource.GenreCartoon,
                        Resource.GenreHorror,
                        Resource.GenreFantastic,
                        Resource.GenreHistorical
                    };
                }
            }
        }

        public class RatingsBase : ListStringBase
        {
            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
                }
            }
        }

        public class CategoryBase : ListStringBase
        {
            public enum Indexes
            {
                Films,
                Serials,
                Books,
                Roads,
                Events
            }

            public enum IndexesExt
            {
                Users = 101, // Пользователи
                Logs = 102, // Логи
                Improvements = 103, // Улучшения
                Errors = 104, // Ошибки
                About = 105, // Страница О
                Settings = 106 // Настройки
            }

            public enum IndexesMain
            {
                Main = 200,
                Memes = 201,
                Childs = 202,
                Realt = 203,
                Imt = 204
            }

            public enum IndexesTest
            {
                Realt = 901
            }

            public static bool IsCategoryExt(int category)
            {
                return category == (int) IndexesExt.Users || category == (int) IndexesExt.Logs ||
                       category == (int) IndexesExt.Improvements || category == (int) IndexesExt.Errors;
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string>
                    {
                        Resource.Films,
                        Resource.Serials,
                        Resource.Books,
                        Resource.Tracks,
                        Resource.Events
                    };
                }
            }
        }

        public class RolesBase : ListStringBase
        {
            public enum Indexes
            {
                Admin,
                Tester
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string>
                    {
                        Resource.Administrator,
                        Resource.Tester
                    };
                }
            }
        }

        public class LanguagesBase : ListStringBase
        {
            public enum Indexes
            {
                English,
                Russian
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.English, Resource.Russian};
                }
            }

            public int GetIdDb(string s)
            {
                Load();
                return s == English ? (int) Indexes.English : (int) Indexes.Russian;
            }

            public string GetValDb(int i)
            {
                Load();
                return i == (int) Indexes.English ? English : Russian;
            }
        }

        public class ComplexBase : ListStringBase
        {
            public enum Indexes
            {
                All
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.All, Resource.WEB, Resource.Android, Resource.PC, "2048"};
                }
            }
        }

        public class RecordPerPageBase : ListStringBase
        {
            public enum Indexes
            {
                All
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = IsDebug
                        ? new List<string> {Resource.All, "3", "5", "20", "500"}
                        : new List<string> {Resource.All, "20", "50", "100", "500"};
                }
            }

            public static class Values
            {
                public static int All = int.MaxValue;
            }
        }

        public class EnabledDisabledBase : ListStringBase
        {
            public enum Indexes
            {
                Enabled,
                Disabled
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.Enabled, Resource.Disabled};
                }
            }
        }

        public class EventsTypesBase : ListStringBoolBase
        {
            public enum Indexes
            {
                OneTime, //1 раз в указанную дату
                OneTimeWithPast, //1 раз в указанную дату + показывать пройденное
                EveryMonthInNeedDayWithWhenSundayOrSaturdayThenMonday,
                EveryMonthInNeedDayWithWhenSaturdayOrFridayThenThursdayWhenSundayOrMondayThenTuesday,
                //Каждый месяц нужного числа, если ПТ или СБ значит ЧТ если ВС или ПН значит ВТ
                EveryMonthInNeedDayWithWhenSaturdayThenFridayWhenSundayThenMonday,
                //Каждый месяц нужного числа, если СБ или ВСКР значит в ПН
                EveryYear, //Каждый год без погрешности
                EveryYearWithWhenSaturdayThenFridayAndWhenSundayThenMonday
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string>
                    {
                        Resource.OneTimeOnASpecifiedDate,
                        Resource.OneTimeOnASpecifiedDateObscureEnded,
                        Resource.EachMonthTheRequiredNumberIfTheSaturdayOrSundayThenOnMonday,
                        Resource.EachMonthTheRequiredNumberIfFridayOrSaturdayThenThursdayIfSundayOrMondayThenTuesday,
                        Resource.EachMonthIfSaturdayIsFridayIfSundayIsMonday,
                        Resource.EveryYearWithoutError,
                        Resource.EveryYearIfSaturdayIsFridayIfSundayIsMonday
                    };
                    Type = new List<bool> {false, false, true, true, true, true, true};
                }
            }
        }

        public class ThemesBase : ListStringBase
        {
            protected override void Load()
            {
                if (All == null)
                {
                    All = new List<string>
                    {
                        Resource.Default,
                        "Sandstone",
                        "Superhero",
                        "Paper",
                        "Flatly",
                        "Cyborg",
                        "Cosmo",
                        "Cerulean",
                        "Slate",
                        "Readable",
                        "Darkly"
                    };
                }
            }
        }
    }
}