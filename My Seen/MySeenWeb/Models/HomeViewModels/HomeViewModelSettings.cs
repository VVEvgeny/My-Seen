using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models
{
    public class HomeViewModelSettings
    {
        public bool HasPassword { get; set; }
        public int CountLogins { get; set; }
        public string Lang { get; set; }
        public IEnumerable<SelectListItem> LangList { get; set; }
        public string Rpp { get; set; }
        public string Markers { get; set; }
        public IEnumerable<SelectListItem> RppList { get; set; }

        public IEnumerable<SelectListItem> MarkersOnRoadsList { get; set; }
        public bool HaveData { get; set; }
        public bool VkServiceEnabled { get; set; }

        public string VkServiceEnabledInt
        {
            get
            {
                return VkServiceEnabled
                    ? ((int) Defaults.EnabledDisabledBase.Indexes.Enabled).ToString()
                    : ((int) Defaults.EnabledDisabledBase.Indexes.Disabled).ToString();
            }
        }

        public IEnumerable<SelectListItem> VkServiceEnabledList { get; set; }
        public bool GoogleServiceEnabled { get; set; }

        public string GoogleServiceEnabledInt
        {
            get
            {
                return GoogleServiceEnabled
                    ? ((int) Defaults.EnabledDisabledBase.Indexes.Enabled).ToString()
                    : ((int) Defaults.EnabledDisabledBase.Indexes.Disabled).ToString();
            }
        }

        public IEnumerable<SelectListItem> GoogleServiceEnabledList { get; set; }
        public bool FacebookServiceEnabled { get; set; }

        public string FacebookServiceEnabledInt
        {
            get
            {
                return FacebookServiceEnabled
                    ? ((int) Defaults.EnabledDisabledBase.Indexes.Enabled).ToString()
                    : ((int) Defaults.EnabledDisabledBase.Indexes.Disabled).ToString();
            }
        }

        public IEnumerable<SelectListItem> FacebookServiceEnabledList { get; set; }

        public HomeViewModelSettings(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;
            var ac = new ApplicationDbContext();
            var user = ac.Users.First(u => u.Id == userId);

            Lang = Defaults.Languages.GetIdDb(user.Culture).ToString();
            Rpp = user.RecordPerPage.ToString();
            Markers = user.MarkersOnRoads.ToString();
            VkServiceEnabled = user.VkServiceEnabled;
            GoogleServiceEnabled = user.GoogleServiceEnabled;
            FacebookServiceEnabled = user.FacebookServiceEnabled;
            HasPassword = user.PasswordHash != null;

            var userLogic = new UserLogic();
            CountLogins = userLogic.GetCountLogins(userId);

            LangList =
                Defaults.Languages.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Languages.GetId(sel).ToString(),
                                Selected = Defaults.Languages.GetId(sel).ToString() == Lang
                            })
                    .ToList();
            RppList =
                Defaults.RecordPerPage.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.RecordPerPage.GetId(sel).ToString(),
                                Selected = Defaults.RecordPerPage.GetId(sel).ToString() == Rpp
                            })
                    .ToList();

            MarkersOnRoadsList =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected = Defaults.EnabledDisabled.GetId(sel).ToString() == Markers
                            })
                    .ToList();

            VkServiceEnabledList =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected =
                                    Defaults.EnabledDisabled.GetId(sel) ==
                                    (VkServiceEnabled
                                        ? (int) Defaults.EnabledDisabledBase.Indexes.Enabled
                                        : (int) Defaults.EnabledDisabledBase.Indexes.Disabled)
                            })
                    .ToList();

            GoogleServiceEnabledList =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected =
                                    Defaults.EnabledDisabled.GetId(sel) ==
                                    (GoogleServiceEnabled
                                        ? (int) Defaults.EnabledDisabledBase.Indexes.Enabled
                                        : (int) Defaults.EnabledDisabledBase.Indexes.Disabled)
                            })
                    .ToList();

            FacebookServiceEnabledList =
                Defaults.EnabledDisabled.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.EnabledDisabled.GetId(sel).ToString(),
                                Selected =
                                    Defaults.EnabledDisabled.GetId(sel) ==
                                    (FacebookServiceEnabled
                                        ? (int) Defaults.EnabledDisabledBase.Indexes.Enabled
                                        : (int) Defaults.EnabledDisabledBase.Indexes.Disabled)
                            })
                    .ToList();
        }

    }
}
