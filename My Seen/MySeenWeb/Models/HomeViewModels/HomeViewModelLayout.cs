using System.Linq;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews;

namespace MySeenWeb.Models
{
    public static class HomeViewModelLayout
    {
        public static LayoutView GetLayout()
        {
            var ac= new ApplicationDbContext();
            var data = new LayoutView
            {
                ErrorCount = ac.NLogErrors.Count(),
                ImprovementCount = ac.Bugs.Count(b => b.DateEnd == null)
            };

            return data;
        }
    }
}
