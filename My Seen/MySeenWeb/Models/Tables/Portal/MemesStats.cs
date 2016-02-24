using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.Tables.Portal
{
    public class MemesStats
    {
        [Key]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public string UserId { get; set; }
        [ScriptIgnore]
        public virtual ApplicationUser User { get; set; }
        [ScriptIgnore]
        public int MemesId { get; set; }
        [ScriptIgnore]
        public virtual Memes Memes { get; set; }
        public bool Plus { get; set; }
        public bool Minus { get; set; }
    }
}
