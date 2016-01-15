using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModelEventsMin
    {
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> SelectListEvents { get; set; }

        public HomeViewModelEventsMin(bool onlyEnded)
        {
            TypeList =
                Defaults.EventTypes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EventTypes.GetId(sel).ToString(),
                                Selected = Defaults.EventTypes.GetId(sel) == (int)Defaults.EventsTypesBase.Indexes.OneTime
                            })
                    .ToList();

            SelectListEvents = new List<SelectListItem>
            {
                new SelectListItem {Text = Resource.Active, Value = "0", Selected = !onlyEnded},
                new SelectListItem {Text = Resource.Ended, Value = "1", Selected = onlyEnded}
            }; 
        }
    }
}
