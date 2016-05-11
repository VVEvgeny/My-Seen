using System;
using System.Globalization;
using MySeenLib;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class EventsView : Events
    {
        public bool HaveHistory
        {
            get { return Defaults.EventTypes.GetTypeById(RepeatType) && Date < DateTime.Now; }
        }

        public DateTime DateTo
        {
            get { return CalculateTo((Defaults.EventsTypesBase.Indexes) RepeatType, Date); }
        }

        public string DayOfWeekTo
        {
            get { return GetDayOfWeek(DateTo); }
        }

        public string EstimatedTo
        {
            get { return GetTimeSpan(DateTo); }
        }

        public long EstimatedTicks
        {
            get { return (DateTo - DateTime.Now).Ticks; }
        }

        public bool IsEnd
        {
            get { return DateTo < DateTime.Now; }
        }

        public DateTime DateLast
        {
            get { return CalculateLast((Defaults.EventsTypesBase.Indexes) RepeatType, Date); }
        }

        public string DayOfWeekLast
        {
            get { return GetDayOfWeek(DateLast); }
        }

        public string EstimatedLast
        {
            get { return GetTimeSpan(DateLast); }
        }

        public string DateToText
        {
            get { return DateTo.ToString(CultureInfo.CurrentCulture); }
        }

        public string DateText
        {
            get { return Date.ToString(CultureInfo.CurrentCulture); }
        }

        public string DateLastText
        {
            get { return DateLast.ToString(CultureInfo.CurrentCulture); }
        }

        private static string GetTimeSpan(DateTime date)
        {
            var ts = date - DateTime.Now;
            var years = 0;
            var days = ts.Days;
            if (days > 365)
            {
                do
                {
                    days -= 365;
                    years++;
                } while (days > 365);
            }
            else
                while (days < -365)
                {
                    days += 365;
                    years--;
                }
            var minus = ts.Hours < 0 || ts.Minutes < 0 || ts.Seconds < 0 || ts.Days < 0;

            if (years < 0) years *= -1;
            if (days < 0) days *= -1;
            var hours = ts.Hours;
            if (hours < 0) hours *= -1;
            var minutes = ts.Minutes;
            if (minutes < 0) minutes *= -1;
            var seconds = ts.Seconds;
            if (seconds < 0) seconds *= -1;

            return (minus ? "- " : string.Empty) +
                   (years == 0 ? string.Empty : years + ":") +
                   (years == 0 && days == 0 ? string.Empty : days + ":") +
                   (years == 0 && days == 0 && hours == 0
                       ? string.Empty
                       : (hours < 10 ? "0" + hours : hours.ToString()) + ":") +
                   (years == 0 && days == 0 && hours == 0 && minutes == 0
                       ? string.Empty
                       : (minutes < 10 ? "0" + minutes : minutes.ToString()) + ":") +
                   (years == 0 && days == 0 && hours == 0 && minutes == 0 && seconds == 0
                       ? string.Empty
                       : (seconds < 10 ? "0" + seconds : seconds.ToString()));
        }

        private static string GetDayOfWeek(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return Resource.Monday;
                case DayOfWeek.Tuesday:
                    return Resource.Tuesday;
                case DayOfWeek.Wednesday:
                    return Resource.Wednesday;
                case DayOfWeek.Thursday:
                    return Resource.Thursday;
                case DayOfWeek.Friday:
                    return Resource.Friday;
                case DayOfWeek.Saturday:
                    return Resource.Saturday;
                case DayOfWeek.Sunday:
                    return Resource.Sunday;
            }
            return string.Empty;
        }

        private static DateTime Correct(Defaults.EventsTypesBase.Indexes typeRepeat, DateTime date)
        {
            switch (typeRepeat)
            {
                case Defaults.EventsTypesBase.Indexes.EveryMonthInNeedDayWithWhenSundayOrSaturdayThenMonday:
                    if (date.DayOfWeek == DayOfWeek.Saturday) date = date.AddDays(1);
                    if (date.DayOfWeek == DayOfWeek.Sunday) date = date.AddDays(1);
                    break;
                case
                    Defaults.EventsTypesBase.Indexes
                        .EveryMonthInNeedDayWithWhenSaturdayOrFridayThenThursdayWhenSundayOrMondayThenTuesday:
                    if (date.DayOfWeek == DayOfWeek.Friday) date = date.AddDays(-1);
                    if (date.DayOfWeek == DayOfWeek.Saturday) date = date.AddDays(-2);
                    if (date.DayOfWeek == DayOfWeek.Sunday) date = date.AddDays(2);
                    if (date.DayOfWeek == DayOfWeek.Monday) date = date.AddDays(1);
                    break;
                case Defaults.EventsTypesBase.Indexes.EveryYearWithWhenSaturdayThenFridayAndWhenSundayThenMonday:
                case Defaults.EventsTypesBase.Indexes.EveryMonthInNeedDayWithWhenSaturdayThenFridayWhenSundayThenMonday:
                    if (date.DayOfWeek == DayOfWeek.Saturday) date = date.AddDays(-1);
                    if (date.DayOfWeek == DayOfWeek.Sunday) date = date.AddDays(1);
                    break;
            }
            return date;
        }

        private static DateTime GetNewDate(int year, int month, int day, int hour, int minute, int second)
        {
            if (month > 12) //add months
            {
                month -= 12;
                year++;
            }
            if (month < 1)
            {
                month += 12;
                year--;
            }
            DateTime d;
            while (true)
            {
                try
                {
                    d = new DateTime(year, month, day, hour, minute, second);
                    break;
                }
                catch (Exception)
                {
                    day--;
                }
            }
            return d;
        }

        private static DateTime CalculateTo(Defaults.EventsTypesBase.Indexes typeRepeat, DateTime beginDate)
        {
            var d = DateTime.Now;
            switch (typeRepeat)
            {
                case Defaults.EventsTypesBase.Indexes.EveryMonthInNeedDayWithWhenSundayOrSaturdayThenMonday:
                case
                    Defaults.EventsTypesBase.Indexes
                        .EveryMonthInNeedDayWithWhenSaturdayOrFridayThenThursdayWhenSundayOrMondayThenTuesday:
                case Defaults.EventsTypesBase.Indexes.EveryMonthInNeedDayWithWhenSaturdayThenFridayWhenSundayThenMonday:
                    d = GetNewDate(DateTime.Now.Year, DateTime.Now.Month, beginDate.Day, beginDate.Hour,
                        beginDate.Minute, beginDate.Second);
                    d = Correct(typeRepeat, d);

                    if (d < DateTime.Now)
                    {
                        d = GetNewDate(DateTime.Now.Year, DateTime.Now.Month + 1, beginDate.Day, beginDate.Hour,
                            beginDate.Minute, beginDate.Second);
                        d = Correct(typeRepeat, d);
                    }
                    break;
                case Defaults.EventsTypesBase.Indexes.OneTime:
                case Defaults.EventsTypesBase.Indexes.OneTimeWithPast:
                    d = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, beginDate.Hour, beginDate.Minute,
                        beginDate.Second);
                    break;
                case Defaults.EventsTypesBase.Indexes.EveryYear:
                    d = new DateTime(DateTime.Now.Year, beginDate.Month, beginDate.Day, beginDate.Hour, beginDate.Minute,
                        beginDate.Second);
                    if (d < DateTime.Now)
                        d = new DateTime(DateTime.Now.Year + 1, beginDate.Month, beginDate.Day, beginDate.Hour,
                            beginDate.Minute, beginDate.Second);
                    break;
                case Defaults.EventsTypesBase.Indexes.EveryYearWithWhenSaturdayThenFridayAndWhenSundayThenMonday:
                    d = new DateTime(DateTime.Now.Year, beginDate.Month, beginDate.Day, beginDate.Hour, beginDate.Minute,
                        beginDate.Second);
                    d = Correct(typeRepeat, d);

                    if (d < DateTime.Now)
                    {
                        d = new DateTime(DateTime.Now.Year, beginDate.Month, beginDate.Day, beginDate.Hour,
                            beginDate.Minute, beginDate.Second);
                        d = d.AddYears(1);
                        d = Correct(typeRepeat, d);
                    }
                    break;
            }
            return d;
        }

        private DateTime CalculateLast(Defaults.EventsTypesBase.Indexes typeRepeat, DateTime beginDate)
        {
            var d = DateTime.Now;
            for (var i = 1; i < 100; i++)
            {
                switch (typeRepeat)
                {
                    case Defaults.EventsTypesBase.Indexes.EveryMonthInNeedDayWithWhenSundayOrSaturdayThenMonday:
                    case
                        Defaults.EventsTypesBase.Indexes
                            .EveryMonthInNeedDayWithWhenSaturdayOrFridayThenThursdayWhenSundayOrMondayThenTuesday:
                    case
                        Defaults.EventsTypesBase.Indexes
                            .EveryMonthInNeedDayWithWhenSaturdayThenFridayWhenSundayThenMonday:

                        d = GetNewDate(DateTime.Now.Year, DateTime.Now.Month, beginDate.Day, beginDate.Hour,
                            beginDate.Minute, beginDate.Second);
                        if (d > DateTime.Now)
                            d = GetNewDate(DateTime.Now.Year, DateTime.Now.Month - i, beginDate.Day, beginDate.Hour,
                                beginDate.Minute, beginDate.Second);
                        d = Correct(typeRepeat, d);
                        return d;

                    case Defaults.EventsTypesBase.Indexes.EveryYear:
                    case Defaults.EventsTypesBase.Indexes.EveryYearWithWhenSaturdayThenFridayAndWhenSundayThenMonday:
                        d = new DateTime(DateTime.Now.Year, beginDate.Month, beginDate.Day, beginDate.Hour,
                            beginDate.Minute, beginDate.Second);
                        if (d > DateTime.Now) d = d.AddYears(-i);
                        d = Correct(typeRepeat, d);
                        return d;
                }
            }
            return d;
        }

        public static EventsView Map(Events model)
        {
            if (model == null) return new EventsView();

            return new EventsView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                Date = UmtTime.From(model.Date),
                RepeatType = model.RepeatType,
                DateChange = UmtTime.From(model.DateChange),
                Shared = model.Shared
            };
        }
    }
}