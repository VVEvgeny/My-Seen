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
    public class BugsViewModel
    {
        public static class AFCookies
        {
            public static string CoockieSelectedKey = "bugsSelected";
        }

        public PaginationViewModel Pages { get; set; }
        public IEnumerable<BugsView> Bugs;
        public IEnumerable<SelectListItem> complexList { get; set; }
        public string Complex;

        public void Load(int complex, int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if (complex == (Defaults.Complexes.GetMaxId()))
            {
                Bugs = ac.Bugs.Select(BugsView.Map).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((page - 1) * countInPage).Take(countInPage);
                Pages = new PaginationViewModel(page, ac.Bugs.Count(), countInPage, "Home", "Bugs");
            }
            else
            {
                Bugs = ac.Bugs.Select(BugsView.Map).Where(b => b.Complex == complex).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((page - 1) * countInPage).Take(countInPage);
                Pages = new PaginationViewModel(page, ac.Bugs.Where(b => b.Complex == complex).Count(), countInPage, "Home", "Bugs");
            }

            Complex = complex.ToString();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (string sel in Defaults.Complexes.GetAll())
            {
                listItems.Add(new SelectListItem { Text = sel, Value = Defaults.Complexes.GetId(sel).ToString(), Selected = (Defaults.Complexes.GetId(sel) == complex) });
            }
            complexList = listItems;
        }
    }
}
