using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModelEventsMin
    {
        public IEnumerable<SelectListItem> TypeList { get; set; }

        public HomeViewModelEventsMin()
        {
            var listItems = Defaults.EventTypes.GetAll().Select(sel => new SelectListItem { Text = sel, Value = Defaults.EventTypes.GetId(sel).ToString(), Selected = Defaults.EventTypes.GetId(sel) == Defaults.EventsTypesBase.Indexes.OneTime }).ToList();
            TypeList = listItems;
        }
    }
}
