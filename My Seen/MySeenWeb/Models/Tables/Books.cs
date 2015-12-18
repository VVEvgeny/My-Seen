using System;
using System.ComponentModel.DataAnnotations;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class Books
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string Authors { get; set; }
        public DateTime DateRead { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
        public DateTime DateChange { get; set; }
        public int Year { get; set; }
        public bool Shared { get; set; }
    }
}
