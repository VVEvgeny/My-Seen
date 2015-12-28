using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;

namespace MySeenWeb.Models.TablesViews
{
    public class LogsView : Logs
    {
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
                        string user = firstOrDefault.UserName;
                        if (string.IsNullOrEmpty(user)) return string.Empty;
                        if (user.Contains('@')) user = user.Remove(user.IndexOf('@'));
                        return user;
                    }
                }
                return string.Empty;
            }
        }

        public string DateFirstText 
        {
            get { return DateFirst.ToString(CultureInfo.CurrentCulture); }
        }

        public string DateLastText
        {
            get { return DateLast.ToString(CultureInfo.CurrentCulture); }
        }

        public IEnumerable<string> AddDataText
        {
            get
            {
                var result = new List<string>();
                if (AddData.Length != 0) result.AddRange(AddData.Replace("_", " ").Replace("!%!", "_").Split('_'));
                return result;
            }
        }
    }
}
