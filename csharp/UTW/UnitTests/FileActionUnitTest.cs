using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Managers;

namespace UTW.UnitTests
{
    [TestClass]
    public class FileActionUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            FileManager manager = new FileManager();
            Assert.IsNotNull(manager);
        }
    }
}
