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

            Pages = new PaginationViewModel(page, data.Count(), countInPage, "Home", "", routeValues);
            Data = data.Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
