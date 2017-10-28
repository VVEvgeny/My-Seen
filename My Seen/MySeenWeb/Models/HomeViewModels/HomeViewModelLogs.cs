using System;
using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models
{
    public class HomeViewModelLogs
    {
        public IEnumerable<LogsView> Data { get; set; }
        public Pagination Pages { get; set; }

        public HomeViewModelLogs(int page, int countInPage, string search, bool withBots, int period, ICacheService cache)
        {
            var ac = new ApplicationDbContext();

            var minDate = DateTime.MinValue;
            var maxDate = DateTime.MaxValue;
            switch (period)
            {
                case 0: //today
                    minDate = To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    maxDate =
                        To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59));
                    break;
                case 1: //yeasterday
                    minDate =
                        To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1));
                    maxDate =
                        To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))
                            .AddDays(-1);
                    break;
                case 10: //This Week
                    minDate =
                        To(
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(
                                DayOfWeek.Monday - DateTime.Now.DayOfWeek));
                    maxDate =
                        To(
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(
                                DayOfWeek.Sunday - DateTime.Now.DayOfWeek + 7));
                    break;
                case 11: //Last Week
                    minDate =
                        To(
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7)
                                .AddDays(DayOfWeek.Monday - DateTime.Now.AddDays(-7).DayOfWeek));
                    maxDate =
                        To(
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(-7)
                                .AddDays(DayOfWeek.Sunday - DateTime.Now.AddDays(-7).DayOfWeek + 7));
                    break;
                case 20: //This Month
                    minDate = To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                    maxDate =
                        To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddSeconds(-1));
                    break;
                case 21: //Last Month
                    minDate = To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1));
                    maxDate = To(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddSeconds(-1));
                    break;
                case 99: //All
                    break;
            }

            Pages = new Pagination(page,
                ac.Logs.AsNoTracking().AsEnumerable().Count(
                    f =>
                        (string.IsNullOrEmpty(search) || f.UserAgent.Contains(search)) && f.DateFirst >= minDate &&
                        f.DateLast <= maxDate
                        && (withBots || !BotsLogic.Contains(cache, f.UserAgent))
                    )
                , countInPage);
            Data =
                ac.Logs.AsNoTracking()
                    .Where(
                        f =>
                            (string.IsNullOrEmpty(search) || f.UserAgent.Contains(search)) && f.DateFirst >= minDate &&
                            f.DateLast <= maxDate)
                    .AsEnumerable().Where(f => withBots || !BotsLogic.Contains(cache, f.UserAgent))
                    .OrderByDescending(l => l.DateLast)
                    .Skip(Pages.SkipRecords)
                    .Take(countInPage)
                    //.Skip(() => Pages.SkipRecords)
                    //.Take(() => countInPage)
                    .Select(LogsView.Map);
        }
    }
}