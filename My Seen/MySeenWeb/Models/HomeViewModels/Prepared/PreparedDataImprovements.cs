using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenResources;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.Prepared
{
    public class PreparedDataImprovements
    {
        public int CurrentVersion = Versions.Version;

        public IEnumerable<SelectListItem> SelectedList { get; } =
            Complexes.GetAll()
                .Select(
                    sel =>
                        new SelectListItem
                        {
                            Text = sel,
                            Value = Complexes.GetId(sel).ToString(),
                            Selected = Complexes.GetId(sel) == (int) ComplexBase.Indexes.All
                        })
                .ToList();

        public IEnumerable<SelectListItem> SelectedListForAdd { get; } = Complexes.GetAll()
            .Select(
                sel =>
                    new SelectListItem
                    {
                        Text = sel,
                        Value = Complexes.GetId(sel).ToString(),
                        Selected = Complexes.GetId(sel) == (int) ComplexBase.Indexes.All
                    }).Where(l => l.Value != "0")
            .ToList();

        public IEnumerable<SelectListItem> TypeList { get; } = new List<SelectListItem>
        {
            new SelectListItem {Text = Resource.All, Value = "0", Selected = true},
            new SelectListItem {Text = Resource.Active, Value = "1", Selected = false},
            new SelectListItem {Text = Resource.Ended, Value = "2", Selected = false}
        };
    }
}