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
    public class TracksView : Tracks
    {
        public static TracksView Map(Tracks model)
        {
            if (model == null) return new TracksView();

            return new TracksView
            {
                Id = model.Id,
                Date = model.Date,
                Distance = model.Distance,
                Name = model.Name,
                Type = model.Type
            };
        }
        public string DateText
        {
            get
            {
                return Date.ToShortDateString();
            }
        }
        public string DistanceText
        {
            get
            {
                return ((int)(Distance / (CultureInfoTool.GetCulture()==CultureInfoTool.Cultures.English ? 1.66: 1))).ToString() +" "+ Resource.Km;
            }
        }
    }
}
