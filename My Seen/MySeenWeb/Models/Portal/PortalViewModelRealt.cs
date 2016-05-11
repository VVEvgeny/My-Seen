using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using MySeenLib;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesViews.Portal;

namespace MySeenWeb.Models.Portal
{
    public class PortalViewModelRealt
    {
        public IEnumerable<RealtView> Data { get; set; }
        public IEnumerable<SalaryView> DataSalary { get; set; }

        public string LastUpdatedPrice { get; set; }
        public string LastUpdatedSalary { get; set; }

        //mode == 0 - current data
        //mode == n - current + new calculated (+n)
        public PortalViewModelRealt(int year, int priceChange, int dealsChange, int salaryChange)
        {
            var ac = new ApplicationDbContext();
            Data = ac.Realt.OrderBy(r => r.Date).AsNoTracking().Select(RealtView.Map);

            var enumerable = Data as IList<RealtView> ?? Data.ToList();
            //var lastPriceData = enumerable.Max(r => r.Date);
            //LastUpdatedPrice = lastPriceData.Day + "/" + lastPriceData.Month + "/" + lastPriceData.Year;
            LastUpdatedPrice = UmtTime.From(enumerable.Max(r => r.Date)).ToString(CultureInfo.CurrentCulture);

            if (year != 0)
            {
                var realtViews = Data as IList<RealtView> ?? Data.ToList();
                //Пусть продолжается 
                //-10 уе в неделю
                //+10 предложений в неделю
                var dataDop = new List<RealtView>();
                var dateFirst = realtViews.Max(r => r.Date);
                var priceFirst = 1242;
                var countFirst = 9653;
                for (var i = 0; i < year*365/7; i++) //недель....
                {
                    dateFirst = dateFirst.AddDays(7);
                    if (priceFirst != 0)
                    {
                        priceFirst = priceFirst + priceChange;
                        if (priceFirst < 0) priceFirst = 0;
                    }
                    if (countFirst != 0)
                    {
                        countFirst = countFirst + dealsChange;
                        if (countFirst < 0) countFirst = 0;
                    }

                    dataDop.Add(new RealtView {Date = dateFirst, Price = priceFirst, Count = countFirst});
                }
                Data = realtViews.Concat(dataDop);
            }
            var realtViews2 = Data as IList<RealtView> ?? Data.ToList();
            var salarys = ac.Salary.AsNoTracking().AsEnumerable().ToList();
            DataSalary = SalaryView.Make(realtViews2, salarys, salaryChange);

            var lastSalary = salarys.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month).First();
            LastUpdatedSalary = lastSalary.Month + "/" + lastSalary.Year;
        }
    }
}