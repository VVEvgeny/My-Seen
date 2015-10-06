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
    public class AllFilms
    {
        public static class AFCookies
        {
            public static string CoockieSelectedKey = "eCoockieSelected";
            public static string CoockieSelectedValueFilms = "films";
            public static string CoockieSelectedValueSerials = "serials";
        }
        public enum eSelected
        {
            Films =1,
            Serials =2
        }
        public eSelected Selected;
        public IEnumerable<SelectListItem> selectTypesList { get; set; }
        public AllFilms()
        {
            Selected = eSelected.Films;
            List<SelectListItem> listItems= new List<SelectListItem>();
            foreach(eSelected sel in Enum.GetValues(typeof(eSelected)).Cast<eSelected>())
            {
                listItems.Add(new SelectListItem { Text = sel.ToString(), Value = sel.ToString() });
            }

            selectTypesList = listItems.AsEnumerable();
        }
        public IEnumerable<Film> Films;
        public void LoadFilms(string userId)
        {
            ApplicationDbContext ac= new ApplicationDbContext();
            Films = ac.Films.Where(f => f.UserId == userId).OrderByDescending(f => f.DateSee).AsEnumerable();
        }
    }
     
    public class Film
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public DateTime DateSee { get; set; }
        public int Rate { get; set; }
        public DateTime DateChange { get; set; }

    }
}
