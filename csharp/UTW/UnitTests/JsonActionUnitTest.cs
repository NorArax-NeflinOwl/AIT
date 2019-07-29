using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WPF.Managers;
using WPF.Models;

namespace UTW.UnitTests
{
    [TestClass]
    public class JsonActionUnitTest
    {

        [TestMethod]
        public void LogInfoSerializeTest()
        {
            var obj = new LogInfoModel();
            using(var jsonManager = CryptoJsonManager.Instance)
            {
                var json = jsonManager.Serialize(obj);
                var obj2 = jsonManager.Deserialize<LogInfoModel>(json);
                Assert.IsTrue(obj2 is LogInfoModel);
            }
        }
    }
}
