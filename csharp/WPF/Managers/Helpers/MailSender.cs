using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WPF.Databases.Contexts;
using WPF.Properties;
using WPF.GUI.Windows.Properties;

namespace WPF.Managers.Helpers
{
    public class MailSender
    {
        public static async Task<bool> SendActivationCodeTo(string to, string code, bool showMsg = true)
        {
            var result = true;
            var title = Resources.ACTIVATION_EMAIL_TITLE;
            var content = string.Format(Resources.ACTIVATION_EMAIL_CONTENT, code);

            if(SendTo(to, title, content, showMsg))
            {
                await Task.Delay(10000);
                var mails = MailDownloader.UnreadMailsDownload();
                if (mails.Any(q => q.Summary.Contains(to) && q.Summary.Contains("550 5.1.1") && q.Issued > DateTime.Now.AddSeconds(-20)))
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool SendTo(string to, string title, string content, bool showPopup = false)
        {
            var crypter = new EncryptionManager();
            string from = ConfigurationManager.AppSettings["AppEmail"].ToString();
            string password = crypter.Decrypt(ConfigurationManager.AppSettings["AppEmailPassword"].ToString(), from);

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                message.From = new MailAddress(from);
                message.To.Add(to);
                message.Subject = title;
                message.Body = content;

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(from, password);

                smtpClient.Send(message);

                if (showPopup)
                {
                    MainContext.Instance.Windows.Open(new PopupProperties(Resources.INFORMATION, Resources.EMAIL_SEND, 3), false);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex, "Check your network connection");
            }
            return false;
        }
    }
}
