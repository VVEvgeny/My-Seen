using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesViews.Portal
{
    public class DealsView : Deals
    {
        public static IEnumerable<DealsView> Make(IEnumerable<RealtView> realts, List<Deals> deals, int dealsChange)
        {
            //var lastDealNullable = deals.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month).FirstOrDefault()?.Amount;
            //var lastDeal = lastDealNullable ?? 0;

            return realts.Select(realt => new DealsView
            {
                Year = realt.Date.Year,
                Month = realt.Date.Month,
                Amount =
                    deals.Any(s => s.Year == realt.Date.Year && s.Month == realt.Date.Month)
                        ? deals.First(s => s.Year == realt.Date.Year && s.Month == realt.Date.Month).Amount
                        : 0
                        /*
                        lastDeal + dealsChange < 0
                            ? 0
                            : (lastDeal += dealsChange)
                            */
            });
        }
    }
}