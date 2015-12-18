using System;
using System.ComponentModel.DataAnnotations;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
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
        public int Version { get; set; }
    }
}
