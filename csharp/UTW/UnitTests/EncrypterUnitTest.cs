using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Managers;

namespace UTW.UnitTests
{
    [TestClass]
    public class EncrypterUnitTest
    {
        [TestMethod]
        public void CryptTest()
        {
            var crypter = new EncryptionManager();
            string from = "test";
            string pass = "test";
            string password = crypter.Encrypt(pass, from);

            Assert.AreEqual(pass, crypter.Decrypt(password, from));
        }
    }
}
