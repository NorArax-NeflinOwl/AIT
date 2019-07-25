using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WPF.Models;

namespace UTW.UnitTests
{
    [TestClass]
    public class JsonActionUnitTest
    {
        private JsonSerializerSettings m_Setting;

        [TestMethod]
        public void LogInfoSerializeTest()
        {
            m_Setting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            var obj = new LogInfoModel();
            var json = JsonConvert.SerializeObject(obj, m_Setting);
            var obj2 = JsonConvert.DeserializeObject<LogInfoModel>(json, m_Setting);
            Assert.IsTrue(obj2 is LogInfoModel);
        }
    }
}
