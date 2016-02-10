using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelPreload
    {
        public RatingGenreViewModel RatinngGenre { get; set; }
        public IEnumerable<SelectListItem> EventTypeList { get; set; }
        public IEnumerable<SelectListItem> RoadTypeList { get; set; }
        public IEnumerable<SelectListItem> ImprovementTypeList { get; set; }
        public IEnumerable<SelectListItem> EventSelectListEvents { get; set; }

        public HomeViewModelPreload()
        {
            EventTypeList =
                Defaults.EventTypes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EventTypes.GetId(sel).ToString(),
                                Selected =
                                    Defaults.EventTypes.GetId(sel) == (int) Defaults.EventsTypesBase.Indexes.OneTime
                            })
                    .ToList();
            RoadTypeList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Resource.Foot,
                    Value = ((int) TrackTypes.Foot).ToString(),
                    Selected = true
                },
                new SelectListItem
                {
                    Text = Resource.Car,
                    Value = ((int) TrackTypes.Car).ToString(),
                    Selected = false
                },
                new SelectListItem
                {
                    Text = Resource.Bike,
                    Value = ((int) TrackTypes.Bike).ToString(),
                    Selected = false
                }
            };
            ImprovementTypeList =
                Defaults.Complexes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Complexes.GetId(sel).ToString(),
                                Selected = Defaults.Complexes.GetId(sel) == (int) Defaults.ComplexBase.Indexes.All
                            })
                    .ToList();
            EventSelectListEvents = new List<SelectListItem>
            {
                new SelectListItem {Text = Resource.Active, Value = "0", Selected = true},
                new SelectListItem {Text = Resource.Ended, Value = "1", Selected = false}
            };
        }
    }
}
