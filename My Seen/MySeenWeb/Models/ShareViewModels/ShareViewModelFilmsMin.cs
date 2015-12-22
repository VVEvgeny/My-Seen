namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelFilmsMin : ShareViewModelBaseMin
    {
        public string Key { get; set; }

        public ShareViewModelFilmsMin(string key):base(key, ShareType.Films)
        {
            Key = key;
        }
    }
}
