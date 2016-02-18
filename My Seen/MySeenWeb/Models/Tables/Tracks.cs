using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class Tracks
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public string UserId { get; set; }
        [ScriptIgnore]
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        [ScriptIgnore]
        public DateTime Date { get; set; }
        public string Coordinates { get; set; }
        public double Distance { get; set; }
        [ScriptIgnore]
        public string ShareKey { get; set; }
    }
}
