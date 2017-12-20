using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using MySeenWeb.Add_Code;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tables.Portal;
using MySeenWeb.Models.TablesLogic.Portal;
using MySeenWeb.Models.TablesViews.Portal;
using static MySeenLib.UmtTime;

namespace MySeenWeb.Models.Portal
{
    public class PortalViewModelRealt
    {
        public IEnumerable<RealtView> Data { get; set; }
        public IEnumerable<SalaryView> DataSalary { get; set; }
        public IEnumerable<DealsView> DataDeals { get; set; }

        public string LastUpdatedPrice { get; set; }
        public string LastUpdatedSalary { get; set; }
        public string LastUpdatedDeals { get; set; }

        //mode == 0 - current data
        //mode == n - current + new calculated (+n)
        public PortalViewModelRealt(int year, int priceChange, int proposalChange, int dealsChange, int salaryChange, ICacheService cache)
        {
            Data = RealtLogic.GetAll(cache).OrderBy(r => r.Date).Select(RealtView.Map).ToList();
            DataSalary = SalaryView.Make(Data, SalaryLogic.GetAll(cache), salaryChange);
            DataDeals = DealsView.Make(Data, DealsLogic.GetAll(cache), dealsChange);

            LastUpdatedPrice = From(Data.Max(r => r.Date)).ToShortDateString();

            var lastSalary = SalaryLogic.GetAll(cache).OrderByDescending(f => f.Year).ThenByDescending(f => f.Month).FirstOrDefault();
            if(lastSalary != null) LastUpdatedSalary = lastSalary.Month + "/" + lastSalary.Year;

            var lastDeals = DealsLogic.GetAll(cache).OrderByDescending(f => f.Year).ThenByDescending(f => f.Month).FirstOrDefault();
            if (lastDeals != null) LastUpdatedDeals = lastDeals.Month + "/" + lastDeals.Year;
            

            /*
            var enumerable = Data as IList<RealtView> ?? Data.ToList();
            //var lastPriceData = enumerable.Max(r => r.Date);
            //LastUpdatedPrice = lastPriceData.Day + "/" + lastPriceData.Month + "/" + lastPriceData.Year;
            LastUpdatedPrice = From(enumerable.Max(r => r.Date)).ToString(CultureInfo.CurrentCulture);

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
                        countFirst = countFirst + proposalChange;
                        if (countFirst < 0) countFirst = 0;
                    }

                    dataDop.Add(new RealtView {Date = dateFirst, Price = priceFirst, Count = countFirst});
                }
                Data = realtViews.Concat(dataDop);
            }
            var realtViews2 = Data as IList<RealtView> ?? Data.ToList();
            var salarys = ac.Salary.AsNoTracking().AsEnumerable().ToList();
            DataSalary = SalaryView.Make(realtViews2, salarys, salaryChange);

            */
        }
    }
}