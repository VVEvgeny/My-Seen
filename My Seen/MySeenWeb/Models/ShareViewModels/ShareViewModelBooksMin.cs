namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelBooksMin: ShareViewModelBaseMin
    {
        public ShareViewModelBooksMin(string key, string owner)
            : base(key, owner, ShareType.Books)
        {
        }
    }
}
