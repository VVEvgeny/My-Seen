namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelTracksMin : ShareViewModelBaseMin
    {
        public string Key { get; set; }

        public ShareViewModelTracksMin(string key) : base(key, ShareType.Roads)
        {
            Key = key;
        }
    }
}
