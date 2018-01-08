using System.Collections.Generic;
using MySeenResources;

namespace MySeenLib
{
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
                return s == CultureInfoTool.Cultures.English ? (int) Indexes.English : (int) Indexes.Russian;
            }

            public string GetValDb(int i)
            {
                Load();
                return i == (int) Indexes.English ? CultureInfoTool.Cultures.English : CultureInfoTool.Cultures.Russian;
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
                    All = new List<string>
                    {
                        Resource.All,
                        Resource.WEB,
                        Resource.Android,
                        Resource.PC,
                        "2048",
                        Resource.Extension
                    };
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
                    All = Admin.IsDebug
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
                EveryYearWithWhenSaturdayThenFridayAndWhenSundayThenMonday,
                EveryMonth
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