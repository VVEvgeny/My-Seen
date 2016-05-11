using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models
{
    public class HomeViewModelImprovements
    {
        public IEnumerable<BugsView> Data { get; set; }
        public Pagination Pages { get; set; }
        public bool CanControl { get; set; }

        public HomeViewModelImprovements(string userId, int complex, int page, int countInPage, string search, int ended)
        {
            var ac = new ApplicationDbContext();
            CanControl = UserRolesLogic.IsAdmin(userId);

            if (string.IsNullOrEmpty(Complexes.GetById(complex)))
                complex = (int) ComplexBase.Indexes.All;

            Pages = new Pagination(page,
                ac.Bugs.Count(
                    b =>
                        (ended == 0 || (ended == 1 && b.DateEnd == null) || (ended == 2 && b.DateEnd != null)) &&
                        (CanControl || b.UserId == userId) &&
                        (complex == (int) ComplexBase.Indexes.All || b.Complex == complex) &&
                        (string.IsNullOrEmpty(search) || b.Text.Contains(search))), countInPage);

            Data = ac.Bugs.AsNoTracking()
                .Where(
                    b =>
                        (ended == 0 || (ended == 1 && b.DateEnd == null) || (ended == 2 && b.DateEnd != null)) &&
                        (CanControl || b.UserId == userId) &&
                        (complex == (int) ComplexBase.Indexes.All || b.Complex == complex) &&
                        (string.IsNullOrEmpty(search) || b.Text.Contains(search)))
                .OrderByDescending(b => b.DateEnd == null)
                .ThenByDescending(b => b.DateEnd)
                .ThenByDescending(b => b.DateFound)
                .Skip(() => Pages.SkipRecords).Take(() => countInPage).Select(BugsView.Map);
        }
    }
}