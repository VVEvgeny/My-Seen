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

        public IEnumerable<string> UserRoles;

        public HomeViewModel(string userId, int markers, HttpRequestBase request)
        {
            Markers = markers == (int) Defaults.EnabledDisabledBase.Indexes.Enabled;
            Meta = MetaBase.Create(request);

            var logic = new UserRolesLogic();
            UserRoles = logic.GetRoles(userId);
        }
    }
}
