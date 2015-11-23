﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;

namespace MySeenLib
{
    public static class Versions
    {
        //Строка с версией библиотеки в ресурсах LibVersionNum
        public static int Web = 6;
        public static int Android = 1;
        public static int AndroidLib = 1;
        public static int Pc = 1;
    }
    public static class Admin
    {
        public static bool IsAdmin(string userName)
        {
            return userName.ToLower() == "vvevgeny@gmail.com";
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
                #if DEBUG
                    return "http://localhost:44301";
                #else
                    return "http://myseen.by/";
                #endif
            }
        }
        public static string ApiHostAndroid
        {
            get
            {
                #if DEBUG
                    return "https://10.0.2.2:443";
                #else
                    return ApiHost;
                #endif
            }
        }

        public static string ApiUsers = @"/api/ApiUsers/";
        public static string ApiSync = @"/api/ApiSync/";
        public static string ShareTracks = @"/Share/Tracks/";

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
                NoLongerSupportedVersion = 6
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
            public abstract void Load();
            public List<string> All;
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
                if (All != null) return All.IndexOf(str);
                return -1;
            }

            public string GetById(int id)
            {
                if (All == null) Load();
                if (All != null && id >= All.Count) return "";
                if (All != null) return All[id];
                return "";
            }
            public int GetMaxId()
            {
                if (All == null) Load();
                if (All != null) return All.Count - 1;
                return 0;
            }

            public string GetMaxValue()
            {
                if (All == null) Load();
                if (All != null && All.Count == 0) return "";
                if (All != null) return All[GetMaxId()];
                return "";
            }
        }
        public class GenresBase : ListStringBase
        {
            public override void Load()
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
                        Resource.GenreFantastic
                    };
                }
            }
        }
        public class RatingsBase : ListStringBase
        {
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
                }
            }
        }
        public class CategoryBase : ListStringBase
        {
            public static int FilmIndex = 0;
            public static int SerialIndex = 1;
            public static int BookIndex = 2;
            public static int TrackIndex = 3;
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.Films, Resource.Serials, Resource.Books, Resource.Tracks};
                }
            }
        }
        public class LanguagesBase : ListStringBase
        {
            public static class Indexes
            {
                public static int English = 0;
                public static int Russian = 1;
            }
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.English, Resource.Russian};
                }
            }
            public int GetIdDb(string s)
            {
                Load();
                return s == CultureInfoTool.Cultures.English ? Indexes.English : Indexes.Russian;
            }
            public string GetValDb(int i)
            {
                Load();
                return i == Indexes.English ? CultureInfoTool.Cultures.English : CultureInfoTool.Cultures.Russian;
            }
        }
        public class ComplexBase : ListStringBase
        {
            public static int IndexAll = 0;
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.All, Resource.WEB, Resource.Android, Resource.PC};
                }
            }
        }
        public class RecordPerPageBase : ListStringBase
        {
            public static int IndexAll = 0;
            public static int ValAll = int.MaxValue;
            public override void Load()
            {
                if (All == null)
                {
                    All = new List<string> {Resource.All, "20", "50", "100", "500"};
                }
            }
        }

        public static GenresBase Genres = new GenresBase();
        public static RatingsBase Ratings = new RatingsBase();
        public static CategoryBase Categories = new CategoryBase();
        public static LanguagesBase Languages = new LanguagesBase();
        public static ComplexBase Complexes = new ComplexBase();
        public static RecordPerPageBase RecordPerPage = new RecordPerPageBase();

        private static readonly List<ListStringBase> AllResourcesLink = new List<ListStringBase> { Genres, Ratings, Categories, Languages, Complexes, RecordPerPage };

        public static void ReloadResources()
        {
            foreach (var val in AllResourcesLink)
            {
                val.Reload();
            }
        }
    }
    public static class Validations
    {
        public static bool ValidateName(ref string message, string filmName)
        {
            if (filmName.Length < 1)
            {
                message = Resource.ShortUserName;
                return false;
            }
            return true;
        }
        public static bool ValidateUserName(ref string message, string userName)
        {
            if (userName.Length < 5)
            {
                message = Resource.ShortUserName;
                return false;
            }
            return true;
        }
        public static bool ValidateEmail(ref string message, string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                message = Resource.EmailIncorrect;
                return false;
            }
            return true;
        }
        public static bool ValidatePassword(ref string message, string password, string passwordConfirm)
        {
            if (password != passwordConfirm)
            {
                message = Resource.PasswordsNotEqual;
                return false;
            }
            if (password.Length < 6)
            {
                message = Resource.PasswordLength;
                return false;
            }
            if (password.Contains("0") || password.Contains("1") || password.Contains("2") || password.Contains("3") || password.Contains("4") || password.Contains("5") || password.Contains("6") || password.Contains("7") || password.Contains("8") || password.Contains("9"))
            {
                //Потом мож какие другие контроли
            }
            else
            {
                message = Resource.PasswordNOTContainsDigit;
                return false;
            }
            return true;
        }
    }
}
