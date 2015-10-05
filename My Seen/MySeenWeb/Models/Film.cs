using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public DateTime DateSee { get; set; }
        public int Rate { get; set; }
        public DateTime DateChange { get; set; }

    }
}
