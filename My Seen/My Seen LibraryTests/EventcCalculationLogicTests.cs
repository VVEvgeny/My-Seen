using MySeenLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MySeenLib.Tests
{
    [TestFixture()]
    public class EventcCalculationLogicTests
    {
        [Test()]
        public void CalculateToTest()
        {
            var l = new List<DateTime> { new DateTime(2018, 1, 31, 0, 0, 0), new DateTime(2018, 2, 28, 0, 0, 0) };
            var datetime =
                EventcCalculationLogic.CalculateTo(Defaults.EventsTypesBase.Indexes.EveryMonth,
                    new DateTime(2017, 1, 31, 0, 0, 0), l);

            Assert.AreEqual(datetime, new DateTime(2018, 3, 31, 0, 0, 0));
        }

        [Test()]
        public void GetSkippedTimesTest()
        {
            var l = new List<DateTime> { new DateTime(2018, 1, 31, 0, 0, 0), new DateTime(2018, 2, 28, 0, 0, 0) };

            var s =
                EventcCalculationLogic.GetSkippedTimes(Defaults.EventsTypesBase.Indexes.EveryMonth,
                    new DateTime(2017, 1, 31, 0, 0, 0), l);

            Assert.AreEqual(l, s);
        }
        /*
        [Test()]
        public void GetSkippedTimesTest2()
        {
            var l = new List<DateTime> { new DateTime(2018, 8, 12, 0, 0, 0), new DateTime(2019, 8, 12, 0, 0, 0) };

            var s =
                EventcCalculationLogic.GetSkippedTimes(Defaults.EventsTypesBase.Indexes.EveryYear,
                    new DateTime(2017, 8, 12, 0, 0, 0));

            Assert.AreEqual(l.Count, s.Count());
            Assert.AreEqual(l, s);
        }
        */
        //public static DateTime CalculateLast(Defaults.EventsTypesBase.Indexes typeRepeat, DateTime beginDate)
        [Test()]
        public void CalculateToTest2()
        {
            var s =
                EventcCalculationLogic.CalculateTo(Defaults.EventsTypesBase.Indexes.EveryYear,
                    new DateTime(2017, 8, 12, 0, 0, 0));

            Assert.AreEqual(new DateTime(2020, 8, 12), s);
        }
    }
}