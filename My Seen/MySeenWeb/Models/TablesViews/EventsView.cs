using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.Tables;
using static MySeenLib.Defaults;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews
{
    public class EventsView : Events
    {
        public bool HaveHistory => EventTypes.GetTypeById(RepeatType) && Date < DateTime.Now;

        public DateTime DateTo => EventcCalculationLogic.CalculateTo((EventsTypesBase.Indexes) RepeatType, Date, EventsSkip.Select(f => f.Date).ToList());

        public string DayOfWeekTo => EventcCalculationLogic.GetDayOfWeek(DateTo);

        public string EstimatedTo => EventcCalculationLogic.GetTimeSpan(DateTo);

        public long EstimatedTicks
        {
            get
            {
                if (EventsSkip.Select(f => f.Date).Contains(DateTo) &&
                    ((EventsTypesBase.Indexes) RepeatType == EventsTypesBase.Indexes.OneTime ||
                     (EventsTypesBase.Indexes) RepeatType == EventsTypesBase.Indexes.OneTimeWithPast))
                {
                    return -1;
                }
                return (DateTo - DateTime.Now).Ticks;
            }
        }

        public bool CanBeDelayed
        {
            get
            {
                if ((EventsTypesBase.Indexes)RepeatType == EventsTypesBase.Indexes.OneTime ||
                     (EventsTypesBase.Indexes)RepeatType == EventsTypesBase.Indexes.OneTimeWithPast)
                {
                    return false;
                }
                else if (IsEnd)
                {
                    return false;
                }
                return true;
            }
        }
        public bool IsEnd => DateTo < DateTime.Now;

        public DateTime DateLast => EventcCalculationLogic.CalculateLast((EventsTypesBase.Indexes) RepeatType, Date);

        public string DayOfWeekLast => EventcCalculationLogic.GetDayOfWeek(DateLast);

        public string EstimatedLast => EventcCalculationLogic.GetTimeSpan(DateLast);

        public string DateToText => DateTo.ToString(CultureInfo.CurrentCulture).Remove(DateTo.ToString(CultureInfo.CurrentCulture).Length - 3);

        public string DateText => Date.ToString(CultureInfo.CurrentCulture);

        public string DateLastText => DateLast.ToString(CultureInfo.CurrentCulture).Remove(DateLast.ToString(CultureInfo.CurrentCulture).Length - 3);

        public IEnumerable<string> SkippedTimes => EventcCalculationLogic
            .GetSkippedTimes((EventsTypesBase.Indexes) RepeatType, Date, EventsSkip.Select(f => f.Date).ToList())
            .Select(v => v.ToString(CultureInfo.CurrentCulture)
                .Remove(v.ToString(CultureInfo.CurrentCulture).Length - 3));

        public static EventsView Map(Events model)
        {
            if (model == null) return new EventsView();

            return new EventsView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                Date = From(model.Date),
                RepeatType = model.RepeatType,
                DateChange = From(model.DateChange),
                Shared = model.Shared,
                EventsSkip = model.EventsSkip
            };
        }
       
    }
}