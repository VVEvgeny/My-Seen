namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelBooksMin: ShareViewModelBaseMin
    {
        public string Key { get; set; }

        public ShareViewModelBooksMin(string key) : base(key, ShareType.Books)
        {
            Key = key;
        }
    }
}
