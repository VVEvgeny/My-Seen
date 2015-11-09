using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MySeenLib;

namespace MySeenWeb.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public int Lang { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> LangList { get; set; }
        public int RPP { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RPPList { get; set; }

        public bool havedata { get; set; }

        public void LoadSelectList()
        {
            List<System.Web.Mvc.SelectListItem> listItems = new List<System.Web.Mvc.SelectListItem>();
            foreach (string sel in Defaults.Languages.GetAll())
            {
                listItems.Add(new System.Web.Mvc.SelectListItem { Text = sel, Value = Defaults.Languages.GetId(sel).ToString(), Selected = (Defaults.Languages.GetId(sel) == Lang) });
            }
            LangList = listItems;

            listItems = new List<System.Web.Mvc.SelectListItem>();
            foreach(string sel in Defaults.RecordPerPage.GetAll())
            {
                listItems.Add(new System.Web.Mvc.SelectListItem { Text = sel, Value = Defaults.RecordPerPage.GetId(sel).ToString(), Selected = (Defaults.RecordPerPage.GetId(sel) == RPP) });
            }
            RPPList = listItems;
        }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6, ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "ThePasswordAndConfirmationPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resource))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6, ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "ThePasswordAndConfirmationPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}