using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables;
using static MySeenLib.Defaults;

namespace MySeenWeb.Models.TablesViews
{
    public class BugsView : Bugs
    {
        public string ComplexText => Complexes.GetById(Complex);

        public string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(UserId))
                {
                    var ac = new ApplicationDbContext();
                    if (ac.Users.Any(u => u.Id == UserId))
                    {
                        return ac.Users.First(u => u.Id == UserId).UserName;
                    }
                }
                return string.Empty;
            }
        }

        public string DateEndText => DateEnd?.ToShortDateString() ?? string.Empty;

        public string DateFoundText => DateFound.ToShortDateString();

        public string VersionText => Version != 0 ? Version.ToString() : string.Empty;

        public bool Ended => !string.IsNullOrEmpty(TextEnd);

        public static BugsView Map(Bugs model)
        {
            if (model == null) return new BugsView();
            return new BugsView
            {
                DateEnd = model.DateEnd,
                DateFound = model.DateFound,
                Id = model.Id,
                Text = model.Text,
                TextEnd = model.TextEnd,
                UserId = model.UserId,
                Complex = model.Complex,
                Version = model.Version
            };
        }
    }
}