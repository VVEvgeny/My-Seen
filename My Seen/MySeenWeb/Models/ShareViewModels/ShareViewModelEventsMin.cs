namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelEventsMin : ShareViewModelBaseMin
    {
        public string Key { get; set; }

        public ShareViewModelEventsMin(string key):base(key, ShareType.Events)
        {
            Key = key;
        }
    }
}
