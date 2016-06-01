using System;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using static MySeenLib.Admin;

namespace MySeenWeb.Models.Tools
{
    public static class LogSave
    {
        public static void Save(string userId, string ipAdress, string userAgent, string addData)
        {
            if (!IsDebug)
            {
                var ac = new ApplicationDbContext();
                var date = DateTime.Now.ToShortDateString();

                if (
                    !ac.Logs.Any(
                        l =>
                            l.IPAdress == ipAdress && l.UserAgent == userAgent && l.UserId == userId &&
                            l.OnlyDate == date))
                {
                    ac.Logs.Add(new Logs
                    {
                        IPAdress = ipAdress,
                        UserId = userId,
                        UserAgent = userAgent,
                        OnlyDate = date,
                        DateFirst = DateTime.Now,
                        DateLast = DateTime.Now,
                        AddData = addData,
                        Count = 1
                    });
                }
                else
                {
                    var log =
                        ac.Logs.First(
                            l =>
                                l.IPAdress == ipAdress && l.UserAgent == userAgent && l.UserId == userId &&
                                l.OnlyDate == date);
                    log.DateLast = DateTime.Now;
                    log.Count++;
                    if (!string.IsNullOrEmpty(addData)) log.AddData += "!%!" + addData;
                }
                ac.SaveChanges();
            }
        }
    }
}