using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelBooks
    {
        public IEnumerable<BooksView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public bool HaveData
        {
            get { return Data.Any(); }
        }
        public ShareViewModelBooks(string key, int page, int countInPage)
        {
            var ac = new ApplicationDbContext();

            Pages = new PaginationViewModel(page, ac.Books.Count(f => f.User.ShareBooksKey == key && f.Shared), countInPage);
            Data = ac.Books.Where(f => f.User.ShareBooksKey == key && f.Shared)
                .OrderByDescending(f => f.DateRead)
                .Select(BooksView.Map)
                .Skip((Pages.CurentPage - 1)*countInPage)
                .Take(countInPage);
        }
    }
}
