using System.Collections.Generic;

namespace MySeenWeb.Models.TablesViews
{
    public class ShareTrackInfo : ITrackInfo
    {
        public IEnumerable<List<Location>> Data { get; set; }

        public override void CallcMinMaxCenter()
        {
            Path = new List<Location>();
            foreach (var item in Data) Path.AddRange(item);
            base.CallcMinMaxCenter();
        }
    }
}