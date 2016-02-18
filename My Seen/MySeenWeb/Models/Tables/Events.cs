using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class Events
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public string UserId { get; set; }
        [ScriptIgnore]
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public int RepeatType { get; set; }
        [ScriptIgnore]
        public DateTime Date { get; set; }
        [ScriptIgnore]
        public DateTime DateChange { get; set; }
        public bool Shared { get; set; }
    }
}
