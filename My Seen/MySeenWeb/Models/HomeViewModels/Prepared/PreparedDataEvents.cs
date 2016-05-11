using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataEvents
    {
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> SelectedList { get; set; }
        public string DateTimeNow { get; set; }

        public PreparedDataEvents()
        {
            TypeList =
                EventTypes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = EventTypes.GetId(sel).ToString(),
                                Selected =
                                    EventTypes.GetId(sel) == (int) EventsTypesBase.Indexes.OneTime
                            })
                    .ToList();

            SelectedList = new List<SelectListItem>
            {
                new SelectListItem {Text = Resource.Active, Value = "0", Selected = true},
                new SelectListItem {Text = Resource.Ended, Value = "1", Selected = false}
            };

            DateTimeNow = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }
    }
}