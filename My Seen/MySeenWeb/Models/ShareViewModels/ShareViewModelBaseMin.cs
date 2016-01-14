using System.Linq;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelBaseMin
    {
        public string Key { get; set; }
        public string Owner { get; set; }

        public enum ShareType
        {
            Films,
            Serials,
            Books,
            Events,
            Roads
        }
        public bool VkontakteServicesEnabled { get; set; }
        public bool GoogleServicesEnabled { get; set; }
        public bool FacebookServicesEnabled { get; set; }


        public ShareViewModelBaseMin(string key, string owner, ShareType type)
        {
            Key = key;
            Owner = owner;

            var ac = new ApplicationDbContext();
            if (ac.Users.Any(f => 
                (type == ShareType.Books && f.ShareBooksKey == key)
                ||
                (type == ShareType.Films && f.ShareFilmsKey == key)
                ||
                (type == ShareType.Serials && f.ShareSerialsKey == key)
                ||
                (type == ShareType.Events && f.ShareEventsKey == key)
                ))
            {
                var user = ac.Users.First(f =>
                    (type == ShareType.Books && f.ShareBooksKey == key)
                    ||
                    (type == ShareType.Films && f.ShareFilmsKey == key)
                    ||
                    (type == ShareType.Serials && f.ShareSerialsKey == key)
                    ||
                    (type == ShareType.Events && f.ShareEventsKey == key)
                    );

                VkontakteServicesEnabled = user.VkServiceEnabled;
                GoogleServicesEnabled = user.GoogleServiceEnabled;
                FacebookServicesEnabled = user.FacebookServiceEnabled;
            }
        }
        public void LoadFromUserId(string userId)
        {
            var ac = new ApplicationDbContext();
            if (ac.Users.Any(f => f.Id == userId))
            {
                var user = ac.Users.First(f => f.Id == userId);

                VkontakteServicesEnabled = user.VkServiceEnabled;
                GoogleServicesEnabled = user.GoogleServiceEnabled;
                FacebookServicesEnabled = user.FacebookServiceEnabled;
            }
        }

        protected ShareViewModelBaseMin()
        {
            
        }
    }
}
