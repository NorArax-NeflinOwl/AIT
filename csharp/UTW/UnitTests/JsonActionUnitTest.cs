using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WPF.Managers;
using WPF.Models;
using WPF.Models.Enums;

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

        [TestMethod]
        public void SerializeListTest()
        {
            var log = new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                Message = new SimpleMessageInfoModel("Test")
            };
            var list = new List<LogInfoModel>();
            list.Add(log);
            var json = CryptoJsonManager.Instance.Serialize(log);
            var json2 = CryptoJsonManager.Instance.Serialize(list);

            var obj = CryptoJsonManager.Instance.Deserialize<List<LogInfoModel>>(json2);
            Assert.IsNotNull(obj);
        }
    }
}
