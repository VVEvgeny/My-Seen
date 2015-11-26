using System.Collections.Generic;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public class HomeViewModelTrackEditor
    {
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public HomeViewModelTrackEditor(string userId)
        {
            TypeList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Resource.FootBike,
                    Value = ((int) TrackTypes.Foot).ToString(),
                    Selected = true
                },
                new SelectListItem {Text = Resource.Car, Value = ((int) TrackTypes.Car).ToString(), Selected = false}
            }; 
        }
    }
}
