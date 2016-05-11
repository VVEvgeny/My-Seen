using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySeenLib;
using MySeenWeb.Models.Meta;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public bool Markers { get; set; }
        public MetaBase Meta { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool HaveLanguage { get; set; }
        public int Theme { get; set; }

        public HomeViewModel(string userId, int markers, int theme, HttpRequestBase request)
        {
            Markers = markers == (int) Defaults.EnabledDisabledBase.Indexes.Enabled;
            Meta = MetaBase.Create(request);
            Theme = theme;

            var logic = new UserRolesLogic();
            UserRoles = logic.GetRoles(userId);

            if (request.UserLanguages != null && request.UserLanguages.Any())
                HaveLanguage =
                    request.UserLanguages.Any(lang => lang.ToLower().Contains("ru") || lang.ToLower().Contains("en"));
            else HaveLanguage = false;
        }
    }
}