using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Managers.Helpers;

namespace UTW.UnitTests
{
    [TestClass]
    public class DirectoryInfoStorage
    {
        private static readonly string dirPath = @"C:\SCC";

        [TestMethod]
        public void TestMethod()
        {
            var size = DirectoryInfoProvider.GetDirectorySize(dirPath);
            Assert.IsTrue(size > 0);

            var result = DirectoryInfoProvider.GetSize(size);
            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var info = DirectoryInfoProvider.GetString2PrintDirectorySize(dirPath);
            Assert.IsTrue(!string.IsNullOrEmpty(info));
        }
    }
}
