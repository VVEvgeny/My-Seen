using System;
using System.Security.Policy;
using System.Web;
using MySeenLib;
using MySeenWeb.Models.Meta;
using MySeenWeb.Models.TablesLogic.Portal;

namespace MySeenWeb.Models
{
    public class HomeViewModel
    {
        public bool Markers { get; set; }
        public MetaBase Meta { get; set; }

        public HomeViewModel(int markers, HttpRequestBase request)
        {
            Markers = markers == (int) Defaults.EnabledDisabledBase.Indexes.Enabled;

            if (MetaBase.IsBot(request.UserAgent))
            {
                if (request.Path.ToLower().Contains(MetaPortalMemes.Path))
                {
                    Meta = new MetaPortalMemes(request);
                }
                else
                {
                    Meta = new MetaBase(request);
                }
            }
            else
            {
                Meta = new MetaBase(request);
            }
        }
    }
}
