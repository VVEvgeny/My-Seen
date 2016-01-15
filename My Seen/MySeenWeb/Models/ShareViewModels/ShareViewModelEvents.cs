using System.Collections.Generic;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelEvents
    {
        public IEnumerable<EventsView> Data { get; set; }
        public PaginationViewModel Pages { get; set; }

        public bool HaveData
        {
            get { return Data.Any(); }
        }
        public ShareViewModelEvents(string key, int page, int countInPage)
        {
            var ac = new ApplicationDbContext();

            var data =
                ac.Events.Where(f => f.User.ShareEventsKey == key && f.Shared)
                    .Select(EventsView.Map)
                    .Where(e => e.EstimatedTicks > 0 || e.RepeatType == (int)Defaults.EventsTypesBase.Indexes.OneTimeWithPast)
                    .OrderBy(e => e.EstimatedTicks);
            
            Pages = new PaginationViewModel(page, data.Count(), countInPage);
            Data = data.Skip((Pages.CurentPage - 1) * countInPage).Take(countInPage);
        }
    }
}
