using System.Linq;
using MySeenWeb.Models.Database;
using MySeenWeb.Models.Database.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class LogsView : Logs
    {
        public string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(UserId))
                {
                    var ac = new ApplicationDbContext();
                    var firstOrDefault = ac.Users.FirstOrDefault(u => u.Id == UserId);
                    if (firstOrDefault != null)
                    {
                        var user = firstOrDefault.UserName;
                        if (string.IsNullOrEmpty(user)) return string.Empty;
                        if (user.Contains('@')) user = user.Remove(user.IndexOf('@'));
                        return user;
                    }
                }
                return string.Empty;
            }
        }

        public static LogsView Map(Logs model)
        {
            if (model == null) return new LogsView();

            return new LogsView
            {
                AddData = model.AddData,
                Count = model.Count,
                DateFirst = model.DateFirst,
                DateLast = model.DateLast,
                Id = model.Id,
                IPAdress = model.IPAdress,
                OnlyDate = model.OnlyDate,
                PageName = model.PageName,
                UserAgent = model.UserAgent,
                UserId = model.UserId
            };
        }
    }
}