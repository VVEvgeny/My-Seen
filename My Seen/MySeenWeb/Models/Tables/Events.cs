using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models.Tables
{
    public class Events
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public int RepeatType { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateChange { get; set; }
        public bool Shared { get; set; }
    }
}
