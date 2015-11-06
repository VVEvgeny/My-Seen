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
    public class BugsView: Bugs
    {
        public static BugsView Map(Bugs model)
        {
            if (model == null) return new BugsView();
            return new BugsView
            {
                DateEnd = model.DateEnd,
                DateFound = model.DateFound,
                Id = model.Id,
                Text = model.Text,
                TextEnd = model.TextEnd,
                UserId = model.UserId,
                Complex = model.Complex
            };
        }
        public string ComplexText
        {
            get
            {
                return Defaults.Complexes.GetById(Complex);
            }
        }
        public string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(UserId))
                {
                    ApplicationDbContext ac = new ApplicationDbContext();
                    string user = ac.Users.Where(u => u.Id == UserId).Select(u => u.UserName).FirstOrDefault();
                    if(string.IsNullOrEmpty(user))return string.Empty;
                    if (user.Contains('@')) user = user.Remove(user.IndexOf('@'));
                    return user;
                }
                return string.Empty;
            }
        }
        public string DateEndText
        {
            get
            {
                if (DateEnd != null)
                {
                    return DateEnd.Value.ToShortDateString();
                }
                return string.Empty;
            }
        }
    }
}
