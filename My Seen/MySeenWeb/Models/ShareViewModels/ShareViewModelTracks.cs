using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models.ShareViewModels
{
    /// <summary>
    /// Он грузит только легенду 
    /// основная работа уже при загрузке страницы пойдет работать метод Home/GetTrackByKey(string id)
    /// </summary>
    public class ShareViewModelTracks : ShareViewModelBaseMin
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

        public bool Markers { get; set; }
        public IEnumerable<SelectListItem> YearsList { get; set; }

        public ShareViewModelTracks(string key, int markersOnRoads, int roadsYear)
        {
            Markers = markersOnRoads == Defaults.EnabledDisabledBase.Indexes.Enabled;
            Key = key;
            var userId = string.Empty;
            var ac=new ApplicationDbContext();
            var years = new List<SelectListItem> { 
                new SelectListItem
                {
                    Text = Resource.All,
                    Value = "0",
                    Selected = roadsYear == 0
                } };

            if (ac.Users.Any(u => u.ShareTracksFootKey != null && u.ShareTracksFootKey == key))
            {
                AllUsersTracks = true;
                userId = ac.Users.First(u => u.ShareTracksFootKey == key).Id;
                const int type = (int)TrackTypes.Foot;
                Data = ac.Tracks.Where(t => t.UserId == userId && t.Type == type && (roadsYear == 0 || t.Date.Year == roadsYear)).Select(TracksView.Map);
                foreach (var elem in ac.Tracks.Where(t => t.UserId == userId && t.Type == type).Select(t => t.Date.Year).Distinct())
                {
                    years.Add(new SelectListItem
                    {
                        Text = elem.ToString(),
                        Value = elem.ToString(),
                        Selected = roadsYear == elem
                    });
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksCarKey != null && u.ShareTracksCarKey == key))
            {
                AllUsersTracks = true;
                userId = ac.Users.First(u => u.ShareTracksCarKey == key).Id;
                const int type = (int)TrackTypes.Car;
                Data = ac.Tracks.Where(t => t.UserId == userId && t.Type == type && (roadsYear == 0 || t.Date.Year == roadsYear)).Select(TracksView.Map);
                foreach (var elem in ac.Tracks.Where(t => t.UserId == userId && t.Type == type).Select(t => t.Date.Year).Distinct())
                {
                    years.Add(new SelectListItem
                    {
                        Text = elem.ToString(),
                        Value = elem.ToString(),
                        Selected = roadsYear == elem
                    });
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksCarKey != null && u.ShareTracksBikeKey == key))
            {
                AllUsersTracks = true;
                userId = ac.Users.First(u => u.ShareTracksBikeKey == key).Id;
                const int type = (int)TrackTypes.Bike;
                Data = ac.Tracks.Where(t => t.UserId == userId && t.Type == type && (roadsYear == 0 || t.Date.Year == roadsYear)).Select(TracksView.Map);
                foreach (var elem in ac.Tracks.Where(t => t.UserId == userId && t.Type == type).Select(t => t.Date.Year).Distinct())
                {
                    years.Add(new SelectListItem
                    {
                        Text = elem.ToString(),
                        Value = elem.ToString(),
                        Selected = roadsYear == elem
                    });
                }
            }
            else if (ac.Users.Any(u => u.ShareTracksAllKey != null && u.ShareTracksAllKey == key))
            {
                AllUsersTracks = true;
                userId = ac.Users.First(u => u.ShareTracksAllKey == key).Id;
                Data = ac.Tracks.Where(t => t.UserId == userId && (roadsYear == 0 || t.Date.Year == roadsYear)).Select(TracksView.Map);
                foreach (var elem in ac.Tracks.Where(t => t.UserId == userId).Select(t => t.Date.Year).Distinct())
                {
                    years.Add(new SelectListItem
                    {
                        Text = elem.ToString(),
                        Value = elem.ToString(),
                        Selected = roadsYear == elem
                    });
                }
            }
            else
            {
                AllUsersTracks = false;
                Data = ac.Tracks.Where(t => t.ShareKey == key).Select(TracksView.Map).ToList();
                if (Data.Any())
                {
                    years.Clear();
                    years.Add(new SelectListItem
                    {
                        Text = Data.First().Date.Year.ToString(), Value = Data.First().Date.Year.ToString(), Selected = true
                    });
                    userId = Data.First().UserId;
                }
            }
            YearsList = years.OrderByDescending(y => y.Text);

            if (!string.IsNullOrEmpty(userId)) LoadFromUserId(userId);
        }
    }
}
