using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
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
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public string ShareKey { get; set; }

        public HomeViewModelEvents(string userId, int page, int countInPage, string search)
        {
            var routeValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(search))
            {
                routeValues.Add("search", search);
            }

            var ac = new ApplicationDbContext();
            var listItems = Defaults.EventTypes.GetAll().Select(sel => new SelectListItem { Text = sel, Value = Defaults.EventTypes.GetId(sel).ToString(), Selected = Defaults.EventTypes.GetId(sel) == Defaults.EventsTypesBase.Indexes.OneTime }).ToList();
            TypeList = listItems;

            //пока не знаю как 2 условие, причем 1 из них не по пол. таблицы а высчитываемое на основании двух других завернуть в Count
            var data =
                ac.Events.Where(f => f.UserId == userId && (string.IsNullOrEmpty(search) || f.Name.Contains(search)))
                    .Select(EventsView.Map)
                    .Where(e => e.EstimatedTicks > 0 || e.RepeatType == Defaults.EventsTypesBase.Indexes.OneTimeWithPast)
                    .OrderBy(e => e.EstimatedTicks);

            ShareKey = ac.Users.First(u => u.Id == userId).ShareEventsKey;

            Pages = new PaginationViewModel(page, data.Count(), countInPage, "Home", "", routeValues);
            Data = data.Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
        public static string GetShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            var key = ac.Users.First(t => t.Id == userId).ShareEventsKey;
            if (!ac.Events.First(e => e.Id == iid).Shared) return "-";
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareEvents + key;
        }
        public static string GenerateShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            var key = ac.Users.First(t => t.Id == userId).ShareEventsKey;
            ac.Events.First(e => e.Id == iid).Shared = true;
            ac.SaveChanges();
            return MySeenWebApi.ApiHost + MySeenWebApi.ShareEvents + key;
        }
        public static string DeleteShare(string id, string userId)
        {
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);
            ac.Events.First(e => e.Id == iid).Shared = false;
            ac.SaveChanges();
            return "-";
        }
        
    }
}
