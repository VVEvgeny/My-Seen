using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models
{
    public class Serials
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public int LastSeason { get; set; }
        public int LastSeries { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateLast { get; set; }
        public int Genre { get; set; }
        public int Rate { get; set; }
        public DateTime? DateChange { get; set; }
        public bool isDeleted { get; set; }
    }
}
