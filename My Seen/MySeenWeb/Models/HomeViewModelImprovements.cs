using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelImprovements
    {
        public PaginationViewModel Pages { get; set; }
        public IEnumerable<BugsView> Bugs { get; set; }
        public IEnumerable<SelectListItem> ComplexList { get; set; }
        public string Complex;

        public HomeViewModelImprovements(int complex, int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if (complex == Defaults.ComplexBase.IndexAll)
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(), countInPage, "Home", "");
                Bugs = ac.Bugs.Select(BugsView.Map).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
            }
            else
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(b => b.Complex == complex), countInPage, "Home", "");
                Bugs = ac.Bugs.Select(BugsView.Map).Where(b => b.Complex == complex).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
            }

            Complex = complex.ToString();
            List<SelectListItem> listItems = Defaults.Complexes.GetAll().Select(sel => new SelectListItem {Text = sel, Value = Defaults.Complexes.GetId(sel).ToString(), Selected = (Defaults.Complexes.GetId(sel) == complex)}).ToList();
            ComplexList = listItems;
        }
    }
}
