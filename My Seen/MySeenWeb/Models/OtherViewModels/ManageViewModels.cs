using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MySeenLib;

namespace MySeenWeb.Models.OtherViewModels
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
        public int Rpp { get; set; }
        public int Markers { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RppList { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> MarkersOnRoadsList { get; set; }
        public bool HaveData { get; set; }

        public bool VkServiceEnabled { get; set; }
        public int VkServiceEnabledInt
        {
            get
            {
                return VkServiceEnabled
                    ? Defaults.EnabledDisabledBase.Indexes.Enabled
                    : Defaults.EnabledDisabledBase.Indexes.Disabled;
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> VkServiceEnabledList { get; set; }
        public bool GoogleServiceEnabled { get; set; }
        public int GoogleServiceEnabledInt
        {
            get
            {
                return GoogleServiceEnabled
                    ? Defaults.EnabledDisabledBase.Indexes.Enabled
                    : Defaults.EnabledDisabledBase.Indexes.Disabled;
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> GoogleServiceEnabledList { get; set; }
        public bool FacebookServiceEnabled { get; set; }
        public int FacebookServiceEnabledInt
        {
            get
            {
                return FacebookServiceEnabled
                    ? Defaults.EnabledDisabledBase.Indexes.Enabled
                    : Defaults.EnabledDisabledBase.Indexes.Disabled;
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> FacebookServiceEnabledList { get; set; }

        public void LoadSelectList()
        {
            var listItems =
                Defaults.Languages.GetAll()
                    .Select(
                        sel =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Languages.GetId(sel).ToString(),
                                Selected = Defaults.Languages.GetId(sel) == Lang
                            })
                    .ToList();
            LangList = listItems;

            listItems =
                Defaults.RecordPerPage.GetAll()
                    .Select(
                        sel =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.RecordPerPage.GetId(sel).ToString(),
                                Selected = Defaults.RecordPerPage.GetId(sel) == Rpp
                            })
                    .ToList();
            RppList = listItems;

            listItems =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected = Defaults.EnabledDisabled.GetId(sel) == Markers
                            })
                    .ToList();
            MarkersOnRoadsList = listItems;

            listItems =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected = Defaults.EnabledDisabled.GetId(sel) == (VkServiceEnabled ? Defaults.EnabledDisabledBase.Indexes.Enabled : Defaults.EnabledDisabledBase.Indexes.Disabled)
                            })
                    .ToList();
            VkServiceEnabledList = listItems;

            listItems =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected = Defaults.EnabledDisabled.GetId(sel) == (GoogleServiceEnabled ? Defaults.EnabledDisabledBase.Indexes.Enabled : Defaults.EnabledDisabledBase.Indexes.Disabled)
                            })
                    .ToList();
            GoogleServiceEnabledList = listItems;

            listItems =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new System.Web.Mvc.SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected = Defaults.EnabledDisabled.GetId(sel) == (FacebookServiceEnabled ? Defaults.EnabledDisabledBase.Indexes.Enabled : Defaults.EnabledDisabledBase.Indexes.Disabled)
                            })
                    .ToList();
            FacebookServiceEnabledList = listItems;
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