using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;
using MySeenLib;
using System.Globalization;

namespace MySeenWeb.Models
{
    public class HomeViewModelSerials
    {
        public HomeViewModelSerials()
        {

        }
        public HomeViewModelSerials(string userId, int page, int countInPage,ref PaginationViewModel Pages)
        {
            Load(userId, page, countInPage,ref Pages);
        }
        public IEnumerable<SerialsView> Data { get; set; }

        public void Load(string userId, int page, int countInPage,ref PaginationViewModel Pages)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Serials.Where(f => f.UserId == userId && f.isDeleted != true).Count(), countInPage, "Home", "");
            Data = ac.Serials.Where(f => f.UserId == userId && f.isDeleted != true).OrderByDescending(f => f.DateLast).Select(SerialsView.Map).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
