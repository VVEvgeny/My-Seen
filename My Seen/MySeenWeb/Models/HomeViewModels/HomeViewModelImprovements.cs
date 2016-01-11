using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
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
                Pages = new PaginationViewModel(page, ac.Bugs.Count(), countInPage);
                Data = ac.Bugs.AsNoTracking()
                    .OrderByDescending(b => b.DateEnd == null)
                    .ThenByDescending(b => b.DateEnd)
                    .ThenByDescending(b => b.DateFound)
                    .Skip(() => (Pages.CurentPage - 1)*countInPage).Take(() => countInPage).Select(BugsView.Map);
            }
            else
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(b => b.Complex == complex), countInPage);
                Data = ac.Bugs.AsNoTracking()
                    .Where(b => b.Complex == complex)
                    .OrderByDescending(b => b.DateEnd == null)
                    .ThenByDescending(b => b.DateEnd)
                    .ThenByDescending(b => b.DateFound)
                    .Skip(() => (Pages.CurentPage - 1)*countInPage).Take(() => countInPage).Select(BugsView.Map);
            }
        }
    }
}
