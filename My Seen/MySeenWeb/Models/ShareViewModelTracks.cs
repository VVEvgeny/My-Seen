using System.Collections.Generic;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public class ShareViewModelTracks
    {
        public string Key { get; set; }
        public IEnumerable<TracksView> Data { get; set; }
        public bool AllUsersTracks { get; set; }
        public int Count
        {
            get { return Data.Count(); }
        }
        public double Distance
        {
            get
            {
                return Data.Sum(item => item.Distance);
            }
        }
        public string DistanceText
        {
            get
            {
                return ((int)(Distance / (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English ? 1.66 : 1))).ToString() + " " + Resource.Km;
            }
        }

        public ShareViewModelTracks(string key)
        {
            Key = key;
            var ac=new ApplicationDbContext();
            if (ac.Users.Any(u => u.ShareTracksKey != null && u.ShareTracksKey == key))
            {
                AllUsersTracks = true;
                var userId = ac.Users.First(u => u.ShareTracksKey == key).Id;
                Data = ac.Tracks.Where(t => t.UserId == userId).Select(TracksView.Map);
            }
            else
            {
                AllUsersTracks = false;
                Data = ac.Tracks.Where(t => t.ShareKey == key).Select(TracksView.Map);
            }
        }
    }
}
