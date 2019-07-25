using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Enums;
using WPF.Managers.Helpers;
using WPF.Validators;

namespace UTW.Tests
{
    [TestClass]
    public class GeneratorActionUnitTest
    {
        [TestMethod]
        public void Sha256LengthTest()
        {
            var hash = Generator.GenerateSha256Hash("admin");

            Assert.IsTrue(hash.Length == 64);
        }


        [TestMethod]
        public void Sha256VerifyTest()
        {
            Assert.IsTrue(Generator.VerifySha256Hash("admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"));
        }


        [TestMethod]
        public void IDGeneratorTest()
        {
            Assert.IsTrue(BasePropertiesValidator.ValidateID(Generator.IDGenerator(IDInerfixEnum.ACC)));
        }
    }
}
