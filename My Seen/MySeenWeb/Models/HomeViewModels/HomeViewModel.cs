using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.Meta;
using MySeenWeb.Models.TablesLogic;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public bool Markers { get; set; }
        public MetaBase Meta { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool HaveLanguage { get; set; }
        public Style.Style Style { get; set; }

        public HomeViewModel(string userId, int markers, int theme, int animationEnabled, HttpRequestBase request, ICacheService cache)
        {
            Markers = markers == (int) EnabledDisabledBase.Indexes.Enabled;
            Meta = MetaBase.Create(request, cache);

            Style = new Style.Style(theme, animationEnabled);

            var logic = new UserRolesLogic();
            UserRoles = logic.GetRoles(userId);

            if (request.UserLanguages != null && request.UserLanguages.Any())
                HaveLanguage =
                    request.UserLanguages.Any(lang => lang.ToLower().Contains("ru") || lang.ToLower().Contains("en"));
            else HaveLanguage = false;
        }
    }
}