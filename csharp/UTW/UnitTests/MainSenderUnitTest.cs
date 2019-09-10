using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WPF.Managers.Helpers;

namespace UTW.UnitTests
{
    [TestClass]
    public class MainSenderUnitTest
    {
        [TestMethod]
        public void SendTest()
        {
            //Assert.IsTrue(MailSender.SendTo(ConfigurationManager.AppSettings["AppEmail"].ToString(), "UnitTest", "This is mail from unit test. Please don’t replay all for this email."));
        }

        [TestMethod]
        public void DownloadTest()
        {
            Assert.IsNotNull(MailDownloader.UnreadMailsDownload());
        }

        [TestMethod]
        public void SendActivatedMailTest()
        {
            //Assert.IsTrue(MailSender.SendActivationCodeTo("ppud7368@gmail.com", "", false).Result == false);
        }
    }
}
