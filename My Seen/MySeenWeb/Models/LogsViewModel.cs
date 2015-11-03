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
    public class LogsViewModel
    {
        public IEnumerable<LogsView> Logs;

        public void Load()
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            Logs = ac.Logs.Select(LogsView.Map).OrderByDescending(l => l.DateLast);
        }
    }
}
