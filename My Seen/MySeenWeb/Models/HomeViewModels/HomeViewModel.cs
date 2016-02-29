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

        public HomeViewModel(string userId, int markers, HttpRequestBase request)
        {
            Markers = markers == (int) Defaults.EnabledDisabledBase.Indexes.Enabled;
            Meta = MetaBase.Create(request);

            var logic = new UserRolesLogic();
            UserRoles = logic.GetRoles(userId);

            HaveLanguage = false;
            if (request.UserLanguages !=null)
            {
                if (request.UserLanguages.Any())
                {
                    HaveLanguage = request.UserLanguages.Any(lang => lang.ToLower().Contains("ru") || lang.ToLower().Contains("en"));
                }
            }
        }
    }
}
