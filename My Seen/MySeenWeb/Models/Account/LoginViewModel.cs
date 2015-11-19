using System.ComponentModel.DataAnnotations;
using MySeenLib;

namespace MySeenWeb.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof (Resource))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof (Resource))]
        public string Password { get; set; }

        [Display(Name = "Remember", ResourceType = typeof (Resource))]
        public bool RememberMe { get; set; }
    }
}