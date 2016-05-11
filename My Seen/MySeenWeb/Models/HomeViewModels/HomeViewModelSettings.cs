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
        public IEnumerable<SelectListItem> Themes { get; set; }
        public string Theme { get; set; }

        public HomeViewModelSettings(string userId, int lang, int rpp, int theme)
        {
            if (string.IsNullOrEmpty(userId))
            {
                Lang = lang.ToString();
                Rpp = Defaults.RecordPerPage.GetId(rpp.ToString()).ToString();
                Theme = theme.ToString();
            }
            else
            {
                var ac = new ApplicationDbContext();
                var user = ac.Users.First(u => u.Id == userId);

                Lang = Defaults.Languages.GetIdDb(user.Culture).ToString();
                Rpp = user.RecordPerPage.ToString();
                Markers = user.MarkersOnRoads.ToString();
                HasPassword = user.PasswordHash != null;
                Theme = user.Theme.ToString();

                var userLogic = new UserLogic();
                CountLogins = userLogic.GetCountLogins(userId);

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
            }

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

            Themes =
                Defaults.Themes.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.Themes.GetId(sel).ToString(),
                                Selected = Defaults.Themes.GetId(sel).ToString() == Theme
                            })
                    .ToList();
        }
    }
}