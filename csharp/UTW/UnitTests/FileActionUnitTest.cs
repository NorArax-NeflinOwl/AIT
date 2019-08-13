using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Managers;
using System.IO;

namespace UTW.UnitTests
{
    [TestClass]
    public class FileActionUnitTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            FileManager manager = new FileManager();
            Assert.IsNotNull(manager);
        }

        [TestMethod]
        public void HiddenFolderTestMethod()
        {
            var folderPath = "C:\\Databases";
            var fileSubPath = "sqlite.db";
            FileManager.CreateDBFile(folderPath, fileSubPath);
            Assert.IsTrue(File.Exists(Path.Combine(folderPath, fileSubPath)));
        }
    }
}
