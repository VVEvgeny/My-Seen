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
    public class HomeViewModelLogs
    {
        public IEnumerable<LogsView> Logs { get; set; }
        public PaginationViewModel Pages { get; set; }

        public HomeViewModelLogs(int page, int countInPage)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Pages = new PaginationViewModel(page, ac.Logs.Count(), countInPage,"Home","");
            Logs = ac.Logs.Select(LogsView.Map).OrderByDescending(l => l.DateLast).Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}