using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.TablesViews.Portal
{
    public class MemesView : Memes
    {
        public string DateText => Date.ToString(CultureInfo.CurrentCulture);

        public string UserName => UserName_;

        private string UserName_ { get; set; }

        private MemesStatsView Stats_ { get; set; }

        public MemesStatsView Stats => Stats_;

        public static IEnumerable<MemesView> Map(IEnumerable<Memes> model, string userId)
        {
            var list = new List<MemesView>();
            list.AddRange(model.Select(elem => Map(elem, userId)));
            return list;
        }

        private static MemesView Map(Memes model, string userId)
        {
            if (model == null) return new MemesView();

            var ap = new ApplicationDbContext();

            return new MemesView
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
                Date = From(model.Date),
                Image = model.Image,
                UserName_ =
                    ap.Users.Any(u => u.Id == model.UserId)
                        ? ap.Users.FirstOrDefault(u => u.Id == model.UserId)?.UserName
                        : "",
                Stats_ = MemesStatsView.Map(ap.MemesStats.Where(m => m.MemesId == model.Id), userId)
            };
        }
    }
}