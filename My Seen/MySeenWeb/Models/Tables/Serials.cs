using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class Serials
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public string UserId { get; set; }
        [ScriptIgnore]
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public int LastSeason { get; set; }
        public int LastSeries { get; set; }
        [ScriptIgnore]
        public DateTime DateBegin { get; set; }
        [ScriptIgnore]
        public DateTime DateLast { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
        [ScriptIgnore]
        public DateTime DateChange { get; set; }
        [ScriptIgnore]
        public int Year { get; set; }
        public bool Shared { get; set; }
    }
}
