using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class Books
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public string UserId { get; set; }
        [ScriptIgnore]
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string Authors { get; set; }
        [ScriptIgnore]
        public DateTime DateRead { get; set; }
        [ScriptIgnore]
        public int Genre { get; set; }
        [ScriptIgnore]
        public int Rating { get; set; }
        [ScriptIgnore]
        public DateTime DateChange { get; set; }
        [ScriptIgnore]
        public int Year { get; set; }
        public bool Shared { get; set; }
    }
}
