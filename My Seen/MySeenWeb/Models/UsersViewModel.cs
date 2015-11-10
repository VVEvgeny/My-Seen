using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class UsersView
    {
        public string Name { get; set; }
        public string Culture { get; set; }
        public int FilmsCount { get; set; }
        public int SerialsCount { get; set; }
        public int BooksCount { get; set; }
        public string RegiserDate { get; set; }
    }
    public class UsersViewModel
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
                Culture = (model.Culture==CultureInfoTool.Cultures.English ?
                        Defaults.Languages.GetById(Defaults.LanguagesBase.Indexes.English): Defaults.Languages.GetById(Defaults.LanguagesBase.Indexes.Russian)
                        ),
                FilmsCount = ap.Films.Where(f=>f.UserId==model.Id).Count(),
                SerialsCount = ap.Serials.Where(f => f.UserId == model.Id).Count(),
                BooksCount = ap.Books.Where(f => f.UserId == model.Id).Count()
            };
        }

        public IEnumerable<UsersView> Users;
        public PaginationViewModel Pages { get; set; }
        public void Load(int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Users.Count(), countInPage, "Home", "Users");
            Users = ac.Users.Select(Map).OrderBy(l => l.RegiserDate).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
