using System.ComponentModel.DataAnnotations;
using MySeenLib;

namespace MySeenWeb.Models.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof (Resource))]
        public string Email { get; set; }
    }
}