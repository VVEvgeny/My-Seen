using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace MySeenWeb.Models.Tables.Portal
{
    public class Realt
    {
        [Key]
        public int Id { get; set; }
        [ScriptIgnore]
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}
