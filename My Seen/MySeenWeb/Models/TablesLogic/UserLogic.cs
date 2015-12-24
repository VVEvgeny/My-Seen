using System.Linq;
using MySeenWeb.Models.OtherViewModels;

namespace MySeenWeb.Models.TablesLogic
{
    public class UserLogic : ApplicationUser
    {
        private readonly ApplicationDbContext _ac;
        public string ErrorMessage;
        public UserLogic()
        {
            ErrorMessage = string.Empty;
            _ac = new ApplicationDbContext();
        }

        public bool IsExist(string email)
        {
            return _ac.Users.Any(u => u.Email == email);
        }

        public bool IsExistInProvider(string provider, string key)
        {
            return _ac.UserLogins.Any(u => u.LoginProvider == provider && u.ProviderKey == key);
        }

        public string GetEmailByProvider(string provider, string key)
        {
            var userId = _ac.UserLogins.First(ul => ul.LoginProvider == provider && ul.ProviderKey == key).UserId;
            return _ac.Users.First( u => u.Id == userId).Email;
        }
    }
}
