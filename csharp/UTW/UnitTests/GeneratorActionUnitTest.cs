using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Models.Enums;
using WPF.Managers.Helpers;
using WPF.Managers.Validators;

namespace UTW.UnitTests
{
    [TestClass]
    public class GeneratorActionUnitTest
    {
        [TestMethod]
        public void Sha256LengthTest()
        {
            var hash = Generators.GenerateSha256Hash("admin");

            Assert.IsTrue(hash.Length == 64);
        }


        [TestMethod]
        public void Sha256VerifyTest()
        {
            Assert.IsTrue(Generators.VerifySha256Hash("admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"));
        }


        [TestMethod]
        public void IDGeneratorTest()
        {
            Assert.IsTrue(BasePropertiesValidator.ValidateID(Generators.RecordIDGenerator(TableInerfixEnum.ACC)));
        }
    }
}
