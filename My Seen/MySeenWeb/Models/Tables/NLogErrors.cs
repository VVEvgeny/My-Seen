using System;
using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models.Tables
{
    public class NLogErrors
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string StackTrace { get; set; }
        public string Variables { get; set; }
    }
}
