using System.Collections.Generic;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelImprovements
    {
        public IEnumerable<BugsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelImprovements(int complex, int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            if (complex == Defaults.ComplexBase.Indexes.All)
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(), countInPage, "Home", "");
                Data = ac.Bugs.Select(BugsView.Map).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
            }
            else
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(b => b.Complex == complex), countInPage, "Home", "");
                Data = ac.Bugs.Select(BugsView.Map).Where(b => b.Complex == complex).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
            }
        }
    }
}
