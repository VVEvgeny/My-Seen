using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelEvents
    {
        public IEnumerable<EventsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }
        public bool IsMyData { get; set; }
        public HomeViewModelEvents(string userId, int page, int countInPage, string search, int onlyEnded, string shareKey)
        {
            var ac = new ApplicationDbContext();

            //пока не знаю как 2 условие, причем 1 из них не по пол. таблицы а высчитываемое на основании двух других завернуть в Count
            var data =
                ac.Events.AsNoTracking().Where(f =>
                    ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                     ||
                     (!string.IsNullOrEmpty(shareKey) && f.User.ShareEventsKey == shareKey && f.Shared))
                    && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                    .Select(EventsView.Map)
                    .Where(
                        e =>
                            onlyEnded == 1
                                ? e.EstimatedTicks <= 0 &&
                                  e.RepeatType != (int) Defaults.EventsTypesBase.Indexes.OneTimeWithPast
                                : e.EstimatedTicks > 0 ||
                                  e.RepeatType == (int) Defaults.EventsTypesBase.Indexes.OneTimeWithPast
                    )
                    .OrderBy(e => e.EstimatedTicks);

            Pages = new PaginationViewModel(page, data.Count(), countInPage);
            Data = data.Skip(Pages.SkipRecords).Take(countInPage);
            IsMyData = !string.IsNullOrEmpty(shareKey) && Data.Any() && Data.First().UserId == userId;
        }
    }
}
