using System;
using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models.Tables
{
    public class Logs
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string IPAdress { get; set; }
        public string OnlyDate { get; set; }
        public DateTime DateFirst { get; set; }
        public DateTime DateLast { get; set; }
        public int Count { get; set; }
        public string PageName { get; set; }
        public string AddData { get; set; }
    }
}
