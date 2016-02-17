using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelBooks
    {
        public IEnumerable<BooksView> Data { get; set; }
        public Pagination Pages { get; set; }
        public bool IsMyData { get; set; }
        public HomeViewModelBooks(string userId, int page, int countInPage, string search, string shareKey)
        {
            var ac = new ApplicationDbContext();
            Pages = new Pagination(page, ac.Books.Count(f =>
                ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                 ||
                 (!string.IsNullOrEmpty(shareKey) && f.User.ShareBooksKey == shareKey && f.Shared))
                && (string.IsNullOrEmpty(search) || f.Name.Contains(search))), countInPage);

            Data = ac.Books.AsNoTracking().Where(f =>
                ((string.IsNullOrEmpty(shareKey) && f.UserId == userId)
                 ||
                 (!string.IsNullOrEmpty(shareKey) && f.User.ShareBooksKey == shareKey && f.Shared))
                && (string.IsNullOrEmpty(search) || f.Name.Contains(search))).OrderByDescending(f => f.DateRead)
                .Skip(() => Pages.SkipRecords).Take(() => countInPage).Select(BooksView.Map);

            IsMyData = !string.IsNullOrEmpty(shareKey) && Data.Any() && Data.First().UserId == userId;
        }
    }
}
