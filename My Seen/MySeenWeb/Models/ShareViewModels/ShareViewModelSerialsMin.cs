namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelSerialsMin : ShareViewModelBaseMin
    {
        public string Key { get; set; }

        public ShareViewModelSerialsMin(string key):base(key, ShareType.Serials)
        {
            Key = key;
        }
    }
}
