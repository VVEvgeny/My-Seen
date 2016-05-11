using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesViews.Portal
{
    public class MemesStatsView
    {
        public bool Select { get; set; }
        public int Plus { get; set; }
        public int Minus { get; set; }

        public static MemesStatsView Map(IEnumerable<MemesStats> model, string userId)
        {
            if (model == null) return new MemesStatsView();

            return new MemesStatsView
            {
                Select = model.Any(m => m.UserId == userId),
                Plus = model.Count(m => m.Plus),
                Minus = model.Count(m => m.Minus)
            };
        }
    }
}