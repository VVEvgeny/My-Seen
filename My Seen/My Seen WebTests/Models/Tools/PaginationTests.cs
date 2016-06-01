using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySeenWeb.Models.Tools.Tests
{
    [TestClass]
    public class PaginationTests
    {
        [TestMethod]
        public void PaginationTest_1_0_1()
        {
            var pagination = new Pagination(1, 0, 1);
            Assert.AreEqual(pagination.IsFirstPage, true);
            Assert.AreEqual(pagination.IsLastPage, true);
            Assert.AreEqual(pagination.CurentPage, 1);
            Assert.AreEqual(pagination.IsMiddlePage, true);
            Assert.AreEqual(pagination.MiddlePage, 1);
            Assert.AreEqual(pagination.LastPage, 1);
            Assert.AreEqual(pagination.SkipRecords, 0);
        }

        [TestMethod]
        public void PaginationTest_1_100_10()
        {
            var pagination = new Pagination(1, 100, 10);
            Assert.AreEqual(pagination.IsFirstPage, true);
            Assert.AreEqual(pagination.IsLastPage, false);
            Assert.AreEqual(pagination.CurentPage, 1);
            Assert.AreEqual(pagination.IsMiddlePage, false);
            Assert.AreEqual(pagination.MiddlePage, 5);
            Assert.AreEqual(pagination.LastPage, 10);
            Assert.AreEqual(pagination.SkipRecords, 0);
        }

        [TestMethod]
        public void PaginationTest_101_100_10()
        {
            var pagination = new Pagination(101, 100, 10);
            Assert.AreEqual(pagination.IsFirstPage, false);
            Assert.AreEqual(pagination.IsLastPage, true);
            Assert.AreEqual(pagination.CurentPage, 10);
            Assert.AreEqual(pagination.IsMiddlePage, false);
            Assert.AreEqual(pagination.MiddlePage, 5);
            Assert.AreEqual(pagination.LastPage, 10);
            Assert.AreEqual(pagination.SkipRecords, 90);
        }

        [TestMethod]
        public void PaginationTest_50000_100000_1()
        {
            var pagination = new Pagination(50000, 100000, 1);
            Assert.AreEqual(pagination.IsFirstPage, false);
            Assert.AreEqual(pagination.IsLastPage, false);
            Assert.AreEqual(pagination.CurentPage, 50000);
            Assert.AreEqual(pagination.IsMiddlePage, true);
            Assert.AreEqual(pagination.MiddlePage, 50000);
            Assert.AreEqual(pagination.LastPage, 100000);
            Assert.AreEqual(pagination.SkipRecords, 49999);
        }
    }
}