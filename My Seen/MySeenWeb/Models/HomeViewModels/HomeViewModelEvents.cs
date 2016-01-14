using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public bool HaveData
        {
            get { return Data.Any(); }
        }

        public HomeViewModelEvents(string userId, int page, int countInPage, string search, bool onlyEnded)
        {
            var ac = new ApplicationDbContext();

            //пока не знаю как 2 условие, причем 1 из них не по пол. таблицы а высчитываемое на основании двух других завернуть в Count
            var data =
                ac.Events.AsNoTracking().Where(f => f.UserId == userId && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                    .Select(EventsView.Map)
                    .Where(
                        e =>
                            onlyEnded
                                ? e.EstimatedTicks <= 0 &&
                                  e.RepeatType != Defaults.EventsTypesBase.Indexes.OneTimeWithPast
                                : e.EstimatedTicks > 0 ||
                                  e.RepeatType == Defaults.EventsTypesBase.Indexes.OneTimeWithPast)
                    .OrderBy(e => e.EstimatedTicks);

            Pages = new PaginationViewModel(page, data.Count(), countInPage);
            Data = data.Skip(Pages.SkipRecords).Take(countInPage);
        }
        public static string GetShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            var key = ac.Users.AsNoTracking().First(t => t.Id == userId).ShareEventsKey;
            if (!ac.Events.AsNoTracking().First(e => e.Id == iid).Shared) return "-";
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareEvents + key;
        }
        public static string GenerateShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            var key = ac.Users.AsNoTracking().First(t => t.Id == userId).ShareEventsKey;
            ac.Events.AsNoTracking().First(e => e.Id == iid).Shared = true;
            ac.SaveChanges();
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareEvents + key;
        }
        public static string DeleteShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            ac.Events.AsNoTracking().First(e => e.Id == iid).Shared = false;
            ac.SaveChanges();
            return "-";
        }
        
    }
}
