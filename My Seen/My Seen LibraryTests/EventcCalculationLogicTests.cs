using MySeenLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
    }
}