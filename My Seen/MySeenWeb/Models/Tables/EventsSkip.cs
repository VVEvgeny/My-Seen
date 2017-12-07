using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Script.Serialization;

namespace MySeenWeb.Models.Tables
{
    public class EventsSkip
    {
        [Key]
        [ScriptIgnore]
        public int Id { get; set; }
        //Foreign key for Standard
        [ScriptIgnore]
        public int EventId { get; set; }
        [ScriptIgnore]
        public virtual Events Event { get; set; }
        [ScriptIgnore]
        public DateTime Date { get; set; }

        //public string DateText => Date.ToString(CultureInfo.CurrentCulture).Remove(Date.ToString(CultureInfo.CurrentCulture).Length - 3);
    }
}
