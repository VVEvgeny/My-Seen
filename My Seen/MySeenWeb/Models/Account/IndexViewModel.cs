using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;

namespace MySeenWeb.Models.Account
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public int Lang { get; set; }
        public IEnumerable<SelectListItem> LangList { get; set; }
        public int Rpp { get; set; }
        public IEnumerable<SelectListItem> RppList { get; set; }

        public bool HaveData { get; set; }

        public void LoadSelectList()
        {
            var listItems =
                Defaults.Languages.GetAll()
                    .Select(
                        sel =>
                            new SelectListItem
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
                            new SelectListItem
                            {
                                Text = sel,
                                Value = Defaults.RecordPerPage.GetId(sel).ToString(),
                                Selected = Defaults.RecordPerPage.GetId(sel) == Rpp
                            })
                    .ToList();
            RppList = listItems;
        }
    }
}