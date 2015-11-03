using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;

namespace MySeenWeb.Models
{
    public class Bugs
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime DateFound { get; set; }
        public string Text { get; set; }
        public DateTime? DateEnd { get; set; }
        public string TextEnd { get; set; }
        public int Complex { get; set; }
    }
}
