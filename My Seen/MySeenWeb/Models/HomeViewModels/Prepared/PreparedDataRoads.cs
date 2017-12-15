using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using MySeenResources;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataRoads
    {
        public string DateTimeNow { get; } = DateTime.Now.ToString(CultureInfo.CurrentCulture).Remove(DateTime.Now.ToString(CultureInfo.CurrentCulture).Length - 3);

        public IEnumerable<SelectListItem> TypeList { get; } = new List<SelectListItem>
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