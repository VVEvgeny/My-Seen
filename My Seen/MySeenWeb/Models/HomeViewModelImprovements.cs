﻿using System;
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
    public class HomeViewModelImprovements
    {
        public PaginationViewModel Pages { get; set; }
        public IEnumerable<BugsView> Bugs { get; set; }
        public IEnumerable<SelectListItem> complexList { get; set; }
        public string Complex;

        public HomeViewModelImprovements(int complex, int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            if (complex == Defaults.ComplexBase.IndexAll)
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Count(), countInPage, "Home", "");
                Bugs = ac.Bugs.Select(BugsView.Map).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
            }
            else
            {
                Pages = new PaginationViewModel(page, ac.Bugs.Where(b => b.Complex == complex).Count(), countInPage, "Home", "");
                Bugs = ac.Bugs.Select(BugsView.Map).Where(b => b.Complex == complex).OrderByDescending(b => b.DateEnd == null).ThenByDescending(b => b.DateEnd).ThenByDescending(b => b.DateFound).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
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