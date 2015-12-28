using System.Collections.Generic;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public class HomeViewModelRoadsMin
    {
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public HomeViewModelRoadsMin()
        {
            Type = ((int)TrackTypes.Foot).ToString();

            TypeList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Resource.Foot,
                    Value = ((int) TrackTypes.Foot).ToString(),
                    Selected = true
                },
                new SelectListItem {Text = Resource.Car, Value = ((int) TrackTypes.Car).ToString(), Selected = false},
                new SelectListItem {Text = Resource.Bike, Value = ((int) TrackTypes.Bike).ToString(), Selected = false}
            }; 
        }
    }
}
