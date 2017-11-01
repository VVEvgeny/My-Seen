using System.ComponentModel.DataAnnotations;

namespace MySeenWeb.Models.Tables.Portal
{
    public class Deals
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Amount { get; set; }
    }
}
