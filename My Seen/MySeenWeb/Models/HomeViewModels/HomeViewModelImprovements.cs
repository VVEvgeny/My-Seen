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

        public HomeViewModelImprovements(int complex, int page, int countInPage, string search)
        {
            var ac = new ApplicationDbContext();

            Pages = new PaginationViewModel(page,
                ac.Bugs.Count(
                    b =>
                        (complex == (int) Defaults.ComplexBase.Indexes.All || b.Complex == complex) &&
                        (string.IsNullOrEmpty(search) || b.Text.Contains(search))), countInPage);

            Data = ac.Bugs.AsNoTracking()
                .Where(
                    b =>
                        (complex == (int) Defaults.ComplexBase.Indexes.All || b.Complex == complex) &&
                        (string.IsNullOrEmpty(search) || b.Text.Contains(search)))
                .OrderByDescending(b => b.DateEnd == null)
                .ThenByDescending(b => b.DateEnd)
                .ThenByDescending(b => b.DateFound)
                .Skip(() => Pages.SkipRecords).Take(() => countInPage).Select(BugsView.Map);
        }
    }
}
