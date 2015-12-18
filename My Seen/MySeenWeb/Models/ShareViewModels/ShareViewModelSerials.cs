using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelSerials
    {
        public IEnumerable<SerialsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public bool HaveData
        {
            get { return Data.Any(); }
        }
        public ShareViewModelSerials(string key, int page, int countInPage)
        {
            var ac = new ApplicationDbContext();

            Pages = new PaginationViewModel(page, ac.Serials.Count(f => f.User.ShareSerialsKey == key && f.Shared), countInPage);
            Data = ac.Serials.Where(f => f.User.ShareSerialsKey == key && f.Shared)
                .OrderByDescending(f => f.DateLast)
                .Select(SerialsView.Map)
                .Skip((Pages.CurentPage - 1)*countInPage)
                .Take(countInPage);
        }
    }
}
