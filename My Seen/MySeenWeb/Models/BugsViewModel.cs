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

        public IEnumerable<BugsView> Bugs;
        public IEnumerable<SelectListItem> complexList { get; set; }
        public string Complex;

        public void Load(int complex)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if (complex == (Defaults.Complexes.GetMaxId() + 1))
            {
                Bugs = ac.Bugs.Select(BugsView.Map).OrderByDescending(b => b.DateFound);
            }
            else
            {
                Bugs = ac.Bugs.Select(BugsView.Map).Where(b => b.Complex == complex).OrderByDescending(b => b.DateFound);
            }

            Complex = complex.ToString();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (string sel in Defaults.Complexes.GetAll())
            {
                listItems.Add(new SelectListItem { Text = sel, Value = Defaults.Complexes.GetId(sel).ToString(), Selected = (Defaults.Complexes.GetId(sel) == complex) });
            }
            listItems.Add(new SelectListItem { Text = Resource.All, Value = (Defaults.Complexes.GetMaxId() + 1).ToString(), Selected = ((Defaults.Complexes.GetMaxId() + 1) == complex) });
            complexList = listItems;
        }
    }
}
