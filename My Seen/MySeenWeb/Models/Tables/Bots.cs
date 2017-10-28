using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models.Tables
{
    public class Bots
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserAgent { get; set; }
        public int Language { get; set; }
    }
}
