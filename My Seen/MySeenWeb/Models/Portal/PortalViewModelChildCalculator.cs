using System;
using System.Collections.Generic;

namespace MySeenWeb.Models.Portal
{
    public class ChildCalculatorView
    {
        public int Day { get; set; }
        public int Boy { get; set; }
        public int Girl { get; set; }
    }

    public class ChildsCalculatorView
    {
        public int Month { get; set; }
        public List<ChildCalculatorView> Data { get; set; }
    }

    public class PortalViewModelChildCalculator
    {
        public IEnumerable<ChildsCalculatorView> Data { get; set; }

        public PortalViewModelChildCalculator(int year, string dateWoman, string dateMan)
        {
            var items = new List<ChildsCalculatorView>();

            DateTime dateW;
            DateTime dateM;

            try
            {
                dateW = Convert.ToDateTime(dateWoman);
                dateM = Convert.ToDateTime(dateMan);
            }
            catch (Exception)
            {
                try
                {
                    dateW = new DateTime(Convert.ToInt32(dateWoman.Split('/')[2]),
                        Convert.ToInt32(dateWoman.Split('/')[1]), Convert.ToInt32(dateWoman.Split('/')[0]));
                    dateM = new DateTime(Convert.ToInt32(dateMan.Split('/')[2]), Convert.ToInt32(dateMan.Split('/')[1]),
                        Convert.ToInt32(dateMan.Split('/')[0]));
                }
                catch (Exception)
                {
                    dateW = DateTime.Now;
                    dateM = DateTime.Now;
                }
            }

            for (var i = 1; i < 13; i++)
            {
                var item = new ChildsCalculatorView
                {
                    Month = i,
                    Data = new List<ChildCalculatorView>()
                };

                for (var j = 1; j < DateTime.DaysInMonth(year, i) + 1; j++)
                {
                    var man = GetDaysFromLastUpdate(dateM, new DateTime(year, i, j), MyEnum.Man);
                    var woman = GetDaysFromLastUpdate(dateW, new DateTime(year, i, j), MyEnum.Woman);
                    var percent = (man + woman)/100;

                    var wp = (int) (woman/percent);
                    var mp = (int) (man/percent);
                    if (wp + mp >= 100)
                    {
                        wp--;
                        mp--;
                    }
                    if (wp < 0) wp = 0;
                    if (mp < 0) mp = 0;
                    //У кого меньше такой и будет пол
                    item.Data.Add(new ChildCalculatorView
                    {
                        Day = j,
                        Boy = wp,
                        Girl = mp
                    });
                }
                items.Add(item);
            }
            Data = items;
        }

        private double GetDaysFromLastUpdate(DateTime date, DateTime forDate, MyEnum type)
        {
            var span = forDate - date;
            return span.TotalDays%(365*(type == MyEnum.Man ? 4 : 3));
        }

        private enum MyEnum
        {
            Man,
            Woman
        }
    }
}