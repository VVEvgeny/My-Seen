using System.ComponentModel.DataAnnotations;
using MySeenLib;

namespace MySeenWeb.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof (Resource))]
        public string Email { get; set; }
    }
}