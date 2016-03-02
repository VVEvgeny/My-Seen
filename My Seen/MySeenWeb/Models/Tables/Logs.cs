using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace MySeenWeb.Models.Tables
{
    public class Logs
    {
        [Key]
        public int Id { get; set; }
        [ScriptIgnore]
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public string OnlyDate { get; set; }
        [ScriptIgnore]
        public DateTime DateFirst { get; set; }
        [ScriptIgnore]
        public DateTime DateLast { get; set; }
        public int Count { get; set; }
        public string AddData { get; set; }
    }
}
