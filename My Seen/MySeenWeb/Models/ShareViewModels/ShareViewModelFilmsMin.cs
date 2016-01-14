namespace MySeenWeb.Models.ShareViewModels
{
    public class ShareViewModelFilmsMin : ShareViewModelBaseMin
    {
        public ShareViewModelFilmsMin(string key, string owner)
            : base(key, owner, ShareType.Films)
        {
        }
    }
}
