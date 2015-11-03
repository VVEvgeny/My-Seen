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
                UserName = model.UserName,
                DateFound = model.DateFound,
                Id = model.Id,
                Text = model.Text,
                TextEnd = model.TextEnd,
                UserId = model.UserId
            };
        }
        public string ComplexText
        {
            get
            {
                return Defaults.Complexes.GetById(Complex);
            }
        }
    }
}
