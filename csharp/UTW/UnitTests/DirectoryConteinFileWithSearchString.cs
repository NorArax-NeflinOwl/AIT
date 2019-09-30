using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Managers.Helpers;

namespace UTW.UnitTests
{
    [TestClass]
    public class DirectoryConteinFileWithSearchString
    {
        [TestMethod]
        public void TestMethod()
        {
            var dirPath = @"D:\AIT";
            var searchString = "test";
            var result = FileFinder.GetPaths(dirPath, searchString);

            Assert.IsTrue(result != null);
        }
    }
}
