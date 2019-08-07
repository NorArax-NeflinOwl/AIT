using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using WPF.Databases.Contexts;
using WPF.Properties;
using WPF.UI.Windows.Properties;

namespace WPF.Managers.Helpers
{
    public class MailSender
    {
        public static void SendActivationCodeTo(string to, string code)
        {
            var title = Resources.ACTIVATION_EMAIL_TITLE;
            var content = string.Format(Resources.ACTIVATION_EMAIL_CONTENT, code);
            SendTo(to, title, content, true);
        }

        public static void SendTo(string to, string title, string content, bool showPopup = false)
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
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
        }
    }
}
