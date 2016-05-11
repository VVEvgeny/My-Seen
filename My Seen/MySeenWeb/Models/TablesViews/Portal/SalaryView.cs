using System.Collections.Generic;
using System.Linq;
using MySeenWeb.Models.Tables.Portal;

namespace MySeenWeb.Models.TablesViews.Portal
{
    public class SalaryView : Salary
    {
        public static IEnumerable<SalaryView> Make(IEnumerable<RealtView> realts, List<Salary> salarys, int salaryChange)
        {
            var lastSalary = salarys.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month).First().Amount;

            return realts.Select(realt => new SalaryView
            {
                Year = realt.Date.Year,
                Month = realt.Date.Month,
                Amount =
                    salarys.Any(s => s.Year == realt.Date.Year && s.Month == realt.Date.Month)
                        ? salarys.First(s => s.Year == realt.Date.Year && s.Month == realt.Date.Month).Amount
                        : lastSalary + salaryChange < 0 ? 0 : (lastSalary += salaryChange)
            });
        }
    }
}