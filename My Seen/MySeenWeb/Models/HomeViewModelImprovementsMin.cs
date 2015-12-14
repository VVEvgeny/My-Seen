using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class HomeViewModelImprovementsMin
    {
        public IEnumerable<SelectListItem> ComplexList { get; set; }
        public string Complex;

        public HomeViewModelImprovementsMin()
        {
            Complex = Defaults.ComplexBase.Indexes.All.ToString();
            var listItems = Defaults.Complexes.GetAll().Select(sel => new SelectListItem { Text = sel, Value = Defaults.Complexes.GetId(sel).ToString(), Selected = (Defaults.Complexes.GetId(sel) == Defaults.ComplexBase.Indexes.All) }).ToList();
            ComplexList = listItems;
        }
    }
}
