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
    public static class LogSave
    {
        private static string GetOnlyDateNow()
        {
            return DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString();
        }
        public static void Save(string userId, string ipAdress, string userAgent, string pageName)
        {
            Save(userId, ipAdress, userAgent, pageName, string.Empty);
        }
        public static void Save(string userId, string ipAdress, string userAgent, string pageName, string addData)
        {
            ApplicationDbContext ac = new ApplicationDbContext();
            string date = GetOnlyDateNow();

            if (ac.Logs.Where(l => l.IPAdress == ipAdress && l.UserAgent == userAgent && l.UserId == userId && l.OnlyDate == date && l.PageName == pageName).Count() == 0)
            {
                ac.Logs.Add(new Logs { IPAdress = ipAdress, UserId = userId, UserAgent = userAgent, OnlyDate = date, DateFirst = DateTime.Now, DateLast = DateTime.Now, PageName=pageName, AddData=addData});
            }
            else
            {
                Logs log = ac.Logs.Where(l => l.IPAdress == ipAdress && l.UserAgent == userAgent && l.UserId == userId && l.OnlyDate == date && l.PageName == pageName).First();
                log.DateLast = DateTime.Now;
                log.Count++;
                if (!string.IsNullOrEmpty(addData)) log.AddData += "!%!" + addData;
            }
            ac.SaveChanges();
        }
    }
}
