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
    public class LogsView: Logs
    {
        public static LogsView Map(Logs model)
        {
            if (model == null) return new LogsView();

            return new LogsView
            {
                AddData = model.AddData,
                Count = model.Count,
                DateFirst = model.DateFirst,
                DateLast = model.DateLast,
                Id = model.Id,
                IPAdress = model.IPAdress,
                OnlyDate = model.OnlyDate,
                PageName = model.PageName,
                UserAgent = model.UserAgent,
                UserId = model.UserId
            };
        }
        public string UserName
        {
            get
            {
                if(!string.IsNullOrEmpty(UserId))
                {
                    ApplicationDbContext ac = new ApplicationDbContext();
                    string user = ac.Users.Where(u => u.Id == UserId).Select(u => u.UserName).FirstOrDefault();
                    if (string.IsNullOrEmpty(user)) return string.Empty;
                    if (user.Contains('@')) user = user.Remove(user.IndexOf('@'));
                    return user;
                }
                return string.Empty;
            }
        }
    }
}
