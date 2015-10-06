using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public static class AFCookies
        {
            public static string CoockieSelectedKey = "eSelected";
            public static string CoockieSelectedValueFilms = eSelected.Films.ToString();
            public static string CoockieSelectedValueSerials = eSelected.Serials.ToString();
        }
        public enum eSelected
        {
            Films,
            Serials
        }
        public eSelected Selected;
        public bool isSelectedFilm()
        {
            if (Selected == eSelected.Films) return true;
            return false;
        }
        public IEnumerable<SelectListItem> selectList { get; set; }
        public HomeViewModel()
        {
            Selected = eSelected.Films;
        }
        public IEnumerable<Films> Films;
        public IEnumerable<Serials> Serials;
        public void LoadSelectList()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (eSelected sel in Enum.GetValues(typeof(eSelected)).Cast<eSelected>())
            {
                listItems.Add(new SelectListItem { Text = sel.ToString(), Value = sel.ToString(), Selected = (Selected == sel) });
            }
            selectList = listItems.AsEnumerable();
        }
        public void LoadFilms(string userId)
        {
            ApplicationDbContext ac= new ApplicationDbContext();
            Films = ac.Films.Where(f => f.UserId == userId).OrderByDescending(f => f.DateSee).AsEnumerable();
        }
        public void LoadSerials(string userId)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Serials = ac.Serials.Where(f => f.UserId == userId).OrderByDescending(f => f.DateLast).AsEnumerable();
        }
    }
}
