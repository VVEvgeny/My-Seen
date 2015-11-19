using System.ComponentModel.DataAnnotations;
using MySeenLib;

namespace MySeenWeb.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof (Resource))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6,
            ErrorMessageResourceType = typeof (Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof (Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof (Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "ThePasswordAndConfirmationPasswordDoNotMatch",
            ErrorMessageResourceType = typeof (Resource))]
        public string ConfirmPassword { get; set; }
    }
}