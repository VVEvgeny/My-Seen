using System;
using System.ComponentModel.DataAnnotations;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.Database.Tables
{
    public class Films
    {
        [Key]
        public int Id { get; set; }

        //Foreign key for Standard
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public DateTime DateSee { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
        public DateTime DateChange { get; set; }
        // ReSharper disable once InconsistentNaming
        public bool? isDeleted { get; set; }
    }
}