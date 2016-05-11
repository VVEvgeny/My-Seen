using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataRoads
    {
        public string DateTimeNow { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }

        public PreparedDataRoads()
        {
            DateTimeNow = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            TypeList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Resource.Foot,
                    Value = ((int) RoadTypes.Foot).ToString(),
                    Selected = true
                },
                new SelectListItem
                {
                    Text = Resource.Car,
                    Value = ((int) RoadTypes.Car).ToString(),
                    Selected = false
                },
                new SelectListItem
                {
                    Text = Resource.Bike,
                    Value = ((int) RoadTypes.Bike).ToString(),
                    Selected = false
                }
            };
        }
    }
}