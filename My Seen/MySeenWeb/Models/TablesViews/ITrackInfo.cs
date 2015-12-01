using System.Collections.Generic;
using System.Linq;

namespace MySeenWeb.Models.TablesViews
{
    public class ITrackInfo
    {
        public List<Location> Path { get; set; }
        public Location Min { get; set; }
        public Location Center { get; set; }
        public Location Max { get; set; }
        public Location Start
        {
            get { return Path[0]; }
        }
        public Location End
        {
            get { return Path[Path.Count - 1]; }
        }

        public virtual void CallcMinMaxCenter()
        {
            if (!Path.Any()) return;
            var minX = Path[0].Latitude;
            var maxX = Path[0].Latitude;

            var minY = Path[0].Longitude;
            var maxY = Path[0].Longitude;

            foreach (var l in Path)
            {
                if (minX > l.Latitude) minX = l.Latitude;
                if (maxX < l.Latitude) maxX = l.Latitude;
                if (maxY < l.Longitude) maxY = l.Longitude;
                if (minY > l.Longitude) minY = l.Longitude;
            }
            Max = new Location(maxX, maxY);
            Min = new Location(minX, minY);
            Center = new Location((maxX + minX) / 2, (maxY + minY) / 2);
        }
    }
}