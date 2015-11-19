using System.Collections.Generic;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.Database;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.HomeModels.Ext
{
    public class UsersViewModel
    {
        public IEnumerable<UsersView> Users;

        public UsersViewModel(int page, int countInPage)
        {
            var ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Users.Count(), countInPage, "Home", "");
            Users =
                ac.Users.Select(Map)
                    .OrderBy(l => l.RegiserDate)
                    .Skip((Pages.CurentPage - 1)*countInPage)
                    .Take(countInPage);
        }

        public PaginationViewModel Pages { get; set; }

        public UsersView Map(ApplicationUser model)
        {
            if (model == null) return new UsersView();

            var ap = new ApplicationDbContext();

            return new UsersView
            {
                Name = model.UserName.Remove(model.UserName.IndexOf('@')),
                RegiserDate = model.RegisterDate.ToShortDateString(),
                //Culture = model.Culture,
                Culture = model.Culture == CultureInfoTool.Cultures.English
                    ? Defaults.Languages.GetById(Defaults.LanguagesBase.Indexes.English)
                    : Defaults.Languages.GetById(Defaults.LanguagesBase.Indexes.Russian),
                FilmsCount = ap.Films.Count(f => f.UserId == model.Id),
                SerialsCount = ap.Serials.Count(f => f.UserId == model.Id),
                BooksCount = ap.Books.Count(f => f.UserId == model.Id),
                TracksCount = ap.Tracks.Count(f => f.UserId == model.Id)
            };
        }
    }
}