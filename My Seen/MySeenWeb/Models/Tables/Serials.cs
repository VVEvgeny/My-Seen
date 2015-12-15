using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models.Tables
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
        public int Rating { get; set; }
        public DateTime DateChange { get; set; }
        public int Year { get; set; }
        public bool Shared { get; set; }
    }
}
