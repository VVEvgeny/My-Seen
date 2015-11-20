using System.Collections.Generic;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models
{
    public class HomeViewModelUsers
    {
        public UsersView Map(ApplicationUser model)
        {
            if (model == null) return new UsersView();

            ApplicationDbContext ap = new ApplicationDbContext();

            return new UsersView
            {
                Name = model.UserName.Remove(model.UserName.IndexOf('@')),
                RegiserDate = model.RegisterDate.ToShortDateString(),
                //Culture = model.Culture,
                Culture = (model.Culture == CultureInfoTool.Cultures.English ?
                        Defaults.Languages.GetById(Defaults.LanguagesBase.Indexes.English) : Defaults.Languages.GetById(Defaults.LanguagesBase.Indexes.Russian)
                        ),
                FilmsCount = ap.Films.Count(f => f.UserId == model.Id),
                SerialsCount = ap.Serials.Count(f => f.UserId == model.Id),
                BooksCount = ap.Books.Count(f => f.UserId == model.Id),
                TracksCount = ap.Tracks.Count(f => f.UserId == model.Id)
            };
        }

        public IEnumerable<UsersView> Users;
        public PaginationViewModel Pages { get; set; }
        public HomeViewModelUsers(int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Users.Count(), countInPage, "Home", "");
            Users = ac.Users.Select(Map).OrderBy(l => l.RegiserDate).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
