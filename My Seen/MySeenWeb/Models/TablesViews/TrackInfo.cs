using System;

namespace MySeenWeb.Models.TablesViews
{
    public class TrackInfo : ITrackInfo
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string DateText
        {
            get { return Date.ToShortDateString(); }
        }
    }
}