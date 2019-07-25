using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.Managers.Helpers;

namespace UTW.UnitTests
{
    [TestClass]
    public class MainSenderUnitTest
    {
        [TestMethod]
        public void SendTest()
        {
            MailSender.SendTo("ppudi7368@gmail.com", "UnitTest", "This is mail from unit test. Please don’t replay all for this email.");
        }
    }
}
