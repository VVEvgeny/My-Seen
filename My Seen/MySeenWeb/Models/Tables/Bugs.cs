using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables
{
    public class Bugs
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public string UserId { get; set; }
        [ScriptIgnore]
        public virtual ApplicationUser User { get; set; }
        [ScriptIgnore]
        public DateTime DateFound { get; set; }
        public string Text { get; set; }
        [ScriptIgnore]
        public DateTime? DateEnd { get; set; }
        public string TextEnd { get; set; }
        public int Complex { get; set; }
        public int Version { get; set; }
    }
}
