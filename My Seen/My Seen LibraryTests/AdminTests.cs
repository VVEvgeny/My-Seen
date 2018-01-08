using NUnit.Framework;

namespace MySeenLib.Tests
{
    [TestFixture()]
    public class AdminTests
    {
        [Test()]
        public void IsDebugTest()
        {
#if DEBUG
            Assert.AreEqual(true, Admin.IsDebug);    
#else
            Assert.AreEqual(false, Admin.IsDebug);
#endif

        }
    }
}