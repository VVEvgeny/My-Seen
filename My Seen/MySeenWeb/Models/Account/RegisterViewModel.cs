using System.ComponentModel.DataAnnotations;
using MySeenLib;

namespace MySeenWeb.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof (Resource))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6,
            ErrorMessageResourceType = typeof (Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof (Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof (Resource))]
        [Compare("Password", ErrorMessageResourceName = "ThePasswordAndConfirmationPasswordDoNotMatch",
            ErrorMessageResourceType = typeof (Resource))]
        public string ConfirmPassword { get; set; }
    }
}