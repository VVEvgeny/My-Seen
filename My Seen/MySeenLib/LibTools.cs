using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;

namespace MySeenLib
{
    public static class Versions
    {
        //Строка с версией библиотеки в ресурсах LibVersionNum
        public static int Web = 21;
        public static int Android = 1;
        public static int AndroidLib = 1;
        public static int Pc = 1;
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
        public static DateTime? To(DateTime? datetime)
        {
            return !datetime.HasValue ? (DateTime?) null : datetime.Value.ToUniversalTime();
        }

        public static DateTime From(DateTime datetime)
        {
            return datetime.ToLocalTime();
        }
        public static DateTime? From(DateTime? datetime)
        {
            return !datetime.HasValue ? (DateTime?) null : datetime.Value.ToLocalTime();
        }
    }
    public static class CultureInfoTool
    {
        public static string CoockieCultureKey = "_culture";
        public static class Cultures
        {
            static Cultures()
            {
                Russian = "ru";
                English = "en";
            }

            public static string English { get; private set; }

            public static string Russian { get; private set; }
        }
        public static string GetFirst2Culture()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString().Substring(0,2);
        }
        public static string GetCulture()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString();
        }
        public static bool SetCulture(string cult)
        {
            if (GetCulture() == cult) return false;
            var culture = new CultureInfo(cult);

            var datetimeformat = culture.DateTimeFormat;
            datetimeformat.LongTimePattern = cult == Cultures.English ? "h:mm:ss tt" : "HH:mm:ss";
            culture.DateTimeFormat = datetimeformat;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Defaults.ReloadResources();

            return true;
        }
    }
    public static class MySeenWebApi
    {
        public static int ApiVersion = 2;
        public static string ApiHost
        {
            get
            {
                return Admin.IsDebug ? "http://localhost:44301" : "http://myseen.by";
            }
        }
        public static string ApiHostAndroid
        {
            get
            {
                return Admin.IsDebug ? "https://10.0.2.2:443" : ApiHost;
            }
        }

        public static string ApiUsers = @"/api/ApiUsers/";
        public static string ApiSync = @"/api/ApiSync/";

        /*
        public static string ShareFilms = @"/Share/Films/";
        public static string ShareTracks = @"/Share/Tracks/";
        public static string ShareEvents = @"/Share/Events/";
        public static string ShareSerials = @"/Share/Serials/";
        public static string ShareBooks = @"/Share/Books/";
        */
        public static string ShareTracks = @"/roads/shared/";
        public static string ShareEvents = @"/events/shared/";
        public static string ShareFilms = @"/films/shared/";
        public static string ShareSerials = @"/serials/shared/";
        public static string ShareBooks = @"/books/shared/";

        public enum SyncModesApiUsers
        {
            IsUserExists = 1
        }
        public enum SyncModesApiData
        {
            GetAll = 1,
            PostAll = 2
        }
        public class SyncJsonAnswer
        {
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
            [JsonProperty("Value")]
            public Values Value { get; set; }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
        public enum DataModes
        {
            Film = 1,
            Serial = 2,
            Book = 3
        }
        public class SyncJsonData
        {
            [JsonProperty("DataMode")]
            public int DataMode { get; set; }//in DataModes
            [JsonProperty("Id")]
            public int? Id { get; set; }
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Genre")]
            public int Genre { get; set; }
            [JsonProperty("Rating")]
            public int Rating { get; set; }
            [JsonProperty("DateSee")]
            public DateTime DateSee { get; set; }
            [JsonProperty("DateChange")]
            public DateTime DateChange { get; set; }
            [JsonProperty("IsDeleted")]
            public bool? IsDeleted { get; set; }

            //serials
            [JsonProperty("LastSeason")]
            public int LastSeason { get; set; }
            [JsonProperty("LastSeries")]
            public int LastSeries { get; set; }
            [JsonProperty("DateLast")]
            public DateTime DateLast { get; set; }
            [JsonProperty("DateBegin")]
            public DateTime DateBegin { get; set; }

            //books
            [JsonProperty("DateRead")]
            public DateTime DateRead { get; set; }
            [JsonProperty("Authors")]
            public string Authors { get; set; }
        }
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
    }
    public static class Defaults
    {
        public abstract class ListStringBase
        {
            protected abstract void Load();

            protected List<string> All;
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
                return All != null ? All.IndexOf(str) : -1;
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
                return All != null ? All.Count - 1 : 0;
            }

            public string GetMaxValue()
            {
                if (All == null) Load();
                if (All != null && All.Count == 0) return "";
                return All != null ? All[GetMaxId()] : "";
            }
        }
        public abstract class ListStringBoolBase: ListStringBase
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
                Memes = 201
            }

            public static bool IsCategoryExt(int category)
            {
                return category == (int)IndexesExt.Users || category == (int)IndexesExt.Logs ||
                       category == (int)IndexesExt.Improvements || category == (int)IndexesExt.Errors;
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
                return s == CultureInfoTool.Cultures.English ? (int)Indexes.English : (int)Indexes.Russian;
            }
            public string GetValDb(int i)
            {
                Load();
                return i == (int)Indexes.English ? CultureInfoTool.Cultures.English : CultureInfoTool.Cultures.Russian;
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
                    All = new List<string> {Resource.All, Resource.WEB, Resource.Android, Resource.PC};
                }
            }
        }
        public class RecordPerPageBase : ListStringBase
        {
            public enum Indexes
            {
                All
            }
            public static class Values
            {
                public static int All = int.MaxValue;
            }

            protected override void Load()
            {
                if (All == null)
                {
                    All = Admin.IsDebug
                        ? new List<string> {Resource.All, "3", "5", "20", "500"}
                        : new List<string> {Resource.All, "20", "50", "100", "500"};
                }
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
                    All = new List<string> { Resource.Enabled, Resource.Disabled };
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
                EveryMonthInNeedDayWithWhenSaturdayOrFridayThenThursdayWhenSundayOrMondayThenTuesday, //Каждый месяц нужного числа, если ПТ или СБ значит ЧТ если ВС или ПН значит ВТ
                EveryMonthInNeedDayWithWhenSaturdayThenFridayWhenSundayThenMonday, //Каждый месяц нужного числа, если СБ или ВСКР значит в ПН
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
                    Type = new List<bool> { false, false, true, true, true, true, true };
                }
            }
        }

        public static readonly GenresBase Genres = new GenresBase();
        public static readonly RatingsBase Ratings = new RatingsBase();
        public static readonly CategoryBase Categories = new CategoryBase();
        public static readonly LanguagesBase Languages = new LanguagesBase();
        public static readonly ComplexBase Complexes = new ComplexBase();
        public static readonly RecordPerPageBase RecordPerPage = new RecordPerPageBase();
        public static readonly EnabledDisabledBase EnabledDisabled = new EnabledDisabledBase();
        public static readonly EventsTypesBase EventTypes = new EventsTypesBase();
        public static readonly RolesBase RolesTypes = new RolesBase();

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
            RolesTypes
        };

        public static void ReloadResources()
        {
            foreach (var val in AllResourcesLink)
            {
                val.Reload();
            }
        }
    }
}
