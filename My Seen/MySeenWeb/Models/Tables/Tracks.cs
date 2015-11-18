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
    public class Tracks
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public string Coordinates { get; set; }
        public double Distance { get; set; }
        public string ShareKey { get; set; }
    }
}
