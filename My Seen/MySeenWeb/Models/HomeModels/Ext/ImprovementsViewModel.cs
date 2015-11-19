using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.Database;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.HomeModels.Ext
{
    public class ImprovementsViewModel
    {
        public string Complex;

        public ImprovementsViewModel(int complex, int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            if (complex == Defaults.ComplexBase.IndexAll)
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(), countInPage, "Home", "");
                Bugs =
                    ac.Bugs.Select(BugsView.Map)
                        .OrderByDescending(b => b.DateEnd == null)
                        .ThenByDescending(b => b.DateEnd)
                        .ThenByDescending(b => b.DateFound)
                        .Skip((Pages.CurentPage - 1)*countInPage)
                        .Take(countInPage);
            }
            else
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(b => b.Complex == complex), countInPage, "Home", "");
                Bugs =
                    ac.Bugs.Select(BugsView.Map)
                        .Where(b => b.Complex == complex)
                        .OrderByDescending(b => b.DateEnd == null)
                        .ThenByDescending(b => b.DateEnd)
                        .ThenByDescending(b => b.DateFound)
                        .Skip((Pages.CurentPage - 1)*countInPage)
                        .Take(countInPage);
            }

            Complex = complex.ToString();
            var listItems =
                Defaults.Complexes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Complexes.GetId(sel).ToString(),
                                Selected = Defaults.Complexes.GetId(sel) == complex
                            })
                    .ToList();
            ComplexList = listItems;
        }

        public PaginationViewModel Pages { get; set; }
        public IEnumerable<BugsView> Bugs { get; set; }
        public IEnumerable<SelectListItem> ComplexList { get; set; }
    }
}